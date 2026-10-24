# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY DecisionLens.Web/DecisionLens.Web.csproj DecisionLens.Web/
RUN dotnet restore DecisionLens.Web/DecisionLens.Web.csproj

COPY DecisionLens.Web/ DecisionLens.Web/
RUN dotnet publish DecisionLens.Web/DecisionLens.Web.csproj -c Release -o /app/publish --no-restore

# Run stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "DecisionLens.Web.dll"]