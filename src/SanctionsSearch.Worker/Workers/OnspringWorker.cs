
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

  private Task RunSearchRequests()
  {
    _logger.LogInformation("Running search requests");

    return Task.CompletedTask;
  }
}