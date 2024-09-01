namespace SanctionsSearch.Worker.Models;

class SearchResult(int searchRequestId, List<Hit> hits)
{
  public int SearchRequestId { get; init; } = searchRequestId;
  public List<Hit> Hits { get; init; } = hits;
}

class Hit
{
  public string Name { get; init; } = string.Empty;
  public string Address { get; init; } = string.Empty;
  public string Type { get; init; } = string.Empty;
  public List<string> Programs { get; init; } = [];
}