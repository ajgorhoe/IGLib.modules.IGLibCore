
#nullable disable


/***************************************************/
/*                                                 */
/*  CLASS IMPLEMENTATION OF 2D MATRICES & VECTORS  */
/*                                                 */
/***************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    /// <summary>Vector or point in a 2 dimensional space.</summary>
    /// $A Igor Jul08; Oct10;
    [Serializable]
    public class Vector2d : VectorBase, IVector, ICloneable
    {

        #region Special2D  // Specific for 2D

        #region Data

        private vec2 _v;

        /// <summary>Gets dimension of the vector.</summary>
        public override int Length
        { get { return 2; } }


        /// <summary>Gets or sets the element indexed by <c>i</c> in the 2D <c>Vector</c>.</summary>
        /// <param name="i">Element index, 0 - based, must be between 0 and 1.</param>
        public override double this[int i]
        {
            get
            {
                if (i == 0)
                    return _v.x;
                else if (i == 1)
                    return _v.y;
                else
                    throw new Exception("Component index of a 3D vector must be between 0 and 1. Passed: " + i + ".");
            }
            set
            {
                if (i == 0)
                    _v.x = value;
                else if (i == 1)
                    _v.y = value;
                else
                    throw new Exception("Component index of a 3D vector must be between 0 and 1. Passed: " + i + ".");
            }
        }
        

        /// <summary>Creates and returns a copy of the current 2D vector,
        /// which is of the same type as the current vector.</summary>
        public Vector2d GetCopyThis()
        {
            Vector2d ret = new Vector2d(0.0);
            for (int i = 0; i < 2; ++i)
                ret[i] = this[i];
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
        public Vector2d GetNewThis(int length)
        {
            if (length != 2)
                throw new ArgumentException("Dimenson of the newly created 2D vector should be 2.");
            return new Vector2d(0.0);
        }

        /// <summary>Creates and returns a new 2D vector of the specified dimension in such a way that the type
        /// of the returned vector is the same as that of the current vector.</summary>
        /// <param name="length">Dimension of the returned vector, must be 2.</param>
        public override VectorBase GetNewBase(int length)
        {
            if (length != 2)
                throw new ArgumentException("Dimenson of the newly created 2D vector should be 2.");
            return new Vector2d(0.0);
        }


        /// <summary>Creates and returns a new vector with the same dimension and of the same type as 
        /// the current vector.</summary>
        public Vector2d GetNewThis()
        {
            return new Vector2d(0.0);
        }

        /// <summary>Creates and returns a new vector with the same dimension and of the same type as 
        /// the current vector.</summary>
        public override VectorBase GetNewBase()
        {
            return new Vector2d(0.0);
        }


        /// <summary>Creates and returns a new matrix with the specified dimensona, and of a type that is 
        /// consistent with the type of the current vector.</summary>
        /// <param name="rowCount">Number of rows of the returned matrix.</param>
        /// <param name="columnCount">Number of rows of the returned matrix.</param>
        /// <returns>A matrix with specified dimensions, whose type is consistent with the type of the current vector.</returns>
        public virtual Matrix2d GetNewMatrixThis(int rowCount, int columnCount)
        {
            if (rowCount != 2 || columnCount != 2)
                throw new ArgumentException("A 2D matrix can only be created with dimensions 2x2.");
            return new Matrix2d(0.0);
        }

        /// <summary>Creates and returns a new matrix with the specified dimensona, and of a type that is 
        /// consistent with the type of the current vector.</summary>
        /// <param name="rowCount">Number of rows of the returned matrix.</param>
        /// <param name="columnCount">Number of rows of the returned matrix.</param>
        /// <returns>A matrix with specified dimensions, whose type is consistent with the type of the current vector.</returns>
        public override MatrixBase GetNewMatrixBase(int rowCount, int columnCount)
        {
            if (rowCount != 2 || columnCount != 2)
                throw new ArgumentException("A 2D matrix can only be created with dimensions 2x2.");
            return new Matrix2d(0.0);
        }


        /// <summary>Creates and returns a new matrix of a type that is consistent with the type of the current
        /// vector, and with both dimensions equal to the dimension of the current vector.</summary>
        public Matrix2d GetNewMatrixThis()
        {
            return new Matrix2d(0.0);
        }

        /// <summary>Creates and returns a new matrix of a type that is consistent with the type of the current
        /// vector, and with both dimensions equal to the dimension of the current vector.</summary>
        public override MatrixBase GetNewMatrixBase()
        {
            return new Matrix2d(0.0);
        }



        /// <summary>Returns a deep copy of a 2D vector.</summary>
        object ICloneable.Clone()
        {
            return GetCopyThis();
        }


        #endregion Data



        #region Construction

        /// <summary>Copy constructor.
        /// Initializes components of a 2D vector with components of the specified vector.</summary>
        /// <param name="v">Vectr whose components are copied to the initialized vector.</param>
        public Vector2d(Vector2d v)
        {
            if (v == null)
                throw new ArgumentNullException("Vector is not specified in copy constructor (null reference).");
            X = v.X; Y = v.Y;
        }


        /// <summary>Initializes components of a 2D vector with components of the specified vector.</summary>
        /// <param name="v">Vector whose components are copied to the initialized vector.</param>
        public Vector2d(vec2 v)
        {
            X = v.x; Y = v.y;
        }

        /// <summary>Initializes components of a 2D vector with the specified values.</summary>
        /// <param name="x">Value assigned to the first compontnt of the vector.</param>
        /// <param name="y">Value assigned to the second compontnt of the vector.</param>
        public Vector2d(double x, double y)
        {
            this.X = x; this.Y = y;
        }

        /// <summary>Initializes all component of a 2D vector with the specified value.</summary>
        /// <param name="comp">Value assigned to all vector components.</param>
        public Vector2d(double comp)
        {
            X = Y = comp;
        }


        //Remark: unit vector is abolished because its use causef problems becaues of ambiguity.
        // Use the static BasisVector function instead!
        ///// <summary>Constructs a 2-dimensional unit vector for i'th coordinate.</summary>
        // /// <param name="i">Coordinate index.</param>
        // public Vector2d(int i)
        // {
        //     if (i < 0 || i >= 2)
        //         throw new ArgumentException("Creation of unit vector: index out of range: "
        //             + i.ToString() + ", should be between 0 and 2.");
        //     this.X = this.Y = 0.0;
        //     this[i] = 1.0;
        // }



        /// <summary>Constructs a vector from a 1-D array.</summary>
        /// <remarks>See also <see cref="Vector2d.Create(double[])"/></remarks>
        /// <param name="components">One-dimensional array of vector components.</param>
        public Vector2d(double[] components) 
        {
            if (components==null)
                throw new ArgumentNullException("2D vector creation: array of components not specified (null argument).");
            int length=components.Length;
            if (length != 2)
                throw new ArgumentException("2D vector creation: array dimension should be 2.");
            for (int i = 0; i < 2; ++i)
                this[i] = components[i];
        }

        ///// <summary>Constructs a vector from a 1-D array, directly using the provided array as internal data structure.</summary>
        ///// <param name="vec">One-dimensional array of doubles.</param>
        ///// <seealso cref="Create"/>
        //public Vector2d(MathNet.Numerics.LinearAlgebra.Vector vec) 
        //{
        //    if (vec==null)
        //        throw new ArgumentNullException("Vector creation: array of components not specified (null argument).");
        //    int length=vec.Length;
        //    if (length != 2)
        //        throw new ArgumentException("2D vector creation: array of components is not 2 dimensional.");
        //    for (int i=0; i<2; ++i)
        //        this[i] = vec[i];
        //}

        /// <summary>Constructs a 2D vector from another vector.</summary>
        /// <param name="vec">Vector whose copy is created.</param>
        public Vector2d(IVector vec) 
        {
            if (vec==null)
                throw new ArgumentNullException("2D vector creation: vector to be copied is not specified (null argument).");
            int length=vec.Length;
            if (length != 2)
                throw new ArgumentException("2D vector creation: array of components contains should have dimension 2.");
            for (int i = 0; i < length; ++i)
                this[i] = vec[i];
        }


        #region StaticConstruction


        /// <summary>Constructs a vector from a 1-D array.</summary>
        public static Vector2d Create(double[] components)
        {
            return new Vector2d(components);
        }

        ///// <summary>Constructs a vector as a copy of a MathNet.Numerics.LinearAlgebra.Vector object.</summary>
        //public static Vector2d Create(MathNet.Numerics.LinearAlgebra.Vector vec)
        //{
        //    return new Vector2d(vec);
        //}


        /// <summary>Constructs a 2D vector as a copy of another IVector object.</summary>
        public static Vector2d Create(IVector vec)
        {
            return new Vector2d(vec);
        }

        ///// <summary>Generates vector with random elements.</summary>
        ///// <param name="randomDistribution">Continuous Random Distribution or Source</param>
        ///// <returns>A 2-dimensional vector with random elements distributed according
        ///// to the specified random distribution.</returns>
        //public static Vector2d Random(IContinuousGenerator randomDistribution)
        //{
        //    Vector2d ReturnedString = new Vector2d(0.0);
        //    for (int i = 0; i < 2; i++)
        //    {
        //        ReturnedString[i] = randomDistribution.NextDouble();
        //    }
        //    return ReturnedString;
        //}

        /// <summary>Generates 2-dimensional vector with random elements uniformly distributed on [0, 1).</summary>
        public static Vector2d Random()
        {
            Vector2d ret = new Vector2d(0.0);
            ret.SetRandom();
            return ret;
            // return Random(new SystemRandomSource());
        }

        /// <summary>Generates an 2-dimensional vector filled with 1.</summary>
        public static Vector2d Ones()
        {
            return new Vector2d(1.0);
        }

        /// <summary>Generates an 2-dimensional vector filled with 0.</summary>
        public static Vector2d Zeros()
        {
            return new Vector2d(0.0);
        }

        /// <summary>Generates an d2-dimensional unit vector for i-th coordinate.</summary>
        /// <param name="i">Coordinate index.</param>
        public static Vector2d BasisVector(int i)
        {
            Vector2d ret = new Vector2d(0.0);
            ret[i] = 1.0;
            return ret;
        }

        #endregion StaticConstruction

        #endregion Construction



        

        #region Access

        /// <summary>Gets the struct representation of this 2D vector.</summary>
        public vec2 Vec
        {
            get { return _v; }
            set { _v = value; }
        }

        /// <summary>1st component.</summary>
        public double X
        {
            get { return _v.x; }
            set { _v.x = value; }
        }

        /// <summary>2nd component.</summary>
        public double Y
        {
            get { return _v.y; }
            set { _v.y = value; }
        }



        #endregion Access

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
        /// interface), has the same dimension and equal elements as the current vector.</para></summary>
        /// <remarks>This method calls the <see cref="VectorBase.Equals(object)"/> to obtain the returned value, which is
        /// standard for all implementations of the <see cref="IVector"/> function.
        /// <para>Overrides the <see cref="object.Equals(object)"/> method.</para></remarks>
        public override bool Equals(Object obj)
        {
            return VectorBase.Equals(this, obj as IVector);
        }

        #endregion Auxiliary


        #region Operations

        /// <summary>Gets a 2 norm of the current 2D vector.</summary>
        public override double Norm
        { get { return Norm2; } }

        /// <summary>Gets a 2 norm of the current 2D vector.</summary>
        public override double Norm2 { get { return Vec.Norm2; } }

        /// <summary>Gets an 1 norm of the current 2D vector -
        /// sum of absolute values of components.</summary>
        public override double Norm1 { get { return Vec.Norm1; } }

        /// <summary>Gets an infinity norm of the current 2D vector -
        /// maximal absolute component value.</summary>
        public override double NormInf
        { get { return Vec.NormInf; } }

        /// <summary>Normalizes this 2D vector.</summary>
        public new Vector2d Normalized()
        { return new Vector2d(Vec.Normalized()); }

        /// <summary>Normalizes this 2D vector in 1 norm.</summary>
        public Vector2d Normalized1()
        { return new Vector2d(Vec.Normalized1()); }

        /// <summary>Normalizes this 2D vector in 2 norm.</summary>
        public Vector2d Normalized2()
        { return new Vector2d(Vec.Normalized2()); }

        /// <summary>Normalizes this 2D vector in infinity norm.</summary>
        public Vector2d NormalizedInfinity()
        { return new Vector2d(Vec.NormalizedInfinity()); }

        /// <summary>Returns scalar product of the current and the specified vector.</summary>
        public double ScalarProduct(Vector2d v)
        { return Vec.ScalarProduct(v.Vec); }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public Vector2d VectorProduct(Vector2d v)
        { return new Vector2d(Vec.ScalarProduct(v.Vec)); }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public Vector2d CrossProduct(Vector2d v)
        { return VectorProduct(v); }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public Vector2d Cross(Vector2d v)
        { return VectorProduct(v); }

        /// <summary>Returns dyadic product of the current and the specified vector.</summary>
        public Matrix2d DyadicProduct(Vector2d v)
        { return new Matrix2d(Vec.DyadicProduct(v.Vec)); }

        /// <summary>Returns the current vector multiplied by the specified scalar.</summary>
        /// <param name="k">Factor by which the current vector is multiplied.</param>
        public Vector2d Multiply(double k)
        { return new Vector2d(Vec.Multiply(k)); }

        /// <summary>Returns sum of the current vector and the specified vector.</summary>
        public Vector2d Add(Vector2d a)
        { return new Vector2d(Vec.Add(a.Vec)); }

        /// <summary>Returns difference between the current vector and the specified vector.</summary>
        public Vector2d Subtract(Vector2d a)
        { return new Vector2d(Vec.Subtract(a.Vec)); }

        #endregion Operations


        #region StaticMethods

        /// <summary>Returns a copy of the specified 2D vector.</summary>
        /// <param name="v">Vector whose copy is returned.</param>
        public static Vector2d Copy(Vector2d v)
        {
            return new Vector2d(v);
        }

        /// <summary>Negates the specified vector and stores its copy in the resulting vector.</summary>
        /// <param name="v">Vectr to be negated.</param>
        /// <param name="res">Vector where the result is stored.</param>
        public static void Negate(Vector2d v, ref Vector2d res)
        {
            res.X = -v.X; res.Y = -v.Y;
        }

        #endregion StaticMethods


        #region OperatorsOverloading

        /// <summary>Unary plus, returns the operand.</summary>
        public static Vector2d operator +(Vector2d v)
        {
            return Vector2d.Copy(v);
        }

        /// <summary>Unary negation, returns the negative operand.</summary>
        public static Vector2d operator -(Vector2d v)
        {
            return new Vector2d(-v.X, -v.Y);
        }

        /// <summary>Vector addition.</summary>
        public static Vector2d operator +(Vector2d a, Vector2d b)
        {
            return a.Add(b);
        }


        /// <summary>Vector subtraction.</summary>
        public static Vector2d operator -(Vector2d a, Vector2d b)
        {
            return a.Subtract(b); ;
        }


        /// <summary>Scalar product of two 2D vectors.</summary>
        public static double operator *(Vector2d a, Vector2d b)
        {
            return a.ScalarProduct(b);
        }

        /// <summary>Product of a 2D vector by a scalar.</summary>
        public static Vector2d operator *(Vector2d a, double b)
        {
            return a.Multiply(b);
        }

        /// <summary>Product of a 2D vector by a scalar.</summary>
        public static Vector2d operator *(double a, Vector2d b)
        {
            return b.Multiply(a);
        }

        /// <summary>Vector subtraction.</summary>
        public static Vector2d operator /(Vector2d a, double b)
        {
            return new Vector2d(a.X / b, a.Y / b);
        }

        #endregion  OperatorsOverloading


        #region InputOutput

        /// <summary>Returns string representation of the 2D vector.</summary>
        public override string ToString()
        {
            return Vec.ToString();
        }

        /// <summary>Reads 2D vector components from a console.</summary>
        public void Read()
        {
            Read(null);
        }


        /// <summary>Reads 2D vector components from a console.</summary>
        /// <param name="name">Name of the vector to be read; it is written as orientation to the user
        /// and can be null.</param>
        public void Read(string name)
        {
            Vec.Read(name);
        }

        #endregion InputOutput

        #endregion Special2D



    }  // class Vector2D


}

