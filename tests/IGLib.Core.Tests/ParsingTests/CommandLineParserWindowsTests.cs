
// #nullable disable

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using IGLib.Tests.Base;
using IGLib.Commands.Tests;
using System.Text;
using System.Linq;

using IGLib.Parsing;

namespace IGLib.Commands.Tests
{

    /// <summary>Tests for generic commands (implementations of <see cref="IGenericCommand"/>).</summary>
    public class CommandLineParserWindowsTests : CommandLineParserTests_Base<CommandLineParserWindowsTests>
    {


        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> and <see cref="TestBase{TestClassType}.Console"/> properties.</summary>
        /// <param name=""></param>
        public CommandLineParserWindowsTests(ITestOutputHelper output) :  base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output and Console property.
        }


        protected override ICommandLineParser CommandLineParser { get; } = new CommandLineParserWindows();


        #region CommandLineToArguments

        /// <summary>Tests conversion of command-line to a set of arguments by the current type <see cref="ICommandLineParser"/>,
        /// which is defined for the current test class by the overridden <see cref="CommandLineParser"/> property. The round-trip
        /// conversion is checked when applicable.</summary>
        /// <param name="isRoundTrip">Whether round-trip conversion sgould be checked for the current input. When
        /// true, the parsed arguments are converted back to command-line string by the current converter, then
        /// converted to a set of arguments again, after which it is checked that the same arguments are obtained
        /// as before. Note that conversion back to command-line may not be identical in many cases because variable
        /// number of whitespace characters between arguments, or different quoting can produce the same results.</param>
        /// <param name="commandLine">The command line from which arguments are parsed.</param>
        /// <param name="expectedArgs">The expected arguments that should be obtained by parsing.</param>
        /// <param name="shouldThrow">Whether parsing should throw an exception.</param>
        [Theory]
        // Empty string or null:
        [InlineData(true, "", new string[] { })]
        [InlineData(true, null, new string[] { }, true)]  // should throw, as indicated by the last parameter
        // Basic Examples:
        [InlineData(true, "arg1", new string[] { "arg1" })]
        [InlineData(true, "arg1 arg2", new string[] { "arg1", "arg2" })]
        // Command-line arguments in double quotes:
        [InlineData(true, "arg1 \"argument 1\"", new string[] { "arg1", "argument 1" })]
        protected override void CommandlineToArgs_RoundTripConversionWorksCorrectly(bool isRoundTrip, string? commandLine,
            string[] expectedArgs, bool shouldThrow = false)
        {
            Func<ICommandLineParser, string?, string[]> commandLineToArgsFunc
                = (parser, commandLine) => { return parser.CommandLineToArgs(commandLine!); };
            CommandlineToArgs_Conversion_TestBase(isRoundTrip, commandLine,
                expectedArgs, shouldThrow, commandLineToArgsFunc);
        }


        /// <summary>Tests conversion of command-line to a set of arguments by the current type <see cref="ICommandLineParser"/>,
        /// which is defined for the current test class by the overridden <see cref="CommandLineParser"/> property. The round-trip
        /// conversion is checked when applicable.</summary>
        /// <param name="isRoundTrip">Whether round-trip conversion sgould be checked for the current input. When
        /// true, the parsed arguments are converted back to command-line string by the current converter, then
        /// converted to a set of arguments again, after which it is checked that the same arguments are obtained
        /// as before. Note that conversion back to command-line may not be identical in many cases because variable
        /// number of whitespace characters between arguments, or different quoting can produce the same results.</param>
        /// <param name="commandLine">The command line from which arguments are parsed.</param>
        /// <param name="expectedArgs">The expected arguments that should be obtained by parsing.</param>
        /// <param name="shouldThrow">Whether parsing should throw an exception.</param>
        [Theory]
        // Empty string or null:
        [InlineData(true, "", new string[] { })]
        [InlineData(true, null, new string[] { }, true)]  // should throw, as indicated by the last parameter
        // Basic Examples:
        [InlineData(true, "arg1", new string[] { "arg1" })]
        [InlineData(true, "arg1 arg2", new string[] { "arg1", "arg2" })]
        // Command-line arguments in double quotes:
        [InlineData(true, "arg1 \"argument 1\"", new string[] { "arg1", "argument 1" })]
        protected override void CommandlineToArgs_RoundTripConversionWitSecondOverloadWorksCorrectly(bool isRoundTrip, string? commandLine,
            string[] expectedArgs, bool shouldThrow = false)
        {
            Func<ICommandLineParser, string?, string[]> commandLineToArgsFunc
                = (parser, commandLine) => {

                    // int CommandLineToArgs(ReadOnlySpan<char> commandLine, List<string> destination);
                    if (commandLine == null)
                    {
                        return Array.Empty<string>();
                    }
                    ReadOnlySpan<char> commandLineSpan = commandLine != null ? commandLine.AsSpan() : ReadOnlySpan<char>.Empty;
                    List<string> destination = new List<string>();
                    int numArgs = parser.CommandLineToArgs(commandLineSpan, destination);
                    string[] argsArray = destination.ToArray();
                    return argsArray;
                };
            CommandlineToArgs_Conversion_TestBase(isRoundTrip, commandLine,
                expectedArgs, shouldThrow, commandLineToArgsFunc);
        }



            #endregion CommandLineToArguments


        }


}

