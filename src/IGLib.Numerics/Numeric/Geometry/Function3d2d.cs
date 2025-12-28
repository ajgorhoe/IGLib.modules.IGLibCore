
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;


namespace IG.Num
{


    /// <summary>Represents 3D vector functions of 2 variables.</summary>
    /// $A Igor Oct09;
    public interface IFunc3d2d
    {
        
        /// <summary>Whether calculation of function value is defined.</summary>
        bool ValueDefined { get; }

        /// <summary>Whether calculation of function ngradient is defined.</summary>
        bool GradientDefined { get; }

        /// <summary>Whether calculation of function Hessian is defined.</summary>
        bool HessianDefined { get; }

        /// <summary>Calculates and returns value of the first component of the current function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        double Value1(double x, double y);

        /// <summary>Calculates and returns value of the second component of the current function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        double Value2(double x, double y);

        /// <summary>Calculates and returns value of the third component of the current function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        double Value3(double x, double y);

        /// <summary>Calculates and returns value of the first component of the current function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see>Value</see>) struct.</param>
        /// <returns>Function value.</returns>
        double Value1(vec2 parameters);

        /// <summary>Calculates and returns value of the second component of the current function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see>Value</see>) struct.</param>
        /// <returns>Function value.</returns>
        double Value2(vec2 parameters);

        /// <summary>Calculates and returns value of the third component of the current function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see>Value</see>) struct.</param>
        /// <returns>Function value.</returns>
        double Value3(vec2 parameters);

        /// <summary>Calculates gradient of the first component of the current function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        void Gradient1(double x, double y, out double gradx, out double grady);

        /// <summary>Calculates gradient of the second component of the current function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        void Gradient2(double x, double y, out double gradx, out double grady);

        /// <summary>Calculates gradient of the third component of the current function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        void Gradient3(double x, double y, out double gradx, out double grady);

        /// <summary>Calculates and returns gradient of the first component of the current function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        vec2 Gradient1(vec2 parameters);

        /// <summary>Calculates and returns gradient of the second component of the current function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        vec2 Gradient2(vec2 parameters);

        /// <summary>Calculates and returns gradient of the third component of the current function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        vec2 Gradient3(vec2 parameters);

        /// <summary>Calculates Hessian of the first component of the current function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        void Hessian1(double x, double y, out double dxx, out double dyy,
            out double dxy);

