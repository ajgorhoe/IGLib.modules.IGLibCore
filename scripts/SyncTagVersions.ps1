<#
.SYNOPSIS
  Synchronize tags across multiple repositories by tagging each repo's branch
  (default: main, with fallback to master) with a common version.

.DESCRIPTION
  Runs in two iterations over a set of local repositories:
  1) First iteration: for each repo, check out the target branch (main by default,
     fallback to master if main doesn't exist), optionally pull, then compute the
     version via GitVersion. Collects each repo’s version.
  2) Between iterations: choose the MAXIMUM version among collected versions
     (comparison by Major, then Minor, then Patch) as the base. Apply optional
     bumps/increments (Major/Minor/Patch). If -PreReleaseLabel is provided and a
     bump/increment occurred, re-apply as `X.Y.Z-<label>.1`. Final tag is prefixed
     with `v` if not already present (e.g., `v2.1.0`).
  3) Second iteration: tag & push the chosen version on the target branch for each
     repo (skips where the tag already matches what the repo would assign).

  Each repo visit:
    - verifies the directory is a valid Git repository root (for both passes)
    - restores the previous working directory and previously checked out branch
    - never throws to the caller; returns a result object with success/failure info

.PARAMETER RepoDirs
  Array of repository directory paths. Each can be absolute or relative to the
  script's directory. Each path must point to the ROOT of the git repo.

.PARAMETER Branch
  Branch to operate on. Default is 'main'. If 'main' does not exist in a repo,
  the first pass will fall back to 'master'. The effective branch used is stored
  per repo and reused in the second pass.

.PARAMETER Pull
  If specified, the first pass fetches tags and pulls latest changes (fast-forward
  only) before version calculation.

.PARAMETER BumpMajor
  If specified (and -IncrementMajor is 0), increments MAJOR by 1 for the final
  synchronized version (resets Minor/Patch to 0).

.PARAMETER BumpMinor
  If specified (and -IncrementMinor is 0), increments MINOR by 1 for the final
  synchronized version (resets Patch to 0).

.PARAMETER BumpPatch
  If specified (and -IncrementPatch is 0), increments PATCH by 1 for the final
  synchronized version.

.PARAMETER IncrementMajor
  Integer (default 0). If > 0, increments MAJOR by that amount (resets Minor/Patch to 0).
  Overrides -BumpMajor.

.PARAMETER IncrementMinor
  Integer (default 0). If > 0, increments MINOR by that amount (resets Patch to 0).
  Overrides -BumpMinor.

.PARAMETER IncrementPatch
  Integer (default 0). If > 0, increments PATCH by that amount.
  Overrides -BumpPatch.

.PARAMETER PreReleaseLabel
  Optional prerelease label applied AFTER a bump/increment.
  If provided AND a bump/increment was applied, the final tag becomes `X.Y.Z-<label>.1`.
  Allowed characters: [0-9A-Za-z-.]. Ignored if no bump/increment occurs.

.OUTPUTS
  Prints instructive progress and surveys. Returns nothing (script-style).

.EXAMPLE
  .\SyncTagVersions.ps1 -RepoDirs ..\RepoA, ..\RepoB
  # Computes versions on 'main' (fallback to 'master' per repo), picks the maximum,
  # then tags both repos with that version.

.EXAMPLE
  .\SyncTagVersions.ps1 -RepoDirs C:\src\RepoA, C:\src\RepoB -Pull -BumpPatch
  # Pulls first, calculates versions, picks the maximum, bumps PATCH by 1, and tags.

.EXAMPLE
  .\SyncTagVersions.ps1 -RepoDirs .\RepoA, .\RepoB -IncrementMinor 2 -PreReleaseLabel rc
  # Picks maximum, bumps MINOR by +2 to a stable X.(Y+2).0, then applies '-rc.1'.
#>

