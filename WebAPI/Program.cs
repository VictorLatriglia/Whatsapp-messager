using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Context;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Services;
using Whatsapp_bot.Utils.Middleware;
using Microsoft.Extensions.Configuration;

namespace Whatsapp_bot;
[ExcludeFromCodeCoverage]
sealed class Program
{
    private Program()
    {

    }
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Configuration.AddJsonFile("./SpeechDictionary/Spanish/SummaryRequests.json");
        builder.Configuration.AddJsonFile("./SpeechDictionary/Spanish/Numbers.json");
        builder.Configuration.AddJsonFile("./SpeechDictionary/Spanish/Affirmations.json");
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.Configure<ApiBehaviorOptions>(o =>
        {
            o.InvalidModelStateResponseFactory = actionContext =>
                {
                    actionContext.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                    return new OkObjectResult(actionContext.ModelState);
                };
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.AddScoped<MetaControlledResponseFilter>();
        builder.Services.AddSqlite<ApplicationDbContext>("Filename=MyDatabase.db");
        builder.Services.AddScoped<DbContext, ApplicationDbContext>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped(typeof(IHttpService<,>), typeof(HttpService<,>));
        builder.Services.AddTransient<ILoggerService, LoggerService>();
        builder.Services.AddTransient<IVaultInformationService, VaultInformationService>();
        builder.Services.AddTransient<IUserInformationService, UserInformationService>();
        builder.Services.AddTransient<IWhatsappMessageSenderService, WhatsappMessageSenderService>();
        builder.Services.AddTransient<ISpeechRecognitionService, SpeechRecognitionService>();

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