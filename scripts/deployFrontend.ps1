Param (
	[Parameter(Mandatory=$true)]
	[string]
	$frontendName
)

function  UploadStaticApp {
	Param ([string]$path, [string]$appName)
	
	Write-Host '****** UploadStaticApp...'
	
	az storage blob upload-batch -s $path -d '$web' --account-name $appName
	
	if ((-Not $?) -or ($LASTEXITCODE -ne 0 -and $LASTEXITCODE -ne $null)) {
	  throw "There was an error!"
	}
}

UploadStaticApp "$env:ARTIFACTS\$env:CI_COMMIT_SHORT_SHA\frontend\" $frontendName
