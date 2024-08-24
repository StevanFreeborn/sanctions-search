namespace SanctionsSearch.Worker.Models;

class DatabaseMaintainer(
  IUnitOfWork unitOfWork,
  IOfacFileService ofacFileService,
  ILogger<DatabaseMaintainer> logger
) : IDatabaseMaintainer, IDisposable
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IOfacFileService _ofacFileService = ofacFileService;
  private readonly ILogger<DatabaseMaintainer> _logger = logger;
  private readonly CsvConfiguration _csvConfig = new(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
  private readonly List<CsvReader> _csvReaders = [];
  private readonly List<StreamReader> _streamReaders = [];

  private IEnumerable<T> GetRecordsFromStream<T>(Stream stream)
  {
    var reader = new StreamReader(stream);
    var csv = new CsvReader(reader, _csvConfig);

    csv.Context.RegisterClassMap<SdnMap>();
    csv.Context.RegisterClassMap<AddressMap>();

    _streamReaders.Add(reader);
    _csvReaders.Add(csv);

    return csv.GetRecords<T>();
  }

  public async Task BuildSdnTableAsync()
  {
    var result = await _ofacFileService.GetSdnFileAsync();

    if (result.IsFailed)
    {
      _logger.LogError("Failed to get SDN file from OFAC.");
      return;
    }

    using var stream = result.Value;
    var records = GetRecordsFromStream<Sdn>(stream);

    foreach (var record in records)
    {
      await _unitOfWork.Sdns.Upsert(record);
    }

    await _unitOfWork.SaveChangesAsync();
  }

  public async Task BuildAddressTableAsync()
  {
    var result = await _ofacFileService.GetAddressFileAsync();

    if (result.IsFailed)
    {
      _logger.LogError("Failed to get Address file from OFAC.");
      return;
    }

    using var stream = result.Value;
    var records = GetRecordsFromStream<Address>(stream);

    foreach (var record in records)
    {
      var sdn = await _unitOfWork.Sdns.Find(s => s.Id == record.SdnId);

      if (sdn.Count() is 0)
      {
        _logger.LogWarning("Address's SDN with ID {Id} not found. Skipping address.", record.SdnId);
        continue;
      }

      await _unitOfWork.Addresses.Upsert(record);
    }

    await _unitOfWork.SaveChangesAsync();
  }

  public Task BuildCommentTableAsync()
  {
    throw new NotImplementedException();
  }

  public Task BuiltAliasTableAsync()
  {
    throw new NotImplementedException();
  }

  public void Dispose()
  {
    _csvReaders.ForEach(csv => csv.Dispose());
    _streamReaders.ForEach(reader => reader.Dispose());
  }
}