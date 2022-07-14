using Whatsapp_bot.Controllers;
using Whatsapp_bot.Models;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Services;

namespace Tests;
public class WhatsappMessageControllerTests
{
    Mock<IWhatsappMessageSenderService> _messageSenderMock;
    Mock<ILoggerService> _loggerServiceMock;
    Mock<IUserInformationService> _userServiceMock;
    Mock<ISpeechRecognitionService> _speechServiceMock;
    Mock<IUserOutgoingsService> _userOutgoingsService;
    Mock<IUserConversationService> _userConvoService;
    WhatsappMessagesData BaseData;

    public WhatsappMessageControllerTests()
    {
        _messageSenderMock = new Mock<IWhatsappMessageSenderService>();
        _loggerServiceMock = new Mock<ILoggerService>();
        _userServiceMock = new Mock<IUserInformationService>();
        _speechServiceMock = new Mock<ISpeechRecognitionService>();
        _userOutgoingsService = new Mock<IUserOutgoingsService>();
        _userConvoService = new Mock<IUserConversationService>();
        BaseData = new WhatsappMessagesData
        {
            Object = "This is a test",
            Entry = new List<EntryData>{
                new EntryData{
                    Id="This is a test",
                    Changes = new List<ChangesOnMessages>{
                        new ChangesOnMessages{
                            Field="This is a test",
                            Value = new ChangeValue{
                                messaging_product="This is a test",
                                Metadata=new ChangeValueMetadata{
                                    display_phone_number="This is a test",
                                    phone_number_id="This is a test"
                                },
                                Contacts = new List<ContactInformation>{
                                    new ContactInformation{
                                        wa_id="This is a test",
                                        Profile = new ProfileData{
                                            name="This is a test"
                                        }
                                    }
                                },
                                Messages = new List<MessagesData>{
                                    new MessagesData{
                                        From ="This is a test",
                                        id="This is a test",
                                        timestamp="This is a test",
                                        type ="This is a test",
                                        text = new TextData{
                                            body = "This is a TEST"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

    }

    private void ModifyBaseDataText(string text) =>
        BaseData.Entry[0].Changes[0].Value.Messages[0].text.body = text;

    [Fact]
    public async Task SendMessage_Success()
    {
        // Given
        _messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));

        WhatsappSenderController controller = new WhatsappSenderController(
            _messageSenderMock.Object,
            _loggerServiceMock.Object,
            _userServiceMock.Object,
            _speechServiceMock.Object,
            _userOutgoingsService.Object,
            _userConvoService.Object);
        // When
        var res = await controller.SendMessage("This is a test", "12345");

        // Then
        Assert.Equal("OK", res);
    }

    [Fact]
    public async Task MessageReceived_UserNotRecogniced_Success()
    {
        // Given
        _messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));


        WhatsappSenderController controller = new WhatsappSenderController(
            _messageSenderMock.Object,
            _loggerServiceMock.Object,
            _userServiceMock.Object,
            _speechServiceMock.Object,
            _userOutgoingsService.Object,
            _userConvoService.Object);
        // When
        var res = await controller.MessageReceived(BaseData);

        // Then
        Assert.Equal("OK", res);
    }

    [Fact]
    public async Task MessageReceived_UserRequestSummary_Success()
    {
        // Given
        _messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));
        _userServiceMock.Setup(x => x.GetUserAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new User { Id = Guid.NewGuid().ToString() }));
        _speechServiceMock.Setup(x => x.UserRequestOutgoingsSummary(It.IsAny<string>())).Returns(true);
        _userOutgoingsService.Setup(x => x.GetOutgoingsSummary(It.IsAny<User>()))
            .Returns(Task.FromResult(new List<UserOutgoing>
            {
                new UserOutgoing{
                    Ammount = 1111,
                    TagId = "TestTag",
                    Tag = new OutgoingsTag
                    {
                        Id = "TestTag",
                        Name = "Test Tag",
                        OutgoingsCategoryId = "TestCategory",
                        OutgoingsCategory = new OutgoingsCategory
                        {
                            Id = "TestCategory",
                            Name = "Test category"
                        }
                    }
                }
            }));

        WhatsappSenderController controller = new WhatsappSenderController(
            _messageSenderMock.Object,
            _loggerServiceMock.Object,
            _userServiceMock.Object,
            _speechServiceMock.Object,
            _userOutgoingsService.Object,
            _userConvoService.Object);

        // When
        var res = await controller.MessageReceived(BaseData);

        // Then
        Assert.Equal("OK", res);
    }


    [Fact]
    public async Task MessageReceived_UserTextContainsNumbers_NotMatchedTag_Success()
    {
        // Given
        _messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));
        _userServiceMock.Setup(x => x.GetUserAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new User { Id = Guid.NewGuid().ToString() }));
        _speechServiceMock.Setup(x => x.UserRequestOutgoingsSummary(It.IsAny<string>())).Returns(false);
        Mock<IVaultInformationService> vaultMock = new Mock<IVaultInformationService>();
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.Numbers)).Returns(new List<string> { "3" });
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.SummaryRequest)).Returns(new List<string> { "summary" });
        SpeechRecognitionService speechService = new SpeechRecognitionService(vaultMock.Object);
        _userConvoService.Setup(x => x.GetAvailableTags()).Returns(Task.FromResult(new List<OutgoingsTag> { new OutgoingsTag { Name = "test" } } as IList<OutgoingsTag>));
        ModifyBaseDataText("NonAplicable 3000");
        WhatsappSenderController controller = new WhatsappSenderController(
            _messageSenderMock.Object,
            _loggerServiceMock.Object,
            _userServiceMock.Object,
            speechService,
            _userOutgoingsService.Object,
            _userConvoService.Object);

        // When
        var res = await controller.MessageReceived(BaseData);

        // Then
        Assert.Equal("OK", res);
    }
    [Fact]
    public async Task MessageReceived_UserTextContainsNumbers_WithMatchedTag_Success()
    {
        // Given
        _messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));
        _userServiceMock.Setup(x => x.GetUserAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new User { Id = Guid.NewGuid().ToString() }));
        _speechServiceMock.Setup(x => x.UserRequestOutgoingsSummary(It.IsAny<string>())).Returns(false);
        Mock<IVaultInformationService> vaultMock = new Mock<IVaultInformationService>();
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.Numbers)).Returns(new List<string> { "3" });
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.SummaryRequest)).Returns(new List<string> { "summary" });
        SpeechRecognitionService speechService = new SpeechRecognitionService(vaultMock.Object);
        _userConvoService.Setup(x => x.GetAvailableTags()).Returns(Task.FromResult(new List<OutgoingsTag> { new OutgoingsTag { Name = "test" } } as IList<OutgoingsTag>));
        ModifyBaseDataText("Test 3000");
        WhatsappSenderController controller = new WhatsappSenderController(
            _messageSenderMock.Object,
            _loggerServiceMock.Object,
            _userServiceMock.Object,
            speechService,
            _userOutgoingsService.Object,
            _userConvoService.Object);

        // When
        var res = await controller.MessageReceived(BaseData);

        // Then
        Assert.Equal("OK", res);
    }

    [Fact]
    public async Task MessageReceived_UserTextWithoutNumbers_WithExistingTag_Success()
    {
        // Given
        _messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));
        _userServiceMock.Setup(x => x.GetUserAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new User { Id = Guid.NewGuid().ToString() }));
        _speechServiceMock.Setup(x => x.UserRequestOutgoingsSummary(It.IsAny<string>())).Returns(false);
        Mock<IVaultInformationService> vaultMock = new Mock<IVaultInformationService>();
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.Numbers)).Returns(new List<string> { "3" });
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.SummaryRequest)).Returns(new List<string> { "summary" });
        SpeechRecognitionService speechService = new SpeechRecognitionService(vaultMock.Object);
        _userConvoService.Setup(x => x.GetAvailableTags()).Returns(Task.FromResult(new List<OutgoingsTag> { new OutgoingsTag { Name = "test", OutgoingsCategory = new OutgoingsCategory { Name = "TestCategory" } } } as IList<OutgoingsTag>));
        _userConvoService.Setup(x => x.GetConversation(It.IsAny<User>())).Returns(Task.FromResult(new Conversation { Ammount = 1111 }));
        _userConvoService.Setup(x => x.UpdateConversationCategory(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(new Conversation { Ammount = 1111 }));
        ModifyBaseDataText("Test");
        WhatsappSenderController controller = new WhatsappSenderController(
            _messageSenderMock.Object,
            _loggerServiceMock.Object,
            _userServiceMock.Object,
            speechService,
            _userOutgoingsService.Object,
            _userConvoService.Object);

        // When
        var res = await controller.MessageReceived(BaseData);

        // Then
        Assert.Equal("OK", res);
    }

    [Fact]
    public async Task MessageReceived_UserTextWithoutNumbers_WithouthExistingTag_Success()
    {
        // Given
        _messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));
        _userServiceMock.Setup(x => x.GetUserAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new User { Id = Guid.NewGuid().ToString() }));
        _speechServiceMock.Setup(x => x.UserRequestOutgoingsSummary(It.IsAny<string>())).Returns(false);
        Mock<IVaultInformationService> vaultMock = new Mock<IVaultInformationService>();
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.Numbers)).Returns(new List<string> { "3" });
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.SummaryRequest)).Returns(new List<string> { "summary" });
        SpeechRecognitionService speechService = new SpeechRecognitionService(vaultMock.Object);
        _userConvoService.Setup(x => x.GetAvailableTags()).Returns(Task.FromResult(new List<OutgoingsTag> { new OutgoingsTag { Name = "test", OutgoingsCategory = new OutgoingsCategory { Name = "TestCategory" } } } as IList<OutgoingsTag>));
        _userConvoService.Setup(x => x.GetConversation(It.IsAny<User>())).Returns(Task.FromResult(new Conversation { Ammount = 1111, TagName = "test" }));
        _userConvoService.Setup(x => x.UpdateConversationCategory(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(new Conversation { Ammount = 1111 }));
        ModifyBaseDataText("NonApplicable");
        WhatsappSenderController controller = new WhatsappSenderController(
            _messageSenderMock.Object,
            _loggerServiceMock.Object,
            _userServiceMock.Object,
            speechService,
            _userOutgoingsService.Object,
            _userConvoService.Object);

        // When
        var res = await controller.MessageReceived(BaseData);

        // Then
        Assert.Equal("OK", res);
    }

    [Fact]
    public async Task MessageReceived_UserTextWithoutNumbers_GivingConfirmation_Success()
    {
        // Given
        _messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));
        _userServiceMock.Setup(x => x.GetUserAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new User { Id = Guid.NewGuid().ToString() }));
        _speechServiceMock.Setup(x => x.UserRequestOutgoingsSummary(It.IsAny<string>())).Returns(false);
        Mock<IVaultInformationService> vaultMock = new Mock<IVaultInformationService>();
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.Numbers)).Returns(new List<string> { "3" });
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.SummaryRequest)).Returns(new List<string> { "summary" });
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.Affirmations)).Returns(new List<string> { "yes" });
        SpeechRecognitionService speechService = new SpeechRecognitionService(vaultMock.Object);
        _userConvoService.Setup(x => x.GetAvailableTags()).Returns(Task.FromResult(new List<OutgoingsTag> { new OutgoingsTag { Name = "test", OutgoingsCategory = new OutgoingsCategory { Name = "TestCategory" } } } as IList<OutgoingsTag>));
        _userConvoService.Setup(x => x.GetConversation(It.IsAny<User>())).Returns(Task.FromResult(new Conversation { Ammount = 1111, TagName = "test", CategoryName = "TestCategory" }));
        _userConvoService.Setup(x => x.UpdateConversationCategory(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(new Conversation { Ammount = 1111 }));
        ModifyBaseDataText("yes");
        WhatsappSenderController controller = new WhatsappSenderController(
            _messageSenderMock.Object,
            _loggerServiceMock.Object,
            _userServiceMock.Object,
            speechService,
            _userOutgoingsService.Object,
            _userConvoService.Object);

        // When
        var res = await controller.MessageReceived(BaseData);

        // Then
        Assert.Equal("OK", res);
    }

    [Fact]
    public async Task MessageReceived_UserTextWithoutNumbers_DoesNotGiveConfirmation_Success()
    {
        // Given
        _messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));
        _userServiceMock.Setup(x => x.GetUserAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(new User { Id = Guid.NewGuid().ToString() }));
        _speechServiceMock.Setup(x => x.UserRequestOutgoingsSummary(It.IsAny<string>())).Returns(false);
        Mock<IVaultInformationService> vaultMock = new Mock<IVaultInformationService>();
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.Numbers)).Returns(new List<string> { "3" });
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.SummaryRequest)).Returns(new List<string> { "summary" });
        vaultMock.Setup(x => x.GetUserKeyWords(SpeechType.Affirmations)).Returns(new List<string> { "yes" });
        SpeechRecognitionService speechService = new SpeechRecognitionService(vaultMock.Object);
        _userConvoService.Setup(x => x.GetAvailableTags()).Returns(Task.FromResult(new List<OutgoingsTag> { new OutgoingsTag { Name = "test", OutgoingsCategory = new OutgoingsCategory { Name = "TestCategory" } } } as IList<OutgoingsTag>));
        _userConvoService.Setup(x => x.GetConversation(It.IsAny<User>())).Returns(Task.FromResult(new Conversation { Ammount = 1111, TagName = "test", CategoryName = "TestCategory" }));
        _userConvoService.Setup(x => x.UpdateConversationCategory(It.IsAny<User>(), It.IsAny<string>())).Returns(Task.FromResult(new Conversation { Ammount = 1111 }));
        ModifyBaseDataText("no");
        WhatsappSenderController controller = new WhatsappSenderController(
            _messageSenderMock.Object,
            _loggerServiceMock.Object,
            _userServiceMock.Object,
            speechService,
            _userOutgoingsService.Object,
            _userConvoService.Object);

        // When
        var res = await controller.MessageReceived(BaseData);

        // Then
        Assert.Equal("OK", res);
    }


    [Fact]
    public async Task MessageReceived_Failure()
    {
        // Given
        _loggerServiceMock.Setup(x => x.SaveLog(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<ActionType>()))
            .Returns(Task.FromResult(Log.Build("This is a test", true, ActionType.InternalProcess)));

        _messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));

        //Object left incomplete on purpose
        WhatsappMessagesData data = new WhatsappMessagesData
        {
            Entry = new List<EntryData>{
                new EntryData{
                    Changes = new List<ChangesOnMessages>{
                        new ChangesOnMessages()
                    }
                }
            }
        };

        WhatsappSenderController controller = new WhatsappSenderController(
            _messageSenderMock.Object,
            _loggerServiceMock.Object,
            _userServiceMock.Object,
            _speechServiceMock.Object,
            _userOutgoingsService.Object,
            _userConvoService.Object);
        // When
        await Assert.ThrowsAsync(typeof(NullReferenceException), async () =>
        {
            await controller.MessageReceived(data);
        });
    }

}