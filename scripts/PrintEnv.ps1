# Print the current directory
Write-Output "`nOutput from PrintEnv.ps1:"
Write-Output "`nCurrent Directory: $(Get-Location)"

# Print the contents of the current directory
Write-Output "`n`nCurrent Directory Contents:"
Get-ChildItem | ForEach-Object {
    $name = $_.Name
    $lastWriteTime = $_.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
    $length = $_.Length
    Write-Output "$name`t$lastWriteTime`t$length"
}

# Print all environment variables
Write-Output "`n`nEnvironment variables:"
Get-ChildItem Env: | ForEach-Object { Write-Output "$($_.Name)=$($_.Value)" }
