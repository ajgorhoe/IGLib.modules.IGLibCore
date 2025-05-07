
# TestBase Repository

This repository contains base classes for C# XUnit tests. This provides some additional functionality to test classes, such as Output and Console properties, which can be used to output partial results on tests output.

Support for ILogger messages is yet to be added.

**Note**: This project **does not contain actual tests**. Tests for the `IGLib.Core` projects are provided in the `IGLib.Core.Tests` project. 

Some **dummy test samples** are provided to **demonstrate some uses of the** `XUnit` **framework**, especially different ways of providing test parameters, which can simplify the test structure.

## Using the TestBase Class

The 'TestBase' class can be used as base class for test classes and provides some **useful utilities like writing to test output**. It is a Generic class that takes the actual base class as generic parameter. Usage in Test classes can be as follows:

~~~csharp
public class SpeedTests : TestBase<SpeedTests>
{
    public SpeedTests(ITestOutputHelper output) :
        base(output)
    {
        // Here comes any other initialization code when necessary
        // ...
    }
~~~

In the above example, the test class `SpeedTests` **inherits from** `TestBase<SpeedTests>` (note using test class as generic parameter to `TestBase`). Besides, it is important to provide a **constructor** that takes an `ITestOutputHelper` object as parameter, which enables the `XUnit` framework to inject an object of type `ITestOutputHelper` used for generating test output, and the constructor **must call the base constructor with this parameter**, which initializes properties `Output` and `Console` that can be used to produce test output (see the subsections below).

### Using Output and Console Properties from the TestBase Class

The `Output` and `Console` properties both hold **references to the same object** of type `ITestOutputHelper`, defined as follows:

~~~csharp
public interface ITestOutputHelper
{
    void WriteLine(string message);
    void WriteLine(string format, params object[] args);
}
~~~

Within a **test class that inherits from `TestBase<TestClass>`** (see the containing section), use of the properties `Output` and `Console`, which both contain a reference to the same `ITestOutputHelper` object, is simple:

~~~csharp
public class MyTestClass : TestBase<MyTestClass>
{
    // 
    [Fact]
    protected void MyTestMethod()
    {
        double a = Math.PI;
        Output.WriteLine($"This is my test method. Value of a is {a}.");
        // Body of the test method, potentially with other outputs...
        // ...
    }
    // ...
}
~~~

Use the `Output` property where you would like to make distinction with the `System.Console` class, and use the `Console` property if you might need to copy code between test methods and methods of usual classes that produce console output.

Producing detailed output in test functions has many advantages, based on specific case. In more complex cases, output enables following precisely what happened in tests before a failed assertion or other failure (like unexpected exception), sometimes providing crucial information for quickly identifying the causes of test failures without even resorting to debugging. In many cases, tests also act as additional documentation with live examples of how to use the methods, and well designed outputs can be of great help for understanding what happens in the test code.