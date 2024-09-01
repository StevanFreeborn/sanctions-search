namespace SanctionsSearch.Worker.Models;

class SearchRequest
{
  public int Id { get; init; }
  public string Name { get; set; } = string.Empty;
}