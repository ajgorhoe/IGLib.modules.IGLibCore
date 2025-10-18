
# Clones or updates the depencencies repositories for IGLibCore repo.
Write-Host "`n`nCloning / updating basic dependency repositories of IGLibCore repository ...`n"

# Get the script directory such that relative paths can be resolved:
$scriptPath = $MyInvocation.MyCommand.Path
$scriptDir = Split-Path $scriptPath -Parent
$scriptFilename = [System.IO.Path]::GetFileName($scriptPath)

Write-Host "Script directory: $scriptDir"

Write-Host "`nUpdating IGLibScripts:"
& $(Join-Path $scriptDir "UpdateRepo_IGLibScripts.ps1")

Write-Host "  ... updating IGLibCore dependencies complete.`n`n"

