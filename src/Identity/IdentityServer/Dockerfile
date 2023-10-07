ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:7.0
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/core/aspnet:7.0

FROM ${SDK_IMAGE} AS builder

WORKDIR /src

COPY ["src/BuildingBlocks/Shared/*.csproj", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/*.csproj", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/*.csproj", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/*.csproj", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/EfCore/*.csproj", "src/BuildingBlocks/EfCore/"]

COPY ["src/Identity/IdentityServer/*.csproj", "src/Identity/IdentityServer/"]
RUN dotnet restore src/Identity/IdentityServer/ /property:Configuration=Release -nowarn:msb3202,nu1503

COPY ["src/_schema/proto/promag/.", "src/_schema/proto/promag/"]
COPY ["src/BuildingBlocks/Shared/.", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/.", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/.", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/.", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/EfCore/.", "src/BuildingBlocks/EfCore/"]

COPY ["src/Identity/IdentityServer/.", "src/Identity/IdentityServer/"]

RUN dotnet publish src/Identity/IdentityServer/ -c Release -o /app --no-restore

FROM ${RUNTIME_IMAGE}
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Development

COPY --from=builder /app .
COPY ["wait-for-it.sh", "./"]

RUN ["chmod", "+x", "./wait-for-it.sh"]

ENTRYPOINT [ "dotnet",  "IdentityServer.dll"]

