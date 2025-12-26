// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{



    /// <summary>Scalar functions of vector arguments, with eventual affine transformation of parameters.
    /// If transformation is defined then actual function is evaluated as reference function evaluated
    /// at inverse affine transformed parameters.</summary>
    /// $A Igor xx May10 Dec10;
    public interface IScalarFunction: IScalarFunctionUntransformed
    {

        #region Data


        /// <summary>Affine transformation of parameters. Actual function is evaluated as reference function evaluated
        /// at inverse affine transformed parameters.</summary>
        IAffineTransformation Transformation
        { get; set; }

        #endregion Data

        #region Evaluation

        /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
        /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
        double ReferenceValue(IVector parameters);

        /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
        /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
        void ReferenceGradientPlain(IVector parameters, IVector gradient);

        /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
        /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
        void ReferenceHessianPlain(IVector parameters, IMatrix hessian);

        #endregion Evaluation

    }  // interface IScalarFunction


    /// <summary>Scalar functions of vector arguments.</summary>
    /// $A Igor xx May10;
    public interface IScalarFunctionUntransformed
    {

        #region Data
        /// <summary>Returns a short name of the function.</summary>
        string Name { get; }

        /// <summary>Returns a short description of the function.</summary>
        string Description { get; }

        #endregion Data

        #region Evaluation
        
        /// <summary>Evaluates whatever needs to be evaluated and stores the results on the specified storage object.</summary>
        /// <param name="data">Evaluation data object where results of calculation are stored.
        /// It also contains flags taht specify what needs to be calculated, and resulting flags specifying what has been calculated.</param>
        void Evaluate(IScalarFunctionResults data);
        
        /// <summary>Returns the value of this function at the specified parameter.</summary>
        double Value(IVector x);

        /// <summary>Tells whether value of the function is defined by implementation.</summary>
        bool ValueDefined { get; }

        /// <summary>Calculates first order derivatives (gradient) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first order derivatives (the gradient) are stored.</param>
        void GradientPlain(IVector parameters, IVector gradient);

        /// <summary>Calculates first order derivatives (gradient) of this function at the specified parameters.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first order derivatives (gradient) are stored. Passed by reference.</param>
        void Gradient(IVector parameters, ref IVector gradient);

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        bool GradientDefined { get; }

        /// <summary>Calculates the second derivative (Hessian matrix) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian matrix) are stored.</param>
        void HessianPlain(IVector parameters, IMatrix hessian);

        /// <summary>Calculates the second derivative (Hessian matrix) of this function at the specified parameters.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian matrix) are stored. Passed by reference.</param>
        void Hessian(IVector parameters, ref IMatrix hessian);

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        bool HessianDefined { get; }


        #endregion Evaluation


    }  // interface IScalarFunctionUntransformed

    

}
