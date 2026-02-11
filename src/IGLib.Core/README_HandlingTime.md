
# Handling Times and Dates in .NET

This is a short guide with explanatory notes on handling dates and times in .NET,  including types, conversions, time zones, etc. This is relevant for topics such as [Parsing Date and Time Data](./README_ParsingUtilities.md#parsing-date-and-time-data).

**Contents**:

* [Links](#links)
* []
* [Time Zones and `TimeZoneInfo` class](#time-zones-and-timezoneinfo-class)
* [Formatting Time and Date Values](#formatting-time-and-date-values)
  * [Format Providers](#date-and-time-format-providers)
* About Specifying Times of Events ()

## Links

* **[Dates, Times, and Time Zones](https://learn.microsoft.com/en-us/dotnet/standard/datetime/)**
* [DateTimeOffset vs DateTime in C#](https://code-maze.com/csharp-datetimeoffset-vs-datetime/) (CodeMaze)
* [Converting Between DateTime and DateTimeOffset](https://www.crystalnet-tech.com/RuntimeLibrary/Help/html/scr/Converting_Between_DateTime_and_DateTimeOffset.html) (CrystalNet)
* **Types**:
  * [DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime)
    * **[Supplemental API Remarks](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-datetime)**
      * [ToBinary and FromBinary methods](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-datetime-tobinary)
      * [TryParse method](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-datetime-tryparse)
  * [DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset)
  * [TimeSpan]()
    * [Supplemental API Remarks](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-timespan)
      * [Parse method](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-timespan-parse)
      * [TryParse methods](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-timespan-tryparse)
  * [TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly)
  * [DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly)
  * [TimeZoneInfo](https://learn.microsoft.com/en-us/dotnet/api/system.timezoneinfo)
  * [TimeZoneInfo](https://learn.microsoft.com/en-us/dotnet/api/system.timezoneinfo)
  * [TimeProvider](https://learn.microsoft.com/en-us/dotnet/api/system.timeprovider)
  * Format-related types (fot Parse(), TryParse(), ToString()):
    * [IFormatProvider Interface / Remarks](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider#remarks) + derived type:
      * [CultureInfo Class](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)
      * [DateTimeFormatInfo Class](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo)
      * [DateTime API Remarks / TryParse method](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-datetime-tryparse)

## Time Zones and `TimeZoneInfo` class


~~~csharp

~~~


## Formatting Time and Date Values

### Date and Time Format Providers

See also:

* [IFormatProvider Interface / Remarks](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider#remarks) and **derived types**:
  * [CultureInfo Class](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)
  * [DateTimeFormatInfo Class](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo)
  * [NumberFormatInfo Class](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.numberformatinfo)
* [DateTime API Remarks / TryParse method](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-datetime-tryparse)

The [DateTime.TryParse(String, IFormatProvider, DateTimeStyles, DateTime)](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tryparse#system-datetime-tryparse(system-string-system-iformatprovider-system-globalization-datetimestyles-system-datetime@)) method parses a string that can contain date, time, and time zone information. It is similar to the corresponding `DateTime.Parse()` method, but it does not throw an exception if parsing fails. This method attempts to ignore unrecognized data and parse the input string completely. If the string contains a time but no date, the method by default substitutes the current date or, if `styles` includes the [NoCurrentDateDefault](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.datetimestyles#system-globalization-datetimestyles-nocurrentdatedefault) flag, it substitutes `DateTime.Date.MinValue`. If the string contains a date but no time, 12:00 midnight (0:00) is used as the default time. If a date is present but its year component consists of only two digits, it is converted to a year in the `provider` parameter's current calendar based on the value of the [Calendar.TwoDigitYearMax](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.calendar.twodigityearmax) property. Any leading, inner, or trailing white space characters in the string are ignored. The date and time can be bracketed with a pair of leading and trailing number sign characters ('#', U+0023), and can be trailed with one or more NULL characters (U+0000).

Specific valid formats for date and time elements, as well as the names and symbols used in dates and times, are defined by the `IFormatProvider` parameter, which can be any of the following:

* A [CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo) object that represents the culture whose formatting is used in the input string parameter. The [DateTimeFormatInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo) object returned by the [CultureInfo.DateTimeFormat](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.datetimeformat) property defines the formatting used in the parsed input string.
* A [DateTimeFormatInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo) object that defines the formatting used in the input string.
* A custom [IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider) implementation. Its [IFormatProvider.GetFormat](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider.getformat) method returns a [DateTimeFormatInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo) object that defines the formatting used in the input string.

If `IFormatProvider` is `null`, the current culture is used.

If parsed string is the string representation of a leap day in a leap year in the current calendar, the method parses it successfully. If it represents a leap day in a non-leap year in the current calendar of `provider`, the parse operation fails and the method returns `false`.

## About Specifying Times of Events

### Gregorian Calendar

See also:

* [Gregorian Calendar](https://en.wikipedia.org/wiki/Gregorian_calendar) (Wikipedia)
* [Calendar](https://en.wikipedia.org/wiki/Calendar)
  * [Calendar Epoch](https://en.wikipedia.org/wiki/Epoch)
  * [Calendar era](https://en.wikipedia.org/wiki/Calendar_era)




---

## Auxiliary

---

~~~csharp

~~~

---
