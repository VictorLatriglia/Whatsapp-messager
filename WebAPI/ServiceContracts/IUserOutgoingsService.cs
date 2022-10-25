using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.ServiceContracts;
public interface IUserOutgoingsService
{
    Task<MoneyMovement> AddOutgoing(double ammount, string tag, string category, User user);
    Task<List<MoneyMovement>> GetOutgoingsSummary(User user);
    Task<OutgoingsCategory> GetCategoryBasedOnPreviousTag(string text);
}