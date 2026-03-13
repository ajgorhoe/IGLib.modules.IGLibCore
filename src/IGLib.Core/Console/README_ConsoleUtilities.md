
# Console Utilities

This project directory contains some helper utilities for working with console, such as reading various input in graceful way (e.g. allowing default values for numbers and booleans, different ways of inserting boolean values, etc.), utilities for inputting passwords via console, etc.

These utilities work with [console abstractions](./README_ConsoleAbstractions.md) rather than directly using the static [System.Console class](https://learn.microsoft.com/en-us/dotnet/api/system.console). This provides a proper background for testing classes that produce console output or read input from console. Example of how console abstractions enable easy testability can be found in tests of these utilities.

See also:

* [Common console section](./README_Console.md) of this library
* [Console abstractions](./README_ConsoleAbstractions.md) - console utilities provided here work with console abstractions, which also provide direct access to `System.Console` adapter.

**Contents**:

* [Details on Provided Utilities](#details-on-provided-utilities)
* [Where to Find Advanced Console UI and Other Libraries](#where-to-find-advanced-console-ui-and-other-libraries) - explains what this library does not provide and provides links to places where such functionality can be found
* [Remarks on Reading Passwords form Console](#remarks-on-reading-passwords-form-console)

## Details on Provided Utilities

## Where to Find Advanced Console UI and Other Libraries

Utilities provided here are very basic and **do not** provide rendering graphical user interfaces (GUIs) using system's console. We don't intend to support building GUI-like applications using console, our view is that proper GUI frameworks should be used for this purpose such as `WinForms` or `WPF` (MS Windows-only) or `Avalonia` or `MAUI` (cross-platform). Utilities provided here just provide some helpers for plain console use but using console abstractions rather than directly using the static `Sysem.Console` class.

If you ned libraries for rendering advanced console UI or other advanced console libraries, you can check out the following:

* [Terminal.Gui](https://github.com/gui-cs/Terminal.Gui) - a library for creating advanced graphical user interfaces (GUI) using only console
* [Spectre.Console](https://github.com/spectreconsole/spectre.console) - another library for advanced GUIs using console
* [A list of console libraries for .NET](https://github.com/goblinfactory/konsole?tab=readme-ov-file#other-net-console-libraries)

## Remarks on Reading Passwords form Console

The `ConsoleUtilities` class also contains some utilities for reading passwords from console (including using console abstractions). Below is a **summary** of some key points about **securely reading passwords from the console in .NET**, including a simplified implementation and use.

### Securely Reading Passwords from the Console in .NET

Reading passwords from a console application requires special care to prevent accidental disclosure. Two main aspects must be considered:

1. **Preventing the password from being visible during input**
2. **Avoiding insecure storage of the password in memory**

### Preventing Password Echo on the Console

The standard `Console.ReadLine()` method echoes characters as the user types them, which is unsuitable for passwords. Instead, passwords should be read using:

~~~csharp
Console.ReadKey(intercept: true)
~~~

The `intercept: true` parameter ensures the pressed key **is not printed to the console**.

Optionally, a masking character (e.g., `*`) may be printed to indicate input progress.

### Why Storing Passwords in `string` Is Unsafe

In .NET, `string` objects are **immutable**, which introduces several security issues when storing sensitive information in them:

1. **Immutable memory**

   Once a `string` is created, its contents cannot be modified.

2. **Cannot be cleared**

   The password remains in memory until the garbage collector reclaims the object.

3. **Possible additional copies**

   During execution, the runtime may create additional copies of the string.

4. **Memory dump exposure**

   If the process memory is dumped (e.g., debugging, crash dumps), the plaintext password may appear.

Because of these properties, storing sensitive secrets in a `string` increases the likelihood that the password remains in memory longer than necessary.

### Preferred Alternative: Mutable Character Buffers

A better approach is to store passwords in a **mutable buffer**, such as:

* `char[]`
* `List<char>`
* `Span<char>`

These structures allow the program to **explicitly overwrite the memory containing the password** once it is no longer needed. This reduces the risk of passwords lingering in memory.

### Why `SecureString` Is No Longer Recommended

In older versions of .NET, the `SecureString` class was intended to address password security concerns. It attempted to:

* Encrypt the password in memory
* Allow controlled disposal
* Reduce plaintext exposure

However, modern .NET guidance **discourages using `SecureString`** for new development for the following reasons:

1. **Limited real security benefit**

   Secrets often need to be converted back to plaintext to be used (e.g., authentication APIs).

2. **Platform differences**

   Some platforms cannot reliably protect the memory in the intended way.

3. **Added complexity**

   The API introduces complexity without providing meaningful security improvements.

Because of these issues, it is recommended to **use normal memory buffers but minimize secret lifetime and clear memory explicitly**.

### Recommended Password Reading Method

A practical and safe implementation uses a `List<char>` as a dynamically sized buffer.

Advantages:

* avoids difficult buffer size decisions
* supports editing (backspace)
* allows explicit wiping of memory

Example implementation (simple):

~~~csharp
using System;
using System.Collections.Generic;

static char[] ReadPasswordChars()
{
    var buffer = new List<char>(40);
    while (true)
    {
        var key = Console.ReadKey(intercept: true);

        if (key.Key == ConsoleKey.Enter)
        {
            Console.WriteLine();
            break;
        }
        if (key.Key == ConsoleKey.Backspace && buffer.Count > 0)
        {
            buffer[buffer.Count - 1] = '\0';
            buffer.RemoveAt(buffer.Count - 1);
            Console.Write("\b \b");
            continue;
        }
        buffer.Add(key.KeyChar);
        Console.Write('*');
    }
    char[] result = buffer.ToArray();
    // wipe temporary storage
    for (int i = 0; i < buffer.Count; i++)
        buffer[i] = '\0';
    buffer.Clear();
    return result;
}
~~~

This implementation:

* prevents console echo
* supports backspace editing
* minimizes intermediate memory exposure
* clears temporary buffers before returning

The actual implementation in the `ConsoleUtilities` class provides some additional improvements.
