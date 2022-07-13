using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.ServiceContracts;
public interface IUserInformationService
{
    Task<User> AddUser(string name, string userPhone);
    Task<User> GetUserAsync(string userPhone);
}