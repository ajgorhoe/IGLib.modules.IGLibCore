
using System;

namespace IGLib.ConsoleAbstractions;

public abstract class ConsoleKeyInputBase : IConsoleKeyInput
{
    public abstract ConsoleKeyInfo ReadKey(bool intercept = false);
    public abstract bool KeyAvailable { get; }
}

public abstract class ConsoleColorsBase : IConsoleColors
{
    public abstract ConsoleColor ForegroundColor { get; set; }
    public abstract ConsoleColor BackgroundColor { get; set; }
    public abstract void ResetColor();
}

public abstract class ConsoleScreenBase : IConsoleScreen
{
    public abstract void Clear();
    public abstract void SetCursorPosition(int left, int top);
    public abstract int CursorLeft { get; set; }
    public abstract int CursorTop { get; set; }
}

