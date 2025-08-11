# 🐳 MyOpenTelemetryApi - Podman Setup

This guide will help you run the MyOpenTelemetryApi application using Podman with PostgreSQL.

## 📋 Prerequisites

- [Podman](https://podman.io/getting-started/installation) installed
- [podman-compose](https://github.com/containers/podman-compose) (optional, but recommended)
- `curl` and `jq` (for testing, optional)

### Installing Prerequisites

#### Ubuntu/Debian:
```bash
sudo apt update
sudo apt install -y podman curl jq
pip3 install podman-compose
```

#### RHEL/Fedora/CentOS:
```bash
sudo dnf install -y podman curl jq python3-pip
pip3 install podman-compose
```

#### macOS:
```bash
brew install podman curl jq
pip3 install podman-compose
```

## 🚀 Quick Start

### Option 1: Using the Setup Script (Recommended)

1. **Make the setup script executable:**
   ```bash
   chmod +x setup-podman.sh
   ```

2. **Run the setup script:**
   ```bash
   ./setup-podman.sh
   ```

3. **To see logs during startup:**
   ```bash
   ./setup-podman.sh --logs
   ```

### Option 2: Manual Setup

1. **Create necessary directories:**
   ```bash
   mkdir -p logs init-db
   ```

2. **Build and start services:**
   ```bash
   # Using podman-compose
   podman-compose up --build -d
   
   # OR using podman compose
   podman compose up --build -d
   ```

3. **Check service status:**
   ```bash
   podman ps
   ```

## 🔧 Configuration

The application runs with the following default configuration:

| Service | Port | Credentials |
|---------|------|-------------|
| API | 5174 | N/A |
| PostgreSQL | 5432 | User: `myoteluser`<br>Password: `myotelpass123`<br>Database: `myoteldb` |

### Environment Variables

Key environment variables configured in the container:

- `ASPNETCORE_ENVIRONMENT=Development`
- `ConnectionStrings__DefaultConnection` - PostgreSQL connection string
- `OpenTelemetry__ServiceName=MyOpenTelemetryApi-Podman`
- `OpenTelemetry__Exporter__Console__Enabled=true`
- `OpenTelemetry__Exporter__File__Enabled=true`

## 🧪 Testing the Application

### Health Checks
```bash
# Basic health check
curl http://localhost:5174/api/health | jq

# Readiness check
curl http://localhost:5174/api/health/ready | jq

# Get all contacts (should return empty array initially)
curl http://localhost:5174/api/contacts | jq
```

### Creating Test Data
```bash
# Create a test contact
curl -X POST http://localhost:5174/api/contacts \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "John",
    "lastName": "Doe",
    "company": "Tech Corp",
    "emailAddresses": [{
      "email": "john.doe@example.com",
      "type": "Work",
      "isPrimary": true
    }],
    "phoneNumbers": [{
      "number": "+1-555-123-4567",
      "type": "Mobile",
      "isPrimary": true
    }]
  }' | jq

# Get all contacts to see the created contact
curl http://localhost:5174/api/contacts | jq
```

### Creating Test Groups and Tags
```bash
# Create a group
curl -X POST http://localhost:5174/api/groups \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Work Colleagues",
    "description": "People from work"
  }' | jq

# Create a tag
curl -X POST http://localhost:5174/api/tags \
  -H "Content-Type: application/json" \
  -d '{
    "name": "VIP",
    "colorHex": "#FF0000"
  }' | jq

# Get all groups
curl http://localhost:5174/api/groups | jq

# Get all tags
curl http://localhost:5174/api/tags | jq
```

## 📊 Monitoring and Logs

### Application Logs
```bash
# View application logs
podman-compose logs -f api

# View PostgreSQL logs
podman-compose logs -f db

# View all logs
podman-compose logs -f
```

### OpenTelemetry File Logs
The application writes OpenTelemetry logs to `./logs/otel-logs.json`:
```bash
# View structured logs
tail -f logs/otel-logs.json | jq

# Monitor logs in real-time with formatting
tail -f logs/otel-logs.json | while read line; do echo "$line" | jq -C; done
```

### Container Management
```bash
# View running containers
podman ps

# Stop all services
podman-compose down

# Restart just the API
podman-compose restart api

# Rebuild and restart
podman-compose up --build -d

# Shell into API container
podman exec -it myotel-api /bin/bash

# Shell into PostgreSQL container
podman exec -it myotel-postgres psql -U myoteluser -d myoteldb
```

## 🗄️ Database Management

### Connect to PostgreSQL
```bash
# Using podman exec
podman exec -it myotel-postgres psql -U myoteluser -d myoteldb

# Using external psql client (if installed)
psql -h localhost -p 5432 -U myoteluser -d myoteldb
```

### Common SQL Queries
```sql
-- List all tables
\dt

-- View contacts
SELECT * FROM "Contacts";

-- View groups
SELECT * FROM "Groups";

-- View tags
SELECT * FROM "Tags";

-- View contact-group relationships
SELECT c."FirstName", c."LastName", g."Name" as "GroupName"
FROM "Contacts" c
JOIN "ContactGroups" cg ON c."Id" = cg."ContactId"
JOIN "Groups" g ON cg."GroupId" = g."Id";
```

## 🔄 Development Workflow

### Rebuilding After Code Changes
```bash
# Stop services
podman-compose down

# Rebuild and start
podman-compose up --build -d

# Or use the script
./setup-podman.sh --no-test
```

### Resetting Database
```bash
# Stop services
podman-compose down

# Remove database volume
podman volume rm myopentelemetryapi_postgres_data

# Start services (will recreate database)
podman-compose up -d
```

### Cleanup Everything
```bash
# Stop and remove containers
podman-compose down

# Remove volumes
podman volume prune

# Remove images
podman image prune -a

# Or use the cleanup function in the script
./setup-podman.sh
```

## 🔧 Troubleshooting

### Common Issues

1. **Port already in use:**
   ```bash
   # Check what's using the port
   netstat -tulpn | grep :5174
   
   # Stop the conflicting service or change ports in docker-compose.yml
   ```

2. **Database connection failed:**
   ```bash
   # Check if PostgreSQL is running
   podman exec myotel-postgres pg_isready -U myoteluser -d myoteldb
   
   # Check database logs
   podman logs myotel-postgres
   ```

3. **API not starting:**
   ```bash
   # Check API logs
   podman logs myotel-api
   
   # Check if API container is running
   podman ps -a
   ```

4. **Permission issues (SELinux):**
   ```bash
   # If you're on a system with SELinux, add :Z to volume mounts
   # This is already done in podman-compose.yml
   ```

### Performance Issues
```bash
# Check resource usage
podman stats

# View container resource limits
podman inspect myotel-api | jq '.[0].HostConfig'
```

## 🎯 Next Steps

- **Add Observability Stack:** Uncomment Jaeger, Prometheus, and Grafana services in `podman-compose.yml`
- **Configure OTLP Export:** Set up OpenTelemetry Protocol export to external systems
- **Add SSL/TLS:** Configure HTTPS for production-like setup
- **Persistent Data:** Configure persistent volumes for production use
- **Load Testing:** Use tools like `hey` or `wrk` to test API performance
- **Custom Metrics:** Add custom application metrics to monitor business logic

## 📚 Useful Resources

- [Podman Documentation](https://docs.podman.io/)
- [OpenTelemetry Documentation](https://opentelemetry.io/docs/)
- [ASP.NET Core Docker](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/)
- [PostgreSQL Docker](https://hub.docker.com/_/postgres)

---

**Happy Coding! 🎉**