using System;
using System.Collections.Generic;
using System.Text;

namespace IGLib.ConsoleAbstractions.Testing
{
    public abstract record ConsoleEvent;

    public sealed record WriteEvent(string Text) : ConsoleEvent;

    public sealed record WriteLineEvent(string Text) : ConsoleEvent;

    public sealed record ReadLineEvent(string? Returned) : ConsoleEvent;

    public sealed record ReadKeyEvent(ConsoleKeyInfo Returned, bool Intercept) : ConsoleEvent;

    public sealed class FakeConsole : ConsoleBase, IConsoleKeyInput
    {
        private readonly Queue<string?> _lines = new();
        private readonly Queue<ConsoleKeyInfo> _keys = new();
        private readonly StringBuilder _currentLine = new();
        private readonly IConsole? _loggingConsole;

        public FakeConsole(IConsole? loggingConsole = null)
        {
            _loggingConsole = loggingConsole;
        }

        public List<ConsoleEvent> Events { get; } = new();

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