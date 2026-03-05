using System;
using System.Collections.Generic;
using System.Text;

namespace IGLib.ConsoleAbstractions.Testing
{
    /// <summary>
    /// Represents a single interaction event with a console abstraction.
    /// </summary>
    public abstract record ConsoleEvent;

    /// <summary>
    /// Event recorded when <see cref="IConsole.Write(string)"/> is called.
    /// </summary>
    /// <param name="Text">The text written (never <c>null</c>; <c>null</c> is normalized to empty string).</param>
    public sealed record WriteEvent(string Text) : ConsoleEvent;

    /// <summary>
    /// Event recorded when <see cref="IConsole.WriteLine(string)"/> is called.
    /// </summary>
    /// <param name="Text">The text written as the line content (never <c>null</c>; <c>null</c> is normalized to empty string).</param>
    public sealed record WriteLineEvent(string Text) : ConsoleEvent;

    /// <summary>
    /// Event recorded when <see cref="IConsole.ReadLine"/> is called.
    /// </summary>
    /// <param name="Returned">The returned line (may be <c>null</c> if scripted input is exhausted).</param>
    public sealed record ReadLineEvent(string? Returned) : ConsoleEvent;

    /// <summary>
    /// Event recorded when <see cref="IConsoleKeyInput.ReadKey(bool)"/> is called.
    /// </summary>
    /// <param name="Returned">The returned key info.</param>
    /// <param name="Intercept">The value of the <c>intercept</c> argument passed to <c>ReadKey</c>.</param>
    public sealed record ReadKeyEvent(ConsoleKeyInfo Returned, bool Intercept) : ConsoleEvent;

    /// <summary>
    /// A test double for console interaction.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="FakeConsole"/> supports:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Scripted line input via <see cref="EnqueueLine(string)"/> and <see cref="EnqueueLines(IEnumerable{string})"/>.</description></item>
    /// <item><description>Scripted key input via <see cref="EnqueueKey(ConsoleKeyInfo)"/> and <see cref="EnqueueKeys(IEnumerable{ConsoleKeyInfo})"/>.</description></item>
    /// <item><description>Recording all interactions to <see cref="Events"/> for deterministic assertions.</description></item>
    /// <item><description>Capturing produced output text in <see cref="OutputText"/>.</description></item>
    /// <item><description>Optional logging/teeing to another <see cref="IConsole"/> (e.g., an xUnit adapter) via the constructor.</description></item>
    /// </list>
    /// <para>
    /// Note: This type is intended for unit tests and is not designed for concurrent use without external synchronization.
    /// </para>
    /// </remarks>
    public sealed class FakeConsole : ConsoleBase, IConsoleKeyInput
    {

        private readonly Queue<string?> _lines = new();
        private readonly Queue<ConsoleKeyInfo> _keys = new();
        private readonly StringBuilder _currentLine = new();
        private readonly IConsole? _loggingConsole;

        /// <summary>
        /// Initializes a new instance of <see cref="FakeConsole"/>.
        /// </summary>
        /// <param name="loggingConsole">
        /// Optional console to which the fake will log its operations (useful for diagnosing failing tests).
        /// The logging output does not affect <see cref="Events"/> or <see cref="OutputText"/>.
        /// </param>
        public FakeConsole(IConsole? loggingConsole = null)
        {
            _loggingConsole = loggingConsole;
        }

        /// <summary>
        /// Gets the ordered list of recorded console interaction events.
        /// </summary>
        public List<ConsoleEvent> Events { get; } = new();

        /// <summary>
        /// Gets the accumulated output produced by <see cref="Write(string)"/> and <see cref="WriteLine(string)"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="Write(string)"/> appends to the current line buffer.
        /// <see cref="WriteLine(string)"/> flushes the current line buffer (plus optional argument) into
        /// this text and appends <see cref="Environment.NewLine"/>.
        /// </para>
        /// </remarks>
        public string OutputText { get; private set; } = string.Empty;

        public void EnqueueLine(string? line) => _lines.Enqueue(line);

        public void EnqueueLines(IEnumerable<string?> lines)
        {
            if (lines is null) throw new ArgumentNullException(nameof(lines));
            foreach (var line in lines) _lines.Enqueue(line);
        }

        public void EnqueueKey(ConsoleKeyInfo key) => _keys.Enqueue(key);

        public void EnqueueKeys(IEnumerable<ConsoleKeyInfo> keys)
        {
            if (keys is null) throw new ArgumentNullException(nameof(keys));
            foreach (var key in keys) _keys.Enqueue(key);
        }

        public void Reset()
        {
            Events.Clear();
            OutputText = string.Empty;
            _currentLine.Clear();
        }

        public override string? ReadLine()
        {
            var value = _lines.Count > 0 ? _lines.Dequeue() : null;

            Events.Add(new ReadLineEvent(value));
            Log($"[ReadLine] -> {Format(value)}");

            return value;
        }

        /// <inheritdoc />
        public override void Write(string? value)
        {
            var s = value ?? string.Empty;

            _currentLine.Append(s);
            Events.Add(new WriteEvent(s));
            Log($"[Write] {Format(s)}");
        }

        public override void WriteLine(string? value = null)
        {
            if (value is not null)
            {
                _currentLine.Append(value);
            }

            var line = _currentLine.ToString();

            OutputText += line + Environment.NewLine;
            Events.Add(new WriteLineEvent(line));
            Log($"[WriteLine] {Format(line)}");

            _currentLine.Clear();
        }

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            if (_keys.Count == 0)
                throw new InvalidOperationException("No scripted keys are available. Enqueue keys before calling ReadKey().");

            var key = _keys.Dequeue();

            Events.Add(new ReadKeyEvent(key, intercept));
            Log($"[ReadKey intercept={intercept}] -> Key={key.Key}, Char={FormatChar(key.KeyChar)}");

            return key;
        }

        public bool KeyAvailable => _keys.Count > 0;

        private void Log(string message)
        {
            if (_loggingConsole is null) return;
            _loggingConsole.WriteLine(message);
        }

        private static string Format(string? s) => s is null ? "<null>" : s;

        private static string FormatChar(char c)
            => c == '\0' ? "<NUL>" : c.ToString();
    }
}