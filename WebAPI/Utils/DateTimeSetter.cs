using System.Diagnostics.CodeAnalysis;

namespace Whatsapp_bot.Utils;
[ExcludeFromCodeCoverage]
public struct ManagedDateTime
{
    private static DateTime now;
    public static DateTime Now
    {
        get
        {
            now = DateTime.UtcNow;
            return now.AddHours(-5);
        }
        set
        {
            now = value;
        }
    }
}