# PowerShell script for running Acceptance tests
Write-Host "Starting Acceptance Tests..." -ForegroundColor Green

# Stop any existing containers
Write-Host "Stopping existing containers..." -ForegroundColor Yellow
docker-compose --profile e2e --profile acceptance down

# Start services for Acceptance (with WireMock)
Write-Host "Starting Acceptance services (using WireMock)..." -ForegroundColor Yellow
$env:BGG_BASE_URL = "http://wiremock:8080"
docker-compose --profile acceptance up -d --build

# Wait for services to be ready
Write-Host "Waiting for services to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Run Acceptance tests
Write-Host "Running Acceptance tests..." -ForegroundColor Green
$env:TEST_ENVIRONMENT = "Acceptance"
dotnet test PlaywrightTestsDotNet --filter "FullyQualifiedName~AcceptanceTests" --logger "console;verbosity=normal"

# Store test result
$testResult = $LASTEXITCODE

# Stop containers
Write-Host "Stopping containers..." -ForegroundColor Yellow
docker-compose --profile acceptance down

# Exit with test result code
exit $testResult