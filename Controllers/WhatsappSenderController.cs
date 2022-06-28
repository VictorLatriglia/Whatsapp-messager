using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Whatsapp_bot.Controllers;

[ApiController]
[Route("[controller]")]
public class WhatsappSenderController : ControllerBase
{
    private readonly ILogger<WhatsappSenderController> _logger;
    private readonly IHttpClientFactory _clientFactory;
    public WhatsappSenderController(
        ILogger<WhatsappSenderController> logger,
        IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;
    }

    [HttpPost("SendMessage")]
    public async Task<string> SendMessage(string message, string phoneNumber)
    {
        return await SendMessagePrivate(phoneNumber, message);
    }

    public async Task<string> MessageReceived()
    {
        string body;
        using (StreamReader reader = new StreamReader(Request.Body))
        {
            body = await reader.ReadToEndAsync();
        }
        if (Request.Method == HttpMethod.Get.ToString())
        {
            await SendMessagePrivate(Environment.GetEnvironmentVariable("WHATSAPP_PHONE_NUMBER"), "SE RECIBE SOLIICTUD DE PAIRING POR PARTE DE META: \n" + body);
            var data = JsonConvert.DeserializeObject<WhatsappVerificationModel>(body);
            return data.hub_challenge;
        }
        return await SendMessagePrivate(Environment.GetEnvironmentVariable("WHATSAPP_PHONE_NUMBER"), "EL SIGUIENTE ES EL BODY RECIBIDO: \n" + body);
    }

    private async Task<string> SendMessagePrivate(string phoneNumber, string message)
    {
        var json = new SendMessageModel(phoneNumber, message);

        string jsonString = JsonConvert.SerializeObject(json);

        var client = new RestClient("https://graph.facebook.com/v12.0/" + Environment.GetEnvironmentVariable("WHATSAPP_PHONE_ID") + "/messages");

        var request = new RestRequest();
        request.AddHeader("Content-Type", "application/json");
        request.AddHeader("Authorization", "Bearer " + Environment.GetEnvironmentVariable("WHATSAPP_TOKEN"));

        request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
        var response = await client.PostAsync(request);
        Console.WriteLine(response.Content);

        return response.Content;
    }
}
