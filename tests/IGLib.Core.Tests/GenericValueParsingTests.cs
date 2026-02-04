
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


        #region GenericTryParseTests


        /// <summary>Base method for execution of generic tests for parsing values of diffeerent types from strings.</summary>
        /// <typeparam name="ValueType">The type of the values that are parsed from strings in the current test using this method.</typeparam>
        /// <param name="parsedString">String from which a number or other simple type is parsed.</param>
        /// <param name="expectedSuccess"></param>
        /// <param name="expectedResult"></param>
        protected void TryParse_WorksCorrectly_Base<ValueType>(string? parsedString, bool expectedSuccess, ValueType expectedResult)
            where ValueType : struct
        {
            // Arrange:
            Console.WriteLine($"Testing the generic TryParse method for type {typeof(ValueType).Name}.");
            Console.WriteLine($"  Parsing string:   '{parsedString}'");
            Console.WriteLine($"  Should be parsed: {expectedSuccess}");
            if (expectedSuccess)
            {
                Console.WriteLine($"  Expected result: {expectedResult}");
            }
            // Act:
            ValueType parseResult;
            bool wasParsed = TryParse<ValueType>(parsedString!, out parseResult, Global.DefaultFormatProvider);
            if (wasParsed)
            {
                Console.WriteLine($"Value was successfully parsed from input string.");
                Console.WriteLine($"  Parsed result: {parseResult}");
            }
            else
            {
                Console.WriteLine($"Value COULD NOT BE PARSED from input string.");
            }
                // Assert:
                wasParsed.Should().Be(expectedSuccess, because: $"whether the value can be parsed from input string should be: {expectedSuccess}");
            if (expectedSuccess)
            {
                parseResult.Should().Be(expectedResult, because: $"the parsed value should be: {expectedResult}");
            }
        }


        // PARING BOOLEAN:

        /// <summary>See <see cref="TryParse_WorksCorrectly_Base{ValueType}(string?, bool, ValueType)"/></summary>
        [Theory]
        // Basic results from bool.Parse:
        [InlineData("true", true, true)]
        [InlineData("false", true, false)]
        // trailing and leading spaces are ignored with "true" / "false":
        [InlineData(" true  ", true, true)]
        [InlineData("  false ", true, false)]
        [InlineData("true  ", true, true)]
        [InlineData("  false", true, false)]
        // case doesn't matter with "true" / "false":
        [InlineData("tRuE", true, true)]
        [InlineData("falSE", true, false)]
        [InlineData(" tRuE  ", true, true)]
        [InlineData("  FAlse ", true, false)]
        [InlineData("trUe  ", true, true)]
        [InlineData("  faLSe", true, false)]
        
        // Results from predefined strings that are also parsed to true or false:
        [InlineData("yes", true, true)]
        [InlineData("no", true, false)]
        [InlineData("y", true, true)]
        [InlineData("n", true, false)]
        [InlineData("1", true, true)]
        [InlineData("0", true, false)]
        // trailing and leading spaces with predefined strings:
        [InlineData("  yes", true, true)]
        [InlineData("no  ", true, false)]
        [InlineData("   yes  ", true, true)]
        [InlineData("  no   ", true, false)]
        [InlineData("  y", true, true)]
        [InlineData("n  ", true, false)]
        [InlineData("  y    ", true, true)]
        [InlineData("    n  ", true, false)]
        // case with leading predefined strings:
        [InlineData("Yes", true, true)]
        [InlineData("No", true, false)]
        [InlineData("YES", true, true)]
        [InlineData("NO", true, false)]
        [InlineData("  Yes", true, true)]
        [InlineData("NO  ", true, false)]
        [InlineData("   yES  ", true, true)]
        [InlineData("  nO   ", true, false)]
        [InlineData("  Y", true, true)]
        [InlineData("N  ", true, false)]
        [InlineData("  Y    ", true, true)]
        [InlineData("    N  ", true, false)]

        // Results from strings represention of integer values (non-zero, which map to True):
        [InlineData("2", true, true)]
        [InlineData("48943953", true, true)]
        [InlineData("-2", true, true)]         // negative long values also work
        [InlineData("-48943953", true, true)]
        [InlineData("9223372036854775807", true, true)]  // long.MaxValue works
        [InlineData("-9223372036854775808", true, true)]  // long.MinValue works
        // leading and trailing spaces are ignored with integer representations:
        [InlineData("2  ", true, true)]
        [InlineData("48943953  ", true, true)]
        [InlineData("  -2  ", true, true)]         // negative long values also work
        [InlineData("   -48943953", true, true)]

        // What is not working as integer representation parsable to bool:
        [InlineData("9223372036854775808", false, true)]  // long overflow - NOT SUPPORTED
        [InlineData("-9223372036854775809", false, true)]  // negative long overflow - NOT SUPPORTED
        [InlineData("a8f9", false, true)]  // hexadecimal representation without a prefix is NOT SUPORTED
        [InlineData("0xa8f9", false, true)]  // hexadecimal representation with 0x prefix is also NOT SUPORTED
        [InlineData("9,223,372", false, true)]  // numbers with thousand separators are NOT SUPPORTED
        [InlineData("-9,223,372", false, true)]  // negative numbers with thousand separators are NOT SUPPORTED
        // digit separators are not allowed:
        [InlineData("48_943_953", false, true)]
        [InlineData("48,943,953", false, true)]
        // null and empty string:
        [InlineData(null, false, true)]
        [InlineData("", false, true)]
        protected void TryParseGeneric_OfBool_WorksCorrectly(string? parsedString, bool expectedSuccess, bool expectedResult)
        {
            TryParse_WorksCorrectly_Base<bool>(parsedString, expectedSuccess, expectedResult);
        }


        // PARSING DOUBLE:

        /// <summary>See <see cref="TryParse_WorksCorrectly_Base{ValueType}(string?, bool, ValueType)"/></summary>
        [Theory]
        // Usual decimal forms of inputing double values:
        [InlineData("0.005452", true, 0.005452)]
        [InlineData("2.3", true, 2.3)]
        [InlineData("+2.3", true, +2.3)]
        [InlineData("-2.3", true, -2.3)]
        [InlineData("-0.005452", true, -0.005452)]
        [InlineData("-753987935.005452", true, -753987935.005452)]
        // thousands separators (commas):
        [InlineData("-753,987,935.005452", true, -753987935.005452)]
        [InlineData("27,005,452,213.24e-5", true, 27005452213.24e-5)]

        // Exponential forms of inputing double values:
        [InlineData("2e3", true, 2000)]
        [InlineData("2.3e2", true, 230)]
        [InlineData("2.5e12", true, 2.5e12)]
        [InlineData("2.5e-6", true, 2.5e-6)]
        [InlineData("-2.5e12", true, -2.5e12)]
        [InlineData("-2.5e-6", true, -2.5e-6)]
        // capitalized exponentisl symbol:
        [InlineData("2E3", true, 2000)]
        [InlineData("2.3E2", true, 230)]
        [InlineData("2.5E-6", true, 2.5e-6)]
        [InlineData("-2.5E-6", true, -2.5e-6)]

        // Integer forms:
        [InlineData("24", true, 24)]
        [InlineData("-5385", true, -5385)]
        [InlineData("+5385", true, 5385)]
        // thousands separators (commas):
        [InlineData("2,347", true, 2347)]
        [InlineData("-53,858,917", true, -53858917)]
        [InlineData("+53,858,917", true, 53858917)]
        // use of thousand separators with different grouping is allowed:
        [InlineData("-2,87,89", true, -28789)]
        [InlineData("+2,87,89", true, 28789)]
        
        //// Overflows produce positive or negativee infinity:
        //[InlineData("2.7976931348623157E+308", false, 0)]
        //[InlineData("1.7976931348623157E+309", false, 0)]
        //[InlineData("-2.7976931348623157E+308", false, 0)]
        //[InlineData("-1.7976931348623157E+309", false, 0)]

        // Things that do not work:
        // digit separators are not supported:
        [InlineData("2_825_934_521", false, 2_825_934_521)]
        [InlineData("-24_521", false, -24_521)]
        protected void TryParseGeneric_OfDouble_WorksCorrectly(string? parsedString, bool expectedSuccess, double expectedResult)
        {
            TryParse_WorksCorrectly_Base<double>(parsedString, expectedSuccess, expectedResult);
        }


        #endregion GenericTryParseTests



        #region Specific TryParseTests

        // TODO: Tests for specific methods should be added here when these methods are implemented.



        #endregion Specific TryParseTests




    }

}

