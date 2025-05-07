
# The IGLib.Core Project

<a href="https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md"><img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" /></a>

* [Project repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore)
* [IGLib.Core project within the repository](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/src/IGLib.Core)

This project contains **Core Utilities** of the **IGLib** (the **Investigative Generic Library**). These are common utilities that may be used in other more specialized libraries of the *IGLib*, but are of more general usability and can be used in other projects, too.

## Basic Information

This project is also used in the **[lejgacy IGLib](https://github.com/ajgorhoe/IGLib.workspace.base.iglib)** (see the [README file](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md)), which is still  used in several applications. the project is **required to target both .NET Framework 4.8 and newer Frameworks** such as *.NET 8 and higher* (the latter versions will change as new stable or long term support frameworks arrive), and also supports **.NET Standard** 2.0. Please note that **new C# language features are enabled** via `<LangVersion>latest</LangVersion>` in the project file.

This library **should not contain complex third-party dependencies**. In particular, it should **not rely on code that extensively uses reflection**. This is necessary when one wants to allow *self-contained deployment scenarios where trimming is used*. **Code that** provides generally used core utilities but **does not satisfy these constraints** should be **put to the extended project counterpart, `IGLib.CoreExtended`**.

### Testing

**Unit tests** and some lower level integration tests are provided in the [IGLib.Core.Tests](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/tests/IGLib.Core.Tests) project.
