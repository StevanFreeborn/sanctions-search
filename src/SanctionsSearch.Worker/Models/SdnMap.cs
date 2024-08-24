namespace SanctionsSearch.Worker.Models;

class SdnMap : ClassMap<Sdn>
{
  public SdnMap()
  {
    Map(m => m.Id).Index(0);
    Map(m => m.Name).Index(1).TypeConverter<NullCharacterConverter>();
    Map(m => m.Type).Index(2).TypeConverter<NullCharacterConverter>();
    Map(m => m.Program).Index(3).TypeConverter<NullCharacterConverter>();
    Map(m => m.Title).Index(4).TypeConverter<NullCharacterConverter>();
    Map(m => m.CallSign).Index(5).TypeConverter<NullCharacterConverter>();
    Map(m => m.VesselType).Index(6).TypeConverter<NullCharacterConverter>();
    Map(m => m.Tonnage).Index(7).TypeConverter<NullCharacterConverter>();
    Map(m => m.GrossRegisteredTonnage).Index(8).TypeConverter<NullCharacterConverter>();
    Map(m => m.VesselFlag).Index(9).TypeConverter<NullCharacterConverter>();
    Map(m => m.VesselOwner).Index(10).TypeConverter<NullCharacterConverter>();
    Map(m => m.Remarks).Index(11).TypeConverter<NullCharacterConverter>();
  }
}
