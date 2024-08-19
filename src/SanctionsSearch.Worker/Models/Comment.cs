namespace SanctionsSearch.Worker.Models;

class Comment
{
  public int SdnId { get; set; }
  public int Id { get; set; }
  public string Remarks { get; set; } = string.Empty;

  public virtual Sdn Sdn { get; set; } = default!;
}