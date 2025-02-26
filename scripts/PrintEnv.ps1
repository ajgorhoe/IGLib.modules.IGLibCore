# Print the current directory
Write-Output "`nOutput from PrintEnv.ps1:"
Write-Output "`nCurrent Directory: $(Get-Location)"

# Print the contents of the current directory
Write-Output "`n`nCurrent Directory Contents:`n"
Get-ChildItem | ForEach-Object { Write-Output $_.FullName }

# Print all environment variables
Write-Output "`n`nEnvironment variables:"
Get-ChildItem Env: | ForEach-Object { Write-Output "$($_.Name)=$($_.Value)" }
