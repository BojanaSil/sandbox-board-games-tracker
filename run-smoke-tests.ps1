# PowerShell script for running Smoke tests
Write-Host "Starting Smoke Tests..." -ForegroundColor Green

# Stop any existing containers
Write-Host "Stopping existing containers..." -ForegroundColor Yellow
docker-compose --profile e2e --profile acceptance down

# Start basic services for smoke tests
Write-Host "Starting services for smoke tests..." -ForegroundColor Yellow
docker-compose --profile e2e up -d --build

# Wait for services to be ready
Write-Host "Waiting for services to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Run Smoke tests
Write-Host "Running Smoke tests..." -ForegroundColor Green
$env:TEST_ENVIRONMENT = "E2E"
dotnet test PlaywrightTestsDotNet --filter "FullyQualifiedName~SmokeTests" --logger "console;verbosity=normal"

# Store test result
$testResult = $LASTEXITCODE

# Stop containers
Write-Host "Stopping containers..." -ForegroundColor Yellow
docker-compose --profile e2e down

# Exit with test result code
exit $testResult