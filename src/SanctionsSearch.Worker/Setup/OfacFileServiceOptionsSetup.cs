namespace SanctionsSearch.Worker.Setup;

class OfacFileServiceOptionsSetup(IConfiguration configuration) : IConfigureOptions<OfacFileServiceOptions>
{
  private const string SectionName = nameof(OfacFileServiceOptions);
  private readonly IConfiguration _configuration = configuration;

  public void Configure(OfacFileServiceOptions options) => _configuration.GetSection(SectionName).Bind(options);
}