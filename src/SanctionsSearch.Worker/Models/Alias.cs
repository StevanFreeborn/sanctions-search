namespace SanctionsSearch.Worker.Models;

class Alias : Entity
{
  public int SdnId { get; set; }
  public string Type { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string Remarks { get; set; } = string.Empty;

  public virtual Sdn Sdn { get; set; } = default!;
}