[CmdletBinding()]
param(
  [Parameter(Mandatory)]
  [string[]] $RepoDirs,

  [string] $Branch = 'main',

  [switch] $Pull,

  [switch] $BumpMajor,
  [switch] $BumpMinor,
  [switch] $BumpPatch,

  [int] $IncrementMajor = 0,
  [int] $IncrementMinor = 0,
  [int] $IncrementPatch = 0,

  [string] $PreReleaseLabel
)

$ErrorActionPreference = 'Stop'

# ---------- Utility helpers ----------

function Resolve-CanonicalPath {
  param([string]$PathText)

  if ([string]::IsNullOrWhiteSpace($PathText)) { return $PathText }
  # Absolute or relative to script directory
  $p =
    if ([IO.Path]::IsPathRooted($PathText)) { $PathText }
    else { Join-Path -Path $PSScriptRoot -ChildPath $PathText }

  $sep = [IO.Path]::DirectorySeparatorChar
  $p = $p -replace '[\\/]', [string]$sep
  $full = [IO.Path]::GetFullPath($p)
  if ($full.Length -gt 3) { $full = $full.TrimEnd($sep) }
  return $full
}

function Is-GitRoot {
  param([string]$Path)
  if (-not (Test-Path -LiteralPath $Path)) { return $false }
  # Quick check: .git folder exists
  if (-not (Test-Path -LiteralPath (Join-Path $Path '.git'))) { return $false }
  # Strong check: does git see this as work-tree AND is toplevel == path?
  $inside = (git -C "$Path" rev-parse --is-inside-work-tree 2>$null).Trim()
  if ($inside -ne 'true') { return $false }
  $top    = (git -C "$Path" rev-parse --show-toplevel    2>$null).Trim()
  if (-not $top) { return $false }
  # Normalize both
  $normTop  = Resolve-CanonicalPath $top
  $normPath = Resolve-CanonicalPath $Path
  return [String]::Equals($normTop, $normPath, [StringComparison]::OrdinalIgnoreCase)
}

function Ensure-GitVersionTool {
  try { $null = & dotnet gitversion /version; return $true }
  catch {
    Write-Host "  GitVersion.Tool not available here. Installing local tool..." -ForegroundColor Yellow
    if (-not (Test-Path -LiteralPath ".config/dotnet-tools.json")) {
      dotnet new tool-manifest | Out-Null
    }
    dotnet tool install GitVersion.Tool --version "*" | Out-Null
    try { $null = & dotnet gitversion /version; return $true } catch { return $false }
  }
}

function Get-GitVersionJson {
  # returns PSCustomObject or $null
  try {
    $raw = & dotnet gitversion /output json
    return ($raw | ConvertFrom-Json)
  } catch {
    return $null
  }
}

function Parse-SemVerBase {
  param([string]$SemVerLike) # e.g., "1.2.3", "1.2.3-beta.4+5"
  if ([string]::IsNullOrWhiteSpace($SemVerLike)) { return $null }
  $numeric = $SemVerLike.Split('-', 2)[0].Split('+', 2)[0]
  if ($numeric -notmatch '^(?<maj>\d+)\.(?<min>\d+)\.(?<pat>\d+)$') { return $null }
  [int[]]@([int]$Matches['maj'], [int]$Matches['min'], [int]$Matches['pat'])
}

function Compare-SemVerBase {
  param([string]$A, [string]$B)
  $pa = Parse-SemVerBase $A; $pb = Parse-SemVerBase $B
  if ($null -eq $pa -or $null -eq $pb) { return 0 } # treat unparsable as equal
  # returns 1 if A > B, -1 if A < B, 0 equal
  for ($i=0; $i -lt 3; $i++) {
    if ($pa[$i] -gt $pb[$i]) { return 1 }
    if ($pa[$i] -lt $pb[$i]) { return -1 }
  }
  return 0
}

