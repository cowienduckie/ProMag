ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:7.0
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/aspnet:7.0

FROM ${SDK_IMAGE} AS builder

WORKDIR /src

COPY ["src/BuildingBlocks/Shared/*.csproj", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/*.csproj", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.Vault/*.csproj", "src/BuildingBlocks/Configuration.Vault/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/*.csproj", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/GraphQl/*.csproj", "src/BuildingBlocks/GraphQl/"]

COPY ["src/Gateways/GraphQl.Gateway/*.csproj", "src/Gateways/GraphQl.Gateway/"]

RUN dotnet restore src/Gateways/GraphQl.Gateway/ /property:Configuration=Release -nowarn:msb3202,nu1503

COPY ["src/_schema/proto/promag/.", "src/_schema/proto/promag/"]
COPY ["src/BuildingBlocks/Shared/.", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/.", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.Vault/.", "src/BuildingBlocks/Configuration.Vault/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/.", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/GraphQl/.", "src/BuildingBlocks/GraphQl/"]

COPY ["src/Gateways/GraphQl.Gateway/.", "src/Gateways/GraphQl.Gateway/"]

RUN dotnet publish src/Gateways/GraphQl.Gateway/ -c Release -o /app --no-restore

FROM ${RUNTIME_IMAGE}
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Development

COPY --from=builder /app .

ENTRYPOINT [ "dotnet",  "GraphQl.Gateway.dll"]

