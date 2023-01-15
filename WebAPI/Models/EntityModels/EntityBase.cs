using Whatsapp_bot.Utils;

namespace Whatsapp_bot.Models.EntityModels;
public abstract class EntityBase
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }

    protected EntityBase()
    {

        Id = Guid.NewGuid();
        CreatedOn = ManagedDateTime.Now;
        UpdatedOn = ManagedDateTime.Now;
    }

}