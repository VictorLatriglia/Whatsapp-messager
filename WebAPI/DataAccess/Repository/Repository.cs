using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Context;

namespace Whatsapp_bot.DataAccess.Repository;
public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext Context;
    private readonly  DbSet<T> DataSet;
    public Repository(DbContext context)
    {
        Context = context;
        DataSet = context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await DataSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<IList<T>> GetAllAsync()
    {
        return await DataSet.AsQueryable().ToListAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        DataSet.Remove(entity);
        await Context.SaveChangesAsync();
    }
}