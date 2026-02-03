
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

        [Theory]
        // basic results from bool.Parse:
        [InlineData("true", true, true)]
        [InlineData("false", true, false)]
        // results from predefined strings that can mean true or false:
        [InlineData("yes", true, true)]
        [InlineData("no", true, false)]
        [InlineData("y", true, true)]
        [InlineData("n", true, false)]
        [InlineData("1", true, true)]
        [InlineData("0", true, false)]
        // results from strings representinf integer values (non-zero, which map to True):
        [InlineData("2", true, true)]
        [InlineData("15", true, true)]
        [InlineData("a8f9", false, true)]  // hexadecimal representation not supported
        [InlineData("0xa8f9", true, true)]  // hexadecimal representation not supported
        
        public void TryParse_Bool_WorksCorrectly(string parsedString, bool shouldBeParsed, bool expectedResult)
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

