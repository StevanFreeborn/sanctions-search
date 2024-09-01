
namespace SanctionsSearch.Worker.Workers;

public class OnspringWorker(
  ILogger<OnspringWorker> logger,
  TimeProvider timeProvider,
  IServiceScopeFactory serviceScopeFactory
) : IHostedService, IDisposable
{
  private readonly ILogger<OnspringWorker> _logger = logger;
  private readonly TimeProvider _timeProvider = timeProvider;
  private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
  private ITimer? _timer;

  public Task StartAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Onspring worker started");

    _timer = _timeProvider.CreateTimer(
      callback: async _ => await RunSearchRequests(),
      state: null,
      dueTime: TimeSpan.Zero,
      period: TimeSpan.FromMinutes(1)
    );

    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Onspring worker stopped");

    _timer?.Change(Timeout.InfiniteTimeSpan, TimeSpan.Zero);

    return Task.CompletedTask;
  }

  public void Dispose()
  {
    _timer?.Dispose();

    GC.SuppressFinalize(this);
  }

  private async Task RunSearchRequests()
  {
    var searchBatchIdProperty = LogContext.PushProperty("SearchBatchId", Guid.NewGuid());

    _logger.LogInformation("Running search requests");

    try
    {
      using var scope = _serviceScopeFactory.CreateAsyncScope();
      var onspringService = scope.ServiceProvider.GetRequiredService<IOnspringService>();
      var searchService = scope.ServiceProvider.GetRequiredService<ISearchService>();

      var searchRequests = await onspringService.GetSearchRequestsAsync();

      if (searchRequests.Count is 0)
      {
        _logger.LogInformation("No search requests found");
        return;
      }

      foreach (var searchRequest in searchRequests)
      {
        var searchBatchItemIdProperty = LogContext.PushProperty("SearchBatchItemId", Guid.NewGuid());

        try
        {
          _logger.LogInformation("Processing search request {SearchRequestId}", searchRequest.Id);

          await onspringService.UpdateSearchRequestAsProcessingAsync(searchRequest);
          var searchResult = await searchService.PerformSearchAsync(searchRequest);
          await onspringService.AddSearchResultAsync(searchResult);

          _logger.LogInformation("Search request {SearchRequestId} processed", searchRequest.Id);
        }
        catch (Exception ex)
        {
          await onspringService.UpdateSearchRequestAsFailedAsync(
            searchRequest,
            $"Failed to process search request: {ex.Message}"
          );

          _logger.LogError(ex, "Failed to process search request {SearchRequestId}", searchRequest.Id);
        }
        finally
        {
          searchBatchItemIdProperty.Dispose();
        }
      }
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to run search requests");
    }
    finally
    {
      searchBatchIdProperty.Dispose();
    }
  }
}