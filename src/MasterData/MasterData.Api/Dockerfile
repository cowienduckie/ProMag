ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:7.0
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/aspnet:7.0

FROM ${SDK_IMAGE} AS builder

WORKDIR /src

COPY ["src/BuildingBlocks/Shared/*.csproj", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/*.csproj", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/*.csproj", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/*.csproj", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/GraphQl/*.csproj", "src/BuildingBlocks/GraphQl/"]
COPY ["src/BuildingBlocks/EfCore/*.csproj", "src/BuildingBlocks/EfCore/"]

COPY ["src/MasterData/MasterData.Api/*.csproj", "src/MasterData/MasterData.Api/"]
COPY ["src/MasterData/MasterData/*.csproj", "src/MasterData/MasterData/"]

RUN dotnet restore src/MasterData/MasterData.Api/ /property:Configuration=Release -nowarn:msb3202,nu1503

COPY ["src/_schema/proto/promag/.", "src/_schema/proto/promag/"]

COPY ["src/BuildingBlocks/Shared/.", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/.", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/.", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/.", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/GraphQl/.", "src/BuildingBlocks/GraphQl/"]
COPY ["src/BuildingBlocks/EfCore/.", "src/BuildingBlocks/EfCore/"]

COPY ["src/MasterData/MasterData.Api/.", "src/MasterData/MasterData.Api/"]
COPY ["src/MasterData/MasterData/.", "src/MasterData/MasterData/"]

RUN dotnet publish src/MasterData/MasterData.Api/ -c Release -o /app --no-restore

FROM ${RUNTIME_IMAGE}
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Development

COPY --from=builder /app .
COPY ["wait-for-it.sh", "./"]
RUN chmod +x wait-for-it.sh

RUN ["chmod", "+x", "./wait-for-it.sh"]

ENTRYPOINT [ "dotnet",  "MasterData.Api.dll"]