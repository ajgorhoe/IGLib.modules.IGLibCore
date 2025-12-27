// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;


namespace IG.Num
{


    /// <summary>Represents scalar functions of 2 variables.</summary>
    /// $A Igor Apr09;
    public interface IFunc2d
    {

        /// <summary>Whether calculation of function value is defined.</summary>
        bool ValueDefined { get; }

        /// <summary>Whether calculation of function ngradient is defined.</summary>
        bool GradientDefined { get; }

        /// <summary>Whether calculation of function Hessian is defined.</summary>
        bool HessianDefined { get; }

        /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        double Value(double x, double y);
        
        /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see>Value</see>) struct.</param>
        /// <returns>Function value.</returns>
        double Value(vec2 parameters);

        /// <summary>Calculates gradient of the current 2D scalar function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        void Gradient(double x, double y, out double gradx, out double grady);
        
        /// <summary>Calculates and returns gradient of the current 2D scalar function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        vec2 Gradient(vec2 parameters);

        /// <summary>Calculates Hessian of the current 2D scalar function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        void Hessian(double x, double y, out double dxx, out double dyy, out double dxy);

        /// <summary>Calculates and returns Hessian of the current 2D scalar function and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        mat2 Hessian(vec2 parameters);

    }


    /// <summary>Base class for scalar functions of 2 variables (implementations of the <see cref="IFunc2d"/> interface).
    /// that do not implement calculation of the Hessian (second derivatives).
    /// <para>Beside the <see>IFunc2d</see> interface, this class also implements the <see>IScalarFunction</see> interface.
    /// This does  not affect efficiency but adds the functionality for using objects as general (untransformed) scalar functions.</para></summary>
    /// $A Igor Apr09;
    public abstract class Func2dBaseNoHessian : Func2dBase, IFunc2d, IScalarFunctionUntransformed
    {

        public Func2dBaseNoHessian(): base()
        { HessianDefined = false; GradientDefined = true; ValueDefined = true; }

        /// <summary>Function for calculating function Hessian, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Hessian(double x, double y, out double dxx, out double dyy, out double dxy)
        {
            throw new NotImplementedException("Evaluation of Hessian is not defined.");
        }

    }  // abstract class Func2dBaseNoHessian


    /// <summary>Base class for scalar functions of 2 variables (implementations of the <see cref="IFunc2d"/> interface).
    /// that do not implement calculation of function Hessian (second derivatives) or gradient.
    /// <para>Beside the <see>IFunc2d</see> interface, this class also implements the <see>IScalarFunction</see> interface.
    /// This does  not affect efficiency but adds the functionality for using objects as general (untransformed) scalar functions.</para></summary>
    /// $A Igor Apr09;
    public abstract class Func2dBaseNoGradient : Func2dBase, IFunc2d, IScalarFunctionUntransformed
    {

        public Func2dBaseNoGradient() : base()
        { HessianDefined = false; GradientDefined = false; }

        /// <summary>Function for calculating function gradient, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Gradient(double x, double y, out double gradx, out double grady)
        {
            throw new NotImplementedException("Evaluation of Gradient is not defined.");
        }

        /// <summary>Function for calculating function Hessian, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Hessian(double x, double y, out double dxx, out double dyy, out double dxy)
        {
            throw new NotImplementedException("Evaluation of Hessian is not defined.");
        }

    }  // abstract class Func2dBaseNoGradient



    /// <summary>Base class for scalar functions of 2 variables (base for implementation of <see cref="IFunc2d"/> interface).
    /// <para>Beside the <see>IFunc2d</see> interface, this class also implements the <see>IScalarFunction</see> interface.
    /// This does  not affect efficiency but adds the functionality for using objects as general (untransformed) scalar functions.</para></summary>
    /// $A Igor Apr09;
    public abstract class Func2dBase : ScalarFunctionUntransformedBase, IFunc2d, IScalarFunctionUntransformed
    {

        public Func2dBase() { }

        #region Func2dBase


        #region Flags

        private bool _valueDefined = true;

        /// <summary>Whether calculation of function value is defined.
        /// Defaulet is true.</summary>
        public override bool ValueDefined 
        {
            get { return _valueDefined; }
            protected set { _valueDefined = value; }
        }

        private bool _gradientDefined = false;

        /// <summary>Whether calculation of functio ngradient is defined.
        /// Default is false.</summary>
        public override bool GradientDefined
        {
            get { return _gradientDefined; }
            protected set { _gradientDefined = value; }
        }
        
        private bool _hessianDefined = false;

        /// <summary>Whether calculation of function Hessian is defined.
        /// Default is false.</summary>
        public override bool HessianDefined {
            get { return _hessianDefined; }
            protected set { _hessianDefined = value; } 
        }

        #endregion Flags

        /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        public abstract double Value(double x, double y);

        /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see>Value</see>) struct.</param>
        /// <returns>Function value.</returns>
        public virtual double Value(vec2 parameters)
        {
            return Value(parameters.x, parameters.y);
        }

        /// <summary>Calculates gradient of the current 2D scalar function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        public abstract void Gradient(double x, double y, out double gradx, out double grady);

        /// <summary>Calculates and returns gradient of the current 2D scalar function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        public virtual vec2 Gradient(vec2 parameters)
        {
            double x = parameters.x, y = parameters.y, dx, dy;
            Gradient(x, y, out dx, out dy);
            return new vec2(dx, dy);
        }

        /// <summary>Calculates Hessian of the current 2D scalar function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        public abstract void Hessian(double x, double y, out double dxx, out double dyy, 
            out double dxy);

        /// <summary>Calculates and returns Hessian of the current 2D scalar function and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        public virtual mat2 Hessian(vec2 parameters)
        {
            double x = parameters.x, y = parameters.y, dxx, dyy, dxy;
            Hessian(x, y, out dxx, out dyy, out dxy);
            return new mat2(
                dxx, dxy,
                dxy, dyy
                );
        }

        #endregion Func2dBase

        #region IScalarFunction

        
        /// <summary>Gets number of parameters.</summary>
        public int NumParam
        { get { return 2; } }

        /// <summary>Returns the value of the current function at the specified parameters.</summary>
        /// <param name="parameters">Vector of parameters for which value is evaluated. Its dimension 
        /// must be equal to 2.</param>
        public override double Value(IVector parameters)
        {
            if (parameters.Length != NumParam)
                throw new ArgumentException("Wrong dimension of parameter vector, " + parameters.Length + " instead of " + NumParam + ".");
            double x = parameters[0], y = parameters[1];
            return Value(x, y);
        }

        /// <summary>Calculates first order derivatives (gradient) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first order derivatives (the gradient) are stored.</param>
        public override void GradientPlain(IVector parameters, IVector gradient)
        {
            if (parameters.Length!=NumParam)
                throw new ArgumentException("Wrong dimension of parameter vector, " + parameters.Length + " instead of " + NumParam + ".");
            double x = parameters[0], y = parameters[1], z = parameters[2], dx, dy;
            Gradient(x, y, out dx, out dy);
            gradient[1] = dx;
            gradient[2] = dy;
        }


        /// <summary>Calculates the second derivative (Hessian matrix) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian matrix) are stored.</param>
        public override void HessianPlain(IVector parameters, IMatrix hessian)
        {
            if (parameters.Length != NumParam)
                throw new ArgumentException("Wrong dimension of parameter vector, " + parameters.Length + " instead of " + NumParam + ".");
            double x = parameters[0], y = parameters[1], dxx, dyy, dxy;
            Hessian(x, y, out dxx, out dyy, out dxy);
            hessian[0, 0] = dxx;
            hessian[0, 1] = dxy;
            hessian[1, 0] = dxy;
            hessian[1, 1] = dyy;
        }

        #endregion IScalarFunction

        #region Static

        /// <summary>Demonstrates use of a 2D function by printing to the console 5 values and gradients of the 
        /// function on the line connecting the two points (0, 0, 0) and (1, 1, 1).</summary>
        /// <param name="f">Function whose values and gradients are evaluated and printed.</param>
        protected static void TestFunction(IFunc2d f)
        {
            double
                xmin = 0, xmax = 1,
                ymin = 0, ymax = 1;
            int numPoints = 5;
            TestFunction(f, xmin, xmax, ymin, ymax, numPoints);
        }


        /// <summary>Demonstrates use of a 2D function by printing to the console some values and gradients of the 
        /// function on the line connecting the two specified points.</summary>
        /// <param name="f">Function whose values and gradients are evaluated and printed.</param>
        /// <param name="xmin">Min. x.</param>
        /// <param name="xmax">Max. x.</param>
        /// <param name="ymin">Min. y.</param>
        /// <param name="ymax">Max. y.</param>
        /// <param name="numPoints">Number of points in which function values and gradients are printed.</param>
        protected static void TestFunction(IFunc2d f,
           double xmin, double xmax,
           double ymin, double ymax,
           int numPoints)
        {
            Console.WriteLine("Values of the function at some points:");
            for (int i = 0; i < numPoints; ++i)
            {
                double x = xmin + (double)i * (xmax - xmin) / ((double)numPoints - 1.0);
                double y = ymin + (double)i * (ymax - ymin) / ((double)numPoints - 1.0);
                if (!f.ValueDefined)
                    Console.WriteLine("Function value is not defined.");
                else
                {
                    double value = f.Value(x, y);
                    Console.Write("x = " + x + ", y = " + y + "; f(x,y) = " + value);
                    double dx, dy;
                    f.Gradient(x, y, out dx, out dy);
                    if (f.GradientDefined)
                        Console.Write(", grad f(x,z) = {" + dx + ", " + dy + "}");
                    Console.WriteLine();
                }
            }
        } // TestFunction(IFunc3d)

        /// <summary>Example of use of a 2D function.</summary>
        public static void Example()
        {
            Console.WriteLine();
            Console.WriteLine("Example use of 2D function f(x, y) = x*y.");
            IFunc2d func = new Func2dExamples.Func2dXY();
            TestFunction(func,
                0, 1,  // minx, maxx
                0, 1,  // miny, maxy
                5  // numPoints
                );
            Console.WriteLine();
        }

        #endregion Static

    }  // class Func2dBase




    /// <summary>Base class for scalar functions of 2 variables (base for implementation of <see cref="IFunc2d"/> interface).
    /// <para>Derive from this class when your basic implementations of evaluation methods are in vector form (parameters as <see cref="vec2"/> struct).</para>
    /// <para>Beside the <see cref="IFunc2d"/> interface, this class also implements the <see cref="IScalarFunction"/> interface.
    /// This does  not affect efficiency but adds the functionality for using objects as general (untransformed) scalar functions.</para></summary>
    /// $A Igor Aug09;
    public abstract class Func2dVectorFormBase : Func2dBase, IFunc2d, IScalarFunctionUntransformed
    {

        /// <summary>Constructor.</summary>
        public Func2dVectorFormBase() : base() { }

        #region ToOverride


        /// <summary>Calculates and returns value of the current 2D scalar function.
        /// <para>This method must be overridden in derives classes.</para></summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see cref="vec2"/>) struct.</param>
        /// <returns>Function value.</returns>
        public override double Value(vec2 parameters)
        {
            throw new NotImplementedException("Calculation of function value is not implemented.");
        }


        /// <summary>Calculates and returns gradient of the current 2D scalar function.
        /// <para>This method must be overridden in derives classes where calculation of function gradient is implemented.</para></summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        public override vec2 Gradient(vec2 parameters)
        {
            throw new NotImplementedException("Calculation of function gradient is not implemented.");
        }

        /// <summary>Calculates and returns Hessian of the current 2D scalar function and returns it
        /// (in the form of a <see cref="mat2"/> struct).
        /// <para>This method must be overridden in derives classes where calculation of function Hessian is implemented.</para></summary>
        public override mat2 Hessian(vec2 parameters)
        {
            throw new NotImplementedException("Calculation of function Hessian is not implemented.");
        }

        #endregion ToOverride

        #region Dependent

        /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        public sealed override double Value(double x, double y)
        {
            return Value(new vec2(x, y));
        }

        /// <summary>Calculates gradient of the current 2D scalar function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        public sealed override void Gradient(double x, double y, out double gradx, out double grady)
        {
            vec2 grad = Gradient(new vec2(x, y));
            gradx = grad.x;
            grady = grad.y;
        }

        /// <summary>Calculates Hessian of the current 3D scalar function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        public sealed override void Hessian(double x, double y, out double dxx, out double dyy, 
            out double dxy)
        {
            mat2 hess = Hessian(new vec2(x, y));
            dxx = hess.xx;
            dyy = hess.yy;
            dxy = hess.xy;
        }

        #endregion Dependent

    } // abstract class Func2dVectorFormBase


    /// <summary>Linear (Affine) function of 2 variables. 
    /// <para>Function is evaluated according to fl(x) = b^T*x + c
    /// where x is vector of parameters, b is the vector of linear coefficients
    /// (gradient at x=0) and c is the scalar term (function value at x=0).</para></summary>
    /// $A Igor Aug09;
    public class Func2dLinear : Func2dVectorFormBase, IFunc2d, IScalarFunctionUntransformed
    {

        #region Construction
        private Func2dLinear()
            : base()
        { ValueDefined = true; GradientDefined = true; HessianDefined = true; }

        /// <summary>Creation of a linear 2D scalar function.</summary>
        /// <param name="gradient0">Vector of linear coefficients - gradient of the linear function.</param>
        /// <param name="scalarTerm">Constant term.</param>
        public Func2dLinear(vec2 gradient0, double scalarTerm)
            : this()
        {
            this.Gradient0 = gradient0;
            this.ScalarTerm = scalarTerm;
        }

        #endregion Construction

        #region Data

        /// <summary>Returns the number of scalar constants that specify the current function.</summary>
        public static int GetNumConstants()
        {
            return 4;
        }

        private vec2 _b;

        /// <summary>Vector of linear coefficients (equal to gradient of the function).</summary>
        public vec2 Gradient0
        {
            get { return _b; }
            protected set { _b = value; }
        }

        private double _c;

        /// <summary>Scalar additive constant.</summary>
        public double ScalarTerm
        {
            get { return _c; }
            protected set { _c = value; }
        }

        #endregion Data

        #region Evaluation


        /// <summary>Calculates and returns value of the current 2D linear function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see cref="vec2"/>) struct.</param>
        /// <returns>Function value.</returns>
        public override double Value(vec2 parameters)
        {
            return Gradient0 * parameters + ScalarTerm;
        }


        /// <summary>Calculates and returns gradient of the current 2D linear function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct).</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        public override vec2 Gradient(vec2 parameters)
        {
            return Gradient0;
        }

        /// <summary>Calculates and returns Hessian of the current 2D linear function (identical to zero matrix) and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        public override mat2 Hessian(vec2 parameters)
        { return new mat2(0.0); }

        #endregion Evaluation

    }  // class Func2dLinear


    /// <summary>Quadratic function of 2 variables. 
    /// <para>Function is evaluated according to q(x) = (1/2)*x^T*G*x + b^T*x + c
    /// where x is vector of parameters, G is constant Hessian matrix, b is the vector of linear coefficients
    /// (gradient at x=0) and c is the scalar term (function value at x=0).</para></summary>
    /// $A Igor Aug09;
    public class Func2dQuadratic : Func2dVectorFormBase, IFunc2d, IScalarFunctionUntransformed
    {

        #region Construction
        private Func2dQuadratic()
            : base()
        { ValueDefined = true; GradientDefined = true; HessianDefined = true; }

        /// <summary>Creation of a quadratic 2D scalar function.
        /// <para>WARNING:</para>
        /// <para>Matrix argument is interpreted as Hessian, i.e. twice the matrix of quadratic coefficients.</para></summary>
        /// <param name="hessian">Matrix of second derivatives (Hessian).</param>
        /// <param name="gradient0">Vector of linear coefficients - gradient of quadratic function at x = 0.</param>
        /// <param name="scalarTerm">Constant term.</param>
        public Func2dQuadratic(mat2 hessian, vec2 gradient0, double scalarTerm)
            : this()
        {
            this.HessianMatrix = hessian;
            this.Gradient0 = gradient0;
            this.ScalarTerm = scalarTerm;
        }

        #endregion Construction

        #region Data

        /// <summary>Returns the number of scalar constants that specify the current function.</summary>
        public static int GetNumConstants()
        {
            return 10;
        }

        private mat2 _G;

        /// <summary>Twice the matrix of quadratic coefficients (Hessian matrix of second derivatives).</summary>
        public mat2 HessianMatrix
        {
            get { return _G; }
            protected set { _G = value; }
        }


        private vec2 _b;

        /// <summary>Vector of linear coefficients (equal to gradient of the function at x=0).</summary>
        public vec2 Gradient0
        {
            get { return _b; }
            protected set { _b = value; }
        }

        private double _c;

        /// <summary>Scalar additive constant.</summary>
        public double ScalarTerm
        {
            get { return _c; }
            protected set { _c = value; }
        }

        #endregion Data

        #region Evaluation


        /// <summary>Calculates and returns value of the current 2D quadratic function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see cref="vec2"/>) struct.</param>
        /// <returns>Function value.</returns>
        public override double Value(vec2 parameters)
        {
            return 0.5 * (parameters * (HessianMatrix * parameters)) + Gradient0 * parameters + ScalarTerm;
        }


        /// <summary>Calculates and returns gradient of the current 2D quadratic function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        public override vec2 Gradient(vec2 parameters)
        {
            return HessianMatrix * parameters + Gradient0;
        }

        /// <summary>Calculates and returns Hessian of the current 2D quadratic function and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        public override mat2 Hessian(vec2 parameters)
        { return HessianMatrix; }

        #endregion Evaluation

    } // class Func2dQuadratic


    /// <summary>Example classes of type <see cref="Func2dBase"/>.</summary>
    /// $A Igor Aug09 Oct09;
    public static class Func2dExamples
    {
        
        /// <summary>Function f(x, y) = x * y.
        /// <para>Value and gradient are defined.</para></summary>
        public class Func2dXY : Func2dBaseNoHessian, IFunc2d, IScalarFunctionUntransformed
        {

            /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            public override double Value(double x, double y)
            {
                return x * y;
            }

            /// <summary>Calculates gradient of the current 2D scalar function and returns its components
            /// through the specified output variables.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            /// <param name="gradx">First component of the returned gradient.</param>
            /// <param name="grady">Second component of the returned gradient.</param>
            public override void Gradient(double x, double y, out double gradx, out double grady)
            {
                gradx = y; grady = x;
            }

        }

    } // static class Func2dExamples



    /// <summary>Base class for scalar functions of 2 variables.
    /// <para>This willl be base class for most of other classes that implement the <see cref="IFunc2d"/> 
    /// interface. It provides a set of useful static methods.</para></summary>
    /// <remarks>TODO:
    /// <para>Consider whether this class is really needed. Maybe <see cref="Func2dBase"/> is enough.
    /// That class contains additional methods that implement the <see cref="IScalarFunctionUntransformed"/>
    /// interface, and maybe this does not justify implementation of another class just to be simpler.</para></remarks>
    /// $A Igor Apr09;
    public abstract class Func2dBasePlain_ToConsider : IFunc2d
    {

        #region Construction

        /// <summary>Constructor.</summary>
        public Func2dBasePlain_ToConsider() { }

        #endregion Construction

        #region Flags

        private bool _valueDefined = true;

        /// <summary>Whether calculation of function value is defined.
        /// Defaulet is true.</summary>
        public virtual bool ValueDefined
        {
            get { return _valueDefined; }
            protected set { _valueDefined = value; }
        }

        private bool _gradientDefined = false;

        /// <summary>Whether calculation of functio ngradient is defined.
        /// Default is false.</summary>
        public virtual bool GradientDefined
        {
            get { return _gradientDefined; }
            protected set { _gradientDefined = value; }
        }

        private bool _hessianDefined = false;

        /// <summary>Whether calculation of function Hessian is defined.
        /// Default is false.</summary>
        public virtual bool HessianDefined
        {
            get { return _hessianDefined; }
            protected set { _hessianDefined = value; }
        }

        #endregion Flags

        /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        public abstract double Value(double x, double y);

        /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see>Value</see>) struct.</param>
        /// <returns>Function value.</returns>
        public virtual double Value(vec2 parameters)
        {
            return Value(parameters.x, parameters.y);
        }

        /// <summary>Calculates gradient of the current 2D scalar function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        public abstract void Gradient(double x, double y, out double gradx, out double grady);

        /// <summary>Calculates and returns gradient of the current 2D scalar function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        public virtual vec2 Gradient(vec2 parameters)
        {
            double x = parameters.x, y = parameters.y, dx, dy;
            Gradient(x, y, out dx, out dy);
            return new vec2(dx, dy);
        }

        /// <summary>Calculates Hessian of the current 2D scalar function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        public abstract void Hessian(double x, double y, out double dxx, out double dyy,
            out double dxy);

        /// <summary>Calculates and returns Hessian of the current 2D scalar function and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        public virtual mat2 Hessian(vec2 parameters)
        {
            double x = parameters.x, y = parameters.y, dxx, dyy, dxy;
            Hessian(x, y, out dxx, out dyy, out dxy);
            return new mat2(
                dxx, dxy,
                dxy, dyy
                );
        }


    }  // abstract class Func2dBasePlain_ToConsider




}