namespace Tests;

using System.Linq.Expressions;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;
using Whatsapp_bot.Services;

public class UserInformationServiceTests
{
    [Fact]
    public async Task AddUser_Success()
    {
        // Given
        Mock<IRepository<User>> userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.FromResult(new User { Id = Guid.NewGuid().ToString() }));
        UserInformationService userService = new UserInformationService(userRepoMock.Object);
        // When
        var res = await userService.AddUser("Test", "1234567890");
        // Then
        Assert.NotEqual("", res.Id);
    }
}