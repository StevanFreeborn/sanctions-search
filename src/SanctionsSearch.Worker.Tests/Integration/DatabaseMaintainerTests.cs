namespace SanctionsSearch.Worker.Tests.Integration;

public class DatabaseMaintainerTests : DatabaseTest
{
  private readonly MockHttpMessageHandler _mockHttp = new();
  private readonly OfacFileServiceOptionsFaker _ofacFileServiceOptionsFaker = new();
  private readonly OfacFileServiceOptions _ofacFileServiceOptions;
  private readonly DatabaseMaintainer _databaseMaintainer;
  private static MemoryStream CreateCsvStream(string csv) => new(Encoding.UTF8.GetBytes(csv));

  public DatabaseMaintainerTests()
  {
    _ofacFileServiceOptions = _ofacFileServiceOptionsFaker.Generate();

    var ofacFileService = new OfacFileService(
      _mockHttp.ToHttpClient(),
      _loggerFactory.CreateLogger<OfacFileService>(),
      _ofacFileServiceOptions
    );

    var uow = new EfUnitOfWork(_context, _loggerFactory);
    var logger = _loggerFactory.CreateLogger<DatabaseMaintainer>();
    _databaseMaintainer = new DatabaseMaintainer(uow, ofacFileService, logger);
  }

  [Fact]
  public async Task BuildSdnTableAsync_WhenCalled_ItShouldAddSdnCsvRecordsToDatabase()
  {
    var testCsv = """
      6906,"AL-IRAQI, Abd al-Hadi","individual","SDGT",-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,"DOB 1961; POB Mosul, Iraq; nationality Iraq; Gender Male."
      6907,"SHIHATA, Thirwat Salah","individual","SDGT",-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,"DOB 29 Jun 1960; POB Egypt."
      6908,"AHMAD, Tariq Anwar al-Sayyid","individual","SDGT",-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,"DOB 15 Mar 1963; POB Alexandria, Egypt."
    """;

    var testStream = CreateCsvStream(testCsv);

    _mockHttp
      .When(_ofacFileServiceOptions.GetSdnFileUri().ToString())
      .Respond("text/csv", testStream);

    var now = DateTimeOffset.UtcNow;

    _timeProviderMock
      .Setup(x => x.GetUtcNow())
      .Returns(now);

    await _databaseMaintainer.BuildSdnTableAsync();

    var sdns = await _context.Set<Sdn>().ToListAsync();

    sdns.Should().HaveCount(3);

    sdns[0].Should().BeEquivalentTo(new Sdn()
    {
      Id = 6906,
      Name = "AL-IRAQI, Abd al-Hadi",
      Type = "individual",
      Program = "SDGT",
      Remarks = "DOB 1961; POB Mosul, Iraq; nationality Iraq; Gender Male.",
      CreatedAt = now.DateTime,
      UpdatedAt = now.DateTime
    });

    sdns[1].Should().BeEquivalentTo(new Sdn()
    {
      Id = 6907,
      Name = "SHIHATA, Thirwat Salah",
      Type = "individual",
      Program = "SDGT",
      Remarks = "DOB 29 Jun 1960; POB Egypt.",
      CreatedAt = now.DateTime,
      UpdatedAt = now.DateTime
    });

    sdns[2].Should().BeEquivalentTo(new Sdn()
    {
      Id = 6908,
      Name = "AHMAD, Tariq Anwar al-Sayyid",
      Type = "individual",
      Program = "SDGT",
      Remarks = "DOB 15 Mar 1963; POB Alexandria, Egypt.",
      CreatedAt = now.DateTime,
      UpdatedAt = now.DateTime
    });
  }
}