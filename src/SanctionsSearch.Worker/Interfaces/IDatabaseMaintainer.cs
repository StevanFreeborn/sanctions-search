namespace SanctionsSearch.Worker.Interfaces;

interface IDatabaseMaintainer
{
  Task BuildSdnTableAsync();
  Task BuildAddressTableAsync();
  Task BuiltAliasTableAsync();
  Task BuildCommentTableAsync();
}