
// PARAMETERIZED FUNCTONS (parametric families of functions).
// REMARK: Class structure of this file follows equivalent concepts to those related to ScalarFunctionParametric and related classes/interfaces

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{



    /// <summary>Parameterized real-valued functions of single variable.</summary>
    /// $A Igor Dec10;
    public interface IRealFunctionParametric: IRealFunction
    {

        #region Data

        /// <summary>Parameters that define the specific function out of parametric familiy of functions.</summary>
        IVector Parameters
        {
            get;
            set;
        }

        /// <summary>Gets number of parameters that define the specific function out of parametric family of functions.</summary>
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

    } // interface IRealFunctionParametric


    /// <summary>Parameterized real-valued functions of single variable.</summary>
    /// $A Igor Dec10;
    public interface IRealFunctionOneParametric : IRealFunctionParametric,
        IRealFunction
    {

        /// <summary>Gets or sets the (only) parameter that defines the current function out of one parametric family of functions.</summary>
        double Parameter
        {
            get;
            set;
        }
    }


    /// <summary>Base class for parameterized real-valued functions of single variable.
    /// Parameters that completely define the function out of parametric family of functions can be queried or set.
    /// Parameters are represented and stored as vector.</summary>
    /// <remarks>This base class does not implement data containing parameters. This enables to derive
    /// base classes with different representations of parameters, e.g. with only one number for one parametric functions.</remarks>
    /// $A Igor Dec10;
    public abstract class RealFunctionParametric: RealFunctionParametricBase,
        IRealFunction, IRealFunctionParametric
    {

        #region Construction

        /// <summary>Default constructor is inaccessible because it has no meaning.</summary>
        private RealFunctionParametric()
            : base(null)
        { }


        /// <summary>Constructor. 
        /// Creates a new parametric function with the specified parameters and transformation of coordinates.</summary>
        /// <param name="parameters">Vector of parameters that define a specific function out of a parametric family of functions.</param>
        public RealFunctionParametric(IVector parameters):
            base(parameters)
        { }

        #endregion Construction


        #region data

        IVector _parameters;

        /// <summary>Parameters that define the specific function out of parametric familiy of functions.</summary>
        public override IVector Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        #endregion data

    }  // class RealFunctionParametric




    /// <summary>Base class for one parametric families of real-valued functions of single variable.
    /// Parameters that completely define the function out of parametric family of functions can be queried or set.</summary>
    /// <remarks>This base class does not implement data containing parameters. This enables to derive
    /// base classes with different representations of parameters, e.g. with only one number for one parametric functions.</remarks>
    /// $A Igor Dec10;
    public abstract class RealFunctionOneParametric : RealFunctionParametricBase,
        IRealFunction, IRealFunctionParametric, IRealFunctionOneParametric
    {


        #region Construction

        /// <summary>Auxiliary vector whose only function is to enable calling base constructors with parameters specified as vector.</summary>
        protected static Vector _parAux = new Vector(1);

        /// <summary>Default constructor is inaccessible because it has no meaning.</summary>
        private RealFunctionOneParametric()
            : base(null)
        {  }


        /// <summary>Cobnstructor. 
        /// Creates a new one parametric function with the specified parameter.</summary>
        /// <param name="parameter">Parameter that define a specific function out of a parametric family of functions.</param>
        public RealFunctionOneParametric(double parameter):
            base(_parAux)
        {
            this.Parameter = parameter;
        }

        /// <summary>Cobnstructor. 
        /// Creates a new one parametric function with the specified parameters.</summary>
        /// <param name="parameters">Vector of parameters that define a specific function out of a parametric family of functions.</param>
        public RealFunctionOneParametric(IVector parameters):
            base(parameters)
        { }

        #endregion Construction


        #region Data

        private double _parameter;

        /// <summary>Gets or sets parameters that define the specific function out of parametric familiy of functions.
        /// Usually a reference is set or returned.</summary>
        public override IVector Parameters
        {
            get { return new Vector(1, _parameter); }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("1D vector of parameters of a parameterized function not specified (null reference).");
                if (value.Length != 1)
                    throw new ArgumentException("Invalid dimension " + value.Length + " of parameters for one-parametric family of functions (should be 1).");
                _parameter = value[0];
            }
        }

        /// <summary>Gets or sets the (only) parameter that defines the current function out of parametric family of functions.</summary>
        public virtual double Parameter
        {
            get { return _parameter; }
            set { _parameter = value; }
        }

        /// <summary>Gets number of parameters that define the current function out of parametric 
        /// family of functions.
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
                throw new ArgumentNullException("Parameter of the parameterized function is not specified (null argument).");
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

    }  // class RealFunctionOneParametricBase



    /// <summary>Base class for parameterized real-valued functions of single variable.
    /// Parameters that completely define the function out of parametric family of functions can be queried or set.</summary>
    /// <remarks>This base class does not implement data containing parameters. This enables to derive
    /// base classes with different representations of parameters, e.g. with only one number for one parametric functions.</remarks>
    /// $A Igor Dec10;
    public abstract class RealFunctionParametricBase : RealFunctionBase,
        IRealFunction, IRealFunctionParametric
    {

        #region Construction

        /// <summary>Default constructor is inaccessible because it has no meaning.</summary>
        private RealFunctionParametricBase()
        { }

        /// <summary>Cobnstructor. 
        /// Creates a new parametric real-valued functions of single variable with the specified parameters.</summary>
        /// <param name="parameters">Vector of parameters that define a specific function out of a parametric family of functions.</param>
        public RealFunctionParametricBase(IVector parameters):
            base()
        {
            this.Parameters = parameters;
        }

        #endregion Construction


        #region Data

        /// <summary>Gets or sets parameters that define the specific function out of parametric familiy of functions.
        /// Usually a reference is set or returned.</summary>
        public abstract IVector Parameters
        {
            get;
            set;
        }

        /// <summary>Gets number of parameters that define the current function out of parametric 
        /// family of functions.</summary>
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

        #endregion Data

    }  // class RealFunctionParametricBase


}