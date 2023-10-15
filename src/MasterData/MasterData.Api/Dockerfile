ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:7.0
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/aspnet:7.0

# Step 1: Build
FROM ${SDK_IMAGE} AS build
WORKDIR /src

## Copy .csproj of projects in service
COPY ["src/MasterData/MasterData.Api/MasterData.Api.csproj", "src/MasterData/MasterData.Api/"]
COPY ["src/MasterData/MasterData/MasterData.csproj", "src/MasterData/MasterData/"]

## Copy .csproj of projects in dependencies
COPY ["src/BuildingBlocks/Shared/*.csproj", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/*.csproj", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/*.csproj", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/*.csproj", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/GraphQl/*.csproj", "src/BuildingBlocks/GraphQl/"]
COPY ["src/BuildingBlocks/EfCore/*.csproj", "src/BuildingBlocks/EfCore/"]

## Restore dependencies and packages
RUN dotnet restore "src/MasterData/MasterData.Api/MasterData.Api.csproj" /p:Configuration=Release

## Copy all files
COPY ["src/_schema/proto/promag/.", "src/_schema/proto/promag/"]

COPY ["src/BuildingBlocks/Shared/.", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/.", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/.", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/.", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/GraphQl/.", "src/BuildingBlocks/GraphQl/"]
COPY ["src/BuildingBlocks/EfCore/.", "src/BuildingBlocks/EfCore/"]

COPY ["src/MasterData/MasterData.Api/.", "src/MasterData/MasterData.Api/"]
COPY ["src/MasterData/MasterData/.", "src/MasterData/MasterData/"]

## Build API
RUN dotnet build src/MasterData/MasterData.Api/ -c Release -o /app/build

# Step 2: Publish
FROM build AS publish
RUN dotnet publish src/MasterData/MasterData.Api/ -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Step 3: Runtime
FROM ${RUNTIME_IMAGE} AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Development

COPY --from=publish /app/publish .
COPY ["wait-for-it.sh", "./"]

RUN ["chmod", "+x", "./wait-for-it.sh"]

ENTRYPOINT [ "dotnet",  "MasterData.Api.dll"]