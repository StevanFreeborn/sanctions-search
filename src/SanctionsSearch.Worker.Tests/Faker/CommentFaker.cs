namespace SanctionsSearch.Worker.Tests.Faker;

class CommentFaker : Faker<Comment>
{
  public CommentFaker()
  {
    RuleFor(x => x.SdnId, f => f.Random.Int(1, int.MaxValue));
    RuleFor(x => x.Remarks, f => f.Lorem.Sentence());
  }
}