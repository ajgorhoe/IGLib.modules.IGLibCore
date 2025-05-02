
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;

using static IGLib.Tests.Base.UtilSpeedTesting;

namespace IGLib.Tests.Base
{

    /// <summary><para>This test class provides various speed tests. These can be used to test speed of a 
    /// computer for certain kind of operations (integer, floating point, memory allocation, etc.) under
    /// specific conditions (single / multi-threaded computation, etc.)</para>
    /// <para>This test class inherits from <see cref="TestBase{ExampleTestClass}"/>, from which it inherits
    /// properties  <see cref="TestBase{TestClass}.Output"/> and its alias <see cref="TestBase{TestClass}.Console"/> 
    /// of type <see cref="ITestOutputHelper"/>, which can be used to write on test's output. Only the 
    /// simple <see cref="ITestOutputHelper.WriteLine(string)"/> and <see cref="ITestOutputHelper.WriteLine(string, 
    /// object[])"/> methods are available for writing to test Output.</para>
    /// </summary>
    public class SpeedTests : TestBase<ExampleTestClass>
    {
        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> property.</summary>
        /// <param name=""></param>
        public SpeedTests(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output property.
        }

        #region TestUtilSpeedTestingTeste

        // Tests correctness of speed testing utilities.

        /// <summary>Verifies that finite arithmetic series calculaed numerically matches the result
        /// calculatec by analytic formula.</summary>
        /// <param name="n">Number of elements of the sum.</param>
        /// <param name="a">The first element of the sum.</param>
        /// <param name="d">The constant difference between the next element of the sequence and the previous element.</param>
        /// <param name="tolerance">The tolerance, allowed discrepancy between both results to account fo rounding errors.</param>
        [Theory]
        [InlineData(2, 10, 1, 0)]
        [InlineData(3, 2, 1, 0)]
        [InlineData(200, 54, 18, 0)]
        [InlineData(2, 1.5, 0.6, 1e-10)]
        public void UtilSpeedTesting_NumericArithmeticSeriesMatchesAnalytic(int n, int a, int d, double tolerance)
        {
            Console.WriteLine($"Testing that numerical finite arithmetic series gives the same results as the analytical calculation.");
            Console.WriteLine($"Parameters:");
            Console.WriteLine($"  n:   {n}         (number of elements)");
            Console.WriteLine($"  a:   {a}         (the first element)");
            Console.WriteLine($"  d:   {d}         (difference between successive elements)");
            Console.WriteLine($"  tol: {tolerance}     (tolerance (max. allowed discrepany))");
            double analyticalResult = UtilSpeedTesting.ArithmeticSeriesAnalytical(n, a, d);
            Console.WriteLine($"Analytical result: S = {analyticalResult}");
            double numericalResult = UtilSpeedTesting.ArithmeticSeriesNumerical(n, a, d);
            Console.WriteLine($"Numerical result:  S = {numericalResult}");
            Console.WriteLine($"  Difference: {numericalResult - analyticalResult}, tolerance: {tolerance}");
            numericalResult.Should().BeApproximately(analyticalResult, tolerance);
        }


        #endregion TestUtilSpeedTesting



    }

}

