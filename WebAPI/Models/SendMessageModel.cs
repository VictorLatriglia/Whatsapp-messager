namespace Whatsapp_bot.Models;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class SendMessageModel
{
    public readonly string messaging_product = "whatsapp";
    public readonly string recipient_type = "individual";
    public string to { get; set; }
    public readonly string type = "text";
    public readonly WhatsappTextModel text;
    public SendMessageModel(string to, string body)
    {
        this.to = to;
        this.text = new WhatsappTextModel(body);
    }
}

public class WhatsappTextModel
{
    public readonly bool preview_url = false;
    public string body { get; set; }
    public WhatsappTextModel(string body)
    {
        this.body = body;
    }
}