namespace SanctionsSearch.Worker.Tests.Unit;

public class EfUnitOfWorkTests
{
  private readonly Mock<DbContext> _contextMock = new();
  private readonly Mock<ILogger<EfUnitOfWork>> _loggerMock = new();
  private readonly Mock<ILoggerFactory> _loggerFactoryMock = new();
  private readonly EfUnitOfWork _unitOfWork;

  public EfUnitOfWorkTests()
  {
    _unitOfWork = new(_contextMock.Object, _loggerMock.Object, _loggerFactoryMock.Object);
  }

  [Fact]
  public void Sdns_WhenCalled_ShouldReturnSdnRepository()
  {
    _unitOfWork.Sdns.Should().BeOfType<SdnRepository>();
  }

  [Fact]
  public void Addresses_WhenCalled_ShouldReturnAddressRepository()
  {
    _unitOfWork.Addresses.Should().BeOfType<AddressRepository>();
  }

  [Fact]
  public void Aliases_WhenCalled_ShouldReturnAliasRepository()
  {
    _unitOfWork.Aliases.Should().BeOfType<AliasRepository>();
  }

  [Fact]
  public void Comments_WhenCalled_ShouldReturnCommentRepository()
  {
    _unitOfWork.Comments.Should().BeOfType<CommentRepository>();
  }

  [Fact]
  public void SaveChangesAsync_WhenADatabaseUpdateExceptionIsThrown_ItShouldBeCaughtAndLogged()
  {
    _contextMock
      .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
      .ThrowsAsync(new DbUpdateException());

    var action = _unitOfWork.SaveChangesAsync;

    action.Should().NotThrowAsync();
  }

  [Fact]
  public void SaveChangesAsync_WhenExceptionIsThrown_ItShouldNotBeCaught()
  {
    _contextMock
      .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
      .ThrowsAsync(new Exception());

    var action = _unitOfWork.SaveChangesAsync;

    action.Should().ThrowAsync<Exception>();
  }

  [Fact]
  public void DisposeAsync_WhenExceptionIsThrown_ItShouldBeCaught()
  {
    _contextMock
      .Setup(x => x.DisposeAsync())
      .Throws(new Exception());

    var action = async () => await _unitOfWork.DisposeAsync();

    action.Should().NotThrowAsync();
  }

  [Fact]
  public void Dispose_WhenExceptionIsThrown_ItShouldBeCaught()
  {
    _contextMock
      .Setup(x => x.Dispose())
      .Throws(new Exception());

    var action = _unitOfWork.Dispose;

    action.Should().NotThrow();
  }
}