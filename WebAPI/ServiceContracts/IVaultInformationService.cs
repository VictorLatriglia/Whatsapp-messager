using Whatsapp_bot.Services;

namespace Whatsapp_bot.ServiceContracts;
public interface IVaultInformationService
{
    string GetParameter(string key);
    List<string> GetUserKeyWords(SpeechType speechType);
}