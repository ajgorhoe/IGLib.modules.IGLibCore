// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{

    /// <summary>Storage for results of a scalar function.
    /// Includes parameters at which function was (or should be) evaluated, flags specifying what has been and what 
    /// should be evaluated, calculated value, gradient and Hessian of the function in the specified point.
    /// </summary>
    /// $A Igor xx Dec09 Nov10;
    public interface IScalarFunctionResults
    {

        #region Characteristics

        // DIMENSIONS OF THE SPACE:

        /// <summary>Number of parameters.</summary>
        int NumParameters { get; set; }

        #endregion Characteristics

        #region Operation

        // OPERATION PARAMETERS:

        /// <summary>Indicates whether just references can be copied when setting function
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

        // FUNCTION RESULTS:

        // FUNCTION VALUE:

        /// <summary>Value of the function.</summary>
        double Value { get; set; }

        /// <summary>Returns the value of the function.</summary>
        double GetValue();

        /// <summary>Sets the value of the function.</summary>
        /// <param name="value">Value to be assigned to the function.</param>
        void SetValue(double value);


        // FUNCTION GRADIENT:

        /// <summary>Function gradient.
        /// If CopyReferences=true (false by default) then only the reference is copied in assignments.</summary>
        IVector Gradient { get; set; }

        /// <summary>Returns the function gradient.</summary>
        IVector GetGradient();

        /// <summary>Sets the function gradient.
        /// If CopyReferences=true (false by default) then only the reference is copied.</summary>
        /// <param name="value">Value to be assigned.</param>
        void SetGradient(IVector value);

        /// <summary>Sets the function gradient.
        /// Only the reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        void SetGradientReference(IVector reference);

        /// <summary>Returns the specified component of the function gradient.</summary>
        /// <param name="index">Index of the component.</param>
        double GetGradient(int index);

        /// <summary>Sets the specified component of the function gradient.</summary>
        /// <param name="index">Index of gradient component to be set.</param>
        /// <param name="value">Value of the function gradient component.</param>
        void SetGradient(int index, double value);


        // FUNCTION HESSIAN:

        /// <summary>Function Hessian (matrix of second derivatives).
        /// If CopyReferences=true (false by default) then only the reference is copied in assignments.</summary>
        IMatrix Hessian { get; set; }

        /// <summary>Returns the function's Hessian.</summary>
        IMatrix GetHessian();

        /// <summary>Sets the functions' Hessian.
        /// If CopyReferences=true (false by default) then only the reference is copied.</summary>
        /// <param name="value">Value to be assigned.</param>
        void SetHessian(IMatrix value);

        /// <summary>Sets the functions' Hessian.
        /// Only the reference is copied.</summary>
        /// <param name="reference">Reference to be assigned.</param>
        void SetHessianReference(IMatrix reference);

        /// <summary>Returns the specified component of the function;s Hessian.</summary>
        /// <param name="rowIndex">Row index of the component (counting from 0).</param>
        /// <param name="columnIndex">Column index of the component (counting from 0).</param>
        double GetHessian(int rowIndex, int columnIndex);

        /// <summary>Sets the specified component of the function's Hessian.</summary>
        /// <param name="rowIndex">Row index of the component (counting from 0).</param>
        /// <param name="columnIndex">Column index of the component (counting from 0).</param>
        /// <param name="value">Value to be assigned to Hessian.</param>
        void SetHessian(int rowIndex, int columnIndex, double value);

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

        /// <summary>Allocate space for function gradient.</summary>
        void AllocateGradient();

        /// <summary>Allocates space for function Hessian.</summary>
        void AllocateHessian();

        #endregion Allocation

        #region Flags

        // REQUEST FLAGS:

        /// <summary>Indicates whether calculation of function value is/was requested.</summary>
        bool ReqValue { get; set; }

        /// <summary>Indicates whether calculation of function gradient is/was requested.</summary>
        bool ReqGradient { get; set; }

        /// <summary>Indicates whether calculation of function Hessian is/was requested.</summary>
        bool ReqHessian { get; set; }


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

        /// <summary>Indicates whether calculation of function value is/was requested.</summary>
        bool CalculatedValue { get; set; }

        /// <summary>Indicates whether calculation of function gradient is/was requested.</summary>
        bool CalculatedGradient { get; set; }

        /// <summary>Indicates whether calculation of function Hessian is/was requested.</summary>
        bool CalculatedHessian { get; set; }

        #endregion Flags


        #region Helper methods


        /// <summary>Copies data from another analysis results.</summary>
        /// <param name="results">Analysis results which data is copied from.</param>
        void Copy(IScalarFunctionResults results);

        /// <summary>Returns an exact deep copy of the current object.</summary>
        IScalarFunctionResults GetCopy();

        #endregion Helper methods

    }  // interface IScalarFunctionResults


}