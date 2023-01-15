namespace Whatsapp_bot.Models.EntityModels;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class MoneyMovement : EntityBase
{
    public double Ammount { get; set; }
    public Guid CategoryId { get; set; }
    public OutgoingsCategory Category { get; set; }
    public string Tag { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public static MoneyMovement Build(double Ammount, string Tag, Guid UserId, Guid CategoryId)
    {
        return new MoneyMovement
        {
            Ammount = Ammount,
            Tag = Tag,
            CategoryId = CategoryId,
            UserId = UserId
        };
    }

}