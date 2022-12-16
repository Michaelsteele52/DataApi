FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY *.sln .
COPY src/DataApi/DataApi.csproj src/DataApi/
COPY src/DataApi.Integrations.WebClients/DataApi.Integrations.WebClients.csproj src/DataApi.Integrations.WebClients/

COPY test/DataApi.Test.Unit/DataApi.Test.Unit.csproj test/DataApi.Test.Unit/
COPY test/DataApi.Integrations.WebClients.Test.Unit/DataApi.Integrations.WebClients.Test.Unit.csproj test/DataApi.Integrations.WebClients.Test.Unit/

RUN dotnet restore
COPY . .

WORKDIR /source/src/DataApi.Integrations.WebClients
RUN dotnet build -c Release -o /app

WORKDIR /source/src/DataApi
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet build -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT [ "dotnet", "DataApi.dll"]