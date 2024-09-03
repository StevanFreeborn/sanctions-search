FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . .

FROM base AS build-stage
RUN dotnet publish -c Release -o dist

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build-stage /app/dist ./
ENTRYPOINT ["dotnet", "SanctionsSearch.Worker.dll"]