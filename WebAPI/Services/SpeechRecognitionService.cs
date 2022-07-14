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
    public bool TextContainsNumbers(string text, out List<string> IdentifiedNumbers)
    {
        List<string> TextParts = text.ToLower().Split(' ').ToList();
        var Numbers = _vaultService.GetUserKeyWords(SpeechType.Numbers);
        IdentifiedNumbers = new List<string>();
        foreach (var part in TextParts)
        {
            foreach (var number in Numbers)
            {
                if (part.Contains(number))
                {
                    IdentifiedNumbers.Add(part);
                    break;
                }
            }
        }
        return IdentifiedNumbers.Count > 0;
    }

    public bool UserGivesConfirmation(string text)
    {
        var words = _vaultService.GetUserKeyWords(SpeechType.Affirmations);
        var intersect = words.Intersect(text.ToLower().Split(' ')).ToList();
        return intersect.Count > 0;
    }

    public bool UserRequestOutgoingsSummary(string text)
    {
        var words = _vaultService.GetUserKeyWords(SpeechType.SummaryRequest);
        var intersect = words.Intersect(text.ToLower().Split(' ')).ToList();
        return intersect.Count > 0;
    }
}