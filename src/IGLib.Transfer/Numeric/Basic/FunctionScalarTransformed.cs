// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    /// <summary>Scalar function that is defined as some reference function evaluated at inverse 
    /// affine-trasformed parameters.
    /// This class is typically used to convert scalar functions without affine transformation of coordinates 
    /// (interface <see cref="IScalarFunctionUntransformed"/>) to those that support affine transformation of coordinates.</summary>
    /// $A Igor xx;
    public class ScalarFunctionTransformed : ScalarFunctionBase,
        IScalarFunction
    {

        #region Construction

        /// <summary>Default constructor disabled.</summary>
        private ScalarFunctionTransformed()
        { }

        /// <summary>Constructor. Creates a new scalar function based on a reference function evaluated at inverse
        /// affine transformed parameters.</summary>
        /// <param name="referenceFunction">The reference function that is used for evaluation of the current function.
        /// Value of the current function equals value of the reference function at inverse affine transformed parameters.</param>
        /// <param name="transf"></param>
        public ScalarFunctionTransformed(IScalarFunctionUntransformed referenceFunction, IAffineTransformation transf) :
            base(transf)
        { }

        #endregion Construction


        #region Data

        // private new string _name;

        // private new string _description;

        private string _nameDerived;

        private string _descriptionDerived;


        /// <summary>Gets function name.</summary>
        public override string Name
        {
            get
            {
                if (_name != null)  // check if specific name is defined for this function
                    return _name;
                else if (_nameDerived != null) // check if name that is derived from reference function is already stored
                    return _nameDerived;
                else
                {
                    // try to define a name derived form a reference function:
                    if (ReferenceFunction != null)
                    {
                        if (ReferenceFunction.Name != null)
                            _nameDerived = ReferenceFunction.Name + " With affine transformation of coordinates.";
                    }
                    return _nameDerived;
                }
            }
        }

        /// <summary>Sets name of the function.</summary>
        /// <param name="value">Function name to be set.</param>
        protected virtual void SetName(string value)
        {
            _name = value;
        }


        /// <summary>Gets a short description of the function.</summary>
        public override string Description
        {
            get
            {
                if (_description != null)  // check if specific description is defined for this function
                    return _description;
                else
                    if (_descriptionDerived != null) // check if description that is derived from reference function is already stored
                        return _descriptionDerived;
                    else
                    {
                        // try to define a description derived form a reference function:
                        if (ReferenceFunction != null)
                        {
                            if (ReferenceFunction.Description != null)
                                _descriptionDerived = ReferenceFunction.Description + " With affine transformation of coordinates.";
                        }
                        return _descriptionDerived;
                    }
            }
        }

        /// <summary>Sets a short description of a function.</summary>
        /// <param name="value">String to which function description is set.</param>
        protected virtual void SetDescription(string value)
        {
            _description = value;
        }

        IScalarFunctionUntransformed _referenceFunction;

        /// <summary>Reference function that is used for evaluation of the current scalar function.
        /// The current function is defined as this reference function evaluated at inverse Affine transformed coordinates.</summary>
        public IScalarFunctionUntransformed ReferenceFunction
        {
            get { return _referenceFunction; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Reference function is not specified (null reference).");
                _referenceFunction = value;
                _nameDerived = null;
                _descriptionDerived = null;
            }
        }

        #endregion Data


        #region Evaluation

        /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
        /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
        public override double ReferenceValue(IVector parameters)
        {
            return ReferenceFunction.Value(parameters);
        }


        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool ValueDefined
        {
            get { return ReferenceFunction.ValueDefined; }
            protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
        }


        /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
        /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
        public override void ReferenceGradientPlain(IVector parameters, IVector gradient)
        {
            ReferenceFunction.GradientPlain(parameters, gradient);
        }

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool GradientDefined
        {
            get { return ReferenceFunction.GradientDefined; }
            protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }

        }


        /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
        /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
        public override void ReferenceHessianPlain(IVector parameters, IMatrix hessian)
        {
            ReferenceFunction.HessianPlain(parameters, hessian);
        }

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool HessianDefined
        {
            get { return ReferenceFunction.HessianDefined; }
            protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }

        }

        #endregion Evaluation

    }  // class ScalarFunctionTransformed


}