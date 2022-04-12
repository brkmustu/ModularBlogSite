#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UserPortalModule/UserPortalModule.EventProcessor/UserPortalModule.EventProcessor.csproj", "src/UserPortalModule/UserPortalModule.EventProcessor/"]
COPY ["src/UserPortalModule/UserPortalModule.Persistence/UserPortalModule.Persistence.csproj", "src/UserPortalModule/UserPortalModule.Persistence/"]
COPY ["src/UserPortalModule/UserPortalModule.Application/UserPortalModule.Application.csproj", "src/UserPortalModule/UserPortalModule.Application/"]
COPY ["src/CoreModule/CoreModule.Application/CoreModule.Application.csproj", "src/CoreModule/CoreModule.Application/"]
COPY ["src/CoreModule/CoreModule.Domain/CoreModule.Domain.csproj", "src/CoreModule/CoreModule.Domain/"]
COPY ["src/CoreModule/CoreModule.Persistence/CoreModule.Persistence.csproj", "src/CoreModule/CoreModule.Persistence/"]
RUN dotnet restore "src/UserPortalModule/UserPortalModule.EventProcessor/UserPortalModule.EventProcessor.csproj"
COPY . .
WORKDIR "/src/src/UserPortalModule/UserPortalModule.EventProcessor"
RUN dotnet build "UserPortalModule.EventProcessor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserPortalModule.EventProcessor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserPortalModule.EventProcessor.dll"]