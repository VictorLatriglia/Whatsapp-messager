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
    public WhatsappSenderController(
        IWhatsappMessageSenderService whatsappMessageSenderService,
        ILoggerService loggerService,
        IUserInformationService userService)
    {
        _whatsappMessageSenderService = whatsappMessageSenderService;
        _loggerService = loggerService;
        _userService = userService;
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
            var text = message.text.body;
            if (user == null)
            {
                await _loggerService.SaveLog($"Numero de telefono {userPhone} no reconocido envió {text}", true, ActionType.MessageReceived);
                return await SendMessagePrivate(userPhone,
                "Lo sentimos, no estás registrado en la plataforma todavía");
            }
            if (text.ToLower().Contains("resumen"))
            {
                var outgoings = await _userService.GetOutgoingsSummary(user);
                return await SendMessagePrivate(userPhone,
                    PlatanizatorService.PlatanizeOutgoings(outgoings));
            }
            var recognicedParts = text.Split(' ');
            if (recognicedParts != null && recognicedParts.Length == 3)
            {
                var result = await _userService.AddOutgoing(Convert.ToDouble(recognicedParts[2]), recognicedParts[1], recognicedParts[0], user);
                await _loggerService.SaveLog("Gasto añadido", false, ActionType.MessageReceived);
                return await SendMessagePrivate(userPhone,
                    $"Gracias! hemos registrado tu gasto de ${recognicedParts[2]} en {recognicedParts[1]}");
            }
            else
            {
                await _loggerService.SaveLog("No se pudo añadir el gasto, la data no estuvo en el formato correcto", true, ActionType.MessageReceived);
                return await SendMessagePrivate(userPhone,
                    "Lo sentimos, no registraste el dato de manera correcta. \nRecuerda que debe ser en formato:\nCategoría Etiqueta Valor\nPor ejemplo:\nAlimentos café 5500");
            }
        }
        catch (Exception ex)
        {
            await _loggerService.SaveLog(ex.ToString(), true, ActionType.MessageReceived);
            throw;
        }
    }

    private async Task<string> SendMessagePrivate(string phoneNumber, string message)
    {
        return await _whatsappMessageSenderService.SendMessage(phoneNumber, message);
    }
}
