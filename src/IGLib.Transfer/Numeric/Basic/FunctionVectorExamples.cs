// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IG.Num
{

    /// <summary>Various vector function examples.</summary>
    /// $A Igor xx May10 Dec10;
    public static class VectorFunctionExamples
    {



        /// <summary>Example - 2 component vector function of 2 variables.
        /// The first component is the Rosenbrock function and the second 
        /// component is a radially symmetric paraboloid with radius 2 of the
        /// zero contour, centered at coordinate origin.</summary>
        /// $A Igor May10 Dec10;
        public class RosenrockAndCircle: VectorFunctionBase, IVectorFunction
        {

            #region Construction

            public RosenrockAndCircle()
                : base()
            {
                ValueDefined = true;
                DerivativeDefined = true;
                SecondDerivativeDefined = false;
                F0 = new ScalarFunctionExamples.Rosenbrock();
                F1 = new ScalarFunctionExamples.ParaboloidSymmetric2D(4);
                Description = "2D component vector function of two variables. " + Environment.NewLine +
                    "First component: Rosenbrock function f(x,y) = f(x,y) = (1-x)^2 + 100 * (y-x^2)^2." +
                    "Second component: symmetric paraboloid  f(x,y) = x^2 + y^2 - " 
                            + R0Square.ToString() + "." ;
            }

            #endregion construction

            #region Data

          

            #endregion Data


            /// <summary>The first component of the current vector function.</summary>
            protected IScalarFunction F0;

            protected double R0Square = 4;

            /// <summary>The second component of the current vector function.</summary>
            protected IScalarFunction F1;



            /// <summary>Performs evaluation of requwester function results and writes them
            /// to the provided data structure.</summary>
            /// <param name="evaluationData">Data structure where request parameters are
            /// obtained and where results are written.</param>
            public override void Evaluate(IVectorFunctionResults evaluationData)
            {
                if (evaluationData.NumFunctions <= 0)
                    evaluationData.NumFunctions = 2;
                else if (evaluationData.NumFunctions != 2)
                    throw new InvalidDataException("Invalid number of vector function components: "
                        + evaluationData.NumFunctions + " instead of 2.");
                if (evaluationData.NumParameters <= 0)
                    evaluationData.NumParameters = 2;
                else if (evaluationData.NumParameters != 2)
                    throw new InvalidDataException("Invalid number of vector function parameters: "
                        + evaluationData.NumParameters + " instead of 2.");
                IVector parameters = evaluationData.Parameters;
                if (parameters == null)
                    throw new InvalidDataException("Vector of parameters not specified (null reference).");
                else if (parameters.Length != 2)
                    throw new InvalidDataException("Invalid dimension of vector of parameters, " 
                        + parameters.Length + " instead of 2.");
                evaluationData.Calculated = false;
                if (evaluationData.ReqValues)
                {
                    evaluationData.AllocateValuesList();
                    evaluationData.Values[0] = F0.Value(parameters);
                    evaluationData.Values[1] = F1.Value(parameters);
                    evaluationData.CalculatedValues = true;
                }
                if (evaluationData.ReqGradients)
                {
                    evaluationData.AllocateGradients();
                    IVector grad0 = evaluationData.Gradients[0];
                    F0.Gradient(parameters, ref grad0);
                    IVector grad1 = evaluationData.Gradients[1];
                    F1.Gradient(parameters, ref grad1);
                    evaluationData.Gradients[0] = grad0;
                    evaluationData.Gradients[1] = grad1;
                    evaluationData.CalculatedGradients = true;
                }
                if (evaluationData.ReqHessians)
                {
                    evaluationData.AllocateHessians();
                    IMatrix hess0 = evaluationData.Hessians[0];
                    F0.Hessian(parameters, ref hess0);
                    IMatrix hess1 = evaluationData.Hessians[1];
                    F1.Hessian(parameters, ref hess1);
                    evaluationData.Hessians[0] = hess0;
                    evaluationData.Hessians[1] = hess1;
                    evaluationData.CalculatedGradients = true;
                }
            }


        }  // class RosenBrockAndCircle


    }  // static class VectorFunctionExamples

}