
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{

 
    /// <summary>Storage for results of a scalar function.
    /// Includes parameters at which function was (or should be) evaluated, flags specifying what has been and what 
    /// should be evaluated, calculated value, gradient and Hessian of the function in the specified point.</summary>
    /// $A Igor xx Dec09 Nov10;
    public class ScalarFunctionResults : IScalarFunctionResults
    {


        #region Constructors

        // TODO: VERIFY AND ELABORATE CONSTRUCTORS!!!!

        /// <summary>1 parameter, no constraints, 1 objective function.
        /// No gradients required</summary>
        public ScalarFunctionResults()
        {  }

        /// <summary>1 parameter, no constraints, 1 objective function.</summary>
        /// <param name="reqGradients">Whether gradient of the objective function is required.</param>
        public ScalarFunctionResults(bool reqGradients)
        { }

        /// <summary>Specified number of parameters, 1 objective, no constraints.
        /// No gradients required.</summary>
        /// <param name="numParameters">Number of parameters.</param>
        public ScalarFunctionResults(int numParameters) 
        {
            this.NumParameters = numParameters;
        }

        /// <summary>Specified number of parameters, 1 objective, no constraints.
        /// No gradients required.</summary>
        /// <param name="numParameters">Number of parameters.</param>
        /// <param name="reqGradients">Whether gradients are requested.</param>
        public ScalarFunctionResults(int numParameters, bool reqGradients)
        {
            this.NumParameters = numParameters;
            this.ReqGradient = true;
        }


        #endregion Constructors


        #region InternalData


        protected int _numParameters = 1;
        protected IVector _parameters;

        protected double _value;
        protected IVector _Gradient;
        protected IMatrix _Hessian;

        protected bool _copyReferences = false;

        protected bool _reqValue = true;
        protected bool _reqGradient = false;
        protected bool _reqHessian = false;

        protected int _errorCode = 0;
        protected string _errorString = null;

        protected bool _calcValue = false;
        protected bool _calcGradient = false;
        protected bool _calcHessian = false;


        #region TODELETE

        protected int _numObjectives = 1;

        protected int _numConstraints = 0;

        protected int _numEqualityConstraints = 0;




        protected List<double> _constraints;

        protected List<IVector> _constraintGradients;

        protected List<IMatrix> _constraintHessians;



        protected bool _reqConstraints = true;

        protected bool _reqConstraintGradients = false;

        protected bool _reqConstraintHessians = false;

        protected bool _calcConstraints = false;

        protected bool _calcConstraintGradients = false;

        protected bool _calcConstraintHessians = false;

        #endregion TODELETE



        #endregion InternalData


        #region Characteristics

        // CHARACTERISTICS (DIMENSIONS) OF THE OPTIMIZATION PROBLEM:

        /// <summary>Number of parameters.</summary>
        public virtual int NumParameters 
        { 
            get { return _numParameters; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Number of parameters can not be less than 1.");
                if (value != _numParameters)
                {
                    Parameters = null;
                    // TODO: Consider why not recetting the CalculatedObjective and CalculatedConstraints flags?
                    // And why this is necessary in the first place??
                    CalculatedGradient = false;
                    CalculatedHessian = false;
                }
                _numParameters = value;
            }
        }


        #endregion Characteristics


        #region Operation

        // OPERATION PARAMETERS:

        /// <summary>Indicates whether just references can be copied when setting function
        /// parameters or results.
        /// If false then deep copy is always performed.
        /// Default is false.</summary>
        public virtual bool CopyReferences
        {
            get { return _copyReferences; }
            set
            {
                bool previous = _copyReferences;
                _copyReferences = value;
                if (previous == true && value == false)
                {
                    // If we have copied references before, but now we copy values, we must
                    // make copies of all previous objects (we can do this simply by assignment
                    // because assignment will now perform deep copying rather than just reference copying):

                    Copy(this);
 
                }
            }
        }

        #endregion Operation


        #region Parameters

        // OPTIMIZATION PARAMETERS:

        /// <summary>Optimization parameters.
        /// If CopyReferences=true (false by default) then only the reference is copied when assigning.</summary>
        public virtual IVector Parameters 
        {
            get { return _parameters; }
            set
            {
                Calculated = false;
                if (CopyReferences || value == null)
                    _parameters = value;
                else
                    _parameters = value.GetCopy();
                if (value != null)
                    NumParameters = value.Length;
            }
        }

        /// <summary>Returns vector of optimization parameters.</summary>
        public virtual IVector GetParameters()
        { return Parameters; }

        /// <summary>Sets the vector of optimization parameters.
        /// If CopyReferences=true (false by default) then only the reference is copied.</summary>
        /// <param name="value">Value to be assigned.</param>
        public virtual void SetParameters(IVector value)
        { Parameters = value; }

        /// <summary>Sets the vector of optimization parameters.
        /// Only the reference is copied.</summary>
        /// <param name="reference">Vector reference to be assigned.</param>
        public virtual void SetParametersReference(IVector reference)
        {
            Calculated = false;
            _parameters = reference;
            if (reference != null)
                NumParameters = reference.Length;
        }

        /// <summary>Returns specific optimization parameter.
        /// Throws exception if not defined or index out of bounds.</summary>
        /// <param name="index">Index of parameter to be returned (counting from 0).</param>
        public virtual double GetParameter(int index)
        { return Parameters[index]; }

        /// <summary>Sets the specified optimization parameter.</summary>
        /// <param name="index">Index of parameter to be set (counting from 0).</param>
        /// <param name="value">Parameter value.</param>
        public virtual void SetParameter(int index, double value)
        {
            AllocateParameters();  // this performs Calculated=false
            Parameters[index] = value;
        }

        #endregion Parameters


        #region Results

        // FUNCTION RESULTS:

        // VALUE:

        /// <summary>Value of the function.</summary>
        public virtual double Value
        { get { return _value; } set { _value = value; } }

        /// <summary>Returns the value of the function.</summary>
        public virtual double GetValue()
        { return Value; }

        /// <summary>Sets the value of the function.</summary>
        /// <param name="value">Value to be assigned to the objective function.</param>
        public virtual void SetValue(double value)
        { Value = value; }


        // FUNCTION GRADIENT:

        /// <summary>Function gradient.
        /// If CopyReferences=true (false by default) then only the reference is copied in assignments.</summary>
        public virtual IVector Gradient 
        {
            get { return _Gradient; }
            set
            {
                if (CopyReferences || value == null)
                    _Gradient = value;
                else
                    _Gradient = value.GetCopy();
            }
        }

        /// <summary>Returns the function gradient.</summary>
        public virtual IVector GetGradient()
        { return Gradient; }

        /// <summary>Sets the function gradient.
        /// If CopyReferences=true (false by default) then only the reference is copied.</summary>
        /// <param name="value">Value to be assigned.</param>
        public virtual void SetGradient(IVector value)
        { Gradient = value; }


        /// <summary>Sets the function gradient.
        /// Only the reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        public virtual void SetGradientReference(IVector reference)
        {
            _Gradient = reference;
        }

        /// <summary>Returns the specified component of the function gradient.</summary>
        /// <param name="index">Index of the component.</param>
        public virtual double GetGradient(int index)
        { return Gradient[index]; }

        /// <summary>Sets the specified component of the function gradient.</summary>
        /// <param name="index">Index of gradient component to be set.</param>
        /// <param name="value">Value of the gradient component.</param>
        public virtual void SetGradient(int index, double value)
        {
            AllocateGradient();
            Gradient[index] = value; 
        }


        // FUNCTION HESSIAN:

        /// <summary>Function Hessian (matrix of second derivatives).
        /// If CopyReferences=true (false by default) then only the reference is copied in assignments.</summary>
        public virtual IMatrix Hessian 
        {
            get { return _Hessian; }
            set
            {
                if (CopyReferences || value == null)
                    _Hessian = value;
                else
                {
                    _Hessian = value.GetCopy();
                }
            }
        }

        /// <summary>Returns the function's Hessian.</summary>
        public virtual IMatrix GetHessian()
        { return Hessian; }

        /// <summary>Sets the functions' Hessian.
        /// If CopyReferences=true (false by default) then only the reference is copied.</summary>
        /// <param name="value">Value to be assigned.</param>
        public virtual void SetHessian(IMatrix value)
        { Hessian = value; }

        /// <summary>Sets the functions' Hessian.
        /// Only the reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        public virtual void SetHessianReference(IMatrix reference)
        { _Hessian = reference; }

        /// <summary>Returns the specified component of the function's Hessian.</summary>
        /// <param name="rowIndex">Row index of the component (counting from 0).</param>
        /// <param name="columnIndex">Column index of the component (counting from 0).</param>
        public virtual double GetHessian(int rowIndex, int columnIndex)
        { return Hessian[rowIndex, columnIndex]; }

        /// <summary>Sets the specified component of the function's Hessian.</summary>
        /// <param name="rowIndex">Row index of the component (counting from 0).</param>
        /// <param name="columnIndex">Column index of the component (counting from 0).</param>
        /// <param name="value">Value to be assigned to Hessian.</param>
        public virtual void SetHessian(int rowIndex, int columnIndex, double value)
        {
            AllocateHessian();
            Hessian[rowIndex, columnIndex] = value;
        }


        #endregion Results

        #region Allocation

        // ALLOCATION OF FIELDS:

        /// <summary>Sets all objects (parameters and result objects) to null.
        /// If the references are not assigned elsewhere, these objects become eligible for garbage collection.</summary>
        public virtual void NullifyAll()
        {
            Parameters = null;
            Gradient = null;
            Hessian = null;
        }

        /// <summary>Sets all result objects to null.
        /// If the references are not assigned elsewhere, these objects become eligible for garbage collection.</summary>
        public virtual void NullifyResults()
        {
            Gradient = null;
            Hessian = null;
        }


        /// <summary>Allocates space for parameters.</summary>
        public virtual void AllocateParameters()
        {
            Calculated = false;
            if (_parameters == null)
                _parameters = new Vector(NumParameters);
            else
            {
                if (Parameters.Length != NumParameters)
                    _parameters = new Vector(NumParameters);
            }
        }


        /// <summary>Allocates space for all requested result objects.</summary>
        public virtual void AllocateRequested()
        {
            if (ReqGradient)
                AllocateGradient();
            if (ReqHessian)
                AllocateHessian();
        }


        /// <summary>Allocate space for function gradient.</summary>
        public virtual void AllocateGradient()
        {
            if (_Gradient == null)
                _Gradient = new Vector(NumParameters);
            else
            {
                if (_Gradient.Length!=NumParameters)
                    _Gradient = new Vector(NumParameters);
            }
        }

        /// <summary>Allocates space for function Hessian.</summary>
        public virtual void AllocateHessian()
        {
            if (_Hessian == null)
                _Hessian = new Matrix(NumParameters, NumParameters);
            else
            {
                if (_Hessian.RowCount != NumParameters ||
                    _Hessian.ColumnCount != NumParameters)
                    _Hessian = new Matrix(NumParameters, NumParameters);
            }
        }


        #endregion Allocation

        #region Flags

        // REQUEST FLAGS:

        /// <summary>Indicates whether calculation of function value is/was requested.</summary>
        public virtual bool ReqValue
        { get { return _reqValue; } set { _reqValue = value; } }

        /// <summary>Indicates whether calculation of function gradient is/was requested.</summary>
        public virtual bool ReqGradient
        { get { return _reqGradient; } set { _reqGradient = value; } }

        /// <summary>Indicates whether calculation of function Hessian is/was requested.</summary>
        public virtual bool ReqHessian
        { get { return _reqHessian; } set { _reqHessian = value; } }
        

        // CALCULATED FLAGS:

        /// <summary>Sets all calculated flags to false, error code to 0 (no error) and error string to null.</summary>
        public virtual void ResetResults()
        {
            CalculatedValue = false;
            CalculatedGradient = false;
            CalculatedHessian = false;
        }

        /// <summary>Error code.
        ///   0 - everything is OK.
        ///   negative value - something went wrong.</summary>
        public virtual int ErrorCode
        { get { return _errorCode; } set { _errorCode = value; } }

        /// <summary>Error string indicating what went wrong.</summary>
        public virtual String ErrorString
        { get { return _errorString; } set { _errorString = value; } }


        /// <summary>Collectively gets or sets calculated flags.
        /// Set false: all calculated flags are set to false.
        /// Set true: all calculated flags for which the corresponding request flags are true, 
        ///     are set to truee, others are set to false.
        /// Get: returns true if all the flags for which the corresponding request flags are true,
        ///     are also true. Otherwise returns false.
        /// </summary>
        public virtual  bool Calculated
        { 
            get {
                return ( 
                    (!ReqValue ||  CalculatedValue) &&
                    (!ReqGradient || CalculatedGradient) &&
                    (!ReqHessian || CalculatedHessian)
                  );
            }
            set
            {
                if (value == false)
                {
                    CalculatedValue = false;
                    CalculatedGradient = false;
                    CalculatedHessian = false;
                }
                else
                {
                    if (ReqValue == true)
                        CalculatedValue = true;
                    if (ReqGradient == true)
                        CalculatedGradient = true;
                    if (ReqHessian == true)
                        CalculatedHessian = true;
                }
            }
        }

        /// <summary>Indicates whether calculation of function value is/was requested.</summary>
        public virtual bool CalculatedValue
        { get { return _calcValue; } set { _calcValue = value;  } }

        /// <summary>Indicates whether calculation of function gradient is/was requested.</summary>
        public virtual bool CalculatedGradient
        { get { return _calcGradient; } set { _calcGradient = value; } }

        /// <summary>Indicates whether calculation of function Hessian is/was requested.</summary>
        public virtual bool CalculatedHessian
        { get { return _calcHessian; } set { _calcHessian = value; } }

        #endregion Flags

        #region Helper methods

        /// <summary>Copies data from another analysis results.</summary>
        /// <param name="results">Analysis results which data is copied from.</param>
        public virtual void Copy(IScalarFunctionResults results)
        {
            this.CopyReferences = false;
            this.NumParameters = results.NumParameters;

            this.Parameters = results.Parameters;

            this.Value = results.Value;
            this.Gradient = results.Gradient;
            this.Hessian = results.Hessian;

            this.ReqValue = results.ReqValue;
            this.ReqGradient = results.ReqGradient;
            this.ReqHessian = results.ReqHessian;
            this.CalculatedValue = results.CalculatedValue;
            this.CalculatedGradient = results.CalculatedGradient;
            this.CalculatedHessian = results.CalculatedHessian;

            this.ErrorCode = results.ErrorCode;
            this.ErrorString = results.ErrorString;

        }  // Copy()

        /// <summary>Returns an exact deep copy of the current object.</summary>
        public virtual IScalarFunctionResults GetCopy()
        {
            ScalarFunctionResults ret = new ScalarFunctionResults();
            ret.Copy(this);
            return ret;
        } 


        #endregion Helper methods



    }  // class ScalarFunctionResults


}
