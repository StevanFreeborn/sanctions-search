namespace SanctionsSearch.Worker.Tests.Unit;

public class AddressRepositoryTests
{
  private readonly Mock<DbContext> _context = new();
  private readonly Mock<ILogger<AddressRepository>> _logger = new();
  private readonly AddressFaker _faker = new();
  private readonly AddressRepository _repository;

  public AddressRepositoryTests()
  {
    _repository = new AddressRepository(_context.Object, _logger.Object);
  }

  [Fact]
  public async Task Upsert_WhenExceptionIsThrown_ItShouldLogError()
  {
    var entity = _faker.Generate();
    var mockSet = new Mock<DbSet<Address>>();

    mockSet
      .Setup(x => x.FindAsync(entity.Id))
      .Throws<Exception>();

    _context
      .Setup(x => x.Set<Address>())
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
      .Setup(x => x.Set<Address>())
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