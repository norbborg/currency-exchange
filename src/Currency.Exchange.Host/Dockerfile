FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Currency.Exchange.Host/Currency.Exchange.Host.csproj", "Currency.Exchange.Host/"]
RUN dotnet restore "Currency.Exchange.Host/Currency.Exchange.Host.csproj"
COPY . .
WORKDIR "/src/src/Currency.Exchange.Host"
RUN dotnet build "Currency.Exchange.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Currency.Exchange.Host.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Currency.Exchange.Host.dll"]