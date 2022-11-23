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

function ClearFolder {
	Remove-Item "$env:ARTIFACTS\$env:CI_COMMIT_SHORT_SHA\*" -Force -Recurse -ErrorAction SilentlyContinue
}

function CopyAll {
	Write-Host "****** Archiving artifacts... "
	
	robocopy "artifacts" "$env:ARTIFACTS\$env:CI_COMMIT_SHORT_SHA" /e /np /njh /nfl /ndl
	
	if ($LASTEXITCODE -gt 8 -and $LASTEXITCODE -ne $null) {
	  throw "There was an error! exit code=$LASTEXITCODE"
	}
}

function CopyBackendSourceForDbMigrations {
	Write-Host "****** Copying backend code-base for db migrations..."
	
	robocopy "src\backend" "$env:ARTIFACTS\$env:CI_COMMIT_SHORT_SHA\src_backend_for_db_migrations\src\backend" /e /np /njh /nfl /ndl
	
	if ($LASTEXITCODE -gt 8 -and $LASTEXITCODE -ne $null) {
	  throw "There was an error! exit code=$LASTEXITCODE"
	}
	
	robocopy "deployment\database" "$env:ARTIFACTS\$env:CI_COMMIT_SHORT_SHA\src_backend_for_db_migrations\deployment\database" /e /np /njh /nfl /ndl
	
	if ($LASTEXITCODE -gt 8 -and $LASTEXITCODE -ne $null) {
	  throw "There was an error! exit code=$LASTEXITCODE"
	}
}

ClearFolder
CopyAll
CopyBackendSourceForDbMigrations