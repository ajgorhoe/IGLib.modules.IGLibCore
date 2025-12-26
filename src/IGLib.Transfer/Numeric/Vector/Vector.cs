// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

// VECTORS; This module is based on teh MathNet external library.


using System;
using System.Collections.Generic;
using System.Linq;

//using Vector_MathNet = MathNet.Numerics.LinearAlgebra.Vector;

using Vector_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.DenseVector;
using VectorBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Vector<double>;

namespace IG.Num
{



    /// <summary>Real vector class.</summary>
    /// $A Igor Jan08 Jul10 Nov10;
    [Serializable]
    public class Vector : VectorBase, IVector, ICloneable
    {

        // protected internal MathNetVector Base = null;


        #region Construction

        /// <summary>Creates a new vector with dimension 0.
        /// <para>Such a vector can serve for future reallocation.</para></summary>
       protected internal Vector()
        {
            _elements = null;
            _length = 0;
        }

       /// <summary>Constructs a vector from aanother array.</summary>
       /// <param name="vec">Vector whose components are copied to the current vector.</param>
       /// <seealso cref="Create(double[])"/>
       public Vector(IVector vec)
       {
           if (vec == null)
               throw new ArgumentNullException("Vector creation: array of components not specified (null argument).");
           int length = vec.Length;
           if (length <= 0)
               throw new ArgumentException("Vector creation: array of components contains no values, can not create a vector with dimensionality 0.");
           _length = vec.Length;
           _elements = new double[_length];
           for (int i = 0; i < _length; ++i)
               _elements[i] = vec[i];
       }

        /// <summary>Constructs an n-dimensional vector of zeros.</summary>
        /// <param name="n">Dimensionality of vector.</param>
        public Vector(int n)
        {
            if (n <= 0)
                throw new ArgumentException("Can not create a vector with dimension less thatn 1. Provided dimensionality: " 
                    + n.ToString() + ".");
            _length = n;
            _elements = new double[_length];
            for (int i = 0; i < _length; ++i)
                _elements[i] = 0;
        }

        /// <summary>Constructs an n-dimensional unit vector for i'th coordinate.</summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <param name="which">Specifies which unit vector is constructed (which components equals one).</param>
        public Vector(int n, int which)
        {
            if (n <= 0)
                throw new ArgumentException("Can not create a vector with dimension less than 1. Provided dimension: "
                    + n.ToString() + ".");
            if (which < 0 || which >= n - 1)
                throw new ArgumentException("Creation of unit vector: index out of range: "
                    + which.ToString() + ", should be between 0 and " + (n - 1).ToString() + ".");
            _length = n;
            _elements = new double[_length];
            for (int i = 0; i < _length; ++i)
                _elements[i] = 0;
            _elements[which] = 1.0;
        }

        /// <summary>Constructs an n-dimensional constant vector with all components initialized to the specified value.</summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <param name="value">Value to which all components are set.</param>
        public Vector(int n, double value)
        {  
            if (n <= 0)
                throw new ArgumentException("Can not create a vector with dimensionality less thatn 1. Provided dimensionality: "
                    + n.ToString() + ".");
            _length = n;
            _elements = new double[_length];
            for (int i = 0; i < _length; ++i)
                _elements[i] = value;
        }


        /// <summary>Constructs a vector from a list of elements by copying its elements.</summary>
        /// <param name="components">Object implementing IList interface that contains vector elements.</param>
        public Vector(IList<double> components)
        {
            if (components == null)
                throw new ArgumentNullException("Vector creation: list of components not specified (null argument).");
            int length = components.Count;
            if (length <= 0)
                throw new ArgumentException("Vector creation: list of components contains no values; can not create a vector with dimensionality 0.");
            _length = length;
            _elements = new double[_length];
            for (int i = 0; i < length; ++i)
                _elements[i] = components[i];
        }

        /// <summary>Constructs a vector from a 1-D array, either by directly using the array or by copying its elements.</summary>
        /// <param name="components">One-dimensional array of vector elements.</param>
        /// <param name="copyElements">If true then elements of the provided array are copied (default), otherwise the array
        /// is directly used as internal structure containing elementts.</param>
        public Vector (double [] components, bool copyElements = true)
        {
            if (components==null)
                throw new ArgumentNullException("Vector creation: array of components not specified (null argument).");
            int length=components.Length;
            if (length <= 0)
                throw new ArgumentException("Vector creation: array of components contains no values; can not create a vector with dimensionality 0.");
            _length = length;
            if (copyElements)
            {
                _elements = new double[_length];
                for (int i = 0; i < length; ++i)
                    _elements[i] = components[i];
            }
            else
            {
                _elements = components;
            }
        }

