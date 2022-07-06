namespace Tests;

using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.Services;
using Xunit;
using Moq;

public class LoggerServiceTests
{
    [Fact]
    public async Task SaveLog_Success()
    {
        // Given
        Mock<IRepository<Log>> moq = new Mock<IRepository<Log>>();
        moq.Setup(x => x.AddAsync(It.IsAny<Log>())).Returns(Task.FromResult(new Log()));
        LoggerService service = new LoggerService(moq.Object);

        // When
        var res = await service.SaveLog("From Unit test", false, ActionType.InternalProcess);
        
        // Then
        Assert.NotEqual("", res.Id);
    }
}