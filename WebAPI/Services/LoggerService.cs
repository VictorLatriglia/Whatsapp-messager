using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Services;
public class LoggerService : ILoggerService
{
    readonly IRepository<Log> LogRepository;
    public LoggerService(IRepository<Log> logRepository)
    {
        LogRepository = logRepository;
    }
    public async Task<Log> SaveLog(string logData, bool withErrors, ActionType action)
    {
        Log entity = Log.Build(logData, withErrors, action);
        return await LogRepository.AddAsync(entity);
    }
}