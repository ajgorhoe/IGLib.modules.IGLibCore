
# Auxiliary Notes - Handling Time

This file contains auxiliary notes on handling time, which might be used later.

**Conntents**:

* [Understanding Impact of Leap Seconds](#understanding-impact-of-leap-seconds)

## Understanding Impact of Leap Seconds







#### The `DateTimeOffset` Struct

In order to provide consistency of comparison and arithmetic operations on times instances, one can use the **[DateTimeOffset struct](https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset)** instead of `DateTime`.



`DateTimeOffset` does not eliminate every inconsistency**, but it solves the most dangerous ones. It shifts the problem from "mathematical errors" to "contextual ambiguity."

---

##### What is actually stored in `DateTimeOffset`?

A `DateTimeOffset` is a value type that stores two primary components:

1. **A UTC DateTime (64-bit):** Internally, it keeps the time normalized to UTC (as a tick count).
2. **An Offset (16-bit):** The difference between that UTC time and the local time at the moment the value was created (e.g., `+01:00`).

Unlike `DateTime`, which has a `Kind` (an enum of 0, 1, or 2), `DateTimeOffset` stores the **exact numerical offset**.

---

##### How it handles the Inconsistencies

Let’s revisit your modes of inconsistency using `DateTimeOffset` for a **UTC+01:00** zone.

###### 1. "False Equality" (Fixed)

Because `DateTimeOffset` compares the **UTC normalized time**, it correctly identifies when two different nominal strings represent the same moment.

~~~csharp
// 10:00 UTC
var utc = new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero);
// 11:00 UTC+1
var local = new DateTimeOffset(2026, 1, 1, 11, 0, 0, TimeSpan.FromHours(1));

bool isEqual = (utc == local); 
// Result: True. 
// Logic: Both are 10:00 UTC.

~~~

###### 2. "Fall Back" Overlap (Fixed)

During the "Fall Back" hour, you can now distinguish between the first 2:30 AM (Offset +2) and the second 2:30 AM (Offset +1).

~~~csharp
var first230 = new DateTimeOffset(2026, 10, 25, 2, 30, 0, TimeSpan.FromHours(2));
var second230 = new DateTimeOffset(2026, 10, 25, 2, 30, 0, TimeSpan.FromHours(1));

bool areSame = (first230 == second230); 
// Result: False. 
// They are physically 60 minutes apart.

~~~

###### 3. Elapsed Time Calculation (Fixed)

Subtraction now uses the underlying UTC ticks, so the result is physically accurate regardless of DST changes.

---

##### The Remaining Inconsistency: "The Static Offset Trap"

The one thing `DateTimeOffset` **does not** know is the **Rules of the Time Zone**. It only knows the **Offset** at the moment the object was created.

###### The "Future DST" Problem

If you take a `DateTimeOffset` in January (UTC+1) and add 6 months, the arithmetic is physically correct (it adds exactly  hours worth of ticks), but the **Offset remains +1**.

**Example:**

~~~csharp
// January in Vienna (UTC+1)
var winter = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.FromHours(1));

// Add 6 months
var summer = winter.AddMonths(6); 

// Result: July 1st, 12:00 PM +01:00
// INCONSISTENCY: In July, Vienna is actually +02:00 (CEST). 

~~~

In this case, the `summer` variable represents a physically valid moment in time, but it is **not the correct local time** for that date in that location. To fix this, you would need `TimeZoneInfo` to "re-project" the UTC time back into the local rules.

##### Summary Comparison

| Feature | `DateTime` | `DateTimeOffset` |
| --- | --- | --- |
| **Storage** | Ticks + 2-bit Kind | Ticks (UTC) + 16-bit Offset |
| **Point-in-time Equality** | No (Nominal only) | **Yes** (Absolute) |
| **Duration Math** | No (Ignores shifts) | **Yes** (UTC-based) |
| **DST Awareness** | No | **No** (Offset is static) |

Would you like to see how `TimeZoneInfo.ConvertTime` is used to bridge the gap between a `DateTimeOffset` and actual local wall-clock rules?

---

> Q: Since DateTimeOffset stores a UTC datetime and an 16 bit offset, are these obtained available directly as public properties? 

> A: Not exactly. While `DateTimeOffset` stores those two components internally, the public properties behave a bit differently to ensure the object remains easy to use for "wall-clock" time.
>
> Here is how the internal storage maps to the public API:

##### 1. The Internal Storage

Internally, `DateTimeOffset` uses a private 64-bit field for **Ticks** (which, unlike a regular `DateTime`, are always normalized to UTC) and a 16-bit field for the **Offset** (in minutes).

##### 2. The Public Properties

You cannot "directly" grab the internal private fields, but the public properties provide that data in a reconstructed format:

* **`.UtcDateTime`**: This returns a `DateTime` object with `Kind.Utc`. It is essentially the "normalized" 64-bit tick count.
* **`.Offset`**: This returns a `TimeSpan` representing the 16-bit offset.
* **`.DateTime`**: **Warning!** This returns the "wall-clock" time (UTC + Offset) as a `DateTime` with `Kind.Unspecified`.
* **`.LocalDateTime`**: This converts the UTC ticks to the **current system's** local time zone, regardless of what the original offset was.

---

##### The Property Layout

| Property | Value for `11:00 +01:00` | `DateTime.Kind` |
| --- | --- | --- |
| **`.UtcDateTime`** | `10:00:00` | `Utc` |
| **`.Offset`** | `01:00:00` | N/A (`TimeSpan`) |
| **`.DateTime`** | `11:00:00` | **`Unspecified`** |
| **`.LocalDateTime`** | *Depends on your PC* | `Local` |

##### Why the `.DateTime` property is "Unspecified"

This is a critical detail for your writing on inconsistencies. When you call `.DateTime` on a `DateTimeOffset`, the resulting `DateTime` loses its offset metadata.

If you then perform arithmetic on that resulting `DateTime`, you fall right back into the "nominal value" traps we discussed earlier. The `DateTimeOffset` protects the timeline, but as soon as you "extract" the `DateTime` component, you lose that protection.

> **Pro-Tip:** If you need to perform calculations that are physically accurate, **always** stay within the `DateTimeOffset` domain. Only drop down to `.DateTime` if you are passing data to a legacy API that doesn't support the offset type.

Would you like to see how the `==` operator is implemented under the hood in the .NET Source code to see how it uses these internal fields?
