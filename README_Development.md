
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
  * **[CI/CD](#iglib-cicd)** for IGLib Repositories
    * [Basic Setup for Continuous Integration](#basic-setup-for-continuous-integration)
      * [Issue: Multiple Remotes not Allowed in GitHub Actions](#issue-multiple-remotes-not-allowed-in-github-actions)
    * **[Tagging Toolkit]()** (scripts)
    * **[Multi-Repository GitVersion Tagging Toolkit](https://github.com/ajgorhoe/IGLib.modules.IGLibScripts/blob/main/psutils/RepositoryVersionTagging/README_VersionTaggingToolkit.md)** on GitHub - a set of scripts for tagging versions and for synchronization of version tags across GitHub repositories;
      * [Local path to the document](../IGLibScripts/psutils/RepositoryVersionTagging/README_VersionTaggingToolkit.md) (works if IGLibScripts is cloned side-by-side)
      * Scripts availablr in [scripts/](./scripts/) directory

    * **[Versioning IGLib Modules](#versioning-iglib-modules)**
      * See also the [Versioning document on Wiki](https://github.com/ajgorhoe/wiki.IGLib/blob/main/IGLib/general/CiCd/Versioning.md) (*private repo*!)
* Old doc. on [CI/CD on GitLab](./doc/readme.GitLab_CI_CD_old.md) (currently not relevant)
* **[To Do](#things-to-be-done)**

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

## IGLib CI/CD

We currently use Github Actions for Continuous integration. [GitVersion](#versioning-iglib-modules) is used to automatize versioning. The setup is intended to be simple and to fully support continuous delivery if necessary. Currently, the priority is to make workflows simple and enable efficient work in the current conditions.

### Basic Setup for Continuous Integration

The template setup for continuous integration is in the [IGLibLore](https://github.com/ajgorhoe/IGLib.modules.IGLibCore) repository, and [IGLibGraphics3D](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D) can also be referred to for handling dependencies. This is supported by **GitHub Actions configuration files** in the [.github/workflows/ directory](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/.github/workflows) ([build.yml](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/.github/workflows/build.yml)) and some **scripts in the [scripts/](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/scripts) directory** of each repository that are also intended to support continuous integration, for example:

* [UpdateDepencencyRepos.ps1](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/scripts/UpdateDepencencyRepos.ps1) and [UpdateDepencencyReposExtended.ps1](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/scripts/UpdateDepencencyReposExtended.ps1) are intended to clone or update all repositories containing dependencies of the certain repository that are referenced via source projects.
  * Each repository contains its own version; customized versions of these scripts are available in all IGLib repositories that have such dependencies, and they call scripts for cloning/updating individual repositories, such as [UpdateRepo_IGLibScripts.ps1](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/scripts/UpdateRepo_IGLibScripts.ps1) in IGLibCore, or [UpdateRepo_IGLibCore.ps1](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D/blob/main/scripts/UpdateRepo_IGLibCore.ps1) in the IGLibGraphics3D repository; specific scripts available depend on what is referenced via source code projects by projects of the specific repository.
  * Updating/cloning scripts for individual repositories rely on [UpdateOrCloneRepository.ps1](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/scripts/UpdateOrCloneRepository.ps1) to do the job, and the end script only provide 
* [TagVersion.ps1](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/scripts/TagVersion.ps1) is intended to create a version tag on the specified branch.
* [SyncTagVersions_All.ps1](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/scripts/SyncTagVersions_All.ps1) is intended to synchronize version tags across IGLib repositories (the list or repositories is baked in the script but it can be extended) and relies on [SyncTagVersions.ps1](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/scripts/SyncTagVersions.ps1) to do the job. These scripts are only available in the IGLibCore repository, which is also used for central administration of other repositories. The latter script is maintained in the [IGLibScripts](https://github.com/ajgorhoe/IGLib.modules.IGLibScripts/tree/main/psutils/RepositoryVersionTagging) repository.
* [ShowDirectoryTree.ps1](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/scripts/ShowDirectoryTree.ps1) is used in continuous integration to inspect directory structure on the runner;s host machine, which may be very useful for troubleshooting when something goes wrong.

Note that the IGLib Core repository does not have actual dependencies in source code. Handling on such dependencies in continuous integration is better demonstrated in other repositories, such as [IGLibGraphics3D](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D).

PowerShell **scripts** are often used to **make certain tasks** (such as cloning repositories, building and testing code, tagging versions) **more uniform** across local development environments and CI hosts.

#### Issue: Multiple Remotes not Allowed in GitHub Actions

GitHub Actions do **not allow multiple remotes** for repositories checked out in GitHub Actions when these repositories contains code that is built. This can be seen e.g. in [commit 8616617](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D/commit/8616617e11d3dcd824c1c0aca4923e2973940fc6), where [CI build failed](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D/actions/runs/18587784528/job/52995585273#step:12:21) because the dependency repository `IGLibCore` was cloned with multiple remotes assigned. The following error is reported:

`WARN [25-10-17 8:59:07:80] An error occurred:
  **2 remote(s) have been detected**. When being run on a build server, the Git repository is expected to bear one (and no more than one) remote.`

This is immediately followed by this error:

`...\.nuget\packages\gitversion.msbuild\6.4.0\tools\GitVersion.MsBuild.targets(26,9): error MSB3073: The **command "dotnet --roll-forward Major** "...\gitversion.dll" "...\IGLibCore\tests\IGLib.TestBase"  -output file -outputfile "obj\gitversion.json"" exited with code 1.`

**Attempt to fix** this issue:

Multiple remotes were detected in the dependency repository `IGLibCore` because it was cloned via `UpdateDepencencyReposExtended.ps1`, which in turn calls 'UpdateRepo_IGLibCore.ps1', which calls `UpdateOrCloneRepository.ps1` to do the job. A fix was attempted by **detecting whether the script runs on GitHub Actions**, and **if yes, removing the additional remotes**. The most generic solution would be to do this in the `UpdateOrCloneRepository.ps1` because this script is always called, and it is centrally maintained in the `IGLibScripts` repository. However, such a hard-coded solution in a general script would be exaggerated (though convenient), and currently this is handled in each repository's scripts for cloning a specific dependency repository, such as [scripts/UpdateRepo_IGLibCore.ps1](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D/blob/main/scripts/UpdateRepo_IGLibCore.ps1) in the `IGLibGraphics3D` repository. IGLibGraphics3D also contains the UpdateRepo_IGLibScripts.ps1, but this repo dows not need to exclude additional remotes when run on GitHub Actions because it is not in involved in builds via MSBuild. The fix is provided by the following code at the end of the configuration section of `UpdateRepo_IGLibCore.ps1`:

~~~powershell
# Remove secondary and tertiary remotes when running on GitHub Actions:
if ($env:GITHUB_ACTIONS -eq "true") {
    $global:CurrentRepo_AddressSecondary = $null
    $global:CurrentRepo_RemoteSecondary = $null
    $global:CurrentRepo_AddressTertiary = $null
    $global:CurrentRepo_RemoteTertiary = $null
}
~~~

### Versioning IGLib Modules

**See also** the [Versioning document on Wiki](https://github.com/ajgorhoe/wiki.IGLib/blob/main/IGLib/general/CiCd/Versioning.md) (*private repo*!)

In this context, IGLib Module means a set of projects contained in a single repository.

We use **[GitVersion](https://gitversion.net/docs/usage/msbuild)** for versioning. This is done by including the following in .NET project files (.csproj):

~~~xml
<ItemGroup>
  <PackageReference Include="GitVersion.MsBuild" Version="*" PrivateAssets="All" />
</ItemGroup>
~~~

The attribute `<PrivateAssets>all</PrivateAssets>` prevents the task from becoming a dependency of the package we build.

This integrates directly with MSBuild as `MSBuild task`; restore pulls the tool in, and the tool computes versions during builds (or packaging, etc.) and integrated them into generated artifacts. The tool is configured in `GitVersion.yml` ([local version here](./GitVersion.yml)).

> **Important** - using *GitVersion* with local configuration (`GitVersion.yml`) has the potential of **causing errors in the build, packaging, or tagging** (see [Helper Scripts](#helper-scripts-for-version-tagging)) **process** due to **errors in configuration files**.
> These errors are **sometimes not reported in the same way** as other errors and may therefore be more difficult to find.
> If you encounter unusual error behavior with no clear indication of the root cause, quickly **check whether GitVersion operates correctly** on the affected branch: checkout the branch on which the failed procedure is executed and **execute** the following **command line** within the repository directory:
> `  `**`dotnet gitversion /showvariable FullSemVer`**
> If there are problems with the tool (often due to misconfiguration), `GitVersion` will fail to calculate the version of the checked-out commit, and you will normally get some useful indication of the cause.

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
dotnet gitversion /showvariable AssemblySemFileVer
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

### Manually Handling Tags

In order to use GitVersion efficiently, you should **tag at least the versions of main branch that are released ot published somewhere**.

If you know the version tag you want to asign, you can do this on a local repostory using something like this:

~~~powershell
# Tag the current commit on main as 1.4.0:
git checkout main
git pull
git tag -a v1.4.0 -m "Release 1.4.0"
git push origin v1.4.0
~~~

Manual **tagging a branch** is error prone, and it is easy to create a wrong tag. One mitigation is to run a script where the **current version** is **calculated by GitVersion** (which correctly increments major, minor and patch numbers when commits are added and branched merged), then use this version in tagging. Below is a **PowerShell script** that **tags the `main` branch with correct version number**:

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

# Listing of tags with hashes:
git show-ref --tags

# Listing in CUSTOM format, with arbitrary details:
git for-each-ref refs/tags --format="%(refname:short) %(objectname:short) %(taggerdate:short) %(taggername) %(taggeremail): '%(subject)'"
# Sample output:
# v2.0.84 85265a7 2025-10-12 Name Surname <name.surname@gmail.com>: 'Sync version v2.0.84'
# v2.0.90 5801c85 2025-10-13 Name Surname <name.surname@gmail.com>: 'Sync version v2.0.90'

# Listing in CUSTOM format, SORTED by dates:
git for-each-ref refs/tags --sort=taggerdate --format="%(refname:short)  %(objectname:short)  %(taggerdate:short)  '%(subject)'"
# Sample output:
# v2.0.84  85265a7  2025-10-12  'Sync version v2.0.84'
# v2.0.90  5801c85  2025-10-13  'Sync version v2.0.90'

# Show detailed information about specific tag:
git show v2.0.91
~~~

Removing tags added by accident:

~~~powershell
# Remove the tag ("v1.2.3" in this case) only locally:
git tag -d v1.2.3
# Remove the tag on the remote called origin:
git push origin --delete tag v1.2.3

# Fetch tags and verify the tags were actually deleted:
git fetch --tags --verbose
git tag
# Or, use date-sorted output with additional information:
git for-each-ref refs/tags --sort=taggerdate --format="%(refname:short)  %(objectname:short)  %(taggerdate:short)  '%(subject)'"
~~~

After cleanup, adding the correct tag:

~~~powershell
git checkout main  # or whatever branch needs to be tagged
git pull           # if necessary to get sync changes
git tag -a v1.2.3 -m "Version 1.2.3"
git push origin v1.2.3
~~~

### Example Workflow

See detailed workflow on the Wiki .

* tag the last version on the `main` branch
  
* branch off `develop` and other branches like `feature` or `release` branches; they will be versioned according to rules specified in basic configuration plus additional configuration (e.g. `GitVersion.yml` in the repository root)
* when merging back to the `main` branch, tag the main branch with the version calculated by the GitVersion tool;
  * use the `scripts/TagVersion.ps1` script of a repository to do this (run it without parameters)
  * not every commit needs to be tagged; you can explicitly tag only important versions
* start again from the second point
* ...
* If applicable, you can synchronize version tags across a group of related repositories
  * Use the `scripts/SyncTagVersions_All.ps1` script within the locally cloned `IGLibCore` repository to do that
  * Only important versions are usually synchronized across the repositories; this may be used to signify important benchmarks in development of multiple related repos that constitute a larger software
  * In many cases, you can run it without parameters, but:
    * It may be good if you run with `-DryRun` first, to indentify any potential issues
    * Sometimes you may also want to add a custom tag via `-CustomTag` parameter (and possibly a `CustomTagMessage`)

### Version Other Repositories

* IGLib Core repos:
  * Copy from IGLibCore (the core repo):
    * GitVersion.yml
    * scripts/TagVersion
  * Tag repo's versions manually if necessary
  * When creating releases, Sync-tag repos versions via sync-tag

### Helper Scripts for Version Tagging

There are a couple of scripts that assist with tagging the current version in repositories. Below is the list of these scripts with very brief descriptions, while for more information, refer to links under the `See also` (end of this section).

**[scripts/TagVersion.ps1](./scripts/TagVersion.ps1)** **creates a version tag** on the main branch (default is `main`, with fallback to `master` when main does not exist) or any other specified branch **and pushes it** to the remote origin. The tag assigned to the branch is calculated by GitVersion, and can be increased via `-bump*` switches or `-increment*` parameters ($* \in$ \{Major, Minor, Patch\}). Version number assigned can also be modified, e.g. any part of *SemVer* can be forcibly increased. This script is intended for use on the current local repository. Every repository should have this script in its `scritps/` directory.

**[scripts/SyncTagVersions.ps1](./scripts/SyncTagVersions.ps1)** - **synchronized** creation of version tags **across repositories**. **Creates version tags** on the main branch (default) or any other specified branch **on the specified groups of repositories** and **pushes it** to the corresponding remote origin. It **calculates the maximum version** on the specified branch across repositories, then applies this tag in all repositories. Repositories are specified via `-RepoDirs` parameter (array of strings); `-dryrun` switch can be used to just print the information on what would be applied (*no side effects*). Tagging itself works in the same way as in `TagVersion.ps1`. This script is available in `IGLibCore`'s `scripts/` directory and is typically run via helper script, `SyncTagVersions_All.ps1`.

**[scripts/SyncTagVersions_All.ps1](./scripts/SyncTagVersions.ps1)** or similar - a helper script for **synchronized** creation of version tags **across repositories**. **Calls `SyncTagVersions.ps1`** to do the job and accepts the same parameters, but has the **default group of repositories pre-defined** (hard-coded), such that in many cases it is run without any parameter passed to the script.

**See also**:

* [Multi-Repository GitVersion Tagging Toolkit](https://github.com/ajgorhoe/IGLib.modules.IGLibScripts/blob/main/psutils/RepositoryVersionTagging/README_VersionTaggingToolkit.md) on GitHub contains detailed descriptions and *user manuals* for the above mentioned scripts.
  * [Containing directory](https://github.com/ajgorhoe/IGLib.modules.IGLibScripts/tree/main/psutils/RepositoryVersionTagging) on GitHub
  * [Local path to the document](../../IGLibScripts/psutils/RepositoryVersionTagging/README_VersionTaggingToolkit.md) (works if IGLibScripts is cloned side-by-side)
* Related Documentation:
  * [Versioning IGLib Modules](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README_Development.md#versioning-iglib-modules) - developers' documentation for IGLib
    * [Local path to the document](../README_Development.md#versioning-iglib-modules)
  * [Versioning document on Wiki](https://github.com/ajgorhoe/wiki.IGLib/blob/main/IGLib/general/CiCd/Versioning.md) (*private repo*!)

## Things to Be Done

This section contains unarranged quick notes on what needs to be done.

**Versioning**:

* Documentation
  * Wiki:
    * Compile document About versioning with GitVersion (mention alternatives, exclude scripts documentation)
* CI/CD:
  * Create CI/CD scripts for event aggregator repo
    * Do / solve:
      * Updete dependend repositories
      * Build solution
        * Build for different targets
      * Execute tests
        * Questions: for which targets? Maybe only .NET Framework and latest .NET?
      * Create artifacts
        * Do we also need artifacts for test projects?
      * Create a package
        * Solve: 
          * Which targets included?
          * 
      * Publish a package



