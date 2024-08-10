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
  builder.Services.AddHostedService<Worker>();

  var host = builder.Build();
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
