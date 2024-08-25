namespace SanctionsSearch.Worker.Options;

class DbOptions
{
  public string DatabaseName { get; set; } = string.Empty;
  public int RefreshIntervalInHours { get; set; } = 1;
}