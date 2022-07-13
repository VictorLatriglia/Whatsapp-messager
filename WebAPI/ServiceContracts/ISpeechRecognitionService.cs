namespace Whatsapp_bot.ServiceContracts;
public interface ISpeechRecognitionService
{
    bool UserRequestOutgoingsSummary(string text);
    bool TextContainsNumbers(string text, out List<string> IdentifiedNumbers);
    bool UserGivesConfirmation(string text);
}