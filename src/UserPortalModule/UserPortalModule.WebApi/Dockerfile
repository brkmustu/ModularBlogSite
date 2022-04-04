#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UserPortalModule/UserPortalModule.WebApi/UserPortalModule.WebApi.csproj", "src/UserPortalModule/UserPortalModule.WebApi/"]
COPY ["src/CoreModule/CoreModule.Web/CoreModule.Web.csproj", "src/CoreModule/CoreModule.Web/"]
COPY ["src/CoreModule/CoreModule.Application/CoreModule.Application.csproj", "src/CoreModule/CoreModule.Application/"]
COPY ["src/UserPortalModule/UserPortalModule.Domain/UserPortalModule.Domain.csproj", "src/UserPortalModule/UserPortalModule.Domain/"]
COPY ["src/UserPortalModule/UserPortalModule.Domain.Shared/UserPortalModule.Domain.Shared.csproj", "src/UserPortalModule/UserPortalModule.Domain.Shared/"]
COPY ["src/CoreModule/CoreModule.Domain/CoreModule.Domain.csproj", "src/CoreModule/CoreModule.Domain/"]
COPY ["src/CoreModule/CoreModule/CoreModule.csproj", "src/CoreModule/CoreModule/"]
COPY ["src/UserPortalModule/UserPortalModule.Persistence/UserPortalModule.Persistence.csproj", "src/UserPortalModule/UserPortalModule.Persistence/"]
COPY ["src/UserPortalModule/UserPortalModule.Application/UserPortalModule.Application.csproj", "src/UserPortalModule/UserPortalModule.Application/"]
COPY ["src/UserPortalModule/UserPortalModule.Application.Contracts/UserPortalModule.Application.Contracts.csproj", "src/UserPortalModule/UserPortalModule.Application.Contracts/"]
COPY ["src/UserPortalModule/UserPortalModule.Infrastructure/UserPortalModule.Infrastructure.csproj", "src/UserPortalModule/UserPortalModule.Infrastructure/"]
RUN dotnet restore "src/UserPortalModule/UserPortalModule.WebApi/UserPortalModule.WebApi.csproj"
COPY . .
WORKDIR "/src/src/UserPortalModule/UserPortalModule.WebApi"
RUN dotnet build "UserPortalModule.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserPortalModule.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserPortalModule.WebApi.dll"]