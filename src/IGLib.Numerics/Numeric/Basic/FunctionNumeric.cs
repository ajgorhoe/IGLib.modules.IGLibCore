// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

// NUMERICAL ALGORITHMS FOR FUNCTIONS (integration, differentiation, etc.)


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{
    public static partial class Numeric
    {

        #region Integration


        /// <summary>Returns numerical integral of a function calculated by the composite Simpson's rule.
        /// Error: -(to-from)*h^4*f(4)(mi)/180</summary>
        /// <param name="f">Function to be integrated.</param>
        /// <param name="from">Lower bound of integration interval.</param>
        /// <param name="to">Upper bound of integration interval.</param>
        /// <param name="numintervals">Number of sub-intervals (1 less than the numbe of evaluation points).</param>
        /// <returns>Integral approximation.</returns>
        public static double IntegralSimpson(DlgFunctionValue f, double from, double to, int numintervals)
        {
            double ret = 0;
            if (numintervals < 2)
                numintervals = 2;
            if (numintervals % 2 != 0)
                ++numintervals;
            double h = (to - from) / numintervals;
            ret += f(from) + f(to);
            double s = 0;
            for (int j = 1; j <= numintervals / 2 - 1; ++j)
                s += f(from + (2 * j) * h);
            ret += 2 * s;
            s = 0;
            for (int j = 1; j <= numintervals / 2; ++j)
                s += f(from + (2 * j - 1) * h);
            ret += 4 * s;
            ret *= (h / 3);
            return ret;
        }

        /// <summary>Returns numerical integral of a function calculated by the Simpson's rule.
        /// Function is specified as a table of values in equidistant points.
        /// Intervals need to be equidistant, minimal number of points is 3 and must be odd.
        /// Error: -(to-from)*h^4*f(4)(mi)/180</summary>
        /// <param name="tabx">Table of equidistant points at which function is evaluated.</param>
        /// <param name="tabf">Table of function values in points contained in tabx.</param>
        /// <returns>Integral approximation.</returns>
        public static double IntegralSimpson(double[] tabx, double[] tabf)
        {
            double tolequidistant = 0.01;
            if (tabx==null)
                throw new ArgumentNullException("Simpson's integration: table of nodes is not specified (null reference).");
            if (tabf==null)
                throw new ArgumentNullException("Simpson's integration: table of function values is not specified (null reference).");
            if (tabf.Length != tabx.Length)
                throw new ArgumentException("Simpson's integration: table of function values has different length ("
                    + tabf.Length + ") than table of nodes (" + tabx.Length + ").");
            int numintervals = tabx.Length - 1;
            if (numintervals < 2)
                throw new ArgumentException("Simpson's integration: number of intervals is less than 2.");
            if (numintervals % 2 != 0)
                throw new ArgumentException("Simpson's integration: number of intervals is odd.") ;
            double ret = 0;
            double h1, h2, error;
            bool increasing = (tabx[1] > tabx[0]);  // points ordered by increasing x  
            int i = 0;
            while (i < numintervals)
            {
                // Verify consistence of ordering:
                if (increasing)
                {
                    if (tabx[i] > tabx[i + 1] || tabx[i + 1] > tabx[i + 2])
                        throw new ArgumentException("Simpson's integration: ordering of nodes is not consistent at interval "
                        + i.ToString() + "." );
                } else
                {
                    if (tabx[i] < tabx[i + 1] || tabx[i + 1] < tabx[i + 2])
                        throw new ArgumentException("Simpson's integration: ordering of nodes is not consistent at interval "
                        + i.ToString() + "." );
                }
                // Verify equidistance of intervals:
                h1 = tabx[i + 1] - tabx[i];
                h2 = tabx[i + 2] - tabx[i+1];
                error = Math.Abs((h1-h2)/h1);
                if (error > tolequidistant)
                    throw new ArgumentException("Simpson's integration: intervals are not equidistant at interval "
                        + i.ToString() + ". Relative error: " + error + "." );
                ret += (tabf[i] + 4 * tabf[i + 1] + tabf[i + 2])*(tabx[i+2]-tabx[i])/6.0;
                i += 2;
            }
            return ret;
        }


        /// <summary>Returns numerical integral of a function calculated by the composite trapezoidal rule.
        /// Error: -(a-b)*h^2*f''(mi)/12</summary>
        /// <param name="f">Function to be integrated.</param>
        /// <param name="from">Lower bound of integration interval.</param>
        /// <param name="to">Upper bound of integration interval.</param>
        /// <param name="numintervals">Number of sub-intervals (1 less than the numbe of evaluation points).</param>
        /// <returns>Integral approximation.</returns>
        public static double IntegralTrapezoidal(DlgFunctionValue f, double from, double to, int numintervals)
        {
            double ret = 0;
            if (numintervals < 1)
                numintervals = 1;
            double h = (to - from) / numintervals;
            ret += f(from) + f(to);
            double s = 0;
            for (int j = 1; j <= numintervals - 1; ++j)
                s += f(from + j * h);
            ret += 2 * s;
            ret *= (h / 2);
            return ret;
        }

        /// <summary>Returns numerical integral of a function calculated by the trapezoidal rule.
        /// Function is specified as a table of values in specified points.
        /// Intervals do not need to be equidistant, minimal number of points is 2.
        /// Error: -(a-b)*h^2*f''(mi)/12</summary>
        /// <param name="tabx">Table of equidistant points at which function is evaluated.</param>
        /// <param name="tabf">Table of function values in points contained in tabx.</param>
        /// <returns>Integral approximation.</returns>
        public static double IntegralTrapezoidal(double[] tabx, double[] tabf)
        {
            if (tabx == null)
                throw new ArgumentNullException("Trapezoidal integration: table of nodes is not specified (null reference).");
            if (tabf == null)
                throw new ArgumentNullException("Trapezoidal integration: table of function values is not specified (null reference).");
            if (tabf.Length != tabx.Length)
                throw new ArgumentException("Trapezoidal integration: table of function values has different length ("
                    + tabf.Length + ") than table of nodes (" + tabx.Length + ").");
            int numintervals = tabx.Length - 1;
            if (numintervals < 1)
                throw new ArgumentException("Trapezoidal integration: number of intervals is less than 1.");
            double ret = 0;
            bool increasing = (tabx[1] > tabx[0]);  // points ordered by increasing x  
            int i = 0;
            while (i < numintervals)
            {
                // Verify consistence of ordering:
                if (increasing)
                {
                    if (tabx[i] > tabx[i + 1])
                        throw new ArgumentException("Trapezoidal integration: ordering of nodes is not consistent at interval "
                        + i.ToString() + ".");
                }
                else
                {
                    if (tabx[i] < tabx[i + 1])
                        throw new ArgumentException("Trapezoidal integration: ordering of nodes is not consistent at interval "
                        + i.ToString() + ".");
                }
                ret += (tabf[i] + tabf[i + 1]) * (tabx[i + 1] - tabx[i]) / 2.0;
                ++i;
            }
            return ret;
        }



        // TODO: check what is wrong with IntegralSimpsonTab and IntegralTrapeZoidalTab!

        /// <summary>Calculates numerical integral of a function by Simpson's rule, but through a table of values.</summary>
        private static double IntegralSimpsonTab(DlgFunctionValue f, double from, double to, int numintervals)
        {
            if (numintervals < 2)
                numintervals = 2;
            if (numintervals % 2 != 0)
                ++numintervals;
            double[] tabx, taby;
            tabx = new double[numintervals+1];
            taby = new double[numintervals+1];
            double h = (to - from) / (double) numintervals;
            double x;
            for (int i=0;i<=numintervals;++i)
            {
                x = from+i*h;
                tabx[i] = x;
                taby[i] = f(x);
            }
            return IntegralSimpson(tabx,taby);
        }

        /// <summary>Calculates numerical integral of a function by Trapezoidal rule, but through a table of values.</summary>
        private static double IntegralTrapeZoidalTab(DlgFunctionValue f, double from, double to, int numintervals)
        {
            if (numintervals < 1)
                numintervals = 1;
            double[] tabx, taby;
            tabx = new double[numintervals+1];
            taby = new double[numintervals+1];
            double h = (to - from) / (double) numintervals;
            double x;
            for (int i=0;i<=numintervals;++i)
            {
                x = from+((double) i)*h;
                tabx[i] = x;
                taby[i] = f(x);
            }
            return IntegralTrapezoidal(tabx,taby);
        }

        #endregion  // Integration


        #region Differentiation

        /// <summary>Calculates numerical derivative of a function according to the forward difference formula.
        /// Error: -h*f''(mi)/2</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="step">Step size used in differentiation.</param>
        /// <returns>Derivative approximation.</returns>
        public static double DerivativeFD(DlgFunctionValue f, double x, double step)
        {
            return (f(x + step) - f(x)) / step;
        }

        /// <summary>Calculates numerical derivative of f according to the backward difference formula.
        /// Error: h*f''(mi)/2</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="step">Step size used in differentiation.</param>
        /// <returns>Derivative approximation.</returns>
        public static double DerivativeBD(DlgFunctionValue f, double x, double step)
        {
            return (f(x) - f(x-step)) / step;
        }

        /// <summary>Calculates numerical derivative of a function according to the central difference formula.
        /// Error: -h^2*f(3)(mi)/6</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="step">Step size used in differentiation.</param>
        /// <returns>Derivative approximation.</returns>
        public static double DerivativeCD(DlgFunctionValue f, double x, double step)
        {
            return (f(x+step) - f(x-step)) / (2.0*step);
        }

        /// <summary>Calculates the derivative of a function with a 4 point formula.
        /// Error is of order O(h^4).</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="h">Step size.</param>
        public static double Derivative4Point(DlgFunctionValue f, double x, double h)
        {
            return (-f(x + 2 * h) + 8 * f(x + h) - 8 * f(x - h) + f(x - 2 * h)) / (12 * h);
        }

        /// <summary>Calculates numerical second derivative of a function according to the central difference formula.
        /// Error: -h^2*f(4)(mi)/12</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="step">Step size used in differentiation.</param>
        /// <returns>Derivative approximation.</returns>
        public static double SecondDerivativeCD(DlgFunctionValue f, double x, double step)
        {
            return (f(x - step) - 2 * f(x) + f(x + step)) / (step * step);
        }

        /// <summary>Calculates the second order derivative of a function with a 5 point formula.
        /// Error is O(h^4).</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="h">Step size.</param>
        public static double SecondDerivative5Point(DlgFunctionValue f, double x, double h)
        {
            return (-f(x + 2 * h) + 16 * f(x + h) - 30 * f(x) + 16 * f(x - h) - f(x - 2 * h)) / (12 * h * h);
        }

        /// <summary>Calculates the third order derivative of a function with a 5 point formula.
        /// Error is O(h^2).</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="h">Step size.</param>
        public static double ThirdDerivative4Point(DlgFunctionValue f, double x, double h)
        {
            return (f(x + 2 * h) - 2 * f(x + h) + 2 * f(x - h) - f(x - 2 * h)) / (2 * h * h * h);
        }

        /// <summary>Calculates the third order derivative of a function with a 5 point formula.
        /// Error is O(h^4).</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="h">Step size.</param>
        public static double ThirdDerivative6Point(DlgFunctionValue f, double x, double h)
        {
            return (-f(x + 3 * h) + 8 * f(x + 2 * h) - 13 * f(x + h) + 13 * f(x - h) - 8 * f(x - 2 * h) + f(x - 3 * h)) / (8 * h * h * h);
        }

        /// <summary>Calculates the fourth order derivative of a function with a 5 point formula.
        /// Error is O(h^2).</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="h">Step size.</param>
        public static double FourthDerivative5Point(DlgFunctionValue f, double x, double h)
        {
            double h4 = h * h;
            h4 *= h4;  // h^4
            return (f(x + 2 * h) - 4 * f(x + h) + 6 * f(x) - 4 * f(x - h) + f(x - 2 * h)) / (h4);
        }

        /// <summary>Calculates the fourth order derivative of a function with a 5 point formula.
        /// Error is O(h^4).</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="h">Step size.</param>
        public static double FourthDerivative7Point(DlgFunctionValue f, double x, double h)
        {
            double h4 = h * h;
            h4 *= h4;  // h^4
            return (-f(x+3*h)+12*f(x+2*h)-39*f(x+h)+56*f(x)-39*f(x-h)+12*f(x-2*h)-f(x-3*h)) / (6 * h4);
        }

        /// <summary>Calculates the fifth order derivative of a function with a 7 point formula.</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="h">Step size.</param>
        public static double FifthDerivative7Point(DlgFunctionValue f, double x, double h)
        {
            double h5 = h * h;
            h5 *= h5*h;  // h^5
            return (-f(x-3*h)+4*f(x-2*h)-5*f(x-h)+5*f(x+h)-4*f(x+2*h)+f(x+3*h))/ (2*h5);
        }

        /// <summary>Calculates the fifth order derivative of a function with a 9 point formula.</summary>
        /// <param name="f">Function whose derivative is calculated.</param>
        /// <param name="x">Value of independent variable at which derivative is calculated.</param>
        /// <param name="h">Step size.</param>
        public static double FifthDerivative9Point(DlgFunctionValue f, double x, double h)
        {
           double h5 = h * h;
            h5 *= h5*h;  // h^5
            return (f(x-4*h)-9*f(x-3*h)+26*f(x-2*h)-29*f(x-h)+29*f(x+h)-26*f(x+2*h)+9*f(x+3*h)-f(x+4*h)) / (6*h5);
        }



        #endregion  // Differentiation


        #region Testing

        /// <summary>Tests numerical differentiation methods.</summary>
        public static void TestDifferentiation()
        {

            // Test of numerical derivatives on exponential function:
            double x = 0.1;
            double h = 0.01;
            double der, numder, err, relerr;
            DlgFunctionValue f = Math.Exp;
            double 
                der1 = Math.Exp(x),
                der2 = Math.Exp(x),
                der3 = Math.Exp(x),
                der4 = Math.Exp(x),
                der5 = Math.Exp(x);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Test of numerical differentiation of Exp(x), x = " 
                + x.ToString() + ", h = " + h.ToString());
            // first order derivatives:
            Console.WriteLine();
            Console.WriteLine("First order derivative:");
            Console.WriteLine("Forward difference:");
            der = der1; numder = DerivativeFD(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("Backward difference:");
            der = der1; numder = DerivativeBD(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("Central difference:");
            der = der1; numder = DerivativeCD(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("4 point:");
            der = der1; numder = Derivative4Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            // second order derivatives:
            Console.WriteLine();
            Console.WriteLine("Second order derivative:");
            Console.WriteLine("Central difference:");
            der = der1; numder = SecondDerivativeCD(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("5 point:");
            der = der1; numder = SecondDerivative5Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            // third order derivatives:
            Console.WriteLine();
            Console.WriteLine("Third order derivative f(3)(x):");
            Console.WriteLine("4 point:");
            der = der1; numder = ThirdDerivative4Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("6 point:");
            der = der1; numder = ThirdDerivative6Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            // fourth order derivatives:
            Console.WriteLine();
            Console.WriteLine("Fourth order derivative f(4)(x):");
            Console.WriteLine("5 point:");
            der = der1; numder = FourthDerivative5Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("7 point:");
            der = der1; numder = FourthDerivative7Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            // fifth order derivatives:
            Console.WriteLine();
            Console.WriteLine("Fifth order derivative f(5)(x):");
            Console.WriteLine("7 point:");
            der = der1; numder = FifthDerivative7Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("9 point:");
            der = der1; numder = FifthDerivative9Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);

            Console.WriteLine();
            Console.WriteLine("Repeatind calculation with increased accuracy ...");
            h = 1.0e-3;
            Console.WriteLine("Test of numerical differentiation of Exp(x), x = " 
                + x.ToString() + ", h = " + h.ToString());
            // first order derivatives:
            Console.WriteLine();
            Console.WriteLine("First order derivative:");
            Console.WriteLine("Forward difference:");
            der = der1; numder = DerivativeFD(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("Backward difference:");
            der = der1; numder = DerivativeBD(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("Central difference:");
            der = der1; numder = DerivativeCD(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("4 point:");
            der = der1; numder = Derivative4Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            // second order derivatives:
            Console.WriteLine();
            Console.WriteLine("Second order derivative:");
            Console.WriteLine("Central difference:");
            der = der1; numder = SecondDerivativeCD(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("5 point:");
            der = der1; numder = SecondDerivative5Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            // third order derivatives:
            Console.WriteLine();
            Console.WriteLine("Third order derivative f(3)(x):");
            Console.WriteLine("4 point:");
            der = der1; numder = ThirdDerivative4Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("6 point:");
            der = der1; numder = ThirdDerivative6Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            // fourth order derivatives:
            Console.WriteLine();
            Console.WriteLine("Fourth order derivative f(4)(x):");
            Console.WriteLine("5 point:");
            der = der1; numder = FourthDerivative5Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("7 point:");
            der = der1; numder = FourthDerivative7Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            // fifth order derivatives:
            Console.WriteLine();
            Console.WriteLine("Fifth order derivative f(5)(x):");
            Console.WriteLine("7 point:");
            der = der1; numder = FifthDerivative7Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);
            Console.WriteLine("9 point:");
            der = der1; numder = FifthDerivative9Point(f, x, h);
            err = numder - der;
            relerr = 2 * Math.Abs(err) / (Math.Abs(numder) + Math.Abs(der));
            Console.WriteLine("  Analytical: " + der.ToString() + ", numerical: " + numder.ToString()
                + Environment.NewLine + "      Error: " + err + ", rel. err.: " + relerr);


            Console.WriteLine(Environment.NewLine);
        }


        /// <summary>Test for numerical integration methods.</summary>
        public static void TestIntegration()
        {
            DlgFunctionValue f = Math.Exp;
            double from = 0, to = 1;
            double exact, trapezoid, trapezoidtab, simpson, simpsontab;
            exact = Math.Exp(to) - Math.Exp(from);

            Console.WriteLine("Test of numerical integral algorithms:");

            int numintervals = 10;
            Console.WriteLine("Number of intervals: " +  numintervals.ToString());
            trapezoid = IntegralTrapezoidal(f, from, to, numintervals);
            trapezoidtab = IntegralTrapeZoidalTab(f, from, to, numintervals);
            simpson = IntegralSimpson(f, from, to, numintervals);
            simpsontab = IntegralSimpsonTab(f, from, to, numintervals);
            Console.WriteLine();
            Console.WriteLine("Comparison of integration methods: ");
            Console.WriteLine("                    Exact: " + exact.ToString());
            Console.WriteLine("              Trapezoidal: " + trapezoid.ToString() + ", error: " + (trapezoid-exact).ToString());
            Console.WriteLine("Trapezoidal through table: " + trapezoidtab.ToString() + ", error: " + (trapezoidtab - exact).ToString());
            Console.WriteLine("                  Simpson: " + simpson.ToString() + ", error: " + (simpson - exact).ToString());
            Console.WriteLine("    Simpson through table: " + simpsontab.ToString() + ", error: " + (simpsontab - exact).ToString());


            numintervals = 100;
            Console.WriteLine("Number of intervals: " + numintervals.ToString());
            trapezoid = IntegralTrapezoidal(f, from, to, numintervals);
            trapezoidtab = IntegralTrapeZoidalTab(f, from, to, numintervals);
            simpson = IntegralSimpson(f, from, to, numintervals);
            simpsontab = IntegralSimpsonTab(f, from, to, numintervals);
            Console.WriteLine();
            Console.WriteLine("Comparison of integration methods: ");
            Console.WriteLine("                    Exact: " + exact.ToString());
            Console.WriteLine("              Trapezoidal: " + trapezoid.ToString() + ", error: " + (trapezoid - exact).ToString());
            Console.WriteLine("Trapezoidal through table: " + trapezoidtab.ToString() + ", error: " + (trapezoidtab - exact).ToString());
            Console.WriteLine("                  Simpson: " + simpson.ToString() + ", error: " + (simpson - exact).ToString());
            Console.WriteLine("    Simpson through table: " + simpsontab.ToString() + ", error: " + (simpsontab - exact).ToString());

            Console.WriteLine();  Console.WriteLine();
        }

        #endregion  // Testing

    }
}
