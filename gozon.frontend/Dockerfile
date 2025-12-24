FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["gozon.frontend/gozon.frontend.csproj", "gozon.frontend/"]
RUN dotnet restore "gozon.frontend/gozon.frontend.csproj"
COPY . .
WORKDIR "/src/gozon.frontend"
RUN dotnet build "gozon.frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gozon.frontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gozon.frontend.dll"]