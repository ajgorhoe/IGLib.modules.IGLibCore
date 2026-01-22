using System;
using System.Collections.Generic;
using System.Text;

namespace CommandLineParsing
{
    public interface ICommandLineParserPlain
    {
        string[] CommandLineToArgs(string commandLine);

        // Convenience wrapper: accepts any enumerable.
        string ArgsToCommandLine(IEnumerable<string> commandLineArguments);
    }

    public interface ICommandLineParser : ICommandLineParserPlain
    {
        int CommandLineToArgs(ReadOnlySpan<char> commandLine, List<string> destination);

        // Optimized “write into caller-provided buffer”.
        void ArgsToCommandLine(ReadOnlySpan<string> args, StringBuilder destination);

        // Thin wrappers (no extra allocations for arrays; indexed loop for lists)
        void ArgsToCommandLine(IReadOnlyList<string> args, StringBuilder destination);
        void ArgsToCommandLine(IEnumerable<string> args, StringBuilder destination);
    }

    internal static class CommandLineBuilder
    {
        internal static void AppendWindows(ReadOnlySpan<string> args, StringBuilder sb)
        {
            bool first = true;

            for (int i = 0; i < args.Length; i++)
            {
                if (!first) sb.Append(' ');
                first = false;

                string a = args[i] ?? string.Empty;

                if (!NeedsWindowsQuoting(a))
                {
                    sb.Append(a);
                    continue;
                }

                sb.Append('"');

                int backslashes = 0;
                foreach (char ch in a)
                {
                    if (ch == '\\')
                    {
                        backslashes++;
                        continue;
                    }

                    if (ch == '"')
                    {
                        sb.Append('\\', backslashes * 2 + 1);
                        sb.Append('"');
                        backslashes = 0;
                        continue;
                    }

                    if (backslashes > 0)
                    {
                        sb.Append('\\', backslashes);
                        backslashes = 0;
                    }

                    sb.Append(ch);
                }

                if (backslashes > 0)
                    sb.Append('\\', backslashes * 2);

                sb.Append('"');
            }
        }

        internal static void AppendPosix(ReadOnlySpan<string> args, StringBuilder sb)
        {
            bool first = true;

            for (int i = 0; i < args.Length; i++)
            {
                if (!first) sb.Append(' ');
                first = false;

                string a = args[i] ?? string.Empty;

                if (a.Length == 0)
                {
                    sb.Append("''");
                    continue;
                }

                if (IsPosixSafeBareword(a))
                {
                    sb.Append(a);
                    continue;
                }

                // Prefer single quotes when possible
                if (a.IndexOf('\'') < 0) // compatible with older TFMs
                {
                    sb.Append('\'').Append(a).Append('\'');
                    continue;
                }

                // Contains single quotes: use 'foo'"'"'bar'
                sb.Append('\'');
                foreach (char ch in a)
                {
                    if (ch == '\'')
                        sb.Append("'\"'\"'");
                    else
                        sb.Append(ch);
                }
                sb.Append('\'');
            }
        }

        private static bool NeedsWindowsQuoting(string arg)
        {
            if (arg.Length == 0) return true;
            foreach (char c in arg)
            {
                if (IsWindowsWhite(c) || c == '"')
                    return true;
            }
            return false;
        }

        private static bool IsWindowsWhite(char c)
            => c == ' ' || c == '\t' || c == '\r' || c == '\n';

        private static bool IsPosixSafeBareword(string s)
        {
            foreach (char c in s)
            {
                if (!(char.IsLetterOrDigit(c) || c is '_' or '-' or '.' or '/' or ':' or '@' or '+'))
                    return false;
            }
            return true;
        }
    }

    public sealed class WindowsCommandLineParser : ICommandLineParser
    {
        public string[] CommandLineToArgs(string commandLine)
        {
            if (commandLine is null) throw new ArgumentNullException(nameof(commandLine));
            var list = new List<string>();
            CommandLineToArgs(commandLine.AsSpan(), list);
            return list.ToArray();
        }

        public int CommandLineToArgs(ReadOnlySpan<char> commandLine, List<string> destination)
        {
            if (destination is null) throw new ArgumentNullException(nameof(destination));
            int before = destination.Count;
            ParseWindows(commandLine, destination);
            return destination.Count - before;
        }

        // --- Builders (thin wrappers) ---

        public string ArgsToCommandLine(IEnumerable<string> commandLineArguments)
        {
            if (commandLineArguments is null) throw new ArgumentNullException(nameof(commandLineArguments));
            var sb = new StringBuilder();
            ArgsToCommandLine(commandLineArguments, sb);
            return sb.ToString();
        }