function Compute-BumpedVersion {
  <#
    From a base SemVer (possibly with prerelease/build), apply one increment.
    Only one of the three increments may be > 0. Returns stable X.Y.Z string.
  #>
  param(
    [string]$SemVerBase,
    [int]$IncrementMajor,
    [int]$IncrementMinor,
    [int]$IncrementPatch
  )
  foreach ($n in @($IncrementMajor, $IncrementMinor, $IncrementPatch)) {
    if ($n -lt 0) { throw "Increments must be >= 0." }
  }

  $pos = @($IncrementMajor, $IncrementMinor, $IncrementPatch | Where-Object { $_ -gt 0 })
  if ($pos.Count -gt 1) { throw "Specify only one increment among Major/Minor/Patch." }
  if ($pos.Count -eq 0) { return $null } # no bump requested

  $parts = Parse-SemVerBase $SemVerBase
  if ($null -eq $parts) { throw "Unable to parse SemVer '$SemVerBase' for bumping." }

  $maj,$min,$pat = $parts

  if ($IncrementMajor -gt 0) { $maj += $IncrementMajor; $min = 0; $pat = 0 }
  elseif ($IncrementMinor -gt 0) { $min += $IncrementMinor; $pat = 0 }
  else { $pat += $IncrementPatch }

  return "{0}.{1}.{2}" -f $maj, $min, $pat
}

function Test-TagExistsLocal {
  param([string]$TagName)
  $null = git show-ref --verify --quiet "refs/tags/$TagName"
  return ($LASTEXITCODE -eq 0)
}

function Test-TagExistsRemote {
  param([string]$TagName, [string]$Remote = 'origin')
  $out = git ls-remote --tags $Remote "refs/tags/$TagName" 2>$null
  return -not [string]::IsNullOrWhiteSpace($out)
}

# ---------- Per-repo operations (iteration 1 & 2) ----------

function Invoke-RepoFirstPass {
  <#
    For a single repo:
      - validate root
      - checkout Branch (fallback to master if Branch=='main' and main missing)
      - optional pull
      - compute version via GitVersion
      - return object with fields (or $null on failure)
    Must NOT throw.
  #>
  param(
    [string]$OrigPath,  # as provided by user
    [string]$AbsPath,   # canonical absolute path
    [string]$Branch,
    [switch]$Pull
  )

  $result = [PSCustomObject]@{
    OrigPath     = $OrigPath
    AbsPath      = $AbsPath
    RepoName     = (Split-Path -Leaf $AbsPath)
    Branch       = $Branch
    UsedBranch   = $null
    Version      = $null
    Error        = $null
    Success      = $false
  }

  $origLoc = Get-Location
  $initialBranch = $null
  try {
    Set-Location -LiteralPath $AbsPath

    if (-not (Is-GitRoot $AbsPath)) {
      $result.Error = "Not a valid Git repository root."
      return $result
    }

    # Capture current branch
    $initialBranch = (git rev-parse --abbrev-ref HEAD 2>$null).Trim()

    # Try target branch; fallback to 'master' if requested 'main' and missing
    $target = $Branch
    $checkoutOk = $true
    try {
      git checkout "$target" 2>$null | Out-Null
    } catch {
      $checkoutOk = $false
    }

    if (-not $checkoutOk -and $Branch -eq 'main') {
      try {
        git rev-parse --verify --quiet "refs/heads/master" 2>$null | Out-Null
        if ($LASTEXITCODE -eq 0) {
          git checkout master 2>$null | Out-Null
          $target = 'master'
          $checkoutOk = $true
        }
      } catch { }
    }

    if (-not $checkoutOk) {
      $result.Error = "Cannot check out branch '$Branch' (no fallback)."
      return $result
    }

    $result.UsedBranch = $target

    if ($Pull) {
      Write-Host "  [$($result.RepoName)] pulling latest on '$target'..." -ForegroundColor DarkCyan
      git fetch --tags origin 2>$null | Out-Null
      git pull --ff-only 2>$null | Out-Null
    }

    # Ensure GitVersion tool is available (local manifest)
    if (-not (Ensure-GitVersionTool)) {
      $result.Error = "GitVersion.Tool could not be installed."
      return $result
    }

    $gv = Get-GitVersionJson
    if ($null -eq $gv -or [string]::IsNullOrWhiteSpace($gv.FullSemVer)) {
      $result.Error = "GitVersion did not return a valid version."
      return $result
    }

    $result.Version = $gv.FullSemVer
    $result.Success = $true
    return $result
  }
  catch {
    $result.Error = $_.Exception.Message
    return $result
  }
  finally {
    # restore branch and directory
    try {
      if ($initialBranch) {
        $curr = (git rev-parse --abbrev-ref HEAD 2>$null).Trim()
        if ($curr -and $curr -ne $initialBranch) { git checkout "$initialBranch" 2>$null | Out-Null }
      }
    } catch { }
    Set-Location $origLoc
  }
}

