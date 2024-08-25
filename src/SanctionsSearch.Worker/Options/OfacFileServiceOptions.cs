namespace SanctionsSearch.Worker.Options;

class OfacFileServiceOptions
{
  public string Url { get; init; } = "https://sanctionslistservice.ofac.treas.gov/api/PublicationPreview/exports";
  public string SdnFileName { get; init; } = "SDN.CSV";
  public string AddressFileName { get; init; } = "ADD.CSV";
  public string AltNamesFileName { get; init; } = "ALT.CSV";
  public string CommentsFileName { get; init; } = "SDN_COMMENTS.CSV";
  public string ConPrimaryNameFileName { get; init; } = "CONS_PRIM.CSV";
  public string ConAddressesFileName { get; init; } = "CONS_ADD.CSV";
  public string ConAltNamesFileName { get; init; } = "CONS_ALT.CSV";
  public string ConCommentsFileName { get; init; } = "CONS_COMMENTS.CSV";

  public Uri GetSdnFileUri() => new($"{Url}/{SdnFileName}");
  public Uri GetAddressFileUri() => new($"{Url}/{AddressFileName}");
  public Uri GetAltNamesFileUri() => new($"{Url}/{AltNamesFileName}");
  public Uri GetCommentsFileUri() => new($"{Url}/{CommentsFileName}");
  public Uri GetConPrimaryNameFileUri() => new($"{Url}/{ConPrimaryNameFileName}");
  public Uri GetConAddressesFileUri() => new($"{Url}/{ConAddressesFileName}");
  public Uri GetConAltNamesFileUri() => new($"{Url}/{ConAltNamesFileName}");
  public Uri GetConCommentsFileUri() => new($"{Url}/{ConCommentsFileName}");
}