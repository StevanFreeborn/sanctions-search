namespace SanctionsSearch.Worker.Extensions;

static class GetPagedRecordsResponseExtensions
{
  public static bool HasMorePages(this GetPagedRecordsResponse response)
  {
    return response.PageNumber < response.TotalPages;
  }
}