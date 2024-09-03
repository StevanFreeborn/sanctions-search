# Sanctions Search

⚠️ **Under Construction** ⚠️

SanctionsSearch is an automated solution to perform sanctions search in Onspring against the Office of Foreign Assets Control (OFAC) SDN list. The app runs as a worker service that hosts two separate worker processes. The first worker process is responsible for fetching the latest SDN list from the OFAC website and updating a local database with the latest information. The second worker process is responsible for performing sanctions search against the local database using search criteria provided by Onspring and returning the search results to Onspring.

## Configuration

The app handles configuration using a `.env` file if running with Docker or using the `appsettings.json` file if running without Docker. There are a number of configuration options many of which default values are provided, but the following are required in order for the app to run successfully:

TODO: Add required configuration options

> [!NOTE]
> You can find a sample `.env` file [here](./src/SanctionsSearch.Worker/example.env) and a sample `appsettings.json` file [here](./src/SanctionsSearch.Worker/appsettings.Example.json) that provide a list of all the configuration options.

## Usage

TODO: Add usage instructions

### With Docker

TODO: Add Docker usage instructions

### Without Docker

TODO: Add usage instructions

## Technologies

- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [.NET](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [SQLite](https://www.sqlite.org/index.html)
- [Serilog](https://serilog.net/)
- [Docker](https://www.docker.com/)
- [xUnit](https://xunit.net/)
- [Moq](https://github.com/devlooped/moq)
- [FluentAssertions](https://fluentassertions.com/)
- [Faker.Net](https://keyizhang.com/Faker.Net/)
