#if true

using System;
using System.Collections.Generic;
using System.Diagnostics;
using FluentAssertions;
using LearnCs.Lib;

namespace IGLib.Tests.Base
{

    public class UtilSpeedTesting
    {

        /// <summary>Analytically calculates and returns the specified finite  arythmetic series
        /// (from the sequence: a_i = a0 + (i-1) * d).
        /// <para>This is mainly used to verify the results of <see cref="ArithmeticSeriesNumerical(int, double, double)"/>,
        /// which is used for simple speed tests on the host computer.</para></summary>
        /// <param name="n">Number of terms in the series to be summed.</param>
        /// <param name="a0">The first element of the arithmetic sequence.</param>
        /// <param name="d">Difference between successive elements of the sequence (next - current).</param>
        /// <returns>The sum of elements of the sequence, calculated analytically.</returns>
        public static double ArithmeticSeriesAnalytical(int n, double a0, double d)
        {
            return n * ((2 * a0 + (n - 1) * d) / 2);
        }

        /// <summary>Analytically calculates and returns the specified finite arythmetic series
        /// (from the sequence: a_i = a0 + (i-1) * d).
        /// <para>This method is mainly used for simple speed tests on the host computer.</para></summary>
        /// <param name="n">Number of terms in the series to be summed.</param>
        /// <param name="a0">The first element of the arithmetic sequence.</param>
        /// <param name="d">Difference between successive elements of the sequence (next - current).</param>
        /// <returns>Thee sum of elements of the sequence, calculated analytically.</returns>
        public static double ArithmeticSeriesNumerical(int n, double a0, double d)
        {
            double an = a0;
            double Sn = 0;
            for (int i = 0; i < n; i++)
            {
                Sn += an;
                an += d;
            }
            return Sn;
        }



        /// <summary>Analytically calculates and returns the specified finite geometric series
        /// (from the geometric sequence: a_i = a0 * k^(i-1)).
        /// <para>This is mainly used to verify the results of <see cref="GeometricSeriesNumerical(int, double, double)"/>,
        /// which is used for simple speed tests on the host computer.</para></summary>
        /// <param name="n">Number of terms in the series to be summed.</param>
        /// <param name="a0">The first element of the geometric sequence.</param>
        /// <param name="k">Quotient between successive elements of the sequence (next / current).</param>
        /// <returns>The sum of elements of the sequence, calculated analytically.</returns>
        public static double GeometricSeriesAnalytical(int n, double a0, double k)
        {
            if (k == 1)
            {
                return a0 * n;
            }
            return a0 * (1 - Math.Pow(k, n)) / (1 - k);
        }

        /// <summary>Numerically calculates and returns the specified finite geometric series
        /// (from the geometric sequence: a_i = a0 * k^(i-1)).
        /// <para>This method is mainly used for simple speed tests on the host computer.</para></summary>
        /// <param name="n">Number of terms in the series to be summed.</param>
        /// <param name="a0">The first element of the geometric sequence.</param>
        /// <param name="k">Quotient between successive elements of the sequence (next / current).</param>
        /// <returns>Thee sum of elements of the sequence, calculated numerically.</returns>
        public static double GeometricSeriesNumerical(int n, double a0, double k)
        {
            double an = a0;
            double Sn = 0;
            for (int i = 0; i < n; i++)
            {
                Sn += an;
                an *= k;
            }
            return Sn;
        }

        /// <summary>Numerically calculates and returns the specified finite geometric series by 
        /// calculating each term of the series directly, rather than accumulate it during iteration.
        /// (from the geometric sequence: a_i = a0 * k^(i-1)).
        /// <para>This method is mainly used for simple speed tests on the host computer.</para></summary>
        /// <param name="n">Number of terms in the series to be summed.</param>
        /// <param name="a0">The first element of the geometric sequence.</param>
        /// <param name="k">Quotient between successive elements of the sequence (next / current).</param>
        /// <returns>Thee sum of elements of the sequence, calculated numerically.</returns>
        public static double GeometricSeriesNumerical_DirectElementCalculation(int n, double a0, double k)
        {
            double an;
            double Sn = 0;
            for (int i = 0; i < n; i++)
            {
                an = a0 * Math.Pow(k, i);
                Sn += an;
            }
            return Sn;
        }

        //public const int StandardSpeedTestGeometricSeries_n = 100_000;

        //public const double StandardSpeedTestGeometricSeries_a0 = 1.0;

        //public const double StandardSpeedTestGeometricSeries_k = 1.0 - 1e-4;

