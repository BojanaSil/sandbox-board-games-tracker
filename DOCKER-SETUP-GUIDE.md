# Docker Setup Guide

This guide explains how to set up and run the Board Games Tracker application using Docker on your local machine.

## Prerequisites

### 1. Install Required Software
- **Docker Desktop** (Windows/Mac) or Docker Engine (Linux)
- **SQL Server** (any of the following):
  - SQL Server 2019 or later
  - SQL Server Express (free)
  - SQL Server Developer Edition (free)
- **SQL Server Management Studio (SSMS)** or Azure Data Studio (optional but recommended)

### 2. Configure SQL Server

#### Enable SQL Server Authentication
1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance
3. Right-click on the server name and select **Properties**
4. Go to **Security** page
5. Under **Server authentication**, select **SQL Server and Windows Authentication mode**
6. Click OK and restart the SQL Server service

#### Enable TCP/IP Protocol
1. Open **SQL Server Configuration Manager**
2. Expand **SQL Server Network Configuration**
3. Click on **Protocols for MSSQLSERVER** (or your instance name)
4. Right-click on **TCP/IP** and select **Enable**
5. Double-click on **TCP/IP** to open properties
6. In the **Protocol** tab, ensure **Enabled** is set to **Yes**
7. In the **IP Addresses** tab:
   - Scroll to **IPAll** section
   - Set **TCP Port** to `1433`
   - Clear **TCP Dynamic Ports** (leave it empty)
8. Click OK and restart the SQL Server service

#### Configure Windows Firewall
Open PowerShell as Administrator and run:
```powershell
New-NetFirewallRule -DisplayName "SQL Server Port 1433" -Direction Inbound -Protocol TCP -LocalPort 1433 -Action Allow
```

### 3. Create Database and User

Open SSMS or Azure Data Studio and run the following SQL commands:

```sql
-- Create the database
CREATE DATABASE BoardGamesDb;
GO

-- Create a SQL login for Docker containers
CREATE LOGIN dockeruser WITH PASSWORD = 'dockerdbboardgames1!';
GO

-- Switch to the new database
USE BoardGamesDb;
GO

-- Create a database user for the login
CREATE USER dockeruser FOR LOGIN dockeruser;
GO

-- Make dockeruser the database owner (for migrations)
ALTER AUTHORIZATION ON DATABASE::BoardGamesDb TO dockeruser;
GO
```

## Setup Steps

### 1. Clone the Repository
```bash
git clone <repository-url>
cd sandbox-board-games-tracker
```

### 2. Verify SQL Server Connection
Test that SQL Server is accessible with the dockeruser credentials:

**Windows Command Prompt:**
```cmd
sqlcmd -S localhost,1433 -U dockeruser -P dockerdbboardgames1! -Q "SELECT DB_NAME()"
```

**PowerShell:**
```powershell
sqlcmd -S localhost,1433 -U dockeruser -P dockerdbboardgames1! -Q "SELECT DB_NAME()"
```

You should see `BoardGamesDb` as the output.

### 3. Build and Run with Docker Compose

```bash
# Build and start the containers
docker-compose up -d --build

# Or if you want to see the logs
docker-compose up --build
```

The application will:
- Start the backend API on http://localhost:5001
- Start the frontend on http://localhost:4300
- Connect to your local SQL Server instance
- Run database migrations automatically on startup

### 4. Verify the Application

- **Frontend**: Open http://localhost:4300 in your browser
- **Backend API**: Test with `curl http://localhost:5001/api/boardgame` or open in browser
- **API Documentation**: Open http://localhost:5001/swagger (if Swagger is enabled)

## Common Issues and Solutions

### Issue: "Cannot connect to SQL Server"
**Solution:**
- Ensure SQL Server service is running
- Verify TCP/IP is enabled and listening on port 1433
- Check Windows Firewall allows connections on port 1433
- Verify the dockeruser credentials are correct

### Issue: "CREATE DATABASE permission denied"
**Solution:**
- Ensure the database `BoardGamesDb` is created before running Docker
- Verify dockeruser is the database owner:
  ```sql
  ALTER AUTHORIZATION ON DATABASE::BoardGamesDb TO dockeruser;
  ```

### Issue: "host.docker.internal not found" (Linux only)
**Solution:**
- On Linux, you may need to use your machine's actual IP address instead of `host.docker.internal`
- Find your IP with `ip addr show` and update the connection string in docker-compose.yml

### Issue: Migrations fail
**Solution:**
- Check backend logs: `docker logs sandbox-board-games-tracker-backend-1`
- Ensure dockeruser has db_owner permissions on BoardGamesDb
- Manually run migrations if needed:
  ```bash
  cd backend/BoardGamesTrackerApi
  dotnet ef database update
  ```

## Useful Docker Commands

```bash
# View running containers
docker ps

# View container logs
docker logs sandbox-board-games-tracker-backend-1
docker logs sandbox-board-games-tracker-frontend-1

# Stop all containers
docker-compose down

# Stop and remove all containers, networks, and volumes
docker-compose down -v

# Rebuild containers (after code changes)
docker-compose up -d --build

# Execute commands in running container
docker exec -it sandbox-board-games-tracker-backend-1 /bin/bash
```

## Environment Variables

You can customize ports using environment variables:

```bash
# Linux/Mac
export BACKEND_PORT=5002
export FRONTEND_PORT=4301
docker-compose up -d

# Windows PowerShell
$env:BACKEND_PORT="5002"
$env:FRONTEND_PORT="4301"
docker-compose up -d
```

## Development Notes

- The backend container runs migrations automatically on startup
- Connection string in docker-compose.yml uses `host.docker.internal` to connect to host's SQL Server
- Frontend proxies API requests to the backend container
- Both containers run in development mode with hot-reload support (if configured)