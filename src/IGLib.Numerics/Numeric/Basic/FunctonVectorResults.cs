
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{


    /// <summary>Vector function results.
    /// Used to transfer parameters input (e.g. vector of parameters, request flags)
    /// to the vector function and to store function output results (e.g. values, their gradients, 
    /// error codes, and flags indicating what has actually been calculated).
    /// REMARKS:
    ///   Property CopyReferences specifies whether only references are copied when individial object
    /// fields are assigned and set (when the property is true), or values are actually copied
    /// (when false - deep copy). Each setter method also has the variant that always copies only
    /// the reference (function name appended by "Reference"). This makes possible to avoid duplication
    /// of allocated data and also to avoid having different data with the same references.
    ///   In the beginning of analysis functions, call ResetResults().</summary>
    /// $A Igor xx Apr10;
    public class VectorFunctionResults : IVectorFunctionResults
    {

        #region Constructors

        // TODO: VERIFY AND ELABORATE CONSTRUCTORS!!!!

        /// <summary>1 parameter, 1 function. 
        /// No gradients required</summary>
        public VectorFunctionResults()
        { }

        /// <summary>1 parameter, 1 function, gradients required.</summary>
        /// <param name="reqGradients">Whether gradient of the function gradient is required.</param>
        public VectorFunctionResults(bool reqGradients)
        { }

        /// <summary>Specified number of parameters, 1 function.
        /// No gradients required.</summary>
        /// <param name="numParameters">Number of parameters.</param>
        public VectorFunctionResults(int numParameters)
        {
            this.NumParameters = numParameters;
        }

        /// <summary>Specified number of parameters, 1 function.
        /// No gradients required.</summary>
        /// <param name="numParameters">Number of parameters.</param>
        /// <param name="reqGradients">Whether gradients are requested.</param>
        public VectorFunctionResults(int numParameters, bool reqGradients)
        {
            this.NumParameters = numParameters;
            this.ReqGradients = reqGradients;
        }

        /// <summary>Specified number of parameters and functions.
        /// No gradients required.</summary>
        /// <param name="numParameters">Number of parameters.</param>
        /// <param name="numFunctions">Number of functions.</param>
        public VectorFunctionResults(int numParameters, int numFunctions)
        {
            this.NumParameters = numParameters;
            this.NumFunctions = numFunctions;
        }

        /// <summary>Specified number of parameters and functions.
        /// Gradients may be required, dependent on teh <paramref name="reqGradients"/>.</summary>
        /// <param name="numParameters">Number of parameters.</param>
        /// <param name="numFunctions">Number of functions.</param>
        /// <param name="reqGradients">Whether gradients are required.</param>
        public VectorFunctionResults(int numParameters, int numFunctions, bool reqGradients)
        {
            this.NumParameters = numParameters;
            this.NumFunctions = numFunctions;
            this.ReqValues = true;
            this.ReqGradients = reqGradients;
        }

        #endregion Constructors


        #region InternalData


        protected int _numParameters = 1;

        protected int _numFunctions = 0;


        protected IVector _parameters;

        protected List<double> _values;

        protected List<IVector> _gradients;

        protected List<IMatrix> _hessians;



        protected bool _copyReferences = false;


        protected bool _reqvalues = true;

        protected bool _reqGradients = false;

        protected bool _reqHessians = false;

        protected int _errorCode = 0;

        protected string _errorString = null;

        protected bool _calcValues = false;

        protected bool _calcGradients = false;

        protected bool _calcHessians = false;



        #endregion InternalData


        #region Characteristics

        // CHARACTERISTICS (DIMENSIONS) OF THE VECTOR FUNCTION:

        /// <summary>Number of parameters.</summary>
        public virtual int NumParameters
        {
            get { return _numParameters; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Number of parameters can not be less than 1.");
                //if (value != _numParameters)
                //{
                //    Parameters = null;
                //    CalculatedGradients = false;
                //    CalculatedHessians = false;
                //}
                _numParameters = value;
            }
        }

        /// <summary>Number of functions.</summary>
        public virtual int NumFunctions
        {
            get { return _numFunctions; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Number of functions can not be less than 0.");
                //if (value != _numFunctions)
                //{
                //    CalculatedValues = false;
                //    CalculatedGradients = false;
                //    CalculatedHessians = false;
                //}
                _numFunctions = value;
            }
        }


        #endregion Characteristics

        #region Operation

        // OPERATION PARAMETERS:

        /// <summary>Indicates whether just references can be copied when setting optimization
        /// parameters or results.
        /// If false then deep copy is always be performed.
        /// Default is false.</summary>
        /// 
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
                    IVector tmpParameters = Parameters;
                    Parameters = null;
                    Parameters = tmpParameters;
                    List<double> tmpValues = Values;
                    Values = null;
                    Values = tmpValues;
                    List<IVector> tmpGradients = Gradients;
                    Gradients = null;
                    Gradients = tmpGradients;
                    List<IMatrix> tmpHessians = Hessians;
                    Hessians = null;
                    Hessians = tmpHessians;
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
        /// <param name="reference">Reference to be assigned.</param>
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
            AllocateParameters();  // this will execute Calculated=false
            Parameters[index] = value;
        }

        #endregion Parameters

        #region Results

        // OPTIMIZATION RESULTS:



        // FUNCTION VALUES:

        /// <summary>Function values.
        /// If CopyReferences=true (false by default) then only the list reference is copied in assignments.</summary>
        public virtual List<double> Values
        {
            get { return _values; }
            set
            {
                if (CopyReferences || value == null)
                    _values = value;
                else
                {
                    AllocateValuesList();
                    for (int which = 0; which < NumFunctions; ++which)
                        _values[which] = value[which];
                }
            }
        }

        /// <summary>Returns a list of function values.</summary>
        public virtual List<double> GetValues()
        { return Values; }

        /// <summary>Sets the list of function values.
        /// If CopyReferences=true (false by default) then only the list reference is copied.</summary>
        /// <param name="values">Values of functions.</param>
        public virtual void SetValues(List<double> values)
        { Values = values; }

        /// <summary>Sets the list of function values.
        /// Only the list reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        public virtual void SetValuesReference(List<double> reference)
        { _values = reference; }

        /// <summary>Returns the specified function value.</summary>
        /// <param name="which">Specifies which function to return (counting from 0).</param>
        public virtual double GetValue(int which)
        { return Values[which]; }


        /// <summary>Sets the specified function value.</summary>
        /// <param name="which">Specifies which function is set (counting from 0).</param>
        /// <param name="value">Assigned value of the function.</param>
        public virtual void SetValue(int which, double value)
        {
            AllocateValuesList();
            Values[which] = value;
        }


        // FUNCTION GRADIENTS:

        /// <summary>Function gradients.
        /// If CopyReferences=true (false by default) then only the list reference is copied in assignments.</summary>
        public virtual List<IVector> Gradients
        {
            get { return _gradients; }
            set
            {
                if (CopyReferences || value == null)
                    _gradients = value;
                else
                {
                    AllocateGradientsList();
                    for (int which = 0; which < NumFunctions; ++which)
                    {
                        if (value[which] == null)
                            _gradients[which] = null;
                        else
                            _gradients[which] = value[which].GetCopy();
                    }
                }
            }
        }

        /// <summary>Returns a list of function gradients.</summary>
        public virtual List<IVector> GetGradients()
        { return Gradients; }

        /// <summary>Sets function gradients.
        /// If CopyReferences=true (false by default) then only the list reference is copied.</summary>
        /// <param name="values">Value to be assigned.</param>
        public virtual void SetGradients(List<IVector> values)
        { Gradients = values; }

        /// <summary>Sets function gradients.
        /// Only the list reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        public virtual void SetGradientsReference(List<IVector> reference)
        { _gradients = reference; }

        /// <summary>Returns the gradient of the specified function.</summary>
        /// <param name="which">Specifies which function to take (couonted from 0).</param>
        public virtual IVector GetGradient(int which)
        { return Gradients[which]; }


        /// <summary>Returns the specific function gradient component.</summary>
        /// <param name="which">Specifies which function to take (couonted from 0).</param>
        /// <param name="index">Specifies gradient component (conted form 0).</param>
        public virtual double GetGradient(int which, int index)
        { return Gradients[which][index]; }

        /// <summary>Sets the specified function gradient.
        /// If CopyReferences=true (false by default) then only the reference is copied.</summary>
        /// <param name="which">Specifies which function to take (couonted from 0).</param>
        /// <param name="value">Value of the gradient to be assigned.</param>
        public virtual void SetGradient(int which, IVector value)
        {
            AllocateGradientsList();
            if (CopyReferences || value == null)
                _gradients[which] = value;
            else
            {
                _gradients[which] = value.GetCopy();
            }
        }

        /// <summary>Sets the specified function gradient.
        /// Only the reference is copied.</summary>
        /// <param name="which">Specifies which function to take (couonted from 0).</param>
        /// <param name="reference">Gradient reference to be assigned.</param>
        public virtual void SetGradientReference(int which, IVector reference)
        {
            AllocateGradientsList();
            _gradients[which] = reference;
        }

        /// <summary>Sets the specified fuction gradient component.</summary>
        /// <param name="which">Specifies which function to take (couonted from 0).</param>
        /// <param name="index">Specified index of gradient coponent to be set.</param>
        /// <param name="value">Value to be assigned to the specified component.</param>
        public virtual void SetGradient(int which, int index, double value)
        {
            AllocateGradient(which);
            Gradients[which][index] = value;
        }



        // FUNCTION HESSIANS:

        /// <summary>Functions' Hessians (matrices of second derivatives).
        /// If CopyReferences=true (false by default) then only the list reference is copied in assignments.</summary>
        public virtual List<IMatrix> Hessians
        {
            get { return _hessians; }
            set
            {
                if (CopyReferences || value == null)
                    _hessians = value;
                else
                {
                    AllocateHessiansList();
                    for (int which = 0; which < NumFunctions; ++which)
                    {
                        if (value[which] == null)
                            _hessians[which] = null;
                        else
                            _hessians[which] = value[which].GetCopy();
                    }
                }
            }
        }

        /// <summary>Returns the list of functions' Hessians.</summary>
        public virtual List<IMatrix> GetHessians()
        { return Hessians; }

        /// <summary>Sets functios' Hessians.
        /// If CopyReferences=true (false by default) then only the list reference is copied.</summary>
        /// <param name="values">List of Hessians to be assigned.</param>
        public virtual void SetHessians(List<IMatrix> values)
        { Hessians = values; }

        /// <summary>Sets functios' Hessians.
        /// Only the list reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        public virtual void SetHessiansReference(List<IMatrix> reference)
        { _hessians = reference; }

        /// <summary>Returns Hessian of the specified function.</summary>
        /// <param name="which">Specifies which function it applies to (counting from 0).</param>
        public virtual IMatrix GetHessian(int which)
        { return Hessians[which]; }

        /// <summary>Returns the specified component of Hessian of the specified function.</summary>
        /// <param name="which">Specifies which function it applies to (counting from 0).</param>
        /// <param name="rowIndex">Row index of the component (counting from 0).</param>
        /// <param name="columnIndex">Column index of the component (counting from 0).</param>
        public virtual double GetHessian(int which, int rowIndex, int columnIndex)
        { return Hessians[which][rowIndex, columnIndex]; }

        /// <summary>Sets the specified function's Hessian.
        /// If CopyReferences=true (false by default) then only the reference is copied.</summary>
        /// <param name="which">Specifies which function it applies to (counting from 0).</param>
        /// <param name="value">Hessian matrix to be assigned.</param>
        public virtual void SetHessian(int which, IMatrix value)
        {
            AllocateHessiansList();
            if (CopyReferences || value == null)
                Hessians[which] = value;
            else
            {
                Hessians[which] = value.GetCopy();
            }
        }

        /// <summary>Sets the specified function's Hessian.
        /// Only the reference is copied.</summary>
        /// <param name="which">Specifies which function it applies to (counting from 0).</param>
        /// <param name="reference">Hessian matrix reference to be assigned.</param>
        public virtual void SetHessianReference(int which, IMatrix reference)
        {
            AllocateHessiansList();
            Hessians[which] = reference;
        }

        /// <summary>Sets the specified component of the specified function's Hessian.</summary>
        /// <param name="which">Specifies which function it applies to (counting from 0).</param>
        /// <param name="rowIndex">Row index of the component (counting from 0).</param>
        /// <param name="columnIndex">Column index of the component (counting from 0).</param>
        /// <param name="value">Value to which the specified Hessian elemennt is set.</param>
        public virtual void SetHessian(int which, int rowIndex, int columnIndex, double value)
        {
            AllocateHessian(which);
            Hessians[which][rowIndex, columnIndex] = value;
        }

        #endregion Results

        #region Allocation

        // ALLOCATION OF FIELDS:

        /// <summary>Sets all objects (parameters and result objects) to null.
        /// If the references are not assigned elsewhere, these objects become eligible for garbage collection.</summary>
        public virtual void NullifyAll()
        {
            Parameters = null;
            Values = null;
            Gradients = null;
            Hessians = null;
        }

        /// <summary>Sets all result objects to null.
        /// If the references are not assigned elsewhere, these objects become eligible for garbage collection.</summary>
        public virtual void NullifyResults()
        {
            Values = null;
            Gradients = null;
            Hessians = null;
        }


        /// <summary>Allocates space for parameters.</summary>
        public virtual void AllocateParameters()
        {
            Calculated = true;
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
            if (ReqValues)
                AllocateValuesList();
            if (ReqGradients)
                AllocateGradients();
            if (ReqHessians)
                AllocateHessians();

        }



        /// <summary>Allocates space for list of function values.</summary>
        public virtual void AllocateValuesList()
        {
            Util.ResizeList(ref _values, NumFunctions, 0.0, true /* ReduceCapacity */);
        }

        /// <summary>Allocates space for list of function gradients.</summary>
        public virtual void AllocateGradientsList()
        {
            Util.ResizeList(ref _gradients, NumFunctions, null, true /* ReduceCapacity */);
        }

        /// <summary>Allocates space for function gradients (including for the list, if necessarty).</summary>
        public virtual void AllocateGradients()
        {
            AllocateGradientsList();
            for (int which = 0; which < NumFunctions; ++which)
            {
                IVector v = _gradients[which];
                if (v == null)
                    _gradients[which] = new Vector(NumParameters);
                else
                {
                    if (v.Length != NumParameters)
                        _gradients[which] = new Vector(NumParameters);
                }
            }
        }

        /// <summary>Allocates space for the specified function gradient.</summary>
        /// <param name="which">Specifies which function it applies to (countinf form 0).</param>
        public virtual void AllocateGradient(int which)
        {
            AllocateGradientsList();
            IVector v = _gradients[which];
            if (v == null)
                _gradients[which] = new Vector(NumParameters);
            else
            {
                if (v.Length != NumParameters)
                    _gradients[which] = new Vector(NumParameters);
            }
        }

        /// <summary>Allocates space for the list of functions' Hessians.</summary>
        public virtual void AllocateHessiansList()
        {
            Util.ResizeList(ref _hessians, NumFunctions, null, true /* ReduceCapacity */);
        }

        /// <summary>Allocates space for functions' Hessians (including space for the list, if necessary).</summary>
        public virtual void AllocateHessians()
        {
            AllocateHessiansList();
            for (int which = 0; which < NumFunctions; ++which)
            {
                IMatrix m = _hessians[which];
                if (m == null)
                    _hessians[which] = new Matrix(NumParameters, NumParameters);
                else
                {
                    if (m.RowCount != NumParameters || m.ColumnCount != NumParameters)
                        _hessians[which] = new Matrix(NumParameters, NumParameters);
                }
            }
        }

        /// <summary>Allocates space for the specified fucnction's Hessian.</summary>
        /// <param name="which">Specifies which function it applies to (conting form 0).</param>
        public virtual void AllocateHessian(int which)
        {
            AllocateHessiansList();
            IMatrix m = _hessians[which];
            if (m == null)
                _hessians[which] = new Matrix(NumParameters, NumParameters);
            else
            {
                if (m.RowCount != NumParameters || m.ColumnCount != NumParameters)
                    _hessians[which] = new Matrix(NumParameters, NumParameters);
            }
        }

        #endregion Allocation

        #region Flags

        // REQUEST FLAGS:


        /// <summary>Indicates whether calculation of functions is/was requested.</summary>
        public virtual bool ReqValues
        { get { return _reqvalues; } set { _reqvalues = value; } }

        /// <summary>Indicates whether calculation of function gradients is/was requested.</summary>
        public virtual bool ReqGradients
        { get { return _reqGradients; } set { _reqGradients = value; } }

        /// <summary>Indicates whether calculation of functions' Hessians is/was requested.</summary>
        public virtual bool ReqHessians
        { get { return _reqHessians; } set { _reqHessians = value; } }


        // CALCULATED FLAGS:

        /// <summary>Sets all calculated flags to false, error code to 0 (no error) and error string to null.</summary>
        public virtual void ResetResults()
        {
            CalculatedValues = false;
            CalculatedGradients = false;
            CalculatedHessians = false;
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
        public virtual bool Calculated
        {
            get
            {
                return (
                    (!ReqValues || CalculatedValues) &&
                    (!ReqGradients || CalculatedGradients) &&
                    (!ReqHessians || CalculatedHessians)
                  );
            }
            set
            {
                if (value == false)
                {
                    CalculatedValues = false;
                    CalculatedGradients = false;
                    CalculatedHessians = false;
                }
                else
                {
                    if (ReqValues == true)
                        CalculatedValues = true;
                    if (ReqGradients == true)
                        CalculatedGradients = true;
                    if (ReqHessians)
                        CalculatedHessians = true;
                }
            }
        }

        /// <summary>Indicates whether calculation of functions is/was requested.</summary>
        public virtual bool CalculatedValues
        { get { return _calcValues; } set { _calcValues = value; } }

        /// <summary>Indicates whether calculation of functions' gradients is/was requested.</summary>
        public virtual bool CalculatedGradients
        { get { return _calcGradients; } set { _calcGradients = value; } }

        /// <summary>Indicates whether calculation of functions' Hessian is/was requested.</summary>
        public virtual bool CalculatedHessians
        { get { return _calcHessians; } set { _calcHessians = value; } }

        #endregion Flags

        #region HelperFunctions


        /// <summary>Copies data from another vector function results.</summary>
        /// <param name="results">Vector function results which data is copied from.</param>
        public virtual void Copy(IVectorFunctionResults results)
        {
            // TODO: cack if something is missing here!

            this.CopyReferences = false;
            this.NumParameters = results.NumParameters;
            this.NumFunctions = results.NumFunctions;

            this.Parameters = results.Parameters;
            this.Values = results.Values;
            this.Gradients = results.Gradients;
            this.Hessians = results.Hessians;

            this.ReqValues = results.ReqValues;
            this.ReqGradients = results.ReqGradients;
            this.ReqHessians = results.ReqHessians;

            this.CalculatedValues = results.CalculatedValues;
            this.CalculatedGradients = results.CalculatedGradients;
            this.CalculatedHessians = results.CalculatedHessians;

            this.ErrorCode = results.ErrorCode;
            this.ErrorString = results.ErrorString;

        }  // Copy()

        /// <summary>Returns an exact deep copy of the current object.</summary>
        public virtual IVectorFunctionResults GetCopy()
        {
            IVectorFunctionResults ret = new VectorFunctionResults();
            ret.Copy(this);
            return ret;
        } 

        #endregion HelperFunctons

        #region InputOutput

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Vector function results structure: ");
            sb.AppendLine("Request flags: values: " + ReqValues + ", gradients: "
                    + ReqGradients + ", hessians: " + ReqHessians);
            sb.AppendLine("Calculation flags: values: " + CalculatedValues + ", gradients: "
                    + CalculatedGradients + ", hessians: " + CalculatedHessians);
            sb.AppendLine("Error code: " + ErrorCode);
            sb.AppendLine("Error string: " + ErrorString);
            sb.AppendLine("Vector of parameters: ");
            sb.AppendLine(Parameters.ToString());
            sb.AppendLine("Values: ");
            sb.AppendLine(Util.ListToString(Values));
            sb.AppendLine("Gradients: ");
            sb.AppendLine(Util.ListToString(Gradients));
            sb.AppendLine("Hessians: ");
            sb.AppendLine(Util.ListToString(Hessians));
            return sb.ToString();
        }


        #endregion


    }  // class VectorFunctionResults



}
