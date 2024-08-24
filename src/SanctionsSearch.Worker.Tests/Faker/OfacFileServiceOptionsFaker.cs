namespace SanctionsSearch.Worker.Tests.Faker;

class OfacFileServiceOptionsFaker : Faker<OfacFileServiceOptions>
{
  public OfacFileServiceOptionsFaker()
  {
    RuleFor(x => x.Url, f => f.Internet.Url());
    RuleFor(x => x.SdnFileName, f => f.System.FileName("csv"));
    RuleFor(x => x.AddressFileName, f => f.System.FileName("csv"));
    RuleFor(x => x.AltNamesFileName, f => f.System.FileName("csv"));
    RuleFor(x => x.CommentsFileName, f => f.System.FileName("csv"));
    RuleFor(x => x.ConPrimaryNameFileName, f => f.System.FileName("csv"));
    RuleFor(x => x.ConAddressesFileName, f => f.System.FileName("csv"));
    RuleFor(x => x.ConAltNamesFileName, f => f.System.FileName("csv"));
    RuleFor(x => x.ConCommentsFileName, f => f.System.FileName("csv"));
  }
}