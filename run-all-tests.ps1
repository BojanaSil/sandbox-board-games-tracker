# PowerShell script for running all tests sequentially
Write-Host "Running All Tests Sequentially..." -ForegroundColor Green

# Run E2E tests first
Write-Host "`n========== E2E TESTS ==========`n" -ForegroundColor Cyan
& ./run-e2e-tests.ps1
$e2eResult = $LASTEXITCODE

# Run Acceptance tests
Write-Host "`n========== ACCEPTANCE TESTS ==========`n" -ForegroundColor Cyan
& ./run-acceptance-tests.ps1
$acceptanceResult = $LASTEXITCODE

# Summary
Write-Host "`n========== TEST SUMMARY ==========`n" -ForegroundColor Cyan
if ($e2eResult -eq 0) {
    Write-Host "✓ E2E Tests: PASSED" -ForegroundColor Green
} else {
    Write-Host "✗ E2E Tests: FAILED" -ForegroundColor Red
}

if ($acceptanceResult -eq 0) {
    Write-Host "✓ Acceptance Tests: PASSED" -ForegroundColor Green
} else {
    Write-Host "✗ Acceptance Tests: FAILED" -ForegroundColor Red
}

# Exit with failure if any test failed
if (($e2eResult -ne 0) -or ($acceptanceResult -ne 0)) {
    exit 1
}
exit 0