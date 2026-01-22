using System;
using System.Collections.Generic;
using System.Text;

namespace IGLib.Parsing

{

    /// <summary>
    /// Command-line parser that follows a POSIX-shell-like quoting and escaping subset:
    /// whitespace separates arguments, single and double quotes group text, and backslash escapes
    /// the following character (with limited special handling inside double quotes).
    ///
    /// This parser intentionally does NOT implement shell expansions (variables, globbing, command
    /// substitution, etc.).
    /// </summary>
    public sealed class CommandLineParserPosix : ICommandLineParser
    {
        /// <inheritdoc />
        public string[] CommandLineToArgs(string commandLine)
        {
            if (commandLine is null) throw new ArgumentNullException(nameof(commandLine));
            var list = new List<string>();
            CommandLineToArgs(commandLine.AsSpan(), list);
            return list.ToArray();
        }

        /// <inheritdoc />
        public int CommandLineToArgs(ReadOnlySpan<char> commandLine, List<string> destination)
        {
            if (destination is null) throw new ArgumentNullException(nameof(destination));
            int before = destination.Count;
            ParsePosix(commandLine, destination);
            return destination.Count - before;
        }

        /// <inheritdoc />
        public string ArgsToCommandLine(IReadOnlyList<string> commandLineArguments)
        {
            if (commandLineArguments is null) throw new ArgumentNullException(nameof(commandLineArguments));
            return BuildPosix(commandLineArguments);
        }

        // ---------------------------
        // POSIX-like parsing/building
        // ---------------------------

        private static void ParsePosix(ReadOnlySpan<char> s, List<string> args)
        {
            int i = 0;
            while (i < s.Length)
            {
                // Skip whitespace
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
                            // Trailing backslash: treat as literal backslash
                            sb.Append('\\');
                            break;
                        }

                        char next = s[i];

                        // Inside double quotes, honor \" and \\ (conservative subset).
                        if (inDouble)
                        {
                            if (next == '"' || next == '\\')
                            {
                                sb.Append(next);
                                i++;
                                continue;
                            }

                            // Otherwise keep backslash literally
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

                // Unterminated quotes: best-effort behavior (no exception)
                args.Add(sb.ToString());

                // Skip whitespace between args
                while (i < s.Length && char.IsWhiteSpace(s[i])) i++;
            }
        }

        private static string BuildPosix(IReadOnlyList<string> argv, StringBuilder? sb = null)
        {
            if (sb==null)
                sb = new StringBuilder();
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

                // Prefer single quotes (simple and widely compatible)
                if (!a.Contains("'"))
                {
                    sb.Append('\'').Append(a).Append('\'');
                    continue;
                }

                // Contains single quotes: use the classic close/open trick: 'foo'"'"'bar'
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
            // Conservative: allow only alnum and a few common safe symbols.
            foreach (char c in s)
            {
                if (!(char.IsLetterOrDigit(c) || c is '_' or '-' or '.' or '/' or ':' or '@' or '+'))
                    return false;
            }
            return true;
        }
    }

}