        /// <summary>Constructs a vector from a 1-D array or from listed variable number of components. 
        /// <para>Elements of array are copied.</para></summary>
        /// <param name="components">One-dimensional array of vector elements.</param>
        /// <seealso cref="Create(double[])"/>
        public Vector(params double[] components) 
        {
            if (components==null)
                throw new ArgumentNullException("Vector creation: array of components not specified (null argument).");
            int length=components.Length;
            if (length <= 0)
                throw new ArgumentException("Vector creation: array of components contains no values; can not create a vector with dimensionality 0.");
            _length = length;
            _elements = new double[length];
            for (int i = 0; i < length; ++i)
                _elements[i] = components[i];
        }

        /// <summary>Constructs a vector from a 1-D array, directly using the provided array as internal data structure.</summary>
        /// <param name="vec">One-dimensional array of doubles.</param>
        /// <seealso cref="Create(double[])"/>
        protected internal Vector(VectorBase_MathNetNumerics vec) 
        {
            if (vec==null)
                throw new ArgumentNullException("Vector creation: array of components not specified (null argument).");
            int length=vec.Count;
            if (length <= 0)
                throw new ArgumentException("Vector creation: array of components contains no values, can not create a vector with dimensionality 0.");
            _length = vec.Count;
            _elements = new double[_length];
            for (int i = 0; i < _length; ++i)
                _elements[i] = vec[i];
        }

        #region StaticConstruction


        /// <summary>Constructs a vector from a 1-D array.</summary>
        public static Vector Create(double[] components)
        {
            return new Vector(components);
        }



        /// <summary>Constructs a vector as a copy of a MathNetVector object.</summary>
        protected internal static Vector Create(VectorBase_MathNetNumerics vec)
        {
            Vector ret = null;
            if (vec == null)
                throw new ArgumentNullException("Vector is not specified (null reference).");
            if (vec.Count <= 0)
                throw new ArgumentNullException("Vector length is 0.");
            ret = new Vector(vec.Count);
            for (int i = 0; i < vec.Count; ++i)
                ret[i] = vec[i];
            return ret;
        }


        /// <summary>Constructs a vector as a copy of another Vector object.</summary>
        /// <param name="vec">Vector whose elements are used for the newly created vector.</param>
        public static Vector Create(IVector vec)
        {
            return new Vector(vec);
        }



        /// <summary>Generates vector with random elements uniformly distributed on [0, 1).</summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <param name="rand">Random number generator used to generate vector elements. If null then a global generator is taken.</param>
        /// <returns>An n-dimensional vector with uniformly distributedrandom elements in <c>[0, 1)</c> interval.</returns>
        public static Vector Random(int n, IRandomGenerator rand = null)
        {
            if (rand == null)
                rand = RandomGenerator.Global;
            Vector ret = new Vector(n);
            ret.SetRandom(rand);
            return ret;
        }

        /// <summary>Generates an d2-dimensional vector filled with 1.</summary>
        /// <param name="n">Dimensionality of vector.</param>
        public static Vector Ones(int n)
        {
            return new Vector(n, 1.0);
        }

        /// <summary>Generates an d2-dimensional vector filled with 0.</summary>
        /// <param name="n">Dimensionality of vector.</param>
        public static Vector Zeros(int n)
        {
            return new Vector(n, 0.0);
        }

        /// <summary>Generates an d2-dimensional unit vector for i-th coordinate.</summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <param name="i">Coordinate index.</param>
        public static Vector BasisVector(int n,int i)
        {
            return new Vector(n, i);
        }

        #endregion StaticConstruction

        #endregion  Construction


        #region Data
         
        protected double[] _elements;

        protected int _length;

        /// <summary>Gets dimension of the vector.</summary>
        public override int Length
        { get { return _length; } }

        /// <summary>Sets the dimension of the current vector.
        /// This setter must be used very restrictively - only in setters that can change vector dimension.
        /// Setter is defined separately from getter because the base class' property does not define
        /// a getter.</summary>
        protected virtual int LengthSetter
        { set { _length = value; } }

