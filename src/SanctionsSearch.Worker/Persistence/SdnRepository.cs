namespace SanctionsSearch.Worker.Persistence;

class SdnRepository(
  AppDbContext dbContext,
  ILogger<SdnRepository> logger
) : EfRepository<Sdn>(dbContext, logger), ISdnRepository
{
}