#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ManagementModule/ManagementModule.WebApi/ManagementModule.WebApi.csproj", "src/ManagementModule/ManagementModule.WebApi/"]
COPY ["src/ManagementModule/ManagementModule.Persistence/ManagementModule.Persistence.csproj", "src/ManagementModule/ManagementModule.Persistence/"]
COPY ["src/ManagementModule/ManagementModule.Application/ManagementModule.Application.csproj", "src/ManagementModule/ManagementModule.Application/"]
COPY ["src/CoreModule/CoreModule.Application/CoreModule.Application.csproj", "src/CoreModule/CoreModule.Application/"]
COPY ["src/UserPortalModule/UserPortalModule.Domain/UserPortalModule.Domain.csproj", "src/UserPortalModule/UserPortalModule.Domain/"]
COPY ["src/UserPortalModule/UserPortalModule.Domain.Shared/UserPortalModule.Domain.Shared.csproj", "src/UserPortalModule/UserPortalModule.Domain.Shared/"]
COPY ["src/CoreModule/CoreModule.Domain/CoreModule.Domain.csproj", "src/CoreModule/CoreModule.Domain/"]
COPY ["src/CoreModule/CoreModule/CoreModule.csproj", "src/CoreModule/CoreModule/"]
COPY ["src/ManagementModule/ManagementModule.Application.Contracts/ManagementModule.Application.Contracts.csproj", "src/ManagementModule/ManagementModule.Application.Contracts/"]
COPY ["src/ManagementModule/ManagementModule.Domain.Shared/ManagementModule.Domain.Shared.csproj", "src/ManagementModule/ManagementModule.Domain.Shared/"]
COPY ["src/ManagementModule/ManagementModule.Domain/ManagementModule.Domain.csproj", "src/ManagementModule/ManagementModule.Domain/"]
COPY ["src/ManagementModule/ManagementModule.Infrastructure/ManagementModule.Infrastructure.csproj", "src/ManagementModule/ManagementModule.Infrastructure/"]
COPY ["src/CoreModule/CoreModule.Web/CoreModule.Web.csproj", "src/CoreModule/CoreModule.Web/"]
RUN dotnet restore "src/ManagementModule/ManagementModule.WebApi/ManagementModule.WebApi.csproj"
COPY . .
WORKDIR "/src/src/ManagementModule/ManagementModule.WebApi"
RUN dotnet build "ManagementModule.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ManagementModule.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ManagementModule.WebApi.dll"]