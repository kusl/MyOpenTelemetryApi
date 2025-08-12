#!/bin/bash

# run-migrations.sh - Script to run Entity Framework migrations

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}🗄️ Entity Framework Migration Runner${NC}"
echo "========================================"

# Check if containers are running
if ! podman ps | grep -q myotel-postgres; then
    echo -e "${RED}❌ PostgreSQL container is not running${NC}"
    echo -e "${YELLOW}💡 Run './setup-podman.sh' first to start the containers${NC}"
    exit 1
fi

if ! podman ps | grep -q myotel-api; then
    echo -e "${RED}❌ API container is not running${NC}"
    echo -e "${YELLOW}💡 Run './setup-podman.sh' first to start the containers${NC}"
    exit 1
fi

# Function to run migrations using the API container (startup migrations)
check_automatic_migrations() {
    echo -e "${BLUE}🔄 Checking automatic migration status...${NC}"
    
    echo -e "${YELLOW}Automatic migrations run on container startup.${NC}"
    echo -e "${YELLOW}Checking API container logs for migration status...${NC}"
    
    if podman logs myotel-api 2>/dev/null | grep -q "Applying database migrations"; then
        if podman logs myotel-api 2>/dev/null | grep -q "Database migrations applied successfully"; then
            echo -e "${GREEN}✅ Automatic migrations completed successfully${NC}"
        else
            echo -e "${RED}❌ Automatic migrations may have failed${NC}"
            echo -e "${YELLOW}💡 Check logs: podman logs myotel-api${NC}"
        fi
    else
        echo -e "${YELLOW}⚠️  No migration logs found - container may still be starting${NC}"
    fi
    
    echo -e "${BLUE}💡 Migrations run automatically when the API container starts${NC}"
}

