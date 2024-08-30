namespace SanctionsSearch.Worker.Services;

class OnspringService(
  HttpClient httpClient,
  OnspringOptions options,
  ILogger<OnspringService> logger
) : IOnspringService
{
  private readonly IOnspringClient _client = new OnspringClient(options.ApiKey, httpClient);
  private readonly ILogger<OnspringService> _logger = logger;

  public async Task<List<SearchRequest>> GetSearchRequestsAsync()
  {
    var queryRequest = new QueryRecordsRequest()
    {
      AppId = options.SearchRequestOptions.AppId,
      FieldIds = [
        options.SearchRequestOptions.NameFieldId,
        options.SearchRequestOptions.AddressFieldId,
        options.SearchRequestOptions.CityFieldId,
        options.SearchRequestOptions.StateFieldId,
        options.SearchRequestOptions.ZipFieldId,
        options.SearchRequestOptions.CountryFieldId
      ],
    };

    var records = new ConcurrentBag<ResultRecord>();

    var initialResponse = await _client.QueryRecordsAsync(queryRequest);

    if (initialResponse.IsSuccessful is false)
    {
      _logger.LogError(
        "Failed to retrieve search requests: {StatusCode} - {Error}",
        initialResponse.StatusCode,
        initialResponse.Message
      );
      return [];
    }

    initialResponse.Value.Items.ForEach(records.Add);

    if (initialResponse.Value.HasMorePages())
    {
      // TODO: Fan out and collect remaining pages
    }

    return records.Select(MapRecordToSearchRequest).ToList();
  }

  private SearchRequest MapRecordToSearchRequest(ResultRecord record)
  {
    var searchRequest = new SearchRequest();

    foreach (var field in record.FieldData)
    {
      if (field.FieldId == options.SearchRequestOptions.NameFieldId)
      {
        searchRequest.Name = field.GetStringValue();
      }
      else if (field.FieldId == options.SearchRequestOptions.AddressFieldId)
      {
        searchRequest.Address = field.GetStringValue();
      }
      else if (field.FieldId == options.SearchRequestOptions.CityFieldId)
      {
        searchRequest.City = field.GetStringValue();
      }
      else if (field.FieldId == options.SearchRequestOptions.StateFieldId)
      {
        searchRequest.State = field.GetStringValue();
      }
      else if (field.FieldId == options.SearchRequestOptions.ZipFieldId)
      {
        searchRequest.Zip = field.GetStringValue();
      }
      else if (field.FieldId == options.SearchRequestOptions.CountryFieldId)
      {
        searchRequest.Country = field.GetStringValue();
      }
    }

    return searchRequest;
  }
}