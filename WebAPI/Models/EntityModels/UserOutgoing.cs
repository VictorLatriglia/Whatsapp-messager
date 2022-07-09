namespace Whatsapp_bot.Models.EntityModels;
public class UserOutgoing : EntityBase
{
    public double Ammount { get; set; }
    public string TagId { get; set; }
    public OutgoingsTag Tag { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public static UserOutgoing Build(double Ammount, string TagId, string UserId)
    {
        return new UserOutgoing
        {
            Ammount = Ammount,
            TagId = TagId,
            UserId = UserId
        };
    }

}