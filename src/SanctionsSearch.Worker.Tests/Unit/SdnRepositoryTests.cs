namespace SanctionsSearch.Worker.Tests.Unit;

public class SdnRepositoryTests
{
  private readonly Mock<DbContext> _contextMock = new();
  private readonly Mock<ILogger<SdnRepository>> _loggerMock = new();
  private readonly SdnFaker _faker = new();
  private readonly SdnRepository _repository;

  public SdnRepositoryTests()
  {
    _repository = new SdnRepository(_contextMock.Object, _loggerMock.Object);
  }

  [Fact]
  public async Task Upsert_WhenExceptionIsThrown_ItShouldLogError()
  {
    var entity = _faker.Generate();
    var mockSet = new Mock<DbSet<Sdn>>();

    mockSet
      .Setup(x => x.FindAsync(entity.Id))
      .Throws<Exception>();

    _contextMock
      .Setup(x => x.Set<Sdn>())
      .Returns(mockSet.Object);

    await _repository.Upsert(entity);

    _loggerMock.Verify(
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
    _contextMock
      .Setup(x => x.Set<Sdn>())
      .Throws<Exception>();

    var result = await _repository.Find(x => x.Id == 1);

    result.Should().BeEmpty();

    _loggerMock.Verify(
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