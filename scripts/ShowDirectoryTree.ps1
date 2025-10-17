<#
.SYNOPSIS
    Displays a directory tree with indentation and optional depth control.

.DESCRIPTION
    This script shows the contents of a directory recursively up to a specified depth.
    Each entry is listed with its name and type (Dir/File), indented to reflect hierarchy.
    Hidden and system items can optionally be included using the -IncludeHidden switch.

.PARAMETER Path
    The base directory path to display. Defaults to the current directory.

.PARAMETER MaxDepth
    The maximum recursion depth. Defaults to unlimited depth.

.PARAMETER IncludeHidden
    Include hidden and system files and directories if specified.

.EXAMPLE
    .\Show-Tree.ps1 -Path "C:\Projects" -MaxDepth 2

.EXAMPLE
    .\Show-Tree.ps1 -Path "." -IncludeHidden

.NOTES
    Author: ChatGPT (GPT-5)
    Last updated: 2025-10-16
#>

param(
    [string]$Path = ".",
    [int]$MaxDepth = [int]::MaxValue,
    [switch]$IncludeHidden
)

function Show-Tree {
    <#
    .SYNOPSIS
        Recursive helper function for displaying directory trees.
    .DESCRIPTION
        Lists the files and directories under a given path with indentation.
        Traverses recursively up to a specified depth, and optionally includes hidden files.
    .PARAMETER Path
        The current directory to process.
    .PARAMETER MaxDepth
        The maximum recursion depth.
    .PARAMETER CurrentDepth
        The current recursion level (used internally).
    .PARAMETER IncludeHidden
        Include hidden/system items if specified.
    #>
    param(
        [string]$Path,
        [int]$MaxDepth,
        [int]$CurrentDepth = 0,
        [switch]$IncludeHidden
    )

    if ($CurrentDepth -ge $MaxDepth) { return }

    # Collect directory contents (with or without hidden files)
    if ($IncludeHidden) {
        $items = Get-ChildItem -LiteralPath $Path -Force | Sort-Object PSIsContainer, Name
    }
    else {
        $items = Get-ChildItem -LiteralPath $Path | Sort-Object PSIsContainer, Name
    }

    foreach ($item in $items) {
        $indent = ' ' * ($CurrentDepth * 2)
        $type = if ($item.PSIsContainer) { 'Dir ' } else { 'File' }
        Write-Host ("{0}{1} {2}" -f $indent, $type, $item.Name)

        # Recurse into subdirectories
        if ($item.PSIsContainer) {
            Show-Tree -Path $item.FullName -MaxDepth $MaxDepth -CurrentDepth ($CurrentDepth + 1) -IncludeHidden:$IncludeHidden
        }
    }
}

# --- Run the function with top-level parameters ---
Show-Tree -Path $Path -MaxDepth $MaxDepth -IncludeHidden:$IncludeHidden
