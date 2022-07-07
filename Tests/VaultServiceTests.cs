using Whatsapp_bot.Services;

namespace Tests;
public class VaultServiceTests
{
    [Fact]
    public void GetParameter_Success()
    {
        // Given
        Environment.SetEnvironmentVariable("TEST", "TEST");
        VaultInformationService vaultService = new VaultInformationService();

        // When
        var res = vaultService.GetParameter("TEST");

        // Then
        Assert.Equal("TEST", res);
    }

    [Fact]
    public void GetParameter_Failure()
    {
        // Given
        VaultInformationService vaultService = new VaultInformationService();

        // When
        Assert.Throws(typeof(ArgumentNullException), 
            () => vaultService.GetParameter("ThisDoesntExists"));

    }
}