using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;
using IGLib.Tests.Base;

namespace IGLib.Commands.Tests
{

    /// <summary><para>This test class just serves as example of how to create unit tests.</para>
    /// <para>Tests can be run either by right-clicking the test class or test method name and selecting
    /// "Run Tests", or opening the Visual Studio's Test Explorer and running tests from there (don't 
    /// forget to build the code beforehand).</para>
    /// <para>This test class inherits from <see cref="TestBase{ExampleTestClass}"/>, from which it inherits
    /// property  <see cref="TestBase{TestClass}.Output"/> of type <see cref="ITestOutputHelper"/>, which can 
    /// be used to write on test's output. Only the simple <see cref="ITestOutputHelper.WriteLine(string)"/>
    /// and <see cref="ITestOutputHelper.WriteLine(string, object[])"/> methods are available for writing 
    /// to test Output. In the Test Explorer in Visual Studio you can execute the tests and you can very
    /// nicely see which tests fail, but sometimes it is beneficial to write additional output before the
    /// assertions are made, such that you can efficiently trace causes of errors.</para></summary>
    public class SampleTests : TestBase<SampleTests>
    {
        
        
        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> property.</summary>
        /// <param name=""></param>
        public SampleTests(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output and Console property.
        }


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



    }

}

