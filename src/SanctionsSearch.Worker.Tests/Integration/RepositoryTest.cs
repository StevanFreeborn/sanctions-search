namespace SanctionsSearch.Worker.Tests.Integration;

public class RepositoryTest : DatabaseTest, IAsyncLifetime
{
  private readonly SdnFaker _faker = new();
  private readonly Sdn _sdn;
  protected int SdnId => _sdn.Id;

  public RepositoryTest()
  {
    _sdn = _faker.Generate();
    _sdn.Id = SdnFaker.ReservedId;
  }

  public override async Task InitializeAsync()
  {
    await base.InitializeAsync();
    await _context.Set<Sdn>().AddAsync(_sdn);
  }

  public override async Task DisposeAsync()
  {
    await base.DisposeAsync();
  }
}