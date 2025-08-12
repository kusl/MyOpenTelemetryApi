I see the issue! The problem is with the health check syntax in the compose file. The `CMD-SHELL` format isn't being parsed correctly by podman-compose. Let me fix this for you.Now I need to update the Dockerfile to install curl since it's needed for the health check:Now let's create a simplified version that should work better with podman-compose:Now let's update the setup script to use the simple version and add some better error handling:Perfect! The issue was with the health check syntax in the compose files. The problem is that `podman-compose` doesn't handle `CMD-SHELL` the same way Docker does. Here's what I've fixed:

## 🔧 **Issues Fixed:**

1. **Health Check Syntax**: Changed from `CMD-SHELL` format to `CMD` array format which is more compatible with podman-compose
2. **Added curl to container**: Updated Dockerfile to install curl for health checks
3. **Created simplified compose file**: `docker-compose.simple.yml` without complex health checks
4. **Extended wait times**: Increased timeouts for service startup (especially for .NET app startup)
5. **Better error messages**: Added log viewing hints when services fail to start

## 🚀 **Try running it now:**

```bash
# Clean up any existing containers first
podman stop myotel-api myotel-postgres 2>/dev/null || true
podman rm myotel-api myotel-postgres 2>/dev/null || true

# Run the fixed setup
bash setup-podman.sh
```

## 🔍 **If you still get issues, try these debugging steps:**

```bash
# Check what containers are running
podman ps -a

# Check the database logs
podman logs myotel-postgres

# Check the API logs  
podman logs myotel-api

# Manual database test
podman exec -it myotel-postgres psql -U myoteluser -d myoteldb -c "SELECT version();"
```

The setup should now work much better with podman-compose! The simplified compose file removes the problematic health check syntax while still providing all the functionality you need for development.