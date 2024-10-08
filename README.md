# Sanctions Search

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
- [Faker.Net](https://keyizhang.com/Faker.Net/) (Generate Fake Test Data)

## Onspring Configuration

This app is only useful if you have access to an Onspring instance and have configured the necessary apps and fields to work with it. In Onspring you should have two apps, one for Search Request and one for Search Result. The Search Request app acts as a queue for search requests that need to be processed. The Search Result app stores individual search results for each search request. The search results will be related back to the search request that generated them using a reference field.

The Search Request app should have the following fields:

- Name (Text)
- Status (Single Select List)
  - It should have the following values:
    - A value indicating the request is awaiting processing.
    - A value indicating the request is processing.
    - A value indicating the request was processed successfully.
    - A value indicating the request was processed with an error.
- Error (Text)

The Search Result app should have the following fields:

- Search Request (Reference)
- Name (Text)
- Address (Text)
- Type (Single Select List)
- Programs (Multi Select List)

## SanctionsSearch Configuration

The app handles configuration using environment variables or a `.env` file if running with Docker or using environment variables or a `appsettings.json` file if running without Docker. There are a number of configuration options many of which default values are provided, but the following are required in order for the app to run successfully:

- `OnspringOptions__ApiKey`
  - The API key for the Onspring API. Note this key needs to have all the necessary permissions to create, read, and update records in the Search Request and Search Result apps.
- `OnspringOptions__SearchRequestOptions__AppId`
  - The app ID for the Search Request app in Onspring.
- `OnspringOptions__SearchRequestOptions__NameFieldId`
  - The field ID for the Name field in the Search Request app in Onspring.
- `OnspringOptions__SearchRequestOptions__StatusFieldId`
  - The field ID for the Status field in the Search Request app in Onspring.
- `OnspringOptions__SearchRequestOptions__AwaitingProcessingStatusId`
  - The list value ID for the awaiting processing value for the Status field in the Search Request app in Onspring.
- `OnspringOptions__SearchRequestOptions__ProcessingStatusId`
  - The list value ID for the processing value for the Status field in the Search Request app in Onspring.
- `OnspringOptions__SearchRequestOptions__ProcessedSuccessStatusId`
  - The list value ID for the processed success value for the Status field in the Search Request app in Onspring.
- `OnspringOptions__SearchRequestOptions__ProcessedErrorStatusId`
  - The list value ID for the processed error value for the Status field in the Search Request app in Onspring.
- `OnspringOptions__SearchRequestOptions__ErrorFieldId`
  - The field ID for the Error field in the Search Request app in Onspring.
- `OnspringOptions__SearchResultOptions__AppId`
  - The app ID for the Search Result app in Onspring.
- `OnspringOptions__SearchResultOptions__SearchRequestFieldId`
  - The field ID for the Search Request field in the Search Result app in Onspring.
- `OnspringOptions__SearchResultOptions__NameFieldId`
  - The field ID for the Name field in the Search Result app in Onspring.
- `OnspringOptions__SearchResultOptions__AddressFieldId`
  - The field ID for the Address field in the Search Result app in Onspring.
- `OnspringOptions__SearchResultOptions__TypeFieldId`
  - The field ID for the Type field in the Search Result app in Onspring.
- `OnspringOptions__SearchResultOptions__ProgramsFieldId`
  - The field ID for the Programs field in the Search Result app in Onspring.

> [!NOTE]
> You can find a sample `.env` file [here](./src/SanctionsSearch.Worker/example.env) and a sample `appsettings.json` file [here](./src/SanctionsSearch.Worker/appsettings.Example.json) that provide a list of all the configuration options.

## Usage

Because I intend this to be primarily a proof of concept app for how someone can further augment, extend, and leverage the flexibility and extensibility of Onspring to automate business processes I am currently NOT providing a pre-built image or executable for the app. It will be up to you to build and publish it for your platform then determine how to host and maintain it. You are also welcome to further modify the app to better suit your needs.

Regardless of the method you choose to run the app, you will need to clone the repository and build the app for your platform of choice.

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

## OFAC Data

The app uses the OFAC SDN list as the source of sanctions data. The app will download the latest SDN list from the OFAC website and update a local SQLite database with the latest information. The app will then use this local database to perform sanctions search against the data. If you are running the app with Docker and want to persist the local database you will need to mount a volume to the `Data` directory in the container.

```sh
docker run -d --name sanctions-search --env-file .env -v /path/to/data:/app/Data sanctions-search
```

## Logging

The app uses Serilog for logging. By default, the app logs to the console and to a file in the `Logs` directory. If you are running the app with Docker and want to persist the logs you will need to mount a volume to the `Logs` directory in the container.

```sh
docker run -d --name sanctions-search --env-file .env -v /path/to/logs:/app/Logs sanctions-search
```
