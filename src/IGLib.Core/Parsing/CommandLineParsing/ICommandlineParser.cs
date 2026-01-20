using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IGLib.Parsing
{

    /// <summary>
    /// Extended command-line parser contract that includes allocation-friendly overloads.
    /// This interface is useful for high-throughput scenarios where the command line may be a slice
    /// of a larger buffer and you want to reuse a destination list.
    /// </summary>
    public interface ICommandLineParser : ICommandLineParserPlain
    {

        /// <summary>
        /// Splits a command-line span into individual arguments and appends them to <paramref name="destination"/>.
        /// This overload is allocation-friendly: it does not allocate intermediate substrings and allows callers
        /// to reuse the destination list across calls.
        /// </summary>
        /// <param name="commandLine">The raw command-line characters to parse.</param>
        /// <param name="destination">List that receives parsed arguments (appended).</param>
        /// <returns>The number of arguments added to <paramref name="destination"/>.</returns>
        int CommandLineToArgs(ReadOnlySpan<char> commandLine, List<string> destination);

    }


}
