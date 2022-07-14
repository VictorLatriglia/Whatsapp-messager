using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Whatsapp_bot.ServiceContracts;

namespace Whatsapp_bot.Utils.Middleware;
[ExcludeFromCodeCoverage]
public class MetaControlledResponseFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
        //The Catching of the result will be executed on the previous method, although this method
        //can not be ommited as per the IResultFilter interface
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
    }
}
[ExcludeFromCodeCoverage]
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