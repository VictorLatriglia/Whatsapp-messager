using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.ServiceContracts;
public interface IUserOutgoingsService
{
    Task<Image> AddImage(string imageId, Guid UserId, string messageToReplyId);
    Task AddCategory(string categoryName);
    Task<MoneyMovement> AddOutgoing(double ammount, string tag, string category, User user);
    Task<List<MoneyMovement>> GetOutgoingsSummary(User user);
    Task<List<MoneyMovement>> GetOutgoingsSummary(Guid userId, DateTime beginDate, DateTime? endDate);
    Task<OutgoingsCategory> GetCategoryBasedOnPreviousTag(string text);
    Task<IList<OutgoingsCategory>> GetAvailableCategories();
    Task UpdateMovement(Guid userId, MoneyMovement data);
    Task DeleteMovement(Guid movementId);
    Task<OutgoingsCategory> GetCategoryById(Guid? categoryId);
}