using Microsoft.Extensions.Configuration;
using Whatsapp_bot.Services;

namespace Tests;
public class VaultServiceTests
{
    [Fact]
    public void GetParameter_Success()
    {
        // Given
        Environment.SetEnvironmentVariable("TEST", "TEST");
        IConfigurationBuilder config = new ConfigurationBuilder();
        
        VaultInformationService vaultService = new VaultInformationService(config.Build());

        // When
        var res = vaultService.GetParameter("TEST");

        // Then
        Assert.Equal("TEST", res);
    }

    [Fact]
    public void GetParameter_Failure()
    {
        // Given
        IConfigurationBuilder config = new ConfigurationBuilder();
        
        VaultInformationService vaultService = new VaultInformationService(config.Build());

        // When
        Assert.Throws(typeof(ArgumentNullException), 
            () => vaultService.GetParameter("ThisDoesntExists"));

    }
}