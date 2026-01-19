using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum CommandLineStyle
{
    Windows, // MSVC/CommandLineToArgvW-like
    Posix    // sh/bash-like quoting subset (no expansions)
}

public interface ICommandLineParser
{
    string[] CommandLineToArgs(string commandLine);
    string ArgsToCommandLine(IEnumerable<string> commandLineArguments);
}

public sealed class CommandLineParser : ICommandLineParser
{
    public CommandLineStyle Style { get; }

    public CommandLineParser(CommandLineStyle style = CommandLineStyle.Windows)
        => Style = style;

    public string[] CommandLineToArgs(string commandLine)
    {
        if (commandLine is null) throw new ArgumentNullException(nameof(commandLine));
        var list = new List<string>();
        CommandLineToArgs(commandLine.AsSpan(), list);
        return list.ToArray();
    }

    // Allocation-friendly overload
    public int CommandLineToArgs(ReadOnlySpan<char> commandLine, List<string> destination)
    {
        if (destination is null) throw new ArgumentNullException(nameof(destination));
        int before = destination.Count;

        switch (Style)
        {
            case CommandLineStyle.Windows:
                ParseWindows(commandLine, destination);
                break;
            case CommandLineStyle.Posix:
                ParsePosix(commandLine, destination);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return destination.Count - before;
    }

    public string ArgsToCommandLine(IEnumerable<string> commandLineArguments)
    {
        if (commandLineArguments is null) throw new ArgumentNullException(nameof(commandLineArguments));

        switch (Style)
        {
            case CommandLineStyle.Windows:
                return BuildWindows(commandLineArguments);
            case CommandLineStyle.Posix:
                return BuildPosix(commandLineArguments);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    // ---------------------------
    // Windows parsing/building
    // ---------------------------
    private static void ParseWindows(ReadOnlySpan<char> s, List<string> args)
    {
        int i = 0;

        // Skip leading whitespace
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
                        // Backslashes before a quote:
                        // - even: output slashCount/2 backslashes, treat quote as delimiter toggle
                        // - odd:  output slashCount/2 backslashes, output literal quote
                        int pairs = slashCount / 2;
                        sb.Append('\\', pairs);

                        if ((slashCount % 2) == 1)
                        {
                            sb.Append('"'); // escaped quote
                            i++; // consume the quote
                        }
                        else
                        {
                            // quote toggles inQuotes
                            inQuotes = !inQuotes;
                            i++; // consume the quote
                        }
                    }
                    else
                    {
                        // Literal backslashes
                        sb.Append('\\', slashCount);
                    }

                    continue;
                }

                sb.Append(c);
                i++;
            }

            args.Add(sb.ToString());

            // Skip whitespace between args
            while (i < s.Length && IsWindowsWhite(s[i])) i++;
        }
    }

    private static string BuildWindows(IEnumerable<string> argv)
    {
        var sb = new StringBuilder();
        bool first = true;

        foreach (var arg in argv)
        {
            if (!first) sb.Append(' ');
            first = false;

            string a = arg ?? string.Empty;

            if (!NeedsWindowsQuoting(a))
            {
                sb.Append(a);
                continue;
            }

            sb.Append('"');

            // Windows escaping rule for building:
            // - Backslashes are literal unless before a quote or end-of-arg when quoted.
            // - Escape " as \" and double preceding backslashes accordingly.
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
                    // Escape all pending backslashes (double them), then escape quote
                    sb.Append('\\', backslashes * 2 + 1);
                    sb.Append('"');
                    backslashes = 0;
                    continue;
                }

                // Normal char: emit pending backslashes as-is
                if (backslashes > 0)
                {
                    sb.Append('\\', backslashes);
                    backslashes = 0;
                }
                sb.Append(ch);
            }

            // If quoted, trailing backslashes must be doubled before closing quote
            if (backslashes > 0)
                sb.Append('\\', backslashes * 2);

            sb.Append('"');
        }

        return sb.ToString();
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

    // ---------------------------
    // POSIX-like parsing/building
    // ---------------------------
    private static void ParsePosix(ReadOnlySpan<char> s, List<string> args)
    {
        int i = 0;
        while (i < s.Length)
        {
            // skip whitespace
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
                        // trailing backslash: treat as literal backslash
                        sb.Append('\\');
                        break;
                    }

                    char next = s[i];
                    // In double quotes, backslash typically escapes \, ", $, `, and newline in many shells.
                    // Since we avoid expansions, we at least honor \" and \\ and \newline simplistically.
                    if (inDouble)
                    {
                        if (next == '"' || next == '\\')
                        {
                            sb.Append(next);
                            i++;
                            continue;
                        }
                        // otherwise keep the backslash literally
                        sb.Append('\\');
                        sb.Append(next);
                        i++;
                        continue;
                    }

                    // Unquoted: backslash escapes any next char
                    sb.Append(next);
                    i++;
                    continue;
                }

                sb.Append(c);
                i++;
            }

            // If quotes are unterminated, we still return best-effort (no exception).
            args.Add(sb.ToString());

            while (i < s.Length && char.IsWhiteSpace(s[i])) i++;
        }
    }

    private static string BuildPosix(IEnumerable<string> argv)
    {
        var sb = new StringBuilder();
        bool first = true;

        foreach (var arg in argv)
        {
            if (!first) sb.Append(' ');
            first = false;

            string a = arg ?? string.Empty;

            if (a.Length == 0)
            {
                sb.Append("''");
                continue;
            }

            // If safe, emit as-is
            if (IsPosixSafeBareword(a))
            {
                sb.Append(a);
                continue;
            }

            // Prefer single quotes (simplest, most portable)
            if (!a.Contains('\''))
            {
                sb.Append('\'').Append(a).Append('\'');
                continue;
            }

            // If it contains single quotes, use the classic close/open trick: 'foo'"'"'bar'
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

        return sb.ToString();
    }

    private static bool IsPosixSafeBareword(string s)
    {
        // Conservative: only allow alnum and a few common safe symbols.
        // If you want broader, extend this set.
        foreach (char c in s)
        {
            if (!(char.IsLetterOrDigit(c) || c is '_' or '-' or '.' or '/' or ':' or '@' or '+'))
                return false;
        }
        return true;
    }
}
