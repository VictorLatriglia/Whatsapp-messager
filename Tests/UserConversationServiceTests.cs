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
    public async Task CreateConversation_NoCategory_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        Mock<IRepository<OutgoingsCategory>> categoryMock = new Mock<IRepository<OutgoingsCategory>>();
        convoMock.Setup(x => x.AddAsync(It.IsAny<Conversation>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid() }));
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, categoryMock.Object);

        // When
        var res = await service.CreateConversation(new User { Id = Guid.NewGuid() }, 1);
        // Then
        Assert.NotNull(res);
    }

    [Fact]
    public async Task CreateConversation_WithCategory_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        convoMock.Setup(x => x.AddAsync(It.IsAny<Conversation>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid() }));
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "MyTestDatabase")
            .Options;
        var Context = new ApplicationDbContext(options);
        var categoryRepo = new Repository<OutgoingsCategory>(Context);
        await categoryRepo.AddAsync(new OutgoingsCategory { Name = "TEST"});
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, categoryRepo);

        // When
        var res = await service.CreateConversation(new User { Id = Guid.NewGuid() }, 1, "TEST");
        // Then
        Assert.NotNull(res);
    }

    

    [Fact]
    public async Task GetConversation_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        Mock<IRepository<OutgoingsCategory>> categoryMock = new Mock<IRepository<OutgoingsCategory>>();
        convoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid() }));
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, categoryMock.Object);

        // When
        var res = await service.GetConversation(new User { Id = Guid.NewGuid() });
        // Then
        Assert.NotNull(res);
    }

    [Fact]
    public async Task UpdateConversation_Category_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        Mock<IRepository<OutgoingsCategory>> categoryMock = new Mock<IRepository<OutgoingsCategory>>();
        convoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid() }));
        convoMock.Setup(x => x.UpdateAsync(It.IsAny<Conversation>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid() }));
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, categoryMock.Object);

        // When
        var res = await service.UpdateConversationCategory(new User { Id = Guid.NewGuid() }, "TEST");
        // Then
        Assert.NotNull(res);
    }
   
    [Fact]
    public async Task DeleteConversation_Success()
    {
        Mock<IRepository<Conversation>> convoMock = new Mock<IRepository<Conversation>>();
        Mock<IRepository<OutgoingsCategory>> categoryMock = new Mock<IRepository<OutgoingsCategory>>();
        convoMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Conversation, bool>>>())).Returns(Task.FromResult(new Conversation { Id = Guid.NewGuid() }));
        convoMock.Setup(x => x.DeleteAsync(It.IsAny<Conversation>())).Returns(Task.CompletedTask);
        // Given
        UserConversationService service = new UserConversationService(convoMock.Object, categoryMock.Object);

        // When
        Task completeDeletion = service.DeleteConversation(new User { Id = Guid.NewGuid() });
        await completeDeletion;
        // Then
        Assert.Equal(Task.CompletedTask, completeDeletion);
    }
}