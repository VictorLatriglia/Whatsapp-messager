using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.ServiceContracts;
public interface IUserInformationService
{
    Task<User> AddUser(string name, string userPhone);
    Task<UserOutgoing> AddOutgoing(double ammount, string tag, string category, string userPhone);

}