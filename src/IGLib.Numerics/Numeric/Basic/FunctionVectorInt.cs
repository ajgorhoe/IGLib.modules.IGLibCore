// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{


    /// <summary>Vector function of a vector argument.</summary>
    /// $A Igor xx May10 Dec10;
    public interface IVectorFunction : ILockable
    {

        #region Data

        /// <summary>Returns a short name of the function.</summary>
        string Name { get; }

        /// <summary>Returns a short description of the function.</summary>
        string Description { get; }

        /// <summary>Sets number of parameters of the current vector function to the specified value.</summary>
        /// <param name="num">Number of parameters.</param>
        void SetNumParameters(int num);

        /// <summary>Sets number of values of the vector function to the specified value.</summary>
        /// <param name="num">Number of values.</param>
        void SetNumValues(int num);

        /// <summary>Gets number of parameters of the current vector function
        /// (-1 for not defined, in case that function works with different 
        /// numbers of parameters).</summary>
        int NumParameters { get; }

        /// <summary>Gets number of values of the current vector function
        /// (-1 for not defined, e.g. in case that function works with different 
        /// numbers of parameters and number of functions depends on number of
        /// parameters).</summary>
        int NumValues { get; }

        /// <summary>Tells whether value of the function is defined by implementation.</summary>
        bool ValueDefined { get; }

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        bool DerivativeDefined { get; }

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        bool SecondDerivativeDefined { get; }

        /// <summary>Tells whether the function supports evaluation of individual components.
        /// If not then evaluation is performed through the Evaluate function.</summary>
        bool ComponentWiseEvaluation { get; }

        #endregion Data

        #region Evaluation


        /// <summary>Performs evaluation of requested vector function results and writes them
        /// to the provided data structure.</summary>
        /// <param name="evaluationData">Data structure where request parameters are
        /// obtained and where results are written.</param>
        void Evaluate(IVectorFunctionResults evaluationData);


        #region ComponentWise


        /// <summary>Calculates and returns the particular component of the vector
        /// function value.</summary>
        /// <param name="evaluationData">Evaluation data that contains function parameters and
        /// can store function resuts. If the function does not support component-wise evaluation
        /// then results will be stored to this structure and returned from it. This makes reuse
        /// possible - when different components are evaluated subsequently with the same parameters,
        /// results are calculated only for the first time.</param>
        /// <param name="which">Specifies which function to evaluate.</param>
        double Value(IVectorFunctionResults evaluationData, int which);

        /// <summary>Calculates and returns the particular component of the vector
        /// function derivative.</summary>
        /// <param name="evaluationData">Evaluation data that contains function parameters and
        /// can store function resuts. If the function does not support component-wise evaluation
        /// then results will be stored to this structure and returned from it. This makes reuse
        /// possible - when different components are evaluated subsequently with the same parameters,
        /// results are calculated only for the first time.</param>
        /// <param name="which">Specifies which function to take.</param>
        /// <param name="component">Specifies which compoonent of the gradient should be returned.</param>
        double Derivative(IVectorFunctionResults evaluationData, int which,
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
        double SecondDerivative(IVectorFunctionResults evaluationData, int which,
            int rowNum, int columnNum);


        /// <summary>Calculates and returns the particular component of the vector
        /// function value.</summary>
        /// <param name="parameters">Parameters for which value of the specified component is calculated.</param>
        /// <param name="which">Specifies which function to evaluate.</param>
        double Value(IVector parameters, int which);

        /// <summary>Calculates and returns the particular component of the vector
        /// function derivative.</summary>
        /// <param name="parameters">Parameters for which derivative of the specified component is calculated.</param>
        /// <param name="which">Specifies which function to take.</param>
        /// <param name="component">Specifies which compoonent of the gradient should be returned.</param>
        double Derivative(IVector parameters, int which,
            int component);

        /// <summary>Calculates and returns the particular component of the vector
        /// function's second derivative (Hessian).</summary>
        /// <param name="parameters">Parameters for which the specified second derivative of the specified component is calculated.</param>
        /// <param name="which">Specifies which function to take.</param>
        /// <param name="rowNum">Specifies which row of the Hessian (matrix of second derivatives) should 
        /// be returned.</param>
        /// <param name="columnNum">Specifies which column of the Hessian (matrix of second derivatives) should 
        /// be returned.</param>
        double SecondDerivative(IVector parameters, int which,
            int rowNum, int columnNum);

        #endregion ComponentWise


        /// <summary>Returns the value of this function at the specified parameter.</summary>
        void Value(IVector x, ref List<double> value);

        /// <summary>Returns the first derivative of this function at the specified parameter.</summary>
        void Derivative(IVector x, ref List<IVector> derivative);


        /// <summary>Returns the second derivative (Hessian) of this function at the specified arameter.</summary>
        void SecondDerivative(IVector x, ref List<IMatrix> secondDerivative);


        /// <summary>Calculation of values, gradients, and hessians of the vector function 
        /// according to request flags.</summary>
        /// <param name="parameters">Parameters at which evaluation takes place.</param>
        /// <param name="calculateValues">Flag for calculation of function values, input/output.</param>
        /// <param name="values">Function values, output.</param>
        /// <param name="calculateGradients">Flag for calculation of functions gradients, input/output.</param>
        /// <param name="gradients">Gradients, output.</param>
        /// <param name="calculateHessians">Flag for calculation of Hessians, input/output.</param>
        /// <param name="hessians">Functions' hessians, output.</param>
        void Evaluate(IVector parameters, ref bool calculateValues, ref List<double> values,
                ref bool calculateGradients, ref List<IVector> gradients,
                ref bool calculateHessians, ref List<IMatrix> hessians);


        #endregion Evaluation

    }  // interface IVectorFunction

    

}
