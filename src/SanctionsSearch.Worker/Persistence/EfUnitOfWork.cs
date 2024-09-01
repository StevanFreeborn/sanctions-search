namespace SanctionsSearch.Worker.Persistence;

class EfUnitOfWork(
  DbContext context,
  ILoggerFactory loggerFactory
) : IUnitOfWork, IAsyncDisposable, IDisposable
{
  private readonly DbContext _context = context;
  public ISdnRepository Sdns { get; } = new SdnRepository(context, loggerFactory.CreateLogger<SdnRepository>());
  public IAddressRepository Addresses { get; } = new AddressRepository(context, loggerFactory.CreateLogger<AddressRepository>());
  public IAliasRepository Aliases { get; } = new AliasRepository(context, loggerFactory.CreateLogger<AliasRepository>());
  public ICommentRepository Comments { get; } = new CommentRepository(context, loggerFactory.CreateLogger<CommentRepository>());

  public async Task SaveChangesAsync()
  {
    await _context.SaveChangesAsync();
  }

  public async ValueTask DisposeAsync()
  {
    await _context.DisposeAsync();
  }

  public void Dispose()
  {
    _context.Dispose();
  }
}