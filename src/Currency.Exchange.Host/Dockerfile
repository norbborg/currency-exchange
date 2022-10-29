#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["currency.exchange.host/currency.exchange.host.csproj", "currency.exchange.host/"]
RUN dotnet restore "currency.exchange.host/currency.exchange.host.csproj"
COPY . .
WORKDIR "/src/currency.exchange.host"
RUN dotnet build "currency.exchange.host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "currency.exchange.host.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "currency.exchange.host.dll"]