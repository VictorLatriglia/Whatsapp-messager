using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class UserConversationService : IUserConversationService
{
    readonly IRepository<Conversation> _conversationsRepo;
    readonly IRepository<OutgoingsTag> _tagsRepo;
    readonly IRepository<OutgoingsCategory> _categoriesRepo;
    readonly IRepository<UserOutgoing> _outgoingsRepo;
    readonly IUserInformationService _userInformationService;

    public UserConversationService(
        IRepository<Conversation> conversationsRepo,
        IRepository<OutgoingsTag> tagsRepo,
        IRepository<OutgoingsCategory> categoriesRepo,
        IRepository<UserOutgoing> outgoingsRepo,
        IUserInformationService userInformationService
    )
    {
        _conversationsRepo = conversationsRepo;
        _tagsRepo = tagsRepo;
        _categoriesRepo = categoriesRepo;
        _outgoingsRepo = outgoingsRepo;
        _userInformationService = userInformationService;
    }
    public async Task<Conversation> CreateConversation(User user, double ammount, string tag = "")
    {
        OutgoingsTag tagStored = null;
        if (!string.IsNullOrEmpty(tag))
        {
            tagStored = await _tagsRepo.AsQueryable().Include(x => x.OutgoingsCategory)
                .FirstOrDefaultAsync(x => x.Name.Equals(tag));
        }
        Conversation convo = Conversation.Build(user.Id, tagStored?.Name ?? "", tagStored?.OutgoingsCategory.Name ?? "", ammount);
        return await _conversationsRepo.AddAsync(convo);
    }

    public async Task DeleteConversation(User user)
    {
        var convo = await _conversationsRepo.GetAsync(x => x.UserId.Equals(user.Id));
        if (convo != null)
            await _conversationsRepo.DeleteAsync(convo);
    }

    public async Task<IList<OutgoingsTag>> GetAvailableTags() =>
        await _tagsRepo.GetAllAsync();

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