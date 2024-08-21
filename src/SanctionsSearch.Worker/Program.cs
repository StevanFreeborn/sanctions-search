namespace SanctionsSearch.Worker;

class Program
{
  async static Task Main(string[] args)
  {
    if (EF.IsDesignTime) return;

    Log.Logger = new LoggerConfiguration()
      .Enrich.WithProperty("Application", "SanctionsSearch.Worker")
      .Enrich.WithEnvironmentName()
      .Enrich.WithMachineName()
      .Enrich.WithProcessId()
      .Enrich.WithThreadId()
      .Enrich.WithExceptionDetails()
      .Enrich.FromLogContext()
      .MinimumLevel.Debug()
      .WriteTo.Console()
      .WriteTo.File(new CompactJsonFormatter(), "logs/log.json", rollingInterval: RollingInterval.Day)
      .CreateLogger();

    try
    {
      Log.Information("Starting Sanctions Search worker");

      var builder = CreateHostBuilder(args);

      var host = builder.Build();

      using var scope = host.Services.CreateScope();
      var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
      var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

      if (pendingMigrations.Any())
      {
        Log.Information("Applying pending migrations");
        await context.Database.MigrateAsync();
      }

      await host.RunAsync();

      Log.Information("Stopping Sanctions Search worker");
    }
    catch (Exception ex)
    {
      Log.Fatal(ex, "Worker terminated unexpectedly");
    }
    finally
    {
      await Log.CloseAndFlushAsync();
    }
  }

  static HostApplicationBuilder CreateHostBuilder(string[] args)
  {
    var builder = Host.CreateApplicationBuilder(args);

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog();

    builder.Services.ConfigureOptions<OfacFileServiceOptionsSetup>();
    builder.Services.ConfigureOptions<DbOptionsSetup>();
    builder.Services.AddScoped(rs => rs.GetRequiredService<IOptionsSnapshot<DbOptions>>().Value);

    builder.Services.AddSingleton(TimeProvider.System);
    builder.Services.AddDbContext<AppDbContext>();
    builder.Services.AddHostedService<Worker>();

    return builder;
  }
}