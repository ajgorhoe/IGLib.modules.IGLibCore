
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{


    /// <summary>Base class for VectorFunctionBase and VectorFunctionBaseComponentwise.
    /// In general, use <see name="VectorFunctionBase"/> and <see name="VectorFunctionBaseComponentwise"/>
    /// in order to derive from.
    /// </summary>
    /// $A Igor xx May10 Dec10;
    /// TODO: Implement linear combination of functions (and add them to the interface!!!)
    public abstract class VectorFunctionBaseGeneral : IVectorFunction, ILockable
    {

        public VectorFunctionBaseGeneral()
        {
            // ResultsStore = new VectorFunctionBase.ObjectStoreResults(this);
        }

        #region Data

        protected object _lock = new object();

        public object Lock
        { get { return _lock; } }

        protected string _name;

        protected string _description;

        protected int _numParameters = -1;  // -1 for undefined

        protected int _numValues = -1;  // -1 for undefined

        protected bool _valuesDefined = true;

        protected bool _derivativeDefined = false;

        protected bool _secondDerivativeDefined = false;

        protected bool _componentEvaluation = false;


        /// <summary>Returns a short name of the function.</summary>
        public virtual string Name
        {
            get 
            {
                if (string.IsNullOrEmpty(_name))
                    _name = "VectorFunction_" + this.GetType().Name;
                return _name; 
            }
            protected internal set { _name = value; }
        }

        /// <summary>Returns a short description of the function.</summary>
        public virtual string Description
        {
            get 
            {
                if (string.IsNullOrEmpty(_description))
                    _description = "Vector function " + this.GetType().FullName + ".";
                return _description; 
            }
            protected internal set { _description = value; }
        }


        // TODO: 
        // Find proper handlng (public/private) for NumParameters and NumValues! If not settable then exceptions will be thrown!

        /// <summary>Sets number of parameters of the current vector function to the specified value.</summary>
        /// <param name="num">Number of parameters.</param>
        public void SetNumParameters(int num)
        { NumParameters = num; }

        /// <summary>Sets number of values of the vector function to the specified value.</summary>
        /// <param name="num"></param>
        public void SetNumValues (int num)
        { NumValues = num; }



        /// <summary>Gets number of parameters of the current vector function
        /// (-1 for not defined, in case that function works with different 
        /// numbers of parameters).</summary>
        public virtual int NumParameters
        {
            get { return _numParameters; }
            protected set { _numParameters = value; }
        }

        /// <summary>Gets number of values of the current vector function
        /// (-1 for not defined, e.g. in case that function works with different 
        /// numbers of parameters and number of functions depends on number of
        /// parameters).</summary>
        public virtual int NumValues
        {
            get { return _numValues; }
            protected set { _numValues = value; }
        }


        /// <summary>Tells whether the function supports evaluation of individual components.
        /// If not then evaluation is performed through the Evaluate function.</summary>
        public virtual bool ComponentWiseEvaluation
        {
            get { return _componentEvaluation; }
            protected set { _componentEvaluation = value; }
        }


        #endregion Data


        // Nothing is defined here, should be defined in subclass!

        #region Evaluation

        /// <summary>Tells whether value of the function is defined by implementation.</summary>
        public virtual bool ValueDefined
        {
            get { return _valuesDefined; }
            protected set { _valuesDefined = value; }
        }

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public virtual bool DerivativeDefined
        {
            get { return _derivativeDefined; }
            protected set { _derivativeDefined = value; }
        }

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public virtual bool SecondDerivativeDefined
        {
            get { return _secondDerivativeDefined; }
            protected set { _secondDerivativeDefined = value; }
        }


        /// <summary>Performs evaluation of requwester function results and writes them
        /// to the provided data structure.</summary>
        /// <param name="evaluationData">Data structure where request parameters are
        /// obtained and where results are written.</param>
        public abstract void Evaluate(IVectorFunctionResults evaluationData);


         #region ComponentWise

        /// <summary>Calculates and returns the particular component of the vector
        /// function value.</summary>
        /// <param name="evaluationData">Evaluation data that contains function parameters and
        /// can store function resuts. If the function does not support component-wise evaluation
        /// then results will be stored to this structure and returned from it. This makes reuse
        /// possible - when different components are evaluated subsequently with the same parameters,
        /// results are calculated only for the first time.</param>
        /// <param name="which">Specifies which function to evaluate.</param>
        public abstract double Value(IVectorFunctionResults evaluationData, int which);

        /// <summary>Calculates and returns the particular component of the vector
        /// function derivative.</summary>
        /// <param name="evaluationData">Evaluation data that contains function parameters and
        /// can store function resuts. If the function does not support component-wise evaluation
        /// then results will be stored to this structure and returned from it. This makes reuse
        /// possible - when different components are evaluated subsequently with the same parameters,
        /// results are calculated only for the first time.</param>
        /// <param name="which">Specifies which function to take.</param>
        /// <param name="component">Specifies which compoonent of the gradient should be returned.</param>
        public abstract double Derivative(IVectorFunctionResults evaluationData, int which,
            int component);

        /// <summary>Calculates and returns the particular component of the vector
        /// function's second derivative (Hessian).</summary>
        /// <param name="evaluationData">Evaluation data that contains function parameters and
        /// can store function resuts. If the function does not support component-wise evaluation
        /// then results will be stored to this structure and returned from it. This makes reuse
        /// possible - when different components are evaluated subsequently with the same parameters,
        /// results are calculated only for the first time.</param>
        /// <param name="which">Specifies which function to take.</param>
        /// <param name="rowNum">Specifies which row of the Hessian (matrix of second derivatives) should 
        /// be returned.</param>
        /// <param name="columnNum">Specifies which column of the Hessian (matrix of second derivatives) should 
        /// be returned.</param>
        public abstract double SecondDerivative(IVectorFunctionResults evaluationData, int which,
            int rowNum, int columnNum);


        VectorFunctionResults _auxResults = null;

        protected IVectorFunctionResults AuxResults
        {
            get {
                if (_auxResults == null)
                    _auxResults = new VectorFunctionResults();
                return _auxResults;
            }
        } 

        /// <summary>Calculates and returns the particular component of the vector
        /// function value.</summary>
        /// <param name="parameters">Parameters for which value of the specified component is calculated.</param>
        /// <param name="which">Specifies which function to evaluate.</param>
        public virtual double Value(IVector parameters, int which)
        {
            IVectorFunctionResults res = AuxResults;
            res.SetParametersReference(parameters);
            res.ReqValues = true;
            res.ReqGradients = false;
            res.ReqHessians = false;
            Evaluate(res);
            if (res.CalculatedValues == true)
                return res.Values[which];
            else throw new InvalidOperationException("Counld not calculate the specific vector function value.");
        }

        /// <summary>Calculates and returns the particular component of the vector
        /// function derivative.</summary>
        /// <param name="parameters">Parameters for which derivative of the specified component is calculated.</param>
        /// <param name="which">Specifies which function to take.</param>
        /// <param name="component">Specifies which compoonent of the gradient should be returned.</param>
        public virtual double Derivative(IVector parameters, int which, int component)
        {
            IVectorFunctionResults res = AuxResults;
            res.SetParametersReference(parameters);
            res.ReqValues = false;
            res.ReqGradients = true;
            res.ReqHessians = false;
            Evaluate(res);
            if (res.CalculatedGradients == true)
                return res.Gradients[which][component];
            else throw new InvalidOperationException("Counld not calculate the specific vector function derivative.");
        }

        /// <summary>Calculates and returns the particular component of the vector
        /// function's second derivative (Hessian).</summary>
        /// <param name="parameters">Parameters for which the specified second derivative of the specified component is calculated.</param>
        /// <param name="which">Specifies which function to take.</param>
        /// <param name="rowNum">Specifies which row of the Hessian (matrix of second derivatives) should 
        /// be returned.</param>
        /// <param name="columnNum">Specifies which column of the Hessian (matrix of second derivatives) should 
        /// be returned.</param>
        public virtual double SecondDerivative(IVector parameters, int which, int rowNum, int columnNum)
        {
            IVectorFunctionResults res = AuxResults;
            res.SetParametersReference(parameters);
            res.ReqValues = false;
            res.ReqGradients = false;
            res.ReqHessians = true;
            Evaluate(res);
            if (res.CalculatedHessians == true)
                return res.Values[which];
            else throw new InvalidOperationException("Counld not calculate the specific vector function second derivative.");
        }


        #endregion ComponentWise




        /// <summary>Returns the value of vector function at the specified parameter.</summary>
        // TODO: introduce store of vector function results!
        public void Value(IVector parameters, ref List<double> values)
        {
            bool calculateValues = true;
            bool calculateGradients = false;
            List<IVector> gradients = null;
            bool calculateHessians = false;
            List<IMatrix> hessians = null;
            Evaluate(parameters, ref calculateValues, ref values,
                ref calculateGradients, ref gradients,
                ref calculateHessians, ref  hessians);
        }

        /// <summary>Returns the first derivative of this function at the specified parameter.</summary>
        public void Derivative(IVector parameters, ref List<IVector> gradients)
        {
            bool calculateValues = false;
            List<double> values = null;
            bool calculateGradients = true;
            bool calculateHessians = false;
            List<IMatrix> hessians = null;
            Evaluate(parameters, ref calculateValues, ref values,
                ref calculateGradients, ref gradients,
                ref calculateHessians, ref  hessians);
        }

        /// <summary>Returns the second derivative (Hessian) of this function at the specified arameter.</summary>
        public void SecondDerivative(IVector parameters, ref List<IMatrix> hessians)
        {
            bool calculateValues = false;
            List<double> values = null;
            bool calculateGradients = false;
            List<IVector> gradients = null;
            bool calculateHessians = true;
            // List<IMatrix> hessians = null;
            Evaluate(parameters, ref calculateValues, ref values,
                ref calculateGradients, ref gradients,
                ref calculateHessians, ref  hessians);
        }


        /// <summary>Calculation of values, gradients, and hessians of the vector function 
        /// according to request flags.</summary>
        /// <param name="parameters">Parameters at which evaluation takes place.</param>
        /// <param name="calculateValues">Flag for calculation of function values, input/output.</param>
        /// <param name="values">Function values, output.</param>
        /// <param name="calculateGradients">Flag for calculation of functions gradients, input/output.</param>
        /// <param name="gradients">Gradients, output.</param>
        /// <param name="calculateHessians">Flag for calculation of Hessians, input/output.</param>
        /// <param name="hessians">Functions' hessians, output.</param>
        public abstract void Evaluate(IVector parameters, ref bool calculateValues, ref List<double> values,
                ref bool calculateGradients, ref List<IVector> gradients,
                ref bool calculateHessians, ref List<IMatrix> hessians);


        #endregion Evaluation


        #region LinearCombinations

        /// <summary>Returns value of linear combination of functions contained in this vector
        /// function, with specified coefficients at specified parameters.</summary>
        /// <param name="evaluationData">Data used for evaluation that also contains parameters
        /// (in evaluationData.Parameters)</param>
        /// <param name="coefficients">Coefficients of linear combination.</param>
        public double LinearCombinationValue(IVectorFunctionResults evaluationData, IVector coefficients)
        {
            double ret = 0;
            for (int which = 0; which < coefficients.Length; ++which)
                ret += coefficients[which] * Value(evaluationData, which);
            return ret;
        }

        /// <summary>Returns gradient of linear combination of functions contained in this vector
        /// function, with specified coefficients at specified parameters.</summary>
        /// <param name="evaluationData">Data used for evaluation that also contains parameters
        /// (in evaluationData.Parameters)</param>
        /// <param name="coefficients">Coefficients of linear combination.</param>
        /// <param name="res">Output parameter where gradient is written to.</param>
        public void LinearCombinationDerivative(IVectorFunctionResults evaluationData, 
            IVector coefficients, ref IVector res)
        {
            if (evaluationData == null)
                throw new ArgumentException("Linear combination Gradient: Evaluation object containing parameters is null.");
            IVector parameters = evaluationData.Parameters;
            if (parameters == null)
                throw new ArgumentException("Linear combination Gradient: Parameters are not specified on evaluaton object.");
            if (coefficients == null)
                throw new ArgumentException("Linear combination Gradient: Coefficients are not specified.");
            int dim = parameters.Length;
            int numTerms = coefficients.Length;
            if (res == null)
                res = new Vector(dim);
            else if (res.Length!= dim)
                res = new Vector(dim);
            for (int i = 0; i < dim; ++i)
                    res[i] = 0.0;
            for (int which = 0; which < numTerms; ++which)
                for (int i = 0; i < dim; ++i)
                        res[i] += Derivative(evaluationData, which, i);

        }

        /// <summary>Returns the specified component of gradient of  combination of functions 
        /// contained in this vector function, with specified coefficients at specified parameters.</summary>
        /// <param name="evaluationData">Data used for evaluation that also contains parameters
        /// (in evaluationData.Parameters)</param>
        /// <param name="coefficients">Coefficients of linear combination.</param>
        /// <param name="component">Specifies which gradient component to return.</param>
        public double LinearCombinationDerivative(IVectorFunctionResults evaluationData, 
                IVector coefficients, int component)
        {
            double ret = 0;
            for (int which = 0; which < coefficients.Length; ++which)
                ret += coefficients[which] * Derivative(evaluationData, which, component);
            return ret;
        }

        public void LinearCombinationSecondDerivative(IVectorFunctionResults evaluationData, IVector coefficients,
            ref IMatrix res)
        {
            if (evaluationData==null)
                throw new ArgumentException("Linear combination Hessian: Evaluation object containing parameters is null.");
            IVector parameters = evaluationData.Parameters;
            if (parameters==null)
                throw new ArgumentException("Linear combination Hessian: Parameters are not specified on evaluaton object.");
            if (coefficients == null)
                throw new ArgumentException("Linear combination Hessian: Coefficients are not specified.");
            int dim = parameters.Length;
            int numTerms = coefficients.Length;
            if (res==null)
                res = new Matrix(dim, dim);
            else if (res.RowCount!=dim || res.ColumnCount!=dim)
                res = new Matrix(dim, dim);
            for (int i=0; i<dim; ++i)
                for (int j=0;j<dim;++j)
                    res[i,j]=0.0;
            for (int which = 0; which<numTerms; ++which)
                for (int i=0; i<dim; ++i)
                    for (int j=0;j<dim;++j)
                        res[i,j] += SecondDerivative(evaluationData, which, i, j);
        }


        /// <summary>Returns the specified component of Hessian of  combination of functions 
        /// contained in this vector function, with specified coefficients at specified parameters.</summary>
        /// <param name="evaluationData">Data used for evaluation that also contains parameters
        /// (in evaluationData.Parameters)</param>
        /// <param name="coefficients">Coefficients of linear combination.</param>
        /// <param name="rowNum">Row number of the returned component.</param>
        /// <param name="columnNum">Column number of the returned component.</param>
        public double LinearCombinationSecondDerivative(IVectorFunctionResults evaluationData, 
            IVector coefficients, int rowNum, int columnNum)
        {
            double ret = 0;
            for (int which = 0; which < coefficients.Length; ++which)
                ret += coefficients[which] * SecondDerivative(evaluationData, which, 
                    rowNum, columnNum);
            return ret;
        }



        /// <summary>Returns value of linear combination of functions contained in this vector
        /// function, with specified coefficients at specified parameters.</summary>
        /// <param name="parameters">Parameters where functions are evaluated.</param>
        /// <param name="coefficients">Coefficients of linear combination.</param>
        public double LinearCombinationValue(IVector parameters, IVector coefficients)
        {
            double ret = 0;
            for (int which = 0; which < coefficients.Length; ++which)
                ret += coefficients[which] * Value(parameters, which);
            return ret;
        }

        /// <summary>Returns gradient of linear combination of functions contained in this vector
        /// function, with specified coefficients at specified parameters.</summary>
        /// <param name="parameters">Parameters where functions are evaluated.</param>
        /// <param name="coefficients">Coefficients of linear combination.</param>
        /// <param name="res">Output parameter where gradient is written to.</param>
        public void LinearCombinationDerivative(IVector parameters,
            IVector coefficients, ref IVector res)
        {
            if (parameters == null)
                throw new ArgumentException("Linear combination Gradient: Parameters are not specified (null reference).");
            if (coefficients == null)
                throw new ArgumentException("Linear combination Gradient: Coefficients are not specified.");
            int dim = parameters.Length;
            int numTerms = coefficients.Length;
            if (res == null)
                res = new Vector(dim);
            else if (res.Length != dim)
                res = new Vector(dim);
            for (int i = 0; i < dim; ++i)
                res[i] = 0.0;
            for (int which = 0; which < numTerms; ++which)
                for (int i = 0; i < dim; ++i)
                    res[i] += Derivative(parameters, which, i);

        }

        /// <summary>Returns the specified component of gradient of  combination of functions 
        /// contained in this vector function, with specified coefficients at specified parameters.</summary>
        /// <param name="parameters">Parameters where functions are evaluated.</param>
        /// <param name="coefficients">Coefficients of linear combination.</param>
        /// <param name="component">Specifies which gradient component to return.</param>
        public double LinearCombinationDerivative(IVector parameters,
                IVector coefficients, int component)
        {
            double ret = 0;
            for (int which = 0; which < coefficients.Length; ++which)
                ret += coefficients[which] * Derivative(parameters, which, component);
            return ret;
        }

        /// <summary>Calculates second derivatives of the linear combination of components of vector functions and 
        /// stores them to the specified matrix.</summary>
        /// <param name="parameters">Parameters where linear combination second derivative is evaluated.</param>
        /// <param name="coefficients">Coefficients of the linear combination.</param>
        /// <param name="res">Matrix where second derivatives of the linear combination are stored.</param>
        public void LinearCombinationSecondDerivative(IVector parameters, IVector coefficients,
            ref IMatrix res)
        {
            if (parameters == null)
                throw new ArgumentException("Linear combination Hessian: Parameters are not specified (null reference).");
            if (coefficients == null)
                throw new ArgumentException("Linear combination Hessian: Coefficients are not specified.");
            int dim = parameters.Length;
            int numTerms = coefficients.Length;
            if (res == null)
                res = new Matrix(dim, dim);
            else if (res.RowCount != dim || res.ColumnCount != dim)
                res = new Matrix(dim, dim);
            for (int i = 0; i < dim; ++i)
                for (int j = 0; j < dim; ++j)
                    res[i, j] = 0.0;
            for (int which = 0; which < numTerms; ++which)
                for (int i = 0; i < dim; ++i)
                    for (int j = 0; j < dim; ++j)
                        res[i, j] += SecondDerivative(parameters, which, i, j);
        }


        /// <summary>Returns the specified component of Hessian of  combination of functions 
        /// contained in this vector function, with specified coefficients at specified parameters.</summary>
        /// <param name="parameters">Parameters where functions are evaluated.</param>
        /// <param name="coefficients">Coefficients of linear combination.</param>
        /// <param name="rowNum">Row number of the returned component.</param>
        /// <param name="columnNum">Column number of the returned component.</param>
        public double LinearCombinationSecondDerivative(IVector parameters,
            IVector coefficients, int rowNum, int columnNum)
        {
            double ret = 0;
            for (int which = 0; which < coefficients.Length; ++which)
                ret += coefficients[which] * SecondDerivative(parameters, which,
                    rowNum, columnNum);
            return ret;
        }


        #endregion LinearCombinations



        #region ResultStore

        /// <summary>Storage for <see cref="IVectorFunctionResults"/> objects.</summary>
        protected class ObjectStoreResults : ObjectStore<IVectorFunctionResults>,
                IObjectStore<IVectorFunctionResults>
        {
            private ObjectStoreResults() { }

            public ObjectStoreResults(IVectorFunction vecfunc)
            {
                this._vecfunc = vecfunc;
            }

            protected IVectorFunction _vecfunc;



            protected override IVectorFunctionResults TryGetNew()
            {
                // Only generate new vector function results if the current function has specified dimension of input and output space:
                if (_vecfunc.NumParameters > 0 && _vecfunc.NumValues > 0)
                {
                    return new VectorFunctionResults(_vecfunc.NumParameters, _vecfunc.NumValues);
                }
                else
                    return null;
            }

            public override bool  TryStore(IVectorFunctionResults obj)
            {
                if (obj!=null)
                {
                    obj.NullifyAll();
                }
                return base.TryStore(obj);
            }

        }


        private volatile VectorFunctionBase.ObjectStoreResults _resultStore;

        /// <summary>Store of result objects for reuse.</summary>
        protected VectorFunctionBase.ObjectStoreResults ResultsStore
        {
            get {
                if (_resultStore == null)
                {
                    lock(Lock)
                    {
                        if (_resultStore == null)
                            _resultStore = new VectorFunctionBase.ObjectStoreResults(this);
                    }
                }
                return _resultStore;
            }
        }




        #endregion ResultStore


        #region Numeric


        // TODO: implement this!
        /// <summary>Calculates numerical derivative of this function. Central difference formula is used.</summary>
        /// <param name="x">Point at which derivative is calculated.</param>
        /// <param name="stepSize">Step size.</param>
        /// <param name="derivative">Numerical derivative.</param>
        public virtual void NumericalDerivative(IVector x, IVector stepSize, ref List<IVector> derivative)
        {
            // return Numeric.DerivativeCD(Value, x, stepsize);
            throw new NotSupportedException("Function " + Name + ": Evaluation of second order derivative is not supported.");
        }

        // TODO: implement this!
        /// <summary>Calculates numerical second order derivative of this function. Central difference formula is used.</summary>
        /// <param name="x">Point at which second order derivative is calculated.</param>
        /// <param name="stepsizes">Step size.</param>
        /// <param name="secondDerivatives">List of vectrs where the calculated second derivatives are stored.</param>
        /// <returns>Numerical derivative.</returns>
        public virtual void NumericalSecondDerivative(IVector x, IVector stepsizes, ref List<IVector> secondDerivatives)
        {
            // return Numeric.SecondDerivativeCD(Value, x, stepsize);
            throw new NotSupportedException("Function " + Name + ": Evaluation of second order derivative is not supported.");
        }

        #endregion Numeric


        #region Testing

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


    }


}
