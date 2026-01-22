using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IGLib.Parsing
{

    /// <summary>Plain command-line parser contract that operates on <see cref="string"/> and returns a new array.
    /// Use this when you do not need allocation-friendly overloads.</summary>
    public interface ICommandLineParserPlain
    {

        /// <summary>Splits a command-line string into individual arguments.
        /// The returned array contains the arguments in order; it may include the command name if present
        /// in the provided <paramref name="commandLine"/>.</summary>
        /// <param name="commandLine">The raw command-line string to parse.</param>
        /// <returns>An array of parsed arguments.</returns>
        string[] CommandLineToArgs(string commandLine);

        /// <summary>Builds a command-line string from a sequence of arguments such that, when the returned 
        /// string is parsed with <see cref="CommandLineToArgs(string)"/>, it round-trips back to the same
        /// argument sequence (for this parser's rules).</summary>
        /// <param name="commandLineArguments">The arguments to encode into a command line.</param>
        /// <returns>A command-line string representation of the provided arguments.</returns>
        string ArgsToCommandLine(IReadOnlyList<string> commandLineArgumennts);
    
    }

}
