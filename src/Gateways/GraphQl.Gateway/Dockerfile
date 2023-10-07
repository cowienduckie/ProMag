FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Gateways/GraphQl.Gateway/GraphQl.Gateway.csproj", "src/Gateways/GraphQl.Gateway/"]
RUN dotnet restore "src/Gateways/GraphQl.Gateway/GraphQl.Gateway.csproj"
COPY . .
WORKDIR "/src/src/Gateways/GraphQl.Gateway"
RUN dotnet build "GraphQl.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GraphQl.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GraphQl.Gateway.dll"]
