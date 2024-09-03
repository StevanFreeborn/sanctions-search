namespace SanctionsSearch.Worker.Extensions;

public class RecordFieldValueExtensionsTests
{
  [Fact]
  public void GetStringValue_WhenRecordFieldValueTypeIsString_ReturnsValue()
  {
    var value = "value";

    var recordFieldValue = new StringFieldValue(1, value);

    recordFieldValue.GetStringValue().Should().Be(value);
  }

  [Fact]
  public void GetStringValue_WhenRecordFieldValueTypeIsNotString_ReturnsEmptyString()
  {
    var recordFieldValue = new IntegerFieldValue(1, 1);

    recordFieldValue.GetStringValue().Should().BeEmpty();
  }

  [Fact]
  public void GetStringValue_WhenRecordFieldValueValueIsNull_ReturnsEmptyString()
  {
    var recordFieldValue = new StringFieldValue(1, null);

    recordFieldValue.GetStringValue().Should().BeEmpty();
  }
}