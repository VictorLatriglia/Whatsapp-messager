using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.DataAccess.Context;
public class ApplicationDbContext : DbContext, IDbContext
{
    public DbSet<Log> Logs { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts)
    {

    }
}