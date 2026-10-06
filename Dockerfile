FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Restauration d'abord : cette couche est mise en cache tant que les .csproj ne changent pas
COPY src/Domain/Domain.csproj src/Domain/
COPY src/Application/Application.csproj src/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/WebAPI/WebAPI.csproj src/WebAPI/
RUN dotnet restore src/WebAPI/WebAPI.csproj

COPY src/ src/
RUN dotnet publish src/WebAPI/WebAPI.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "WebAPI.dll"]
