namespace SanctionsSearch.Worker.Interfaces;

interface IUnitOfWork
{
  ISdnRepository Sdns { get; }
  Task SaveChangesAsync();
}