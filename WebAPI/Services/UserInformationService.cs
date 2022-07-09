using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class UserInformationService : IUserInformationService
{
    readonly IRepository<User> _userRepo;
    readonly IRepository<UserOutgoing> _userOutgoingRepo;
    readonly IRepository<OutgoingsCategory> _categoriesRepo;
    readonly IRepository<OutgoingsTag> _tagsRepo;

    public UserInformationService(
        IRepository<User> userRepo,
        IRepository<UserOutgoing> userOutgoingRepo,
        IRepository<OutgoingsCategory> categoriesRepo,
        IRepository<OutgoingsTag> tagsRepo)
    {
        _userRepo = userRepo;
        _userOutgoingRepo = userOutgoingRepo;
        _categoriesRepo = categoriesRepo;
        _tagsRepo = tagsRepo;
    }
    public async Task<UserOutgoing> AddOutgoing(double ammount, string tag, string category, string userPhone)
    {
        var savedTag = await _tagsRepo.GetAsync(x => x.Name.Equals(tag.ToUpper()));
        var savedCategory = await _categoriesRepo.GetAsync(x => x.Name.Equals(category.ToUpper()));
        if (savedTag == null && savedCategory == null)
        {
            savedCategory = await _categoriesRepo.AddAsync(OutgoingsCategory.Build(category));
            savedTag = await _tagsRepo.AddAsync(OutgoingsTag.Build(tag, savedCategory.Id));
        }
        if (savedTag == null)
        {
            savedTag = await _tagsRepo.AddAsync(OutgoingsTag.Build(tag, savedCategory.Id));
        }
        var user = await _userRepo.GetAsync(x => x.PhoneNumber.Equals(userPhone));
        return await _userOutgoingRepo.AddAsync(UserOutgoing.Build(ammount, savedTag.Id, user.Id));
    }

    public async Task<User> AddUser(string name, string userPhone)
    {
        return await _userRepo.AddAsync(User.Build(name, userPhone));
    }
}