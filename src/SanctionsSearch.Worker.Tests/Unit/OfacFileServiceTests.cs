namespace SanctionsSearch.Worker.Tests.Unit;

public class OfacFileServiceTests : IDisposable
{
  private const string TestCsv = "Name,Address\nJohn Doe,123 Main St\n";
  private static Stream TestStream => new MemoryStream(Encoding.UTF8.GetBytes(TestCsv));
  private readonly MockHttpMessageHandler _mockHttp = new();
  private readonly Mock<ILogger<OfacFileService>> _mockLogger = new();
  private readonly OfacFileServiceOptionsFaker _optionsFaker = new();
  private readonly OfacFileServiceOptions _options;
  private readonly OfacFileService _service;

  public OfacFileServiceTests()
  {
    _options = _optionsFaker.Generate();

    _service = new OfacFileService(
      _mockHttp.ToHttpClient(),
      _mockLogger.Object,
      _options
    );
  }

  private static string GetStreamContent(Stream stream)
  {
    using var reader = new StreamReader(stream);
    return reader.ReadToEnd();
  }

  [Fact]
  public void Constructor_WhenCalledWithNullClient_ThrowsArgumentNullException()
  {
    var act = () => new OfacFileService(
      null!,
      _mockLogger.Object,
      _options
    );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Constructor_WhenCalledWithNullLogger_ThrowsArgumentNullException()
  {
    var act = () => new OfacFileService(
      _mockHttp.ToHttpClient(),
      null!,
      _options
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
    var act = () => new OfacFileService(
      _mockHttp.ToHttpClient(),
      _mockLogger.Object,
      null!
    );

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public async Task GetSdnFileAsync_WhenCalled_ItShouldReturnStream()
  {
    _mockHttp
      .When(_options.GetSdnFileUri().ToString())
      .Respond("text/csv", TestStream);

    var result = await _service.GetSdnFileAsync();

    result.IsSuccess.Should().BeTrue();
    GetStreamContent(result.Value).Should().Be(TestCsv);
  }

  [Fact]
  public async Task GetSdnFileAsync_WhenRequestFails_ItShouldReturnFailure()
  {
    _mockHttp
      .When(_options.GetSdnFileUri().ToString())
      .Respond(HttpStatusCode.InternalServerError);

    var result = await _service.GetSdnFileAsync();

    result.IsFailed.Should().BeTrue();
  }

  [Fact]
  public async Task GetSdnFileAsync_WhenExceptionThrown_ItShouldReturnFailure()
  {
    _mockHttp
      .When(_options.GetSdnFileUri().ToString())
      .Throw(new Exception("Test exception"));

    var result = await _service.GetSdnFileAsync();

    result.IsFailed.Should().BeTrue();
  }

  [Fact]
  public async Task GetAddressFileAsync_WhenCalled_ItShouldReturnStream()
  {
    _mockHttp
      .When(_options.GetAddressFileUri().ToString())
      .Respond("text/csv", TestStream);

    var result = await _service.GetAddressFileAsync();

    result.IsSuccess.Should().BeTrue();
    GetStreamContent(result.Value).Should().Be(TestCsv);
  }

  [Fact]
  public async Task GetAddressFileAsync_WhenRequestFails_ItShouldReturnFailure()
  {
    _mockHttp
      .When(_options.GetAddressFileUri().ToString())
      .Respond(HttpStatusCode.InternalServerError);

    var result = await _service.GetAddressFileAsync();

    result.IsFailed.Should().BeTrue();
  }

  [Fact]
  public async Task GetAltNamesFileAsync_WhenCalled_ItShouldReturnStream()
  {
    _mockHttp
      .When(_options.GetAltNamesFileUri().ToString())
      .Respond("text/csv", TestStream);

    var result = await _service.GetAltNamesFileAsync();

    result.IsSuccess.Should().BeTrue();
    GetStreamContent(result.Value).Should().Be(TestCsv);
  }

  [Fact]
  public async Task GetAltNamesFileAsync_WhenRequestFails_ItShouldReturnFailure()
  {
    _mockHttp
      .When(_options.GetAltNamesFileUri().ToString())
      .Respond(HttpStatusCode.InternalServerError);

    var result = await _service.GetAltNamesFileAsync();

    result.IsFailed.Should().BeTrue();
  }

  [Fact]
  public async Task GetCommentsFileAsync_WhenCalled_ItShouldReturnStream()
  {
    _mockHttp
      .When(_options.GetCommentsFileUri().ToString())
      .Respond("text/csv", TestStream);

    var result = await _service.GetCommentsFileAsync();

    result.IsSuccess.Should().BeTrue();
    GetStreamContent(result.Value).Should().Be(TestCsv);
  }

  [Fact]
  public async Task GetCommentsFileAsync_WhenRequestFails_ItShouldReturnFailure()
  {
    _mockHttp
      .When(_options.GetCommentsFileUri().ToString())
      .Respond(HttpStatusCode.InternalServerError);

    var result = await _service.GetCommentsFileAsync();

    result.IsFailed.Should().BeTrue();
  }

  [Fact]
  public async Task GetConPrimaryNameFileAsync_WhenCalled_ItShouldReturnStream()
  {
    _mockHttp
      .When(_options.GetConPrimaryNameFileUri().ToString())
      .Respond("text/csv", TestStream);

    var result = await _service.GetConPrimaryNameFileAsync();

    result.IsSuccess.Should().BeTrue();
    GetStreamContent(result.Value).Should().Be(TestCsv);
  }

  [Fact]
  public async Task GetConPrimaryNameFileAsync_WhenRequestFails_ItShouldReturnFailure()
  {
    _mockHttp
      .When(_options.GetConPrimaryNameFileUri().ToString())
      .Respond(HttpStatusCode.InternalServerError);

    var result = await _service.GetConPrimaryNameFileAsync();

    result.IsFailed.Should().BeTrue();
  }

  [Fact]
  public async Task GetConAddressesFileAsync_WhenCalled_ItShouldReturnStream()
  {
    _mockHttp
      .When(_options.GetConAddressesFileUri().ToString())
      .Respond("text/csv", TestStream);

    var result = await _service.GetConAddressesFileAsync();

    result.IsSuccess.Should().BeTrue();
    GetStreamContent(result.Value).Should().Be(TestCsv);
  }

  [Fact]
  public async Task GetConAddressesFileAsync_WhenRequestFails_ItShouldReturnFailure()
  {
    _mockHttp
      .When(_options.GetConAddressesFileUri().ToString())
      .Respond(HttpStatusCode.InternalServerError);

    var result = await _service.GetConAddressesFileAsync();

    result.IsFailed.Should().BeTrue();
  }

  [Fact]
  public async Task GetConAltNamesFileAsync_WhenCalled_ItShouldReturnStream()
  {
    _mockHttp
      .When(_options.GetConAltNamesFileUri().ToString())
      .Respond("text/csv", TestStream);

    var result = await _service.GetConAltNamesFileAsync();

    result.IsSuccess.Should().BeTrue();
    GetStreamContent(result.Value).Should().Be(TestCsv);
  }

  [Fact]
  public async Task GetConAltNamesFileAsync_WhenRequestFails_ItShouldReturnFailure()
  {
    _mockHttp
      .When(_options.GetConAltNamesFileUri().ToString())
      .Respond(HttpStatusCode.InternalServerError);

    var result = await _service.GetConAltNamesFileAsync();

    result.IsFailed.Should().BeTrue();
  }

  [Fact]
  public async Task GetConCommentsFileAsync_WhenCalled_ItShouldReturnStream()
  {
    _mockHttp
      .When(_options.GetConCommentsFileUri().ToString())
      .Respond("text/csv", TestStream);

    var result = await _service.GetConCommentsFileAsync();

    result.IsSuccess.Should().BeTrue();
    GetStreamContent(result.Value).Should().Be(TestCsv);
  }

  [Fact]
  public async Task GetConCommentsFileAsync_WhenRequestFails_ItShouldReturnFailure()
  {
    _mockHttp
      .When(_options.GetConCommentsFileUri().ToString())
      .Respond(HttpStatusCode.InternalServerError);

    var result = await _service.GetConCommentsFileAsync();

    result.IsFailed.Should().BeTrue();
  }

  public void Dispose()
  {
    _mockHttp.Dispose();

    GC.SuppressFinalize(this);
  }
}