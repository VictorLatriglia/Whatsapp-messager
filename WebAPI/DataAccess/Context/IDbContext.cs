using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.DataAccess.Context;
public interface IDbContext
{
    public DbSet<Log> Logs { get; set; }
}