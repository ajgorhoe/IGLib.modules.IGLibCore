
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

Write-Host "Nothing to do in the extended depenndencies section.`n"

# Already in the basic update script (UpdateDependencyRepos):
# Write-Host "`nUpdating IGLibScripts:"
# & $(Join-Path $scriptDir "UpdateRepo_IGLibScripts.ps1")


Write-Host "  ... updating IGLibCore EXTENDED dependencies complete.`n`n"

