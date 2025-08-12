# Use the official .NET 9 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the .NET 9 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["src/MyOpenTelemetryApi.Api/MyOpenTelemetryApi.Api.csproj", "src/MyOpenTelemetryApi.Api/"]
COPY ["src/MyOpenTelemetryApi.Application/MyOpenTelemetryApi.Application.csproj", "src/MyOpenTelemetryApi.Application/"]
COPY ["src/MyOpenTelemetryApi.Domain/MyOpenTelemetryApi.Domain.csproj", "src/MyOpenTelemetryApi.Domain/"]
COPY ["src/MyOpenTelemetryApi.Infrastructure/MyOpenTelemetryApi.Infrastructure.csproj", "src/MyOpenTelemetryApi.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/MyOpenTelemetryApi.Api/MyOpenTelemetryApi.Api.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/src/MyOpenTelemetryApi.Api"
RUN dotnet build "MyOpenTelemetryApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyOpenTelemetryApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app

# Install curl for health checks
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create logs directory with proper permissions
RUN mkdir -p /app/logs && chmod 755 /app/logs

# Copy published application
COPY --from=publish /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos "" --uid 1000 appuser && chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "MyOpenTelemetryApi.Api.dll"]