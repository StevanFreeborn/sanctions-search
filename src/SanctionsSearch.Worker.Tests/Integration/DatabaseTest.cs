namespace SanctionsSearch.Worker.Tests.Integration;

public class DatabaseTest : IAsyncLifetime
{
  private readonly AppDbContext _appDbContext;
  protected readonly Mock<TimeProvider> _timeProviderMock = new();
  protected readonly DbContext _context;
  protected readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.ClearProviders());


  public DatabaseTest()
  {
    var options = new DbOptions { DatabaseName = $"{Guid.NewGuid()}.db" };
    var context = new AppDbContext(options, _timeProviderMock.Object);

    _appDbContext = context;
    _context = context;
  }

  public virtual async Task InitializeAsync()
  {
    await _context.Database.MigrateAsync();
  }

  public virtual async Task DisposeAsync()
  {
    await _appDbContext.DisposeAsync();

    SqliteConnection.ClearAllPools();

    File.Delete(_appDbContext.DatabasePath);
  }
}