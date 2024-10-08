namespace SanctionsSearch.Worker.Workers;

public class DatabaseWorker(
  ILogger<DatabaseWorker> logger,
  TimeProvider timeProvider,
  IServiceScopeFactory serviceScopeFactory
) : IHostedService, IDisposable
{
  private readonly TimeProvider _timeProvider = timeProvider;
  private readonly ILogger<DatabaseWorker> _logger = logger;
  private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
  private ITimer? _timer;

  public async Task StartAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Database worker started");

    await UpdateDatabase();

    using var scope = _serviceScopeFactory.CreateAsyncScope();
    var dbOptions = scope.ServiceProvider.GetRequiredService<DbOptions>();

    _timer = _timeProvider.CreateTimer(
      callback: async _ => await UpdateDatabase(),
      state: null,
      dueTime: TimeSpan.FromHours(dbOptions.RefreshIntervalInHours),
      period: TimeSpan.FromHours(dbOptions.RefreshIntervalInHours)
    );
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    _logger.LogInformation("Database worker stopped");

    _timer?.Change(Timeout.InfiniteTimeSpan, TimeSpan.Zero);

    return Task.CompletedTask;
  }

  public void Dispose()
  {
    _timer?.Dispose();

    GC.SuppressFinalize(this);
  }

  private async Task UpdateDatabase()
  {
    var updateIdProperty = LogContext.PushProperty("DatabaseUpdateId", Guid.NewGuid());

    _logger.LogInformation("Updating database");

    try
    {
      using var scope = _serviceScopeFactory.CreateAsyncScope();
      var databaseMaintainer = scope.ServiceProvider.GetRequiredService<IDatabaseMaintainer>();

      await databaseMaintainer.BuildSdnTableAsync();
      await databaseMaintainer.BuildAddressTableAsync();
      await databaseMaintainer.BuiltAliasTableAsync();
      await databaseMaintainer.BuildCommentTableAsync();
      await databaseMaintainer.CleanupTablesAsync();

      _logger.LogInformation("Database updated");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error updating database");
    }
    finally
    {
      updateIdProperty.Dispose();
    }
  }
}