// GENERIC FUNCTIONS

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{

    public enum Operation { None = 0, Addition, Subtraction, Multiplication, Division, 
        Composition, UnaryPlus, UnaryMinus }

    /// <summary>Operators on univariate real functions, transforms a function to obtain another one.</summary>
    /// <param name="f">Function that is transformed.</param>
    /// <returns>Transformed function.</returns>
    /// $A Igor xx;
    public delegate RealFunction DlgFunctionTransformation(RealFunction f);

    /// <summary>Represents real function of real variable.</summary>
    /// $A Igor xx;
    public delegate double DlgFunctionValue(double x);

    /// <summary>Represents derivative of real function of real variable of arbitrary order.</summary>
    /// $A Igor xx;
    public delegate double DlgFunctionHigherDerivative(double x, int order);

    /// <summary>Reprents parametric family of real functions of real variable.</summary>
    /// $A Igor xx;
    public delegate double DlgFunctionParametric(double x, Vector p);

    /// <summary>Arbitrary order derivative of parametric family of real functions with respect to function argument.</summary>
    /// $A Igor xx;
    public delegate double DlgFunctionParametricHigherDerivative(double x, Vector p, int order);


    /// <summary>Interface for real functions.</summary>
    /// $A Igor xx;
    public interface IRealFunction
    {

        #region Data

        /// <summary>Returns a short name of the function.</summary>
        string Name { get; }

        /// <summary>Returns a short description of the function.</summary>
        string Description { get; }

        #endregion Data

        #region Evaluation

        /// <summary>Returns the value of this function at the specified parameter.</summary>
        double Value(double x);
    
        /// <summary>Tells whether value of the function is defined by implementation.</summary>
        bool ValueDefined { get; }


        /// <summary>Returns the first derivative of this function at the specified parameter.</summary>
        double Derivative(double x);

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        bool DerivativeDefined { get; }


        /// <summary>Returns the derivative of the given order of this function at the specified parameter.</summary>
        double Derivative(double x, int order);

        /// <summary>Tells whether the derivative of the given order is defined for this function (by implementation, not mathematically)</summary>
        bool HigherDerivativeDefined(int order);

        /// <summary>Returns the second derivative of the given order of this function at the specified arameter.</summary>
        double SecondDerivative(double x);

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        bool SecondDerivativeDefined { get; }


        /// <summary>Returns integral from 0 to x of the function.
        /// Throws an exception if the integral is not defined.</summary>
        /// <param name="x">Upper bound of the integral.</param>
        double Integral(double x);

        /// <summary>Tells whether analytical itegral of the function is defined or not.</summary>
        bool IntegralDefined { get; }


        /// <summary>Returns Inverse of the current function applied to function argument.
        /// Throws an exception if the inverse function is not defined.</summary>
        double Inverse(double y);

        /// <summary>Tells whether analytical inverse function is defined or not.</summary>
        bool InverseDefined { get; }

        #endregion Evaluation

        #region Numeric
        
        /// <summary>Tabulates the current function and its first and second derivatives 
        /// (when available) on the specified interval, in the specified number of points.</summary>
        /// <param name="from">Left interval bound.</param>
        /// <param name="to">Right interval bound.</param>
        /// <param name="numPoints">Number of points in which the function is calculated.</param>
        void Tabulate(double from, double to, int numPoints);

        /// <summary>Tabulates the current function and its first and eventually second derivatives 
        /// (when available) on the specified interval, in the specified number of points.</summary>
        /// <param name="from">Left interval bound.</param>
        /// <param name="to">Right interval bound.</param>
        /// <param name="numPoints">Number of points in which the function is calculated.</param>
        /// <param name="printDerivatives">Whether to print the  derivatives.</param>
        void Tabulate(double from, double to, int numPoints, bool printDerivatives);

        /// <summary>Tabulates the current function and eventually its first and second derivatives 
        /// (when available) on the specified interval, in the specified number of points.</summary>
        /// <param name="from">Left interval bound.</param>
        /// <param name="to">Right interval bound.</param>
        /// <param name="numPoints">Number of points in which the function is calculated.</param>
        /// <param name="printDerivatives">Whether to print the derivatives.</param>
        /// <param name="printSecondDerivatives">Whether to print the second derivatives.</param>
        void Tabulate(double from, double to, int numPoints,
            bool printDerivatives, bool printSecondDerivatives);

        /// <summary>Calculates numerical integral of this function. Simpson's formula is usually used.</summary>
        /// <param name="from">Lower integral limit.</param>
        /// <param name="to">Upper integral limit.</param>
        /// <param name="numintervals">Number of subintervals (1 less thatn the number of evaluation points)</param>
        /// <returns>Numerical integral.</returns>
        double NumericalIntegral(double from, double to, int numintervals);

        /// <summary>Calculates numerical derivative of this function. Central difference formula is used.</summary>
        /// <param name="x">Point at which derivative is calculated.</param>
        /// <param name="stepsize">Step size.</param>
        /// <returns>Numerical derivative.</returns>
        double NumericalDerivative(double x, double stepsize);

        /// <summary>Calculates numerical second order derivative of this function. Central difference formula is used.</summary>
        /// <param name="x">Point at which second order derivative is calculated.</param>
        /// <param name="stepsize">Step size.</param>
        /// <returns>Numerical derivative.</returns>
        double NumericalSecondDerivative(double x, double stepsize);

        #endregion Numeric

    }  //  interface IRealFunction




    /// <summary>Base class for real functions of real variable.</summary>
    /// <remarks>
    /// <para>A number of predefined functions can be bound in the class <see cref="Func"/>.</para>
    /// </remarks>
    /// $A Igor xx;
    public abstract class RealFunctionBase : IRealFunction
    {

        #region Data

        //protected string _name;

        //protected string _description;

        ///// <summary>Returns a short name of the function.</summary>
        //public virtual string Name { get; protected internal set; }

        ///// <summary>Returns a short description of the function.</summary>
        //public abstract string Description { get; protected internal set; }

        protected string _name;

        protected string _description;


        /// <summary>Returns a short name of thecurrent function.</summary>
        public virtual string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = "Function_" + this.GetType().Name;
                return _name;
            }
            protected internal set { _name = value; }
        }

        /// <summary>Returns a short description of the current function.</summary>
        public virtual string Description
        {
            get
            {
                if (string.IsNullOrEmpty(_description))
                    _description = "Function " + this.GetType().FullName + ".";
                return _description;
            }
            protected internal set { _description = value; }
        }

        /// <summary>Returns a function object that represents a derivative of this function.
        /// Returns null if this is not implemented.</summary>
        public virtual RealFunctionBase DerivativeFunction
        { get { return null; } }

        /// <summary>Returns a function object that represents a definite integral of this function from 0 to 1.
        /// Returns null if not implemented.</summary>
        public virtual RealFunctionBase IntegralFunction
        { get { return null; } }

        /// <summary>Returns a function object that represents an inverse function of this function.
        /// Returns null if not implemented.</summary>
        public virtual RealFunctionBase InverseFunction
        { get { return null; } }


        #endregion // Data

        #region Evaluation

        /// <summary>Returns the value of this function at the specified parameter.</summary>
        public abstract double Value(double x);

        /// <summary>Tells whether value of the function is defined by implementation.</summary>
        public abstract bool ValueDefined { get; protected internal set; }


        /// <summary>Returns the first derivative of this function at the specified parameter.</summary>
        public abstract double Derivative(double x);

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public abstract bool DerivativeDefined { get; protected internal set; }


        /// <summary>Returns the derivative of the given order of this function at the specified parameter.</summary>
        public abstract double Derivative(double x, int order);

        /// <summary>Tells whether the derivative of the given order is defined for this function (by implementation, not mathematically)</summary>
        public abstract bool HigherDerivativeDefined(int order);


        /// <summary>Returns the second derivative of the given order of this function at the specified arameter.</summary>
        public abstract double SecondDerivative(double x);

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public abstract bool SecondDerivativeDefined { get; protected internal set; }


        /// <summary>Returns integral from 0 to x of the function.
        /// Throws an exception if the integral is not defined.</summary>
        /// <param name="x">Upper bound of the integral.</param>
        public abstract double Integral(double x);

        /// <summary>Tells whether analytical itegral of the function is defined or not.</summary>
        public abstract bool IntegralDefined { get; protected internal set; }


        /// <summary>Returns Inverse of the current function applied to function argument.
        /// Throws an exception if the inverse function is not defined.</summary>
        public abstract double Inverse(double y);

        /// <summary>Tells whether analytical inverse function is defined or not.</summary>
        public abstract bool InverseDefined { get; protected internal set; }

        #endregion Evaluation

        #region Numeric

        /// <summary>Tabulates the current function and its first and second derivatives 
        /// (when available) on the specified interval, in the specified number of points.</summary>
        /// <param name="from">Left interval bound.</param>
        /// <param name="to">Right interval bound.</param>
        /// <param name="numPoints">Number of points in which the function is calculated.</param>
        public void Tabulate(double from, double to, int numPoints)
        {
            Tabulate(from, to, numPoints, true /* printDerivatives */);
        }

        /// <summary>Tabulates the current function and its first and eventually second derivatives 
        /// (when available) on the specified interval, in the specified number of points.</summary>
        /// <param name="from">Left interval bound.</param>
        /// <param name="to">Right interval bound.</param>
        /// <param name="numPoints">Number of points in which the function is calculated.</param>
        /// <param name="printDerivatives">Whether to print the  derivatives.</param>
        public void Tabulate(double from, double to, int numPoints, bool printDerivatives)
        {
            Tabulate(from, to, numPoints, true, true /* printSecondDerivatives */);
        }

        /// <summary>Tabulates the current function and eventually its first and second derivatives 
        /// (when available) on the specified interval, in the specified number of points.</summary>
        /// <param name="from">Left interval bound.</param>
        /// <param name="to">Right interval bound.</param>
        /// <param name="numPoints">Number of points in which the function is calculated.</param>
        /// <param name="printDerivatives">Whether to print the derivatives.</param>
        /// <param name="printSecondDerivatives">Whether to print the second derivatives.</param>
        public void Tabulate(double from, double to, int numPoints,
            bool printDerivatives, bool printSecondDerivatives)
        {
            if (!this.ValueDefined)
                throw new ArgumentException("Can not tabulate the function, value is not defined.");
            Console.WriteLine("Tabulating function, {0} values from {0,G5} to {1,G5}");
            if (printDerivatives)
            {
                if (!this.DerivativeDefined)
                {
                    printDerivatives = false;
                    Console.WriteLine("Derivatives will not be printed; not defined.");
                }
            }
            if (printSecondDerivatives)
            {
                if (!this.SecondDerivativeDefined)
                {
                    printSecondDerivatives = false;
                    Console.WriteLine("Second derivatives will not be printed; not defined.");
                }
            }
            Console.WriteLine("Function: " + this.ToString());
            Console.Write("  {0,10}  {1,-10}", "x", "f(x)");
            if (printDerivatives)
                Console.Write("  {0,-10}", "f'(x)");
            if (printSecondDerivatives)
                Console.Write("  {0,-10}", "f''(x)");
            Console.WriteLine();
            double step = to - from;
            if (numPoints > 2)
                step /= (double)(numPoints - 1);
            for (int i = 0; i < numPoints; ++i)
            {
                double x = from + (double)i * step;
                double f = this.Value(x);
                Console.Write("  {0,10:G8}  {1,-10:G8}", x, f);
                if (printDerivatives)
                {
                    double fDer = this.Derivative(x);
                    Console.Write("  {0,-10:G8}", fDer);
                }
                if (printSecondDerivatives)
                {
                    double fDer2 = this.SecondDerivative(x);
                    Console.Write("  {0,-10:G8}", fDer2);
                }
                Console.WriteLine();
            }
        }


        /// <summary>Calculates numerical integral of this function. Simpson's formula is used.</summary>
        /// <param name="from">Lower integral limit.</param>
        /// <param name="to">Upper integral limit.</param>
        /// <param name="numintervals">Number of subintervals (1 less thatn the number of evaluation points)</param>
        /// <returns>Numerical integral.</returns>
        public virtual double NumericalIntegral(double from, double to, int numintervals)
        {
            return Numeric.IntegralSimpson(Value, from, to, numintervals);
        }

        /// <summary>Calculates numerical derivative of this function. Central difference formula is used.</summary>
        /// <param name="x">Point at which derivative is calculated.</param>
        /// <param name="stepsize">Step size.</param>
        /// <returns>Numerical derivative.</returns>
        public virtual double NumericalDerivative(double x, double stepsize)
        {
            return Numeric.DerivativeCD(Value, x, stepsize);
        }

        /// <summary>Calculates numerical second order derivative of this function. Central difference formula is used.</summary>
        /// <param name="x">Point at which second order derivative is calculated.</param>
        /// <param name="stepsize">Step size.</param>
        /// <returns>Numerical derivative.</returns>
        public virtual double NumericalSecondDerivative(double x, double stepsize)
        {
            return Numeric.SecondDerivativeCD(Value, x, stepsize);
        }

        #endregion Numeric

        #region Testing


        /// <summary>Performs numerical tests with parameters adjusted for specific function.
        /// This function can be be overridden in derived classes, however its current implementation
        /// may be relatively well suited for most weighting and basic functions.</summary>
        public virtual void Test()
        {
            Test(-1.18, 2.1, 17, 1.0e-6, 5.0e-3);
        }

        /// <summary>Performs some numerical tests on the current function, such as correctness of
        /// first and second derivatives, integral and inverse of the function.
        /// Results are written to the standard output. Whenever a numerical result does not match
        /// the corresponding analytical value calculated by the function, a visible notification
        /// is written.</summary>
        /// <param name="from">Lower bound of the interval where tests are performed.</param>
        /// <param name="to">Upper bound of the interval where tests are performed.</param>
        /// <param name="numProbes">Number of points for which evaluations and comparisons are performed.</param>
        /// <param name="stepSize">Step size used for numerical integration and differentiation.</param>
        /// <param name="tolerance">Absolute tolarence for match between analytical and numeriacl values
        /// (visible notification is launched whenever difference is larger than tolerance).</param>
        public virtual void Test(double from, double to, int numProbes, double stepSize, double tolerance)
        {
            Console.WriteLine();
            Console.WriteLine("Testing correctness of implementation of function " + Name + ".");
            Console.WriteLine("Test intervel: [" + from + ", " + to + "] , num. probes: " 
                + numProbes + ", step: " + stepSize + ", tolerance: ", + tolerance);
            double x;
            double error, relativeerror;
            if (tolerance<0)
                tolerance = -tolerance;
            double probeStep = (to-from)/((double) numProbes - 1.0);
            // First derivative:
            if (DerivativeDefined)
            {
                Console.WriteLine();
                Console.WriteLine("Derivative:");
                double der, numder;
                for (int i=0; i<numProbes; ++i)
                {
                    x = from + (double)i * probeStep;
                    der = Derivative(x);
                    numder = NumericalDerivative(x, stepSize);
                    error = numder - der;
                    relativeerror = Math.Abs(2 * (numder - der) / (Math.Abs(numder) + Math.Abs(der)));
                    if (Math.Abs(error) > tolerance)
                        Console.WriteLine("!!!! large ERROR:");
                    Console.WriteLine("x: " + x.ToString() + ", der.: " + der + ", " + Environment.NewLine + 
                        "        err: " + error + ", rel.: " + relativeerror );
                }
            } else
                Console.WriteLine("Analytical derivative is not defined." + Environment.NewLine);
            // Second derivative:
            if (SecondDerivativeDefined)
            {
                Console.WriteLine();
                Console.WriteLine("Second derivative:");
                double der, numder;
                for (int i=0; i<numProbes; ++i)
                {
                    x = from + (double)i * probeStep;
                    der = SecondDerivative(x);
                    numder = NumericalSecondDerivative(x, stepSize);
                    error = numder - der;
                    relativeerror = Math.Abs(2 * (numder - der) / (Math.Abs(numder) + Math.Abs(der)));
                    if (Math.Abs(error) > tolerance)
                        Console.WriteLine("!!!! large ERROR:");
                    Console.WriteLine("x: " + x.ToString() + ", der.: " + der + ", " + Environment.NewLine +
                        "        err: " + error + ", rel.: " + relativeerror);
                }
            } else
                Console.WriteLine("Analytical second derivative is not defined." + Environment.NewLine);

            // Integral:
            if (IntegralDefined)
            {
                Console.WriteLine();
                Console.WriteLine("Integral:");
                double integral, numintegral;
                for (int i = 0; i < numProbes; ++i)
                {
                    x = from + (double)i * probeStep;
                    integral = Integral(x);
                    numintegral = NumericalIntegral(0, x, numProbes);
                    error = numintegral - integral;
                    relativeerror = Math.Abs(2 * (numintegral - integral) / (Math.Abs(numintegral) + Math.Abs(integral)));
                    if (Math.Abs(error) > tolerance)
                        Console.WriteLine("!!!! large ERROR:");
                    Console.WriteLine("x: " + x.ToString() + ", int.: " + integral + ", " + Environment.NewLine +
                        "        err: " + error + ", rel.: " + relativeerror);
                }
            } else
                Console.WriteLine("Analytical integral is not defined." + Environment.NewLine);

            // Inverse:
            if (InverseDefined)
            {
                Console.WriteLine();
                Console.WriteLine("Inverse:");
                double y;
                double xinv;
                double yinv;
                for (int i = 0; i < numProbes; ++i)
                {
                    x = from + (double)i * probeStep;
                    y = Value(x);
                    xinv = Inverse(y);
                    yinv = Value(xinv);
                    error = yinv - y;
                    relativeerror = Math.Abs(2 * (yinv - y) / (Math.Abs(yinv) + Math.Abs(y)));
                    if (Math.Abs(error) > tolerance)
                        Console.WriteLine("!!!! large ERROR:");
                    Console.WriteLine("x: " + x.ToString() + ", x inv.: " + xinv + ", " + Environment.NewLine +
                        "        err: " + error + ", rel.: " + relativeerror);
                }
            } else
                Console.WriteLine("Analytical inverse is not defined." + Environment.NewLine);
            Console.WriteLine();
        }

        public static void ExampleTests()
        {
            Console.WriteLine();
            Console.WriteLine("RealFunction tests of implementation: ");
            RealFunction rf;
            double
                Kx = 1.11,
                Sx = 0.132,
                Ky = 3.1,
                Sy = -0.06;

            //Console.WriteLine(Environment.NewLine + Environment.NewLine 
            //        + "Basic elementary functions:");
            //rf = Func.GetExp(Kx, Sx, Ky, Sy);
            //rf.Test();

            //Console.WriteLine(Environment.NewLine + Environment.NewLine
            //        + "Weighting functions with finite support:");
            //rf = Func.GetWeightPol3(Kx, Sx, Ky, Sy);
            //rf.Test();
            //rf = Func.GetWeightPol4(Kx, Sx, Ky, Sy);
            //rf.Test();
            //rf = Func.GetWeightPol5(Kx, Sx, Ky, Sy);
            //rf.Test();
            //rf = Func.GetWeightPol7(Kx, Sx, Ky, Sy);
            //rf.Test();

            Console.WriteLine(Environment.NewLine + Environment.NewLine
                    + "Weighting functions with infinite support:");
            rf = Func.GetWeightGauss(Kx, Sx, Ky, Sy);
            rf.Test();
            //rf = Func.GetWeightReciprocalPower(1, Kx, Sx, Ky, Sy);
            //rf.Test();
            //rf = Func.GetWeightReciprocalPower(2, Kx, Sx, Ky, Sy);
            //rf.Test();
            //rf = Func.GetWeightReciprocalPower(3, Kx, Sx, Ky, Sy);
            //rf.Test();
            //rf = Func.GetWeightReciprocalPower(4, Kx, Sx, Ky, Sy);
            //rf.Test();
            //rf = Func.GetWeightReciprocalPower(5, Kx, Sx, Ky, Sy);
            //rf.Test();
            //rf = Func.GetWeightReciprocalPower(10, Kx, Sx, Ky, Sy);
            //rf.Test();
        }


        #endregion Testing

        #region Auxiliary

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return this.Name;
            else
                return "Real function";
        }

        #endregion Auxiliary


    }  // class FunctionBase


    /// <summary>Represents a real function of a real variabe.</summary>
    /// $A Igor xx;
    public partial class RealFunction : RealFunctionBase, IRealFunction, ICloneable
    {

        #region Construction

        protected RealFunction()
        {  }

        /// <summary>Constructs a new function where delegates are provided for calculation of function 
        /// valu.</summary>
        /// <param name="valueDelegate">Delegate that calculates funciton value.
        /// If null then the function does not have value defined.</param>
        public RealFunction(DlgFunctionValue valueDelegate): 
            this(valueDelegate, null, null, null, null, null)
        { }


        /// <summary>Constructs a new function where delegates are provided for calculation of function 
        /// valu and first derivative.</summary>
        /// <param name="valueDelegate">Delegate that calculates funciton value.
        /// If null then the function does not have value defined.</param>
        /// <param name="derivativeDelegate">Delegate that calculates function derivative.
        /// If null then the function does not have derivative defined.</param>
        public RealFunction(DlgFunctionValue valueDelegate, DlgFunctionValue derivativeDelegate) :
            this(valueDelegate, derivativeDelegate, null, null, null, null)
        { }

        public RealFunction(DlgFunctionValue valueDelegate, DlgFunctionValue derivativeDelegate,
            DlgFunctionValue secondDerivativeDelegate) :
            this(valueDelegate, derivativeDelegate, secondDerivativeDelegate, null, null, null)
        { }

        /// <summary>Constructs a new function where delegates are provided for calculation of function 
        /// value, derivatives and integral.</summary>
        /// <param name="valueDelegate">Delegate that calculates funciton value.
        /// If null then the function does not have value defined.</param>
        /// <param name="derivativeDelegate">Delegate that calculates function derivative.
        /// If null then the function does not have derivative defined.</param>
        /// <param name="secondDerivativeDelegate">Delegate that calculates function second derivative.
        /// If null then the function does not have second derivative defined.</param>
        /// <param name="integralDelegate">Delegate that calculates funciton integral.
        /// If null then the function does not have integral defined.</param>
        public RealFunction(DlgFunctionValue valueDelegate, DlgFunctionValue derivativeDelegate,
            DlgFunctionValue secondDerivativeDelegate,
            DlgFunctionValue integralDelegate) :
            this(valueDelegate, derivativeDelegate, secondDerivativeDelegate, null, integralDelegate, null)
        { }

        /// <summary>Constructs a new function where delegates are provided for calculation of function 
        /// value, derivatives, integral and inverse.</summary>
        /// <param name="valueDelegate">Delegate that calculates funciton value.
        /// If null then the function does not have value defined.</param>
        /// <param name="derivativeDelegate">Delegate that calculates function derivative.
        /// If null then the function does not have derivative defined.</param>
        /// <param name="secondDerivativeDelegate">Delegate that calculates function second derivative.
        /// If null then the function does not have second derivative defined.</param>
        /// <param name="integralDelegate">Delegate that calculates funciton integral.
        /// If null then the function does not have integral defined.</param>
        /// <param name="inverseDelegate">Delegate that calculates funciton inverse.
        /// If null then the function does not have inverse defined.</param>
        public RealFunction(DlgFunctionValue valueDelegate, DlgFunctionValue derivativeDelegate,
            DlgFunctionValue secondDerivativeDelegate,
            DlgFunctionValue integralDelegate, DlgFunctionValue inverseDelegate) :
            this(valueDelegate, derivativeDelegate, secondDerivativeDelegate, null, integralDelegate, inverseDelegate)
        { }


        /// <summary>Constructs a new function where delegates are provided for calculation of function 
        /// value, derivatives, integral and inverse.</summary>
        /// <param name="valueDelegate">Delegate that calculates funciton value.
        /// If null then the function does not have value defined.</param>
        /// <param name="derivativeDelegate">Delegate that calculates function derivative.
        /// If null then the function does not have derivative defined.</param>
        /// <param name="secondDerivativeDelegate">Delegate that calculates function second derivative.
        /// If null then the function does not have second derivative defined.</param>
        /// <param name="higherDerivativeDelegate">Delegate that calculates function higher derivatives.
        /// If null then the function does not have higher derivatives defined.</param>
        /// <param name="integralDelegate">Delegate that calculates funciton integral.
        /// If null then the function does not have integral defined.</param>
        /// <param name="inverseDelegate">Delegate that calculates funciton inverse.
        /// If null then the function does not have inverse defined.</param>
        public RealFunction(DlgFunctionValue valueDelegate, DlgFunctionValue derivativeDelegate,
            DlgFunctionValue secondDerivativeDelegate, DlgFunctionHigherDerivative higherDerivativeDelegate,
            DlgFunctionValue integralDelegate, DlgFunctionValue inverseDelegate)
        {
            this.ValueDlg = valueDelegate;
            if (valueDelegate != null)
                _valueDefined = true;
            else
                _valueDefined = false;

            this.DerivativeDlg = derivativeDelegate;
            if (derivativeDelegate != null)
                _derivativeDefined = true;
            else
                _derivativeDefined = false;

            this.SecondDerivativeDlg = secondDerivativeDelegate;
            if (secondDerivativeDelegate != null)
                _secondDerivativeDefined = true;
            else
                _secondDerivativeDefined = false;

            this.HigherDerivativeDlg = higherDerivativeDelegate;
            //if (higherDerivativeDelegate != null)
            //    _higherDerivativeDefined = true;
            //else
            //    _higherDerivativeDefined = false;

            this.IntegralDlg = integralDelegate;
            if (integralDelegate != null)
                _integralDefined = true;
            else
                _integralDefined = false;

            this.InverseDlg = inverseDelegate;
            if (inverseDelegate != null)
                _inverseDefined = true;
            else
                _inverseDefined = false;
        }


        #endregion Construction


        #region Data

        // Delegates for different tasks:


        private RealFunction
            funcDerivative = null,
            funcIntegral = null,
            funcInverse = null;

        protected internal DlgFunctionTransformation
            DerivativeFunctionDlg = null,
            InverseFunctionDlg = null,
            IntegralFunctionDlg = null;

        public RealFunction CloneFunction()
        {
            RealFunction ret = new RealFunction();
            ret.DerivativeFunctionDlg = DerivativeFunctionDlg;
            return ret;
        }

        public object Clone()
        {
            return CloneFunction();
        }

        /// <summary>Returns a function that represents a derivative of the current function.</summary>
        public override RealFunctionBase DerivativeFunction
        {
            get
            {
                if (funcDerivative == null)
                {
                    if (DerivativeFunctionDlg != null)
                        funcDerivative = DerivativeFunctionDlg(this);
                }
                return funcDerivative;
            }
        }

        /// <summary>Returns a function that represents inverse of the current function.</summary>
        public override RealFunctionBase InverseFunction
        {
            get
            {
                if (funcInverse == null)
                {
                    if (InverseFunctionDlg != null)
                        funcInverse = InverseFunctionDlg(this);
                }
                return funcInverse;
            }
        }

        /// <summary>Returns a function that represents definite integral of the current function from 0 to function argument.</summary>
        public override RealFunctionBase IntegralFunction
        {
            get
            {
                if (funcIntegral == null)
                {
                    if (IntegralFunctionDlg != null)
                        funcIntegral = IntegralFunctionDlg(this);
                }
                return funcIntegral;
            }
        }



        /// <summary>Returns function value.</summary>
        protected internal DlgFunctionValue ValueDlg = null;

        /// <summary>Returns function derivative.</summary>
        protected internal DlgFunctionValue DerivativeDlg = null;

        /// <summary>Returns function derivative.</summary>
        protected internal DlgFunctionValue SecondDerivativeDlg = null;

        /// <summary>Returns function arbitrary order derivative.</summary>
        protected internal DlgFunctionHigherDerivative HigherDerivativeDlg = null;

        /// <summary>Returns function definite integral from 0 to function argument.</summary>
        protected internal DlgFunctionValue IntegralDlg = null;

        /// <summary>Returns function definite integral from 0 to function argument.</summary>
        protected internal DlgFunctionValue InverseDlg = null;



        /// <summary>Updates internal data dependencies. This function must be called whenever data on which
        /// other data is dependent changes.</summary>
        private void UpdateInternalData()
        {
            if (_shiftX != 0.0 || _scaleX != 1.0)
                _transfX = true;
            else
                _transfX = false;
            if (_shiftY != 0.0 || _scaleY!= 1.0)
                _transfY = true;
            else
                _transfY = false;
            funcDerivative = funcIntegral = funcInverse = null;
        }

        public double TransformedArgument(double t)
        { return ((t - _shiftX) / _scaleX); }

        public double InverseTransformedArgument(double x)
        { return (_scaleX * x + _shiftX); }

        private bool 
            _transfX = false,  // whether or not function domain is transformed
            _transfY = false;  // whether or not function value is transformed


        /// <summary>Whether or not transformation (stretch/shift) is applied to the reference function, either in x or to y.
        /// Setter sets value both for transformation in X and Y direction.</summary>
        protected bool DoTransform
        {
            get { return _transfX || _transfY; }
            private set { 
                _transfX = value;
                _transfY = value;
            }
        }

        /// <summary>Whether or not reference function is stretched/shifted in x direction.</summary>
        protected bool TransformX
        { 
            get { return _transfX; }
            private set { _transfX = value; }
        }

        /// <summary>Whether or not reference function is stretched/shifted in x direction.</summary>
        protected bool TransformY
        {
            get { return _transfY; }
            private set { _transfY = value; }
        }




        
        private double _scaleX = 1.0, _shiftX = 0.0, _scaleY = 1.0, _shiftY = 0.0;

        /// <summary>Gets or sets the scaling factor for independent variable.</summary>
        public double ScaleX
        { get { return _scaleX; } set { _scaleX = value; UpdateInternalData(); } }

        /// <summary>Gets or sets the shift of independent variable.</summary>
        public double ShiftX
        { get { return _shiftX; } set { _shiftX = value; UpdateInternalData(); } }

        /// <summary>Gets or sets the scaling factor for function value.</summary>
        public double ScaleY
        { get { return _scaleY; } set { _scaleY = value; UpdateInternalData(); } }

        /// <summary>Gets or sets the shift for function value.</summary>
        public double ShiftY
        { get { return _shiftY; } set { _shiftY = value; UpdateInternalData(); } }

        /// <summary>Sets parameters of the affine transformation parameters for both co-ordinates.</summary>
        /// <param name="kx">Scaling factor for independent variable.</param>
        /// <param name="sx">Shift for independent variable.</param>
        /// <param name="ky">Scaling factor for function value.</param>
        /// <param name="sy">Shift for function value.</param>
        /// <returns></returns>
        public virtual void SetTransformationParameters(double kx, double sx, double ky, double sy)
        {
            _scaleX = kx; _shiftX = sx; _scaleY = ky; _shiftY = sy;
            UpdateInternalData();
        }

        /// <summary>Sets parameters of the affine transformation parameters for independent variable.</summary>
        /// <param name="kx">Scaling factor for independent variable.</param>
        /// <param name="sx">Shift for independent variable.</param>
        public virtual void SetXTransformationParameters(double kx, double sx)
        {
            _scaleX = kx; _shiftX = sx;
            UpdateInternalData();
        }


        #endregion  // Data

        #region Evaluation

        #region EvaluationReference

        // Reference evaluation functions - non-scaled and non-shifted (such as Math.Sin);
        // They use corresponding delegates for evaluation, and must be overriden in order to
        // avoid calculation through a delegate (speed up!).



        /// <summary>Returns the value of reference (untransformed) function.</summary>
        protected virtual double RefValue(double x)
        {
            if (ValueDlg != null)
                return ValueDlg(x);
            throw new NotSupportedException("Function " + Name + ": value is not defined.");
        }


        /// <summary>Returns the first derivative of reference (untransformed) function.</summary>
        protected virtual double RefDerivative(double x)
        {
            if (DerivativeDlg != null)
                return DerivativeDlg(x);
            throw new NotSupportedException("Function " + Name + ": derivative is not defined.");
        }


        /// <summary>Returns the derivative of the given order of reference (untransformed) function.</summary>
        protected virtual double RefDerivative(double x, int order)
        {
            if (order < 1)
                throw new ArgumentException("Derivative order should be greater than 0.");
            if (HigherDerivativeDlg != null)
                return HigherDerivativeDlg(x, order);
            throw new NotSupportedException("Function " + Name + ": arbitrary derivative is not defined.");
        }


        /// <summary>Returns the second derivative of the given order of reference (untransformed) function.</summary>
        protected virtual double RefSecondDerivative(double x)
        {
            if (SecondDerivativeDlg != null)
                return SecondDerivativeDlg(x);
            throw new NotSupportedException("Function " + Name + ": second derivative is not defined.");
        }


        /// <summary>Returns definite integral of reference (untransformed) function from 0 to the function argument.</summary>
        protected virtual double RefIntegral(double x)
        {
            if (IntegralDlg != null)
                return IntegralDlg(x);
            throw new NotSupportedException("Function " + Name + ": integral is not defined.");
        }


        /// <summary>Returns inverse of the reference (untransformed) function.</summary>
        protected virtual double RefInverse(double y)
        {
            if (InverseDlg != null)
                return InverseDlg(y);
            throw new NotSupportedException("Function " + Name + ": inverse function is not defined.");
        }


        #endregion EvaluatonReference



        /// <summary>Returns the value of this function at the specified parameter.</summary>
        public override double Value(double x)
        {
            if (DoTransform)
                return _shiftY + _scaleY * RefValue((x - _shiftX) / _scaleX);
            else
                return RefValue(x);
        }

        protected bool _valueDefined = false;

        /// <summary>Tells whether value of the function is defined by implementation.
        /// Getter returns true if internal flag is set OR appropriate delegate is defined.
        /// Setter sets the internal flag (i.e. delegate must also be set to null if getter should return false).</summary>
        public override bool ValueDefined 
        { 
            get { return (_valueDefined || ValueDlg != null); }
            protected internal set { _valueDefined = value; }
        }


        /// <summary>Returns the first derivative of this function at the specified parameter.</summary>
        public override double Derivative(double x)
        {
            if (DoTransform)
                return _scaleY * RefDerivative((x - _shiftX) / _scaleX) / _scaleX;
            else
                return RefDerivative(x);
        }

        protected bool _derivativeDefined = false;

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically).
        /// Getter returns true if internal flag is set OR appropriate delegate is defined.
        /// Setter sets the internal flag (i.e. delegate must also be set to null if getter should return false).</summary>
        public override bool DerivativeDefined 
        { 
            get { return (_derivativeDefined || DerivativeDlg != null) ; }
            protected internal set { _derivativeDefined = value; }
        }


        /// <summary>Returns the derivative of the given order of this function at the specified parameter.</summary>
        public override double Derivative(double x, int order) 
        {
            if (order < 1)
                throw new ArgumentException("Derivative order should be greater than 0.");
            if (DoTransform)
                return _scaleY * RefDerivative((x - _shiftX) / _scaleX, order) / Math.Pow(_scaleX,order);
            else
                return RefDerivative(x, order);
        }

        protected int _highestDerivativeDefined = 0;

        /// <summary>Tells whether the derivative of the given order is defined for this function (by implementation, not mathematically).
        /// Returns true if either the internal variable indicates true or the appropriate delegate is non-null.</summary>
        public override bool HigherDerivativeDefined(int order) 
        {
            return (_highestDerivativeDefined >= order || _highestDerivativeDefined < 0 
                    || HigherDerivativeDlg!=null); 
        }

        /// <summary>Sets the internal variable that specifies which is the highest order derivative devined
        /// (-1 for unlimited).</summary>
        /// <param name="order">Highest order for which derivative is defined. -1 means that all derivatives are 
        /// defined.</param>
        protected internal virtual void setHighestDerivativeDefined(int order)
        {
            _highestDerivativeDefined = order;
        }

        


        /// <summary>Returns the second derivative of the given order of this function at the specified arameter.</summary>
        public override double SecondDerivative(double x)
        {
            if (DoTransform)
                return _scaleY * RefSecondDerivative((x - _shiftX) / _scaleX) / (_scaleX*_scaleX);
            else
                return RefSecondDerivative(x);
        }

        protected bool _secondDerivativeDefined = false;

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically).
        /// Getter returns true if internal flag is set OR appropriate delegate is defined.
        /// Setter sets the internal flag (i.e. delegate must also be set to null if getter should return false).</summary>
        public override bool SecondDerivativeDefined 
        {
            get { return (_secondDerivativeDefined || SecondDerivativeDlg != null); }
            protected internal set { _secondDerivativeDefined = value; }
        }


        /// <summary>Returns definite integral of the current function from 0 to the function argument.</summary>
        public override double Integral(double x)
        {
            if (DoTransform)
                return _shiftY * x + _scaleX * _scaleY * (RefIntegral((x - _shiftX) / _scaleX) 
                        - RefIntegral(-_shiftX / _scaleX));
            else
                return RefIntegral(x);
        }

        protected bool _integralDefined = false;

        /// <summary>Indicates whether integral is defined for this function (w.r. implementation).
        /// Getter returns true if internal flag is set OR appropriate delegate is defined.
        /// Setter sets the internal flag (i.e. delegate must also be set to null if getter should return false).</summary>
        public override bool IntegralDefined 
        { 
            get { return (_integralDefined || IntegralDlg!=null); }
            protected internal set { _integralDefined = true; }
        }


        /// <summary>Returns value of the inverse of the current function at the specified value of dependent variable.</summary>
        public override double Inverse(double y)
        {
            if (DoTransform)
                return _scaleX * RefInverse((y-_shiftY)/_scaleY) + _shiftX;
            else
                return RefInverse(y);
        }

        protected bool _inverseDefined = false;

        /// <summary>Indicates whether inverse is defined for this function (w.r. implementation).
        /// Getter returns true if internal flag is set OR appropriate delegate is defined.
        /// Setter sets the internal flag (i.e. delegate must also be set to null if getter should return false).</summary>
        public override bool InverseDefined 
        { 
            get { return (_inverseDefined || InverseDlg != null); }
            protected internal set { _inverseDefined = true; }
        }


        #endregion // Evaluation

        #region PredefinedFunctions

        // For predefined functions, look at the class Func!

        #endregion PredefinedFunctions


        #region Testing


        /// <summary>Compares calculation times of three ways of evaluations of exponential function:
        /// directly by Math.Exp(), through a delegate initialized by this function, and through a 
        /// Function object initialized by this function.
        /// The number of repetitions is pre-defined.</summary>
        public static void TestSpeed()
        {
            long numeval = (long) 1.0e8;
            TestSpeed(numeval);
        }

        /// <summary>Compares calculation times of three ways of evaluations of exponential function:
        /// directly by Math.Exp(), through a delegate initialized by this function, and through a 
        /// Function object initialized by this function.</summary>
        /// <param name="numiterations">Number of repeated evaluations of function.</param>
        public static void TestSpeed(long numiterations)
        {

            DlgFunctionValue fdlg = new DlgFunctionValue(Math.Exp);
            RealFunction f = Func.GetExp();
            StopWatch1 t = new StopWatch1();
            int i, irep;
            long numit;
            double x=1.0, value,t0,t1,t2,t3;

            //Warming up:


            // True calculation:

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Test of efficiency of evaluation of functions represented by Function objects:");
            Console.WriteLine("Comparison of direct evaluation, evaluation through a delegate, and evaluatin through Function object.");

            for (irep = 0; irep < 2; ++irep)
            {
                Console.WriteLine();
                if (irep == 0)
                {
                    numit = numiterations / 10;
                    Console.WriteLine("Performing a warming-up test, reduced number of iterations:");
                }
                else
                {
                    Console.WriteLine("Performing the test:");
                    numit = numiterations;
                }


                Console.WriteLine("Running just an empty loop...");
                t.Reset();
                t.Start();
                for (i = 0; i < numit; ++i)
                {
                    ;
                }
                t.Stop();
                if (irep > 0)
                {
                    Console.WriteLine("Finished. Evaluation time report:");
                    Console.WriteLine(t.ToString());
                }
                t0 = t.CpuTime;

                Console.WriteLine("Evaluating function Math.Exp()...");
                t.Reset();
                t.Start();
                for (i = 0; i < numit; ++i)
                {
                    value = Math.Exp(x);
                }
                t.Stop();
                if (irep > 0)
                {
                    Console.WriteLine("Finished. Evaluation time report:");
                    Console.WriteLine(t.ToString());
                }
                t1 = t.CpuTime;

                Console.WriteLine("Evaluating a function delegate initialized with the same function...");
                t.Reset();
                t.Start();
                for (i = 0; i < numit; ++i)
                {
                    value = fdlg(x);
                }
                t.Stop();
                if (irep > 0)
                {
                    Console.WriteLine("Finished. Evaluation time report:");
                    Console.WriteLine(t.ToString());
                }
                t2 = t.CpuTime;

                Console.WriteLine("Evaluating Function initialized with the same function...");
                t.Reset();
                t.Start();
                for (i = 0; i < numit; ++i)
                {
                    value = f.Value(x);
                }
                t.Stop();
                if (irep > 0)
                {
                    Console.WriteLine("Finished. Evaluation time report:");
                    Console.WriteLine(t.ToString());
                }
                t3 = t.CpuTime;

                Console.WriteLine();
                Console.WriteLine("Results:");
                Console.WriteLine("Calculation time relative to function evaluation, together with a loop:");
                Console.WriteLine("Evaluation through a delegate:               {0}", t2 / t1);
                Console.WriteLine("Evaluation through a Function object object: {0}", t3 / t1);
                Console.WriteLine("Calculation time relative to function evaluation, together without a loop:");
                Console.WriteLine("Evaluation through a delegate:               {0}", (t2 - t0) / (t1 - t0));
                Console.WriteLine("Evaluation through a Function object object: {0}", (t3 - t0) / (t1 - t0));
                Console.WriteLine();

            }


            Console.WriteLine("");
        }


        #endregion Testing


    }  // class Class Function


    /// <summary>Polynomial real functions of one variable.</summary>
    /// $A Igor xx;
    class FunctionPolynomial : RealFunction
    {


        #region Initialization

        FunctionPolynomial(double[] coefficients)
        {
            _coefficients = coefficients;
            Init();
        }

        private void Init()
        {
            ValueDlg = ReferenceValue;
            DerivativeDlg = ReferencDerivative;
            HigherDerivativeDlg = ReferenceHigherDerivative;
            SecondDerivativeDlg = ReferenceSecondDerivative;
            IntegralDlg = ReferenceIntegral;
            InverseDlg = ReferenceInverse;
        }

        #endregion // Initialization

        #region Data

        private double[] _coefficients = null;

        /// <summary>Returns order of the polynomial.</summary>
        public int Order
        { get { if (_coefficients == null) return -1; else return _coefficients.Length - 1; } }

        #endregion Data


        private double ReferenceValue(double x)
        {
            double ret = 0;
            double pow = 1;
            int order = Order;
            for (int i = 0; i <= order; ++i)
            {
                ret += pow * _coefficients[i];
                pow *= x;
            }
            return ret;
        }

        private double ReferencDerivative(double x)
        {
            double ret = 0;
            double pow = 1;
            int order = Order;
            for (int i = 1; i <= order; ++i)
            {
                ret += pow * _coefficients[i] * (double) i;
                pow *= x;
            }
            return ret;
        }

        double DerivativeMonomial(double x, int power, int order)
        {
            double ret = 0;
            if (order > power)
                return 0;
            else
            {
                ret = 1;
                while (order > 0)
                {
                    ret *= power * Math.Pow(x, power - 1);
                    --order;
                }
            }
            return ret;
        }

        private double ReferenceHigherDerivative(double x, int order)
        {
            double ret = 0;
            int polorder=Order;
            for (int i = 0; i <= polorder; ++i)
                ret += DerivativeMonomial(x, i, order);
            return ret;
        }

        private double ReferenceSecondDerivative(double x)
        {
            double ret = 0;
            double pow = 1;
            int order = Order;
            for (int i = 2; i <= order; ++i)
            {
                ret += pow * _coefficients[i] * (double)(i*(i-1));
                pow *= x;
            }
            return ret;
        }

        private double ReferenceIntegral(double x)
        {
            double ret = 0;
            double pow = x;
            int order = Order;
            for (int i = 0; i <= order; ++i)
            {
                ret += pow * _coefficients[i] * (double)((i+1));
                pow *= x;
            }
            return ret;
        }

        private double ReferenceInverse(double y)
        {
            if (Order == 0)
                throw new Exception("Inverse is not defined for polynomials of order 0.");
            if (Order == 1)
                return (y - _coefficients[0]) / _coefficients[1];
            throw new Exception("Inverse is not defined for polynomials of order" +  Order.ToString());
        }


        public override bool ValueDefined
        { get { return true; } }

        public override bool DerivativeDefined
        { get { return true; } }

        public override bool HigherDerivativeDefined(int order)
        { return true; }

        public override bool SecondDerivativeDefined
        { get { return true; } }

        public override bool IntegralDefined
        { get { return true; } }

        public override bool InverseDefined
        { get { return false; } }



    }  // class FunctionPolynomial


    /// <summary>Composition of real functions.</summary>
    /// $A Igor xx;
    public class ComposedFunction : RealFunction
    {

        // TODO: implement this!

        double[] _parameters = null;

        public double[] Parameters
        {
            get { return _parameters; }
        }

        RealFunction[] _functions = null;

        RealFunction[] Functions
        {
            get { return _functions; }
        }
    }  // class ComposedFunction






}
