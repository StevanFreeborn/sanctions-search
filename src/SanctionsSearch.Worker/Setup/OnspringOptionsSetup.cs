namespace SanctionsSearch.Worker.Setup;

class OnspringOptionsSetup(IConfiguration configuration) : IConfigureOptions<OnspringOptions>
{
  private const string SectionName = nameof(OnspringOptions);
  private readonly IConfiguration _configuration = configuration;

  public void Configure(OnspringOptions options) => _configuration.GetSection(SectionName).Bind(options);
}