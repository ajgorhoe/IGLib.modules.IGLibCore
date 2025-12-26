// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/



    /***************************************************/
    /*                                                 */
    /*  CLASS IMPLEMENTATION OF 2D MATRICES & VECTORS  */
    /*                                                 */
    /***************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathNet.Numerics;
using MathNet.Numerics.Distributions;
//using MathNet.Numerics.RandomSources;

namespace IG.Num
{

    /// <summary>Matrix in a 2 dimensional space.</summary>
    /// $A Igor Jul08; Oct10;
    [Serializable]
    public class Matrix2d : MatrixBase, IMatrix, ICloneable
    {


        #region Construction

        /// <summary>Copy constructor.
        /// Initializes components of a 2D matrix with components of another 2D matrix.</summary>
        /// <param name="m">Matrix whose components are copied to the initialized matrix.</param>
        public Matrix2d(Matrix2d m)
        {
            if (m==null)
                throw new ArgumentNullException("Matrix is not specified in copy constructor (null reference).");
            XX = m.XX;  XY = m.XY;  
            YX = m.YX;  YY = m.YY;  
        }

        /// <summary>Constructor.
        /// Initializes components of a 2D matrix with components of another matrix.
        /// The specified matrix should be 2*2.</summary>
        /// <param name="m">Matrix whose components are copied to the initialized matrix (should be 2*2).</param>
        public Matrix2d(IMatrix m)
        {
            if (m == null)
                throw new ArgumentNullException("Matrix is not specified in constructor (null reference).");
            else if (m.RowCount != 2 || m.ColumnCount != 2)
                throw new ArgumentException("Incorrect dimensons of matrix passed to Matrix2D constructor, should be 2*2."
                    + Environment.NewLine + "  Actual dimensions: " + m.RowCount + "*" + m.ColumnCount + ".");
            XX = m[0,0];  XY = m[0,1]; 
            YX = m[1,0];  YY = m[1,1];  
        }

        /// <summary>Initializes 2D matrix structure with the specified components.</summary>
        public Matrix2d(Vector2d rowx, Vector2d rowy)
        {
            if (rowx == null || rowy == null)
                throw new ArgumentNullException("One or more row vectors are not specified in constructor (null reference).");
            XX = rowx.X; XY = rowx.Y; 
            YX = rowy.X; YY = rowy.Y; 
        }

        /// <summary>Initializes components of a 2D matrix with components of the specified matrix.</summary>
        /// <param name="m">Matrix whose components are copied to the initialized matrix.</param>
        public Matrix2d(mat2 m)
        { _m=m; }

        /// <summary>Initializes 2D matrix structure with the specified components.</summary>
        public Matrix2d(double xx, double xy, 
                    double yx, double yy
                    )
        {
            this.XX = xx; this.XY = xy; 
            this.YX = yx; this.YY = yy; 
        }

        /// <summary>Initializes 2D with the specified component.</summary>
        /// <param name="component">Value that is assigned to all matrix components.</param>
        public Matrix2d(double component)
        {
            this.XX = component; this.XY = component; 
            this.YX = component; this.YY = component; 
        }

        /// <summary>Initializes 2D matrix structure with the specified components.</summary>
        public Matrix2d(vec2 xrow, vec2 yrow, vec2 zrow)
        {
            _m = new mat2(xrow, yrow);
        }

        
        /// <summary>Initializes a 2D matrix with elements of a jagged array.</summary>
        /// <param name="A">Array from which a 2D matrix is constructed.</param>
        public Matrix2d(double[][] A)
        {
            if (A == null)
                throw new ArgumentNullException("Jagged array to create a 2D matrix from is not specified (null reference).");
            if (A.Rank != 2)
                throw new ArgumentException("Jagged array to create a 2D matrix from is not a rank 2 array.");
            if (A.Length != 2)
                throw new ArgumentException("Jagged array to create a 2D matrix from does not have 2 rows.");
            for (int i = 0; i < 2; ++i)
            {
                if (A[i].Length != 2)
                    throw new ArgumentException("Jagged array to create a 2D matrix from has inconsistent dimensions. Row "
                        + i + "has length " + A[i].Length + "Instead of 2.");
                for (int j = 0; j < 2; ++j)
                    this[i, j] = A[i][j];
            }
        }

        /// <summary>Initializes a 2D matrix with elements of a rectangular array.</summary>
        /// <param name="A">Array from which a 2D matrix is constructed.</param>
        public Matrix2d(double[,] A)
        {
            if (A == null)
                throw new ArgumentNullException("Rectangular array to create a 2D matrix from is not specified (null reference).");
            if (A.GetLength(0) != 2)
                throw new ArgumentException("Rectangular array to create a 2D matrix does not have 2 rows.");
            if (A.GetLength(1) != 2)
                throw new ArgumentException("Rectangular array to create a 2D matrix from does not have 2 columns.");
            for (int i = 0; i < 2; ++i)
                for (int j = 0; j < 2; ++j)
                    this[i, j] = A[i,j];
        }


        #region StaticConstruction

        /// <summary>Creates and returns a 2D matrix that is a copy of another 2D matrix.</summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrix2d Create(Matrix2d mat)
        {
            return new Matrix2d(mat);
        }

        /// <summary>Creates and returns a 2D matrix that is a copy of another (general) matrix.
        /// That matrix should be a 2*2 matrix, otherwise exception is thrown.</summary>
        /// <param name="mat">Matrix whose components are copied to the created matrix.
        /// Should be a 2*2 matrix, otherwisee exception is thrown.</param>
        /// <returns></returns>
        public static Matrix2d Create(IMatrix mat)
        {
            return new Matrix2d(mat);
        }

        /// <summary>Constructs a matrix from a copy of a 2-D array by deep-copy.</summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        public static Matrix2d Create(double[][] A)
        {
            return new Matrix2d(A);
        }

        /// <summary>Constructs a 2D matrix from a copy of a 2-D array by deep-copy.</summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        public static Matrix2d Create(double[,] A)
        {
            return new Matrix2d(A);
        }

        /// <summary>Construct a complex matrix from a set of real column vectors.</summary>
        public static Matrix2d CreateFromColumns(IList<IVector> columnVectors)
        {
            if (columnVectors == null)
                throw new ArgumentNullException("List of column vectors to create a 2D matrix from is not specified (null reference).");
            if (columnVectors.Count != 2)
                throw new ArgumentException("List of column vectors to create a 2D matrix from does not contain 2 vectors.");
            int rows = columnVectors[0].Length;
            int columns = columnVectors.Count;
            if (rows!=2)
                throw new ArgumentException("Length of column vectors to create a 2D matrix from is not 2.");
            double[,] newData = new double[2,2];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    newData[i,j] = columnVectors[j][i];
            return new Matrix2d(newData);
        }

        /// <summary>Construct a complex matrix from a set of real row vectors.</summary>
        public static Matrix2d CreateFromRows(IList<Vector> rowVectors)
        {
            if (rowVectors == null)
                throw new ArgumentNullException("List of row vectors to create a 2D matrix from is not specified (null reference).");
            int rows = rowVectors.Count;
            int columns = rowVectors[0].Length;
            if (rows != 2)
                throw new ArgumentException("List of row vectors to create a 2D matrix from does not contain 2 vectors.");
            if (columns != 2)
                throw new ArgumentException("Length of row vectors to create a 2D matrix from is not 2.");
            double[][] newData = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                newData[i] = rowVectors[i].ToArray();
            }
            return new Matrix2d(newData);
        }


        // TODO: add a couple of other creations from lists of doubles, from submatirices, etc.!


        /// <summary>Creates a d1*d2 identity matrix.</summary>
        /// <returns>An d1*d2 matrix with ones on the diagonal and zeros elsewhere.</returns>
        public static Matrix2d Identity()
        {
            Matrix2d ret = new Matrix2d(0.0);
            for (int i = 0; i < 2; i++)
            {
                ret[i, i] = 1.0;
            }
            return ret;
        }


        /// <summary>Creates a 2D matrix filled with 0.</summary>
        public static Matrix2d Zeros()
        {
            return new Matrix2d(0.0);
        }

       /// <summary>Creates a 2D matrix filled with 1.</summary>
        public static Matrix2d Ones()
        {
            return new Matrix2d(1.0);
        }

        /// <summary>Creates a new diagonal d1*d2 matrix based on the diagonal vector.</summary>
        /// <param name="diagonalVector">The values of the matrix diagonal.</param>
        /// <returns>A 2x2 matrix with the values from the diagonal vector on the diagonal and zeros elsewhere.</returns>
        public static Matrix2d Diagonal(IVector diagonalVector)
        {
            if (diagonalVector == null)
                throw new ArgumentNullException("Vector to create a diagonal 2D matrix from is not specified (nul reference).");
            if (diagonalVector.Length != 2)
                throw new ArgumentException("Vector to create a diagonal matrix from does not have 2 elements.");
            Matrix2d ret = new Matrix2d(0.0);
            for (int i = 0; i < 2; ++i)
                ret[i, i] = diagonalVector[i];
            return ret;
        }



        /// <summary>Creates and returns a 2D matrix with uniformly distributed random elements in the [0, 1) interval.</summary>
        /// <returns>A 2D matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
        public static Matrix2d Random()
        {
            Matrix2d ret = new Matrix2d(0.0);
            MatrixBase.SetRandom(ret);
            return ret;
        }



        /// <summary>Creates and returns a 2D matrix with uniformly distributed random elements in the [0, 1) interval.</summary>
        /// <param name="rnd">Random generator that is used for generation of elements.</param>
        /// <returns>A 2D matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
        public static Matrix2d Random(IRandomGenerator rnd)
        {
            Matrix2d ret = new Matrix2d(0.0);
            MatrixBase.SetRandom(ret, rnd);
            return ret;
        }

        #endregion StaticConstruction



        #endregion Construction
        

        #region Data

        private mat2 _m; // struct holding the data

        /// <summary>Gets the first dimension (number of rows) of the 2D matrix, i.e. 2.</summary>
        public override int RowCount
        { get { return 2; } }

        /// <summary>Gets the second dimension (number of columns) of the 2D matrix, i.e. 2.</summary>
        public override int ColumnCount
        { get { return 2; } }


        /// <summary>Gets the struct representation of this 2D matrix.</summary>
        protected mat2 Mat
        { 
            get { return _m; }
            set { _m = value; }
        }

        /// <summary>XX component.</summary>
        public double XX
        {
            get { return _m.xx; }
            set { _m.xx = value; }
        }

        /// <summary>XY component.</summary>
        public double XY
        {
            get { return _m.xy; }
            set { _m.xy = value; }
        }


        /// <summary>YX component.</summary>
        public double YX
        {
            get { return _m.yx; }
            set { _m.yx = value; }
        }

        /// <summary>YY component.</summary>
        public double YY
        {
            get { return _m.yy; }
            set { _m.yy = value; }
        }


        /// <summary>Index operator.</summary>
        /// <param name="i">First index of matrix element.</param>
        /// <param name="j">Second index of matrix element.</param>
        /// <returns>The specified component of a 2D matrix.</returns>
        public override double this[int i, int j]
        {
            get
            {
                if (i == 0)
                {
                    if (j == 0)
                        return _m.xx;
                    else if (j == 1)
                        return _m.xy;
                    else
                        throw new IndexOutOfRangeException("2D matrix does not have component ["
                            + i + ", " + j + "]");
                }
                else if (i == 1)
                {
                    if (j == 0)
                        return _m.yx;
                    else if (j == 1)
                        return _m.yy;
                    else
                        throw new IndexOutOfRangeException("2D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else
                    throw new IndexOutOfRangeException("2D vector does not have component [" + i + "]");
            }
            set
            {
                if (i == 0)
                {
                    if (j == 0)
                        _m.xx = value;
                    else if (j == 1)
                        _m.xy = value;
                    else
                        throw new IndexOutOfRangeException("2D matrix does not have component ["
                            + i + ", " + j + "]");
                }
                else if (i == 1)
                {
                    if (j == 0)
                        _m.yx = value;
                    else if (j == 1)
                        _m.yy = value;
                    throw new IndexOutOfRangeException("2D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else
                    throw new IndexOutOfRangeException("2D matrix does not have component ["
                        + i + ", " + j + "]");
            }
        } // this[i,j]


        /// <summary>Gets or sets the x-row of the 2D matrix.</summary>
        public Vector2d RowX
        {
            get { return new Vector2d(XX, XY); }
            set { XX = value.X; XY = value.Y; }
        }

        /// <summary>Gets or sets the y-row of the 2D matrix.</summary>
        public Vector2d RowY
        {
            get { return new Vector2d(YX, YY); }
            set { YX = value.X; YY = value.Y; }
        }

        /// <summary>Gets or sets the x-column of the 2D matrix.</summary>
        public Vector2d ColumnX
        {
            get { return new Vector2d(XX, YX); }
            set { XX = value.X; YX = value.Y; }
        }

        /// <summary>Gets or sets the y-column of the 2D matrix.</summary>
        public Vector2d ColumnY
        {
            get { return new Vector2d(XY, YY); }
            set { XY = value.X; YY = value.Y;  }
        }


        /// <summary>Returns a copy of the current 2D matrix.</summary>
        /// <returns></returns>
        public virtual Matrix2d GetCopyThis()
        {
            Matrix2d ret = new Matrix2d(0.0);
            for (int i = 0; i < 2; ++i)
                for (int j = 0; j < 2; ++j)
                    ret[i, j] = this[i, j];
            return ret;
        }

        /// <summary>Returns a deep copy of the current 2D matrix.</summary>
        public override MatrixBase GetCopyBase()
        {
            return GetCopyThis();
        }

        /// <summary>Returns a copy of the current 2D matrix.</summary>
        public Matrix2d GetNewThis(int rowCount, int ColumnCount)
        {
            if (rowCount != 2 || ColumnCount != 2)
                throw new ArgumentException("Creation of a new 2D matrix: both dimensions must be equal to 2.");
            return new Matrix2d(0.0);
        }



        /// <summary>Creates and returns a new 2D matrix with the specified dimensions, and of the same type as 
        /// the current matrix.</summary>
        /// <param name="rowCount">Number fo rows of the newly created matrix. Must be 2.</param>
        /// <param name="columnCount">Number of columns of the newly created matrix. Must be 2.</param>
        /// <returns>A newly created matrix of the specified dimensions and of the same type as the current matrix.</returns>
        public override MatrixBase GetNewBase(int rowCount, int columnCount)
        {
            if (rowCount != 2 || ColumnCount != 2)
                throw new ArgumentException("Creation of a new 2D matrix: both dimensions must be equal to 2.");
            return new Matrix2d(0.0);
        }


        /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
        /// the current matrix.</summary>
        public Matrix2d GetNewThis()
        {
            return new Matrix2d(0.0);
        }

        /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
        /// the current matrix.</summary>
        public override MatrixBase GetNewBase()
        {
            return new Matrix2d(0.0);
        }

        /// <summary>Creates and returns a new vector with the specified dimension, 
        /// and of the type that is consistent with the type of the current matrix.</summary>
        /// <param name="length">Dimension of the newly created vector.</param>
        /// <returns>A newly created vector of the specified dimension and of the same type as the current vector.</returns>
        public Vector2d GetNewVectorThis(int length)
        {
            if (length!=2)
                throw new ArgumentException("Creation of a new 2D vector: the dimension must be equal to 2.");
            return new Vector2d(0.0);
        }


        /// <summary>Creates and returns a new vector with the specified dimension, 
        /// and of the type that is consistent with the type of the current matrix.</summary>
        /// <param name="length">Dimension of the newly created vector.</param>
        /// <returns>A newly created vector of the specified dimension and of the same type as the current vector.</returns>
        public override VectorBase GetNewVectorBase(int length)
        {
            return GetNewVectorThis(length);
        }


        #endregion Data



        #region Auxiliary


        /// <summary>Returns the hash value (hash function) of the current matrix.</summary>
        /// <remarks>
        /// <para>This method calls the <see cref="MatrixBase.GetHashCode()"/> to calculate the 
        /// hash code, which is standard for all implementations of the <see cref="IMatrix"/> interface.</para>
        /// <para>Two matrices that have the same dimensions and equal elements will produce the same hash codes.</para>
        /// <para>Probability that two different matrices will produce the same hash code is small but it exists.</para>
        /// <para>Overrides the <see cref="object.GetHashCode"/> method.</para>
        /// </remarks>
        public override int GetHashCode()
        {
            return MatrixBase.GetHashCode(this);
        }

        /// <summary>Returns a value indicating whether the specified object is equal to the current matrix.
        /// <para>True is returned if the object is a non-null matrix (i.e. it implements the <see cref="IMatrix"/>
        /// interface), and has the same dimension and equal elements as the current matrix.</para></summary>
        /// <remarks>This method calls the <see cref="MatrixBase.Equals(object)"/> to obtain the returned value, which is
        /// standard for all implementations of the <see cref="IMatrix"/> interface.
        /// <para>Overrides the <see cref="object.Equals(object)"/> method.</para></remarks>
        public override bool Equals(Object obj)
        {
            return MatrixBase.Equals(this, obj as IMatrix);
        }

        #endregion Auxiliary


        #region Operations

        /// <summary>Get Forbenius (or euclidean) norm of the matrix - 
        /// square root of sum of squares of components.</summary>
        [Obsolete("Use NormForbenius instead!")]
        public override double NormEuclidean
        { get { return Mat.NormEuclidean; } }

        /// <summary>Get Forbenius (or Euclidean) norm of the matrix - 
        /// square root of sum of squares of components.</summary>
        public override double NormForbenius
        { get { return Mat.NormForbenius; } }

        /// <summary>Get Forbenius (or euclidean) norm of the matrix - 
        /// square root of sum of squares of components.</summary>
        public override double Norm
        { get { return Mat.Norm; } }

        /// <summary>Get the 1 norm of the matrix -
        /// maximum over columns of sum of absolute values of components.</summary>
        public double Norm1
        { get { return Mat.Norm1; } }

        /// <summary>Get the infinity norm of the matrix -
        /// maximum over rows of sum of absolute values of components.</summary>
        public double NormInfinity
        { get { return Mat.NormInfinity; } }

        /// <summary>Returns this matrix normalized with Euclidean norm.</summary>
        public Matrix2d NormalizedEuclidean()
        { return new Matrix2d(Mat.NormalizedEuclidean()); }

        /// <summary>Returns this matrix normalized with Euclidean norm.</summary>
        public Matrix2d NormalizedForbenius()
        { return new Matrix2d(Mat.NormalizedForbenius()); }

        /// <summary>Returns this matrix normalized with 1 norm.</summary>
        public Matrix2d Normalized1()
        { return new Matrix2d(Mat.Normalized1()); }

        /// <summary>Returns this matrix normalized with infinity norm.</summary>
        public Matrix2d NormalizedInfinity()
        { return new Matrix2d(Mat.NormalizedInfinity()); }

        /// <summary>Gets matrix determinant.</summary>
        public double Determinant
        { get { return Mat.Determinant; } }

        /// <summary>Gets matrix determinant.</summary>
        public double Det
        { get { return Mat.Det; } }

        /// <summary>Gets matrix trace (sum of diagonal elements).</summary>
        public override double Trace
        { get { return Mat.Trace; } }


        /// <summary>Gets transpose of the current matrix.</summary>
        public new Matrix2d T
        { get { return new Matrix2d(Mat.Transpose); } }


        /// <summary>Gets inverse of the current matrix.</summary>
        public Matrix2d Inverse
        { get { return new Matrix2d(Mat.Inverse); } }

        /// <summary>Gets inverse of the current matrix.</summary>
        public Matrix2d Inv { get { return this.Inverse; } }

        /// <summary>Returns solution of system of equations with the current system matrix and 
        /// the specified right-hand sides.</summary>
        /// <param name="b">Vector of right-hand sides of equations.</param>
        /// <returns></returns>
        public Vector2d Solve(Vector2d b)
        { return new Vector2d(Mat.Solve(b.Vec)); }


        /// <summary>Returns sum of the current matrix and the specified matrix.</summary>
        public Matrix2d Add(Matrix2d a)
        { return new Matrix2d(Mat.Add(a.Mat)); }

        /// <summary>Returns difference between the current matrix and the specified matrix.</summary>
        public Matrix2d Subtract(Matrix2d a)
        { return new Matrix2d(Mat.Subtract(a.Mat)); }

        /// <summary>Right-multiplies the current 2D matrix with the specified matrix and returns the product.</summary>
        /// <param name="b">Right-hand side factor of multiplication.</param>
        /// <returns>this*b</returns>
        public Matrix2d MultiplyRight(Matrix2d b)
        { return new Matrix2d(Mat.MultiplyRight(b.Mat)); }

        /// <summary>Left-multiplies the current 2D matrix with the specified matrix and returns the product.</summary>
        /// <param name="b">Left-hand side factor of multiplication.</param>
        /// <returns>b*this</returns>
        public Matrix2d MultiplyLeft(Matrix2d b)
        {
            return new Matrix2d(Mat.MultiplyLeft(b.Mat));
        }


        /// <summary>Right-multiplies the current 2D matrix with the specified 2D vector and returns the product.</summary>
        /// <param name="b">Right-hand side factor of multiplication.</param>
        /// <returns>this*b</returns>
        public Vector2d Multiply(Vector2d b)
        { return new Vector2d(Mat.Multiply(b.Vec)); }

        /// <summary>Multiplies the current 2D matrix with the specified scalar and returns the product.</summary>
        /// <param name="b">Factor of multiplication.</param>
        /// <returns>this*b</returns>
        public Matrix2d Multiply(double b)
        { return new Matrix2d(Mat.Multiply(b)); }

        #endregion Operations


        #region StaticMethods

        /// <summary>Returns a copy of the specified 2D matrix.</summary>
        /// <param name="m">Matrix whose copy is returned.</param>
        public static Matrix2d Copy(Matrix2d m)
        { return new Matrix2d(m); }

        /// <summary>Negates the specified 2D matrix and stores its copy in the resulting matrix.</summary>
        /// <param name="m">Matrix to be negated.</param>
        /// <param name="res">Matrix where the result is stored.</param>
        public static void Negate(Matrix2d m, ref Matrix2d res)
        {
            mat2 lm = m.Mat, lres = res.Mat;
            mat2.Negate(lm, ref lres);
            res.Mat = lres ;
        }



        /// <summary>Calculates eigenvectors and eigenvalues of a 2x2 matrix a and stores
        /// eigenvectors to lines of eigenvec and eigenvalues to eigenval. eigenvec
        /// can be the same matrix as a. The number of different real eigenvalues is
        /// returned.
        ///   Ref.: linalg.nb
        /// Not tested yet!
        /// </summary>
        /// <param name="a">2D Matrix whose eigenvalues and eigenvectors are calculated.</param>
        /// <param name="eigenvec">Matrix where eigenvectors are stored as rows.</param>
        /// <param name="eigenval">Vector where eigenvalues are stored.</param>
        /// <returns>The number of different real eigenvalues.</returns>
        /// $A Igor Aug08, Oct10;
        public static int EigenSystem2d(Matrix2d a, ref Matrix2d eigenvec, ref Vector2d eigenval)
        {
            if (eigenvec == null)
                eigenvec = new Matrix2d(0.0);
            if (eigenval==null)
                eigenval=new Vector2d(0.0);
            mat2 eigvec = eigenvec.Mat;
            vec2 eigval = eigenval.Vec;
            int ret = mat2.EigenSystem2d(a.Mat, ref eigvec, ref eigval);
            eigenvec.Mat = eigvec;
            eigenval.Vec = eigval;
            return ret;
        }  // EigenSystem2d

        // -> -> ->


        #endregion StaticMethods


        #region OperatorsOverloading

        /// <summary>Unary plus for 2D matrices, returns the operand.</summary>
        public static Matrix2d operator +(Matrix2d m)
        {
            return Copy(m);
        }

        /// <summary>Unary negation for 2D matrices, returns the negative operand.</summary>
        public static Matrix2d operator -(Matrix2d a)
        {
            return new Matrix2d(
                -a.XX, -a.XY,
                -a.YX, -a.YY
                );
        }

        /// <summary>Matrix addition in 2D.</summary>
        public static Matrix2d operator +(Matrix2d a, Matrix2d b)
        {
            return a.Add(b);
        }


        /// <summary>Matrix subtraction in 2D.</summary>
        public static Matrix2d operator -(Matrix2d a, Matrix2d b)
        {
            return a.Subtract(b); ;
        }

        /// <summary>Matrix multiplication in 2D.</summary>
        public static Matrix2d operator *(Matrix2d a, Matrix2d b)
        {
            return a.MultiplyRight(b); ;
        }

        /// <summary>Matrix with vector multiplication in 2D.</summary>
        public static Vector2d operator *(Matrix2d a, Vector2d b)
        {
            return a.Multiply(b); ;
        }


        /// <summary>Product of a 2D matrix by a scalar.</summary>
        public static Matrix2d operator *(Matrix2d a, double b)
        {
            return a.Multiply(b);
        }

        /// <summary>Product of a 2D matrix by a scalar.</summary>
        public static Matrix2d operator *(double a, Matrix2d b)
        {
            return b.Multiply(a);
        }

        /// <summary>Division of a 2D matrix by a scalar.</summary>
        public static Matrix2d operator /(Matrix2d a, double b)
        {
            return a.Multiply(1 / b);
        }


        #endregion  OperatorsOverloading


        #region Misc_Utilities


        /// <summary>Returns a deep copy of a vector.</summary>
        object ICloneable.Clone()
        {
            return Create(this);
        }


        #endregion  // Misc_Utilities



        #region InputOutput

        /// <summary>Reads 2D matrix components from a console.</summary>
        public void Read()
        {
            Read(null);
        }


        /// <summary>Reads 2D matrix components from a console.</summary>
        /// <param name="name">Name of the matrix to be read; it is written as orientation to the user
        /// and can be null.</param>
        public void Read(string name)
        {
            _m.Read(name);
        }

        #endregion InputOutput


    }  // class Matrix2d


}
