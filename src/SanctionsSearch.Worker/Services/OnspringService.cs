
namespace SanctionsSearch.Worker.Services;

class OnspringService(
  IOnspringClient client,
  OnspringOptions options,
  ILogger<OnspringService> logger
) : IOnspringService
{
  private readonly OnspringOptions _options = options;
  private readonly ILogger<OnspringService> _logger = logger;
  private readonly IOnspringClient _client = client;

  public Task<Result> AddSearchResultAsync(SearchResult result)
  {
    // TODO: Implement this method
    // - we should add a new record for each hit
    // - each new hit record should reference the request
    // - we should update the request status to processed
    // - we should return a Result indicating success or failure
    throw new NotImplementedException();
  }

  public async Task<List<SearchRequest>> GetSearchRequestsAsync()
  {
    var queryRequest = new QueryRecordsRequest()
    {
      AppId = _options.SearchRequestOptions.AppId,
      Filter = $"{_options.SearchRequestOptions.StatusFieldId} contains '{_options.SearchRequestOptions.AwaitingProcessingStatusId}'",
      FieldIds = [
        _options.SearchRequestOptions.NameFieldId,
        _options.SearchRequestOptions.AddressFieldId,
        _options.SearchRequestOptions.CityFieldId,
        _options.SearchRequestOptions.StateFieldId,
        _options.SearchRequestOptions.ZipFieldId,
        _options.SearchRequestOptions.CountryFieldId
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
      var remainingPageNumbers = Enumerable.Range(initialResponse.Value.PageNumber + 1, initialResponse.Value.TotalPages - 1);
      var pagingRequests = remainingPageNumbers.Select(num => new PagingRequest { PageNumber = num });
      var remainingRequests = pagingRequests.Select(async pageRequest =>
      {
        try
        {
          var res = await _client.QueryRecordsAsync(queryRequest, pageRequest);

          if (res.IsSuccessful is false)
          {
            _logger.LogError(
              "Failed to retrieve search requests for page {PageNumber}: {StatusCode} - {Error}",
              pageRequest.PageNumber,
              res.StatusCode,
              res.Message
            );

            return;
          }

          res.Value.Items.ForEach(records.Add);
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Failed to retrieve search requests for page {PageNumber}", pageRequest.PageNumber);
        }
      });

      await Task.WhenAll(remainingRequests);
    }

    return records.Select(MapRecordToSearchRequest).ToList();
  }

  private SearchRequest MapRecordToSearchRequest(ResultRecord record)
  {
    var searchRequest = new SearchRequest
    {
      Id = record.RecordId
    };

    foreach (var field in record.FieldData)
    {
      if (field.FieldId == _options.SearchRequestOptions.NameFieldId)
      {
        searchRequest.Name = field.GetStringValue();
      }
      else if (field.FieldId == _options.SearchRequestOptions.AddressFieldId)
      {
        searchRequest.Address = field.GetStringValue();
      }
      else if (field.FieldId == _options.SearchRequestOptions.CityFieldId)
      {
        searchRequest.City = field.GetStringValue();
      }
      else if (field.FieldId == _options.SearchRequestOptions.StateFieldId)
      {
        searchRequest.State = field.GetStringValue();
      }
      else if (field.FieldId == _options.SearchRequestOptions.ZipFieldId)
      {
        searchRequest.Zip = field.GetStringValue();
      }
      else if (field.FieldId == _options.SearchRequestOptions.CountryFieldId)
      {
        searchRequest.Country = field.GetStringValue();
      }
    }

    return searchRequest;
  }
}