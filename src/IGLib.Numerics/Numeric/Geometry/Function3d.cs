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
    public interface IFunc3d
    {

        /// <summary>Whether calculation of function value is defined.</summary>
        bool ValueDefined { get; }

        /// <summary>Whether calculation of function ngradient is defined.</summary>
        bool GradientDefined { get; }

        /// <summary>Whether calculation of function Hessian is defined.</summary>
        bool HessianDefined { get; }

        /// <summary>Calculates and returns value of the current 3D scalar function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="z">Third parameter.</param>
        double Value(double x, double y, double z);

        /// <summary>Calculates and returns value of the current 3D scalar function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see cref="vec3"/>) struct.</param>
        /// <returns>Function value.</returns>
        double Value(vec3 parameters);

        /// <summary>Calculates gradient of the current 3D scalar function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="z">Third parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        /// <param name="gradz">Third component of the returned gradient.</param>
        void Gradient(double x, double y, double z, out double gradx, out double grady, out double gradz);

        /// <summary>Calculates and returns gradient of the current 3D scalar function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec3"/> struct)</param>
        /// <returnreturns>Gradient of the current 3D scalar function (in form of the <see cref="vec3"/> struct)</returnreturns>
        vec3 Gradient(vec3 parameters);

        /// <summary>Calculates Hessian of the current 3D scalar function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="z">Third parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dzz">Component 3-3 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        /// <param name="dxz">Component 1-3 of the calculated Hessian.</param>
        /// <param name="dyz">Component 2-3 of the calculated Hessian.</param>
        void Hessian(double x, double y, double z, out double dxx, out double dyy, out double dzz,
            out double dxy, out double dxz, out double dyz);

        /// <summary>Calculates and returns Hessian of the current 3D scalar function and returns it
        /// (in the form of a <see cref="mat3"/> struct).</summary>
        mat3 Hessian(vec3 parameters);


    }


    /// <summary>Base class for scalar functions of 3 variables (implementations of the <see cref="IFunc3d"/> interface).
    /// that do not implement calculation of the Hessian (second derivatives).
    /// <para>Beside the <see cref="IFunc3d"/> interface, this class also implements the <see cref="IScalarFunction"/> interface.
    /// This does  not affect efficiency but adds the functionality for using objects as general (untransformed) scalar functions.</para></summary>
    /// $A Igor Apr09;
    public abstract class Func3dBaseNoHessian : Func3dBase, IFunc3d, IScalarFunctionUntransformed
    {

        protected Func3dBaseNoHessian()
            : base()
        { ValueDefined = true; GradientDefined = true; HessianDefined = false; }

        /// <summary>Function for calculating function Hessian, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Hessian(double x, double y, double z, out double dxx, out double dyy, out double dzz, out double dxy, out double dxz, out double dyz)
        {
            throw new NotImplementedException("Evaluation of Hessian is not defined.");
        }

    }  // abstract class Func3dBaseNoHessian


    /// <summary>Base class for scalar functions of 3 variables (implementations of the <see cref="IFunc3d"/> interface).
    /// that do not implement calculation of function Hessian (second derivatives) or gradient.
    /// <para>Beside the <see cref="IFunc3d"/> interface, this class also implements the <see cref="IScalarFunction"/> interface.
    /// This does  not affect efficiency but adds the functionality for using objects as general (untransformed) scalar functions.</para></summary>
    /// $A Igor Apr09;
    public abstract class Func3dBaseNoGradient : Func3dBase, IFunc3d, IScalarFunctionUntransformed
    {

        protected Func3dBaseNoGradient()
            : base()
        { GradientDefined = false; HessianDefined = false; }

        /// <summary>Function for calculating function gradient, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Gradient(double x, double y, double z, out double gradx, out double grady, out double gradz)
        {
            throw new NotImplementedException("Evaluation of Gradient is not defined.");
        }

        /// <summary>Function for calculating function Hessian, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Hessian(double x, double y, double z, out double dxx, out double dyy, out double dzz, out double dxy, out double dxz, out double dyz)
        {
            throw new NotImplementedException("Evaluation of Hessian is not defined.");
        }

    }  // abstract class Func3dBaseNoGradient



    /// <summary>Base class for scalar functions of 3 variables (base for implementation of <see cref="IFunc3d"/> interface).
    /// <para>Beside the <see cref="IFunc3d"/> interface, this class also implements the <see cref="IScalarFunction"/> interface.
    /// This does  not affect efficiency but adds the functionality for using objects as general (untransformed) scalar functions.</para></summary>
    /// $A Igor Apr09;
    public abstract class Func3dBase : ScalarFunctionUntransformedBase, IFunc3d, IScalarFunctionUntransformed
    {

        protected Func3dBase() { }

        #region Func3dBase


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
        public override bool HessianDefined
        {
            get { return _hessianDefined; }
            protected set { _hessianDefined = value; }
        }

        #endregion Flags

        /// <summary>Calculates and returns value of the current 3D scalar function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="z">Third parameter.</param>
        public abstract double Value(double x, double y, double z);

        /// <summary>Calculates and returns value of the current 3D scalar function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see cref="vec3"/> struct.</param>
        /// <returns>Function value.</returns>
        public virtual double Value(vec3 parameters)
        {
            return Value(parameters.x, parameters.y, parameters.z);
        }

        /// <summary>Calculates gradient of the current 3D scalar function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="z">Third parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        /// <param name="gradz">Third component of the returned gradient.</param>
        public abstract void Gradient(double x, double y, double z, out double gradx, out double grady, out double gradz);

        /// <summary>Calculates and returns gradient of the current 3D scalar function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec3"/> struct)</param>
        /// <returnreturns>Gradient of the current 3D scalar function (in form of the <see cref="vec3"/> struct)</returnreturns>
        public virtual vec3 Gradient(vec3 parameters)
        {
            double x = parameters.x, y = parameters.y, z = parameters.z, dx, dy, dz;
            Gradient(x, y, z, out dx, out dy, out dz);
            return new vec3(dx, dy, dz);
        }

        /// <summary>Calculates Hessian of the current 3D scalar function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="z">Third parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dzz">Component 3-3 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        /// <param name="dxz">Component 1-3 of the calculated Hessian.</param>
        /// <param name="dyz">Component 2-3 of the calculated Hessian.</param>
        public abstract void Hessian(double x, double y, double z, out double dxx, out double dyy, out double dzz,
            out double dxy, out double dxz, out double dyz);

        /// <summary>Calculates and returns Hessian of the current 3D scalar function and returns it
        /// (in the form of a <see cref="mat3"/> struct).</summary>
        public virtual mat3 Hessian(vec3 parameters)
        {
            double x = parameters.x, y = parameters.y, z = parameters.z, dxx, dyy, dzz, dxy, dxz, dyz;
            Hessian(x, y, z, out dxx, out dyy, out dzz, out dxy, out dxz, out dyz);
            return new mat3(
                dxx, dxy, dxz,
                dxy, dyy, dyz,
                dxz, dyz, dzz
                );
        }

        #endregion Func3dBase


        #region IScalarFunction


        /// <summary>Gets number of parameters.</summary>
        public int NumParam
        { get { return 3; } }

        /// <summary>Returns the value of the current function at the specified parameters.</summary>
        /// <param name="parameters">Vector of parameters for which value is evaluated. Its dimension 
        /// must be equal to 3.</param>
        public override double Value(IVector parameters)
        {
            if (parameters.Length != NumParam)
                throw new ArgumentException("Wrong dimension of parameter vector, " + parameters.Length + " instead of " + NumParam + ".");
            double x = parameters[0], y = parameters[1], z = parameters[2];
            return Value(x, y, z);
        }

        /// <summary>Calculates first order derivatives (gradient) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first order derivatives (the gradient) are stored.</param>
        public override void GradientPlain(IVector parameters, IVector gradient)
        {
            if (parameters.Length != NumParam)
                throw new ArgumentException("Wrong dimension of parameter vector, " + parameters.Length + " instead of " + NumParam + ".");
            double x = parameters[0], y = parameters[1], z = parameters[2], dx, dy, dz;
            Gradient(x, y, z, out dx, out dy, out dz);
            gradient[1] = dx;
            gradient[2] = dy;
            gradient[3] = dz;
        }


        /// <summary>Calculates the second derivative (Hessian matrix) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian matrix) are stored.</param>
        public override void HessianPlain(IVector parameters, IMatrix hessian)
        {
            if (parameters.Length != NumParam)
                throw new ArgumentException("Wrong dimension of parameter vector, " + parameters.Length + " instead of " + NumParam + ".");
            double x = parameters[0], y = parameters[1], z = parameters[2], dxx, dyy, dzz, dxy, dxz, dyz;
            Hessian(x, y, z, out dxx, out dyy, out dzz, out dxy, out dxz, out dyz);
            hessian[0, 0] = dxx;
            hessian[0, 1] = dxy;
            hessian[0, 2] = dxz;
            hessian[1, 0] = dxy;
            hessian[1, 1] = dyy;
            hessian[1, 2] = dyz;
            hessian[2, 0] = dxz;
            hessian[2, 1] = dyz;
            hessian[2, 2] = dzz;
        }

        #endregion IScalarFunction


        #region Static

        /// <summary>Demonstrates use of a 3D function by printing to the console 5 values and gradients of the 
        /// function on the line connecting the two points (0, 0, 0) and (1, 1, 1).</summary>
        /// <param name="f">Function whose values and gradients are evaluated and printed.</param>
        protected static void TestFunction(IFunc3d f)
        {
            double
                xmin = 0, xmax = 1,
                ymin = 0, ymax = 1,
                zmin = 0, zmax = 1;
            int numPoints = 5;
            TestFunction(f, xmin, xmax, ymin, ymax, zmin, zmax,numPoints);
        }


        /// <summary>Demonstrates use of a 3D function by printing to the console some values and gradients of the 
        /// function on the line connecting the two specified points.</summary>
        /// <param name="f">Function whose values and gradients are evaluated and printed.</param>
        /// <param name="xmin">Min. x.</param>
        /// <param name="xmax">Max. x.</param>
        /// <param name="ymin">Min. y.</param>
        /// <param name="ymax">Max. y.</param>
        /// <param name="zmin">Min. z.</param>
        /// <param name="zmax">Max. z.</param>
        /// <param name="numPoints">Number of points in which function values and gradients are printed.</param>
         protected static void TestFunction(IFunc3d f, 
            double xmin, double xmax,
            double ymin, double ymax,
            double zmin, double zmax,
            int numPoints)
        {
            Console.WriteLine("Values of the function at some points:");
            for (int i = 0; i < numPoints; ++i)
            {
                double x = xmin + (double)i * (xmax - xmin) / ((double) numPoints - 1.0);
                double y = ymin + (double)i * (ymax - ymin) / ((double)numPoints - 1.0);
                double z = zmin + (double)i * (zmax - zmin) / ((double)numPoints - 1.0);
                if (!f.ValueDefined)
                    Console.WriteLine("Function value is not defined.");
                else
                {
                    double value = f.Value(x, y, z);
                    Console.Write("x = " + x + ", y = " + y + ", z = " + z + "; f(x,y,z) = " + value);
                    double dx, dy, dz;
                    f.Gradient(x, y, z, out dx, out dy, out dz);
                    if (f.GradientDefined)
                        Console.Write(", grad f(x,z,y) = {"  + dx + ", " + dy + ", " + dz + "}");
                    Console.WriteLine();
                }
            }
        } // TestFunction(IFunc3d)

        /// <summary>Example of use of a 3D function.</summary>
        public static void Example()
        {
            Console.WriteLine();
            Console.WriteLine("Example use of 3D function f(x, y, z) = x*y*z.");
            IFunc3d func = new Func3dExamples.Func3dXYZ();
            TestFunction(func,
                0, 1,  // minx, maxx
                0, 1,  // miny, maxy
                0, 1,  // minz, maxz
                5  // numPoints
                );
            Console.WriteLine();
        }

        #endregion Static


    }  // class Func3dScalarBase

    
    /// <summary>Base class for scalar functions of 3 variables (base for implementation of <see cref="IFunc3d"/> interface).
    /// <para>Derive from this class when your basic implementations of evaluation methods are in vector form (parameters as <see cref="vec3"/> struct).</para>
    /// <para>Beside the <see cref="IFunc3d"/> interface, this class also implements the <see cref="IScalarFunction"/> interface.
    /// This does  not affect efficiency but adds the functionality for using objects as general (untransformed) scalar functions.</para></summary>
    /// $A Igor Aug09;
    public abstract class Func3dVectorFormBase : Func3dBase, IFunc3d, IScalarFunctionUntransformed
    {

        /// <summary>Constructor.</summary>
        protected Func3dVectorFormBase() : base() { }

        #region ToOverride


        /// <summary>Calculates and returns value of the current 3D scalar function.
        /// <para>This method must be overridden in derives classes.</para></summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see cref="vec3"/>) struct.</param>
        /// <returns>Function value.</returns>
        public override double Value(vec3 parameters)
        {
            throw new NotImplementedException("Calculation of function value is not implemented.");
        }


        /// <summary>Calculates and returns gradient of the current 3D scalar function.
        /// <para>This method must be overridden in derives classes where calculation of function gradient is implemented.</para></summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec3"/> struct)</param>
        /// <returnreturns>Gradient of the current 3D scalar function (in form of the <see cref="vec3"/> struct)</returnreturns>
        public override vec3 Gradient(vec3 parameters)
        {
            throw new NotImplementedException("Calculation of function gradient is not implemented.");
        }

        /// <summary>Calculates and returns Hessian of the current 3D scalar function and returns it
        /// (in the form of a <see cref="mat3"/> struct).
        /// <para>This method must be overridden in derives classes where calculation of function Hessian is implemented.</para></summary>
        public override mat3 Hessian(vec3 parameters)
        {
            throw new NotImplementedException("Calculation of function Hessian is not implemented.");
        }

        #endregion ToOverride

        #region Dependent

        /// <summary>Calculates and returns value of the current 3D scalar function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="z">Third parameter.</param>
        public sealed override double Value(double x, double y, double z)
        {
            return Value(new vec3(x, y, z));
        }

        /// <summary>Calculates gradient of the current 3D scalar function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="z">Third parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        /// <param name="gradz">Third component of the returned gradient.</param>
        public sealed override void Gradient(double x, double y, double z, out double gradx, out double grady, out double gradz)
        {
            vec3 grad = Gradient(new vec3(x, y, z));
            gradx = grad.x;
            grady = grad.y;
            gradz = grad.z;
        }

        /// <summary>Calculates Hessian of the current 3D scalar function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="z">Third parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dzz">Component 3-3 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        /// <param name="dxz">Component 1-3 of the calculated Hessian.</param>
        /// <param name="dyz">Component 2-3 of the calculated Hessian.</param>
        public sealed override void Hessian(double x, double y, double z, out double dxx, out double dyy, out double dzz,
            out double dxy, out double dxz, out double dyz)
        {
            mat3 hess = Hessian(new vec3(x, y, z));
            dxx = hess.xx;
            dyy = hess.yy;
            dzz = hess.zz;
            dxy = hess.xy;
            dxz = hess.xz;
            dyz = hess.yz;
        }

        #endregion Dependent

    } // abstract class Func3dVectorFormBase


    /// <summary>Linear (Affine) function of 3 variables. 
    /// <para>Function is evaluated according to fl(x) = b^T*x + c
    /// where x is vector of parameters, b is the vector of linear coefficients
    /// (gradient at x=0) and c is the scalar term (function value at x=0).</para></summary>
    /// $A Igor Aug09;
    public class Func3dLinear : Func3dVectorFormBase, IFunc3d, IScalarFunctionUntransformed
    {
        
        #region Construction 
                private Func3dLinear() : base()
        { ValueDefined = true; GradientDefined = true; HessianDefined = true; }

        /// <summary>Creation of a linear 3D scalar function.</summary>
        /// <param name="gradient0">Vector of linear coefficients - gradient of the linear function.</param>
        /// <param name="scalarTerm">Constant term.</param>
        public Func3dLinear(vec3 gradient0, double scalarTerm) : this()
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

        private vec3 _b;

        /// <summary>Vector of linear coefficients (equal to gradient of the function).</summary>
        public vec3 Gradient0
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

        
        /// <summary>Calculates and returns value of the current 3D linear function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see cref="vec3"/>) struct.</param>
        /// <returns>Function value.</returns>
        public override double Value(vec3 parameters)
        {
            return Gradient0 * parameters + ScalarTerm;
        }


        /// <summary>Calculates and returns gradient of the current 3D linear function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec3"/> struct).</param>
        /// <returnreturns>Gradient of the current 3D scalar function (in form of the <see cref="vec3"/> struct)</returnreturns>
        public override vec3 Gradient(vec3 parameters)
        {
            return Gradient0;
        }

        /// <summary>Calculates and returns Hessian of the current 3D linear function (identical to zero matrix) and returns it
        /// (in the form of a <see cref="mat3"/> struct).</summary>
        public override mat3 Hessian(vec3 parameters)
        { return new mat3(0.0); }

        #endregion Evaluation

    } // class Func3dLinear


    /// <summary>Quadratic function of 3 variables. 
    /// <para>Function is evaluated according to q(x) = (1/2)*x^T*G*x + b^T*x + c
    /// where x is vector of parameters, G is constant Hessian matrix, b is the vector of linear coefficients
    /// (gradient at x=0) and c is the scalar term (function value at x=0).</para></summary>
    /// $A Igor Aug09;
    public class Func3dQuadratic : Func3dVectorFormBase, IFunc3d, IScalarFunctionUntransformed
    {

        #region Construction 
                private Func3dQuadratic()
            : base()
        { ValueDefined = true; GradientDefined = true; HessianDefined = true; }

        /// <summary>Creation of a quadratic 3D scalar function.
        /// <para>WARNING:</para>
        /// <para>Matrix argument is interpreted as Hessian, i.e. twice the matrix of quadratic coefficients.</para></summary>
        /// <param name="hessian">Matrix of second derivatives (Hessian).</param>
        /// <param name="gradient0">Vector of linear coefficients - gradient of quadratic function at x = 0.</param>
        /// <param name="scalarTerm">Constant term.</param>
        public Func3dQuadratic(mat3 hessian, vec3 gradient0, double scalarTerm) : this()
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

            private mat3 _G;

            /// <summary>Twice the matrix of quadratic coefficients (Hessian matrix of second derivatives).</summary>
            public mat3 HessianMatrix
            {
                get { return _G; }
                protected set { _G = value; }
            }


            private vec3 _b;

            /// <summary>Vector of linear coefficients (equal to gradient of the function at x=0).</summary>
            public vec3 Gradient0
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

        
        /// <summary>Calculates and returns value of the current 3D quadratic function.</summary>
            /// <param name="parameters">Vector of function parameters (in form of the <see cref="vec3"/>) struct.</param>
        /// <returns>Function value.</returns>
        public override double Value(vec3 parameters)
        {
            return 0.5 * (parameters * (HessianMatrix * parameters)) + Gradient0 * parameters + ScalarTerm;
        }


        /// <summary>Calculates and returns gradient of the current 3D quadratic function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec3"/> struct)</param>
        /// <returnreturns>Gradient of the current 3D scalar function (in form of the <see cref="vec3"/> struct)</returnreturns>
        public override vec3 Gradient(vec3 parameters)
        {
            return HessianMatrix * parameters + Gradient0;
        }

        /// <summary>Calculates and returns Hessian of the current 3D quadratic function and returns it
        /// (in the form of a <see cref="mat3"/> struct).</summary>
        public override mat3 Hessian(vec3 parameters)
        { return HessianMatrix; }

        #endregion Evaluation

    } // class Func3dQuadratic


    /// <summary>Example classes of type <see cref="Func3dBase"/>.</summary>
    /// $A Igor Aug09 Oct09;
    public static class Func3dExamples
    {
        
        /// <summary>Function f(x, y, z) = x * y * z.
        /// <para>Value and gradient are defined.</para></summary>
        public class Func3dXYZ : Func3dBaseNoHessian, IFunc3d, IScalarFunctionUntransformed
        {

            /// <summary>Calculates and returns value of the current 3D scalar function.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            /// <param name="z">Third parameter.</param>
            public override double Value(double x, double y, double z)
            {
                return x*y*z;
            }

            /// <summary>Calculates gradient of the current 3D scalar function and returns its components
            /// through the specified output variables.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            /// <param name="z">Third parameter.</param>
            /// <param name="gradx">First component of the returned gradient.</param>
            /// <param name="grady">Second component of the returned gradient.</param>
            /// <param name="gradz">Third component of the returned gradient.</param>
            public override void Gradient(double x, double y, double z, out double gradx, out double grady, out double gradz)
            {
                gradx = y * z; grady = x * z; gradz = x * y;
            }

        }

    } // static class Func3dExamples



}


