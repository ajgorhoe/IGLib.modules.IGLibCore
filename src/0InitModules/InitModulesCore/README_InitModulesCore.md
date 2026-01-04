
# Initialization Project `InitModulesCore`

See also [repository's README.md](../../../README.md) ([on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md)) and [this file on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/0InitModules/InitModulesCore/README_InitModulesCore.md).

## About `InitModulesCore`

This project, when "built" in an [IDE](https://en.wikipedia.org/wiki/Integrated_development_environment) like `Visual Studio` or via `MSBuild` or the `dotnet tool`, performs initializations for software projects in this repository, e.g. by cloning dependency repositories containing referenced projects (other IGLib repositories or external repositories).

Specifically, this project performs cloning (or updating, if already cloned) of basic source project dependencies for projects in the `IGLibCore` repository:

* It runs the `PowerShell` script [/scripts/UpdateDependencyRepos.ps1](../../../scripts/UpdateDependencyRepos.ps1)
  * This clones/updates the repositories containing dependency projects (see the above linked script file for precise information on which repositories are included)

## General Information on Initialization Projects in IGLib

Projects that need such initialization need to reference this project as normal project reference but with two additional attributes, `ReferenceOutputAssembly="false"` and `PrivateAssets="All"` (in the `.csproj` file, within an `<ItemGroup>`, within `<ProjectReference Include="..." ReferenceOutputAssembly="false" PrivateAssets="All" />`). For example:

~~~xml
<Project Sdk="Microsoft.NET.Sdk">
  ...
  <ItemGroup>
    <ProjectReference Include="..\..\src\0InitModules\InitModulesCore\InitModulesCore.csproj"      
      ReferenceOutputAssembly="false" PrivateAssets="All" />
  </ItemGroup>
  ...
</Project>
~~~

Attribute 'ReferenceOutputAssembly=false` is used  to avoid build dependencies, and `PrivateAssets="All"` to avoid propagating the reference to other projects.

The whole **concept** relies on two things: **running initialization tasks via C# initialization project** build (which *calls a `PowerShell` initialization script* to run the initialization tasks), and **referencing that project in the projects that need the initialization tasks being run** *before  these projects are built*. The mechanism brings several **advantages**:

* It is guaranteed that **initialization tasks are run before the dependent project is built** for the first time: the build system takes care of that by usual build order rules, which is achieved by dependent project referencing the initialization project that runs the initialization scripts
* Once the **initialization** project has been run, it **will not be run again**. This is the correct way of doing things because initialization normally does not need to be performed again once it has been performed. The only reason for performing initialization again is that circumstances change, for example, the dependency project has changer on remote source control server and changes should be updated, or the task itself changes (e.g. initialization scripts need to clone an additional source repository). Such situations can be dealt with manually by "rebuilding" the initialization project (quotes are used because the initialization project is *rebuilt* technically, in terms how this task is called in the context of a C# project (which is only used as a mechanism to run the tasks), but in reality initialization projects aren't actually built).
* When there is a situation **when initialization needs to be run again**, this can be **achieved by rebuilding the initialization project** (**or** by rebuilding **any dependent** project, via the usual chain building rules)

As mentioned, building the initialization project runs the specific initialization script, which runs the necessary initialization tasks. This is achieved project configuration settings similar to those below, taken from `InitModulesApps.csproj`:

~~~xml
<Project Sdk="Microsoft.NET.Sdk">
    
    ...
	
    <PropertyGroup>
		<!-- Select either Windows powershell or cross-platform pwsh to execute scripts: -->
		<_PSExe Condition="'$(OS)' == 'Windows_NT'">powershell</_PSExe>
		<_PSExe Condition="'$(OS)' != 'Windows_NT'">pwsh</_PSExe>
		<_ExecPolicy Condition="'$(OS)' == 'Windows_NT'">-ExecutionPolicy Bypass</_ExecPolicy>
		<!-- Define the command to be executed (use a SINGLE LINE for command!): -->
		<InitCommand1>
			$(_PSExe) -NoProfile $(_ExecPolicy) -File "$(MSBuildProjectDirectory)/../../../scripts/UpdateDependencyRepos.ps1"
			<!-- Add eventual script arguments to the above line, e.g.:
				-Arg1 "$(SomeArg)" -Arg2 "$(AnotherArg)"  
				-->
		</InitCommand1>
		<InitCommand1  Condition="'$(GITHUB_ACTIONS)' == 'true'">
			$(_PSExe) -NoProfile $(_ExecPolicy) -Command "Write-Host \"`n`nSKIPPING INITIALIZATION of dependency repositories in GitHub Actions environment.`n`n\""
		</InitCommand1>
	</PropertyGroup>

	<!-- Execution of the script that clones or updates all the dependency repositories: -->
	<Target Name="RunInitializationScripts" AfterTargets="PostBuildEvent">

		<Message Text="
========== Initialization Project ==========" Importance="high" />
		<Message Text="From InitModulesScriptingCs:" Importance="high" />
		<Message Text="Initialization command:" Importance="high" />
		<Message Text="  $(InitCommand1)" Importance="high" />
		<Message Text="----------
" Importance="high" />

		<!--
		This target runs the script that clones or updates all the dependency repositories.
		The script is run in a post-build step. Therefore, if the project is up-to-date and does not need
		to be built, the script is not run. You can force re-running the scritp simply by rebuilding the
		project in Visual Studio, via command-line, or by any other means.
		-->
		<Exec Command="$(InitCommand1)" ContinueOnError="true" />
	</Target>

    ...

</Project>
~~~

A post-build event runs a script to perform initialization tasks. 

Beside the above configuration options that get the work done, initialization projects will usually have the following set of properties defined:

~~~xml
<Project Sdk="Microsoft.NET.Sdk"> 
	<!-- Utility project; Initializes dependencies for the Scripting and ScriptingTests projects:
	  * Clones / updates source repositories of dependencies side by side
	See: https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/0InitModules/InitializationProjects.md -->
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

    ...

</Project>
~~~

Targeting *.NET Standard 2.0* (`<TargetFramework>netstandard2.0</TargetFramework>`) ensures wide compatibility (the project can be referenced from may other projects). The other configuration options ensure that no usual outputs are generated (like `AssmblyInfo.cs` or a `DLL`) when the project is built. This necessitates using additional options when referencing the project (`ReferenceOutputAssembly="false"` and `PrivateAssets="All"`).


