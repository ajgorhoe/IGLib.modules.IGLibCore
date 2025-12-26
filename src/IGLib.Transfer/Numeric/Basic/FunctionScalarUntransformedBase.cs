// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using IG.Lib;

namespace IG.Num
{


    /// <summary>Base class for defining scalar functions without possibility of affine transformation of parameters.</summary>
    /// <remarks>More or less all scalar functions should derive from this class.</remarks>
    /// $A Igor xx May10 Dec10;
    /// TODO: implement ICloneable
    public abstract class ScalarFunctionUntransformedBase : IScalarFunctionUntransformed
    {

        #region Data

        protected string _name;

        protected string _description;


        /// <summary>Returns a short name of thecurrent function.</summary>
        public virtual string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    _name = "ScalarFunction_" + this.GetType().Name;
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
                    _description = "Scalar function " + this.GetType().FullName + ".";
                return _description;
            }
            protected internal set { _description = value; }
        }

        #endregion // Data

        #region Evaluation

        /// <summary>Evaluates whatever needs to be evaluated and stores the results on the specified storage object.</summary>
        /// <param name="data">Evaluation data object where results of calculation are stored.
        /// It also contains flags taht specify what needs to be calculated, and resulting flags specifying what has been calculated.</param>
        public virtual void Evaluate(IScalarFunctionResults data)
        {
            if (data == null)
                throw new ArgumentNullException("Object containing calculation request flags and storage for results is not specified (null reference).");
            IVector parameters;
            int dim;
            parameters = data.Parameters;
            if (parameters==null)
                throw new ArgumentNullException("Vector of parameters is not specified on evaluation data (null reference).");
            dim = parameters.Length;
            data.Calculated = false;  // reset all calculated flags to false
            data.ErrorCode = 0;
            data.ErrorString = null;
            if (data.ReqValue)
            {
                try
                {
                    if (this.ValueDefined)
                    {
                        data.Value = Value(parameters);
                        data.CalculatedValue = true;
                    } else
                    {
                        data.ErrorCode = -1;
                        data.ErrorString = "Could not calculate scalar function value: not defined for function \"" + this.Name + "\".";
                    }
                }
                catch(Exception)
                {
                    data.ErrorCode = -11;
                    data.ErrorString = "Could not calculate scalar function value (exception thrown), function \"" + this.Name + "\".";
                }
            }
            if (data.ReqGradient)
            {
                try
                {
                    if (this.GradientDefined)
                    {
                        // data.SetGradient(this.Derivative(parameters))
                        IVector grad = data.Gradient;
                        if (grad == null)
                            grad = parameters.GetNew(dim);
                        else if (grad.Length != dim)
                            grad = parameters.GetNew(dim);
                        this.Gradient(parameters, ref grad);
                        data.SetGradientReference(grad);
                        data.CalculatedGradient = true;
                    } else
                    {
                        data.ErrorCode = -2;
                        data.ErrorString = "Could not calculate scalar function gradient. Gradient not defined for function \"" + this.Name + "\".";
                    }
                }
                catch(Exception)
                {
                    data.ErrorCode = -12;
                    data.ErrorString = "Could not calculate scalar function gradient (exception thrown), function \"" + this.Name + "\".";
                }
            }
            if (data.ReqGradient)
            {
                try
                {
                    if (this.GradientDefined)
                    {
                        // data.SetGradient(this.Derivative(parameters))
                        IVector grad = data.Gradient;
                        if (grad == null)
                            grad = parameters.GetNew(dim);
                        else if (grad.Length != dim)
                            grad = parameters.GetNew(dim);
                        this.Gradient(parameters, ref grad);
                        data.SetGradientReference(grad);
                        data.CalculatedGradient = true;
                    }
                    else
                    {
                        data.ErrorCode = -2;
                        data.ErrorString = "Could not calculate scalar function gradient. Gradient not defined for function \"" + this.Name + "\".";
                    }
                }
                catch (Exception)
                {
                    data.ErrorCode = -12;
                    data.ErrorString = "Could not calculate scalar function gradient (exception thrown), function \"" + this.Name + "\".";
                }
            }
            if (data.ReqHessian)
            {
                try
                {
                    if (this.HessianDefined)
                    {
                        // data.SetGradient(this.Derivative(parameters))
                        IMatrix hess = data.Hessian;
                        if (hess == null)
                            hess = parameters.GetNewMatrix(dim, dim);
                        else if (hess.RowCount != dim || hess.ColumnCount != dim)
                            hess = parameters.GetNewMatrix(dim, dim);
                        this.Hessian(parameters, ref hess);
                        data.SetHessianReference(hess);
                        data.CalculatedHessian = true;
                    }
                    else
                    {
                        data.ErrorCode = -3;
                        data.ErrorString = "Could not calculate scalar function hessian. Hessian not defined for function \"" + this.Name + "\".";
                    }
                }
                catch (Exception)
                {
                    data.ErrorCode = -13;
                    data.ErrorString = "Could not calculate scalar function hessian (exception thrown), function \"" + this.Name + "\".";
                }
            }
        }


        /// <summary>Returns the value of this function at the specified parameter.</summary>
        public abstract double Value(IVector parameters);

        /// <summary>Tells whether value of the function is defined by implementation.</summary>
        public abstract bool ValueDefined { get; protected set; }

        /// <summary>Calculates first order derivatives (gradient) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first order derivatives (the gradient) are stored.</param>
        public abstract void GradientPlain(IVector parameters, IVector gradient);

        /// <summary>Calculates first order derivatives (gradient) of this function at the specified parameters.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first order derivatives (gradient) are stored. Passed by reference.</param>
        public virtual void Gradient(IVector parameters, ref IVector gradient)
        {
            if (parameters == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate graditent. Vector of parameters not specified (null reference).");
            if (gradient == null)
                gradient = parameters.GetNew();
            else if (gradient.Length!=parameters.Length)
                gradient = parameters.GetNew();
            GradientPlain(parameters, gradient);
        }

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public abstract bool GradientDefined { get; protected set; }


        /// <summary>Calculates the second derivative (Hessian matrix) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian matrix) are stored.</param>
        public abstract void HessianPlain(IVector parameters, IMatrix hessian);

        /// <summary>Calculates the second derivative (Hessian matrix) of this function at the specified parameters.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian matrix) are stored. Passed by reference.</param>
        public virtual void Hessian(IVector parameters, ref IMatrix hessian)
        {
            if (parameters == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate Hessian. Vector of parameters not specified (null reference).");
            if (hessian == null)
                hessian = parameters.GetNewMatrix();
            else if (hessian.RowCount != parameters.Length || hessian.ColumnCount!=parameters.Length)
                hessian = parameters.GetNewMatrix();
            HessianPlain(parameters, hessian);
        }

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public abstract bool HessianDefined { get; protected set; }

        #endregion Evaluation

        #region Numeric


        // TODO: Supplement the range of numerical tools 



        /// <summary>Calculates numerical derivatives (gradient) of this function. Forward difference formula is normally used.
        /// <para>WARNING: Dimensions of vector arguments must match. This function does not check for 
        /// consistency of argument dimensions.</para>
        /// <para>REMARK: Vector x is changed during operation, but is set to initial value before function 
        /// returns (unless an exception is thrown).</para>
        /// </summary>
        /// <param name="x">Point at which derivative is calculated.</param>
        /// <param name="stepSizes">Step size for numerical differentiation.</param>
        /// <param name="gradient">Vector where calculated derivatives (function gradient) are stored.</param>
        public virtual void NumericalGradientForwardPlain(IVector x, IVector stepSizes, IVector gradient)
        {
            if (!ValueDefined)
                throw new NotSupportedException("Function " + Name + ": Can not calculate numerical gradient of a scalar function, function value not defined.");
            int dim = x.Length;
            double value = Value(x);
            for (int i=0; i<dim; ++i)
            {
                double compx = x[i];
                double h = stepSizes[i];
                x[i]+=h;
                double valPert=Value(x);
                gradient[i] = (valPert-value)/h;
                x[i]=compx;
            }
        }

        /// <summary>Calculates numerical derivatives (gradient) of this function. Forward difference formula is normally used.
        /// <para>If result vector doesn't have correct dimensions or it is not allocated, then allocation is performed first.</para>
        /// <para>Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</para>
        /// </summary>
        /// <param name="x">Point (vector of parameters) at which gradient is calculated.</param>
        /// <param name="stepSizes">Step sizes for numerical differentiation.</param>
        /// <param name="gradient">Vector where the calculated gradient is stored.</param>
        public void NumericalGradientForward(IVector x, IVector stepSizes, ref IVector gradient)
        {
            if (x == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate numerical gradient, parameters not specified (null argument).");
            int dim = x.Length;
            if (stepSizes == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate numerical gradient, step sizes not specified (null argument).");
            else if (stepSizes.Length != dim)
                throw new ArgumentException("Function " + Name + ": Can not calculate numerical gradient, dimensions of parameters and step sizes do not match.");
            if (gradient == null)
                gradient = x.GetNew(dim);
            else if (gradient.Length != dim)
                gradient = x.GetNew(dim);
            NumericalGradientForwardPlain(x, stepSizes, gradient);
        }

        /// <summary>Calculates numerical derivatives (gradient) of this function. Forward difference formula is used.
        /// <para>Warning: This method internally allocates a new vector where step sizes are stored.</para>
        /// <para>If result vector doesn't have correct dimensions or it is not allocated, then allocation is performed first.</para>
        /// <para>Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</para>
        /// </summary>
        /// <param name="x">Point (vector of parameters) at which gradient is calculated.</param>
        /// <param name="stepSize">Step size for numerical differentiation (the same in all directions).</param>
        /// <param name="gradient">Vector where the calculated gradient is stored.</param>
        /// TODO: When NumericalDerivativePlain is tested enough, replace this function in sulch a way
        /// that stepSize is used directly in a copy of that function (currently a vetor of step sizes is created where each component holds the same step size.)
        public void NumericalGradientForward(IVector x, double stepSize, ref IVector gradient)
        {
            IVector stepSizes = x.GetNew(x.Length);
            for (int i = 0; i < x.Length; ++i)
                stepSizes[i] = stepSize;
            NumericalGradientForward(x, stepSizes, ref gradient);
        }


        /// <summary>Calculates numerical derivatives (gradient) of this function. Forward difference formula is normally used.
        /// <para>Warning: Dimensions of vector arguments must match. This function does not check for 
        /// consistency of argument dimensions.</para>
        /// <para>Remark: Vector x is changed during operation, but is set to initial value before function 
        /// returns (unless an exception is thrown).</para>
        /// </summary>
        /// <param name="x">Point at which derivative is calculated.</param>
        /// <param name="stepSizes">Step size for numerical differentiation.</param>
        /// <param name="gradient">Vector where calculated derivatives (function gradient) are stored.</param>
        public virtual void NumericalGradientCentralPlain(IVector x, IVector stepSizes, IVector gradient)
        {
            if (!ValueDefined)
                throw new NotSupportedException("Function " + Name + ": Can not calculate numerical gradient of a scalar function, function value not defined.");
            int dim = x.Length;
            double value = Value(x);
            for (int i = 0; i < dim; ++i)
            {
                double compx = x[i];
                double h = stepSizes[i];
                x[i] = compx + h;
                double valPlus = Value(x);
                x[i] = compx - h;
                double valMinus = Value(x);

                gradient[i] = (valPlus - valMinus) / (2.0*h);
                x[i] = compx;
            }
        }

        /// <summary>Calculates numerical derivatives (gradient) of this function. Forward difference formula is normally used.
        /// <para>If result vector doesn't have correct dimensions or it is not allocated, then allocation is performed first.</para>
        /// <para>Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</para>
        /// </summary>
        /// <param name="x">Point (vector of parameters) at which gradient is calculated.</param>
        /// <param name="stepSizes">Step sizes for numerical differentiation.</param>
        /// <param name="gradient">Vector where the calculated gradient is stored.</param>
        public void NumericalGradientCentral(IVector x, IVector stepSizes, ref IVector gradient)
        {
            if (x == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate numerical gradient, parameters not specified (null argument).");
            else if (stepSizes == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate numerical gradient, step sizes not specified (null argument).");
            else if (stepSizes.Length != x.Length)
                throw new ArgumentException("Function " + Name + ": Can not calculate numerical gradient, dimensions of parameters and step sizes do not match.");
            if (gradient == null)
                gradient = x.GetNew(x.Length);
            else if (gradient.Length != x.Length)
                gradient = x.GetNew(x.Length);
            NumericalGradientCentralPlain(x, stepSizes, gradient);
        }


        /// <summary>Calculates numerical derivatives (gradient) of this function. Forward difference formula is normally used.
        /// <para>Warning: This method internally allocates a new vector where step sizes are stored.</para>
        /// <para>If the result vector doesn't have correct dimensions or it is not allocated, then allocation is performed first.</para>
        /// <para>Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</para>
        /// </summary>
        /// <param name="x">Point (vector of parameters) at which gradient is calculated.</param>
        /// <param name="stepSize">Step size for numerical differentiation (the same in all directions).</param>
        /// <param name="gradient">Vector where the calculated gradient is stored.</param>
        /// TODO: When NumericalDerivativePlain is tested enough, replace this function in sulch a way
        /// that stepSize is used directly in a copy of that function (currently a vetor of step sizes is created where each component holds the same step size.)
        public void NumericalGradientCentral(IVector x, double stepSize, ref IVector gradient)
        {
            IVector stepSizes = x.GetNew(x.Length);
            for (int i = 0; i < x.Length; ++i)
                stepSizes[i] = stepSize;
            NumericalGradientCentral(x, stepSizes, ref gradient);
        }
        


        /// <summary>Calculates numerical second derivatives (Hessian) of this function. Forward difference formula is normally used.
        /// WARNING: Dimensions of vector arguments must match. This function does not check for consistency of argument dimensions.
        /// REMARK: Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</summary>
        /// <param name="x">Point (vector of parameters) at which derivative is calculated.</param>
        /// <param name="aux">Auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="stepSizes">Step size for numerical differentiation.</param>
        /// <param name="hessian">Matrix where calculated second derivatives (function Hessian) are stored.</param>
        public virtual void NumericalHessianForwardPlain(IVector x, IVector stepSizes, IVector aux, IMatrix hessian)
        {
            if (!ValueDefined)
                throw new NotSupportedException("Function " + Name + ": Can not calculate numerical Hessian of a scalar function, function value not defined.");
            int dim = x.Length;
            double value = Value(x);
            // First, store values at perturbed parameters (where parameters are one by one perturbed 
            // by step sizes while all other parameters are kept unchanged) to the auxiliary vector:
            for (int i=0; i<dim; ++i)
            {
                double compx = x[i];
                double h_i = stepSizes[i];
                x[i]+=h_i;
                aux[i]=Value(x);
                x[i]=compx;
            }
            // Calculate diagonal terms:
            for (int i=0; i<dim; ++i)
            {
                double compx = x[i];
                double h = stepSizes[i];
                x[i]-=h;
                double valueMinus = Value(x);
                double ValuePlus = aux[i];
                // Diagonal second derivative is calculated according to the central difference formula:
                hessian[i,i]=(ValuePlus-2*value+valueMinus)/(h*h);
                x[i]=compx;
            }
            for (int i=0; i<dim; ++i)
                for (int j=0; j<dim; ++j)
                {
                    double valuePlus_i = aux[i];
                    double valuePlus_j = aux[j];
                    double h_i = stepSizes[i];
                    double h_j = stepSizes[j];
                    double compx_i = x[i];
                    double compx_j = x[j];
                    x[i]+=stepSizes[i];
                    x[j]+=stepSizes[j];
                    double valuePlus_i_j = Value(x);
                    // Reset vector of parameters to the initial state:
                    x[i] = compx_i;
                    x[j] = compx_j;
                    // double der_i_j = ((valuePlus_i_j-valuePlus_i)/h_j-(valuePlus_j-value)/h_j)/h_i;
                    double der_i_j = ((valuePlus_i_j-valuePlus_i)-(valuePlus_j-value))/(h_i*h_j);
                    hessian[i,j] = der_i_j;
                    hessian[j,i] = der_i_j;
                }
        }
        
        /// <summary>Calculates numerical second derivatives (Hessian) of this function. Forward difference formula is normally used.
        /// REMARK: if auxiliary vector or result matrix don't have correct dimensions or they are not allocated, then allocation is performed first.
        /// REMARK: Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</summary>
        /// <param name="x">Point (vector of parameters) at which derivative is calculated.</param>
        /// <param name="aux">Auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="stepSizes">Step size for numerical differentiation.</param>
        /// <param name="hessian">Matrix where calculated second derivatives (function Hessian) are stored.</param>
        public virtual void NumericalHessianForward(IVector x, IVector stepSizes, ref IVector aux, ref IMatrix hessian)
        {
            if (x == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate numerical gradient, parameters not specified (null argument).");
            else if (stepSizes == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate numerical gradient, step sizes not specified (null argument).");
            else if (stepSizes.Length != x.Length)
                throw new ArgumentException("Function " + Name + ": Can not calculate numerical gradient, dimensions of parameters and step sizes do not match.");
            int dim = x.Length;
            if (hessian == null)
                hessian = x.GetNewMatrix(dim, dim);
            else if (hessian.RowCount != dim || hessian.ColumnCount != dim)
                hessian = x.GetNewMatrix(x.Length, x.Length);
            if (aux == null)
                aux = x.GetNew(dim);
            else if (aux.Length != dim)
                aux = x.GetNew(dim);
            NumericalHessianForwardPlain(x, stepSizes, aux, hessian);
        }

        /// <summary>Calculates numerical second derivatives (Hessian) of this function. Forward difference formula is normally used.
        /// REMARK: if auxiliary vector or result matrix don't have correct dimensions or they are not allocated, then allocation is performed first.
        /// REMARK: Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</summary>
        /// <param name="x">Point (vector of parameters) at which derivative is calculated.</param>
        /// <param name="aux">Auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="stepSize">Step size for numerical differentiation (the same in all directions).</param>
        /// <param name="hessian">Matrix where calculated second derivatives (function Hessian) are stored.</param>
        /// TODO: When NumericalSecondDerivativePlain is tested enough, replace this function in sulch a way
        /// that stepSize is used directly in a copy of that function (currently a vetor of step sizes is created where each component holds the same step size.)
        public virtual void NumericalHessianForward(IVector x, double stepSize, ref IVector aux, ref IMatrix hessian)
        {
            IVector stepSizes = x.GetNew(x.Length);
            for (int i = 0; i < x.Length; ++i)
                stepSizes[i] = stepSize;
            NumericalHessianForward(x, stepSizes, ref aux, ref hessian);
        }

        

        /// <summary>Calculates numerical second derivatives (Hessian) of this function. Forward difference formula is normally used.
        /// WARNING: Dimensions of vector arguments must match. This function does not check for consistency of argument dimensions.
        /// REMARK: Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</summary>
        /// <param name="x">Point (vector of parameters) at which derivative is calculated.</param>
        /// <param name="aux1">Auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="aux2">Another auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="stepSizes">Step size for numerical differentiation.</param>
        /// <param name="hessian">Matrix where calculated second derivatives (function Hessian) are stored.</param>
        public virtual void NumericalHessianCentralPlain(IVector x, IVector stepSizes,
            IVector aux1, IVector aux2, IMatrix hessian)
        {
            NumericalHessianCentralPlain(x, stepSizes,
              aux1, aux2, hessian, true /* useAnalyticalGradients */);
        }

        /// <summary>Calculates numerical second derivatives (Hessian) of this function. Forward difference formula is normally used.
        /// REMARK: if auxiliary vector or result matrix don't have correct dimensions or they are not allocated, then allocation is performed first.
        /// REMARK: Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</summary>
        /// <param name="x">Point (vector of parameters) at which derivative is calculated.</param>
        /// <param name="aux1">Auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="aux2">Another auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="stepSizes">Step size for numerical differentiation.</param>
        /// <param name="hessian">Matrix where calculated second derivatives (function Hessian) are stored.</param>
        public virtual void NumericalHessianCentral(IVector x, IVector stepSizes,
            ref IVector aux1, ref IVector aux2, ref IMatrix hessian)
        {
            if (x == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate numerical gradient, parameters not specified (null argument).");
            else if (stepSizes == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate numerical gradient, step sizes not specified (null argument).");
            else if (stepSizes.Length != x.Length)
                throw new ArgumentException("Function " + Name + ": Can not calculate numerical gradient, dimensions of parameters and step sizes do not match.");
            int dim = x.Length;
            if (hessian == null)
                hessian = x.GetNewMatrix(dim, dim);
            else if (hessian.RowCount != dim || hessian.ColumnCount != dim)
                hessian = x.GetNewMatrix(x.Length, x.Length);
            if (aux1 == null)
                aux1 = x.GetNew(dim);
            else if (aux1.Length != dim)
                aux1 = x.GetNew(dim);
            if (aux2 == null)
                aux2 = x.GetNew(dim);
            else if (aux2.Length != dim)
                aux2 = x.GetNew(dim);
            NumericalHessianCentralPlain(x, stepSizes, aux1, aux2, hessian);
        }

        /// <summary>Calculates numerical second derivatives (Hessian) of this function. Forward difference formula is normally used.
        /// REMARK: if auxiliary vector or result matrix don't have correct dimensions or they are not allocated, then allocation is performed first.
        /// REMARK: Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</summary>
        /// <param name="x">Point (vector of parameters) at which derivative is calculated.</param>
        /// <param name="stepSize">Step sizes for numerical differentiation.</param>
        /// <param name="aux1">Auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="aux2">The secont auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="hessian">Matrix where calculated second derivatives (function Hessian) are stored.</param>
        /// TODO: When NumericalSecondDerivativePlain is tested enough, replace this function in sulch a way
        /// that stepSize is used directly in a copy of that function (currently a vetor of step sizes is created where each component holds the same step size.)
        public virtual void NumericalHessianCentral(IVector x, double stepSize,
            ref IVector aux1, ref IVector aux2, ref IMatrix hessian)
        {
            IVector stepSizes = x.GetNew(x.Length);
            for (int i = 0; i < x.Length; ++i)
                stepSizes[i] = stepSize;
            NumericalHessianCentral(x, stepSizes, ref aux1, ref aux2, ref hessian);
        }



        /// <summary>Calculates numerical second derivatives (Hessian) of this function by the central difference formula.
        /// WARNING: Dimensions of vector arguments must match. This function does not check for consistency of argument dimensions.
        /// REMARK: Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</summary>
        /// <param name="x">Point (vector of parameters) at which derivative is calculated.</param>
        /// <param name="aux1">Auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="aux2">Another auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="stepSizes">Step size for numerical differentiation.</param>
        /// <param name="hessian">Matrix where calculated second derivatives (function Hessian) are stored.</param>
        /// <param name="useAnalyticalGradient">If true then analytical gradient are used when provided by the current function.</param>
        public virtual void NumericalHessianCentralPlain(IVector x, IVector stepSizes, 
            IVector aux1, IVector aux2, IMatrix hessian, bool useAnalyticalGradient)
        {
            if (useAnalyticalGradient && !GradientDefined)
                useAnalyticalGradient = false;  // analytical gradients not used if not provided
            if (!ValueDefined && !useAnalyticalGradient)
                throw new NotSupportedException("Function " + Name + ": Can not calculate numerical Hessian of a scalar function, function value not defined.");
            int dim = x.Length;
            for (int j = 0; j < dim; ++j)
            {
                double compx_j = x[j];
                double h_j = stepSizes[j];
                x[j] = compx_j - h_j;
                if (useAnalyticalGradient)
                    this.GradientPlain(x, aux1);
                else
                    this.NumericalGradientCentralPlain(x, stepSizes, aux1);
                x[j] = compx_j + h_j;
                if (useAnalyticalGradient)
                    this.GradientPlain(x, aux2);
                else
                    this.NumericalGradientCentralPlain(x, stepSizes, aux2);
                x[j] = compx_j;  // restore the component to its original value.
                h_j *= 2.0;
                for (int i = 0; i < dim; ++i)
                {
                    hessian[i, j] = (aux2[i] - aux1[i]) / h_j;
                }
            }
        }

        /// <summary>Calculates numerical second derivatives (Hessian) of this function by the central difference formula.
        /// REMARK: if auxiliary vector or result matrix don't have correct dimensions or they are not allocated, then allocation is performed first.
        /// REMARK: Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</summary>
        /// <param name="x">Point (vector of parameters) at which derivative is calculated.</param>
        /// <param name="aux1">Auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="aux2">Another auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="stepSizes">Step size for numerical differentiation.</param>
        /// <param name="hessian">Matrix where calculated second derivatives (function Hessian) are stored.</param>
        /// <param name="useAnalyticalGradient">If true then analytical gradient are used when provided by the current function.</param>
        public virtual void NumericalHessianCentral(IVector x, IVector stepSizes,
            ref IVector aux1, ref IVector aux2, ref IMatrix hessian, bool useAnalyticalGradient)
        {
            if (x == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate numerical gradient, parameters not specified (null argument).");
            else if (stepSizes == null)
                throw new ArgumentNullException("Function " + Name + ": Can not calculate numerical gradient, step sizes not specified (null argument).");
            else if (stepSizes.Length != x.Length)
                throw new ArgumentException("Function " + Name + ": Can not calculate numerical gradient, dimensions of parameters and step sizes do not match.");
            int dim = x.Length;
            if (hessian == null)
                hessian = x.GetNewMatrix(dim, dim);
            else if (hessian.RowCount != dim || hessian.ColumnCount != dim)
                hessian = x.GetNewMatrix(x.Length, x.Length);
            if (aux1 == null)
                aux1 = x.GetNew(dim);
            else if (aux1.Length != dim)
                aux1 = x.GetNew(dim);
            if (aux2 == null)
                aux2 = x.GetNew(dim);
            else if (aux2.Length != dim)
                aux2 = x.GetNew(dim);
            NumericalHessianCentralPlain(x, stepSizes, aux1, aux2, hessian, useAnalyticalGradient);
        }

        /// <summary>Calculates numerical second derivatives (Hessian) of this function. Forward difference formula is normally used.
        /// REMARK: if auxiliary vector or result matrix don't have correct dimensions or they are not allocated, then allocation is performed first.
        /// REMARK: Vector x is changed during operation, but is set to initial value before function returns (unless an exception is thrown).</summary>
        /// <param name="x">Point (vector of parameters) at which derivative is calculated.</param>
        /// <param name="stepSize">Step sizes for numerical differentiation.</param>
        /// <param name="aux1">Auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="aux2">The second auxiliary storage vector, must be of the same dimension as vector of parameters.</param>
        /// <param name="hessian">Matrix where calculated second derivatives (function Hessian) are stored.</param>
        /// <param name="useAnalyticalGradient">If true then analytical gradient are used when provided by the current function.</param>
        /// <remarks>
        /// TODO: When NumericalSecondDerivativePlain is tested enough, replace this function in sulch a way
        /// that stepSize is used directly in a copy of that function (currently a vetor of step sizes is created where each component holds the same step size.)
        /// </remarks>
        public virtual void NumericalHessianCentral(IVector x, double stepSize,
            ref IVector aux1, ref IVector aux2, ref IMatrix hessian, bool useAnalyticalGradient)
        {
            IVector stepSizes = x.GetNew(x.Length);
            for (int i = 0; i < x.Length; ++i)
                stepSizes[i] = stepSize;
            NumericalHessianCentral(x, stepSizes, ref aux1, ref aux2, ref hessian, useAnalyticalGradient);
        }


        #endregion Numeric


        #region Testing

        /// <summary>Tests speed of evaluation of the current scalar function and writes results to the console.</summary>
        /// <param name="parameters">Central value of parameters at which function value, gradient and/or Hessian is evaluated.</param>
        /// <param name="numEvaluations">Number of evaluations performed.</param>
        /// <param name="randomPerturbations">Whether to use random perturbations or not.</param>
        /// <param name="relativePerturbationSize">Relative size of random perturbations, when used.</param>
        /// <param name="calcValue">Whether function value is calculated.</param>
        /// <param name="calcGradient">Whether function gradient is calculated.</param>
        /// <param name="calcHessian">Whether function Hessian is calculated.</param>
        /// <param name="writeLastResult">Whether results of the last calculation are written to console.</param>
        /// <remarks>The specified number of function values, gradients and/or Hessians is evaluated, and time
        /// of evaluation is reported (with average time for a single evaluation also reported).</remarks>
        public void TestSpeed(IVector parameters, int numEvaluations, bool randomPerturbations, double relativePerturbationSize, 
            bool calcValue, bool calcGradient, bool calcHessian, bool writeLastResult)
        {
            if (parameters == null)
                throw new ArgumentException("Parameters where gradient is calculated are not specified (null argument).");
            int dim = parameters.Length;
            if (dim <= 0)
                throw new ArgumentException("Dimension of vector of parameters is less than 1 (" + dim + ").");
            if(numEvaluations<1)
                throw new ArgumentException("Number of evaluations is less than 1.");
            if (!(calcValue || calcGradient || calcHessian))
                throw new ArgumentException("Nothing to be calculated, neither value nor gradient nor Hessian.");
            if (calcValue && !this.ValueDefined)
                throw new ArgumentException("Calculation of function value is requested  but is not defined for the current scalar functon.");
            if (calcGradient && !this.GradientDefined)
                throw new ArgumentException("Calculation of function gradient is requested  but is not defined for the current scalar functon.");
            if (calcHessian && !this.HessianDefined)
                throw new ArgumentException("Calculation of function Hessian is requested  but is not defined for the current scalar functon.");
            if (relativePerturbationSize <= 0)
                relativePerturbationSize = 1.0e-10;
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("Test of evaluation speed of a scalar function...");
            Console.WriteLine("Function name: " + Name);
            Console.WriteLine("Description: " + Description);
            Console.WriteLine("Function type: " + this.GetType().Name);
            Console.WriteLine("Dimension: " + dim);
            Console.WriteLine("Parameters: " + Environment.NewLine + "  " + parameters.ToString("G4"));
            if (randomPerturbations)
                Console.WriteLine("Parameters will be randomly perturbated, max. relative perturbation size: " 
                    + relativePerturbationSize + ".");
            IVector actualParam = null;
            IRandomGenerator rand = RandomGenerator.Global;
            double value = 11111111.11111111; // detection of errors made easier
            IVector gradient = null;
            IMatrix hessian = null; ;
            StopWatch1 t = new StopWatch1();
            t.Start();
            for (int i = 0; i < numEvaluations; ++i)
            {
                if (randomPerturbations)
                {
                    Vector.Multiply(parameters, 1.0 + rand.NextDouble(), ref actualParam); 
                }
                else
                    actualParam = parameters;
                if (calcValue)
                    value = this.Value(parameters);
                if (calcGradient)
                    this.Gradient(parameters, ref gradient);
                if (calcHessian)
                    this.Hessian(parameters, ref hessian);
            }
            Console.WriteLine("Evaluation finished.");
            if (writeLastResult)
            {
                Console.WriteLine("Last parameters: " + Environment.NewLine + "  " + parameters.ToString("G4"));
                if (calcValue)
                    Console.WriteLine("Calculated value at last parameters: " + value.ToString("G6", CultureInfo.InvariantCulture));
                if (calcGradient)
                {
                    Console.WriteLine("Calculated gradient at last parameters: "
                        + Environment.NewLine + gradient.ToString("G4"));
                }
                if (calcHessian)
                {
                    Console.WriteLine("Calculated Hessian at last parameters: "
                        + Environment.NewLine + hessian.ToString("G4"));
                }
                Console.WriteLine();
            }
            Console.WriteLine(numEvaluations + " evaluations performed, parameter dimension: {0}.", parameters.Length);
            Console.Write("Evaluated:");
            if (calcValue)
                Console.Write(" value");
            if (calcGradient)
                Console.Write(" gradient");
            if (calcHessian)
                Console.Write(" Hessian");
            Console.WriteLine();
            Console.WriteLine("{0} evaluations finished in {1,0:G5} s (CPU: {2,0:G5} s.",
                numEvaluations, t.TotalTime, t.TotalCpuTime);
            Console.WriteLine("Average evaluation time: {0,0:G3} s (CPU: {1,0:G3} s)",
                t.TotalTime/(double) numEvaluations, t.TotalCpuTime/(double) numEvaluations);
            Console.WriteLine();
            Console.WriteLine("Test of evaluation speed done.");
            Console.WriteLine("==========");
            Console.WriteLine();
        }


        /// <summary>Tests the calculation of gradients of the current scalar function.
        /// <para>Differentiation is performed according to the central difference formula.</para>
        /// <para>Calculations of numerical and analytical gradients can be verified.</para>
        /// <para>Numerical gradients can be calculated at different step szes that are iteratively
        /// reduced by the specified integer factor, which gives indication of precision.</para></summary>
        /// <param name="parameters">Vector of parameters at which function gradient is calculated.</param>
        /// <param name="stepSizes">Vector of initial step sizes used for function differentiation.</param>
        /// <param name="stepReductionFactor">Factor greater than 1 by which step size is reduced at each iteration.</param>
        /// <param name="numStepReductions">Number of step size reductions.</param>
        /// <param name="checkNumerical">Whether check is also made only on basis of numerical values.</param>
        /// <param name="writeErrorComponents">Whether gradient errors (and differences between results 
        /// at different step sizes in the case of pure numerical check) in component-wise form are also 
        /// written to the console (if fallse, only norms are written).</param>
        /// <param name="writeGradientComponents">Whether gradients (calculated and analytical) are written in 
        /// component-wise form.</param>
        public void TestGradient(IVector parameters, IVector stepSizes, int stepReductionFactor, int numStepReductions,
            bool checkNumerical, bool writeErrorComponents, bool writeGradientComponents)
        {
            TestGradient(parameters, stepSizes, stepReductionFactor, numStepReductions,
                checkNumerical, writeErrorComponents, writeGradientComponents, true /* useCentralDifference */);
        }

        /// <summary>Tests the calculation of gradients of the current scalar function.
        /// <para>Calculations of numerical and analytical gradients can be verified.</para>
        /// <para>Numerical gradients can be calculated at different step szes that are iteratively
        /// reduced by the specified integer factor, which gives indication of precision.</para></summary>
        /// <param name="parameters">Vector of parameters at which function gradient is calculated.</param>
        /// <param name="stepSizes">Vector of initial step sizes used for function differentiation.</param>
        /// <param name="stepReductionFactor">Factor greater than 1 by which step size is reduced at each iteration.</param>
        /// <param name="numStepReductions">Number of step size reductions.</param>
        /// <param name="checkNumerical">Whether check is also made only on basis of numerical values.</param>
        /// <param name="writeErrorComponents">Whether gradient errors (and differences between results 
        /// at different step sizes in the case of pure numerical check) in component-wise form are also 
        /// written to the console (if fallse, only norms are written).</param>
        /// <param name="writeGradientComponents">Whether gradients (calculated and analytical) are written in 
        /// component-wise form.</param>
        /// <param name="useCentralDifference">Whether the more precise but also more time consuming central difference scheme
        /// is used for numerical differentiation.</param>
        public void TestGradient(IVector parameters, IVector stepSizes, int stepReductionFactor, int numStepReductions,
            bool checkNumerical, bool writeErrorComponents, bool writeGradientComponents, bool useCentralDifference)
        {
            if (parameters == null)
                throw new ArgumentException("Parameters where gradient is calculated are not specified (null argument).");
            int dim = parameters.Length;
            if (dim <= 0)
                throw new ArgumentException("Dimension of vector of parameters is less than 1 (" + dim + ").");
            if (stepSizes == null)
                throw new ArgumentException("Vector of initial step sizes for numerical differentiation is not specified (null argument).");
            if (stepSizes.Length != dim)
                throw new Exception("Dimension of vector of step sizes (" + stepSizes.Length + ") is different than the number of function parameters ("
                    + stepSizes.Length);
            if (numStepReductions <= 0)
                numStepReductions = 1;
            //if (stepReductionFactor <= 0)
            //    throw new ArgumentException("Step reduction factor can not be less or equal to 0 (" + stepReductionFactor + " passed).");
            if (stepReductionFactor <= 1)
                stepReductionFactor = 2;

            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("Test of numerical gradients of a scalar function...");
            Console.WriteLine("Function name: " + Name);
            Console.WriteLine("Description: " + Description);
            Console.WriteLine("Function type: " + this.GetType().Name);
            Console.WriteLine("Dimension: " + dim);
            Console.WriteLine("Parameters: " + Environment.NewLine + "  " + parameters.ToString("G4"));
            Console.WriteLine("Initial step sizes: " + Environment.NewLine + "  " + stepSizes.ToString("G4"));
            bool hasAnalytical = this.GradientDefined;
            if (!hasAnalytical)
                checkNumerical = true;
            List<IVector> numericalGradients = new List<IVector>();
            List<IVector> stepVectors = new List<IVector>();
            IVector differenceVector = new Vector(dim);
            double differenceNorm;
            for (int i = 0; i <= numStepReductions; ++i)
            {
                IVector steps = stepSizes.GetCopy();
                Vector.Multiply(steps, 1.0 / Math.Pow((double) stepReductionFactor, (double)i), ref steps);
                IVector grad = new Vector(dim);
                if (useCentralDifference)
                    NumericalGradientCentral(parameters, steps, ref grad);
                else
                    NumericalGradientForward(parameters, steps, ref grad);
                numericalGradients.Add(grad);
                stepVectors.Add(steps);
                //Console.WriteLine("Steps: " + steps.ToString("G8"));
                //Console.WriteLine("    {0}. steps norm: {1,0:G12}", i, steps.NormEuclidean);
            }
            if (checkNumerical)
            {
                if (writeGradientComponents || (writeErrorComponents && numericalGradients.Count>1))
                {
                    Console.WriteLine();
                    Console.WriteLine("Numerical gradients calculated with different step sizes:");
                    for (int i = 0; i < numericalGradients.Count; ++i)
                    {
                        if (i < numericalGradients.Count - 1)
                        {
                            // Calculate the difference between the current and the next (reduced-step) numerical
                            // gradient:
                            Vector.Subtract(numericalGradients[i], numericalGradients[i + 1], ref differenceVector);
                            differenceNorm = differenceVector.NormEuclidean;
                        } else
                        {
                            differenceVector.SetConstant(0.0);
                            differenceNorm = 0.0;
                        }
                        Console.WriteLine("Step size vector No. {0,0:N}, norm = {1,0:G3}, reduction: 1/{2}", 
                            i, stepVectors[i].NormEuclidean, (int)Math.Round(Math.Pow(stepReductionFactor, i)));
                        if (i < numericalGradients.Count-1)
                        {
                            Console.WriteLine("  Norm of the difference with the next result: {0,0:G4}.", differenceNorm);
                        }
                        if (writeGradientComponents)
                        {
                            Console.WriteLine("  Numerical gradient: ");
                            Console.WriteLine("  " + numericalGradients[i].ToString("G5"));
                        }
                        if (writeErrorComponents && i < numericalGradients.Count - 1)
                        {
                            Console.WriteLine("  Difference with the next result (norm = {0,0:G3}):", differenceNorm);
                            Console.WriteLine("  " + differenceVector.ToString("G3"));
                        }
                    }
                }
                if (numericalGradients.Count > 1)
                {
                    // Write norms of differences between calculated num. gradients at different steps in a table:
                    Console.WriteLine();
                    Console.WriteLine("Norms of differences between numerical gradients at consecutive step sizes:");
                    Console.WriteLine("  {0,3:S}: {1,10:S}    {2,-12:S};  {3,-10:S} {4, -10:S}", 
                        "No.", "step norm", "step reduction", "diff. norm", "rel. err.");
                    for (int i = 0; i < numericalGradients.Count - 1; ++i)
                    {
                        Vector.Subtract(numericalGradients[i], numericalGradients[i + 1], ref differenceVector);
                        differenceNorm = differenceVector.NormEuclidean;
                        Console.WriteLine("  {0,3:D}: {1,10:G3}    1/{2,-12:D};  {3,-10:G3} {4,-10:G3}",
                            i, stepVectors[i].NormEuclidean, (int)Math.Round(Math.Pow(stepReductionFactor,i)), 
                            differenceNorm, differenceNorm/(numericalGradients[i].NormEuclidean + 1.0e-40));
                    }
                }
            }
            if (hasAnalytical)
            {
                // Compare gradients to the analytical gradient:
                IVector analyticalGradient = new Vector(dim);
                analyticalGradient.SetConstant(111111.111111);  // enable detection of components that are not evaluated by the gradient function
                this.Gradient(parameters, ref analyticalGradient);
                if (writeGradientComponents || writeErrorComponents)
                {
                    Console.WriteLine();
                    Console.WriteLine("Numerical gradients calculated with different steps, compared to analytical:");
                    Console.WriteLine("Analytical gradient: " + Environment.NewLine + "  " 
                        + analyticalGradient.ToString("G4"));
                    for (int i = 0; i < numericalGradients.Count; ++i)
                    {
                        // Calculate the difference between the current and the next (reduced-step) numerical
                        // gradient:
                        Vector.Subtract(numericalGradients[i], analyticalGradient, ref differenceVector);
                        differenceNorm = differenceVector.NormEuclidean;
                        Console.WriteLine("Step size vector No. {0,0:N}, norm = {1,0:G3}, reduction: 1/{2}",
                            i, stepVectors[i].NormEuclidean, (int)Math.Round(Math.Pow(stepReductionFactor, i)));
                        Console.WriteLine("  Norm of the difference with analytical gradient: {0,0:G4}.", differenceNorm);
                        if (writeGradientComponents)
                        {
                            Console.WriteLine("  Numerical gradient: ");
                            Console.WriteLine("  " + numericalGradients[i].ToString("G5"));
                        }
                        if (writeErrorComponents)
                        {
                            Console.WriteLine("  Difference with the analytical gradient (norm = {0,0:G3}):", differenceNorm);
                            Console.WriteLine("  " + differenceVector.ToString("G3"));
                        }
                    }
                }
                // Write norms of differences between numerical and analytical gradients at different steps in a table:
                Console.WriteLine();
                Console.WriteLine("Norms of differences between numerical and analytical gradient at different step sizes:");
                Console.WriteLine("  {0,3:S}: {1,10:S}    {2,-12:S};  {3,-10:s} {4, -10:S}",
                    "No.", "step norm", "step reduction", "dif. norm", "rel. err.");
                for (int i = 0; i < numericalGradients.Count - 1; ++i)
                {
                    Vector.Subtract(numericalGradients[i], analyticalGradient, ref differenceVector);
                    differenceNorm = differenceVector.NormEuclidean;
                    Console.WriteLine("  {0,3:D}: {1,10:G3}    1/{2,-12:G};  {3,-10:G3} {4,-10:G3}",
                        i, stepVectors[i].NormEuclidean, (int)Math.Round(Math.Pow(stepReductionFactor, i)),
                        differenceNorm, differenceNorm/(analyticalGradient.NormEuclidean + 1.0e-40));
                }
            }
            Console.WriteLine();
            Console.WriteLine("Test of numerical gradients done.");
            Console.WriteLine("==========");
            Console.WriteLine();
        }




        /// <summary>Tests the calculation of Hessians of the current scalar function.
        /// <para>Calculations of numerical and analytical Hessians can be verified.</para>
        /// <para>Numerical Hessians can be calculated at different step szes that are iteratively
        /// reduced by the specified integer factor, which gives indication of precision.</para></summary>
        /// <param name="parameters">Vector of parameters at which function Hessian is calculated.</param>
        /// <param name="stepSizes">Vector of initial step sizes used for function differentiation.</param>
        /// <param name="stepReductionFactor">Factor greater than 1 by which step size is reduced at each iteration.</param>
        /// <param name="numStepReductions">Number of step size reductions.</param>
        /// <param name="checkNumerical">Whether check is also made only on basis of numerical values.</param>
        /// <param name="writeErrorComponents">Whether Hessian errors (and differences between results 
        /// at different step sizes in the case of pure numerical check) in component-wise form are also 
        /// written to the console (if fallse, only norms are written).</param>
        /// <param name="writeHessianComponents">Whether Hessians (calculated and analytical) are written in 
        /// component-wise form.</param>
        public void TestHessian(IVector parameters, IVector stepSizes, int stepReductionFactor, int numStepReductions,
            bool checkNumerical, bool writeErrorComponents, bool writeHessianComponents)
        {
            TestHessian(parameters, stepSizes, stepReductionFactor, numStepReductions,
            checkNumerical, writeErrorComponents, writeHessianComponents, true /* useCentralDifference */);
        }


        /// <summary>Tests the calculation of Hessians of the current scalar function.
        /// <para>Calculations of numerical and analytical Hessians can be verified.</para>
        /// <para>Numerical Hessians can be calculated at different step szes that are iteratively
        /// reduced by the specified integer factor, which gives indication of precision.</para></summary>
        /// <param name="parameters">Vector of parameters at which function Hessian is calculated.</param>
        /// <param name="stepSizes">Vector of initial step sizes used for function differentiation.</param>
        /// <param name="stepReductionFactor">Factor greater than 1 by which step size is reduced at each iteration.</param>
        /// <param name="numStepReductions">Number of step size reductions.</param>
        /// <param name="checkNumerical">Whether check is also made only on basis of numerical values.</param>
        /// <param name="writeErrorComponents">Whether Hessian errors (and differences between results 
        /// at different step sizes in the case of pure numerical check) in component-wise form are also 
        /// written to the console (if fallse, only norms are written).</param>
        /// <param name="writeHessianComponents">Whether Hessians (calculated and analytical) are written in 
        /// component-wise form.</param>
        /// <param name="useCentralDifference">Whether the central difference formula is used for numerial differentiation.</param>
        public void TestHessian(IVector parameters, IVector stepSizes, int stepReductionFactor, int numStepReductions,
            bool checkNumerical, bool writeErrorComponents, bool writeHessianComponents, bool useCentralDifference)
        {
            if (parameters == null)
                throw new ArgumentException("Parameters where Hessian is calculated are not specified (null argument).");
            int dim = parameters.Length;
            if (dim <= 0)
                throw new ArgumentException("Dimension of vector of parameters is less than 1 (" + dim + ").");
            if (stepSizes == null)
                throw new ArgumentException("Vector of initial step sizes for numerical differentiation is not specified (null argument).");
            if (stepSizes.Length != dim)
                throw new Exception("Dimension of vector of step sizes (" + stepSizes.Length + ") is different than the number of function parameters ("
                    + stepSizes.Length);
            if (numStepReductions <= 0)
                numStepReductions = 1;
            //if (stepReductionFactor <= 0)
            //    throw new ArgumentException("Step reduction factor can not be less or equal to 0 (" + stepReductionFactor + " passed).");
            if (stepReductionFactor <= 1)
                stepReductionFactor = 2;

            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("Test of numerical Hessians of a scalar function...");
            Console.WriteLine("Function name: " + Name);
            Console.WriteLine("Description: " + Description);
            Console.WriteLine("Function type: " + this.GetType().Name);
            Console.WriteLine("Dimension: " + dim);
            Console.WriteLine("Parameters: " + Environment.NewLine + "  " + parameters.ToString("G4"));
            Console.WriteLine("Initial step sizes: " + Environment.NewLine + "  " + stepSizes.ToString("G4"));
            bool hasAnalytical = this.HessianDefined;
            if (!hasAnalytical)
                checkNumerical = true;
            List<IMatrix> numericalHessians = new List<IMatrix>();
            List<IVector> stepVectors = new List<IVector>();
            IMatrix differenceMatrix = new Matrix(dim, dim);
            IVector auxVector1 = new Vector(dim);
            IVector auxVector2 = new Vector(dim);
            double differenceNorm;
            for (int i = 0; i <= numStepReductions; ++i)
            {
                IVector steps = stepSizes.GetCopy();
                Vector.Multiply(steps, 1.0 / Math.Pow((double)stepReductionFactor, (double)i), ref steps);
                IMatrix hess = new Matrix(dim, dim);
                if (useCentralDifference)
                    NumericalHessianCentral(parameters, steps, ref auxVector1, ref auxVector2, ref hess);
                else
                    NumericalHessianForward(parameters, steps, ref auxVector1, ref hess);
                numericalHessians.Add(hess);
                stepVectors.Add(steps);
                //Console.WriteLine("Steps: " + steps.ToString("G8"));
                //Console.WriteLine("    {0}. steps norm: {1,0:G12}", i, steps.NormForbenius);
            }
            if (checkNumerical)
            {
                if (writeHessianComponents || (writeErrorComponents && numericalHessians.Count > 1))
                {
                    Console.WriteLine();
                    Console.WriteLine("Numerical Hessians calculated with different step sizes:");
                    for (int i = 0; i < numericalHessians.Count; ++i)
                    {
                        if (i < numericalHessians.Count - 1)
                        {
                            // Calculate the difference between the current and the next (reduced-step) numerical
                            // Hessian:
                            Matrix.Subtract(numericalHessians[i], numericalHessians[i + 1], ref differenceMatrix);
                            differenceNorm = differenceMatrix.NormForbenius;
                        }
                        else
                        {
                            differenceMatrix.SetConstant(0.0);
                            differenceNorm = 0.0;
                        }
                        Console.WriteLine("Step size vector No. {0,0:N}, norm = {1,0:G3}, reduction: 1/{2}",
                            i, stepVectors[i].NormEuclidean, (int)Math.Round(Math.Pow(stepReductionFactor, i)));
                        if (i < numericalHessians.Count - 1)
                        {
                            Console.WriteLine("  Norm of the difference with the next result: {0,0:G4}.", differenceNorm);
                        }
                        if (writeHessianComponents)
                        {
                            Console.WriteLine("  Numerical Hessian: ");
                            Console.WriteLine("  " + numericalHessians[i].ToString("G5"));
                        }
                        if (writeErrorComponents && i < numericalHessians.Count - 1)
                        {
                            Console.WriteLine("  Difference with the next result (norm = {0,0:G3}):", differenceNorm);
                            Console.WriteLine("  " + differenceMatrix.ToString("G3"));
                        }
                    }
                }
                if (numericalHessians.Count > 1)
                {
                    // Write norms of differences between calculated num. Hessians at different steps in a table:
                    Console.WriteLine();
                    Console.WriteLine("Norms of differences between numerical Hessians at consecutive step sizes:");
                    Console.WriteLine("  {0,3:S}: {1,10:S}    {2,-12:S};  {3,-10:S} {4, -10:S}",
                        "No.", "step norm", "step reduction", "diff. norm", "rel. err.");
                    for (int i = 0; i < numericalHessians.Count - 1; ++i)
                    {
                        Matrix.Subtract(numericalHessians[i], numericalHessians[i + 1], ref differenceMatrix);
                        differenceNorm = differenceMatrix.NormForbenius;
                        Console.WriteLine("  {0,3:D}: {1,10:G3}    1/{2,-12:D};  {3,-10:G3} {4,-10:G3}",
                            i, stepVectors[i].NormEuclidean, (int)Math.Round(Math.Pow(stepReductionFactor, i)),
                            differenceNorm, differenceNorm / (numericalHessians[i].NormForbenius + 1.0e-40));
                    }
                }
            }
            if (hasAnalytical)
            {
                // Compare Hessians to the analytical Hessian:
                IMatrix analyticalHessian = new Matrix(dim, dim);
                analyticalHessian.SetConstant(111111.111111);  // enable detection of components that are not evaluated by the Hessian function
                this.Hessian(parameters, ref analyticalHessian);
                if (writeHessianComponents || writeErrorComponents)
                {
                    Console.WriteLine();
                    Console.WriteLine("Numerical Hessians calculated with different steps, compared to analytical:");
                    Console.WriteLine("Analytical Hessian: " + Environment.NewLine + "  "
                        + analyticalHessian.ToString("G4"));
                    for (int i = 0; i < numericalHessians.Count; ++i)
                    {
                        // Calculate the difference between the current and the next (reduced-step) numerical
                        // Hessian:
                        Matrix.Subtract(numericalHessians[i], analyticalHessian, ref differenceMatrix);
                        differenceNorm = differenceMatrix.NormForbenius;
                        Console.WriteLine("Step size vector No. {0,0:N}, norm = {1,0:G3}, reduction: 1/{2}",
                            i, stepVectors[i].NormEuclidean, (int)Math.Round(Math.Pow(stepReductionFactor, i)));
                        Console.WriteLine("  Norm of the difference with analytical Hessian: {0,0:G4}.", differenceNorm);
                        if (writeHessianComponents)
                        {
                            Console.WriteLine("  Numerical Hessian: ");
                            Console.WriteLine("  " + numericalHessians[i].ToString("G5"));
                        }
                        if (writeErrorComponents)
                        {
                            Console.WriteLine("  Difference with the analytical Hessian (norm = {0,0:G3}):", differenceNorm);
                            Console.WriteLine("  " + differenceMatrix.ToString("G3"));
                        }
                    }
                }
                // Write norms of differences between numerical and analytical Hessians at different steps in a table:
                Console.WriteLine();
                Console.WriteLine("Norms of differences between numerical and analytical Hessian at different step sizes:");
                Console.WriteLine("  {0,3:S}: {1,10:S}    {2,-12:S};  {3,-10:s} {4, -10:S}",
                    "No.", "step norm", "step reduction", "dif. norm", "rel. err.");
                for (int i = 0; i < numericalHessians.Count - 1; ++i)
                {
                    Matrix.Subtract(numericalHessians[i], analyticalHessian, ref differenceMatrix);
                    differenceNorm = differenceMatrix.NormForbenius;
                    Console.WriteLine("  {0,3:D}: {1,10:G3}    1/{2,-12:G};  {3,-10:G3} {4,-10:G3}",
                        i, stepVectors[i].NormEuclidean, (int)Math.Round(Math.Pow(stepReductionFactor, i)),
                        differenceNorm, differenceNorm / (analyticalHessian.NormForbenius + 1.0e-40));
                }
            }
            Console.WriteLine();
            Console.WriteLine("Test of numerical Hessians done.");
            Console.WriteLine("==========");
            Console.WriteLine();
        }






        /// <summary>Performs numerical tests on the function, with output written to the console.
        /// Derivatives and second derivatives are tested and compared to numerical derivatives.</summary>
        /// <param name="from">Initial parameters on which tests are performed.</param>
        /// <param name="to">Final parameters on which tests are performed.</param>
        /// <param name="numProbes">Number of points on which tests are performed. Points are linearly distributed between from and to.</param>
        /// <param name="stepSizes">Step sizes for numerical differentiation.</param>
        /// <param name="tolerance">Absolute tolerance for match between numerical and analytical values.</param>
        /// <param name="testDerivatives">Specifies whether first order derivatives should be tested.</param>
        /// <param name="testSecondDerivatives">Specifies whether second order derivatives should be tested.</param>
        /// <param name="printDifferences">Specifies whether differences between numerical and analytical derivatives should be printed.</param>
        /// <param name="printResults">Specifies whether results should be printed.</param>
        public void Test(IVector from, IVector to, int numProbes, IVector stepSizes, double tolerance, 
            bool testDerivatives, bool testSecondDerivatives, bool printDifferences, bool printResults)
        {
            if (from == null)
                throw new ArgumentNullException("Vector of initial function parameters is not specified (null argument).");
            else if (to == null)
                throw new ArgumentNullException("Vector of final function parameters is not specified (null argument).");
            else if (stepSizes == null)
                throw new ArgumentException("Vector of step sizes is not specified (null argument).");
            else if (from.Length != to.Length)
                throw new ArgumentException("Dimensions of initial and final vector of parameters don't match.");
            else if (stepSizes.Length != from.Length)
                throw new ArgumentException("Dimension of step sizes does not match the dimension of parameters.");
            Console.WriteLine();
            Console.WriteLine("Testing correctness of implementation of scalar function " + Name + ".");
            Console.WriteLine("Number of test points: " + numProbes);

            IVector unitStep = from.GetNew(); // difference between two consecutive points of the table 
            Vector.Subtract(to, from, ref unitStep);
            if (numProbes > 2)
                Vector.Divide(unitStep, numProbes - 1, ref unitStep);
            IVector step = from.GetNew();  // difference between teh current point and the initial point (from)
            IVector x = from.GetNew();
            if (testDerivatives)
            {
                Console.WriteLine();
                Console.WriteLine("Testing first order derivatives (comparing with numerical)...");
                if (!GradientDefined)
                    Console.WriteLine("ERROR: evaluation of first order derivatives is not defined for this function.");
                else
                {
                    for (int iPoint = 0; iPoint < numProbes; ++iPoint)
                    {
                        if (iPoint == 0)
                            Vector.Copy(from, ref x);
                        else if (iPoint == numProbes - 1)
                            Vector.Copy(to, ref x);
                        else
                        {
                            Vector.Multiply(unitStep, (double)iPoint, ref step);
                            Vector.Add(from, step, ref x);
                        }
                        IVector gradAnalytical = x.GetNew();
                        IVector gradNumerical = x.GetNew();
                        this.Gradient(x, ref gradAnalytical);
                        this.NumericalGradientForward(x, stepSizes, ref gradNumerical);
                        IVector diff = null;
                        Vector.Subtract(gradNumerical, gradAnalytical, ref diff);
                        if (printResults)
                        {
                            Console.WriteLine("Analytical gradient: ");
                            Console.WriteLine(gradAnalytical.ToString());
                            Console.WriteLine("Numerical gradient: ");
                            Console.WriteLine(gradNumerical.ToString());
                            Console.WriteLine();
                        }
                        if (printDifferences)
                        {
                            Console.WriteLine("Difference between numerical and analytical gradient:");
                            Console.WriteLine(diff.ToString());
                            Console.WriteLine();
                        }
                        for (int icomp = 0; icomp < from.Length; ++icomp)
                        {
                            if (Math.Abs(diff[icomp]) > tolerance)
                            {
                                Console.WriteLine("!!!! Large ERROR in gradient component " + icomp + ".");
                                Console.WriteLine("  Analytical: " + gradAnalytical[icomp] + ", numerical: " + gradNumerical[icomp]);
                                Console.WriteLine("  Error: " + (diff[icomp]) + ", relative: " + (Math.Abs(diff[icomp] / gradAnalytical[icomp])));
                            }
                        }

                        Console.WriteLine("Testing of first order derivatives done.");
                    }
                }
            }
            if (testSecondDerivatives)
            {
                Console.WriteLine();
                Console.WriteLine("Testing second order derivatives (comparing with numerical)...");
                if (!HessianDefined)
                    Console.WriteLine("ERROR: evaluation of second order derivatives is not defined for this function.");
                else
                {
                    for (int iPoint = 0; iPoint < numProbes; ++iPoint)
                    {
                        if (iPoint == 0)
                            Vector.Copy(from, ref x);
                        else if (iPoint == numProbes - 1)
                            Vector.Copy(to, ref x);
                        else
                        {
                            Vector.Multiply(unitStep, (double)iPoint, ref step);
                            Vector.Add(from, step, ref x);
                        }
                        IMatrix hessAnalytical = x.GetNewMatrix();
                        IMatrix hessNumerical = x.GetNewMatrix();
                        IVector aux = x.GetNew();
                        this.Hessian(x, ref hessAnalytical);
                        this.NumericalHessianForward(x, stepSizes, ref aux, ref hessNumerical);
                        IMatrix diff = null;
                        Matrix.Subtract(hessNumerical, hessAnalytical, ref diff);
                        if (printResults)
                        {
                            Console.WriteLine("Analytical Hessian: ");
                            Console.WriteLine(hessAnalytical.ToString());
                            Console.WriteLine("Numerical gradient: ");
                            Console.WriteLine(hessNumerical.ToString());
                            Console.WriteLine();
                        }
                        if (printDifferences)
                        {
                            Console.WriteLine("Difference between numerical and analytical Hessian:");
                            Console.WriteLine(diff.ToString());
                            Console.WriteLine();
                        }
                        for (int irow = 0; irow < from.Length; ++irow)
                            for (int icol = 0; icol<from.Length; ++icol)
                            {
                                if (Math.Abs(diff[irow, icol]) > tolerance)
                                {
                                    Console.WriteLine("!!!! Large ERROR in Hessian component [" + irow + ", " + icol + "].");
                                    Console.WriteLine("  Analytical: " + hessAnalytical[irow, icol] + ", numerical: " + hessNumerical[irow, icol]);
                                    Console.WriteLine("  Error: " + (diff[irow, icol]) + ", relative: " + (Math.Abs(diff[irow, icol] / hessAnalytical[irow, icol])));
                                }
                            }
                        Console.WriteLine("Testing of second order derivatives done.");
                    }
                }
            }
            Console.WriteLine("Test completed for scalar function " + Name + ".");
            Console.WriteLine();
        }

        /// <summary>Performs numerical tests on the function, with output written to the console.
        /// Derivatives and second derivatives are tested and compared to numerical derivatives.
        /// This method tests first and second order derivatives, prints differences between analytical
        /// and numerical values to the console, and prints reports where absolute differences exceed rolerance.</summary>
        /// <param name="from">Initial parameters on which tests are performed.</param>
        /// <param name="to">Final parameters on which tests are performed.</param>
        /// <param name="numProbes">Number of points on which tests are performed. Points are linearly distributed between from and to.</param>
        /// <param name="stepSizes">Step sizes for numerical differentiation.</param>
        /// <param name="tolerance">Absolute tolerance for match between numerical and analytical values.</param>
        public void Test(IVector from, IVector to, int numProbes, IVector stepSizes, double tolerance)
        {
            Test(from, to, numProbes, stepSizes, tolerance,
                true /* testDerivatives */, true /* testSecondDerivatives */, true /* printDifferences */, false /* printResults */);
        }

        
        /// <summary>Performs numerical tests on the function, with output written to the console.
        /// Derivatives and second derivatives are tested and compared to numerical derivatives.</summary>
        /// <param name="from">Initial parameters on which tests are performed.</param>
        /// <param name="to">Final parameters on which tests are performed.</param>
        /// <param name="numProbes">Number of points on which tests are performed. Points are linearly distributed between from and to.</param>
        /// <param name="stepSize">Step size for numerical differentiation (the same in all directions).</param>
        /// <param name="tolerance">Absolute tolerance for match between numerical and analytical values.</param>
        /// <param name="testDerivatives">Specifies whether first order derivatives should be tested.</param>
        /// <param name="testSecondDerivatives">Specifies whether second order derivatives should be tested.</param>
        /// <param name="printDifferences">Specifies whether differences between numerical and analytical derivatives should be printed.</param>
        /// <param name="printResults">Specifies whether results should be printed.</param>
        public void Test(IVector from, IVector to, int numProbes, double stepSize, double tolerance,
            bool testDerivatives, bool testSecondDerivatives, bool printDifferences, bool printResults)
        {
            IVector stepSizesVec = from.GetNew(from.Length);
            for (int i = 0; i < from.Length; ++i)
                stepSizesVec[i] = stepSize;
            Test(from, to, numProbes, stepSizesVec, tolerance,
                testDerivatives, testSecondDerivatives, printDifferences, printResults);
        }

        /// <summary>Performs numerical tests on the function, with output written to the console.
        /// Derivatives and second derivatives are tested and compared to numerical derivatives.</summary>
        /// <param name="from">Initial parameters on which tests are performed.</param>
        /// <param name="to">Final parameters on which tests are performed.</param>
        /// <param name="numProbes">Number of points on which tests are performed. Points are linearly distributed between from and to.</param>
        /// <param name="stepSize">Step size for numerical differentiation (the same in all directions).</param>
        /// <param name="tolerance">Absolute tolerance for match between numerical and analytical values.</param>
        public void Test(IVector from, IVector to, int numProbes, double stepSize, double tolerance)
        {
            Test(from, to, numProbes, stepSize, tolerance,
               true /* testDerivatives */, true /* testSecondDerivatives */, true /* printDifferences */, false /* printResults */);
        }



        //// TODO: implement this function!
        ///// <summary>Performs some tests on the function, with output written to a console.</summary>
        ///// <param name="from"></param>
        ///// <param name="to"></param>
        ///// <param name="numintervals"></param>
        ///// <param name="tolerance"></param>
        //public void Test(double from, double to, int numintervals, double tolerance)
        //{
        //}

        #endregion Testing


    }  //  class ScalarFunctionBase




}  // namespace IG.Num