        /// <summary>Gets or sets the element indexed by <c>i</c> in the <c>Vector</c>.</summary>
        /// <param name="i">Element index, 0 - based.</param>
        public override double this[int i]
        { get { return _elements[i]; } set { _elements[i] = value; } }


        //$$
        //#region MathNetSupport

        ////  WARNING: This region will be removed in the future! MathNet.Numerics library will be used instead of MathNet!
        
        //Vector_MathNet _copyMathNet;

        //protected bool _mathNetConsistent = false;


        ///// <summary>Tells whether the internal MathNet representation of the current vector is 
        ///// consistent with the current vector. The MathNet representation is used for operations that
        ///// are used from that library such as different kinds of decompositions.</summary>
        ///// <remarks>Currrently, an internal flag indicating consistency of the MathNet matrix is not used.
        ///// Every time this property is required, the consistence is actually verified by comparing values.
        ///// There may be derived matrix classes where the falg is actually used. These must keep track
        ///// when anything in the matrix changes and invalidate the flag on each such event.</remarks>
        //protected internal virtual bool IsCopyMathNetConsistent
        //{
        //    get
        //    {
        //        if (_copyMathNet == null)
        //            return false;
        //        else if (_copyMathNet.Length != Length)
        //            return false;
        //        else
        //        {
        //            for (int i = 0; i < _length; ++i)
        //            {
        //                if (_copyMathNet[i] != _elements[i])
        //                    return false;
        //            }
        //        }
        //        return true;
        //    }
        //    protected set
        //    {
        //        _mathNetConsistent = value;
        //    }
        //}

        ////$$
        /////// <summary>Copies values from the specified Math.Net vector.</summary>
        /////// <param name="v">Vector from which elements are copied.</param>
        ////protected void CopyFromMatNetMatrix(Vector_MathNet v)
        ////{
        ////    if (v == null)
        ////        throw new ArgumentNullException("Vector to copy components from is not specified (null reference).");
        ////    if (v.Length == 0)
        ////        throw new ArgumentException("Vector to copy components from has 0 elements.");
        ////    // Copy the array to a jagged array:
        ////    int numRows = _length = v.Length;
        ////    _elements = new double[_length];
        ////    for (int i = 0; i < _length; ++i)
        ////    {
        ////        _elements[i] = v[i];
        ////    }
        ////}

        ///// <summary>Gets the internal MathNet representation of the current vector.
        ///// Representation is created on demand. However, the same copy is returned
        ///// as long as it is consistent with the current matrix.
        ///// Use GetCopyMathNet() to create a new copy each time.</summary>
        //protected internal virtual Vector_MathNet CopyMathNet
        //{
        //    get
        //    {
        //        if (!IsCopyMathNetConsistent)
        //        {
        //            _copyMathNet = new Vector_MathNet(_length);
        //            for (int i = 0; i < _length; ++i)
        //                _copyMathNet[i] = this[i];
        //        }
        //        return _copyMathNet;
        //    }
        //}

        ///// <summary>Creates and returns a newly allocated MathNet representation of the current vector.</summary>
        //protected internal Vector_MathNet GetCopyMathNet()
        //{
        //    Vector_MathNet ReturnedString = new Vector_MathNet(_length);
        //    for (int i = 0; i < _length; ++i)
        //        ReturnedString[i] = this[i];
        //    return ReturnedString;
        //}

        //#endregion MathNetSupport


        #region MathNetNumerics

        protected Vector_MathNetNumerics _copyMathNetNumerics;

        protected bool _mathNetNumericsConsistent = false;

        /// <summary>Tells whether the internal MathNet Numerics representation of the current vector is 
        /// consistent with the current vector. The MathNet Numerics representation is used for operations that
        /// are used from that library such as different kinds of decompositions.</summary>
        /// <remarks>Currrently, an internal flag indicating consistency of the MathNet matrix is not used.
        /// Every time this property is required, the consistence is actually verified by comparing values.
        /// There may be derived matrix classes where the falg is actually used. These must keep track
        /// when anything in the matrix changes and invalidate the flag on each such event.</remarks>
        public virtual bool IsCopyMathNetNumericsConsistent
        {
            get
            {
                if (_copyMathNetNumerics == null)
                    return false;
                else if (_copyMathNetNumerics.Count != Length)
                    return false;
                else
                {
                    for (int i = 0; i < _length; ++i)
                    {
                        if (_copyMathNetNumerics[i] != _elements[i])
                            return false;
                    }
                }
                return true;
            }
            protected set
            {
                _mathNetNumericsConsistent = value;
            }
        }

