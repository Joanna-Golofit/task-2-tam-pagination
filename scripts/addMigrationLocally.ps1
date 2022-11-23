Param (
	[Parameter(Mandatory=$true)]
	[string]
	$newMigrationName
)

Push-Location
Set-Location "..\src\backend"

dotnet ef migrations add $newMigrationName -s TeamsAllocationManager.Api -p TeamsAllocationManager.Database

Pop-Location