

using System;
using System.IO;

namespace IGLib.ConsoleAbstractions
{

    /// <summary>Adapter for the <see cref="System.Console"/> class. Implements the <see cref="IFullConsole"/> 
    /// interface by forwarding all the operations to the usual <see cref="System.Console"/> static class.
    /// <para>This includes the static <see cref="GlobalConsole"/> property, to ensure that the <see cref="System.Console"/>
    /// is always available via this console abstraction.</para></summary>
    /// <remarks>The API of <see cref="IConsole"/>  and other console abstractions is rather limited. In order 
    /// to use additional overlodes as available in the <see cref="System.Console"/> class, include the extension 
    /// methods for console abstraction interfaces by addinng the corresponding namespace via:</para>
    /// <para>using IGLib.ConsoleAbstractions.Extensions;</para></remarks>
    public sealed class SystemConsole : IFullConsole, 
        IConsoleWithKeyInput
    {

        // Global System console, named simply GlobalConsole:

        private static Lazy<SystemConsole> _global = new Lazy<SystemConsole>(() => new SystemConsole());

        /// <summary>Global lazily initialized instance of <see cref="SystemConsole"/>.</summary>
        public static SystemConsole GlobalConsole => _global.Value;


        public string? ReadLine() => Console.ReadLine();

        public void Write(string? value) => Console.Write(value);
        public void WriteLine(string? value = null) => Console.WriteLine(value);

        public void Clear() => Console.Clear();
        public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);
        public int CursorLeft { get => Console.CursorLeft; set => Console.CursorLeft = value; }
        public int CursorTop { get => Console.CursorTop; set => Console.CursorTop = value; }

        public ConsoleKeyInfo ReadKey(bool intercept = false) => Console.ReadKey(intercept);
        public bool KeyAvailable => Console.KeyAvailable;

        public ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }
        public ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }
        public void ResetColor() => Console.ResetColor();
    }

}