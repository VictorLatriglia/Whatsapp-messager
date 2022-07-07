using Whatsapp_bot.Controllers;
using Whatsapp_bot.Models;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;

namespace Tests;
public class WhatsappMessageControllerTests
{
    [Fact]
    public async Task SendMessage_Success()
    {
        // Given
        Mock<IWhatsappMessageSenderService> messageSenderMock = new Mock<IWhatsappMessageSenderService>();
        Mock<ILoggerService> loggerServiceMock = new Mock<ILoggerService>();

        messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));

        WhatsappSenderController controller = new WhatsappSenderController(messageSenderMock.Object, loggerServiceMock.Object);
        // When
        var res = await controller.SendMessage("This is a test", "12345");

        // Then
        Assert.Equal("OK", res);
    }

    [Fact]
    public async Task MessageReceived_Success()
    {
        // Given
        Mock<IWhatsappMessageSenderService> messageSenderMock = new Mock<IWhatsappMessageSenderService>();
        Mock<ILoggerService> loggerServiceMock = new Mock<ILoggerService>();

        messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult("OK"));

        WhatsappMessagesData data = new WhatsappMessagesData
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

        WhatsappSenderController controller = new WhatsappSenderController(messageSenderMock.Object, loggerServiceMock.Object);
        // When
        var res = await controller.MessageReceived(data);

        // Then
        Assert.Equal("OK", res);
    }

    [Fact]
    public async Task MessageReceived_Failure()
    {
        // Given
        Mock<IWhatsappMessageSenderService> messageSenderMock = new Mock<IWhatsappMessageSenderService>();
        Mock<ILoggerService> loggerServiceMock = new Mock<ILoggerService>();

        loggerServiceMock.Setup(x => x.SaveLog(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<ActionType>()))
            .Returns(Task.FromResult(Log.Build("This is a test", true, ActionType.InternalProcess)));

        messageSenderMock.Setup(x => x.SendMessage(It.IsAny<string>(), It.IsAny<string>()))
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

        WhatsappSenderController controller = new WhatsappSenderController(messageSenderMock.Object, loggerServiceMock.Object);
        // When
        await Assert.ThrowsAsync(typeof(NullReferenceException), async () =>
        {
            await controller.MessageReceived(data);
        });
    }

}