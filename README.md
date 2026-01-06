
# IGLibCore (Investigative Generic Library)

<img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" />

This repository contains basic portions of the restructured ***Investigative Generic Library*** (***IGLib***) in the [repository section](#about-this-repository---iglibcore). The current document also contains **information about the complete *IGLib***, which consists of multiple repositories. **IGLib** is currently undergoing some changes.

**Contents**:

* [Links](#links)
* [About the Repository](#about-this-repository---iglibcore) - information about the current repository and contained projects
  * [Projects within the IGLibCore Repository](#projects-within-the-iglibcore-repository)
  * [Building and Running](#building-and-running)
  * [IGLib Restructuring](#iglib-restructuring)
* [The Investigative Generic Library (IGLib)](#the-investigative-generic-library-iglib) - information about IGLib as a whole; see also *[README-scripts](./scripts/README_scripts.md)* for a quick list of repositories with links
  * [Repositories](#repositories) - about repositories that constitute *IGLib*
  * [IGLib Restructuring](#iglib-restructuring)
  * [Legacy IGLib Libraries](#legacy-iglib-libraries)
* [License](#license)

## Links

* [This repository - *IGLibCore*](https://github.com/ajgorhoe/IGLib.modules.IGLibCore) ([readme](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/master/README.md))
  * *[Container repository](https://github.com/ajgorhoe/iglibmodules)* ([readme](https://github.com/ajgorhoe/iglibmodules/blob/master/README.md)) used to clone this and other IGLib repositories, such that dependencies can be handled by a common Visual Studio repository
  * [Basic Legacy IGLib repository](https://github.com/ajgorhoe/IGLib.workspace.base.iglib) ([readme](https://github.com/ajgorhoe/IGLib.workspace.base.iglib-/blob/master/README.md))
* **[Development Wikis]()** (private repository)

## About this Repository - 'IGLibCore'

[This repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore) contains the [IGLibCore library](./src/IGLib.Core/), which includes common lower-level utilities, and other libraries commonly used by more specialized libraries and by applications of both the new and the legacy IGLib, as well as some other libraries and applications. *IGLib* consists of several other libraries and demo applications. You can find more details [below](#the-investigative-generic-library-iglib) and in the [Links Section](#links).

### Projects within the `IGLibCore` Repository

* [src/IGLib.Core](./src/IGLib.Core/README_IGLib.Core.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Core/README_IGLib.Core.md))
* [src/IGLib.CoreExtended](./src/IGLib.CoreExtended/README_IGLib.CoreExtended.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.CoreExtended/README_IGLib.CoreExtended.md))
* [src/IGLib.Numerics](./src/IGLib.Numerics/README_IGLib.Numerics.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Numerics/README_IGLib.Numerics.md))
* [src/IGLib.WinForms](./src/IGLib.WinForms/README_IGLib.WinForms.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.WinForms/README_IGLib.WinForms.md))
* [src/IGLib.Transfer](./src/IGLib.Transfer/README_IGLib.Transfer.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Transfer/README_IGLib.Transfer.md))
* ...
* [src/0InitModules/InitModulesCore](./src/0InitModules/InitModulesCore/README_InitModulesCore.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/0InitModules/InitModulesCore/README_InitModulesCore.md))
* [src/0InitModules/InitModulesCoreExtended](./src/0InitModules/InitModulesCoreExtended/README_InitModulesCoreExtended.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/0InitModules/InitModulesCoreExtended/README_InitModulesCoreExtended.md))

### Building and Running

To clone and work with this and other IGLib libraries (build, run, develop), use the new *[IGLib cotainer repository (iglibmodules)](https://github.com/ajgorhoe/iglibmodules)*. See the container repository's [readme file](https://github.com/ajgorhoe/iglibmodules/blob/main/README.md) for further instructions.

The legacy IGLib's base library is located at:
*<https://github.com/ajgorhoe/IGLib.workspace.base.iglib.git>*

## The Investigative Generic Library (IGLib)

### Repositories

* [IGLibCore](https://github.com/ajgorhoe/IGLib.modules.IGLibCore) (this repository) - a base library containing some common utiities with few dependencies (UI, graphics, reflection dependencies are not allowed).
* [iglib](https://github.com/ajgorhoe/IGLib.workspace.base.iglib) - the legacy IGLib's base libraries. Used also by newer applications and new IGLib, will continued to be maintained. Contains [documentation](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md) for the legacy IGLib.
* **Helper Repositories**:
  * [iglibmodules](https://github.com/ajgorhoe/iglibmodules) - a container repository used for cloning and building and running locally the IGLib libraries  and applications.
  * [IGLibScripts](https://github.com/ajgorhoe/IGLib.modules.IGLibScripts/) contains some usful scripts (e.g. for cloning and updating repositories, backups, some Windows utilities, etc.), also used by IGLib for cloning and updating the reoisitories.
  * [codedoc](https://github.com/ajgorhoe/IGLib.workspace.doc.codedoc) contains scripts and other tools for generating and viewing code documentation for software projects, including IGLib projects.
    * [CodeDocumentation](https://github.com/ajgorhoe/CodeDocumentation) contains compiled code documentation for parts of IGLib (experimental), generated via [codedoc](https://github.com/ajgorhoe/IGLib.workspace.doc.codedoc), then copied to and committed to tge repository.
* [IGLibGraphics3D](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D) - a 3D graphics library.
* [IGLibScripting](d:\users\ws\ws\other\iglibmodules\IGLibScripting) - Rudimentary scripting utilities; some utiities will be transferred here from legacy IGLib.
* [IGLibScriptingCs](https://github.com/ajgorhoe/IGLib.modules.IGLibScriptingCs) - C# scripting, dynamic building and execution. Some utilities from legacyLib will be modernized in this repository.
* [IGLibSandbox](https://github.com/ajgorhoe/IGLib.modules.IGLibSandbox) - a *private* repository for prototyping, experimental, and early development

### IGLib Restructuring

[Legacy IGLib libraries](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md) contain lots of useful tools from different areas. Not all of these libraries were made publicly available, as many were developed within corporate environment. Due to complex dependency structure, some of these libraries were not ported to the new open source .NET (Core) when it was introduced by Microsoft in 2014. Some libraries could be ported to .NET but were slowly abandoned because the new .NET ecosystem changed significantly and some of the crucial dependencies were not actively developed or supported any more.

Because of this, many libraries of the *legacy IGLib Framework* were phased out. Some others are not actively developed, but are used in existing applications. Only the [IGLib](https://github.com/ajgorhoe/IGLib.workspace.base.iglib) base library is still used in new applications, but it is also not developed, except for the necessary fixes needed in the applications that use it. The newer libraries were started after the new .NET Core (later renamed to .NET) became stable.

The figure below shows dependencies betweem some IGLib modules, some legacy ones as well as the newer modules developed on the newer .NET Core / .NET (click the image to view searchable version full screen, or [click here to open a bitmap version](https://ajgorhoe.github.io/IGLibFramework/images/IGLib/IGLibDependencyGraph.jpg)):

<a href="https://ajgorhoe.github.io/IGLibFramework/images/IGLib/IGLibDependencyGraph.svg"><img src="./doc/images/IGLibDependencyGraph.svg" width="max(80%), 800px"></img></a>

### Legacy *IGLib* Libraries

Bulding legacy IGLib libraries is a bit more complex due to their dependencies. See the [IGLib Framework repo](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/)'s [readme file](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md) to learn more about this.

## License

Copyright © Igor Grešovnik.

See [LICENSE.md](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md) ([local version](./LICENSE.md)) for license information.

Disclaimer:  
The repository owner reserves the right to change the license to any of the permissive open source licenses, such as the Apache-2 or MIT license.
