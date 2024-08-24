namespace SanctionsSearch.Worker.Persistence;

class EfUnitOfWork(DbContext context, ILoggerFactory loggerFactory) : IUnitOfWork, IAsyncDisposable
{
  private readonly DbContext _context = context;
  private readonly ILogger<EfUnitOfWork> _logger = loggerFactory.CreateLogger<EfUnitOfWork>();
  public ISdnRepository Sdns { get; } = new SdnRepository(context, loggerFactory.CreateLogger<SdnRepository>());
  public IAddressRepository Addresses { get; } = new AddressRepository(context, loggerFactory.CreateLogger<AddressRepository>());
  public IAliasRepository Aliases { get; } = new AliasRepository(context, loggerFactory.CreateLogger<AliasRepository>());
  public ICommentRepository Comments { get; } = new CommentRepository(context, loggerFactory.CreateLogger<CommentRepository>());

  public async Task SaveChangesAsync()
  {
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateException ex)
    {
      _logger.LogError(ex, "Failed to save changes to the database.");
    }
  }

  public async ValueTask DisposeAsync()
  {
    try
    {
      await _context.DisposeAsync();
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to dispose of the database context.");
    }
  }
}