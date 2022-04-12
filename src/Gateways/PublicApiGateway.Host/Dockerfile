#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Gateways/PublicApiGateway.Host/PublicApiGateway.Host.csproj", "src/Gateways/PublicApiGateway.Host/"]
COPY ["src/CoreModule/CoreModule.Web/CoreModule.Web.csproj", "src/CoreModule/CoreModule.Web/"]
COPY ["src/CoreModule/CoreModule.Application/CoreModule.Application.csproj", "src/CoreModule/CoreModule.Application/"]
COPY ["src/CoreModule/CoreModule.Domain/CoreModule.Domain.csproj", "src/CoreModule/CoreModule.Domain/"]
RUN dotnet restore "src/Gateways/PublicApiGateway.Host/PublicApiGateway.Host.csproj"
COPY . .
WORKDIR "/src/src/Gateways/PublicApiGateway.Host"
RUN dotnet build "PublicApiGateway.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PublicApiGateway.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PublicApiGateway.Host.dll"]