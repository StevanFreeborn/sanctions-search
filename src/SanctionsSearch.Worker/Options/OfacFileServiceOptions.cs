namespace SanctionsSearch.Worker.Options;

class OfacFileServiceOptions
{
  public string Url { get; init; } = string.Empty;
  public string SdnFileName { get; init; } = string.Empty;
  public string AddressFileName { get; init; } = string.Empty;
  public string AltNamesFileName { get; init; } = string.Empty;
  public string CommentsFileName { get; init; } = string.Empty;
  public string ConPrimaryNameFileName { get; init; } = string.Empty;
  public string ConAddressesFileName { get; init; } = string.Empty;
  public string ConAltNamesFileName { get; init; } = string.Empty;
  public string ConCommentsFileName { get; init; } = string.Empty;

  public Uri GetSdnFileUri() => new($"{Url}/{SdnFileName}");
  public Uri GetAddressFileUri() => new($"{Url}/{AddressFileName}");
  public Uri GetAltNamesFileUri() => new($"{Url}/{AltNamesFileName}");
  public Uri GetCommentsFileUri() => new($"{Url}/{CommentsFileName}");
  public Uri GetConPrimaryNameFileUri() => new($"{Url}/{ConPrimaryNameFileName}");
  public Uri GetConAddressesFileUri() => new($"{Url}/{ConAddressesFileName}");
  public Uri GetConAltNamesFileUri() => new($"{Url}/{ConAltNamesFileName}");
  public Uri GetConCommentsFileUri() => new($"{Url}/{ConCommentsFileName}");
}