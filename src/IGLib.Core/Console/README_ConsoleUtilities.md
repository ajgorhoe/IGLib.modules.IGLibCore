
# Console Utilities

This project directory contains some helper utilities for working with console, such as reading various input in graceful way (e.g. allowing default values for numbers and booleans, different ways of inserting boolean values, etc.), utilities for inputting passwords via console, etc.

These utilities work with [console abstractions](./README_ConsoleAbstractions.md) rather than directly using the static [System.Console class](https://learn.microsoft.com/en-us/dotnet/api/system.console). This provides a proper background for testing classes that produce console output or read input from console. Example of how console abstractions enable easy testability can be found in tests of these utilities.

See also:

* [Common console section](./README_Console.md) of this library
* [Console abstractions](./README_ConsoleAbstractions.md) - console utilities provided here work with console abstractions, which also provide direct access to `System.Console` adapter.

**Contents**:

* [Details on Provided Utilities](#details-on-provided-utilities)
* [Where to Find Advanced Console UI and Other Libraries](#where-to-find-advanced-console-ui-and-other-libraries) - explains what this library does not provide and provides links to places where such functionality can be found 

## Details on Provided Utilities


## Where to Find Advanced Console UI and Other Libraries

Utilities provided here are very basic and **do not** provide rendering graphical user interfaces (GUIs) using system's console. We don't intend to support building GUI-like applications using console, our view is that proper GUI frameworks should be used for this purpose such as `WinForms` or `WPF` (MS Windows-only) or `Avalonia` or `MAUI` (cross-platform). Utilities provided here just provide some helpers for plain console use but using console abstractions rather than directly using the static `Sysem.Console` class.

If you ned libraries for rendering advanced console UI or other advanced console libraries, you can check out the following:

* [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui) - a library for creating advanced graphical user interfaces (GUI) using only console
* [Spectre.Console](https://github.com/spectreconsole/spectre.console) - another library for advanced GUIs using console
* [A list of console libraries for .NET](https://github.com/goblinfactory/konsole?tab=readme-ov-file#other-net-console-libraries)





