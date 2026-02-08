
# Generic Parsing Utilities

This project contains generic parsing utilities to parse strings into various basic types such as numeric integer (`int`, `long`, `uint`, etc.) and floating point types (`float`, `double`, `decimal`), `char`, date and time types (`DateAndTime`, `DateAndTimeOffset`, `DateOnly`, `TimeOnly`) - see the scope below.

These utilities are used to convert user input to supported type, especially console input but also input obtained fom text files or graphical user interfaces.

## Generic `TryParse` Method

Signature of the method is

~~~csharp
public static bool TryParse<ParsedType>(string str, out ParsedType valueVariable, 
    IFormatProvider? formatProvider = null)
    where ParsedType : struct
{ ... }
~~~

The **scope of** the generic **type parameter** is the following:

* Built-in [integral numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types): `sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`,  `nint` (`IntPtr`), `unint`  (`UIntPtr`)
* Built-in [floating-point numeric types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types): `float`, `double`, `decimal`
* [char](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/char) (`System.Char`)
* [bool](https://learn.microsoft.com/en-us/dotnet/standard/datetime/) (`System.Boolean`)
* [date and time types](): `DateTime`,  `DateTimeOffset`, `TimeOnly`, `DateOnly` (with `TimeSpan` possibly be added later)

The method follows similar rules as static `TryParse` methods for simple types in .NET, such as [int.TryParse](https://learn.microsoft.com/en-us/dotnet/api/system.uint32.tryparse?view=net-10.0) method, and majority of them just call these methods with the appropriate arguments.

The **type parameter constraint** is `struct` and is very broad, in order to also include types like `DateTimeOffset`, `TimeOnly`, and `DateOnly`, for which the need to be parsed often appears in the intended applications.

### `TryParse` Parameters and Settings