        public void ArgsToCommandLine(ReadOnlySpan<string> args, StringBuilder destination)
        {
            if (destination is null) throw new ArgumentNullException(nameof(destination));
            CommandLineBuilder.AppendWindows(args, destination);
        }

        public void ArgsToCommandLine(IReadOnlyList<string> args, StringBuilder destination)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));
            if (destination is null) throw new ArgumentNullException(nameof(destination));

            // Fast path for arrays -> span (no allocation, no copying)
            if (args is string[] arr)
            {
                CommandLineBuilder.AppendWindows(arr.AsSpan(), destination);
                return;
            }

            // Generic indexed path without ToArray()
            // (could also special-case List<string> if you want)
            for (int i = 0; i < args.Count; i++)
            {
                if (i > 0) destination.Append(' ');
                AppendOneWindows(args[i], destination);
            }
        }

        public void ArgsToCommandLine(IEnumerable<string> args, StringBuilder destination)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));
            if (destination is null) throw new ArgumentNullException(nameof(destination));

            // Try to avoid copies when possible:
            if (args is string[] arr)
            {
                CommandLineBuilder.AppendWindows(arr.AsSpan(), destination);
                return;
            }
            if (args is IReadOnlyList<string> rol)
            {
                ArgsToCommandLine(rol, destination);
                return;
            }

            // Fallback: enumerate once
            bool first = true;
            foreach (var a in args)
            {
                if (!first) destination.Append(' ');
                first = false;
                AppendOneWindows(a, destination);
            }
        }

        private static void AppendOneWindows(string? arg, StringBuilder sb)
        {
            string a = arg ?? string.Empty;

            // Same logic as in shared builder, but per-arg
            if (!NeedsWindowsQuotingLocal(a))
            {
                sb.Append(a);
                return;
            }

            sb.Append('"');
            int backslashes = 0;
            foreach (char ch in a)
            {
                if (ch == '\\')
                {
                    backslashes++;
                    continue;
                }

                if (ch == '"')
                {
                    sb.Append('\\', backslashes * 2 + 1);
                    sb.Append('"');
                    backslashes = 0;
                    continue;
                }

                if (backslashes > 0)
                {
                    sb.Append('\\', backslashes);
                    backslashes = 0;
                }

                sb.Append(ch);
            }

            if (backslashes > 0)
                sb.Append('\\', backslashes * 2);

            sb.Append('"');

            static bool NeedsWindowsQuotingLocal(string arg2)
            {
                if (arg2.Length == 0) return true;
                foreach (char c in arg2)
                {
                    if (c == '"' || c == ' ' || c == '\t' || c == '\r' || c == '\n')
                        return true;
                }
                return false;
            }
        }

        // --- Windows parser (unchanged) ---
        private static void ParseWindows(ReadOnlySpan<char> s, List<string> args)
        {
            int i = 0;
            while (i < s.Length && IsWindowsWhite(s[i])) i++;

            while (i < s.Length)
            {
                var sb = new StringBuilder();
                bool inQuotes = false;

                while (i < s.Length)
                {
                    char c = s[i];

                    if (!inQuotes && IsWindowsWhite(c))
                        break;

                    if (c == '"')
                    {
                        inQuotes = !inQuotes;
                        i++;
                        continue;
                    }

                    if (c == '\\')
                    {
                        int slashStart = i;
                        while (i < s.Length && s[i] == '\\') i++;
                        int slashCount = i - slashStart;

                        if (i < s.Length && s[i] == '"')
                        {
                            int pairs = slashCount / 2;
                            sb.Append('\\', pairs);

                            if ((slashCount % 2) == 1)
                            {
                                sb.Append('"');
                                i++;
                            }
                            else
                            {
                                inQuotes = !inQuotes;
                                i++;
                            }
                        }
                        else
                        {
                            sb.Append('\\', slashCount);
                        }

                        continue;
                    }

                    sb.Append(c);
                    i++;
                }

                args.Add(sb.ToString());
                while (i < s.Length && IsWindowsWhite(s[i])) i++;
            }

            static bool IsWindowsWhite(char c)
                => c == ' ' || c == '\t' || c == '\r' || c == '\n';
        }
    }

    public sealed class PosixCommandLineParser : ICommandLineParser
    {
        public string[] CommandLineToArgs(string commandLine)
        {
            if (commandLine is null) throw new ArgumentNullException(nameof(commandLine));
            var list = new List<string>();
            CommandLineToArgs(commandLine.AsSpan(), list);
            return list.ToArray();
        }

        public int CommandLineToArgs(ReadOnlySpan<char> commandLine, List<string> destination)
        {
            if (destination is null) throw new ArgumentNullException(nameof(destination));
            int before = destination.Count;
            ParsePosix(commandLine, destination);
            return destination.Count - before;
        }

        // --- Builders (thin wrappers) ---

        public string ArgsToCommandLine(IEnumerable<string> commandLineArguments)
        {
            if (commandLineArguments is null) throw new ArgumentNullException(nameof(commandLineArguments));
            var sb = new StringBuilder();
            ArgsToCommandLine(commandLineArguments, sb);
            return sb.ToString();
        }

        public void ArgsToCommandLine(ReadOnlySpan<string> args, StringBuilder destination)
        {
            if (destination is null) throw new ArgumentNullException(nameof(destination));
            CommandLineBuilder.AppendPosix(args, destination);
        }

        public void ArgsToCommandLine(IReadOnlyList<string> args, StringBuilder destination)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));
            if (destination is null) throw new ArgumentNullException(nameof(destination));

            if (args is string[] arr)
            {
                CommandLineBuilder.AppendPosix(arr.AsSpan(), destination);
                return;
            }

            for (int i = 0; i < args.Count; i++)
            {
                if (i > 0) destination.Append(' ');
                AppendOnePosix(args[i], destination);
            }
        }

        public void ArgsToCommandLine(IEnumerable<string> args, StringBuilder destination)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));
            if (destination is null) throw new ArgumentNullException(nameof(destination));

            if (args is string[] arr)
            {
                CommandLineBuilder.AppendPosix(arr.AsSpan(), destination);
                return;
            }
            if (args is IReadOnlyList<string> rol)
            {
                ArgsToCommandLine(rol, destination);
                return;
            }

            bool first = true;
            foreach (var a in args)
            {
                if (!first) destination.Append(' ');
                first = false;
                AppendOnePosix(a, destination);
            }
        }

        private static void AppendOnePosix(string? arg, StringBuilder sb)
        {
            string a = arg ?? string.Empty;

            if (a.Length == 0)
            {
                sb.Append("''");
                return;
            }

            if (IsPosixSafeBarewordLocal(a))
            {
                sb.Append(a);
                return;
            }

            if (a.IndexOf('\'') < 0)
            {
                sb.Append('\'').Append(a).Append('\'');
                return;
            }

            sb.Append('\'');
            foreach (char ch in a)
            {
                if (ch == '\'')
                    sb.Append("'\"'\"'");
                else
                    sb.Append(ch);
            }
            sb.Append('\'');

            static bool IsPosixSafeBarewordLocal(string s)
            {
                foreach (char c in s)
                {
                    if (!(char.IsLetterOrDigit(c) || c is '_' or '-' or '.' or '/' or ':' or '@' or '+'))
                        return false;
                }
                return true;
            }
        }

        // --- POSIX parser (unchanged) ---
        private static void ParsePosix(ReadOnlySpan<char> s, List<string> args)
        {
            int i = 0;
            while (i < s.Length)
            {
                while (i < s.Length && char.IsWhiteSpace(s[i])) i++;
                if (i >= s.Length) break;

                var sb = new StringBuilder();
                bool inSingle = false;
                bool inDouble = false;

                while (i < s.Length)
                {
                    char c = s[i];

                    if (!inSingle && !inDouble && char.IsWhiteSpace(c))
                        break;

                    if (!inDouble && c == '\'')
                    {
                        inSingle = !inSingle;
                        i++;
                        continue;
                    }

                    if (!inSingle && c == '"')
                    {
                        inDouble = !inDouble;
                        i++;
                        continue;
                    }

                    if (!inSingle && c == '\\')
                    {
                        i++;
                        if (i >= s.Length)
                        {
                            sb.Append('\\');
                            break;
                        }

                        char next = s[i];

                        if (inDouble)
                        {
                            if (next == '"' || next == '\\')
                            {
                                sb.Append(next);
                                i++;
                                continue;
                            }

                            sb.Append('\\');
                            sb.Append(next);
                            i++;
                            continue;
                        }

                        sb.Append(next);
                        i++;
                        continue;
                    }

                    sb.Append(c);
                    i++;
                }

                args.Add(sb.ToString());
                while (i < s.Length && char.IsWhiteSpace(s[i])) i++;
            }
        }
    }
}
