
# IGLibCore (Investigative Generic Library) - Information for Developers

<img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" />

This repository contains basic portions of the restructured ***Investigative Generic Library*** (***IGLib***). The current document contains some basic information for developers. More can be found in the [Developers' Wiki](https://github.com/ajgorhoe/wiki.IGLib/blob/main/IGLib/development/InfoForDeveloers.md).

**Contents** (the **original** content is **in the [Developers' Wiki](https://github.com/ajgorhoe/wiki.IGLib/blob/main/IGLib/development/InfoForDeveloers.md)**):

* **[Instructions for Developers](#instructions-for-developers)**
  * [IGLibCore project](#the-iglibcore-and-other-base-modules)
  * [Unified Structure](#unified-structure-of-iglib-modules)
    * [Directory Structure](#directory-structure)
    * [Solution Structure](#solution-structure)
* [To Do](#things-to-be-done)

## Instructions for Developers

**Remark**: Don't make significant additions to these sections. Do it in the [Developers' Wiki](https://github.com/ajgorhoe/wiki.IGLib/blob/main/IGLib/development/InfoForDeveloers.md) and make only the necessary corrections and updates here.

### Unified Structure of IGLib Modules

As good example of structure, see the **IGLibSandbox** module (a private repository), or **IGLibScripting**, or **IGlibScriptingCs** for modules with more complex dependencies.

#### Directory Structure

* ***Module/*** - cloned module's repository directory
  * *.git/*
  * ***src/***
    * *Project1/* - the project directory for *Project1*
      * *Project1.csproj* - the project file; Project file should be such that all code sources are within the project directory and no sources need to be excluded from build.
    * *Project2/* - the project directory for *Project2*
      * *Project2.csproj*
    * ... - as a rule, IGLib modules should not consist of too many projects; many will consist of a single code project and a test project
    * *TestProject1/*
      * *TestProject1.csproj*
    * *TestProject2/*
      * TestProject2.csproj - there can be one or more test projects. It is not necessary that test projects correspond 1-to-1 with code projects.
  * ***Module.sln*** - VS solution to handle module's projects and their dependencies
  * *README.md* - markdown readme file with links to other documentation and resources
  * *LICENSE.md* - the license file
  * *.gitignore/* - Git's global ignore file
  * ... - other files, when necessary
* ***_external/*** - directory containing **external dependencies** that are referenced as source code projects
  * *Depencency1/*
  * *Dependency2/*
  * ...
* ***_scripts/*** - contains update scripts and other common scrippts for managing repository clones
  * *0bootstrappingscripts/* - bootstrapping scripts for initializing the environment such that cloning, updating, and other scripts can work correctly
    * *[IGLibScripts/]* - clone of the IGLibScripts repository, which is cloned via bootstrapping scripts (the directory does not exist before bootstrapping scripts are executed); scripts in this repository are used to manage the dependencies and for other tasks
    * *fallback/* - contains copies of the initial scripts necessary for bootstrapping
    * *BootstrapScripting.bat* - performs bootstrapping on Windows
  * *UpdateModule_IGLibCore.bat* - Windows batch script for cloning or updating the *IGLibCore*/ module
  * *UpdateModule_IGLibScripting.bat* - performs cloning or updating the *IGLibScripting* module
  * *UpdateModule_IGLibScripts.bat* - performs cloning or updating the *IGLibScripts* module, which contains various useful scripts used in the development and for other tasks
  ...
* *[IGLibCore/]* - base IGLib module, cloned when demanded ( on Windows, this is typically  by running *_scripts/UpdateModule_IGLibCore.bat*)
* *[IGLibScriptiing/]* - another IGLib module, can be cloned by runnning *_scripts/UpdateModule_IGLibScripting.bat*
* *[IGLibScripts]* - another IGLib module, can be cloned by running *_scripts/UpdateModule_IGLibScripts.bat*
* ... (other IGLib module directories)

#### Solution Structure

* Dependencies/
  * External/ - contains projects for external dependencies that may be referenced via source code projects
  * IGLibModules/ - contains IGLib projects that may be referenced via source code projects
    * [IGLibModule1/] - directory for project files from *IGLibModule1*
* Project1 (.csproj) - the first code project that constitutes the module
* Project2 (.csproj) - the second code project that constitutes the module; there can be more than one projects in an IGLib module, but there should not be too many (the structure should tend to make code units with few dependencies that are clearly organized)
* TestProject1 (.csproj) - the first test project for the module
* TestProject2 (.csproj) - the second test project for the module; test projects do not need to directly correspond to the source projects of the module; often there may be a single test project for multiple source projects

#### Remarks on Project Structure

We promote more granular modules, but not for every cost. Ease and comprehensible logic is of more importance that granularity.

**IGLib modules** can consist of **multiple projects**. The basic function of modules is to contain logically connected functionality. However, for many reasons it may be useful to break modules to different projects, e.g., to **separate out parts that have more demanding requirements regarding dependencies**. Especially, it makes sense to **separate out parts of the module** into different projects when they have:

* **very large dependencies** such as Roslyn
* **dependencies that change frequently**
* **dependency that may be problematic in other aspects**, e.g., external dependencies that are **not well maintained** or that don't take care of **backward compatibility** or are **not cross-platform**.

Division of modules into 

### The IGLibCore and Other Base Modules

**IGLibCore**:

* Should be **fully cross-platform**.
* Should have **no dependencies on UI or web frameworks**.
* Should maintain **compatibility with .NET Standard 2.0 and .NET Framework 4.8(.x)**, such that it can be reused by the Legacy Framework, and tools can be easily moved from the legacy IGLib to IGLibCore
* Should **Not have dependencies on other IGLib modules**.
* Should **not have dependencies on external source projects**.
* Should **avoid dependcncies on NuGet packages** and should **only have dependencies on .NET base libraries**.
  * Should **avoid dependencies on complex .NET stuff** and stuff that is likely subject to changes (based on past experience) **such as serialization**.




## In Development

### Versioning

We use **`GitVersion`** for versioning. This is done by including the following in .NET project files (.csproj):

~~~xml
	<ItemGroup>
		<PackageReference Include="GitVersion.MsBuild" Version="*" PrivateAssets="All" />
	</ItemGroup>
~~~

This integrates directly with MSBuild; restore pulls it in, and it computes versions during build. The tool is configured in `GitVersion.yml` ([local version here](./GitVersion.yml)).

**Workflow**: