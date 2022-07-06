namespace Whatsapp_bot.ServiceContracts;
public interface IWhatsappMessageSenderService
{
    Task<string> SendMessage(string phoneNumber,string message);
}