

using System;
using System.IO;

namespace IGLib.ConsoleUtils
{

    /// <summary>Adapter for the <see cref="System.Console"/> class. Implements the <see cref="IFullConsole"/> 
    /// interface by forwarding all the operations to the usual <see cref="System.Console"/> static class.</summary>
    public sealed class SystemConsole : IFullConsole
    {

        // Global System console, named simply Global:

        protected static Lazy<SystemConsole> _global = new Lazy<SystemConsole>(() => new SystemConsole());

        /// <summary>Global lazily initialized instance of <see cref="GlobalConsole"/>.</summary>
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