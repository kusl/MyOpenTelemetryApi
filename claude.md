Ah, I understand now! You're SSH'd into a Debian machine (let's say at `192.168.0.120`) and you want to access the API from your local computer's browser. The current setup only binds to `localhost` on the Debian machine, so it's not accessible from external machines.

Let me fix the port bindings to expose the services on all network interfaces:Now let me create a network testing script to verify external access:Now let me update the setup script to include network testing:Perfect! Now I've fixed the networking to allow external access. Here's what changed:

## 🌐 **External Access Configuration:**

### **Port Bindings Changed:**
```yaml
# Before (localhost only)
ports:
  - "5174:8080"
  - "5432:5432"

# After (all interfaces)  
ports:
  - "0.0.0.0:5174:8080"
  - "0.0.0.0:5432:5432"
```

### **Now You Can Access:**
From your local computer's browser:
- **🌐 API**: `http://192.168.0.120:5174/api/health`
- **🌐 Root Page**: `http://192.168.0.120:5174/`
- **🌐 Contacts**: `http://192.168.0.120:5174/api/contacts`
- **🗄️ Database**: `192.168.0.120:5432` (for tools like pgAdmin)

## 🚀 **Try it now:**

```bash
# Rebuild with external access
./cleanup-and-retry.sh

# Test external network access
chmod +x test-external-access.sh
./test-external-access.sh
```

The network test script will:
- ✅ Detect all IP addresses on your Debian machine
- ✅ Check if ports are bound correctly for external access
- ✅ Test API endpoints from each IP address
- ✅ Show you the exact URLs to use from your browser
- ✅ Check firewall status and provide troubleshooting tips

## 🔒 **Firewall Considerations:**

If you can't access from your local machine, you might need to open the firewall:

```bash
# On your Debian machine
sudo ufw allow 5174/tcp  # For API access
sudo ufw allow 5432/tcp  # For database access (optional)
```

## 🎯 **Expected URLs from your browser:**
- `http://192.168.0.120:5174/` - Beautiful landing page
- `http://192.168.0.120:5174/api/health` - JSON health status
- `http://192.168.0.120:5174/api/contacts` - Empty contacts array initially

The test script will show you all available IP addresses and test each one automatically!