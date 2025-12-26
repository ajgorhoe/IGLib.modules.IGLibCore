// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{



    /// <summary>Base class for scalar functions with affine transformation of co-ordinates.
    /// The reference function is defined while the actual function is defined as that reference function of transformed coordinates.
    /// If transformation is not specified then function reduces to the reference one.</summary>
    /// $A Igor xx May10 Dec10;
    public abstract class ScalarFunctionBase : ScalarFunctionUntransformedBase, 
        IScalarFunction
    {
    
        #region Construction

        /// <summary>Default constructor for scalar functions of affine transformed parameters.
        /// Constructs untransformed reference function.</summary>
        protected ScalarFunctionBase() {  }

        /// <summary>Constructs a scalar function that is identical to some reference funciton acting on affine transformed parameters.</summary>
        /// <param name="transf">Affine transformation of parameters from the reference to the actual coordinate system. 
        /// If null then parameters are not transformed.</param>
        public ScalarFunctionBase(IAffineTransformation transf)
        {
            this.Transformation = transf;
        }

        #endregion Construction

        #region Data

        protected IVector _refParam;

        protected IVector _refGrad;

        protected IMatrix _refHess;

        private IAffineTransformation _transf;

        /// <summary>Transformation of parameters.
        /// Actual function is evaluated as some reference function evaluated at inverse affine-transformed parameters.</summary>
        public virtual IAffineTransformation Transformation
        {
            get { return _transf; }
            set
            {
                _transf = value;
            }
        }


        #endregion Data

        #region Evaluation

        /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
        /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
        public abstract double ReferenceValue(IVector parameters);

        
       /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
        /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
        public abstract void ReferenceGradientPlain(IVector parameters, IVector gradient);
        

        /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
        /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
        public abstract void ReferenceHessianPlain(IVector parameters, IMatrix hessian);
        


        /// <summary>Returns the value of this function at the specified parameter.</summary>
        public override double Value(IVector x)
        {
            //if (x == null)
            //    throw new ArgumentNullException("Vector of parameters not specified when calculationg value of scalar function \"" + Name + "\" (nulll reference).");
            if (_transf == null)
                return ReferenceValue(x);
            else
            {
                _transf.TransformBackCoordinates(x, ref _refParam);
                return ReferenceValue(_refParam);
            }
        }

        ///// <summary>Tells whether value of the function is defined by implementation.</summary>
        //public override bool ValueDefined { get { return true; } }

       /// <summary>Returns the first derivative of this function at the specified parameter.</summary>
        /// <param name="x">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
        public override void GradientPlain(IVector x, IVector gradient)
        {
            //if (x == null)
            //    throw new ArgumentException("Vector of parameters not specified when calculationg gradient of scalar function \"" + Name +"\" (nulll reference).");
            int dim = x.Length;
            if (_transf == null)
                ReferenceGradientPlain(x, gradient);
            else
            {
                _transf.TransformBackCoordinates(x, ref _refParam);
                if (_refGrad == null)
                    _refGrad = x.GetNew(dim);
                else if (_refGrad.Length!=dim)
                    _refGrad = x.GetNew(dim);
                ReferenceGradientPlain(_refParam, _refGrad);
                _transf.TransformGradientPlain(_refGrad, gradient);
            }
        }

        ///// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        //public virtual bool GradientDefined { get { return Function.DerivativeDefined; } }


        /// <summary>Returns the second derivative (Hessian) of this function at the specified arameter.</summary>
        /// <param name="x">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
        public override void HessianPlain(IVector x, IMatrix hessian)
        {
            //if (x == null)
            //    throw new ArgumentException("Vector of parameters not specified when calculationg gradient of scalar function \"" + Name +"\" (nulll reference).");
            int dim = x.Length;
            if (_transf == null)
                ReferenceHessianPlain(x, hessian);
            else
            {
                _transf.TransformBackCoordinates(x, ref _refParam);
                if (_refHess == null)
                    _refHess = x.GetNewMatrix(dim, dim);
                else if (_refHess.RowCount != dim || _refHess.ColumnCount !=dim)
                    _refHess = x.GetNewMatrix(dim, dim);
                ReferenceHessianPlain(_refParam, _refHess);
                _transf.TransformHessianPlain(_refHess, hessian);
            }
        }

        ///// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        //public override bool HessianDefined { get { return Function.SecondDerivativeDefined; } }


        #endregion Evaluation

    } //  class ScalarFunctionTransformedBase

}