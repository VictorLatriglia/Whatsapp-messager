namespace Whatsapp_bot.Utils;
public struct ManagedDateTime
{
    private static DateTime now;
    public static DateTime Now
    {
        get
        {
            if (now == default)
            {
                now = DateTime.UtcNow;
            }
            return now.AddHours(-5);
        }
        set
        {
            now = value;
        }
    }
}