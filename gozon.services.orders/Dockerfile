FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["gozon.services.orders/gozon.services.orders.csproj", "gozon.services.orders/"]
COPY ["gozon.shared/gozon.shared.csproj", "gozon.shared/"]
RUN dotnet restore "gozon.services.orders/gozon.services.orders.csproj"
COPY . .
WORKDIR "/src/gozon.services.orders"
RUN dotnet build "gozon.services.orders.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gozon.services.orders.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gozon.services.orders.dll"]