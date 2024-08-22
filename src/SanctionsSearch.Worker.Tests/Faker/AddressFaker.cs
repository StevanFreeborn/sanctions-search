namespace SanctionsSearch.Worker.Tests.Faker;

class AddressFaker : Faker<Address>
{
  public AddressFaker()
  {
    RuleFor(x => x.SdnId, f => f.Random.Int(1, int.MaxValue));
    RuleFor(x => x.StreetAddress, f => f.Address.StreetAddress());
    RuleFor(x => x.CityProvincePostal, f => $"{f.Address.City()}, {f.Address.StateAbbr()} {f.Address.ZipCode()}");
    RuleFor(x => x.Country, f => f.Address.Country());
    RuleFor(x => x.Remarks, f => f.Lorem.Sentence());
  }
}