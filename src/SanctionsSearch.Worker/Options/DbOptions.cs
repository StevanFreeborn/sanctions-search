namespace SanctionsSearch.Worker.Options;

class DbOptions
{
  public string DatabaseName { get; init; } = "SanctionsSearch.db";
  public int RefreshIntervalInHours { get; init; } = 1;
}