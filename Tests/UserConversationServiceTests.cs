using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Context;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.Services;

namespace Tests;
public class UserConversationServiceTests
{
    [Fact]
    public async Task CreateConversation_NoTag_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        Mock<IRepository<OutgoingsTag>> tagsMock = new Mock<IRepository<OutgoingsTag>>();
        convoMock.Setup(x => x.AddAsync(It.IsAny<Conversation>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid().ToString() }));
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, tagsMock.Object);

        // When
        var res = await service.CreateConversation(new User { Id = Guid.NewGuid().ToString() }, 1);
        // Then
        Assert.NotNull(res);
    }

    [Fact]
    public async Task CreateConversation_WithTag_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        convoMock.Setup(x => x.AddAsync(It.IsAny<Conversation>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid().ToString() }));
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "MyTestDatabase")
            .Options;
        var Context = new ApplicationDbContext(options);
        var tagsRepo = new Repository<OutgoingsTag>(Context);
        await tagsRepo.AddAsync(new OutgoingsTag { Name = "TEST", OutgoingsCategory = new OutgoingsCategory { Name = "CategoryTest" } });
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, tagsRepo);

        // When
        var res = await service.CreateConversation(new User { Id = Guid.NewGuid().ToString() }, 1, "TEST");
        // Then
        Assert.NotNull(res);
    }

    [Fact]
    public async Task GetTags_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        Mock<IRepository<OutgoingsTag>> tagsMock = new Mock<IRepository<OutgoingsTag>>();
        tagsMock.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(new List<OutgoingsTag> { new OutgoingsTag { Id = Guid.NewGuid().ToString() } } as IList<OutgoingsTag>));
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, tagsMock.Object);

        // When
        var res = await service.GetAvailableTags();
        // Then
        Assert.NotNull(res);
    }

    [Fact]
    public async Task GetConversation_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        Mock<IRepository<OutgoingsTag>> tagsMock = new Mock<IRepository<OutgoingsTag>>();
        convoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid().ToString() }));
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, tagsMock.Object);

        // When
        var res = await service.GetConversation(new User { Id = Guid.NewGuid().ToString() });
        // Then
        Assert.NotNull(res);
    }

    [Fact]
    public async Task UpdateConversation_Tag_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        Mock<IRepository<OutgoingsTag>> tagsMock = new Mock<IRepository<OutgoingsTag>>();
        convoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid().ToString() }));
        convoMock.Setup(x => x.UpdateAsync(It.IsAny<Conversation>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid().ToString() }));
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, tagsMock.Object);

        // When
        var res = await service.UpdateConversationTag(new User { Id = Guid.NewGuid().ToString() }, "TEST");
        // Then
        Assert.NotNull(res);
    }
    [Fact]
    public async Task UpdateConversation_Category_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        Mock<IRepository<OutgoingsTag>> tagsMock = new Mock<IRepository<OutgoingsTag>>();
        convoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid().ToString() }));
        convoMock.Setup(x => x.UpdateAsync(It.IsAny<Conversation>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid().ToString() }));
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, tagsMock.Object);

        // When
        var res = await service.UpdateConversationCategory(new User { Id = Guid.NewGuid().ToString() }, "TEST");
        // Then
        Assert.NotNull(res);
    }

    [Fact]
    public async Task DeleteConversation_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        Mock<IRepository<OutgoingsTag>> tagsMock = new Mock<IRepository<OutgoingsTag>>();
        convoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid().ToString() }));
        convoMock.Setup(x => x.DeleteAsync(It.IsAny<Conversation>())).Returns(Task.CompletedTask);
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, tagsMock.Object);

        // When
        Task completeDeletion = service.DeleteConversation(new User { Id = Guid.NewGuid().ToString() });
        await completeDeletion;
        // Then
        Assert.Equal(Task.CompletedTask, completeDeletion);
    }
}