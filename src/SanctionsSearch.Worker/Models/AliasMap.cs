namespace SanctionsSearch.Worker.Models;

class AliasMap : ClassMap<Alias>
{
  public AliasMap()
  {
    Map(m => m.SdnId).Index(0);
    Map(m => m.Id).Index(1);
    Map(m => m.Type).Index(2).TypeConverter<NullCharacterConverter>();
    Map(m => m.Name).Index(3).TypeConverter<NullCharacterConverter>();
    Map(m => m.Remarks).Index(4).TypeConverter<NullCharacterConverter>();
  }
}