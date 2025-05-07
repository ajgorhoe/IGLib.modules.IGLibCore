
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;
using System;
using System.Diagnostics;

using IGLib.Tests;
using static IGLib.Tests.UtilSpeedTesting;
using System.Runtime.CompilerServices;

namespace IGLib.Tests.Base
{

    /// <summary><para>This test class provides unit tests for speed testing utilities, especially from
    /// the <see cref="UtilSpeedTesting"/> class. These can be used to test speed of a  computer for 
    /// certain kind of operations (integer, floating point, memory allocation, etc.) under
    /// specific conditions (single / multi-threaded computation, etc.)</para>
    /// <para>This test class inherits from <see cref="TestBase{ExampleTestClass}"/>, from which it inherits
    /// properties  <see cref="TestBase{TestClass}.Output"/> and its alias <see cref="TestBase{TestClass}.Console"/> 
    /// of type <see cref="ITestOutputHelper"/>, which can be used to write on test's output. Only the 
    /// simple <see cref="ITestOutputHelper.WriteLine(string)"/> and <see cref="ITestOutputHelper.WriteLine(string, 
    /// object[])"/> methods are available for writing to test Output.</para>
    /// </summary>
    public class UtilSpeedTestingTests : TestBase<ExampleTestClass>
    {
        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> property.</summary>
        /// <param name=""></param>
        public UtilSpeedTestingTests(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output property.
        }


        #region SpeedTestInfo

        [Fact]
        void SpeedTestInfoDouble_PropertiesAreCorrect()
        {
            // Arrange
            // Creation parameters:
            string testId = ConstGeom.TestId;
            string referenceMachineId = ConstMachineHpLaptop24.MachineId;
            // Test-specific parameters:
            int numExecutions = ConstGeom.NumExecutions;
            string par1Name = "a0";
            double par1Value = ConstGeom.a0;
            string par2Name = "k";
            double par2Value = ConstGeom.k;
            // Expected results:
            double analyticalResult = ConstGeom.AnalyticResult;
            double tolerance = ConstGeom.Tolerance;
            double referenceExecutionsPerSecond = ConstGeom.RefExecutionsPerSecond_HpLaptop24;
            // results:
            Stopwatch sw = Stopwatch.StartNew();
            double result = GeometricSeriesNumerical(numExecutions, par1Value, par2Value);
            sw.Stop();
            double executionTimeSeconds = sw.Elapsed.TotalSeconds;

            // Act
            SpeedTestInfo info = new SpeedTestInfo(testId, referenceMachineId);
            // test specific parameters:
            info.NumExecutions = numExecutions;
            info.Parameters.AddRange([(par1Name, par1Value), (par2Name, par2Value)]);
            // expected results:
            info.AnalyticalResult = analyticalResult;
            info.Tolerance = tolerance;
            info.ReferenceExecutionsPerSecond = referenceExecutionsPerSecond;
            // results:
            info.Result = result;
            info.ExecutionTimeSeconds = executionTimeSeconds;
            
            // Assert
            // creation:
            info.Should().NotBeNull();
            info.TestId.Should().Be(testId);
            info.ReferenceMachineId.Should().Be(referenceMachineId);
            // test-specific parameters:
            info.NumExecutions.Should().Be(numExecutions);
            info.Parameters.Should().NotBeNull();
            info.Parameters.Count.Should().Be(2);
            var p1 = info.Parameters[0];
            p1.ParameterName.Should().Be(par1Name);
            p1.ParameterValue.Should().Be(par1Value);
            var p2 = info.Parameters[1];
            p2.ParameterName.Should().Be(par2Name);
            p2.ParameterValue.Should().Be(par2Value);
            // expected results:
            info.AnalyticalResult.Should().Be(analyticalResult);
            info.Tolerance.Should().Be(tolerance);
            info.ReferenceExecutionsPerSecond.Should().Be(referenceExecutionsPerSecond);
            // results:
            info.Exception.Should().BeNull();
            info.Result.Should().Be(result);
            info.CanCalculateDiscrepancy.Should().BeTrue();
            info.Discrepancy.Should().BeApproximately(info.Result - info.AnalyticalResult, 1e-8);
            info.ExecutionTimeSeconds.Should().Be(executionTimeSeconds);
            info.NumExecutionsPerSecond.Should().BeApproximately(
                (double)numExecutions / executionTimeSeconds, 1e-3);
            info.MegaExecutionsPerSecond.Should().BeApproximately(
                info.NumExecutionsPerSecond / 1e6, 1e-6);
            info.SpeedFactor.Should().BeApproximately(info.NumExecutionsPerSecond / referenceExecutionsPerSecond, 1e-8);
        }


