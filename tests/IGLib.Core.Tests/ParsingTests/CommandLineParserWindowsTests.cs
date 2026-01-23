
// #nullable disable

#undef CallBaseClassImplementations


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

        /// <inheritdoc />
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
#if CallBaseClassImplementations
            // REMARK: Tests are not detected by the test framework if we call the base implementation here.
            base.CommandlineToArgs_RoundTripConversionWorksCorrectly(isRoundTrip, commandLine,
                expectedArgs, shouldThrow);
#else
            //Func<ICommandLineParser, string?, string[]> commandLineToArgsFunc
            //    = (parser, commandLine) => { return parser.CommandLineToArgs(commandLine!); };
            CommandlineToArgs_Conversion_TestBase(isRoundTrip, commandLine,
                expectedArgs, shouldThrow, CommandLineToArgs);  // commandLineToArgsFunc);
#endif
        }


        /// <inheritdoc />
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
#if CallBaseClassImplementations
            // REMARK: Tests are not detected by the test framework if we call the base implementation here.
            base.CommandlineToArgs_RoundTripConversionWitSecondOverloadWorksCorrectly(isRoundTrip, commandLine,
                expectedArgs, shouldThrow);
#else
            //Func<ICommandLineParser, string?, string[]> commandLineToArgsFunc
            //    = (parser, commandLine) => {

            //        // int CommandLineToArgs(ReadOnlySpan<char> commandLine, List<string> destination);
            //        if (commandLine == null)
            //        {
            //            return Array.Empty<string>();
            //        }
            //        ReadOnlySpan<char> commandLineSpan = commandLine != null ? commandLine.AsSpan() : ReadOnlySpan<char>.Empty;
            //        List<string> destination = new List<string>();
            //        int numArgs = parser.CommandLineToArgs(commandLineSpan, destination);
            //        string[] argsArray = destination.ToArray();
            //        return argsArray;
            //    };
            CommandlineToArgs_Conversion_TestBase(isRoundTrip, commandLine,
                expectedArgs, shouldThrow, CommandLineToArgsOverride1);  // commandLineToArgsFunc);
#endif
        }


        #endregion CommandLineToArguments


    }


}

