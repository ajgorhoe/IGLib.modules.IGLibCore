
// #nullable disable

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using IGLib.Tests.Base;
using IGLib.Commands.Tests;
using System.Text;
using System.Linq;

using IGLib;

using static IGLib.BaseUtils;

namespace IGLib.Tests
{

    /// <summary>Tests the generic TryParse method (implemented in <see cref="BaseUtils.TryParse{BasicType}(string, out BasicType, IFormatProvider?)"/>).</summary>
    public class TryParseGenericTests : TestBase<TryParseGenericTests>
    {

        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> and <see cref="TestBase{TestClassType}.Console"/> properties.</summary>
        /// <param name=""></param>
        public TryParseGenericTests(ITestOutputHelper output) : base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output and Console property.
        }


        /// <summary>Base generic method for execution of tests for parsing values of diffeerent types from strings.</summary>
        /// <typeparam name="ValueType">The type of the values that are parsed from strings in the current test using this method.</typeparam>
        /// <param name="parsedString"></param>
        /// <param name="shouldBeParsed"></param>
        /// <param name="expectedResult"></param>
        protected void TryParse_WorksCorrectly_Base<ValueType>(string? parsedString, bool shouldBeParsed, ValueType expectedResult)
            where ValueType : struct
        {
            // Arrange:
            Console.WriteLine($"Testing the generic TryParse method for type {typeof(ValueType).Name}.");
            Console.WriteLine($"  Parsing string:   '{parsedString}'");
            Console.WriteLine($"  Should be parsed: {shouldBeParsed}");
            if (shouldBeParsed)
            {
                Console.WriteLine($"  Expected result: {expectedResult}");
            }
            Console.WriteLine($"  Should be parsed: {shouldBeParsed}");
            // Act:
            ValueType parseResult;
            bool wasParsed = TryParse<ValueType>(parsedString!, out parseResult, Global.DefaultFormatProvider);
            // Assert:
            wasParsed.Should().Be(shouldBeParsed, because: $"whether the value can be parsed from input string should be: {shouldBeParsed}");
            if (shouldBeParsed)
            {
                parseResult.Should().Be(expectedResult, because: $"the parsed value should be: {expectedResult}");
            }
        }


        [Theory]
        // Basic results from bool.Parse:
        [InlineData("true", true, true)]
        [InlineData("false", true, false)]
        // trailing and leading spaces are ignored:
        [InlineData(" true  ", true, true)]
        [InlineData("  false ", true, false)]
        [InlineData("true  ", true, true)]
        [InlineData("  false", true, false)]
        // results from predefined strings that can mean true or false:
        [InlineData("yes", true, true)]
        [InlineData("no", true, false)]
        [InlineData("y", true, true)]
        [InlineData("n", true, false)]
        [InlineData("1", true, true)]
        [InlineData("0", true, false)]
        // Results from strings represention of integer values (non-zero, which map to True):
        [InlineData("2", true, true)]
        [InlineData("48943953", true, true)]
        [InlineData("-2", true, true)]         // negative long values also work
        [InlineData("-48943953", true, true)]
        [InlineData("9223372036854775807", true, true)]  // long.MaxValue works
        [InlineData("-9223372036854775808", true, true)]  // long.MinValue works
        // what is not working as integer representation parsable to bool:
        [InlineData("9223372036854775808", false, true)]  // long overflow - NOT SUPPORTED
        [InlineData("-9223372036854775809", false, true)]  // negative long overflow - NOT SUPPORTED
        [InlineData("a8f9", false, true)]  // hexadecimal representation without a prefix is NOT SUPORTED
        [InlineData("0xa8f9", false, true)]  // hexadecimal representation with 0x prefix is also NOT SUPORTED
        [InlineData("9,223,372", false, true)]  // numbers with thousand separators are NOT SUPPORTED
        [InlineData("-9,223,372", false, true)]  // negative numbers with thousand separators are NOT SUPPORTED
        // null and empty string:
        [InlineData(null, false, true)]
        [InlineData("", false, true)]
        protected void TryParse_OfBool_WorksCorrectly(string? parsedString, bool shouldBeParsed, bool expectedResult)
        {
            TryParse_WorksCorrectly_Base<bool>(parsedString, shouldBeParsed, expectedResult);
        }


        protected void TryParse_OfBool_WorksCorrectly_AvoidGeneric(string parsedString, bool shouldBeParsed, bool expectedResult)
        {
            // Arrange:
            Console.WriteLine($"Testing the generic TryParse method for type {expectedResult.GetType().Name}.");
            Console.WriteLine($"  Parsing string:   '{parsedString}'");
            Console.WriteLine($"  Should be parsed: {shouldBeParsed}");
            if (shouldBeParsed)
            {
                Console.WriteLine($"  Expected result: {expectedResult}");
            }
            Console.WriteLine($"  Should be parsed: {shouldBeParsed}");
            // Act:
            bool parseResult;
            bool wasParsed = TryParse<bool>(parsedString, out parseResult, Global.DefaultFormatProvider);
            // Assert:
            wasParsed.Should().Be(shouldBeParsed, because: $"whether the value can be parsed from input string should be: {shouldBeParsed}");
            if (shouldBeParsed)
            {
                parseResult.Should().Be(expectedResult, because: $"the parsed value should be: {expectedResult}");
            }
        }



    }

}

