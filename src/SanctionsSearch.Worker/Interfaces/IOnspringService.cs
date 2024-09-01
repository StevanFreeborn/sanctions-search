namespace SanctionsSearch.Worker.Interfaces;

interface IOnspringService
{
  Task<Result> UpdateSearchRequestAsProcessingAsync(SearchRequest request);
  Task<Result> UpdateSearchRequestAsFailedAsync(SearchRequest request, string error);
  Task<List<SearchRequest>> GetSearchRequestsAsync();
  Task<Result> AddSearchResultAsync(SearchResult result);
}