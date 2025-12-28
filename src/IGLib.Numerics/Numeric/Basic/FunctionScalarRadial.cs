
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{




    /// <summary>Radial scalar functions.</summary>
    /// $A Igor xx Nov10;
    public class ScalarFunctionRadial: ScalarFunctionBase, 
        IScalarFunction
    {

        #region Construction

        private ScalarFunctionRadial() { }

        /// <summary>Constructor. Creates a new radial function without specifying coordinate transformation.</summary>
        /// <param name="function">A real-valued function of one parameter that defined the radial function.</param>
        /// <param name="transf">Affine transformation of coordinates.
        /// Actual function is calculated as some reference function evaluated at inverse affine transformed parameters.
        /// If null then transformation is not applied.</param>
        public ScalarFunctionRadial(IRealFunction function, IAffineTransformation transf)
            : base(transf)
        {
            this.Function = function;
        }

        /// <summary>Constructor. Creates a new radial function without specifying coordinate transformation.</summary>
        /// <param name="function">A real-valued function of one parameter that defined the radial function.</param>
        public ScalarFunctionRadial(IRealFunction function): this(function, null)
        {
            this.Function = function;
        }

        #endregion Construction

        #region Data

        private IRealFunction _function;

        /// <summary>Gets or sets a real-valued function of one variable that defines the current radial function.</summary>
        public virtual IRealFunction Function
        {
            get { return _function; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Function of scalar argument that defines the radial basis function is not specified (null reference).");
                _function = value;
            }
        }

        /// <summary>Changes the 1D function that defines the current radial function.
        /// <para>Warning: the function should be set only exceptionally. Do not do this when immutability is required.</para></summary>
        /// <param name="function1d">Function to be set.</param>
        public virtual void SetFunction(IRealFunction function1d)
        {
            Function = function1d;
        }

        private double _epsilon;

        /// <summary>Gets or sets a small number used as criteria of where to calculate things (especially
        /// derivatives) in a special way in order to overcome singularities in expressions.
        /// For example, some expressions contain divisions by vector norm. When vector norm is close to 0,
        /// this results in nearly singular terms, which must be replaced by suitable limits.</summary>
        public double Epsilon
        {
            get { return _epsilon; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Small parameter defining environment where calculation is special due to singularities"
                        + Environment.NewLine + "Must be greater or equal to 0.");
                _epsilon = value;
            }
        }

        /// <summary>Returns a short name of the function.</summary>
        public override string Name
        {
            get { return "Radial scalar function"; }
        }

        /// <summary>Returns a short description of the function.</summary>
        public override string Description
        {
            get { return "A radial scalar function."; }
        }

        #endregion Data

        #region Evaluation

        /// <summary>Returns the value of this function at the specified parameter.</summary>
        public override double ReferenceValue(IVector x)
        {
            //if (x == null)
            //    throw new ArgumentNullException("Vector of parameters not specified when calculationg gradient of scalar function \"" + Name + "\" (nulll reference).");
            return Function.Value(x.Norm);
        }

        /// <summary>Tells whether value of the function is defined by implementation.</summary>
        public override bool ValueDefined { get { return true; }
            protected set { throw new InvalidOperationException("Can not set the flag for value defined."); }

        }

       /// <summary>Returns the first derivative of this function at the specified parameter.</summary>
        /// <param name="x">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="gradient">Vector where first derivatives (gradient) are stored.</param>
        public override void ReferenceGradientPlain(IVector x, IVector gradient)
        {
            //if (x == null)
            //    throw new ArgumentException("Vector of parameters not specified when calculationg gradient of scalar function \"" + Name +"\" (nulll reference).");
            int dim = x.Length;
            if (gradient == null)
                gradient = x.GetNew(dim);
            else if (gradient.Length!=dim)
                gradient = x.GetNew(dim);
            double norm = x.Norm;
            if (norm>Epsilon)
            {
                Vector.CopyPlain(x,gradient);
                gradient.Normalize();
                Vector.MultiplyPlain(gradient, Function.Derivative(norm), gradient);
            } else
            {
                for (int i = 0; i < dim; ++i)
                    gradient[i] = 0.0;
            }
        }

        /// <summary>Tells whether the first derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool GradientDefined { get { return Function.DerivativeDefined; }
            protected set { throw new InvalidOperationException("Can not set the flag for gradient defined."); }
        }


        /// <summary>Returns the second derivative (Hessian) of this function at the specified arameter.</summary>
        /// <param name="x">Vector of parameters where derivatives are evaluated.</param>
        /// <param name="hessian">Matrix where second derivatives (Hessian) are stored.</param>
        public override void ReferenceHessianPlain(IVector x, IMatrix hessian)
        {
            int dim = x.Length;
            double normx = x.Norm;
            double fDer = Function.Derivative(normx);
            double fSecondDer = Function.SecondDerivative(normx);
            if (normx < Epsilon)
            {
                // Close to zero, hessian matrix is diagonal, diagonal elements equal to second derivatives 
                // of 1-D function from which this scalar function is derived.
                hessian.SetZero();
                for (int i = 0; i < dim; ++i)
                    hessian[i, i] = fSecondDer;
            } else
            {
                double normxSquare = normx * normx;
                double normxCube = normxSquare * normx;
                double element;
                // Nondiagonal terms:
                for (int i=1; i<dim; ++i)
                    for (int j = 0; j < i; ++j)
                    {
                        double prod_x_i_j = x[i]*x[j];
                        element = fSecondDer * prod_x_i_j / normxSquare
                            - fDer * prod_x_i_j / normxCube;
                        hessian[i,j] = element;
                        hessian[j, i] = element;
                    }
                for (int i = 0; i < dim; ++i)
                {
                    double x_i_square = x[i]; // square of component
                    x_i_square *= x_i_square;
                    hessian[i,i]=fSecondDer*x_i_square/normxSquare
                        + fDer*((1/normx)+(x_i_square/normxCube));
                }
            }
        }

        /// <summary>Tells whether the second derivative is defined for this function (by implementation, not mathematically)</summary>
        public override bool HessianDefined { get { return Function.SecondDerivativeDefined; }
            protected set { throw new InvalidOperationException("Can not set the flag for Hessian defined."); }
        }


        #endregion Evaluation


    }  //  class ScalarFunctionRadial




    /// <summary>Base class for one parametric families of radial scalar functions with affine transformation of co-ordinates.
    /// Parameters that completely define the function out of parametric family of functions can be queried or set.
    /// Affine transformation of coordinates is included: the reference function evaluation must be defined 
    /// while the actual function is defined as that reference function of transformed coordinates.
    /// If transformation is not specified then function reduces to the reference one.</summary>
    /// <remarks>This base class does not implement data containing parameters. This enables to derive
    /// base classes with different representations of parameters, e.g. with only one number for one parametric functions.</remarks>
    /// $A Igor Dec10;
    public class ScalarFunctionRadialOneParametric : ScalarFunctionRadialParametric,
        IScalarFunction, IScalarFunctionParametric, IScalarFunctionOneParametric
    {

        #region Construction

        /// <summary>Auxiliary vector whose only function is to enable calling base constructors with parameters specified as vector.</summary>
        protected static Vector _parAux = new Vector(1);

        /// <summary>Default constructor is inaccessible because it has no meaning.</summary>
        private ScalarFunctionRadialOneParametric()
            : base(null, null, null)
        { }


        /// <summary>Constructor. Creates a new radial scalar function without specifying coordinate transformation.</summary>
        /// <param name="function">A real-valued function of one parameter that defined the radial function.</param>
        /// <param name="parameters">Parameters that define the function out of a parametric family of functions.</param>
        /// <param name="transf">Affine transformation of coordinates.
        /// Actual function is calculated as some reference function evaluated at inverse affine transformed parameters.
        /// If null then transformation is not applied.</param>
        public ScalarFunctionRadialOneParametric(IRealFunctionOneParametric function, IVector parameters, IAffineTransformation transf): 
            base(function, transf)
        {
            this.FunctionOneParametric = function;
            if (parameters!=null)
                this.Parameters = parameters;
        }

        /// <summary>Constructor. Creates a new parameterized radial scalar function without specifying coordinate transformation.</summary>
        /// <param name="function">A real-valued function of one parameter that defined the radial function.</param>
        /// <param name="transf">Affine transformation of coordinates.
        /// Actual function is calculated as some reference function evaluated at inverse affine transformed parameters.
        /// If null then transformation is not applied.</param>
        public ScalarFunctionRadialOneParametric(IRealFunctionOneParametric function, IAffineTransformation transf): 
            this(function, null /* parameters */, transf)
        {  }

        /// <summary>Constructor. Creates a new parameterized radial scalar function without specifying coordinate transformation.</summary>
        /// <param name="function">A real-valued function of one parameter that defined the radial function.</param>
        /// <param name="parameters">Parameters that define the function out of a parametric family of functions.</param>
        public ScalarFunctionRadialOneParametric(IRealFunctionOneParametric function, IVector parameters) : 
            this(function, parameters, null /* transformation */)
        {  }


        /// <summary>Constructor. Creates a new parameterized radial scalar function without specifying coordinate transformation
        /// or parameters.</summary>
        /// <param name="function">A real-valued function of one parameter that defined the radial function.</param>
        public ScalarFunctionRadialOneParametric(IRealFunctionOneParametric function) : 
            this(function, null /* parameters */, null /* transformation */)
        {  }

        #endregion Construction


        #region Data

        private IRealFunctionOneParametric _functionOneParametric;

        /// <summary>Gets or sets the parametrivc real-valued function of one variable that defines the current radial function.</summary>
        public virtual IRealFunctionOneParametric FunctionOneParametric
        {
            get { return _functionOneParametric; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Oneparametric real function of real argument that defines a radial function is not specified (null reference).");
                _functionOneParametric = value;
                base.FunctionParametric = value;
            }
        }

        /// <summary>Gets or sets the oneparametric real-valued function of one variable that defines the current radial function.
        /// When setting, the argument must be of appropriate type (IRealFunctionOneParametric).</summary>
        public override IRealFunctionParametric FunctionParametric
        {
            get { return _functionOneParametric; }
            protected set
            {
                IRealFunctionOneParametric func = value as IRealFunctionOneParametric;
                if (value == null)
                    throw new ArgumentNullException("Oneparametric real function of real argument that defines a radial function is not specified (null reference).");
                if (func == null)
                    throw new ArgumentException("Incorrect Real function of one variable, not oneparametric.");
                _functionOneParametric = func;
            }
        }

        /// <summary>Gets or sets a real-valued function of one variable that defines the current radial function.
        /// When setting, the argument must be of appropriate type (IRealFunctionOneParametric).</summary>
        public override IRealFunction Function
        {
            get { return _functionOneParametric; }
            protected set
            {
                IRealFunctionOneParametric func = value as IRealFunctionOneParametric;
                if (value == null)
                    throw new ArgumentNullException("Oneparametric real function of real argument that defines a radial function is not specified (null reference).");
                if (func == null)
                    throw new ArgumentException("Incorrect Real function of one variable, not oneparametric.");
                _functionOneParametric = func;
            }
        }


        /// <summary>Gets or sets parameters that define the specific function out of parametric familiy of scalar functions.</summary>
        public override IVector Parameters
        {
            get { return new Vector(1, Parameter); }
            set
            {
                //if (value == null)
                //    throw new ArgumentNullException("1D vector of parameters of a parameterized function not specified (null reference).");
                if (value.Length != 1)
                    throw new ArgumentException("Invalid dimension " + value.Length + " of parameters for one-parametric family of functions (should be 1).");
                Parameter = value[0];
            }
        }

        /// <summary>Gets or sets the (only) parameter that defines the current function out of parametric family of functions.</summary>
        public virtual double Parameter
        {
            get { return FunctionOneParametric.Parameter; }
            set { FunctionOneParametric.Parameter = value; }
        }

        /// <summary>Gets number of parameters that define the current function out of parametric 
        /// family of scalar functions.
        /// Always evaluates to 1 because this is a one parametric family of functions.</summary>
        public override int NumParameters
        {
            get { return 1; }
        }

        /// <summary>Returns parameters of the current parametric function as an array.
        /// Since this class is for one parametric family of functions, an array of length 1 is always returned.</summary>
        public override double[] GetParameters()
        {
            double[] ret = new double[1];
            ret[0] = Parameter;
            return ret;
        }

        /// <summary>Sets parameters of the current parametric functions, where parameters are specified as array.</summary>
        /// <param name="parameters">Array of parameters. Array length must be 1 because this class represents a one parametric family..</param>
        public override void SetParameters(double[] parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("Parameter of the parameterized scalar function is not specified (null argument).");
            if (parameters.Length != 1)
                throw new ArgumentException("Invalid length of parameters array " + parameters.Length + " for one parametric family of functions (should be 1).");
            Parameter = parameters[0];
        }

        /// <summary>Returns value of the specified parameter of the parameterized function.</summary>
        /// <param name="which">Specifies which parameter is returned, must be 0 because this class represents a one parametric family of functions.</param>
        public override double GetParameter(int which)
        {
            if (which != 0)
                throw new ArgumentException("Invalid parameter index " + which + " for one parametric family of functions (should be 0).");
            return Parameter;
        }

        /// <summary>Sets the specific parameters of the parameterized function.</summary>
        /// <param name="which">Specifies which parameter is set, must be 0 because this class represents a one parametric family of functions.</param>
        /// <param name="value">Value of the specified parameter.</param>
        public override void SetParameter(int which, double value)
        {
            if (which != 0)
                throw new ArgumentException("Invalid parameter index " + which + " for one parametric family of functions (should be 0).");
            Parameter = value;
        }

        #endregion Data

    }  // class ScalarFunctionRadialOneParametric



    /// <summary>Parametric scalar function.</summary>
    public class ScalarFunctionRadialParametric: ScalarFunctionRadial,
        IScalarFunction, IScalarFunctionParametric
    {

        #region Construction
        
        private ScalarFunctionRadialParametric(): base(null, null) { }

        /// <summary>Constructor. Creates a new radial scalar function without specifying coordinate transformation.</summary>
        /// <param name="function">A real-valued function of one parameter that defined the radial function.</param>
        /// <param name="parameters">Parameters that define the function out of a parametric family of functions.</param>
        /// <param name="transf">Affine transformation of coordinates.
        /// Actual function is calculated as some reference function evaluated at inverse affine transformed parameters.
        /// If null then transformation is not applied.</param>
        public ScalarFunctionRadialParametric(IRealFunctionParametric function, IVector parameters, IAffineTransformation transf) : 
            base(function, transf)
        {
            this.FunctionParametric = function;
            if (parameters!=null)
                this.Parameters = parameters;
        }

        /// <summary>Constructor. Creates a new parameterized radial scalar function without specifying coordinate transformation.</summary>
        /// <param name="function">A real-valued function of one parameter that defined the radial function.</param>
        /// <param name="transf">Affine transformation of coordinates.
        /// Actual function is calculated as some reference function evaluated at inverse affine transformed parameters.
        /// If null then transformation is not applied.</param>
        public ScalarFunctionRadialParametric(IRealFunctionParametric function, IAffineTransformation transf) : 
            this(function, null /* parameters */, transf)
        {  }

        /// <summary>Constructor. Creates a new parameterized radial scalar function without specifying coordinate transformation.</summary>
        /// <param name="function">A real-valued function of one parameter that defined the radial function.</param>
        /// <param name="parameters">Parameters that define the function out of a parametric family of functions.</param>
        public ScalarFunctionRadialParametric(IRealFunctionParametric function, IVector parameters): 
            this(function, parameters, null /* transformation */)
        {  }


        /// <summary>Constructor. Creates a new parameterized radial scalar function without specifying coordinate transformation
        /// or parameters.</summary>
        /// <param name="function">A real-valued function of one parameter that defined the radial function.</param>
        public ScalarFunctionRadialParametric(IRealFunctionOneParametric function): 
            this(function, null /* parameters */, null /* transformation */)
        {  }

        #endregion Construction


        #region Data

        private IRealFunctionParametric _functionParametric;

        /// <summary>Gets or sets the parametrivc real-valued function of one variable that defines the current radial function.</summary>
        public virtual IRealFunctionParametric FunctionParametric
        {
            get { return _functionParametric; }
            protected set
            {
                if (value == null)
                    throw new ArgumentNullException("Parametric real function of real argument that defines a radial function is not specified (null reference).");
                _functionParametric = value;
            }
        }

        /// <summary>Gets or sets a real-valued function of one variable that defines the current radial function.
        /// When setting, the argument must be of appropriate type (IRealFunctionParametric).</summary>
        public override IRealFunction Function
        {
            get { return _functionParametric; }
            protected set
            {
                IRealFunctionParametric func = value as IRealFunctionParametric;
                if (value == null)
                    throw new ArgumentNullException("Real function of one variable that defines a radial function is not specified (null reference).");
                if (func == null)
                    throw new ArgumentException("Incorrect Real function of one variable, not parametric.");
                _functionParametric = func;
            }
        }

        #region IScalarFunctionParametric

        /// <summary>Parameters that define the specific function out of parametric familiy of scalar functions.</summary>
        public virtual IVector Parameters
        {
            get { return FunctionParametric.Parameters; }
            set { FunctionParametric.Parameters = value; }
        }

        /// <summary>Gets number of parameters that define the current function out of parametric 
        /// family of scalar functions.</summary>
        public virtual int NumParameters
        {
            get
            {
                if (Parameters == null)
                    return 0;
                else
                    return Parameters.Length;
            }
        }

        /// <summary>Returns parameters of the current parametric function as an array.</summary>
        /// <remarks>Usually, implementations use vector as natural representation of parameters, therefore this function
        /// creates an array and copies values.</remarks>
        public virtual double[] GetParameters()
        {
            IVector param = Parameters;
            int dim = param.Length;
            double[] ret = new double[dim];
            for (int i = 0; i < dim; ++i)
            {
                ret[i] = param[i];
            }
            return ret;
        }

        /// <summary>Sets parameters of the current parametric functions, where parameters are specified as array.</summary>
        /// <param name="parameters">Array of parameters. Array length must correspond to actual number of parameters.</param>
        public virtual void SetParameters(double[] parameters)
        {
            IVector param = this.Parameters;
            int dim = parameters.Length;
            if (param == null)
                this.Parameters = (param = new Vector(dim));
            else if (param.Length != dim)
                this.Parameters = (param = param.GetNew(dim));
            for (int i = 0; i < dim; ++i)
                param[i] = parameters[i];
        }

        /// <summary>Returns value of the specified parameter of the parameterized function.</summary>
        /// <param name="which">Specifies which parameter is returned.</param>
        public virtual double GetParameter(int which)
        {
            return Parameters[which];
        }

        /// <summary>Sets the specific parameters of the parameterized function.</summary>
        /// <param name="which">Specifies which parameter is set.</param>
        /// <param name="value">Value of the specified parameter.</param>
        public virtual void SetParameter(int which, double value)
        {
            Parameters[which] = value;
        }

        #endregion IScalarFunctionParametric

        #endregion Data

    }


}


