FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PollForge.API/PollForge.API.csproj", "src/PollForge.API/"]
COPY ["src/PollForge.Application/PollForge.Application.csproj", "src/PollForge.Application/"]
COPY ["src/PollForge.Domain/PollForge.Domain.csproj", "src/PollForge.Domain/"]
COPY ["src/PollForge.Infrastructure/PollForge.Infrastructure.csproj", "src/PollForge.Infrastructure/"]
RUN dotnet restore "src/PollForge.API/PollForge.API.csproj"
COPY . .
WORKDIR "/src/src/PollForge.API"
RUN dotnet build "PollForge.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PollForge.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PollForge.API.dll"]
