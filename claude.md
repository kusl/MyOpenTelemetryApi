The issue is that the cleanup script isn't properly removing the existing containers. Let me fix the cleanup script to handle this better:Also, let me update the main setup script's cleanup function to be more thorough:Now let me create a quick emergency cleanup script for when things get really stuck:Perfect! Now you have multiple options to handle stuck containers:

## 🚨 **Immediate Solution:**

```bash
# Make the emergency cleanup script executable
chmod +x emergency-cleanup.sh

# Run the nuclear cleanup option
./emergency-cleanup.sh

# Then start fresh
./setup-podman.sh
```

## 🔧 **What the emergency cleanup does:**

1. **Stops ALL containers** with "myotel" prefix
2. **Force removes containers** by name and by image
3. **Removes built images** so they get rebuilt
4. **Cleans up networks and volumes**
5. **Prunes unused resources**
6. **Shows you what's left**

## 🛠️ **Or try the improved regular cleanup:**

```bash
# The regular cleanup is now more thorough
./cleanup-and-retry.sh
```

## 🔍 **Why this happened:**

The error shows that container `myotel-postgres` with ID `0f2d1130c01d...` already exists. This usually happens when:
- Previous cleanup didn't complete properly
- Container is in a weird state (exited but not removed)
- Name conflict from previous runs

## 📋 **Check current state:**

```bash
# See what containers exist
podman ps -a

# See what's using the names
podman ps -a --filter "name=myotel"
```

The emergency cleanup script will handle all these edge cases and give you a completely clean slate to work with!