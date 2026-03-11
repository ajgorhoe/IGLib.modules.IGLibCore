
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

    /// <summary>Examples and tests of using the <see cref="TestBase{TestClassType}.Console"/> property and its actual type <see cref="XUnitOutputConsole"/>
    /// in the default setting where the console is line buffered.</summary>
    /// <remarks>See also the <see cref="XUnitOutputConsole"/> documentation and remarks.</remarks>
    public class XUnitTestConsoleExamples_SwitchBuffering : TestBase<XUnitTestConsoleExamples_DefaultLineBuffered>
    {


        /// <summary>Calls the base constructor in such a way that the <see cref="IConsole"/> object <see cref="TestBase{TestClassType}.Console"/>
        /// is unbuffered. The <see cref="TestBase{TestClassType}.IsConsoleOutputLineBuffered"/> property will evaluate to false before
        /// actively changing buffering.</summary>
        /// <param name="output">The <see cref="ITestOutputHelper"/> object that will be accessible via the <see cref="TestBase{TestClassType}.Output"/> property,
        /// and will also be wrapped by an <see cref="XUnitOutputConsole"/> object accessible via the <see cref="TestBase{TestClassType}.Console"/>
        /// property (declared type <see cref="IConsole"/>).</param>
        public XUnitTestConsoleExamples_SwitchBuffering(ITestOutputHelper output) :  base(output, isConsoleLineBuffered: false)  // calls base class's constructor
        {
            // Remark: the base constructor will assign the Output and Console properties.
        }

        /// <summary>Expected actual type of the <see cref="TestBase{TestClassType}.Console"/> property.</summary>
        public static Type ExpectedConsoleType => XUnitTestConsoleExamples_DefaultLineBuffered.ExpectedConsoleType;


        /// <summary>Basic precondition test - <see cref="TestBase{TestClassType}"/> is not null.</summary>
        [Fact]
        protected void XUnitTestConsole_ConsolePropertyIsNotNull()
        {
            Console.WriteLine($"This verifies that the {nameof(Console)} property is not null:\n");
            Console.WriteLine($"Is the {nameof(Console)} property null: {Console == null}");
            Console.Should().NotBeNull(because: "The {nameof(Console)} property should be properly initialized and should not be null.");
        }

        [Fact]
        /// <summary>Basic precondition test - <see cref="TestBase{TestClassType}"/> is of correct type (<see cref="XUnitOutputConsole"/>).</summary>
        protected void XUnitTestConsole_ConsolePropertyIsOfCorrectType()
        {
            Console.WriteLine($"This verifies that the {nameof(Console)} property is of correct type:\n");
            Console.Should().NotBeNull(because: "PRECOND: the {nameof(Console)} property should not be null.");
            Type consoleType = Console.GetType();
            Console.WriteLine($"Type of the {nameof(Console)} property: {consoleType.FullName};\n  expected: {ExpectedConsoleType.FullName}");
            consoleType.Should().Be(ExpectedConsoleType);
        }


        [Fact]
        protected void XUnitTestConsole_SwitchToLineBufferedConsoleWorksCorrectly()
        {
            Console.WriteLine($"Testing switch to line buffered test {nameof(Console)} via test class's switch:\n");
            bool? isLineBufferedInitial = IsConsoleOutputLineBuffered;
            Console.WriteLine($"Is test's {nameof(Console)} currently line buffered: {isLineBufferedInitial}");
            // Check some basic pre-conditions:
            Console.WriteLine($"Is {nameof(Console)} null: {Console == null}");
            Console.Should().NotBeNull(because: "PRECOND: the {nameof(Console)} property should be initilized and not be nulll");
            Console.WriteLine($"Actual type of the {nameof(Console)} property: {Console.GetType().FullName}");
            Console.Should().BeOfType(ExpectedConsoleType, because: $"PRECOND: the {nameof(Console)} property should be of type {ExpectedConsoleType.FullName}");
            try
            {
                // Set the console line buffered mode:
                XUnitOutputConsole console = Console as XUnitOutputConsole;
                Console.WriteLine($"Switching console to line buffered mode via class's {nameof(IsConsoleOutputLineBuffered)} switch...");
                IsConsoleOutputLineBuffered = true;
                Console.WriteLine("After the switch:");
                Console.WriteLine($"The value of {nameof(IsConsoleOutputLineBuffered)} property: {IsConsoleOutputLineBuffered}");
                IsConsoleOutputLineBuffered.Should().BeTrue(because: $"after switching to unbuffered mode, the {
                    IsConsoleOutputLineBuffered} property should be false");
                Console.WriteLine($"The value of {nameof(Console)}.{nameof(console.IsLineBuffered)} property: {console.IsLineBuffered}");
                console.IsLineBuffered.Should().BeTrue(because: $"the value of the {nameof(console.IsLineBuffered)} propety on {
                    nameof(Console)} should be the same as the value of {nameof(IsConsoleOutputLineBuffered)} property on the test class");
            }
            finally
            {
                // restore the initial value of IsConsoleOutputLineBuffered:
                IsConsoleOutputLineBuffered = isLineBufferedInitial;
            }
        }

        [Fact]
        protected void XUnitTestConsole_SwitchUnBufferedConsoleWorksCorrectly()
        {
            Console.WriteLine($"Testing switch to unbuffered test {nameof(Console)} via test class's switch:\n");
            bool? isLineBufferedInitial = IsConsoleOutputLineBuffered;
            Console.WriteLine($"Is test's {nameof(Console)} currently line buffered: {isLineBufferedInitial}");
            // Check some basic pre-conditions:
            Console.WriteLine($"Is {nameof(Console)} null: {Console == null}");
            Console.Should().NotBeNull(because: "PRECOND: the {nameof(Console)} property should be initilized and not be nulll");
            Console.WriteLine($"Actual type of the {nameof(Console)} property: {Console.GetType().FullName}");
            Console.Should().BeOfType(ExpectedConsoleType, because: $"PRECOND: the {nameof(Console)} property should be of type {ExpectedConsoleType.FullName}");
            try
            {
                // Set the console to unbuffered mode:
                XUnitOutputConsole console = Console as XUnitOutputConsole;
                Console.WriteLine($"Switching console to unbuffered mode via class's {nameof(IsConsoleOutputLineBuffered)} switch...");
                IsConsoleOutputLineBuffered = false;
                Console.WriteLine("After the switch:");
                Console.WriteLine($"The value of {nameof(IsConsoleOutputLineBuffered)} property: {IsConsoleOutputLineBuffered}");
                IsConsoleOutputLineBuffered.Should().BeFalse(because: $"after switching to unbuffered mode, the {
                    IsConsoleOutputLineBuffered} property should be false");
                Console.WriteLine($"The value of {nameof(Console)}.{nameof(console.IsLineBuffered)} property: {console.IsLineBuffered}");
                console.IsLineBuffered.Should().BeFalse(because: $"the value of the {nameof(console.IsLineBuffered)} propety on {
                    nameof(Console)} should be the same as the value of {nameof(IsConsoleOutputLineBuffered)} property on the test class");
            }
            finally
            {
                // restore the initial value of IsConsoleOutputLineBuffered:
                IsConsoleOutputLineBuffered = isLineBufferedInitial;
            }
        }


        [Fact]
        protected void XUnitTestConsole_SwitchToLineBufferedConsoleViaConsolePropertyWorksCorrectly()
        {
            Console.WriteLine($"Testing switch to line buffered test {nameof(Console)} via test console's switch:\n");
            bool? isLineBufferedInitial = IsConsoleOutputLineBuffered;
            Console.WriteLine($"Is test's {nameof(Console)} currently line buffered: {isLineBufferedInitial}");
            // Check some basic pre-conditions:
            Console.WriteLine($"Is {nameof(Console)} null: {Console == null}");
            Console.Should().NotBeNull(because: "PRECOND: the {nameof(Console)} property should be initilized and not be nulll");
            Console.WriteLine($"Actual type of the {nameof(Console)} property: {Console.GetType().FullName}");
            Console.Should().BeOfType(ExpectedConsoleType, because: $"PRECOND: the {nameof(Console)} property should be of type {ExpectedConsoleType.FullName}");
            try
            {
                // Set the console line buffered mode:
                XUnitOutputConsole console = Console as XUnitOutputConsole;
                Console.WriteLine($"\nSwitching console to line buffered mode via {nameof(Console)}'s {nameof(console.IsLineBuffered)} switch...");
                console.IsLineBuffered = true;
                Console.WriteLine("After the switch:");
                Console.WriteLine($"The value of {nameof(IsConsoleOutputLineBuffered)} property: {IsConsoleOutputLineBuffered}");
                IsConsoleOutputLineBuffered.Should().BeTrue(because: $"after switching to unbuffered mode, the {IsConsoleOutputLineBuffered} property should be false");
                Console.WriteLine($"The value of {nameof(Console)}.{nameof(console.IsLineBuffered)} property: {console.IsLineBuffered}");
                console.IsLineBuffered.Should().BeTrue(because: $"the value of the {nameof(console.IsLineBuffered)} propety on {nameof(Console)} should be the same as the value of {nameof(IsConsoleOutputLineBuffered)} property on the test class");
            }
            finally
            {
                // restore the initial value of IsConsoleOutputLineBuffered:
                IsConsoleOutputLineBuffered = isLineBufferedInitial;
            }
        }

        [Fact]
        protected void XUnitTestConsole_SwitchUnBufferedConsoleViaConsolePropertyWorksCorrectly()
        {
            Console.WriteLine($"Testing switch to unbuffered test {nameof(Console)} via test console's switch:\n");
            bool? isLineBufferedInitial = IsConsoleOutputLineBuffered;
            Console.WriteLine($"Is test's {nameof(Console)} currently line buffered: {isLineBufferedInitial}");
            // Check some basic pre-conditions:
            Console.WriteLine($"Is {nameof(Console)} null: {Console == null}");
            Console.Should().NotBeNull(because: "PRECOND: the {nameof(Console)} property should be initilized and not be nulll");
            Console.WriteLine($"Actual type of the {nameof(Console)} property: {Console.GetType().FullName}");
            Console.Should().BeOfType(ExpectedConsoleType, because: $"PRECOND: the {nameof(Console)} property should be of type {ExpectedConsoleType.FullName}");
            try
            {
                // Set the console to unbuffered mode:
                XUnitOutputConsole console = Console as XUnitOutputConsole;
                Console.WriteLine($"\nSwitching console to unbuffered mode via {nameof(Console)}'s {nameof(console.IsLineBuffered)} switch...");
                console.IsLineBuffered = false;
                Console.WriteLine("After the switch:");
                Console.WriteLine($"The value of {nameof(IsConsoleOutputLineBuffered)} property: {IsConsoleOutputLineBuffered}");
                IsConsoleOutputLineBuffered.Should().BeFalse(because: $"after switching to unbuffered mode, the {
                    IsConsoleOutputLineBuffered} property should be false");
                Console.WriteLine($"The value of {nameof(Console)}.{nameof(console.IsLineBuffered)} property: {console.IsLineBuffered}");
                console.IsLineBuffered.Should().BeFalse(because: $"the value of the {nameof(console.IsLineBuffered)} propety on {
                    nameof(Console)} should be the same as the value of {nameof(IsConsoleOutputLineBuffered)} property on the test class");
            }
            finally
            {
                // restore the initial value of IsConsoleOutputLineBuffered:
                IsConsoleOutputLineBuffered = isLineBufferedInitial;
            }
        }


    }

}

