
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
            base.CommandlineToArgs_RoundTripConversionWorksCorrectly(isRoundTrip, commandLine,
                expectedArgs, shouldThrow);
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
            base.CommandlineToArgs_RoundTripConversionWitSecondOverloadWorksCorrectly(isRoundTrip, commandLine,
                expectedArgs, shouldThrow);
        }



            #endregion CommandLineToArguments


        }


}

