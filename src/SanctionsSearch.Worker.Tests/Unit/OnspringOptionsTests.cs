namespace SanctionsSearch.Worker.Tests.Unit;

public class OnspringOptionsTests
{
  [Fact]
  public void OnspringOptions_WhenCreated_ShouldHaveDefaultValues()
  {
    var onspringOptions = new OnspringOptions();

    var searchRequestOptions = onspringOptions.SearchRequestOptions;
    var searchResultOptions = onspringOptions.SearchResultOptions;

    onspringOptions.BaseUrl.Should().Be("https://api.onspring.com");
    onspringOptions.ApiKey.Should().Be(string.Empty);
    onspringOptions.SearchIntervalInMinutes.Should().Be(1);
    searchRequestOptions.AppId.Should().Be(0);
    searchRequestOptions.NameFieldId.Should().Be(0);
    searchRequestOptions.StatusFieldId.Should().Be(0);
    searchRequestOptions.AwaitingProcessingStatusId.Should().Be(Guid.Empty);
    searchRequestOptions.ProcessingStatusId.Should().Be(Guid.Empty);
    searchRequestOptions.ProcessedSuccessStatusId.Should().Be(Guid.Empty);
    searchRequestOptions.ProcessedErrorStatusId.Should().Be(Guid.Empty);
    searchRequestOptions.ErrorFieldId.Should().Be(0);
    searchResultOptions.AppId.Should().Be(0);
    searchResultOptions.SearchRequestFieldId.Should().Be(0);
    searchResultOptions.NameFieldId.Should().Be(0);
    searchResultOptions.AddressFieldId.Should().Be(0);
    searchResultOptions.TypeFieldId.Should().Be(0);
    searchResultOptions.ProgramsFieldId.Should().Be(0);
  }
}