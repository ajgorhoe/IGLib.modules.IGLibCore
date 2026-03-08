
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

namespace IGLib.Commands.Tests
{

    /// <summary>Tests for generic commands (implementations of <see cref="IGenericCommand"/>).</summary>
    public class XUnitTestConsoleExamples_DefaultLineBuffered : TestBase<XUnitTestConsoleExamples_DefaultLineBuffered>
    {


        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's property <see cref="Output"/> and the property <see cref="TestBase{TestClassType}.Console"/> of type <see cref="IConsole"/>.</summary>
        /// <param name=""></param>
        public XUnitTestConsoleExamples_DefaultLineBuffered(ITestOutputHelper output) :  base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign the Output and Console properties.
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


        #region CommandCreation TO REMOVE

        [Fact]
        protected void CommandCreation_CommandsHaveDistinctIDsGrowingWithCreationTime()
        {
            // Arrange:
            Console.WriteLine("Testing that created commands have distinct IDs and that IDs grow with time of creation:\n");
            // Act:
            IGenericCommand cmd1 = new TestCmd1();
            Console.WriteLine($"Created command {nameof(cmd1)} of type {cmd1?.GetType()?.Name}, ID = {cmd1?.Id}");
            IGenericCommand cmd2 = new TestCmd2();
            Console.WriteLine($"Created command {nameof(cmd2)} of type {cmd2?.GetType()?.Name}, ID = {cmd2?.Id}");
            IGenericCommand cmd3 = new TestCmd3();
            Console.WriteLine($"Created command {nameof(cmd3)} of type {cmd3?.GetType()?.Name}, ID = {cmd3?.Id}");
            cmd1.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd1)}");
            cmd2.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd2)}");
            cmd3.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd3)}");
            // Assert:
            cmd1.Id.Should().NotBe(cmd2.Id, because: "cmd1 and cmd2 are different instances and must have different IDs");
            cmd1.Id.Should().NotBe(cmd3.Id, because: "cmd1 and cmd3 are different instances and must have different IDs");
            cmd2.Id.Should().NotBe(cmd3.Id, because: "cmd2 and cmd3 are different instances and must have different IDs");
            cmd2.Id.Should().BeGreaterThan(cmd1.Id, because: "cmd2 was created later than cmd1 and should have greater ID.");
            cmd3.Id.Should().BeGreaterThan(cmd2.Id, because: "cmd2 was created later than cmd1 and should have greater ID.");
        }

        #endregion CommandCreation


    }



}

