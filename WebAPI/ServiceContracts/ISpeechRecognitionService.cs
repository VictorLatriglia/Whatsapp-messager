namespace Whatsapp_bot.ServiceContracts;
public interface ISpeechRecognitionService
{
    bool UserRequestOutgoingsSummary(string text);
    bool TextContainsNumbers(string text);
    bool UserGivesConfirmation(string text);
}