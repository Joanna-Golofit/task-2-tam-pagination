<#
.SYNOPSIS
    .
.DESCRIPTION
    Script used to run frontend code analysis.
#>

Push-Location
Set-Location "src\frontend"

Write-Host "****** Installing npm modules..."
npm install --no-audit --no-progress

Write-Host "****** Linting..."
npm run lint

if (-Not $?) {
  Pop-Location
  throw "Linting failed!"
}

Write-Host "****** Analyzing circular dependencies..."
npm run madge

if (-Not $?) {
  Pop-Location
  throw "Circular dependency in ClientApp frontend app!"
}

Pop-Location

Write-Host "****** Frontend code analysis succeeded"
