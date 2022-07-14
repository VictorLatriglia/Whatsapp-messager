using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.ServiceContracts;
public interface IUserOutgoingsService
{
    Task<UserOutgoing> AddOutgoing(double ammount, string tag, string category, User user);
    Task<List<UserOutgoing>> GetOutgoingsSummary(User user);
}