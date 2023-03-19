using Whatsapp_bot.Models;

namespace Whatsapp_bot.ServiceContracts;
public interface IWhatsappMessageSenderService
{
    Task<string> SendMessage(string phoneNumber,string message, string replyToMessageId = "");
    Task<string> SendMessage(WhatsappListTemplate message);
}