# Function to manually create database schema (SQL approach)
create_schema_manually() {
    echo -e "${BLUE}🔄 Creating database schema manually using SQL...${NC}"
    
    echo -e "${YELLOW}This will execute the initial migration SQL directly${NC}"
    
    # Check if the InitialCreate migration exists
    if [ ! -f "src/MyOpenTelemetryApi.Infrastructure/Data/Migrations/20250804231722_InitialCreate.cs" ]; then
        echo -e "${RED}❌ InitialCreate migration file not found${NC}"
        exit 1
    fi
    
    # Create tables manually using SQL
    cat << 'EOF' | podman exec -i myotel-postgres psql -U myoteluser -d myoteldb
-- Create migration history table
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

-- Insert migration record
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion") 
VALUES ('20250804231722_InitialCreate', '9.0.8')
ON CONFLICT ("MigrationId") DO NOTHING;

-- Create tables (based on the migration file)
CREATE TABLE IF NOT EXISTS "Contacts" (
    "Id" uuid NOT NULL,
    "FirstName" character varying(100) NOT NULL,
    "LastName" character varying(100) NOT NULL,
    "MiddleName" character varying(100),
    "Nickname" character varying(50),
    "Company" character varying(200),
    "JobTitle" character varying(100),
    "DateOfBirth" timestamp with time zone,
    "Notes" character varying(1000),
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Contacts" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Groups" (
    "Id" uuid NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" character varying(500),
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Groups" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "Tags" (
    "Id" uuid NOT NULL,
    "Name" character varying(50) NOT NULL,
    "ColorHex" character varying(7),
    CONSTRAINT "PK_Tags" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Tags_Name" ON "Tags" ("Name");

CREATE TABLE IF NOT EXISTS "Addresses" (
    "Id" uuid NOT NULL,
    "ContactId" uuid NOT NULL,
    "StreetLine1" character varying(200),
    "StreetLine2" character varying(200),
    "City" character varying(100),
    "StateProvince" character varying(100),
    "PostalCode" character varying(20),
    "Country" character varying(100),
    "Type" character varying(20) NOT NULL,
    "IsPrimary" boolean NOT NULL,
    CONSTRAINT "PK_Addresses" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Addresses_Contacts_ContactId" FOREIGN KEY ("ContactId") REFERENCES "Contacts" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_Addresses_ContactId" ON "Addresses" ("ContactId");

CREATE TABLE IF NOT EXISTS "EmailAddresses" (
    "Id" uuid NOT NULL,
    "ContactId" uuid NOT NULL,
    "Email" character varying(254) NOT NULL,
    "Type" character varying(20) NOT NULL,
    "IsPrimary" boolean NOT NULL,
    CONSTRAINT "PK_EmailAddresses" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_EmailAddresses_Contacts_ContactId" FOREIGN KEY ("ContactId") REFERENCES "Contacts" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_EmailAddresses_ContactId" ON "EmailAddresses" ("ContactId");

CREATE TABLE IF NOT EXISTS "PhoneNumbers" (
    "Id" uuid NOT NULL,
    "ContactId" uuid NOT NULL,
    "Number" character varying(50) NOT NULL,
    "Type" character varying(20) NOT NULL,
    "IsPrimary" boolean NOT NULL,
    CONSTRAINT "PK_PhoneNumbers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_PhoneNumbers_Contacts_ContactId" FOREIGN KEY ("ContactId") REFERENCES "Contacts" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_PhoneNumbers_ContactId" ON "PhoneNumbers" ("ContactId");

CREATE TABLE IF NOT EXISTS "ContactGroups" (
    "ContactId" uuid NOT NULL,
    "GroupId" uuid NOT NULL,
    "AddedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_ContactGroups" PRIMARY KEY ("ContactId", "GroupId"),
    CONSTRAINT "FK_ContactGroups_Contacts_ContactId" FOREIGN KEY ("ContactId") REFERENCES "Contacts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ContactGroups_Groups_GroupId" FOREIGN KEY ("GroupId") REFERENCES "Groups" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_ContactGroups_GroupId" ON "ContactGroups" ("GroupId");

CREATE TABLE IF NOT EXISTS "ContactTags" (
    "ContactId" uuid NOT NULL,
    "TagId" uuid NOT NULL,
    CONSTRAINT "PK_ContactTags" PRIMARY KEY ("ContactId", "TagId"),
    CONSTRAINT "FK_ContactTags_Contacts_ContactId" FOREIGN KEY ("ContactId") REFERENCES "Contacts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_ContactTags_Tags_TagId" FOREIGN KEY ("TagId") REFERENCES "Tags" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_ContactTags_TagId" ON "ContactTags" ("TagId");

EOF
    
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✅ Database schema created successfully${NC}"
    else
        echo -e "${RED}❌ Failed to create database schema${NC}"
        exit 1
    fi
}
# Function to run migrations using local dotnet CLI
run_migrations_locally() {
    echo -e "${BLUE}🔄 Running migrations using local .NET CLI...${NC}"
    
    if ! command -v dotnet &> /dev/null; then
        echo -e "${RED}❌ .NET CLI not found locally${NC}"
        echo -e "${YELLOW}💡 Try using automatic migrations or manual schema creation${NC}"
        exit 1
    fi
    
    # Set the connection string for local execution - PostgreSQL is exposed on localhost:5432
    export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=myoteldb;Username=myoteluser;Password=myotelpass123;"
    
    cd src/MyOpenTelemetryApi.Api
    
    echo -e "${YELLOW}📋 Listing pending migrations...${NC}"
    dotnet ef migrations list --verbose
    
    echo -e "${YELLOW}🔄 Applying migrations...${NC}"
    dotnet ef database update --verbose
    
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✅ Migrations completed successfully${NC}"
    else
        echo -e "${RED}❌ Migration failed${NC}"
        exit 1
    fi
    
    cd ../..
}

# Function to check database status
check_database_status() {
    echo -e "${BLUE}📊 Checking database status...${NC}"
    
    echo -e "${YELLOW}Testing database connection...${NC}"
    if podman exec myotel-postgres pg_isready -U myoteluser -d myoteldb; then
        echo -e "${GREEN}✅ Database connection OK${NC}"
    else
        echo -e "${RED}❌ Database connection failed${NC}"
        exit 1
    fi
    
    echo -e "${YELLOW}Listing existing tables...${NC}"
    podman exec -it myotel-postgres psql -U myoteluser -d myoteldb -c "\dt"
    
    echo -e "${YELLOW}Checking migration history...${NC}"
    podman exec -it myotel-postgres psql -U myoteluser -d myoteldb -c "SELECT * FROM \"__EFMigrationsHistory\";" 2>/dev/null || echo "No migration history table found"
}

# Function to reset database (destructive)
reset_database() {
    echo -e "${RED}⚠️  WARNING: This will DELETE ALL DATA in the database!${NC}"
    read -p "Are you sure you want to continue? (yes/no): " confirm
    
    if [ "$confirm" = "yes" ]; then
        echo -e "${YELLOW}🗑️ Dropping and recreating database...${NC}"
        
        # Drop and recreate database
        podman exec myotel-postgres psql -U myoteluser -d postgres -c "DROP DATABASE IF EXISTS myoteldb;"
        podman exec myotel-postgres psql -U myoteluser -d postgres -c "CREATE DATABASE myoteldb;"
        
        echo -e "${GREEN}✅ Database reset complete${NC}"
        echo -e "${BLUE}🔄 Now run migrations to recreate schema${NC}"
    else
        echo -e "${YELLOW}Operation cancelled${NC}"
    fi
}

# Function to create a new migration
create_migration() {
    local migration_name="$1"
    
    if [ -z "$migration_name" ]; then
        echo -e "${RED}❌ Migration name is required${NC}"
        echo -e "${YELLOW}Usage: $0 create <migration_name>${NC}"
        exit 1
    fi
    
    echo -e "${BLUE}📝 Creating new migration: $migration_name${NC}"
    
    if ! command -v dotnet &> /dev/null; then
        echo -e "${RED}❌ .NET CLI not found locally${NC}"
        echo -e "${YELLOW}💡 Install .NET SDK to create migrations${NC}"
        exit 1
    fi
    
    cd src/MyOpenTelemetryApi.Api
    
    # Set the connection string for local execution - PostgreSQL is exposed on localhost:5432
    export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=myoteldb;Username=myoteluser;Password=myotelpass123;"
    
    dotnet ef migrations add "$migration_name" --verbose
    
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✅ Migration created successfully${NC}"
        echo -e "${BLUE}💡 Don't forget to run migrations to apply changes${NC}"
    else
        echo -e "${RED}❌ Failed to create migration${NC}"
        exit 1
    fi
    
    cd ../..
}

# Parse command line arguments
case "${1:-}" in
    "container"|"c")
        run_migrations_in_container
        ;;
    "local"|"l")
        run_migrations_locally
        ;;
    "status"|"s")
        check_database_status
        ;;
    "reset"|"r")
        reset_database
        ;;
    "create")
        create_migration "$2"
        ;;
    "help"|"h"|"")
        echo "Usage: $0 <command> [options]"
        echo ""
        echo "Commands:"
        echo "  container, c     - Run migrations using the API container (recommended)"
        echo "  local, l         - Run migrations using local .NET CLI"
        echo "  status, s        - Check database and migration status"
        echo "  reset, r         - Reset database (WARNING: destructive!)"
        echo "  create <name>    - Create a new migration"
        echo "  help, h          - Show this help message"
        echo ""
        echo "Examples:"
        echo "  $0 container           # Run migrations in container"
        echo "  $0 status              # Check database status"
        echo "  $0 create AddUserTable # Create new migration"
        echo ""
        echo -e "${BLUE}💡 Recommended: Use 'container' method for consistency${NC}"
        ;;
    *)
        echo -e "${RED}❌ Unknown command: $1${NC}"
        echo -e "${YELLOW}💡 Use '$0 help' to see available commands${NC}"
        exit 1
        ;;
esac