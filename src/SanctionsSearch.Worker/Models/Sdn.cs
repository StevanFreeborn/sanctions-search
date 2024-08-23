namespace SanctionsSearch.Worker.Models;

class Sdn : Entity
{
  public string Name { get; set; } = string.Empty;
  public string Type { get; set; } = string.Empty;
  public string Program { get; set; } = string.Empty;
  public string Title { get; set; } = string.Empty;
  public string CallSign { get; set; } = string.Empty;
  public string VesselType { get; set; } = string.Empty;
  public string Tonnage { get; set; } = string.Empty;
  public string GrossRegisteredTonnage { get; set; } = string.Empty;
  public string VesselFlag { get; set; } = string.Empty;
  public string VesselOwner { get; set; } = string.Empty;
  public string Remarks { get; set; } = string.Empty;

  public virtual ICollection<Address> Addresses { get; set; } = [];
  public virtual ICollection<Alias> Aliases { get; set; } = [];
  public virtual ICollection<Comment> Comments { get; set; } = [];
}