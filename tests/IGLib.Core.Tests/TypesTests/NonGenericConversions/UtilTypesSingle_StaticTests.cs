
//#nullable disable

//using System;
//using Xunit;
//using FluentAssertions;
//using Xunit.Abstractions;
//using System.Collections.Generic;
//using IGLib.Tests.Base;


////using static IGLib.Types.Extensions.UtilTypes;

//using UtilTypesSingle = IGLib.Types.Single.Extensions.UtilTypesSingle;

//#if false  // please leave this block such that 
//using static IGLib.Types.Extensions.UtilTypes;
//using IGLib.Types.Extensions;
//#endif

//namespace IGLib.Types.Single.Tests
//{

//    /// <summary>Tests of type utilities from <see cref="IGLib.Types.Extensions.UtilTypesSingle"/>, mainly type conversion tests.
//    /// <para>The "using static" directive is used, such that utility methods can be called without
//    /// stating the namespace and containing class, but they cannot be called as extension methods.</para>
//    /// <para>This contains the most complete tests of <see cref="IGLib.Types.Extensions.UtilTypesSingle"/></para>
//    /// <para>For tests with extension methods, see <see cref="UtilTypes_ExtensionMetodsTests"/></para></summary>
//    public class UtilTypesSingleTests : TestBase<UtilTypesSingleTests>
//    {
        
        
//        /// <summary>This constructor, when called by the test framework, will bring in an object 
//        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
//        /// accessed through the base class's <see cref="Output"/> property.</summary>
//        /// <param name=""></param>
//        public UtilTypesSingleTests(ITestOutputHelper output) :
//            base(output)  // calls base class's constructor
//        {   }


//#if false  // change condition to true when testing effects of using directives!
//        /// <summary>This contains a code block that demonstrates in what ways the methods from <see cref="UtilTypes"/>
//        /// can be called, and what #using directives one needs for this.</summary>
//        protected static void TestSyntax()
//        {
//            object o = (double) 2.0;

//            // Needs:
//            // using  IGLib.Types.Extensions
//            bool isConvertible1 = IsConvertibleToInt(o);

//            // Needs:
//            // using static IGLib.Types.Extensions.UtilTypes;
//            bool isConvertible2 = o.IsConvertibleToInt();

//            // Needes either of:
//            // using IGLib.Types.Extensions;  // but this also enables extension methods, which is not always wanted
//            // using UtilTypes = IGLib.Types.Extensions.UtilTypes;
//            bool isConvertible3 = UtilTypes.IsConvertibleToInt(o);

//        }
//#endif


//        #region ToInt_Tests

//        [Theory]
//        // ****
//        // Conversion of string:
//        // ****
//        [InlineData("2", false, 2)]
//        [InlineData("-2", false, -2)]
//        [InlineData("25_000", false, 25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
//        [InlineData("-25_000", false, -25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
//        [InlineData("25,000", false, 25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
//        [InlineData("-25,000", false, -25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
//        // String presentations are floating point instead of integers, conversion throws exception:
//        [InlineData("2.0", false, 2, true, "floating point, not an int")]
//        [InlineData("-2.0", false, -2, true, "floating point, not an int")]
//        // Converson of string, OUT OF RANGE:
//        [InlineData("2147483649", false, 0, true, "greater than max int")]
//        [InlineData("-2147483650", false, 0, true, "less than min int")]
//        // String representations are hexadecimal numbers - not supported:
//        [InlineData("0x1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
//        [InlineData("1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
//        // ****
//        // Conversion of double:
//        // ****
//        [InlineData((double)2.0, false, 2)]
//        [InlineData((double)-2.0, false, -2)]
//        // Conversion of double, inprecise - ROUNDED:
//        [InlineData((double)2.4999, false, 2, false, "should be rounded below (precise not requested)")]
//        [InlineData((double)2.5001, false, 3, false, "should be rounded above (precise not requested)")]
//        [InlineData((double)-2.4999, false, -2, false, "should be rounded above (precise not requested)")]
//        [InlineData((double)-2.5001, false, -3, false, "should be rounded below (precise not requested)")]
//        // Converson of double, OUT OF RANGE:
//        [InlineData((double)int.MaxValue + 2.0, false, 0, true, "greater than max int")]
//        [InlineData((double)int.MinValue - 2.0, false, 0, true, "less than min int")]
//        // Conversion of double, precise conversion required:
//        [InlineData((double)2.0, true, 2, false, "precise conversion requested, can be done")]
//        [InlineData((double)-2.0, true, -2, false, "precise conversion requested, can be done")]
//        [InlineData((double)2.1, true, 2, true, "precise conversion requested, cannot be done because of decimal part")]
//        [InlineData((double)-2.1, true, -2, true, "precise conversion requested, cannot be done because of decimal part")]
//        // ****
//        // Conversion of long:
//        // ****
//        [InlineData((long)2, false, 2)]
//        [InlineData((long)-2, false, -2)]
//        // Converson of long, OUT OF RANGE:
//        [InlineData((long)int.MaxValue + 2, false, 0, true, "greater than max int")]
//        [InlineData((long)int.MinValue - 2, false, 0, true, "less than min int")]
//        // ****
//        // Conversion of unsigned int:
//        // ****
//        [InlineData((uint)2, false, 2)]
//        // Converson of unsigned int, OUT OF RANGE:
//        [InlineData((uint)int.MaxValue + 2, false, 0, true, "greater than max int")]
//        // Conversion of bool:
//        // ****
//        [InlineData((bool)true, false, 1)]
//        [InlineData((bool)false, false, 0)]
//        // ****
//        // Conversion of char:
//        // ****
//        [InlineData((char)'x', false, (int)'x', false, "conversion of ASCII character to int")]
//        [InlineData((char)2836, false, 2836, false, "conversion of non-ASCII character to int")]
//        [Obsolete]
//        protected void ToInt_WorksCorrectly(object converted, bool precise, int expectedResult,
//            bool failureExpected = false, string comment = null)
//        {
//            ToIntTestCommon(true, converted, precise, expectedResult, failureExpected, comment);
//        }


