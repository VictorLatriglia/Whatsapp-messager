namespace Whatsapp_bot.Models;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class UserCreationModel
{
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
}