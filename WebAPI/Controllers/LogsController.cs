namespace Whatsapp_bot.Controllers;
using Microsoft.AspNetCore.Mvc;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

[ApiController]
[Route("[controller]")]
public class LogsController : ControllerBase
{
    readonly ILoggerService _loggerService;
    public LogsController(ILoggerService loggerService)
    {
        _loggerService = loggerService;
    }

    [HttpGet]
    public async Task<IList<Log>> GetLogsAsync()
    {
        return await _loggerService.GetAll();
    }
}