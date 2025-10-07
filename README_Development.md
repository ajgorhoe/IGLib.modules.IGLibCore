
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
  * [The IGLibCore and Other Base Modules](#the-iglibcore-and-other-base-modules)
* [To Do](#things-to-be-done)
* [Versioning IHLib Modules]()

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

### Versioning IGLib Modules

**See also** the [Versioning document o Wiki](https://github.com/ajgorhoe/wiki.IGLib/blob/main/IGLib/general/CiCd/Versioning.md) (private repo!)

In this context, IGLib Module means a set of projects contained in a single repository.

We use **`[GitVersion](https://gitversion.net/docs/usage/msbuild)`** for versioning. This is done by including the following in .NET project files (.csproj):

~~~xml
	<ItemGroup>
		<PackageReference Include="GitVersion.MsBuild" Version="*" PrivateAssets="All" />
	</ItemGroup>
~~~

The attribute `<PrivateAssets>all</PrivateAssets>` prevents the task from becoming a dependency of the package we build.

This integrates directly with MSBuild as `MSBuild task`; restore pulls the tool in, and the tool computes versions during builds (or packaging, etc.) and integrated them into generated artifacts. The tool is configured in `GitVersion.yml` ([local version here](./GitVersion.yml)).

In order for the tool to be able to compute the version, it needas a branch, and there must be a **commit with a version tag** (such as `4.1.12` or `v4.1.12`) reachable from that branch. If the branch itself is tagged, this defines the version. Otherwise, the **[semantic version](https://en.wikipedia.org/wiki/Software_versioning#Semantic_versioning) (*SemVer*)** (`major.minor.patch`) is calculated from the path leading from the closest tagged commit to the current commit, according to configured rules. For example, one can define which part of the version (mayor/minor/patch) gets incremented for a specific branch. For more information, you can check these **GitVersion documentation** pags:

* [GitVersion - Commandline Arguments](https://gitversion.net/docs/usage/cli/arguments), [assembly patching](https://gitversion.net/docs/usage/cli/assembly-patch)
* [GitVersion - MSBuild Task](https://gitversion.net/docs/usage/msbuild)
* [GitVersion - Configuration](https://gitversion.net/docs/reference/configuration), [version variables](https://gitversion.net/docs/reference/variables)
* [GitVersion - Reuirements](https://gitversion.net/docs/reference/requirements)
* [GitVersion - Version Incrementing](https://gitversion.net/docs/reference/version-increments),  [Version Sources](https://gitversion.net/docs/reference/version-sources)
* [GitVersion - Varsioning Modes](https://gitversion.net/docs/reference/modes/)
* [Branching Strategies](https://gitversion.net/docs/learn/branching-strategies/)
  * [Brenching Strategies - Overview](https://gitversion.net/docs/learn/branching-strategies/overview)
    * [Branchig strategy - GitFlow](https://gitversion.net/docs/learn/branching-strategies/gitflow/), [examples](https://gitversion.net/docs/learn/branching-strategies/gitflow/examples)
    * [Branchig strategy - GitHubFlow](https://gitversion.net/docs/learn/branching-strategies/githubflow/), [examples](https://gitversion.net/docs/learn/branching-strategies/githubflow/examples)

> **Important** - use in **GitHub Acions**:
> You must **disable shallow fetch** by setting **`fetch-depth: 0`** in your checkout step; without it, GitHub Actions might perform a shallow clone, which will cause GitVersion to display an error message.

The version for the current or a specified commit in a local repository can be calculated by using a **global dotnet tool**. **Install** the tool in the following way:

~~~powershell
dotnet tool install -g GitVersion.Tool
~~~

Then, in order to calculate version number for the currently checked-out branch, run one of the following commands:

~~~powershell
dotnet gitversion /showvariable SemVer
dotnet gitversion /showvariable FullSemVer
dotnet gitversion /showvariable NuGetVersionV2
~~~

Or, to get various version information in JSON:

~~~powershell
$gv = dotnet gitversion /output json | ConvertFrom-Json
~~~

When building a C# project from a repository, the GitVersion tool will **calculate the version** and **add the version-related properties** `Version` (e.g. `1.2.3`), `AssemblyVersion` (e.g. `1.2.0.0`), `FileVersion` (e.g. `1.2.3.0`) and InformationalVersion` (e.g. `1.2.3+Branch.main.Sha.abcdef`) **to** the generated **assembly**. When creating a `NuGet` package with `dotnet pack`, version will be set for the NuGet package.

You can **override behavior with certain strings in commit messages** ([see configuration doc](https://gitversion.net/docs/reference/configuration)):

* `+semver: major` (or `+semver: breaking`) in commit message will cause bump (increase) in major version
* `+semver: minor` (or `semver: feature`) will cause bump in minor version
* `+semver: patch` or `+semver: fix` will cause bump in patch version
* `+semver: skip` or `+semver: none` will cause that bump that would otherwise be made (automatically) is skipped

> **Warnig**:
> `GitVersion` **does not work when projects / solutions are built in Visual Studio**. This is because `GitVersion` developers only support later .NET LTS and the tool needs .NET Core runtime (see [this Github Discussion](https://github.com/GitTools/GitVersion/discussions/4130)). `Visual Studio` uses stand-alone `MSBuild` that is built *for .NET Framework*, and this cannot be changed (Microsoft only releases MsBuild that is built for .NET Framework).
> When **building or packaging with the `dotnet` tool**, **`GitVersion` works** because the `dotnet` tool contains an embedded MSBuild that is built for .NET (Core) (see [this Stack Overflow answer](https://stackoverflow.com/questions/79584977/how-can-i-force-visual-studio-to-use-msbuild-for-net-9)).

In order to use GitVersion efficiently, you should tag at least the versions of main branch that are released ot published somewhere. If you know the version you want to asign, can do this on a local repostory using something like this:

~~~powershell
# Tag the current commit on main as 1.4.0:
git checkout main
git pull
git tag -a v1.4.0 -m "Release 1.4.0"
git push origin v1.4.0
~~~

Manual tagging is error prone, and it is easy to create a wrong tag. One mitigation is to run a script where after updating the main branch, the current version is calculated by GitVersion (which correctly increments major, minor and patch numbers when commits are added and branched merged), then use this version in tagging. Below is a **PowerShell script* that tags the `main` branch with correct version number*:

~~~powershell
# Update the main branch:
git checkout main
git pull  # optional - uncomment if convenient
# Get the current version on the main branch:
$CurrentVersion = $(dotnet gitversion /showvariable FullSemVer)
# Tag the version and push it:
git tag -a "v${CurrentVersion}" -m "Released version ${CurrentVersion}"
git push origin "v${CurrentVersion}"
~~~

**Manually Handling Tags**:

It may happen that come commits were wrongly tagged by version tags. In such cases, manual interventions with checking, deleting or re-aplying correct tags may be necessary. Here are some basic tips.

Showing tags:

~~~powershell
# list local tags:
git tag
# list tags on the specified remote (origin in this case):
git ls-remote --tags origin
# List both local tags and tags from certain remote (origin):
git fetch --tags
git tag
git ls-remote --tags
# or:
git fetch --tags
git tag -l
~~~

Removing tags:

~~~powershell
# Remove the tag ("v1.2.3" in this case) only locally:
git tag -d v1.2.3
# Remove the tag on the remote called origin:
git push origin --delete tag v1.2.3
~~~

After cleanup, adding the correct tag:

~~~powershell
git checkout main  # or whatever branch needs to be tagged
git pull           # if necessary to get sync changes
git tag -a v1.2.3 -m "Version 1.2.3"
git push origin v1.2.3
~~~



**Example Workflow**:

* tag the last verison on the `main` branch
* branch off `develop` and other branches like `feature` or `release` branches; they will be versioned according to rules specified in basic configuration plus additional configuration (e.g. `GitVersion.yml` in the repository root)
* when merging back to the `main` branch, tag the main branch with the version calculated by the GitVersion tool
* start again from the second pooint

  