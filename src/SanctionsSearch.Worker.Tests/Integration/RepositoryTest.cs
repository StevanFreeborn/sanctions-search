namespace SanctionsSearch.Worker.Tests.Integration;

public class RepositoryTest : IAsyncLifetime
{
  private readonly AppDbContext _appDbContext;
  protected readonly Mock<TimeProvider> _timeProvider = new();
  protected readonly DbContext _context;

  public RepositoryTest()
  {
    var options = new DbOptions { DatabaseName = $"{Guid.NewGuid()}.db" };
    var context = new AppDbContext(options, _timeProvider.Object);

    _appDbContext = context;
    _context = context;
  }

  public async Task InitializeAsync()
  {
    await _context.Database.MigrateAsync();
  }

  public async Task DisposeAsync()
  {
    await _appDbContext.DisposeAsync();

    SqliteConnection.ClearAllPools();

    File.Delete(_appDbContext.DatabasePath);
  }
}