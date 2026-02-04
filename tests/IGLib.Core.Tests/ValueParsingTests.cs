
// #nullable disable

using FluentAssertions;
using IGLib;
using IGLib.Commands.Tests;
using IGLib.Tests.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static IGLib.ParsingUtils;

namespace IGLib.Tests
{

    /// <summary>Tests the generic TryParse method (implemented in <see cref="ParsingUtils.TryParse{BasicType}(string, out BasicType, IFormatProvider?)"/>).</summary>
    public class ParsingValuesTests : TestBase<ParsingValuesTests>
    {

        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> and <see cref="TestBase{TestClassType}.Console"/> properties.</summary>
        /// <param name=""></param>
        public ParsingValuesTests(ITestOutputHelper output) : base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output and Console property.
        }


        IFormatProvider GetFormatProvider(string? cultureKey = null)
        {
            return cultureKey switch
            {
                null => CultureInfo.InvariantCulture,
                "" => CultureInfo.InvariantCulture,
                "Invariant" => CultureInfo.InvariantCulture,
                "Current" => CultureInfo.CurrentCulture,
                "CurrentUI" => CultureInfo.CurrentUICulture,
                _ => new CultureInfo(cultureKey!)
            };
        }


        #region GenericTryParseTests


        /// <summary>Base method for execution of generic tests for parsing values of diffeerent types from strings.</summary>
        /// <typeparam name="ValueType">The type of the values that are parsed from strings in the current test using this method.</typeparam>
        /// <param name="parsedString">String from which a number or other simple type is parsed.</param>
        /// <param name="expectedSuccess">Whether parsing should succeed with the specified input string <paramref name="parsedString"/>.</param>
        /// <param name="expectedResult">The expected result (has no meaning when <paramref name="expectedSuccess"/> is false).</param>
        /// <param name="cultureKey">Optional key specifying the culture to be used for parsing. Default is null, which maps ro
        /// <see cref="CultureInfo.InvariantCulture"/>. The <see cref="GetFormatProvider(string?)"/> method is used to map
        /// the key to <see cref="CultureInfo"/> and thus <see cref="IFormatProvider"/>.</param>
        /// <param name="skipValueVerification">Whether verification of parsed value is skipped even when <paramref name="expectedSuccess"/> 
        /// is true. This is used in some cases where the correct expected value could not be provided, e.g. due to limitations in the
        /// XUnit framework, where for example in DateeTime and similar tests the DateTime value cannot be provided as test parameter
        /// via the <see cref="InlineDataAttribute"/> (canot form a constatnt <see cref="DateTime"/> value).</param>
        protected void TryParse_WorksCorrectly_Base<ValueType>(string? parsedString,
            bool expectedSuccess, ValueType expectedResult, string? cultureKey, bool skipValueVerification = false)
            where ValueType : struct
        {
            // Arrange:
            IFormatProvider formatProvider = GetFormatProvider(cultureKey);
            Console.WriteLine($"Testing the generic TryParse method for type {typeof(ValueType).Name}.");
            Console.WriteLine($"  Parsing string:   '{parsedString}'");
            Console.WriteLine($"  Should be parsed: {expectedSuccess}");
            Console.WriteLine($"  Using format provider: `{formatProvider}`");  // no need to add: (CultureInfo: '{(formatProvider as CultureInfo)?.Name}')
            if (expectedSuccess && !skipValueVerification)
            {
                Console.WriteLine($"  Expected result: {expectedResult}");
            } else if (skipValueVerification)
            {
                Console.WriteLine("  Value verificattion will be skipped (correct expected value not provided).");
            }
            // Act:
            ValueType parseResult;
            bool wasParsed = TryParse<ValueType>(parsedString!, out parseResult, formatProvider);
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
            if (expectedSuccess && !skipValueVerification)
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
        protected void TryParseGeneric_OfBool_WorksCorrectly(string? parsedString,
            bool expectedSuccess, bool expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<bool>(parsedString, expectedSuccess, expectedResult, cultureKey);
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

        //// Overflows should produce positive or negativee infinity:
        [InlineData("2.7976931348623157E+308", true, double.PositiveInfinity)]
        [InlineData("1.7976931348623157E+309", true, double.PositiveInfinity)]
        [InlineData("-2.7976931348623157E+308", true, double.NegativeInfinity)]
        [InlineData("-1.7976931348623157E+309", true, double.NegativeInfinity)]

        // Things that do not work:
        // digit separators are not supported:
        [InlineData("2_825_934_521", false, 2_825_934_521)]
        [InlineData("-24_521", false, -24_521)]

        // CultureInfo specified (default is gefined by Global.DefaultFormatProvider and should be CultureInfo.InvariantCulture):
        // decimal separator (`.` vs. `,` in some cultures):
        [InlineData("3.14", true, 3.14, "en-US")]
        [InlineData("3,14", true, 3.14, "de-DE")]
        [InlineData("3,14", true, 314, "en-US")]  // WARNING: , is thousands separator in this culture, risk of errors
        [InlineData("3.14", true, 314, "de-DE")]  // WARNING: . is thousands separator in this culture, risk of errors
        [InlineData("3.14", true, 3.14, "Invariant")]
        // thousands separator (`,` vs. `.` in some cultures):
        [InlineData("1,234.5", true, 1234.5, "en-US")]
        [InlineData("1.234,5", true, 1234.5, "de-DE")]
        // thousand separator after decimal separator creates parsing error:
        [InlineData("1,234.5", false, 0.0, "de-DE")]  // WARNING: . is thousands separator in this culture, risk of errors
        [InlineData("1.234,5", false, 0.0, "en-US")]  // WARNING: , is thousands separator in this culture, risk of errors
        protected void TryParseGeneric_OfDouble_WorksCorrectly(string? parsedString,
            bool expectedSuccess, double expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<double>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING FLOAT:

        [Theory]
        [InlineData("3.14", true, 3.14f)]
        [InlineData("1e3", true, 1000f)]
        [InlineData("-1e-3", true, -0.001f)]
        [InlineData("3_14", false, 0f)]
        protected void TryParseGeneric_OfFloat_WorksCorrectly(string? parsedString,
            bool expectedSuccess, float expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<float>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING DECIMAL:

        [Theory]

        // Ovrflow:
        [InlineData("1e28", true, 1e28, null)]
        [InlineData("1e29", false, 0.0, null)] // decimal overflow
        //[InlineData("79228162514264337593543950335", true, decimal.MaxValue)]
        //[InlineData("-79228162514264337593543950335", true, decimal.MinValue)]
        [InlineData("1e100", false, 0)] // exponent overflow for decimal

        // Things that do not work:
        [InlineData("1_234.56", false, 0.0, null)] // digit separators are not supported

        // Cultre specified:
        [InlineData("1234.56", true, 1234.56, "en-US")]
        [InlineData("1234,56", true, 1234.56, "de-DE")]

        [InlineData("1,234.56", true, 1234.56, "en-US")]
        [InlineData("1.234,56", true, 1234.56, "de-DE")]
        protected void TryParseGeneric_OfDecimal_WithCulture_WorksCorrectly(string? parsedString,
            bool expectedSuccess, decimal expectedResult, string? cultureKey = null)
        {
            // var x = 79228162514264337593543950335m;
            TryParse_WorksCorrectly_Base(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING BYTE:

        [Theory]
        [InlineData("0", true, (byte)0)]
        [InlineData("255", true, (byte)255)]
        [InlineData(" 128 ", true, (byte)128)]
        [InlineData("-1", false, (byte)0)]          // underflow
        [InlineData("256", false, (byte)0)]         // overflow
        [InlineData("1,000", false, (byte)0)]       // thousands not allowed for byte
        [InlineData("1_0", false, (byte)0)]
        [InlineData(null, false, (byte)0)]
        [InlineData("", false, (byte)0)]
        protected void TryParseGeneric_OfByte_WorksCorrectly(string? parsedString,
            bool expectedSuccess, byte expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<byte>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING SBYTE:

        [Theory]
        [InlineData("-128", true, (sbyte)-128)]
        [InlineData("127", true, (sbyte)127)]
        [InlineData(" -5 ", true, (sbyte)-5)]
        [InlineData("-129", false, (sbyte)0)]
        [InlineData("128", false, (sbyte)0)]
        [InlineData("1,000", false, (sbyte)0)]
        protected void TryParseGeneric_OfSByte_WorksCorrectly(string? parsedString,
            bool expectedSuccess, sbyte expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<sbyte>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING SHORT:

        [Theory]
        [InlineData("-32,768", true, (short)-32768)]
        [InlineData("32,767", true, (short)32767)]
        [InlineData("40,000", false, (short)0)]
        protected void TryParseGeneric_OfInt16_WorksCorrectly(string? parsedString,
            bool expectedSuccess, short expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<short>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING USHORT:

        [Theory]
        [InlineData("65,535", true, (ushort)65535)]
        [InlineData("-1", false, (ushort)0)]
        protected void TryParseGeneric_OfUInt16_WorksCorrectly(string? parsedString,
            bool expectedSuccess, ushort expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<ushort>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING INT:

        [Theory]
        [InlineData("2,147,483,647", true, int.MaxValue)]
        [InlineData("-2,147,483,648", true, int.MinValue)]
        [InlineData("2,147,483,648", false, 0)]
        protected void TryParseGeneric_OfInt32_WorksCorrectly(string? parsedString,
            bool expectedSuccess, int expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<int>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING UINT:

        [Theory]
        [InlineData("4,294,967,295", true, uint.MaxValue)]
        [InlineData("-1", false, 0u)]
        protected void TryParseGeneric_OfUInt32_WorksCorrectly(string? parsedString,
            bool expectedSuccess, uint expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<uint>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING LONG:

        [Theory]
        [InlineData("9,223,372,036,854,775,807", true, long.MaxValue)]
        [InlineData("-9,223,372,036,854,775,808", true, long.MinValue)]
        protected void TryParseGeneric_OfInt64_WorksCorrectly(string? parsedString,
            bool expectedSuccess, long expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<long>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING ULONG:

        [Theory]
        [InlineData("18,446,744,073,709,551,615", true, ulong.MaxValue)]
        [InlineData("-1", false, 0ul)]
        protected void TryParseGeneric_OfUInt64_WorksCorrectly(string? parsedString,
            bool expectedSuccess, ulong expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<ulong>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING CHAR:

        [Theory]
        [InlineData("a", true, 'a')]
        [InlineData("Z", true, 'Z')]
        [InlineData("ab", false, '\0')]
        [InlineData("", false, '\0')]
        protected void TryParseGeneric_OfChar_WorksCorrectly(string? parsedString,
            bool expectedSuccess, char expectedResult, string? cultureKey = null)
        {
            TryParse_WorksCorrectly_Base<char>(parsedString, expectedSuccess, expectedResult, cultureKey);
        }


        // PARSING DATETIME:

        ///public static DateTime { get; set; } = new DateTime(2024, 01, 01, 12, 30, 0, DateTimeKind.Utc);

        [Theory]
        [InlineData("2024-01-01", true, "Invariant", true)]
        [InlineData("2024-01-01T12:30:00Z", true, "Invariant", true)]
        [InlineData("not-a-date", false, "Invariant", true)]
        protected void TryParseGeneric_OfDateTime_WorksCorrectly(string? parsedString,
            bool expectedSuccess, string? cultureKey = null, bool skipValueVerification = false)
        {
            DateTime expectedResult = DateTime.Now; // dummy value
            TryParse_WorksCorrectly_Base<DateTime>(parsedString, expectedSuccess, expectedResult, cultureKey, skipValueVerification);
        }







        #endregion GenericTryParseTests



        #region Specific TryParseTests

        // TODO: Tests for specific methods should be added here when these methods are implemented.



        #endregion Specific TryParseTests




    }

}

