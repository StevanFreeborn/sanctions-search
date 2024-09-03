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

  public async Task<Result> AddSearchResultAsync(SearchResult result)
  {
    var typeValues = result.Hits.Select(h => h.Type).ToList();
    var typeFieldPairs = await GetOrAddListValuePairs(
      _options.SearchResultOptions.TypeFieldId,
      typeValues
    );

    var programValues = result.Hits.SelectMany(h => h.Programs).ToList();
    var programFieldPairs = await GetOrAddListValuePairs(
      _options.SearchResultOptions.ProgramsFieldId,
      programValues
    );

    var resultRecords = result.Hits.Select(hit =>
    {
      var resultRecord = new ResultRecord()
      {
        AppId = _options.SearchResultOptions.AppId,
        FieldData = [
          new IntegerFieldValue()
          {
            FieldId = _options.SearchResultOptions.SearchRequestFieldId,
            Value = result.SearchRequestId
          },
          new StringFieldValue()
          {
            FieldId = _options.SearchResultOptions.NameFieldId,
            Value = hit.Name
          },
          new StringFieldValue()
          {
            FieldId = _options.SearchResultOptions.AddressFieldId,
            Value = hit.Address
          },
        ]
      };

      if (string.IsNullOrWhiteSpace(hit.Type) is false)
      {
        resultRecord.FieldData.Add(
          new GuidFieldValue()
          {
            FieldId = _options.SearchResultOptions.TypeFieldId,
            Value = typeFieldPairs[hit.Type]
          }
        );
      }

      var programValues = hit.Programs
        .Where(p => string.IsNullOrWhiteSpace(p) is false)
        .Select(p => programFieldPairs[p])
        .ToList();

      if (programValues.Any())
      {
        resultRecord.FieldData.Add(
          new GuidListFieldValue()
          {
            FieldId = _options.SearchResultOptions.ProgramsFieldId,
            Value = programValues
          }
        );
      }

      return resultRecord;
    });

    var saveResultRequests = resultRecords.Select(_client.SaveRecordAsync);
    var saveResultResponses = await Task.WhenAll(saveResultRequests);

    foreach (var response in saveResultResponses)
    {
      if (response.IsSuccessful is false)
      {
        _logger.LogError(
          "Failed to save search result: {StatusCode} - {Error}",
          response.StatusCode,
          response.Message
        );
      }
    }

    var isFailed = saveResultResponses.Any(r => r.IsSuccessful is false);
    var updatedSearchRequest = isFailed
      ? new ResultRecord()
      {
        AppId = _options.SearchRequestOptions.AppId,
        RecordId = result.SearchRequestId,
        FieldData = [
              new GuidFieldValue()
              {
                FieldId = _options.SearchRequestOptions.StatusFieldId,
                Value = _options.SearchRequestOptions.ProcessedErrorStatusId
              },
          new StringFieldValue()
          {
            FieldId = _options.SearchRequestOptions.ErrorFieldId,
            Value = "Unable to save all search results"
          }
            ]
      }
      : new ResultRecord()
      {
        AppId = _options.SearchRequestOptions.AppId,
        RecordId = result.SearchRequestId,
        FieldData = [
              new GuidFieldValue()
              {
                FieldId = _options.SearchRequestOptions.StatusFieldId,
                Value = _options.SearchRequestOptions.ProcessedSuccessStatusId
              },
          new StringFieldValue()
          {
            FieldId = _options.SearchRequestOptions.ErrorFieldId,
            Value = string.Empty
          }
            ]
      };

    var updateRequestResponse = await _client.SaveRecordAsync(updatedSearchRequest);

    if (updateRequestResponse.IsSuccessful is false)
    {
      _logger.LogError(
        "Failed to update search request {RequestId} status: {StatusCode} - {Error}",
        result.SearchRequestId,
        updateRequestResponse.StatusCode,
        updateRequestResponse.Message
      );
    }

    return isFailed ? Result.Fail("Failed to saving all search results") : Result.Ok();
  }

  public async Task<List<SearchRequest>> GetSearchRequestsAsync()
  {
    var queryRequest = new QueryRecordsRequest()
    {
      AppId = _options.SearchRequestOptions.AppId,
      Filter = $"{_options.SearchRequestOptions.StatusFieldId} contains '{_options.SearchRequestOptions.AwaitingProcessingStatusId}'",
      FieldIds = [
        _options.SearchRequestOptions.NameFieldId,
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

  public async Task<Result> UpdateSearchRequestAsFailedAsync(SearchRequest request, string error)
  {
    var updatedSearchRequest = new ResultRecord()
    {
      AppId = _options.SearchRequestOptions.AppId,
      RecordId = request.Id,
      FieldData = [
        new GuidFieldValue()
        {
          FieldId = _options.SearchRequestOptions.StatusFieldId,
          Value = _options.SearchRequestOptions.ProcessedErrorStatusId
        },
        new StringFieldValue()
        {
          FieldId = _options.SearchRequestOptions.ErrorFieldId,
          Value = error
        }
      ]
    };

    var updateRequestResponse = await _client.SaveRecordAsync(updatedSearchRequest);

    if (updateRequestResponse.IsSuccessful is false)
    {
      _logger.LogError(
        "Failed to update search request {RequestId} status: {StatusCode} - {Error}",
        request.Id,
        updateRequestResponse.StatusCode,
        updateRequestResponse.Message
      );

      return Result.Fail("Failed to update search request status");
    }

    return Result.Ok();
  }

  public async Task<Result> UpdateSearchRequestAsProcessingAsync(SearchRequest request)
  {
    var updatedSearchRequest = new ResultRecord()
    {
      AppId = _options.SearchRequestOptions.AppId,
      RecordId = request.Id,
      FieldData = [
        new GuidFieldValue()
        {
          FieldId = _options.SearchRequestOptions.StatusFieldId,
          Value = _options.SearchRequestOptions.ProcessingStatusId
        }
      ]
    };

    var updateRequestResponse = await _client.SaveRecordAsync(updatedSearchRequest);

    if (updateRequestResponse.IsSuccessful is false)
    {
      _logger.LogError(
        "Failed to update search request {RequestId} status: {StatusCode} - {Error}",
        request.Id,
        updateRequestResponse.StatusCode,
        updateRequestResponse.Message
      );

      return Result.Fail("Failed to update search request status");
    }

    return Result.Ok();
  }

  private async Task<Dictionary<string, Guid>> GetOrAddListValuePairs(int listFieldId, List<string> values)
  {
    var pairs = new Dictionary<string, Guid>();

    var getFieldResponse = await _client.GetFieldAsync(listFieldId);

    if (getFieldResponse.IsSuccessful is false)
    {
      _logger.LogError(
        "Failed to retrieve field information for list field with id {ListFieldId}: {StatusCode} - {Error}",
        listFieldId,
        getFieldResponse.StatusCode,
        getFieldResponse.Message
      );

      return pairs;
    }

    if (getFieldResponse.Value is not ListField listField)
    {
      _logger.LogError("Field with id {ListFieldId} is not a list field", listFieldId);
      return pairs;
    }

    foreach (var value in values.Where(v => string.IsNullOrWhiteSpace(v) is false).Distinct())
    {
      var existingListFieldValue = listField.Values.FirstOrDefault(v => string.Equals(v.Name, value, StringComparison.InvariantCultureIgnoreCase));

      if (existingListFieldValue is not null)
      {
        pairs.Add(value, existingListFieldValue.Id);
        continue;
      }

      var saveListFieldValueRequest = new SaveListItemRequest()
      {
        ListId = listField.ListId,
        Name = value
      };

      var saveListFieldValueResponse = await _client.SaveListItemAsync(saveListFieldValueRequest);

      if (saveListFieldValueResponse.IsSuccessful is false)
      {
        _logger.LogError(
          "Failed to save list field value for list field with id {ListFieldId}: {StatusCode} - {Error}",
          listFieldId,
          saveListFieldValueResponse.StatusCode,
          saveListFieldValueResponse.Message
        );

        continue;
      }

      pairs.Add(value, saveListFieldValueResponse.Value.Id);
    }

    return pairs;
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
    }

    return searchRequest;
  }
}