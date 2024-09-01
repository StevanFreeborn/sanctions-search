namespace SanctionsSearch.Worker.Models;

class Address : Entity
{
  public int SdnId { get; set; }
  public string StreetAddress { get; set; } = string.Empty;
  public string CityProvincePostal { get; set; } = string.Empty;
  public string Country { get; set; } = string.Empty;
  public string Remarks { get; set; } = string.Empty;

  public virtual Sdn Sdn { get; set; } = default!;

  public override string ToString()
  {
    return $"{StreetAddress}, {CityProvincePostal}, {Country}";
  }
}