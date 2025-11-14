
#nullable disable

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



    //    #region ConvertTo_Tests


    //    [Theory]
    //    // ****
    //    // Conversion of string:
    //    // ****
    //    [InlineData("2", false, 2)]
    //    [InlineData("-2", false, -2)]
    //    // String presentations are floating point instead of integers, conversion throws exception:
    //    [InlineData("2.0", false, 2, true, "floating point, not an int")]
    //    // Converson of string, OUT OF RANGE:
    //    [InlineData("2147483649", false, 0, true, "greater than max int")]
    //    // ****
    //    // Conversion of double:
    //    // ****
    //    [InlineData((double)2.0, false, 2)]
    //    // Conversion of double, precise conversion required:
    //    [InlineData((double)2.0, true, 2, false, "precise conversion requested, can be done")]
    //    [InlineData((double)2.1, true, 2, true, "precise conversion requested, cannot be done because of decimal part")]
    //    // ****
    //    // Conversion of long:
    //    // ****
    //    [InlineData((long)2, false, 2)]
    //    // Converson of long, OUT OF RANGE:
    //    [InlineData((long)int.MaxValue + 2, false, 0, true, "greater than max int")]
    //    [InlineData((long)int.MinValue - 2, false, 0, true, "less than min int")]
    //    // ****
    //    // Conversion of unsigned int:
    //    // ****
    //    [InlineData((uint)2, false, 2)]
    //    // Conversion of bool:
    //    // ****
    //    [InlineData((bool)true, false, 1)]
    //    [InlineData((bool)false, false, 0)]
    //    // ****
    //    // Conversion of char:
    //    // ****
    //    [InlineData((char)'x', false, (int)'x', false, "conversion of ASCII character to int")]
    //    [InlineData((char)2836, false, 2836, false, "conversion of non-ASCII character to int")]
    //    [Obsolete]
    //    protected void ConvertTo_CanBeCalledAsExtensionMethod(object converted, bool precise, int expectedResult,
    //bool exceptionExpected = false, string comment = null)
    //    {
    //        Output.WriteLine($"Testing conversion of objects to type int:");
    //        Output.WriteLine($"Object to be converted: {converted}, type: {converted?.GetType().Name}");
    //        Output.WriteLine($"Precise conversion required: {(precise ? "yes" : "no")}.");
    //        Console.WriteLine("");
    //        if (exceptionExpected)
    //        {
    //            Console.WriteLine("Exception is expected.");
    //        }
    //        else
    //        {
    //            Console.WriteLine($"Expected result of conversion: {expectedResult}");
    //        }
    //        if (!string.IsNullOrEmpty(comment))
    //        {
    //            Console.WriteLine($"Additional comment: \n  {comment}");
    //        }
    //        Console.WriteLine("");
    //        // Arrange & Act:
    //        bool exceptionThrown = false;
    //        Console.WriteLine("Result of conversion:");
    //        try
    //        {
    //            int result = converted.ToInt(precise: precise);
    //            Console.WriteLine($"Result of conversion: {result}, expected: {expectedResult}");
    //        }
    //        catch (Exception ex)
    //        {
    //            exceptionThrown = true;
    //            Console.WriteLine($"{ex.GetType().Name} thrown: {ex.Message}");
    //        }
    //        if (exceptionExpected)
    //        {
    //            exceptionThrown.Should().BeTrue(because: "This conversion attempt is expected to throw an exception.");
    //        }
    //        else
    //        {
    //            exceptionThrown.Should().BeFalse(because: "This conversion should be performed without exceptions.");
    //        }
    //    }

    //    #endregion ConvertTo_Tests



    }

}