function Invoke-RepoSecondPass {
  <#
    For a single repo:
      - validate root
      - checkout the 'UsedBranch'
      - apply provided tag (if not already there)
      - push tag
      - recompute version (return it)
    Must NOT throw.
  #>
  param(
    [string]$OrigPath,
    [string]$AbsPath,
    [string]$UsedBranch,
    [string]$TagToApply
  )

  $result = [PSCustomObject]@{
    OrigPath     = $OrigPath
    AbsPath      = $AbsPath
    RepoName     = (Split-Path -Leaf $AbsPath)
    Branch       = $UsedBranch
    AppliedTag   = $TagToApply
    Recalc       = $null
    Skipped      = $false
    Error        = $null
    Success      = $false
  }

  $origLoc = Get-Location
  $initialBranch = $null
  try {
    Set-Location -LiteralPath $AbsPath

    if (-not (Is-GitRoot $AbsPath)) {
      $result.Error = "Not a valid Git repository root."
      return $result
    }

    $initialBranch = (git rev-parse --abbrev-ref HEAD 2>$null).Trim()

    # checkout the branch we used in pass 1
    try {
      git checkout "$UsedBranch" 2>$null | Out-Null
    } catch {
      $result.Error = "Cannot check out branch '$UsedBranch'."
      return $result
    }

    # Skip if tag already present locally or at origin
    $tag = $TagToApply
    if (Test-TagExistsLocal $tag) {
      $result.Skipped = $true
    } elseif (Test-TagExistsRemote $tag) {
      $result.Skipped = $true
    }

    if (-not $result.Skipped) {
      Write-Host "  [$($result.RepoName)] tagging '$UsedBranch' with '$tag'..." -ForegroundColor Green
      git tag -a "$tag" -m "Sync release $tag" 2>$null | Out-Null
      git push origin "$tag" 2>$null | Out-Null
    } else {
      Write-Host "  [$($result.RepoName)] tag '$tag' already exists (local or remote) — skipping." -ForegroundColor DarkYellow
    }

    if (-not (Ensure-GitVersionTool)) {
      $result.Error = "GitVersion.Tool could not be installed for recalc."
      return $result
    }

    $gv2 = Get-GitVersionJson
    if ($null -eq $gv2 -or [string]::IsNullOrWhiteSpace($gv2.FullSemVer)) {
      $result.Error = "GitVersion did not return a valid version after tagging."
      return $result
    }

    $result.Recalc  = $gv2.FullSemVer
    $result.Success = $true
    return $result
  }
  catch {
    $result.Error = $_.Exception.Message
    return $result
  }
  finally {
    # restore branch and directory
    try {
      if ($initialBranch) {
        $curr = (git rev-parse --abbrev-ref HEAD 2>$null).Trim()
        if ($curr -and $curr -ne $initialBranch) { git checkout "$initialBranch" 2>$null | Out-Null }
      }
    } catch { }
    Set-Location $origLoc
  }
}

# ---------- Parameter echo / pre-processing ----------

# Effective increments: integers override switches; switches imply +1
$effMaj = $IncrementMajor
$effMin = $IncrementMinor
$effPat = $IncrementPatch
if ($BumpMajor.IsPresent -and $effMaj -le 0) { $effMaj = 1 }
if ($BumpMinor.IsPresent -and $effMin -le 0) { $effMin = 1 }
if ($BumpPatch.IsPresent -and $effPat -le 0) { $effPat = 1 }