        #endregion SpeedTestInfo



        #region UtilSpeedTesting_Tests

        // Tests correctness of speed testing utilities.

        /// <summary>Verifies that finite arithmetic series calculaed analytically is correct.
        /// <para>Test is based on the separate built-in analytical calculation of the expected result in
        /// this method. For double verification, the test <see cref="UtilSpeedTesting_ArithmeticSeriesNumericalMatchesAnalyticalResult"/>
        /// compares results of the method for analytical calculation of the finite arithmetic series with the method for numerical
        /// calculation, which reduces the possibility of wrong implementation to the minimum.</para>
        /// <para>The tested method will be used to chack correctness in other tests.</para></summary>
        /// <param name="n">Number of elements of the sum.</param>
        /// <param name="a0">The first element of the sum.</param>
        /// <param name="k">The constant difference between the next element of the sequence and the previous element.</param>
        /// <param name="tolerance">The tolerance, allowed discrepancy between both results to account fo rounding errors.</param>
        [Theory]
        [InlineData(2, 10, 1, 0)]
        [InlineData(3, 2, 1, 0)]
        [InlineData(2, 1.5, 0.6, 1e-8)]
        [InlineData(12, 6.35, 1.34, 1e-8)]
        [InlineData(15, 1.64e2, -21.86, 1e-8)]
        public void UtilSpeedTesting_ArithmeticSeriesAnalyticalMethodIsCorrect(int n, double a0, double d, double tolerance)
        {
            Console.WriteLine($"Testing that numerical finite geometric series gives the same results as the analytical calculation.");
            Console.WriteLine($"Parameters:");
            Console.WriteLine($"  n:   {n}         (number of elements)");
            Console.WriteLine($"  a:   {a0}         (the first element)");
            Console.WriteLine($"  d:   {d}         (difference between successive elements)");
            Console.WriteLine($"  tol: {tolerance}     (tolerance (max. allowed discrepancy))");
            double analyticalResult = UtilSpeedTesting.ArithmeticSeriesAnalytical(n, a0, d);
            Console.WriteLine($"Analytical result: S = {analyticalResult}");
            double controlResult = n * (2 * a0 + (n - 1) * d) / 2.0;
            Console.WriteLine($"Control result:  S = {controlResult}");
            Console.WriteLine($"  Difference: {controlResult - analyticalResult}, tolerance: {tolerance}");
            controlResult.Should().BeApproximately(analyticalResult, tolerance);

        }

