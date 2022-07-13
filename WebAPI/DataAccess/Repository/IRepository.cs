using System.Linq.Expressions;

namespace Whatsapp_bot.DataAccess.Repository;
public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<IList<T>> GetAllAsync();
    Task<T> UpdateAsync(T entity);
    Task<T> GetAsync(Expression<Func<T,bool>> query);
    Task<List<T>> QueryAsync(Expression<Func<T,bool>> query);
    Task DeleteAsync(T entity);

    IQueryable<T> AsQueryable();
}