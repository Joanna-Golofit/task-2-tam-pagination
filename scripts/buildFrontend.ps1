Param (
  [Parameter(Mandatory=$true)]
  [string]
  $configuration,
  [Parameter(Mandatory=$true)]
  [string]
  $version,
  [Parameter(Mandatory=$true)]
  [string]
  $commit,
  [Parameter(Mandatory=$true)]
  [string]
  $envName
)
	
function BuildFront {
	Write-Host "****** Building FRONTEND..."
	Push-Location
	
	Set-Location "src\frontend\"
	
	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
	  throw "There was an error!"
	}
	
	if(($env:CI_ENVIRONMENT_NAME -eq "stage") -and ($env:REACT_APP_VERSION -like "*.")){
		$env:REACT_APP_VERSION="$($env:REACT_APP_VERSION)development"
	}
	
	Write-Host $env:REACT_APP_VERSION
	
	Write-Host "Installing npm modules..."
	npm install --no-progress
	Write-Host "Building..."
	npm run build

	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
	  throw "There was an error!"
	}
	
	Write-Host "****** Build front app succedded"	

	Pop-Location
}

function PublishFront {
	Write-Host "****** Copying FRONTEND build to artifacts folder..."
	robocopy "src\frontend\build" "artifacts\frontend" /e /np /njh /nfl /ndl

	if ($LASTEXITCODE -gt 8 -and $LASTEXITCODE -ne $null) {
	  throw "There was an error! exit code=$LASTEXITCODE"
	}
	
	Write-Host "****** Publish front app succedded"	
}

BuildFront
PublishFront
