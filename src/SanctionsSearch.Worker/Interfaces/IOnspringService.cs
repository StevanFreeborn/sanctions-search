namespace SanctionsSearch.Worker.Interfaces;

interface IOnspringService
{
  Task<List<SearchRequest>> GetSearchRequestsAsync();
}