        /// <summary>Verifies that finite arithmetic series calculaed numerically matches the result
        /// calculatec by analytic formula.</summary>
        /// <param name="n">Number of elements of the sum.</param>
        /// <param name="a0">The first element of the sum.</param>
        /// <param name="d">The constant difference between the next element of the sequence and the previous element.</param>
        /// <param name="tolerance">The tolerance, allowed discrepancy between both results to account fo rounding errors.</param>
        [Theory]
        [InlineData(2, 10, 1, 0)]
        [InlineData(3, 2, 1, 0)]
        [InlineData(200, 54, 18, 0)]
        [InlineData(2, 1.5, 0.6, 1e-8)]
        [InlineData(100, 6.35, 1.34, 1e-8)]
        [InlineData(10, -5.32, 0.06, 1e-8)]
        [InlineData(15, 1.64e2, -21.86, 1e-8)]
        [InlineData(15, -45.96, -2.312, 1e-8)]
        public void UtilSpeedTesting_ArithmeticSeriesNumericalMatchesAnalyticalResult(int n, double a0, double d, double tolerance)
        {
            Console.WriteLine($"Testing that numerical finite arithmetic series gives the same results as the analytical calculation.");
            Console.WriteLine($"Parameters:");
            Console.WriteLine($"  n:   {n}         (number of elements)");
            Console.WriteLine($"  a:   {a0}         (the first element)");
            Console.WriteLine($"  d:   {d}         (difference between successive elements)");
            Console.WriteLine($"  tol: {tolerance}     (tolerance (max. allowed discrepancy))");
            double analyticalResult = UtilSpeedTesting.ArithmeticSeriesAnalytical(n, a0, d);
            Console.WriteLine($"Analytical result: S = {analyticalResult}");
            double numericalResult = UtilSpeedTesting.ArithmeticSeriesNumerical(n, a0, d);
            Console.WriteLine($"Numerical result:  S = {numericalResult}");
            Console.WriteLine($"  Difference: {numericalResult - analyticalResult}, tolerance: {tolerance}");
            numericalResult.Should().BeApproximately(analyticalResult, tolerance);
        }


        // Geometric series:

        /// <summary>Verifies that finite geometric series calculaed analytically is correct.
        /// <para>Test is based on the separate built-in analytical calculation of the expected result by
        /// this method. For double verification, the test <see cref="UtilSpeedTesting_GeometricSeriesNumericalMatchesAnalyticalResult(int, double, double, double)"/>
        /// compares results of the method for analytical calculation of the finite arithmetic series with the method for numerical
        /// calculation, which reduces the possibility of wrong implementation.</para>
        /// <para>The tested method will be used to chack correctness in other tests.</para></summary>
        /// <param name="n">Number of elements of the sum.</param>
        /// <param name="a0">The first element of the sum.</param>
        /// <param name="d">The constant quotient between the next element of the sequence and the current element.</param>
        /// <param name="tolerance">The tolerance, allowed discrepancy between both results, to account for rounding errors.</param>
        [Theory]
        [InlineData(2, 10, 2, 0)]
        [InlineData(3, 4, 0.5, 1e-8)]
        [InlineData(100, 41.4, 0.95, 1e-8)]
        [InlineData(100, 12.3, 1.05, 1e-8)]
        [InlineData(100, 12.3, -1.05, 1e-8)]
        public void UtilSpeedTesting_GeometricSeriesAnalyticalMethodIsCorrect(int n, double a0, double k, double tolerance)
        {
            Console.WriteLine($"Testing that analytical finite arithmetic series is calculated correctly...");
            Console.WriteLine($"Parameters:");
            Console.WriteLine($"  n:   {n}         (number of elements)");
            Console.WriteLine($"  a:   {a0}         (the first element)");
            Console.WriteLine($"  k:   {k}         (difference between successive elements)");
            Console.WriteLine($"  tol: {tolerance}     (tolerance (max. allowed discrepancy))");
            double analyticalResult = UtilSpeedTesting.GeometricSeriesAnalytical(n, a0, k);
            Console.WriteLine($"Analytical result: S = {analyticalResult}");
            double controlResult;
            if (k == 1)
            {
                controlResult = a0 * n;
            }
            else
            {
                controlResult = a0 * (1 - Math.Pow(k, n)) / (1 - k);
            }
            Console.WriteLine($"Control result:  S = {controlResult}");
            Console.WriteLine($"  Difference: {controlResult - analyticalResult}, tolerance: {tolerance}");
            controlResult.Should().BeApproximately(analyticalResult, tolerance);
        }

