FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/DataApi/DataApi.csproj", "DataApi/"]
COPY ["src/DataApi.Integrations.WebClients/DataApi.Integrations.WebClients.csproj", "DataApi.Integrations.WebClients/"]
RUN dotnet restore "DataApi/DataApi.csproj"
COPY . .
WORKDIR "/src/DataApi"
RUN dotnet build "DataApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataApi.dll", "--environment = Development"]