namespace Tests;
using Microsoft.EntityFrameworkCore;
using Whatsapp_bot.DataAccess.Context;
using Whatsapp_bot.DataAccess.Repository;
using Whatsapp_bot.Models.EntityModels;

public class RepositoryTests
{
    ApplicationDbContext Context;
    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "MyTestDatabase")
            .Options;
        Context = new ApplicationDbContext(options);
    }
    [Fact]
    public async Task AddInstance_Success()
    {
        // Given
        Repository<Log> repo = new Repository<Log>(Context);

        // When
        var log = await repo.AddAsync(Log.Build("TEST", false, ActionType.InternalProcess));

        // Then
        Assert.NotEqual(Guid.Empty, log.Id);
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Given
        Repository<Log> repo = new Repository<Log>(Context);

        // When
        var log = await repo.AddAsync(Log.Build("TEST", false, ActionType.InternalProcess));
        var logs = await repo.GetAllAsync();
        // Then
        Assert.Contains(log, logs);
    }

    [Fact]
    public async Task GetOne_Success()
    {
        // Given
        Repository<Log> repo = new Repository<Log>(Context);

        // When
        var log = await repo.AddAsync(Log.Build("TEST", false, ActionType.InternalProcess));
        var savedLog = await repo.GetAsync(x => x.Id.Equals(log.Id));
        // Then
        Assert.NotNull(savedLog);
    }

    [Fact]
    public async Task GetQueried_Success()
    {
        // Given
        Repository<Log> repo = new Repository<Log>(Context);

        // When
        var log = await repo.AddAsync(Log.Build("TEST", false, ActionType.InternalProcess));
        var logs = await repo.QueryAsync(x => x.Id.Equals(log.Id));
        // Then
        Assert.Contains(log, logs.ToList());
    }

    [Fact]
    public async Task AsQueryable_Success()
    {
        // Given
        Repository<Log> repo = new Repository<Log>(Context);

        // When
        var log = await repo.AddAsync(Log.Build("TEST", false, ActionType.InternalProcess));
        var logs = repo.AsQueryable();
        // Then
        Assert.Contains(log, logs.ToList());
    }

    [Fact]
    public async Task Update_Success()
    {
        // Given
        Repository<Log> repo = new Repository<Log>(Context);

        // When
        var log = await repo.AddAsync(Log.Build("TEST", false, ActionType.InternalProcess));
        var logs = await repo.GetAllAsync();
        Assert.Contains(log, logs);
        log.LogData = "MODIFIED";
        await repo.UpdateAsync(log);
        logs = await repo.GetAllAsync();
        // Then
        Assert.Equal("MODIFIED", log.LogData);
    }

    [Fact]
    public async Task Delete_Success()
    {
        // Given
        Repository<Log> repo = new Repository<Log>(Context);

        // When
        var log = await repo.AddAsync(Log.Build("TEST", false, ActionType.InternalProcess));
        var logs = await repo.GetAllAsync();
        Assert.Contains(log, logs);
        await repo.DeleteAsync(log);
        logs = await repo.GetAllAsync();
        // Then
        Assert.DoesNotContain(log, logs);
    }
}