ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:7.0
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/aspnet:7.0

# Step 1: Build
FROM ${SDK_IMAGE} AS build
WORKDIR /src

## Copy .csproj of projects in service
COPY ["src/Identity/IdentityServer/*.csproj", "src/Identity/IdentityServer/"]

## Copy .csproj of projects in dependencies
COPY ["src/BuildingBlocks/Shared/*.csproj", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/*.csproj", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/*.csproj", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/*.csproj", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/EfCore/*.csproj", "src/BuildingBlocks/EfCore/"]

## Restore dependencies and packages
RUN dotnet restore "src/Identity/IdentityServer/IdentityServer.csproj" /p:Configuration=Release

## Copy all files
COPY ["src/_schema/proto/promag/.", "src/_schema/proto/promag/"]
COPY ["src/BuildingBlocks/Shared/.", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/.", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/.", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/.", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/EfCore/.", "src/BuildingBlocks/EfCore/"]

COPY ["src/Identity/IdentityServer/.", "src/Identity/IdentityServer/"]

## Build API
WORKDIR "/src/src/Identity/IdentityServer/"
RUN dotnet build "IdentityServer.csproj" -c Release -o /app/build

# Step 2: Publish
FROM build AS publish
WORKDIR "/src/src/Identity/IdentityServer"
RUN dotnet publish "IdentityServer.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Step 3: Runtime
FROM ${RUNTIME_IMAGE} AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Development

COPY --from=publish /app/publish .
COPY ["wait-for-it.sh", "./"]

RUN ["chmod", "+x", "./wait-for-it.sh"]

ENTRYPOINT [ "dotnet",  "IdentityServer.dll"]
