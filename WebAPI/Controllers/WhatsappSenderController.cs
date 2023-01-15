using Microsoft.AspNetCore.Mvc;
using Whatsapp_bot.Models;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.Utils.Middleware;
using Whatsapp_bot.Utils;
using Newtonsoft.Json;

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

    [HttpPost]
    public async Task<IActionResult> AddCategory(string categoryName)
    {
        await _userOutgoingsService.AddCategory(categoryName);
        return Ok();
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

            var text = message.IsReply ? message.interactive.list_reply.title.ToLower() : message.text.body.ToLower();

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

            if (text.Contains("cancelar"))
            {
                await _userConversationsService.DeleteConversation(user);
                return await SendMessagePrivate(userPhone,
                $"Listo, hemos cancelado el registro actual");
            }

            if (await _userConversationsService.GetConversation(user) is Conversation convo)
            {
                return await HandleRequestWithPreviousConvo(user, text, userPhone, convo);
            }

            if (_speechRecognitionService.TextContainsNumbers(text, out List<string> numbers))
            {
                return await HandleRequestWithNumbers(user, text, userPhone, numbers);
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

    private async Task<string> HandleRequestWithNumbers(User user, string text, string userPhone, List<string> numbers)
    {
        List<string> TextParts = text.Split(' ').Except(numbers).ToList();
        var category = await _userOutgoingsService.GetCategoryBasedOnPreviousTag(TextParts[0]);
        double ammount = Convert.ToDouble(numbers.FirstOrDefault());
        var convo = await _userConversationsService.CreateConversation(user, ammount, TextParts[0]);
        if (category == null || string.IsNullOrEmpty(category.Name))
        {
            var modelData = new WhatsappListTemplate(
                userPhone, "Categoría",
                $"Vale, Sobre qué categoría deberíamos almacenar esto?",
                "Ver categorías",
                "Categorías disponibles: ",
                await _userConversationsService.GetAvailableCategories());
            await _loggerService.SaveLog("Enviando info de categorias: " + JsonConvert.SerializeObject(modelData), false, ActionType.InternalProcess);
            return await _whatsappMessageSenderService.SendMessage(modelData);
        }
        else
        {
            await _userConversationsService.UpdateConversationCategory(user, category.Name);
        }
        if (user.AutoSaveOutgoings)
        {
            return await SaveOutgoing(user, userPhone, convo);
        }

        return await SendMessagePrivate(userPhone,
            $"Hola! Recibimos tu solicitud de registro por {ammount.ToString("C")} en {category.Name}.\n Está bien lo que vamos a agregar? Confirmanos que te entendimos bien y almacenaremos la información. \nGracias!");
    }

    private async Task<string> SaveOutgoing(User user, string userPhone, Conversation convo)
    {
        await _userOutgoingsService.AddOutgoing(convo.Ammount, convo.TagName, convo.CategoryName, user);
        await _userConversationsService.DeleteConversation(user);
        await _loggerService.SaveLog("Gasto añadido", false, ActionType.MessageReceived);
        return await SendMessagePrivate(userPhone,
            $"Listo! hemos registrado tu gasto de {convo.Ammount.ToString("C")} en {convo.TagName}");
    }

    private async Task<string> HandleRequestWithPreviousConvo(User user, string text, string userPhone, Conversation convo)
    {
        if (string.IsNullOrEmpty(convo.CategoryName))
        {
            convo = await _userConversationsService.UpdateConversationCategory(user, text);
            if (user.AutoSaveOutgoings)
            {
                return await SaveOutgoing(user, userPhone, convo);
            }
            return await SendMessagePrivate(userPhone,
                $"Vale, creo que ya te entendemos, almacenamos entonces {convo.Ammount.ToString("C")} en {convo.CategoryName}?");
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
