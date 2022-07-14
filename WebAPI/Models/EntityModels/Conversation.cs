using System.Diagnostics.CodeAnalysis;

namespace Whatsapp_bot.Models.EntityModels;
[ExcludeFromCodeCoverage]
public class Conversation : EntityBase
{
    public string UserId { get; set; }
    public string TagName { get; set; }
    public string CategoryName { get; set; }
    public double Ammount { get; set; }

    public static Conversation Build(
        string UserId,
        string TagName = "",
        string CategoryName = "",
        double Ammount = 0)
    {
        return new Conversation
        {
            UserId = UserId,
            TagName = TagName,
            CategoryName = CategoryName,
            Ammount = Ammount
        };
    }
}