using Whatsapp_bot.Models;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Services;
using Whatsapp_bot.Utils;

namespace Tests;
public class WhatsappMessageServiceTests
{
    [Fact]
    public async Task SendMessageSuccess()
    {
        // Given
        Mock<IHttpService<SendMessageModel, string>> httpMock = new Mock<IHttpService<SendMessageModel, string>>();
        Mock<IHttpService<WhatsappListTemplate, string>> httpListMock = new Mock<IHttpService<WhatsappListTemplate, string>>();
        Mock<IVaultInformationService> vaultInformationMock = new Mock<IVaultInformationService>();

        httpMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<SendMessageModel>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));
        httpListMock.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<WhatsappListTemplate>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));

        vaultInformationMock.Setup(x => x.GetParameter(Globals.WHATSAPP_BASE_URL)).Returns("https://graph.facebook.com/v12.0/{PHONE_ID}/messages");
        vaultInformationMock.Setup(x => x.GetParameter(Globals.WHATSAPP_PHONE_ID)).Returns("This is a test");
        vaultInformationMock.Setup(x => x.GetParameter(Globals.WHATSAPP_TOKEN)).Returns("This is a test");

        WhatsappMessageSenderService whatsappService = new WhatsappMessageSenderService(httpMock.Object, httpListMock.Object, vaultInformationMock.Object);

        // When
        var result = await whatsappService.SendMessage("123Test", "This is a test");

        // Then
        Assert.Equal("OK", result);
    }
}