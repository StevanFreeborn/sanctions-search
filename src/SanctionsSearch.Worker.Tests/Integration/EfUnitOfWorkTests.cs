namespace SanctionsSearch.Worker.Tests.Integration;

public class EfUnitOfWorkTests : DatabaseTest
{
  private readonly EfUnitOfWork _uow;
  private readonly SdnFaker _sdnFaker = new();
  private readonly AddressFaker _addressFaker = new();
  private readonly AliasFaker _aliasFaker = new();
  private readonly CommentFaker _commentFaker = new();

  public EfUnitOfWorkTests()
  {
    var loggerFactory = LoggerFactory.Create(c => c.ClearProviders());
    _uow = new(_context, loggerFactory);
  }

  [Fact]
  public async Task SaveChangesAsync_WhenCalled_ItShouldPersistChangesToDatabase()
  {
    var now = DateTimeOffset.UtcNow;

    _timeProviderMock
      .Setup(m => m.GetUtcNow())
      .Returns(now);

    var sdn = _sdnFaker.Generate();

    var address = _addressFaker.Generate();
    address.SdnId = sdn.Id;

    var alias = _aliasFaker.Generate();
    alias.SdnId = sdn.Id;

    var comment = _commentFaker.Generate();
    comment.SdnId = sdn.Id;

    await _uow.Sdns.Upsert(sdn);
    await _uow.Addresses.Upsert(address);
    await _uow.Aliases.Upsert(alias);
    await _uow.Comments.Upsert(comment);
    await _uow.SaveChangesAsync();

    var savedSdn = await _context.Set<Sdn>()
      .Include(s => s.Addresses)
      .Include(s => s.Aliases)
      .Include(s => s.Comments)
      .FirstOrDefaultAsync();

    savedSdn.Should().NotBeNull();
    savedSdn.Should().BeEquivalentTo(
      sdn,
      opts => opts
        .Excluding(s => s.Addresses)
        .Excluding(s => s.Aliases)
        .Excluding(s => s.Comments)
    );

    savedSdn!.Addresses.Should().HaveCount(1);
    savedSdn.Aliases.Should().HaveCount(1);
    savedSdn.Comments.Should().HaveCount(1);

    var savedAddress = savedSdn.Addresses.First(a => a.Id == address.Id);
    var savedAlias = savedSdn.Aliases.First(a => a.Id == alias.Id);
    var savedComment = savedSdn.Comments.First(a => a.Id == comment.Id);

    savedAddress.Should().BeEquivalentTo(address);
    savedAlias.Should().BeEquivalentTo(alias);
    savedComment.Should().BeEquivalentTo(comment);
  }

  [Fact]
  public async Task DisposeAsync_WhenCalled_ItShouldDisposeContext()
  {
    await _uow.DisposeAsync();

    var action = () => _context.SaveChangesAsync();

    await action.Should().ThrowAsync<ObjectDisposedException>();
  }

  [Fact]
  public void Dispose_WhenCalled_ItShouldDisposeContext()
  {
    _uow.Dispose();

    var action = () => _context.SaveChangesAsync();

    action.Should().ThrowAsync<ObjectDisposedException>();
  }
}