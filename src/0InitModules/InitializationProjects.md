
# Initialization MSBuild (C#) Projects

See the [example project file, `InitModulesEventAggregator.csproj`](./InitModulesEventAggregator/InitModulesEventAggregator.csproj) ([on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibEventAggregator/blob/main/src/0InitModules/InitModulesEventAggregator/InitModulesEventAggregator.csproj)). 

## Executive Summary

What **we need** is a **utility project** in MSBuild that:

* Produces **no DLL/EXE**
  * As a compromise solution, such outputs can be produced, and in the referencing project the reference is included wih `<ProjectReference Include="xx.csproj PrivateAssets="all" ReferenceOutputAssembly="false" />`
* Runs **PowerShell scripts** when built
* Supports **incremental build** (only rebuilds when inputs change, or when explicitly cleaned/rebuilt)
* Behaves like a normal project in dependency chains (so dependent projects force it to build first, but don’t rerun scripts if it’s already up-to-date).

### Implementation

One can  do this **without faking it with a dummy output file** and **without dummy source files** - MSBuild has a concept of *inputs* and *outputs* for a target, which can be utilized for our purpose.

Set **`TargetFramework`** to **`netstandard2.0`** to maximize usability. This avoids issues when changing target frameworks in referencing projects.

When **referencing the project**, use the `PrivateAssets="all"` and `ReferenceOutputAssembly="false"` attributes in the project file:

~~~xml
  <ItemGroup>
  <ProjectReference Include="InitDependencies.csproj"
    PrivateAssets="all" ReferenceOutputAssembly="false" />
  </ItemGroup>
~~~

Below are the main points on how to create and reference an initialization project that performs initialization tasks for several projects before these projects are built, which typically includes tunning scripts that clone / update the dependency repositories.

### Initialization Project like a Normal Project

A normal library project can be used as initialization project. This provides a good guaranty that build behavior will be the same as with normal dependencies (especially the **incremental build**).

Set **`TargetFramework`** to **`netstandard2.0`** to maximize usability. This avoids issues when changing target frameworks in referencing projects.

Use settings that **prevent generation of output files** such as a DDLL in bin directory.

Example top-most configuration of properties:

~~~xml
<PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <OutputType>Library</OutputType>
    <!-- Prevent generation of assembly info, build output and packing: -->
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <CopyBuildOutputToOutputDirectory>false</CopyBuildOutputToOutputDirectory>
    <IncludeBuildOutput>false</IncludeBuildOutput>
    <IsPackable>false</IsPackable>
    <!-- Ensure that the post-build event is run even if the project is up-to-date and does not need building: 
    <RunPostBuildEvent>OnOutputUpdated</RunPostBuildEvent>
    -->
</PropertyGroup>
~~~

The project can include a **dummy source file**, but this is **not necessary** and is better avoided. Copying a **dummy output file** that is copied to project output directory is also **not necessary**.

### Running Initialization Scripts

We usually choose to use **PowerShell scripts**, as this is best for cross-platform builds (these often occur e.g. due to development on Windows, and CI/CD or server deployment on Linux).

It is a good practice to assemble commands by defining some variables first, to contain things as command shell, script paths, and argument, possibly also handling some system dependencies in this way.

We can define some **auxiliary variables in a `PropertyGroup`** and then refeence them when assembling the commands. We then **run initialization scripts in a dedicated Target element**. Here is an example of this:

