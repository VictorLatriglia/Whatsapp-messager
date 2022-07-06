using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.ServiceContracts;
public interface ILoggerService
{
    Task<Log> SaveLog(string logData, bool withErrors, ActionType action);
}