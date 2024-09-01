namespace SanctionsSearch.Worker.Models;

class SearchResult
{
  public int SearchRequestId { get; init; }
  public List<Hit> Hits { get; init; } = [];

  public SearchResult(int searchRequestId, List<Hit> hits)
  {
    SearchRequestId = searchRequestId;
    Hits = hits;
  }
}

class Hit
{
  public string Name { get; init; } = string.Empty;
  public string Address { get; init; } = string.Empty;
  public string Type { get; init; } = string.Empty;
  public List<string> Programs { get; init; } = [];
}