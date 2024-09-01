namespace SanctionsSearch.Worker.Tests.Integration;

public class SearchServiceTests : DatabaseTest
{
  private readonly SearchService _searchService;
  private readonly SdnFaker _sdnFaker = new();
  private readonly AddressFaker _addressFaker = new();

  public SearchServiceTests()
  {
    var uow = new EfUnitOfWork(_context, _loggerFactory);
    _searchService = new SearchService(uow);
  }

  [Fact]
  public async Task PerformSearchAsync_WhenCalledWithEmptyRequest_ItShouldReturnEmptyResult()
  {
    var request = new SearchRequest();

    var result = await _searchService.PerformSearchAsync(request);

    result.SearchRequestId.Should().Be(request.Id);
    result.Hits.Should().BeEmpty();
  }

  [Fact]
  public async Task PerformSearchAsync_WhenCalledWithRequest_ItShouldReturnHits()
  {
    var sdn = _sdnFaker.Generate();
    sdn.Name = "PUTIN, Vladimir Vladimirovich";

    var address = _addressFaker.Generate();
    address.SdnId = sdn.Id;

    await _context.Set<Sdn>().AddAsync(sdn);
    await _context.Set<Address>().AddAsync(address);
    await _context.SaveChangesAsync();

    var request = new SearchRequest
    {
      Name = "Putin",
    };

    var result = await _searchService.PerformSearchAsync(request);

    result.SearchRequestId.Should().Be(request.Id);
    result.Hits.Should().HaveCount(1);
    result.Hits.First().Name.Should().Be(sdn.Name);
    result.Hits.First().Address.Should().Be(address.ToString());
  }
}