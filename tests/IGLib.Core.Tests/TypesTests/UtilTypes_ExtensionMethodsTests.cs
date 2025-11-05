using System;
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;
using IGLib.Tests.Base;

using IGLib.Types.Extensions;

namespace IGLib.Types.Tests
{

    /// <summary>Tests of type utilities, mainly type conversion utilities.
    /// <para>The "using static" directive is used, such that utility methods can be called without
    /// staticng the namespace and containing class, but they cannot be called as extension methods.</para></summary>
    public class UtilTypes_ExtensionMetodsTests : TestBase<UtilTypes_ExtensionMetodsTests>
    {
        
        
        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> property.</summary>
        /// <param name=""></param>
        public UtilTypes_ExtensionMetodsTests(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        {   }


        [Fact]
        protected void ToInt_CanBeCalledAsExtensionMethod()
        {
            Console.WriteLine("Testing conversion to int by extension method:");
            object o = null;
            int result = 0, expected = 0;

            Console.WriteLine("\nConverting string object to int:");
            o = (string)"2";
            expected = 2;
            result = o.ToInt();
            Console.WriteLine($"Converted object: {o}, type: {o.GetType().Name}, expected result: {expected}, actual: {result}");
            result.Should().Be(expected);
            o = (string)"-2";
            expected = -2;
            result = o.ToInt();
            Console.WriteLine($"Converted object: {o}, type: {o.GetType().Name}, expected result: {expected}, actual: {result}");
            result.Should().Be(expected);

            Console.WriteLine("\nConverting double object to int:");
            o = (double)2.1;
            expected = 2;
            result = o.ToInt();
            Console.WriteLine($"Converted object: {o}, type: {o.GetType().Name}, expected result: {expected}, actual: {result}");
            result.Should().Be(expected);
            o = (double)-2.0;
            expected = -2;
            result = o.ToInt(precise: true);
            Console.WriteLine($"Converted object: {o}, type: {o.GetType().Name}, expected result (precise): {expected}, actual: {result}");
            result.Should().Be(expected);

            Console.WriteLine("\nConverting long object to int:");
            o = (long)2;
            expected = 2;
            result = o.ToInt();
            Console.WriteLine($"Converted object: {o}, type: {o.GetType().Name}, expected result: {expected}, actual: {result}");
            result.Should().Be(expected);
            o = (long)-2;
            expected = -2;
            result = o.ToInt();
            Console.WriteLine($"Converted object: {o}, type: {o.GetType().Name}, expected result: {expected}, actual: {result}");
            result.Should().Be(expected);

            Console.WriteLine("\nConverting boolean object to int:");
            o = (bool)true;
            expected = 1;
            result = o.ToInt();
            Console.WriteLine($"Converted object: {o}, type: {o.GetType().Name}, expected result: {expected}, actual: {result}");
            result.Should().Be(expected);
            o = (bool)false;
            expected = 0;
            result = o.ToInt();
            Console.WriteLine($"Converted object: {o}, type: {o.GetType().Name}, expected result: {expected}, actual: {result}");
            result.Should().Be(expected);

        }




    }

}

