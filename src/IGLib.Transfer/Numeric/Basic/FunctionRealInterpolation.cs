// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// SIMPLE INTERPOLATIONS (linear, quadratic, cubic)
    
using IG.Lib;

namespace IG.Num
{
    
    /// <summary>Various commonly used real functions of one variable.</summary>
    /// $A Igor xx;
    public partial class Func
    {

        #region Interpolation

        #region Linear Interpolation


        /// <summary>Creates and returns linear interpolation function with specified two values.</summary>
        /// <param name="x1">Abscissa of the first point.</param>
        /// <param name="y1">Value at the first point.</param>
        /// <param name="x2">Abscissa of the second point.</param>
        /// <param name="y2">Value at the second point.</param>
        public static Linear GetLinearInterpolation(double x1, double y1, double x2, double y2)
        {
            Linear func = new Linear(0.0 ,0.0);
            func.SetInterpolation(x1, y1, x2, y2);
            return func;
        }

        /// <summary>Creates and returns linear interpolation function with specified value and derivative.</summary>
        /// <param name="x1">Abscissa of the first point.</param>
        /// <param name="y1">Value at the first point.</param>
        /// <param name="d1">Derivative at the first point.</param>
        public static Linear GetLinearInterpolation(double x1, double y1, double d1)
        {
        Linear func = new Linear(0.0 ,0.0);
        func.SetInterpolation(x1, y1, d1);
        return func;
        }

        /// <summary>Creates and returns a new linear function with the specified coefficient.</summary>
        /// <param name="a1">Leading coefficient - coefficient of the linear term.</param>
        /// <param name="a0">Constant term coefficient.</param>
        public static Linear GetLinear(double a1, double a0)
        {
            return new Linear(a1, a0);
        }

        /// <summary>Linear function, f(x) = a1*x + a0.
        /// Specific properties:
        ///   Zero - returns a zero.
        ///   HasZero - either the function has a zero or not.
        ///   </summary>
        public class Linear : RealFunction
        {

            #region Initialization

            /// <summary>Creates a linear function, coefficients are specified in the descending order.</summary>
            /// <param name="a1">Linear term coefficient.</param>
            /// <param name="a0">Constant term coefficient.</param>
            public Linear(double a1, double a0)
            {
                Name = "LinearFunction";
                this.a1 = a1;
                this.a0 = a0;
            }

            /// <summary>Sets coefficients of the linear function represented by the current object.</summary>
            /// <param name="a1"></param>
            /// <param name="a0"></param>
            public void SetCoefficients(double a1, double a0)
            {
                this.a1 = a1;
                this.a0 = a0;
            }

            /// <summary>Initializes the linear interpolation function with specified two values.</summary>
            /// <param name="x1">Abscissa of the first point.</param>
            /// <param name="y1">Value at the first point.</param>
            /// <param name="x2">Abscissa of the second point.</param>
            /// <param name="y2">Value at the second point.</param>
            public void SetInterpolation(double x1, double y1, double x2, double y2)
            {
                a1 = (y2 - y1) / (x2 - x1);
                a0 = y1 - x1 * (y2 - y1) / (x2 - x1);
            }

            /// <summary>Initializes the linear interpolation function with specified value and derivative.</summary>
            /// <param name="x1">Abscissa of the first point.</param>
            /// <param name="y1">Value at the first point.</param>
            /// <param name="d1">Derivative at the first point.</param>
            public void SetInterpolation(double x1, double y1, double d1)
            {
                a1 = d1;
                a0 = y1 - x1 * d1;
            }


            #endregion Initialization

            #region Data and Properties

            protected double _a0, _a1;

            /// <summary>Returns the constant term coefficient of the linear function.</summary>
            public double a0
            {
                get { return _a0; }
                protected set { _a0 = value; }
            }

            /// <summary>Returns the linear term coefficitne of the linear function.</summary>
            public double a1
            {
                get { return _a1; }
                protected set { _a1 = value; }
            }

            /// <summary>Returns zero of the current linear function.
            /// Throws InvalidOperationException if the function does not have zeros.</summary>
            public double Zero
            {
                get {
                    if (a1 == 0.0)
                        throw new InvalidOperationException("Linear function does not have zeros because leading coefficient is 0.");
                    return -a0 / a1;
                }
            }

            /// <summary>Returns true if the linear function has a zero, false otherwise.</summary>
            public bool HasZero
            {
                get {
                    if (a1 == 0.0)
                        return false;
                    return true;
                }
            }


            #endregion Data and Properties

            protected override double RefValue(double x)
            {
                return a0 + a1*x;
            }

            public override bool ValueDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for value defined for function " + Name + ".");
                }
            }

            protected override double RefDerivative(double x)
            {
                return a1;
            }

