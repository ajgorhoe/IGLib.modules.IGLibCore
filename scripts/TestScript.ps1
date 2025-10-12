

# Run: .\TestScript.ps1 -RepoDirs @("ab", "cd") -Sw

# This is for testing transferring parameters and switches to called scripts.
# REMOVE when not needed any more.

[CmdletBinding()]
param(
  [string[]] $RepoDirs = @(),
  [switch]$Sw
  )


Write-Host "`n`nScript TestScript was called."

Write-Host "`nThis tests transfer of parameters to called scripts."

Write-Host ("`nRepoDirs: `n{0}" -f ($RepoDirs -join ", `n"))

if ($Sw) {
  Write-Host "Switch is set."
}
if (-not $Sw) {
  Write-Host "Switch is NOT set."
}

if ($Sw.IsPresent) {
  Write-Host "Switch is set, tested via IsPresent."
}
if (-not $Sw.IsPresent) {
  Write-Host "Switch is NOT set, tested via IsPresent."
}

[bool]$b = $Sw
Write-Host "Sw assigned to bool b: $b "
[bool] $a = $Sw.IsPresent
Write-Host "Sw.IsPresent assigned to bool a: $a "

Write-Host "Sw: $Sw "

Write-Host "Sv.IsPresent: $Sv.IsPresent "

if ($Sw) {
  Write-Host "`n`nSwitch is set, Running TestScript again with switch not set..."
  ./TestScript.ps1 -Sw:$(-not $Sw)
}

Write-Host "`n"

