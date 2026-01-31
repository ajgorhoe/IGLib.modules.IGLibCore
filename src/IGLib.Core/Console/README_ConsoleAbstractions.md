
# Console Abstractions

See also the [main Console page](./README_Console.md) and the [Console Utilities page](./README_ConsoleUtilities.md).

This contains abstractions of the [System.Console class](https://learn.microsoft.com/en-us/dotnet/api/system.console). Using abstractions and implementing classes instead of the original `System.Console` class makes easier to test classes that interact via console when these classes use abstractions.

The main implementation is the `SystemConsole` class, which together with the `IConsole`'s extension methods replicates large portion of the commonly used `System.Console`'s API. Methods of this class forward work to methods of the static `System.Console` class with equivalent signatures.

Another important implementation is the one that implements a fake console, which can be used in automatic tests. In classes that use console abstractions instead of `System.Console`, the default implementation (based on System.Console and used in production) can be replaced by the fake console for testing, which enables simulating user input and capturing console output in order to test whether behavior of a class that uses console for output and user's input is correct.

Abstractions come with layered interfaces, such that oly the area of the `Console` API that are interested for the class can be focused on.



