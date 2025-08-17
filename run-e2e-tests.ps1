# PowerShell script for running E2E tests
Write-Host "Starting E2E Tests..." -ForegroundColor Green

# Stop any existing containers
Write-Host "Stopping existing containers..." -ForegroundColor Yellow
docker-compose --profile e2e --profile acceptance down

# Start services for E2E (no WireMock, use real BGG API)
Write-Host "Starting E2E services (using real BGG API)..." -ForegroundColor Yellow
docker-compose --profile e2e up -d --build

# Wait for services to be ready
Write-Host "Waiting for services to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Run E2E tests
Write-Host "Running E2E tests..." -ForegroundColor Green
$env:TEST_ENVIRONMENT = "E2E"
dotnet test PlaywrightTestsDotNet --filter "FullyQualifiedName~EndToEndTests" --logger "console;verbosity=normal"

# Store test result
$testResult = $LASTEXITCODE

# Stop containers
Write-Host "Stopping containers..." -ForegroundColor Yellow
docker-compose --profile e2e down

# Exit with test result code
exit $testResult