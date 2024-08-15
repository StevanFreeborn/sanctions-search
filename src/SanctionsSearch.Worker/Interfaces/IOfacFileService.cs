using FluentResults;

namespace SanctionsSearch.Worker.Interfaces;

interface IOfacFileService
{
  Task<Result<Stream>> GetSdnFileAsync();
  Task<Result<Stream>> GetAddressFileAsync();
  Task<Result<Stream>> GetAltNamesFileAsync();
  Task<Result<Stream>> GetCommentsFileAsync();
  Task<Result<Stream>> GetConPrimaryNameFileAsync();
  Task<Result<Stream>> GetConAddressesFileAsync();
  Task<Result<Stream>> GetConAltNamesFileAsync();
  Task<Result<Stream>> GetConCommentsFileAsync();
}
