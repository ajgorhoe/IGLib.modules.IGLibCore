
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

