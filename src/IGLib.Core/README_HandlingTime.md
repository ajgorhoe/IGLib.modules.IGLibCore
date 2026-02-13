
# Handling Times and Dates in .NET

This is a short guide with explanatory notes on handling dates and times in .NET,  including types, conversions, time zones, etc. This is relevant for topics such as [Parsing Date and Time Data](./README_ParsingUtilities.md#parsing-date-and-time-data).

**Contents**:

* [Links](#links)
* [Logic of Working with Time and Dates in .NET](#logic-of-working-with-time-and-dates-in-net)
  * [Introduction - Handling Time-related Data in Software](#introduction-to-handling-time-related-data--in-software)
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

## Logic of Working with Time and Dates in .NET

### Introduction to Handling Time-related Data  in Software

When working with time-related data, it is crucial to define the context beforehand and identify the scope of time handling according to the needs. In the most **basic scenario**, all times can be defined as **local times**, corresponding to the *[time zone](https://en.wikipedia.org/wiki/Time_zone) of the host device on which the software is operating. The advantage of this is that times are readily stored in memory, recorded, and displayed or printed as times displayed in local clocks and used in the specific local environment, provided that the time zone of the device is correctly set and time is synchronized.

However, using only local times has two major limitations. First, some time zones observe [daylight saving time (DST)](https://en.wikipedia.org/wiki/Daylight_saving_time). Once a year (usually in the spring), the clocks are moved forward. This creates a gap in the displayed times of closely spaced events just after the shift. At another time of year, the clocks are moved back, causing another strange effect: some times are duplicated. Within the time interval of the shift, times have an ambiguous meaning because they can refer to either summer or winter time. Therefore, local time is not monotonic, making it unsuitable for recording events where the sequence or exact intervals need to be preserved.

A second limitation arises when times need to be exchanged between different locations on Earth or when times of events occurring in different locations (across time zones) need to be recorded. Local times describing the same point in time vary across time zones by approximately one day. The maximum difference between local times on Earth at any given moment is actually 26 hours rather than 24, and there is a two-hour window every day when three different calendar days exist simultaneously. To correlate local times with physical time, we must state the wall-clock time and the time zone in which it is recorded, which complicates software maintenance.

To overcome the limitations of using local time, we use a standardized time, or coordinate time, which is consistent regardless of where on Earth an event occurs or where information about distributed events is gathered. Universal Coordinated Time (UTC) is the primary time standard by which the world regulates clocks and is used in computer software as a standardized time.

Using a consistent strategy to handle times in software can significantly reduce complexity and increase maintainability and interoperability with other software. For example, event times are recorded in UTC, as are times exchanged between distributed locations or stored persistently. UTC times are converted to or from local times when needed, typically in use cases involving user interaction. For instance, when a user inputs a time at which an alarm should go off or an action should be triggered by the software, or when a time of an event is displayed to a user in their local time for easier interpretation. In some cases, times may need to be converted between time zones or displayed for different time zones (e.g., when planning itineraries or coordinating remote meetings with participants across time zones). This can be achieved by keeping times in UTC and converting them to different time zones.

Special attention is needed when comparing or ordering times, or when performing arithmetic operations on times, such as calculating time intervals between events or calculating times by adding or subtracting specified time intervals from a known time. To obtain correct results, map the involved times to the same representation (often UTC) before performing operations. It simplifies matters to keep all recorded times in the appropriate standardized format and to convert to local times or specific time zones only when necessary.

Conversions to and from Universal Coordinated Time (UTC) are generally available in system and base programming language libraries. UTC is an adequate standard time for a wide range of software applications.

However, UTC may not be appropriate in scenarios where precise time interval calculations, the correct ordering of events in time, and a strictly monotonic measure of time are critical. Examples include high-frequency trading, scientific instrumentation, distributed systems logging, navigation, and aerospace.

One limitation of UTC is the introduction of leap seconds. These seconds are necessary to synchronize the UTC, which is based on precise physical time, with the rotation of the Earth and, consequently, the length of the day. The speed of Earth's rotation slows over time due to interactions with the Moon and Sun, as well as processes that cause mass redistribution within Earth. Leap seconds are occasionally introduced to keep UTC in sync with the varying length of the astronomical day.  This practice began in 1972, and recent dates with a leap second include December 31, 2008; June 30, 2012; June 30, 2015; and December 21, 2016. As of 2026, no additional leap seconds have been introduced. The variations in the speed of Earth's rotation are significantly non-uniform, as reflected by the different spacing between leap seconds. Starting around 2020, the Earth began an unexpected speed-up phase, prompting discussions about a "negative leap second" (omitting a second). When a positive leap second occurs, UTC repeats or stretches time, and the clock sequence is 23:59:59 → 23:59:60 → 00:00:00. However, most computer systems (those using Unix time) cannot represent the :60 part and handle a leap second by "stepping" the clock back one second. This means that the same timestamps appear twice within the one-second interval before midnight, which causes a time ordering problem.





For more information on the limitations of UTC and system time-related utilities, see the section [Limitations of UTC and System Time-Related Utilities](#limitations-of-the-utc-and-system-time-capturing-utilities).






## Time Zones and `TimeZoneInfo` class

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

#### Limitations of the UTC and System' Time Capturing Utilities



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
