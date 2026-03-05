using System;
using System.Text;
using Xunit.Abstractions;

namespace IGLib.ConsoleAbstractions.Testing;

/// <summary>An <see cref="IConsole"/> implementation that can be used in xUnit tests.
/// <para>This console can use line buffered output mode in order to ensure the correct behavior of the 
/// <see cref="Write(string?)"/> method. The output <see cref="Write(string?)"/> is buffered (accumulated) 
/// and is not flushed to the test's standard output until WriteLine() is called. The <see cref="IsLineBuffered"/>
/// property defines whether the output is line buffered. It can be set via optional constructor parameter, or
/// explicitly after construction.</para>
/// <para>Alternatively, output is not line buffered. In this case the <see cref="Write(string?)"/> method 
/// appends a character that indicates that the following line should be part of the current line. This creates
/// an awckward output, but the advantage is that the <see cref="Write(string?)"/> method writes to standard
/// output immediately, and output cannot be skipped because there was no <see cref="WriteLine(string?)"/>
/// called after <see cref="Write(string?)"/>.</para>
/// <para>If output is line buffered, one can use the <see cref="Flush"/> method or an empty <see cref="WriteLine(string?)"/>
/// at the end of a test or in finally block, in orderr to ensure that all outputts of <see cref="Write(string?)"/>
/// are visible in test's output.</para></summary>
public sealed class XUnitOutputConsole : IConsole
{

    public XUnitOutputConsole(ITestOutputHelper output, bool isLineBuffered = true)
    {
        _output = output;
        IsLineBuffered = isLineBuffered;
    }

    private readonly ITestOutputHelper _output;
    private readonly StringBuilder _buffer = new();
    private const string LineContinuationCharacter = "⏎"; // U+23CE, "Return Symbol"

    /// <summary>Whether the output is line buffered or not.</summary>
    public bool IsLineBuffered { get; set; } = true;
    
    public string? ReadLine()
        => throw new NotSupportedException("No input support.");

    public void Write(string? value)
    {
        if (value != null)
        {
            _buffer.Append(value);
            if (!IsLineBuffered)
            {
                // When output is not line buffered, we don't wait and we flush the output buffer immediately.
                // This causes the newline to be appended to the argument of Write. Therefore, we append a special
                // character to indicate that the next line is a continuation of the current line.
                WriteLine(LineContinuationCharacter); // Flush immediately if not line buffered
            } 
        }
    }

    public void WriteLine(string? value = null)
    {
        if (value is not null) _buffer.Append(value);
        _output.WriteLine(_buffer.ToString());
        _buffer.Clear();
    }

    // Optional: flush on dispose / final assertion
    public void Flush()
    {
        if (_buffer.Length > 0)
        {
            _output.WriteLine(_buffer.ToString());
            _buffer.Clear();
        }
    }
}