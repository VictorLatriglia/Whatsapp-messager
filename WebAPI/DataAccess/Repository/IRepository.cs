namespace Whatsapp_bot.DataAccess.Repository;
public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<IList<T>> GetAllAsync();
    Task DeleteAsync(T entity);
}