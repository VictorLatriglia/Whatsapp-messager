namespace Whatsapp_bot.Models;
public class SendMessageModel
{
    public readonly string messaging_product = "whatsapp";
    public readonly string recipient_type = "individual";
    public string To { get; set; }
    public readonly string type = "text";
    public readonly WhatsappTextModel text;
    public SendMessageModel(string to, string body)
    {
        this.To = to;
        this.text = new WhatsappTextModel(body);
    }
}

public class WhatsappTextModel
{
    public readonly bool preview_url = false;
    public string Body { get; set; }
    public WhatsappTextModel(string body)
    {
        this.Body = body;
    }
}