        /// <summary>Verifies that finite geometric series calculaed numerically matches the result
        /// calculatec by an analytic formula. Numerical calculation is performed by <see cref="UtilSpeedTesting.GeometricSeriesNumerical(int, double, double)"/></summary>
        /// <param name="n">Number of elements of the sum.</param>
        /// <param name="a0">The first element of the sum.</param>
        /// <param name="k">The constant quotient between the next element of the sequence and the current element.</param>
        /// <param name="tolerance">The tolerance, allowed discrepancy between both results, to account fo rounding errors.</param>
        [Theory]
        [InlineData(2, 10, 2, 0)]
        [InlineData(3, 4, 0.5, 1e-8)]
        [InlineData(100, 41.4, 0.95, 1e-8)]
        [InlineData(100, 12.3, 1.05, 1e-8)]
        [InlineData(100, 12.3, -1.05, 1e-8)]
        [InlineData(100, 1, 1.0001, 1e-8)]
        [InlineData(100, 1, 0.999, 1e-8)]
        public void UtilSpeedTesting_GeometricSeriesNumericalMatchesAnalyticalResult(int n, double a0, double k, double tolerance)
        {
            // Arrange
            // Console.WriteLine($"Testing that numerical finite geometric series gives the same results as the analytical calculation.");
            Console.WriteLine($"Parameters:");
            Console.WriteLine($"  n:   {n}         (number of elements)");
            Console.WriteLine($"  a:   {a0}         (the first element)");
            Console.WriteLine($"  k:   {k}         (difference between successive elements)");
            Console.WriteLine($"  tol: {tolerance}     (tolerance (max. allowed discrepancy))");
            double analyticalResult = UtilSpeedTesting.GeometricSeriesAnalytical(n, a0, k);
            Console.WriteLine($"Analytical result: S = {analyticalResult}");
            // Act
            double numericalResult = UtilSpeedTesting.GeometricSeriesNumerical(n, a0, k);
            Console.WriteLine($"Numerical result:  S = {numericalResult}");
            Console.WriteLine($"  Difference: {numericalResult - analyticalResult}, tolerance: {tolerance}");
            // Assert
            numericalResult.Should().BeApproximately(analyticalResult, tolerance);
        }



        /// <summary>Verifies that finite geometric series calculaed numerically matches the result
        /// calculatec by an analytic formula. Numerical calculation is performed by <see cref="UtilSpeedTesting.GeometricSeriesNumerical(int, double, double)"/></summary>
        /// <param name="n">Number of elements of the sum.</param>
        /// <param name="a0">The first element of the sum.</param>
        /// <param name="k">The constant quotient between the next element of the sequence and the current element.</param>
        /// <param name="tolerance">The tolerance, allowed discrepancy between both results, to account fo rounding errors.</param>
        [Theory]
        [InlineData(2, 10, 2, 0)]
        [InlineData(3, 4, 0.5, 1e-8)]
        [InlineData(100, 41.4, 0.95, 1e-8)]
        [InlineData(100, 12.3, 1.05, 1e-8)]
        [InlineData(100, 12.3, -1.05, 1e-8)]
        [InlineData(100, 1, 1.0001, 1e-8)]
        [InlineData(100, 1, 0.999, 1e-8)]
        public void UtilSpeedTesting_GeometricSeriesNumerical_DirectElementCalculation_MatchesAnalyticalResult(int n, double a0, double k, double tolerance)
        {
            // Arrange
            // Console.WriteLine($"Testing that numerical finite geometric series gives the same results as the analytical calculation.");
            Console.WriteLine($"Parameters:");
            Console.WriteLine($"  n:   {n}         (number of elements)");
            Console.WriteLine($"  a:   {a0}         (the first element)");
            Console.WriteLine($"  k:   {k}         (difference between successive elements)");
            Console.WriteLine($"  tol: {tolerance}     (tolerance (max. allowed discrepancy))");
            double analyticalResult = UtilSpeedTesting.GeometricSeriesAnalytical(n, a0, k);
            Console.WriteLine($"Analytical result: S = {analyticalResult}");
            // Act
            double numericalResult = UtilSpeedTesting.GeometricSeriesNumerical_DirectElementCalculation(n, a0, k);
            Console.WriteLine($"Numerical result:  S = {numericalResult}");
            Console.WriteLine($"  Difference: {numericalResult - analyticalResult}, tolerance: {tolerance}");
            // Assert
            numericalResult.Should().BeApproximately(analyticalResult, tolerance);
        }


