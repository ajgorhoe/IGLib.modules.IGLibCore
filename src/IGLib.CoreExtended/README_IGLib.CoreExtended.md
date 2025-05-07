
# The IGLib.CoreExtended Project

<a href="https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md"><img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" /></a>

* [Project repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCoreExtended)
* [IGLib.CoreExtended project within the repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/src/IGLib.CoreExtended)

This project contains **Core Utilities** of the **IGLib** (the **Investigative Generic Library**) that **do not satisfy strict constraints for inclusion in `IGLib.Core`** (see the [IGLib.Core readme file](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md)). These are common utilities that may be used in other more specialized libraries of the *IGLib*, but are of more general usability and can be used in other projects, too.

## Basic Information

This project is also used in the **[legacy IGLib](https://github.com/ajgorhoe/IGLib.workspace.base.iglib)** (see the [README file](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md)), which is still  used in several applications. the project is **required to target both .NET Framework 4.8 and newer Frameworks** such as *.NET 8 and higher* (the latter versions will change as new stable or long term support frameworks arrive), and also supports **.NET Standard** 2.0. Please note that **new C# language features are enabled** via `<LangVersion>latest</LangVersion>` in the project file.

This library **should not contain complex third-party dependencies**. If sufficiently justified, some third-party dependencies dependencies of very general use **may be allowed in the future**.

In particular, this library **can contain code that relies on reflection** (i.e., the code that needs to include the reflection namespace via `using System.Reflection`). Therefore, this library **contains code that would fit into `IGLib.Core` but does not satisfy the strict constraints** of the IGLib.Core library. Because of this, the library **may not be suitable for inclusion in some reduced capability projects**, e.g. project that need to allow *self-contained deployment scenarios where trimming is used*. One such example may be client Blazor projects where trimming is essential for reducing application loading times, but this may also apply to many other scenario, e.g. those with limited hardware resources (like some embedded applications).

The sole **purpose of this library** is to provide supplemental **core utilities that would break constraints of `IGLib.Core`**. In this way, `IGLib.Core` can include most of the code that represent highly reusable core utilities, but can still be included in projects with the aforementioned constraints. **This library** (the *`IGLib.CoreExtended`*) can be added to projects without those constraints to **supplement `IGLib.Core`**, e.g. to provide useful utilities that rely on reflection or some other thing that may limit the usability of the library in certain restricted scenarios.

### Testing

**Unit tests** and some lower level integration tests are provided in the [IGLib.Core.Tests](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/tests/IGLib.Core.Tests) project.
