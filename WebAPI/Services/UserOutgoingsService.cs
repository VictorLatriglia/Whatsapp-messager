using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class UserOutgoingsService : IUserOutgoingsService
{
    readonly IRepository<UserOutgoing> _userOutgoingRepo;
    readonly IRepository<OutgoingsCategory> _categoriesRepo;
    readonly IRepository<OutgoingsTag> _tagsRepo;

    public UserOutgoingsService(
        IRepository<UserOutgoing> userOutgoingRepo,
        IRepository<OutgoingsCategory> categoriesRepo,
        IRepository<OutgoingsTag> tagsRepo)
    {
        _userOutgoingRepo = userOutgoingRepo;
        _categoriesRepo = categoriesRepo;
        _tagsRepo = tagsRepo;
    }
    public async Task<UserOutgoing> AddOutgoing(double ammount, string tag, string category, User user)
    {
        var savedTag = await _tagsRepo.GetAsync(x => x.Name.Equals(tag.ToLower()));
        var savedCategory = await _categoriesRepo.GetAsync(x => x.Name.Equals(category.ToLower()));
        if (savedTag == null && savedCategory == null)
        {
            savedCategory = await _categoriesRepo.AddAsync(OutgoingsCategory.Build(category));
            savedTag = await _tagsRepo.AddAsync(OutgoingsTag.Build(tag, savedCategory.Id));
        }
        if (savedTag == null)
        {
            savedTag = await _tagsRepo.AddAsync(OutgoingsTag.Build(tag, savedCategory.Id));
        }
        return await _userOutgoingRepo.AddAsync(UserOutgoing.Build(ammount, savedTag.Id, user.Id));
    }

    public async Task<List<UserOutgoing>> GetOutgoingsSummary(User user)
    {
        var userOutgoings = await _userOutgoingRepo.AsQueryable()
            .Include(x => x.Tag).ThenInclude(x => x.OutgoingsCategory)
            .Where(x => x.UserId.Equals(user.Id))
            .ToListAsync();
        return userOutgoings;
    }
}