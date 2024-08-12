namespace SanctionsSearch.Worker.Services;

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
