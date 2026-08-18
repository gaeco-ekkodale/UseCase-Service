FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Api/UseCaseService.Api.csproj", "Api/"]
COPY ["Events/UseCaseService.Events.csproj", "Events/"]
COPY ["Domain/UseCaseService.Domain.csproj", "Domain/"]
COPY ["Infrastructure/UseCaseService.Infrastructure.csproj", "Infrastructure/"]

RUN --mount=type=secret,id=nugetconfig \
    dotnet restore "Api/UseCaseService.Api.csproj" --configfile /run/secrets/nugetconfig

COPY . .

WORKDIR "/src/Api"
RUN dotnet build "UseCaseService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UseCaseService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UseCaseService.Api.dll"]
