namespace SanctionsSearch.Worker.Options;

class OnspringOptions
{
  public string BaseUrl { get; init; } = "https://api.onspring.com";
  public string ApiKey { get; init; } = string.Empty;
  public int SearchIntervalInMinutes { get; init; } = 1;
  public SearchRequestOptions SearchRequestOptions { get; init; } = new SearchRequestOptions();
  public SearchResultOptions SearchResultOptions { get; init; } = new SearchResultOptions();
}

class SearchRequestOptions
{
  public int AppId { get; init; }
  public int NameFieldId { get; init; }
  public int StatusFieldId { get; init; }
  public Guid AwaitingProcessingStatusId { get; init; }
  public Guid ProcessingStatusId { get; init; }
  public Guid ProcessedSuccessStatusId { get; init; }
  public Guid ProcessedErrorStatusId { get; init; }
  public int ErrorFieldId { get; init; }
}

class SearchResultOptions
{
  public int AppId { get; init; }
  public int SearchRequestFieldId { get; init; }
  public int NameFieldId { get; init; }
  public int AddressFieldId { get; init; }
  public int TypeFieldId { get; init; }
  public int ProgramsFieldId { get; init; }
}