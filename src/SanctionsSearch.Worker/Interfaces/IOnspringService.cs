namespace SanctionsSearch.Worker.Interfaces;

interface IOnspringService
{
  Task<List<SearchRequest>> GetSearchRequestsAsync();
  Task<Result> AddSearchResultAsync(SearchResult result);
}