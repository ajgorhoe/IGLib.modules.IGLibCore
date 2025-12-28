// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using IG.Lib;
using IG.Num;

namespace TSTX
{



    public static class Test1
    {


    }



}

namespace IG.Num
{
    /// <summary>Creation of a number of standard real mathematical functions in one dimension.
    /// Conttains subclasses for specific functions ans corresponding static creator methods.
    /// Creator methods come in 3 different version: for reference form of the function (e.g. just 
    /// Exp[x]), for form shifted and stretched in x direction, and general form shifted and stretched
    /// in both directions.
    /// </summary>
    /// $A Igor xx;
    public static partial class Func
    {

        #region ExpLog

        /// <summary>Creates and returns a new real exponential function object.</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetExp(double Kx, double Sx, double Ky, double Sy)
        { return new Exp(Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new real exponential function object.</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetExp(double Kx, double Sx)
        { return new Exp(Kx, Sx); }

        /// <summary>Creates and returns a new real exponential function object.</summary>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetExp()
        { return new Exp(); }

        /// <summary>A RealFunction class representing exponential function.</summary>
        public class Exp : RealFunction
        {

            public Exp(): this(1.0, 0.0, 1.0, 0.0)
            {  }

            public Exp(double Kx, double Sx) : this(Kx, Sx, 1.0, 0.0)
            { }

            public Exp(double Kx, double Sx, double Ky, double Sy)
            {
                Name = "Exp";
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            protected override double RefValue(double x)
            { return Math.Exp(x); }

            public override bool ValueDefined
            {
                get { return true; }
                protected internal set { throw new InvalidOperationException(
                    "Can not set a flag for value defined for function " + Name + "."); }
            }

            protected override double RefDerivative(double x)
            { return Math.Exp(x); }

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
            { return Math.Exp(x); }

            public override bool SecondDerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for second derivative defined for function " + Name + ".");
                }
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            { throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                + Name + "."); }

            protected override double RefIntegral(double x)
            { return Math.Exp(x) - 1.0; }

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
                if (x <= 0)
                {
                    if (x == 0)
                        return Double.NegativeInfinity;
                    else
                        throw new OverflowException("Inverse is not defined for negative arguments for function Exp.");
                }
                return Math.Log(x); 
            }

            public override bool InverseDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

        } // class FuncExp



        #endregion ExpLog

        #region ReciprocalPower