        #endregion UtilSpeedTesting_Tests


        #region StandardSpeedTestPreparation

        // This region contains tests that assist in finding suitable parameters for
        // standard speed tests.

        /// <summary>Performs calculation of geometric series at the same parameters as <see cref="UtilSpeedTesting.StandardSpeedTestGeometricSeriesBasic(bool)"/>,
        /// except that calculation is performed with different numbers of terms, in order to find the suitable
        /// parameter for standard tests.</summary>
        /// <param name="n">Number of elements of the sum.</param>
        /// <param name="a0">The first element of the sum.</param>
        /// <param name="k">The constant quotient between the next element of the sequence and the current element.</param>
        /// <param name="tolerance">The tolerance, allowed discrepancy between both results, to account fo rounding errors.</param>
        [Theory]
#if false
        [InlineData(1, ConstGeom.a0, ConstGeom.k, ConstGeom.Tolerance)]
        [InlineData(10, ConstGeom.a0, ConstGeom.k, ConstGeom.Tolerance)]
        [InlineData(100, ConstGeom.a0, ConstGeom.k, ConstGeom.Tolerance)]
#endif
        [InlineData(1_000, ConstGeom.a0, ConstGeom.k, ConstGeom.Tolerance)]
        [InlineData(10_000, ConstGeom.a0, ConstGeom.k, ConstGeom.Tolerance)]
        [InlineData(100_000, ConstGeom.a0, ConstGeom.k, ConstGeom.Tolerance)]
#if false
        [InlineData(1000_000, ConstGeom.a0, ConstGeom.k, ConstGeom.Tolerance)]
#endif
        public void StandardSpeedTestPreparationGeometric_ComparingDifferentNumbersOfExecutioin(int n, double a0, double k, double tolerance)
        {
            // Arrange
            // Console.WriteLine($"Testing that numerical finite geometric series gives the same results as the analytical calculation.");
            Console.WriteLine($"Parameters:");
            Console.WriteLine($"  n:   {n}         (number of elements)");
            Console.WriteLine($"  a:   {a0}         (the first element)");
            Console.WriteLine($"  d:   {k}         (difference between successive elements)");
            Console.WriteLine($"  tol: {tolerance}     (tolerance (max. allowed discrepancy))");
            Console.WriteLine($"  k^n: {Math.Pow(k, n)}");
            Console.WriteLine("");
            double analyticalResult = UtilSpeedTesting.GeometricSeriesAnalytical(n, a0, k);
            Console.WriteLine($"Analytical result: S = {analyticalResult}");
            // Act
            Stopwatch sw = Stopwatch.StartNew();
            double numericalResult = UtilSpeedTesting.GeometricSeriesNumerical(n, a0, k);
            sw.Stop();
            Console.WriteLine($"Numerical result:  S = {numericalResult}");
            Console.WriteLine($"  Difference: {numericalResult - analyticalResult}, tolerance: {tolerance}");
            // Assert
            numericalResult.Should().BeApproximately(analyticalResult, tolerance, 
                because: "Precond: Calculation needs to be correct in order to use it in speed tests.");
            Console.WriteLine("");
            double calculationsPerSecond = (double)n / sw.Elapsed.TotalSeconds;
            Console.WriteLine($"Geometric series with {n} terms was calculated in {sw.Elapsed.TotalSeconds} s.");
            Console.WriteLine($"  Calculations per second: {calculationsPerSecond}");
            Console.WriteLine($"  Millions  per second:    {calculationsPerSecond / 1.0e6}");
        }

#endregion StandardSpeedTestPreparation



    }

}

