
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;
using System;
using System.Diagnostics;

using static IGLib.Tests.UtilSpeedTesting;

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


        #endregion TestUtilSpeedTesting





        /// <summary>Performs calculation of geometric series at the same parameters as <see cref="UtilSpeedTesting.StandardSpeedTestGeometricSeriesBasic(bool)"/>,
        /// except that calculation is performed with different numbers of terms, in order to find the suitable
        /// parameter for standard tests.</summary>
        /// <param name="n">Number of elements of the sum.</param>
        /// <param name="a0">The first element of the sum.</param>
        /// <param name="k">The constant quotient between the next element of the sequence and the current element.</param>
        /// <param name="tolerance">The tolerance, allowed discrepancy between both results, to account fo rounding errors.</param>
        [Theory]
        [InlineData(1, TstGeom_a0, TstGeom_k, TstGeom_Tolerance)]
        [InlineData(10, TstGeom_a0, TstGeom_k, TstGeom_Tolerance)]
        [InlineData(100, TstGeom_a0, TstGeom_k, TstGeom_Tolerance)]
        [InlineData(1_000, TstGeom_a0, TstGeom_k, TstGeom_Tolerance)]
        [InlineData(10_000, TstGeom_a0, TstGeom_k, TstGeom_Tolerance)]
        [InlineData(100_000, TstGeom_a0, TstGeom_k, TstGeom_Tolerance)]
        [InlineData(1000_000, TstGeom_a0, TstGeom_k, TstGeom_Tolerance)]
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


        /// <summary>Performs the stanard speed test with calculation of a finite geometric series, with
        /// standard parameters, and reports the resutlrs.</summary>
        [Fact]
        public void StandardSpeedTestGeometricSeries()
        {
            // Arrange
            int n = TstGeom_n;
            double a0 = TstGeom_a0;
            double k = TstGeom_k;
            double tolerance = TstGeom_Tolerance;
            Console.WriteLine($"Performing standard speed test: numerical finite geometric series...");
            Console.WriteLine($"Parameters:");
            Console.WriteLine($"  n:   {n}         (number of elements)");
            Console.WriteLine($"  a:   {a0}         (the first element)");
            Console.WriteLine($"  d:   {k}         (quotient of successive elements)");
            Console.WriteLine($"  tol: {tolerance}     (tolerance (max. allowed discrepancy))");
            Console.WriteLine($"  k^n: {Math.Pow(k, n)}");
            double analyticalResult = UtilSpeedTesting.GeometricSeriesAnalytical(n, a0, k);
            // Act
            Stopwatch sw = Stopwatch.StartNew();
            double numericalResult = UtilSpeedTesting.GeometricSeriesNumerical(n, a0, k);
            sw.Stop();
            Console.WriteLine($"Numerical result:  S = {numericalResult}");
            Console.WriteLine($"Analytical result: S = {analyticalResult}");
            Console.WriteLine($"  Difference: {numericalResult - analyticalResult}, tolerance: {tolerance}");
            // Assert
            numericalResult.Should().BeApproximately(analyticalResult, tolerance,
                because: "Precond: Calculation needs to be correct in order to use it in speed tests.");
            Console.WriteLine("");
            double iterationsPerSecond = (double)n / sw.Elapsed.TotalSeconds;
            double speedFactor = iterationsPerSecond / TstGeom_IterationsPerSecondIhp24;
            Console.WriteLine($"Geometric series with {n} terms was calculated in {sw.Elapsed.TotalSeconds} s.");
            Console.WriteLine($"  Calculations per second: {iterationsPerSecond}");
            Console.WriteLine($"  Millions  per second:    {iterationsPerSecond / 1.0e6}");
            Console.WriteLine($"  peed factor:             {speedFactor}");
        }



        /// <summary>Performs the stanard speed test with calculation of a finite geometric series, with
        /// standard parameters, but with directly calculating each term by using <see cref="Math.Pow(double, double)"/>
        /// rather than accumulating sequence elements iteratively..</summary>
        [Fact]
        public void StandardSpeedTestGeometricSeries_DirectElementCalculation()
        {
            // Arrange
            int n = TstGeom_n;
            double a0 = TstGeom_a0;
            double k = TstGeom_k;
            double tolerance = TstGeom_Tolerance;
            Console.WriteLine($"Performing standard speed test: numerical finite geometric series...");
            Console.WriteLine($"Parameters:");
            Console.WriteLine($"  n:   {n}         (number of elements)");
            Console.WriteLine($"  a:   {a0}         (the first element)");
            Console.WriteLine($"  d:   {k}         (quotient of successive elements)");
            Console.WriteLine($"  tol: {tolerance}     (tolerance (max. allowed discrepancy))");
            Console.WriteLine($"  k^n: {Math.Pow(k, n)}");
            double analyticalResult = UtilSpeedTesting.GeometricSeriesAnalytical(n, a0, k);
            // Act
            Stopwatch sw = Stopwatch.StartNew();
            double numericalResult = UtilSpeedTesting.GeometricSeriesNumerical_DirectElementCalculation(n, a0, k);
            sw.Stop();
            Console.WriteLine($"Numerical result:  S = {numericalResult}");
            Console.WriteLine($"Analytical result: S = {analyticalResult}");
            Console.WriteLine($"  Difference: {numericalResult - analyticalResult}, tolerance: {tolerance}");
            // Assert
            numericalResult.Should().BeApproximately(analyticalResult, tolerance,
                because: "Precond: Calculation needs to be correct in order to use it in speed tests.");
            Console.WriteLine("");
            double iterationsPerSecond = (double)n / sw.Elapsed.TotalSeconds;
            double speedFactor = iterationsPerSecond / TstGeom_IterationsPerSecondIhp24;
            Console.WriteLine($"Geometric series with {n} terms was calculated in {sw.Elapsed.TotalSeconds} s.");
            Console.WriteLine($"  Calculations per second: {iterationsPerSecond}");
            Console.WriteLine($"  Millions  per second:    {iterationsPerSecond / 1.0e6}");
            Console.WriteLine($"  peed factor:             {speedFactor}");
        }


        #region SpeedTests




        #endregion S[eedTests



    }

}