~~~xml
<PropertyGroup>
    <!-- Select either Windows powershell or cross-platform pwsh to execute scripts: -->
    <_PSExe Condition="'$(OS)' == 'Windows_NT'">powershell</_PSExe>
    <_PSExe Condition="'$(OS)' != 'Windows_NT'">pwsh</_PSExe>
    <_ExecPolicy Condition="'$(OS)' == 'Windows_NT'">-ExecutionPolicy Bypass</_ExecPolicy>
    <!-- Define the command to be executed (be careful about newline syntax!): -->
    <InitCommand>
        $(_PSExe) -NoProfile $(_ExecPolicy) -File `
            &quot;$(MSBuildProjectDirectory)/../../scripts/UpdateDepencencyRepos.ps1&quot;
            <!-- Add eventual script arguments here, e.g.:
                -Arg1 "$(SomeArg)" -Arg2 "$(AnotherArg)"   -->
    </InitCommand>
</PropertyGroup>
<!-- Execution of the script that clones or updates all the dependency repositories: -->
<Target Name="RunInitializationScripts" AfterTargets="PostBuildEvent">
    <Message Text="Initialization command:" Importance="high" />
    <Message Text="  $(InitCommand)" Importance="high" />
    <Exec Command="$(InitCommand)" ContinueOnError="true" />
</Target>

~~~

### Special Initialization Project

A special form of initialization project can be used to additionally prevent generation of any output and running of build tasks. We actually prefer a normal project with settings as above.


### Referencing Project

In the referencing project, **reference the initialization project** like this:

~~~xml
  <ItemGroup>
  <ProjectReference Include="InitDependencies.csproj"
    ReferenceOutputAssembly="false" PrivateAssets="All" />
  </ItemGroup>
~~~

The **`ReferenceOutputAssembly="false"`** attribute causes that **assemblies generated by the referenced project are not referenced**, therefore avoiding additional dependencies.

The **`PrivateAssets="All"`** attribute means means *“use this in this project, but don’t let anything from it flow transitively to projects that depend on me”*. This **blocks all asset types of the referenced project from flowing past the current project**: `compile`, `runtime`, `build`, `buildTransitive`, `contentFiles`, `native`, and `analyzers`. You still get those assets inside the referencing project; they just won’t be re-exported to its consumers.

Further from the previous, the **`Private=false` excludes the referenced project’s runtime assets** from output of the current project.

Related: You can use `ExcludeAssets="..."` to remove selected asset types even for the current project, while IncludeAssets="..." specifies the asset types that you do want to have in the referencing project.

## Minimal Example: Utility Project File (.csproj)

~~~xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>

    <!-- Prevent any assembly from being built -->
    <OutputType>None</OutputType>
    <ProduceReferenceAssembly>false</ProduceReferenceAssembly>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <!-- Scripts that are considered "inputs" -->
    <InitScripts Include="Scripts\*.ps1" />
  </ItemGroup>

  <!-- Define the build behavior -->
  <Target Name="RunInitScripts"
          Inputs="@(InitScripts);$(MSBuildProjectFile)"
          Outputs="$(IntermediateOutputPath)InitScripts.timestamp"
          BeforeTargets="Build">
    
    <Message Text="Running initialization scripts for $(MSBuildProjectName)..." Importance="high" />

    <!-- Run each PowerShell script -->
    <Exec Command="powershell -ExecutionPolicy Bypass -File %(InitScripts.Identity)" />

    <!-- Touch a file to mark target as up-to-date -->
    <Touch Files="$(IntermediateOutputPath)InitScripts.timestamp" AlwaysCreate="true" />

  </Target>

</Project>
~~~

### Explanation

1. **No DLL/EXE is produced**

   * `OutputType=None`, `ProduceReferenceAssembly=false`, and removing compilation ensures no binaries are generated.

2. **Incremental build support**

   * MSBuild decides if a target is up-to-date based on its `Inputs` and `Outputs`.
   * `Inputs="@(InitScripts);$(MSBuildProjectFile)"` → if any script or project file changes, the target will run again.
   * `Outputs="$(IntermediateOutputPath)InitScripts.timestamp"` → if the timestamp file is missing or older than inputs, the target runs.

3. **Runs before dependent projects**

   * `BeforeTargets="Build"` ensures the scripts execute as part of this project’s normal build.
   * If another project references this one, MSBuild will build it first (standard project dependency chain).

4. **Supports clean/rebuild**

   * `Clean` deletes everything under `$(IntermediateOutputPath)`, which removes `InitScripts.timestamp`.
   * Next build will re-run scripts, exactly like a normal compiled project.

5. **Behaves like a standard project**

   * If nothing changes, building again does **not** rerun scripts.
   * If dependent projects are built and this project is already up-to-date, scripts are **not rerun**.
   * If this project is rebuilt, scripts **run again**.

### Behavior Summary

* **Build once → scripts run → timestamp created.**
* **Build again → scripts skipped unless inputs changed.**
* **Rebuild → scripts always rerun.**
* **Clean → timestamp removed → next build reruns scripts.**
* **Dependencies → if another project depends on this one, scripts run first if out-of-date.**

This achieves *the same semantics as a normal C# project build*, except instead of producing a `.dll`, it just executes the scripts.











### Prevent Initialization Project Outputs from Getting into Dependent Projects

We want:

* Other projects to **depend** on the utility project (so its scripts run first).
* But **no assembly** from it should be copied into the consuming project’s output.
* No "dangling reference" at runtime.

This is achieved by `PrivateAssets="all"` on a `<ProjectReference>`.

**In the consumer (dependent) project, iclude reference to the initialization project in the following way:

~~~xml
<ItemGroup>
  <ProjectReference Include="..\MyInitScripts\MyInitScripts.csproj" PrivateAssets="all" />
</ItemGroup>
~~~

**Effect**:

* `MyApp` will build only after `MyInitScripts` is built (so scripts are guaranteed to run first).
* Because `PrivateAssets="all"`, nothing from `MyInitScripts` flows into `MyApp`’s output directory or NuGet dependencies.
* Since *`MyInitScripts` produces no DLL*, there’s nothing to copy anyway - but this ensures safety if you later add content.

**Behavior**:

1. If you `dotnet build MyApp`, MSBuild sees that `MyApp` depends on `MyInitScripts`.
1. It builds `MyInitScripts`:
   * Runs PowerShell scripts only if out-of-date (based on inputs/outputs).
   * Marks as complete by touching a timestamp file.
1. Then it builds `MyApp`.
1. Nothing from `MyInitScripts` is copied to `MyApp\bin`.

This gives the semantics of a "real project dependency" with **no dummy outputs** and correct MSBuild incremental behavior.
