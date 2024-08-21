using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using SanctionsSearch.Worker.Tests.Fakes;

namespace SanctionsSearch.Worker.Tests.Integration;

public class EfRepositoryTests : IAsyncLifetime
{
  private readonly AppDbContext _context;
  private readonly EfRepository<Sdn> _sdnRepository;
  private readonly SdnFaker _sdnFaker = new();

  public EfRepositoryTests()
  {
    var loggerFactory = LoggerFactory.Create(builder => builder.ClearProviders());

    _context = new AppDbContext(new DbOptions { DatabaseName = $"{Guid.NewGuid()}.db" });
    _sdnRepository = new EfRepository<Sdn>(_context, loggerFactory.CreateLogger<EfRepository<Sdn>>());
  }

  [Fact]
  public async Task Upsert_WithNewEntity_ShouldAddEntityToDatabase()
  {
    var sdn = _sdnFaker.Generate();

    await _sdnRepository.Upsert(sdn);
    await _context.SaveChangesAsync();

    var result = await _context.Sdns.FindAsync(sdn.Id);

    result.Should().BeEquivalentTo(sdn);
    result!.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    result.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
  }

  [Fact]
  public async Task Upsert_WithExistingEntity_ShouldUpdateEntityInDatabase()
  {
    var sdn = _sdnFaker.Generate();

    await _sdnRepository.Upsert(sdn);
    await _context.SaveChangesAsync();

    var createdSdn = await _context.Sdns.FindAsync(sdn.Id);

    createdSdn.Should().BeEquivalentTo(sdn);
    createdSdn!.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    createdSdn.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));

    var updatedSdn = _sdnFaker.Generate();
    updatedSdn.Id = sdn.Id;

    await _sdnRepository.Upsert(updatedSdn);
    await _context.SaveChangesAsync();

    var result = await _context.Sdns.FindAsync(sdn.Id);

    result.Should().BeEquivalentTo(updatedSdn);
    result!.CreatedAt.Should().BeSameDateAs(createdSdn.CreatedAt);
    result.UpdatedAt.Should().BeAfter(createdSdn.UpdatedAt);
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