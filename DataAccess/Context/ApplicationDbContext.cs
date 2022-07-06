using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.DataAccess.Context;
public class ApplicationDbContext : DbContext
{
    public DbSet<Log> Logs { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=MyDatabase.db");
    }
}