        /// <summary>Calculates Hessian of the second component of the current function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        void Hessian2(double x, double y, out double dxx, out double dyy,
            out double dxy);

        /// <summary>Calculates Hessian of the third component of the current function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        void Hessian3(double x, double y, out double dxx, out double dyy,
            out double dxy);

        /// <summary>Calculates and returns Hessian of the first component of the current function and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        mat2 Hessian1(vec2 parameters);

        /// <summary>Calculates and returns Hessian of the second component of the current function and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        mat2 Hessian2(vec2 parameters);

        /// <summary>Calculates and returns Hessian of the third component of the current function and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        mat2 Hessian3(vec2 parameters);

        #region ComponentFunctions

        /// <summary>Returns the scalar function that represents the first component of the current 3D vector function of 2 variables.</summary>
        Func2dBase Component1 { get; }

        /// <summary>Returns the scalar function that represents the second component of the current 3D vector function of 2 variables.</summary>
        Func2dBase Component2 { get; }

        /// <summary>Returns the scalar function that represents the third component of the current 3D vector function of 2 variables.</summary>
        Func2dBase Component3 { get; }

        #endregion ComponentFunctions

    }  // interface IFunc3d2d


    /// <summary>Base class for 3D vector functions of 2 variables (implementations of the <see cref="IFunc3d2d"/> interface)
    /// that do not implement calculation of the Hessian (second derivatives).</summary>
    /// $A Igor Oct09;
    public abstract class Func3d2dBaseNoHessian : Func3d2dBase, IFunc3d2d
    {

        protected Func3d2dBaseNoHessian()
            : base()
        { HessianDefined = false; GradientDefined = true; ValueDefined = true; }

        /// <summary>Function for calculating Hessian of the first component, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Hessian1(double x, double y, out double dxx, out double dyy, out double dxy)
        {
            throw new NotImplementedException("Evaluation of Hessian is not defined.");
        }

        /// <summary>Function for calculating Hessian of the second component, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Hessian2(double x, double y, out double dxx, out double dyy, out double dxy)
        {
            throw new NotImplementedException("Evaluation of Hessian is not defined.");
        }

        /// <summary>Function for calculating Hessian of the third component, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Hessian3(double x, double y, out double dxx, out double dyy, out double dxy)
        {
            throw new NotImplementedException("Evaluation of Hessian is not defined.");
        }

    }  // abstract class Func3d2dBaseNoHessian


    /// <summary>Base class for 3D vector functions of 2 variables (implementations of the <see cref="IFunc3d2d"/> interface)
    /// that do not implement calculation of function Hessian (second derivatives) or gradient.</summary>
    /// $A Igor Oct09;
    public abstract class Func3d2dBaseNoGradient : Func3d2dBase, IFunc3d2d
    {

        protected Func3d2dBaseNoGradient()
            : base()
        { HessianDefined = false; GradientDefined = false; }

        /// <summary>Function for calculating gradient of the first component of vector function, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Gradient1(double x, double y, out double gradx, out double grady)
        {
            throw new NotImplementedException("Evaluation of Gradient is not defined.");
        }

        /// <summary>Function for calculating gradient of the second component of vector function, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Gradient2(double x, double y, out double gradx, out double grady)
        {
            throw new NotImplementedException("Evaluation of Gradient is not defined.");
        }

        /// <summary>Function for calculating gradient of the third component of vector function, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Gradient3(double x, double y, out double gradx, out double grady)
        {
            throw new NotImplementedException("Evaluation of Gradient is not defined.");
        }

        /// <summary>Function for calculating Hessian of the first component, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Hessian1(double x, double y, out double dxx, out double dyy, out double dxy)
        {
            throw new NotImplementedException("Evaluation of Hessian is not defined.");
        }

        /// <summary>Function for calculating Hessian of the second component, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Hessian2(double x, double y, out double dxx, out double dyy, out double dxy)
        {
            throw new NotImplementedException("Evaluation of Hessian is not defined.");
        }

        /// <summary>Function for calculating Hessian of the third component, just throws the <see cref="NotImplementedException"/> exception.</summary>
        public override void Hessian3(double x, double y, out double dxx, out double dyy, out double dxy)
        {
            throw new NotImplementedException("Evaluation of Hessian is not defined.");
        }

    }  // abstract class Func3d2dBaseNoGradient


    /// <summary>Base class for 3D vector functions of 2 variables (base for implementation of <see cref="IFunc3d2d"/> interface).</summary>
    /// $A Igor Oct09;
    public abstract class Func3d2dBase : IFunc3d2d
    {

        protected Func3d2dBase() { }


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


        /// <summary>Calculates and returns value of the first component of the current function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        public abstract double Value1(double x, double y);

        /// <summary>Calculates and returns value of the second component of the current function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        public abstract double Value2(double x, double y);

        /// <summary>Calculates and returns value of the third component of the current function.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        public abstract double Value3(double x, double y);

        /// <summary>Calculates and returns value of the first component of the current function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see>Value</see>) struct.</param>
        /// <returns>Function value.</returns>
        public virtual double Value1(vec2 parameters)
        {
            return Value1(parameters.x, parameters.y);
        }

        /// <summary>Calculates and returns value of the second component of the current function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see>Value</see>) struct.</param>
        /// <returns>Function value.</returns>
        public virtual double Value2(vec2 parameters)
        {
            return Value2(parameters.x, parameters.y);
        }

        /// <summary>Calculates and returns value of the third component of the current function.</summary>
        /// <param name="parameters">Vector of function parameters (in form of the <see>Value</see>) struct.</param>
        /// <returns>Function value.</returns>
        public virtual double Value3(vec2 parameters)
        {
            return Value3(parameters.x, parameters.y);
        }

        /// <summary>Calculates gradient of the first component of the current function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        public abstract void Gradient1(double x, double y, out double gradx, out double grady);

        /// <summary>Calculates gradient of the second component of the current function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        public abstract void Gradient2(double x, double y, out double gradx, out double grady);

        /// <summary>Calculates gradient of the third component of the current function and returns its components
        /// through the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="gradx">First component of the returned gradient.</param>
        /// <param name="grady">Second component of the returned gradient.</param>
        public abstract void Gradient3(double x, double y, out double gradx, out double grady);

        /// <summary>Calculates and returns gradient of the first component of the current function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        public virtual vec2 Gradient1(vec2 parameters)
        {
            double x = parameters.x, y = parameters.y, dx, dy;
            Gradient1(x, y, out dx, out dy);
            return new vec2(dx, dy);
        }

        /// <summary>Calculates and returns gradient of the second component of the current function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        public virtual vec2 Gradient2(vec2 parameters)
        {
            double x = parameters.x, y = parameters.y, dx, dy;
            Gradient2(x, y, out dx, out dy);
            return new vec2(dx, dy);
        }

        /// <summary>Calculates and returns gradient of the third component of the current function.</summary>
        /// <param name="parameters">Vector of parameters (in form of the <see cref="vec2"/> struct)</param>
        /// <returnreturns>Gradient of the current 2D scalar function (in form of the <see cref="vec2"/> struct)</returnreturns>
        public virtual vec2 Gradient3(vec2 parameters)
        {
            double x = parameters.x, y = parameters.y, dx, dy;
            Gradient3(x, y, out dx, out dy);
            return new vec2(dx, dy);
        }

        /// <summary>Calculates Hessian of the first component of the current function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        public abstract void Hessian1(double x, double y, out double dxx, out double dyy,
            out double dxy);

        /// <summary>Calculates Hessian of the second component of the current function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        public abstract void Hessian2(double x, double y, out double dxx, out double dyy,
            out double dxy);

        /// <summary>Calculates Hessian of the third component of the current function and returns its component through
        /// the specified output variables.</summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
        /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
        /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
        public abstract void Hessian3(double x, double y, out double dxx, out double dyy,
            out double dxy);

        /// <summary>Calculates and returns Hessian of the first component of the current function and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        public virtual mat2 Hessian1(vec2 parameters)
        {
            double x = parameters.x, y = parameters.y, dxx, dyy, dxy;
            Hessian1(x, y, out dxx, out dyy, out dxy);
            return new mat2(
                dxx, dxy,
                dxy, dyy
                );
        }

        /// <summary>Calculates and returns Hessian of the second component of the current function and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        public virtual mat2 Hessian2(vec2 parameters)
        {
            double x = parameters.x, y = parameters.y, dxx, dyy, dxy;
            Hessian2(x, y, out dxx, out dyy, out dxy);
            return new mat2(
                dxx, dxy,
                dxy, dyy
                );
        }

        /// <summary>Calculates and returns Hessian of the third component of the current function and returns it
        /// (in the form of a <see cref="mat2"/> struct).</summary>
        public virtual mat2 Hessian3(vec2 parameters)
        {
            double x = parameters.x, y = parameters.y, dxx, dyy, dxy;
            Hessian3(x, y, out dxx, out dyy, out dxy);
            return new mat2(
                dxx, dxy,
                dxy, dyy
                );
        }


        /// <summary>Gets number of parameters.</summary>
        public int NumParam
        { get { return 2; } }

        /// <summary>Gets number of components of the current vector function.</summary>
        public int NumValues
        { get { return 3; } }

        #region ComponentFunctions

        private Func2dBase _comp1, _comp2, _comp3;

        /// <summary>Returns the scalar function that represents the first component of the current 3D vector function of 2 variables.</summary>
        public virtual Func2dBase Component1 {
            get { 
                if (_comp1==null)
                    _comp1 = new ComponentFunction1(this);
                return _comp1;
            }
        }

        /// <summary>Returns the scalar function that represents the second component of the current 3D vector function of 2 variables.</summary>
        public virtual Func2dBase Component2
        {
            get
            {
                if (_comp2 == null)
                    _comp2 = new ComponentFunction2(this);
                return _comp2;
            }
        }
        /// <summary>Returns the scalar function that represents the third component of the current 3D vector function of 2 variables.</summary>
        public virtual Func2dBase Component3
        {
            get
            {
                if (_comp3 == null)
                    _comp3 = new ComponentFunction3(this);
                return _comp3;
            }
        }
        /// <summary>Base function for component functions, provides internal variable for vector function.</summary>
        protected abstract class ComponentFunctionBase: Func2dBase
        {

            private ComponentFunctionBase() { }

            protected ComponentFunctionBase (IFunc3d2d func)
            { this.VectorFunction = func; }

            protected IFunc3d2d _vectorFunction;

            /// <summary>A 3D vector function of two variables that is the base for component 2D scalar functions.</summary>
            public IFunc3d2d VectorFunction
            {
                get { return _vectorFunction; }
                protected set {
                    if (value==null)
                        throw new ArgumentException("Vector function to be a base for component scalar functions is not specified (null reference).");
                    _vectorFunction = value;
                    this.ValueDefined = value.ValueDefined;
                    this.GradientDefined = value.GradientDefined;
                    this.HessianDefined = this.HessianDefined;
                }
            }
        }

        /// <summary>Class that represents a scalar function of 2 variables that is the first component of 
        /// the specified 3D vector function of 2 variables.</summary>
        protected class ComponentFunction1 : ComponentFunctionBase, IFunc2d, IScalarFunctionUntransformed
        {
            public ComponentFunction1(IFunc3d2d func)
                : base(func)
            {  }

            /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            public override double Value(double x, double y)
            {
                return _vectorFunction.Value1(x, y);
            }

            /// <summary>Calculates gradient of the current 2D scalar function and returns its components
            /// through the specified output variables.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            /// <param name="gradx">First component of the returned gradient.</param>
            /// <param name="grady">Second component of the returned gradient.</param>
            public override void Gradient(double x, double y, out double gradx, out double grady)
            {
                _vectorFunction.Gradient1(x, y, out gradx, out grady);
            }

            /// <summary>Calculates Hessian of the current 2D scalar function and returns its component through
            /// the specified output variables.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
            /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
            /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
            public override void Hessian(double x, double y, out double dxx, out double dyy, out double dxy)
            {
                _vectorFunction.Hessian1(x, y, out dxx, out dyy, out dxy);
            }

        }  // class Component1

        /// <summary>Class that represents a scalar function of 2 variables that is the first component of 
        /// the specified 3D vector function of 2 variables.</summary>
        protected class ComponentFunction2 : ComponentFunctionBase, IFunc2d, IScalarFunctionUntransformed
        {
            public ComponentFunction2(IFunc3d2d func)
                : base(func)
            { }

            /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            public override double Value(double x, double y)
            {
                return _vectorFunction.Value2(x, y);
            }

            /// <summary>Calculates gradient of the current 2D scalar function and returns its components
            /// through the specified output variables.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            /// <param name="gradx">First component of the returned gradient.</param>
            /// <param name="grady">Second component of the returned gradient.</param>
            public override void Gradient(double x, double y, out double gradx, out double grady)
            {
                _vectorFunction.Gradient2(x, y, out gradx, out grady);
            }

            /// <summary>Calculates Hessian of the current 2D scalar function and returns its component through
            /// the specified output variables.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
            /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
            /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
            public override void Hessian(double x, double y, out double dxx, out double dyy, out double dxy)
            {
                _vectorFunction.Hessian2(x, y, out dxx, out dyy, out dxy);
            }

        }  // class Component2

        /// <summary>Class that represents a scalar function of 2 variables that is the third component of 
        /// the specified 3D vector function of 2 variables.</summary>
        protected class ComponentFunction3 : ComponentFunctionBase, IFunc2d, IScalarFunctionUntransformed
        {
            public ComponentFunction3(IFunc3d2d func)
                : base(func)
            { }

            /// <summary>Calculates and returns value of the current 2D scalar function.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            public override double Value(double x, double y)
            {
                return _vectorFunction.Value3(x, y);
            }

            /// <summary>Calculates gradient of the current 2D scalar function and returns its components
            /// through the specified output variables.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            /// <param name="gradx">First component of the returned gradient.</param>
            /// <param name="grady">Second component of the returned gradient.</param>
            public override void Gradient(double x, double y, out double gradx, out double grady)
            {
                _vectorFunction.Gradient3(x, y, out gradx, out grady);
            }

            /// <summary>Calculates Hessian of the current 2D scalar function and returns its component through
            /// the specified output variables.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            /// <param name="dxx">Component 1-1 of the calculated Hessian.</param>
            /// <param name="dyy">Component 2-2 of the calculated Hessian.</param>
            /// <param name="dxy">Component 1-2 of the calculated Hessian.</param>
            public override void Hessian(double x, double y, out double dxx, out double dyy, out double dxy)
            {
                _vectorFunction.Hessian3(x, y, out dxx, out dyy, out dxy);
            }

        }  // class Component3

        #endregion ComponentFunctions


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

    }  // class Func2dScalarBase



    /// <summary>Contains a number of example 3D vector functions of 2 variables.
    /// <para>Many of the contained nested classes define interesting parametric surfaces.</para></summary>
    /// $A Igor Oct09;
    public class Func3d2dExamples: M 
    {

        /// <summary>Base class for easy definition of parametric surfaces intended for plotting.</summary>
        /// $A Igor Oct09;
        public abstract class ParametricSurface : Func3d2dBaseNoGradient, IFunc3d2d
        {

            /// <summary>Base class for classes that define various parametric surfaces.</summary>
            protected ParametricSurface() 
            { }

            /// <summary>Base class for classes that define various parametric surfaces.</summary>
            protected ParametricSurface(double minX, double maxX, double minY, double maxY): this()
            {
                this.MinX = minX;
                this.MaxX = maxX;
                this.MinY = minY;
                this.MaxY = maxY;
            }

            private double _minX = -1, _maxX = 1, _minY = -1, _maxY = 1;
            private int _numx = 20, _numy = 20;

            /// <summary>Minimal value of the first parameter.</summary>
            public double MinX
            { get { return _minX; } set { _minX = value; } }

            /// <summary>Maximal value of the first parameter.</summary>
            public double MaxX
            { get { return _maxX; } set { _maxX = value; } }

            /// <summary>Minimal value of the second parameter.</summary>
            public double MinY
            { get { return _minY; } set { _minY = value; } }

            /// <summary>Maximal value of the second parameter.</summary>
            public double MaxY
            { get { return _maxY; } set { _maxY = value; } }

            /// <summary>Recommended number of points along the first parameter used to plot the surface.</summary>
            public int NumX
            { get { return _numx; } set { _numx = value; } }

            /// <summary>Recommended number of points along the second parameter used to plot the surface.</summary>
            public int NumY
            { get { return _numy; } set { _numy = value; } }

            /// <summary>Sets the bounds.</summary>
            /// <param name="minX">Lower bound of the first parameter.</param>
            /// <param name="maxX">Upper bound of the first parameter.</param>
            /// <param name="minY">Lower bound of the first parameter.</param>
            /// <param name="maxY">Upper bound of the second parameter.</param>
            public void SetBounds(double minX, double maxX, double minY, double maxY)
            {
                this.MinX = minX; this.MaxX = maxX;
                this.MinY = minY; this.MaxY = maxY;
            }

            /// <summary>Sets recommended number of plot points along parameter curves.</summary>
            /// <param name="numX">Recommended number of plot points alog the first parameter.</param>
            /// <param name="numY">Recommended number of plot points alog the second parameter.</param>
            public void SetNumPoints(int numX, int numY)
            { this.NumX = numX; this.NumY = numY; }

            protected abstract double f1(double u, double v);

            protected abstract double f2(double u, double v);

            protected abstract double f3(double u, double v);

            /// <summary>Calculates and returns value of the first component of the current function.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            public override double Value1(double x, double y)
            {
                return f1(x, y);
            }

            /// <summary>Calculates and returns value of the second component of the current function.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            public override double Value2(double x, double y)
            {
                return f2(x, y);
            }

            /// <summary>Calculates and returns value of the third component of the current function.</summary>
            /// <param name="x">First parameter.</param>
            /// <param name="y">Second parameter.</param>
            public override double Value3(double x, double y)
            {
                return f3(x, y);
            }
        }


        #region SurfaacePlot

        /// <summary>Base class for easy definition of surfaces defined through a function of 2 variables.</summary>
        public abstract class Surface : ParametricSurface, IFunc3d2d
        {


            protected override double f1(double u, double v)
            { return u; }

            protected override double f2(double u, double v)
            { return v; }


        }

        /// <summary>Parametric surface that is a graph of the function f(x,y)=x*y.</summary>
        /// $A Igor Oct09;
        public class SaddleXY: Surface, IFunc3d2d
        {
            protected override double f3(double x, double y)
            {
                return x*y;
            }
        }

        /// <summary>Surface that is a graph of the function z/c=x^2/a^2-y^2/b^2.</summary>
        /// $A Igor Oct09;
        public class HyperbolicParaboloid: Surface, IFunc3d2d
        {

            /// <summary>Construct a definition of a unit-scaled hyperbollic paraboloid.</summary>
            public HyperbolicParaboloid() : this(1, 1, 1) { }

            /// <summary>Constructs a scaled hyperbollic paraboloid.</summary>
            /// <param name="a">Scaling factor along x direction.</param>
            /// <param name="b">Scaling factor along y direction.</param>
            /// <param name="c">Scaling factor along z direction.</param>
            public HyperbolicParaboloid(double a, double b, double c)
            { this.a = a; this.b = b; this.c = c; }

            public double a, b, c;

            protected override double f3(double x, double y)
            {
                return c*( x*x/(a*a)-y*y/(b*b) );
            }
        }

        /// <summary>Surface that is a graph of the function z/c=x^2/a^2+y^2/b^2.</summary>
        /// $A Igor Oct09;
        public class Paraboloid : Surface, IFunc3d2d
        {

            /// <summary>Constructs a rotationally symmetric paraboloid.</summary>
            public Paraboloid() : this(1, 1, 1) { }


            /// <summary>Constructs a rotationally symmetric paraboloid.</summary>
            /// <param name="a">Measure of curvature in the x-z nad in the y-z plane.</param>
            public Paraboloid(double a) : this(a, a, 1) { }


            /// <summary>Constructs a rotationally symmetric paraboloid.</summary>
            /// <param name="a">Measure of curvature in the x-z nad in the y-z plane.</param>
            /// <param name="c">Stretching factor in z direction.</param>
            public Paraboloid(double a, double c) : this(a, a, c) { }

            /// <summary>Constructs a paraboloid.</summary>
            /// <param name="a">Measure of curvature in the x-z plane.</param>
            /// <param name="b">Measure of curvature in the y-z plane.</param>
            /// <param name="c">Stretching factor in z direction.</param>
            public Paraboloid(double a, double b, double c)
            { this.a = a; this.b = b; this.c = c; }

            public double a, b, c;

            protected override double f3(double x, double y)
            {
                return c * (x * x / (a * a) + y * y / (b * b));
            }
        }


        #endregion SurfacePlot

        #region BasicSurfaces

        /// <summary>Parametric equation of an origin-centered (ellipsoidal) cylindrical surface
        /// in form of 3D vector function of 2 variables.</summary>
        /// $A Igor Oct09;
        public class CylinderParametric : ParametricSurface, IFunc3d2d
        {

            /// <summary>Construct a rotationally symmetric parametric definition of origin-centered cylinder of radius 1 and height 1.</summary>
            public CylinderParametric()
                : this(1, 1, 1)
            { }

            /// <summary>Constructs parametric definition of rotationally symmetric origin-centered ellipsoid with the specified radius.</summary>
            /// <param name="r">Radius of the cylinder.</param>
            /// <param name="h">Height of the cylinder.</param>
            public CylinderParametric(double r, double h): this(r, r, h)
            {  }


            /// <summary>Constructs parametric definition of an origin-centered ellipsoid with the specified values of half-axes.</summary>
            /// <param name="a">Half-axis in the first coordinate direction.</param>
            /// <param name="b">Half-axis in the second coordinate direction.</param>
            /// <param name="h">Height of the cylinder.</param>
            public CylinderParametric(double a, double b, double h): base()
            {
                this.a = a; this.b = b; this.h = h;
                MinX = -pi; MaxX = pi;
                MinY = 0; MaxY = 1;
            }

            public double a, b, h;

            protected override double f1(double u, double v)
            {
                return a * cos(u);
            }

            protected override double f2(double u, double v)
            {
                return b * sin(u);
            }

            protected override double f3(double u, double v)
            {
                return h*(v-0.5);
            }

        }

        /// <summary>Parametric equation of an origin-centered ellipsoid surface
        /// in form of 3D vector function of 2 variables.</summary>
        /// $A Igor Oct09;
        public class EllipsoidParametric: ParametricSurface, IFunc3d2d
        {

            /// <summary>Construct an ellipsoid with some typical values for half-axes.</summary>
            public EllipsoidParametric(): this(2, 1, 0.5)
            { }


            /// <summary>Constructs parametric definition of an origin-centered ellipsoid by the specified values of half-axes.</summary>
            /// <param name="a">Half-axis in the first coordinate direction.</param>
            /// <param name="b">Half-axis in the second coordinate direction.</param>
            /// <param name="c">Half-axis in the third coordinate direction.</param>
            public EllipsoidParametric(double a, double b, double c)
            { 
                this.a = a; this.b = b; this.c = c;
                MinX = -pi; MaxX = pi;
                MinY = -0.5 * pi; MaxY = 0.5 * pi;
            }

            public double a, b, c;

            protected override double f1(double u, double v)
            {
                return a * cos(u) * cos(v);
            }

            protected override double f2(double u, double v)
            {
                return b * sin(u) * cos(v);
            }

            protected override double f3(double u, double v)
            {
                return c * sin(v);
            }

        }

        /// <summary>Parametric equation of an origin-centered sphere.</summary>
        /// $A Igor Oct09;
        public class SphereParametric: EllipsoidParametric, IFunc3d2d
        {

            /// <summary>Constructs parametric definition of a unit sphere
        /// in form of 3D vector function of 2 variables.</summary>
            public SphereParametric(): this(1) { }

            /// <summary>Construct a parametric definition of an origin-centered sphere with a specified radius.</summary>
            /// <param name="r"></param>
            public SphereParametric(double r): base(r, r, r)
            {  }
        }

        /// <summary>Parametric definition of an origin-centered upper two-sheeted hyperboloid 
        /// surface (x^2/a^2+y^2/b^2-z^2/c^2-1)
        /// in form of 3D vector function of 2 variables.</summary>
        /// $A Igor Oct09;
        public class HyperboloidTwosheetedUpperParametric : HyperboloidParametric, IFunc3d2d
        {

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            public HyperboloidTwosheetedUpperParametric()
                : this(1, 1, 1)
            { }

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            /// <param name="r">Radius of the smallest cross-section.</param>
            /// <param name="c">Height of the hyperboloid.</param>
            public HyperboloidTwosheetedUpperParametric(double r, double c)
                : this(r, r, c)
            { }


            /// <summary>Constructs parametric definition of an origin-centered hyperboloid 
            /// with the specified parameter-stretching factors.</summary>
            /// <param name="a">Half-axis in the first coordinate direction at the smallest cross-section.</param>
            /// <param name="b">Half-axis in the second coordinate direction at the smallest cross-section.</param>
            /// <param name="c">Height.</param>
            public HyperboloidTwosheetedUpperParametric(double a, double b, double c)
            {
                this.a = a; this.b = b; this.c = c;
                MinX = -0.5 *0; MaxX = 1;
                MinY = 0; MaxY = 2.0 * pi;
            }

            //public double a, b, c;

            protected override double f1(double u, double v)
            {
                return a * sinh(u) * cos(v);
            }

            protected override double f2(double u, double v)
            {
                return a * sinh(u) * sin(v);
            }

            protected override double f3(double u, double v)
            {
                return c * ch(u);
            }

        }  // class HyperboloidTwosheetedUpperParametric

        /// <summary>Parametric definition of an origin-centered upper two-sheeted hyperboloid 
        /// surface (x^2/a^2+y^2/b^2-z^2/c^2-1)
        /// in form of 3D vector function of 2 variables.</summary>
        /// $A Igor Oct09;
        public class HyperboloidTwosheetedLowerParametric : HyperboloidParametric, IFunc3d2d
        {

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            public HyperboloidTwosheetedLowerParametric()
                : this(1, 1, 1)
            { }

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            /// <param name="r">Radius of the smallest cross-section.</param>
            /// <param name="c">Height of the hyperboloid.</param>
            public HyperboloidTwosheetedLowerParametric(double r, double c)
                : this(r, r, c)
            { }


            /// <summary>Constructs parametric definition of an origin-centered hyperboloid 
            /// with the specified parameter-stretching factors.</summary>
            /// <param name="a">Half-axis in the first coordinate direction at the smallest cross-section.</param>
            /// <param name="b">Half-axis in the second coordinate direction at the smallest cross-section.</param>
            /// <param name="c">Height.</param>
            public HyperboloidTwosheetedLowerParametric(double a, double b, double c)
            {
                this.a = a; this.b = b; this.c = c;
                MinX = -0.5 * 0; MaxX = 1.0;
                MinY = 0; MaxY = 2.0 * pi;
            }

            //public double a, b, c;

            protected override double f1(double u, double v)
            {
                return a * sinh(u) * cos(v);
            }

            protected override double f2(double u, double v)
            {
                return a * sinh(u) * sin(v);
            }

            protected override double f3(double u, double v)
            {
                return - c * ch(u);
            }

        } // class HyperboloidTwosheetedLowerParametric


        /// <summary>Parametric definition of an origin-centered hyperboloid surface (x^2/a^2+y^2/b^2-z^2/c^2=1)
        /// in form of 3D vector function of 2 variables.</summary>
        /// $A Igor Oct09;
        public class HyperboloidParametric : ParametricSurface, IFunc3d2d
        {
            
            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            public HyperboloidParametric(): this(1, 1, 1)
            { }

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            /// <param name="r">Radius of the smallest cross-section.</param>
            /// <param name="c">Height of the hyperboloid.</param>
            public HyperboloidParametric(double r, double c)
                : this(r, r, c)
            { }


            /// <summary>Constructs parametric definition of an origin-centered hyperboloid 
            /// with the specified parameter-stretching factors.</summary>
            /// <param name="a">Half-axis in the first coordinate direction at the smallest cross-section.</param>
            /// <param name="b">Half-axis in the second coordinate direction at the smallest cross-section.</param>
            /// <param name="c">Height.</param>
            public HyperboloidParametric(double a, double b, double c)
            { 
                this.a = a; this.b = b; this.c = c;
                MinX = -0.5; MaxX = 0.5;
                MinY = 0; MaxY = 2.0 * pi;
            }

            public double a, b, c;

            protected override double f1(double u, double v)
            {
                return a * sqrt(1 + u*u) * cos(v);
            }

            protected override double f2(double u, double v)
            {
                return b * sqrt(1 + u*u) * sin(v);
            }

            protected override double f3(double u, double v)
            {
                return c * u;
            }

        }

        /// <summary>Alternative parametric definition of an origin-centered hyperboloid surface (x^2/a^2+y^2/b^2-z^2/c^2=1)
        /// in form of 3D vector function of 2 variables.</summary>
        /// $A Igor Oct09;
        public class HyperboloidParametric2 : ParametricSurface, IFunc3d2d
        {

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            public HyperboloidParametric2()
                : this(1, 1, 1)
            { }

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            /// <param name="r">Radius of the smallest cross-section.</param>
            /// <param name="c">Height of the hyperboloid.</param>
            public HyperboloidParametric2(double r, double c)
                : this(r, r, c)
            { }


            /// <summary>Constructs parametric definition of an origin-centered hyperboloid 
            /// with the specified parameter-stretching factors.</summary>
            /// <param name="a">Half-axis in the first coordinate direction at the smallest cross-section.</param>
            /// <param name="b">Half-axis in the second coordinate direction at the smallest cross-section.</param>
            /// <param name="c">Height.</param>
            public HyperboloidParametric2(double a, double b, double c)
            {
                this.a = a; this.b = b; this.c = c;
                MinX = 0; MaxX = 2.0*pi;
                MinY = -0.5; MaxY = 0.5;
            }

            public double a, b, c;

            protected override double f1(double u, double v)
            {
                return a * ch(v) * cos(u);
            }

            protected override double f2(double u, double v)
            {
                return a * ch(v) * sin(u);
            }

            protected override double f3(double u, double v)
            {
                return c * sh(v);
            }

        }

        /// <summary>Alternative parameterization of an origin-centered hyperboloid surface (x^2/a^2+y^2/b^2-z^2/c^2=1)
        /// in form of 3D vector function of 2 variables.
        /// <para>One set of parameter curves are straight lines running in clockwise direction when watched from above.</para></summary>
        /// <remarks>Parameterization is similar to <see cref="HyperboloidParametricMinus"/>, with difference in signs.</remarks>
        /// $A Igor Oct09;
        public class HyperboloidParametricPlus : HyperboloidParametric, IFunc3d2d
        {

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            public HyperboloidParametricPlus()
                : this(1, 1, 1)
            { }

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            /// <param name="r">Radius of the smallest cross-section.</param>
            /// <param name="c">Height of the hyperboloid.</param>
            public HyperboloidParametricPlus(double r, double c)
                : this(r, r, c)
            { }

            /// <summary>Constructs parametric definition of an origin-centered hyperboloid 
            /// with the specified parameter-stretching factors.</summary>
            /// <param name="a">Half-axis in the first coordinate direction at the smallest cross-section.</param>
            /// <param name="b">Half-axis in the second coordinate direction at the smallest cross-section.</param>
            /// <param name="c">Height.</param>
            public HyperboloidParametricPlus(double a, double b, double c)
            {
                this.a = a; this.b = b; this.c = c;
                MinX = 0; MaxX = 2*pi;
                MinY = -0.5; MaxY = 0.5;
            }

            protected override double f1(double u, double v)
            {
                return a * (cos(u) - v * sin(u));
            }

            protected override double f2(double u, double v)
            {
                return a * (sin(u) + v * cos(u));
            }

            protected override double f3(double u, double v)
            {
                return c * v;
            }

        }

        /// <summary>Alternative parameterization of an origin-centered hyperboloid surface (x^2/a^2+y^2/b^2-z^2/c^2=1)
        /// in form of 3D vector function of 2 variables.
        /// <para>One set of parameter curves are straight lines running in counter clockwise direction when watched from above.</para></summary>
        /// <remarks>Parameterization is similar to <see cref="HyperboloidParametricPlus"/>, with difference in signs.</remarks>
        /// $A Igor Oct09;
        public class HyperboloidParametricMinus : HyperboloidParametric, IFunc3d2d
        {

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            public HyperboloidParametricMinus()
                : this(1, 1, 1)
            { }

            /// <summary>Construct a rotationally symmetric hyperboloid.</summary>
            /// <param name="r">Radius of the smallest cross-section.</param>
            /// <param name="c">Height of the hyperboloid.</param>
            public HyperboloidParametricMinus(double r, double c)
                : this(r, r, c)
            { }

            /// <summary>Constructs parametric definition of an origin-centered hyperboloid 
            /// with the specified parameter-stretching factors.</summary>
            /// <param name="a">Half-axis in the first coordinate direction at the smallest cross-section.</param>
            /// <param name="b">Half-axis in the second coordinate direction at the smallest cross-section.</param>
            /// <param name="c">Height.</param>
            public HyperboloidParametricMinus(double a, double b, double c)
            {
                this.a = a; this.b = b; this.c = c;
                MinX = 0; MaxX = 2 * pi;
                MinY = -0.5; MaxY = 0.5;
            }


            protected override double f1(double u, double v)
            {
                return a * (cos(u) + v * sin(u));
            }

            protected override double f2(double u, double v)
            {
                return a * (sin(u) - v * cos(u));
            }

            protected override double f3(double u, double v)
            {
                return  c * v;
            }

        }

        /// <summary>Parametric equation of a toroid surface
        /// in form of 3D vector function of 2 variables.
        /// <para>Can be combined with <see cref="TorusVertical"/> to show 2 interlocked toroids.</para></summary>
        /// <remarks><para>See also http://www.vtk.org/VTK/img/ParametricSurfaces.pdf .</para></remarks>
        /// $A Igor Oct09;
        public class Torus : ParametricSurface, IFunc3d2d
        {

            /// <summary>Constructs a parametric definition of a torus with radius of centerline 1 and radius of surface circles 0.25.</summary>
            public Torus(): this(0.25, 1)
            {  }


            /// <summary>Constructs a parametric definition of a torus with the radius of centerline equal to 1 and the
            /// specified ratio between  radius of the radius of cross-section circles and radius of the centerline.</summary>
            /// <param name="radiusRatio">Ratio between the radius of cross-section circles and radius of the centerline
            /// (normally between 0 and 1).</param>
            public Torus(double radiusRatio)
                : this(1.0, radiusRatio)
            { }

            /// <summary>Constructs a parametric definition of a torus with the specified radius of centerline 
            /// and radius of surface circles.</summary>
            /// <param name="a">Radius of cross-section circles.</param>
            /// <param name="c">Radius of the centerline, should normally be greater than <paramref name="a"/>.</param>
            public Torus(double a, double c)
            { 
                this.a = a; this.c = c; 
                SetBounds(0, 2 * pi, 0, 2 * pi);
                SetNumPoints(50, 30);
            }

            public double a = 0.25;

            public double c = 1.0;

            protected override double f1(double u, double v)
            {
                return (c + a * cos(v)) * cos(u);
            }

            protected override double f2(double u, double v)
            {
                return (c + a * cos(v)) * sin(u);
            }

            protected override double f3(double u, double v)
            {
                return a * sin(v);
            }

        }



        /// <summary>Parametric equation of a horizontal toroid surface
        /// in form of 3D vector function of 2 variables.
        /// <para>Can be combined with <see cref="TorusVertical"/> to show 2 interlocked toroids.</para></summary>
        /// <remarks><para>Precise parameterization is taken  from the Mathematica notebook ParametricSurfaces.nb.</para></remarks>
        /// $A Igor Oct11;
        public class TorusHorizontal : ParametricSurface, IFunc3d2d
        {

            public TorusHorizontal()
            {
                SetBounds(0, 2*pi, 0, 2*pi);
                SetNumPoints(50,30);
            }

            protected override double f1(double u, double v)
            {
                return 4 + (3 + cos(v)) * sin(u);
            }

            protected override double f2(double u, double v)
            {
                return 4 + (3 + cos(v)) * cos(u);
            }

            protected override double f3(double u, double v)
            {
                return 4 + sin(v);
            }

        }


        /// <summary>Parametric equation of a horizontal toroid surface
        /// in form of 3D vector function of 2 variables.
        /// <para>Can be combined with <see cref="TorusHorizontal"/> to show 2 interlocked toroids.</para></summary>
        /// <remarks><para>Precise parameterization is taken  from the Mathematica notebook ParametricSurfaces.nb.</para></remarks>
        /// $A Igor Oct11;
        public class TorusVertical : TorusHorizontal, IFunc3d2d
        {

            public TorusVertical()
                : base()
            { }

            protected override double f1(double u, double v)
            {
                return 8 + (3 + cos(v)) * cos(u);
            }

            protected override double f2(double u, double v)
            {
                return 3 + sin(v);
            }

            protected override double f3(double u, double v)
            {
                return 4 + (3 + cos(v)) * sin(u);
            }

        }


        #endregion BasicSurfaces

        #region MinimalSurfaces

        /// <summary>Parametric equation of the Enneper surface,
        /// in form of 3D vector function of 2 variables.</summary>
        /// <remarks><para>See also: http://en.wikipedia.org/wiki/Enneper%27s_surface ,
        /// http://mathworld.wolfram.com/EnnepersMinimalSurface.html .</para></remarks>
        /// $A Igor Oct11;
        public class EnneperSurface : ParametricSurface, IFunc3d2d
        {

            public EnneperSurface()
                : base()
            {
                SetBounds(
                    -2, 2,
                    -2, 2
                    );
                SetNumPoints(60, 30);
            }

            protected override double f1(double u, double v)
            {
                return u * (1-u*u/3.0 + v*v/3);
            }

            protected override double f2(double u, double v)
            {
                return -v * (1 - v * v / 3.0 + u * u / 3); ;
            }

            protected override double f3(double u, double v)
            {
                return (u*u - v*v)/3;
            }

        }

        #endregion MinimalSurfaces

        #region Topology



        /// <summary>Defines the Möbius strip (a parametric surface), a surface with only one side and only one boundary component.</summary>
        /// <remarks><para>See also: http://www.vtk.org/VTK/img/ParametricSurfaces.pdf, http://en.wikipedia.org/wiki/M%C3%B6bius_strip. 
        /// Precise parameterization is taken  from the Mathematica notebook ParametricSurfaces.nb.</para></remarks>
        /// $A Igor Nov09;
        public class MobiusStrip: EllipsoidParametric, IFunc3d2d
        {

            public MobiusStrip()
            { SetBounds(0, 2 * pi, -1, 1); SetNumPoints(40, 8); }


            protected override double f1(double t, double r)
            {
                return cos(t) * (3.0 + r * cos(t/2.0));
            }

            protected override double f2(double t, double r)
            {
                return sin(t) * (3.0 + r * cos(t/2.0));
            }

            protected override double f3(double t, double r)
            {
                return r * sin(t/2.0);
            }


        }


        /// <summary>Parametric equation of the umbilic torus surface, a closed single-edged surface in 3D, 
        /// in form of 3D vector function of 2 variables.</summary>
        /// <remarks><para>See also: http://en.wikipedia.org/wiki/Umbilic_torus .</para></remarks>
        /// $A Igor Oct11;
        public class UmbilicTorus : ParametricSurface, IFunc3d2d
        {

            public UmbilicTorus()
                : base()
            {
                SetBounds(-pi, pi, -pi, pi);
                SetNumPoints(60, 30);
            }

            protected override double f1(double u, double v)
            {
                return sin(u) * (7.0 + cos((u / 3.0) - 2.0 * v) + 2.0 * cos((u / 3.0) + v));
            }

            protected override double f2(double u, double v)
            {
                return cos(u) * (7.0 + cos((u / 3.0) - 2.0 * v) + 2.0 * cos((u / 3.0) + v));
            }

            protected override double f3(double u, double v)
            {
                return sin((u / 3.0) - 2.0 * v) + 2.0 * sin((u / 3.0) + v);
            }

        }


        /// <summary>Defines the Klein's bottle (a parametric surface), a closed surface with no interior and exterior.</summary>
        /// <remarks><para>See also: http://www.vtk.org/VTK/img/ParametricSurfaces.pdf where definition is taken from.</para></remarks>
        /// $A Igor Nov09;
        public class KleinBottle : ParametricSurface, IFunc3d2d
        {

            public KleinBottle()
                : base()
            {
                SetBounds(
                    0.0 * pi, 1.0 * pi, 
                    0.0 * pi, 2.0 * pi);
                SetNumPoints(50, 50);
            }

            protected override double f1(double u, double v)
            {
                return -(2.0/15.0) * cos(u) * (
                    3.0*cos(v) + 5.0*sin(u)*cos(v)*cos(u) - 30.0*sin(u)
                    - 60.0 * sin(u) * pow(cos(u),6) + 90.0 * sin(u) * pow(cos(u), 4) );
            }

            protected override double f2(double u, double v)
            {
                return -(1.0/15.0) * sin(u) * (
                    80.0 * cos(v) * pow(cos(u),7) * sin(u) + 48.0 * cos(v) * pow(cos(u), 6)
                    - 80.0 * cos(v) * pow(cos(u), 5) * sin(u) - 48.0 * cos(v) * pow(cos(u), 4)
                    - 5.0 * cos(v) * pow(cos(u),3) * sin(u) - 3.0 * cos(v) * pow(cos(u), 2)
                    + 5.0 * sin(u) * cos(v) * cos(u) + 3.0 * cos(v) - 60.0 * sin(u)
                    );
            }

            protected override double f3(double u, double v)
            {
                return (2.0/15.0) * sin(v) * (3.0 + 5.0 * sin(u) * cos(u));
            }


        } // class KleinBottle
        
        /// <summary>Defines the Klein's bottle (a parametric surface), a closed surface with no interior and exterior.</summary>
        /// <remarks><para>See also: http://www.vtk.org/VTK/img/ParametricSurfaces.pdf . Precise parameterization is taken 
        /// from the Mathematica notebook ParametricSurfaces.nb.</para></remarks>
        /// $A Igor Oct11;
        public class KleinBottle1 : ParametricSurface, IFunc3d2d
        {

            public KleinBottle1() : base()
            {
                SetBounds(0, 2 * pi, 0, 2 * pi);
                SetNumPoints(50, 50);
            }

            public const double Defaulta = 0.5;

            public double a = Defaulta;

            protected override double f1(double u, double v)
            {
                return cos(u) * (3 + cos(u/2.0) * sin(v) - sin(u/2.0) * sin(2.0 * v));
            }

            protected override double f2(double u, double v)
            {
                return sin(u) * (3 + cos(u/2.0) * sin(v) - sin(u/2.0) * sin(2.0 * v));
            }

            protected override double f3(double u, double v)
            {
                return sin(u/2.0) * sin(v) + cos(u/2.0) * sin(2 * v);
            }
            
        } // class KleinBottle1


        /// <summary>Defines the Klein's bottle (a parametric surface), a closed surface with no interior and exterior.</summary>
        /// <remarks><para>See also: http://www.vtk.org/VTK/img/ParametricSurfaces.pdf (parameterization is taken from there).</para></remarks>
        /// $A Igor Oct11;
        public class KleinBottle2 : ParametricSurface, IFunc3d2d
        {

            public KleinBottle2() : base()
            {
                SetBounds(0, 2 * pi, 0, 2 * pi);
                SetNumPoints(50, 50);
            }

            public KleinBottle2(double a)
                : this()
            { this.a = a; }

            public const double Defaulta = 0.5;

            public double a = Defaulta;

            protected override double f1(double u, double v)
            {
                return cos(u)*(a + sin(v) * cos(0.5*u) - 0.5 * (sin(2*v) * sin(0.5 * u)));
            }

            protected override double f2(double u, double v)
            {
                return sin(u) * (a + sin(v) * cos(0.5 * u) - 0.5 * (sin(2 * v) * sin(0.5 * u)));
            }

            protected override double f3(double u, double v)
            {
                return sin(0.5*u) * sin(v) + 0.5 * (sin(2*v) * cos(0.4*u));
            }

            
        } // class KleinBottle2

        #endregion Topology

        #region InterestingSurfaces


        /// <summary>Defines a parametric definition of a snail shell - like surface (a parametric surface)
        /// in form of 3D vector function of 2 variables.
        /// <para>WARNING: Function of this class must be re-checked.</para></summary>
        /// <remarks><para>See also: http://www.vtk.org/VTK/img/ParametricSurfaces.pdf (parameterization is taken from there).</para></remarks>
        /// $A Igor Oct11;
        public class SnailConicSpiral_ToCheck : ParametricSurface, IFunc3d2d
        {

            /// <summary>Constructs parametric definition of a snail shell-like parametric surface with some
            /// default parameters.</summary>
            public SnailConicSpiral_ToCheck()
                : this(0.2, 0.1, 0, 2)
            { }


            /// <summary>Creates a parametric definition of a snail shell/like surface with the specified parameters.</summary>
            /// <param name="a">Ratio between spiral diameter and height of one turn. </param>
            /// <param name="b"></param>
            /// <param name="c">Defines radius of free space spiral turns.
            /// 0 means there is no hole in the middle.</param>
            /// <param name="n">Number of full turns of the spiral.</param>
            public SnailConicSpiral_ToCheck(double a, double b, double c, double n)
            {
                this.a = a; this.b = b; this.c = c; this.n = n;
                SetBounds(0, 2 * pi, 0, 2 * pi);
                SetNumPoints(50, 50);
            }


            public double a = 0.2, b = 0.1, c = 0, n = 2;

            protected override double f1(double u, double v)
            {
                return a * (1 - v / (2.0 * pi)) * cos(n * v) * (1.0 + cos(u)) + c * cos(n*v);
            }

            protected override double f2(double u, double v)
            {
                return a * (1 - v / (2.0 * pi)) * sin(n * v) * (1.0 + cos(u)) + c * sin(n*v);
            }

            protected override double f3(double u, double v)
            {
                return (b * v + a * (1.0 - v / (2.0 * pi)) * sin(u)) / (2.0 * pi);
            }

        } // class SnailConicSpiral


        /// <summary>Definition of parametric surface that ressembles a snail's shell
        /// in form of 3D vector function of 2 variables.</summary>
        /// <remarks><para> Precise parameterization is taken  from the Mathematica notebook ParametricSurfaces.nb.</para></remarks>
        /// $A Igor Oct09;
        public class SnailShell1 : ParametricSurface, IFunc3d2d
        {

            public SnailShell1()
            {
                MinX = 0;  MaxX = 2*pi;
                MinY = -15; MaxY = 6;
                NumX = 50; NumY = 50;
            }

            protected override double f1(double u, double v)
            {
                return pow(1.16,v) * cos(v) * (1 + cos(u));
            }

            protected override double f2(double u, double v)
            {
                return -pow(1.16, v) * sin(v) * (1 + cos(u));
            }

            protected override double f3(double u, double v)
            {
                return -2.0 * pow(1.16,v) * (1 + sin(u));
            }

        }
            
        /// <summary>Definition of parametric surface that ressembles a snail's shell (a bit stretched along the axis)
        /// in form of 3D vector function of 2 variables.</summary>
        /// <remarks><para>Precise parameterization is taken  from the Mathematica notebook ParametricSurfaces.nb.</para></remarks>
        /// $A Igor Nov09;
        public class SnailShell1Streched : ParametricSurface, IFunc3d2d
        {

            public SnailShell1Streched()
            {
                SetBounds(
                    0, 6*pi,
                    0, 2*pi
                );
                SetNumPoints(50, 50);
            }

            protected override double f1(double u, double v)
            {
                return 2.0 * (1.0 - exp(u/(6.0*pi)) ) * cos(u) * pow(cos(v/2.0),2.0);
            }

            protected override double f2(double u, double v)
            {
                return 2.0 * (-1.0 + exp(u/(6.0*pi)) ) * sin(u) * pow(cos(v/2.0),2);
            }

            protected override double f3(double u, double v)
            {
                return 1.0 - exp(u/(3.0*pi)) - sin(v) + exp(u/(6.0*pi)) * sin(v);
            }

        }

        #endregion InterestingSurfaces



    } // class Func3d2dExamples



}