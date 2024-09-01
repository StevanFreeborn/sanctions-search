

namespace SanctionsSearch.Worker.Services;

class SearchService(IUnitOfWork uow) : ISearchService
{
  private readonly IUnitOfWork _uow = uow;

  public async Task<SearchResult> PerformSearchAsync(SearchRequest request)
  {
    var results = await _uow.Sdns.Find(request.ToSdnFilter(), sdn => sdn.Addresses);
    var hits = results.Select(sdn => sdn.ToHit()).ToList();
    return new(request.Id, hits);
  }
}