# Testing Guide for Board Games Tracker

This document describes how to run the various test suites for the Board Games Tracker application.

## Prerequisites

Before running tests, ensure you have the following installed:
- Docker Desktop
- .NET 8 SDK
- PowerShell (Windows) or Bash (Linux/Mac)
- SQL Server (running locally on port 1433)

## Test Architecture

The project uses approach with three test categories:

1. **Smoke Tests** - Basic health checks to verify services are running
2. **E2E Tests** - End-to-end tests using real external APIs (BoardGameGeek)
3. **Acceptance Tests** - Integration tests using WireMock to mock external APIs

### Key Design Features

- **Single Backend Instance**: One backend service configured via environment variables
- **Docker Compose Profiles**: Separate profiles for E2E and Acceptance environments
- **WireMock Integration**: Acceptance tests use WireMock to mock the BoardGameGeek API
- **Test Isolation**: Each test run starts with fresh containers, ensuring no data contamination

## Running Tests Locally

### Quick Start - Using Scripts

The easiest way to run tests is using the provided scripts:

#### Windows (PowerShell)
```powershell
# Run E2E tests only
.\run-e2e-tests.ps1

# Run Acceptance tests only
.\run-acceptance-tests.ps1

# Run all tests sequentially
.\run-all-tests.ps1
```

#### Linux/Mac (Bash)
```bash
# Make scripts executable (first time only)
chmod +x run-*.sh

# Run E2E tests only
./run-e2e-tests.sh

# Run Acceptance tests only
./run-acceptance-tests.sh
```

### Manual Test Execution

If you prefer to run tests manually or need more control:

#### Running E2E Tests

E2E tests use the real BoardGameGeek API:

```bash
# 1. Stop any existing containers
docker-compose --profile e2e --profile acceptance down

# 2. Start services for E2E (backend + frontend)
docker-compose --profile e2e up -d

# 3. Wait for services to be ready (about 10 seconds)
# You can verify by checking: http://localhost:5001/health

# 4. Run E2E tests
dotnet test PlaywrightTestsDotNet --filter "FullyQualifiedName~EndToEndTests"

# 5. Stop containers when done
docker-compose --profile e2e down
```

#### Running Acceptance Tests

Acceptance tests use WireMock to mock external APIs:

```bash
# 1. Stop any existing containers
docker-compose --profile e2e --profile acceptance down

# 2. Start services for Acceptance (backend + frontend + WireMock)
# Note: Set BGG_BASE_URL to point to WireMock
export BGG_BASE_URL=http://wiremock:8080  # Linux/Mac
set BGG_BASE_URL=http://wiremock:8080     # Windows

docker-compose --profile acceptance up -d

# 3. Wait for services to be ready
# Verify WireMock: http://localhost:9876/__admin/mappings
# Verify backend: http://localhost:5001/health

# 4. Run Acceptance tests
export TEST_ENVIRONMENT=Acceptance  # Linux/Mac
set TEST_ENVIRONMENT=Acceptance     # Windows

dotnet test PlaywrightTestsDotNet --filter "FullyQualifiedName~AcceptanceTests"

# 5. Stop containers when done
docker-compose --profile acceptance down
```

#### Running Smoke Tests

Smoke tests can run with either profile:

```bash
# Start either E2E or Acceptance environment
docker-compose --profile e2e up -d

# Run smoke tests
dotnet test PlaywrightTestsDotNet --filter "FullyQualifiedName~SmokeTests"

# Stop containers
docker-compose --profile e2e down
```

### Running All Tests

To run all test suites sequentially:

```bash
# Windows
.\run-all-tests.ps1

# Linux/Mac
./run-all-tests.sh
```

## Understanding the Test Flow

### Container Lifecycle per Test Run

Each test execution follows this pattern:

1. **Stop** - Clean up any existing containers
2. **Start** - Launch fresh containers with appropriate configuration
3. **Wait** - Allow services to fully initialize
4. **Test** - Execute the test suite
5. **Stop** - Clean up containers after tests complete

This approach ensures:
- **Clean State**: Each test run starts with a fresh database
- **No Interference**: Tests don't affect each other
- **Predictable Results**: Same behavior locally and in CI/CD

### Service Endpoints

When containers are running:

- **Frontend**: http://localhost:4300
- **Backend API**: http://localhost:5001
- **WireMock Admin** (Acceptance only): http://localhost:9876/__admin
- **WireMock API** (Acceptance only): http://localhost:9876

### Configuration Details

#### E2E Environment
- Backend connects to: `https://boardgamegeek.com`
- Real API responses with actual BGG data
- Tests may be slower due to external API calls

#### Acceptance Environment
- Backend connects to: `http://wiremock:8080`
- Mocked responses defined in `/wiremock/mappings/`
- Fast and predictable test execution


## Troubleshooting

### Common Issues and Solutions

1. **Port Already in Use**
   ```bash
   # Find and stop conflicting containers
   docker ps
   docker stop <container-id>
   ```

2. **Tests Can't Connect to Services**
   ```bash
   # Verify services are running
   docker ps
   
   # Check service health
   curl http://localhost:5001/health
   ```

3. **Database Connection Issues**
   - Ensure SQL Server is running on port 1433
   - Verify credentials in docker-compose.yml
   - Check Docker Desktop's host.docker.internal setting

4. **WireMock Not Responding**
   ```bash
   # Check WireMock is running (Acceptance only)
   curl http://localhost:9876/__admin/mappings
   
   # View WireMock logs
   docker logs sandbox-board-games-tracker-wiremock-1
   ```

5. **Playwright Browser Issues**
   ```bash
   # Install Playwright browsers
   cd PlaywrightTestsDotNet
   pwsh bin/Debug/net8.0/playwright.ps1 install
   ```

## Best Practices

1. **Always Clean Up**: Use the provided scripts that handle cleanup automatically
2. **Check Service Health**: Verify services are ready before running tests
3. **Review Logs**: If tests fail, check Docker logs for insights
4. **Isolate Issues**: Run test categories separately to identify problems
5. **Keep WireMock Mappings Updated**: Update `/wiremock/mappings/` when API changes

## Test Data Management

### Database State
- Each test run starts with a fresh database (migrations run on startup)
- No manual cleanup needed between runs
- Test data is isolated per test execution

### WireMock Stubs
- Defined in `/wiremock/mappings/bgg-search.json`
- Returns predictable, static responses
- Easy to modify for new test scenarios

## Contributing

When adding new tests:

1. Place tests in appropriate folders:
   - `SmokeTests/` - Basic health checks
   - `EndToEndTests/` - Real API integration tests
   - `AcceptanceTests/` - Mocked API tests

2. Follow naming conventions:
   - Test classes: `*Test.cs`
   - Test methods: Describe what is being tested

3. Use the appropriate test fixtures:
   - `TestFixture` - Base fixture for all tests
   - `IClassFixture<TestFixture>` - For test class initialization

4. Ensure tests are idempotent and can run in any order

## Summary

This testing approach provides:
- **Flexibility**: Run any combination of tests
- **Isolation**: No interference between test runs
- **CI/CD Ready**: Works identically locally and in pipelines
- **Maintainability**: Clear separation of concerns
- **Reliability**: Predictable, repeatable test execution

The key innovation is using Docker Compose profiles to manage different test environments while maintaining a single backend codebase that's configured through environment variables. This makes the system both simple and powerful.