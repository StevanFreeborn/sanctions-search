namespace SanctionsSearch.Worker.Services;

interface IOfacFileService
{
}

class OfacFileService(
  HttpClient client,
  ILogger<OfacFileService> logger,
  IOptionsSnapshot<OfacFileServiceOptions> options
) : IOfacFileService
{
  private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));
  private readonly ILogger<OfacFileService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  private readonly OfacFileServiceOptions _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
}

class OfacFileServiceOptions
{
  public string Url { get; set; } = string.Empty;
  public string SdnFileName { get; set; } = string.Empty;
  public string AddressFileName { get; set; } = string.Empty;
  public string AltNamesFileName { get; set; } = string.Empty;
  public string CommentsFileName { get; set; } = string.Empty;
  public string ConPrimaryNameFileName { get; set; } = string.Empty;
  public string ConAddressesFileName { get; set; } = string.Empty;
  public string ConAltNamesFileName { get; set; } = string.Empty;
  public string ConCommentsFileName { get; set; } = string.Empty;
}