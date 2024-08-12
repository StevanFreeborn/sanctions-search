using Moq;

using RichardSzalay.MockHttp;

namespace SanctionsSearch.Worker.Tests.Unit;

public class OfacFileServiceTests
{
  private readonly MockHttpMessageHandler _mockHttp = new();
  private readonly Mock<ILogger<OfacFileService>> _mockLogger = new();
  private readonly Mock<IOptionsSnapshot<OfacFileServiceOptions>> _mockOptions = new();

  [Fact]
  public void Constructor_WhenCalledWithNullClient_ThrowsArgumentNullException()
  {
    var act = () => new OfacFileService(
      null!,
      _mockLogger.Object,
      _mockOptions.Object
    );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_WhenCalledWithNullLogger_ThrowsArgumentNullException()
  {
    var act = () => new OfacFileService(
      _mockHttp.ToHttpClient(),
      null!,
      _mockOptions.Object
    );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_WhenCalledWithNullOptions_ThrowsArgumentNullException()
  {
    var act = () => new OfacFileService(
      _mockHttp.ToHttpClient(),
      _mockLogger.Object,
      null!
    );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_WhenCalledWithNullOptionsValue_ThrowsArgumentNullException()
  {
    _mockOptions.Setup(x => x.Value).Returns(() => null!);

    var act = () => new OfacFileService(
      _mockHttp.ToHttpClient(),
      _mockLogger.Object,
      _mockOptions.Object
    );

    act.Should().Throw<ArgumentNullException>();
  }
}