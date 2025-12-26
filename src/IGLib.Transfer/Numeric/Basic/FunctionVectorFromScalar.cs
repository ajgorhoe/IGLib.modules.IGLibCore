using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    public class VectorFunctionFromScalar: VectorFunctionBaseComponentWise, IVectorFunction
    {

        protected VectorFunctionFromScalar()
        {  }


        public VectorFunctionFromScalar(params IScalarFunction[] functions): this()
        {
            if (functions != null)
            {
                for (int i = 0; i < functions.Length; ++i)
                {
                    this.ScalarFunctions.Add(functions[i]);
                    //this.NumParameters = functions[i].
                }
            }
        }


        private List<IScalarFunction> _scalarFunctions = new List<IScalarFunction>();
        

        protected List<IScalarFunction> ScalarFunctions
        {
            get {
                if (_scalarFunctions == null)
                {
                    lock(Lock)
                    {
                        if (_scalarFunctions == null)
                            _scalarFunctions = new List<IScalarFunction>();
                    }
                }
                return _scalarFunctions; }
        }

        /// <summary>Returns number of values, i.e. dimension of function codomain.</summary>
        public override int NumValues
        {
            get
            {
                return ScalarFunctions.Count;
            }
            protected set
            {
                if (value != _scalarFunctions.Count)
                    throw new InvalidCastException("Number of function values can not be set to " + value 
                        + " for this vector function, must be equal to nmber of functons(" + _scalarFunctions.Count + ").");
                //base.NumValues = value;
            }
        }





        //public override double Value(IVector parameters, int which)
        //{
        //    return ScalarFunctions[which].Value(parameters);
        //}


        //public override double Derivative(IVector parameters, int which, int component)
        //{
        //    IVector grad = null;
        //    ScalarFunctions[which].Gradient(parameters, ref grad);
        //    return grad[component];
        //}

        //public override double SecondDerivative(IVector parameters, int which, int rowNum, int columnNum)
        //{
        //    IMatrix hessian = null;
        //    ScalarFunctions[which].Hessian(parameters, ref hessian);
        //    return hessian[rowNum, columnNum];
        //}





        public override bool ValueDefined
        {
            get
            {
                bool ret = true;
                for (int i = 1; i < NumValues && ret; ++i)
                {
                    if (!ScalarFunctions[i].ValueDefined)
                        ret = false;
                }
                return ret;
            }

            protected set
            {
                base.ValueDefined = value;
            }
        }


        public override bool DerivativeDefined
        {
            get
            {
                bool ret = true;
                for (int i = 1; i < NumValues && ret; ++i)
                {
                    if (!ScalarFunctions[i].GradientDefined)
                        ret = false;
                }
                return ret;
            }
            protected set
            {
                base.DerivativeDefined = value;
            }
        }



        public override bool SecondDerivativeDefined
        {
            get
            {
                bool ret = true;
                for (int i = 1; i < NumValues && ret; ++i)
                {
                    if (!ScalarFunctions[i].HessianDefined)
                        ret = false;
                }
                return ret;
            }
            protected set
            {
                base.SecondDerivativeDefined = value;
            }
        }



        public override double Value(IVectorFunctionResults evaluationData, int which)
        {
            double ret = ScalarFunctions[which].Value(evaluationData.Parameters);
            evaluationData.SetValue(which, ret);
            return ret;
        }


        public override double Derivative(IVectorFunctionResults evaluationData, int which, int component)
        {
            IVector grad = null;
            ScalarFunctions[which].Gradient(evaluationData.Parameters, ref grad);
            evaluationData.SetGradient(which, grad);
            return grad[component];
        }

        public override double SecondDerivative(IVectorFunctionResults evaluationData, int which, int rowNum, int columnNum)
        {
            IMatrix hessian = null;
            ScalarFunctions[which].Hessian(evaluationData.Parameters, ref hessian);
            evaluationData.SetHessian(which, hessian);
            return hessian[rowNum, columnNum];
        }


    }



}
