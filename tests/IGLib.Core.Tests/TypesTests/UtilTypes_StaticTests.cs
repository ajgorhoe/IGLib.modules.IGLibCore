using System;
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;
using IGLib.Tests.Base;

using static IGLib.Types.Extensions.UtilTypes;
using System.Numerics;

#if false  // please leave this block such that 
using static IGLib.Types.Extensions.UtilTypes;
using IGLib.Types.Extensions;
using UtilTypes = IGLib.Types.Extensions.UtilTypes;
#endif

namespace IGLib.Types.Tests
{

    /// <summary>Tests of type utilities from <see cref="IGLib.Types.Extensions.UtilTypes"/>, mainly type conversion tests.
    /// <para>The "using static" directive is used, such that utility methods can be called without
    /// stating the namespace and containing class, but they cannot be called as extension methods.</para>
    /// <para>This contains the most complete tests of <see cref="IGLib.Types.Extensions.UtilTypes"/></para>
    /// <para>For tests with extension methods, see <see cref="UtilTypes_ExtensionMetodsTests"/></para></summary>
    public class UtilTypesTests : TestBase<UtilTypesTests>
    {
        
        
        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> property.</summary>
        /// <param name=""></param>
        public UtilTypesTests(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        {   }


#if false  // change condition to true when testing effects of using directives!
        /// <summary>This contains a code block that demonstrates in what ways the methods from <see cref="UtilTypes"/>
        /// can be called, and what #using directives one needs for this.</summary>
        protected static void TestSyntax()
        {
            object o = (double) 2.0;

            // Needs:
            // using  IGLib.Types.Extensions
            bool isConvertible1 = IsConvertibleToInt(o);

            // Needs:
            // using static IGLib.Types.Extensions.UtilTypes;
            bool isConvertible2 = o.IsConvertibleToInt();

            // Needes either of:
            // using IGLib.Types.Extensions;  // but this also enables extension methods, which is not always wanted
            // using UtilTypes = IGLib.Types.Extensions.UtilTypes;
            bool isConvertible3 = UtilTypes.IsConvertibleToInt(o);

        }
#endif

        [Theory]
        // ****
        // Conversion of string:
        // ****
        [InlineData("2", false, 2)]
        [InlineData("-2", false, -2)]
        [InlineData("25_000", false, 25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
        [InlineData("-25_000", false, -25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
        [InlineData("25,000", false, 25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
        [InlineData("-25,000", false, -25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
        // String presentations are floating point instead of integers, conversion throws exception:
        [InlineData("2.0", false, 2, true, "floating point, not an int")]
        [InlineData("-2.0", false, -2, true, "floating point, not an int")]
        // Converson of string, OUT OF RANGE:
        [InlineData("2147483649", false, 0, true, "greater than max int")]
        [InlineData("-2147483650", false, 0, true, "less than min int")]
        // ****
        // Conversion of double:
        // ****
        [InlineData((double)2.0, false, 2)]
        [InlineData((double)-2.0, false, -2)]
        // Conversion of double, inprecise - ROUNDED:
        [InlineData((double)2.4999, false, 2, false, "should be rounded below (precise not requested)")]
        [InlineData((double)2.5001, false, 3, false, "should be rounded above (precise not requested)")]
        [InlineData((double)-2.4999, false, -2, false, "should be rounded above (precise not requested)")]
        [InlineData((double)-2.5001, false, -3, false, "should be rounded below (precise not requested)")]
        // Converson of double, OUT OF RANGE:
        [InlineData((double)int.MaxValue + 2.0, false, 0, true, "greater than max int")]
        [InlineData((double)int.MinValue - 2.0, false, 0, true, "less than min int")]
        // Conversion of double, precise conversion required:
        [InlineData((double)2.0, true, 2, false, "precise conversion requested, can be done")]
        [InlineData((double)-2.0, true, -2, false, "precise conversion requested, can be done")]
        [InlineData((double)2.1, true, 2, true, "precise conversion requested, cannot be done because of decimal part")]
        [InlineData((double)-2.1, true, -2, true, "precise conversion requested, cannot be done because of decimal part")]
        // ****
        // Conversion of long:
        // ****
        [InlineData((long)2, false, 2)]
        [InlineData((long)-2, false, -2)]
        // Converson of long, OUT OF RANGE:
        [InlineData((long)int.MaxValue + 2, false, 0, true, "greater than max int")]
        [InlineData((long)int.MinValue - 2, false, 0, true, "less than min int")]
        // ****
        // Conversion of unsigned int:
        // ****
        [InlineData((uint)2, false, 2)]
        // Converson of unsigned int, OUT OF RANGE:
        [InlineData((uint)int.MaxValue + 2, false, 0, true, "greater than max int")]
        // Conversion of bool:
        // ****
        [InlineData((bool)true, false, 1)]
        [InlineData((bool)false, false, 0)]
        // ****
        // Conversion of char:
        // ****
        [InlineData((char)'x', false, (int)'x', false, "conversion of ASCII character to int")]
        [InlineData((char)2836, false, 2836, false, "conversion of non-ASCII character to int")]
        protected void ToInt_WorksCorrectly(object converted, bool precise, int expectedResult,
            bool exceptionExpected = false, string comment = null)
        {
            Output.WriteLine($"Testing conversion of objects to type int:");
            Output.WriteLine($"Object to be converted: {converted}, type: {converted?.GetType().Name}");
            Output.WriteLine($"Precise conversion required: {(precise?"yes": "no")}.");
            Console.WriteLine("");
            if (exceptionExpected)
            {
                Console.WriteLine("Exception is expected.");
            }
            else
            {
                Console.WriteLine($"Expected result of conversion: {expectedResult}");
            }
            if (!string.IsNullOrEmpty(comment))
            {
                Console.WriteLine($"Additional comment: \n  {comment}");
            }
            Console.WriteLine("");
            // Arrange:
            // Act:
            bool exceptionThrown = false;
            Console.WriteLine("Result of conversion:");
            try
            {
                int result = converted.ToInt(precise: precise);
                Console.WriteLine($"Result of conversion: {result}, expected: {expectedResult}");
            }
            catch(Exception ex)
            {
                exceptionThrown = true;
                Console.WriteLine($"{ex.GetType().Name} thrown: {ex.Message}");
            }
            if (exceptionExpected)
            {
                exceptionThrown.Should().BeTrue(because: "This conversion attempt is expected to throw an exception.");
            }
            else
            {
                exceptionThrown.Should().BeFalse(because: "This conversion should be performed without exceptions.");
            }
        }








        #region Samples_ToDeleteLater

        /// <summary><para>Typical form of a simple unit test. The [Fact] attribute in square brackets (type 
        /// <see cref="FactAttribute"/>) is used to tell that this is a test method, and it can be executed by
        /// the test environment when you run tests (this also enables the "Run Tests" item in the context
        /// menu when you right-click the test method name, enabling to quickly run the test).</para>
        /// <para>The <see cref="TestBase{TestClass}.Output"/> property inherited from the test class can
        /// be used to write additional information to the output of the test.</para>
        /// <para>Assertions are used to verify that the result match what is expected. In this case,
        /// "fluent assertions" are used, which are implemented as extension methods for various types. In 
        /// this example, the int.Should() extension method is used to do the assertion. If the assertion
        /// fails then the test would fail and this would be visible in Test Explorer. The test would also
        /// fail if unhandled exception is thrown in a test.</para>>
        /// <para>This fictional demonstrative example tests that the + operator correctly sums two integer
        /// numbers.</para></summary>
        [Fact]
        protected void TestDemo_Simple_Addition_Test()
        {
            Output.WriteLine($"Testing addition of two numbers of type int:");
            // ** Arrange:
            int a = 22;
            int b = 7;
            int expectedResult = 29;
            // ** Act:
            int result = a + b;
            Output.WriteLine($"Input: a={a}, b={b}, result vs. expected: {result} vs. {expectedResult}");
            // ** Assert:
            result.Should().Be(expectedResult);
        }


        /// <summary><para>This method shows how to use tests with parameters. Procedure is as follows:</para>
        /// <para>  * Add parameters to the test method. In this case, parameters <paramref name="a"/> and
        /// <paramref name="b"/> represent summands that will be added in each test, and the <paramref name="expectedResult"/>
        /// parameter states the expected result of summation for these summands, such that we can verify correctness.</para>
        /// <para>  * Add the attribute [Theory] to the test method (not [Fact], which is for non-parametric tests).</para>
        /// <para>  * Add one [InlineData(...)] attribute with listed parameter values for each set of
        /// parameters for which the test should be executed.</para></summary>
        [Theory]                 // Use [Theory] attribute instead of [Fact] for parameterized tests
        [InlineData(2, 8, 10)]   // Use [InlineData(...)] attribute to list sets of parameter values (each set means a separate test)
        [InlineData(0, 0, 0)]
        [InlineData(0, 22, 22)]
        [InlineData(14, 0, 14)]
        [InlineData(-3, 0, -3)]
        [InlineData(-4, -5, -9)]
        [InlineData(-2, 5, 3)]
        [InlineData(2, -6, -4)]
        protected void TestDemo_Addition_Test(int a, int b, int expectedResult)
        {
            Output.WriteLine($"Testing addition of two numbers of type int:");
            // Arrange
            // Act
            int result = a + b;  // perform the operation that we want to test
            // Write informative output to the test output
            Output.WriteLine($"Input: a={a}, b={b}, result vs. expected: {result} vs. {expectedResult}");
            // Assert
            result.Should().Be(expectedResult); // assert correctness of operation
        }

        #endregion Samples_ToDeleteLater


    }

}

