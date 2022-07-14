using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Services;

namespace Tests;
public class SpeechRecognitionServiceTests
{
    [Fact]
    public void TextContainsNumbers_True_Success()
    {
        // Given
        Mock<IVaultInformationService> vaultMock = new Mock<IVaultInformationService>();
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.Numbers)).Returns(new List<string> { "1" });
        SpeechRecognitionService service = new SpeechRecognitionService(vaultMock.Object);
        // When
        List<string> Numbers;
        var doesContain = service.TextContainsNumbers("TEST 1", out Numbers);

        // Then
        Assert.True(doesContain);
        Assert.NotEmpty(Numbers);
    }

    [Fact]
    public void TextAffirms_True_Success()
    {
        // Given
        Mock<IVaultInformationService> vaultMock = new Mock<IVaultInformationService>();
        SpeechRecognitionService service = new SpeechRecognitionService(vaultMock.Object);
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.Affirmations)).Returns(new List<string> { "yes" });
        // When
        var affirms = service.UserGivesConfirmation("Yes");

        // Then
        Assert.True(affirms);
    }

    [Fact]
    public void TextRequestSummary_True_Success()
    {
        // Given
        Mock<IVaultInformationService> vaultMock = new Mock<IVaultInformationService>();
        SpeechRecognitionService service = new SpeechRecognitionService(vaultMock.Object);
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.SummaryRequest)).Returns(new List<string> { "report" });
        // When
        var requestsSummary = service.UserRequestOutgoingsSummary("Report");

        // Then
        Assert.True(requestsSummary);
    }
}