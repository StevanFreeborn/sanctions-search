global using System.Net;
global using System.Text;

global using Bogus;

global using FluentAssertions;

global using Microsoft.Data.Sqlite;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;

global using Moq;

global using Onspring.API.SDK.Models;

global using RichardSzalay.MockHttp;

global using SanctionsSearch.Worker.Extensions;
global using SanctionsSearch.Worker.Interfaces;
global using SanctionsSearch.Worker.Models;
global using SanctionsSearch.Worker.Options;
global using SanctionsSearch.Worker.Persistence;
global using SanctionsSearch.Worker.Services;
global using SanctionsSearch.Worker.Tests.Faker;