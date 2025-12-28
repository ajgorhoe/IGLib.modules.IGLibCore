
#nullable disable

// PARAMETERIZED SCALAR FUNCTONS (parametric families of scalar functions).

// REMARK: Class structure of this file follows equivalent concepts to those related to FunctionParametric and related classes/interfaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{




    /// <summary>Parameterized scalar functions with affine transformation of coordinates.</summary>
    /// $A Igor xx Dec10;
    public interface IScalarFunctionParametric : IScalarFunction
    {

        #region Data

        /// <summary>Parameters that define the specific function out of parametric familiy of scalar functions.</summary>
        IVector Parameters
        {
            get;
            set;
        }

        /// <summary>Gets number of parameters that define the specific function out of parametric family of scalar functions.</summary>
        int NumParameters
        {
            get;
        }

        /// <summary>Returns parameters of the current parametric function as an array.</summary>
        /// <remarks>Usually, implementations use vector as natural representation of parameters, therefore this function
        /// creates an array and copies values.</remarks>
        double[] GetParameters();

        /// <summary>Sets parameters of the current parametric functions, where parameters are specified as array.</summary>
        /// <param name="parameters">Array of parameters. Array length must correspond to actual number of parameters.</param>
        void SetParameters(double[] parameters);

        /// <summary>Returns value of the specified parameter of the parameterized function.</summary>
        /// <param name="which">Specifies which parameter is returned.</param>
        double GetParameter(int which);

        /// <summary>Sets the specific parameters of the parameterized function.</summary>
        /// <param name="which">Specifies which parameter is set.</param>
        /// <param name="value">Value of the specified parameter.</param>
        void SetParameter(int which, double value);

        #endregion Data

    } // interface IScalarFunctionParametric


    public interface IScalarFunctionOneParametric : IScalarFunctionParametric
    {

        /// <summary>Gets or sets the (only) parameter that defines the current function out of parametric family of functions.</summary>
        double Parameter
        {
            get;
            set;
        }

    }


    /// <summary>Base class for parameterized scalar functions with affine transformation of co-ordinates.
    /// Parameters that completely define the function out of parametric family of functions can be queried or set.
    /// Parameters are represented and stored as vector.
    /// Affine transformation of coordinates is included: the reference function evaluation must be defined 
    /// while the actual function is defined as that reference function of transformed coordinates.
    /// If transformation is not specified then function reduces to the reference one.</summary>
    /// <remarks>This base class does not implement data containing parameters. This enables to derive
    /// base classes with different representations of parameters, e.g. with only one number for one parametric functions.</remarks>
    /// $A Igor Dec10;
    public abstract class ScalarFunctionParametric : ScalarFunctionParametricBase,
        IScalarFunction, IScalarFunctionParametric
    {

        #region Construction

        /// <summary>Default constructor is inaccessible because it has no meaning.</summary>
        private ScalarFunctionParametric(): base(null, null)
        {  }

        /// <summary>Constructor. Does not define affine transformation of the function.</summary>
        /// <param name="parameters">Vector of parameters that define a specific function out of a parametric family of functions.</param>
        public ScalarFunctionParametric(IVector parameters): 
            base(parameters)
        {  }

        /// <summary>Cobnstructor. 
        /// Creates a new parametric scalar function with the specified parameters and transformation of coordinates.</summary>
        /// <param name="parameters">Vector of parameters that define a specific function out of a parametric family of functions.</param>
        /// <param name="transformation">Affine transformation of coordinates.
        /// Actual function is calculated as some reference function evaluated at inverse affine transformed parameters.
        /// If null then transformation is not applied.</param>
        public ScalarFunctionParametric(IVector parameters, IAffineTransformation transformation):
            base(parameters, transformation)
        {  }

        #endregion Construction


        #region data

        IVector _parameters;

        /// <summary>Parameters that define the specific function out of parametric familiy of scalar functions.</summary>
        public override IVector Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        #endregion data

    }  // class ScalarFunctionParametric




    /// <summary>Base class for one parametric families of scalar functions with affine transformation of co-ordinates.
    /// Parameters that completely define the function out of parametric family of functions can be queried or set.
    /// Affine transformation of coordinates is included: the reference function evaluation must be defined 
    /// while the actual function is defined as that reference function of transformed coordinates.
    /// If transformation is not specified then function reduces to the reference one.</summary>
    /// <remarks>This base class does not implement data containing parameters. This enables to derive
    /// base classes with different representations of parameters, e.g. with only one number for one parametric functions.</remarks>
    /// $A Igor Dec10;
    public abstract class ScalarFunctionOneParametric: ScalarFunctionParametricBase,
        IScalarFunction, IScalarFunctionParametric
    {

        #region Construction

        /// <summary>Auxiliary vector whose only function is to enable calling base constructors with parameters specified as vector.</summary>
        protected static Vector _parAux = new Vector(1);

        /// <summary>Default constructor is inaccessible because it has no meaning.</summary>
        private ScalarFunctionOneParametric(): base(null,null)
        { }

        /// <summary>Constructor. Does not define affine transformation of the function.</summary>
        /// <param name="parameter">Parameter that define a specific function out of a parametric family of functions.</param>
        public ScalarFunctionOneParametric(double parameter): base(_parAux)
        {
            // TODO: check if the call of base function with that argument actually makes sense!
            this.Parameter = parameter;
        }

        /// <summary>Constructor. 
        /// Creates a new parametric scalar function with the specified parameter and transformation of coordinates.</summary>
        /// <param name="parameter">Parameter that defines a specific function out of a parametric family of functions.</param>
        /// <param name="transformation">Affine transformation of coordinates.
        /// Actual function is calculated as some reference function evaluated at inverse affine transformed parameters.
        /// If null then transformation is not applied.</param>
        public ScalarFunctionOneParametric(double parameter, IAffineTransformation transformation):
            base(_parAux, transformation)
        {
            this.Parameter = parameter;
        }

        /// <summary>Constructor. Does not define affine transformation of the function.</summary>
        /// <param name="parameters">Vector of parameters that define a specific function out of a parametric family of functions.</param>
        public ScalarFunctionOneParametric(IVector parameters): 
            base(parameters)
        {  }

        /// <summary>Cobnstructor. 
        /// Creates a new parametric scalar function with the specified parameters and transformation of coordinates.</summary>
        /// <param name="parameters">Vector of parameters that define a specific function out of a parametric family of functions.</param>
        /// <param name="transformation">Affine transformation of coordinates.
        /// Actual function is calculated as some reference function evaluated at inverse affine transformed parameters.
        /// If null then transformation is not applied.</param>
        public ScalarFunctionOneParametric(IVector parameters, IAffineTransformation transformation):
            base(parameters, transformation)
        {  }

        #endregion Construction


        #region Data

        private double _parameter;

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
            get { return _parameter; }
            set { _parameter = value; }
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
            if (parameters.Length!=1)
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

    }  // class ScalarFunctionOneParametric



    /// <summary>Base class for parameterized scalar functions with affine transformation of co-ordinates.
    /// Parameters that completely define the function out of parametric family of functions can be queried or set.
    /// Affine transformation of coordinates is included: the reference function evaluation must be defined 
    /// while the actual function is defined as that reference function of transformed coordinates.
    /// If transformation is not specified then function reduces to the reference one.</summary>
    /// <remarks>This base class does not implement data containing parameters. This enables to derive
    /// base classes with different representations of parameters, e.g. with only one number for one parametric functions.</remarks>
    /// $A Igor Dec10;
    public abstract class ScalarFunctionParametricBase:  ScalarFunctionBase,
        IScalarFunction, IScalarFunctionParametric
    {

        #region Construction

        /// <summary>Default constructor is inaccessible because it has no meaning.</summary>
        private ScalarFunctionParametricBase()
        {  }

        /// <summary>Constructor. Does not define affine transformation of the function.</summary>
        /// <param name="parameters">Vector of parameters that define a specific function out of a parametric family of functions.</param>
        public ScalarFunctionParametricBase(IVector parameters)
            : this(parameters, null)
        {  }

        /// <summary>Cobnstructor. 
        /// Creates a new parametric scalar function with the specified parameters and transformation of coordinates.</summary>
        /// <param name="parameters">Vector of parameters that define a specific function out of a parametric family of functions.</param>
        /// <param name="transformation">Affine transformation of coordinates.
        /// Actual function is calculated as some reference function evaluated at inverse affine transformed parameters.
        /// If null then transformation is not applied.</param>
        public ScalarFunctionParametricBase(IVector parameters, IAffineTransformation transformation):
            base(transformation)
        {
            this.Parameters = parameters;
        }

        #endregion Construction

        
        #region Data

        /// <summary>Gets or sets parameters that define the specific function out of parametric familiy of scalar functions.
        /// Usually a reference is set or returned.</summary>
        public abstract IVector Parameters
        {
            get;
            set;
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
            for (int i=0; i<dim; ++i)
            {
                ret[i]=param[i];
            }
            return ret;
        }

        /// <summary>Sets parameters of the current parametric functions, where parameters are specified as array.</summary>
        /// <param name="parameters">Array of parameters. Array length must correspond to actual number of parameters.</param>
        public virtual void SetParameters(double[] parameters)
        {
            IVector param = this.Parameters;
            int dim=parameters.Length;
            if (param==null)
                this.Parameters=(param=new Vector(dim));
            else if (param.Length!=dim)
                this.Parameters=(param=param.GetNew(dim));
            for (int i=0; i<dim; ++i)
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

        #endregion Data

    }  // class ScalarFunctionParametricBase


}