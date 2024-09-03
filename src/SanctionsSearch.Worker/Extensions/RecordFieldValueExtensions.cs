namespace SanctionsSearch.Worker.Extensions;

static class RecordFieldValueExtensions
{
  public static string GetStringValue(this RecordFieldValue recordFieldValue)
  {
    try
    {
      return recordFieldValue.AsString() ?? string.Empty;
    }
    catch
    {
      return string.Empty;
    }
  }
}