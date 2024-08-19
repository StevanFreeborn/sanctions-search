using System.Reflection;

using Microsoft.Data.Sqlite;

namespace SanctionsSearch.Worker.Persistence;

class AppDbContext(IOptionsSnapshot<DbOptions> options) : DbContext, IDisposable
{
  private readonly IOptionsSnapshot<DbOptions> _options = options;
  private SqliteConnection? _connection;
  public DbSet<Sdn> Sdns { get; set; } = default!;
  public DbSet<Address> Addresses { get; set; } = default!;
  public DbSet<Alias> Aliases { get; set; } = default!;
  public DbSet<Comment> Comments { get; set; } = default!;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var folder = Environment.SpecialFolder.LocalApplicationData;
    var path = Environment.GetFolderPath(folder);
    var dataDir = Path.Combine(path, "SanctionsSearch", "Data");

    if (Directory.Exists(dataDir) == false)
    {
      Directory.CreateDirectory(dataDir);
    }

    var dbPath = Path.Combine(dataDir, _options.Value.DatabaseName);

    _connection = new SqliteConnection($"Data Source={dbPath}");
    _connection.Open();

    optionsBuilder.UseSqlite(_connection);
  }

  public override void Dispose()
  {
    _connection?.Dispose();
    base.Dispose();
  }
}