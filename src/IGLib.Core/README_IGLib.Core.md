
# The `IGLib.Core` Project

This project contains **Core Utilities** of the **IGLib** (the **Investigative Generic Library**). These are common utilities that may be used in other more specialized libraries of the *IGLib*, but are of more general usability and can be used in other projects, too.

Code in this project should not use reflection and should not have significant dependence on external libraries. It is meant to be a slim library, suitable for ahead of time (AOT) compilation and deployment on devices with limited resources where trimming might be required.  

<a href="https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md"><img src="https://ajgorhoe.github.io/icons/IGLibIcon_256x256.png" alt="[IGLib]" align="right" width="48pt"
  style="float: right; max-width: 30%; width: 48pt; margin-left: 8pt;" /></a>

* [Additional Information about the Project](#additional-information-about-the-project)
  * Other documents:
    * **[Console Abstractions and Console Utilities](./Console/README_Console.md)**:
      * [Abstractions](./Console/README_ConsoleAbstractions.md); [Utilities](./Console/README_ConsoleUtilities.md)
    * [Generic parsing Utilities](./README_ParsingUtilities.md)
  * [Testing](#testing)
* Other:
* [Repository's README](../../README.md) ([on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/README.md))
  * [About the Repository](../../README.md#about-this-repository---iglibcore)
    * [Projects within IGLib.Core repository](../../README.md#projects-within-the-iglibcore-repository)
  * [The Investigative Generic Library (IGLib)](../../README.md#the-investigative-generic-library-iglib) - information about IGLib as a whole
* [this document on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Core/README_IGLib.Core.md); [project directory on GitHub](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/src/IGLib.Core)

## Additional Information about the Project

This project is also used in the **[legacy IGLib](https://github.com/ajgorhoe/IGLib.workspace.base.iglib)** (see the [README file](https://github.com/ajgorhoe/IGLib.workspace.base.iglib/blob/master/README.md)), which is still  used in several applications. the project is **required to target both .NET Framework 4.8 and newer Frameworks** such as *.NET 8 and higher* (the latter versions will change as new stable or long term support frameworks arrive), and also supports **.NET Standard** 2.0. Please note that **new C# language features are enabled** via `<LangVersion>latest</LangVersion>` in the project file.

This library **should not contain complex third-party dependencies**. In particular, it should **not rely on code that extensively uses reflection**. This is necessary when one wants to allow *self-contained deployment scenarios where trimming is used*. **Code that** provides generally used core utilities but **does not satisfy these constraints** should be **put to the extended project counterpart, `IGLib.CoreExtended`**.

For documentation on some of the utilities provided by this library, see other documents, such as:

* **[Console Abstractions and Console Utilities](./Console/README_Console.md)**:
  * [Abstractions](./Console/README_ConsoleAbstractions.md); [Utilities](./Console/README_ConsoleUtilities.md)
* [Parsing Utilities](./README_ParsingUtilities.md)

### Testing

**Unit tests** and some lower level integration tests are provided in the [IGLib.Core.Tests](https://github.com/ajgorhoe/IGLib.modules.IGLibCore/tree/main/tests/IGLib.Core.Tests) project.
