namespace SanctionsSearch.Worker.Tests.Unit;

public class CommentTests
{
  [Fact]
  public void SdnId_SetValue_ShouldGetSameValue()
  {
    var comment = new Comment();
    comment.SdnId = 123;
    comment.SdnId.Should().Be(123);
  }

  [Fact]
  public void Remarks_SetValue_ShouldGetSameValue()
  {
    var comment = new Comment();
    comment.Remarks = "Some remarks";
    comment.Remarks.Should().Be("Some remarks");
  }

  [Fact]
  public void Sdn_SetValue_ShouldGetSameValue()
  {
    var comment = new Comment();
    var sdn = new Sdn();
    comment.Sdn = sdn;
    comment.Sdn.Should().Be(sdn);
  }

  [Fact]
  public void Constructor_DefaultValues_ShouldBeSet()
  {
    var comment = new Comment();
    comment.Remarks.Should().Be(string.Empty);
  }

  [Fact]
  public void Constructor_WhenInitializingProperties_ShouldSetProperties()
  {
    var comment = new Comment
    {
      SdnId = 123,
      Remarks = "Some remarks"
    };

    comment.SdnId.Should().Be(123);
    comment.Remarks.Should().Be("Some remarks");
  }
}