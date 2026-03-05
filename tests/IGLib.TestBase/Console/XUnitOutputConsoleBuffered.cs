using System;
using System.Text;
using Xunit.Abstractions;

namespace IGLib.ConsoleAbstractions.Testing;

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