#if true

using System;
using System.Collections.Generic;

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

        /// <summary>Analytically calculates and returns the specified finite geometric series
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




    }

}

#endif // if false