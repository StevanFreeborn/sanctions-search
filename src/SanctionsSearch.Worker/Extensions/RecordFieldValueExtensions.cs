namespace SanctionsSearch.Worker.Extensions;

static class RecordFieldValueExtensions
{
  public static string GetStringValue(this RecordFieldValue recordFieldValue) =>
    recordFieldValue.Type is not ResultValueType.String
      ? string.Empty
      : recordFieldValue.AsString() ?? string.Empty;
}