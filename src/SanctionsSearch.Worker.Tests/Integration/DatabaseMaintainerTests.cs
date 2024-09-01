namespace SanctionsSearch.Worker.Tests.Integration;

public class DatabaseMaintainerTests : DatabaseTest, IDisposable
{
  private const string TestSdnCsv = """
    6906,"AL-IRAQI, Abd al-Hadi","individual","SDGT",-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,"DOB 1961; POB Mosul, Iraq; nationality Iraq; Gender Male."
    6907,"SHIHATA, Thirwat Salah","individual","SDGT",-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,"DOB 29 Jun 1960; POB Egypt."
    6908,"AHMAD, Tariq Anwar al-Sayyid","individual","SDGT",-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,-0- ,"DOB 15 Mar 1963; POB Alexandria, Egypt."
  """;
  private const string TestAddressCsv = """
    306,201,"Dai-Ichi Bldg. 6th Floor, 10-2 Nihombashi, 2-chome, Chuo-ku","Tokyo 103","Japan",-0- 
    306,202,"Federico Boyd Avenue & 51 Street","Panama City","Panama",-0-
  """;
  private const string TestAliasCsv = """
    555,477,"aka","COPROVA",-0- 
    555,478,"aka","COPROVA SARL",-0- 
  """;
  private const string TestCommentCsv = """
    27307,"G TEAM'; a.k.a. 'RED DOT'; a.k.a. 'TEMP.HERMIT'; a.k.a. 'GROUP 77'; a.k.a. 'ZINC'; a.k.a. 'APT-C-26'; a.k.a. 'APPLEWORM'."
    28263,"hn'; Linked To: LAZARUS GROUP."
  """;

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

    var dbMaintainerLogger = _loggerFactory.CreateLogger<DatabaseMaintainer>();

