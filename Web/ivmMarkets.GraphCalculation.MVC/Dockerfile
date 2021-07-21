#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "ivmMarketsGraphCalculation.sln" "ivmMarketsGraphCalculation.sln"

COPY "BuildingBlocks/EventBus/EventBus/EventBus.csproj" "BuildingBlocks/EventBus/EventBus/EventBus.csproj"
COPY "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj" "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"
COPY "BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj" "BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj"
COPY "Services/ivmMarkets.GraphCalculation.InputParameters.API.csproj" "Services/ivmMarkets.GraphCalculation.InputParameters.API.csproj"
COPY "Services/ivmMarkets.GraphCalculation.Fab.API.csproj" "Services/ivmMarkets.GraphCalculation.Fab.API.csproj"
COPY "Services/ivmMarkets.GraphCalculation.Fcd.API.csproj" "Services/ivmMarkets.GraphCalculation.Fcd.API.csproj"
COPY "Services/ivmMarkets.GraphCalculation.Fe.API.csproj" "Services/ivmMarkets.GraphCalculation.Fe.API.csproj"
COPY "Services/ivmMarkets.GraphCalculation.Fx.API.csproj" "Services/ivmMarkets.GraphCalculation.Fx.API.csproj"
COPY "Services/ivmMarkets.GraphCalculation.Fy.API.csproj" "Services/ivmMarkets.GraphCalculation.Fy.API.csproj"
COPY "Services/ivmMarkets.GraphCalculation.Result.API.csproj" "Services/ivmMarkets.GraphCalculation.Result.API.csproj"
COPY "Web/ivmMarkets.GraphCalculation.MVC/ivmMarkets.GraphCalculation.MVC.csproj" "Web/ivmMarkets.GraphCalculation.MVC/ivmMarkets.GraphCalculation.MVC.csproj"

COPY "docker-compose.dcproj" "docker-compose.dcproj"

COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "ivmMarketsGraphCalculation.sln"

COPY . .
WORKDIR /src/Web/ivmMarkets.GraphCalculation.MVC
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ivmMarkets.GraphCalculation.MVC.dll"]