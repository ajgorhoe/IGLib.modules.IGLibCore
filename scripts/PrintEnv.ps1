# Print the current directory
Write-Output "`nOutput from PrintEnv.ps1:"
Write-Output "`nCurrent Directory: $(Get-Location)"

# Print the contents of the current directory
Write-Output "`n`nCurrent Directory and some subdirectories contents:"
Get-ChildItem | ForEach-Object {
    $name = $_.Name
    $lastWriteTime = $_.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
    $length = $_.Length
    Write-Output "$name`t$lastWriteTime`t$length"
}


# Print the contents of some subdirectories:
Write-Output "`n`nSome other directories' contents:"

# Array of relative directory paths:
$directories = @("_external", "IGLibCore", "tests")

# Check if the array is null or empty
if ($null -eq $directories -or $directories.Count -eq 0) {
    Write-Output "The directories array is null or empty."
} else {
    foreach ($dir in $directories) {
        # Check if the directory exists
        if (Test-Path -Path $dir) {
            # Get the absolute path of the directory
            $absolutePath = (Resolve-Path -Path $dir).Path
            Write-Output "Directory: $absolutePath"
            
            # List the contents of the directory
            Get-ChildItem -Path $dir | ForEach-Object {
                $name = $_.Name
                $lastWriteTime = $_.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
                $length = $_.Length
                Write-Output "$name`t$lastWriteTime`t$length"
            }
            Write-Output "" # Add a blank line between directories
        } else {
            Write-Output "Directory '$dir' does not exist."
        }
    }
}

# Print all environment variables
Write-Output "`n`nEnvironment variables:"
Get-ChildItem Env: | ForEach-Object { Write-Output "$($_.Name)=$($_.Value)" }
