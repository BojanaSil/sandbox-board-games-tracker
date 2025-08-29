# PowerShell script for running all tests sequentially
Write-Host "Running All Tests Sequentially..." -ForegroundColor Green

# Run Smoke tests first
Write-Host "`n========== SMOKE TESTS ==========`n" -ForegroundColor Cyan
& ./run-smoke-tests.ps1
$smokeResult = $LASTEXITCODE

# Run Acceptance tests
Write-Host "`n========== ACCEPTANCE TESTS ==========`n" -ForegroundColor Cyan
& ./run-acceptance-tests.ps1
$acceptanceResult = $LASTEXITCODE

# Run E2E tests
Write-Host "`n========== E2E TESTS ==========`n" -ForegroundColor Cyan
& ./run-e2e-tests.ps1
$e2eResult = $LASTEXITCODE

# Summary
Write-Host "`n========== TEST SUMMARY ==========`n" -ForegroundColor Cyan
if ($smokeResult -eq 0) {
    Write-Host "[PASS] Smoke Tests: PASSED" -ForegroundColor Green
} else {
    Write-Host "[FAIL] Smoke Tests: FAILED" -ForegroundColor Red
}

if ($acceptanceResult -eq 0) {
    Write-Host "[PASS] Acceptance Tests: PASSED" -ForegroundColor Green
} else {
    Write-Host "[FAIL] Acceptance Tests: FAILED" -ForegroundColor Red
}

if ($e2eResult -eq 0) {
    Write-Host "[PASS] E2E Tests: PASSED" -ForegroundColor Green
} else {
    Write-Host "[FAIL] E2E Tests: FAILED" -ForegroundColor Red
}

# Exit with failure if any test failed
if (($smokeResult -ne 0) -or ($acceptanceResult -ne 0) -or ($e2eResult -ne 0)) {
    exit 1
}
exit 0