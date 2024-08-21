namespace SanctionsSearch.Worker.Persistence;

class AppDbContext(DbOptions options) : DbContext
{
  private readonly DbOptions _options = options;
  private SqliteConnection? _connection;
  internal string DatabasePath { get; private set; } = string.Empty;
  public DbSet<Sdn> Sdns { get; set; } = default!;
  public DbSet<Address> Addresses { get; set; } = default!;
  public DbSet<Alias> Aliases { get; set; } = default!;
  public DbSet<Comment> Comments { get; set; } = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var folder = Assembly.GetExecutingAssembly().Location;
    var path = Path.GetDirectoryName(folder);
    var dataDir = Path.Combine(path!, "Data");

    if (Directory.Exists(dataDir) == false)
    {
      Directory.CreateDirectory(dataDir);
    }

    var dbPath = Path.Combine(dataDir, _options.DatabaseName);
    DatabasePath = dbPath;

    _connection = new SqliteConnection($"Data Source={dbPath}");
    _connection.Open();

    optionsBuilder.UseSqlite(_connection);
  }

  public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    // TODO: Use TimeProvider instead of DateTime.UtcNow
    var now = DateTime.UtcNow;

    foreach (var changedEntity in ChangeTracker.Entries())
    {
      if (changedEntity.Entity is Entity entity)
      {
        switch (changedEntity.State)
        {
          case EntityState.Added:
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
            break;

          case EntityState.Modified:
            Entry(entity).Property(x => x.CreatedAt).IsModified = false;
            entity.UpdatedAt = now;
            break;
        }
      }
    }

    return await base.SaveChangesAsync(cancellationToken);
  }

  public async override ValueTask DisposeAsync()
  {
    if (_connection is not null)
    {
      await _connection.DisposeAsync();
    }

    await base.DisposeAsync();
  }
}