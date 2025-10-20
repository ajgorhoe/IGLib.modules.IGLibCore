// Copyright © Igor Grešovnik (2008 - present). License:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    // WARNING:
    // This source files was copied from iglib_base;
    // TODO:
    // Consolidate later!
    // Remark: There should only be some changes in methods like Sum, Mean, Average and StandardDeviation.


    /// <summary>Example class that derives from the <see cref="M"/> class.
    /// Implements a method that uses basic mathematical functions implemented in M.</summary>
    /// <remarks>
    /// TODO: Join the FuncMath class with this class (incorporate that class and abolish it)!!
    /// </remarks>
    /// $A Igor xx Jul11;
    [Obsolete("Experimental functionality, should not be used in produciton code!")]
    public class ExampleMathClass : M
    {

        public ExampleMathClass()
            : base()
        { }

        /// <summary>Method in which comfortable mathematic functions form the base class <see cref="M"/> are used.</summary>
        /// <param name="x">Argument of a real-valued function.</param>
        /// <returns>Some function of the argument.</returns>
        public double f(double x)
        {
            double ret = sin(x + e * e + pow(1.2, 3.5));
            double xr = pow(1.34, 2.55) + zero;
            double r = (double)(pow(3.5, 2.55) + zero);
            return (double)ret;
        }

        /// <summary>A nested class that does not derive from the <see cref="M"/> class, 
        /// but still statidc functions from that class can be used because it is embedded in that class.</summary>
        public class Nested
        {

            /// <summary>Method in which comfortable mathematic functions form the base class <see cref="M"/> are used.</summary>
            /// <param name="x">Argument of a real-valued function.</param>
            /// <returns>Some function of the argument.</returns>
            public double f(double x)
            {
                double ret = sin(x + pow(e, 2) + pow(1.2, 3.5));
                double xr = pow(1.34, 2.55) + zero;
                double r = (double)(pow(3.5, 2.55) + zero);
                return (double)ret;
            }

        }

    } // class ExampleMathClass


    //public class UtilMath : M
    //{
    //}

    /// <summary>Defines some mathematical functions to be used in derived and other classes.
    /// <para>* Standard mathematical functions  and constants with short names are implemented, e.g. sin() instesd of Math.Sin().</para>
    /// <para>  ** These functions are public and static such that they can be used out of the derived classes, too.</para>
    /// <para>    *** In particular, some script classes derive from this one, in order to use simple-named mathematical functions.</para>
    /// <para>  ** Some functions are defined with several names, in order to reduce probability of errors in scripts.</para></summary>
    public abstract partial class M
    {


        #region Constants

        /// <summary>Natural logarithmic base.</summary>
        public static double e
        { get { return Math.E; } }

        /// <summary>Ratio of the circumference of a circle to its diameter.</summary>
        public static double pi
        { get { return Math.PI; } }

        /// <summary>Zero (value 0.0).</summary>
        public static double zero
        { get { return 0.0; } }

        /// <summary>One (value 1.0).</summary>
        public static double one
        { get { return 1.0; } }

        #endregion Constants


        #region SwapMethods

        /// <summary>Swaps values of the two specified variables of type double.</summary>
        /// <param name="a">First variable.</param>
        /// <param name="b">Second variable.</param>
        public static void Swap(ref double a, ref double b)
        {
            double s = a;
            a = b;
            b = s;
        }

        /// <summary>Swaps values of the two specified variables of type int.</summary>
        /// <param name="a">First variable.</param>
        /// <param name="b">Second variable.</param>
        public static void Swap(ref int a, ref int b)
        {
            int s = a;
            a = b;
            b = s;
        }

        /// <summary>Swaps values of the two specified variables of type string.</summary>
        /// <param name="a">First variable.</param>
        /// <param name="b">Second variable.</param>
        public static void Swap(ref string a, ref string b)
        {
            string s = a;
            a = b;
            b = s;
        }

        #endregion SwapMethods


        #region Functions

        #region SignFunctions

        /// <summary>Absolute value.</summary>
        public static double abs(double a)
        { return Math.Abs(a); }

        /// <summary>Returns a value indicating the sign of a number.</summary>
        public static double sign(double a)
        { return Math.Sign(a); }

        #endregion SignFunctions


        #region RoundingFunctions

        /// <summary>Returns the smallest integral value that is greater than or equal to the specified decimal number.</summary>
        public static double ceil(double a)
        { return Math.Ceiling(a); }

        /// <summary>Returns the largest integer less than or equal to the specified number.</summary>
        public static double floor(double a)
        { return Math.Floor(a); }

        /// <summary>Calculates the integral part of a specified number.</summary>
        public static double trunc(double a)
        { return Math.Truncate(a); }

        #endregion RoundingFunctions


        #region MinMaxFunctions

        /// <summary>Smallest of two numbers.</summary>
        public static double min(double a, double b) { return (a <= b ? a : b); }

        /// <summary>Largest of two numbers.</summary>
        public static double max(double a, double b) { return (a >= b ? a : b); }

        /// <summary>Smallest of three numbers.</summary>
        public static double min(double a, double b, double c)
        {
            a = (a <= b ? a : b);
            return (a <= c ? a : c);
        }

        /// <summary>Largest of three numbers.</summary>
        public static double max(double a, double b, double c)
        {
            a = (a >= b ? a : b);
            return (a >= c ? a : c);
        }

        /// <summary>Smallest of four numbers.</summary>
        public static double min(double a, double b, double c, double d)
        {
            a = (a <= b ? a : b);
            b = (c <= d ? c : d);
            return (a <= b ? a : b);
        }

        /// <summary>Largest of four numbers.</summary>
        public static double max(double a, double b, double c, double d)
        {
            a = (a >= b ? a : b);
            b = (c >= d ? c : d);
            return (a >= b ? a : b);
        }

        /// <summary>Smallest of five numbers.</summary>
        public static double min(double a, double b, double c, double d, double e)
        {
            a = (a <= b ? a : b);
            b = (c <= d ? c : d);
            a = (a <= b ? a : b);
            return (a <= e ? a : e);
        }

        /// <summary>Largest of five numbers.</summary>
        public static double max(double a, double b, double c, double d, double e)
        {
            a = (a >= b ? a : b);
            b = (c >= d ? c : d);
            a = (a >= b ? a : b);
            return (a >= e ? a : e);
        }

        /// <summary>Smallest of six numbers.</summary>
        public static double min(double a, double b, double c, double d, double e, double f)
        {
            a = (a <= b ? a : b);
            b = (c <= d ? c : d);
            a = (a <= b ? a : b);
            b = (e <= f ? e : f);
            return (a <= b ? a : b);
        }

        /// <summary>Largest of six numbers.</summary>
        public static double max(double a, double b, double c, double d, double e, double f)
        {
            a = (a >= b ? a : b);
            b = (c >= d ? c : d);
            a = (a >= b ? a : b);
            b = (e >= f ? e : f);
            return (a >= b ? a : b);
        }

        /// <summary>Minimal of the specified values.</summary>
        public static double min(params double[] numbers)
        {
            if (numbers != null)
            {
                int num = numbers.Length;
                if (num > 0)
                {
                    double ret = numbers[0];
                    for (int i = 1; i < num; ++i)
                    {
                        if (numbers[i] < ret)
                            ret = numbers[i];
                    }
                    return ret;
                }
            }
            throw new ArgumentException("Minimal among several values: no values specified.");
        }

        /// <summary>Maximal of the specified values.</summary>
        public static double max(params double[] numbers)
        {
            if (numbers != null)
            {
                int num = numbers.Length;
                if (num > 0)
                {
                    double ret = numbers[0];
                    for (int i = 1; i < num; ++i)
                    {
                        if (numbers[i] > ret)
                            ret = numbers[i];
                    }
                    return ret;
                }
            }
            throw new ArgumentException("Maximal among several values: no values specified.");
        }

        #endregion MinMaxFunctions


        #region ProductsAndSums

        /// <summary>Sum of the specified values.</summary>
        public static double sum(params double[] numbers)
        {
            if (numbers != null)
            {
                int num = numbers.Length;
                if (num > 0)
                {
                    double ret = 0;
                    for (int i = 0; i < num; ++i)
                    {
                        ret += numbers[i];
                    }
                    return ret;
                }
            }
            throw new ArgumentException("Sum of several values: no values specified.");
        }

        /// <summary>Product of the specified values.</summary>
        public static double prod(params double[] numbers)
        {
            if (numbers != null)
            {
                int num = numbers.Length;
                if (num > 0)
                {
                    double ret = 1;
                    for (int i = 0; i < num; ++i)
                    {
                        ret *= numbers[i];
                    }
                    return ret;
                }
            }
            throw new ArgumentException("Product of several values: no values specified.");
        }

        #endregion ProductsAndSums


        #region PowersAndRoots

        /// <summary>Returns a specified number raised to the specified power.</summary>
        public static double pow(double a, double b)
        { return Math.Pow(a, b); }

        /// <summary>Returns the square of a specified number.</summary>
        public static double sqr(double a)
        { return a * a; }

        /// <summary>Returns the square of a specified number.</summary>
        public static double cube(double a)
        { return a * a * a; }

        /// <summary>Square.</summary>
        public static double pow2(double x) { return x * x; }

        /// <summary>3rd power.</summary>
        public static double pow3(double x) { return x * x * x; }

        /// <summary>4th power.</summary>
        public static double pow4(double x) { double sq = x * x; return sq * sq; }

        /// <summary>5th power.</summary>
        public static double pow5(double x) { double sq = x * x; return sq * sq * x; }

        /// <summary>6th power.</summary>
        public static double pow6(double x) { double sq = x * x; return sq * sq * sq; }

        /// <summary>Returns the square root of a specified number.</summary>
        public static double sqrt(double a) { return Math.Sqrt(a); }

        /// <summary>Returns the square root of a specified number.</summary>
        public static double root2(double a) { return Math.Sqrt(a); }

        /// <summary>Returns the cubic root of a specified number.</summary>
        public static double root3(double a) { return Math.Pow(a, 1.0 / 3.0); }

        #endregion PowersAndRoots


        #region RandomNumbers

        /// <summary>Returns a uniformly distributed random number greater than or equal to 0.0, 
        /// and less or equal than 1.0.</summary>
        public static double rand()
        {
            return RandomGenerator.Global.NextDoubleInclusive();
        }

        /// <summary>Returns a uniformly distributed random number greater than or equal to min, 
        /// and less or equal than max.</summary>
        public static double rand(double min, double max)
        {
            return RandomGenerator.Global.NextDoubleInclusive(min, max);
        }


        /// <summary>Returns a Gaussian distributed random number with the specified mean and standard deviation.</summary>
        /// <param name="mean">Mean value of the distribution.</param>
        /// <param name="standardDeviation">Standard deviation of the distribution.</param>
        public static double randgauss(double mean, double standardDeviation)
        {
            return RandomGaussian.Global.NextGaussian(mean, standardDeviation);
        }


        #endregion RandomNumbers





        #region LogExpFunctions

        /// <summary>Returns e raised to the specified power.</summary>
        public static double exp(double a)
        { return Math.Exp(a); }

        /// <summary>Returns the natural (base e) logarithm of a specified number.</summary>
        public static double log(double a)
        { return Math.Log(a); }

        /// <summary>Returns the natural (base e) logarithm of a specified number.</summary>
        public static double ln(double a)
        { return Math.Log(a); }

        /// <summary>Returns the base 10 logarithm of a specified number.</summary>
        public static double log10(double a)
        { return Math.Log10(a); }

        /// <summary>Returns the base 10 logarithm of a specified number.</summary>
        public static double lg(double a)
        { return Math.Log10(a); }

        /// <summary>Returns the base 2 logarithm of a specified number.</summary>
        public static double log2(double a)
        { return Math.Log(a, 2); }

        /// <summary>Returns the logarithm of a specified number in a specified base.</summary>
        /// <param name="a">Number whose logarithm is returned.</param>
        /// <param name="logBase">Base of the logarithm.</param>
        public static double log(double a, double logBase)
        { return Math.Log(a, logBase); }

        #endregion LogExpFunctions


        #region AngleFunctions

        /// <summary>Converts angle in radians to angle in degrees and returns it.</summary>
        public static double deg(double x) { return ((x) * 180 / Math.PI); }

        /// <summary>Converts angle in degrees to angle in radians and returns it.</summary>
        public static double rad(double x) { return ((x) * Math.PI / 180); }

        #endregion AngleFunctions


        #region TrigonometricFunctions

        /// <summary>Returns the sine of the specified angle.</summary>
        public static double sin(double a)
        { return Math.Sin(a); }

        /// <summary>Returns the cosine of the specified angle.</summary>
        public static double cos(double a)
        { return Math.Cos(a); }

        /// <summary>Returns the tangent of the specified angle.</summary>
        public static double tg(double a)
        { return Math.Tan(a); }

        /// <summary>Returns cotangent of the specified angle.</summary>
        public static double ctg(double x) { return (1 / Math.Tan(x)); }

        #endregion TrigonometricFunctions


        #region InverseTrigonometricFunctions

        /// <summary>Returns the angle whose cosine is the specified number.</summary>
        public static double arccos(double a)
        { return Math.Acos(a); }
        /// <summary>Returns the angle whose sine is the specified number.</summary>
        public static double arcsin(double a)
        { return Math.Asin(a); }

        /// <summary>Returns the angle whose tangent is the specified number.</summary>
        public static double arctg(double a)
        { return Math.Atan(a); }

        /// <summary>Arc cotangent, inverse of 1/tan(x).</summary>
        public static double arcctg(double x) { return ((x) == 0 ? 0.5 * Math.PI : Math.Atan(1 / (x))); }

        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        public static double arctg2(double a, double b)
        { return Math.Atan2(a, b); }

        #endregion InverseTrigonometricFunctions


        #region HyperbolicFunctions

        /// <summary>Returns the hyperbolic sine of the specified angle.</summary>
        public static double Sinh(double a)
        { return Math.Sinh(a); }

        /// <summary>Returns the hyperbolic sine of the specified angle.</summary>
        public static double sinh(double a)
        { return Math.Sinh(a); }

        /// <summary>Returns the hyperbolic sine of the specified angle.</summary>
        public static double sh(double a)
        { return Math.Sinh(a); }

        /// <summary>Returns the hyperbolic cosine of the specified angle.</summary>
        public static double ch(double a)
        { return Math.Cosh(a); }

        /// <summary>Returns the hyperbolic tangent of the specified angle.</summary>
        public static double th(double a)
        { return Math.Tanh(a); }

        /// <summary>Hyperblic cotangent, 1/Math.Tanh.</summary>
        public static double cth(double x) { return (1 / Math.Tanh(x)); }

        #endregion HyperbolicFunctions


        #region InverseHyperbolicFunctions

        /// <summary>Inverse hyperbolic sine.</summary>
        public static double arsh(double x) { return Math.Log((x) + Math.Sqrt(pow2(x) + 1)); }

        /// <summary>Inverse hyperbolic cosine.</summary>
        public static double arch(double x) { return Math.Log((x) + Math.Sqrt(pow2(x) - 1)); }

        /// <summary>Inverse hyperbolic tangent.</summary>
        public static double arth(double x) { return (0.5 * Math.Log((1 + (x)) / (1 - (x)))); }

        /// <summary>Inverse hyperbolic cotangent.</summary>
        public static double arcth(double x) { return (0.5 * Math.Log(((x) + 1) / ((x) - 1))); }

        #endregion InverseHyperbolicFunctions


        #endregion Functions


        #region Combinatorics

        /// <summary>Array of all factorials that do not produce overflow.</summary>
        private static Int64[] factorials64 = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320,
                                       362880, 3628800, 39916800, 479001600, 6227020800, 87178291200,
                                       1307674368000, 20922789888000, 355687428096000, 6402373705728000,
                                       121645100408832000, 2432902008176640000 };


        /// <summary>Whether or not factorials array has been tested.</summary>
        private static bool factorialsArrayChecked = false;

        /// <summary>Tests ehether the factoirals in the hard-coded array (<see cref="factorials64"/>) of factorials are correct; 
        /// Throws ArgumentException if any of them is incorrect.</summary>
        /// <returns>true if test is successful.</returns>
        public static bool CheckFactorialsArray()
        {
            for (int i = 0; i < factorials64.Length; ++i)
            {
                if (factorials64[i] != facCalculated(i))
                    throw new ArgumentException("Factorial of " + i + "has the wrong value in the array: "
                        + factorials64[i] + " instead of " + facCalculated(i));
            }
            return true;
        }


        /// <summary>Writes to the console all factorials that can be calculatet.</summary>
        public static void TestFactorials()
        {
            TestFactorials(0);
        }


        /// <summary>Writes to the console the first n factorials.</summary>
        /// <param name="max">Largest number whose factorial will be calculated.</param>
        public static void TestFactorials(int max)
        {
            Console.WriteLine();
            Console.WriteLine("Test of factorials (tabulated vs. calculated): ");
            if (max <= 0)
                max = factorials64.Length;
            for (int n = 0; n < max; ++n)
            {
                long facFromArray = fac(n);
                long facCalc = facCalculated(n);
                if (facFromArray != facCalc)
                    Console.WriteLine("Error: the following factorial is incorrect.");
                Console.WriteLine("Factorial of " + n + " = " + facFromArray + " / " + facCalc);
            }
            Console.WriteLine();
        }

        /// <summary>Returns factorial of the specified number.</summary>
        public static long fac(int factor)
        {
            if (!factorialsArrayChecked)
                CheckFactorialsArray();
            return factorials64[factor];
        }

        /// <summary>Returns factorial of the specified number - less efficient approac, but does not rely
        /// on hard coded array.</summary>
        public static long facCalculated(long n)
        {
            if (n == 0) return 1;
            long t = n;
            while (n-- > 2) t *= n;
            return t;
        }

        /// <summary>Calculates and returns the falling power of the specified number.</summary>
        /// <param name="n"></param>
        /// <param name="p"></param>
        /// <remarks>See http://en.wikipedia.org/wiki/Falling_power
        /// </remarks>
        public static long fallingPower(long n, long p)
        {
            long t = 1;
            for (long i = 0; i < p; i++) t *= n--;
            return t;
        }

        /// <summary>Returns binomial coefficient <paramref name="n"/> over <paramref name="k"/>.</summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        public static long binomial(long n, long k)
        {
            if (k > n - k)
            {
                k = n - k;
            }
            long c = 1;
            for (int i = 0; i < k; i++)
            {
                c = c * (n - i);
                c = c / (i + 1);
            }
            return c;
        }

        /// <summary>Writes the first few binomial coefficients to the console.</summary>
        public static void TestBinomialCoefficients()
        {
            TestBinomialCoefficients(20);
        }

        /// <summary>Writes binomial coefficients up to the specified number to the console.</summary>
        /// <param name="nMax">Maximal enumerator until which coefficients are written.</param>
        public static void TestBinomialCoefficients(int nMax)
        {
            Console.WriteLine();
            Console.WriteLine("Pascal triangle (binomial coefficients): ");
            for (int n = 0; n < nMax; ++n)
            {
                for (int k = 0; k <= n; ++k)
                    Console.Write(binomial(n, k) + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        #endregion Combinatorics


        #region Statistics







        /// <summary>Calculates and returns sum of its arguments (or elements of the specified double array).</summary>
        /// <param name="sampleElements">Either array of double numbers or multiple number parameters whose mean value is calculated.</param>
        public static double Sum(params double[] sampleElements)
        {
            IEnumerable<double> samp = sampleElements;
            return Sum(samp);
        }

        /// <summary>Calculates and returns sum of elemennts the specified sample.</summary>
        /// <param name="sample">Collection containing observations within the sample.</param>
        public static double Sum(IEnumerable<double> sample)
        {
            if (sample == null)
                return 0;
                // throw new ArgumentException("Sample not specified (null reference).");
            double ret = 0;
            int numElements = 0;
            foreach (double element in sample)
            {
                ret += element;
                ++numElements;
            }
            return ret;
        }

        /// <summary>Calculates and returns proudct of its arguments (or elements of the specified double array).</summary>
        /// <param name="sampleElements">Either array of double numbers or multiple number parameters whose mean value is calculated.</param>
        public static double Product(params double[] sampleElements)
        {
            IEnumerable<double> samp = sampleElements;
            return Product(samp);
        }

        /// <summary>Calculates and returns product of ellements of the specified sample.</summary>
        /// <param name="sample">Collection containing observations within the sample.</param>
        public static double Product(IEnumerable<double> sample)
        {
            if (sample == null)
                return 1;
                // throw new ArgumentException("Sample not specified (null reference).");
            double ret = 1;
            int numElements = 0;
            foreach (double element in sample)
            {
                ret += element;
                ++numElements;
            }
            return ret;
        }

        /// <summary>Calculates and returns mean value of its arguments (or elements of the specified double array).</summary>
        /// <param name="sampleElements">Either array of double numbers or multiple number parameters whose mean value is calculated.</param>
        /// <exception cref="ArgumentException">When number of elements is 0 or null argument is specified.</exception>
        public static double Mean(params double[] sampleElements)
        {
            IEnumerable<double> samp = sampleElements;
            return Mean(samp);
        }

        /// <summary>Returns mean value of the specified sample.</summary>
        /// <param name="sample">Collection containing observations within the sample.</param>
        /// <exception cref="ArgumentException">When number of elements is 0 or null argument is specified.</exception>
        public static double Mean(IEnumerable<double> sample)
        {
            if (sample == null)
                throw new ArgumentException("Sample not specified (null reference).");
            double ret = 0;
            int numElements = 0;
            foreach (double element in sample)
            {
                ret += element;
                ++numElements;
            }
            if (numElements < 1)
            {
                throw new ArgumentException("Cannot calculate sample mean, number of elements is less than 1.");
            }
            return ret / numElements;
        }


        /// <summary>The same as <see cref="Mean(double[])"/></summary>
        /// <see cref="Mean(double[])"/>
        /// <exception cref="ArgumentException">When number of elements is 0 or null argument is specified.</exception>
        public static double Average(params double[] sampleElements)
        {
            return Mean(sampleElements);
        }

        /// <summary>The same as <see cref="Mean(IEnumerable{double})"/></summary>
        /// <seealso cref="Mean(IEnumerable{double})"/>
        public static double Average(IEnumerable<double> sample)
        {
            return Mean(sample);
        }


        /// <summary>Calculates and returns mean value of its arguments (or elements of the specified double array).</summary>
        /// <param name="sampleElements">Either array of double numbers or multiple number parameters whose standard deviation estimate is calculated.</param>
        /// <exception cref="ArgumentException">When number of elements or parameters is less than 2 or null argumennt is specified.</exception>
        public static double StandardDeviation(params double[] sampleElements)
        {
            IEnumerable<double> samp = sampleElements;
            return StandardDeviation(samp);
        }

        /// <summary>Returns estimation of the standard deviation of a random value based on the specified sample.</summary>
        /// <param name="sample">Collection containing all observations of the sample.</param>
        /// <exception cref="ArgumentException">When number of elements is less than 2 or null argumennt is specified.</exception>
        public static double StandardDeviation(IEnumerable<double> sample)
        {
            if (sample == null)
                throw new ArgumentException("Sample not specified (null reference).");
            double ret = 0;
            int numElements = 0;
            double mean = Mean(sample); 
            foreach (double element in sample)
            {
                ret += (element-mean)*(element-mean);
                ++numElements;
            }
            if (numElements < 2)
                throw new ArgumentException("Can not calculate mean value of a sample with less than 2 elements.");
            return Math.Sqrt(ret / (numElements - 1));
        }
        


        #endregion Statistics



    }  // class M


    /// <summary>Defines some mathematical functions to be used in derived classes. In addition to 
    /// functions defined in the <see cref="M"/> class, functions are defined under other names, and 
    /// some additional functions are also defined.
    /// <para>* Standard mathematical functions  and constants with short names are implemented, e.g. sin() instesd of Math.Sin().</para>
    /// <para>  ** These functions are static such that they can be used out of the derived classes, too.</para>
    /// <para>    *** In particular, some script classes derive from this one, in order to use simple-named mathematical functions.</para>
    /// <para>  ** Some functions are defined with several names, in order to reduce probability of errors in scripts.</para></summary>
    public abstract partial class MExt : M
    {

        #region Constants

        /// <summary>Natural logarithmic base.</summary>
        public static double E
        { get { return Math.E; } }

        /// <summary>Ratio of the circumference of a circle to its diameter.</summary>
        public static double Pi
        { get { return Math.PI; } }

        /// <summary>Zero (value 0.0).</summary>
        public static double Zero
        { get { return 0.0; } }

        /// <summary>One (value 1.0).</summary>
        public static double One
        { get { return 1.0; } }

        #endregion Constants


        #region Functions

        #region SignFunctions

        /// <summary>Absolute value.</summary>
        public static double Abs(double a)
        { return Math.Abs(a); }

        /// <summary>Returns a value indicating the sign of a number.</summary>
        public static double Sign(double a)
        { return Math.Sign(a); }

        /// <summary>Returns a value indicating the sign of a number.</summary>
        public static double sgn(double a)
        { return Math.Sign(a); }

        #endregion SignFunctions


        #region RoundingFunctions

        /// <summary>Returns the smallest integral value that is greater than or equal to the specified decimal number.</summary>
        public static double Ceiling(double a)
        { return Math.Ceiling(a); }

        /// <summary>Returns the smallest integral value that is greater than or equal to the specified decimal number.</summary>
        public static double ceiling(double a)
        { return Math.Ceiling(a); }

        /// <summary>Returns the largest integer less than or equal to the specified number.</summary>
        public static double Floor(double a)
        { return Math.Floor(a); }

        /// <summary>Calculates the integral part of a specified double-precision floating-point number.</summary>
        public static double Truncate(double a)
        { return Math.Truncate(a); }

        /// <summary>Calculates the integral part of a specified number.</summary>
        public static double truncate(double a)
        { return Math.Truncate(a); }

        #endregion RoundingFunctions


        #region MinMaxFunctions

        /// <summary>Smallest of two numbers.</summary>
        public static double Min(double a, double b) { return (a <= b ? a : b); }

        /// <summary>Largest of two numbers.</summary>
        public static double Max(double a, double b) { return (a >= b ? a : b); }

        /// <summary>Smallest of three numbers.</summary>
        public static double Min(double a, double b, double c)
        {
            a = (a <= b ? a : b);
            return (a <= c ? a : c);
        }

        /// <summary>Largest of three numbers.</summary>
        public static double Max(double a, double b, double c)
        {
            a = (a >= b ? a : b);
            return (a >= c ? a : c);
        }

        /// <summary>Smallest of four numbers.</summary>
        public static double Min(double a, double b, double c, double d)
        {
            a = (a <= b ? a : b);
            b = (c <= d ? c : d);
            return (a <= b ? a : b);
        }

        /// <summary>Largest of four numbers.</summary>
        public static double Max(double a, double b, double c, double d)
        {
            a = (a >= b ? a : b);
            b = (c >= d ? c : d);
            return (a >= b ? a : b);
        }

        /// <summary>Smallest of five numbers.</summary>
        public static double Min(double a, double b, double c, double d, double e)
        {
            a = (a <= b ? a : b);
            b = (c <= d ? c : d);
            a = (a <= b ? a : b);
            return (a <= e ? a : e);
        }

        /// <summary>Largest of five numbers.</summary>
        public static double Max(double a, double b, double c, double d, double e)
        {
            a = (a >= b ? a : b);
            b = (c >= d ? c : d);
            a = (a >= b ? a : b);
            return (a >= e ? a : e);
        }

        /// <summary>Smallest of six numbers.</summary>
        public static double Min(double a, double b, double c, double d, double e, double f)
        {
            a = (a <= b ? a : b);
            b = (c <= d ? c : d);
            a = (a <= b ? a : b);
            b = (e <= f ? e : f);
            return (a <= b ? a : b);
        }

        /// <summary>Largest of six numbers.</summary>
        public static double Max(double a, double b, double c, double d, double e, double f)
        {
            a = (a >= b ? a : b);
            b = (c >= d ? c : d);
            a = (a >= b ? a : b);
            b = (e >= f ? e : f);
            return (a >= b ? a : b);
        }

        /// <summary>Minimal of the specified values.</summary>
        public static double Min(params double[] numbers)
        {
            if (numbers != null)
            {
                int num = numbers.Length;
                if (num > 0)
                {
                    double ret = numbers[0];
                    for (int i = 1; i < num; ++i)
                    {
                        if (numbers[i] < ret)
                            ret = numbers[i];
                    }
                    return ret;
                }
            }
            throw new ArgumentException("Minimal among several values: no values specified.");
        }

        /// <summary>Maximal of the specified values.</summary>
        public static double Max(params double[] numbers)
        {
            if (numbers != null)
            {
                int num = numbers.Length;
                if (num > 0)
                {
                    double ret = numbers[0];
                    for (int i = 1; i < num; ++i)
                    {
                        if (numbers[i] > ret)
                            ret = numbers[i];
                    }
                    return ret;
                }
            }
            throw new ArgumentException("Maximal among several values: no values specified.");
        }

        #endregion MinMaxFunctions


        #region ProductsAndSums

        ///// <summary>Sum of the specified values.</summary>
        //public static double Sum(params double[] numbers)
        //{
        //    if (numbers != null)
        //    {
        //        int num = numbers.Length;
        //        if (num > 0)
        //        {
        //            double ret = 0;
        //            for (int i = 0; i < num; ++i)
        //            {
        //                ret += numbers[i];
        //            }
        //            return ret;
        //        }
        //    }
        //    throw new ArgumentException("Sum of several values: no values specified.");
        //}

        ///// <summary>Product of the specified values.</summary>
        //public static double Product(params double[] numbers)
        //{
        //    if (numbers != null)
        //    {
        //        int num = numbers.Length;
        //        if (num > 0)
        //        {
        //            double ret = 1;
        //            for (int i = 0; i < num; ++i)
        //            {
        //                ret *= numbers[i];
        //            }
        //            return ret;
        //        }
        //    }
        //    throw new ArgumentException("Product of several values: no values specified.");
        //}

        /// <summary>Product of the specified values.</summary>
        public static double product(params double[] numbers)
        {
            if (numbers != null)
            {
                int num = numbers.Length;
                if (num > 0)
                {
                    double ret = 1;
                    for (int i = 0; i < num; ++i)
                    {
                        ret *= numbers[i];
                    }
                    return ret;
                }
            }
            throw new ArgumentException("Product of several values: no values specified.");
        }

        #endregion ProductsAndSums


        #region PowersAndRoots

        /// <summary>Returns a specified number raised to the specified power.</summary>
        public static double Pow(double a, double b)
        { return Math.Pow(a, b); }

        /// <summary>Returns the square of a specified number.</summary>
        public static double Sqr(double a)
        { return a * a; }

        /// <summary>Returns the square of a specified number.</summary>
        public static double Cube(double a)
        { return a * a * a; }

        /// <summary>Square.</summary>
        public static double Pow2(double x) { return x * x; }

        /// <summary>3rd power.</summary>
        public static double Pow3(double x) { return x * x * x; }

        /// <summary>4th power.</summary>
        public static double Pow4(double x) { double sq = x * x; return sq * sq; }

        /// <summary>5th power.</summary>
        public static double Pow5(double x) { double sq = x * x; return sq * sq * x; }

        /// <summary>6th power.</summary>
        public static double Pow6(double x) { double sq = x * x; return sq * sq * sq; }

        /// <summary>Returns the square root of a specified number.</summary>
        public static double Sqrt(double a) { return Math.Sqrt(a); }

        /// <summary>Returns the square root of a specified number.</summary>
        public static double Root2(double a) { return Math.Sqrt(a); }

        /// <summary>Returns the cubic root of a specified number.</summary>
        public static double Root3(double a) { return Math.Pow(a, 1.0 / 3.0); }


        #endregion PowersAndRoots


        #region RandomNumbers

        /// <summary>Returns a uniformly distributed random number greater than or equal to 0.0, 
        /// and less or equal than 1.0.</summary>
        public static double Rand()
        {
            return RandomGenerator.Global.NextDoubleInclusive();
        }

        /// <summary>Returns a uniformly distributed random number greater than or equal to min, 
        /// and less or equal than max.</summary>
        public static double Rand(double min, double max)
        {
            return RandomGenerator.Global.NextDoubleInclusive(min, max);
        }


        /// <summary>Returns a Gaussian distributed random number with the specified mean and standard deviation.</summary>
        /// <param name="mean">Mean value of the distribution.</param>
        /// <param name="standardDeviation">Standard deviation of the distribution.</param>
        public static double RandGauss(double mean, double standardDeviation)
        {
            return RandomGaussian.Global.NextGaussian(mean, standardDeviation);
        }

        #endregion RandomNumbers


        #region LogExpFunctions

        /// <summary>Returns e raised to the specified power.</summary>
        public static double Exp(double a)
        { return Math.Exp(a); }

        /// <summary>Returns the natural (base e) logarithm of a specified number.</summary>
        public static double Log(double a)
        { return Math.Log(a); }

        /// <summary>Returns the base 10 logarithm of a specified number.</summary>
        public static double Log10(double a)
        { return Math.Log10(a); }

        /// <summary>Returns the base 2 logarithm of a specified number.</summary>
        public static double Log2(double a)
        { return Math.Log(a, 2); }

        /// <summary>Returns the logarithm of a specified number in a specified base.</summary>
        /// <param name="a">Number whose logarithm is returned.</param>
        /// <param name="logBase">Base of the logarithm.</param>
        public static double Log(double a, double logBase)
        { return Math.Log(a, logBase); }

        #endregion LogExpFunctions


        #region AngleFunctions

        /// <summary>Converts angle in radians to angle in degrees and returns it.</summary>
        public static double Degrees(double x) { return ((x) * 180 / Math.PI); }

        /// <summary>Converts angle in degrees to angle in radians and returns it.</summary>
        public static double Radians(double x) { return ((x) * Math.PI / 180); }

        #endregion AngleFunctions


        #region TrigonometricFunctions

        /// <summary>Returns the sine of the specified angle.</summary>
        public static double Sin(double a)
        { return Math.Sin(a); }

        /// <summary>Returns the cosine of the specified angle.</summary>
        public static double Cos(double a)
        { return Math.Cos(a); }

        /// <summary>Returns the tangent of the specified angle.</summary>
        public static double Tan(double a)
        { return Math.Tan(a); }

        /// <summary>Returns the tangent of the specified angle.</summary>
        public static double tan(double a)
        { return Math.Tan(a); }

        /// <summary>Returns cotangent of the specified angle.</summary>
        public static double Cot(double x) { return (1 / Math.Tan(x)); }

        #endregion TrigonometricFunctions


        #region InverseTrigonometricFunctions

        /// <summary>Returns the angle whose cosine is the specified number.</summary>
        public static double Acos(double a)
        { return Math.Acos(a); }

        /// <summary>Returns the angle whose cosine is the specified number.</summary>
        public static double acos(double a)
        { return Math.Acos(a); }

        /// <summary>Returns the angle whose sine is the specified number.</summary>
        public static double Asin(double a)
        { return Math.Asin(a); }

        /// <summary>Returns the angle whose sine is the specified number.</summary>
        public static double asin(double a)
        { return Math.Asin(a); }

        /// <summary>Returns the angle whose tangent is the specified number.</summary>
        public static double Atan(double a)
        { return Math.Atan(a); }

        /// <summary>Returns the angle whose tangent is the specified number.</summary>
        public static double atan(double a)
        { return Math.Atan(a); }

        /// <summary>Returns the angle whose tangent is the specified number.</summary>
        public static double arctan(double a)
        { return Math.Atan(a); }

        /// <summary>Arc cotangent, inverse of 1/tan(x).</summary>
        public static double Acot(double x) { return ((x) == 0 ? 0.5 * Math.PI : Math.Atan(1 / (x))); }

        /// <summary>Arc cotangent, inverse of 1/tan(x).</summary>
        public static double acot(double x) { return ((x) == 0 ? 0.5 * Math.PI : Math.Atan(1 / (x))); }

        /// <summary>Arc cotangent, inverse of 1/tan(x).</summary>
        public static double ArcCot(double x) { return ((x) == 0 ? 0.5 * Math.PI : Math.Atan(1 / (x))); }

        /// <summary>Arc cotangent, inverse of 1/tan(x).</summary>
        public static double arccot(double x) { return ((x) == 0 ? 0.5 * Math.PI : Math.Atan(1 / (x))); }

        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        public static double Atan2(double a, double b)
        { return Math.Atan2(a, b); }

        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        public static double atan2(double a, double b)
        { return Math.Atan2(a, b); }

        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        public static double arctan2(double a, double b)
        { return Math.Atan2(a, b); }

        #endregion InverseTrigonometricFunctions


        #region HyperbolicFunctions

        ///// <summary>Returns the hyperbolic sine of the specified angle.</summary>
        //public static double Sinh(double a)
        //{ return Math.Sinh(a); }

        ///// <summary>Returns the hyperbolic sine of the specified angle.</summary>
        //public static double sinh(double a)
        //{ return Math.Sinh(a); }

        /// <summary>Returns the hyperbolic cosine of the specified angle.</summary>
        public static double Cosh(double a)
        { return Math.Cosh(a); }

        /// <summary>Returns the hyperbolic cosine of the specified angle.</summary>
        public static double cosh(double a)
        { return Math.Cosh(a); }

        /// <summary>Returns the hyperbolic tangent of the specified angle.</summary>
        public static double Tanh(double a)
        { return Math.Tanh(a); }

        /// <summary>Returns the hyperbolic tangent of the specified angle.</summary>
        public static double tanh(double a)
        { return Math.Tanh(a); }

        /// <summary>Hyperblic cotangent, 1/Math.Tanh.</summary>
        public static double Coth(double x) { return (1 / Math.Tanh(x)); }

        /// <summary>Hyperblic cotangent, 1/Math.Tanh.</summary>
        public static double coth(double x) { return (1 / Math.Tanh(x)); }

        #endregion HyperbolicFunctions


        #region InverseHyperbolicFunctions

        /// <summary>Inverse hyperbolic sine.</summary>
        public static double Arsinh(double x) { return Math.Log((x) + Math.Sqrt(pow2(x) + 1)); }

        /// <summary>Inverse hyperbolic cosine.</summary>
        public static double Arcosh(double x) { return Math.Log((x) + Math.Sqrt(pow2(x) - 1)); }

        /// <summary>Inverse hyperbolic tangent.</summary>
        public static double Artanh(double x) { return (0.5 * Math.Log((1 + (x)) / (1 - (x)))); }

        /// <summary>Inverse hyperbolic cotangent.</summary>
        public static double Arcoth(double x) { return (0.5 * Math.Log(((x) + 1) / ((x) - 1))); }

        #endregion InverseHyperbolicFunctions

        #endregion Functions


        #region Combinatorics

        /// <summary>Returns factorial of the specified number.</summary>
        public static long Factorial(int factor)
        {
            return fac(factor);
        }

        /// <summary>Returns factorial of the specified number.</summary>
        public static long factorial(int factor)
        {
            return fac(factor);
        }

        /// <summary>Returns binomial coefficient <paramref name="n"/> over <paramref name="k"/>.</summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        public static long BinomialCoefficient(long n, long k)
        {
            return binomial(n, k);
        }


        #endregion Combinatorics

    }

}




namespace IG.Num.Experimental
{



    /// <summary>Extension of type double by a struct.
    /// Provides binary operaror ^ (left operand raised to the power of right operand).</summary>
    [Obsolete("")]
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public struct xdouble
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {

        public xdouble(double value) { Value = value; }

        /// <summary>Value.</summary>
        public double Value;

        /// <summary>Returns true if the specified number equals the current number, 
        /// and false otherwise.</summary>
        /// <param name="a">Number that is compared.</param>
        /// <returns>True if <paramref name="a"/> equals the current number, false otherwise.</returns>
        public bool Equals(xdouble a)
        { return a.Value == this.Value; }

        /// <summary>Returns true if the specified object equals the current number, 
        /// and false otherwise.</summary>
        /// <param name="obj">Object that is compared.</param>
        /// <returns>True if <paramref name="obj"/> equals the current number, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is xdouble)
                return Equals((xdouble)obj);
            return false;
        }

        /// <summary>Hath function for the <see cref="xdouble"/> type.</summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>Conversion form type <see cref="xdouble"/> to double.
        /// Defined as explicit because we don't want to modify operators for type double.</summary>
        public static explicit operator double(xdouble a)
        { return a.Value; }


        /// <summary>Implicit conversion form type double to <see cref="xdouble"/>.</summary>
        public static implicit operator xdouble(double a)
        { return new xdouble(a); }

        /// <summary>Implicit conversion form type float to <see cref="xdouble"/>.</summary>
        public static implicit operator xdouble(float a)
        { return new xdouble(a); }

        /// <summary>Implicit conversion form type int to <see cref="xdouble"/>.</summary>
        public static implicit operator xdouble(int a)
        { return new xdouble(a); }


        /// <summary>First operand raised to the power of the second operand.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand - power.</param>
        public static xdouble operator ^(xdouble a, xdouble b)
        { return Math.Pow(a.Value, b.Value); }

        public static xdouble operator +(xdouble a, xdouble b)
        { return a.Value + b.Value; }

        public static xdouble operator -(xdouble a, xdouble b)
        { return a.Value - b.Value; }

        public static xdouble operator *(xdouble a, xdouble b)
        { return a.Value * b.Value; }

        public static xdouble operator /(xdouble a, xdouble b)
        { return a.Value / b.Value; }

        public static xdouble operator %(xdouble a, xdouble b)
        { return a.Value % b.Value; }

        public static xdouble operator +(xdouble a)
        { return a; }

        public static xdouble operator -(xdouble a)
        { return -(a.Value); }

        public static xdouble operator ++(xdouble a)
        { return ++(a.Value); }

        public static xdouble operator --(xdouble a)
        { return --(a.Value); }

        public static bool operator <(xdouble a, xdouble b)
        { return (a.Value < b.Value); }

        public static bool operator <=(xdouble a, xdouble b)
        { return (a.Value <= b.Value); }

        public static bool operator >(xdouble a, xdouble b)
        { return (a.Value > b.Value); }

        public static bool operator >=(xdouble a, xdouble b)
        { return (a.Value >= b.Value); }

        public static bool operator ==(xdouble a, xdouble b)
        { return (a.Value == b.Value); }

        public static bool operator !=(xdouble a, xdouble b)
        { return (a.Value != b.Value); }



        /// <summary>Writes results of some example operations where type xdouble is inbolved.</summary>
        public static void Example()
        {

            int ia = 2;
            // int ib = 3;
            int ir;
            double a = 1.1;
            double b = 1.2;
            double r;
            xdouble xa = 1.1;
            xdouble xb = 1.2;
            xdouble xr;


            xr = a;
            Console.WriteLine("x{0} = {1}", xr, a);

            r = (double)xa;
            Console.WriteLine("{0} = (double) x{1}", r, xa);

            r = (double)(a + xb);
            Console.WriteLine("(double) ({0} + x{1}) = {2}", a, xb, r);

            xr = a + xb;
            Console.WriteLine("{0} + x{1} = x{2}", a, xb, xr);

            xr = b % xa;
            Console.WriteLine("{0} % x{1} = x{2}", b, xa, xr);

            xr = ia * xa;
            Console.WriteLine("i{0} * x{1} = x{2}", ia, xa, xr);

            ir = (int)xa;
            Console.WriteLine("i{0} = (int) x{1}", ir, xa);

            xr = xa ^ ia;

            xr = xa ^ xb;
            Console.WriteLine("x{0} ^ x{1} = x{2}", xa, xb, xr);

            xr = (xdouble)a ^ b;
            Console.WriteLine("(xdouble) {0} ^ {1} = x{2}", a, b, xr);

        }
    } 



    /// <summary>Example class that derives from the <see cref="M"/> class.
    /// Implements a method that uses basic mathematical functions implemented in M.</summary>
    /// $A Igor xx Jul11;
    /// <remarks>
    /// TODO: Join the FuncMath class with this class (incorporate that class and abolish it)!!
    /// </remarks>
    [Obsolete("Experimental functionality, should not be used in produciton code!")]
    public class ExampleMathClassExperimental : MX
    {

        public ExampleMathClassExperimental()
            : base()
        { }

        /// <summary>Method in which comfortable mathematic functions form the base class <see cref="M"/> are used.</summary>
        /// <param name="x">Argument of a real-valued function.</param>
        /// <returns>Some function of the argument.</returns>
        public double f(double x)
        {
            xdouble ret = sin(x + E ^ 2 + 1.2 ^ 3.5);
            xdouble xr = 1.34 ^ 2.55 + zero;
            double r = (double)(3.5 ^ 2.55 + zero);
            return (double)ret;
        }

        /// <summary>A nested class that does not derive from the <see cref="M"/> class, 
        /// but still statidc functions from that class can be used because it is embedded in that class.</summary>
        public class Nested
        {

            /// <summary>Method in which comfortable mathematic functions form the base class <see cref="M"/> are used.</summary>
            /// <param name="x">Argument of a real-valued function.</param>
            /// <returns>Some function of the argument.</returns>
            public double f(double x)
            {
                xdouble ret = sin(x + E ^ 2 + 1.2 ^ 3.5);
                xdouble xr = 1.34 ^ 2.55 + zero;
                double r = (double)(3.5 ^ 2.55 + zero);
                return (double)ret;
            }

        }

    } // class ExampleClass


    /// <summary>Defines some mathematical functions to be used in derived classes. Beside the 
    /// auxiliary functions already in class <see cref="M"/>, this class also defines functions
    /// that use type <see cref="xdouble"/> instead of double.
    /// <para>* Standard mathematical functions  and constants with short names are implemented, e.g. sin() instesd of Math.Sin().</para>
    /// <para>  ** These functions are static such that they can be used out of the derived classes, too.</para>
    /// <para>  ** Some functions are defined with several names, in order to reduce probability of errors in scripts.</para>
    /// <para>  ** Functions operate on the type <see cref="xdouble"/> rather than <see cref="double"/>, 
    /// such that operator ^ can be used in expressions.</para></summary>
    [Obsolete("Experimental functionality, should not be used in produciton code!")]
    public abstract partial class MX : MExt
    {

        #region xdouble

        /// <summary>Natural logarithmic base.</summary>
        public static new xdouble E
        { get { return Math.E; } }

        /// <summary>Natural logarithmic base.</summary>
        public static new xdouble e
        { get { return Math.E; } }

        /// <summary>Ratio of the circumference of a circle to its diameter.</summary>
        public static new xdouble Pi
        { get { return Math.PI; } }

        /// <summary>Ratio of the circumference of a circle to its diameter.</summary>
        public static new xdouble pi
        { get { return Math.PI; } }

        /// <summary>Zero (value 0.0).</summary>
        public static new xdouble Zero
        { get { return 0.0; } }

        /// <summary>Zero (value 0.0).</summary>
        public static new xdouble zero
        { get { return 0.0; } }

        /// <summary>One (value 1.0).</summary>
        public static new xdouble One
        { get { return 1.0; } }

        /// <summary>One (value 1.0).</summary>
        public static new xdouble one
        { get { return 1.0; } }

        /// <summary>Absolute value.</summary>
        public static xdouble Abs(xdouble a)
        { return Math.Abs(a.Value); }

        /// <summary>Absolute value.</summary>
        public static xdouble abs(xdouble a)
        { return Math.Abs(a.Value); }

        /// <summary>Returns the angle whose cosine is the specified number.</summary>
        public static xdouble Acos(xdouble a)
        { return Math.Acos(a.Value); }

        /// <summary>Returns the angle whose cosine is the specified number.</summary>
        public static xdouble acos(xdouble a)
        { return Math.Acos(a.Value); }

        /// <summary>Returns the angle whose cosine is the specified number.</summary>
        public static xdouble arccos(xdouble a)
        { return Math.Acos(a.Value); }

        /// <summary>Returns the angle whose sine is the specified number.</summary>
        public static xdouble Asin(xdouble a)
        { return Math.Asin(a.Value); }

        /// <summary>Returns the angle whose sine is the specified number.</summary>
        public static xdouble asin(xdouble a)
        { return Math.Asin(a.Value); }

        /// <summary>Returns the angle whose sine is the specified number.</summary>
        public static xdouble arcsin(xdouble a)
        { return Math.Asin(a.Value); }

        /// <summary>Returns the angle whose tangent is the specified number.</summary>
        public static xdouble Atan(xdouble a)
        { return Math.Atan(a.Value); }

        /// <summary>Returns the angle whose tangent is the specified number.</summary>
        public static xdouble atan(xdouble a)
        { return Math.Atan(a.Value); }

        /// <summary>Returns the angle whose tangent is the specified number.</summary>
        public static xdouble arctan(xdouble a)
        { return Math.Atan(a.Value); }

        /// <summary>Returns the angle whose tangent is the specified number.</summary>
        public static xdouble arctg(xdouble a)
        { return Math.Atan(a.Value); }

        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        public static xdouble Atan2(xdouble a, xdouble b)
        { return Math.Atan2(a.Value, b.Value); }

        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        public static xdouble atan2(xdouble a, xdouble b)
        { return Math.Atan2(a.Value, b.Value); }

        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        public static xdouble arctan2(xdouble a, xdouble b)
        { return Math.Atan2(a.Value, b.Value); }

        /// <summary>Returns the angle whose tangent is the quotient of two specified numbers.</summary>
        public static xdouble arctg2(xdouble a, xdouble b)
        { return Math.Atan2(a.Value, b.Value); }

        /// <summary>Returns the smallest integral value that is greater than or equal to the specified decimal number.</summary>
        public static xdouble Ceiling(xdouble a)
        { return Math.Ceiling(a.Value); }

        /// <summary>Returns the smallest integral value that is greater than or equal to the specified decimal number.</summary>
        public static xdouble ceiling(xdouble a)
        { return Math.Ceiling(a.Value); }

        /// <summary>Returns the smallest integral value that is greater than or equal to the specified decimal number.</summary>
        public static xdouble ceil(xdouble a)
        { return Math.Ceiling(a.Value); }

        /// <summary>Returns the cosine of the specified angle.</summary>
        public static xdouble Cos(xdouble a)
        { return Math.Cos(a.Value); }

        /// <summary>Returns the cosine of the specified angle.</summary>
        public static xdouble cos(xdouble a)
        { return Math.Cos(a.Value); }

        /// <summary>Returns the hyperbolic cosine of the specified angle.</summary>
        public static xdouble Cosh(xdouble a)
        { return Math.Cosh(a.Value); }

        /// <summary>Returns the hyperbolic cosine of the specified angle.</summary>
        public static xdouble cosh(xdouble a)
        { return Math.Cosh(a.Value); }

        /// <summary>Returns the hyperbolic cosine of the specified angle.</summary>
        public static xdouble ch(xdouble a)
        { return Math.Cosh(a.Value); }

        /// <summary>Returns e raised to the specified power.</summary>
        public static xdouble Exp(xdouble a)
        { return Math.Exp(a.Value); }

        /// <summary>Returns e raised to the specified power.</summary>
        public static xdouble exp(xdouble a)
        { return Math.Exp(a.Value); }

        /// <summary>Returns the largest integer less than or equal to the specified number.</summary>
        public static xdouble Floor(xdouble a)
        { return Math.Floor(a.Value); }

        /// <summary>Returns the largest integer less than or equal to the specified number.</summary>
        public static xdouble floor(xdouble a)
        { return Math.Floor(a.Value); }

        /// <summary>Returns the natural (base e) logarithm of a specified number.</summary>
        public static xdouble Log(xdouble a)
        { return Math.Log(a.Value); }

        /// <summary>Returns the natural (base e) logarithm of a specified number.</summary>
        public static xdouble log(xdouble a)
        { return Math.Log(a.Value); }

        /// <summary>Returns the natural (base e) logarithm of a specified number.</summary>
        public static xdouble ln(xdouble a)
        { return Math.Log(a.Value); }

        /// <summary>Returns the base 10 logarithm of a specified number.</summary>
        public static xdouble Log10(xdouble a)
        { return Math.Log10(a.Value); }

        /// <summary>Returns the base 10 logarithm of a specified number.</summary>
        public static xdouble log10(xdouble a)
        { return Math.Log10(a.Value); }

        /// <summary>Returns the base 10 logarithm of a specified number.</summary>
        public static xdouble lg(xdouble a)
        { return Math.Log10(a.Value); }

        /// <summary>Returns the base 2 logarithm of a specified number.</summary>
        public static xdouble Log2(xdouble a)
        { return Math.Log(a.Value, 2); }

        /// <summary>Returns the base 2 logarithm of a specified number.</summary>
        public static xdouble log2(xdouble a)
        { return Math.Log(a.Value, 2); }

        /// <summary>Returns the logarithm of a specified number in a specified base.</summary>
        /// <param name="a">Number whose logarithm is returned.</param>
        /// <param name="logBase">Base of the logarithm.</param>
        public static xdouble Log(xdouble a, xdouble logBase)
        { return Math.Log(a.Value, logBase.Value); }

        /// <summary>Returns the logarithm of a specified number in a specified base.</summary>
        /// <param name="a">Number whose logarithm is returned.</param>
        /// <param name="logBase">Base of the logarithm.</param>
        public static xdouble log(xdouble a, xdouble logBase)
        { return Math.Log(a.Value, logBase.Value); }

        /// <summary>Returns the larger of two numbers.</summary>
        public static xdouble Max(xdouble a, xdouble b)
        { return Math.Max(a.Value, b.Value); }

        /// <summary>Returns the larger of two numbers.</summary>
        public static xdouble max(xdouble a, xdouble b)
        { return Math.Max(a.Value, b.Value); }

        /// <summary>Returns the smaller of two numbers.</summary>
        public static xdouble Min(xdouble a, xdouble b)
        { return Math.Min(a.Value, b.Value); }

        /// <summary>Returns the smaller of two numbers.</summary>
        public static xdouble min(xdouble a, xdouble b)
        { return Math.Min(a.Value, b.Value); }

        /// <summary>Returns a specified number raised to the specified power.</summary>
        public static xdouble Pow(xdouble a, xdouble b)
        { return Math.Pow(a.Value, b.Value); }

        /// <summary>Returns a specified number raised to the specified power.</summary>
        public static xdouble pow(xdouble a, xdouble b)
        { return Math.Pow(a.Value, b.Value); }

        /// <summary>Returns a value indicating the sign of a number.</summary>
        public static xdouble Sign(xdouble a)
        { return Math.Sign(a.Value); }

        /// <summary>Returns a value indicating the sign of a number.</summary>
        public static xdouble sign(xdouble a)
        { return Math.Sign(a.Value); }

        /// <summary>Returns a value indicating the sign of a number.</summary>
        public static xdouble sgn(xdouble a)
        { return Math.Sign(a.Value); }

        /// <summary>Returns the sine of the specified angle.</summary>
        public static xdouble Sin(xdouble a)
        { return Math.Sin(a.Value); }

        /// <summary>Returns the sine of the specified angle.</summary>
        public static xdouble sin(xdouble a)
        { return Math.Sin(a.Value); }

        /// <summary>Returns the hyperbolic sine of the specified angle.</summary>
        public static xdouble Sinh(xdouble a)
        { return Math.Sinh(a.Value); }

        /// <summary>Returns the hyperbolic sine of the specified angle.</summary>
        public static xdouble sinh(xdouble a)
        { return Math.Sinh(a.Value); }

        /// <summary>Returns the hyperbolic sine of the specified angle.</summary>
        public static xdouble sh(xdouble a)
        { return Math.Sinh(a.Value); }

        /// <summary>Returns the square of a specified number.</summary>
        public static xdouble Sqr(xdouble a)
        { return a.Value * a.Value; }

        /// <summary>Returns the square of a specified number.</summary>
        public static xdouble sqr(xdouble a)
        { return a.Value * a.Value; }

        /// <summary>Returns the square of a specified number.</summary>
        public static xdouble Cube(xdouble a)
        { return a.Value * a.Value * a.Value; }

        /// <summary>Returns the square of a specified number.</summary>
        public static xdouble cube(xdouble a)
        { return a.Value * a.Value * a.Value; }

        /// <summary>Returns the square root of a specified number.</summary>
        public static xdouble Sqrt(xdouble a)
        { return Math.Sqrt(a.Value); }

        /// <summary>Returns the square root of a specified number.</summary>
        public static xdouble sqrt(xdouble a)
        { return Math.Sqrt(a.Value); }

        /// <summary>Returns the tangent of the specified angle.</summary>
        public static xdouble Tan(xdouble a)
        { return Math.Tan(a.Value); }

        /// <summary>Returns the tangent of the specified angle.</summary>
        public static xdouble tan(xdouble a)
        { return Math.Tan(a.Value); }

        /// <summary>Returns the tangent of the specified angle.</summary>
        public static xdouble tg(xdouble a)
        { return Math.Tan(a.Value); }

        /// <summary>Returns the hyperbolic tangent of the specified angle.</summary>
        public static xdouble Tanh(xdouble a)
        { return Math.Tanh(a.Value); }

        /// <summary>Returns the hyperbolic tangent of the specified angle.</summary>
        public static xdouble tanh(xdouble a)
        { return Math.Tanh(a.Value); }

        /// <summary>Returns the hyperbolic tangent of the specified angle.</summary>
        public static xdouble th(xdouble a)
        { return Math.Tanh(a.Value); }

        /// <summary>Calculates the integral part of a specified double-precision floating-point number.</summary>
        public static xdouble Truncate(xdouble a)
        { return Math.Truncate(a.Value); }

        /// <summary>Calculates the integral part of a specified number.</summary>
        public static xdouble truncate(xdouble a)
        { return Math.Truncate(a.Value); }

        /// <summary>Calculates the integral part of a specified number.</summary>
        public static xdouble trunc(xdouble a)
        { return Math.Truncate(a.Value); }

        #endregion xdouble

    }  // Class MX

}
