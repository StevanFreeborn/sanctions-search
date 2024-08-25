using FluentResults;

namespace SanctionsSearch.Worker.Tests.Unit;

public class DatabaseMaintainerTests : IDisposable
{
  private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
  private readonly Mock<IOfacFileService> _ofacFileServiceMock = new();
  private readonly Mock<ILogger<DatabaseMaintainer>> _loggerMock = new();
  private readonly DatabaseMaintainer _databaseMaintainer;

  public DatabaseMaintainerTests()
  {
    _databaseMaintainer = new(_unitOfWorkMock.Object, _ofacFileServiceMock.Object, _loggerMock.Object);
  }

  [Fact]
  public async Task BuildSdnTableAsync_WhenUnableToRetrieveCsvFile_ItShouldLogError()
  {
    _ofacFileServiceMock
      .Setup(x => x.GetSdnFileAsync())
      .ReturnsAsync(Result.Fail<Stream>("Error"));

    await _databaseMaintainer.BuildSdnTableAsync();

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
  public async Task BuildAddressTableAsync_WhenUnableToRetrieveCsvFile_ItShouldLogError()
  {
    _ofacFileServiceMock
      .Setup(x => x.GetAddressFileAsync())
      .ReturnsAsync(Result.Fail<Stream>("Error"));

    await _databaseMaintainer.BuildAddressTableAsync();

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
  public async Task BuildAliasTableAsync_WhenUnableToRetrieveCsvFile_ItShouldLogError()
  {
    _ofacFileServiceMock
      .Setup(x => x.GetAltNamesFileAsync())
      .ReturnsAsync(Result.Fail<Stream>("Error"));

    await _databaseMaintainer.BuiltAliasTableAsync();

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
  public async Task BuildCommentsTableAsync_WhenUnableToRetrieveCsvFile_ItShouldLogError()
  {
    _ofacFileServiceMock
      .Setup(x => x.GetCommentsFileAsync())
      .ReturnsAsync(Result.Fail<Stream>("Error"));

    await _databaseMaintainer.BuildCommentTableAsync();

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

  public void Dispose()
  {
    _databaseMaintainer.Dispose();

    GC.SuppressFinalize(this);
  }
}