        /// <summary>Copies values from the specified MathNet Numerics vector.</summary>
        /// <param name="v">Vector from which elements are copied.</param>
        protected void CopyFromMatNetNumericsVector(Vector_MathNetNumerics v)
        {
            if (v == null)
                throw new ArgumentNullException("Vector to copy components from is not specified (null reference).");
            if (v.Count == 0)
                throw new ArgumentException("Vector to copy components from has 0 elements.");
            // Copy the array to a jagged array:
            int numRows = _length = v.Count;
            _elements = new double[_length];
            for (int i = 0; i < _length; ++i)
            {
                _elements[i] = v[i];
            }
        }

        /// <summary>Gets the internal MathNet Numerics representation of the current vector.
        /// Representation is created on demand. However, the same copy is returned
        /// as long as it is consistent with the current matrix.
        /// Use GetCopyMathNet() to create a new copy each time.</summary>
        protected internal virtual Vector_MathNetNumerics CopyMathNetNumerics
        {
            get
            {
                if (!IsCopyMathNetNumericsConsistent)
                {
                    _copyMathNetNumerics = new Vector_MathNetNumerics(_length);
                    for (int i = 0; i < _length; ++i)
                        _copyMathNetNumerics[i] = this[i];
                }
                return _copyMathNetNumerics;
            }
        }

        /// <summary>Creates and returns a newly allocated MathNet Numerics representation of the current vector.</summary>
        protected internal Vector_MathNetNumerics GetCopyMathNetNumerics()
        {
            Vector_MathNetNumerics ret = new Vector_MathNetNumerics(_length);
            for (int i = 0; i < _length; ++i)
                ret[i] = this[i];
            return ret;
        }

        #endregion MathNetNumerics



       /// <summary>Creates and returns a copy of the current vector,
       /// which is of the same type as the current vector.</summary>
        public Vector GetCopyThis()
        {
            Vector ret = new Vector(Length);
            for (int i=0; i<Length; ++i)
                ret[i]=this[i];
            return ret;
       }

        /// <summary>Creates and returns a copy of the current vector, which is of the same 
        /// type as the current vector.</summary>
        public override VectorBase GetCopyBase()
        {
            return GetCopyThis();
        }

 
        /// <summary>Creates and returns a new vector of the specified dimension in such a way that the type
        /// of the returned vector is the same as that of the current vector.</summary>
        /// <param name="length">Dimension of the returned vector.</param>
        public Vector GetNewThis(int length)
        {
            return new Vector(length);
        }
 
        /// <summary>Creates and returns a new vector of the specified dimension in such a way that the type
        /// of the returned vector is the same as that of the current vector.</summary>
        /// <param name="length">Dimension of the returned vector.</param>
        public override VectorBase GetNewBase(int length)
        {
            return new Vector(length);
        }


        /// <summary>Creates and returns a new vector with the same dimension and of the same type as 
        /// the current vector.</summary>
        public Vector GetNewThis()
        { 
            return new Vector(this.Length); 
        }

        /// <summary>Creates and returns a new vector with the same dimension and of the same type as 
        /// the current vector.</summary>
        public override VectorBase GetNewBase()
        { 
            return new Vector(this.Length); 
        }


        /// <summary>Creates and returns a new matrix with the specified dimensona, and of a type that is 
        /// consistent with the type of the current vector.</summary>
        /// <param name="rowCount">Number of rows of the returned matrix.</param>
        /// <param name="columnCount">Number of rows of the returned matrix.</param>
        /// <returns>A matrix with specified dimensions, whose type is consistent with the type of the current vector.</returns>
        public virtual Matrix GetNewMatrixThis(int rowCount, int columnCount)
        {
            return new Matrix(rowCount,columnCount);
        }

        /// <summary>Creates and returns a new matrix with the specified dimensona, and of a type that is 
        /// consistent with the type of the current vector.</summary>
        /// <param name="rowCount">Number of rows of the returned matrix.</param>
        /// <param name="columnCount">Number of rows of the returned matrix.</param>
        /// <returns>A matrix with specified dimensions, whose type is consistent with the type of the current vector.</returns>
        public override MatrixBase GetNewMatrixBase(int rowCount, int columnCount)
        {
            return new Matrix(rowCount,columnCount);
        }


