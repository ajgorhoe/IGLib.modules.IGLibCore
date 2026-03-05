using System;

namespace IGLib.ConsoleAbstractions;

/// <summary>A composed full console (aggregator class) composed of individual capability implementations.
/// Delegates methods of <see cref="IFullConsole"/> to individual parts.</summary>
public sealed class ComposedConsole : IFullConsole
{
    public ComposedConsole(
        IConsole console,
        IConsoleKeyInput keys,
        IConsoleColors colors,
        IConsoleScreen screen)
    {
        _console = console;
        _keys = keys;
        _colors = colors;
        _screen = screen;
    }

    private readonly IConsole _console;
    private readonly IConsoleKeyInput _keys;
    private readonly IConsoleColors _colors;
    private readonly IConsoleScreen _screen;

    public string? ReadLine() => _console.ReadLine();

    public void Write(string? value) => _console.Write(value);

    public void WriteLine(string? value = null) => _console.WriteLine(value);


    public ConsoleKeyInfo ReadKey(bool intercept = false) => _keys.ReadKey(intercept);

    public bool KeyAvailable => _keys.KeyAvailable;


    public ConsoleColor ForegroundColor { get => _colors.ForegroundColor; set => _colors.ForegroundColor = value; }

    public ConsoleColor BackgroundColor { get => _colors.BackgroundColor; set => _colors.BackgroundColor = value; }

    public void ResetColor() => _colors.ResetColor();


    public void Clear() => _screen.Clear();

    public void SetCursorPosition(int left, int top) => _screen.SetCursorPosition(left, top);
    public int CursorLeft { get => _screen.CursorLeft; set => _screen.CursorLeft = value; }

    public int CursorTop { get => _screen.CursorTop; set => _screen.CursorTop = value; }

}