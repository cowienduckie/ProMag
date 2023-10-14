ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:7.0
ARG RUNTIME_IMAGE=mcr.microsoft.com/dotnet/aspnet:7.0

FROM ${SDK_IMAGE} AS builder

WORKDIR /src

COPY ["src/ServiceStatus/*.csproj", "src/ServiceStatus/"]

RUN dotnet restore src/ServiceStatus/ /property:Configuration=Release -nowarn:msb3202,nu1503

COPY ["src/ServiceStatus/.", "src/ServiceStatus/"]

RUN dotnet publish src/ServiceStatus/ -c Release -o /app --no-restore

FROM ${RUNTIME_IMAGE}
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Development

COPY --from=builder /app .

ENTRYPOINT [ "dotnet",  "Promag.ServiceStatus.dll"]

