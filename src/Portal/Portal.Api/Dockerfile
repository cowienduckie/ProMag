ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:7.0
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/aspnet:7.0

# Step 1: Build
FROM ${SDK_IMAGE} AS build
WORKDIR /src

## Copy .csproj of projects in service
COPY ["src/Portal/Portal.Api/*.csproj", "src/Portal/Portal.Api/"]
COPY ["src/Portal/Portal/*.csproj", "src/Portal/Portal/"]
COPY ["src/Portal/Portal.FunctionalTests/*.csproj", "src/Portal/Portal.FunctionalTests/"]

## Copy .csproj of projects in dependencies
COPY ["src/BuildingBlocks/Shared/*.csproj", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/*.csproj", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/*.csproj", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/*.csproj", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/Configuration.Vault/*.csproj", "src/BuildingBlocks/Configuration.Vault/"]
COPY ["src/BuildingBlocks/GraphQl/*.csproj", "src/BuildingBlocks/GraphQl/"]
COPY ["src/BuildingBlocks/EfCore/*.csproj", "src/BuildingBlocks/EfCore/"]

## Restore dependencies and packages
COPY ["nuget.config", "./"]

RUN dotnet restore "src/Portal/Portal.Api/Portal.Api.csproj" /p:Configuration=Release --configfile nuget.config
RUN dotnet restore "src/Portal/Portal.FunctionalTests/Portal.FunctionalTests.csproj" /p:Configuration=Release --configfile nuget.config

## Copy all files
COPY ["src/_schema/proto/promag/.", "src/_schema/proto/promag/"]

COPY ["src/BuildingBlocks/Shared/.", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/.", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/.", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/.", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/Configuration.Vault/.", "src/BuildingBlocks/Configuration.Vault/"]
COPY ["src/BuildingBlocks/GraphQl/.", "src/BuildingBlocks/GraphQl/"]
COPY ["src/BuildingBlocks/EfCore/.", "src/BuildingBlocks/EfCore/"]

COPY ["src/Portal/Portal.Api/.", "src/Portal/Portal.Api/"]
COPY ["src/Portal/Portal/.", "src/Portal/Portal/"]
COPY ["src/Portal/Portal.FunctionalTests/.", "src/Portal/Portal.FunctionalTests/"]

# Step 2: Publish
FROM build AS publish
RUN dotnet publish src/Portal/Portal.Api/ -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Step 3: Functional Testing
FROM build AS functionaltest
WORKDIR "/src/src/Portal/Portal.FunctionalTests"

# Step 4: Runtime
FROM ${RUNTIME_IMAGE} AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Development

COPY --from=publish /app/publish .
COPY ["wait-for-it.sh", "./"]

RUN ["chmod", "+x", "./wait-for-it.sh"]

ENTRYPOINT [ "dotnet",  "Portal.Api.dll"]