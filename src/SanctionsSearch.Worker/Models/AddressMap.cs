namespace SanctionsSearch.Worker.Models;

class AddressMap : ClassMap<Address>
{
  public AddressMap()
  {
    Map(m => m.SdnId).Index(0);
    Map(m => m.Id).Index(1);
    Map(m => m.StreetAddress).Index(2).TypeConverter<NullCharacterConverter>();
    Map(m => m.CityProvincePostal).Index(3).TypeConverter<NullCharacterConverter>();
    Map(m => m.Country).Index(4).TypeConverter<NullCharacterConverter>();
    Map(m => m.Remarks).Index(5).TypeConverter<NullCharacterConverter>();
  }
}