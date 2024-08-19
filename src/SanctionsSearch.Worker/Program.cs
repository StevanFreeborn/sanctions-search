using SanctionsSearch.Worker.Persistence;
using SanctionsSearch.Worker.Setup;

if (EF.IsDesignTime)
{
  Host.CreateDefaultBuilder().Build().Run();
  return;
}

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

  var builder = Host.CreateApplicationBuilder(args);

  builder.Logging.ClearProviders();
  builder.Logging.AddSerilog();

  builder.Services.ConfigureOptions<OfacFileServiceOptionsSetup>();
  builder.Services.ConfigureOptions<DbOptionsSetup>();

  builder.Services.AddDbContext<AppDbContext>();
  builder.Services.AddHostedService<Worker>();

  var host = builder.Build();

  using var scope = host.Services.CreateScope();
  var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
  var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

  if (pendingMigrations.Any())
  {
    Log.Information("Applying pending migrations");
    await context.Database.MigrateAsync();
  }

  host.Run();

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