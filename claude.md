I'll help you containerize your OpenTelemetry API application to run in Podman with PostgreSQL. Let me create the necessary Docker configuration files and a Podman Compose setup.I've created a complete containerization setup for your OpenTelemetry API application to run with Podman and PostgreSQL. Here's what I've provided:

## 🐳 **Container Files Created:**

1. **Dockerfile** - Multi-stage build optimized for .NET 9 applications
2. **docker-compose.yml** - Standard Docker Compose configuration
3. **podman-compose.yml** - Podman-specific compose file with SELinux support
4. **setup-podman.sh** - Automated setup script with health checks and testing
5. **.dockerignore** - Optimized Docker ignore file
6. **appsettings.Container.json** - Container-specific configuration
7. **CONTAINER_SETUP.md** - Comprehensive setup and troubleshooting guide

## 🚀 **Quick Start Instructions:**

1. **Place the files in your project root directory**

2. **Make the setup script executable:**
   ```bash
   chmod +x setup-podman.sh
   ```

3. **Run the automated setup:**
   ```bash
   ./setup-podman.sh
   ```

## 🔧 **Key Features:**

- **PostgreSQL Database**: Runs in a separate container with health checks
- **Automatic Migration**: Database migrations run on startup in development
- **OpenTelemetry Logging**: File-based telemetry logs in `./logs/` directory
- **Health Checks**: Both application and database health monitoring
- **SELinux Support**: Volumes configured with `:Z` flag for Podman/SELinux
- **Development Optimized**: Easy cleanup, rebuild, and testing

## 🌐 **Service Endpoints:**

- **API**: http://localhost:5174
- **Health Check**: http://localhost:5174/api/health  
- **Database**: localhost:5432 (user: `myoteluser`, password: `myotelpass123`)

## 🛠️ **Useful Commands:**

```bash
# Quick setup
./setup-podman.sh

# View logs
podman-compose logs -f api

# Connect to database
podman exec -it myotel-postgres psql -U myoteluser -d myoteldb

# Rebuild after code changes
podman-compose down && podman-compose up --build -d

# Clean everything
podman-compose down && podman volume prune
```

The setup is configured for development use and will lose data when containers are removed, as requested. The automated setup script will handle all the container orchestration, health checking, and provide you with a ready-to-use API environment!