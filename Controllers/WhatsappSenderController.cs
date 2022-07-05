using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Whatsapp_bot.Controllers;

[ApiController]
[Route("[controller]")]
public class WhatsappSenderController : ControllerBase
{
    private readonly ILogger<WhatsappSenderController> _logger;
    private readonly IWhatsappMessageSenderService _whatsappMessageSenderService;
    public WhatsappSenderController(
        ILogger<WhatsappSenderController> logger,
        IWhatsappMessageSenderService whatsappMessageSenderService)
    {
        _logger = logger;
        _whatsappMessageSenderService = whatsappMessageSenderService;
    }

    [HttpPost("SendMessage")]
    public async Task<string> SendMessage(string message, string phoneNumber)
    {
        return await SendMessagePrivate(phoneNumber, message);
    }

    [HttpGet("MessageReceived")]
    public async Task<string> Verification()
    {
        string hub_mode = Request.Query["hub.mode"];
        string hub_challenge = Request.Query["hub.challenge"];
        string hub_verify_token = Request.Query["hub.verify.token"];
        await SendMessagePrivate(Environment.GetEnvironmentVariable("WHATSAPP_PHONE_NUMBER"), $"SE RECIBE SOLIICTUD DE PAIRING POR PARTE DE META: \n {hub_mode} \n {hub_challenge} \n {hub_verify_token}");

        return hub_challenge;
    }

    [HttpPost("MessageReceived")]
    public async Task<string> MessageReceived()
    {
        string body;
        using (StreamReader reader = new StreamReader(Request.Body))
        {
            body = await reader.ReadToEndAsync();
        }
        try
        {
            var data = JsonConvert.DeserializeObject<WhatsappMessagesData>(body);
            if (data == null) throw new Exception("Not recognized");
            if (data.Entry == null) throw new Exception("Not recognized");
            if (data.Entry[0] == null) throw new Exception("Not recognized");
            var firstEntry = data.Entry[0];
            if (firstEntry.Changes == null) throw new Exception("Not recognized");
            if (firstEntry.Changes[0] == null) throw new Exception("Not recognized");
            var firstChange = firstEntry.Changes[0];
            if (firstChange.Value == null) throw new Exception("Not recognized");
            if (firstChange.Value.Messages == null) throw new Exception("Not recognized");
            if (firstChange.Value.Messages[0] == null) throw new Exception("Not recognized");
            var message = firstChange.Value.Messages[0];
            if (message.text == null) throw new Exception("Not recognized");
            if (message.text.body == null) throw new Exception("Not recognized");


            return await SendMessagePrivate(Environment.GetEnvironmentVariable("WHATSAPP_PHONE_NUMBER"), "EL SIGUIENTE ES EL BODY RECIBIDO: \n" + body);

        }
        catch (Exception ex)
        {
            throw new Exception(body + " ===== " + ex.ToString());
        }
    }

    private async Task<string> SendMessagePrivate(string phoneNumber, string message)
    {
        return await _whatsappMessageSenderService.SendMessage(phoneNumber,message);;
    }
}
