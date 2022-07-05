public class WhatsappMessageSenderService : IWhatsappMessageSenderService
{
    private readonly IHttpService<SendMessageModel, string> HttpService;
    private readonly IVaultInformationService VaultInformationService;

    public WhatsappMessageSenderService(
        IHttpService<SendMessageModel, string> httpService,
        IVaultInformationService vaultInformationService)
    {
        HttpService = httpService;
        VaultInformationService = vaultInformationService;
    }
    public async Task<string> SendMessage(string phoneNumber, string message)
    {
        var objectData = new SendMessageModel(phoneNumber, message);

        var baseUrl = VaultInformationService.GetParameter(Globals.WHATSAPP_BASE_URL);
        var phoneId = VaultInformationService.GetParameter(Globals.WHATSAPP_PHONE_ID);
        var token = VaultInformationService.GetParameter(Globals.WHATSAPP_TOKEN);

        baseUrl = baseUrl.Replace("{PHONE_ID}", phoneId);

        var result = await HttpService.PostAsync(baseUrl, objectData, token);
        return result;
    }
}