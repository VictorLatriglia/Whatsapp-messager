using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Whatsapp_bot.DataAccess.Context;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Services;
using Whatsapp_bot.Utils.Middleware;
using Hangfire;
using Hangfire.Server;
using Hangfire.Storage.SQLite;
using Whatsapp_bot.Utils;

namespace Whatsapp_bot
{
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
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<MetaControlledResponseFilter>();
            builder.Services.AddTransient<IVaultInformationService, VaultInformationService>();
            builder.Services.AddSqlServer<ApplicationDbContext>(Environment.GetEnvironmentVariable(Globals.SQL_CONNECTION_STRG));
            builder.Services.AddScoped<DbContext, ApplicationDbContext>();
            builder.Services.AddSingleton<ILogInService, LogInService>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IHttpService<,>), typeof(HttpService<,>));
            builder.Services.AddTransient<ILoggerService, LoggerService>();
            builder.Services.AddTransient<IUserInformationService, UserInformationService>();
            builder.Services.AddTransient<IWhatsappMessageSenderService, WhatsappMessageSenderService>();
            builder.Services.AddTransient<ISpeechRecognitionService, SpeechRecognitionService>();
            builder.Services.AddTransient<IUserConversationService, UserConversationService>();
            builder.Services.AddTransient<IUserOutgoingsService, UserOutgoingsService>();
            builder.Services.AddTransient<IBackgroundAutoRemember, BackgroundAutoRemember>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(pol =>
            {
                pol.AllowAnyOrigin();
                pol.AllowAnyHeader();
                pol.AllowAnyMethod();
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}