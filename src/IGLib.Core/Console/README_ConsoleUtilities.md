
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






