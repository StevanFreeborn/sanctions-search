namespace SanctionsSearch.Worker.Persistence;

class EfRepository<T> : IRepository<T> where T : Entity
{
  protected readonly DbContext _context;
  protected readonly ILogger<EfRepository<T>> _logger;

  public EfRepository(DbContext context, ILogger<EfRepository<T>> logger)
  {
    _context = context;
    _logger = logger;
  }

  public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[]? includes)
  {
    var query = _context.Set<T>().AsQueryable();

    if (includes is not null)
    {
      query = includes.Aggregate(query, (current, include) => current.Include(include));
    }

    return await query.Where(predicate).ToListAsync();
  }

  public async Task Upsert(T entity)
  {
    var existing = await _context.Set<T>().FindAsync(entity.Id);

    if (existing is null)
    {
      await _context.Set<T>().AddAsync(entity);
      return;
    }

    _context.Entry(existing).CurrentValues.SetValues(entity);
  }
}