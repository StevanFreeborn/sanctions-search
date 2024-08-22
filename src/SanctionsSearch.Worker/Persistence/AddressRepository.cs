namespace SanctionsSearch.Worker.Persistence;

class AddressRepository(
  DbContext dbContext,
  ILogger<AddressRepository> logger
) : EfRepository<Address>(dbContext, logger), IAddressRepository
{
}