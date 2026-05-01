FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY LinguaQuest.Web.csproj ./
RUN dotnet restore LinguaQuest.Web.csproj

COPY . .
RUN dotnet publish LinguaQuest.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

CMD ["sh", "-c", "dotnet LinguaQuest.Web.dll --urls http://0.0.0.0:${PORT:-10000}"]
