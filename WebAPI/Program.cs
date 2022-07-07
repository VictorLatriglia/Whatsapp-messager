using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Context;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Services;

namespace Whatsapp_bot;
[ExcludeFromCodeCoverage]
sealed class Program
{
    private Program(){
            
    }
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSqlite<ApplicationDbContext>("Filename=MyDatabase.db");
        builder.Services.AddScoped<DbContext, ApplicationDbContext>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped(typeof(IHttpService<,>), typeof(HttpService<,>));
        builder.Services.AddTransient<ILoggerService, LoggerService>();
        builder.Services.AddTransient<IVaultInformationService, VaultInformationService>();
        builder.Services.AddTransient<IWhatsappMessageSenderService, WhatsappMessageSenderService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}