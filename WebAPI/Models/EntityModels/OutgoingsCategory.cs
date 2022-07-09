namespace Whatsapp_bot.Models.EntityModels;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class OutgoingsCategory : EntityBase
{
    public string Name { get; set; }

    public static OutgoingsCategory Build(string Name)
    {
        return new OutgoingsCategory
        {
            Name = Name
        };
    }
}