# Print the current directory
Write-Output "Current Directory: $(Get-Location)"

# Print all environment variables
Get-ChildItem Env: | ForEach-Object { Write-Output "$($_.Name)=$($_.Value)" }
