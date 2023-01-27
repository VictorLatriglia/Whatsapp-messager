using Whatsapp_bot.Models.EntityModels;

namespace Whatsapp_bot.Models
{
    public class MoneyMovementUpdate
    {
        public double Ammount { get; set; }
        public Guid CategoryId { get; set; }
        public Guid Id { get; set; }
        public string Tag { get; set; }

        public Guid UserId { get; set; }

        public MoneyMovement BuildEntityDBO()
        {
            return new MoneyMovement
            {
                Ammount = Ammount,
                Tag = Tag,
                CategoryId = CategoryId,
                UserId = UserId,
                Id = Id
            };
        }
    }
}
