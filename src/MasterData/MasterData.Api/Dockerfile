FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/MasterData/MasterData.Api/MasterData.Api.csproj", "src/MasterData/MasterData.Api/"]
RUN dotnet restore "src/MasterData/MasterData.Api/MasterData.Api.csproj"
COPY . .
WORKDIR "/src/src/MasterData/MasterData.Api"
RUN dotnet build "MasterData.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MasterData.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MasterData.Api.dll"]
