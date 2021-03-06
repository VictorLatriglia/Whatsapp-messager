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
        Mock<IRepository<UserOutgoing>> userOutgoingRepoMock = new Mock<IRepository<UserOutgoing>>();
        Mock<IRepository<OutgoingsCategory>> categoriesRepoMock = new Mock<IRepository<OutgoingsCategory>>();
        Mock<IRepository<OutgoingsTag>> tagsRepoMock = new Mock<IRepository<OutgoingsTag>>();
        categoriesRepoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OutgoingsCategory, bool>>>()))
            .Returns(Task.FromResult(new OutgoingsCategory { Id = Guid.NewGuid().ToString() }));
        tagsRepoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OutgoingsTag, bool>>>()))
            .Returns(Task.FromResult(new OutgoingsTag { Id = Guid.NewGuid().ToString() }));
        userOutgoingRepoMock.Setup(x => x.AddAsync(It.IsAny<UserOutgoing>()))
                    .Returns(Task.FromResult(new UserOutgoing { Id = Guid.NewGuid().ToString() }));

        UserOutgoingsService userService = new UserOutgoingsService(userOutgoingRepoMock.Object, categoriesRepoMock.Object, tagsRepoMock.Object);
        // When
        var res = await userService.AddOutgoing(1111, "TEST", "TEST", new User { Id = Guid.NewGuid().ToString() });
        // Then
        Assert.NotEqual("", res.Id);
    }

    [Fact]
    public async Task AddOutgoing_NoTags()
    {
        // Given
        Mock<IRepository<UserOutgoing>> userOutgoingRepoMock = new Mock<IRepository<UserOutgoing>>();
        Mock<IRepository<OutgoingsCategory>> categoriesRepoMock = new Mock<IRepository<OutgoingsCategory>>();
        Mock<IRepository<OutgoingsTag>> tagsRepoMock = new Mock<IRepository<OutgoingsTag>>();
        categoriesRepoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OutgoingsCategory, bool>>>()))
            .Returns(Task.FromResult(new OutgoingsCategory { Id = Guid.NewGuid().ToString() }));
        tagsRepoMock.Setup(x => x.AddAsync(It.IsAny<OutgoingsTag>()))
            .Returns(Task.FromResult(new OutgoingsTag { Id = Guid.NewGuid().ToString() }));
        userOutgoingRepoMock.Setup(x => x.AddAsync(It.IsAny<UserOutgoing>()))
                .Returns(Task.FromResult(new UserOutgoing { Id = Guid.NewGuid().ToString() }));
        UserOutgoingsService userService = new UserOutgoingsService(userOutgoingRepoMock.Object, categoriesRepoMock.Object, tagsRepoMock.Object);
        // When
        var res = await userService.AddOutgoing(1111, "TEST", "TEST", new User { Id = Guid.NewGuid().ToString() });
        // Then
        Assert.NotEqual("", res.Id);
    }

    [Fact]
    public async Task AddOutgoing_NoTagsNoCategory()
    {
        // Given
        Mock<IRepository<UserOutgoing>> userOutgoingRepoMock = new Mock<IRepository<UserOutgoing>>();
        Mock<IRepository<OutgoingsCategory>> categoriesRepoMock = new Mock<IRepository<OutgoingsCategory>>();
        Mock<IRepository<OutgoingsTag>> tagsRepoMock = new Mock<IRepository<OutgoingsTag>>();
        categoriesRepoMock.Setup(x => x.AddAsync(It.IsAny<OutgoingsCategory>()))
            .Returns(Task.FromResult(new OutgoingsCategory { Id = Guid.NewGuid().ToString() }));
        tagsRepoMock.Setup(x => x.AddAsync(It.IsAny<OutgoingsTag>()))
            .Returns(Task.FromResult(new OutgoingsTag { Id = Guid.NewGuid().ToString() }));
        userOutgoingRepoMock.Setup(x => x.AddAsync(It.IsAny<UserOutgoing>()))
                .Returns(Task.FromResult(new UserOutgoing { Id = Guid.NewGuid().ToString() }));
        UserOutgoingsService userService = new UserOutgoingsService(userOutgoingRepoMock.Object, categoriesRepoMock.Object, tagsRepoMock.Object);
        // When
        var res = await userService.AddOutgoing(1111, "TEST", "TEST", new User { Id = Guid.NewGuid().ToString() });
        // Then
        Assert.NotEqual("", res.Id);
    }
}