        /// <summary>Creates and returns a new matrix of a type that is consistent with the type of the current
        /// vector, and with both dimensions equal to the dimension of the current vector.</summary>
        public Matrix GetNewMatrixThis()
        {
            return new Matrix(this.Length, this.Length);
        }

        /// <summary>Creates and returns a new matrix of a type that is consistent with the type of the current
        /// vector, and with both dimensions equal to the dimension of the current vector.</summary>
        public override MatrixBase GetNewMatrixBase()
        {
            return new Matrix(this.Length, this.Length);
        }


        /// <summary>Returns a deep copy of a vector.</summary>
        object ICloneable.Clone()
        {
            return GetCopyThis();
        }


        #endregion Data


        #region Operations


        #region StaticOperations


        #region Products

        // Currently all static products are inherited from the base class.

        #endregion  // Products


        #endregion StaticOperations


        #endregion  // Operations


        //#region Operators_Overloading


        ///// <summary>Unary plus, returns the operand.</summary>
        //public static Vector operator + (Vector v) 
        //{
        //    Vector ReturnedString = v.GetCopyThis();
        //    Copy(v,ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Unary negation, returns the negative operand.</summary>
        //public static Vector operator -(Vector v) 
        //{ 
        //    Vector ReturnedString = v.GetCopyThis();
        //    Negate(v, ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Vector addition.</summary>
        //public static Vector operator + (Vector a, Vector b) 
        //{
        //    Vector ReturnedString = a.GetCopyThis();
        //    Add(a, b, ReturnedString);
        //    return ReturnedString; 
        //}


        ///// <summary>Vector subtraction.</summary>
        //public static Vector operator - (Vector a, Vector b) 
        //{
        //    Vector ReturnedString = a.GetCopyThis();
        //    Subtract(a, b, ReturnedString);
        //    return ReturnedString; 
        //}


        ///// <summary>Scalar product of two vectors.</summary>
        //public static double operator * (Vector a, Vector b) 
        //{
        //    return ScalarProduct(a,b); 
        //}

        ///// <summary>Product of a vector by a scalar.</summary>
        //public static Vector operator * (Vector a, double  b) 
        //{
        //    Vector ReturnedString = a.GetCopyThis();
        //    Multiply(a, b, ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Product of a vector by a scalar.</summary>
        //public static Vector operator * (double a, Vector  b) 
        //{
        //    Vector ReturnedString = b.GetCopyThis();
        //    Multiply(b , a, ReturnedString); 
        //    return ReturnedString;
        //}

        ///// <summary>Vector subtraction.</summary>
        //public static Vector operator / (Vector a, double b) 
        //{
        //    Vector ReturnedString = a.GetCopyThis();
        //    Divide(a, b, ReturnedString);
        //    return ReturnedString;
        //}


        //#endregion  // Operators_Overloading



        #region Auxiliary

        /// <summary>Returns the hash code (hash function) of the current vector.</summary>
        /// <remarks>
        /// <para>This method calls the <see cref="VectorBase.GetHashCode()"/> to calculate the 
        /// hash code, which is standard for all implementations of the <see cref="IVector"/> interface.</para>
        /// <para>Two vectors that have the same dimensions and equal elements will produce the same hash codes.</para>
        /// <para>Probability that two different vectors will produce the same hash code is small but it exists.</para>
        /// <para>Overrides the <see cref="object.GetHashCode"/> method.</para>
        /// </remarks>
        public override int GetHashCode()
        {
            return VectorBase.GetHashCode(this);
        }

        /// <summary>Returns a value indicating whether the specified object is equal to the current vector.
        /// <para>True is returned if the object is a non-null vector (i.e. it implements the <see cref="IVector"/>
        /// interface), and has the same dimension and equal elements as the current vector.</para></summary>
        /// <remarks>This method calls the <see cref="VectorBase.Equals(object)"/> to obtain the returned value, which is
        /// standard for all implementations of the <see cref="IVector"/> interface.
        /// <para>Overrides the <see cref="object.Equals(object)"/> method.</para></remarks>
        public override bool Equals(Object obj)
        {
            return VectorBase.Equals(this, obj as IVector);
        }

        #endregion Auxiliary


    }  // class Vector




} // namespace IG.Num
