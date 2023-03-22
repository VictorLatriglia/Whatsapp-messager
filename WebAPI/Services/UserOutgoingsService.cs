using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class UserOutgoingsService : IUserOutgoingsService
{
    readonly IRepository<MoneyMovement> _userOutgoingRepo;
    readonly IRepository<OutgoingsCategory> _categoriesRepo;
    readonly IRepository<Image> _imageRepo;

    public UserOutgoingsService(
        IRepository<MoneyMovement> userOutgoingRepo,
        IRepository<OutgoingsCategory> categoriesRepo,
        IRepository<Image> imageRepo)
    {
        _userOutgoingRepo = userOutgoingRepo;
        _categoriesRepo = categoriesRepo;
        _imageRepo = imageRepo;
    }

    public async Task<Image> AddImage(string imageId, Guid UserId, string messageToReplyId)
    {
        return await _imageRepo.AddAsync(Image.Build(imageId, UserId, messageToReplyId));
    }

    public async Task AddCategory(string categoryName)
    {
        await _categoriesRepo.AddAsync(OutgoingsCategory.Build(categoryName));
    }

    public async Task<IList<OutgoingsCategory>> GetAvailableCategories()
    {
        return await _categoriesRepo.GetAllAsync();
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

    public async Task<List<MoneyMovement>> GetOutgoingsSummary(Guid userId, DateTime beginDate, DateTime? endDate)
    {
        var userOutgoings = await _userOutgoingRepo.AsQueryable()
            .Include(x => x.Category)
            .Where(x => x.UserId.Equals(userId)).Where(x => x.CreatedOn >= beginDate && (endDate == null || x.CreatedOn <= endDate))
            .ToListAsync();
        return userOutgoings;
    }

    public async Task<OutgoingsCategory> GetCategoryBasedOnPreviousTag(string text)
    {
        var category = (await _userOutgoingRepo.AsQueryable()
            .Include(x => x.Category)
            .Where(x => x.Tag.Contains(text))
            .Select(x => x.Category)
            .ToListAsync())?.FirstOrDefault() ?? default;
        return category;
    }

    public async Task UpdateMovement(Guid userId, MoneyMovement data)
    {
        var currentData = await _userOutgoingRepo.GetAsync(x => x.Id == data.Id);

        currentData.CategoryId = data.CategoryId;
        currentData.Ammount = data.Ammount;
        currentData.CreatedOn = data.CreatedOn;
        currentData.Tag = data.Tag;

        await _userOutgoingRepo.UpdateAsync(currentData);
    }

    public async Task DeleteMovement(Guid movementId)
    {
        var data = await _userOutgoingRepo.GetAsync(x => x.Id == movementId);
        if (data != null)
            await _userOutgoingRepo.DeleteAsync(data);
    }

    public async Task<OutgoingsCategory> GetCategoryById(Guid? categoryId)
    {
        return await _categoriesRepo.GetAsync(x => x.Id == categoryId);
    }
}