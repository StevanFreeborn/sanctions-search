namespace SanctionsSearch.Worker.Tests.Unit;

public class GetPagedRecordsResponseExtensionsTests
{
  [Fact]
  public void HasMorePages_WhenPageNumberIsLessThanTotalPages_ReturnsTrue()
  {
    var response = new GetPagedRecordsResponse
    {
      PageNumber = 1,
      TotalPages = 2
    };

    response.HasMorePages().Should().BeTrue();
  }

  [Fact]
  public void HasMorePages_WhenPageNumberIsEqualToTotalPages_ReturnsFalse()
  {
    var response = new GetPagedRecordsResponse
    {
      PageNumber = 2,
      TotalPages = 2
    };

    response.HasMorePages().Should().BeFalse();
  }
}