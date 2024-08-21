namespace SanctionsSearch.Worker.Persistence;

class EfUnitOfWork(AppDbContext context, ILoggerFactory loggerFactory) : IUnitOfWork, IAsyncDisposable
{
  private readonly AppDbContext _context = context;
  public ISdnRepository Sdns { get; } = new SdnRepository(context, loggerFactory.CreateLogger<SdnRepository>());

  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }

  public async ValueTask DisposeAsync()
  {
    await _context.DisposeAsync();
  }
}