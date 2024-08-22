namespace SanctionsSearch.Worker.Persistence;

class AliasRepository(
  DbContext dbContext,
  ILogger<AliasRepository> logger
) : EfRepository<Alias>(dbContext, logger), IAliasRepository
{
}