Param (
	[Parameter(Mandatory=$true)]
	[string]
	$rgName,
	[Parameter(Mandatory=$true)]
	[string]
	$functionName,
	[Parameter(Mandatory=$true)]
	[string]
	$dbConnectionString
)

$script:errorStrings = @('exception:', 'error')
$script:errorStringsRegex = [string]::Join('|', $errorStrings)

function PackAndPublish {
	Param ([string]$path, [string]$appName)
	
	Write-Host '****** Compressing and publishing Azure functions...'
	
	Push-Location
	Set-Location $path
	
	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
	  throw "There was an error!"
	}
	
	Compress-Archive -Path * -DestinationPath "data.zip" -Force
	
	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
	  throw "There was an error!"
	}

	az functionapp deployment source config-zip --resource-group $rgName --name $appName --src "data.zip"
	
	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
	  throw "There was an error!"
	}

	Pop-Location
}

function ApplyDbMigrations {
	Param ([string]$path)
	Write-Host "****** Running DB migrations... path:" $path
	
	Push-Location
	Set-Location $path
	
	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
	  throw "There was an error!"
	}
	
	Get-Location

	$env:ConnectionStrings__SqlConnectionString=$dbConnectionString

	$outputMigr = & "C:\Program Files\dotnet-tools\dotnet-ef.exe" database update -s TeamsAllocationManager.Api -p TeamsAllocationManager.Database
	
	Write-Output $outputMigr

	$env:ConnectionStrings__SqlConnectionString=""
	
	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
		$env:ConnectionStrings__SqlConnectionString=""
		throw "Run DB migrations failed!"
	}
	
	if($outputMigr -match $errorStringsRegex){
		$errorText = $outputMigr -match $errorStringsRegex
		throw "Run DB migrations failed! Error text: $errorText"
	}
	
	#Remove-Item "$env:ARTIFACTS\$env:CI_COMMIT_SHORT_SHA\src_backend_for_db_migrations\*" -Force -Recurse -ErrorAction SilentlyContinue
	
	Write-Host "****** Run DB migrations succeded"
}

function  UploadStaticApp {
	Param ([string]$path, [string]$appName)
	
	Write-Host '****** UploadStaticApp...'
	
	az storage blob upload-batch -s $path -d '$web' --account-name $appName
	
	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
	  throw "There was an error!"
	}
}

PackAndPublish "$env:ARTIFACTS\$env:CI_COMMIT_SHORT_SHA\backend\" $functionName
ApplyDbMigrations "$env:ARTIFACTS\$env:CI_COMMIT_SHORT_SHA\src_backend_for_db_migrations\src\backend"
