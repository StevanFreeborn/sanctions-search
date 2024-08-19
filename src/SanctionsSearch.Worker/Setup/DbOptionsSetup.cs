namespace SanctionsSearch.Worker.Setup;

class DbOptionsSetup(IConfiguration configuration) : IConfigureOptions<DbOptions>
{
  private const string SectionName = nameof(DbOptions);
  private readonly IConfiguration _configuration = configuration;

  public void Configure(DbOptions options) => _configuration.GetSection(SectionName).Bind(options);
}