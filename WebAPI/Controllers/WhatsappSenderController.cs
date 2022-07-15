using Microsoft.AspNetCore.Mvc;
using Whatsapp_bot.Models;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.Utils.Middleware;
using Whatsapp_bot.Utils;

namespace Whatsapp_bot.Controllers;

[ApiController]
[Route("[controller]")]
public class WhatsappSenderController : ControllerBase
{
    private readonly IWhatsappMessageSenderService _whatsappMessageSenderService;
    private readonly ILoggerService _loggerService;
    private readonly IUserInformationService _userService;
    private readonly ISpeechRecognitionService _speechRecognitionService;
    private readonly IUserOutgoingsService _userOutgoingsService;
    private readonly IUserConversationService _userConversationsService;
    public WhatsappSenderController(
        IWhatsappMessageSenderService whatsappMessageSenderService,
        ILoggerService loggerService,
        IUserInformationService userService,
        ISpeechRecognitionService speechRecognitionService,
        IUserOutgoingsService userOutgoingsService,
        IUserConversationService userConversationsService)
    {
        _whatsappMessageSenderService = whatsappMessageSenderService;
        _loggerService = loggerService;
        _userService = userService;
        _speechRecognitionService = speechRecognitionService;
        _userOutgoingsService = userOutgoingsService;
        _userConversationsService = userConversationsService;
    }

    [HttpPost("SendMessage")]
    public async Task<string> SendMessage(string message, string phoneNumber)
    {
        return await SendMessagePrivate(phoneNumber, message);
    }

    [HttpGet("MessageReceived")]
    public string Verification()
    {
        string hub_challenge = Request.Query["hub.challenge"];
        return hub_challenge;
    }

    [HttpPost("MessageReceived")]
    [ServiceFilter(typeof(MetaControlledResponseFilter))]
    [TypeFilter(typeof(MetaExceptionFilter))]
    public async Task<string> MessageReceived(WhatsappMessagesData data)
    {
        try
        {
            var firstEntry = data.Entry[0];

            var firstChange = firstEntry.Changes[0];

            var message = firstChange.Value.Messages[0];

            var userPhone = message.From;
            var user = await _userService.GetUserAsync(userPhone);
            var text = message.text.body.ToLower();
            if (user == null)
            {
                await _loggerService.SaveLog($"Numero de telefono {userPhone} no reconocido envió {text}", true, ActionType.MessageReceived);
                return await SendMessagePrivate(userPhone,
                "Lo sentimos, no estás registrado en la plataforma todavía");
            }

            if (_speechRecognitionService.UserRequestOutgoingsSummary(text.ToLower()))
            {
                var outgoings = await _userOutgoingsService.GetOutgoingsSummary(user);
                return await SendMessagePrivate(userPhone,
                    PlatanizatorService.PlatanizeOutgoings(outgoings));
            }
            if (text.Contains("autoaceptar"))
            {
                bool autoAccept = true;
                if (text.Contains("no"))
                    autoAccept = false;
                await _userService.ChangeUserAutoAcceptance(user, autoAccept);
                return await SendMessagePrivate(userPhone,
                $"Listo, hemos cambiado la autoaceptación. {(autoAccept ? "Ahora guardaremos automáticamente los registros sin solicitar una confirmación" : "A partir de ahora te solicitaremos una confirmación para añadir cada gasto")}");
            }
            List<string> numbers;
            var availableTags = await _userConversationsService.GetAvailableTags();
            if (_speechRecognitionService.TextContainsNumbers(text, out numbers))
            {
                return await HandleRequestWithNumbers(user, text, userPhone, numbers, availableTags);
            }
            if (await _userConversationsService.GetConversation(user) is Conversation convo)
            {
                return await HandleRequestWithPreviousConvo(user, text, userPhone, convo, availableTags);
            }
            await _loggerService.SaveLog("No se pudo añadir el gasto, la data no estuvo en el formato correcto", true, ActionType.MessageReceived);
            return await SendMessagePrivate(userPhone,
                "Lo sentimos, no registraste el dato de manera correcta. Necesitamos al menos el valor del gasto para comenzar el proceso");
        }
        catch (Exception ex)
        {
            await _loggerService.SaveLog(ex.ToString(), true, ActionType.MessageReceived);
            throw;
        }
    }

