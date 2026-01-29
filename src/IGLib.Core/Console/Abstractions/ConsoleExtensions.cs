

using System;
using System.IO;

namespace IGLib.ConsoleUtils
{

    public static class ConsoleExtensions
    {
        public static void Write(this IConsole c, object? value)
            => c.Write(value?.ToString());

        public static void WriteLine(this IConsole c, object? value)
            => c.WriteLine(value?.ToString());

        public static void Write(this IConsole c, int value) => c.Write(value.ToString());
        public static void WriteLine(this IConsole c, int value) => c.WriteLine(value.ToString());

        public static void Write(this IConsole c, string format, params object?[] args)
            => c.Write(string.Format(format, args));

        public static void WriteLine(this IConsole c, string format, params object?[] args)
            => c.WriteLine(string.Format(format, args));

        // add the rest as you wish (bool, double, decimal, etc.)
    }

}