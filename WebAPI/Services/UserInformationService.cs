using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class UserInformationService : IUserInformationService
{
    readonly IRepository<User> _userRepo;

    public UserInformationService(
        IRepository<User> userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<User> AddUser(string name, string userPhone)
    {
        return await _userRepo.AddAsync(User.Build(name, userPhone));
    }
    public async Task<User> GetUserAsync(string userPhone)
    {
        return await _userRepo.GetAsync(x => x.PhoneNumber.Equals(userPhone));
    }
    public async Task<User> ChangeUserAutoAcceptance(User user, bool autoAccept)
    {
        user.AutoSaveOutgoings = autoAccept;
        return await _userRepo.UpdateAsync(user);
    }
}