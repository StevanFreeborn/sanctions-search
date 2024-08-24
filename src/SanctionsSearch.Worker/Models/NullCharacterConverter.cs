using CsvHelper.TypeConversion;

namespace SanctionsSearch.Worker.Models;

class NullCharacterConverter : DefaultTypeConverter
{
  private const string NullCharacter = "-0-";

  public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
  {
    return text is null || text.Trim() is NullCharacter
      ? string.Empty
      : text;
  }
}