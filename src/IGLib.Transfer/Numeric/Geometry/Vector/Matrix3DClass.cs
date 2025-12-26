// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/



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

    /// <summary>Matrix in a 3 dimensional space.</summary>
    /// $A Igor Jul08; Oct10;
    [Serializable]
    public class Matrix3d : MatrixBase, IMatrix
    {


        #region Construction

        /// <summary>Copy constructor.
        /// Initializes components of a 3D matrix with components of another 3D matrix.</summary>
        /// <param name="m">Matrix whose components are copied to the initialized matrix.</param>
        public Matrix3d(Matrix3d m)
        {
            if (m==null)
                throw new ArgumentNullException("Matrix is not specified in copy constructor (null reference).");
            XX = m.XX;  XY = m.XY;  XZ = m.XZ;
            YX = m.YX;  YY = m.YY;  YZ = m.YZ;
            ZX = m.ZX;  ZY = m.ZY;  ZZ = m.ZZ;
        }

        /// <summary>Constructor.
        /// Initializes components of a 3D matrix with components of another matrix.
        /// The specified matrix should be 3*3.</summary>
        /// <param name="m">Matrix whose components are copied to the initialized matrix (should be 3*3).</param>
        public Matrix3d(IMatrix m)
        {
            if (m == null)
                throw new ArgumentNullException("Matrix is not specified in constructor (null reference).");
            else if (m.RowCount != 3 || m.ColumnCount != 3)
                throw new ArgumentException("Incorrect dimensons of matrix passed to Matrix3D constructor, should be 3*3."
                    + Environment.NewLine + "  Actual dimensions: " + m.RowCount + "*" + m.ColumnCount + ".");
            XX = m[0,0];  XY = m[0,1];  XZ = m[0,2];
            YX = m[1,0];  YY = m[1,1];  YZ = m[1,2];
            ZX = m[2, 0]; ZY = m[2, 1]; ZZ = m[2, 2];
        }

        /// <summary>Initializes 3D matrix structure with the specified components.</summary>
        public Matrix3d(Vector3d rowx, Vector3d rowy, Vector3d rowz)
        {
            if (rowx == null || rowy == null || rowz == null)
                throw new ArgumentNullException("One or more row vectors are not specified in constructor (null reference).");
            XX = rowx.X; XY = rowx.Y; XZ = rowx.Z;
            YX = rowy.X; YY = rowy.Y; YZ = rowy.Z;
            ZX = rowz.X; ZY = rowz.Y; ZZ = rowz.Z;
        }

        /// <summary>Initializes components of a 3D matrix with components of the specified matrix.</summary>
        /// <param name="m">Matrix whose components are copied to the initialized matrix.</param>
        public Matrix3d(mat3 m)
        { _m=m; }

        /// <summary>Initializes 3D matrix structure with the specified components.</summary>
        public Matrix3d(double xx, double xy, double xz, 
                    double yx, double yy, double yz,
                    double zx, double zy, double zz)
        {
            this.XX = xx; this.XY = xy; this.XZ = xz;
            this.YX = yx; this.YY = yy; this.YZ = yz;
            this.ZX = zx; this.ZY = zy; this.ZZ = zz;
        }

        /// <summary>Initializes 3D with the specified component.</summary>
        /// <param name="component">Value that is assigned to all matrix components.</param>
        public Matrix3d(double component)
        {
            this.XX = component; this.XY = component; this.XZ = component;
            this.YX = component; this.YY = component; this.YZ = component;
            this.ZX = component; this.ZY = component; this.ZZ = component;
        }

        /// <summary>Initializes 3D matrix structure with the specified components.</summary>
        public Matrix3d(vec3 xrow, vec3 yrow, vec3 zrow)
        {
            _m = new mat3(xrow, yrow, zrow);
        }


        /// <summary>Initializes a 3D matrix with elements of a jagged array.</summary>
        /// <param name="A">Array from which a 3D matrix is constructed.</param>
        public Matrix3d(double[][] A)
        {
            if (A == null)
                throw new ArgumentNullException("Jagged array to create a 3D matrix from is not specified (null reference).");
            if (A.Rank != 2)
                throw new ArgumentException("Jagged array to create a 3D matrix from is not a rank 2 array.");
            if (A.Length != 3)
                throw new ArgumentException("Jagged array to create a 3D matrix from does not have 3 rows.");
            for (int i = 0; i < 3; ++i)
            {
                if (A[i].Length != 3)
                    throw new ArgumentException("Jagged array to create a 2D matrix from has inconsistent dimensions. Row "
                        + i + "has length " + A[i].Length + "Instead of 3.");
                for (int j = 0; j < 3; ++j)
                    this[i, j] = A[i][j];
            }
        }

        /// <summary>Initializes a 3D matrix with elements of a rectangular array.</summary>
        /// <param name="A">Array from which a 3D matrix is constructed.</param>
        public Matrix3d(double[,] A)
        {
            if (A == null)
                throw new ArgumentNullException("Rectangular array to create a 3D matrix from is not specified (null reference).");
            if (A.GetLength(0) != 3)
                throw new ArgumentException("Rectangular array to create a 3D matrix does not have 3 rows.");
            if (A.GetLength(1) != 3)
                throw new ArgumentException("Rectangular array to create a 3D matrix from does not have 3 columns.");
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    this[i, j] = A[i,j];
        }


        #region StaticConstruction

        /// <summary>Creates and returns a 3D matrix that is a copy of another 3D matrix.</summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrix3d Create(Matrix3d mat)
        {
            return new Matrix3d(mat);
        }

        /// <summary>Creates and returns a 3D matrix that is a copy of another (general) matrix.
        /// That matrix should be a 3*3 matrix, otherwise exception is thrown.</summary>
        /// <param name="mat">Matrix whose components are copied to the created matrix.
        /// Should be a 3*3 matrix, otherwisee exception is thrown.</param>
        /// <returns></returns>
        public static Matrix3d Create(IMatrix mat)
        {
            return new Matrix3d(mat);
        }

        /// <summary>Constructs a matrix from a copy of a 2-D array by deep-copy.</summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        public static Matrix3d Create(double[][] A)
        {
            return new Matrix3d(A);
        }

        /// <summary>Constructs a 3D matrix from a copy of a 2-D array by deep-copy.</summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        public static Matrix3d Create(double[,] A)
        {
            return new Matrix3d(A);
        }

        /// <summary>Construct a complex matrix from a set of real column vectors.</summary>
        public static Matrix3d CreateFromColumns(IList<IVector> columnVectors)
        {
            if (columnVectors == null)
                throw new ArgumentNullException("List of column vectors to create a 3D matrix from is not specified (null reference).");
            if (columnVectors.Count != 3)
                throw new ArgumentException("List of column vectors to create a 3D matrix from does not contain 3 vectors.");
            int rows = columnVectors[0].Length;
            int columns = columnVectors.Count;
            if (rows!=3)
                throw new ArgumentException("Length of column vectors to create a 3D matrix from is not 3.");
            double[,] newData = new double[3,3];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    newData[i,j] = columnVectors[j][i];
            return new Matrix3d(newData);
        }

        /// <summary>Construct a complex matrix from a set of real row vectors.</summary>
        public static Matrix3d CreateFromRows(IList<Vector> rowVectors)
        {
            if (rowVectors == null)
                throw new ArgumentNullException("List of row vectors to create a 3D matrix from is not specified (null reference).");
            int rows = rowVectors.Count;
            int columns = rowVectors[0].Length;
            if (rows != 3)
                throw new ArgumentException("List of row vectors to create a 3D matrix from does not contain 3 vectors.");
            if (columns != 3)
                throw new ArgumentException("Length of row vectors to create a 3D matrix from is not 3.");
            double[][] newData = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                newData[i] = rowVectors[i].ToArray();
            }
            return new Matrix3d(newData);
        }


        // TODO: add a couple of other creations from lists of doubles, from submatirices, etc.!


        /// <summary>Creates a d1*d2 identity matrix.</summary>
        /// <returns>An d1*d2 matrix with ones on the diagonal and zeros elsewhere.</returns>
        public static Matrix3d Identity()
        {
            Matrix3d ret = new Matrix3d(0.0);
            for (int i = 0; i < 3; i++)
            {
                ret[i, i] = 1.0;
            }
            return ret;
        }


        /// <summary>Creates a 3D matrix filled with 0.</summary>
        public static Matrix3d Zeros()
        {
            return new Matrix3d(0.0);
        }

       /// <summary>Creates a 3D matrix filled with 1.</summary>
        public static Matrix3d Ones()
        {
            return new Matrix3d(1.0);
        }

        /// <summary>Creates a new diagonal d1*d2 matrix based on the diagonal vector.</summary>
        /// <param name="diagonalVector">The values of the matrix diagonal.</param>
        /// <returns>A d1*d2 matrix with the values from the diagonal vector on the diagonal and zeros elsewhere.</returns>
        public static Matrix3d Diagonal(IVector diagonalVector)
        {
            if (diagonalVector == null)
                throw new ArgumentNullException("Vector to create a diagonal 3D matrix from is not specified (nul reference).");
            if (diagonalVector.Length != 3)
                throw new ArgumentException("Vector to create a diagonal matrix from does not have 3 elements.");
            Matrix3d ret = new Matrix3d(0.0);
            for (int i = 0; i < 3; ++i)
                ret[i, i] = diagonalVector[i];
            return ret;
        }



        /// <summary>Creates and returns a 3D matrix with uniformly distributed random elements in the [0, 1) interval.</summary>
        /// <returns>A 3D matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
        public static Matrix3d Random()
        {
            Matrix3d ret = new Matrix3d(0.0);
            MatrixBase.SetRandom(ret);
            return ret;
        }



        /// <summary>Creates and returns a 3D matrix with uniformly distributed random elements in the [0, 1) interval.</summary>
        /// <param name="rnd">Random generator that is used for generation of elements.</param>
        /// <returns>A 3D matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
        public static Matrix3d Random(IRandomGenerator rnd)
        {
            Matrix3d ret = new Matrix3d(0.0);
            MatrixBase.SetRandom(ret, rnd);
            return ret;
        }

        #endregion StaticConstruction



        #endregion Construction
        

        #region Data


        private mat3 _m;

        /// <summary>Gets the first dimension (number of rows) of the 3D matrix, i.e. 3.</summary>
        public override int RowCount
        { get { return 3; } }

        /// <summary>Gets the second dimension (number of columns) of the 3D matrix, i.e. 3.</summary>
        public override int ColumnCount
        { get { return 3; } }

        /// <summary>Gets the struct representation of this 3D matrix.</summary>
        protected mat3 Mat
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

        /// <summary>XZ component.</summary>
        public double XZ
        {
            get { return _m.xz; }
            set { _m.xz = value; }
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

        /// <summary>YZ component.</summary>
        public double YZ
        {
            get { return _m.yz; }
            set { _m.yz = value; }
        }

        /// <summary>ZX component.</summary>
        public double ZX
        {
            get { return _m.zx; }
            set { _m.zx = value; }
        }

        /// <summary>ZY component.</summary>
        public double ZY
        {
            get { return _m.zy; }
            set { _m.zy = value; }
        }

        /// <summary>ZZ component.</summary>
        public double ZZ
        {
            get { return _m.zz; }
            set { _m.zz = value; }
        }

        /// <summary>Index operator.</summary>
        /// <param name="i">Component index.</param>
        /// <param name="j">Component index.</param>
        /// <returns>The specified component of a 3D vector.</returns>
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
                    else if (j == 2)
                        return _m.xz;
                    else
                        throw new IndexOutOfRangeException("3D matrix does not have component ["
                            + i + ", " + j + "]");
                }
                else if (i == 1)
                {
                    if (j == 0)
                        return _m.yx;
                    else if (j == 1)
                        return _m.yy;
                    else if (j == 2)
                        return _m.yz;
                    throw new IndexOutOfRangeException("3D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else if (i == 2)
                {
                    if (j == 0)
                        return _m.zx;
                    else if (j == 1)
                        return _m.zy;
                    else if (j == 2)
                        return _m.zz;
                    throw new IndexOutOfRangeException("3D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else
                    throw new IndexOutOfRangeException("3D vector does not have component [" + i + "]");
            }
            set
            {
                if (i == 0)
                {
                    if (j == 0)
                        _m.xx = value;
                    else if (j == 1)
                        _m.xy = value;
                    else if (j == 2)
                        _m.xz = value;
                    else
                        throw new IndexOutOfRangeException("3D matrix does not have component ["
                            + i + ", " + j + "]");
                }
                else if (i == 1)
                {
                    if (j == 0)
                        _m.yx = value;
                    else if (j == 1)
                        _m.yy = value;
                    else if (j == 2)
                        _m.yz = value;
                    throw new IndexOutOfRangeException("3D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else if (i == 2)
                {
                    if (j == 0)
                        _m.zx = value;
                    else if (j == 1)
                        _m.zy = value;
                    else if (j == 2)
                        _m.zz = value;
                    throw new IndexOutOfRangeException("3D matrix does not have component ["
                        + i + ", " + j + "]");
                }
                else
                    throw new IndexOutOfRangeException("3D matrix does not have component ["
                        + i + ", " + j + "]");
            }
        } // this[i,j]


        /// <summary>Gets or sets the x-row of the 3D matrix.</summary>
        public Vector3d RowX
        {
            get { return new Vector3d(XX, XY, XZ); }
            set { XX = value.X; XY = value.Y; XZ = value.Z; }
        }

        /// <summary>Gets or sets the y-row of the 3D matrix.</summary>
        public Vector3d RowY
        {
            get { return new Vector3d(YX, YY, YZ); }
            set { YX = value.X; YY = value.Y; YZ = value.Z; }
        }

        /// <summary>Gets or sets the z-row of the 3D matrix.</summary>
        public Vector3d RowZ
        {
            get { return new Vector3d(ZX, ZY, ZZ); }
            set { ZX = value.X; ZY = value.Y; ZZ = value.Z; }
        }

        /// <summary>Gets or sets the x-column of the 3D matrix.</summary>
        public Vector3d ColumnX
        {
            get { return new Vector3d(XX, YX, ZX); }
            set { XX = value.X; YX = value.Y; ZX = value.Z; }
        }

        /// <summary>Gets or sets the y-column of the 3D matrix.</summary>
        public Vector3d ColumnY
        {
            get { return new Vector3d(XY, YY, ZY); }
            set { XY = value.X; YY = value.Y; ZY = value.Z; }
        }

        /// <summary>Gets or sets the z-column of the 3D matrix.</summary>
        public Vector3d ColumnZ
        {
            get { return new Vector3d(XZ, YZ, ZZ); }
            set { XZ = value.X; YZ = value.Y; ZZ = value.Z; }
        }



        /// <summary>Returns a copy of the current 3D matrix.</summary>
        /// <returns></returns>
        public virtual Matrix3d GetCopyThis()
        {
            Matrix3d ret = new Matrix3d(0.0);
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    ret[i, j] = this[i, j];
            return ret;
        }

        /// <summary>Returns a deep copy of the current 3D matrix.</summary>
        public override MatrixBase GetCopyBase()
        {
            return GetCopyThis();
        }

        /// <summary>Returns a copy of the current 3D matrix.</summary>
        public Matrix3d GetNewThis(int rowCount, int ColumnCount)
        {
            if (rowCount != 3 || ColumnCount != 3)
                throw new ArgumentException("Creation of a new 3D matrix: both dimensions must be equal to 3.");
            return new Matrix3d(0.0);
        }

        /// <summary>Creates and returns a new 3D matrix with the specified dimensions, and of the same type as 
        /// the current matrix.</summary>
        /// <param name="rowCount">Number fo rows of the newly created matrix. Must be 3.</param>
        /// <param name="columnCount">Number of columns of the newly created matrix. Must be 3.</param>
        /// <returns>A newly created matrix of the specified dimensions and of the same type as the current matrix.</returns>
        public override MatrixBase GetNewBase(int rowCount, int columnCount)
        {
            if (rowCount != 3 || ColumnCount != 3)
                throw new ArgumentException("Creation of a new 3D matrix: both dimensions must be equal to 3.");
            return new Matrix3d(0.0);
        }

        
                
        /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
        /// the current matrix.</summary>
        public Matrix3d GetNewThis()
        {
            return new Matrix3d(0.0);
        }
                
        /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
        /// the current matrix.</summary>
        public override MatrixBase GetNewBase()
        {
            return new Matrix3d(0.0);
        }


        /// <summary>Creates and returns a new 3D vector with the specified dimension (which must be null), 
        /// and of the type that is consistent with the type of the current matrix.</summary>
        /// <param name="length">Dimension of the newly created vector.</param>
        /// <returns>A newly created vector of the specified dimension and of the same type as the current vector.</returns>
        public Vector3d GetNewVectorThis(int length)
        {
            if (length != 3)
                throw new ArgumentException("Creation of a new 3D vector: the dimension must be equal to 2.");
            return new Vector3d(0.0);
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


        /// <summary>Returns the hash code (hash function) of the current matrix.</summary>
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
        public override double NormForbenius
        { get { return Mat.NormForbenius; } }

        /// <summary>Get Forbenius (or euclidean) norm of the matrix - 
        /// square root of sum of squares of components.</summary>
        [Obsolete("Use NormForbenius instead!")]
        public override double NormEuclidean
        { get { return Mat.NormEuclidean; } }

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
        public Matrix3d NormalizedEuclidean()
        { return new Matrix3d(Mat.NormalizedEuclidean()); }

        /// <summary>Returns this matrix normalized with Euclidean norm.</summary>
        public Matrix3d NormalizedForbenius()
        { return new Matrix3d(Mat.NormalizedForbenius()); }

        /// <summary>Returns this matrix normalized with 1 norm.</summary>
        public Matrix3d Normalized1()
        { return new Matrix3d(Mat.Normalized1()); }

        /// <summary>Returns this matrix normalized with infinity norm.</summary>
        public Matrix3d NormalizedInfinity()
        { return new Matrix3d(Mat.NormalizedInfinity()); }

        /// <summary>Gets matrix determinant.</summary>
        double Determinant
        { get { return Mat.Determinant; } }

        /// <summary>Gets matrix determinant.</summary>
        double Det
        { get { return Mat.Det; } }

        /// <summary>Gets matrix trace (sum of diagonal elements).</summary>
        public override double Trace
        { get { return Mat.Trace; } }

        /// <summary>Gets transpose of the current matrix.</summary>
        public new Matrix3d T
        { get { return new Matrix3d(Mat.Transpose); } }

        /// <summary>Gets inverse of the current matrix.</summary>
        Matrix3d Inverse
        { get { return new Matrix3d(Mat.Inverse); } }

        /// <summary>Gets inverse of the current matrix.</summary>
        Matrix3d Inv { get { return this.Inverse; } }

        /// <summary>Returns solution of system of equations with the current system matrix and 
        /// the specified right-hand sides.</summary>
        /// <param name="b">Vector of right-hand sides of equations.</param>
        /// <returns></returns>
        Vector3d Solve(Vector3d b)
        { return new Vector3d(Mat.Solve(b.Vec)); }


        /// <summary>Returns sum of the current matrix and the specified matrix.</summary>
        Matrix3d Add(Matrix3d a)
        { return new Matrix3d(Mat.Add(a.Mat)); }

        /// <summary>Returns difference between the current matrix and the specified matrix.</summary>
        Matrix3d Subtract(Matrix3d a)
        { return new Matrix3d(Mat.Subtract(a.Mat)); }

        /// <summary>Right-multiplies the current 3D matrix with the specified matrix and returns the product.</summary>
        /// <param name="b">Right-hand side factor of multiplication.</param>
        /// <returns>this*b</returns>
        Matrix3d MultiplyRight(Matrix3d b)
        { return new Matrix3d(Mat.MultiplyRight(b.Mat)); }

        /// <summary>Left-multiplies the current 3D matrix with the specified matrix and returns the product.</summary>
        /// <param name="b">Left-hand side factor of multiplication.</param>
        /// <returns>b*this</returns>
        Matrix3d MultiplyLeft(Matrix3d b)
        {
            return new Matrix3d(Mat.MultiplyLeft(b.Mat));
        }


        /// <summary>Right-multiplies the current 3D matrix with the specified 3D vector and returns the product.</summary>
        /// <param name="b">Right-hand side factor of multiplication.</param>
        /// <returns>this*b</returns>
        Vector3d Multiply(Vector3d b)
        { return new Vector3d(Mat.Multiply(b.Vec)); }

        /// <summary>Multiplies the current 3D matrix with the specified scalar and returns the product.</summary>
        /// <param name="b">Factor of multiplication.</param>
        /// <returns>this*b</returns>
        Matrix3d Multiply(double b)
        { return new Matrix3d(Mat.Multiply(b)); }

        #endregion Operations


        #region StaticMethods

        /// <summary>Returns a copy of the specified 3D matrix.</summary>
        /// <param name="m">Matrix whose copy is returned.</param>
        public static Matrix3d Copy(Matrix3d m)
        { return new Matrix3d(m); }

        /// <summary>Negates the specified 3D matrix and stores its copy in the resulting matrix.</summary>
        /// <param name="m">Matrix to be negated.</param>
        /// <param name="res">Matrix where the result is stored.</param>
        public static void Negate(Matrix3d m, ref Matrix3d res)
        {
            mat3 lm = m.Mat, lres = res.Mat;
            mat3.Negate(lm, ref lres);
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
        public static int EigenSystem2d(Matrix3d a, ref Matrix3d eigenvec, ref Vector3d eigenval)
        {
            if (eigenvec == null)
                eigenvec = new Matrix3d(0.0);
            if (eigenval==null)
                eigenval=new Vector3d(0.0);
            mat3 eigvec = eigenvec.Mat;
            vec3 eigval = eigenval.Vec;
            int ret = mat3.EigenSystem2d(a.Mat, ref eigvec, ref eigval);
            eigenvec.Mat = eigvec;
            eigenval.Vec = eigval;
            return ret;
        }  // EigenSystem2d

        // -> -> ->


        #endregion StaticMethods


        #region OperatorsOverloading

        /// <summary>Unary plus for 3D matrices, returns the operand.</summary>
        public static Matrix3d operator +(Matrix3d m)
        {
            return Copy(m);
        }

        /// <summary>Unary negation for 3D matrices, returns the negative operand.</summary>
        public static Matrix3d operator -(Matrix3d a)
        {
            return new Matrix3d(
                -a.XX, -a.XY, -a.XZ,
                -a.YX, -a.YY, -a.YZ,
                -a.ZX, -a.ZY, -a.ZZ
                );
        }

        /// <summary>Matrix addition in 3D.</summary>
        public static Matrix3d operator +(Matrix3d a, Matrix3d b)
        {
            return a.Add(b);
        }


        /// <summary>Matrix subtraction in 3D.</summary>
        public static Matrix3d operator -(Matrix3d a, Matrix3d b)
        {
            return a.Subtract(b); ;
        }

        /// <summary>Matrix multiplication in 3D.</summary>
        public static Matrix3d operator *(Matrix3d a, Matrix3d b)
        {
            return a.MultiplyRight(b); ;
        }

        /// <summary>Matrix with vector multiplication in 3D.</summary>
        public static Vector3d operator *(Matrix3d a, Vector3d b)
        {
            return a.Multiply(b); ;
        }


        /// <summary>Product of a 3D matrix by a scalar.</summary>
        public static Matrix3d operator *(Matrix3d a, double b)
        {
            return a.Multiply(b);
        }

        /// <summary>Product of a 3D matrix by a scalar.</summary>
        public static Matrix3d operator *(double a, Matrix3d b)
        {
            return b.Multiply(a);
        }

        /// <summary>Division of a 3D matrix by a scalar.</summary>
        public static Matrix3d operator /(Matrix3d a, double b)
        {
            return a.Multiply(1 / b);
        }


        #endregion  OperatorsOverloading



        #region InputOutput

        /// <summary>Reads 3D matrix components from a console.</summary>
        public void Read()
        {
            Read(null);
        }


        /// <summary>Reads 3D matrix components from a console.</summary>
        /// <param name="name">Name of the matrix to be read; it is written as orientation to the user
        /// and can be null.</param>
        public void Read(string name)
        {
            _m.Read(name);
        }

        #endregion InputOutput


    }  // class Matrix3d


}
