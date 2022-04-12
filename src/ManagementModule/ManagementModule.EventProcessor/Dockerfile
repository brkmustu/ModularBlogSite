#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ManagementModule/ManagementModule.EventProcessor/ManagementModule.EventProcessor.csproj", "src/ManagementModule/ManagementModule.EventProcessor/"]
COPY ["src/ManagementModule/ManagementModule.Persistence/ManagementModule.Persistence.csproj", "src/ManagementModule/ManagementModule.Persistence/"]
COPY ["src/CoreModule/CoreModule.Persistence/CoreModule.Persistence.csproj", "src/CoreModule/CoreModule.Persistence/"]
COPY ["src/CoreModule/CoreModule.Domain/CoreModule.Domain.csproj", "src/CoreModule/CoreModule.Domain/"]
COPY ["src/ManagementModule/ManagementModule.Application/ManagementModule.Application.csproj", "src/ManagementModule/ManagementModule.Application/"]
COPY ["src/CoreModule/CoreModule.Application/CoreModule.Application.csproj", "src/CoreModule/CoreModule.Application/"]
RUN dotnet restore "src/ManagementModule/ManagementModule.EventProcessor/ManagementModule.EventProcessor.csproj"
COPY . .
WORKDIR "/src/src/ManagementModule/ManagementModule.EventProcessor"
RUN dotnet build "ManagementModule.EventProcessor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ManagementModule.EventProcessor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ManagementModule.EventProcessor.dll"]