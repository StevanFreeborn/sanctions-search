namespace SanctionsSearch.Worker.Persistence;

class EfRepository<T> : IRepository<T> where T : Entity
{
  protected readonly AppDbContext _context;
  protected readonly ILogger<EfRepository<T>> _logger;

  public EfRepository(AppDbContext context, ILogger<EfRepository<T>> logger)
  {
    _context = context;
    _logger = logger;
  }

  public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
  {
    try
    {
      return await _context.Set<T>().Where(predicate).ToListAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error finding entities of type {Type}", typeof(T).Name);
      return [];
    }
  }

  public async Task Upsert(T entity)
  {
    try
    {
      var existing = await _context.Set<T>().FindAsync(entity.Id);

      if (existing is null)
      {
        await _context.Set<T>().AddAsync(entity);
        return;
      }

      _context.Entry(existing).CurrentValues.SetValues(entity);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error upserting entity of type {Type} with {Id}", typeof(T).Name, entity.Id);
    }
  }
}