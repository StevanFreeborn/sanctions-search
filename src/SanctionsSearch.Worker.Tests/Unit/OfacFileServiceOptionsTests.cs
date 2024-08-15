namespace SanctionsSearch.Worker.Tests.Unit;

public class OfacFileServiceOptionsTests
{
  [Fact]
  public void GetSdnFileUri_WhenCalled_ReturnsUri()
  {
    var options = new OfacFileServiceOptions
    {
      Url = "https://example.com",
      SdnFileName = "sdn.csv"
    };

    var uri = options.GetSdnFileUri();

    uri.Should().Be(new Uri("https://example.com/sdn.csv"));
  }

  [Fact]
  public void GetAddressFileUri_WhenCalled_ReturnsUri()
  {
    var options = new OfacFileServiceOptions
    {
      Url = "https://example.com",
      AddressFileName = "address.csv"
    };

    var uri = options.GetAddressFileUri();

    uri.Should().Be(new Uri("https://example.com/address.csv"));
  }

  [Fact]
  public void GetAltNamesFileUri_WhenCalled_ReturnsUri()
  {
    var options = new OfacFileServiceOptions
    {
      Url = "https://example.com",
      AltNamesFileName = "alt_names.csv"
    };

    var uri = options.GetAltNamesFileUri();

    uri.Should().Be(new Uri("https://example.com/alt_names.csv"));
  }

  [Fact]
  public void GetCommentsFileUri_WhenCalled_ReturnsUri()
  {
    var options = new OfacFileServiceOptions
    {
      Url = "https://example.com",
      CommentsFileName = "comments.csv"
    };

    var uri = options.GetCommentsFileUri();

    uri.Should().Be(new Uri("https://example.com/comments.csv"));
  }

  [Fact]
  public void GetConPrimaryNameFileUri_WhenCalled_ReturnsUri()
  {
    var options = new OfacFileServiceOptions
    {
      Url = "https://example.com",
      ConPrimaryNameFileName = "con_primary_name.csv"
    };

    var uri = options.GetConPrimaryNameFileUri();

    uri.Should().Be(new Uri("https://example.com/con_primary_name.csv"));
  }

  [Fact]
  public void GetConAddressesFileUri_WhenCalled_ReturnsUri()
  {
    var options = new OfacFileServiceOptions
    {
      Url = "https://example.com",
      ConAddressesFileName = "con_addresses.csv"
    };

    var uri = options.GetConAddressesFileUri();

    uri.Should().Be(new Uri("https://example.com/con_addresses.csv"));
  }

  [Fact]
  public void GetConAltNamesFileUri_WhenCalled_ReturnsUri()
  {
    var options = new OfacFileServiceOptions
    {
      Url = "https://example.com",
      ConAltNamesFileName = "con_alt_names.csv"
    };

    var uri = options.GetConAltNamesFileUri();

    uri.Should().Be(new Uri("https://example.com/con_alt_names.csv"));
  }

  [Fact]
  public void GetConCommentsFileUri_WhenCalled_ReturnsUri()
  {
    var options = new OfacFileServiceOptions
    {
      Url = "https://example.com",
      ConCommentsFileName = "con_comments.csv"
    };

    var uri = options.GetConCommentsFileUri();

    uri.Should().Be(new Uri("https://example.com/con_comments.csv"));
  }
}