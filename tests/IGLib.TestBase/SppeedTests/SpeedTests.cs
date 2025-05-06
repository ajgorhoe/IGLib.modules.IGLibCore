
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;
using System;
using System.Diagnostics;

using IGLib.Tests;
using static IGLib.Tests.UtilSpeedTesting;

namespace IGLib.Tests.Base
{

    /// <summary><para>This test class performs various speed tests, which give some indication of
    /// the current host machine's performance.
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


        #region SpeedTests


        #region SpeedTests.GeometricSeries

        /// <summary>Performs the stanard speed test with calculation of a finite geometric series, with
        /// standard parameters, and reports the resutlrs.</summary>
        [Fact]
        public void StandardSpeedTestGeometricSeries_HpLaptop24()
        {
            Console.WriteLine("Performing standard speed test - finite geometric series...");
            SpeedTestInfo info = null;
            UtilSpeedTesting.StandardSpeedTestGeometricSeries_HpLaptop24(out info, writeToConsole: false);
            info.Should().NotBeNull();
            Console.WriteLine("  ... test completed, information  provided below.");
            Console.WriteLine("");
            Console.WriteLine(info.ToString());
        }


        /// <summary>Performs the stanard speed test with calculation of a finite geometric series, with
        /// standard parameters, with direct calculation of elements of geometric series (using <see cref="Math.Pow"/>,
        /// which is slower than accummulating the powers of the quotient), and reports the resutlrs.</summary>
        [Fact]
        public void StandardSpeedTestGeometricSeries_DirectElementCalculation_HpLaptop24()
        {
            Console.WriteLine("Performing standard speed test - finite geometric series\n  with direct calculation of elements...");
            SpeedTestInfo info = null;
            UtilSpeedTesting.StandardSpeedTestGeometricSeries_DirectElementCalculation_HpLaptop24(out info, writeToConsole: false);
            info.Should().NotBeNull();
            Console.WriteLine("  ... test completed, information  provided below.");
            Console.WriteLine("");
            Console.WriteLine(info.ToString());
        }



        #endregion SpeedTests.GeometricSeries


        #endregion SpeedTests



        #region SpeedTests_LEGACY

        /// <summary>Performs the stanard speed test with calculation of a finite geometric series, with
        /// standard parameters, and reports the resutlrs.</summary>
        /// <remarks>This test can be deleted (it already has a replacement).</remarks>
        [Fact]
        public void Legacy_StandardSpeedTestGeometricSeries_OLD()
        {
            // Arrange
            int n = ConstGeom.NumExecutions;
            double a0 = ConstGeom.a0;
            double k = ConstGeom.k;
            double tolerance = ConstGeom.Tolerance;
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
            double speedFactor = iterationsPerSecond / ConstGeom.RefExecutionsPerSecond_HpLaptop24;
            Console.WriteLine($"Geometric series with {n} terms was calculated in {sw.Elapsed.TotalSeconds} s.");
            Console.WriteLine($"  Calculations per second: {iterationsPerSecond}");
            Console.WriteLine($"  Millions  per second:    {iterationsPerSecond / 1.0e6}");
            Console.WriteLine($"  Speed factor:            {speedFactor}");
        }



        /// <summary>Performs the stanard speed test with calculation of a finite geometric series, with
        /// standard parameters, but with directly calculating each term by using <see cref="Math.Pow(double, double)"/>
        /// rather than accumulating sequence elements iteratively.</summary>
        /// <remarks>This test can be deleted (it already has a replacement).</remarks>
        [Fact]
        public void Legacy_StandardSpeedTestGeometricSeries_DirectElementCalculation_OLD()
        {
            // Arrange
            int n = ConstGeomDirect.NumExecutions;
            double a0 = ConstGeomDirect.a0;
            double k = ConstGeomDirect.k;
            double tolerance = ConstGeomDirect.Tolerance;
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
            double speedFactor = iterationsPerSecond / ConstGeomDirect.RefExecutionsPerSecond_HpLaptop24;
            Console.WriteLine($"Geometric series with {n} terms was calculated in {sw.Elapsed.TotalSeconds} s.");
            Console.WriteLine($"  Calculations per second: {iterationsPerSecond}");
            Console.WriteLine($"  Millions  per second:    {iterationsPerSecond / 1.0e6}");
            Console.WriteLine($"  Speed factor:            {speedFactor}");
        }


        #endregion SpeedTests_LEGACY




    }

}

