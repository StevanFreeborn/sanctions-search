namespace SanctionsSearch.Worker.Tests.Unit;

public class AliasRepositoryTests
{
  private readonly Mock<DbContext> _context = new();
  private readonly Mock<ILogger<AliasRepository>> _logger = new();
  private readonly AliasFaker _faker = new();
  private readonly AliasRepository _repository;

  public AliasRepositoryTests()
  {
    _repository = new AliasRepository(_context.Object, _logger.Object);
  }

  [Fact]
  public async Task Upsert_WhenExceptionIsThrown_ItShouldLogError()
  {
    var entity = _faker.Generate();
    var mockSet = new Mock<DbSet<Alias>>();

    mockSet
      .Setup(x => x.FindAsync(entity.Id))
      .Throws<Exception>();

    _context
      .Setup(x => x.Set<Alias>())
      .Returns(mockSet.Object);

    await _repository.Upsert(entity);

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
      .Setup(x => x.Set<Alias>())
      .Throws<Exception>();

    var result = await _repository.Find(x => x.Id == 1);

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