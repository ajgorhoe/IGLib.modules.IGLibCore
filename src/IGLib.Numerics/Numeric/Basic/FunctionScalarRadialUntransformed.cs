
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    ///// <summary>One parametric real function.</summary>
    ///// $A Igor xx Nov10;
    //public abstract class RealFunctionOneParametric1 : RealFunction,
    //    IRealFunction
    //{

    //    public RealFunctionOneParametric1(double parameter)
    //    {
    //        this.Parameter = parameter;
    //    }

    //    double _parameter;

    //    public double Parameter{ 
    //        get { return _parameter; } 
    //        set { _parameter = value; } 
    //    }

    //}


    

    /// <summary>One parametric radial scalar function (dependent on one tunning parameter).</summary>
    /// $A Igor xx Nov10;
    public class ScalarFunctionRadialUntransformedOneParametric : ScalarFunctionRadialUntransformed
    {
        
        public ScalarFunctionRadialUntransformedOneParametric(RealFunctionOneParametric function, double parameter) : base(function)
        {
            Function = function;
            function.Parameter = parameter;
        }


        RealFunctionOneParametric _parametricfunction;


        public override IRealFunction Function
        {
            get
            {
                return _parametricfunction;
            }
            protected set
            {
                _parametricfunction = value as RealFunctionOneParametric;
                if (_parametricfunction == null)
                    throw new ArgumentException("The specified functon of one variable is not of the correct type.");
            }
        }

        public double Parameter
        { 
            get { return _parametricfunction.Parameter; }
            set { _parametricfunction.Parameter = value; } 
        }

    }





    /// <summary>Radial scalar functions.</summary>
    /// $A Igor xx Nov10;
    public class ScalarFunctionRadialUntransformed : ScalarFunctionUntransformedBase, IScalarFunctionUntransformed
    {

        #region Construction

        protected ScalarFunctionRadialUntransformed() { }

        public ScalarFunctionRadialUntransformed(IRealFunction function)
        {
            this.Function = function;
        }

        #endregion Construction

        #region Data

        private IRealFunction _function;

        public virtual IRealFunction Function
        {
            get { return _function; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Function of scalar argument is not specified (null reference).");
                _function = value;
            }
        }

        private double _epsilon;

        /// <summary>Gets or sets a small number used as criteria of where to calculate things (especially
        /// derivatives) in a special way in order to overcome singularities in expressions.
        /// For example, some expressions contain divisions by vector norm. When vector norm is close to 0,
        /// this results in nearly singular terms, which must be replaced by suitable limits.</summary>
        public double Epsilon
        {
            get { return _epsilon; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Small parameter defining environment where calculation is special due to singularities"
                        + Environment.NewLine + "Must be greater or equal to 0.");
                _epsilon = value;
            }
        }

        /// <summary>Returns a short name of the function.</summary>
        public override string Name
        {
            get { return "Radial scalar function"; }
        }

        /// <summary>Returns a short description of the function.</summary>
        public override string Description
        {
            get { return "A radial scalar function, non-distorted (with concentric isosurfaces)."; }
        }

        #endregion Data

        #region Evaluation

        /// <summary>Returns the value of this function at the specified parameter.</summary>
        public override double Value(IVector x)
        {
            if (x == null)
                throw new ArgumentNullException("Vector of parameters not specified when calculationg gradient of scalar function \"" + Name + "\" (nulll reference).");
            return Function.Value(x.Norm);
        }

        /// <summary>Tells whether value of the function is defined by implementation.</summary>
        public override bool ValueDefined { get { return true;  }
            protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
        }

       /// <summary>Returns the first derivative of this function at the specified parameter.</summary>
        /// <param name="x">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
        public override void GradientPlain(IVector x, IVector gradient)
        {
            if (x == null)
                throw new ArgumentException("Vector of parameters not specified when calculationg gradient of scalar function \"" + Name +"\" (nulll reference).");
            int dim = x.Length;
            if (gradient == null)
                gradient = x.GetNew(dim);
            else if (gradient.Length!=dim)
                gradient = x.GetNew(dim);
            double norm = x.Norm;
            if (norm>Epsilon)
            {
                Vector.CopyPlain(x,gradient);
                gradient.Normalize();
                Vector.MultiplyPlain(gradient, Function.Derivative(norm), gradient);
            } else
            {
                for (int i = 0; i < dim; ++i)
                    gradient[i] = 0.0;
            }
        }

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool GradientDefined { get { return Function.DerivativeDefined; }
            protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
        }


        /// <summary>Returns the second derivative (Hessian) of this function at the specified arameter.</summary>
        /// <param name="x">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
        public override void HessianPlain(IVector x, IMatrix hessian)
        {
            int dim = x.Length;
            double normx = x.Norm;
            double fDer = Function.Derivative(normx);
            double fSecondDer = Function.SecondDerivative(normx);
            if (normx < Epsilon)
            {
                // Close to zero, hessian matrix is diagonal, diagonal elements equal to second derivatives 
                // of 1-D function from which this scalar function is derived.
                hessian.SetZero();
                for (int i = 0; i < dim; ++i)
                    hessian[i, i] = fSecondDer;
            } else
            {
                double normxSquare = normx * normx;
                double normxCube = normxSquare * normx;
                double element;
                // Nondiagonal terms:
                for (int i=1; i<dim; ++i)
                    for (int j = 0; j < i; ++j)
                    {
                        double prod_x_i_j = x[i]*x[j];
                        element = fSecondDer * prod_x_i_j / normxSquare
                            - fDer * prod_x_i_j / normxCube;
                        hessian[i,j] = element;
                        hessian[j, i] = element;
                    }
                for (int i = 0; i < dim; ++i)
                {
                    double x_i_square = x[i]; // square of component
                    x_i_square *= x_i_square;
                    hessian[i,i]=fSecondDer*x_i_square/normxSquare
                        + fDer*((1/normx)+(x_i_square/normxCube));
                }
            }
        }

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool HessianDefined { get { return Function.SecondDerivativeDefined; }
            protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
        }


        #endregion Evaluation


    }  //  class ScalarFunctionRadial


}


