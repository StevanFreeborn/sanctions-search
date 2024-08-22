namespace SanctionsSearch.Worker.Interfaces;

interface IUnitOfWork
{
  ISdnRepository Sdns { get; }
  IAddressRepository Addresses { get; }
  IAliasRepository Aliases { get; }
  ICommentRepository Comments { get; }
  Task SaveChangesAsync();
}