namespace SanctionsSearch.Worker.Tests.Unit;

public class AliasTests
{
  [Fact]
  public void SdnId_SetValue_ShouldGetSameValue()
  {
    var alias = new Alias();
    alias.SdnId = 123;
    alias.SdnId.Should().Be(123);
  }

  [Fact]
  public void Type_SetValue_ShouldGetSameValue()
  {
    var alias = new Alias();
    alias.Type = "Type";
    alias.Type.Should().Be("Type");
  }

  [Fact]
  public void Name_SetValue_ShouldGetSameValue()
  {
    var alias = new Alias();
    alias.Name = "Name";
    alias.Name.Should().Be("Name");
  }

  [Fact]
  public void Remarks_SetValue_ShouldGetSameValue()
  {
    var alias = new Alias();
    alias.Remarks = "Some remarks";
    alias.Remarks.Should().Be("Some remarks");
  }

  [Fact]
  public void Sdn_SetValue_ShouldGetSameValue()
  {
    var alias = new Alias();
    var sdn = new Sdn();
    alias.Sdn = sdn;
    alias.Sdn.Should().Be(sdn);
  }

  [Fact]
  public void Constructor_DefaultValues_ShouldBeSet()
  {
    var alias = new Alias();
    alias.Type.Should().Be(string.Empty);
    alias.Name.Should().Be(string.Empty);
    alias.Remarks.Should().Be(string.Empty);
  }

  [Fact]
  public void Constructor_WhenInitializingProperties_ShouldSetProperties()
  {
    var alias = new Alias
    {
      SdnId = 123,
      Type = "Type",
      Name = "Name",
      Remarks = "Remarks"
    };

    alias.SdnId.Should().Be(123);
    alias.Type.Should().Be("Type");
    alias.Name.Should().Be("Name");
    alias.Remarks.Should().Be("Remarks");
  }
}