if ($PreReleaseLabel -and ($PreReleaseLabel -notmatch '^[0-9A-Za-z\-.]+$')) {
  throw "Invalid -PreReleaseLabel '$PreReleaseLabel'. Allowed: letters, digits, '-' and '.'"
}

Write-Host "=== SyncTagVersions parameters ===" -ForegroundColor Cyan
Write-Host ("Repos: {0}" -f ($RepoDirs -join ", "))
Write-Host ("Branch: {0}  (fallback to 'master' per-repo if 'main' missing)" -f $Branch)
Write-Host ("Pull: {0}" -f ($Pull.IsPresent))
Write-Host ("Increments -> Major:{0} Minor:{1} Patch:{2}" -f $effMaj, $effMin, $effPat)
Write-Host ("PreReleaseLabel: {0}" -f ($PreReleaseLabel ? $PreReleaseLabel : "<none>"))
Write-Host "==================================" -ForegroundColor Cyan

# Build the data table (one row per repo)
$rows = @()
foreach ($rp in $RepoDirs) {
  $rows += [PSCustomObject]@{
    OrigPath     = $rp
    AbsPath      = (Resolve-CanonicalPath $rp)
    RepoName     = (Split-Path -Leaf (Resolve-CanonicalPath $rp))
    Branch       = $Branch
    UsedBranch   = $null
    Version1     = $null
    FinalTag     = $null
    Version2     = $null
    Error1       = $null
    Error2       = $null
    Skipped2     = $false
  }
}

# ---------- First iteration: compute versions ----------

Write-Host "`n--- Pass 1: compute per-repo versions ---" -ForegroundColor Cyan
for ($i=0; $i -lt $rows.Count; $i++) {
  $row = $rows[$i]
  Write-Host ("[{0}/{1}] {2}" -f ($i+1), $rows.Count, $row.OrigPath) -ForegroundColor DarkCyan

  $r = Invoke-RepoFirstPass -OrigPath $row.OrigPath -AbsPath $row.AbsPath -Branch $row.Branch -Pull:$Pull
  if ($null -eq $r) {
    $row.Error1 = "Unknown error (null result)"
    continue
  }
  $row.UsedBranch = $r.UsedBranch
  if ($r.Success) {
    $row.Version1 = $r.Version
  } else {
    $row.Error1 = $r.Error
  }
}

# Survey after pass 1
Write-Host "`n=== Survey after Pass 1 (current versions) ===" -ForegroundColor Cyan
foreach ($row in $rows) {
  $verText = if ($row.Version1) { "'$($row.Version1)'" } else { "''" }
  Write-Host ("{0,-25}  Orig='{1}'  Branch={2}  Version={3}" -f $row.RepoName, $row.OrigPath, ($row.UsedBranch ?? $row.Branch), $verText)
}
Write-Host "==============================================" -ForegroundColor Cyan

# ---------- Choose base version (max by Major/Minor/Patch) ----------

$versionsForMax = $rows | Where-Object { $_.Version1 } | ForEach-Object {
  [PSCustomObject]@{
    Row = $_
    Base = $_.Version1
    Tuple = (Parse-SemVerBase $_.Version1)
  }
} | Where-Object { $_.Tuple -ne $null }

if (-not $versionsForMax -or $versionsForMax.Count -eq 0) {
  Write-Host "`nNo valid versions found in Pass 1; nothing to tag." -ForegroundColor Yellow
  return
}

# Find maximum
$maxItem = $versionsForMax | Sort-Object -Property @{Expression={$_.Tuple[0]};Descending=$true},
                                              @{Expression={$_.Tuple[1]};Descending=$true},
                                              @{Expression={$_.Tuple[2]};Descending=$true} |
           Select-Object -First 1

