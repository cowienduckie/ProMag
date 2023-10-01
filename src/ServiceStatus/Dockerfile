FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ServiceStatus/ServiceStatus.csproj", "src/ServiceStatus/"]
RUN dotnet restore "src/ServiceStatus/ServiceStatus.csproj"
COPY . .
WORKDIR "/src/src/ServiceStatus"
RUN dotnet build "ServiceStatus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceStatus.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceStatus.dll"]
