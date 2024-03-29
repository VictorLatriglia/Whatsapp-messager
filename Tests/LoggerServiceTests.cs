namespace Tests;

using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.Services;

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
        Assert.NotEqual(Guid.Empty, res.Id);
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Given
        Mock<IRepository<Log>> moq = new Mock<IRepository<Log>>();
        moq.Setup(x => x.GetAllAsync()).Returns(
            Task.FromResult(
                new List<Log>()
                {
                    new Log { Id = Guid.NewGuid() }
                } as IList<Log>));
        LoggerService service = new LoggerService(moq.Object);

        // When
        var res = await service.GetAll();

        // Then
        Assert.NotEmpty(res);
    }
}