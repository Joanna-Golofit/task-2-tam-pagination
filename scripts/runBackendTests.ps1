<#
.SYNOPSIS
    .
.DESCRIPTION
    Script used to run backend tests. Generates junit xml and html report.
#>
$failed = $false
$projectPath = (Resolve-Path .\).Path

$Env:SolutionDir = Resolve-Path "src\"

Push-Location
Set-Location "src\backend"

$testAssemblies = Get-ChildItem -Path **\*.Tests.csproj -Recurse
$testAssemblies | ForEach-Object {
  $trx = "$($projectPath)\artifacts\tests\backend\$($_.BaseName).trx"
  $junit = "$($projectPath)\artifacts\tests\backend\$($_.BaseName).xml"
  Remove-Item $trx -ErrorAction Ignore
  Remove-Item $junit -ErrorAction Ignore
  
  dotnet test $_.FullName --configuration Release --verbosity q --logger "trx;logfilename=$trx"
  if (-Not $?) {
    $failed = $true
  }

  $xmlwriter = [System.Xml.XmlWriter]::Create($junit)
  $xslt = New-Object System.Xml.Xsl.XslCompiledTransform
  $arglist = New-object System.Xml.Xsl.XsltArgumentList

  $arglist.AddParam("name", "", $_.BaseName)
  $xslt.Load("$projectPath\scripts\trx.xsl")
  $xslt.Transform($trx, $arglist, $xmlwriter)
  $xmlwriter.Close()
}

Pop-Location

$allTests = "$projectPath\artifacts\tests\backend\backend.xml"
$finalXml = "<testsuites>"
$testResults = Get-ChildItem -Path artifacts\tests\backend\*.xml -Recurse
$testResults | ForEach-Object {
  [xml]$xml = Get-Content $_.FullName
  $finalXml += Select-Xml -Xml $xml -XPath "//testsuites"
}
$finalXml += "</testsuites>"

$finalXml | Set-Content $allTests -Force

$xsl = New-Object System.Xml.Xsl.XslCompiledTransform
$xsl.Load("$projectPath\scripts\testHtml.xsl")
$xsl.Transform($allTests, "$projectPath\artifacts\tests\backend\backend.html")

if ($failed) {
  Pop-Location
  throw "Tests failed!"
}
