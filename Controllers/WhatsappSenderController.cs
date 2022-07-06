using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Whatsapp_bot.Models;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.Controllers;

[ApiController]
[Route("[controller]")]
public class WhatsappSenderController : ControllerBase
{
    private readonly IWhatsappMessageSenderService _whatsappMessageSenderService;
    private readonly ILoggerService _loggerService;
    public WhatsappSenderController(
        IWhatsappMessageSenderService whatsappMessageSenderService,
        ILoggerService loggerService)
    {
        _whatsappMessageSenderService = whatsappMessageSenderService;
        _loggerService = loggerService;
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
    public async Task<string> MessageReceived(WhatsappMessagesData data)
    {
        try
        {

            var firstEntry = data.Entry[0];

            var firstChange = firstEntry.Changes[0];

            var message = firstChange.Value.Messages[0];

            return await SendMessagePrivate(Environment.GetEnvironmentVariable("WHATSAPP_PHONE_NUMBER") ?? "",
            "EL SIGUIENTE ES EL BODY RECIBIDO: \n" + message?.text.body ?? "NO DATA RECONOCIDA");

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
