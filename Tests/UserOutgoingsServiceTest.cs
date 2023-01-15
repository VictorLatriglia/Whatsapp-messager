namespace Tests;

using System.Linq.Expressions;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.Services;

public class UserOutgoingsServiceTests
{
    [Fact]
    public async Task AddOutgoing_Success()
    {
        // Given
        Mock<IRepository<MoneyMovement>> userOutgoingRepoMock = new Mock<IRepository<MoneyMovement>>();
        Mock<IRepository<OutgoingsCategory>> categoriesRepoMock = new Mock<IRepository<OutgoingsCategory>>();
        categoriesRepoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OutgoingsCategory, bool>>>()))
            .Returns(Task.FromResult(new OutgoingsCategory { Id = Guid.NewGuid() }));
        userOutgoingRepoMock.Setup(x => x.AddAsync(It.IsAny<MoneyMovement>()))
                    .Returns(Task.FromResult(new MoneyMovement { Id = Guid.NewGuid() }));

        UserOutgoingsService userService = new UserOutgoingsService(userOutgoingRepoMock.Object, categoriesRepoMock.Object);
        // When
        var res = await userService.AddOutgoing(1111, "TEST", "TEST", new User { Id = Guid.NewGuid() });
        // Then
        Assert.NotEqual(Guid.Empty, res.Id);
    }

    [Fact]
    public async Task AddOutgoing_NoTags()
    {
        // Given
        Mock<IRepository<MoneyMovement>> userOutgoingRepoMock = new Mock<IRepository<MoneyMovement>>();
        Mock<IRepository<OutgoingsCategory>> categoriesRepoMock = new Mock<IRepository<OutgoingsCategory>>();

        categoriesRepoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OutgoingsCategory, bool>>>()))
            .Returns(Task.FromResult(new OutgoingsCategory { Id = Guid.NewGuid() }));

        userOutgoingRepoMock.Setup(x => x.AddAsync(It.IsAny<MoneyMovement>()))
                .Returns(Task.FromResult(new MoneyMovement { Id = Guid.NewGuid() }));
        UserOutgoingsService userService = new UserOutgoingsService(userOutgoingRepoMock.Object, categoriesRepoMock.Object);
        // When
        var res = await userService.AddOutgoing(1111, "TEST", "TEST", new User { Id = Guid.NewGuid() });
        // Then
        Assert.NotEqual(Guid.Empty, res.Id);
    }

    [Fact]
    public async Task AddOutgoing_NoTagsNoCategory()
    {
        // Given
        Mock<IRepository<MoneyMovement>> userOutgoingRepoMock = new Mock<IRepository<MoneyMovement>>();
        Mock<IRepository<OutgoingsCategory>> categoriesRepoMock = new Mock<IRepository<OutgoingsCategory>>();

        categoriesRepoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OutgoingsCategory, bool>>>()))
            .Returns(Task.FromResult(new OutgoingsCategory { Id = Guid.NewGuid(), Name = "TEST" }));
        userOutgoingRepoMock.Setup(x => x.AddAsync(It.IsAny<MoneyMovement>()))
                .Returns(Task.FromResult(new MoneyMovement { Id = Guid.NewGuid() }));
        UserOutgoingsService userService = new UserOutgoingsService(userOutgoingRepoMock.Object, categoriesRepoMock.Object);
        // When
        var res = await userService.AddOutgoing(1111, "TEST", "TEST", new User { Id = Guid.NewGuid() });
        // Then
        Assert.NotEqual(Guid.Empty, res.Id);
    }
}