//        [Theory]
//        // ****
//        // Conversion of string:
//        // ****
//        [InlineData("2", false, 2)]
//        [InlineData("-2", false, -2)]
//        [InlineData("25_000", false, 25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
//        [InlineData("-25_000", false, -25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
//        [InlineData("25,000", false, 25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
//        [InlineData("-25,000", false, -25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
//        // String presentations are floating point instead of integers, conversion throws exception:
//        [InlineData("2.0", false, 2, true, "floating point, not an int")]
//        [InlineData("-2.0", false, -2, true, "floating point, not an int")]
//        // Converson of string, OUT OF RANGE:
//        [InlineData("2147483649", false, 0, true, "greater than max int")]
//        [InlineData("-2147483650", false, 0, true, "less than min int")]
//        // String representations are hexadecimal numbers - not supported:
//        [InlineData("0x1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
//        [InlineData("1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
//        // ****
//        // Conversion of double:
//        // ****
//        [InlineData((double)2.0, false, 2)]
//        [InlineData((double)-2.0, false, -2)]
//        // Conversion of double, inprecise - ROUNDED:
//        [InlineData((double)2.4999, false, 2, false, "should be rounded below (precise not requested)")]
//        [InlineData((double)2.5001, false, 3, false, "should be rounded above (precise not requested)")]
//        [InlineData((double)-2.4999, false, -2, false, "should be rounded above (precise not requested)")]
//        [InlineData((double)-2.5001, false, -3, false, "should be rounded below (precise not requested)")]
//        // Converson of double, OUT OF RANGE:
//        [InlineData((double)int.MaxValue + 2.0, false, 0, true, "greater than max int")]
//        [InlineData((double)int.MinValue - 2.0, false, 0, true, "less than min int")]
//        // Conversion of double, precise conversion required:
//        [InlineData((double)2.0, true, 2, false, "precise conversion requested, can be done")]
//        [InlineData((double)-2.0, true, -2, false, "precise conversion requested, can be done")]
//        [InlineData((double)2.1, true, 2, true, "precise conversion requested, cannot be done because of decimal part")]
//        [InlineData((double)-2.1, true, -2, true, "precise conversion requested, cannot be done because of decimal part")]
//        // ****
//        // Conversion of long:
//        // ****
//        [InlineData((long)2, false, 2)]
//        [InlineData((long)-2, false, -2)]
//        // Converson of long, OUT OF RANGE:
//        [InlineData((long)int.MaxValue + 2, false, 0, true, "greater than max int")]
//        [InlineData((long)int.MinValue - 2, false, 0, true, "less than min int")]
//        // ****
//        // Conversion of unsigned int:
//        // ****
//        [InlineData((uint)2, false, 2)]
//        // Converson of unsigned int, OUT OF RANGE:
//        [InlineData((uint)int.MaxValue + 2, false, 0, true, "greater than max int")]
//        // Conversion of bool:
//        // ****
//        [InlineData((bool)true, false, 1)]
//        [InlineData((bool)false, false, 0)]
//        // ****
//        // Conversion of char:
//        // ****
//        [InlineData((char)'x', false, (int)'x', false, "conversion of ASCII character to int")]
//        [InlineData((char)2836, false, 2836, false, "conversion of non-ASCII character to int")]
//        [Obsolete]
//        protected void IsConvertibleToInt_WorksCorrectly(object converted, bool precise, int expectedResult,
//            bool failureExpected = false, string comment = null)
//        {
//            ToIntTestCommon(false, converted, precise, expectedResult, failureExpected, comment);
//        }

