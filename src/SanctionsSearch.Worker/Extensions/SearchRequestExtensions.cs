namespace SanctionsSearch.Worker.Extensions;

static class SearchRequestExtensions
{
  public static Expression<Func<Sdn, bool>> ToSdnFilter(this SearchRequest request)
  {
    var nameParts = request.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    return sdn => nameParts.All(part => sdn.Name.Contains(part));
  }
}