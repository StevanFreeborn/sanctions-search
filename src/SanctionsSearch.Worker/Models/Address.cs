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
    var address = new StringBuilder();

    if (string.IsNullOrWhiteSpace(StreetAddress) is false)
    {
      address.Append(StreetAddress);
    }

    if (string.IsNullOrWhiteSpace(CityProvincePostal) is false)
    {
      if (address.Length > 0)
      {
        address.Append(", ");
      }

      address.Append(CityProvincePostal);
    }

    if (string.IsNullOrWhiteSpace(Country) is false)
    {
      if (address.Length > 0)
      {
        address.Append(", ");
      }

      address.Append(Country);
    }

    return address.ToString();
  }
}