using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class UserOutgoingsService : IUserOutgoingsService
{
    readonly IRepository<MoneyMovement> _userOutgoingRepo;
    readonly IRepository<OutgoingsCategory> _categoriesRepo;

    public UserOutgoingsService(
        IRepository<MoneyMovement> userOutgoingRepo,
        IRepository<OutgoingsCategory> categoriesRepo)
    {
        _userOutgoingRepo = userOutgoingRepo;
        _categoriesRepo = categoriesRepo;
    }
    public async Task<MoneyMovement> AddOutgoing(double ammount, string tag, string category, User user)
    {
        var savedCategory = await _categoriesRepo.GetAsync(x => x.Name.Equals(category.ToLower()));
        return await _userOutgoingRepo.AddAsync(MoneyMovement.Build(ammount, tag, user.Id, savedCategory.Id));
    }

    public async Task<List<MoneyMovement>> GetOutgoingsSummary(User user)
    {
        var userOutgoings = await _userOutgoingRepo.AsQueryable()
            .Include(x => x.Category)
            .Where(x => x.UserId.Equals(user.Id))
            .ToListAsync();
        return userOutgoings;
    }

    public async Task<OutgoingsCategory> GetCategoryBasedOnPreviousTag(string text)
    {
        var category = (await _userOutgoingRepo.AsQueryable()
            .Include(x => x.Category)
            .Where(x => x.Tag.Contains(text))
            .Select(x => x.Category)
            .ToListAsync())?.FirstOrDefault()??null;
        return category;
    }
}