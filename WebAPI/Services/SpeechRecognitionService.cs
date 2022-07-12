namespace Whatsapp_bot.Services;
using Whatsapp_bot.ServiceContracts;

public class SpeechRecognitionService : ISpeechRecognitionService
{
    readonly IVaultInformationService _vaultService;
    public SpeechRecognitionService(
        IVaultInformationService vaultService)
    {
        _vaultService = vaultService;
    }
    public bool TextContainsNumbers(string text)
    {
        throw new NotImplementedException();
    }

    public bool UserGivesConfirmation(string text)
    {
        throw new NotImplementedException();
    }

    public bool UserRequestOutgoingsSummary(string text)
    {
        var words = _vaultService.GetUserKeyWords(SpeechType.SummaryRequest);
        var intersect = words.Intersect(text.Split(' ')).ToList();
        return intersect.Count > 0;
    }
}