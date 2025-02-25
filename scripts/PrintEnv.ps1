# Print the current directory
Write-Output "Current Directory: $(Get-Location)"

# Print all environment variables
Get-ChildItem Env: | ForEach-Object { Write-Output "$($_.Name)=$($_.Value)" }

# Print the contents of the current directory
Write-Output "Directory Contents:"
Get-ChildItem | ForEach-Object { Write-Output $_.FullName }
