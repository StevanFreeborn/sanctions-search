namespace SanctionsSearch.Worker.Options;

class OnspringOptions
{
  public string BaseUrl { get; set; } = "https://api.onspring.com";
  public string ApiKey { get; set; } = string.Empty;
  public int SearchIntervalInMinutes { get; set; } = 1;
  public SearchRequestOptions SearchRequestOptions { get; set; } = new SearchRequestOptions();
  public SearchResultOptions SearchResultOptions { get; set; } = new SearchResultOptions();
}

class SearchRequestOptions
{
  public int AppId { get; set; }
  public int NameFieldId { get; set; }
  public int AddressFieldId { get; set; }
  public int CityFieldId { get; set; }
  public int StateFieldId { get; set; }
  public int ZipFieldId { get; set; }
  public int CountryFieldId { get; set; }
  public int StatusFieldId { get; set; }
  public Guid AwaitingProcessingStatusId { get; set; }
  public Guid ProcessingStatusId { get; set; }
  public Guid ProcessedSuccessStatusId { get; set; }
  public Guid ProcessedErrorStatusId { get; set; }
  public int ErrorFieldId { get; set; }
}

class SearchResultOptions
{
  public int AppId { get; set; }
  public int SearchRequestFieldId { get; set; }
  public int NameFieldId { get; set; }
  public int AddressFieldId { get; set; }
  public int TypeFieldId { get; set; }
  public int ProgramsFieldId { get; set; }
}