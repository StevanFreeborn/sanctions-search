namespace SanctionsSearch.Worker.Persistence;

class EfUnitOfWork(AppDbContext context, ILoggerFactory loggerFactory) : IUnitOfWork, IAsyncDisposable
{
  private readonly AppDbContext _context = context;
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
}