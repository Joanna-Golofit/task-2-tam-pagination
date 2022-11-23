<#
.SYNOPSIS
    .
.DESCRIPTION
    Script used to run frontend tests. Generates junit xml and html report.
#>
New-Item "artifacts" -type Directory -Force | Out-Null
New-Item "artifacts\tests" -type Directory -Force | Out-Null
New-Item "artifacts\tests\frontend" -type Directory -Force | Out-Null

$failed = $false
$projectPath = (Resolve-Path .\).Path

Push-Location
Set-Location "src\frontend\"
yarn install --no-progress --silent
yarn test:ci

if (-Not $?) {
  $failed = $true
}

Copy-Item "test-results.xml" -Destination "..\..\..\..\artifacts\tests\frontend\frontend-client.xml"

Pop-Location

$xsl = New-Object System.Xml.Xsl.XslCompiledTransform
$xsl.Load("$projectPath\scripts\testHtml.xsl")
$xsl.Transform("$projectPath\artifacts\tests\frontend\frontend-client.xml", "$projectPath\artifacts\tests\frontend\frontend-client.html")

if ($failed) {
  Pop-Location
  throw "Client frontend app tests failed!"
}