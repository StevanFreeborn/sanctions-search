namespace SanctionsSearch.Worker.Tests.Faker;

class AliasFaker : Faker<Alias>
{
  public AliasFaker()
  {
    RuleFor(x => x.SdnId, f => f.Random.Int(1, int.MaxValue));
    RuleFor(x => x.Type, f => f.Lorem.Word());
    RuleFor(x => x.Name, f => f.Person.FullName);
    RuleFor(x => x.Remarks, f => f.Lorem.Sentence());
  }
}