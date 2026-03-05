using System;
using System.Text;
using Xunit.Abstractions;

namespace IGLib.ConsoleAbstractions.Testing;

public sealed class XUnitOutputConsole : ConsoleBase
{
    private readonly ITestOutputHelper _output;
    private readonly StringBuilder _buffer = new();

    public XUnitOutputConsole(ITestOutputHelper output) => _output = output;

    public override string? ReadLine()
        => throw new NotSupportedException("No input support.");

    public override void Write(string? value)
        => _buffer.Append(value);

    public override void WriteLine(string? value = null)
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