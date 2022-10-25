namespace Whatsapp_bot.Models
{

    public class WhatsappListTemplate
    {
        public readonly string messaging_product = "whatsapp";
        public readonly string recipient_type = "individual";
        public string to { get; set; }
        public readonly string type = "interactive";
        public readonly WhatsappInteractiveModel interactive;
        public WhatsappListTemplate(string to, string headerTitle, string messageBody, string buttonText, string listTitle, IList<string> listElements)
        {
            this.to = to;
            interactive = new WhatsappInteractiveModel(headerTitle, messageBody, buttonText, listTitle, listElements);
        }
    }

    public class WhatsappInteractiveModel
    {
        public readonly string type = "list";
        public WhatsappInteractiveHeader header { get; set; }
        public WhatsappInteractiveContent body { get; set; }
        public WhatsappInteractiveAction action { get; set; }
        public WhatsappInteractiveModel(string headerTitle, string messageBody, string buttonText, string listTitle, IList<string> listElements)
        {
            header = new WhatsappInteractiveHeader(headerTitle);
            body = new WhatsappInteractiveContent(messageBody);
            action = new WhatsappInteractiveAction(buttonText, listTitle, listElements);
        }

    }

    public class WhatsappInteractiveAction
    {
        public string button { get; set; }
        public IList<WhatsappInteractiveSection> sections { get; set; }
        public WhatsappInteractiveAction(string button, string listTitle, IList<string> rows)
        {
            this.button = button;
            sections = new List<WhatsappInteractiveSection>()
            {
                new WhatsappInteractiveSection(listTitle,rows.Select(x => new WhatsappInteractiveSectionRow(x)).ToList())
            };
        }

    }

    public class WhatsappInteractiveSection
    {
        public string title { get; set; }
        public IList<WhatsappInteractiveSectionRow> rows { get; set; }
        public WhatsappInteractiveSection(string title, IList<WhatsappInteractiveSectionRow> rows)
        {
            this.title = title;
            this.rows = rows;
        }
    }
    public class WhatsappInteractiveSectionRow
    {
        public string id { get; set; }
        public string title { get; set; }
        public WhatsappInteractiveSectionRow(string title)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
        }
    }

    public class WhatsappInteractiveHeader
    {
        public readonly string type = "text";
        public string text { get; set; }
        public WhatsappInteractiveHeader(string text)
        {
            this.text = text;
        }
    }

    public class WhatsappInteractiveContent
    {
        public string text { get; set; }
        public WhatsappInteractiveContent(string text)
        {
            this.text = text;
        }
    }
}
