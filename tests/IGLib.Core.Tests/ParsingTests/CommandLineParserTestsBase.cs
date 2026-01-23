
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
    public abstract class CommandLineParserTests_Base<TestClass> : TestBase<TestClass>
    {


        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> and <see cref="TestBase{TestClassType}.Console"/> properties.</summary>
        /// <param name=""></param>
        public CommandLineParserTests_Base(ITestOutputHelper output) :  base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output and Console property.
        }

        protected CommandLineParserPosix CreatePosixParser()
        {
            return new CommandLineParserPosix();
        }


        /// <summary>Commandline parser that is used in the specific class; should be
        /// overridden in derived classes.</summary>
        protected abstract ICommandLineParser CommandLineParser { get; } 

        /// <summary>Writes command-line arguments in a readable form via Console property to the test's standard output.</summary>
        /// <param name="args">Command-line arguments to be written.</param>
        /// <param name="newLinesAfter">The number of newlines added after writing the arguments.</param>
        /// <param name="offset">The starting index of the first argument in the collection.</param>
        protected virtual void WriteCommandLineArguments(string[] args, int newLinesAfter = 0, int offset = 1)
        {
            if (args == null)
            {
                Console.WriteLine("  <null>");
            } else if (args.Length == 0)
            {
                Console.WriteLine("  <empty>");
            }
            else
            {
                for (int i = 0; i < args.Length; ++i)
                {
                    Console.WriteLine($"  [{i + offset}]: {args[i]}");
                }
            }
        }




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
        protected virtual void CommandlineToArgs_RoundTripConversionWorksCorrectly(bool isRoundTrip, string? commandLine,
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
        protected virtual void CommandlineToArgs_RoundTripConversionWitSecondOverloadWorksCorrectly(bool isRoundTrip, string? commandLine,
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




        /// <summary>Does the work for test methods that test conversion of command-line string to parsd 
        /// arguments, and possibly back to command-line and again to arguments (round-trip).
        /// <para>Different overloads of the conversion method can be verified by this method, and parameter
        /// <paramref name="commandLineToArgsFunc"/> specifies which overload is used for conversion and how.</para></summary>
        /// <param name="isRoundTrip">Whether round-trip conversion should be performed. If false then only one
        /// conversion from command-line to a set or parsed arguments happens. If true, arguments are converted
        /// back to command-lie, which is converted to arguments again, and the final arguments are compared to
        /// arguments after the first conversion.</param>
        /// <param name="commandLine">Command-linee string to be parsed and convertted to a set of individual 
        /// arguments. This can include command or not (both command and its arguments are parsed in the same way).</param>
        /// <param name="expectedArgs">Expected arguments to be obtained aftter parding. These need to be provided 
        /// because correctness of conversion is assessed on the basis of them (if <paramref name="isRoundTrip"/>
        /// is true then, in addition, the correctness of round-trip through conversion to command-line and additional
        /// conversion to a set of arguments is checked).</param>
        /// <param name="shouldThrow">Whether conversion should throw an exception for the specific input data.</param>
        /// <param name="commandLineToArgsFunc">This delegate (usually passed ass lambda expression) performs the conversion
        /// via the parser stored in the <see cref="ICommandLineParser"/> stored in teh  <see cref="CommandLineParser"/>
        /// property for the test class that calls this method.</param>
        protected virtual void CommandlineToArgs_Conversion_TestBase(bool isRoundTrip, string? commandLine, 
            string[] expectedArgs, bool shouldThrow, Func<ICommandLineParser, string?, string[]> commandLineToArgsFunc)
        {
            // Arrange:
            ICommandLineParser parser = CommandLineParser;
            Console.WriteLine($"Testing conversion of command-line to arguments, parser type: {parser.GetType().Name}");
            if (commandLine == null)
            {
                Console.WriteLine("\nCommand-line string is null.");
            }
            else
            {
                Console.WriteLine($"\nCommand-line string in single quotes:\n  '{commandLine}'");
            }
            // Act:
            string[] parsedArgs;
            try
            {
                parsedArgs = commandLineToArgsFunc(parser, commandLine); // before: parser.CommandLineToArgs(commandLine!);
                Console.WriteLine("\nParsed arguments:");
                WriteCommandLineArguments(parsedArgs, 1);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"\nException {ex.GetType().Name} thrown with message:\n  {ex.Message}");
                if (shouldThrow)
                {
                    Console.WriteLine("  Exception was expected.\n");
                    return;
                }
                else
                {
                    Console.WriteLine("  Exception was NOT expected at this point, the test fails.\n");
                    throw;  // re-throw unexpected exception
                }
            }
            // Assert:
            if (expectedArgs == null)
            {
                parsedArgs.Should().BeNull(because: "the current command-line should result in arguments being null");
            } 
            else if (expectedArgs.Length == 0)
            {
                parsedArgs.Should().NotBeNull(because: "the current command-line should result in empty list of arguments, not null");
                parsedArgs.Length.Should().Be(0, because: "the current command-line should result in empty list of arguments");
            }
            else
            {
                parsedArgs.Should().NotBeNull(because: "the resulting array of parsed arguments should not be null");
                parsedArgs.Length.Should().Be(expectedArgs?.Length, because: 
                    $"the number of resulting arguments should ne {expectedArgs?.Length}, not {parsedArgs.Length}");
                for (int i = 0; i < expectedArgs!.Length; ++i)
                {
                    parsedArgs[i].Should().Be(expectedArgs[i], because:
                        $"the argument at index {i} should be '{expectedArgs[i]}', not '{parsedArgs[i]}'");
                }
            }
            if (!isRoundTrip)
            {
                Console.WriteLine("\nNo round-trip conversion requested, test ends here.\n");
            }
            try
            {
                // Arrange (round-trip):
                Console.WriteLine("\nRound-trip conversion requested, proceeding to convert back to command-line and re-parse:");
                // Act:
                // Convert back to command-line:
                string reconstructedCommandLine = parser.ArgsToCommandLine(parsedArgs);
                Console.WriteLine($"\nReconstructed command-line string in single quotes:\n  '{reconstructedCommandLine}'\n");
                // And parse it again:
                string[] reparsedArgs = commandLineToArgsFunc(parser, commandLine); // before: parser.CommandLineToArgs(reconstructedCommandLine);
                Console.WriteLine("\nRe-parsed arguments from reconstructed command-line:");
                WriteCommandLineArguments(reparsedArgs, 1);
                // Check that reparsed arguments match the original expected arguments:
                if (expectedArgs == null)
                {
                    reparsedArgs.Should().BeNull(because: "the re-parsed arguments should be null");
                }
                else if (expectedArgs.Length == 0)
                {
                    reparsedArgs.Should().NotBeNull(because: "the re-parsed arguments should not be null");
                    reparsedArgs.Length.Should().Be(0, because: "the re-parsed arguments should be empty");
                }
                else
                {
                    reparsedArgs.Should().NotBeNull(because: "the re-parsed arguments should not be null");
                    reparsedArgs.Length.Should().Be(expectedArgs?.Length, because:
                        $"the number of re-parsed arguments should ne {expectedArgs?.Length}, not {reparsedArgs.Length}");
                    for (int i = 0; i < expectedArgs!.Length; ++i)
                    {
                        reparsedArgs[i].Should().Be(expectedArgs[i], because:
                            $"the re-parsed argument at index {i} should be '{expectedArgs[i]}', not '{reparsedArgs[i]}'");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nException {ex.GetType().Name} thrown during round-trip conversion with message:\n  {ex.Message}");
                if (isRoundTrip)
                {
                    Console.WriteLine("  Round-trip conversion was required, test will fail due to exception.\n");
                    throw;  // re-throw expected exception
                }
                else
                {
                    Console.WriteLine("  Round-trip conversion was NOT required, test will not fail because of exception.\n");
                }
            }
        }



        #endregion CommandLineToArguments


















        protected virtual void ArgsToCommandLine_Conversion_TestBase(bool isRoundTrip, bool checkFirstConversion, 
            string[]? args, string[]? expectedArgs, bool shouldThrow, Func<ICommandLineParser, string[], string> builderMethod)
        {
            // Arrange:
            ICommandLineParser parser = CommandLineParser;
            Console.WriteLine($"Testing conversion of command-line arguments to command-line string & back; parser type: {parser.GetType().Name}");






            string? commandLine = null;
            if (commandLine == null)
            {
                Console.WriteLine("\nCommand-line string is null.");
            }
            else
            {
                Console.WriteLine($"\nCommand-line string in single quotes:\n  '{commandLine}'");
            }
            // Act:
            string[] parsedArgs;
            try
            {
                parsedArgs = parser.CommandLineToArgs(commandLine!);
                Console.WriteLine("\nParsed arguments:");
                WriteCommandLineArguments(parsedArgs, 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nException {ex.GetType().Name} thrown with message:\n  {ex.Message}");
                if (shouldThrow)
                {
                    Console.WriteLine("  Exception was expected.\n");
                    return;
                }
                else
                {
                    Console.WriteLine("  Exception was NOT expected at this point, the test fails.\n");
                    throw;  // re-throw unexpected exception
                }
            }
            // Assert:
            if (expectedArgs == null)
            {
                parsedArgs.Should().BeNull(because: "the current command-line should result in arguments being null");
            }
            else if (expectedArgs.Length == 0)
            {
                parsedArgs.Should().NotBeNull(because: "the current command-line should result in empty list of arguments, not null");
                parsedArgs.Length.Should().Be(0, because: "the current command-line should result in empty list of arguments");
            }
            else
            {
                parsedArgs.Should().NotBeNull(because: "the resulting array of parsed arguments should not be null");
                parsedArgs.Length.Should().Be(expectedArgs?.Length, because:
                    $"the number of resulting arguments should ne {expectedArgs?.Length}, not {parsedArgs.Length}");
                for (int i = 0; i < expectedArgs!.Length; ++i)
                {
                    parsedArgs[i].Should().Be(expectedArgs[i], because:
                        $"the argument at index {i} should be '{expectedArgs[i]}', not '{parsedArgs[i]}'");
                }
            }
            if (!isRoundTrip)
            {
                Console.WriteLine("\nNo round-trip conversion requested, test ends here.\n");
            }
            try
            {
                // Arrange (round-trip):
                Console.WriteLine("\nRound-trip conversion requested, proceeding to convert back to command-line and re-parse:");
                // Act:
                // Convert back to command-line:
                string reconstructedCommandLine = parser.ArgsToCommandLine(parsedArgs);
                Console.WriteLine($"\nReconstructed command-line string in single quotes:\n  '{reconstructedCommandLine}'\n");
                // And parse it again:
                string[] reparsedArgs = parser.CommandLineToArgs(reconstructedCommandLine);
                Console.WriteLine("\nRe-parsed arguments from reconstructed command-line:");
                WriteCommandLineArguments(reparsedArgs, 1);
                // Check that reparsed arguments match the original expected arguments:
                if (expectedArgs == null)
                {
                    reparsedArgs.Should().BeNull(because: "the re-parsed arguments should be null");
                }
                else if (expectedArgs.Length == 0)
                {
                    reparsedArgs.Should().NotBeNull(because: "the re-parsed arguments should not be null");
                    reparsedArgs.Length.Should().Be(0, because: "the re-parsed arguments should be empty");
                }
                else
                {
                    reparsedArgs.Should().NotBeNull(because: "the re-parsed arguments should not be null");
                    reparsedArgs.Length.Should().Be(expectedArgs?.Length, because:
                        $"the number of re-parsed arguments should ne {expectedArgs?.Length}, not {reparsedArgs.Length}");
                    for (int i = 0; i < expectedArgs!.Length; ++i)
                    {
                        reparsedArgs[i].Should().Be(expectedArgs[i], because:
                            $"the re-parsed argument at index {i} should be '{expectedArgs[i]}', not '{reparsedArgs[i]}'");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nException {ex.GetType().Name} thrown during round-trip conversion with message:\n  {ex.Message}");
                if (isRoundTrip)
                {
                    Console.WriteLine("  Round-trip conversion was required, test will fail due to exception.\n");
                    throw;  // re-throw expected exception
                }
                else
                {
                    Console.WriteLine("  Round-trip conversion was NOT required, test will not fail because of exception.\n");
                }
            }
        }




    }

}

