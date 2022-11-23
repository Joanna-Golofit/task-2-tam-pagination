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

function BuildDotNet {
	Write-Host "****** Building BACKEND..."
	
	dotnet restore ".\src\backend"
	dotnet build ".\src\backend" --no-restore --no-incremental --configuration $configuration /p:Version=$version /p:TreatWarningsAsErrors=true /warnaserror
	
	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
	  throw "There was an error!"
	}
	
	Write-Host "****** Build backend app succedded"
}

function PublishBack {
	Write-Host "****** Copying BACKEND build to artifacts folder..."
	
	dotnet publish "src\backend\TeamsAllocationManager.Api" --no-build --configuration $configuration --output ".\artifacts\backend" /p:EnvironmentName=$envName

	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
	  throw "There was an error!"
	}
	
	Write-Host "****** Publish backend app succedded"
}

BuildDotNet
PublishBack