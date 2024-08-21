namespace SanctionsSearch.Worker.Tests.Unit;

public class AddressTests
{
  [Fact]
  public void SdnId_SetValue_ShouldGetSameValue()
  {
    var address = new Address();
    address.SdnId = 123;
    address.SdnId.Should().Be(123);
  }

  [Fact]
  public void StreetAddress_SetValue_ShouldGetSameValue()
  {
    var address = new Address();
    address.StreetAddress = "123 Main St";
    address.StreetAddress.Should().Be("123 Main St");
  }

  [Fact]
  public void CityProvincePostal_SetValue_ShouldGetSameValue()
  {
    var address = new Address();
    address.CityProvincePostal = "City, Province, Postal";
    address.CityProvincePostal.Should().Be("City, Province, Postal");
  }

  [Fact]
  public void Country_SetValue_ShouldGetSameValue()
  {
    var address = new Address();
    address.Country = "CountryName";
    address.Country.Should().Be("CountryName");
  }

  [Fact]
  public void Remarks_SetValue_ShouldGetSameValue()
  {
    var address = new Address();
    address.Remarks = "Some remarks";
    address.Remarks.Should().Be("Some remarks");
  }

  [Fact]
  public void Sdn_SetValue_ShouldGetSameValue()
  {
    var address = new Address();
    var sdn = new Sdn();
    address.Sdn = sdn;
    address.Sdn.Should().Be(sdn);
  }

  [Fact]
  public void Constructor_DefaultValues_ShouldBeSet()
  {
    var address = new Address();
    address.StreetAddress.Should().Be(string.Empty);
    address.CityProvincePostal.Should().Be(string.Empty);
    address.Country.Should().Be(string.Empty);
    address.Remarks.Should().Be(string.Empty);
    address.Sdn.Should().Be(default);
  }

  [Fact]
  public void Constructor_WhenInitializingProperties_ShouldSetProperties()
  {
    var address = new Address
    {
      SdnId = 123,
      StreetAddress = "123 Main St",
      CityProvincePostal = "City, Province, Postal",
      Country = "CountryName",
      Remarks = "Some remarks",
      Sdn = new Sdn()
    };

    address.SdnId.Should().Be(123);
    address.StreetAddress.Should().Be("123 Main St");
    address.CityProvincePostal.Should().Be("City, Province, Postal");
    address.Country.Should().Be("CountryName");
    address.Remarks.Should().Be("Some remarks");
    address.Sdn.Should().NotBeNull();
  }
}