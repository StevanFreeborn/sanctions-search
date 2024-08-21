namespace SanctionsSearch.Worker.Models;

class Comment : Entity
{
  public int SdnId { get; set; }
  public string Remarks { get; set; } = string.Empty;

  public virtual Sdn Sdn { get; set; } = default!;
}