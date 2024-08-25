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

  private readonly List<CsvReader> _csvReaders = [];
  private readonly List<StreamReader> _streamReaders = [];

  private bool HandleReadingException(ReadingExceptionOccurredArgs args)
  {
    _logger.LogError(args.Exception, "Error reading CSV record at row {Row}", args.Exception.Context?.Reader?.Parser.Row);
    return false;
  }

  private IEnumerable<T> GetRecordsFromStream<T>(Stream stream)
  {
    var reader = new StreamReader(stream);
    var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
    {
      HasHeaderRecord = false,
      ReadingExceptionOccurred = HandleReadingException
    });

    csv.Context.RegisterClassMap<SdnMap>();
    csv.Context.RegisterClassMap<AddressMap>();
    csv.Context.RegisterClassMap<AliasMap>();
    csv.Context.RegisterClassMap<CommentMap>();

    _streamReaders.Add(reader);
    _csvReaders.Add(csv);

    return csv.GetRecords<T>();
  }

  public async Task BuildSdnTableAsync()
  {
    _logger.LogInformation("Building SDN table");

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
      _logger.LogDebug("Upserting SDN record with ID {Id}", record.Id);
      await _unitOfWork.Sdns.Upsert(record);
    }

    await _unitOfWork.SaveChangesAsync();

    _logger.LogInformation("SDN table built");
  }

  public async Task BuildAddressTableAsync()
  {
    _logger.LogInformation("Building Address table");

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

      _logger.LogDebug("Upserting Address record with ID {Id}", record.Id);
      await _unitOfWork.Addresses.Upsert(record);
    }

    await _unitOfWork.SaveChangesAsync();

    _logger.LogInformation("Address table built");
  }

  public async Task BuiltAliasTableAsync()
  {
    _logger.LogInformation("Building Alias table");

    var result = await _ofacFileService.GetAltNamesFileAsync();

    if (result.IsFailed)
    {
      _logger.LogError("Failed to get Alias file from OFAC.");
      return;
    }

    using var stream = result.Value;
    var records = GetRecordsFromStream<Alias>(stream);

    foreach (var record in records)
    {
      var sdn = await _unitOfWork.Sdns.Find(s => s.Id == record.SdnId);

      if (sdn.Count() is 0)
      {
        _logger.LogWarning("Alias's SDN with ID {Id} not found. Skipping alias.", record.SdnId);
        continue;
      }

      _logger.LogDebug("Upserting Alias record with ID {Id}", record.Id);
      await _unitOfWork.Aliases.Upsert(record);
    }

    await _unitOfWork.SaveChangesAsync();

    _logger.LogInformation("Alias table built");
  }

  public async Task BuildCommentTableAsync()
  {
    _logger.LogInformation("Building Comment table");

    var result = await _ofacFileService.GetCommentsFileAsync();

    if (result.IsFailed)
    {
      _logger.LogError("Failed to get Comment file from OFAC.");
      return;
    }

    using var stream = result.Value;
    var records = GetRecordsFromStream<Comment>(stream);

    foreach (var record in records)
    {
      var sdn = await _unitOfWork.Sdns.Find(s => s.Id == record.SdnId);

      if (sdn.Count() is 0)
      {
        _logger.LogWarning("Comment's SDN with ID {Id} not found. Skipping comment.", record.SdnId);
        continue;
      }

      _logger.LogDebug("Upserting Comment record with ID {Id}", record.Id);
      await _unitOfWork.Comments.Upsert(record);
    }

    await _unitOfWork.SaveChangesAsync();

    _logger.LogInformation("Comment table built");
  }

  public async Task CleanupTablesAsync()
  {
    _logger.LogInformation("Cleaning up tables");

    var result = await _ofacFileService.GetSdnFileAsync();

    if (result.IsFailed)
    {
      _logger.LogError("Failed to get SDN file from OFAC.");
      return;
    }

    using var stream = result.Value;
    var records = GetRecordsFromStream<Sdn>(stream);
    var sdnIds = records.Select(s => s.Id).ToList();

    await _unitOfWork.Sdns.DeleteWhereAsync(s => sdnIds.Contains(s.Id) == false);

    _logger.LogInformation("Tables cleaned up");
  }

  public void Dispose()
  {
    _csvReaders.ForEach(csv => csv.Dispose());
    _streamReaders.ForEach(reader => reader.Dispose());
  }
}