        //public const double StandardSpeedTestGeometricSeries_tolerance = 1.0e-8;



        // Standard parameters for the speed test:
        
        /// <summary>Standard speed test with finite geometric series - number of iterations performed
        /// (i.e., number of terms in the series).</summary>
        public const int TstGeom_n = 100_000;

        /// <summary>Standard speed test with finite geometric series - starting element of the 
        /// geometric sequence.</summary>
        public const double TstGeom_a0 = 1.0;

        /// <summary>Standard speed test with finite geometric series - quotient of successive
        /// elements of the geometric sequence (next / current).</summary>
        public const double TstGeom_k = 1.0 - 1e-4;

        /// <summary>Standard speed test with finite geometric series - tolerance for the discrepancy
        /// between numerically calculated series (which is used in the speed test) and the same
        /// result obtained by the analytic formula.</summary>
        public const double TstGeom_Tolerance = 1.0e-8;

        /// <summary>Standard speed test with finite geometric series - average result (in iterations 
        /// per second) measured on the Ihp24 laptop.</summary>
        public const double TstGeom_IterationsPerSecondIhp24 = 280e6;



        /// <summary>Performs the standard speed test - calculation of a finite geometric series
        /// with specific parameters, and .
        /// <para>Parameters:</para>
        /// <para>  <see cref="TstGeom_n"/> - number of iterations performed (elemnts of the geometric 
        /// sequence that are summed).</para>
        /// <para>  <see cref="TstGeom_a0"/> - first element of the geometric sequence whose elements are summed.</para>
        /// <para>  <see cref="TstGeom_k"/> - quotient between subsequent elements of the geometric series.</para>
        /// <para>  <see cref="TstGeom_Tolerance"/> - tolerance, the allowed absolute discrepancy between the 
        /// numeric and analytic reault (if out of tolerance, exception is thrown in the test method).</para></summary>
        /// <param name="printToConsole">Whether results are printed to console.</param>
        /// <returns>The speed factor - speed relative tothe reference machine, Ihp24.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static double StandardSpeedTestGeometricSeriesIhp24(bool printToConsole = true)
        {
            int n = TstGeom_n;
            double a0 = TstGeom_a0;
            double k = TstGeom_k;
            double tolerance = TstGeom_Tolerance;
            if (printToConsole)
            {
                Console.WriteLine($"Performing speed test - numerical calculation of a finite geometric series...");
                Console.WriteLine($"Parameters:");
                Console.WriteLine($"  n:   {n}         (number of elements)");
                Console.WriteLine($"  a:   {a0}         (the first element)");
                Console.WriteLine($"  d:   {k}         (quotient of successive elements)");
                Console.WriteLine($"  tol: {tolerance}         (tolerance (max. allowed discrepancy))");
                Console.WriteLine($"  k^n: {Math.Pow(k, n)}");
            }
            double analyticalResult = UtilSpeedTesting.GeometricSeriesAnalytical(n, a0, k);
            Stopwatch sw = Stopwatch.StartNew();
            double numericalResult = UtilSpeedTesting.GeometricSeriesNumerical(n, a0, k);
            sw.Stop();
            double iterationsPerSecond = (double)n / sw.Elapsed.TotalSeconds;
            double speedFactor = iterationsPerSecond / TstGeom_IterationsPerSecondIhp24;
            if (printToConsole)
            {
                Console.WriteLine($"Numerical result:  S = {numericalResult}");
                Console.WriteLine($"Analytical result: S = {analyticalResult}");
                Console.WriteLine($"  Difference: {numericalResult - analyticalResult}, tolerance: {tolerance}");
            }
            // Verification - numerical result needs to be correct within the specified tolerance:
            if (Math.Abs(numericalResult - analyticalResult) > tolerance)
            {
                if (printToConsole)
                {
                    Console.WriteLine("WARNING: Diecrepancy between the numerical and analytical result is not within tolerance; exception is being thrown.");
                    throw new InvalidOperationException("Discrepancy between numerical and analytical result is not within tolerance.");
                }
            }
            if (printToConsole)
            {
                Console.WriteLine("");
                Console.WriteLine($"Geometric series with {n} terms was calculated in {sw.Elapsed.TotalSeconds} s.");
                Console.WriteLine($"  Calculations per second: {iterationsPerSecond}");
                Console.WriteLine($"  Millions  per second:    {iterationsPerSecond / 1.0e6}");
                Console.WriteLine($"  Speed factor:            {iterationsPerSecond / TstGeom_IterationsPerSecondIhp24}");
            }
            return iterationsPerSecond;
        }




    }

}

#endif // if false