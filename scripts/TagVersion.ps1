<#
.SYNOPSIS
  Tags the specified branch (default: main) with a version computed by GitVersion,
  optionally bumping major/minor/patch beyond what GitVersion returns.

.DESCRIPTION
  - Runs in the script's directory by default. Use -Directory to target another repo path
    (relative to script location or absolute path).
  - Validates that the target directory is a Git repository.
  - Optionally pulls latest changes before calculating version (-Pull).
  - Supports -Branch to tag a branch other than 'main'.
  - Supports one of -BumpMajor / -BumpMinor / -BumpPatch to increment beyond GitVersion.
  - Restores the original working directory even on error.

.NOTES
  Requires:
    - Git
    - .NET SDK
    - GitVersion.Tool (global or local). If not present as a local tool,
      the script will auto-create a local tool manifest and install it.

#>

[CmdletBinding()]
param(
  [string] $Directory,                       # Optional: repo path (relative to script dir or absolute)
  [string] $Branch = 'main',                 # Branch to tag
  [switch] $Pull,                            # Pull latest before tagging
  [switch] $BumpMajor,
  [switch] $BumpMinor,
  [switch] $BumpPatch
)

$ErrorActionPreference = 'Stop'

function Resolve-TargetDirectory {
  param([string]$Dir)

  if ([string]::IsNullOrWhiteSpace($Dir)) {
    return $PSScriptRoot
  }

  if ([System.IO.Path]::IsPathRooted($Dir)) {
    return (Resolve-Path -LiteralPath $Dir).ProviderPath
  } else {
    $combined = Join-Path -Path $PSScriptRoot -ChildPath $Dir
    return (Resolve-Path -LiteralPath $combined).ProviderPath
  }
}

function Assert-GitRepository {
  param([string]$Path)

  # At minimum, require .git directory
  if (-not (Test-Path -LiteralPath (Join-Path $Path '.git'))) {
    throw "Directory '$Path' does not contain a .git folder. Not a Git repository."
  }

  # Stronger checks
  $inside = (git -C "$Path" rev-parse --is-inside-work-tree).Trim()
  if ($inside -ne 'true') {
    throw "Path '$Path' is not recognized by Git as a working tree."
  }

  # Ensure the toplevel equals the path we think it is
  $top = (git -C "$Path" rev-parse --show-toplevel).Trim()
  $normPath = (Resolve-Path -LiteralPath $Path).ProviderPath
  if ($top -ne $normPath) {
    throw "Path mismatch: repo toplevel is '$top' but script targeted '$normPath'."
  }
}

function Ensure-GitVersionTool {
  # Try a quick probe
  try {
    $null = & dotnet gitversion /version
    return
  } catch {
    Write-Host "GitVersion.Tool not available as a dotnet tool here. Installing locally..." -ForegroundColor Yellow
    if (-not (Test-Path -LiteralPath ".config/dotnet-tools.json")) {
      dotnet new tool-manifest | Out-Null
    }
    dotnet tool install GitVersion.Tool --version "*" | Out-Null
  }
}

function Get-GitVersionJson {
  Ensure-GitVersionTool
  # Fetch full JSON once
  $raw = & dotnet gitversion /output json
  return ($raw | ConvertFrom-Json)
}

function Compute-BumpedVersion {
  param(
    [string]$SemVerBase,   # e.g. 2.0.26 or 2.0.26-beta.5 (we'll sanitize)
    [switch]$Maj,
    [switch]$Min,
    [switch]$Pat
  )

  # Disallow multiple bump switches
  $count = @($Maj, $Min, $Pat | Where-Object { $_ }).Count
  if ($count -gt 1) {
    throw "Provide only one of -BumpMajor, -BumpMinor, or -BumpPatch."
  }

  # If no bump requested, return $null so caller can decide to use FullSemVer
  if ($count -eq 0) { return $null }

  # Strip prerelease/build if present (anything after first '-')
  $numeric = $SemVerBase.Split('-', 2)[0]  # e.g., "2.0.26"
  # Also strip any +build metadata if present (shouldn't be in SemVer typically)
  $numeric = $numeric.Split('+', 2)[0]

  if ($numeric -notmatch '^(?<maj>\d+)\.(?<min>\d+)\.(?<pat>\d+)$') {
    throw "Unable to parse SemVer '$SemVerBase' for bumping."
  }

  $maj = [int]$Matches['maj']
  $min = [int]$Matches['min']
  $pat = [int]$Matches['pat']

  if ($Maj) {
    $maj += 1; $min = 0; $pat = 0
  } elseif ($Min) {
    $min += 1; $pat = 0
  } else {
    $pat += 1
  }

  return "{0}.{1}.{2}" -f $maj, $min, $pat
}

# Preserve caller's working dir
$orig = Get-Location
try {
  $targetDir = Resolve-TargetDirectory -Dir $Directory
  Set-Location -LiteralPath $targetDir

  # Validate repository
  Assert-GitRepository -Path (Get-Location).Path

  # Switch branch if needed
  $currentBranch = (git rev-parse --abbrev-ref HEAD).Trim()
  if ($currentBranch -ne $Branch) {
    Write-Host "Checking out branch '$Branch' (was '$currentBranch')..." -ForegroundColor Cyan
    git checkout "$Branch" | Out-Null
  }

  # Optionally update from origin
  if ($Pull) {
    Write-Host "Fetching and pulling latest changes (with tags)..." -ForegroundColor Cyan
    git fetch --tags origin | Out-Null
    git pull --ff-only | Out-Null
  }

  # Compute version via GitVersion
  $gv = Get-GitVersionJson

  # Decide which version to tag:
  # - If a bump is requested, use bumped SemVer (stable).
  # - Else, use FullSemVer for fidelity (may include -beta.N / +metadata).
  $bumped = Compute-BumpedVersion -SemVerBase $gv.SemVer `
                                  -Maj:$BumpMajor -Min:$BumpMinor -Pat:$BumpPatch

  if ($bumped) {
    $versionToTag = $bumped
    $tagMessage   = "Release $versionToTag (bumped from $($gv.FullSemVer))"
  } else {
    $versionToTag = $gv.FullSemVer
    $tagMessage   = "Release $versionToTag"
  }

  # Normalize tag with 'v' prefix
  $tagName = if ($versionToTag -match '^[vV]\d') { $versionToTag } else { "v$versionToTag" }

  # Ensure tag doesn't already exist
  $exists = $false
  try {
    $null = git rev-parse -q --verify "refs/tags/$tagName" 2>$null
    $exists = $true
  } catch { $exists = $false }

  if ($exists) {
    throw "Tag '$tagName' already exists. Aborting."
  }

  # Create annotated tag and push
  Write-Host "Tagging '$Branch' at $(git rev-parse --short HEAD) with '$tagName'..." -ForegroundColor Green
  git tag -a "$tagName" -m "$tagMessage"
  Write-Host "Pushing tag '$tagName' to origin..." -ForegroundColor Green
  git push origin "$tagName"

  Write-Host "Done. Created and pushed tag: $tagName" -ForegroundColor Green
}
catch {
  Write-Error $_.Exception.Message
  throw
}
finally {
  Set-Location $orig
}