    var uow = new EfUnitOfWork(_context, _loggerFactory);
    _databaseMaintainer = new DatabaseMaintainer(uow, ofacFileService, dbMaintainerLogger);
  }

  [Fact]
  public async Task BuildSdnTableAsync_WhenCalled_ItShouldAddSdnCsvRecordsToDatabase()
  {
    using var testStream = CreateCsvStream(TestSdnCsv);

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

  [Fact]
  public async Task BuildAddressTableAsync_WhenCalledAndNoSdnRecordFound_ItShouldNotAddAddressCsvRecordsToDatabase()
  {
    using var testStream = CreateCsvStream(TestAddressCsv);

    _mockHttp
      .When(_ofacFileServiceOptions.GetAddressFileUri().ToString())
      .Respond("text/csv", testStream);

    await _databaseMaintainer.BuildAddressTableAsync();

    var addresses = await _context.Set<Address>().ToListAsync();

    addresses.Should().BeEmpty();
  }

  [Fact]
  public async Task BuildAddressTableAsync_WhenCalledAndSdnRecordFound_ItShouldAddAddressCsvRecordsToDatabase()
  {
    using var testStream = CreateCsvStream(TestAddressCsv);

    _mockHttp
      .When(_ofacFileServiceOptions.GetAddressFileUri().ToString())
      .Respond("text/csv", testStream);

    var now = DateTimeOffset.UtcNow;

    _timeProviderMock
      .Setup(x => x.GetUtcNow())
      .Returns(now);

    var sdn = new Sdn() { Id = 306 };

    await _context.Set<Sdn>().AddAsync(sdn);
    await _context.SaveChangesAsync();

    await _databaseMaintainer.BuildAddressTableAsync();

    var addresses = await _context.Set<Address>().ToListAsync();

    addresses.Should().BeEquivalentTo(new[]
    {
      new Address()
      {
        SdnId = sdn.Id,
        Id = 201,
        StreetAddress = "Dai-Ichi Bldg. 6th Floor, 10-2 Nihombashi, 2-chome, Chuo-ku",
        CityProvincePostal = "Tokyo 103",
        Country = "Japan",
        CreatedAt = now.DateTime,
        UpdatedAt = now.DateTime,
        Sdn = sdn
      },
      new Address()
      {
        SdnId = sdn.Id,
        Id = 202,
        StreetAddress = "Federico Boyd Avenue & 51 Street",
        CityProvincePostal = "Panama City",
        Country = "Panama",
        CreatedAt = now.DateTime,
        UpdatedAt = now.DateTime,
        Sdn = sdn
      }
    });
  }

  [Fact]
  public async Task BuildAliasTableAsync_WhenCalledAndSdnRecordNotFound_ItShouldNotAddAliasCsvRecordsToDatabase()
  {
    using var testStream = CreateCsvStream(TestAliasCsv);

    _mockHttp
      .When(_ofacFileServiceOptions.GetAltNamesFileUri().ToString())
      .Respond("text/csv", testStream);

    await _databaseMaintainer.BuiltAliasTableAsync();

    var aliases = await _context.Set<Alias>().ToListAsync();

    aliases.Should().BeEmpty();
  }

  [Fact]
  public async Task BuildAliasTableAsync_WhenCalledAndSdnRecordFound_ItShouldAddAliasCsvRecordsToDatabase()
  {
    using var testStream = CreateCsvStream(TestAliasCsv);

    _mockHttp
      .When(_ofacFileServiceOptions.GetAltNamesFileUri().ToString())
      .Respond("text/csv", testStream);

    var now = DateTimeOffset.UtcNow;

    _timeProviderMock
      .Setup(x => x.GetUtcNow())
      .Returns(now);

    var sdn = new Sdn() { Id = 555 };
    await _context.Set<Sdn>().AddAsync(sdn);
    await _context.SaveChangesAsync();

    await _databaseMaintainer.BuiltAliasTableAsync();

    var aliases = await _context.Set<Alias>().ToListAsync();

    aliases.Should().BeEquivalentTo(new[]
    {
      new Alias()
      {
        SdnId = sdn.Id,
        Id = 477,
        Name = "COPROVA",
        Type = "aka",
        CreatedAt = now.DateTime,
        UpdatedAt = now.DateTime,
        Sdn = sdn
      },
      new Alias()
      {
        SdnId = sdn.Id,
        Id = 478,
        Name = "COPROVA SARL",
        Type = "aka",
        CreatedAt = now.DateTime,
        UpdatedAt = now.DateTime,
        Sdn = sdn
      }
    });
  }

  [Fact]
  public async Task BuildCommentTableAsync_WhenCalledAndSdnRecordNotFound_ItShouldNotAddCommentCsvRecordsToDatabase()
  {
    using var testStream = CreateCsvStream(TestCommentCsv);

    _mockHttp
      .When(_ofacFileServiceOptions.GetCommentsFileUri().ToString())
      .Respond("text/csv", testStream);

    await _databaseMaintainer.BuildCommentTableAsync();

    var comments = await _context.Set<Comment>().ToListAsync();

    comments.Should().BeEmpty();
  }

  [Fact]
  public async Task BuildCommentTableAsync_WhenCalledAndSdnRecordFound_ItShouldAddCommentCsvRecordsToDatabase()
  {
    using var testStream = CreateCsvStream(TestCommentCsv);

    _mockHttp
      .When(_ofacFileServiceOptions.GetCommentsFileUri().ToString())
      .Respond("text/csv", testStream);

    var now = DateTimeOffset.UtcNow;

    _timeProviderMock
      .Setup(x => x.GetUtcNow())
      .Returns(now);

    var sdn = new Sdn() { Id = 27307 };
    await _context.Set<Sdn>().AddAsync(sdn);
    await _context.SaveChangesAsync();

    await _databaseMaintainer.BuildCommentTableAsync();

    var comments = await _context.Set<Comment>().ToListAsync();

    comments.Should().BeEquivalentTo(new[]
    {
      new Comment()
      {
        SdnId = sdn.Id,
        Id = 1,
        Remarks = "G TEAM'; a.k.a. 'RED DOT'; a.k.a. 'TEMP.HERMIT'; a.k.a. 'GROUP 77'; a.k.a. 'ZINC'; a.k.a. 'APT-C-26'; a.k.a. 'APPLEWORM'.",
        CreatedAt = now.DateTime,
        UpdatedAt = now.DateTime,
        Sdn = sdn
      }
    });
  }

  public void Dispose()
  {
    _databaseMaintainer.Dispose();
    _mockHttp.Dispose();

    GC.SuppressFinalize(this);
  }
}