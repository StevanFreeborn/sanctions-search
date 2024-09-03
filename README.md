# Sanctions Search

⚠️ **Under Construction** ⚠️

SanctionsSearch is an automated solution to perform sanctions search in Onspring against the Office of Foreign Assets Control (OFAC) SDN list. The app runs as a worker service that hosts two separate worker processes. The first worker process is responsible for fetching the latest SDN list from the OFAC website and updating a local database with the latest information. The second worker process is responsible for performing sanctions search against the local database using search criteria provided by Onspring and returning the search results to Onspring.

## Technologies

- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) (Language)
- [.NET](https://dotnet.microsoft.com/) (Runtime)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) (ORM)
- [SQLite](https://www.sqlite.org/index.html) (Database)
- [Serilog](https://serilog.net/) (Logging)
- [Docker](https://www.docker.com/) (Containerization)
- [xUnit](https://xunit.net/) (Testing)
- [Moq](https://github.com/devlooped/moq) (Mocking)
- [MockHttp](https://github.com/richardszalay/mockhttp) (Mocking)
- [FluentAssertions](https://fluentassertions.com/) (Assertions)
- [Faker.Net](https://keyizhang.com/Faker.Net/) (Faker)

## Onspring Configuration

TODO: Add Onspring configuration instructions.

## SanctionsSearch Configuration

The app handles configuration using environment variables or a `.env` file if running with Docker or using environment variables or a `appsettings.json` file if running without Docker. There are a number of configuration options many of which default values are provided, but the following are required in order for the app to run successfully:

- `OnspringOptions__ApiKey`
  - The API key for the Onspring API. Note this key needs to have all the necessary permissions to create, read, and update records in the Search Request and Search Result apps.
- `OnspringOptions__SearchRequestOptions__AppId`
- `OnspringOptions__SearchRequestOptions__NameFieldId`
- `OnspringOptions__SearchRequestOptions__StatusFieldId`
- `OnspringOptions__SearchRequestOptions__AwaitingProcessingStatusId`
- `OnspringOptions__SearchRequestOptions__ProcessingStatusId`
- `OnspringOptions__SearchRequestOptions__ProcessedSuccessStatusId`
- `OnspringOptions__SearchRequestOptions__ProcessedErrorStatusId`
- `OnspringOptions__SearchRequestOptions__ErrorFieldId`
- `OnspringOptions__SearchResultOptions__AppId`
- `OnspringOptions__SearchResultOptions__SearchRequestFieldId`
- `OnspringOptions__SearchResultOptions__NameFieldId`
- `OnspringOptions__SearchResultOptions__AddressFieldId`
- `OnspringOptions__SearchResultOptions__TypeFieldId`
- `OnspringOptions__SearchResultOptions__ProgramsFieldId`

> [!NOTE]
> You can find a sample `.env` file [here](./src/SanctionsSearch.Worker/example.env) and a sample `appsettings.json` file [here](./src/SanctionsSearch.Worker/appsettings.Example.json) that provide a list of all the configuration options.

## Usage

Currently I'm not providing a pre-built image or executable for the app. Regardless of the method you choose to run the app, you will need to clone the repository and build the app for your platform of choice.

```sh
git clone https://github.com/StevanFreeborn/sanctions-search.git
```

### With Docker

In order to run the app with Docker, you will need to have Docker installed on your machine. You can find Docker Desktop [here](https://www.docker.com/products/docker-desktop).

```sh
cd src/SanctionsSearch.Worker
docker build -t sanctions-search .
docker run -d --name sanctions-search --env-file .env sanctions-search
```

### Without Docker

In order to run the app without Docker, you will need to have the .NET SDK installed on your machine so that you can build and publish the app for your platform of choice. You can find the .NET 8 SDK [here](https://dotnet.microsoft.com/download/dotnet/8.0).

#### Windows Service

You can publish the app for Windows using the following command:

```pwsh
cd src/SanctionsSearch.Worker
dotnet publish -c Release -r win-x64 --self-contained -o dist/win
```

```pwsh
sc.exe create SanctionsSearchWorker binPath= "C:\path\to\dist\win\SanctionsSearch.Worker.exe"
```

#### Linux Service

You can publish the app for Linux using the following command:

```sh
cd src/SanctionsSearch.Worker
dotnet publish -c Release -r linux-x64 --self-contained -o dist/linux
```

You can then set up the app as a service on your Linux machine using Systemd. Note you will want to make sure that your configuration file is in the same directory as the app. Here is an example of a Systemd service file:

```txt
[Unit]
Description=Sanctions Search Worker Service

[Service]
Type=notify
ExecStart=/path/to/dist/linux/SanctionsSearch.Worker

[Install]
WantedBy=multi-user.target
```

#### Executable

Note you can also just run the app as an executable after publishing it for your platform using one of the commands above.
