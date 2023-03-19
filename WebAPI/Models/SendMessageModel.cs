namespace Whatsapp_bot.Models;

using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class SendMessageModel
{
    public readonly string messaging_product = "whatsapp";
    public readonly string recipient_type = "individual";
    public string to { get; set; }
    public readonly string type = "text";
    public readonly WhatsappTextModel text;

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public WhatsappContextModel? context { get; set; }
    public SendMessageModel(string to, string body, string replyToId = "")
    {
        this.to = to;
        this.text = new WhatsappTextModel(body);
        if(!string.IsNullOrEmpty(replyToId))
            context = new WhatsappContextModel { message_id = replyToId };
    }
}

public class WhatsappContextModel
{
    public string message_id { get; set; }
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