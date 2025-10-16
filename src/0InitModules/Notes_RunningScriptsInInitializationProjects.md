
> Q: What are the likely causes hanging of building of a project that references another project that runs a powershell script, when MSBuild is run in GitHub Actions (CI)? 

Here are some diagnostics and fixes you can apply to eliminate the possibility of hanging when building a project running a PowerShell script during build, and to get useful logs when you re-run the job.

What’s most likely

* A PowerShell script invoked during MSBuild (InitModulesGraphics3D) is blocking or waiting for input, or performing network/installer actions that stall on the CI runner. On a local laptop that script may finish quickly (or be interactive), but on GitHub runners it can hang indefinitely.
* Adding that project as a ProjectReference means msbuild will run the script as part of the solution build — causing the whole build step to block.
* The job log is empty because the run was cancelled by timeout; to get diagnostics we must re-run with improved logging and isolation.

Immediate troubleshooting steps (in order)

1. Re-run the workflow with verbose build logging so you can capture where it stalls

* In your workflow, change the build command to: dotnet build IGLibGraphics3D.sln \-v:diag
* Or at least: dotnet build IGLibGraphics3D.sln \-v:d This will show the msbuild target being executed and if it’s stuck in the InitModules target.

2. Isolate the offending project

* Temporarily remove the ProjectReference to InitModulesGraphics3D from projects that depend on it (IGLib.Graphics3D and IGLib.Graphics3D.Tests) and re-run the CI build. If the build completes quickly, the project/script is the cause.
* Alternative: set <ReferenceOutputAssembly>false</ReferenceOutputAssembly> on the ProjectReference so it is not built during solution build (useful for quick test):

<ProjectReference Include="..\\InitModulesGraphics3D\\InitModulesGraphics3D.csproj"> <ReferenceOutputAssembly>false</ReferenceOutputAssembly> </ProjectReference>

3. Avoid running interactive scripts during msbuild on CI

* Modify the InitModulesGraphics3D project so it does not execute the PowerShell script automatically on CI. Use the CI environment variable (GitHub Actions sets CI=true) or an explicit property to skip the target in CI.

Example: wrap the MSBuild Target so it runs only when NOT on CI

<!-- Add to InitModulesGraphics3D.csproj --> <PropertyGroup> <!-- Allow override from CI or command line --> <RunInitScript Condition="'$(RunInitScript)'==''">true</RunInitScript> </PropertyGroup> <Target Name="RunInitScript" BeforeTargets="Build" Condition="'$(CI)'!='true' and '$(RunInitScript)'=='true'"> <Exec Command="pwsh -NoProfile -NonInteractive -ExecutionPolicy Bypass -File &quot;$(MSBuildProjectDirectory)\\scripts\\init-modules.ps1&quot;" /> </Target>

* On GitHub Actions run the build with /p:RunInitScript=false or set environment variable CI=true (which already exists), OR call dotnet build with /p:RunInitScript=false in the workflow step.

4. Make the init script a dedicated workflow step (recommended)

* Instead of running the script as part of msbuild, invoke it explicitly in your workflow as a separate step (so it is visible and easily timed out), and ensure it runs with non-interactive flags and a step-level timeout.

Example (Linux/Ubuntu runner):

* name: Init modules (non-interactive, timeout 10m) run: timeout 10m pwsh \-NoProfile \-NonInteractive \-ExecutionPolicy Bypass ./InitModulesGraphics3D/scripts/init-modules.ps1 shell: pwsh

For Windows runner you can use the built-in Start-Process pattern or the GitHub job-level timeout. Example:

* name: Init modules (Windows) shell: `pwsh run: | $proc \= Start-Process pwsh \-ArgumentList '-NoProfile','-NonInteractive','-ExecutionPolicy','Bypass','-File','.\\InitModulesGraphics3D\\scripts\\init-modules.ps1' \-PassThru \-Wait if ($proc.ExitCode \-ne 0) { exit $proc.ExitCode }`
    
* Configure job-level timeout to a safe value (e.g. timeout-minutes: 30) in the workflow for quick fail-fast during debugging.

5. Make the script non-interactive and robust

* Edit init-modules.ps1 to ensure it never prompts:
  * Avoid Read-Host, interactive prompts
  * For Install-Module or Install-Package use \-Force and \-Confirm:$false
  * Use \-NonInteractive, \-NoProfile for pwsh
  * Add error handling and explicit timeouts if long network calls are made (e.g., wrap network calls with proper timeouts)
* Example pattern to make an Install-Module non-interactive:

if (-not (Get-Module \-ListAvailable \-Name SomeModule)) { Install-Module SomeModule \-Force \-SkipPublisherCheck \-Scope CurrentUser \-Confirm:$false }

6. Add a watchdog or explicit timeout around the script

* Since msbuild Exec has limited timeout controls, run the script from the workflow with an external timeout (as above using timeout on Linux) so the step fails and produces logs rather than hanging forever.

7. If the script must run during build, add a short diagnostic write before/after the script runs

* Add logging to the target so msbuild shows where it is:

