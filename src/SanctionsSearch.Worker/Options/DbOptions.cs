namespace SanctionsSearch.Worker.Options;

class DbOptions
{
  public string DatabaseName { get; set; } = "SanctionsSearch.db";
  public int RefreshIntervalInHours { get; set; } = 1;
}