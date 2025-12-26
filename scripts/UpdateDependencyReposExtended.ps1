
# Clones or updates the dependency repositories for IGLibCore.
Write-Host "`n`nCloning / updating EXTENDED dependency repositories of IGLibCore ..."

# Get the script directory such that relative paths can be resolved:
$scriptPath = $MyInvocation.MyCommand.Path
$scriptDir = Split-Path $scriptPath -Parent
$scriptFilename = [System.IO.Path]::GetFileName($scriptPath)

Write-Host "Script directory: $scriptDir"

# First, call basic update script:
& $(join-path $scriptDir "UpdateDependencyRepos.ps1")

Write-Host "`nUpdating extended dependencies of IGLibCore:`n"

Write-Host "`nUpdating MathNetNumerics:"
& $(Join-Path $scriptDir "UpdateRepo_MathNetNumerics.ps1")


Write-Host "  ... updating IGLibCore EXTENDED dependencies complete.`n`n"

