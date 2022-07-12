using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class VaultInformationService : IVaultInformationService
{
    readonly IConfiguration _configuration;
    public VaultInformationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GetParameter(string key)
    {
        string variable = Environment.GetEnvironmentVariable(key) ?? "";
        if (string.IsNullOrEmpty(variable))
            throw new ArgumentNullException(key);
        return variable;
    }

    public List<string> GetUserKeyWords(SpeechType speechType)
    {
        var json = _configuration.GetRequiredSection(speechType.ToString());
        return json.GetChildren().Select(x => x.Value).ToList();
    }
}

public enum SpeechType
{
    SummaryRequest,
    Numbers,
    Affirmations
}