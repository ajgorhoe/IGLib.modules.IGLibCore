<#
Tags a branch (default: main) with a version computed by GitVersion.
- If -Directory is omitted: script may run from any folder INSIDE a Git repo.
- If -Directory is provided: it MUST be the repo root.
#>

[CmdletBinding()]
param(
  [string] $Directory,
  [string] $Branch = 'main',
  [switch] $Pull,
  [switch] $BumpMajor,
  [switch] $BumpMinor,
  [switch] $BumpPatch
)

$ErrorActionPreference = 'Stop'

function Resolve-TargetDirectory {
  param([string]$Dir)

  if ([string]::IsNullOrWhiteSpace($Dir)) { return $PSScriptRoot }

  if ([System.IO.Path]::IsPathRooted($Dir)) {
    return (Resolve-Path -LiteralPath $Dir).ProviderPath
  } else {
    $combined = Join-Path -Path $PSScriptRoot -ChildPath $Dir
    return (Resolve-Path -LiteralPath $combined).ProviderPath
  }
}

function Normalize-PathCanonical {
  param([string]$PathText)
  if ([string]::IsNullOrWhiteSpace($PathText)) { return $PathText }
  $sep = [IO.Path]::DirectorySeparatorChar
  $p = $PathText -replace '[\\/]', [string]$sep
  $full = [IO.Path]::GetFullPath($p)
  if ($full.Length -gt 3) { $full = $full.TrimEnd($sep) }
  return $full
}

function Get-GitTopLevel {
  param([string]$Path)
  $top = (git -C "$Path" rev-parse --show-toplevel 2>$null).Trim()
  if (-not $top) { return $null }
  return (Normalize-PathCanonical $top)
}

function Assert-GitRepository {
  param(
    [string]$Path,
    [bool]  $RequireTopLevel
  )
  $inside = (git -C "$Path" rev-parse --is-inside-work-tree 2>$null).Trim()
  if ($inside -ne 'true') {
    if ($RequireTopLevel) { throw "Path '$Path' is not the root of a Git repository (or not a working tree)." }
    else { throw "Path '$Path' is not inside a Git working tree." }
  }
  if ($RequireTopLevel) {
    $repoRoot = Get-GitTopLevel -Path $Path
    if (-not $repoRoot) { throw "Could not determine repository toplevel for '$Path'." }
    $normPath = Normalize-PathCanonical $Path
    if (-not [String]::Equals($repoRoot, $normPath, [System.StringComparison]::OrdinalIgnoreCase)) {
      throw "Path mismatch: repo toplevel is '$repoRoot' but script targeted '$normPath'."
    }
  }
}

function Ensure-GitVersionTool {
  try { $null = & dotnet gitversion /version; return }
  catch {
    Write-Host "GitVersion.Tool not available. Installing locally (tool manifest)..." -ForegroundColor Yellow
    if (-not (Test-Path -LiteralPath ".config/dotnet-tools.json")) { dotnet new tool-manifest | Out-Null }
    dotnet tool install GitVersion.Tool --version "*" | Out-Null
  }
}

function Get-GitVersionJson {
  Ensure-GitVersionTool
  $raw = & dotnet gitversion /output json
  return ($raw | ConvertFrom-Json)
}

function Compute-BumpedVersion {
  param(
    [string]$SemVerBase,
    [switch]$Maj, [switch]$Min, [switch]$Pat
  )
  $count = @($Maj, $Min, $Pat | Where-Object { $_ }).Count
  if ($count -gt 1) { throw "Provide only one of -BumpMajor, -BumpMinor, or -BumpPatch." }
  if ($count -eq 0) { return $null }

  $numeric = $SemVerBase.Split('-', 2)[0].Split('+', 2)[0]
  if ($numeric -notmatch '^(?<maj>\d+)\.(?<min>\d+)\.(?<pat>\d+)$') {
    throw "Unable to parse SemVer '$SemVerBase' for bumping."
  }
  $maj = [int]$Matches['maj']; $min = [int]$Matches['min']; $pat = [int]$Matches['pat']
  if ($Maj) { $maj += 1; $min = 0; $pat = 0 }
  elseif ($Min) { $min += 1; $pat = 0 }
  else { $pat += 1 }
  return "{0}.{1}.{2}" -f $maj, $min, $pat
}

function Test-TagExistsLocal {
  param([string]$TagName)
  # Uses exit code instead of try/catch
  $null = git show-ref --verify --quiet "refs/tags/$TagName"
  return ($LASTEXITCODE -eq 0)
}

function Test-TagExistsRemote {
  param([string]$TagName, [string]$Remote = 'origin')
  # Returns non-empty output if remote has the tag
  $out = git ls-remote --tags $Remote "refs/tags/$TagName" 2>$null
  return -not [string]::IsNullOrWhiteSpace($out)
}

# Preserve caller's working dir
$orig = Get-Location
try {
  $targetDir = Resolve-TargetDirectory -Dir $Directory
  Set-Location -LiteralPath $targetDir

  $requireRoot = -not [string]::IsNullOrWhiteSpace($Directory)
  Assert-GitRepository -Path (Get-Location).Path -RequireTopLevel:$requireRoot
  if (-not $requireRoot) {
    $root = Get-GitTopLevel -Path (Get-Location).Path
    if ($root) { Write-Host "Detected repo root: $root" -ForegroundColor DarkGray }
  }

  # Switch branch
  $currentBranch = (git rev-parse --abbrev-ref HEAD).Trim()
  if ($currentBranch -ne $Branch) {
    Write-Host "Checking out branch '$Branch' (was '$currentBranch')..." -ForegroundColor Cyan
    git checkout "$Branch" | Out-Null
  }

  if ($Pull) {
    Write-Host "Fetching and pulling latest changes..." -ForegroundColor Cyan
    git fetch --tags origin 2>$null | Out-Null
    git pull --ff-only 2>$null | Out-Null
  }

  $gv = Get-GitVersionJson

  $bumped = Compute-BumpedVersion -SemVerBase $gv.SemVer `
            -Maj:$BumpMajor -Min:$BumpMinor -Pat:$BumpPatch

  if ($bumped) {
    $versionToTag = $bumped
    $tagMessage   = "Release $versionToTag (bumped from $($gv.FullSemVer))"
  } else {
    $versionToTag = $gv.FullSemVer
    $tagMessage   = "Release $versionToTag"
  }

  $tagName = if ($versionToTag -match '^[vV]\d') { $versionToTag } else { "v$versionToTag" }

  # ✅ Fixed: check exit codes / output rather than try/catch
  if (Test-TagExistsLocal $tagName) { throw "Tag '$tagName' already exists locally. Aborting." }
  if (Test-TagExistsRemote $tagName) { throw "Tag '$tagName' already exists on 'origin'. Aborting." }

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
