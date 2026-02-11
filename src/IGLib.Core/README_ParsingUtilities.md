
# Generic Parsing Utilities

This project contains generic parsing utilities to parse strings into various basic types such as numeric integer (`int`, `long`, `uint`, etc.) and floating point types (`float`, `double`, `decimal`), `char`, `bool`, date and time types (`DateAndTime`, `DateAndTimeOffset`, `DateOnly`, `TimeOnly`) - see the scope below.

These utilities are used to convert user input to supported type, especially console input but also input obtained fom text files or graphical user interfaces.

## Generic `TryParse` Method

Signature of the method is

~~~csharp
public static bool TryParse<ParsedType>(string str, out ParsedType result, 
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

### Generic `TryParse` Parameters and Settings

The method returns `true` if parsing is successful, and false if not.

The first parameter `str` of the `TryParse` method is the string to be parsed.

The second parameter `result` is an `out` parameter that will contain the value of the specific type parsed from the string parameter. If parsing is not successful then default value of the specific type is stored in `result`. Since the **generic type is determined by type of parameter `result`**, it is *not necessary to specify it* explicitly.

The **optional** parameter `formatProvider` can be used to specify the **culture in which the parsed value is represented** in parameter `str`. The only parameter that defines the allowed format of the parsed string `str`. **Default is** `null`, which is translated internally to **[`CultureInfo.InvariantCulture` 🔗](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.invariantculture)**. This is intentional and by design, in order for parsing with omitted parameter to give the same results on different devices (with different system's regional settings); this makes easier when transferring data between computers via text files, but requires users to explicitly specify the parameter (e.g. as `CultureInfo.CurrentCulture`) when wanting to use the system's regional format. This is somewhat different from the behavior of method overloads like `int.Parse(string, out int)`, which resort to [`CultureInfo.CurrentCulture` 🔗](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.currentculture) when there is no `IFormatProvider` parameter.

Built-in parsing methods for basic types provide overloads with parameters defining **additional formatting information** such as the allowed number styles. The generic `TryParse` method does not provide such overloads, and the **additional formatting information** is fixed in the way that it **allows large variety of familiar input formats**. For example:

* For integral numeric types, the method uses [`NumberStyles.Integer | NumberStyles.AllowThousands` 🔗](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberstyles)
* For floating point numeric types, the method uses `NumberStyles.Float | NumberStyles.AllowThousands`

### Parsing Date and Time Data

See also:

* [Handling Times and Dates in .NET](./README_HandlingTime)


