public interface IWhatsappMessageSenderService
{
    Task<string> SendMessage(string phoneNumber,string message);
}