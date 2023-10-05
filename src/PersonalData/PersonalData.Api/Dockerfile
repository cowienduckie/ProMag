FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/PersonalData/PersonalData.Api/PersonalData.Api.csproj", "src/PersonalData/PersonalData.Api/"]
RUN dotnet restore "src/PersonalData/PersonalData.Api/PersonalData.Api.csproj"
COPY . .
WORKDIR "/src/src/PersonalData/PersonalData.Api"
RUN dotnet build "PersonalData.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PersonalData.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PersonalData.Api.dll"]
