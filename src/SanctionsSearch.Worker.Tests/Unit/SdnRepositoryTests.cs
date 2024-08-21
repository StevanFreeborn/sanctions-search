namespace SanctionsSearch.Worker.Tests.Unit;

public class SdnRepositoryTests
{
  private readonly Mock<DbContext> _context = new();
  private readonly Mock<ILogger<SdnRepository>> _logger = new();
  private readonly SdnFaker _sdnFaker = new();
  private readonly SdnRepository _sdnRepository;

  public SdnRepositoryTests()
  {
    _sdnRepository = new SdnRepository(_context.Object, _logger.Object);
  }

  [Fact]
  public async Task Upsert_WhenExceptionIsThrown_ItShouldLogError()
  {
    var sdn = _sdnFaker.Generate();
    var mockSet = new Mock<DbSet<Sdn>>();

    mockSet
      .Setup(x => x.FindAsync(sdn.Id))
      .Throws<Exception>();

    _context
      .Setup(x => x.Set<Sdn>())
      .Returns(mockSet.Object);

    await _sdnRepository.Upsert(sdn);

    _logger.Verify(
      x => x.Log(
        LogLevel.Error,
        It.IsAny<EventId>(),
        It.IsAny<It.IsAnyType>(),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()
      )
    );
  }

  [Fact]
  public async Task Find_WhenExceptionIsThrown_ItShouldReturnEmptyListAndLogError()
  {
    _context
      .Setup(x => x.Set<Sdn>())
      .Throws<Exception>();

    var result = await _sdnRepository.Find(x => x.Id == 1);

    result.Should().BeEmpty();

    _logger.Verify(
      x => x.Log(
        LogLevel.Error,
        It.IsAny<EventId>(),
        It.IsAny<It.IsAnyType>(),
        It.IsAny<Exception>(),
        It.IsAny<Func<It.IsAnyType, Exception?, string>>()
      )
    );
  }
}