$selectedBase = $maxItem.Base
Write-Host ("`nSelected base version (max of Major/Minor/Patch over repos): {0}" -f $selectedBase) -ForegroundColor Green

# Apply increments (if any)
$finalBase = $selectedBase
$didBump   = $false
try {
  $maybe = Compute-BumpedVersion -SemVerBase $selectedBase -IncrementMajor $effMaj -IncrementMinor $effMin -IncrementPatch $effPat
  if ($maybe) { $finalBase = $maybe; $didBump = $true }
} catch {
  Write-Host "Error applying increments: $($_.Exception.Message)" -ForegroundColor Yellow
}

# Apply PreReleaseLabel only if a bump happened and label supplied
if ($didBump -and $PreReleaseLabel) {
  $finalVersion = "$finalBase-$PreReleaseLabel.1"
} else {
  # no bump => keep as-is (FullSemVer), including prerelease/build from the chosen repo
  $finalVersion = $finalBase
}

# Normalize tag (ensure 'v' prefix)
$finalTag = if ($finalVersion -match '^[vV]\d') { $finalVersion } else { "v$finalVersion" }
Write-Host ("Final synchronized tag to apply: {0}" -f $finalTag) -ForegroundColor Green

# ---------- Second iteration: tag all repos (skip identical) ----------

Write-Host "`n--- Pass 2: apply tag to all repos ---" -ForegroundColor Cyan
for ($i=0; $i -lt $rows.Count; $i++) {
  $row = $rows[$i]
  Write-Host ("[{0}/{1}] {2}" -f ($i+1), $rows.Count, $row.OrigPath) -ForegroundColor DarkCyan

  # Skip if repo already would assign exactly this tag (compare against pass1 version + 'v' normalization)
  $already = $false
  if ($row.Version1) {
    $rowFirstTag = if ($row.Version1 -match '^[vV]\d') { $row.Version1 } else { "v$($row.Version1)" }
    if ([String]::Equals($rowFirstTag, $finalTag, [StringComparison]::OrdinalIgnoreCase)) {
      $already = $true
    }
  }

  if ($already) {
    Write-Host "  [$($row.RepoName)] version already matches '$finalTag' — skipping tag." -ForegroundColor DarkYellow
    $row.FinalTag = $finalTag
    $row.Skipped2 = $true
    continue
  }

  $r2 = Invoke-RepoSecondPass -OrigPath $row.OrigPath -AbsPath $row.AbsPath -UsedBranch ($row.UsedBranch ?? $row.Branch) -TagToApply $finalTag
  if ($null -eq $r2) {
    $row.Error2 = "Unknown error (null result)"
    continue
  }

  $row.FinalTag = $finalTag
  $row.Skipped2 = $r2.Skipped
  if ($r2.Success) {
    $row.Version2 = $r2.Recalc
  } else {
    $row.Error2 = $r2.Error
  }
}

# Survey after pass 2
Write-Host "`n=== Survey after Pass 2 (tag results) ===" -ForegroundColor Cyan
foreach ($row in $rows) {
  $v1 = $row.Version1 ? "'$($row.Version1)'" : "''"
  $tag = $row.FinalTag ? "'$($row.FinalTag)'" : "''"
  $v2 = $row.Version2 ? "'$($row.Version2)'" : "''"
  $flag = if ($row.Skipped2) { " (skipped)" } elseif ($row.Error2) { " (error)" } else { "" }
  Write-Host ("{0,-25}  Orig='{1}'  Branch={2}  V1={3}  Tag={4}  V2={5}{6}" -f $row.RepoName, $row.OrigPath, ($row.UsedBranch ?? $row.Branch), $v1, $tag, $v2, $flag)
  if ($row.Error1) { Write-Host ("  Pass1 error: {0}" -f $row.Error1) -ForegroundColor DarkRed }
  if ($row.Error2) { Write-Host ("  Pass2 error: {0}" -f $row.Error2) -ForegroundColor DarkRed }
}
Write-Host "==========================================" -ForegroundColor Cyan