            public override bool DerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for derivative defined for function " + Name + ".");
                }
            }

            protected override double RefSecondDerivative(double x)
            {
                return 0;
            }

            public override bool SecondDerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for second derivative defined for function " + Name + ".");
                }
            }


            protected override double RefDerivative(double x, int order)
            {
                if (order<0)
                    throw new ArgumentException("Derivative order less than 0 is not valid. Function: " + Name);
                if (order == 0)
                    return RefValue(x);
                else if (order == 1)
                    return RefDerivative(x);
                else if (order == 2)
                    return RefSecondDerivative(x);
                else
                    return 0 ;
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            {
                throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                  + Name + ".");
            }

            protected override double RefIntegral(double x)
            {
                    return a0*x + 0.5*a1*x*x;
            }

            public override bool IntegralDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

            protected override double RefInverse(double x)
            {
                if (a1 == 0)
                    throw new InvalidOperationException("Linear function does not have inverse because linear coefficient is 0.");
                return (x-a0)/a1;
            }

            public override bool InverseDefined
            {
                // TODO: define inverse numerically, then set this to true! (create a special procedure for 
                // weighting functions with 
                // finite support!)
                get { 
                    if (a1!=0)
                        return true;
                    else
                        return false;
                }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for inverse defined for function " + Name + ".");
                }
            }

        } // class Linear

        #endregion Linear Interpolation


        #region Quadratic Interpolation


        /// <summary>Creates and returns quadratic interpolation function with specified two values.</summary>
        /// <param name="x1">Abscissa of the first point.</param>
        /// <param name="y1">Value at the first point.</param>
        /// <param name="x2">Abscissa of the second point.</param>
        /// <param name="y2">Value at the second point.</param>
        /// <param name="x3">Abscissa of the third point.</param>
        /// <param name="y3">Value at the third point.</param>
        public static Quadratic GetQuadraticInterpolation(double x1, double y1, 
            double x2, double y2, double x3, double y3)
        {
            Quadratic func = new Quadratic(0.0, 0.0, 0.0);
            func.SetInterpolation(x1, y1, x2, y2, x3, y3);
            return func;
        }

        /// <summary>Creates and returns quadratic interpolation function with specified two function
        /// values and one derivative.</summary>
        /// <param name="x1">Abscissa of the first point.</param>
        /// <param name="y1">Value at the first point.</param>
        /// <param name="d1">Derivative at the first point.</param>
        /// <param name="x2">Abscissa of the second point.</param>
        /// <param name="y2">Value at the second point.</param>
        public static Quadratic GetQuadraticInterpolation(double x1, double y1, double d1,
                double x2, double y2)
        {
            Quadratic func = new Quadratic(0.0, 0.0, 0.0);
            func.SetInterpolation(x1, y1, d1, x2, y2);
            return func;
        }

        /// <summary>Creates and returns quadratic interpolation function with specified two function
        /// values and two approximate derivatives.
        /// The returned interpolatinon is an average of interpolations obtained by taking into account
        /// one or another function.</summary>
        /// <param name="x1">Abscissa of the first point.</param>
        /// <param name="y1">Value at the first point.</param>
        /// <param name="d1">Derivative (approximate) in the first point.</param>
        /// <param name="x2">Abscissa of the second point.</param>
        /// <param name="y2">Value at the second point.</param>
        /// <param name="d2">Derivative (approximate) in the second point/</param>
        public static Quadratic GetQuadraticInterpolation2der(double x1, double y1, double d1,
                double x2, double y2, double d2)
        {
            Quadratic func = new Quadratic(0.0, 0.0, 0.0);
            func.SetInterpolation2der(x1, y1, d1, x2, y2, d2);
            return func;
        }

        /// <summary>Creates and returns a new quadratic function with the specified coefficient.</summary>
        /// <param name="a2">Leading coefficient - coefficient of the quadratic term.</param>
        /// <param name="a1">Linear term coefficient.</param>
        /// <param name="a0">Constant term coefficient.</param>
        public static Quadratic GetQuadratic(double a2, double a1, double a0)
        {
            return new Quadratic(a2, a1, a0);
        }


        // TODO: Create tests for all interpolation functions, usual tests for implementation of derivatives,
        // inverse and integrals, ALSO test of calculation of zeros and extremes!


        // TODO: Create tests for all interpolation functions, usual tests for implementation of derivatives,
        // inverse and integrals, ALSO test of calculation of zeros and extremes!

        /// <summary>Quadratic function, f(x) = a2*x*x + a1*x + a0.
        /// Specific properties:
        ///   Zero - returns a zero.
        ///   Zero1 - returns the fierst zero.
        ///   Zero2 - returns the second zero.
        ///   NumZeros - 0, 1 or 2, returns number of zeros.
        ///   HasZero - either the function has a zero or not.
        ///   </summary>
        public class Quadratic : RealFunction
        {

            #region Initialization

            /// <summary>Creates a quadratic function, coefficients are specified in the descending order.</summary>
            /// <param name="a2">quadratic term coefficient.</param>
            /// <param name="a1">linear term coefficient.</param>
            /// <param name="a0">constant term coefficient.</param>
            public Quadratic(double a2, double a1, double a0)
            {
                Name = "QuadraticFunction";
                this.a2 = a2;
                this.a1 = a1;
                this.a0 = a0;
            }

            /// <summary>Sets coefficients of the quadratic function represented by the current object.</summary>
            /// <param name="a2">quadratic term coeficient.</param>
            /// <param name="a1">linear term coefficient.</param>
            /// <param name="a0">constant term coefficient.</param>
            public void SetCoefficients(double a2, double a1, double a0)
            {
                this.a2 = a2;
                this.a1 = a1;
                this.a0 = a0;
            }

            /// <summary>Initializes the quadratic interpolation function with specified three function values.</summary>
            /// <param name="x1">Abscissa of the first point.</param>
            /// <param name="y1">Value at the first point.</param>
            /// <param name="x2">Abscissa of the second point.</param>
            /// <param name="y2">Value at the second point.</param>
            /// <param name="x3">Abscissa of the third point.</param>
            /// <param name="y3">Value at the third point.</param>
            public void SetInterpolation(double x1, double y1, double x2, double y2,
                        double x3, double y3)
            {
                // Store the first point:
                double x10 = x1;
                double y10 = y1;
                // First, calculate coefficients wher co-ordinate zero is moved to the first point:
                x2-=x1; 
                x3-=x1;
                y2-=y1; 
                y3-=y1;
                x1=y1=0;
                a0=0;
                a1=(x3*x3 *y2- x2*x2*y3)/(x2*x3*x3-x2*x3);
                a2=(x3*y2-x2*y3)/(x2*x2*x3-x2*x3*x3);
                // Then, transform coefficient back:
                a0+=y10+(a2)*x10*x10-a1*x10;
                a1-=2*a2*x10;
            }


            /// <summary>Initializes the quadratic interpolation function with specified two function values
            /// and one derivative.</summary>
            /// <param name="x1">Abscissa of the first point.</param>
            /// <param name="y1">Value at the first point.</param>
            /// <param name="d1">Derivative at the first point.</param>
            /// <param name="x2">Abscissa of the second point.</param>
            /// <param name="y2">Value at the second point.</param>
            public void SetInterpolation(double x1, double y1, double d1, double x2, double y2)
            {
                // Store the first point:
                double x10 = x1;
                double y10 = y1;
                // First, calculate coefficients wher co-ordinate zero is moved to the first point:
                a0=0;
                a1=d1; 
                a2=(y2-d1*x2)/x2*x2;
                // Then, transform coefficient back:
                a0+=y10+a2*x10*x10-a1*x10;
                a1-=2*a2*x10;
            }


            /// <summary>Initializes the quadratic interpolation function with specified two function values
            /// and two approximate derivative. The interpolation becomes average between bunctio nobtained by
            /// taking into account the first derivative and the second derivative.</summary>
            /// <param name="x1">Abscissa of the first point.</param>
            /// <param name="y1">Value at the first point.</param>
            /// <param name="d1">Derivative (approximate) in the first point.</param>
            /// <param name="x2">Abscissa of the second point.</param>
            /// <param name="y2">Value at the second point.</param>
            /// <param name="d2">Derivative (approximate) in the second point.</param>
            public void SetInterpolation2der(double x1, double y1, double d1, 
                    double x2, double y2, double d2)
            {
                // Store the first point:
                double x10 = x1;
                double y10 = y1;
                double x20 = x2;
                double y20 = y2;
                // First, calculate coefficients wher co-ordinate zero is moved to the first point:
                double a12,a11,a22,a21,denom;
                x2-=x1;
                y2-=y1;
                x1=y1=0;
                denom=x2*x2;
                /* Koeficienti parabole, ce upostevamo odvod v prvi tocki: */
                a11=d1; a12=(y2-d1*x2)/denom;
                /* Koeficienti parabole, ce upostevamo odvod v drugi tocki: */
                a21=(2*x2*y2-d2*x2*x2)/denom; a22=(d2*x2-y2)/denom;
                a0=0;
                a1=0.5*(a11+a21);
                a2=0.5*(a12+a22);
                // Then, transform coefficient back:
                a0+=y10+a2*x10*x10-a1*x10;
                a1-=2*a2*x10;
            }


            #endregion Initialization

            #region Data and Properties

            protected double _a0, _a1, _a2;  // coefficients of quadratic polynomial

            /// <summary>Returns the constant term coefficient of the quadratic function.</summary>
            public double a0
            {
                get { return _a0; }
                protected set { _a0 = value; }
            }

            /// <summary>Returns the quadratic term coefficitne of the quadratic function.</summary>
            public double a2
            {
                get { return _a2; }
                protected set { _a2 = value; }
            }

            /// <summary>Returns the linear term coefficitne of the quadratic function.</summary>
            public double a1
            {
                get { return _a1; }
                protected set { _a1 = value; }
            }

            /// <summary>Calculates and returns real zeros of the function.
            /// Also returns the number of real zeros (0, 1 or 2).</summary>
            /// <param name="x1">The first zero (output parameter).</param>
            /// <param name="x2">The second zero (output parameter).</param>
            /// <returns>Number of distinct real zeros.</returns>
            public int Zeros(out double x1, out double x2)
            {
                double dis;  // discriminant
                x1 = x2 = 0.0;
                if (a2 == 0)  /* linear instead of quadratic function */
                {
                    if (a1 == 0)
                        return 0;
                    else
                    {
                        x1 = x2 = -a0 / a1;
                        return 1;
                    }
                } else if ((dis = a1 * a1 - 4 * a2 * a0) < 0)
                    return 0;
                else if (dis == 0)
                {
                    x1 = x2 = -a1 / (2 * a2);
                    return 1;
                }
                else  /* dis>0 */
                {
                    if (a2 > 0)
                    {
                        x1 = (-a1 - Math.Sqrt(dis)) / (2 * a2);
                        x2 = (-a1 + Math.Sqrt(dis)) / (2 * a2);
                    }
                    else
                    {
                        x2 = (-a1 - Math.Sqrt(dis)) / (2 * a2);
                        x1 = (-a1 + Math.Sqrt(dis)) / (2 * a2);
                    }
                    return 2;
                }
            }


            /// <summary>Returns the first zero of the current quadratic function.
            /// Throws InvalidOperationException if the function does not have real zeros.</summary>
            public double Zero
            {
                get
                {
                    int num;
                    double x1, x2;
                    num = Zeros(out x1, out x2);
                    if (num < 1)
                        throw new InvalidOperationException("This quadratic function does not have any zeros.");
                    return x1;
                }
            }


            /// <summary>Returns the first zero of the current quadratic function.
            /// Throws InvalidOperationException if the function does not have real zeros.</summary>
            public double Zero1
            {
                get
                {
                    int num;
                    double x1, x2;
                    num = Zeros(out x1, out x2);
                    if (num < 1)
                        throw new InvalidOperationException("This quadratic function does not have any zeros.");
                    return x1;
                }
            }

            /// <summary>Returns the second zero of the current quadratic function.
            /// Throws InvalidOperationException if the function does not have two distinct real zeros.</summary>
            public double Zero2
            {
                get
                {
                    int num;
                    double x1, x2;
                    num = Zeros(out x1, out x2);
                    if (num < 2)
                        throw new InvalidOperationException("This quadratic function does not have two zeros.");
                    return x2;
                }
            }

            /// <summary>Returns true if this quadratic function has at least one real zero, false otherwise.</summary>
            public bool HasZero
            {
                get
                {
                    int num;
                    double x1, x2;
                    num = Zeros(out x1, out x2);
                    return (num > 0);
                }
            }

            /// <summary>Returns number of zeros of the current quadratic function.</summary>
            public int NumZeros
            {
                get
                {
                    int num;
                    double x1, x2;
                    num = Zeros(out x1, out x2);
                    return (num);
                }
            }

            /// <summary>Returns true if this quadratic function has a strict maximum.</summary>
            public bool HasMaximum
            {
                get{
                if (a2 < 0)
                    return true;
                else
                    return false;
                }
            }

            /// <summary>Gets strict maximum of this quadratic function. 
            /// If the function does not have one then exeption is thrown.</summary>
            public double Maximum
            {
                get 
                {
                    if (a2 >= 0)
                        throw new InvalidOperationException("Quadratic function does not have a strict maximum, leading coefficient non-negative.");
                    return -a1 / (2 * a2);
                }
            }

            /// <summary>Returns true if this quadratic function has a strict minimum.</summary>
            public bool HasMinimum
            {
                get
                {
                    if (a2 > 0)
                        return true;
                    else
                        return false;
                }
            }

            /// <summary>Gets strict minimum of this quadratic function. 
            /// If the function does not have one then exeption is thrown.</summary>
            public double Minimum
            {
                get
                {
                    if (a2 <= 0)
                        throw new InvalidOperationException("Quadratic function does not have a strict minimum, leading coefficient non-negative.");
                    return -a1 / (2 * a2);
                }
            }


            #endregion Data and Properties

            protected override double RefValue(double x)
            {
                return a0 + a1 * x + a2 * x*x;
            }

            public override bool ValueDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for value defined for function " + Name + ".");
                }
            }

            protected override double RefDerivative(double x)
            {
                return 2*a2*x + a1;
            }

            public override bool DerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for derivative defined for function " + Name + ".");
                }
            }

            protected override double RefSecondDerivative(double x)
            {
                return 2*a2;
            }

            public override bool SecondDerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for second derivative defined for function " + Name + ".");
                }
            }


            protected override double RefDerivative(double x, int order)
            {
                if (order < 0)
                    throw new ArgumentException("Derivative order less than 0 is not valid. Function: " + Name);
                if (order == 0)
                    return RefValue(x);
                else if (order == 1)
                    return RefDerivative(x);
                else if (order == 2)
                    return RefSecondDerivative(x);
                else
                    return 0;
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            {
                throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                  + Name + ".");
            }

            protected override double RefIntegral(double x)
            {
                return a2 * Math.Pow(x, 3) / 3 + a1 * Math.Pow(x, 2) / 2 + a0; ;
            }

            public override bool IntegralDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

            protected override double RefInverse(double y)
            {
                double a0, a1, a2, x1, x2;
                a2 = this.a2;
                a1 = this.a1;
                a0 = this.a0 - y;
                double dis;  // discriminant
                x1 = x2 = 0.0;
                if (a2 == 0)  /* linear instead of quadratic function */
                {
                    if (a1 == 0)
                        throw new InvalidOperationException("Current quadratic function does not have inverse at " + y +"."
                            + Environment.NewLine + "Coefficients: a2 = " + a2 + ", a1 = " + a1 + ", a0 = " + a0 +".");
                    else
                    {
                        x1 = x2 = -a0 / a1;
                        return x1;
                    }
                }
                else if ((dis = a1 * a1 - 4 * a2 * a0) < 0)
                    throw new InvalidOperationException("Current quadratic function does not have inverse at " + y +"."
                        + Environment.NewLine + "Coefficients: a2 = " + a2 + ", a1 = " + a1 + ", a0 = " + a0 +".");
                else if (dis == 0)
                {
                    x1 = x2 = -a1 / (2 * a2);
                    return x1;
                }
                else  /* dis>0 */
                {
                    if (a2 > 0)
                    {
                        x1 = (-a1 - Math.Sqrt(dis)) / (2 * a2);
                        x2 = (-a1 + Math.Sqrt(dis)) / (2 * a2);
                    }
                    else
                    {
                        x2 = (-a1 - Math.Sqrt(dis)) / (2 * a2);
                        x1 = (-a1 + Math.Sqrt(dis)) / (2 * a2);
                    }
                    return x1;
                }
            }

            public override bool InverseDefined
            {
                // TODO: define inverse numerically, then set this to true! (create a special procedure for 
                // weighting functions with 
                // finite support!)
                get
                {
                    if (a1 != 0)
                        return true;
                    else
                        return false;
                }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for inverse defined for function " + Name + ".");
                }
            }

        } // class Quadratic


        #endregion Quadratic Interpolation


        #region Cubic Interpolation


        /// <summary>Creates and returns cubic interpolation function with specified two values.</summary>
        /// <param name="x1">Abscissa of the first point.</param>
        /// <param name="y1">Value at the first point.</param>
        /// <param name="x2">Abscissa of the second point.</param>
        /// <param name="y2">Value at the second point.</param>
        /// <param name="x3">Abscissa of the third point.</param>
        /// <param name="y3">Value at the third point.</param>
        /// <param name="x4">Abscissa of the fourth point.</param>
        /// <param name="y4">Value at the fifth point.</param>
        public static Cubic GetCubicInterpolation(double x1, double y1,
            double x2, double y2, double x3, double y3, double x4, double y4)
        {
            Cubic func = new Cubic(0.0, 0.0, 0.0, 0.0);
            func.SetInterpolation(x1, y1, x2, y2, x3, y3, x4, y4);
            return func;
        }

        /// <summary>Creates and returns cubic interpolation function with specified two function
        /// values and one derivative.</summary>
        /// <param name="x1">Abscissa of the first point.</param>
        /// <param name="y1">Value at the first point.</param>
        /// <param name="d1">Derivative at the first point.</param>
        /// <param name="x2">Abscissa of the second point.</param>
        /// <param name="y2">Value at the second point.</param>
        /// <param name="d2">Derivative in the second point.</param>
        public static Cubic GetCubicInterpolation(double x1, double y1, double d1,
                double x2, double y2, double d2)
        {
            Cubic func = new Cubic(0.0, 0.0, 0.0, 0.0);
            func.SetInterpolation(x1, y1, d1, x2, y2, d2);
            return func;
        }

        /// <summary>Creates and returns a new cubic function with the specified coefficient.</summary>
        /// <param name="a3">Leading coefficient - coefficient of the cubic term.</param>
        /// <param name="a2">Coefficient of the quadratic term.</param>
        /// <param name="a1">Linear term coefficient.</param>
        /// <param name="a0">Constant term coefficient.</param>
        public static Cubic GetCubic(double a3, double a2, double a1, double a0)
        {
            return new Cubic(a3, a2, a1, a0);
        }

        /// <summary>Cubic function, f(x) = a3*x*x*x + a2*x*x + a1*x + a0.
        /// Specific properties:
        ///   Zero - returns a zero.
        ///   Zero1 - returns the fierst zero.
        ///   Zero2 - returns the second zero.
        ///   Zero3 - returns the third zero.
        ///   NumZeros - returns number of zeros (0, 1 or 3).
        ///   HasZero - either the function has a zero or not.
        ///   Min - returns function's strict minimum.
        ///   Max - returns function's strict maximum.
        ///   </summary>
        public class Cubic : RealFunction, IRealFunction, ILockable
        {

            #region Initialization

            /// <summary>Creates a cubic function, coefficients are specified in the descending order.</summary>
            /// <param name="a3">cubic term coefficitnt.</param>
            /// <param name="a2">quadratic term coefficient.</param>
            /// <param name="a1">linear term coefficient.</param>
            /// <param name="a0">constant term coefficient.</param>
            public Cubic(double a3, double a2, double a1, double a0)
            {
                Name = "CubicFunction";
                this.a3 = a3;
                this.a2 = a2;
                this.a1 = a1;
                this.a0 = a0;
            }

            /// <summary>Sets coefficients of the cubic function represented by the current object.</summary>
            /// <param name="a3">cubic term coefficient.</param>
            /// <param name="a2">quadratic term coeficient.</param>
            /// <param name="a1">linear term coefficient.</param>
            /// <param name="a0">constant term coefficient.</param>
            public void SetCoefficients(double a3, double a2, double a1, double a0)
            {
                this.a3 = a3;
                this.a2 = a2;
                this.a1 = a1;
                this.a0 = a0;
            }


            /// <summary>Initializes the cubic interpolation function with specified four function values.</summary>
            /// <param name="x1">Abscissa of the first point.</param>
            /// <param name="y1">Value at the first point.</param>
            /// <param name="x2">Abscissa of the second point.</param>
            /// <param name="y2">Value at the second point.</param>
            /// <param name="x3">Abscissa of the third point.</param>
            /// <param name="y3">Value at the third point.</param>
            /// <param name="x4">Abscissa of the third point.</param>
            /// <param name="y4">Value at the third point.</param>
            public void SetInterpolation(double x1, double y1, double x2, double y2,
                        double x3, double y3, double x4, double y4)
            {
                // Store the original points:
                double x10 = x1;
                double y10 = y1;
                double x20 = x2;
                double y20 = y2;
                double x30 = x3;
                double y30 = y3;
                // First, calculate coefficients wher co-ordinate zero is moved to the first point:
                double denom;
                x2 -= x1; x3 -= x1; x4 -= x1;
                y2 -= y1; y3 -= y1; y4 -= y1;
                x1 = y1 = 0;
                a0 = 0;
                a1 = (x2*x2) * (x4*x4) * (x4 - x2) * y3 +
                     x3 * (x3*x3) * ((x4*x4) * y2 - (x2*x2) * y4) +
                     (x3*x3) * (x2 * (x2*x2) * y4 - x4 * (x4*x4) * y2);
                a2 = x2 * x4 * ((x2*x2) - (x4*x4)) * y3 +
                     x3 * (x3*x3) * (x2 * y4 - x4 * y2) +
                     x3 * (x4 * (x4*x4) * y2 - x2 * (x2*x2) * y4);
                a3 = x2 * x4 * (x4 - x2) * y3 + (x3*x3) * (x4 * y2 - x2 * y4) + x3 * ((x2*x2) * y4 - (x4*x4) * y2);
                denom = x2 * (x2 - x3) * x3 * (x2 - x4) * (x3 - x4) * x4;
                a1 /= denom; a2 /= denom; a3 /= denom;
                // Then, transform coefficient back:
                a0 += y10 - (a1) * x10 + (a2) * (x10*x10) - (a3) * x10 * (x10*x10);
                a1 += 3 * (a3) * (x10*x10) - 2 * (a2) * x10;
                a2 -= 3 * (a3) * x10;
            }


            /// <summary>Initializes the cubic interpolation function with specified two function values
            /// and one derivative.</summary>
            /// <param name="x1">Abscissa of the first point.</param>
            /// <param name="y1">Value at the first point.</param>
            /// <param name="d1">Derivative at the first point.</param>
            /// <param name="x2">Abscissa of the second point.</param>
            /// <param name="y2">Value at the second point.</param>
            /// <param name="d2">Derivative at the second point.</param>
            public void SetInterpolation(double x1, double y1, double d1, double x2, double y2, double d2)
            {
                // Store the original points:
                double x10 = x1;
                double y10 = y1;
                double x20 = x2;
                double y20 = y2;
                // First, calculate coefficients wher co-ordinate zero is moved to the first point:
                double denom;
                x2 -= x1;
                y2 -= y1;
                x1 = y1 = 0;
                denom = (x2*x2);
                a0 = 0;
                a1 = d1;
                a2 = (3 * y2 - d2 * x2 - 2 * d1 * x2) / denom;
                a3 = (d1 * x2 + d2 * x2 - 2 * y2) / (x2 * denom);
                // Then, transform coefficient back:
                a0 += y10 - (a1) * x10 + (a2) * (x10*x10) - (a3) * x10 * (x10*x10);
                a1 += 3 * (a3) * (x10*x10) - 2 * (a2) * x10;
                a2 -= 3 * (a3) * x10;
            }


            #endregion Initialization

            
            #region ThreadLocking

            private object _mainLock = new object();

            /// <summary>This object's central lock object to be used by other object.
            /// Do not use this object for locking in class' methods, for this you should use 
            /// InternalLock.</summary>
            public object Lock { get { return _mainLock; } }

            #endregion ThreadLocking



            #region Data and Properties

            protected double _a0, _a1, _a2, _a3;  // coefficients of cubic polynomial

            /// <summary>Returns the constant term coefficient of the cubic function.</summary>
            public double a0
            {
                get { return _a0; }
                protected set { _a0 = value; }
            }

            /// <summary>Returns the linear term coefficitne of the cubic function.</summary>
            public double a1
            {
                get { return _a1; }
                protected set { _a1 = value; }
            }

            /// <summary>Returns the quadratic term coefficitne of the cubic function.</summary>
            public double a2
            {
                get { return _a2; }
                protected set { _a2 = value; }
            }

            /// <summary>Returns the cubic term coefficitne of the cubic function.</summary>
            public double a3
            {
                get { return _a3; }
                protected set { _a3 = value; }
            }

            private Quadratic _refquad;

            protected Quadratic RefQuad
            {
                get
                {
                    if (_refquad == null)
                    {
                        lock (Lock)
                        {
                            if (_refquad == null)
                                _refquad = new Quadratic(0, 0, 0);
                        }
                    }
                    return _refquad;
                }
            }
                



            /// <summary>Calculates and returns real zeros of the function.
            /// Also returns the number of real zeros (0, 1 or 3).</summary>
            /// <param name="x1">The first zero (output parameter).</param>
            /// <param name="x2">The second zero (output parameter).</param>
            /// <param name="x3">The third real zero (output parameter).</param>
            /// <returns>Number of distinct real zeros.</returns>
            public int Zeros(out double x1, out double x2, out double x3)
            {


                int i = 0;
                double p, q, r, d, s, fi, t1, t2, t3, t;
                if (a3 == 0)  /* quadratic instead of cubic parabola */
                {
                    lock (Lock)
                    {
                        RefQuad.SetCoefficients(a2, a1, a0);
                        i = RefQuad.Zeros(out x1, out x2);
                    }
                    x3 = x1;
                    return i;
                }
                else
                {
                    s = -a2 / (3 * a3);  /* shift because of introduction of new variable */
                    p = (3 * a3 * a1 - (a2 * a2)) / (9 * (a3 * a3));
                    q = 0.5 * ((2 * (a2 * a2 * a2) / (27 * (a3 * a3 * a3))) - (a2 * a1 / (3 * (a3 * a3))) + (a0 / a3));
                    r = Math.Sqrt(Math.Abs(p)); if (q < 0) r = -r;
                    if (p == 0)
                    {
                        // if (pr) printf("\ncubzeros: p=0\n");
                        x1 = x2 = x3 = Math.Pow(-2 * q, 1 / 3) + s;
                        return 1;
                    }
                    else if (p >= 0)
                    {
                        //if (pr) printf("\ncubzeros: p>0\n");
                        // There is just one real zero:
                        fi = M.arsh(q / (r * r * r));
                        x1 = x2 = x3 = -2 * r * Math.Sinh(fi / 3) + s;
                        return 1;
                    }
                    else
                    {
                        /* p<0 */
                        if ((d = (q*q) + (p * p * p)) > 0)
                        {
                            //if (pr) printf("\ncubzeros: p<0,d>0\n");
                            /* diskriminanda vecja od 0, ena ali dve realni resitvi: */
                            fi = M.arch(q / (r * r * r));
                            if (r != 0 && fi == 0)
                            {
                                /* two real solutions: */
                                t1 = -2 * r * Math.Cosh(fi / 3) + s;
                                t2 = r * Math.Cosh(fi / 3) + s;
                                if (t1 > t2) /* sorting by size */
                                {
                                    t = t1; t1 = t2; t2 = t;
                                }
                                x1 = t1; x2 = x3 = t2;
                                return 2;
                            }
                            else
                            {
                                /* one real solution: */
                                x1 = x2 = x3 = -2 * r * Math.Cosh(fi / 3) + s;
                                return 1;
                            }
                        }
                        else
                        {
                            // if (pr) printf("\ncubzeros: p<0,d<=0\n");
                            /* discriminant less than 0, there are 3 real zeros: */
                            fi = Math.Acos(q / (r * r * r));
                            t1 = -2 * r * Math.Cos(fi / 3) + s;
                            t2 = 2 * r * Math.Cos(Math.PI / 6 - fi / 3) + s;
                            t3 = 2 * r * Math.Cos(Math.PI / 6 + fi / 3) + s;
                            /* Sortiranje po velikosti: */
                            if (t1 > t3)
                            {
                                t = t1; t1 = t3; t3 = t;
                            }
                            if (t1 > t2)
                            {
                                t = t1; t1 = t2; t2 = t;
                            }
                            else if (t2 > t3)
                            {
                                t = t2; t2 = t3; t3 = t;
                            }
                            if (t1 == t2) /*degenerate cases */
                            {
                                if (t2 == t3)
                                {
                                    x1 = x2 = x3 = t1; return 1;
                                }
                                else
                                {
                                    x1 = t1; x2 = x3 = t3; return 2;
                                }
                            }
                            else if (t2 == t3)
                            {
                                x1 = t1; x2 = x3 = t3; return 2;
                            }
                            else /*nedegeneriran primer */
                            {
                                x1 = t1; x2 = t2; x3 = t3; return 3;
                            }
                        }
                    }
                }
            }  // Zeros()


            /// <summary>Returns the first zero of the current cubic function.
            /// Throws InvalidOperationException if the function does not have real zeros.</summary>
            public double Zero
            {
                get
                {
                    int num;
                    double x1, x2, x3;
                    num = Zeros(out x1, out x2, out x3);
                    if (num < 1)
                        throw new InvalidOperationException("This cubic function does not have any zeros.");
                    return x1;
                }
            }


            /// <summary>Returns the first zero of the current cubic function.
            /// Throws InvalidOperationException if the function does not have real zeros.</summary>
            public double Zero1
            {
                get
                {
                    int num;
                    double x1, x2, x3;
                    num = Zeros(out x1, out x2, out x3);
                    if (num < 1)
                        throw new InvalidOperationException("This cubic function does not have any zeros.");
                    return x1;
                }
            }

            /// <summary>Returns the second zero of the current cubic function.
            /// Throws InvalidOperationException if the function does not have two distinct real zeros.</summary>
            public double Zero2
            {
                get
                {
                    int num;
                    double x1, x2, x3;
                    num = Zeros(out x1, out x2, out x3);
                    if (num < 2)
                        throw new InvalidOperationException("This cubic function does not have two zeros.");
                    return x2;
                }
            }

            /// <summary>Returns the third zero of the current cubic function.
            /// Throws InvalidOperationException if the function does not have three distinct real zeros.</summary>
            public double Zero3
            {
                get
                {
                    int num;
                    double x1, x2, x3;
                    num = Zeros(out x1, out x2, out x3);
                    if (num < 3)
                        throw new InvalidOperationException("This cubic function does not have two zeros.");
                    return x3;
                }
            }

            /// <summary>Returns true if this cubic function has at least one real zero, false otherwise.</summary>
            public bool HasZero
            {
                get
                {
                    int num;
                    double x1, x2, x3;
                    num = Zeros(out x1, out x2, out x3);
                    return (num > 0);
                }
            }

            /// <summary>Returns number of zeros of the current cubic function.</summary>
            public int NumZeros
            {
                get
                {
                    int num;
                    double x1, x2, x3;
                    num = Zeros(out x1, out x2, out x3);
                    return (num);
                }
            }

            /// <summary>Calculates and returns extremes of the cubic function, and 
            /// returns number of extremes.</summary>
            /// <param name="x1">Abscissa of the first extreme.</param>
            /// <param name="y1">Value in the first extreme.</param>
            /// <param name="d1">Second derivative in the first extreme.</param>
            /// <param name="x2">Abscissa of the first extreme.</param>
            /// <param name="y2">Function value in the first extreme.</param>
            /// <param name="d2">Second derivative in the first extreme.</param>
            /// <returns></returns>
            int Extremes(ref double x1, ref double y1, ref double d1, 
                        ref double x2, ref double y2, ref double d2)
            {
                x1 = y1 = d1 = x2 = y2 = d2 = 0.0;
                double dis;
                if (a3 == 0) /* degenerate case (quadratic parabola) */
                {
                    if (a2 == 0)
                        return 0;
                    else
                    {
                        x1 = -a1 / (2 * a2);
                        y1 = a2 * x1 * x1 + a1 * x1 + a0;
                        d1 = 2 * a2;
                        return 1;
                    }
                }
                else
                {
                    if ((dis = (a2*a2) - 3 * a1 * a3) < 0)
                        return 0;
                    else if (dis == 0)
                    {
                        /* To se sicer ne sme zgoditi */
                        x1 = x2 = -a2 / (3 * a3);
                        y1 = y2 = RefValue(x1);
                        d1 = d2 = 2 * a2 + 6 * a3 * (x1);
                        return 1;
                    }
                    else
                    {
                        if (a3 > 0)
                        {
                            x1 = (-a2 - Math.Sqrt(dis)) / (3 * a3);
                            x2 = (-a2 + Math.Sqrt(dis)) / (3 * a3);
                        }
                        else
                        {
                            x2 = (-a2 - Math.Sqrt(dis)) / (3 * a3);
                            x1 = (-a2 + Math.Sqrt(dis)) / (3 * a3);
                        }
                        y1 = RefValue(x1);
                        d1 = 2 * a2 + 6 * a3 * (x1);
                        y2 = RefValue(x2);
                        d2 = 2 * a2 + 6 * a3 * (x2);
                        return 2;
                    }
                }
            }

            /// <summary>Returns true if this cubic function has a strict maximum.</summary>
            public bool HasMaximum
            {
                get
                {
                    bool found = false;
                    double extreme = 0;
                    double x1=0, y1=0, d1=0, x2=0, y2=0, d2=0;
                    int numExtremes;
                    numExtremes = Extremes(ref x1, ref y1, ref d1,
                        ref x2, ref y2, ref d2);
                    if (numExtremes > 0)
                    {
                        if (d1 < 0)
                        {
                            found = true;
                            extreme = x1;
                        }
                    }
                    if (!found && numExtremes > 1)
                    {
                        if (d2 < 0)
                        {
                            found = true;
                            extreme = x2;
                        }
                    }
                    if (!found)
                        throw new InvalidOperationException("Cubic function does not have a maximum."
                            + Environment.NewLine + "  Coefficients: a3 = " + a3
                            + ", a2 = " + a2 + ", a1 = " + a1 + ", a0 = " + a0 + ".");
                    return found;
                }
            }

            /// <summary>Gets strict maximum of this cubic function. 
            /// If the function does not have one then exeption is thrown.</summary>
            public double Maximum
            {
                get
                {
                    bool found = false;
                    double extreme = 0;
                    double x1 = 0, y1 = 0, d1 = 0, x2 = 0, y2 = 0, d2 = 0;
                    int numExtremes;
                    numExtremes = Extremes(ref x1, ref y1, ref d1,
                        ref x2, ref y2, ref d2);
                    if (numExtremes > 0)
                    {
                        if (d1 < 0)
                        {
                            found = true;
                            extreme = x1;
                        }
                    }
                    if (!found && numExtremes > 1)
                    {
                        if (d2 < 0)
                        {
                            found = true;
                            extreme = x2;
                        }
                    }
                    if (!found)
                        throw new InvalidOperationException("Cubic function does not have a maximum."
                            + Environment.NewLine + "  Coefficients: a3 = " + a3
                            + ", a2 = " + a2 + ", a1 = " + a1 + ", a0 = " + a0 + ".");
                    return extreme;
                }
            }

            /// <summary>Returns true if this cubic function has a strict minimum.</summary>
            public bool HasMinimum
            {
                get
                {
                    bool found = false;
                    double extreme = 0;
                    double x1 = 0, y1 = 0, d1 = 0, x2 = 0, y2 = 0, d2 = 0;
                    int numExtremes;
                    numExtremes = Extremes(ref x1, ref y1, ref d1,
                        ref x2, ref y2, ref d2);
                    if (numExtremes > 0)
                    {
                        if (d1 > 0)
                        {
                            found = true;
                            extreme = x1;
                        }
                    }
                    if (!found && numExtremes > 1)
                    {
                        if (d2 > 0)
                        {
                            found = true;
                            extreme = x2;
                        }
                    }
                    if (!found)
                        throw new InvalidOperationException("Cubic function does not have a strict minimum."
                            + Environment.NewLine + "  Coefficients: a3 = " + a3
                            + ", a2 = " + a2 + ", a1 = " + a1 + ", a0 = " + a0 + ".");
                    return found;
                }
            }

            /// <summary>Gets strict minimum of this cubic function. 
            /// If the function does not have one then exeption is thrown.</summary>
            public double Minimum
            {
                get
                {
                    bool found = false;
                    double extreme = 0;
                    double x1 = 0, y1 = 0, d1 = 0, x2 = 0, y2 = 0, d2 = 0;
                    int numExtremes;
                    numExtremes = Extremes(ref x1, ref y1, ref d1,
                        ref x2, ref y2, ref d2);
                    if (numExtremes > 0)
                    {
                        if (d1 > 0)
                        {
                            found = true;
                            extreme = x1;
                        }
                    }
                    if (!found && numExtremes > 1)
                    {
                        if (d2 > 0)
                        {
                            found = true;
                            extreme = x2;
                        }
                    }
                    if (!found)
                        throw new InvalidOperationException("Cubic function does not have a strict minimum."
                            + Environment.NewLine + "  Coefficients: a3 = " + a3
                            + ", a2 = " + a2 + ", a1 = " + a1 + ", a0 = " + a0 + ".");
                    return extreme;
                }
            }


            #endregion Data and Properties

            protected override double RefValue(double x)
            {
                return a0 + a1 * x + a2 * x*x + a3*x*x*x;
            }

            public override bool ValueDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for value defined for function " + Name + ".");
                }
            }

            protected override double RefDerivative(double x)
            {
                return 3 * a3 * x*x + 2 * a2 * x + a1;
            }

            public override bool DerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for derivative defined for function " + Name + ".");
                }
            }

            protected override double RefSecondDerivative(double x)
            {
                return 6 * a3 * x + 2 * a2;
            }

            public override bool SecondDerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for second derivative defined for function " + Name + ".");
                }
            }


            protected override double RefDerivative(double x, int order)
            {
                if (order < 0)
                    throw new ArgumentException("Derivative order less than 0 is not valid. Function: " + Name);
                if (order == 0)
                    return RefValue(x);
                else if (order == 1)
                    return RefDerivative(x);
                else if (order == 2)
                    return RefSecondDerivative(x);
                else if (order == 3)
                        return 6 * a3;
                else
                    return 0;
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            {
                throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                  + Name + ".");
            }

            protected override double RefIntegral(double x)
            {
                return a3 * Math.Pow(x,4)/4 + a2 * Math.Pow(x, 3) / 3 + a1 * Math.Pow(x, 2) / 2 + a0; ;
            }

            public override bool IntegralDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

            private Cubic _refinv;

            protected Cubic RefInv
            {
                get
                {
                    if (_refinv == null)
                    {
                        lock (Lock)
                        {
                            if (_refinv == null)
                                _refinv = new Cubic(0, 0, 0, 0);
                        }
                    }
                    return _refinv;
                }
            }
                



            protected override double RefInverse(double y)
            {
                lock (Lock)
                {
                    RefInv.SetCoefficients(a3, a2, a1, a0-y);
                    if (!RefInv.HasZero)
                        throw new InvalidOperationException("Cubic function does not have inverse at " + y + "."
                            + Environment.NewLine +  "  Coefficients: " + "a3 = " + a3 + ", a2 = " + a2
                            + ", a1 = " + a1 +", a0 = " + a0 + ".");
                    return RefInv.Zero;
                }
            }

            public override bool InverseDefined
            {
                // TODO: define inverse numerically, then set this to true! (create a special procedure for 
                // weighting functions with 
                // finite support!)
                get
                {
                    if (a1 != 0)
                        return true;
                    else
                        return false;
                }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for inverse defined for function " + Name + ".");
                }
            }

        } // class Cubic




        #endregion Cubic Interpolation


        #endregion Interpolation

    }

}
