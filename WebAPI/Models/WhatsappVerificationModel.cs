using System.Diagnostics.CodeAnalysis;

namespace Whatsapp_bot.Models;
[ExcludeFromCodeCoverage]
public class WhatsappVerificationModel
{
    public string hub_mode { get; set; }
    public string hub_challenge { get; set; }
    public string hub_verify_token { get; set; }
}
