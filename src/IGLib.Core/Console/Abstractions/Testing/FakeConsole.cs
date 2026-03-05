using System;
using System.Collections.Generic;
using System.Text;

namespace IGLib.ConsoleAbstractions.Testing;

public abstract record ConsoleEvent;
public sealed record WriteEvent(string Text) : ConsoleEvent;
public sealed record WriteLineEvent(string Text) : ConsoleEvent;
public sealed record ReadLineEvent(string? Returned) : ConsoleEvent;
public sealed record ReadKeyEvent(ConsoleKeyInfo Returned, bool Intercept) : ConsoleEvent;



public sealed class FakeConsole // : ConsoleBase, IConsoleKeyInput
{
    private readonly Queue<string?> _lines = new();
    private readonly Queue<ConsoleKeyInfo> _keys = new();

}

