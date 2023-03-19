namespace Whatsapp_bot.Models.EntityModels
{
    public class Image : EntityBase
    {
        public string ImgId { get; set; }

        public Guid UserId { get; set; }

        public static Image Build(string ImgId, Guid UserId)
        {
            return new Image
            {
                ImgId = ImgId,
                UserId = UserId
            };
        }
    }
}