    private async Task<string> HandleRequestWithNumbers(User user, string text, string userPhone, List<string> numbers, IList<OutgoingsTag> availableTags)
    {
        List<string> TextParts = text.Split(' ').Except(numbers).ToList();
        List<OutgoingsTag> Tags = availableTags.Where(x => TextParts.Contains(x.Name)).ToList();
        OutgoingsTag matchedTag = Tags.FirstOrDefault();
        double ammount = Convert.ToDouble(numbers.FirstOrDefault());
        var convo = await _userConversationsService.CreateConversation(user, ammount, matchedTag?.Name ?? "");
        if (matchedTag == null)
        {
            await _loggerService.SaveLog("Gasto por añadir, pendiente de tag", false, ActionType.MessageReceived);
            return await SendMessagePrivate(userPhone,
                $"Hola! Recibimos tu solicitud de registro por {ammount.ToString("C")} sin embargo no detectamos ningúna etiqueta para él.\n Por favor, escribenos el nombre de la etiqueta y nos encargaremos del resto. \nGracias!");
        }
        if (user.AutoSaveOutgoings)
        {
            return await SaveOutgoing(user, userPhone, convo);
        }

        return await SendMessagePrivate(userPhone,
            $"Hola! Recibimos tu solicitud de registro por {ammount.ToString("C")} en {matchedTag.Name}.\n Está bien lo que vamos a agregar? Confirmanos que te entendimos bien y almacenaremos la información. \nGracias!");
    }

    private async Task<string> SaveOutgoing(User user, string userPhone, Conversation convo)
    {
        await _userOutgoingsService.AddOutgoing(convo.Ammount, convo.TagName, convo.CategoryName, user);
        await _userConversationsService.DeleteConversation(user);
        await _loggerService.SaveLog("Gasto añadido", false, ActionType.MessageReceived);
        return await SendMessagePrivate(userPhone,
            $"Listo! hemos registrado tu gasto de {convo.Ammount.ToString("C")} en {convo.TagName}");
    }

    private async Task<string> HandleRequestWithPreviousConvo(User user, string text, string userPhone, Conversation convo, IList<OutgoingsTag> availableTags)
    {
        if (string.IsNullOrEmpty(convo.TagName))
        {
            var matchedTag = availableTags.FirstOrDefault(x => x.Name.Equals(text));
            convo = await _userConversationsService.UpdateConversationTag(user, text);
            if (matchedTag != null)
            {
                convo = await _userConversationsService.UpdateConversationCategory(user, matchedTag.OutgoingsCategory.Name);
                return await SendMessagePrivate(userPhone,
                    $"Vale, creo que ya te entendemos, almacenamos entonces {convo.Ammount.ToString("C")} en {convo.TagName}?");
            }
            return await SendMessagePrivate(userPhone,
                $"Vale, ya nos estamos entendiendo entonces, sin embargo no tenemos registrada la etiqueta: {convo.TagName}\nSobre qué categoría deberíamos almacenarla?");
        }
        if (string.IsNullOrEmpty(convo.CategoryName))
        {
            convo = await _userConversationsService.UpdateConversationCategory(user, text);
            if (user.AutoSaveOutgoings)
            {
                return await SaveOutgoing(user, userPhone, convo);
            }
            return await SendMessagePrivate(userPhone,
                $"Vale, creo que ya te entendemos, almacenamos entonces {convo.Ammount.ToString("C")} en {convo.TagName}?\nSerá creada la etiqueta en la categoría {convo.CategoryName}, así podrás usarla en el futuro sin mayor problema");
        }
        if (_speechRecognitionService.UserGivesConfirmation(text))
        {
            return await SaveOutgoing(user, userPhone, convo);
        }
        else
        {
            await _userConversationsService.DeleteConversation(user);
            return await SendMessagePrivate(userPhone,
                $"Vale, acabamos de cancelar el registro, puedes agregar nuevos gastos a partir de ahora (o contactarnos si hubo algún problema en el proceso)");
        }

    }
    private async Task<string> SendMessagePrivate(string phoneNumber, string message)
    {
        return await _whatsappMessageSenderService.SendMessage(phoneNumber, message);
    }
}
