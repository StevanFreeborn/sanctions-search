namespace SanctionsSearch.Worker.Persistence;

class SdnRepository(
  DbContext dbContext,
  ILogger<SdnRepository> logger
) : EfRepository<Sdn>(dbContext, logger), ISdnRepository
{
  public Task DeleteWhereAsync(Expression<Func<Sdn, bool>> predicate)
  {
    return _context.Set<Sdn>().Where(predicate).ExecuteDeleteAsync();
  }
}