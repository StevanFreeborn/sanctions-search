namespace SanctionsSearch.Worker.Tests.Integration;

public class SdnRepositoryTests : IAsyncLifetime
{
  private readonly AppDbContext _context;
  private readonly SdnRepository _sdnRepository;
  private readonly SdnFaker _sdnFaker = new();
  private readonly Mock<TimeProvider> _timeProvider = new();

  public SdnRepositoryTests()
  {
    var loggerFactory = LoggerFactory.Create(builder => builder.ClearProviders());
    var options = new DbOptions { DatabaseName = $"{Guid.NewGuid()}.db" };

    _context = new AppDbContext(options, _timeProvider.Object);
    _sdnRepository = new SdnRepository(_context, new Logger<SdnRepository>(loggerFactory));
  }

  [Fact]
  public async Task Upsert_WithNewEntity_ShouldAddEntityToDatabase()
  {
    var now = DateTimeOffset.UtcNow;
    _timeProvider.Setup(x => x.GetUtcNow()).Returns(now);

    var sdn = _sdnFaker.Generate();

    await _sdnRepository.Upsert(sdn);
    await _context.SaveChangesAsync();

    var result = await _context.Sdns.FindAsync(sdn.Id);

    result.Should().BeEquivalentTo(sdn);
    result!.CreatedAt.Should().Be(now.DateTime);
    result.UpdatedAt.Should().Be(now.DateTime);
  }

  [Fact]
  public async Task Upsert_WithExistingEntity_ShouldUpdateEntityInDatabase()
  {
    var createdTimeStamp = DateTimeOffset.UtcNow;
    var updatedTimeStamp = createdTimeStamp.AddSeconds(2);

    _timeProvider.Setup(x => x.GetUtcNow()).Returns(createdTimeStamp);

    var sdn = _sdnFaker.Generate();

    // Insert the entity
    await _sdnRepository.Upsert(sdn);
    await _context.SaveChangesAsync();

    // Assert the entity was created
    var createdSdn = await _context.Sdns.FindAsync(sdn.Id);

    createdSdn.Should().BeEquivalentTo(sdn);
    createdSdn!.CreatedAt.Should().Be(createdTimeStamp.DateTime);
    createdSdn.UpdatedAt.Should().Be(createdTimeStamp.DateTime);

    _timeProvider.Setup(x => x.GetUtcNow()).Returns(updatedTimeStamp);

    // Update the entity
    sdn.Name = "Updated";
    await _sdnRepository.Upsert(sdn);
    await _context.SaveChangesAsync();

    // Assert the entity was updated
    var result = await _context.Sdns.FindAsync(sdn.Id);

    result.Should().BeEquivalentTo(sdn);
    result!.CreatedAt.Should().BeSameDateAs(createdTimeStamp.DateTime);
    result.UpdatedAt.Should().BeSameDateAs(updatedTimeStamp.DateTime);
  }

  [Fact]
  public async Task Find_WithPredicate_ShouldReturnEntitiesMatchingPredicate()
  {
    var sdn1 = _sdnFaker.Generate();
    var sdn2 = _sdnFaker.Generate();
    var sdn3 = _sdnFaker.Generate();

    await _sdnRepository.Upsert(sdn1);
    await _sdnRepository.Upsert(sdn2);
    await _sdnRepository.Upsert(sdn3);
    await _context.SaveChangesAsync();

    var result = await _sdnRepository.Find(x => x.Id == sdn2.Id);

    result.Should().HaveCount(1);
    result.First().Should().BeEquivalentTo(sdn2);
  }

  public async Task InitializeAsync()
  {
    await _context.Database.MigrateAsync();
  }

  public async Task DisposeAsync()
  {
    await _context.DisposeAsync();

    SqliteConnection.ClearAllPools();

    File.Delete(_context.DatabasePath);
  }
}