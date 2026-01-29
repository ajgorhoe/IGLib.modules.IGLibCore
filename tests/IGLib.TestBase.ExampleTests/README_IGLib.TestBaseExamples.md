
# TestBaseExamples Project

This project contains some **example tests**. You can look up the project to see how to do certain things in test projects. In particular, *[ExampleTestClass.cs](./ExampleTestClass.cs)* shows how to create tests with parameters, either with cased where input data that can be specified with constant expressions, or cases where this is not possible.

## Using Loggers in Tests

We have used *`Divergic.Logging.Xunit`*, which is now **deprecated**. There is a **replacement and several alternative approaches**.

---

## What happened to Divergic.Logging.Xunit

According to the NuGet listing, **Divergic.Logging.Xunit has been deprecated and renamed**:

* The package is marked *deprecated* on NuGet.
* The **suggested alternative is `Neovolve.Logging.Xunit`**. ([NuGet][1])

So instead of:

~~~bash
dotnet add package Divergic.Logging.Xunit
~~~

you would install:

~~~bash
dotnet add package Neovolve.Logging.Xunit
~~~

This package is authored by the same maintainer (Rory Primrose) and continues the same purpose. ([NuGet][1])

---

### What these logging helpers do

Both `Divergic.Logging.Xunit` *and* `Neovolve.Logging.Xunit` provide:

* Extensions to get an `ILogger` that writes through xUnit’s `ITestOutputHelper`
* Ability to capture logs from your system-under-test
* Optional cache loggers for assertions

Typical use in a test:

~~~csharp
public class MyTests
{
    private readonly ITestOutputHelper _output;

    public MyTests(ITestOutputHelper output) =>
        _output = output;

    [Fact]
    public void TestLogging()
    {
        using var logger = _output.BuildLogger();
        // or _output.BuildLoggerFor<MyClass>()

        var sut = new MyClass(logger);
        sut.DoSomething();

        // Assert log entries or just output them
    }
}
~~~

Same pattern works with the new package (`Neovolve.Logging.Xunit`). ([NuGet][2])

---

### Alternative packages for xUnit logging support

If you don’t want to use `Neovolve.Logging.Xunit`, there are *other packages* that fulfill similar roles:

#### ➤ MicrosoftExtensions.Logging.Xunit

* Provides an `ILogger` implementation backed by `ITestOutputHelper`
* Works by forwarding logs to xUnit test output
* Works with `Microsoft.Extensions.Logging` ecosystem
* Supports modern .NET and classic test projects ([NuGet][3])

Example:

~~~csharp
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Xunit;

public class MyTests
{
    private readonly LoggerFactory _factory;
    public MyTests(ITestOutputHelper output)
        => _factory = new LoggerFactory().AddXunit(output);

    [Fact]
    public void Test1()
    {
        var logger = _factory.CreateLogger("Test1");
        logger.LogInformation("Hello world!");
    }
}
~~~

(Actual API may differ slightly — check the package docs.)

---

#### MartinCostello.Logging.XUnit (.v3 for xUnit v3)

* Another alternative designed specifically to *hook `ILogger` to xUnit output*
* Two versions exist:

  * `MartinCostello.Logging.XUnit` (for xUnit v2)
  * `MartinCostello.Logging.XUnit.v3` (for xUnit v3) ([libraries.io][4])

---

### 🧾 Using loggers in xUnit tests in general

Yes — it *is* possible and common to use loggers in xUnit tests. There are three typical patterns:

#### 1. **Capture logs to test output**

Useful for debugging when a test runs.

With packages above you tie `ILogger` to `ITestOutputHelper`.

#### 2. **Capture logs for assertions**

If you want to *assert* on log contents (e.g., “validated error logged”), you:

* Use a *cache logger*
* Or write a custom `ILogger` stub that records messages
* Or use a `ListLogger` with assertion helpers

Example (pattern rather than package):

~~~csharp
var logs = new List<(LogLevel level, string msg)>();
ILogger logger = LoggerFactory.Create(builder =>
    builder.AddProvider(new DelegateLoggerProvider((lvl, msg) => logs.Add((lvl, msg))));

var sut = new MyClass(logger);
// exercise sut
Assert.Contains(logs, l => l.msg.Contains("expected"));
~~~

This works without special packages.

#### 3. **Use `Microsoft.Extensions.Logging.Testing`**

The ASP.NET Core team provides some testing helpers for logging in tests (for example `LoggerFactory` extensions that create test stores).

---

### Summary

| Package                               | Status        | Notes                                                        |
| ------------------------------------- | ------------- | ------------------------------------------------------------ |
| **Divergic.Logging.Xunit**            | ❌ Deprecated  | Suggested replacement: `Neovolve.Logging.Xunit` ([NuGet][1]) |
| **Neovolve.Logging.Xunit**            | ✅ Replacement | Continuation of the same pattern                             |
| **MicrosoftExtensions.Logging.Xunit** | ✅ Alternative | Integrates MS logging with xUnit output ([NuGet][3])         |
| **MartinCostello.Logging.XUnit(.v3)** | ✅ Alternative | Another logging integration for xUnit ([libraries.io][4])    |

---

The deprecated Divergic package has a recommended replacement (`Neovolve.Logging.Xunit`). But you’re not limited to it — other community packages exist.

[1]: https://www.nuget.org/packages/Divergic.Logging.Xunit/2.1.0-beta0002?utm_source=chatgpt.com "NuGet Gallery | Divergic.Logging.Xunit 2.1.0-beta0002"
[2]: https://www.nuget.org/packages/Divergic.Logging.Xunit?utm_source=chatgpt.com "NuGet Gallery | Divergic.Logging.Xunit 4.3.1"
[3]: https://www.nuget.org/packages/MicrosoftExtensions.Logging.Xunit?utm_source=chatgpt.com "NuGet Gallery | MicrosoftExtensions.Logging.Xunit 1.2.1"
[4]: https://libraries.io/nuget/MartinCostello.Logging.XUnit.v3?utm_source=chatgpt.com "MartinCostello.Logging.XUnit.v3 0.6.0 on NuGet - Libraries.io - security & maintenance data for open source software"

