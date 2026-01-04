
# Initialization Project `InitModulesCoreExtended`

See also [repository's README.md](../../../README.md) ([on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md)) and [this file on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/0InitModules/InitModulesCore/README_InitModulesCores.md).

## About `InitModulesCore`

This project, when "built" in an [IDE](https://en.wikipedia.org/wiki/Integrated_development_environment) like `Visual Studio` or via `MSBuild` or the `dotnet tool`, performs initialization for software projects in this repository, e.g. by cloning dependency repositories containing referenced projects (other IGLib repositories or external repositories).

Specifically, this project performs cloning (or updating, if already cloned) of basic source project dependencies for projects in the `IGLibCore` repository:

* It runs the `PowerShell` script [/scripts/UpdateDependencyReposExtended.ps1](../../../scripts/UpdateDependencyReposExtended.ps1)
  * This clones/updates basic dependency repositories that are handled by the *[InitModulesCore project](../InitModulesCore/README_InitModulesCore.md)* plus some additional (external) software repositories (see the script file linked above for precise information).

## General Information on Initialization Projects in IGLib

For general information, [see documentation of the `InitModulesCore` project](../InitModulesCore/README_InitModulesCore.md#general-information-on-initialization-projects-in-iglib)



