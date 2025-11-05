using FluentAssertions;
using IGLib.Tests.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace IGLib.Commands.Tests
{

    /// <summary>Base test class for classes that test different variants of <see cref="CommandRunnerExtendedUnsafe"/>.
    /// <para>Tests that are the same for all ariants of these classes should be defined in this class.
    /// Other tests should be defined in specific classes.</para>
    /// <para>For each specific class, redefine the method <see cref="CreateRunner"/>, which is used to
    /// create initialized instance of a specific <typeparamref name="TCommandRunner"/> class.</para></summary>
    /// <typeparam name="TCommandRunner">Type of command runner that is tested. This must be concretized in 
    /// specific tests.</typeparam>
    public class CommandRunnerTests: CommandRunnerTestsBase<ExecutionInfoExtended, CommandRunnerExtendedUnsafe>
    {
        
        
        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> property.</summary>
        /// <param name=""></param>
        public CommandRunnerTests(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output and Console property.
        }

        protected override CommandRunnerExtendedUnsafe CreateRunner()
        {
            return new();
        }


    }

}

