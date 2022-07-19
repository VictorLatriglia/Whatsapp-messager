using System.Diagnostics.CodeAnalysis;

namespace Whatsapp_bot.Models
{

    [ExcludeFromCodeCoverage]
    public class SendMessageTemplateModel
    {
        public readonly string messaging_product = "whatsapp";
        public string to { get; set; }
        public readonly string type = "template";
        public readonly WhatsappTemplateModel template;
        public SendMessageTemplateModel(string to, string userName)
        {
            this.to = to;
            this.template = new WhatsappTemplateModel(userName);
        }
    }

    public class WhatsappTemplateModel
    {
        public string name = "remember_outgoings_registering";
        public WhatsappTemplateLanguageCodes language { get; set; }
        public IList<WhatsappTemplateComponent> components { get; set; }
        public WhatsappTemplateModel(string userName)
        {
            language = new WhatsappTemplateLanguageCodes();
            components = new List<WhatsappTemplateComponent>() { new WhatsappTemplateComponent
            {
                parameters = new List<WhatsappComponentParameter> { new WhatsappComponentParameter(userName) }
            }};
        }
    }

    public class WhatsappTemplateLanguageCodes
    {
        public string code = "es_MX";
    }

    public class WhatsappTemplateComponent
    {
        public readonly string type = "header";
        public IList<WhatsappComponentParameter> parameters { get; set; }

    }
    public class WhatsappComponentParameter
    {
        public string type = "text";
        public string text { get; set; }
        public WhatsappComponentParameter(string text)
        {
            this.text = text;
        }
    }
}
