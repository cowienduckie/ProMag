ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:7.0
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/aspnet:7.0

FROM ${SDK_IMAGE} AS builder

WORKDIR /src

COPY ["src/BuildingBlocks/Shared/*.csproj", "src/BuildingBlocks/Shared/"]
COPY ["src/BuildingBlocks/Configuration.OpenTelemetry/*.csproj", "src/BuildingBlocks/Configuration.OpenTelemetry/"]
COPY ["src/BuildingBlocks/Configuration.MassTransit/*.csproj", "src/BuildingBlocks/Configuration.MassTransit/"]
COPY ["src/BuildingBlocks/Configuration.Vault/*.csproj", "src/BuildingBlocks/Configuration.Vault/"]
COPY ["src/BuildingBlocks/Configuration.Metrics/*.csproj", "src/BuildingBlocks/Configuration.Metrics/"]
COPY ["src/BuildingBlocks/Email.MailKit/*.csproj", "src/BuildingBlocks/Email.MailKit/"]

COPY ["src/Communication/Communication.Api/*.csproj", "src/Communication/Communication.Api/"]
COPY ["src/Communication/Communication/*.csproj", "src/Communication/Communication/"]
COPY ["src/Communication/Communication.FunctionalTests/*.csproj", "src/Communication/Communication.FunctionalTests/"]

RUN dotnet restore src/Communication/Communication.Api/ /property:Configuration=Release -nowarn:msb3202,nu1503
RUN dotnet restore src/Communication/Communication.FunctionalTests/ /property:Configuration=Release -nowarn:msb3202,nu1503

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

RUN dotnet publish src/Communication/Communication.Api/ -c Release -o /app --no-restore

FROM builder AS functionaltest
WORKDIR /src/src/Communication/Communication.FunctionalTests

FROM ${RUNTIME_IMAGE}
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Development

COPY --from=builder /app .

ENTRYPOINT [ "dotnet",  "Communication.Api.dll"]
