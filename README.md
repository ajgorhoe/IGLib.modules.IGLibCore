
# IGLibCore (Investigative Generic Library)

<img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 20%; width: 4em; margin-left: 8pt;" />

This repository contains basic portions of the restructured ***Investigative Generic Library*** (***IGLib***) in the [repository section](#about-this-repository---iglibcore). The current document also contains **information about the complete *IGLib*** in the [respective section](#the-investigative-generic-library-iglib), which consists of multiple repositories. **IGLib** is currently undergoing some changes.

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

* [src/IGLib.Core](./src/IGLib.Core/README_IGLib.Core.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Core/README_IGLib.Core.md)) - basic common utilities for *IGLib*; may not depend on reflection and libraries that would affect trimming
* [src/IGLib.CoreExtended](./src/IGLib.CoreExtended/README_IGLib.CoreExtended.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.CoreExtended/README_IGLib.CoreExtended.md)) - extended common utilities for *IGLib*; can use reflection, but should not depend on heavy weight libraries
* [src/IGLib.Numerics](./src/IGLib.Numerics/README_IGLib.Numerics.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Numerics/README_IGLib.Numerics.md)) - basic numerical utilities; depend on [Math.Net Numerics](https://github.com/mathnet/mathnet-numerics)
* [src/IGLib.WinForms](./src/IGLib.WinForms/README_IGLib.WinForms.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.WinForms/README_IGLib.WinForms.md)) - some common Windows Forms utilities; used e.g. in some [3D graphics](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D/blob/main/README.md) projects
* [src/IGLib.Transfer](./src/IGLib.Transfer/README_IGLib.Transfer.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Transfer/README_IGLib.Transfer.md)) - a transient project, mainly for transfer of utilities from (legacy) [IGLib Framework](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/) or from sandbox environments
* ...
* [src/IGLib.Core.Tests](./tests/IGLib.Core.Tests/README_IGLib.Core.Tests.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/tests/IGLib.Core.Tests/README_IGLib.Core.Tests.md)) - unit tests and low level integration tests for IGLibCore projects
* [src/IGLib.TestBase](./tests/IGLib.TestBase/README_IGLib.TestBase.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/tests/IGLib.TestBase/README_IGLib.TestBase.md)) - utilities for test projects, such as base class for XUnit tests, output and loggers for tests, etc.
* [src/IGLib.TestBase.ExampleTests](./tests/IGLib.TestBase.ExampleTests/README_IGLib.TestBase.ExampleTests.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/tests/IGLib.TestBase.ExampleTests/README_IGLib.TestBase.ExampleTests.md)) - contains some example test classes and methods to look at when creating unit tests (t.g. use of output and loggers, multiple test inputs via method attributes...)
* ...
* [src/0InitModules/InitModulesCore](./src/0InitModules/InitModulesCore/README_InitModulesCore.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/0InitModules/InitModulesCore/README_InitModulesCore.md)) - initialization project for more basic projects; automatically runs initialization script for dependent projects (only once for all buiilds between rebuilds)
* [src/0InitModules/InitModulesCoreExtended](./src/0InitModules/InitModulesCoreExtended/README_InitModulesCoreExtended.md) ([readme on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/0InitModules/InitModulesCoreExtended/README_InitModulesCoreExtended.md)) - initialization project for extended projects, automatically runs initialization script for dependent projects

### Building and Running

To clone and work with this and other IGLib libraries (build, run, develop), use the new *[IGLib cotainer repository (iglibmodules)](https://github.com/ajgorhoe/iglibmodules)*. After cloning the repository, run one of the *PowerShell* scripts within `iglibmodules` clone for cloning / updating IGLib repositories, such as `UpdateRepos_Basic.ps1` or `UpdateReposLegacy_Extended.ps1`. After cloning, open the solution `IGLibCore.sln` within the current repository (`iglibmodules/IGLibCore/`), or one of the common solutions within the `iglibmodules/IGLibCore/` directory, such as `IGLibCore_All.sln`. See also the container repository's [readme file](https://github.com/ajgorhoe/iglibmodules/blob/main/README.md) for further information.

Project can also be built and run without cloning the `iglibmodules` repository. Just clone the current repository into `IGLibScriptingCs/` directory in the appropriate location in the local file system, open the solution in Visual Studio or other ID, and build and run.

Building will automatically clone some dependency repositories in the directory containing the current repository (side-by-side to the repository), including possibly some repositories in the `../_external/` directory.

Even better, you can manually clone the dependency repositories by runninng the scripts `scripts/UpdateDependencyRepos.ps1` and `scripts/UpdateDependencyReposExtended.ps1` before opening the solution. Without this preparation step, you may need to reload the dependency projects after running the first build (which will fail). After reloading the dependency projects, subsequent builds should pass.

The legacy IGLib's base library is located at:
*<https://github.com/ajgorhoe/IGLib.workspace.base.iglib.git>*. When building the repository using the *[iglibmodules container repository](https://github.com/ajgorhoe/iglibmodules)*, the legacy IGLibFramework projects can be build in a similar way as projects in the currend repository; just use the corresponding cloning scripts within the `../iglibmodules/` directory for cloning the legacy IGLib repositories, such as `UpdateReposLegacy_Basic.ps1` or `UpdateReposLegacy_Extended.ps1`.

## The Investigative Generic Library (IGLib)

### Repositories

* [IGLibCore](https://github.com/ajgorhoe/IGLib.modules.IGLibCore) (this repository) - a base library containing some common utiities with few dependencies (UI, graphics, reflection dependencies are not allowed).
* [iglib](https://github.com/ajgorhoe/IGLib.workspace.base.iglib) - the legacy IGLib's base libraries. Used also by newer applications and new IGLib, will continued to be maintained. Contains [documentation](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md) for the legacy IGLib.
* **Helper Repositories**:
  * [iglibmodules](https://github.com/ajgorhoe/iglibmodules) - a container repository used for cloning and building and running locally the IGLib libraries  and applications.
  * [IGLibScripts](https://github.com/ajgorhoe/IGLib.modules.IGLibScripts/) contains some usful scripts (e.g. for cloning and updating repositories, backups, some Windows utilities, etc.), also used by IGLib for cloning and updating the reoisitories.
  * [codedoc](https://github.com/ajgorhoe/IGLib.workspace.doc.codedoc) contains scripts and other tools for generating and viewing code documentation for software projects, including IGLib projects.
    * [CodeDocumentation](https://github.com/ajgorhoe/CodeDocumentation) contains compiled code documentation for parts of IGLib (experimental), generated via [codedoc](https://github.com/ajgorhoe/IGLib.workspace.doc.codedoc), then copied to and committed to tge repository.
* [IGLibGraphics3D](https://github.com/ajgorhoe/IGLib.modules.IGLibGraphics3D) - a 3D graphics library. <img src="https://ajgorhoe.github.io/IGLibFramework/images/Graphics3D/TreFoilKnotRayTracing/TransparentBackground/TrefoilKnot_Blue_RayTracer_BlackBackground_MetallicBSDF_Transparent_1.webp" alt="[Trefoil knot]" align="right" width="10em" style="float: right; max-width: 20%; width: 10em; margin-left: 2pt;" />
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
