namespace Whatsapp_bot.Models.EntityModels;
public class OutgoingsTag : EntityBase
{
    public string Name { get; set; }

    public string OutgoingsCategoryId { get; set; }
    public OutgoingsCategory OutgoingsCategory { get; set; }

    public static OutgoingsTag Build(string Name, string OutgoingsCategoryId)
    {
        return new OutgoingsTag
        {
            Name = Name,
            OutgoingsCategoryId = OutgoingsCategoryId
        };
    }
}