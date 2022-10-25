using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class UserConversationService : IUserConversationService
{
    readonly IRepository<Conversation> _conversationsRepo;
    readonly IRepository<OutgoingsCategory> _categoriesRepo;

    public UserConversationService(
        IRepository<Conversation> conversationsRepo,
        IRepository<OutgoingsCategory> categoriesRepo)
    {
        _conversationsRepo = conversationsRepo;
        _categoriesRepo = categoriesRepo;
    }

    public async Task<IList<string>> GetAvailableCategories() =>
       await _categoriesRepo.AsQueryable().Select(x => x.Name).Take(10).ToListAsync();

    public async Task<Conversation> CreateConversation(User user, double ammount, string tag = "")
    {
        Conversation convo = Conversation.Build(user.Id, tag, "", ammount);
        return await _conversationsRepo.AddAsync(convo);
    }

    public async Task DeleteConversation(User user)
    {
        var convo = await _conversationsRepo.GetAsync(x => x.UserId.Equals(user.Id));
        if (convo != null)
            await _conversationsRepo.DeleteAsync(convo);
    }

    public async Task<Conversation> GetConversation(User user) =>
        await _conversationsRepo.GetAsync(x => x.UserId.Equals(user.Id));


    public async Task<Conversation> UpdateConversationCategory(User user, string category)
    {
        var convo = await GetConversation(user);
        if (convo != null)
        {
            convo.CategoryName = category;
            return await _conversationsRepo.UpdateAsync(convo);
        }
        return convo;
    }

    public async Task<Conversation> UpdateConversationTag(User user, string tag)
    {
        var convo = await GetConversation(user);
        if (convo != null)
        {
            convo.TagName = tag;
            return await _conversationsRepo.UpdateAsync(convo);
        }
        return convo;
    }

}