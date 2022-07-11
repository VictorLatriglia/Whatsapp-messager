namespace Tests;

using System.Linq.Expressions;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.ServiceContracts;
using Whatsapp_bot.Services;
using Whatsapp_bot.Utils;

public class UserInformationServiceTests
{
    [Fact]
    public async Task AddUser_Success()
    {
        // Given
        Mock<IRepository<User>> userRepoMock = new Mock<IRepository<User>>();
        Mock<IRepository<UserOutgoing>> userOutgoingRepoMock = new Mock<IRepository<UserOutgoing>>();
        Mock<IRepository<OutgoingsCategory>> categoriesRepoMock = new Mock<IRepository<OutgoingsCategory>>();
        Mock<IRepository<OutgoingsTag>> tagsRepoMock = new Mock<IRepository<OutgoingsTag>>();
        userRepoMock.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.FromResult(new User { Id = Guid.NewGuid().ToString() }));
        UserInformationService userService = new UserInformationService(userRepoMock.Object, userOutgoingRepoMock.Object, categoriesRepoMock.Object, tagsRepoMock.Object);
        // When
        var res = await userService.AddUser("Test", "1234567890");
        // Then
        Assert.NotEqual("", res.Id);
    }

    [Fact]
    public async Task AddOutgoing_Success()
    {
        // Given
        Mock<IRepository<User>> userRepoMock = new Mock<IRepository<User>>();
        Mock<IRepository<UserOutgoing>> userOutgoingRepoMock = new Mock<IRepository<UserOutgoing>>();
        Mock<IRepository<OutgoingsCategory>> categoriesRepoMock = new Mock<IRepository<OutgoingsCategory>>();
        Mock<IRepository<OutgoingsTag>> tagsRepoMock = new Mock<IRepository<OutgoingsTag>>();
        categoriesRepoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OutgoingsCategory, bool>>>()))
            .Returns(Task.FromResult(new OutgoingsCategory { Id = Guid.NewGuid().ToString() }));
        tagsRepoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OutgoingsTag, bool>>>()))
            .Returns(Task.FromResult(new OutgoingsTag { Id = Guid.NewGuid().ToString() }));
        userOutgoingRepoMock.Setup(x => x.AddAsync(It.IsAny<UserOutgoing>()))
                    .Returns(Task.FromResult(new UserOutgoing { Id = Guid.NewGuid().ToString() }));

        UserInformationService userService = new UserInformationService(userRepoMock.Object, userOutgoingRepoMock.Object, categoriesRepoMock.Object, tagsRepoMock.Object);
        // When
        var res = await userService.AddOutgoing(1111, "TEST", "TEST", new User { Id = Guid.NewGuid().ToString() });
        // Then
        Assert.NotEqual("", res.Id);
    }

    [Fact]
    public async Task AddOutgoing_NoTags()
    {
        // Given
        Mock<IRepository<User>> userRepoMock = new Mock<IRepository<User>>();
        Mock<IRepository<UserOutgoing>> userOutgoingRepoMock = new Mock<IRepository<UserOutgoing>>();
        Mock<IRepository<OutgoingsCategory>> categoriesRepoMock = new Mock<IRepository<OutgoingsCategory>>();
        Mock<IRepository<OutgoingsTag>> tagsRepoMock = new Mock<IRepository<OutgoingsTag>>();
        categoriesRepoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<OutgoingsCategory, bool>>>()))
            .Returns(Task.FromResult(new OutgoingsCategory { Id = Guid.NewGuid().ToString() }));
        tagsRepoMock.Setup(x => x.AddAsync(It.IsAny<OutgoingsTag>()))
            .Returns(Task.FromResult(new OutgoingsTag { Id = Guid.NewGuid().ToString() }));
        userOutgoingRepoMock.Setup(x => x.AddAsync(It.IsAny<UserOutgoing>()))
                .Returns(Task.FromResult(new UserOutgoing { Id = Guid.NewGuid().ToString() }));
        UserInformationService userService = new UserInformationService(userRepoMock.Object, userOutgoingRepoMock.Object, categoriesRepoMock.Object, tagsRepoMock.Object);
        // When
        var res = await userService.AddOutgoing(1111, "TEST", "TEST", new User { Id = Guid.NewGuid().ToString() });
        // Then
        Assert.NotEqual("", res.Id);
    }

    [Fact]
    public async Task AddOutgoing_NoTagsNoCategory()
    {
        // Given
        Mock<IRepository<User>> userRepoMock = new Mock<IRepository<User>>();
        Mock<IRepository<UserOutgoing>> userOutgoingRepoMock = new Mock<IRepository<UserOutgoing>>();
        Mock<IRepository<OutgoingsCategory>> categoriesRepoMock = new Mock<IRepository<OutgoingsCategory>>();
        Mock<IRepository<OutgoingsTag>> tagsRepoMock = new Mock<IRepository<OutgoingsTag>>();
        categoriesRepoMock.Setup(x => x.AddAsync(It.IsAny<OutgoingsCategory>()))
            .Returns(Task.FromResult(new OutgoingsCategory { Id = Guid.NewGuid().ToString() }));
        tagsRepoMock.Setup(x => x.AddAsync(It.IsAny<OutgoingsTag>()))
            .Returns(Task.FromResult(new OutgoingsTag { Id = Guid.NewGuid().ToString() }));
        userOutgoingRepoMock.Setup(x => x.AddAsync(It.IsAny<UserOutgoing>()))
                .Returns(Task.FromResult(new UserOutgoing { Id = Guid.NewGuid().ToString() }));
        UserInformationService userService = new UserInformationService(userRepoMock.Object, userOutgoingRepoMock.Object, categoriesRepoMock.Object, tagsRepoMock.Object);
        // When
        var res = await userService.AddOutgoing(1111, "TEST", "TEST", new User { Id = Guid.NewGuid().ToString() });
        // Then
        Assert.NotEqual("", res.Id);
    }
}