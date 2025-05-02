#if true

using System.Collections.Generic;

using LearnCs.Lib;

namespace IGLib.Tests.Base
{

    public class UtilSpeedTesting
    {

        public static double ArithmeticSeriesAnalytical(int n, double a, double d)
        {
            return n * ((2 * a + (n - 1) * d) / 2);
        }

        public static double ArithmeticSeriesNumerical(int n, double a, double d)
        {
            double an = a;
            double Sn = 0;
            for (int i = 0; i < n; i++)
            {
                Sn += an;
                an += d;
            }
            return Sn;
        }

    }

}

#endif // if false