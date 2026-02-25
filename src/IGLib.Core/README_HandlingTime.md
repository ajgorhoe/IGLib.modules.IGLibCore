
# Handling Times and Dates in .NET

This is a short guide with explanatory notes on handling dates and times in .NET,  including types, conversions, time zones, etc. This is relevant for topics such as [Parsing Date and Time Data](./README_ParsingUtilities.md#parsing-date-and-time-data).

**Contents**:

* [Links](#links)
* [Introduction to Handling Time-related Data  in Software
](#introduction-to-handling-time-related-data--in-software)
* [Logic of Time Handling in .NET](#logic-of-working-with-time-and-dates-in-net)
  * [DateTime and DateTimeOffset Structs](#datetime-and-datetimeoffset-structs)
    * [Logic of Arithmetic Operations, Comparisons, and Conversions Between Time Representations](#logic-of-arithmetic-operations-comparisons-and-conversions-between-time-representations)
    * [DateTimeOffset Struct](#the-datetimeoffset-struct)
    * [DateOnly and TimeOnly Sruct](#the-dateonly-and-timeonly-structs)
* [Time Zones and `TimeZoneInfo` class](#time-zones-and-timezoneinfo-class)
* [Formatting Time and Date Values](#formatting-time-and-date-values)
  * [Format Providers](#date-and-time-format-providers)
* [About Specifying Times of Events](#about-specifying-times-of-events)
  * [Coordinated Universal Time(UTC)](#coordinated-universal-time-utc)
    * [Limitations of UTC and Computers' Time Capturing Capabilities](#limitations-of-utc-and-computers-time-capturing-capabilities)
* [Gregorian Calendar](#gregorian-calendar)

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
  * [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan)
    * [Supplemental API Remarks](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-timespan)
      * [Parse method](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-timespan-parse)
      * [TryParse methods](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-timespan-tryparse)
  * [TimeOnly](https://learn.microsoft.com/en-us/dotnet/api/system.timeonly)
  * [DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly)
  * [TimeZoneInfo](https://learn.microsoft.com/en-us/dotnet/api/system.timezoneinfo)
  * [TimeProvider](https://learn.microsoft.com/en-us/dotnet/api/system.timeprovider)
  * [Calendar](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.calendar) - Represents time in divisions, such as weeks, months, and years. Calculation of year/month/week/day of week, leap years/months/days, addition/subtraction of time intervals, etc.
    * Examples: [Gregorian calendar](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.gregoriancalendar), [Julian calendar](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.juliancalendar), [Hebrew calendar](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.hebrewcalendar), [KoreanLunisolarCalendar](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.koreanlunisolarcalendar), [ChineseLunisolarCalendar](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.chineselunisolarcalendar)
    [Working with calendars](https://learn.microsoft.com/en-us/dotnet/standard/datetime/working-with-calendars)
  * **Format-related** types (fot Parse(), TryParse(), ToString()):
    * [IFormatProvider Interface / Remarks](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider#remarks) + derived type:
      * [CultureInfo Class](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo)
      * [DateTimeFormatInfo Class](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.datetimeformatinfo)
      * [DateTime API Remarks / TryParse method](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-datetime-tryparse)

## Introduction to Handling Time-related Data  in Software

When working with time-related data, it is crucial to define the context beforehand and identify the scope of time handling according to the needs. In the most **basic scenario**, all times can be defined as **local times**, corresponding to the *[time zone](https://en.wikipedia.org/wiki/Time_zone)* of the host device on which the software is operating. The advantage of this is that times are readily stored in memory, recorded, and displayed or printed as times displayed in local clocks and used in the specific local environment, provided that the time zone of the device is correctly set and time is synchronized.

However, using only local times has two major limitations. First, some time zones observe [daylight saving time (DST)](https://en.wikipedia.org/wiki/Daylight_saving_time). Once a year (usually in the spring), the clocks are moved forward. This creates a gap in the displayed times of closely spaced events just after the shift. At another time of year, the clocks are moved back, causing another strange effect: some times are duplicated. Within the time interval of the shift, times have an ambiguous meaning because they can refer to either summer or winter time. Therefore, local time is not monotonic, making it unsuitable for recording events where the sequence or exact intervals need to be preserved.

A second limitation arises when times need to be exchanged between different locations on Earth or when times of events occurring in different locations (across time zones) need to be recorded. Local times describing the same point in time vary across time zones by approximately one day. The maximum difference between local times on Earth at any given moment is actually 26 hours rather than 24, and there is a two-hour window every day when three different calendar days exist simultaneously. To correlate local times with physical time, we must state the wall-clock time and the time zone in which it is recorded, which complicates software maintenance.

To overcome the limitations of using local time, we use a standardized time, or coordinate time, which is consistent regardless of where on Earth an event occurs or where information about distributed events is gathered. Universal Coordinated Time (UTC) is the primary time standard by which the world regulates clocks and is used in computer software as a standardized time.

Using a consistent strategy to handle times in software can significantly reduce complexity and increase maintainability and interoperability with other software. For example, event times are recorded in UTC, as are times exchanged between distributed locations or stored persistently. UTC times are converted to or from local times when needed, typically in use cases involving user interaction. For instance, when a user inputs a time at which an alarm should go off or an action should be triggered by the software, or when a time of an event is displayed to a user in their local time for easier interpretation. In some cases, times may need to be converted between time zones or displayed for different time zones (e.g., when planning itineraries or coordinating remote meetings with participants across time zones). This can be achieved by keeping times in UTC and converting them to different time zones.

Special attention is needed when comparing or ordering times, or when performing arithmetic operations on times, such as calculating time intervals between events or calculating times by adding or subtracting specified time intervals from a known time. To obtain correct results, map the involved times to the same representation (often UTC) before performing operations. It simplifies matters to keep all recorded times in the appropriate standardized format and to convert to local times or specific time zones only when necessary.

Conversions to and from Universal Coordinated Time (UTC) are generally available in system and base programming language libraries. UTC is an adequate standard time for a wide range of software applications.

However, UTC may not be appropriate in some scenarios where precise time interval calculations, the correct ordering of events in time, and a strictly monotonic measure of time are critical. Examples include high-frequency trading, scientific instrumentation, distributed systems logging, navigation, and aerospace.

One limitation of UTC is the introduction of leap seconds. Leap seconds are occasionally introduced to keep UTC in sync with the varying length of the astronomical day (as a consequence of variation of rotational speed of the Earth). Recent dates with a leap second include December 31, 2008; June 30, 2012; June 30, 2015; and December 21, 2016 (as of 2026, no additional leap seconds have been introduced yet). When a positive leap second occurs, UTC repeats or stretches time, and the clock sequence is 23:59:59 → 23:59:60 → 00:00:00. This means that the same timestamps appear twice within the one-second interval before midnight, which causes a time ordering problem.

There are further limitations in precision and accuracy of time capturing time on computers and other general-purpose computational devices. Although types for storing time may have below microsecond resolution, the clock updates much slower (e.g. every 15.6 milliseconds on Windows). Precision of typical PC clocks is not do high and there may be of order of a second per a day drift if not synced. There is also variability in how long it takes to process a clock interrupt.

For more information on the limitations of UTC and system time-related utilities, see the section [Limitations of UTC and System Time-Related Utilities](#limitations-of-the-utc-and-system-time-capturing-utilities).

## Logic of Time Handling in .NET

### `DateTime` and `DateTimeOffset` Structs

*Structs* **[DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime)** and **[DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset)** are the **main types for date and time operations** like timestamping, obtaining the current time, capturing, storing and exchanging times, and performing various date and time calculations. Both store representation of a specific point in time, and provide **properties to extract** various human-familiar **components of date and time** of the day corresponding to the stored point in time: the calendar year (`Year`), month (`month`, between 1 and 12), day of the month (`Day`, between 1 and 12), day of the week (`DayOfWeek`, as `DayOfWeek` enum, `Sunday` (0) thru `Saturday` (6)), day of the year (`DayOfYear`, between 1 and 366), hour (`Hour`, between 0 and 23), minute (`Minute`, between 0 and 59), second (`Second`, between 0 and 59), millisecond (`Millisecond`, between 0 and 999), microsecond (`Microsecond`, between 0 and 999), and nanosecond (`Nanosecond`, between 0 and 900, in 100-nanosecond increments). Static method `DaysInMonth(int year, int month)` returns the number of days in the specified month (of the specified year).

The **`Ticks` property** (of type `long`) represents the number of **100-nanosecond intervals** that have elapsed **since 12:00:00 midnight on January 1, year 1** (in Gregorian calendar, years AD are counted from 1, and years BC are counted from -1 down, there is no year 0). This is what is **actually stored** in a `DateTime` structs, and **other properties are calculated from the `Ticks` value**. A nanosecond is one billionth of a second, so there are ten million ticks in a second. A tick or **100 nanoseconds is the smallest representable interval** in types that keep absolute time in .NET, such as `DateTime` or `DateTimeOffset`.

Both types have a number of **constructors** to create new values of `DateTime` and `DateTimeOffset`. A large group of [DateTime constructors](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.-ctor) *specifically state* components of the current *calendar day and time components*. Up to first 8 `int` parameters specify the *year*, *month*, *day of month*, *hour*, *minute*, *second*, *millisecond*, and *microsecond* of the created `DateTime` value. There can be less leading integer parameters: with *7 integer parameters*, microseconds are omitted; with *8 integer parameters*, milliseconds are also omitted; with *3 integer parameters*, only the date part is specified but the time of the day is not, and is set to 00:00:00 (0r 12:00:00 AM).

Beside integer parameters that define the date and time of the day, one can also specify the [Calendar](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.calendar) and the [DateTimeKind](https://learn.microsoft.com/en-us/dotnet/api/system.datetimekind) parameter. The [`Calendar` parameter](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.calendar) defines how time is divided into years, months and days, and defaults to [Gregorian calendar](#gregorian-calendar). The [`DateTimeKind` parameter](https://learn.microsoft.com/en-us/dotnet/api/system.datetimekind) defines whether the time is represented as local time or UTC time, or unspecified (default) - see the discussion about the `DateTime.Kind` property below.

The other kinds of `DateTime` constructors use the `long` value of `ticks`, or the combination of `DateOnly` and `TimeOnly` structs.

In order to query the current time, the `DateTime` and `DateTimeOffset` provide the **static properties `Now` and `UTCNow`**, which get instances (values) of their type set to the **current date and time** of the computer. **`Now`** is expressed as the **local time** (according to the time zone set on the computer), while **`UTCNow`** is expressed as the **Coordinated Universal Time (UTC)**.

#### Logic of Arithmetic Operations, Comparisons, and Conversions Between Time Representations

`DateTime` can store times as **local times** (expressed in the local [time zone](#time-zones-and-timezoneinfo-class) set on the computer) or as **UTC times**. It has the ***Kind* property**, which is a `DateTimeKind` enum with values `Unspecified` (0), `Utc` (1), and `Local` (2). This specifies whether the contained time is **represented as local or UTC time**, or this is not specified. **Conversions between local and UTC time representations are done consistently** by utilizing the `DateKind.Kind` property: they can result in nominally different times (possibly shifted according to the local time zone's rules), but **conversion result represents the same point in time as the original**. The `Kind` property of the result is set consistently with the target representation. Converting to the same representation does not change the value (for `DateTime`, it preserves both the `Ticks` and the `Kind` property). The `DateTime` and `DateTimeOffset` values are **converted to local representation by the `ToLocalTime`** and **to UTC representation by the `ToUniversalTime` instance method**.

On the contrary, **comparison operations and arithmetic operations on** different representations (local or UTC) of time **do not take into account the `DateTime.Kind` property**. These operations **do not operate consistently on `DateTime` values as points in time**. Instead, they just work on nominal times - the raw numerical values stored within the `DateTime` struct, based on the tick count (the `Ticks` property), ignoring the time zone-related metadata provided by the `Kind` property.

The above behavior has many adverse implications, which are discussed below. This has led some to call the `Datetime` type the most dangerous in the .NET ecosystem. There are **several reasons for such design**; it is partially due to *legacy of the early .NET and backward compatibility*; *performance and "value type" philosophy*: comparison and arithmetic operations operate on `long` integers and don't need to check the `Kind`, handle conversions, or look up system time zone rules; *separation of concerns*: `DateTime` is meant a data container, not a logic engine (it hods the *what*, while `TimezoneInfo` class and developer's logic hod the `where` and `how`). The design is insufficient for modern, cloud-based, multi-timezone applications. To overcome its deficiencies, the [DateTimeOffset](#the-datetimeoffset-struct) can be used.

It is easy to unintentionally **compare or subtract mixed local and UTC representations** stored in `TimeAndDate` values, but these operations will generally produce **mathematically / physically wrong results**: time of the event that occurred later may occur smaller than time of the event that occurred earlier, and calculated difference between two events recorded in local and UTC representation may not correspond to actual time intervals between the two events. This problem can **also** occur when performing comparison or arithmetic operations **on all-local** `DateTime` values, due to daylight saving time observed in some time zones. Another issue is that `DateTime` **does not include complete information** necessary for conversion to UTC time, which provides more mathematically consistent time measure (apart from [some additional limitations](#limitations-of-utc-and-computers-time-capturing-capabilities)). When local time is stored, the value of the `Kind` property (`DateTimeKind.Local`) indicates that the stored time value should be interpreted according to the local time zone of the computer, but **the struct does not provide the information about which time zone** this is. This information is retrieved as global computer setting, residing outside of the struct. The settings **can change due to re-configuration**, and errors can also arise when recorded `DateTime` values are exported and then imported on another geographically dislocated computer, where they are interpreted according to a different local time zone.

Let us take a closer look in the above described behavior on some examples. **Consistent conversion behavior** is demonstrated in the example below. First, a specific local date and time representation is created using the appropriate constructor. The original local representation is then converted to UTC representation by calling the `ToUniversalTime` method, then back to local representation by calling the `ToLocalTime` method. After each step, the created `DateTime` value is written to console, together with its `Kind` property. In the second part, local representation is converted to local representation, which preserves the value. Comparing the values shows that all conversions result in representations of the same point in time than the originals. The generated outputs are for a local time zone that is one hour ahead of UTC. The example would lose the demonstrative value if the local time was the same as UTC (e.g. for locations close to the zeroth meridian, no daylight saving).

~~~csharp
// Demonstration of round-trip conversion of the current time from local to UTC representation and back:
DateTime tNow = new DateTime(2028, 03, 15, 18, 00, 00, DateTimeKind.Local);
Console.WriteLine("Local representation of the current time:");
Console.WriteLine($"  {tNow.ToString()}; Kind: {tNow.Kind}");
DateTime tNowToUtc = tNow.ToUniversalTime();
Console.WriteLine("Converted to UTC representation:");
Console.WriteLine($"  {tNowToUtc.ToString()}; Kind: {tNowToUtc.Kind}");
DateTime tNowToUtcToLocal = tNowToUtc.ToLocalTime();
Console.WriteLine("Converted back to local representation (round-trip):");
Console.WriteLine($"  {tNowToUtcToLocal.ToString()}; Kind: {tNowToUtcToLocal.Kind}");
Console.WriteLine($"  Equals the original: {tNowToUtcToLocal == tNow}");
// Conversion to the same representation preserves the value:
DateTime tNowToLocal = tNow.ToLocalTime();
Console.WriteLine("Original local representation converted to local representation:");
Console.WriteLine($"  {tNowToLocal.ToString()}; Kind: {tNowToLocal.Kind}");
Console.WriteLine($"  Equals the original: {tNowToLocal == tNow}");
// Example output (for a time zone with UTC+01:00)
// Local representation of the current time:
//   3/15/2028 6:00:00 PM; Kind: Local
// Converted to UTC representation:
//   3/15/2028 5:00:00 PM; Kind: Utc
// Converted back to local representation (round-trip):
//   3/15/2028 6:00:00 PM; Kind: Local
//   Equals the original: True
// Original local representation converted to local representation:
//   3/15/2028 6:00:00 PM; Kind: Local
//   Equals the original: True

// Additional test: comparison and difference:
Console.WriteLine($"\nLocal equals UTC: {tNow == tNowToUtc}");
Console.WriteLine($"Local - UTC: {tNow - tNowToUtc}");
// Output:
// Local equals UTC: False
// Local - UTC: 01:00:00
~~~

<!-- 
The example below is similar, but it starts from the current time represented as UTC time, cna creates round trip to local time and then back to UTC time.

~~~csharp
// Demonstration of round-trip conversion of the current time from UTC to local representation and back:
DateTime tUtcNow = DateTime.UtcNow;:
Console.WriteLine("UTC representation of the current time:");
Console.WriteLine($"  {tUtcNow.ToString()}; Kind: {tUtcNow.Kind}");
DateTime tUtcNowToLocal = tUtcNow.ToLocalTime();
Console.WriteLine("Converted to Local representation:");
Console.WriteLine($"  {tUtcNowToLocal.ToString()}; Kind: {tUtcNowToLocal.Kind}");
DateTime tUtcNowToLocalToUtc = tUtcNowToLocal.ToUniversalTime();
Console.WriteLine("Converted back to UTC representation (round-trip):");
Console.WriteLine($"  {tUtcNowToLocalToUtc.ToString()}; Kind: {tUtcNowToLocalToUtc.Kind}");
Console.WriteLine($"  Equals the original: {tUtcNowToLocalToUtc == tUtcNow}");
// Conversion to the same representation preserves the value:
DateTime tUtcNowToUtc = tUtcNow.ToUniversalTime();
Console.WriteLine("Original UTC representation converted to UTC representation:");
Console.WriteLine($"  {tUtcNowToUtc.ToString()}; Kind: {tUtcNowToUtc.Kind}");
Console.WriteLine($"  Equals the original: {tUtcNowToUtc == tUtcNow}");
// Example output (time zone with UTC+01:00)
// UTC representation of the current time:
//   2/22/2026 8:15:27 PM; Kind: Utc
// Converted to Local representation:
//   2/22/2026 9:15:27 PM; Kind: Local
// Converted back to UTC representation (round-trip):
//   2/22/2026 8:15:27 PM; Kind: Utc
//   Equals the original: True
// Original UTC representation converted to UTC representation:
//   2/22/2026 8:15:27 PM; Kind: Utc
//   Equals the original: True
~~~
-->


#### The `DateTimeOffset` Struct


In the following example, we perform similar **conversions of `DateTimeOffset` values**. We **start with UTC representation** of the current time, convert it to local representation, and back to UTC representation.

~~~csharp
// Demonstration of round-trip conversion of the current time from UTC to local representation and back:
DateTimeOffset tUtcNow = DateTimeOffset.UtcNow;
Console.WriteLine("UTC representation of the current time:");
Console.WriteLine($"  {tUtcNow.ToString()}; Kind: {tUtcNow.DateTime.Kind}");
DateTimeOffset tUtcNowToLocal = tUtcNow.ToLocalTime();
Console.WriteLine("Converted to Local representation:");
Console.WriteLine($"  {tUtcNowToLocal.ToString()}; Kind: {tUtcNowToLocal.DateTime.Kind}");
DateTimeOffset tUtcNowToLocalToUtc = tUtcNowToLocal.ToUniversalTime();
Console.WriteLine("Converted back to UTC representation (round-trip):");
Console.WriteLine($"  {tUtcNowToLocalToUtc.ToString()}; Kind: {tUtcNowToLocalToUtc.DateTime.Kind}");
Console.WriteLine($"  Equals the original: {tUtcNowToLocalToUtc == tUtcNow}");
// Conversion to the same representation preserves the value:
DateTimeOffset tUtcNowToUtc = tUtcNow.ToUniversalTime();
Console.WriteLine("Original UTC representation converted to UTC representation:");
Console.WriteLine($"  {tUtcNowToUtc.ToString()}; Kind: {tUtcNowToUtc.DateTime.Kind}");
Console.WriteLine($"  Equals the original: {tUtcNowToUtc == tUtcNow}");
// Example output (time zone with UTC+01:00)
// UTC representation of the current time:
//   2/25/2026 1:09:07 AM +00:00; Kind: Unspecified
// Converted to Local representation:
//   2/25/2026 2:09:07 AM +01:00; Kind: Unspecified
// Converted back to UTC representation (round-trip):
//   2/25/2026 1:09:07 AM +00:00; Kind: Unspecified
//   Equals the original: True
// Original UTC representation converted to UTC representation:
//   2/25/2026 1:09:07 AM +00:00; Kind: Unspecified
//   Equals the original: True

// Additional test: comparison and difference:
Console.WriteLine($"\nLocal equals UTC: {tUtcNowToLocal == tUtcNow}");
Console.WriteLine($"Local - UTC: {tUtcNowToLocal - tUtcNow}");
// Output:
// Local equals UTC: True
// Local - UTC: 00:00:00
~~~





~~~csharp

(DateTime.Now, DateTime.Now.ToUniversalTime(), DateTime.Now.ToUniversalTime().ToLocalTime()).ToString()

( DateTime.UtcNow, DateTime.UtcNow.ToLocalTime(), DateTime.UtcNow.ToLocalTime().ToUniversalTime() ).ToString()

// Mere conversion works correctly: calling ToLocalTime() on already 
// local time returns the same value:
( DateTime.Now, DateTime.Now.ToLocalTime(), DateTime.Now.ToLocalTime().ToLocalTime() )

( DateTime.UtcNow, DateTime.UtcNow.ToUniversalTime(), DateTime.UtcNow.ToUniversalTime().ToUniversalTime() )

// Similar examples on DateTimeOffset:
var to23 = DateTimeOffset.Now;
(to23, to23.ToUniversalTime(), to23.ToUniversalTime().ToLocalTime()).ToString()

to24  = DateTimeOffset.Now;
( to24, to24.ToLocalTime(), to24.ToLocalTime().ToLocalTime() )


~~~

~~~csharp

The `DateTime` and `DateTimeOffset` **do not contain time zone information**. They cannot natively express physical times as local times in arbitrary time zone. Via the **`DateTime.Kind` property**, it can only be indicated **whether** the time stored in a `DateTime` value is **expressed as UTC time or a local time** (i.e., according to the time zone of the current computer), or this is unspecified. However, the  **`DateTime.Kind`** property is **ignored when comparing** `DateTimeKind` values **or performing date and time arithmetic on them**. These operations are performed only on nominal values of time stored in `DateTime` instances, which means that `DateTime.Kind` just provides informative information on the nature of how its value was produced, but is not intended to relate the value to an unambiguous point in time (or physical time). This is perhaps a single most important information to be aware of in order to understand date and time-related operations in .NET.

Let us elaborate more on the statements above, as they are truly important to remember and understand when designing .NET software that handles dates and times. First, .NET contains some methods that **convert between local and UTC representations of `DateTime` and `DateTimeOffset` values**. These methods **work consistently** and as expected, in contrast with comparison and arithmetic operations.  

~~~csharp
// Proof that DateTime with its Kind property does not represent points in time,
// but rather data container for nominal times, without actual time zone / UTC
// context:
(DateTime.Now - DateTime.UtcNow).ToString()

var t1 = DateTime.Now; var t2 = DateTime.UtcNow; 
var diff1 = t1 - t2; var diff1Seconds = diff1.TotalSeconds; 
( t1, t1.Kind, t2, t2.Kind, diff1, Math.Abs(diff1Seconds) > 0.5 )

var t6 = DateTime.Now; var t7 = t6.ToUniversalTime();
var diff6 = t6 - t7; var diff6Seconds = diff6.TotalSeconds; 
( t6, t6.Kind, t7, t7.Kind, diff6, Math.Abs(diff6Seconds) > 0.5 )


// Demonstration of the Kind property:
(DateTime.Now.Kind, DateTime.UtcNow.Kind)

// Demonstration of round-trip conversion:
(DateTime.Now, DateTime.Now.ToUniversalTime(), DateTime.Now.ToUniversalTime().ToLocalTime()).ToString()

( DateTime.UtcNow, DateTime.UtcNow.ToLocalTime(), DateTime.UtcNow.ToLocalTime().ToUniversalTime() ).ToString()

// Mere conversion works correctly: calling ToLocalTime() on already 
// local time returns the same value:
( DateTime.Now, DateTime.Now.ToLocalTime(), DateTime.Now.ToLocalTime().ToLocalTime() )

( DateTime.UtcNow, DateTime.UtcNow.ToUniversalTime(), DateTime.UtcNow.ToUniversalTime().ToUniversalTime() )

// Similar examples on DateTimeOffset:
var to23 = DateTimeOffset.Now;
(to23, to23.ToUniversalTime(), to23.ToUniversalTime().ToLocalTime()).ToString()

to24  = DateTimeOffset.Now;
( to24, to24.ToLocalTime(), to24.ToLocalTime().ToLocalTime() )


// Operations with mixed types - DateTime and DateTimeOffset:

var t28 = DateTime.Now; var str28 = t28.ToString(); var to28 = DateTimeOffset.Parse(str28);
(t28 == to28, t28, to28).ToString()

DateTime.Now == DateTimeOffset.Now

DateTime.Now - DateTimeOffset.Now

(DateTime.Now, DateTimeOffset.Now, new DateTimeOffset(DateTime.Now))

(var t = DateTime.Now, var b = DateTimeOffset.Now)

~~~

~~~csharp
// Some examples for DateTimeOffset:

// These expressions all return 0 time span or ticks when evaluated as UTC + 01:
// Result is TimeSpan:
(new DateTimeOffset(2026, 2, 15, 0, 0, 0, TimeSpan.FromHours(1)) - new DateTime(2026, 2, 15, 0, 0, 0))

// Result is in ticks:
(new DateTimeOffset(2026, 2, 15, 0, 0, 0, TimeSpan.FromHours(1)) - new DateTime(2026, 2, 15, 0, 0, 0))

new DateTimeOffset(2026, 2, 15, 0, 0, 0, TimeSpan.FromHours(1)).Ticks - new DateTime(2026, 2, 15, 0, 0, 0).Ticks

new DateTimeOffset(2026, 2, 15, 0, 0, 0, TimeSpan.FromHours(1)).Ticks - new DateTime(2026, 2, 15, 0, 0, 0, DateTimeKind.Utc).Ticks

~~~


#### The DateOnly and TimeOnly Structs


### Parsing and Formatting Date and Time Strings


The [`DateTime.GetDateTimeFormats` method](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.getdatetimeformats) returns an array of strings containing different representations of the time contained in the `DateTime` instance, supported by the standard date and time format specifiers. Below is an example of getting string representations of the current time, and converting them back to `DateTime` using the [`DateTime.TryParse` method](https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tryparse):

~~~csharp
IFormatProvider formatProvider = CultureInfo.InvariantCulture;
DateTime currentTime = DateTime.Now;
// Get current time representations in different formats:
string[] timeStrings = currentTime.GetDateTimeFormats(formatProvider);
Console.WriteLine($"Retrieved time: {currentTime}");
Console.WriteLine($"There are {timeStrings?.Length} representations of the time stored in the {currentTime.GetType()?.Name} instance:");
foreach (string timeString in timeStrings)
{
  DateTime time;
  bool successful = DateTime.TryParse(timeString, out time);
  if (successful)
  {
    Console.WriteLine($"//   {time.ToString()} parsed from: \"{timeString}\"");
  } else
  {
    Console.WriteLine($"Could not Parse: \"{timeString}\"");
  }
}

// Retrieved time: 2/16/2026 1:27:53 PM
// There are 34 representations of the time stored in the DateTime instance:
//   2/16/2026 12:00:00 AM parsed from: "02/16/2026"
//   2/16/2026 12:00:00 AM parsed from: "2026-02-16"
//   2/16/2026 12:00:00 AM parsed from: "Monday, 16 February 2026"
//   2/16/2026 1:27:00 PM parsed from: "Monday, 16 February 2026 13:27"
//   2/16/2026 1:27:00 PM parsed from: "Monday, 16 February 2026 01:27 PM"
//   2/16/2026 1:27:00 PM parsed from: "Monday, 16 February 2026 13:27"
//   2/16/2026 1:27:00 PM parsed from: "Monday, 16 February 2026 1:27 PM"
//   2/16/2026 1:27:53 PM parsed from: "Monday, 16 February 2026 13:27:53"
//   2/16/2026 1:27:00 PM parsed from: "02/16/2026 13:27"
//   2/16/2026 1:27:00 PM parsed from: "02/16/2026 01:27 PM"
//   2/16/2026 1:27:00 PM parsed from: "02/16/2026 13:27"
//   2/16/2026 1:27:00 PM parsed from: "02/16/2026 1:27 PM"
//   2/16/2026 1:27:00 PM parsed from: "2026-02-16 13:27"
//   2/16/2026 1:27:00 PM parsed from: "2026-02-16 01:27 PM"
//   2/16/2026 1:27:00 PM parsed from: "2026-02-16 13:27"
//   2/16/2026 1:27:00 PM parsed from: "2026-02-16 1:27 PM"
//   2/16/2026 1:27:53 PM parsed from: "02/16/2026 13:27:53"
//   2/16/2026 1:27:53 PM parsed from: "2026-02-16 13:27:53"
//   2/16/2026 12:00:00 AM parsed from: "February 16"
//   2/16/2026 12:00:00 AM parsed from: "February 16"
//   2/16/2026 1:27:53 PM parsed from: "2026-02-16T13:27:53.9079396+01:00"
//   2/16/2026 1:27:53 PM parsed from: "2026-02-16T13:27:53.9079396+01:00"
//   2/16/2026 2:27:53 PM parsed from: "Mon, 16 Feb 2026 13:27:53 GMT"
//   2/16/2026 2:27:53 PM parsed from: "Mon, 16 Feb 2026 13:27:53 GMT"
//   2/16/2026 1:27:53 PM parsed from: "2026-02-16T13:27:53"
//   2/16/2026 1:27:00 PM parsed from: "13:27"
//   2/16/2026 1:27:00 PM parsed from: "01:27 PM"
//   2/16/2026 1:27:00 PM parsed from: "13:27"
//   2/16/2026 1:27:00 PM parsed from: "1:27 PM"
//   2/16/2026 1:27:53 PM parsed from: "13:27:53"
//   2/16/2026 2:27:53 PM parsed from: "2026-02-16 13:27:53Z"
//   2/16/2026 12:27:53 PM parsed from: "Monday, 16 February 2026 12:27:53"
//   2/1/2026 12:00:00 AM parsed from: "2026 February"
//   2/1/2026 12:00:00 AM parsed from: "2026 February"
~~~




## Time Zones and TimeZoneInfo class

See also:

* [Time zones](https://learn.microsoft.com/en-us/dotnet/standard/datetime/time-zone-overview)
  * [Use time zones in date and time arithmetic](https://learn.microsoft.com/en-us/dotnet/standard/datetime/use-time-zones-in-arithmetic)
  * [Converting between DateTime and DateTimeOffset](https://learn.microsoft.com/en-us/dotnet/standard/datetime/converting-between-datetime-and-offset)

To get the local time zone that is set on the current computer:

~~~csharp
TimeZoneInfo localTimezone = TimeZoneInfo.Local;
Console.WriteLine($"Local time zone: {localTimezone.Id}: {localTimezone.DisplayName}");

// Example output:
// Local time zone: Central Europe Standard Time: (UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague
~~~


Example: get and print **all time zones defined on computer**:

~~~cs
using System.Collections.ObjectModel;
ReadOnlyCollection<TimeZoneInfo> allTimeZones;
allTimeZones = TimeZoneInfo.GetSystemTimeZones();
foreach (TimeZoneInfo timeZone in allTimeZones)
    Console.WriteLine($"   {timeZone.Id}: {timeZone.DisplayName}");
~~~

Example partial output:

~~~txt
...
   Azores Standard Time: (UTC-01:00) Azores
   Cape Verde Standard Time: (UTC-01:00) Cabo Verde Is.
   UTC: (UTC) Coordinated Universal Time
   GMT Standard Time: (UTC+00:00) Dublin, Edinburgh, Lisbon, London
   Greenwich Standard Time: (UTC+00:00) Monrovia, Reykjavik
   Sao Tome Standard Time: (UTC+00:00) Sao Tome
   Morocco Standard Time: (UTC+01:00) Casablanca
   W. Europe Standard Time: (UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna
   Central Europe Standard Time: (UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague
   Romance Standard Time: (UTC+01:00) Brussels, Copenhagen, Madrid, Paris
   Central European Standard Time: (UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb
   W. Central Africa Standard Time: (UTC+01:00) West Central Africa
   GTB Standard Time: (UTC+02:00) Athens, Bucharest
   Middle East Standard Time: (UTC+02:00) Beirut
   Egypt Standard Time: (UTC+02:00) Cairo
   E. Europe Standard Time: (UTC+02:00) Chisinau
   West Bank Standard Time: (UTC+02:00) Gaza, Hebron
   South Africa Standard Time: (UTC+02:00) Harare, Pretoria
   FLE Standard Time: (UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius
   Israel Standard Time: (UTC+02:00) Jerusalem
...
~~~

To obtain all time zones with the specified UTC offset:

~~~csharp
public static List<TimeZoneInfo> GetTimeZoneFromOffset(TimeSpan offset) =>
    TimeZoneInfo.GetSystemTimeZones()
    .Where(tz => tz.BaseUtcOffset == offset)
    .ToList();

DateTimeOffset currentTimeOffset = DateTimeOffset.Now;
List<TimeZoneInfo> timeZones = GetTimeZoneFromOffset(currentTimeOffset.Offset);
foreach (TimeZoneInfo timeZone in timeZones)
{
    Console.WriteLine($"Time Zone: {timeZone}");
}
~~~

When run on a computer whose time zone has UTC offset +01:00, the result is something like this:

~~~txt
Time Zone: (UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna
Time Zone: (UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague
Time Zone: (UTC+01:00) Brussels, Copenhagen, Madrid, Paris
Time Zone: (UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb
Time Zone: (UTC+01:00) West Central Africa
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

### Coordinated Universal Time (UTC)

See also:

* [Coordinated Universal Time](https://en.wikipedia.org/wiki/Coordinated_Universal_Time) (Wikipedia)
  * [Time zones](https://en.wikipedia.org/wiki/Time_zone)
    * [Daylight saving time](https://en.wikipedia.org/wiki/Daylight_saving_time)
* [Universal Time (UT1)](https://en.wikipedia.org/wiki/Universal_Time) - a time standard based on Earth's rotation
* [International Atomic Time (TAI)](https://en.wikipedia.org/wiki/International_Atomic_Time) - high-precision atomic coordinate time standard based on the notional passage of proper time on Earth's geoid, weighted average of many atomic clocks; UTC is based on TAI, via leap seconds
  * [Leap second](https://en.wikipedia.org/wiki/Leap_second) - a one-second adjustment occasionally applied to (UTC), to accommodate the difference between more precise time (TAI) and imprecise observed solar time (UT1), which varies due to irregularities and long-term slowdown in the Earth's rotation; since 1972, 27 leap seconds have been added to UTC, recently on December 31, 2016. See also:
  * [Coordinate time](https://en.wikipedia.org/wiki/Einstein_synchronisation) (Wikipedia)
    * [Einstein synchronization](https://en.wikipedia.org/wiki/Einstein_synchronisation)
    * [Relativity of simultaneity](https://en.wikipedia.org/wiki/Relativity_of_simultaneity)
  * [Proper time](https://en.wikipedia.org/wiki/Proper_time) (Wikipedia)

#### Limitations of UTC and Computers' Time Capturing Capabilities


One limitation of UTC is the introduction of leap seconds. These seconds are necessary to synchronize the UTC, which is based on precise physical time, with the rotation of the Earth and, consequently, the length of the day. The speed of Earth's rotation slows over time due to interactions with the Moon and Sun, as well as processes that cause mass redistribution within Earth. Leap seconds are occasionally introduced to keep UTC in sync with the varying length of the astronomical day.  This practice began in 1972, and recent dates with a leap second include December 31, 2008; June 30, 2012; June 30, 2015; and December 21, 2016. As of 2026, no additional leap seconds have been introduced. The variations in the speed of Earth's rotation are significantly non-uniform, as reflected by the different spacing between leap seconds. Starting around 2020, the Earth began an unexpected speed-up phase, prompting discussions about a "negative leap second" (omitting a second). When a positive leap second occurs, UTC repeats or stretches time, and the clock sequence is 23:59:59 → 23:59:60 → 00:00:00. However, most computer systems (those using Unix time) cannot represent the :60 part and handle a leap second by "stepping" the clock back one second. This means that the same timestamps appear twice within the one-second interval before midnight, which causes a time ordering problem.

### Gregorian Calendar

[Gregorian Calendar](https://en.wikipedia.org/wiki/Gregorian_calendar) has *average calendar year `365.2425` days long*, compared to `365.2422`-day 'tropical' or *[solar year](https://en.wikipedia.org/wiki/Tropical_year)* determined by the Earth's revolution around the Sun. It replaced [Julian calendar](https://en.wikipedia.org/wiki/Julian_calendar) with average year of 365.25 days (leap year every 4 years), starting October 15, 1582 in some Catholic countries by papal decree.

Rules: every year *divisible by four is a leap year*, *except for years that are divisible by 100*, *except in turn for years also divisible by 400*.

See also:

* [Gregorian Calendar](https://en.wikipedia.org/wiki/Gregorian_calendar) (Wikipedia)
* [Calendar](https://en.wikipedia.org/wiki/Calendar)
  * [Calendar Epoch](https://en.wikipedia.org/wiki/Epoch) - an origin of calendar era (there may be more than one)
  * [Calendar era](https://en.wikipedia.org/wiki/Calendar_era) - a period of time elapsed since one epoch of a calendar





---

## Auxiliary

---

~~~csharp

~~~

---
