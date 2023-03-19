namespace Whatsapp_bot.Models
{
    public class InvoiceProcessedModel
    {
        public double Ammount { get; set; }
        public Guid UserId { get; set; }
        public string Tag { get; set; }
        public Guid? CategoryId { get; set; }

        public string MessageToReply { get; set; }
    }
}