        /// <summary>Creates and returns a new real reciprocal power function object.</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetReciprocal(double Kx, double Sx, double Ky, double Sy)
        { return new ReciprocalPower(1, Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new real reciprocal power function object.</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetReciprocal(double Kx, double Sx)
        { return new ReciprocalPower(1, Kx, Sx); }

        /// <summary>Creates and returns a new real reciprocal power function object.</summary>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetReciprocal()
        { return new ReciprocalPower(1); }

        /// <summary>Creates and returns a new real reciprocal power function object.</summary>
        /// <param name="power">Power p. Reference function is f(x)=1/(x^p).</param>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetReciprocalPower(int power, double Kx, double Sx, double Ky, double Sy)
        { return new ReciprocalPower(power, Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new real reciprocal power function object.</summary>
        /// <param name="power">Power p. Reference function is f(x)=1/(x^p).</param>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetReciprocalPower(int power, double Kx, double Sx)
        { return new ReciprocalPower(power, Kx, Sx); }

        /// <summary>Creates and returns a new real reciprocal power function object.</summary>
        /// <param name="power">Power p. Reference function is f(x)=1/(x^p).</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetReciprocalPower(int power)
        { return new ReciprocalPower(power); }

        /// <summary>A RealFunction class representing a reciprocal power function.</summary>
        public class ReciprocalPower : RealFunction
        {

            public ReciprocalPower(int power)
                : this(power, 1.0, 0.0, 1.0, 0.0)
            { }

            public ReciprocalPower(int power, double Kx, double Sx)
                : this(power, Kx, Sx, 1.0, 0.0)
            { }

            public ReciprocalPower(int power, double Kx, double Sx, double Ky, double Sy)
            {
                if (_p <= 0)
                    throw new ArgumentException("Power must be greater than 0.");
                Name = "ReciprocalPower";
                _p = power;
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            protected int _p = 1;

            protected override double RefValue(double x)
            { return 1.0 / Math.Pow(x, _p); }

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
            { return -_p / Math.Pow(x, _p + 1.0); }

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
            { return (double)(_p * (_p + 1)) / Math.Pow(x, _p + 2.0); }

            public override bool SecondDerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for second derivative defined for function " + Name + ".");
                }
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
                if (_p == 1)
                    return Math.Log(Math.Abs(x));
                else
                    return 1.0 / ((1.0 - ((double)_p) * Math.Pow(x, 1 - _p)));
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
                return Math.Pow(x, _p);
            }

            public override bool InverseDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

        } // class ReciprocalPower



        #endregion ReciprocalPower



        #region Trigonometric


        #region InverseTrigonometric

        #endregion InverseTrigonometric


        #endregion Trigonometric


        #region Hyperbolic


        #region InverseHyperBolic

        #endregion InverseHyperbolic


        #endregion Hyperbolic


        #region Polynomial

        //IDENTITY OR LINEAR function objects: when translation and scaling is applied to the identity
        // function, linear function is obtained.

        /// <summary>Creates and returns a new real identity (or linear) function object.</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetIdentity(double Kx, double Sx, double Ky, double Sy)
        { return new Identity(Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new real identity (or linear) function object.</summary>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetIdentity(double Kx, double Sx)
        { return new Identity(Kx, Sx); }

        /// <summary>Creates and returns a new real identity (or linear) function object.</summary>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetIdentity()
        { return new Identity(); }

        /// <summary>A RealFunction class representing identity (or linear) function.</summary>
        public class Identity : RealFunction
        {

            public Identity()
                : this(1.0, 0.0, 1.0, 0.0)
            { }

            public Identity(double Kx, double Sx)
                : this(Kx, Sx, 1.0, 0.0)
            { }

            public Identity(double Kx, double Sx, double Ky, double Sy)
            {
                Name = "Identity";
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            protected override double RefValue(double x)
            { return x; }

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
            { return 1.0; }

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
            { return 0.0; }

            public override bool SecondDerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for second derivative defined for function " + Name + ".");
                }
            }

            public override double Derivative(double x, int order)
            { return 0; }

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
            { return x*x*0.5; }

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
                return x;
            }

            public override bool InverseDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

        } // class FuncIdentity



        /// <summary>Creates and returns a new real constant function object.</summary>
        /// <param name="constantValue">Value of the constant function.</param>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <param name="Ky">Scaling factor for dependent variable.</param>
        /// <param name="Sy">Shift in dependent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetConstant(double constantValue, double Kx, double Sx, double Ky, double Sy)
        { return new Constant(constantValue, Kx, Sx, Ky, Sy); }

        /// <summary>Creates and returns a new constant function object.</summary>
        /// <param name="constantValue">Value of the constant function.</param>
        /// <param name="Kx">Scaling factor for independent variable.</param>
        /// <param name="Sx">Shift in independent variable.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetConstant(double constantValue, double Kx, double Sx)
        { return new Constant(constantValue, Kx, Sx); }

        /// <summary>Creates and returns a new real identity (or linear) function object.</summary>
        /// <param name="constantValue">Value of the constant function.</param>
        /// <returns>RealFunction object.</returns>
        public static RealFunction GetConstant(double constantValue)
        { return new Constant(constantValue); }

        /// <summary>A RealFunction class representing a constant function.</summary>
        public class Constant : RealFunction
        {

            /// <summary>Constructs a new constant function.</summary>
            /// <param name="constantValue">Value of the constant function.</param>
            public Constant(double constantValue)
                : this(constantValue, 1.0, 0.0, 1.0, 0.0)
            { }

            /// <summary>Constructs a new constant function.</summary>
            /// <param name="constantValue">Value of the constant function.</param>
            /// <param name="Kx">Scaling factor for independent variable.</param>
            /// <param name="Sx">Shift in independent variable.</param>
            public Constant(double constantValue, double Kx, double Sx)
                : this(constantValue, Kx, Sx, 1.0, 0.0)
            { }

            /// <summary>Constructs a new constant function.</summary>
            /// <param name="constantValue">Value of the constant function.</param>
            /// <param name="Kx">Scaling factor for independent variable.</param>
            /// <param name="Sx">Shift in independent variable.</param>
            /// <param name="Ky">Scaling factor for dependent variable.</param>
            /// <param name="Sy">Shift in dependent variable.</param>
            public Constant(double constantValue, double Kx, double Sx, double Ky, double Sy)
            {
                this.ConstantValue = constantValue;
                Name = "Constant"; // + constantValue.ToString();
                SetTransformationParameters(Kx, Sx, Ky, Sy);
            }

            public double _constantValue = 0.0;

            /// <summary>Value of the current constant function.</summary>
            public double ConstantValue
            {
                get { return _constantValue; }
                set { _constantValue = value; }
            }

            protected override double RefValue(double x)
            { return ConstantValue; }

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
            { return 0.0; }

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
            { return 0.0; }

            public override bool SecondDerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for second derivative defined for function " + Name + ".");
                }
            }

            public override double Derivative(double x, int order)
            { return 0; }

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
            { return ConstantValue * x; }

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
                //return 0;
                if (x != ConstantValue)
                    throw new Exception("Constant function of 1 variable does not have an inverse defined.");
                else return 0;
            }

            public override bool InverseDefined
            {
                get { return false; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

        } // class FuncZero



        #endregion Polynomial

    }
}
