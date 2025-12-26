// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IG.Num
{



    /// <summary>Constant scalar function of vector variable. Function is evaluated according to
    /// f(x) =  c
    /// where x is vector of parameters,  and c is the constant scalar term (function value at x=0).</summary>
    /// <remarks>When quadratic function is created or its parameters set, the Matrix parameter 
    /// of the function is interpreted as Hessian, i.e. twice the matrix of quadratic coefficients.</remarks>
    /// $A Igor xx Dec10;
    public class ScalarFunctionConstant : ScalarFunctionUntransformedBase, IScalarFunctionUntransformed
    {

        #region Construction

        private ScalarFunctionConstant()
            : base()
        { }

        /// <summary>Creation of a constant scalar function.
        /// WARNING:
        /// Matrix argument is interpreted as Hessian, i.e. twice the matrix of quadratic coefficients.</summary>
        /// <param name="constantTerm">Constant term.</param>
        public ScalarFunctionConstant(double constantTerm)
            : this()
        {
            this.ScalarTerm = constantTerm;
        }

        #endregion Construction


        #region Data


        /// <summary>Returns the number of constants that specify the linear function of the 
        /// specified dimension.</summary>
        /// <param name="dim">Dimension of the space in which quadratic function is defined.</param>
        public static int GetNumConstants(int dim)
        {
            return 1;
        }

        /// <summary>Gets the number of constants that define the current function. If e.g. the matrix coefficient or the
        /// vector coefficient is not defined then the corresponding constants are not counted.</summary>
        public int NumActualConstants
        {
            get
            {
                return 1;
            }
        }

        private double _c;

        /// <summary>Scalar additive constant.</summary>
        public double ScalarTerm
        {
            get { return _c; }
            protected set { _c = value; }
        }

        #endregion Data


        #region Evaluation


        /// <summary>Returns the value of this function at the specified parameter.</summary>
        public override double Value(IVector parameters)
        {
            return ScalarTerm;
        }

        /// <summary>Tells whether value of the function is defined by implementation. Always true for this case.</summary>
        public override bool ValueDefined
        {
            get { return true; }
            protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
        }

        /// <summary>Calculates first order derivatives (gradient) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first order derivatives (the gradient) are stored.</param>
        public override void GradientPlain(IVector parameters, IVector gradient)
        {
            gradient.SetZero();
        }

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool GradientDefined
        {
            get { return true; }
            protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
        }


        /// <summary>Calculates the second derivative (Hessian matrix) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian matrix) are stored.</param>
        public override void HessianPlain(IVector parameters, IMatrix hessian)
        {
            hessian.SetZero();
        }


        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool HessianDefined
        {
            get { return true; }
            protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
        }

        #endregion Evaluation


        #region Examples

        /// <summary>Creates and returns quadratic scalar function in 2D.</summary>
        public static ScalarFunctionConstant ExampleFunction2d()
        {
            double scalTerm;
            scalTerm = 2;
            return new ScalarFunctionConstant(scalTerm);
        }


        #endregion Examples


    }  // class ScalarFunctionConstant

    /// <summary>Linear scalar function of vector variable. Function is evaluated according to
    /// q(x) =  b^T*x + c
    /// where x is vector of parameters, b is vector of linear coefficients
    /// (gradient at x=0) and c is the scalar term (function value at x=0).</summary>
    /// <remarks>When quadratic function is created or its parameters set, the Matrix parameter 
    /// of the function is interpreted as Hessian, i.e. twice the matrix of quadratic coefficients.</remarks>
    /// $A Igor xx Dec10;
    public class ScalarFunctionLinear : ScalarFunctionUntransformedBase, IScalarFunctionUntransformed
    {

        #region Construction

        private ScalarFunctionLinear()
            : base()
        { }

        /// <summary>Creation of a linear scalar function.
        /// WARNING:
        /// Matrix argument is interpreted as Hessian, i.e. twice the matrix of quadratic coefficients.</summary>
        /// <param name="gradient0">Vector of linear coefficients - gradient of quadratic function at x = 0. Can be null.</param>
        /// <param name="scalarTerm">Constant term.</param>
        public ScalarFunctionLinear(IVector gradient0, double scalarTerm)
            : this()
        {
            this.Gradient0 = gradient0;
            this.ScalarTerm = scalarTerm;
        }

        #endregion Construction


        #region Data


        /// <summary>Returns the number of constants that specify the linear function of the 
        /// specified dimension.</summary>
        /// <param name="dim">Dimension of the space in which quadratic function is defined.</param>
        public static int GetNumConstants(int dim)
        {
            return dim + 1;
        }

        /// <summary>Gets the number of constants that define the current function. If e.g. the matrix coefficient or the
        /// vector coefficient is not defined then the corresponding constants are not counted.</summary>
        public int NumActualConstants
        {
            get
            {
                int ret = 0;
                int dim = 0;
                if (Gradient0 != null)
                {
                    dim = Gradient0.Length;
                    ret += dim;
                }
                ret += 1;
                return ret;
            }
        }
        
        private IVector _b;

        /// <summary>Vector of linear coefficients (equal to gradient of the function).</summary>
        public IVector Gradient0
        {
            get { return _b; }
            protected set { _b = value; }
        }

        private double _c;

        /// <summary>Scalar additive constant.</summary>
        public double ScalarTerm
        {
            get { return _c; }
            protected set { _c = value; }
        }

        #endregion Data


        #region Evaluation


        /// <summary>Returns the value of this function at the specified parameter.</summary>
        public override double Value(IVector parameters)
        {
            double ret = 0.0;
            // ReturnedString *= 0.5;
            if (Gradient0 != null)
                ret += Vector.ScalarProduct(Gradient0, parameters);
            ret += ScalarTerm;
            return ret;
        }

        /// <summary>Tells whether value of the function is defined by implementation. Always true for this case.</summary>
        public override bool ValueDefined
        {
            get { return true; }
            protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
        }

        /// <summary>Calculates first order derivatives (gradient) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first order derivatives (the gradient) are stored.</param>
        public override void GradientPlain(IVector parameters, IVector gradient)
        {
            gradient.SetZero();
            IVector b = this.Gradient0;
            if (b != null)
                Vector.Add(b, gradient, gradient);
        }

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool GradientDefined
        {
            get { return true; }
            protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
        }


        /// <summary>Calculates the second derivative (Hessian matrix) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian matrix) are stored.</param>
        public override void HessianPlain(IVector parameters, IMatrix hessian)
        {
            hessian.SetZero();
        }


        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool HessianDefined
        {
            get { return true; }
            protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
        }

        #endregion Evaluation


        #region Examples

        /// <summary>Creates and returns quadratic scalar function in 2D.</summary>
        public static ScalarFunctionLinear ExampleFunction2d()
        {
            Vector2d grad0;
            double scalTerm;
            grad0 = new Vector2d(new double[] { 1, 0 });
            scalTerm = 2;
            return new ScalarFunctionLinear(grad0, scalTerm);
        }


        #endregion Examples


    }  // class ScalarFunctionLinear


    /// <summary>Quadratic scalar function of vector variable. Function is evaluated according to
    /// q(x) = (1/2)*x^T*G*x + b^T*x + c
    /// where x is vector of parameters, G is constant Hessian matrix, b is vector of linear coefficients
    /// (gradient at x=0) and c is the scalar term (function value at x=0).</summary>
    /// <remarks>When quadratic function is created or its parameters set, the Matrix parameter 
    /// of the function is interpreted as Hessian, i.e. twice the matrix of quadratic coefficients.</remarks>
    /// $A Igor xx Dec10;
    public class ScalarFunctionQuadratic: ScalarFunctionUntransformedBase, IScalarFunctionUntransformed
    {

        #region Construction

        private ScalarFunctionQuadratic()
            : base()
        {  }

        /// <summary>Creation of a quadratic scalar function.
        /// WARNING:
        /// Matrix argument is interpreted as Hessian, i.e. twice the matrix of quadratic coefficients.</summary>
        /// <param name="hessian">Matrix of second derivatives (Hessian). Can be null (in this case function degrades to linear function).</param>
        /// <param name="gradient0">Vector of linear coefficients - gradient of quadratic function at x = 0. Can be null.</param>
        /// <param name="scalarTerm">Constant term.</param>
        public ScalarFunctionQuadratic(IMatrix hessian, IVector gradient0, double scalarTerm): this()
        {
            this.HessianMatrix = hessian;
            this.Gradient0 = gradient0;
            this.ScalarTerm = scalarTerm;
        }

        #endregion Construction

        #region Examples

        /// <summary>Creates and returns quadratic scalar function in 2D.</summary>
        public static ScalarFunctionQuadratic ExampleFunction2d()
        {
            Matrix2d hessian;
            Vector2d grad0;
            double scalTerm;
            hessian = new Matrix2d(new double[,]
                { 
                    {1,   0.5}, 
                    {0.5, 2  } 
                });
            grad0 = new Vector2d(new double[] { 1, 0 } );
            scalTerm = 2;
            return new ScalarFunctionQuadratic(hessian, grad0, scalTerm);
        }


        /// <summary>Creates and returns quadratic scalar function with diagonal Hessian.</summary>
        /// <returns></returns>
        public static ScalarFunctionQuadratic ExampleFunctionDiagonal2d()
        {
            Matrix2d hessian;
            Vector2d grad0;
            double scalTerm;
            hessian = new Matrix2d(new double[,]
                { 
                    {1, 0}, 
                    {0, 2} 
                });
            grad0 = new Vector2d(new double[] { 1, 0 } );
            scalTerm = 2;
            return new ScalarFunctionQuadratic(hessian, grad0, scalTerm);
        }

        /// <summary>Creates and returns quadratic scalar function with only quadratic terms.</summary>
        /// <returns></returns>
        public static ScalarFunctionQuadratic ExampleFunctionPureQuadratic2d()
        {
            Matrix2d hessian;
            Vector2d grad0;
            double scalTerm;
            hessian = new Matrix2d(new double[,]
                { 
                    {1,   0.5}, 
                    {0.5, 2  } 
                });
            grad0 = new Vector2d(new double[] { 0, 0 } );
            scalTerm = 0;
            return new ScalarFunctionQuadratic(hessian, grad0, scalTerm);
        }


        /// <summary>Creates and returns quadratic function with diagonal Hessian.</summary>
        /// <returns></returns>
        public static ScalarFunctionQuadratic ExampleFunctionDiagonalPureQuadratic2d()
        {
            Matrix2d hessian;
            Vector2d grad0;
            double scalTerm;
            hessian = new Matrix2d(new double[,]
                { 
                    {1, 0}, 
                    {0, 2} 
                });
            grad0 = new Vector2d(new double[] { 0, 0 } );
            scalTerm = 0;
            return new ScalarFunctionQuadratic(hessian, grad0, scalTerm);
        }

        #endregion Examples

        #region Data

        private IMatrix _G;

        private IVector _b;

        private double _c;


        /// <summary>Returns the number of constants that specify the quadratic function of the 
        /// specified dimension.</summary>
        /// <param name="dim">Dimension of the space in which quadratic function is defined.</param>
        public static int GetNumConstants(int dim)
        {
            // return (dim * (dim + 1)) / 2 + dim + 1;
            return (dim + 1) * (dim + 2) / 2;
        }

        /// <summary>Gets the number of constants that define the current function. If e.g. the matrix coefficient or the
        /// vector coefficient is not defined then the corresponding constants are not counted.</summary>
        public int NumActualConstants
        {
            get {
                int ret = 0;
                int dim = 0;
                if (HessianMatrix != null)
                {
                    dim = HessianMatrix.RowCount;
                    ret += (dim * (dim + 1)) / 2;
                }
                if (Gradient0 != null)
                {
                    if (dim == 0)
                        dim = Gradient0.Length;
                    else if (dim != Gradient0.Length)
                        throw new InvalidDataException("Inconsistent dimensions of matrix and vector constants for quadratic function.");
                    ret += dim;
                }
                ret += 1;
                return ret;
            }
        }

        /// <summary>Twice the matrix of quadratic coefficients (Hessian matrix).</summary>
        public IMatrix HessianMatrix
        {
            get { return _G; }
            protected set { _G = value; }
        }

        /// <summary>Vector of linear coefficients (equal to gradient of the function at x=0).</summary>
        public IVector Gradient0
        {
            get { return _b; }
            protected set { _b = value; }
        }

        /// <summary>Scalar additive constant.</summary>
        public double ScalarTerm
        {
            get { return _c; }
            protected set { _c = value; }
        }

        #endregion Data


        #region Evaluation


        /// <summary>Returns the value of this function at the specified parameter.</summary>
        public override double Value(IVector parameters)
        {
            double ret = 0.0;
            if (HessianMatrix != null)
                ret += Matrix.Multiply(parameters, HessianMatrix, parameters);
            ret *= 0.5;
            if (Gradient0 != null)
                ret += Vector.ScalarProduct(Gradient0,parameters);
            ret += ScalarTerm;
            return ret;
        }

        /// <summary>Tells whether value of the function is defined by implementation. Always true for this case.</summary>
        public override bool ValueDefined
        {
            get { return true; }
            protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
        }

        /// <summary>Calculates first order derivatives (gradient) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first order derivatives (the gradient) are stored.</param>
        public override void GradientPlain(IVector parameters, IVector gradient)
        {
            IMatrix G = this.HessianMatrix;
            IVector b = this.Gradient0;
            if (G == null)
                gradient.SetZero();
            else
                Matrix.MultiplyPlain(G, parameters, gradient);
            if (b != null)
                Vector.Add(b, gradient, gradient);
        }

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool GradientDefined
        {
            get { return true; }
            protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
        }


        /// <summary>Calculates the second derivative (Hessian matrix) of this function at the specified parameters.
        /// WARNING: Plain function, does not check consistency of arguments.</summary>
        /// <param name="parameters">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian matrix) are stored.</param>
        public override void HessianPlain(IVector parameters, IMatrix hessian)
        {
            IMatrix G = this.HessianMatrix;
            if (G == null)
            {
                for (int i = 0; i < hessian.RowCount; ++i)
                    for (int j = 0; j < hessian.ColumnCount; ++j)
                        hessian[i, j] = 0.0;
            } else
            {
                for (int i = 0; i < hessian.RowCount; ++i)
                    for (int j = 0; j < hessian.ColumnCount; ++j)
                        hessian[i, j] = G[i, j];
            }
        }


        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool HessianDefined
        {
            get { return true; }
            protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
        }

        #endregion Evaluation


    }  // class ScalarFunctionQuadratic



    /// <summary>Various examples of scalar functions.</summary>
    /// $A Igor xx Dec10;
    public static class ScalarFunctionExamples
    {


        /// <summary>RosenBrock function.
        /// f(x,y) = (1-x)^2 + 100 * (y-x^2)^2</summary>
        /// <remarks>
        /// <para>This function is often used for testing optimization algorithms. It has unique minimum at (1, 1) where the value is 0.</para>
        /// <para>See also:</para>
        /// <para>On Wikipedia:  http://en.wikipedia.org/wiki/Rosenbrock_function </para>
        /// </remarks>
        /// $A Igor xx Dec10;
        public class Rosenbrock : ScalarFunctionBase, IScalarFunction
        {

            #region Construction

            /// <summary>Creates a new untransformed Rosenbrock's function.</summary>
            public Rosenbrock()
            {  }

            /// <summary>Creates a new transformed Rosenbrock's function.</summary>
            /// <param name="transf">Affine transformation that is applied on parameters.
            /// If null then the fuction is identical to the untransformed Rosenbrock function.</param>
            public Rosenbrock(IAffineTransformation transf) : base(transf)
            {  }

            #endregion Construction


            #region Data

            /// <summary>Returns a short name of the function.</summary>
            public override string Name
            {
                get { return "Rosenbrock function"; }
            }

            /// <summary>Returns a short description of the function.</summary>
            public override string Description
            {
                get { return "Rosenbrock function in 2D, possibly with Affinte transformation of parameters."; }
            }

            #endregion Data


            #region Evaluation

            /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
            public override double ReferenceValue(IVector parameters)
            {
                if (parameters == null)
                    throw new ArgumentException("Rosenbrock: vector of parameters not specifed (null argument).");
                else if (parameters.Length!=2)
                    throw new ArgumentException("Rosenbrock: number of parameters should be 2, specified: " + parameters.Length + ".");
                double x = parameters[0];
                double y = parameters[1];
                double term1 = 1 - x;
                double term2 = y - x*x;
                return term1*term1 + 100 * term2 * term2;
            }


            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool ValueDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
            }


            /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
            /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
            public override void ReferenceGradientPlain(IVector parameters, IVector gradient)
            {
                if (parameters == null)
                    throw new ArgumentException("Rosenbrock: vector of parameters not specifed (null argument).");
                else if (parameters.Length != 2)
                    throw new ArgumentException("Rosenbrock: number of parameters should be 2, specified: " + parameters.Length + ".");
                double x = parameters[0];
                double y = parameters[1];
                double term2 = y - x*x;
                gradient[0]=-2*(1-x)-400*x*term2;
                gradient[1]=200*term2;
            }

            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool GradientDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
            }
            

            /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
            /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
            public override void ReferenceHessianPlain(IVector parameters, IMatrix hessian)
            {
                if (parameters == null)
                    throw new ArgumentException("Rosenbrock: vector of parameters not specifed (null argument).");
                else if (parameters.Length != 2)
                    throw new ArgumentException("Rosenbrock: number of parameters should be 2, specified: " + parameters.Length + ".");
                double x = parameters[0];
                double y = parameters[1];
                double term2 = y - x * x;
                hessian[0,0] = 2 + 800*x*x - 400*term2;
                hessian[0, 1] = -400 * x;
                hessian[1, 0] = hessian[0, 1]; // -400 * x;
                hessian[1, 1] = 200;
            }

            /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool HessianDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
            }

            #endregion Evaluation

        }  // class Rosenbrock


        /// <summary>Generalzed multivariate RosenBrock function for Dim >= 2.
        /// <para>f(x,y) = Sum[i=0...N-2]{(1-x_{i})^2 + 100 * (x_{i+1}-x_{i}^2)^2}</para></summary>
        /// <remarks>This is one of the generalizations of the 2D Rosenbrock function. 
        /// <para></para>
        /// <para>Moved from stand-alone class, now nested in the <see cref="ScalarFunctionExamples"/> class.</para>
        /// <para>See also:</para>
        /// <para>I. Grešovnik: Test functions for Unconstrained Minimization, Igor's internal report. </para>
        /// <para>Definition at AlgLib page: http://www.alglib.net/optimization/lbfgsandcg.php#header4 </para>
        /// <para>Definition at Wikipedia: http://en.wikipedia.org/wiki/Rosenbrock_function#Multidimensional_generalisations </para>
        /// </remarks>
        /// $A Igor Nov08 Dec10;
        public class RosenbrockGeneralizedAdjacent : ScalarFunctionBase, IScalarFunction
        {

            #region Construction

            /// <summary>Creates a new untransformed Rosenbrock's function.</summary>
            public RosenbrockGeneralizedAdjacent(): base(null)
            { }

            /// <summary>Creates a new transformed Rosenbrock's function.</summary>
            /// <param name="transf">Affine transformation that is applied on parameters.
            /// If null then the fuction is identical to the untransformed Rosenbrock function.</param>
            public RosenbrockGeneralizedAdjacent(IAffineTransformation transf)
                : base(transf)
            {  }

            #endregion Construction


            #region Data

            /// <summary>Returns a short name of the function.</summary>
            public override string Name
            {
                get { return "Generalized (adjacent) Rosenbrock function."; }
            }

            /// <summary>Returns a short description of the function.</summary>
            public override string Description
            {
                get { return "Generalized (adjacent) Rosenbrock function, possibly with Affinte transformation of parameters."; }
            }


            #endregion Data


            #region Evaluation

            /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
            public override double ReferenceValue(IVector parameters)
            {
                if (parameters == null)
                    throw new ArgumentException("Generalized Rosenbrock: Vector of parameters not specifed (null argument).");
                int dim = parameters.Length;
                if (dim < 2)
                    throw new ArgumentException("Generalized Rosenbrock: Number of parameters should be at least 2, specified: " + dim + ".");
                double ret = 0;
                for (int i = 0; i < dim - 1; ++i)
                {
                    double x = parameters[i];
                    double y = parameters[i+1];
                    double term1 = 1 - x;
                    double term2 = y - x * x;
                    ret+= term1 * term1 + 100 * term2 * term2;
                }
                return ret;
            }


            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool ValueDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
            }


            /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
            /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
            public override void ReferenceGradientPlain(IVector parameters, IVector gradient)
            {
                if (parameters == null)
                    throw new ArgumentException("Generalized Rosenbrock: Vector of parameters not specifed (null argument).");
                int dim = parameters.Length;
                if (dim < 2)
                    throw new ArgumentException("Generalized Rosenbrock: Number of parameters should be at least 2, specified: " + dim + ".");
                if (gradient == null)
                    throw new ArgumentException("Vector for storing gradient is not specified (null argument).");
                if (gradient.Length != dim)
                    throw new ArgumentException("Vector for storing gradient is of wrong dimension (" 
                        + gradient.Length + ", should be " + dim + ").");
                gradient.SetZero();
                for (int i = 0; i < dim - 1; ++i)
                {
                    double x = parameters[i];
                    double y = parameters[i+1];
                    double term2 = y - x * x;
                    gradient[i] += -2 * (1 - x) - 400 * x * term2;
                    gradient[i+1] += 200 * term2;
                }
            }

            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool GradientDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
            }


            /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
            /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
            public override void ReferenceHessianPlain(IVector parameters, IMatrix hessian)
            {
                if (parameters == null)
                    throw new ArgumentException("Generalized Rosenbrock: Vector of parameters not specifed (null argument).");
                int dim = parameters.Length;
                if (dim < 2)
                    throw new ArgumentException("Generalized Rosenbrock: Number of parameters should be at least 2, specified: " + dim + ".");
                if (hessian == null)
                    throw new ArgumentException("Mtrix for storing hessian is not specified (null argument).");
                if (hessian.RowCount != dim || hessian.ColumnCount!=dim)
                    throw new ArgumentException("Matrix for storing hessian is of wrong dimensions (" 
                        + hessian.RowCount + "x" + hessian.ColumnCount + ", should be " + dim + "x" + dim + ").");
                hessian.SetZero();
                for (int i = 0; i < dim - 1; ++i)
                {
                    double x = parameters[i];
                    double y = parameters[i+1];
                    double term2 = y - x * x;
                    hessian[i, i] += 2 + 800 * x * x - 400 * term2;
                    hessian[i, i+1] += -400 * x;
                    hessian[i+1, i] += -400 * x;
                    hessian[i+1, i+1] += 200;
                }
            }

            /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool HessianDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
            }

            #endregion Evaluation


        }  // class RosenBrockGeneralizedAdjacent


        /// <summary>Generalzed multivariate RosenBrock function for Dim >= 2.
        /// <para>f(x,y) = Sum[i=0...N-2]{(1-x_{i})^2 + 100 * (x_{i+1}-x_{i}^2)^2}</para></summary>
        /// <remarks>This is one of the generalizations of the 2D Rosenbrock function. 
        /// <para></para>
        /// <para>Moved from stand-alone class, now nested in the <see cref="ScalarFunctionExamples"/> class.</para>
        /// <para>See also:</para>
        /// <para>I. Grešovnik: Test functions for Unconstrained Minimization, Igor's internal report. </para>
        /// <para>Definition at AlgLib page: http://www.alglib.net/optimization/lbfgsandcg.php#header4 </para>
        /// <para>Definition at Wikipedia: http://en.wikipedia.org/wiki/Rosenbrock_function#Multidimensional_generalisations </para>
        /// </remarks>
        /// $A Igor Nov08 Dec10;
        public class RosenbrockGeneralizedExhaustive : ScalarFunctionBase, IScalarFunction
        {

            #region Construction

            /// <summary>Creates a new untransformed Rosenbrock's function.</summary>
            public RosenbrockGeneralizedExhaustive()
                : base(null)
            { }

            /// <summary>Creates a new transformed Rosenbrock's function.</summary>
            /// <param name="transf">Affine transformation that is applied on parameters.
            /// If null then the fuction is identical to the untransformed Rosenbrock function.</param>
            public RosenbrockGeneralizedExhaustive(IAffineTransformation transf)
                : base(transf)
            { }

            #endregion Construction


            #region Data

            /// <summary>Returns a short name of the function.</summary>
            public override string Name
            {
                get { return "Generalized (exhaustive) Rosenbrock function."; }
            }

            /// <summary>Returns a short description of the function.</summary>
            public override string Description
            {
                get { return "Generalized (exhaustive) Rosenbrock function, possibly with Affinte transformation of parameters."; }
            }


            #endregion Data


            #region Evaluation

            /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
            public override double ReferenceValue(IVector parameters)
            {
                if (parameters == null)
                    throw new ArgumentException("Generalized Rosenbrock: Vector of parameters not specifed (null argument).");
                int dim = parameters.Length;
                if (dim < 2)
                    throw new ArgumentException("Generalized Rosenbrock: Number of parameters should be at least 2, specified: " + dim + ".");
                double ret = 0;
                for (int i = 0; i < dim - 1; ++i)
                    for (int j=i+1; j < dim; ++j)
                    {
                        double x = parameters[i];
                        double y = parameters[j];
                        double term1 = 1 - x;
                        double term2 = y - x * x;
                        ret += term1 * term1 + 100 * term2 * term2;
                    }
                return ret;
            }


            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool ValueDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
            }


            /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
            /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
            public override void ReferenceGradientPlain(IVector parameters, IVector gradient)
            {
                if (parameters == null)
                    throw new ArgumentException("Generalized Rosenbrock: Vector of parameters not specifed (null argument).");
                int dim = parameters.Length;
                if (dim < 2)
                    throw new ArgumentException("Generalized Rosenbrock: Number of parameters should be at least 2, specified: " + dim + ".");
                if (gradient == null)
                    throw new ArgumentException("Vector for storing gradient is not specified (null argument).");
                if (gradient.Length != dim)
                    throw new ArgumentException("Vector for storing gradient is of wrong dimension ("
                        + gradient.Length + ", should be " + dim + ").");
                gradient.SetZero();
                for (int i = 0; i < dim - 1; ++i)
                    for (int j=i+1; j<dim; ++j)
                    {
                        double x = parameters[i];
                        double y = parameters[j];
                        double term2 = y - x * x;
                        gradient[i] += -2 * (1 - x) - 400 * x * term2;
                        gradient[j] += 200 * term2;
                    }
            }

            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool GradientDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
            }


            /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
            /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
            public override void ReferenceHessianPlain(IVector parameters, IMatrix hessian)
            {
                if (parameters == null)
                    throw new ArgumentException("Generalized Rosenbrock: Vector of parameters not specifed (null argument).");
                int dim = parameters.Length;
                if (dim < 2)
                    throw new ArgumentException("Generalized Rosenbrock: Number of parameters should be at least 2, specified: " + dim + ".");
                if (hessian == null)
                    throw new ArgumentException("Mtrix for storing hessian is not specified (null argument).");
                if (hessian.RowCount != dim || hessian.ColumnCount != dim)
                    throw new ArgumentException("Matrix for storing hessian is of wrong dimensions ("
                        + hessian.RowCount + "x" + hessian.ColumnCount + ", should be " + dim + "x" + dim + ").");
                hessian.SetZero();
                for (int i = 0; i < dim - 1; ++i)
                    for (int j = i + 1; j < dim; ++j)
                {
                    double x = parameters[i];
                    double y = parameters[j];
                    double term2 = y - x * x;
                    hessian[i, i] += 2 + 800 * x * x - 400 * term2;
                    hessian[i, j] += -400 * x;
                    hessian[j, i] += -400 * x;
                    hessian[j, j] += 200;
                }
            }

            /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool HessianDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
            }

            #endregion Evaluation


        }  // class RosenbrockGeneralizedAExhausted

        

        /// <summary>Symmetric paraboloid centered at coordinate origin.
        /// f(x,y) = x^2 + y^2 - R2.
        /// If R2 is positive then 0-level is a circle, if it is negative then the paraboloid 
        /// does not intersect with zero-plane. Default is R2 = 1 (default constructor).</summary>
        /// $A Igor Dec10;
        public class ParaboloidSymmetric2D : ScalarFunctionBase, IScalarFunction
        {
            
            #region Construction

            /// <summary>Creates a new untransformed Rosenbrock's function.</summary>
            public ParaboloidSymmetric2D(): this(1.0)
            {  }

            /// <summary>Creates a new symmetric paraboloid function.</summary>
            /// <param name="r0Square">Sqyare of diameter (?).</param>
            public ParaboloidSymmetric2D(double r0Square): base()
            {
                throw new NotImplementedException("This is yet to be defined.");
            }

            #endregion Construction


            #region Data

            /// <summary>Parameter that specifies square of the radius of the circle f(x,y) = 0  when positive, 
            /// otherwise it denotes a negative height of the paraboloid above the zero plane.</summary>
            protected double R0Square;

            /// <summary>Returns a short name of the function.</summary>
            public override string Name
            {
                get { 
                    if (_name==null)
                        _name = "Symmetric paraboloid f(x,y) = x^2 + y^2 - " 
                            + R0Square.ToString() + "." ; 
                    return _name;
                }
            }

            /// <summary>Returns a short description of the function.</summary>
            public override string Description
            {
                get { 
                    if (_description==null)
                        _description = "Symmetric paraboloid in 2D centered around coordinate origin,  f(x,y) = x^2 + y^2 - "
                            + R0Square.ToString() + "." ;
                    return _description;
                }
            }

            #endregion Data


            #region Evaluation

            /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
            public override double ReferenceValue(IVector parameters)
            {
                double x = parameters[0];
                double y = parameters[1];
                return x*x + y*y -R0Square;
            }


            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool ValueDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
            }


            /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
            /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
            public override void ReferenceGradientPlain(IVector parameters, IVector gradient)
            {
                double x = parameters[0];
                double y = parameters[1];
                gradient[0]= 2*x;
                gradient[1]= 2*y;
            }

            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool GradientDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
            }
            

            /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
            /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
            public override void ReferenceHessianPlain(IVector parameters, IMatrix hessian)
            {
                double x = parameters[0];
                double y = parameters[1];
                hessian[0,0] = 2;
                hessian[0, 1] = 0;
                hessian[1, 0] = 0;
                hessian[1, 1] = 2;
            }

            /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool HessianDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
            }

            #endregion Evaluation



        }  // class ParaboloidSymmetric2D

        /// <summary>Example quadratic polynomial in 2D.
        /// f(x,y) = 2*x^2 + y^2 + x*y + x + y + 10.</summary>
        /// $A Igor Dec10;
        public class Quadratic2d : ScalarFunctionBase, IScalarFunction
        {

            #region Construction

            /// <summary>Creates a new untransformed example quadratic polynomial in 2D.</summary>
            public Quadratic2d()
            {  }


            /// <summary>Creates a new transformed example quadratic polynomial in 2D.
            /// Actual function is identical to the reference function applied to inverse transformed parameters.</summary>
            /// <param name="transf">Affine transformation that is applied to parameters. 
            /// If null then the fuction is identical to the untransformed reference function.</param>
            public Quadratic2d(IAffineTransformation transf)
                : base(transf)
            {  }
            
            #endregion Construction


            #region Data

            /// <summary>Returns a short name of the function.</summary>
            public override string Name
            {
                get { return "Example quadratic polynomial 2D"; }
            }

            /// <summary>Returns a short description of the function.</summary>
            public override string Description
            {
                get { return "Example quadratic plynomial in 2D, possibly with Affinte transformation of parameters."; }
            }

            #endregion Data


            #region Evaluation

            /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
            public override double ReferenceValue(IVector parameters)
            {
                double x = parameters[0];
                double y = parameters[1];
                return 10 + x + 2 * x*x + y + x*y + y*y;
            }


            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool ValueDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
            }


            /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
            /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
            public override void ReferenceGradientPlain(IVector parameters, IVector gradient)
            {
                double x = parameters[0];
                double y = parameters[1];
                gradient[0] = 1 + 4 * x + y;
                gradient[1] = 1 + x + 2 * y;
            }

            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool GradientDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
            }
            

            /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
            /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
            public override void ReferenceHessianPlain(IVector parameters, IMatrix hessian)
            {
                double x = parameters[0];
                double y = parameters[1];
                hessian[0, 0] = 4;
                hessian[0, 1] = 1;
                hessian[1, 0] = 1;
                hessian[1, 1] = 2;
            }

            /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool HessianDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
            }

            #endregion Evaluation

        }  // class Quadratic2d


        /// <summary>Example quadratic polynomial in 3D.
        /// f(x,y,z) = x*x + 2*y*y + 4*z*z + x*y + 2*y*z + 4*z*x + x + y + z + 10</summary>
        /// $A Igor Dec10;
        public class Quadratic3d : ScalarFunctionBase, IScalarFunction
        {

            #region Construction 

            /// <summary>Creates a new untransformed example quadratic polynomial in 3D.</summary>
            public Quadratic3d()
            {  }


            /// <summary>Creates a new transformed example quadratic polynomial in 3D.
            /// Actual function is identical to the reference function applied to inverse transformed parameters.</summary>
            /// <param name="transf">Affine transformation that is applied to parameters. 
            /// If null then the fuction is identical to the untransformed reference function.</param>
            public Quadratic3d(IAffineTransformation transf)
                : base(transf)
            {  }

            #endregion Construction


            #region Data

            /// <summary>Returns a short name of the function.</summary>
            public override string Name
            {
                get { return "Example quadratic polynomial 3D"; }
            }

            /// <summary>Returns a short description of the function.</summary>
            public override string Description
            {
                get { return "Example quadratic plynomial in 3D, possibly with Affinte transformation of parameters."; }
            }

            #endregion Data


            #region Evaluation

            /// <summary>Returns the value of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where function is evaluated.</param>
            public override double ReferenceValue(IVector parameters)
            {
                double x = parameters[0];
                double y = parameters[1];
                double z = parameters[2];
                return 10 + x + x*x + y + x*y + 2 * y*y + z + 4 * x*z + 2 * y*z + 4*z*z;
            }


            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool ValueDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }
            }


            /// <summary>Calculates the first derivative (gradient) of this function at the specified parameter in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where derivatives are evaluated.</param>
            /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
            public override void ReferenceGradientPlain(IVector parameters, IVector gradient)
            {
                double x = parameters[0];
                double y = parameters[1];
                double z = parameters[2];
                gradient[0] = 1 + 2 * x + y + 4 * z;
                gradient[1] = 1 + x + 4 * y + 2 * z;
                gradient[2] = 1 + 4 * x + 2 * y + 8 * z;
            }

            /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool GradientDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
            }
            

            /// <summary>Calculates the second derivative (Hessian) of this function at the specified parameters in the reference coordinate system.</summary>
            /// <param name="parameters">Vector of parameters (in the REFERENCE system) where Hessian is evaluated.</param>
            /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
            public override void ReferenceHessianPlain(IVector parameters, IMatrix hessian)
            {
                double x = parameters[0];
                double y = parameters[1];
                double z = parameters[2];
                hessian[0, 0] = 2;
                hessian[0, 1] = 1;
                hessian[0, 2] = 4;
                hessian[1, 0] = 1;
                hessian[1, 1] = 4;
                hessian[1, 2] = 2;
                hessian[2, 0] = 4;
                hessian[2, 1] = 2;
                hessian[2, 2] = 8;
            }

            /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
            public override bool HessianDefined
            {
                get { return true; }
                protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
            }

            #endregion Evaluation

        }  // class Quadratic2d



    }  // class ScalarFunctionExamples





}