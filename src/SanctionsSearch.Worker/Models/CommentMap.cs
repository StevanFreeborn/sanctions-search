namespace SanctionsSearch.Worker.Models;

class CommentMap : ClassMap<Comment>
{
  public CommentMap()
  {
    Map(m => m.SdnId).Index(0);
    Map(m => m.Remarks).Index(1).TypeConverter<NullCharacterConverter>();
  }
}