namespace SanctionsSearch.Worker.Models;

class DatabaseMaintainer(
  IUnitOfWork unitOfWork,
  IOfacFileService ofacFileService,
  ILogger<DatabaseMaintainer> logger
) : IDatabaseMaintainer
{
  private readonly IUnitOfWork _unitOfWork = unitOfWork;
  private readonly IOfacFileService _ofacFileService = ofacFileService;
  private readonly ILogger<DatabaseMaintainer> _logger = logger;

  public async Task BuildSdnTableAsync()
  {
    var result = await _ofacFileService.GetSdnFileAsync();

    if (result.IsFailed)
    {
      _logger.LogError("Failed to get SDN file from OFAC.");
      return;
    }

    using var stream = result.Value;
    using var reader = new StreamReader(stream);
    var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false };
    using var csv = new CsvReader(reader, config);
    csv.Context.RegisterClassMap<SdnMap>();
    var records = csv.GetRecords<Sdn>();

    foreach (var record in records)
    {
      await _unitOfWork.Sdns.Upsert(record);
    }

    await _unitOfWork.SaveChangesAsync();
  }

  public Task BuildAddressTableAsync()
  {
    throw new NotImplementedException();
  }

  public Task BuildCommentTableAsync()
  {
    throw new NotImplementedException();
  }

  public Task BuiltAliasTableAsync()
  {
    throw new NotImplementedException();
  }
}