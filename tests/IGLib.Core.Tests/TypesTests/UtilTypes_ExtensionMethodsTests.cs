
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



        #region GeneralConversionExtensionMethods_Generic_Tests


        /// <summary>Common base method for testing both the method for conversion to another basic type (<see cref="UtilTypes.ConvertTo{TargetType}(object?, bool, IFormatProvider?)"/>)
        /// and the method that tells whether an object can be converted to another basic type (<see cref="UtilTypes.IsConvertibleTo{TargetType}(object?, bool, IFormatProvider?)"/>).
        /// Parameter <paramref name="testConversion"/> specifies whether the conversion method (when true), 
        /// or the method that verifies whether conversion is possible, is tested (<see cref="UtilTypes.IsConvertibleTo{TargetType}(object?, bool, IFormatProvider?)"/>).</summary>
        /// <param name="testConversion">Tf true then the conversion method is tested (<see cref="UtilTypes.ConvertTo{TargetType}(object?, bool, IFormatProvider?)"/>),
        /// while if false, the method that tels whether the object can be converted to target type is tested.</param>
        /// <param name="converted">The object that is to be converted, or for which we query whether the object can be 
        /// converted to targat type.</param>
        /// <param name="precise">Whether conversion should be precise. For exemple, when true, and double is converted to int,
        /// conversion can succeed only when the double number is an integer (does not have the decimal part).</param>
        /// <param name="expectedResult">The expected result of conversion.</param>
        /// <param name="failureExpected">Whether the conversion should fail.</param>
        /// <param name="comment">Optional comment, which will be output and helps in checking and interpreting the results.</param>
        protected void ConvertToTypeTestsCommon<TargetType>(bool testConversion, object converted, bool precise, int expectedResult,
            bool failureExpected = false, string comment = null)
            where TargetType : IConvertible
        {
            if (testConversion)
            {
                Console.WriteLine($"Testing conversion of an object to type int:");
            }
            else
            {
                Console.WriteLine($"Testing the method telling whether an object can be converted to int:");
            }
            Console.WriteLine($"Object to be converted: {converted}, type: {converted?.GetType().Name}");
            Console.WriteLine($"Precise conversion required: {(precise ? "yes" : "no")}.");
            Console.WriteLine("");
            if (failureExpected)
            {
                Console.WriteLine("Conversion is expected to fail.");
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
                TargetType result = converted.ConvertTo<TargetType>(precise: precise);
                Console.WriteLine($"Result of conversion: {result}, expected: {expectedResult}");
                if (testConversion)
                {
                    result.Should().Be(expectedResult);
                }
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Console.WriteLine($"{ex.GetType().Name} thrown: {ex.Message}");
            }
            if (testConversion)
            {
                if (failureExpected)
                {
                    exceptionThrown.Should().BeTrue(because: "this conversion attempt is expected to throw an exception");
                }
                else
                {
                    exceptionThrown.Should().BeFalse(because: "this conversion should be performed without exceptions");
                }
            }
            if (!testConversion)
            {

                bool isConvertible = false;
                bool isConvertibleThrewException = false;
                try
                {
                    isConvertible = converted.IsConvertibleTo<TargetType>(precise: precise);
                    Console.WriteLine($"{nameof(UtilTypes.IsConvertibleTo)} returned {isConvertible}.");
                    if (!testConversion)
                    {
                        isConvertible.Should().Be(!failureExpected, because: $"tesult of {nameof(UtilTypes.IsConvertibleTo)} is expected to be {!failureExpected}");
                    }
                }
                catch (Exception)
                {
                    isConvertibleThrewException = true;
                }
                if (!testConversion)
                {
                    isConvertibleThrewException.Should().BeFalse(because: "IsConvertible should not throw an exception in any case");
                }
            }
        }



        // REMARK: Do these tests ONLY FOR target type int!
        // Doing these tests for different types does not bring added value because we just test that
        // the static methods can also be used as extension methods (and the effect is really the same),
        // which we can do on a smaller set of test, and the static methods are already tested
        // extensively in UtilTypesTests.
        // REMARK: parameter sets can be the same as in UtilTypesTests (let's just test extensively for
        // one target type).


        #region GeneralConversionExtensionMethods_Generic_Tests__For_Int


        /// <summary>Tests whether <see cref="UtilTypes.ConvertTo{TargetType}(object?, bool, IFormatProvider?)"/> 
        /// works correctly for generic target type <see cref="int"/>.</summary>
        /// <param name="converted">Object that is converted to target type.</param>
        /// <param name="precise">Whether conversion should be precise (in this case, conversion fails for values that
        /// cannot be converted precisely, e.g. double to int where the original double has nonzero decimal part).</param>
        /// <param name="expectedResult">Expected result of conversion, agains which assertions are made.</param>
        /// <param name="failureExpected">Whether conversion should fail for the current input data.</param>
        /// <param name="comment">Optional comment (written to output for user inforrmation).</param>
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
        // String representations are hexadecimal numbers - not supported:
        [InlineData("0x1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        [InlineData("1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
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
        protected void ConvertTo_Generic_WorksCorrectlyFor_Int(object converted, bool precise, int expectedResult,
            bool failureExpected = false, string comment = null)
        {
            ConvertToTypeTestsCommon<int>(true, converted, precise, expectedResult, failureExpected, comment);
        }


        /// <summary>Tests whether <see cref="UtilTypes.IsConvertibleTo{TargetType}(object?, bool, IFormatProvider?)"/> 
        /// works correctly for generic target type <see cref="int"/>.</summary>
        /// <param name="converted">Object that is converted to target type.</param>
        /// <param name="precise">Whether conversion should be precise (in this case, conversion fails for values that
        /// cannot be converted precisely, e.g. double to int where the original double has nonzero decimal part).</param>
        /// <param name="expectedResult">Expected result of conversion, agains which assertions are made.</param>
        /// <param name="failureExpected">Whether conversion should fail for the current input data.</param>
        /// <param name="comment">Optional comment (written to output for user inforrmation).</param>
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
        // String representations are hexadecimal numbers - not supported:
        [InlineData("0x1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        [InlineData("1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
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
        protected void IsConvertibleTo_Generic_WorksCorrectlyFor_Int(object converted, bool precise, int expectedResult,
            bool failureExpected = false, string comment = null)
        {
            ConvertToTypeTestsCommon<int>(false, converted, precise, expectedResult, failureExpected, comment);
        }


        #endregion GeneralConversionExtensionMethods_Generic_Tests__For_Int


        #endregion GeneralConversionExtensionMethods_Generic_Tests





        #region GeneralConversion_Tests

        // Tests nongeneric general conversion methods
        // Only test for one type (int) because more extensive testing should be done for
        // generic methods (which also test non-generic ones because generic methods call
        // nongeneric ones to do the job)


        /// <summary>Common base method for testing both the method for conversion to another basic type (<see cref="UtilTypes.ConvertToType(object?, Type, bool, IFormatProvider?)"/>)
        /// and the method that tells whether an object can be converted to another basic type (<see cref="UtilTypes.IsConvertibleToType(object?, Type, bool, IFormatProvider?)"/>).
        /// Parameter <paramref name="testConversion"/> specifies whether the conversion method (when true), 
        /// or the method that verifies whether conversion is possible, is tested (<see cref="UtilTypes.IsConvertibleToType(object?, Type, bool, IFormatProvider?)"/>).</summary>
        /// <param name="testConversion">Tf true then the conversion method is tested (<see cref="UtilTypes.ConvertToType(object?, Type, bool, IFormatProvider?)"/>),
        /// while if false, the method that tels whether the object can be converted to target type is tested.</param>
        /// <param name="targetType">The required target type after conversion.</param>
        /// <param name="converted">The object that is to be converted, or for which we query whether the object can be 
        /// converted to targat type.</param>
        /// <param name="precise">Whether conversion should be precise. For exemple, when true, and double is converted to int,
        /// conversion can succeed only when the double number is an integer (does not have the decimal part).</param>
        /// <param name="expectedResult">The expected result of conversion.</param>
        /// <param name="failureExpected">Whether the conversion should fail.</param>
        /// <param name="comment">Optional comment, which will be output and helps in checking and interpreting the results.</param>
        protected void ConvertToTypeTestsCommon(bool testConversion, Type targetType, object converted, bool precise, int expectedResult,
            bool failureExpected = false, string comment = null)
        {
            if (testConversion)
            {
                Console.WriteLine($"Testing conversion of an object to type int:");
            }
            else
            {
                Console.WriteLine($"Testing the method telling whether an object can be converted to int:");
            }
            Console.WriteLine($"Object to be converted: {converted}, type: {converted?.GetType().Name}");
            Console.WriteLine($"Precise conversion required: {(precise ? "yes" : "no")}.");
            Console.WriteLine("");
            if (failureExpected)
            {
                Console.WriteLine("Conversion is expected to fail.");
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
                object result = converted.ConvertToType(targetType, precise: precise);
                Console.WriteLine($"Result of conversion: {result}, expected: {expectedResult}");
                if (testConversion)
                {
                    result.Should().Be(expectedResult);
                }
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Console.WriteLine($"{ex.GetType().Name} thrown: {ex.Message}");
            }
            if (testConversion)
            {
                if (failureExpected)
                {
                    exceptionThrown.Should().BeTrue(because: "this conversion attempt is expected to throw an exception");
                }
                else
                {
                    exceptionThrown.Should().BeFalse(because: "this conversion should be performed without exceptions");
                }
            }
            if (!testConversion)
            {
                bool isConvertible = false;
                bool isConvertibleThrewException = false;
                try
                {
                    // Act then Assert:
                    isConvertible = converted.IsConvertibleToType(targetType, precise: precise);
                    Console.WriteLine($"{nameof(UtilTypes.IsConvertibleTo)} returned {isConvertible}.");
                    if (!testConversion)
                    {
                        isConvertible.Should().Be(!failureExpected, because: $"tesult of {nameof(UtilTypes.IsConvertibleTo)} is expected to be {!failureExpected}");
                    }
                }
                catch (Exception)
                {
                    isConvertibleThrewException = true;
                }
                if (!testConversion)
                {
                    isConvertibleThrewException.Should().BeFalse(because: "IsConvertible should not throw an exception in any case");
                }
            }
        }


        #region GeneralConversion_Tests__For_Int


        /// <summary>Tests whether <see cref="UtilTypes.ConvertToType(object?, Type, bool, IFormatProvider?)"/> 
        /// works correctly for generic target type <see cref="int"/>.</summary>
        /// <param name="converted">Object that is converted to target type.</param>
        /// <param name="precise">Whether conversion should be precise (in this case, conversion fails for values that
        /// cannot be converted precisely, e.g. double to int where the original double has nonzero decimal part).</param>
        /// <param name="expectedResult">Expected result of conversion, agains which assertions are made.</param>
        /// <param name="failureExpected">Whether conversion should fail for the current input data.</param>
        /// <param name="comment">Optional comment (written to output for user inforrmation).</param>
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
        // String representations are hexadecimal numbers - not supported:
        [InlineData("0x1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        [InlineData("1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
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
        protected void ConvertToType_WorksCorrectlyFor_Int(object converted, bool precise, int expectedResult,
            bool failureExpected = false, string comment = null)
        {
            ConvertToTypeTestsCommon(true, typeof(int), converted, precise, expectedResult, failureExpected, comment);
        }


        /// <summary>Tests whether <see cref="UtilTypes.IsConvertibleToType(object?, Type, bool, IFormatProvider?)"/> 
        /// works correctly for generic target type <see cref="int"/>.</summary>
        /// <param name="converted">Object that is converted to target type.</param>
        /// <param name="precise">Whether conversion should be precise (in this case, conversion fails for values that
        /// cannot be converted precisely, e.g. double to int where the original double has nonzero decimal part).</param>
        /// <param name="expectedResult">Expected result of conversion, agains which assertions are made.</param>
        /// <param name="failureExpected">Whether conversion should fail for the current input data.</param>
        /// <param name="comment">Optional comment (written to output for user inforrmation).</param>
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
        // String representations are hexadecimal numbers - not supported:
        [InlineData("0x1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        [InlineData("1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
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
        protected void IsConvertibleToType_WorksCorrectlyFor_Int(object converted, bool precise, int expectedResult,
            bool failureExpected = false, string comment = null)
        {
            ConvertToTypeTestsCommon(false, typeof(int), converted, precise, expectedResult, failureExpected, comment);
        }


        #endregion GeneralConversion_Tests__For_Int


        #endregion GeneralConversion_Tests







    }

}