<Target Name="RunInitScript" BeforeTargets="Build" Condition="'$(CI)'!='true'"> <Message Text="--- INIT MODULES START ---" Importance="high" /> <Exec Command="pwsh -NoProfile -NonInteractive -ExecutionPolicy Bypass -File &quot;$(MSBuildProjectDirectory)\\scripts\\init-modules.ps1&quot;" /> <Message Text="--- INIT MODULES END ---" Importance="high" /> </Target>

This makes it obvious in logs whether the script started and where it got stuck.

If you prefer minimal changes: disable the script during CI by passing /p:RunInitScript=false in the workflow dotnet build step and run the script separately in its own step. That is the least invasive fix and will immediately prevent the build step from hanging.

Suggested GitHub Actions snippet (complete flow)

* name: Run init script (separate, timed) run: timeout 10m pwsh \-NoProfile \-NonInteractive \-ExecutionPolicy Bypass ./InitModulesGraphics3D/scripts/init-modules.ps1 shell: pwsh
    
* name: Build solution (diagnostic verbosity) run: dotnet build IGLibGraphics3D.sln \-v:d /p:RunInitScript=false shell: pwsh
    

How to get useful logs for follow-up

* Re-run the workflow after applying the changes above (or just re-run with \-v:d and without the init script) so the runner produces logs.
* If it still hangs, capture the step logs and paste the relevant msbuild output lines around the last Message printed (the Target name or Exec command) — that will show exactly which command is stuck.

Summary (recommended quick fix)

1. Stop running the init script during solution build on CI (guard the MSBuild target using CI env or a property).
2. Run init script in its own workflow step with NonInteractive flags and a timeout wrapper.
3. Increase build verbosity (dotnet build \-v:d) for a re-run to capture the failing/stuck point.

If you re-run with these changes and paste the diagnostic msbuild output around where the log stops (or paste the init script), I will provide targeted changes to the script or msbuild target to guarantee CI-safe execution.

---

---

---

---

> Q: In .csproj project files, is there a way to define a property across multiple lines in the way that results in a single-line value (like back-tick in PowerShell or backslash in some other shells, at the end of the lines that are supposed to be continued)?

Short answer: there is no MSBuild/XML "line continuation" character like PowerShell's backtick. A property element preserves the newlines you put in it, so splitting the text across lines will include newline characters (or literal backticks) in the property value and break Exec. To get a single-line value you must either write it on one line or build it at evaluation time (MSBuild property functions or joining item values). Below are safe, practical options with ready-to-use snippets.

1) Simplest — put the command on one line
- The most straightforward fix: make the property a single line (escape quotes with &quot; in XML).

<PropertyGroup>
  <InitCommand1>$(_PSExe) -NoProfile $(_ExecPolicy) -File "$(MSBuildProjectDirectory)/../../../scripts/UpdateDepencencyRepos.ps1"</InitCommand1>
</PropertyGroup>

<Exec Command="$(InitCommand1)" ContinueOnError="true" />

2) Keep it readable, but build a single-line with string concat
- Use an MSBuild property function to concatenate parts into one string (no embedded newlines).

<PropertyGroup>
  <InitCommand1>$([System.String]::Concat($(_PSExe), ' -NoProfile ', $(_ExecPolicy), ' -File ', '\"$(MSBuildProjectDirectory)/../../../scripts/UpdateDepencencyRepos.ps1\"'))</InitCommand1>
</PropertyGroup>

<Exec Command="$(InitCommand1)" ContinueOnError="true" />

3) Keep it modular and readable with an ItemGroup + Join
- Define each token as an item, then join them with a single space.

<ItemGroup>
  <InitArg Include="$(_PSExe)" />
  <InitArg Include="-NoProfile" />
  <InitArg Include="$(_ExecPolicy)" />
  <InitArg Include="-File" />
  <InitArg Include="$(MSBuildProjectDirectory)/../../../scripts/UpdateDepencencyRepos.ps1" />
</ItemGroup>

<PropertyGroup>
  <InitCommand1>$([System.String]::Join(' ', '@(InitArg->'%(Identity)')'))</InitCommand1>
</PropertyGroup>

<Exec Command="$(InitCommand1)" ContinueOnError="true" />

4) Or avoid a property entirely — call Exec inline
- If you don't need the property elsewhere you can put the command directly in the Exec (still one line):

<Target Name="RunInitScript" BeforeTargets="Build">
  <Exec Command="$(_PSExe) -NoProfile $(_ExecPolicy) -File &quot;$(MSBuildProjectDirectory)/../../../scripts/UpdateDepencencyRepos.ps1&quot;" ContinueOnError="true" />
</Target>

Additional practical tips
- Always run PowerShell on CI non-interactively: use -NoProfile -NonInteractive -ExecutionPolicy Bypass.
- Use &quot; or escaped quotes when quoting paths inside XML.
- If you need readability in the project file, use approach (2) or (3). They let you write the command components on separate lines (as item entries or arguments) while the evaluated property contains a single-line command.
- For CI robustness consider moving heavy initialization into a separate workflow step instead of running it during msbuild (so it can be timed out and observed easily).

If you want, paste your exact .csproj fragment and I’ll convert it to one of the options above with correct escaping for your MSBuild variables.










