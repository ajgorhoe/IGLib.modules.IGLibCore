
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;
using System;
using System.Diagnostics;

using IGLib.Tests;
using static IGLib.Tests.UtilSpeedTesting;
using System.Runtime.CompilerServices;

using IGLib.Tests.Base;
using IGLib.Testing;

namespace IGLib.Testing.Tests
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
    public class UtilSpeedTestingTests : TestBase<UtilSpeedTestingTests>
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

        bool PerformActualCalculationInSpeedTestInfo { get; } = false;


        [Fact]
        protected void SpeedTestInfo_TwoParameterConstructorWorksCorrectly()
        {
            string testId = ConstGeom.TestId;
            string referenceMachineId = ConstMachineHpLaptop24.MachineId;
            SpeedTestInfo testInfo = new(testId, referenceMachineId);
            testInfo.Should().NotBeNull();
            testInfo.TestId.Should().Be(testId);
            testInfo.ReferenceMachineId.Should().Be(referenceMachineId);
        }

        [Fact]
        protected void SpeedTestInfo_ParameterlessConstructorWorksCorrectly()
        {
            SpeedTestInfo testInfo = new();
            testInfo.Should().NotBeNull();
            testInfo.TestId.Should().NotBeNullOrEmpty();
            testInfo.ReferenceMachineId.Should().NotBeNullOrEmpty();
            testInfo.TestId.Should().Be(SpeedTestInfo.DefultTestId);
            testInfo.ReferenceMachineId.Should().Be(SpeedTestInfo.DefaultRefeenceMachineId);
        }

        [Fact]
        protected void SpeedTestInfoGeneric_TwoParameterConstructorWorksCorrectly()
        {
            string testId = ConstGeom.TestId;
            string referenceMachineId = ConstMachineHpLaptop24.MachineId;
            SpeedTestInfo<int> testInfo = new(testId, referenceMachineId);
            testInfo.Should().NotBeNull();
            testInfo.TestId.Should().Be(testId);
            testInfo.ReferenceMachineId.Should().Be(referenceMachineId);
        }

        [Fact]
        protected void SpeedTestInfoGeneric_ParameterlessConstructorWorksCorrectly()
        {
            SpeedTestInfo<float> testInfo = new();
            testInfo.Should().NotBeNull();
            testInfo.TestId.Should().NotBeNullOrEmpty();
            testInfo.ReferenceMachineId.Should().NotBeNullOrEmpty();
            testInfo.TestId.Should().Be(SpeedTestInfo.DefultTestId);
            testInfo.ReferenceMachineId.Should().Be(SpeedTestInfo.DefaultRefeenceMachineId);
        }

        /// <summary>Tests the 2 parameter constructor and majority of properties of the \
        /// <see cref="SpeedTestInfo"/> class (which inherits from <see cref="SpeedTestInfo{double}"/>).</summary>
        [Fact]
        protected void SpeedTestInfo_WorksCorrectlyUnderNormalConditions()
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
            double result = default;
            double executionTimeSeconds = default;
            if (PerformActualCalculationInSpeedTestInfo)
            {
                // Use actual calculation to obtain the result and measure time:
                Stopwatch sw = Stopwatch.StartNew();
                result = GeometricSeriesNumerical(numExecutions, par1Value, par2Value);
                sw.Stop();
                executionTimeSeconds = sw.Elapsed.TotalSeconds;
            }
            else
            {
                // Avoid actual calculation and just generate some plausible results:
                result = ConstGeom.AnalyticResult * (1.0 + 1e-12);
                executionTimeSeconds = (1.0 + 0.0493) * numExecutions / ConstGeom.RefExecutionsPerSecond_HpLaptop24;
            }
            // Act
            SpeedTestInfo testInfo = new SpeedTestInfo(testId, referenceMachineId);
            // test specific parameters:
            testInfo.NumExecutions = numExecutions;
            testInfo.Parameters.AddRange([(par1Name, par1Value), (par2Name, par2Value)]);
            // expected results:
            testInfo.AnalyticalResult = analyticalResult;
            testInfo.Tolerance = tolerance;
            testInfo.ReferenceExecutionsPerSecond = referenceExecutionsPerSecond;
            // results:
            testInfo.Result = result;
            testInfo.ExecutionTimeSeconds = executionTimeSeconds;

            // Assert
            // creation:
            testInfo.Should().NotBeNull();
            testInfo.TestId.Should().Be(testId);
            testInfo.ReferenceMachineId.Should().Be(referenceMachineId);
            // test-specific parameters:
            testInfo.NumExecutions.Should().Be(numExecutions);
            testInfo.Parameters.Should().NotBeNull();
            testInfo.Parameters.Count.Should().Be(2);
            var p1 = testInfo.Parameters[0];
            p1.ParameterName.Should().Be(par1Name);
            p1.ParameterValue.Should().Be(par1Value);
            var p2 = testInfo.Parameters[1];
            p2.ParameterName.Should().Be(par2Name);
            p2.ParameterValue.Should().Be(par2Value);
            // expected results:
            testInfo.HasValidResult.Should().BeTrue();
            testInfo.HasExecutionTime.Should().BeTrue();
            testInfo.AnalyticalResult.Should().Be(analyticalResult);
            testInfo.Tolerance.Should().Be(tolerance);
            testInfo.ReferenceExecutionsPerSecond.Should().Be(referenceExecutionsPerSecond);
            // results:
            testInfo.Exception.Should().BeNull();
            testInfo.Result.Should().Be(result);
            testInfo.CanCalculateDiscrepancy.Should().BeTrue();
            testInfo.Discrepancy.Should().BeApproximately(testInfo.Result - testInfo.AnalyticalResult, 1e-8);
            testInfo.ExecutionTimeSeconds.Should().Be(executionTimeSeconds);
            testInfo.NumExecutionsPerSecond.Should().BeApproximately(
                (double)numExecutions / executionTimeSeconds, 1e-3);
            testInfo.MegaExecutionsPerSecond.Should().BeApproximately(
                testInfo.NumExecutionsPerSecond / 1e6, 1e-6);
            testInfo.SpeedFactor.Should().BeApproximately(testInfo.NumExecutionsPerSecond / referenceExecutionsPerSecond, 1e-8);
        }

        /// <summary>Provides a dummy speed test method that can be used in testing of <see cref="SpeedTestInfo"/> class.</summary>
        /// <param name="testInfo">Output parameter - info object created by the speed test executing function.</param>
        /// <param name="exception">Exception that will be thrown at the prescribed location within this method
        /// If null then <see cref="InvalidOperationException"/> with a default message is assigned to this parameter..</param>
        /// <param name="throwBeforeResultsObtained">If true then exception is thrown before the results are obtained by
        /// from the (dummy / imaginary) test.</param>
        /// <param name="throwAfterResultsObtained">If true then exception is thrown after the results are obtained and assigned
        /// to <paramref name="testInfo"/>.</param>
        /// <param name="executeActualTest">If true then actual test is executed, otherwise the test result and duration
        /// are just assigned (values are selected such that they make sense).</param>
        /// <returns>The speed factor calculated form the test.</returns>
        protected double DummySpeedTest(out SpeedTestInfo testInfo,
            Exception exception = null,
            bool throwBeforeResultsObtained = false, bool throwAfterResultsObtained = false, bool executeActualTest = false)
        {
            testInfo = null;
            try
            {
                if (exception == null)
                {
                    exception = new InvalidOperationException("Test exception.");
                }
                // Creation parameters:
                string testId = ConstGeom.TestId;
                string referenceMachineId = ConstMachineHpLaptop24.MachineId;
                testInfo = new SpeedTestInfo(testId, referenceMachineId);
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
                double executionTimeSeconds = default;
                double result = default;
                if (throwBeforeResultsObtained)
                {
                    throw exception;
                }
                if (executeActualTest)
                {
                    // Use actual calculation to obtain the result and measure time:
                    Stopwatch sw = Stopwatch.StartNew();
                    result = GeometricSeriesNumerical(numExecutions, par1Value, par2Value);
                    sw.Stop();
                    executionTimeSeconds = sw.Elapsed.TotalSeconds;
                }
                else
                {
                    // Avoid actual calculation and just generate some plausible results:
                    result = ConstGeom.AnalyticResult * (1.0 + 1e-12);
                    executionTimeSeconds = (1.0 + 0.0493) * numExecutions / ConstGeom.RefExecutionsPerSecond_HpLaptop24;
                }
                // Act
                // test specific parameters:
                testInfo.NumExecutions = numExecutions;
                testInfo.Parameters.AddRange([(par1Name, par1Value), (par2Name, par2Value)]);
                // expected results:
                testInfo.AnalyticalResult = analyticalResult;
                testInfo.Tolerance = tolerance;
                testInfo.ReferenceExecutionsPerSecond = referenceExecutionsPerSecond;
                // results:
                testInfo.Result = result;
                testInfo.ExecutionTimeSeconds = executionTimeSeconds;
                if (throwAfterResultsObtained)
                {
                    throw exception;
                }
            }
            catch (Exception ex)
            {
                if (testInfo != null) { testInfo.Exception = ex; }
                throw;
            }
            if (testInfo != null) { return testInfo.SpeedFactor; }
            return default;
        }


        [Fact]
        protected void SpeedTestInfo_BehaviorWhenExceptionThrownBeforeObtainingResultsIsCorrect()
        {
            Exception thrownException = new InvalidOperationException("Thrown within the speed test.");
            Exception caughtException = null;
            SpeedTestInfo testInfo = null;
            double initialSpeedFactor = -1.0;
            double speedFactor = initialSpeedFactor;
            try
            {
                speedFactor = DummySpeedTest(out testInfo, thrownException, throwBeforeResultsObtained: true);
            }
            catch (Exception ex)
            {
                // In this way the exceptions should be handled whenever speed tests are called.
                caughtException = ex;
            }
            caughtException.Should().NotBeNull(because: "PRECOND: The test method should throw exception.");
            caughtException.Should().BeEquivalentTo(thrownException, because: "PRECOND: The test method should throw the same exception as prescribed.");
            testInfo.Should().NotBeNull(because: "PRECOND: testInfo should be created by the test function before exception is thrown.");
            testInfo.Exception.Should().NotBeNull();
            testInfo.Exception.Should().Be(thrownException);
            testInfo.Result.Should().Be(default);
            testInfo.ExecutionTimeSeconds.Should().Be(SpeedTestInfo.DefaultExecutionTimeSeconds);
            testInfo.HasValidResult.Should().BeFalse();
            testInfo.HasExecutionTime.Should().BeFalse();
            speedFactor.Should().Be(initialSpeedFactor, because: "Speed factor cannot be transferred to the caller when exception is thrown.");
        }


        [Fact]
        protected void SpeedTestInfo_BehaviorWhenExceptionThrownAfterObtainingResultsIsCorrect()
        {
            Exception thrownException = new InvalidOperationException("Thrown within the speed test.");
            Exception caughtException = null;
            SpeedTestInfo testInfo = null;
            double initialSpeedFactor = -1.0;
            double speedFactor = initialSpeedFactor;
            try
            {
                speedFactor = DummySpeedTest(out testInfo, thrownException, throwAfterResultsObtained: true);
            }
            catch (Exception ex)
            {
                // In this way the exceptions should be handled whenever speed tests are called.
                caughtException = ex;
            }
            caughtException.Should().NotBeNull(because: "PRECOND: The test method should throw exception.");
            caughtException.Should().BeEquivalentTo(thrownException, because: "PRECOND: The test method should throw the same exception as prescribed.");
            testInfo.Should().NotBeNull(because: "PRECOND: testInfo should be created by the test function before exception is thrown.");
            testInfo.Exception.Should().NotBeNull();
            testInfo.Exception.Should().Be(thrownException);
            testInfo.Result.Should().NotBe(default);
            testInfo.ExecutionTimeSeconds.Should().NotBe(SpeedTestInfo.DefaultExecutionTimeSeconds);
            testInfo.HasValidResult.Should().BeFalse();
            testInfo.HasExecutionTime.Should().BeFalse();
            speedFactor.Should().Be(initialSpeedFactor, because: "Speed factor cannot be transferred to the caller when exception is thrown.");
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

