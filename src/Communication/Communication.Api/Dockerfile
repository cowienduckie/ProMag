ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:8.0
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/aspnet:8.0

# Step 1: Build
FROM ${SDK_IMAGE} AS build
WORKDIR /src

## Copy .csproj of projects in service
COPY ["src/Communication/Communication/Communication.csproj", "src/Communication/Communication/"]
COPY ["src/Communication/Communication.Api/Communication.Api.csproj", "src/Communication/Communication.Api/"]
COPY ["src/Communication/Communication.FunctionalTests/Communication.FunctionalTests.csproj", "src/Communication/Communication.FunctionalTests/"]

## Copy .csproj of projects in dependencies
COPY ["src/BuildingBlocks/Configuration.MassTransit/Configuration.MassTransit.csproj", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/Configuration.OpenTelemetry.csproj", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Shared/Shared.csproj", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/Configuration.Metrics.csproj", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/Configuration.Vault/Configuration.Vault.csproj", "src/BuildingBlocks/Configuration.Vault/"]
COPY ["src/BuildingBlocks/Email.MailKit/Email.MailKit.csproj", "src/BuildingBlocks/Email.MailKit/"]

## Restore dependencies and packages
COPY ["nuget.config", "./"]

RUN dotnet restore "src/Communication/Communication.Api/Communication.Api.csproj" /p:Configuration=Release --configfile nuget.config
RUN dotnet restore "src/Communication/Communication.FunctionalTests/Communication.FunctionalTests.csproj" /p:Configuration=Release --configfile nuget.config

## Copy all files
COPY ["src/_schema/proto/promag/.", "src/_schema/proto/promag/"]
COPY ["src/BuildingBlocks/Shared/.", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/.", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/.", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/Configuration.Vault/.", "src/BuildingBlocks/Configuration.Vault/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/.", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/Email.MailKit/.", "src/BuildingBlocks/Email.MailKit/"]

COPY ["src/Communication/Communication.Api/.", "src/Communication/Communication.Api/"]
COPY ["src/Communication/Communication/.", "src/Communication/Communication/"]
COPY ["src/Communication/Communication.FunctionalTests/.", "src/Communication/Communication.FunctionalTests/"]

# Step 2: Publish
FROM build AS publish
WORKDIR "/src/src/Communication/Communication.Api"
RUN dotnet publish "Communication.Api.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Step 3: Functional Testing
FROM build AS functionaltest
WORKDIR "/src/src/Communication/Communication.FunctionalTests"

# Step 4: Runtime
FROM ${RUNTIME_IMAGE} AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Development

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Communication.Api.dll"]