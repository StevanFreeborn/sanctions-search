namespace SanctionsSearch.Worker.Services;

class OfacFileService(
  HttpClient client,
  ILogger<OfacFileService> logger,
  OfacFileServiceOptions options
) : IOfacFileService
{
  private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));
  private readonly ILogger<OfacFileService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  private readonly OfacFileServiceOptions _options = options ?? throw new ArgumentNullException(nameof(options));
  private async Task<Result<Stream>> GetFileAsync(Uri fileUri)
  {
    try
    {
      _logger.LogInformation("Downloading file from {FileUri}", fileUri);

      var response = await _client.GetAsync(fileUri);

      if (response.IsSuccessStatusCode is false)
      {
        _logger.LogError("Failed to download file from {FileUri} with Status Code: {StatusCode}", fileUri, response.StatusCode);
        return Result.Fail($"Failed to download file from {fileUri}");
      }

      _logger.LogInformation("Downloaded file from {FileUri}", fileUri);
      var stream = await response.Content.ReadAsStreamAsync();
      return Result.Ok(stream);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to download file from {FileUri}", fileUri);
      return Result.Fail($"Failed to download file from {fileUri}");
    }
  }

  public Task<Result<Stream>> GetSdnFileAsync() => GetFileAsync(_options.GetSdnFileUri());
  public Task<Result<Stream>> GetAddressFileAsync() => GetFileAsync(_options.GetAddressFileUri());
  public Task<Result<Stream>> GetAltNamesFileAsync() => GetFileAsync(_options.GetAltNamesFileUri());
  public Task<Result<Stream>> GetCommentsFileAsync() => GetFileAsync(_options.GetCommentsFileUri());
  public Task<Result<Stream>> GetConPrimaryNameFileAsync() => GetFileAsync(_options.GetConPrimaryNameFileUri());
  public Task<Result<Stream>> GetConAddressesFileAsync() => GetFileAsync(_options.GetConAddressesFileUri());
  public Task<Result<Stream>> GetConAltNamesFileAsync() => GetFileAsync(_options.GetConAltNamesFileUri());
  public Task<Result<Stream>> GetConCommentsFileAsync() => GetFileAsync(_options.GetConCommentsFileUri());
}