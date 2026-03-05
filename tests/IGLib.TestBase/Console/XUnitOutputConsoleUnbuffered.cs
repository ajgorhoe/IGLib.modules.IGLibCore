using System;
using Xunit.Abstractions;
using IGLib.ConsoleAbstractions;

namespace IGLib.Tests;

/// <summary>An <see cref="IConsole"/> implementation to be used in xUnit tests.
/// <para>This console does not use line buffered output. Instead, the <see cref="Write(string?)"/> method 
/// appends a character that signifies that the following line should be part of the current line.</para></summary>
public sealed class XUnitOutputConsole : IConsole
{
    private readonly ITestOutputHelper _output;

    public XUnitOutputConsole(ITestOutputHelper output)
    {
        _output = output;
    }

    private const string LineContinuationCharacter = "⏎"; // U+23CE, "Return Symbol"

    public string? ReadLine()
        => throw new NotSupportedException("XUnitOutputConsole does not support input. Use a scripted/fake console to eemulate input.");

    public void Write(string? value)
    {
        // xUnit only provides WriteLine (with a newline appended); we emulate Write() by appending a special character
        // to indicate that transfer to the next lin is artficial and the next line should be considered a continuation
        // of the current line.
        if (value != null)
        {
          _output.WriteLine(value + LineContinuationCharacter);
        }
    }

    public void WriteLine(string? value = null)
        => _output.WriteLine(value ?? string.Empty);

}