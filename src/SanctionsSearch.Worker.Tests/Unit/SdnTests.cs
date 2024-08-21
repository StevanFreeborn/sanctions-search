namespace SanctionsSearch.Worker.Tests.Unit;

public class SdnTests
{
  [Fact]
  public void Name_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.Name = "Name";
    sdn.Name.Should().Be("Name");
  }

  [Fact]
  public void Type_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.Type = "Type";
    sdn.Type.Should().Be("Type");
  }

  [Fact]
  public void Program_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.Program = "Program";
    sdn.Program.Should().Be("Program");
  }

  [Fact]
  public void Title_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.Title = "Title";
    sdn.Title.Should().Be("Title");
  }

  [Fact]
  public void CallSign_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.CallSign = "CallSign";
    sdn.CallSign.Should().Be("CallSign");
  }

  [Fact]
  public void VesselType_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.VesselType = "VesselType";
    sdn.VesselType.Should().Be("VesselType");
  }

  [Fact]
  public void Tonnage_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.Tonnage = "Tonnage";
    sdn.Tonnage.Should().Be("Tonnage");
  }

  [Fact]
  public void GrossRegisteredTonnage_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.GrossRegisteredTonnage = "GrossRegisteredTonnage";
    sdn.GrossRegisteredTonnage.Should().Be("GrossRegisteredTonnage");
  }

  [Fact]
  public void VesselFlag_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.VesselFlag = "VesselFlag";
    sdn.VesselFlag.Should().Be("VesselFlag");
  }

  [Fact]
  public void VesselOwner_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.VesselOwner = "VesselOwner";
    sdn.VesselOwner.Should().Be("VesselOwner");
  }

  [Fact]
  public void Remarks_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    sdn.Remarks = "Some remarks";
    sdn.Remarks.Should().Be("Some remarks");
  }

  [Fact]
  public void Aliases_SetValue_ShouldGetSameValue()
  {
    var sdn = new Sdn();
    var aliases = new List<Alias>();
    sdn.Aliases = aliases;
    sdn.Aliases.Should().BeSameAs(aliases);
  }

  [Fact]
  public void Constructor_DefaultValues_ShouldBeSet()
  {
    var sdn = new Sdn();
    sdn.Name.Should().Be(string.Empty);
    sdn.Type.Should().Be(string.Empty);
    sdn.Program.Should().Be(string.Empty);
    sdn.Title.Should().Be(string.Empty);
    sdn.CallSign.Should().Be(string.Empty);
    sdn.VesselType.Should().Be(string.Empty);
    sdn.Tonnage.Should().Be(string.Empty);
    sdn.GrossRegisteredTonnage.Should().Be(string.Empty);
    sdn.VesselFlag.Should().Be(string.Empty);
    sdn.VesselOwner.Should().Be(string.Empty);
    sdn.Remarks.Should().Be(string.Empty);
    sdn.Aliases.Should().BeEmpty();
  }

  [Fact]
  public void Constructor_WhenInitializingProperties_ShouldSetProperties()
  {
    var sdn = new Sdn
    {
      Name = "Name",
      Type = "Type",
      Program = "Program",
      Title = "Title",
      CallSign = "CallSign",
      VesselType = "VesselType",
      Tonnage = "Tonnage",
      GrossRegisteredTonnage = "GrossRegisteredTonnage",
      VesselFlag = "VesselFlag",
      VesselOwner = "VesselOwner",
      Remarks = "Remarks",
      Aliases = new List<Alias>()
    };

    sdn.Name.Should().Be("Name");
    sdn.Type.Should().Be("Type");
    sdn.Program.Should().Be("Program");
    sdn.Title.Should().Be("Title");
    sdn.CallSign.Should().Be("CallSign");
    sdn.VesselType.Should().Be("VesselType");
    sdn.Tonnage.Should().Be("Tonnage");
    sdn.GrossRegisteredTonnage.Should().Be("GrossRegisteredTonnage");
    sdn.VesselFlag.Should().Be("VesselFlag");
    sdn.VesselOwner.Should().Be("VesselOwner");
    sdn.Remarks.Should().Be("Remarks");
    sdn.Aliases.Should().BeEmpty();
  }
}