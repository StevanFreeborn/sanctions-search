namespace SanctionsSearch.Worker.Tests.Integration;

public class RepositoryTest : IAsyncLifetime
{
  private readonly AppDbContext _appDbContext;
  private readonly SdnFaker _faker = new();
  private readonly Sdn _sdn;
  protected readonly Mock<TimeProvider> _timeProvider = new();
  protected readonly DbContext _context;
  protected int SdnId => _sdn.Id;

  public RepositoryTest()
  {
    var options = new DbOptions { DatabaseName = $"{Guid.NewGuid()}.db" };
    var context = new AppDbContext(options, _timeProvider.Object);

    _appDbContext = context;
    _context = context;
    _sdn = _faker.Generate();
    _sdn.Id = SdnFaker.ReservedId;
  }

  public async Task InitializeAsync()
  {
    await _context.Database.MigrateAsync();
    await _context.Set<Sdn>().AddAsync(_sdn);
  }

  public async Task DisposeAsync()
  {
    await _appDbContext.DisposeAsync();

    SqliteConnection.ClearAllPools();

    File.Delete(_appDbContext.DatabasePath);
  }
}