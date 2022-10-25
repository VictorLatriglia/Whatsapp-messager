using Whatsapp_bot.Models;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Utils;

namespace Whatsapp_bot.Services;
public class WhatsappMessageSenderService : IWhatsappMessageSenderService
{
    private readonly IHttpService<SendMessageModel, string> HttpSimpleService;
    private readonly IHttpService<WhatsappListTemplate, string> HttpListService;
    private readonly IVaultInformationService VaultInformationService;

    public WhatsappMessageSenderService(
        IHttpService<SendMessageModel, string> httpSimpleService,
        IHttpService<WhatsappListTemplate, string> httpListService,
        IVaultInformationService vaultInformationService)
    {
        HttpSimpleService = httpSimpleService;
        HttpListService = httpListService;
        VaultInformationService = vaultInformationService;
    }
    public async Task<string> SendMessage(string phoneNumber, string message)
    {
        var objectData = new SendMessageModel(phoneNumber, message);

        var baseUrl = VaultInformationService.GetParameter(Globals.WHATSAPP_BASE_URL);
        var phoneId = VaultInformationService.GetParameter(Globals.WHATSAPP_PHONE_ID);
        var token = VaultInformationService.GetParameter(Globals.WHATSAPP_TOKEN);

        baseUrl = baseUrl.Replace("{PHONE_ID}", phoneId);

        var result = await HttpSimpleService.PostAsync(baseUrl, objectData, token);
        return result;
    }

    public async Task<string> SendMessage(WhatsappListTemplate message)
    {
        var baseUrl = VaultInformationService.GetParameter(Globals.WHATSAPP_BASE_URL);
        var phoneId = VaultInformationService.GetParameter(Globals.WHATSAPP_PHONE_ID);
        var token = VaultInformationService.GetParameter(Globals.WHATSAPP_TOKEN);

        baseUrl = baseUrl.Replace("{PHONE_ID}", phoneId);

        var result = await HttpListService.PostAsync(baseUrl, message, token);
        return result;
    }
}