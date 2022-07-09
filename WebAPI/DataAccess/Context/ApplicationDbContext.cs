using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.DataAccess.Context;
public class ApplicationDbContext : DbContext, IDbContext
{
    public DbSet<Log> Logs { get; set; }
    public DbSet<OutgoingsCategory> OutgoingsCategories { get; set; }
    public DbSet<OutgoingsTag> OutgoingsTags { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOutgoing> UserOutgoings { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts)
    {
        Database.EnsureCreated();
    }
}