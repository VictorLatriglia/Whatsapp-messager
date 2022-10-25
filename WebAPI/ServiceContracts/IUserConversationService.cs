namespace Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Models.EntityModels;
public interface IUserConversationService
{
    Task<Conversation> CreateConversation(User user, double ammount, string tag = "");
    Task<Conversation> UpdateConversationTag(User user, string tag);
    Task<Conversation> UpdateConversationCategory(User user, string category);
    Task<Conversation> GetConversation(User user);
    Task DeleteConversation(User user);
    Task<IList<string>> GetAvailableCategories();
}