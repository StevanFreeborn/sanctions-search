namespace SanctionsSearch.Worker.Tests.Faker;

class SdnFaker : Faker<Sdn>
{
  public const int ReservedId = 1;
  private readonly int _minimumId = ReservedId + 1;

  public SdnFaker()
  {
    RuleFor(x => x.Id, f => f.Random.Int(_minimumId, int.MaxValue));
    RuleFor(x => x.Name, f => f.Person.FullName);
    RuleFor(x => x.Type, f => f.Lorem.Word());
    RuleFor(x => x.Program, f => f.Lorem.Word());
    RuleFor(x => x.Title, f => f.Lorem.Word());
    RuleFor(x => x.CallSign, f => f.Lorem.Word());
    RuleFor(x => x.VesselType, f => f.Lorem.Word());
    RuleFor(x => x.Tonnage, f => f.Lorem.Word());
    RuleFor(x => x.GrossRegisteredTonnage, f => f.Lorem.Word());
    RuleFor(x => x.VesselFlag, f => f.Lorem.Word());
    RuleFor(x => x.VesselOwner, f => f.Lorem.Word());
    RuleFor(x => x.Remarks, f => f.Lorem.Word());
  }
}