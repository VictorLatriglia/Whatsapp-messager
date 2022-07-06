using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class VaultInformationService : IVaultInformationService
{
    public string GetParameter(string key)
    {
        string variable = Environment.GetEnvironmentVariable(key) ?? "";
        if (string.IsNullOrEmpty(variable))
            throw new ArgumentNullException(key);
        return variable;
    }
}