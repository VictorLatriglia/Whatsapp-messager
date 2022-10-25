namespace Whatsapp_bot.Models;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class WhatsappMessagesData
{
    public string Object { get; set; }
    public List<EntryData> Entry { get; set; }
}
[ExcludeFromCodeCoverage]
public class EntryData
{
    public string Id { get; set; }
    public List<ChangesOnMessages> Changes { get; set; }
}


[ExcludeFromCodeCoverage]
public class ChangesOnMessages
{
    public ChangeValue Value { get; set; }
    public string Field { get; set; }
}

[ExcludeFromCodeCoverage]
public class ChangeValue
{
    public string messaging_product { get; set; }
    public ChangeValueMetadata Metadata { get; set; }
    public List<ContactInformation> Contacts { get; set; }
    public List<MessagesData> Messages { get; set; }
}

[ExcludeFromCodeCoverage]
public class ChangeValueMetadata
{
    public string display_phone_number { get; set; }
    public string phone_number_id { get; set; }

}

[ExcludeFromCodeCoverage]
public class ContactInformation
{
    public ProfileData Profile { get; set; }
    public string wa_id { get; set; }
}

[ExcludeFromCodeCoverage]
public class ProfileData
{
    public string name { get; set; }
}

[ExcludeFromCodeCoverage]
public class MessagesData
{
    public string From { get; set; }
    public string id { get; set; }
    public string timestamp { get; set; }
    public TextData? text { get; set; }
    public InteractiveReplyData? interactive { get; set; }
    public string type { get; set; }

    public bool IsReply { get => interactive != null; }
}

[ExcludeFromCodeCoverage]
public class TextData
{
    public string body { get; set; }
}

[ExcludeFromCodeCoverage]
public class InteractiveReplyData
{
    public string type { get; set; }
    public InteractiveListeReplyData list_reply { get; set; }
}

[ExcludeFromCodeCoverage]
public class InteractiveListeReplyData
{
    public string id { get; set; }
    public string title { get; set; }
}