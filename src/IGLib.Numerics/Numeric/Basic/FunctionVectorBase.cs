
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{


    /// <summary>
    /// Base class for defining Vector functions whose evaluation is defined in bulk, and evaluation
    /// of individual components can not be performed directly.
    /// For these functioons, individual values and individual gradient or vector components are
    /// evaluated in such a way thet the function is first evaluated as whole, storing its results
    /// to an object implementing <see cref="IVectorFunctionResults"/>.
    /// </summary>
    /// $A Igor xx May10 Dec10;
    /// TODO: implement ICloneable
    public abstract class VectorFunctionBase : VectorFunctionBaseGeneral, 
        IVectorFunction, ILockable
    {

        #region Data

        /// <summary>Returns true, indicating that individual components of functions and
        /// eventually their deirvatives is performed directly.</summary>
        public override bool ComponentWiseEvaluation
        {
            get
            {
                return false;
            }
            protected set
            {
                if (value==true)
                    throw new ArgumentException("ComponentWiseEvaluation can not be set to true in this type of vector function."); ;
            }
        }

        #endregion Data


        #region Evaluation


        /// <summary>Performs evaluation of requwester function results and writes them
        /// to the provided data structure.</summary>
        /// <param name="evaluationData">Data structure where request parameters are
        /// obtained and where results are written.</param>
        public abstract override void Evaluate(IVectorFunctionResults evaluationData);


        #region ComponentWise

        /// <summary>Calculates and returns the particular component of the vector
        /// function value.</summary>
        /// <param name="evaluationData">Evaluation data that contains function parameters and
        /// can store function resuts. If the function does not support component-wise evaluation
        /// then results will be stored to this structure and returned from it. This makes reuse
        /// possible - when different components are evaluated subsequently with the same parameters,
        /// results are calculated only for the first time.</param>
        /// <param name="which">Specifies which function to evaluate.</param>
        public override double Value(IVectorFunctionResults evaluationData, int which)
        {
            if (evaluationData == null)
                throw new ArgumentException("Can not calculate individual vector function value, results object not specified.");
            if (!evaluationData.CalculatedValues)
            {
                evaluationData.ReqValues = true;
                Evaluate(evaluationData);
                if (!evaluationData.CalculatedValues)
                {
                    string message = "Vector function derivative could not be evaluated (function "
                        + which + ").";
                    if (evaluationData.ErrorCode<0)
                        message+= Environment.NewLine + "  Error code: " + evaluationData.ErrorCode;
                    if (!String.IsNullOrEmpty(evaluationData.ErrorString))
                        message+=Environment.NewLine + "  Error string: " + evaluationData.ErrorString; 
                    throw new InvalidOperationException(message);
                }
            }
            return evaluationData.GetValue(which);
        }

        /// <summary>Calculates and returns the particular component of the vector
        /// function derivative.</summary>
        /// <param name="evaluationData">Evaluation data that contains function parameters and
        /// can store function resuts. If the function does not support component-wise evaluation
        /// then results will be stored to this structure and returned from it. This makes reuse
        /// possible - when different components are evaluated subsequently with the same parameters,
        /// results are calculated only for the first time.</param>
        /// <param name="which">Specifies which function to take.</param>
        /// <param name="component">Specifies which compoonent of the gradient should be returned.</param>
        public override double Derivative(IVectorFunctionResults evaluationData, int which,
            int component)
        {
            if (evaluationData == null)
                throw new ArgumentException("Can not calculate individual vector function derivative component, results object not specified.");
            if (!evaluationData.CalculatedGradients)
            {
                evaluationData.ReqGradients = true;
                Evaluate(evaluationData);
                if (!evaluationData.CalculatedGradients)
                {
                    string message = "Vector function derivative could not be evaluated (function "
                        + which + ", component " + component + ").";
                    if (evaluationData.ErrorCode < 0)
                        message += Environment.NewLine + "  Error code: " + evaluationData.ErrorCode;
                    if (!String.IsNullOrEmpty(evaluationData.ErrorString))
                        message += Environment.NewLine + "  Error string: " + evaluationData.ErrorString;
                    throw new InvalidOperationException(message);
                }
            }
            return evaluationData.GetGradient(which, component);
        }

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
        public override double SecondDerivative(IVectorFunctionResults evaluationData, int which,
            int rowNum, int columnNum)
        {
            if (evaluationData == null)
                throw new ArgumentException("Can not calculate individual vector function second derivative component, results object not specified.");
            if (!evaluationData.CalculatedHessians)
            {
                evaluationData.ReqHessians = true;
                Evaluate(evaluationData);
                if (!evaluationData.CalculatedHessians)
                {
                    string message = "Vector function second derivative could not be evaluated (function "
                        + which + ", row " + rowNum + ", column " + columnNum + ").";
                    if (evaluationData.ErrorCode < 0)
                        message += Environment.NewLine + "  Error code: " + evaluationData.ErrorCode;
                    if (!String.IsNullOrEmpty(evaluationData.ErrorString))
                        message += Environment.NewLine + "  Error string: " + evaluationData.ErrorString;
                    throw new InvalidOperationException(message);
                }
            }
            return evaluationData.GetHessian(which, rowNum, columnNum);
        }

        #endregion ComponentWise



        /// <summary>Calculation of values, gradients, and hessians of the vector function 
        /// according to request flags.
        /// Calculated by using Evaluate(IVectorFunctionResults) in this class.</summary>
        /// <param name="parameters">Parameters at which evaluation takes place.</param>
        /// <param name="calculateValues">Flag for calculation of function values, input/output.</param>
        /// <param name="values">Function values, output.</param>
        /// <param name="calculateGradients">Flag for calculation of functions gradients, input/output.</param>
        /// <param name="gradients">Gradients, output.</param>
        /// <param name="calculateHessians">Flag for calculation of Hessians, input/output.</param>
        /// <param name="hessians">Functions' hessians, output.</param>
        /// <returns></returns>
        public override void Evaluate(IVector parameters, ref bool calculateValues, ref List<double> values,
                ref bool calculateGradients, ref List<IVector> gradients,
                ref bool calculateHessians, ref List<IMatrix> hessians)
        {
            lock (Lock)
            {
                IVectorFunctionResults results = ResultsStore.GetEligible();
                if (results == null)
                    throw new InvalidOperationException("Vector function: could not get auxiliary results object from the store.");
                results.Parameters = parameters;
                results.ReqValues = calculateValues;
                results.ReqGradients = calculateGradients;
                results.ReqHessians = calculateHessians;
                // Actual evaluation:
                Evaluate(results);
                if (calculateValues && !results.CalculatedValues)
                {
                    string message = "Vector function values could not be evaluated.";
                    if (results.ErrorCode < 0)
                        message += Environment.NewLine + "  Error code: " + results.ErrorCode;
                    if (!String.IsNullOrEmpty(results.ErrorString))
                        message += Environment.NewLine + "  Error string: " + results.ErrorString;
                    throw new InvalidOperationException(message);
                }
                if (calculateGradients && !results.CalculatedGradients)
                {
                    string message = "Vector function gradients could not be evaluated.";
                    if (results.ErrorCode < 0)
                        message += Environment.NewLine + "  Error code: " + results.ErrorCode;
                    if (!String.IsNullOrEmpty(results.ErrorString))
                        message += Environment.NewLine + "  Error string: " + results.ErrorString;
                    throw new InvalidOperationException(message);
                }
                if (calculateHessians && !results.CalculatedHessians)
                {
                    string message = "Vector function hessians could not be evaluated.";
                    if (results.ErrorCode < 0)
                        message += Environment.NewLine + "  Error code: " + results.ErrorCode;
                    if (!String.IsNullOrEmpty(results.ErrorString))
                        message += Environment.NewLine + "  Error string: " + results.ErrorString;
                    throw new InvalidOperationException(message);
                }
                int numFunc = results.NumFunctions;
                int numParam = results.NumParameters;
                if (calculateValues && results.CalculatedValues)
                {
                    Util.ResizeList(ref values, numFunc, 0.0, true);
                    for (int i = 0; i < numFunc; ++i)
                        values[i] = results.GetValue(i);
                }
                if (calculateGradients && results.CalculatedGradients)
                {
                    Util.ResizeList(ref gradients, numFunc, null, true);
                    for (int which = 0; which < numFunc; ++which)
                    {
                        IVector gradient = gradients[which];
                        if (gradient == null)
                            gradient = gradients[which] = new Vector(numParam);
                        else if (gradient.Length != numParam)
                            gradient = gradients[which] = new Vector(numParam);
                        for (int i = 0; i < numParam; ++i)
                            gradient[i] = results.GetGradient(which, i);
                    }
                }
                ResultsStore.StoreEligible(results);
                if (calculateHessians && results.CalculatedHessians)
                {
                    Util.ResizeList(ref hessians, numFunc, null, true);
                    for (int which = 0; which < numFunc; ++which)
                    {
                        IMatrix hessian = hessians[which];
                        if (hessian == null)
                            hessian = hessians[which] = new Matrix(numParam, numParam);
                        else if (hessian.RowCount != numParam
                                || hessian.ColumnCount != numParam)
                            hessian = hessians[which] = new Matrix(numParam, numParam);
                        for (int i = 0; i < numParam; ++i)
                            for (int j = 0; j < numParam; ++j)
                                hessian[i, j] = results.GetHessian(which, i, j);
                    }
                }
                ResultsStore.StoreEligible(results);
            }
        }  // Evaluate(IVector, ref bool, ref List<double>, ref bool, ref List<IVector>, ref bool, ref List<IMatrix>)


        #endregion Evaluation

    }  // class VectorFunctionBase



    /// <summary>Base class for defining Vector functions whose component-wise evaluation is defined directly.</summary>
    /// $A Igor xx May09 Dec10;
    /// TODO: implement ICloneable
    public abstract class VectorFunctionBaseComponentWise : VectorFunctionBaseGeneral, IVectorFunction, ILockable
    {

        /// <summary>Returns true, indicating that individual components of functions and
        /// eventually their deirvatives is performed directly.</summary>
        public override bool ComponentWiseEvaluation
        {
            get
            {
                return true;
            }
            protected set
            {
                if (value == false)
                    throw new ArgumentException("ComponentWiseEvaluation can not be set to false in this type of vector function."); ;
            }
        }


        #region Evaluation


        /// <summary>Performs evaluation of requwester function results and writes them
        /// to the provided data structure.
        /// Uses Evaluate(IVector, ref bool, ref List{double}, ref bool, ref List{IVector}, ref bool, ref List{IMatrix})
        /// to do the job.</summary>
        /// <param name="evaluationData">Data structure where request parameters are
        /// obtained and where results are written.</param>
        public override void Evaluate(IVectorFunctionResults evaluationData)
        {
            if (evaluationData==null)
                throw new InvalidOperationException("Vector function: evaluation data is not specified.");
            if (evaluationData.Parameters == null)
                throw new InvalidOperationException("Vector function: parameters are not specified.");
            int numPar = this.NumValues;
            int numFunc = this.NumValues;
            // Set basic propeties of vector function on evaluation data:
            evaluationData.NumParameters = numPar;
            evaluationData.NumFunctions = numFunc;
            // Extract calculatio flags from evaluation data:
            bool calculateValues = evaluationData.ReqValues;
            bool calculateGradients = evaluationData.ReqGradients;
            bool calculateHessians = evaluationData.ReqHessians;
            // Get references to objects holding individual results on evaluation data:
            List<double> values = evaluationData.Values;
            List<IVector> gradients = evaluationData.Gradients;
            List<IMatrix> hessians = evaluationData.Hessians;
            // Perform evaluation by another function:
            Evaluate(evaluationData.Parameters, ref calculateValues, ref values,
                ref calculateGradients, ref gradients,
                ref calculateHessians, ref hessians);
            // Store flags that indicate what has been calculated:
            evaluationData.CalculatedValues = calculateValues;
            evaluationData.CalculatedGradients = calculateGradients;
            evaluationData.CalculatedHessians = calculateHessians;
            // Set result references on evaluation data to possibly modified objects
            // (because evaluation function could have changed references, too):
            evaluationData.SetValuesReference(values);
            evaluationData.SetGradientsReference(gradients);
            evaluationData.SetHessiansReference(hessians);
            int errorCode = 0;
            string errorString = null;
            if (evaluationData.ReqValues && !calculateValues)
            {
                errorCode = -1;
                errorString = errorString + "Vector functions could not be evaluated.";
            }
            if (evaluationData.ReqGradients && !calculateGradients)
            {
                if (errorCode>=0)
                    errorCode = -2;
                errorString = errorString + "Vector function gradients could not be evaluated.";
            }
            if (evaluationData.ReqHessians && !calculateHessians)
            {
                if (errorCode >= 0) 
                    errorCode = -1;
                errorString = errorString + "Vector function hessians could not be evaluated.";
            }
        }  // Evaluate(IVectorFunctionResults 


        #region ComponentWise

        /// <summary>Calculates and returns the particular component of the vector
        /// function value.</summary>
        /// <param name="evaluationData">Evaluation data that contains function parameters and
        /// can store function resuts. If the function does not support component-wise evaluation
        /// then results will be stored to this structure and returned from it. This makes reuse
        /// possible - when different components are evaluated subsequently with the same parameters,
        /// results are calculated only for the first time.</param>
        /// <param name="which">Specifies which function to evaluate.</param>
        public abstract override double Value(IVectorFunctionResults evaluationData, int which);

        /// <summary>Calculates and returns the particular component of the vector
        /// function derivative.</summary>
        /// <param name="evaluationData">Evaluation data that contains function parameters and
        /// can store function resuts. If the function does not support component-wise evaluation
        /// then results will be stored to this structure and returned from it. This makes reuse
        /// possible - when different components are evaluated subsequently with the same parameters,
        /// results are calculated only for the first time.</param>
        /// <param name="which">Specifies which function to take.</param>
        /// <param name="component">Specifies which compoonent of the gradient should be returned.</param>
        public abstract override double Derivative(IVectorFunctionResults evaluationData, int which,
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
        public abstract override double SecondDerivative(IVectorFunctionResults evaluationData, int which,
            int rowNum, int columnNum);


        #endregion ComponentWise


        /// <summary>Calculation of values, gradients, and hessians of the vector function 
        /// according to request flags.
        /// Uses functions for component-wise evaluation to do the job.</summary>
        /// <param name="parameters">Parameters at which evaluation takes place.</param>
        /// <param name="calculateValues">Flag for calculation of function values, input/output.</param>
        /// <param name="values">Function values, output.</param>
        /// <param name="calculateGradients">Flag for calculation of functions gradients, input/output.</param>
        /// <param name="gradients">Gradients, output.</param>
        /// <param name="calculateHessians">Flag for calculation of Hessians, input/output.</param>
        /// <param name="hessians">Functions' hessians, output.</param>
        /// <returns></returns>
        public override void Evaluate(IVector parameters, ref bool calculateValues, ref List<double> values,
                ref bool calculateGradients, ref List<IVector> gradients,
                ref bool calculateHessians, ref List<IMatrix> hessians)
        {
            int numFunc = NumValues;
            int numParam = parameters.Length;
            if (numParam <= 0)
                throw new InvalidOperationException("Vector function in inconsystent state: number of parameters is 0.");
            if (numFunc <= 0)
                throw new InvalidOperationException("Vector function in inconsystent state: number of functions is 0.");
            IVectorFunctionResults results = null;
            results = ResultsStore.TryGet();
            if (results == null)
                results = new VectorFunctionResults();
            results.NumFunctions = numFunc;
            results.SetParametersReference(parameters);
            // Calculaton:
            if (calculateValues)
            {
                Util.ResizeList(ref values, numFunc, 0.0, true);
                for (int which = 0; which < numFunc; ++which)
                    values[which] = Value(results, which);
            }
            if (calculateGradients)
            {
                Util.ResizeList(ref gradients, numFunc, null, true);
                for (int which = 0; which < numFunc; ++which)
                {
                    IVector gradient = gradients[which];
                    if (gradient == null)
                        gradient = gradients[which] = new Vector(numParam);
                    else if (gradient.Length != numParam)
                        gradient = gradients[which] = new Vector(numParam);
                    for (int i = 0; i < numParam; ++i)
                        gradient[i] = Derivative(results, which, i);
                }
            }
            if (calculateHessians)
            {
                Util.ResizeList(ref hessians, numFunc, null, true);
                for (int which = 0; which < numFunc; ++which)
                {
                    IMatrix hessian = hessians[which];
                    if (hessian == null)
                        hessian = hessians[which] = new Matrix(numParam, numParam);
                    else if (hessian.RowCount != numParam
                            || hessian.ColumnCount != numParam)
                        hessian = hessians[which] = new Matrix(numParam, numParam);
                    for (int i = 0; i < numParam; ++i)
                        for (int j = 0; j < numParam; ++j)
                        {
                            hessian[i, j] = SecondDerivative(results, which, i, j);
                        }
                }
            }
            lock (Lock)
            {
                // Store results back to auxiliary variable:
                results.SetParametersReference(null);
                ResultsStore.TryStore(results);
            }
        } // Evaluate(IVector, ref bool, ref List<double>, ref bool, ref List<IVector>, ref bool, ref List<IMatrix>)


        #endregion Evaluation


    }  // class VectorFunctionBaseComponentWise


}
