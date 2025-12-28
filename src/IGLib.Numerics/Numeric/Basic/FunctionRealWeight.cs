// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//      **** WEIGHTING FUNCTIONS **** 


namespace IG.Num
{



    /// <summary>Contains subclasses that represent various commonly used functions.</summary>
    /// $A Igor xx;
    public partial class Func
    {


        #region WeightInfiniteSupport


        // Gaussian-based (exp(-x^2)): 

        /// <summary>Creates and returns a new real polynomial weighting function object based on Gaussian function
        /// (exp(-x^2).
        /// Reference function: bell like function with infinite support, 
        ///     0 less than |f(x)| less than or equal to 1
        ///     f(0) = 1</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightGauss(double Kx, double Sx, double Ky, double Sy)
        { return new WeightGauss(Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new real polynomial weighting function object based on Gaussian function
        /// (exp(-x^2).
        /// Reference function: bell like function with infinite support, 
        ///     0 less than |f(x)| less than or equal to 1
        ///     f(0) = 1</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightGauss(double Kx, double Sx)
        { return new WeightGauss(Kx, Sx); }

        /// <summary>Creates and returns a new real polynomial weighting function object based on Gaussian function
        /// (exp(-x^2).
        /// Reference function: bell like function with infinite support, 
        ///     0 less than |f(x)| less than or equal to 1
        ///     f(0) = 1</summary>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightGauss()
        { return new WeightGauss(); }

        /// <summary>A RealFunction class representing bell like polynomial weighting function with finite support,
        ///  based  on Gaussian function (exp(-x^2)).
        /// Reference function: bell like function with infinite support, 
        ///     0 less than |f(x)| less than or equal to 1
        ///     f(0) = 1</summary>
        public class WeightGauss : RealFunction
        {

            public WeightGauss()
                : this(1.0, 0.0, 1.0, 0.0)
            { }

            public WeightGauss(double Kx, double Sx)
                : this(Kx, Sx, 1.0, 0.0)
            { }

            public WeightGauss(double Kx, double Sx, double Ky, double Sy)
            {
                Name = "WeightGauss";
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            protected override double RefValue(double x)
            {
                if (x < 0)
                    return RefValue(-x);
                else
                    return Math.Exp(-x*x);
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
                if (x < 0)
                    return - RefDerivative(-x);
                else return -2*Math.Exp(-x*x) *x;
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
                if (x < 0)
                    return RefSecondDerivative(-x);
                else return -2 * Math.Exp(-x*x) + 4 * Math.Exp(-x*x) * x*x;
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
                if (order == 0)
                    return RefValue(x);
                else if (order == 1)
                    return RefDerivative(x);
                else if (order == 2)
                    return RefSecondDerivative(x);
                else
                    throw new NotImplementedException("Defivatives higher than 2nd order are not defined for function "
                            + Name + ".");
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                if (order > 2)
                    return false;
                else
                    return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            {
                throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                  + Name + ".");
            }

            protected override double RefIntegral(double x)
            {
                if (x < 0)
                    return -RefIntegral(-x);
                else
                    // TODO: implement inverse (use error function from some library!)
                    throw new NotImplementedException("Integral not defined for function: " + Name + ".");
                    // return ;
            }

            public override bool IntegralDefined
            {
                get { return false; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

            protected override double RefInverse(double x)
            {
                if (x < 0)
                    throw new ArgumentException("Inverse not defined for y<0, function: " + Name + ".");
                else if (x > 1)
                    throw new ArgumentException("Inverse not defined for y>1, function: " + Name + ".");
                else if (x == 0)
                    return Double.PositiveInfinity;
                else if (x == 1)
                    return 0;
                else
                    return Math.Sqrt(-Math.Log(x));
            }

            public override bool InverseDefined
            {
                // TODO: define inverse numerically, then set this to true! (create a special procedure for 
                // weighting functions with 
                // finite support!)
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for inverse defined for function " + Name + ".");
                }
            }

        } // class FuncWeightGauss



        // Reciprocal power-based (1/(1+|x|^p)): 

        /// <summary>Creates and returns a new real polynomial weighting function object based on 
        /// reciprocal power function (1/(1+|x|^p)).
        /// Reference function: bell like function with infinite support, 
        ///     0 less than |f(x)| less than or equal to 1
        ///     f(0) = 1</summary>
        /// <param name="power">Power to which argument is raised.</param>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightReciprocalPower(int power, double Kx, double Sx, double Ky, double Sy)
        { return new WeightReciprocalPower(power, Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new real polynomial weighting function object based on 
        /// reciprocal power function (1/(1+|x|^p)).
        /// Reference function: bell like function with infinite support, 
        ///     0 less than |f(x)| less than or equal to 1
        ///     f(0) = 1</summary>
        /// <param name="power">Power to whic argument is raised.</param>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightReciprocalPower(int power, double Kx, double Sx)
        { return new WeightReciprocalPower(power, Kx, Sx); }

        /// <summary>Creates and returns a new real polynomial weighting function object based on 
        /// reciprocal power function (1/(1+|x|^p)).
        /// Reference function: bell like function with infinite support, 
        ///     0 less than |f(x)| less than or equal to 1
        ///     f(0) = 1</summary>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightReciprocalPower(int power)
        { return new WeightReciprocalPower(power); }

        /// <summary>A RealFunction class representing bell like polynomial weighting function with finite support,
        ///  based on reciprocal power function (1/(1+|x|^p) where p>0 is an integer power).
        /// Reference function: bell like function with infinite support, 
        ///     0 less than |f(x)| less than or equal to 1
        ///     f(0) = 1</summary>
        public class WeightReciprocalPower : RealFunction
        {

            /// <summary>Power p in expression 1/(1+|x|^p) for function value.</summary>
            protected int _p = 0;

            public WeightReciprocalPower(int power)
                : this(power, 1.0, 0.0, 1.0, 0.0)
            { }

            public WeightReciprocalPower(int power, double Kx, double Sx)
                : this(power, Kx, Sx, 1.0, 0.0)
            { }

            public WeightReciprocalPower(int power, double Kx, double Sx, double Ky, double Sy)
            {
                this._p = power;
                Name = "WeightReciprocalPower_" + power.ToString();
                if (power < 1)
                    throw new ArgumentException("Power must be greater than 1 in reciprocal power weighting function.");
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            protected override double RefValue(double x)
            {
                if (x < 0)
                    return RefValue(-x);
                else
                    return 1.0/(1.0 + Math.Pow(x,_p));
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
                if (x < 0)
                    return - RefDerivative(-x);
                else return -_p * Math.Pow(x, _p-1)/Math.Pow( 1+Math.Pow(x,_p) ,2);
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
                if (x < 0)
                    return RefSecondDerivative(-x);
                else
                {
                    double denominator = Math.Pow(1 + Math.Pow(x, _p), 2);
                    double counter = 2.0 * _p * _p * Math.Pow(x, 2 * _p - 2) / (1+Math.Pow(x,_p))
                            - (_p - 1) * _p * Math.Pow(x, _p-2);
                    return counter / denominator;
                };
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
                if (order == 0)
                    return RefValue(x);
                else if (order == 1)
                    return RefDerivative(x);
                else if (order == 2)
                    return RefSecondDerivative(x);
                else
                    throw new NotImplementedException("Defivatives higher than 2nd order are not defined for function "
                            + Name + ".");
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                if (order > 2)
                    return false;
                else
                    return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            {
                throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                  + Name + ".");
            }

            protected override double RefIntegral(double x)
            {
                if (x < 0)
                    return -RefIntegral(-x);
                else
                    throw new NotImplementedException("Integral not defined for function: " + Name + ".");
                // return ;
            }

            public override bool IntegralDefined
            {
                get { return false; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

            protected override double RefInverse(double x)
            {
                if (x < 0)
                    throw new ArgumentException("Inverse not defined for y<0, function: " + Name + ".");
                else if (x > 1)
                    throw new ArgumentException("Inverse not defined for y>1, function: " + Name + ".");
                else if (x == 0)
                    return Double.PositiveInfinity;
                else if (x == 1)
                    return 0;
                else
                    return Math.Pow((1.0/x)-1, 1.0/_p);
            }

            public override bool InverseDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for inverse defined for function " + Name + ".");
                }
            }

        } // class FuncWeightReciprocalPower



        #endregion WeightInfiniteSupport



        #region WeightFiniteSupportPolynomial


        // 3rd order polynomial-based:


        /// <summary>Creates and returns a new real polynomial weighting function object.
        /// Reference function: bell like function with final support, 
        ///     0 less than or equal to |f(x)| less than or equall to 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol3(double Kx, double Sx, double Ky, double Sy)
        { return new WeightPol3(Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new real polynomial weighting function object.
        /// Reference function: bell like function with final support, 
        ///     0 less than or equal to |f(x)| less than or equal to 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol3(double Kx, double Sx)
        { return new WeightPol3(Kx, Sx); }

        /// <summary>Creates and returns a new real polynomial weighting function object.
        /// Reference function: bell like function with final support, 
        ///     0 less than or equal to |f(x)| less than or equal to 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol3()
        { return new WeightPol3(); }

        /// <summary>A RealFunction class representing bell like polynomial weighting function with finite support.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        public class WeightPol3 : RealFunction
        {

            public WeightPol3()
                : this(1.0, 0.0, 1.0, 0.0)
            { }

            public WeightPol3(double Kx, double Sx)
                : this(Kx, Sx, 1.0, 0.0)
            { }

            public WeightPol3(double Kx, double Sx, double Ky, double Sy)
            {
                Name = "WeightPol_3";
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            protected override double RefValue(double x)
            {
                if (x < 0)
                    return RefValue(-x);
                else if (x >= 1)
                    return 0;
                else
                    return 1 - 3 * x*x + 2 * x*x*x;
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
                if (x < 0)
                    return - RefDerivative(-x);
                else if (x >= 1)
                    return 0;
                else return -6 * x + 6 * x*x;
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
                if (x < 0)
                    return RefSecondDerivative(-x);
                else if (x >= 1)
                    return 0;
                else return -6 + 12 * x;
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
                if (order == 0)
                    return RefValue(x);
                else if (order == 1)
                    return RefDerivative(x);
                else if (order == 2)
                    return RefSecondDerivative(x);
                else
                    throw new NotImplementedException("Defivatives higher than 2nd order are not defined for function "
                            + Name + ".");
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                if (order>2)
                    return false;
                else
                    return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            {
                throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                  + Name + ".");
            }

            protected override double RefIntegral(double x)
            {
                if (x < 0)
                    return -RefIntegral(-x);
                else if (x > 1)
                    return RefIntegral(1);
                else
                    return x - x*x*x + 0.5 * x*x*x*x;
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
                // TODO: define inverse numerically! (create a special procedure for weighting functions with 
                // finite support!)
                throw new NotImplementedException("Inverse not defined for function: " + Name + ".");
            }

            public override bool InverseDefined
            {
                // TODO: define inverse numerically, then set this to true! (create a special procedure for 
                // weighting functions with 
                // finite support!)
                get { return false; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for inverse defined for function " + Name + ".");
                }
            }

        } // class FuncWeightPol3


        // 4th order polynomial-based: 

        /// <summary>Creates and returns a new real polynomial weighting function object based on 4th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol4(double Kx, double Sx, double Ky, double Sy)
        { return new WeightPol4(Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new real polynomial weighting function object based on 4th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol4(double Kx, double Sx)
        { return new WeightPol4(Kx, Sx); }

        /// <summary>Creates and returns a new real polynomial weighting function object based on 4th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol4()
        { return new WeightPol4(); }

        /// <summary>A RealFunction class representing bell like polynomial weighting function with finite support,
        ///  based on 4th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        public class WeightPol4 : RealFunction
        {

            public WeightPol4()
                : this(1.0, 0.0, 1.0, 0.0) 
            { }

            public WeightPol4(double Kx, double Sx)
                : this(Kx, Sx, 1.0, 0.0)
            { }

            public WeightPol4(double Kx, double Sx, double Ky, double Sy)
            {
                Name = "WeightPol_4";
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            protected override double RefValue(double x)
            {
                if (x < 0)
                    return RefValue(-x);
                else if (x >= 1)
                    return 0;
                else
                    return 1- 6 * x*x+ 8 * x*x*x- 3 * x*x*x*x;
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
                if (x < 0)
                    return - RefDerivative(-x);
                else if (x >= 1)
                    return 0;
                else return -12*x+24*x*x-12*x*x*x;
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
                if (x < 0)
                    return RefSecondDerivative(-x);
                else if (x >= 1)
                    return 0;
                else return -12 + 48 * x -36 * x*x;
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
                if (order == 0)
                    return RefValue(x);
                else if (order == 1)
                    return RefDerivative(x);
                else if (order == 2)
                    return RefSecondDerivative(x);
                else
                    throw new NotImplementedException("Defivatives higher than 2nd order are not defined for function "
                            + Name + ".");
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                if (order > 2)
                    return false;
                else
                    return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            {
                throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                  + Name + ".");
            }

            protected override double RefIntegral(double x)
            {
                if (x < 0)
                    return -RefIntegral(-x);
                else if (x > 1)
                    return RefIntegral(1);
                else
                    return x - 2 * x*x*x + 2 * x*x*x*x - 3.0 * x*x*x*x*x / 5.0;
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
                // TODO: define inverse numerically! (create a special procedure for weighting functions with 
                // finite support!)
                throw new NotImplementedException("Inverse not defined for function: " + Name + ".");
            }

            public override bool InverseDefined
            {
                // TODO: define inverse numerically, then set this to true! (create a special procedure for 
                // weighting functions with 
                // finite support!)
                get { return false; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for inverse defined for function " + Name + ".");
                }
            }

        } // class FuncWeightPol4



        // 5th order polynomial-based: 

        /// <summary>Creates and returns a new real polynomial weighting function object based on 5th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol5(double Kx, double Sx, double Ky, double Sy)
        { return new WeightPol5(Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new real polynomial weighting function object based on 5th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol5(double Kx, double Sx)
        { return new WeightPol5(Kx, Sx); }

        /// <summary>Creates and returns a new real polynomial weighting function object based on 5th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol5()
        { return new WeightPol5(); }

        /// <summary>A RealFunction class representing bell like polynomial weighting function with finite support,
        ///  based on 5th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        public class WeightPol5 : RealFunction
        {

            public WeightPol5()
                : this(1.0, 0.0, 1.0, 0.0)
            { }

            public WeightPol5(double Kx, double Sx)
                : this(Kx, Sx, 1.0, 0.0)
            { }

            public WeightPol5(double Kx, double Sx, double Ky, double Sy)
            {
                Name = "WeightPol_5";
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            protected override double RefValue(double x)
            {
                if (x < 0)
                    return RefValue(-x);
                else if (x >= 1)
                    return 0;
                else
                    return 1 - 10 * x*x*x + 15 * Math.Pow(x, 4) - 6 * Math.Pow(x, 5);
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
                if (x < 0)
                    return - RefDerivative(-x);
                else if (x >= 1)
                    return 0;
                else return -30 * x*x + 60 * x*x*x - 30 * x*x*x*x;
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
                if (x < 0)
                    return RefSecondDerivative(-x);
                else if (x >= 1)
                    return 0;
                else return -60*x + 180*x*x -120*x*x*x;
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
                if (order == 0)
                    return RefValue(x);
                else if (order == 1)
                    return RefDerivative(x);
                else if (order == 2)
                    return RefSecondDerivative(x);
                else
                    throw new NotImplementedException("Defivatives higher than 2nd order are not defined for function "
                            + Name + ".");
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                if (order > 2)
                    return false;
                else
                    return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            {
                throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                  + Name + ".");
            }

            protected override double RefIntegral(double x)
            {
                if (x < 0)
                    return -RefIntegral(-x);
                else if (x > 1)
                    return RefIntegral(1);
                else
                    return x - (5.0/2.0) * Math.Pow(x,4) + 3 * Math.Pow(x, 5) - Math.Pow(x, 6);
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
                // TODO: define inverse numerically! (create a special procedure for weighting functions with 
                // finite support!)
                throw new NotImplementedException("Inverse not defined for function: " + Name + ".");
            }

            public override bool InverseDefined
            {
                // TODO: define inverse numerically, then set this to true! (create a special procedure for 
                // weighting functions with 
                // finite support!)
                get { return false; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for inverse defined for function " + Name + ".");
                }
            }

        } // class FuncWeightPol5



        // 7th order polynomial-based: 

        /// <summary>Creates and returns a new real polynomial weighting function object based on 7th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol7(double Kx, double Sx, double Ky, double Sy)
        { return new WeightPol7(Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new real polynomial weighting function object based on 7th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol7(double Kx, double Sx)
        { return new WeightPol7(Kx, Sx); }

        /// <summary>Creates and returns a new real polynomial weighting function object based on 7th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetWeightPol7()
        { return new WeightPol7(); }

        /// <summary>A RealFunction class representing bell like polynomial weighting function with finite support,
        ///  based on 7th order polynomial.
        /// Reference function: bell like function with final support, 
        ///     0 less or equal |f(x)| less or equal 1
        ///     f(x less than -1) = f(x greater than 1) = 0
        ///     f(0) = 1</summary>
        public class WeightPol7 : RealFunction
        {

            public WeightPol7()
                : this(1.0, 0.0, 1.0, 0.0)
            { }

            public WeightPol7(double Kx, double Sx)
                : this(Kx, Sx, 1.0, 0.0)
            { }

            public WeightPol7(double Kx, double Sx, double Ky, double Sy)
            {
                Name = "WeightPol_7";
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            protected override double RefValue(double x)
            {
                if (x < 0)
                    return RefValue(-x);
                else if (x >= 1)
                    return 0;
                else
                    return 1 - 35 * Math.Pow(x,4) + 84 * Math.Pow(x, 5) - 
                        70 * Math.Pow(x,6) + 20 * Math.Pow(x,7);
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
                if (x < 0)
                    return - RefDerivative(-x);
                else if (x >= 1)
                    return 0;
                else return -140 * Math.Pow(x,3) + 420 * Math.Pow(x,4)
                        -420 * Math.Pow(x, 5) +140*Math.Pow(x, 6);
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
                if (x < 0)
                    return RefSecondDerivative(-x);
                else if (x >= 1)
                    return 0;
                else return -420 * x*x + 1680 * Math.Pow(x, 3)
                        - 2100 * Math.Pow(x, 4) + 840 * Math.Pow(x,5);
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
                if (order == 0)
                    return RefValue(x);
                else if (order == 1)
                    return RefDerivative(x);
                else if (order == 2)
                    return RefSecondDerivative(x);
                else
                    throw new NotImplementedException("Defivatives higher than 2nd order are not defined for function "
                            + Name + ".");
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                if (order > 2)
                    return false;
                else
                    return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            {
                throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                  + Name + ".");
            }

            protected override double RefIntegral(double x)
            {
                if (x < 0)
                    return -RefIntegral(-x);
                else if (x > 1)
                    return RefIntegral(1);
                else
                    return x - 7 * Math.Pow(x,5) + 14 * Math.Pow(x,6)
                        - 10 * Math.Pow(x, 7) + (5.0/2.0) * Math.Pow(x,8);
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
                // TODO: define inverse numerically! (create a special procedure for weighting functions with 
                // finite support!)
                throw new NotImplementedException("Inverse not defined for function: " + Name + ".");
            }

            public override bool InverseDefined
            {
                // TODO: define inverse numerically, then set this to true! (create a special procedure for 
                // weighting functions with 
                // finite support!)
                get { return false; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for inverse defined for function " + Name + ".");
                }
            }

        } // class FuncWeightPol7



        #endregion WeightFiniteSupportPolynomial


    }

}
