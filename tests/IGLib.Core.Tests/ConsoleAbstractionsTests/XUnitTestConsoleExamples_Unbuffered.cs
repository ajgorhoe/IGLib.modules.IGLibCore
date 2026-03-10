
#nullable disable

using Castle.Core.Logging;
using FluentAssertions;
using IGLib.Commands.Tests;
using IGLib.ConsoleAbstractions;
using IGLib.ConsoleAbstractions.Extensions;
using IGLib.Tests.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace IGLib.ConsoleAbstractions.Tests
{



    /// <summary>Examples and tests of the <see cref="TestBase{TestClassType}.Console"/> property and its actual type <see cref="XUnitOutputConsole"/>
    /// in the default setting where the console is unbuffered.</summary>
    public class XUnitTestConsoleExamples_UnBuffered : TestBase<XUnitTestConsoleExamples_UnBuffered>
    {


        /// <summary>Calls the base constructure in such a way that the <see cref="IConsole"/> object <see cref="TestBase{TestClassType}.Console"/>
        /// is unbuffered.</summary>
        /// <param name="output">The <see cref="ITestOutputHelper"/> object that will be accessible via the <see cref="TestBase{TestClassType}.Output"/> property,
        /// and will also be wrapped by an <see cref="XUnitOutputConsole"/> object accessible via the <see cref="TestBase{TestClassType}.Console"/>
        /// property (declared type <see cref="IConsole"/>).</param>
        public XUnitTestConsoleExamples_UnBuffered(ITestOutputHelper output) : base(output, isConsoleLineBuffered: false)
        {
            // Remark: the base constructor will assign the Output and Console properties.
        }

        /// <summary>Expected actual type of the <see cref="TestBase{TestClassType}.Console"/> property.</summary>
        public static Type ExpectedConsoleType => XUnitTestConsoleExamples_DefaultLineBuffered.ExpectedConsoleType;


        [Fact]
        protected void XUnitTestConsole_ConsolePropertyIsNotNull()
        {
            Console.WriteLine($"This verifies that the {nameof(Console)} property is not null:\n");
            Console.WriteLine($"Is the {nameof(Console)} property null: {Console == null}");
            Console.Should().NotBeNull(because: "The {nameof(Console)} property should be properly initialized and should not be null.");
        }

        [Fact]
        protected void XUnitTestConsole_ConsolePropertyIsOfCorrectType()
        {
            Console.WriteLine($"This verifies that the {nameof(Console)} property is of correct type:\n");
            Type expectedConsoleType = typeof(XUnitOutputConsole);
            Console.Should().NotBeNull(because: "PRECOND: the {nameof(Console)} property should not be null.");
            Type consoleType = Console.GetType();
            Console.WriteLine($"Type of the {nameof(Console)} property: {consoleType.FullName};\n  expected: {expectedConsoleType.FullName}");
            consoleType.Should().Be(typeof(XUnitOutputConsole));
        }

        [Fact]
        protected void XUnitTestConsole_ConsoleObjectIsNotLineBuffered()
        {
            Console.WriteLine($"This verifies that the actual {nameof(Console)} property is line buffered.\n");
            Console.WriteLine($"Is {nameof(Console)} null: {Console == null}");
            Console.Should().NotBeNull(because: "PRECOND: the {nameof(Console)} property should not be null.");
            XUnitOutputConsole console = Console as XUnitOutputConsole;
            Console.WriteLine($"Declared type of {nameof(Console)} property: {typeof(IConsole)}");
            Console.WriteLine($"Actual type of {nameof(Console)} property: {Console.GetType().FullName};\n  expected: {typeof(XUnitOutputConsole).FullName}");
            Console.WriteLine($"Is {nameof(Console)} of correct type: {console != null}");
            console.Should().NotBeNull(because: $"PRECOND: The {nameof(Console)} property should be of type {nameof(XUnitOutputConsole)}");
            Console.WriteLine($"Value of {nameof(Console)}'s {nameof(console.IsLineBuffered)} property: {console.IsLineBuffered}");
            console.IsLineBuffered.Should().BeFalse(because: $"In this test class, the {nameof(Console)} should NOT be line buffered.");
        }

        [Fact]
        protected void XUnitTestConsole_Default_IsLineBuffered_IsFalse()
        {
            Console.WriteLine($"This verifies the default value of the {nameof(IsConsoleOutputLineBuffered)} property.\n");
            Console.WriteLine($"Default value is: {IsConsoleOutputLineBuffered}");
            IsConsoleOutputLineBuffered.Should().BeFalse(because: $"In this class, {nameof(Console)} should NOT be line buffered.");
        }


        #region Examples

        // This region contains tests without assertions; the expected output is described in comments and can be verified manually.


        /// <summary>Example that just writes several lines of text using the call to <see cref="IConsole.WriteLine(string?)"/>
        /// method. All output should be visible because this method immediattely flushes the buffer.</summary>
        [Fact]
        protected void XUnitTestConsole_Example_WriteLine_Works()
        {
            Console.WriteLine($"Demonstration of Console.WriteLin(string?):\n");
            Console.WriteLine($"This is line 1 of output.");
            Console.WriteLine($"This is line 2 of output.");
            Console.WriteLine($"This is line 3 of output.");
            // Expected output:
            // Demonstration of Console.WriteLin(string ?):
            // 
            // This is line 1 of output.
            // This is line 2 of output.
            // This is line 3 of output.
        }


        /// <summary>Example that performs several calls to <see cref="IConsole.Write(string?)"/> followed by a call to
        /// <see cref="IConsole.WriteLine(string?)"/>. All output should be visible because the last call to
        /// <see cref="IConsole.WriteLine(string?)"/> should flush the internal buffer even in line buffered mode.</summary>
        [Fact]
        protected void XUnitTestConsole_Example_Write_WorksWhenFollowedByWriteLine()
        {
            Console.WriteLine($"Demonstration of Console.Write(string?) when followed by WriteLine(string?):\n");
            Console.Write($"<part 1>");
            Console.Write($"<part 2>");
            Console.Write($"<part 3>");
            Console.WriteLine($"This line is written after several Write() calls.");
            // Expected output:
            // Demonstration of Console.Write(string ?) when followed by WriteLine(string?):
            // 
            // < part 1 >⏎
            // < part 2 >⏎
            // < part 3 >⏎
            // This line is written after several Write() calls.
        }


        /// <summary>Example that performs several calls to <see cref="IConsole.Write(string?)"/> method, which are NOT followed 
        /// by a call to <see cref="IConsole.WriteLine(string?)"/>. Outputs from <see cref="IConsole.Write(string?)"/> should
        /// be VISIBLE when the test console <see cref="TestBase{TestClassType}.Console"/> is in the unbuffered mode because
        /// <see cref="IConsole.Write(string?)"/> flushes the buffer immediately in this mode.</summary>
        [Fact]
        protected void XUnitTestConsole_Example_Write_DoesNotWorkWhenNotFollowedByWriteLine()
        {
            Console.WriteLine($"Demonstration of Console.Write(string?) when NOT followed by WriteLine(string?):\n");
            Console.Write($"<part 1>");
            Console.Write($"<part 2>");
            Console.Write($"<part 3>");
            // Expected output:
            // Demonstration of Console.Write(string?) when NOT followed by WriteLine(string?):
            // 
            // <part 1>⏎
            // <part 2>⏎
            // <part 3>⏎
        }


        #endregion Examples



        #region SpeedTests

        [Fact]
        protected void XUnitTestConsole_SpeedTest_WriteLine()
        {
            Console.WriteLine($"Testing speed of {nameof(Console)}.{nameof(IConsole.WriteLine)}:\n");
            int numWarmupLines = 20;
            int numWrittenLines = 200;
            double minExecutionsPerSecond = -10_000;  // low treshold to verify that Console.WriteLine() is not extremely slow
            Stopwatch sw = new();
            // Warm up (takes the slow initial runs due to just in time compilation, cache misses, etc.):
            Console.WriteLine($"\nWarming up by performing {numWarmupLines} WriteLine() executions...\n");
            sw.Start();
            for (int i = 0; i < numWarmupLines; ++i)
            {
                Console.WriteLine("Line No. " + i.ToString());
            }
            sw.Stop();
            Console.WriteLine($"\nWarmup: {numWarmupLines} WriteLine-s executed in {sw.Elapsed.TotalSeconds} s ({(double)numWarmupLines / (sw.Elapsed.TotalSeconds * 1e6)} M/s)");
            // Actual measurement:
            Console.WriteLine($"\nSpeed testing by performing {numWrittenLines} WriteLine() executions...\n");
            sw.Reset();
            sw.Start();
            for (int i = 0; i < numWrittenLines; ++i)
            {
                Console.WriteLine("Line No. " + i.ToString());
            }
            sw.Stop();
            double executionsPerSecond = (double)numWrittenLines / (sw.Elapsed.TotalSeconds);
            Console.WriteLine($"\nSpeed: {numWrittenLines} WriteLine-s executed in {sw.Elapsed.TotalSeconds} s ({(double)numWrittenLines / (sw.Elapsed.TotalSeconds * 1e6)} M/s)");
            executionsPerSecond.Should().BeGreaterThan(minExecutionsPerSecond, because: $"WriteLine should not be too slow (there should be at least {minExecutionsPerSecond} executions per second)");
        }

        [Fact]
        protected void XUnitTestConsole_SpeedTest_Write()
        {
            Console.WriteLine($"Testing speed of {nameof(Console)}.{nameof(IConsole.Write)}:\n");
            int numWarmupLines = 20;
            int numWrittenLines = 200;
            double minExecutionsPerSecond = -10_000;  // low treshold to verify that Console.Write() is not extremely slow
            Stopwatch sw = new();
            // Warm up (takes the slow initial runs due to just in time compilation, cache misses, etc.):
            Console.WriteLine($"\nWarming up by performing {numWarmupLines} Write(...) executions...\n");
            sw.Start();
            for (int i = 0; i < numWarmupLines; ++i)
            {
                Console.Write($"<Write {i}>");
            }
            sw.Stop();
            Console.WriteLine($"\nWarmup: {numWarmupLines} Write-s executed in {sw.Elapsed.TotalSeconds} s ({(double)numWarmupLines / (sw.Elapsed.TotalSeconds * 1e6)} M/s)");
            // Actual measurement:
            Console.WriteLine($"\nSpeed testing by performing {numWrittenLines} Write(...) executions...\n");
            sw.Reset();
            sw.Start();
            for (int i = 0; i < numWrittenLines; ++i)
            {
                Console.WriteLine($"<Write {i}>");
            }
            Console.WriteLine("");  // additional WriteLine() to ensure that all Write() calls are flushed to the output
            sw.Stop();
            double executionsPerSecond = (double)numWrittenLines / (sw.Elapsed.TotalSeconds);
            Console.WriteLine($"\nSpeed: {numWrittenLines} Write-s executed in {sw.Elapsed.TotalSeconds} s ({(double)numWrittenLines / (sw.Elapsed.TotalSeconds * 1e6)} M/s)");
            executionsPerSecond.Should().BeGreaterThan(minExecutionsPerSecond, because: $"Write should not be too slow (there should be at least {minExecutionsPerSecond} executions per second)");
        }


        #endregion SpeedTests


    }








}

