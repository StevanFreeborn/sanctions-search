namespace SanctionsSearch.Worker.Models;

class SearchRequest
{
  public int Id { get; init; }
  public string Name { get; set; } = string.Empty;
  public string Address { get; set; } = string.Empty;
  public string City { get; set; } = string.Empty;
  public string State { get; set; } = string.Empty;
  public string Zip { get; set; } = string.Empty;
  public string Country { get; set; } = string.Empty;

  public Expression<Func<Sdn, bool>> ToSdnFilter()
  {
    return sdn => EF.Functions.Like(sdn.Name, $"%{Name}%");
  }
}