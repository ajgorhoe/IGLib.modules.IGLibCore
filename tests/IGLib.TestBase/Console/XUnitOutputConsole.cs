using System;
using System.Text;
using Xunit.Abstractions;

using IGLib.ConsoleAbstractions;

namespace IGLib.Tests.Base;

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
/// are visible in test's output.</para>
/// <para>The <see cref="IConsole.ReadLine"/> is not implemented and its calls will throw <see cref="NotSupportedException"/>.</para>
/// </summary>
/// <remarks>This implementation of the <see cref="IConsole"/> interface is used to bring the interface's API to
/// xUnit tests (except the <see cref="ReadLine"/> method, which throws a <see cref="NotSupportedException"/>).
/// <para>In order TO USE this implementation in xUnit tests, inherit the xUnit test class from <see cref="TestBase{TestClassType}"/>, and
/// make its construcor depend on <see cref="ITestOutputHelper"/> (to be injected by the test framework) and call the base class' constructor,
/// passing it the <see cref="ITestOutputHelper"/> parameter. the object of this type is accessible through the <see cref="TestBase{TestClassType}.Console"/>
/// property.</para></remarks>
public sealed class XUnitOutputConsole : IConsole
{

    /// <summary>Constructor.</summary>
    /// <param name="output">The <see cref="ITestOutputHelper"/> object that is used to write to the xUnit test's 
    /// standard output. This object is normally injected by the framework via test class' constructor. If the
    /// <see cref="TestBase{TestClassType}"/> is used as base class of the test class and its constructor is
    /// called by test class's constructor then the <see cref="XUnitOutputConsole"/> object is automatically
    /// created with the <see cref="ITestOutputHelper"/> object injected, and it is assigned to the 
    /// <see cref="TestBase{TestClassType}.Console"/> property.</param>
    /// <param name="isLineBuffered">Specifies whether the current object is line buffered or not. Default is true,
    /// and this can later be changed or queried via the <see cref="IsLineBuffered"/> property.</param>
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

    /// <summary>The <see cref="ReadLine"/> method cannot be implemented by using the <see cref="ITestOutputHelper"/> 
    /// injected in xUnit tests, therefore this method throws an exception if called.</summary>
    /// <exception cref="NotSupportedException">Thrown if the method is accidentially called, since the <see cref="ReadLine"/>
    /// method cannot be implemented by using the <see cref="ITestOutputHelper"/>  injected in xUnit tests.</exception>
    public string? ReadLine()
        => throw new NotSupportedException("Input is not supported in xUnit tests.");

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public void WriteLine(string? value = null)
    {
        if (value is not null)
        {
            _buffer.Append(value);
        }
        try
        {
            _output.WriteLine(_buffer.ToString());
        }
        catch
        {
            throw;
        }
        finally
        {
            _buffer.Clear();
        }
    }

    /// <summary>Manually flushes the output buffer. All eveentual pending text from the <see cref="Write(string?)"/> 
    /// calls is written to the xUnit test's output. This also adds an extra newline to the xUnit test's output (due 
    /// to limitations of the <see cref="ITestOutputHelper"/>) if the buffer is not empty.</summary>
    public void Flush()
    {
        if (_buffer.Length > 0)
        {
            _output.WriteLine(_buffer.ToString());
            _buffer.Clear();
        }
    }
}