Push-Location
Set-Location "..\src\backend"

dotnet ef database update -s TeamsAllocationManager.Api -p TeamsAllocationManager.Database

Pop-Location