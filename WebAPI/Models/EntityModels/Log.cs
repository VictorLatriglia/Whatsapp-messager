namespace Whatsapp_bot.Models.EntityModels;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Log : EntityBase
{
    public string LogData { get; set; }
    public bool Error { get; set; }
    public string Action { get; set; }

    public static Log Build(string logData, bool error, ActionType action)
    {
        return new Log
        {
            LogData = logData,
            Error = error,
            Action = action.ToString()
        };
    }
}

public enum ActionType
{
    MessageReceived,
    MessageSent,
    InternalProcess
}