namespace SanctionsSearch.Worker.Interfaces;

interface ISdnRepository : IRepository<Sdn>
{
  Task DeleteWhereAsync(Expression<Func<Sdn, bool>> predicate);
}