Push-Location
Set-Location "..\src\backend"

dotnet ef migrations remove -s TeamsAllocationManager.Api -p TeamsAllocationManager.Database

Pop-Location