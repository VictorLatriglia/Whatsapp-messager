using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Utils.Middleware;
public class MetaControlledResponseFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
    }
}
public class MetaExceptionFilter : IExceptionFilter
{
    private readonly ILoggerService _loggerService;

    public MetaExceptionFilter(ILoggerService loggerService)
    {
        _loggerService = loggerService;
    }


    public void OnException(ExceptionContext context)
    {
        _loggerService.SaveLog(context.Exception.ToString(), true, Models.EntityModels.ActionType.MessageReceived)
            .GetAwaiter().GetResult();
        context.Result = new ContentResult
        {
            StatusCode = StatusCodes.Status200OK
        };
    }
}