
#nullable disable

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using IGLib.Tests.Base;
using IGLib.Commands.Tests;
using System.Text;
using System.Linq;

using IGLib.ConsoleAbstractions;
using IGLib.Tests.Base;

namespace IGLib.ConsoleAbstractions.Tests
{

    /// <summary>Examples and tests of using the <see cref="TestBase{TestClassType}.Console"/> property and its actual type <see cref="XUnitOutputConsole"/>
    /// in the default setting where the console is line buffered.</summary>
    public class XUnitTestConsoleExamples_DefaultLineBuffered : TestBase<XUnitTestConsoleExamples_DefaultLineBuffered>
    {


        /// <summary>Calls the base constructure in such a way that the <see cref="IConsole"/> object <see cref="TestBase{TestClassType}.Console"/>
        /// is line buffered. The <see cref="TestBase{TestClassType}.IsConsoleOutputLineBuffered"/> property will evaluate to true.</summary>
        /// <param name="output">The <see cref="ITestOutputHelper"/> object that will be accessible via the <see cref="TestBase{TestClassType}.Output"/> property,
        /// and will also be wrapped by an <see cref="XUnitOutputConsole"/> object accessible via the <see cref="TestBase{TestClassType}.Console"/>
        /// property (declared type <see cref="IConsole"/>).</param>
        public XUnitTestConsoleExamples_DefaultLineBuffered(ITestOutputHelper output) :  base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign the Output and Console properties.
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
        protected void XUnitTestConsole_ConsoleObjectIsLineBuffered()
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
            console.IsLineBuffered.Should().BeTrue(because: $"By default, the {nameof(Console)} should be line buffered.");
        }

        [Fact]
        protected void XUnitTestConsole_DefauleIsLineBuffered()
        {
            Console.WriteLine($"This verifies the default value of the {nameof(IsConsoleOutputLineBuffered)} property.\n");
            Console.WriteLine($"Default value is: {IsConsoleOutputLineBuffered}");
        }


        [Fact]
        protected void XUnitTestConsole_WriteLine_Works()
        {
            Console.WriteLine($"Demonstration of Console.WriteLin(string?):\n");
            Console.WriteLine($"This is line 1 of text");
            Console.WriteLine($"This is line 2 of text");
        }


        [Fact]
        protected void XUnitTestConsole_Write_WorksWhenFollowedByWriteLine()
        {
            Console.WriteLine($"Demonstration of Console.Write(string?) when followed by WriteLine():\n");
            Console.WriteLine($"First part.");
            Console.WriteLine($"Second part.");
            Console.WriteLine($"Third part.");
            Console.WriteLine($" This is line is written after two Write() calls.");
        }

        [Fact]
        protected void XUnitTestConsole_Write_DoesNotWorkWhenNotFollowedByWriteLine()
        {
            Console.WriteLine($"Demonstration of Console.Write(string?) when NOT followed by WriteLine():\n");
            Console.WriteLine($"First part.");
            Console.WriteLine($"Second part.");
            Console.WriteLine($"Third part.");
        }


    }



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


    }








}

