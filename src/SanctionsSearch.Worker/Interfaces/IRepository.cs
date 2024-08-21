namespace SanctionsSearch.Worker.Interfaces;

interface IRepository<T> where T : Entity
{
  Task Upsert(T entity);
  Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
}
