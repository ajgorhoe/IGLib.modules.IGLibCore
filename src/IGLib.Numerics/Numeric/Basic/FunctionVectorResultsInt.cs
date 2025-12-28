// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public interface IVectorFunctionResults
    {

        #region Characteristics

        // CHARACTERISTICS (DIMENSIONS) OF THE VECTOR FUNCTION:

        /// <summary>Number of parameters.</summary>
        int NumParameters { get; set; }

        /// <summary>Number of functions.</summary>
        int NumFunctions { get; set; }

        #endregion Characteristics

        #region Operation

        // OPERATION PARAMETERS:

        /// <summary>Indicates whether just references can be copied when setting optimization
        /// parameters or results.
        /// If false then deep copy is always be performed.
        /// Default is false.</summary>
        bool CopyReferences { get; set; }

        #endregion Operation

        #region Parameters

        // FUNCTION PARAMETERS:

        /// <summary>Optimization parameters.
        /// If CopyReferences=true (false by default) then only the reference is copied when assigning.</summary>
        IVector Parameters { get; set; }

        /// <summary>Returns vector of optimization parameters.</summary>
        IVector GetParameters();

        /// <summary>Sets the vector of optimization parameters.
        /// If CopyReferences=true (false by default) then only the reference is copied.</summary>
        /// <param name="value">Value to be assigned.</param>
        void SetParameters(IVector value);

        /// <summary>Sets the vector of optimization parameters.
        /// Only the reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        void SetParametersReference(IVector reference);

        /// <summary>Returns specific optimization parameter.
        /// Throws exception if not defined or index out of bounds.</summary>
        /// <param name="index">Index of parameter to be returned (counting from 0).</param>
        double GetParameter(int index);

        /// <summary>Sets the specified optimization parameter.</summary>
        /// <param name="index">Index of parameter to be set (counting from 0).</param>
        /// <param name="value">Parameter value.</param>
        void SetParameter(int index, double value);

        #endregion Parameters

        #region Results

        // RESULTS:


        // FUNCTION VALUES:

        /// <summary>Function values.
        /// If CopyReferences=true (false by default) then only the list reference is copied in assignments.</summary>
        List<double> Values { get; set; }

        /// <summary>Returns a list of function values.</summary>
        List<double> GetValues();

        /// <summary>Sets the list of function values.
        /// If CopyReferences=true (false by default) then only the list reference is copied.</summary>
        /// <param name="values">Values of functions.</param>
        void SetValues(List<double> values);

        /// <summary>Sets the list of function values.
        /// Only the list reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        void SetValuesReference(List<double> reference);

        /// <summary>Returns the specified function value.</summary>
        /// <param name="which">Specifies which function to return (counting from 0).</param>
        double GetValue(int which);

        /// <summary>Sets the specified function value.</summary>
        /// <param name="which">Specifies which function is set (counting from 0).</param>
        /// <param name="value">Assigned value of the function.</param>
        void SetValue(int which, double value);


        // FUNCTION GRADIENTS:

        /// <summary>Function gradients.
        /// If CopyReferences=true (false by default) then only the list reference is copied in assignments.</summary>
        List<IVector> Gradients { get; set; }

        /// <summary>Returns a list of function gradients.</summary>
        List<IVector> GetGradients();

        /// <summary>Sets function gradients.
        /// If CopyReferences=true (false by default) then only the list reference is copied.</summary>
        /// <param name="values">Value to be assigned.</param>
        void SetGradients(List<IVector> values);

        /// <summary>Sets function gradients.
        /// Only the list reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        void SetGradientsReference(List<IVector> reference);

        /// <summary>Returns the gradient of the specified function.</summary>
        /// <param name="which">Specifies which function to take (couonted from 0).</param>
        IVector GetGradient(int which);

        /// <summary>Returns the specific function gradient component.</summary>
        /// <param name="which">Specifies which function to take (couonted from 0).</param>
        /// <param name="index">Specifies gradient component (conted form 0).</param>
        double GetGradient(int which, int index);

        /// <summary>Sets the specified function gradient.
        /// If CopyReferences=true (false by default) then only the reference is copied.</summary>
        /// <param name="which">Specifies which function to take (couonted from 0).</param>
        /// <param name="value">Value of the gradient to be assigned.</param>
        void SetGradient(int which, IVector value);

        /// <summary>Sets the specified function gradient.
        /// Only the reference is copied.</summary>
        /// <param name="which">Specifies which function to take (couonted from 0).</param>
        /// <param name="reference">Gradient reference to be assigned.</param>
        void SetGradientReference(int which, IVector reference);

        /// <summary>Sets the specified fuction gradient component.</summary>
        /// <param name="which">Specifies which function to take (couonted from 0).</param>
        /// <param name="index">Specified index of gradient coponent to be set.</param>
        /// <param name="value">Value to be assigned to the specified component.</param>
        void SetGradient(int which, int index, double value);


        // FUNCTION HESSIANS:

        /// <summary>Functions' Hessians (matrices of second derivatives).
        /// If CopyReferences=true (false by default) then only the list reference is copied in assignments.</summary>
        List<IMatrix> Hessians { get; set; }

        /// <summary>Returns the list of functions' Hessians.</summary>
        List<IMatrix> GetHessians();

        /// <summary>Sets functios' Hessians.
        /// If CopyReferences=true (false by default) then only the list reference is copied.</summary>
        /// <param name="values">List of Hessians to be assigned.</param>
        void SetHessians(List<IMatrix> values);

        /// <summary>Sets functios' Hessians.
        /// Only the list reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        void SetHessiansReference(List<IMatrix> reference);

        /// <summary>Returns Hessian of the specified function.</summary>
        /// <param name="which">Specifies which function it applies to (counting from 0).</param>
        IMatrix GetHessian(int which);

        /// <summary>Returns the specified component of Hessian of the specified function.</summary>
        /// <param name="which">Specifies which function it applies to (counting from 0).</param>
        /// <param name="rowIndex">Row index of the component (counting from 0).</param>
        /// <param name="columnIndex">Column index of the component (counting from 0).</param>
        double GetHessian(int which, int rowIndex, int columnIndex);

        /// <summary>Sets the specified function's Hessian.
        /// If CopyReferences=true (false by default) then only the reference is copied.</summary>
        /// <param name="which">Specifies which function it applies to (counting from 0).</param>
        /// <param name="value">Hessian matrix to be assigned.</param>
        void SetHessian(int which, IMatrix value);

        /// <summary>Sets the specified function's Hessian.
        /// Only the reference is copied.</summary>
        /// <param name="which">Specifies which function it applies to (counting from 0).</param>
        /// <param name="reference">Hessian matrix reference to be assigned.</param>
        void SetHessianReference(int which, IMatrix reference);

        /// <summary>Sets the specified component of the specified function's Hessian.</summary>
        /// <param name="which">Specifies which function it applies to (counting from 0).</param>
        /// <param name="rowIndex">Row index of the component (counting from 0).</param>
        /// <param name="columnIndex">Column index of the component (counting from 0).</param>
        /// <param name="value">Value assigned to the specified Hessian component.</param>
        void SetHessian(int which, int rowIndex, int columnIndex, double value);

        #endregion Results

        #region Allocation

        // ALLOCATION OF FIELDS:

        /// <summary>Sets all objects (parameter and result objects) to null.
        /// If the references are not assigned elsewhere, these objects become eligible for garbage collection.</summary>
        void NullifyAll();

        /// <summary>Sets all result objects to null.
        /// If the references are not assigned elsewhere, these objects become eligible for garbage collection.</summary>
        void NullifyResults();

        /// <summary>Allocates space for parameters.</summary>
        void AllocateParameters();

        /// <summary>Allocates space for all requested result objects.</summary>
        void AllocateRequested();

        /// <summary>Allocates space for list of function values.</summary>
        void AllocateValuesList();

        /// <summary>Allocates space for list of function gradients.</summary>
        void AllocateGradientsList();

        /// <summary>Allocates space for function gradients (including for the list, if necessarty).</summary>
        void AllocateGradients();

        /// <summary>Allocates space for the specified function gradient.</summary>
        /// <param name="which">Specifies which function it applies to (countinf form 0).</param>
        void AllocateGradient(int which);

        /// <summary>Allocates space for the list of functions' Hessians.</summary>
        void AllocateHessiansList();

        /// <summary>Allocates space for functions' Hessians (including space for the list, if necessary).</summary>
        void AllocateHessians();

        /// <summary>Allocates space for the specified fucnction's Hessian.</summary>
        /// <param name="which">Specifies which function it applies to (conting form 0).</param>
        void AllocateHessian(int which);

        #endregion Allocation

        #region Flags

        // REQUEST FLAGS:

        /// <summary>Indicates whether calculation of function values is/was requested.</summary>
        bool ReqValues { get; set; }

        /// <summary>Indicates whether calculation of functions gradients is/was requested.</summary>
        bool ReqGradients { get; set; }

        /// <summary>Indicates whether calculation of functions Hessian is/was requested.</summary>
        bool ReqHessians { get; set; }


        // CALCULATED FLAGS:

        /// <summary>Sets all calculated flags to false, error code to 0 (no error) and error string to null.</summary>
        void ResetResults();

        /// <summary>Error code.
        ///   0 - everything is OK.
        ///   negative value - something went wrong.</summary>
        int ErrorCode { get; set; }

        /// <summary>Error string indicating what went wrong.</summary>
        String ErrorString { get; set; }

        /// <summary>Collectively gets or sets calculated flags.
        /// Set false: all calculated flags are set to false.
        /// Set true: all calculated flags for which the corresponding request flags are true, 
        ///     are set to truee, others are set to false.
        /// Get: returns true if all the flags for which the corresponding request flags are true,
        ///     are also true. Otherwise returns false.
        /// </summary>
        bool Calculated { get; set; }

        /// <summary>Indicates whether calculation of function values is/was requested.</summary>
        bool CalculatedValues { get; set; }

        /// <summary>Indicates whether calculation of functions gradient is/was requested.</summary>
        bool CalculatedGradients { get; set; }

        /// <summary>Indicates whether calculation of functions' Hessians are/was requested.</summary>
        bool CalculatedHessians { get; set; }

        #endregion Flags

        #region HelperMethods


        /// <summary>Copies data from another vector function results.</summary>
        /// <param name="results">Vector function results which data is copied from.</param>
        void Copy(IVectorFunctionResults results);

        /// <summary>Returns an exact deep copy of the current object.</summary>
        IVectorFunctionResults GetCopy();

        #endregion HelperMethods

    } // interface IAnalysisResults




}
