
#nullable disable


/***************************************************/
/*                                                 */
/*  CLASS IMPLEMENTATION OF 3D MATRICES & VECTORS  */
/*                                                 */
/***************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    /// <summary>Vector or point in a 3 dimensional space.</summary>
    /// $A Igor Jul08; Oct10;
    /// TODO: Consider whether Vector3d should ingerit from VectorBase!
    [Serializable]
    public class Vector3d : VectorBase, IVector, ICloneable
    {

        #region Special3D  
        // Specific for 3D

        #region Data

        private vec3 _v;

        /// <summary>Gets dimension of the vector.</summary>
        public override int Length
        { get { return 3; } }


        /// <summary>Gets or sets the element indexed by <c>i</c> in the 3D <c>Vector</c>.</summary>
        /// <param name="i">Element index, 0 - based, must be between 0 and 2.</param>
        public override double this[int i]
        {
            get
            {
                if (i == 0)
                    return _v.x;
                else if (i == 1)
                    return _v.y;
                else if (i == 2)
                    return _v.z;
                else
                    throw new Exception("Component index of a 3D vector must be between 0 and 2. Passed: " + i + ".");
            }
            set
            {
                if (i == 0)
                    _v.x = value;
                else if (i == 1)
                    _v.y = value;
                else if (i == 2)
                    _v.z = value;
                else
                    throw new Exception("Component index of a 3D vector must be between 0 and 2. Passed: " + i + ".");
            }
        }

        /// <summary>Creates and returns a copy of the current 3D vector,
        /// which is of the same type as the current vector.</summary>
        public Vector3d GetCopyThis()
        {
            Vector3d ret = new Vector3d(0.0);
            for (int i = 0; i < 3; ++i)
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
        public Vector3d GetNewThis(int length)
        {
            if (length != 3)
                throw new ArgumentException("Dimenson of the newly created 3D vector should be 3.");
            return new Vector3d(0.0);
        }

        /// <summary>Creates and returns a new 3D vector of the specified dimension in such a way that the type
        /// of the returned vector is the same as that of the current vector.</summary>
        /// <param name="length">Dimension of the returned vector, must be 3.</param>
        public override VectorBase GetNewBase(int length)
        {
            if (length != 3)
                throw new ArgumentException("Dimenson of the newly created 3D vector should be 3.");
            return new Vector3d(0.0);
        }

        /// <summary>Creates and returns a new vector with the same dimension and of the same type as 
        /// the current vector.</summary>
        public Vector3d GetNewThis()
        {
            return new Vector3d(0.0);
        }

        /// <summary>Creates and returns a new vector with the same dimension and of the same type as 
        /// the current vector.</summary>
        public override VectorBase GetNewBase()
        {
            return new Vector3d(0.0);
        }

        /// <summary>Creates and returns a new 3D matrix with the specified dimensona, and of a type that is 
        /// consistent with the type of the current vector.</summary>
        /// <param name="rowCount">Number of rows of the returned matrix.</param>
        /// <param name="columnCount">Number of rows of the returned matrix.</param>
        /// <returns>A matrix with specified dimensions, whose type is consistent with the type of the current vector.</returns>
        public virtual Matrix3d GetNewMatrixThis(int rowCount, int columnCount)
        {
            if (rowCount != 3 || columnCount != 3)
                throw new ArgumentException("A 3D matrix can only be created with dimensions 3x3.");
            return new Matrix3d(0.0);
        }

        /// <summary>Creates and returns a new 3D matrix with the specified dimensona, and of a type that is 
        /// consistent with the type of the current vector.</summary>
        /// <param name="rowCount">Number of rows of the returned matrix.</param>
        /// <param name="columnCount">Number of rows of the returned matrix.</param>
        /// <returns>A matrix with specified dimensions, whose type is consistent with the type of the current vector.</returns>
        public override MatrixBase GetNewMatrixBase(int rowCount, int columnCount)
        {
            return GetNewMatrixThis(rowCount, columnCount);
        }

        /// <summary>Creates and returns a new matrix of a type that is consistent with the type of the current
        /// vector, and with both dimensions equal to the dimension of the current vector.</summary>
        public Matrix3d GetNewMatrixThis()
        {
            return new Matrix3d(0.0);
        }

        /// <summary>Creates and returns a new matrix of a type that is consistent with the type of the current
        /// vector, and with both dimensions equal to the dimension of the current vector.</summary>
        public override MatrixBase GetNewMatrixBase()
        {
            return new Matrix3d(0.0);
        }


        /// <summary>Returns a deep copy of a 3D vector.</summary>
        object ICloneable.Clone()
        {
            return GetCopyThis();
        }


        #endregion Data



        #region Construction

        /// <summary>Copy constructor.
        /// Initializes components of a 3D vector with components of the specified vector.</summary>
        /// <param name="v">Vectr whose components are copied to the initialized vector.</param>
        public Vector3d(Vector3d v)
        {
            if (v == null)
                throw new ArgumentNullException("Vector is not specified in copy constructor (null reference).");
            X = v.X; Y = v.Y; Z = v.Z;
        }


        /// <summary>Initializes components of a 3D vector with components of the specified vector.</summary>
        /// <param name="v">Vector whose components are copied to the initialized vector.</param>
        public Vector3d(vec3 v)
        {
            X = v.x; Y = v.y; Z = v.z;
        }

        /// <summary>Initializes components of a 3D vector with the specified values.</summary>
        /// <param name="x">Value of the first element of the vector.</param>
        /// <param name="y">Value of the second element of the vector.</param>
        /// <param name="z">Value of the third element of the vector.</param>
        public Vector3d(double x, double y, double z)
        {
            this.X = x; this.Y = y; this.Z = z;
        }

        /// <summary>Initializes all component of a 3D vector with the specified value.</summary>
        /// <param name="comp">Value assigned to all vector components.</param>
        public Vector3d(double comp)
        {
            X = Y = Z = comp;
        }

        //Remark: unit vector is abolished because its use causef problems becaues of ambiguity.
        // Use the static BasisVector function instead!
        ///// <summary>Constructs a 3-dimensional unit vector for i'th coordinate.</summary>
        // /// <param name="i">Coordinate index.</param>
        // public Vector3d(int i)
        // {
        //     if (i < 0 || i >= 3)
        //         throw new ArgumentException("Creation of unit vector: index out of range: "
        //             + i.ToString() + ", should be between 0 and 2.");
        //     this.X = this.Y = this.Z = 0.0;
        //     this[i] = 1.0;
        // }



        /// <summary>Constructs a vector from a 1-D array.</summary>
        /// <param name="components">One-dimensional array of doubles.</param>
        /// <seealso cref="Create(double[])"/>
        public Vector3d(double[] components) 
        {
            if (components==null)
                throw new ArgumentNullException("3D vector creation: array of components not specified (null argument).");
            int length=components.Length;
            if (length != 3)
                throw new ArgumentException("3D vector creation: array dimension should be 3.");
            for (int i = 0; i < 3; ++i)
                this[i] = components[i];
        }

        ///// <summary>Constructs a vector from a 1-D array, directly using the provided array as internal data structure.</summary>
        ///// <param name="vec">One-dimensional array of doubles.</param>
        ///// <seealso cref="Create"/>
        //public Vector3d(MathNet.Numerics.LinearAlgebra.Vector vec) 
        //{
        //    if (vec==null)
        //        throw new ArgumentNullException("Vector creation: array of components not specified (null argument).");
        //    int length=vec.Length;
        //    if (length != 3)
        //        throw new ArgumentException("3D vector creation: array of components is not 3 dimensional.");
        //    for (int i=0; i<3; ++i)
        //        this[i] = vec[i];
        //}

        /// <summary>Constructs a 3D vector from another vector.</summary>
        /// <param name="vec">Vector whose copy is created.</param>
        public Vector3d(IVector vec) 
        {
            if (vec==null)
                throw new ArgumentNullException("3D vector creation: vector to be copied is not specified (null argument).");
            int length=vec.Length;
            if (length != 3)
                throw new ArgumentException("3D vector creation: array of components contains should have dimension 3.");
            for (int i = 0; i < length; ++i)
                this[i] = vec[i];
        }


        #region StaticConstruction


        /// <summary>Constructs a vector from a 1-D array.</summary>
        public static Vector3d Create(double[] components)
        {
            return new Vector3d(components);
        }

        ///// <summary>Constructs a vector as a copy of a MathNet.Numerics.LinearAlgebra.Vector object.</summary>
        //public static Vector3d Create(MathNet.Numerics.LinearAlgebra.Vector vec)
        //{
        //    return new Vector3d(vec);
        //}


        /// <summary>Constructs a 3D vector as a copy of another IVector object.</summary>
        public static Vector3d Create(IVector vec)
        {
            return new Vector3d(vec);
        }

        ///// <summary>Generates vector with random elements.</summary>
        ///// <param name="randomDistribution">Continuous Random Distribution or Source</param>
        ///// <returns>A 3-dimensional vector with random elements distributed according
        ///// to the specified random distribution.</returns>
        //public static Vector3d Random(IContinuousGenerator randomDistribution)
        //{
        //    Vector3d ReturnedString = new Vector3d(0.0);
        //    for (int i = 0; i < 3; i++)
        //    {
        //        ReturnedString[i] = randomDistribution.NextDouble();
        //    }
        //    return ReturnedString;
        //}

        /// <summary>Generates 3-dimensional vector with random elements uniformly distributed on [0, 1).</summary>
        public static Vector3d Random()
        {
            Vector3d ret = new Vector3d(0.0);
            ret.SetRandom();
            return ret;
            // return Random(new SystemRandomSource());
        }

        /// <summary>Generates an 3-dimensional vector filled with 1s.</summary>
        public static Vector3d Ones()
        {
            return new Vector3d(1.0);
        }

        /// <summary>Generates an 3-dimensional vector filled with 0.</summary>
        public static Vector3d Zeros()
        {
            return new Vector3d(0.0);
        }

        /// <summary>Generates an 3-dimensional unit vector for i-th coordinate.</summary>
        /// <param name="i">Coordinate index.</param>
        public static Vector3d BasisVector(int i)
        {
            Vector3d ret = new Vector3d(0.0);
            ret[i] = 1.0;
            return ret;
        }

        #endregion StaticConstruction







        #endregion Construction



        

        #region Access

        /// <summary>Gets the struct representation of this 3D vector.</summary>
        public vec3 Vec
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

        /// <summary>3rd component.</summary>
        public double Z
        {
            get { return _v.z; }
            set { _v.z = value; }
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
        /// <remarks>This method calls the <see cref="VectorBase.Equals(IVector, IVector)"/> to obtain the returned value, which is
        /// standard for all implementations of the <see cref="IVector"/> function.
        /// <para>Overrides the <see cref="object.Equals(object)"/> method.</para></remarks>
        public override bool Equals(Object obj)
        {
            return VectorBase.Equals(this, obj as IVector);
        }

        #endregion Auxiliary


        #region Operations

        /// <summary>Gets a 2 norm of the current 3D vector.</summary>
        public override double Norm
        { get { return Norm2; } }

        /// <summary>Gets a 2 norm of the current 3D vector.</summary>
        public override double Norm2 { get { return Vec.Norm2; } }

        /// <summary>Gets an 1 norm of the current 3D vector -
        /// sum of absolute values of components.</summary>
        public override double Norm1 { get { return Vec.Norm1; } }

        /// <summary>Gets an infinity norm of the current 3D vector -
        /// maximal absolute component value.</summary>
        public override double NormInf
        { get { return Vec.NormInf; } }

        /// <summary>Normalizes this 3D vector.</summary>
        public new Vector3d Normalized()
        { return new Vector3d(Vec.Normalized()); }

        /// <summary>Normalizes this 3D vector in 1 norm.</summary>
        public Vector3d Normalized1()
        { return new Vector3d(Vec.Normalized1()); }

        /// <summary>Normalizes this 3D vector in 2 norm.</summary>
        public Vector3d Normalized2()
        { return new Vector3d(Vec.Normalized2()); }

        /// <summary>Normalizes this 3D vector in infinity norm.</summary>
        public Vector3d NormalizedInfinity()
        { return new Vector3d(Vec.NormalizedInfinity()); }

        /// <summary>Returns scalar product of the current and the specified vector.</summary>
        public double ScalarProduct(Vector3d v)
        { return Vec.ScalarProduct(v.Vec); }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public Vector3d VectorProduct(Vector3d v)
        { return new Vector3d(Vec.ScalarProduct(v.Vec)); }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public Vector3d CrossProduct(Vector3d v)
        { return VectorProduct(v); }

        /// <summary>Returns vector product of the current and the specified vector.</summary>
        public Vector3d Cross(Vector3d v)
        { return VectorProduct(v); }

        /// <summary>Returns dyadic product of the current and the specified vector.</summary>
        public Matrix3d DyadicProduct(Vector3d v)
        { return new Matrix3d(Vec.DyadicProduct(v.Vec)); }

        /// <summary>Returns mixed product of the current and two other specified vectors.
        /// This equals the volume of the parallelepiped spanned by these vectors.</summary>
        public double MixedProduct(Vector3d b, Vector3d c)
        { return Vec.MixedProduct(b.Vec, c.Vec); }

        /// <summary>Returns the current vector multiplied by the specified scalar.</summary>
        /// <param name="k">Factor by which the current vector is multiplied.</param>
        public Vector3d Multiply(double k)
        { return new Vector3d(Vec.Multiply(k)); }

        /// <summary>Returns sum of the current vector and the specified vector.</summary>
        public Vector3d Add(Vector3d a)
        { return new Vector3d(Vec.Add(a.Vec)); }

        /// <summary>Returns difference between the current vector and the specified vector.</summary>
        public Vector3d Subtract(Vector3d a)
        { return new Vector3d(Vec.Subtract(a.Vec)); }

        #endregion Operations


        #region StaticMethods

        /// <summary>Returns a copy of the specified 3D vector.</summary>
        /// <param name="v">Vector whose copy is returned.</param>
        public static Vector3d Copy(Vector3d v)
        {
            return new Vector3d(v);
        }

        /// <summary>Negates the specified vector and stores its copy in the resulting vector.</summary>
        /// <param name="v">Vectr to be negated.</param>
        /// <param name="res">Vector where the result is stored.</param>
        public static void Negate(Vector3d v, ref Vector3d res)
        {
            res.X = -v.X; res.Y = -v.Y; res.Z = -v.Z;
        }

        #endregion StaticMethods


        #region OperatorsOverloading

        /// <summary>Unary plus, returns the operand.</summary>
        public static Vector3d operator +(Vector3d v)
        {
            return Vector3d.Copy(v);
        }

        /// <summary>Unary negation, returns the negative operand.</summary>
        public static Vector3d operator -(Vector3d v)
        {
            return new Vector3d(-v.X, -v.Y, -v.Z);
        }

        /// <summary>Vector addition.</summary>
        public static Vector3d operator +(Vector3d a, Vector3d b)
        {
            return a.Add(b);
        }


        /// <summary>Vector subtraction.</summary>
        public static Vector3d operator -(Vector3d a, Vector3d b)
        {
            return a.Subtract(b); ;
        }


        /// <summary>Scalar product of two 3D vectors.</summary>
        public static double operator *(Vector3d a, Vector3d b)
        {
            return a.ScalarProduct(b);
        }

        /// <summary>Product of a 3D vector by a scalar.</summary>
        public static Vector3d operator *(Vector3d a, double b)
        {
            return a.Multiply(b);
        }

        /// <summary>Product of a 3D vector by a scalar.</summary>
        public static Vector3d operator *(double a, Vector3d b)
        {
            return b.Multiply(a);
        }

        /// <summary>Vector subtraction.</summary>
        public static Vector3d operator /(Vector3d a, double b)
        {
            return new Vector3d(a.X / b, a.Y / b, a.Z / b);
        }

        #endregion  OperatorsOverloading



        #region InputOutput

        /// <summary>Returns string representation of the 3D vector.</summary>
        public override string ToString()
        {
            return Vec.ToString();
        }

        /// <summary>Reads 3D vector components from a console.</summary>
        public void Read()
        {
            Read(null);
        }


        /// <summary>Reads 3D vector components from a console.</summary>
        /// <param name="name">Name of the vector to be read; it is written as orientation to the user
        /// and can be null.</param>
        public void Read(string name)
        {
            Vec.Read(name);
        }

        #endregion InputOutput

        #endregion Special3D



    }  // class Vector3D


}

