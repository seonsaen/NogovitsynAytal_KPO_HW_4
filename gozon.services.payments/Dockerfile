FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["gozon.services.payments/gozon.services.payments.csproj", "gozon.services.payments/"]
COPY ["gozon.shared/gozon.shared.csproj", "gozon.shared/"]
RUN dotnet restore "gozon.services.payments/gozon.services.payments.csproj"
COPY . .
WORKDIR "/src/gozon.services.payments"
RUN dotnet build "gozon.services.payments.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gozon.services.payments.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gozon.services.payments.dll"]