//        /// <summary>Common method for testing both the method for conversion to int (<see cref="UtilTypesSingle.ToInt(object, bool, IFormatProvider)"/>) 
//        /// and the method that tells whether an object can be converted to int (<see cref="UtilTypesSingle.IsConvertibleToInt(object, bool, IFormatProvider)"/>).
//        /// Parameter <paramref name="testConversion"/> specifies whether the conversion method (when true) 
//        /// or the method that verifies whether conversion is possible is tested.</summary>
//        /// <param name="testConversion">Tf true then the conversion method is tested (<see cref="UtilTypesSingle.ToInt(object, bool, IFormatProvider)),
//        /// while if false, the method that tels whether the object can be converted is tested.</param>
//        /// <param name="converted">The object that is to be converted.</param>
//        /// <param name="precise">Whether conversion should be precise. For exemple, when true, and double is converted to int,
//        /// conversion can succeed only when the double number is an integer (does not have the decimal part).</param>
//        /// <param name="expectedResult">The expected result of conversion.</param>
//        /// <param name="failureExpected">Whether the conversion should fail.</param>
//        /// <param name="comment">Optional comment, which will be output and helps in checking and interpreting the results.</param>
//        [Obsolete]
//        protected void ToIntTestCommon(bool testConversion, object converted, bool precise, int expectedResult,
//            bool failureExpected = false, string comment = null)
//        {
//            if (testConversion)
//            {
//                Console.WriteLine($"Testing conversion of an object to type int:");
//            } else
//            {
//                Console.WriteLine($"Testing the method telling whether an object can be converted to int:");
//            }
//            Console.WriteLine($"Object to be converted: {converted}, type: {converted?.GetType().Name}");
//            Console.WriteLine($"Precise conversion required: {(precise?"yes": "no")}.");
//            Console.WriteLine("");
//            if (failureExpected)
//            {
//                Console.WriteLine("Conversion is expected to fail.");
//            }
//            else
//            {
//                Console.WriteLine($"Expected result of conversion: {expectedResult}");
//            }
//            if (!string.IsNullOrEmpty(comment))
//            {
//                Console.WriteLine($"Additional comment: \n  {comment}");
//            }
//            Console.WriteLine("");
//            // Arrange:
//            // Act:
//            bool exceptionThrown = false;
//            Console.WriteLine("Result of conversion:");
//            try
//            {
//                int result = UtilTypesSingle.ToInt(converted, precise: precise);
//                Console.WriteLine($"Result of conversion: {result}, expected: {expectedResult}");
//                if (testConversion)
//                {
//                    result.Should().Be(expectedResult);
//                }
//            }
//            catch(Exception ex)
//            {
//                exceptionThrown = true;
//                Console.WriteLine($"{ex.GetType().Name} thrown: {ex.Message}");
//            }
//            if (testConversion)
//            {
//                if (failureExpected)
//                {
//                    exceptionThrown.Should().BeTrue(because: "This conversion attempt is expected to throw an exception.");
//                }
//                else
//                {
//                    exceptionThrown.Should().BeFalse(because: "This conversion should be performed without exceptions.");
//                }
//            }
//            bool isConvertible = false;
//            bool isConvertibleThrewException = false;
//            try
//            {
//                isConvertible = UtilTypesSingle.IsConvertibleToInt(converted, precise: precise);
//                if (!testConversion)
//                {
//                    isConvertible.Should().Be(!failureExpected, because: "Result of IsConvertible should be as expected.");
//                }
//            }
//            catch (Exception)
//            {
//                isConvertibleThrewException = true;
//            }
//            if (!testConversion)
//            {
//                isConvertibleThrewException.Should().BeFalse(because: "IsConvertible should not throw an exception in any case.");
//            }
//        }


//        #endregion ToInt_Tests









//    }

//}

