namespace Whatsapp_bot.Models.EntityModels;
public class User : EntityBase
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }

    public static User Build(string Name, string PhoneNumber)
    {
        return new User
        {
            Name = Name,
            PhoneNumber = PhoneNumber
        };
    }

}