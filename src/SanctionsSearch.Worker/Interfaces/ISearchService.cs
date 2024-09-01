namespace SanctionsSearch.Worker.Interfaces;

interface ISearchService
{
  Task<SearchResult> PerformSearchAsync(SearchRequest request);
}