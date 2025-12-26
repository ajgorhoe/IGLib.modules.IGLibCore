// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

    // MATRICES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


//using Matrix_MathNet = MathNet.Numerics.LinearAlgebra.Matrix;

using Matrix_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix;
using MatrixBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Matrix<double>;


namespace IG.Num
{

    /// <summary>Real matrix class.
    /// Some operations are performed by the MathNet.Numerics.LinearAlgebra.Matrix class.</summary>
    /// $A Igor Jan08 Jul10 Nov10;
    [Serializable]
    public class Matrix : MatrixBase, IMatrix
    {

        #region Construction

        /// <summary>Creates a new matrix with dimensions 0.
        /// <para>Such a matrix can serve for future reallocation.</para></summary>
        protected Matrix()
        {
            // Base = null;
            _elements = null;
            _rowCount = 0;
            _columnCount = 0;
        }


        /// <summary>Constructs a matrix from another matrix by copying the provided matrix components 
        /// to the internal data structure.</summary>
        /// <param name="A">Matrix whose components are copied to the current matrix.</param>
        public Matrix(IMatrix A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to copy new matrix components from is not specified (null reference).");
            if (A.RowCount == 0)
                throw new ArgumentException("Matrix to create a new matrix from has 0 rows.");
            if (A.ColumnCount == 0)
                throw new ArgumentException("Matrix to create a new matrix from has 0 columns.");
            // Copy the array to a jagged array:
            int numRows = _rowCount = A.RowCount;
            int numColumns = _columnCount = A.ColumnCount;
            _elements = new double[numRows][];
            for (int i = 0; i < numRows; ++i)
            {
                _elements[i] = new double[numColumns];
                for (int j = 0; j < numColumns; ++j)
                    _elements[i][j] = A[i, j];
            }
        }


        /// <summary>Construct a matrix from MathNet.Numerics.LinearAlgebra.Matrix.
        /// Only a reference of A is copied.</summary>
        /// <param name="A">MathNet.Numerics.LinearAlgebra.Matrix from which a new matrix is created.</param>
        public Matrix(MatrixBase_MathNetNumerics A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to copy new matrix components from is not specified (null reference).");
            if (A.RowCount <= 0)
                throw new ArgumentException("Matrix to create a new matrix from has 0 rows.");
            if (A.ColumnCount <= 0)
                throw new ArgumentException("Matrix to create a new matrix from has 0 columns.");
            // Copy the array to a jagged array:
            int numRows = _rowCount = A.RowCount;
            int numColumns = _columnCount = A.ColumnCount;
            _elements = new double[numRows][];
            for (int i = 0; i < numRows; ++i)
            {
                _elements[i] = new double[numColumns];
                for (int j = 0; j < numColumns; ++j)
                    _elements[i][j] = A[i, j];
            }
        }

        /// <summary>Constructs an d1*d2 - dimensional matrix of zeros.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        public Matrix(int d1, int d2)
        {
            if (d1 <= 0)
                throw new ArgumentException("Can not create a matrix with number of rows less thatn 1. Provided number of rows: "
                    + d1.ToString() + ".");
            if (d2 <= 0)
                throw new ArgumentException("Can not create a matrix with number of columns less thatn 1. Provided number of columns: "
                    + d2.ToString() + ".");
            _rowCount = d1;
            _columnCount = d2;
            _elements = new double[d1][];
            for (int i = 0; i < d1; ++i)
            {
                _elements[i] = new double[d2];
                for (int j = 0; j < d2; ++j)
                    _elements[i][j] = 0;
            }
        }

        /// <summary> Construct an numrows-by-d2 constant matrix with specified value for all elements.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        /// <param name="val">Value of all components.</param>
        public Matrix(int d1, int d2, double val)
        {
            if (d1 <= 0)
                throw new ArgumentException("Can not create a matrix with number of rows less thatn 1. Provided number of rows: "
                    + d1.ToString() + ".");
            if (d2 <= 0)
                throw new ArgumentException("Can not create a matrix with number of columns less thatn 1. Provided number of columns: "
                    + d2.ToString() + ".");
            _elements = new double[d1][];
            for (int i = 0; i < d1; ++i)
            {
                _elements[i] = new double[d2];
                for (int j = 0; j < d2; ++j)
                    _elements[i][j] = val;
            }
        }

        /// <summary>Constructs a square diagonal matrix with specified diagonal values.</summary>
        /// <param name="diagonal">Vector containing diagonal elements of the created matrix.</param>
        public Matrix(IVector diagonal)
        {
            if (diagonal == null)
                throw new ArgumentNullException("Can not create a square diagonal matrix, vector of diagonal elements not specified (null reference).");
            int dim = diagonal.Length;
            if (dim<=0)
                throw new ArgumentException("Can not create a square diagonal matrix, dimension of diagonal vector is 0.");
            _rowCount = _columnCount = dim;
            _elements = new double[dim][];
            for (int i = 0; i < dim; ++i)
            {
                _elements[i] = new double[dim];
                for (int j = 0; j < dim; ++j)
                {
                    if (i == j)
                        _elements[i][j] = diagonal[i];
                    else
                        _elements[i][j] = 0.0;
                }
            }
        }

        /// <summary>Constructs a d*d square matrix with specified diagonal value.</summary>
        /// <param name="dim">Size of the square matrix.</param>
        /// <param name="elementValue">Diagonal value.</param>
        public Matrix(int dim, double elementValue)
        {
            if (dim <= 0)
                throw new ArgumentException("Can not create a square matrix with number of rows and columns less thatn 1. Provided number of rows: "
                    + dim.ToString() + ".");
            _rowCount = _columnCount = dim;
            _elements = new double[dim][];
            for (int i = 0; i < dim; ++i)
            {
                _elements[i] = new double[dim];
                for (int j = 0; j < dim; ++j)
                    _elements[i][j] = elementValue;
            }
        }

        /// <summary>Constructs a matrix from a jagged 2-D array, directly using the provided array as 
        /// internal data structure.</summary>
        /// <param name="A">Two-dimensional jagged array of doubles.</param>
        /// <exception cref="System.ArgumentException">All rows must have the same length.</exception>
        /// <seealso cref="Matrix.Create(double[][])"/>
        /// <seealso cref="Matrix.Create(double[,])"/>
        public Matrix(double[][] A)
        {
            if (A == null)
                throw new ArgumentNullException("Jagged array to create a matrix from is not specified (null reference).");
            if (A[0]==null)
                throw new ArgumentException("Can not determine number of columns of a matrix, the first row is null.");
            int numRows = _rowCount = A.Length;
            int numColumns = _columnCount = A[0].Length;
            if (numRows == 0)
                throw new ArgumentException("Jagged array to create a matrix from has 0 rows.");
            if (numColumns == 0)
                throw new ArgumentException("Invalid jagged array to create a matrix from: The first row has 0 elements.");
            _elements = new double[numRows][];
            for (int i = 0; i < numRows; ++i) 
            {
                if (A[i].Length != numColumns)
                    throw new ArgumentException("Invalid jagged array to create a matrix from: not all rows are of the same length; caused by row No. "
                        + i.ToString());
                _elements[i] = new double[numColumns];
                for (int j = 0; j < numColumns; ++j)
                    _elements[i][j] = A[i][j];
            }
        }

        /// <summary>Constructs a matrix from a 2-D array by deep-copying the provided array 
        /// to the internal data structure.</summary>
        /// <param name="elementTable">Two-dimensional array of doubles.</param>
        public Matrix(double[,] elementTable)
        {
            if (elementTable == null)
                throw new ArgumentNullException("Array to create a matrix from is not specified (null reference).");
            if (elementTable.GetLength(0) == 0)
                throw new ArgumentException("Array to create a matrix from has 0 rows.");
            if (elementTable.GetLength(1) == 0)
                throw new ArgumentException("Array to create a matrix from has 0 columns.");
            // Copy the array to a jagged array:
            int numRows = _rowCount = elementTable.GetLength(0);
            int numColumns = elementTable.GetLength(1);
            _elements = new double[numRows][];
            for (int i = 0; i < numRows; ++i)
            {
                _elements[i] = new double[numColumns];
                for (int j = 0; j < numColumns; ++j)
                    _elements[i][j] = elementTable[i, j];
            }
        }

        /// <summary>Construct a matrix from a one-dimensional packed array.</summary>
        /// <param name="_matrixElements">One-dimensional array of doubles, packed by columns (ala Fortran).</param>
        /// <param name="numRows">Number of rows.</param>
        /// <param name="numColumns">Number of columns. If 0 lthen it is calculated from numberr of rows and length of
        /// the element array.</param>
        /// <exception cref="System.ArgumentException">Array length must be a multiple of numrows.</exception>
        public Matrix(IList<double> _matrixElements, int numRows, int numColumns = 0)
        {
            if (_matrixElements == null)
                throw new ArgumentNullException("Table of matrix elements not specified.");
            else
            {
                int numEl = _matrixElements.Count;
                if (numEl > 0) 
                {
                    if (numEl % numRows != 0)
                        throw new Exception("Inconsistent matrix data: number of elements is " + numEl
                            + "while number of rows is " + numRows + "(non-zero remainder).");
                    _elements = new double[numRows][];
                    _rowCount = numRows;
                    if (numColumns == 0)
                        numColumns = _columnCount = (numEl / numRows);
                    int whichElement = 0;
                    for (int i = 0; i < numRows; ++i)
                    {
                        _elements[i] = new double[numColumns];
                        for (int j = 0; j < numColumns; ++j)
                        {
                            _elements[i][j] = _matrixElements[whichElement];
                            ++whichElement;
                        }
                    }
                }
                else if (numEl < 0)
                    throw new ArgumentException("Number fo elements of a matrix can not be less than 0.");
                else if (numEl == 0)
                {
                    _rowCount = _columnCount = 0;
                    _elements = null;
                }
            }
        }

        /// <summary>Construct a matrix from a one-dimensional packed array.</summary>
        /// <param name="_matrixElements">One-dimensional array of doubles, packed by columns (ala Fortran).</param>
        /// <param name="numRows">Number of rows.</param>
        /// <param name="numColumns">Number of columns. If 0 lthen it is calculated from numberr of rows and length of
        /// the element array.</param>
        /// <exception cref="System.ArgumentException">Array length must be a multiple of numrows.</exception>
        [Obsolete("This method may be unsupported in future versions.")]
        public Matrix(double[] _matrixElements, int numRows, int numColumns = 0)
        {
            if (_matrixElements == null)
                throw new ArgumentNullException("Table of matrix elements not specified.");
            else
            {
                int numEl = _matrixElements.Length;
                if (numEl > 0)
                {
                    if (numEl % numRows != 0)
                        throw new Exception("Inconsistent matrix data: number of elements is " + numEl
                            + "while number of rows is " + numRows + "(non-zero remainder).");
                    _elements = new double[numRows][];
                    _rowCount = numRows;
                    if (numColumns == 0)
                        numColumns = _columnCount = (numEl / numRows);
                    int whichElement = 0;
                    for (int i = 0; i < numRows; ++i)
                    {
                        _elements[i] = new double[numColumns];
                        for (int j = 0; j < numColumns; ++j)
                        {
                            _elements[i][j] = _matrixElements[whichElement];
                            ++whichElement;
                        }
                    }
                }
                else if (numEl < 0)
                    throw new ArgumentException("Number fo elements of a matrix can not be less than 0.");
                else if (numEl == 0)
                {
                    _rowCount = _columnCount = 0;
                    _elements = null;
                }
            }
        }


        #region StaticConstruction



        /// <summary>Constructs a matrix from a copy of a 2-D array by deep-copy.</summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        public static Matrix Create(double[][] A)
        {
            return new Matrix(A);
        }

        /// <summary>Constructs a matrix from a copy of a 2-D array by deep-copy.</summary>
        /// <param name="A">Two-dimensional array of doubles.</param>
        public static Matrix Create(double[,] A)
        {
            return new Matrix(A);
        }

        /// <summary>Construct a complex matrix from a set of real column vectors.</summary>
        public static Matrix CreateFromColumns(IList<Vector> columnVectors)
        {
            if (columnVectors == null)
                throw new ArgumentNullException("List of column vectors to create a matrix from is not specified (null reference).");
            if (columnVectors.Count == 0)
                throw new ArgumentException("List of column vectors to create a matrix from does not contain any vectors.");
            int rows = columnVectors[0].Length;
            int columns = columnVectors.Count;
            double[][] newData = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                double[] newRow = new double[columns];
                for (int j = 0; j < columns; j++)
                {
                    newRow[j] = columnVectors[j][i];
                }

                newData[i] = newRow;
            }
            return new Matrix(newData);
        }

        /// <summary>Construct a complex matrix from a set of real row vectors.</summary>
        public static Matrix CreateFromRows(IList<Vector> rowVectors)
        {
            if (rowVectors == null)
                throw new ArgumentNullException("List of row vectors to create a matrix from is not specified (null reference).");
            if (rowVectors.Count == 0)
                throw new ArgumentException("List of row vectors to create a matrix from does not contain any vectors.");
            int rows = rowVectors.Count;
            int columns = rowVectors[0].Length;
            double[][] newData = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                newData[i] = rowVectors[i].ToArray();
            }
            return new Matrix(newData);
        }


        // TODO: add a couple of other creations from lists of doubles, from submatirices, etc.!



        /// <summary>Creates a d1*d2 identity matrix.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        /// <returns>An d1*d2 matrix with ones on the diagonal and zeros elsewhere.</returns>
        public static Matrix Identity(int d1, int d2)
        {
            double[][] data = new double[d1][];
            for (int i = 0; i < d1; i++)
            {
                double[] col = new double[d2];
                if (i < d2)
                {
                    col[i] = 1.0;
                }

                data[i] = col;
            }
            return new Matrix(data);
        }

        /// <summary>Creates a square identity matrix of dimension d*d.</summary>
        /// <param name="d">Matrix dimension.</param>
        /// <returns>A d*d identity matrix.</returns>
        public static Matrix Identity(int d)
        {
            return Matrix.Identity(d, d);
        }

        /// <summary>Creates a d1*d2 matrix filled with 0.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        public static Matrix Zeros(int d1, int d2)
        {
            return new Matrix(d1, d2,0.0);
        }

        /// <summary>creates a square d*d matrix filled with 0.</summary>
        /// <param name="d">Number of rows and columns.</param>
        public static Matrix Zeros(int d)
        {
            return new Matrix(d, d,0.0);
        }

        /// <summary>Creates a d1*d2 matrix filled with 1.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        public static Matrix Ones(int d1, int d2)
        {
            return new Matrix(d1, d2,1.0);
        }

        /// <summary>Generates a square d*d matrix filled with 1.</summary>
        /// <param name="d">Number of rows and columns.</param>
        public static Matrix Ones(int d)
        {
            return new Matrix(d, d, 1.0);
        }

        /// <summary>Creates a new diagonal d1*d2 matrix based on the diagonal vector.</summary>
        /// <param name="diagonalVector">The values of the matrix diagonal.</param>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        /// <returns>A d1*d2 matrix with the values from the diagonal vector on the diagonal and zeros elsewhere.</returns>
        public static Matrix Diagonal(IVector<double> diagonalVector, int d1, int d2)
        {
            if (diagonalVector == null)
                throw new ArgumentNullException("Vector to create a diagonal matrix from is not specified (nul reference).");
            if (diagonalVector.Length==0)
                throw new ArgumentException("Vector to create a diagonal matrix from has 0 elements.");
            if (d1 < diagonalVector.Length || d2 < diagonalVector.Length)
                throw new ArgumentException("Vector to create a diagonal matrix from is too large.");
            if (d1>diagonalVector.Length && d2>diagonalVector.Length)
                throw new ArgumentException("Vector to create a diagonal matrix from is too small.");
            double[][] data = new double[d1][];
            for (int i = 0; i < d1; i++)
            {
                double[] col = new double[d2];
                if ((i < d2) && (i < diagonalVector.Length))
                {
                    col[i] = diagonalVector[i];
                }
                data[i] = col;
            }
            return new Matrix(data);
        }

        /// <summary>Creates a new square diagonal matrix based on the diagonal vector.</summary>
        /// <param name="diagonalVector">The values of the matrix diagonal.</param>
        /// <returns>A square matrix with the values from the diagonal vector on the diagonal and zeros elsewhere.</returns>
        public static Matrix Diagonal(IVector<double> diagonalVector)
        {
            return Matrix.Diagonal(diagonalVector, diagonalVector.Length, diagonalVector.Length);
        }


        ///// <summary>Creates a d1*d2 matrix with random elements.</summary>
        ///// <param name="d1">Number of rows.</param>
        ///// <param name="d2">Number of columns.</param>
        ///// <param name="randomDistribution">Continuous Random Distribution or Source.</param>
        ///// <returns>An d1*d2 matrix with elements distributed according to the provided distribution.</returns>
        //public static Matrix Random(int d1, int d2, IContinuousGenerator randomDistribution)
        //{
        //    double[][] data = new double[d1][];
        //    for (int i = 0; i < d1; i++)
        //    {
        //        double[] col = new double[d2];
        //        for (int j = 0; j < d2; j++)
        //        {
        //            col[j] = randomDistribution.NextDouble();
        //        }

        //        data[i] = col;
        //    }
        //    return new Matrix(data);
        //}

        ///// <summary>Creates a d*d square matrix with random elements.</summary>
        ///// <param name="d">Number of rows and columns.</param>
        ///// <param name="randomDistribution">Continuous Random Distribution or Source.</param>
        ///// <returns>An d*d matrix with elements distributed according to the provided distribution.</returns>
        //public static Matrix Random(int d, IContinuousGenerator randomDistribution)
        //{
        //    return Random(d, d, randomDistribution);
        //}

        /// <summary>Generates a d1*d2 matrix with uniformly distributed random elements.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        /// <returns>A d1*d2 matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
        public static Matrix Random(int d1, int d2)
        {
            Matrix ret = new Matrix(d1, d2);
            Matrix.SetRandom(ret);
            return ret;
        }

        /// <summary>Creates and returns a d1*d2 matrix with uniformly distributed random elements.</summary>
        /// <param name="d1">Number of rows.</param>
        /// <param name="d2">Number of columns.</param>
        /// <param name="rnd">Random number generator used for generation of element values.</param>
        /// <returns>A d1*d2 matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
        public static Matrix Random(int d1, int d2, IRandomGenerator rnd)
        {
            Matrix ret = new Matrix(d1, d2);
            Matrix.SetRandom(ret, rnd);
            return ret;
        }

        /// <summary>Generates a d*d square matrix with standard-distributed random elements.</summary>
        /// <param name="d">Number of rows and columns.</param>
        /// <returns>A d*d square matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
        public static Matrix Random(int d)
        {
            return Random(d, d);
        }

        #endregion StaticConstruction


        #endregion  // Construction


        #region Data


        /// <summary>Matrix elements.</summary>
        protected double[][] _elements;

        /// <summary>Number of rows.</summary>
        protected int _rowCount;

        /// <summary>Number of columns.</summary>
        protected int _columnCount;

        /// <summary>Gets the first dimension (number of rows) of the matrix.</summary>
        public override int RowCount
        {
            get { return _rowCount; }
        }

        /// <summary>Gets the second dimension (number of columns) of the matrix.</summary>
        public override int ColumnCount
        {
            get { return _columnCount; }
        }

        /// <summary>Sets the first dimension (number of rows) of the matrix.
        /// This setter must be used very restrictively - only in setters that can change matrix dimensions.
        /// Setter is defined separately from getter because the base class' property does not define
        /// a getter.</summary>
        protected virtual int RowCountSetter { set { _rowCount = value; } }

        /// <summary>Sets the first dimension (number of rows) of the matrix.
        /// This setter must be used very restrictively - only in setters that can change matrix dimensions.
        /// Setter is defined separately from getter because the base class' property does not define
        /// a getter.</summary>
        protected virtual int ColumnCountSetter { set { _columnCount = value; } }


        /// <summary>Gets or set the element indexed by <c>(i, j)</c> in the <c>Matrix</c>.</summary>
        /// <param name="i">Row index.</param>
        /// <param name="j">Column index.</param>
        /// <remarks>Component access is currently a bit slower because it works indirectly through the
        /// base matrix' access. This could be corrected if we could assign the components array
        /// _base._data to some internal variable, but there is no way to access the component array
        /// of a Math.Net matrix due to its protection level (which is currently default, i.e. private,
        /// but should be at least protected in order to solve this problem).</remarks>
        public override double this[int i, int j]
        { get { return _elements[i][j]; } set { _elements[i][j] = value; } }


        //$$ 
        //#region MathNetSupport

        //protected Matrix_MathNet _copyMathNet = null;

        //protected bool _mathNetConsistent = false;

        ///// <summary>Tells whether the internal MathNet matrix representation of the current matrix is 
        ///// consistent with the current matrix. The MathNet representation is used for operations that
        ///// are used from that library such as different kinds of decompositions.</summary>
        ///// <remarks>Currrently, an internal flag indicating consistency of the MathNet matrix is not used.
        ///// Every time this property is required, the consistence is actually verified by comparing values.
        ///// There may be derived matrix classes where the falg is actually used. These must keep track
        ///// when anything in the matrix changes and invalidate the flag on each such event.</remarks>
        //[Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        //protected internal virtual bool IsCopyMathNetConsistent
        //{
        //    get
        //    {
        //        if (_copyMathNet == null)
        //            return false;
        //        else if (_copyMathNet.RowCount != _rowCount || _copyMathNet.ColumnCount != ColumnCount)
        //            return false;
        //        else
        //        {
        //            for (int i = 0; i < _rowCount; ++i)
        //                for (int j = 0; j < _columnCount; ++j)
        //                {
        //                    if (_copyMathNet[i, j] != _elements[i][j])
        //                        return false;
        //                }
        //        }
        //        return true;
        //    }
        //    protected set
        //    {
        //        _mathNetConsistent = value;
        //    }
        //}

        ///// <summary>Copies components from a specified Math.Net matrix.</summary>
        ///// <param name="A">Matrix from which elements are copied.</param>
        //[Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        //protected void CopyFromMatNetMatrix(Matrix_MathNet A)
        //{
        //    if (A == null)
        //        throw new ArgumentNullException("Matrix to copy components from is not specified (null reference).");
        //    if (A.RowCount == 0)
        //        throw new ArgumentException("Matrix to copy components from has 0 rows.");
        //    if (A.ColumnCount == 0)
        //        throw new ArgumentException("Matrix to copy components from has 0 columns.");
        //    // Copy the array to a jagged array:
        //    int numRows = _rowCount = A.RowCount;
        //    int numColumns = _columnCount = A.ColumnCount;
        //    _elements = new double[numRows][];
        //    for (int i = 0; i < numRows; ++i)
        //    {
        //        _elements[i] = new double[numColumns];
        //        for (int j = 0; j < numColumns; ++j)
        //            _elements[i][j] = A[i, j];
        //    }
        //}

        ///// <summary>Gets the internal MathNet representation of the current matrix.
        ///// Representation is created on demand. However, the same copy is returned
        ///// as long as it is consistent with the current matrix.
        ///// Use GetCopyMathNet() to create a new copy each time.</summary>
        //[Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        //public virtual Matrix_MathNet CopyMathNet
        //{
        //    get
        //    {
        //        if (!IsCopyMathNetConsistent)
        //        {
        //            _copyMathNet = new Matrix_MathNet(_rowCount, _columnCount);
        //            for (int i = 0; i < _rowCount; ++i)
        //                for (int j = 0; j < _columnCount; ++j)
        //                    _copyMathNet[i, j] = this[i, j];
        //        }
        //        return _copyMathNet;
        //    }
        //}

        ///// <summary>Creates and returns a newly allocated MathNet representation of the current matrix.</summary>
        //[Obsolete("Use MathNet.Numerics library instead of MathNet!")]
        //protected internal Matrix_MathNet GetCopyMathNet()
        //{
        //    int numRows = this.RowCount;
        //    int numColumns = this.ColumnCount;
        //    Matrix_MathNet ReturnedString = new Matrix_MathNet(numRows, numColumns);
        //    for (int i=0; i<numRows; ++i)
        //        for (int j = 0; j < numColumns; ++j)
        //        {
        //            ReturnedString[i,j] = this[i, j];
        //        }
        //    return ReturnedString;
        //}

        //#endregion MathNetSupport


        #region MathNetNumerics

        /// <summary>Copy of the current matrix as Math.Net numerics matrix.</summary>
        protected Matrix_MathNetNumerics _copyMathNetNumerics = null;

        /// <summary>Whetherr the Math.Net Matrix copy is consistent.</summary>
        protected bool _mathNetNumericsConsistent = false;

        /// <summary>Tells whether the internal MathNet.Numerics matrix representation of the current matrix is 
        /// consistent with the current matrix. The MathNet.Numerics representation is used for operations that
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
                else if (_copyMathNetNumerics.RowCount != _rowCount || _copyMathNetNumerics.ColumnCount != ColumnCount)
                    return false;
                else
                {
                    for (int i = 0; i < _rowCount; ++i)
                        for (int j = 0; j < _columnCount; ++j)
                        {
                            if (_copyMathNetNumerics[i, j] != _elements[i][j])
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

        /// <summary>Copies components from a specified MathNet.Numerics matrix.</summary>
        /// <param name="A">Matrix from which elements are copied.</param>
        protected void CopyFromMatNetNumericsMatrix(Matrix_MathNetNumerics A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to copy components from is not specified (null reference).");
            if (A.RowCount == 0)
                throw new ArgumentException("Matrix to copy components from has 0 rows.");
            if (A.ColumnCount == 0)
                throw new ArgumentException("Matrix to copy components from has 0 columns.");
            // Copy the array to a jagged array:
            int numRows = _rowCount = A.RowCount;
            int numColumns = _columnCount = A.ColumnCount;
            _elements = new double[numRows][];
            for (int i = 0; i < numRows; ++i)
            {
                _elements[i] = new double[numColumns];
                for (int j = 0; j < numColumns; ++j)
                    _elements[i][j] = A[i, j];
            }
        }

        /// <summary>Gets the internal MathNet.Numerics representation of the current matrix.
        /// Representation is created on demand. However, the same copy is returned
        /// as long as it is consistent with the current matrix.
        /// Use GetCopyMathNet() to create a new copy each time.</summary>
        public virtual Matrix_MathNetNumerics CopyMathNetNumerics
        {
            get
            {
                if (!IsCopyMathNetNumericsConsistent)
                {
                    _copyMathNetNumerics = new Matrix_MathNetNumerics(_rowCount, _columnCount);
                    for (int i = 0; i < _rowCount; ++i)
                        for (int j = 0; j < _columnCount; ++j)
                            _copyMathNetNumerics[i, j] = this[i, j];
                }
                return _copyMathNetNumerics;
            }
        }

        /// <summary>Creates and returns a newly allocated MathNet.Numerics representation of the current matrix.</summary>
        public Matrix_MathNetNumerics GetCopyMathNetNumerics()
        {
            int numRows = this.RowCount;
            int numColumns = this.ColumnCount;
            Matrix_MathNetNumerics ret = new Matrix_MathNetNumerics(numRows, numColumns);
            for (int i=0; i<numRows; ++i)
                for (int j = 0; j < numColumns; ++j)
                {
                    ret[i,j] = this[i, j];
                }
            return ret;
        }

        #endregion MathNetNumerics



        /// <summary>Creates and returns a copy of the current matrix.</summary>
        /// <returns>A new copy of the current matrix. 
        /// The copy is supposed to be of the same type as the current matrix.</returns>
        public Matrix GetCopyThis()
        {
            int numRows = this.RowCount;
            int numColumns = this.ColumnCount;
            Matrix ret = new Matrix(numRows, numColumns);
            for (int i = 0; i < numRows; ++i)
                for (int j = 0; j < numColumns; ++j)
                    ret[i, j] = this[i, j];
            return ret;
        }

        /// <summary>Creates and returns a copy of the current matrix.</summary>
        /// <returns>A new copy of the current matrix. 
        /// The copy is supposed to be of the same type as the current matrix.</returns>
        public override MatrixBase GetCopyBase()
        {
            return GetCopyThis();
        }


        /// <summary>Creates and returns a new matrix with the specified dimensions, and of the same type as 
        /// the current matrix.</summary>
        /// <param name="rowCount">Number fo rows of the newly created matrix.</param>
        /// <param name="columnCount">Number of columns of the newly created matrix.</param>
        /// <returns>A newly created matrix of the specified dimensions and of the same type as the current matrix.</returns>
        public virtual Matrix GetNewThis(int rowCount, int columnCount)
        {
            return new Matrix(rowCount, columnCount);
        }

        /// <summary>Creates and returns a new matrix with the specified dimensions, and of the same type as 
        /// the current matrix.</summary>
        /// <param name="rowCount">Number fo rows of the newly created matrix.</param>
        /// <param name="columnCount">Number of columns of the newly created matrix.</param>
        /// <returns>A newly created matrix of the specified dimensions and of the same type as the current matrix.</returns>
        public override MatrixBase GetNewBase(int rowCount, int columnCount)
        {
            return new Matrix(rowCount, columnCount);
        }

        /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
        /// the current matrix.</summary>
        public Matrix GetNewThis()
        {
            return new Matrix(this.RowCount, this.ColumnCount);
        }

        /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
        /// the current matrix.</summary>
        public override MatrixBase GetNewBase()
        {
            return new Matrix(this.RowCount, this.ColumnCount);
        }


        /// <summary>Creates and returns a new vector with the specified dimension, 
        /// and of the type that is consistent with the type of the current matrix.</summary>
        /// <param name="length">Dimension of the newly created vector.</param>
        /// <returns>A newly created vector of the specified dimension and of the same type as the current matrix.</returns>
        public Vector GetNewVectorThis(int length)
        {
            return new Vector(length);
        }


        /// <summary>Creates and returns a new vector with the specified dimension, 
        /// and of the type that is consistent with the type of the current matrix.</summary>
        /// <param name="length">Dimension of the newly created vector.</param>
        /// <returns>A newly created vector of the specified dimension and of the same type as the current matrix.</returns>
        public override VectorBase GetNewVectorBase(int length)
        {
            return GetNewVectorThis(length);
        }

        #endregion Data


        #region Conversions

        // TODO: implement this region!

        #endregion  // Conversions


        #region Submatrix_Operations

        // TODO: implement this region!


        #endregion  // Submatrix_Operations


        #region Helpers_Infrastructure

        // TODO: implement this region!


        #endregion  Helpers_Infrastructure


        #region Operations

        /// <summary>Copies components of a matrix to another matrix.
        /// WARNING: dimensions of the specified result matrix must match dimensions of the original matrix.
        /// REMARK: This method is implemented because it is more efficient than the corresponding
        /// method in MatrixBase (due to matched types).</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Materix where copy will be stored. Dimensions must match dimensions of a.</param>
        public static void Copy(Matrix a, Matrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix to be copied is not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result matrix for copy operation is not specified (null reference).");
            else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                throw new ArgumentException("Dimension mismatch at copying matrices.");
            else
            {
                for (int i = 0; i < a.RowCount; ++i)
                    for (int j = 0; j < a.ColumnCount; ++j)
                        result[i, j] = a[i, j];
            }
        }

        /// <summary>Copies components of a matrix to another matrix.
        /// Resulting matrix is allocated or reallocated if necessary.
        /// REMARK: This method is implemented because it is more efficient than the corresponding
        /// method in MatrixBase (due to matched types).</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Materix where copy will be stored. Dimensions must match dimensions of a.</param>
        public static void Copy(Matrix a, ref Matrix result)
        {
            if (a == null)
                result = null;
            else
            {
                if (result == null)
                    result = a.GetNewThis(a.RowCount, a.ColumnCount);
                else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                    result = a.GetNewThis(a.RowCount, a.ColumnCount);
                for (int i = 0; i < a.RowCount; ++i)
                    for (int j = 0; j < a.ColumnCount; ++j)
                        result[i,j] = a[i, j];
            }
        }


        /// <summary>Returns a new matrix that is transpose of the current matrix.
        /// Re-implemented here for efficiency (hides the base class property).</summary>
        public new Matrix T
        {
            get
            {
                Matrix ret = GetNewThis(ColumnCount, RowCount);
                Transpose(this, ret);
                return ret;
            }
        }



        #region Products

        ///// <summary>Matrix multiplication by a scalar.</summary>
        ///// <param name="m1">Matrix that is multiplied.</param>
        ///// <param name="s2">Scalar with which matrix is multiplied.</param>
        ///// <param name="result">Matrix where result is stored.</param>
        //public static void Multiply(Matrix m1, double s2, ref Matrix result)
        //{
        //    if (m1 == null)
        //        throw new ArgumentNullException("The first operand is not specified (null reference).");
        //    if (m1 != result)
        //        Copy(m1, ref result);
        //    result.CopyMathNetNumerics.Multiply(s2);  // perform in-place multiplication
        //}


        //$$
        /// <summary>Kronecker product, A(m*n)*B(p*q)=C(mp*nq)
        /// Result is a block matrix with as many blocks as there are elements of the first matrix,
        /// each block being a product of the second matrix by an element of the first matrix.
        /// Both operands must be defined (non-null).</summary>
        /// <param name="m1">First operand.</param>
        /// <param name="m2">Second operand.</param>
        /// <param name="result"></param>
        public static void KroneckerProduct(Matrix m1, Matrix m2, Matrix result)
        {
            if (m1 == null)
                throw new ArgumentNullException("The first operand is not specified (null reference).");
            if (m2 == null)
                throw new ArgumentNullException("The second operand is not specified (null reference).");
            if (m1.RowCount <= 0 || m1.ColumnCount<=0 || m2.RowCount<=0 || m2.ColumnCount<=0)
                throw new ArgumentException("Inconsistent dimensions of operands.");
            if (m1 == result)
                throw new ArgumentException("Result matrix is the same as the first operand, this is not allowed in matrix multiplication.");
            if (m2 == result)
                throw new ArgumentException("Result matrix is the same as the seconf operand, this is not allowed in matrix multiplication.");

            throw new NotImplementedException("Kronecker product for matrices is not implemented yet.");

            //if (result == null)
            //    result = new Matrix();
            //result.CopyFromMatNetMatrix(Matrix_MathNet.KroneckerProduct(m1.CopyMathNet, m2.CopyMathNet));
        }


        /// <summary>Kronecker or tensor product, A(m*n)*B(p*q)=C(mp*nq)
        /// Result is a block matrix with as many blocks as there are elements of the first matrix,
        /// each block being a product of the second matrix by an element of the first matrix.
        /// Both operands must be defined (non-null).</summary>
        /// <param name="m1">First operand.</param>
        /// <param name="m2">Second operand.</param>
        /// <param name="result"></param>
        public static void KroneckerMultiply(Matrix m1, Matrix m2, Matrix result)
        {
            KroneckerProduct(m1, m2, result);
        }

        /// <summary>Kronecker or tensor product, A(m*n)*B(p*q)=C(mp*nq)
        /// Result is a block matrix with as many blocks as there are elements of the first matrix,
        /// each block being a product of the second matrix by an element of the first matrix.
        /// Both operands must be defined (non-null).</summary>
        /// <param name="m1">First operand.</param>
        /// <param name="m2">Second operand.</param>
        /// <param name="result"></param>
        public static void TensorMultiply(Matrix m1, Matrix m2, Matrix result)
        {
            KroneckerMultiply(m1, m2, result);
        }



        #endregion  // Products


        #region Norms


        ///// <summary>Gets the Forbenius norm of the matrix, the square root of sum of squares of all elements.</summary>
        //public override double NormForbenius
        //{
        //    get
        //    {
        //        if (CopyMathNetNumerics == null)
        //            return 0.0;
        //        else
        //        {
        //            //$$
        //            throw new NotImplementedException("Forbenius norm is not implemented.");
        //            //return CopyMathNetNumerics.NormF();
        //        }
        //    }
        //}

        /// <summary>Gets the Forbenius norm of the matrix, the square root of sum of squares of all elements.</summary>
        [Obsolete("Use NormForbenius instead!")]
        public override double NormEuclidean
        {
            get
            {
                return NormForbenius;
            }
        }

        /// <summary>Gets the Forbenius norm of the matrix, the square root of sum of squares of all elements.</summary>
        public override double Norm
        {
            get
            {
                return NormForbenius;
            }
        }

        /// <summary>Gets the one norm of the matrix, the maximum column sum of absolute values.</summary>
        public virtual double Norm1
        {
            get
            {
                if (CopyMathNetNumerics == null)
                    return 0.0;
                else
                {
                    //$$
                    throw new NotImplementedException("Norm 1 is not implemented.");
                    //return CopyMathNetNumerics.Norm1;
                }
            }
        }

        /// <summary>Gets the two norm of the matrix, i.e. its maximal singular value.</summary>
        public virtual double Norm2
        {
            get
            {
                if (CopyMathNetNumerics == null)
                    return 0.0;
                else
                {
                    //$$
                    throw new NotImplementedException("Norm2 is not implemented.");
                    //return CopyMathNetNumerics.Norm2();
                }
            }
        }

        /// <summary>Gets the infinity norm of the matrix, the maximum row sum of absolute values.</summary>
        public virtual double NormInf
        {
            get
            {
                if (CopyMathNetNumerics == null)
                    return 0.0;
                else
                {
                    //$$
                    throw new NotImplementedException("NormInf is not implemented.");
                    //return CopyMathNetNumerics.NormInf();
                }
            }
        }


        #endregion Norms



        #endregion  // Operations


        #region Decompositions

        // TODO: implement this region!


        #endregion  // Decompositions


        #region Linear_Algebra

        // TODO: implement this region!  (inverse, determinant, condition number, rank, etc.)


        #endregion  // Linear_Algebra


        //#region Operators_Overloading


        ///// <summary>Unary plus, returns the operand.</summary>
        //public static Matrix operator +(Matrix m)
        //{
        //    if (m == null)
        //        return null;
        //    else
        //        return m.GetCopyThis();
        //}

        ///// <summary>Unary negation, returns the negative operand.</summary>
        //public static Matrix operator -(Matrix v)
        //{
        //    Matrix ReturnedString = v.GetCopyThis();
        //    Matrix.Negate(v, ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Matrix addition.</summary>
        //public static Matrix operator +(Matrix a, Matrix b)
        //{
        //    Matrix ReturnedString = a.GetCopyThis();
        //    Matrix.Add(a, b, ReturnedString);
        //    return ReturnedString;
        //}

        /////// <summary>Addition of a scalar to each component of a matrix.</summary>
        ////public static Matrix operator +(Matrix a, double s)
        ////{
        ////    Matrix ReturnedString = null;
        ////    Matrix.Add(a, s, ref ReturnedString);
        ////    return ReturnedString;
        ////}

        /////// <summary>Addition of a scalar to each component of a matrix.</summary>
        ////public static Matrix operator +(double a, Matrix s)
        ////{
        ////    Matrix ReturnedString = null;
        ////    Matrix.Add(s, a, ref ReturnedString);
        ////    return ReturnedString;
        ////}


        ///// <summary>Matrix subtraction.</summary>
        //public static Matrix operator -(Matrix a, Matrix b)
        //{
        //    Matrix ReturnedString = a.GetCopyThis();
        //    Matrix.Subtract(a, b, ReturnedString);
        //    return ReturnedString;
        //}

        /////// <summary>Subtraction of scalar from each component of a matrix.</summary>
        ////public static Matrix operator -(Matrix a, double s)
        ////{
        ////    Matrix ReturnedString = null;
        ////    Matrix.Subtract(a, s, ref ReturnedString);
        ////    return ReturnedString;
        ////}


        ///// <summary>Product of two matrices.</summary>
        //public static IMatrix operator *(Matrix a, Matrix b)
        //{
        //    IMatrix ReturnedString = null;
        //    Multiply(a, b, ref ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Product of a matrix and a vector.</summary>
        //public static IVector operator *(Matrix a, Vector b)
        //{
        //    IVector ReturnedString = null;
        //    Multiply(a, b, ref ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Product of a matrix by a scalar.</summary>
        //public static IMatrix operator *(Matrix a, double b)
        //{
        //    Matrix ReturnedString = null;
        //    Matrix.Multiply(a, b, ref ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Product of a matrix by a scalar.</summary>
        //public static IMatrix operator *(double a, Matrix b)
        //{
        //    Matrix ReturnedString = null;
        //    Matrix.Multiply(b, a, ref ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Matrix subtraction.</summary>
        //public static IMatrix operator /(Matrix a, double b)
        //{
        //    IMatrix ReturnedString = null;
        //    Matrix.Divide(a, b, ref ReturnedString);
        //    return ReturnedString;
        //}



        //#endregion  // Operators_Overloading



        #region Formatting_Parsing


        /// <summary>Returns a string representation of this vector in a readable form.</summary>
        public virtual string ToStringBase()
        {
            if (CopyMathNetNumerics == null)
                return "[[]]";
            else
            {
                return CopyMathNetNumerics.ToString();
            }
        }


        #endregion  // Formatting_Parsing


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
        /// <remarks>This method calls the <see cref="MatrixBase.Equals(Object)"/> to obtain the returned value, which is
        /// standard for all implementations of the <see cref="IMatrix"/> interface.
        /// <para>Overrides the <see cref="object.Equals(Object)"/> method.</para></remarks>
        public override bool Equals(Object obj)
        {
            return MatrixBase.Equals(this, obj as IMatrix);
        }

        #endregion Auxiliary


        /// <summary>A test method, just prints some output.</summary>
        public static new void TestStaticMethodSpecific()
        {
            Console.WriteLine("TestStaticMethod from the Matrix class.");
        }


    }  // class Matrix





    // REMARKS:
    // The class that is commented below is a test Matrix class optimized for use of the MathNet
    // internal representation of a matrix (in the way that the representation is obtained on 
    // demand and querying whether internally stored representation is up to date is very quick).
    // This may be useful inprinciple when a lot of operations from the Math.Net library is performed,
    // but probably we will not need a special class for that. It should be sufficient to provide 
    // methods on the Matrix class (may be static) that create a MathNet copy of the matrix.

    ///// <summary>Real matrix optimized for frequent use of its internal MathNet representation.
    ///// Use is the same as that of the Matrix class, but operation is modified in such a way
    ///// that a flag always keeps track of changes that make MathNet representation inconsistent.
    ///// Therefore, establishing whether the internal MathNet representatino is inconsistend is
    ///// very fast on the account of a bit slover operations that change the matrix state (e.g. 
    ///// setting components).</summary>
    //[Obsolete("Not in use yet.")]
    //public class MatrixMathNet : Matrix
    //{

    //    #region Data


    //    /// <summary>Tells whether the internal MathNet matrix representation of the current matrix is 
    //    /// consistent with the current matrix. The MathNet representation is used for operations that
    //    /// are used from that library such as different kinds of decompositions.</summary>
    //    /// <remarks>This property is optimized because it just returns or sets the value of an
    //    /// internal boolean variable. On account of this, setting components is a bit slower because
    //    /// it must set that flag to false.</remarks>
    //    public override bool MathNetBaseConsistent
    //    {
    //        get
    //        {
    //            return _mathNetConsistent;
    //        }
    //        protected set
    //        {
    //            _mathNetConsistent = value;
    //        }
    //    }

    //    ///// <summary>Copies components from a specified Math.Net matrix.</summary>
    //    ///// <param name="A">Matrix from which elements are copied.</param>
    //    //protected void CopyFromMatNetMatrix(MatNetMatrix A)
    //    //{
    //    //    if (A == null)
    //    //        throw new ArgumentNullException("Matrix to copy new matrix components from is not specified (null reference).");
    //    //    if (A.RowCount == 0)
    //    //        throw new ArgumentException("Matrix to create a new matrix from has 0 rows.");
    //    //    if (A.ColumnCount == 0)
    //    //        throw new ArgumentException("Matrix to create a new matrix from has 0 columns.");
    //    //    // Copy the array to a jagged array:
    //    //    int numRows = _rowCount = A.RowCount;
    //    //    int numColumns = _columnCount = A.ColumnCount;
    //    //    _elements = new double[numRows][];
    //    //    for (int i = 0; i < numRows; ++i)
    //    //    {
    //    //        _elements[i] = new double[numColumns];
    //    //        for (int j = 0; j < numColumns; ++j)
    //    //            _elements[i][j] = A[i, j];
    //    //    }
    //    //}

    //    /// <summary>Gets the internal MathNet representation of the current matrix.</summary>
    //    public virtual MatNetMatrix Base
    //    {
    //        get
    //        {
    //            if (!_mathNetConsistent)
    //            {
    //                int rowCount = RowCount;
    //                int columnCount = ColumnCount;
    //                _base = new MatNetMatrix(rowCount, columnCount);
    //                for (int i = 0; i < rowCount; ++i)
    //                    for (int j = 0; j < columnCount; ++j)
    //                        _base[i, j] = this[i, j];
    //            }
    //            return _base;
    //        }
    //    }


    //    /// <summary>Sets the first dimension (number of rows) of the matrix.
    //    /// This setter must be used very restrictively - only in setters that can change matrix dimensions.
    //    /// Setter is defined separately from getter because the base class' property does not define
    //    /// a getter.</summary>
    //    protected override int RowCountSetter { set { _mathNetConsistent = false; base.RowCountSetter = value; } }

    //    /// <summary>Sets the first dimension (number of rows) of the matrix.
    //    /// This setter must be used very restrictively - only in setters that can change matrix dimensions.
    //    /// Setter is defined separately from getter because the base class' property does not define
    //    /// a getter.</summary>
    //    protected override int ColumnCountSetter { set { _mathNetConsistent = false; base.ColumnCountSetter = value; } }


    //    /// <summary>Gets or set the element indexed by <c>(i, j)</c> in the <c>Matrix</c>.</summary>
    //    /// <param name="i">Row index.</param>
    //    /// <param name="j">Column index.</param>
    //    /// <remarks>Component access is currently a bit slower because it works indirectly through the
    //    /// base matrix' access. This could be corrected if we could assign the components array
    //    /// _base._data to some internal variable, but there is no way to access the component array
    //    /// of a Math.Net matrix due to its protection level (which is currently default, i.e. private,
    //    /// but should be at least protected in order to solve this problem).</remarks>
    //    public override double this[int i, int j]
    //    { get { return _elements[i][j]; } set { _mathNetConsistent = false; _elements[i][j] = value; } }



    //    #endregion Data



    //    #region Construction

    //    protected MatrixMathNet()
    //        : base()
    //    { }


    //    /// <summary>Constructs a matrix from another matrix by copying the provided matrix components 
    //    /// to the internal data structure.</summary>
    //    /// <param name="A">Matrix whose components are copied to the current matrix.</param>
    //    public MatrixMathNet(IMatrix A)
    //        : base(A)
    //    { }

    //    /// <summary>Construct a matrix from MathNet.Numerics.LinearAlgebra.Matrix.
    //    /// Only a reference of A is copied.</summary>
    //    /// <param name="A">MathNet.Numerics.LinearAlgebra.Matrix from which a new matrix is created.</param>
    //    public MatrixMathNet(MatNetMatrix A)
    //        : base(A)
    //    { }

    //    /// <summary>Constructs an d1*d2 - dimensional matrix of zeros.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d1">Number of columns.</param>
    //    public MatrixMathNet(int d1, int d2)
    //        : base(d1, d2)
    //    { }

    //    /// <summary> Construct an numrows-by-d2 constant matrix with specified value for all elements.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    /// <param name="value">Value of all components.</param>
    //    public MatrixMathNet(int d1, int d2, double val)
    //        : base(d1, d2, val)
    //    { }

    //    /// <summary>Constructs a square matrix with specified diagonal values.</summary>
    //    /// <param name="d">Size of the square matrix.</param>
    //    /// <param name="val">Vector of diagonal values.</param>
    //    public MatrixMathNet(IVector diagonal)
    //        : base(diagonal)
    //    { }

    //    /// <summary>Constructs a d*d square matrix with specified diagonal value.</summary>
    //    /// <param name="dim">Size of the square matrix.</param>
    //    /// <param name="elementValue">Diagonal value.</param>
    //    public MatrixMathNet(int dim, double elementValue)
    //        : base(dim, elementValue)
    //    { }

    //    /// <summary>Constructs a matrix from a jagged 2-D array, directly using the provided array as 
    //    /// internal data structure.</summary>
    //    /// <param name="A">Two-dimensional jagged array of doubles.</param>
    //    /// <exception cref="System.ArgumentException">All rows must have the same length.</exception>
    //    /// <seealso cref="Matrix.Create(double[][])"/>
    //    /// <seealso cref="Matrix.Create(double[,])"/>
    //    public MatrixMathNet(double[][] A)
    //        : base(A)
    //    { }

    //    /// <summary>Constructs a matrix from a 2-D array by deep-copying the provided array 
    //    /// to the internal data structure.</summary>
    //    /// <param name="elementTable">Two-dimensional array of doubles.</param>
    //    public MatrixMathNet(double[,] elementTable)
    //        : base(elementTable)
    //    { }

    //    /// <summary>Construct a matrix from a one-dimensional packed array.</summary>
    //    /// <param name="_matrixElements">One-dimensional array of doubles, packed by columns (ala Fortran).</param>
    //    /// <param name="numRows">Number of rows.</param>
    //    /// <exception cref="System.ArgumentException">Array length must be a multiple of numrows.</exception>
    //    [Obsolete("This method may be unsupported in future versions.")]
    //    public MatrixMathNet(double[] _matrixElements, int numRows)
    //        : base(_matrixElements, numRows)
    //    { }


    //    #region StaticConstruction

    //    /// <summary>Constructs a matrix from a copy of a 2-D array by deep-copy.</summary>
    //    /// <param name="A">Two-dimensional array of doubles.</param>
    //    public static new MatrixMathNet Create(double[][] A)
    //    {
    //        if (A == null)
    //            throw new ArgumentNullException("Jagged array to create a matrix from is not specified (null reference).");
    //        if (A.Length == 0)
    //            throw new ArgumentException("Jagged array to create a matrix from has 0 rows.");
    //        if (A[0].Length == 0)
    //            throw new ArgumentException("Jagged array to create a matrix from has 0 columns.");
    //        return new MatrixMathNet(A);
    //    }

    //    /// <summary>Constructs a matrix from a copy of a 2-D array by deep-copy.</summary>
    //    /// <param name="A">Two-dimensional array of doubles.</param>
    //    public static new MatrixMathNet Create(double[,] A)
    //    {
    //        if (A == null)
    //            throw new ArgumentNullException("Array to create a matrix from is not specified (null reference).");
    //        if (A.GetLength(0) == 0)
    //            throw new ArgumentException("Array to create a matrix from has 0 rows.");
    //        if (A.GetLength(1) == 0)
    //            throw new ArgumentException("Array to create a matrix from has 0 columns.");
    //        return new MatrixMathNet(A);
    //    }

    //    /// <summary>Construct a complex matrix from a set of real column vectors.</summary>
    //    public static new MatrixMathNet CreateFromColumns(IList<Vector> columnVectors)
    //    {
    //        if (columnVectors == null)
    //            throw new ArgumentNullException("List of column vectors to create a matrix from is not specified (null reference).");
    //        if (columnVectors.Count == 0)
    //            throw new ArgumentException("List of column vectors to create a matrix from does not contain any vectors.");
    //        int rows = columnVectors[0].Length;
    //        int columns = columnVectors.Count;
    //        double[][] newData = new double[rows][];
    //        for (int i = 0; i < rows; i++)
    //        {
    //            double[] newRow = new double[columns];
    //            for (int j = 0; j < columns; j++)
    //            {
    //                newRow[j] = columnVectors[j][i];
    //            }

    //            newData[i] = newRow;
    //        }
    //        return new MatrixMathNet(newData);
    //    }

    //    /// <summary>Construct a complex matrix from a set of real row vectors.</summary>
    //    public static new MatrixMathNet CreateFromRows(IList<Vector> rowVectors)
    //    {
    //        if (rowVectors == null)
    //            throw new ArgumentNullException("List of row vectors to create a matrix from is not specified (null reference).");
    //        if (rowVectors.Count == 0)
    //            throw new ArgumentException("List of row vectors to create a matrix from does not contain any vectors.");
    //        int rows = rowVectors.Count;
    //        int columns = rowVectors[0].Length;
    //        double[][] newData = new double[rows][];

    //        for (int i = 0; i < rows; i++)
    //        {
    //            newData[i] = rowVectors[i].ToArray();
    //        }
    //        return new MatrixMathNet(newData);
    //    }


    //    // TODO: add a couple of other creations from lists of doubles, from submatirices, etc.!
    //    // Do it in the Matrix class first!



    //    /// <summary>Creates a d1*d2 identity matrix.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    /// <returns>An d1*d2 matrix with ones on the diagonal and zeros elsewhere.</returns>
    //    public static new MatrixMathNet Identity(int d1, int d2)
    //    {
    //        double[][] data = new double[d1][];
    //        for (int i = 0; i < d1; i++)
    //        {
    //            double[] col = new double[d2];
    //            if (i < d2)
    //            {
    //                col[i] = 1.0;
    //            }

    //            data[i] = col;
    //        }
    //        return new MatrixMathNet(data);
    //    }

    //    /// <summary>Creates a square identity matrix of dimension d*d.</summary>
    //    /// <param name="d">Matrix dimension.</param>
    //    /// <returns>A d*d identity matrix.</returns>
    //    public static new MatrixMathNet Identity(int d)
    //    {
    //        return MatrixMathNet.Identity(d, d);
    //    }

    //    /// <summary>Creates a d1*d2 matrix filled with 0.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    public static new MatrixMathNet Zeros(int d1, int d2)
    //    {
    //        return new MatrixMathNet(d1, d2, 0.0);
    //    }

    //    /// <summary>creates a square d*d matrix filled with 0.</summary>
    //    /// <param name="d">Number of rows and columns.</param>
    //    public static new MatrixMathNet Zeros(int d)
    //    {
    //        return new MatrixMathNet(d, d, 0.0);
    //    }

    //    /// <summary>Creates a d1*d2 matrix filled with 1.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    public static new MatrixMathNet Ones(int d1, int d2)
    //    {
    //        return new MatrixMathNet(d1, d2, 1.0);
    //    }

    //    /// <summary>Generates a square d*d matrix filled with 1.</summary>
    //    /// <param name="d1">Number of rows and columns.</param>
    //    public static new MatrixMathNet Ones(int d)
    //    {
    //        return new MatrixMathNet(d, d, 1.0);
    //    }

    //    /// <summary>Creates a new diagonal d1*d2 matrix based on the diagonal vector.</summary>
    //    /// <param name="diagonalVector">The values of the matrix diagonal.</param>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    /// <returns>A d1*d2 matrix with the values from the diagonal vector on the diagonal and zeros elsewhere.</returns>
    //    public static new MatrixMathNet Diagonal(IVector<double> diagonalVector, int d1, int d2)
    //    {
    //        if (diagonalVector == null)
    //            throw new ArgumentNullException("Vector to create a diagonal matrix from is not specified (nul reference).");
    //        if (diagonalVector.Length == 0)
    //            throw new ArgumentException("Vector to create a diagonal matrix from has 0 elements.");
    //        if (d1 < diagonalVector.Length || d2 < diagonalVector.Length)
    //            throw new ArgumentException("Vector to create a diagonal matrix from is too large.");
    //        if (d1 > diagonalVector.Length && d2 > diagonalVector.Length)
    //            throw new ArgumentException("Vector to create a diagonal matrix from is too small.");
    //        double[][] data = new double[d1][];
    //        for (int i = 0; i < d1; i++)
    //        {
    //            double[] col = new double[d2];
    //            if ((i < d2) && (i < diagonalVector.Length))
    //            {
    //                col[i] = diagonalVector[i];
    //            }
    //            data[i] = col;
    //        }
    //        return new MatrixMathNet(data);
    //    }

    //    /// <summary>Creates a new square diagonal matrix based on the diagonal vector.</summary>
    //    /// <param name="diagonalVector">The values of the matrix diagonal.</param>
    //    /// <returns>A square matrix with the values from the diagonal vector on the diagonal and zeros elsewhere.</returns>
    //    public static new Matrix Diagonal(IVector<double> diagonalVector)
    //    {
    //        return MatrixMathNet.Diagonal(diagonalVector, diagonalVector.Length, diagonalVector.Length);
    //    }


    //    /// <summary>Creates a d1*d2 matrix with random elements.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    /// <param name="randomDistribution">Continuous Random Distribution or Source.</param>
    //    /// <returns>An d1*d2 matrix with elements distributed according to the provided distribution.</returns>
    //    public static new MatrixMathNet Random(int d1, int d2, IContinuousGenerator randomDistribution)
    //    {
    //        double[][] data = new double[d1][];
    //        for (int i = 0; i < d1; i++)
    //        {
    //            double[] col = new double[d2];
    //            for (int j = 0; j < d2; j++)
    //            {
    //                col[j] = randomDistribution.NextDouble();
    //            }

    //            data[i] = col;
    //        }
    //        return new MatrixMathNet(data);
    //    }

    //    /// <summary>Creates a d*d square matrix with random elements.</summary>
    //    /// <param name="d">Number of rows and columns.</param>
    //    /// <param name="randomDistribution">Continuous Random Distribution or Source.</param>
    //    /// <returns>An d*d matrix with elements distributed according to the provided distribution.</returns>
    //    public static new MatrixMathNet Random(int d, IContinuousGenerator randomDistribution)
    //    {
    //        return Random(d, d, randomDistribution);
    //    }

    //    /// <summary>Generates a d1*d2 matrix with standard-distributed random elements.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    /// <returns>A d1*d2 matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
    //    public static new MatrixMathNet Random(int d1, int d2)
    //    {
    //        return Random(d1, d2, new StandardDistribution());
    //    }

    //    /// <summary>Generates a d*d square matrix with standard-distributed random elements.</summary>
    //    /// <param name="d">Number of rows and columns.</param>
    //    /// <returns>A d*d square matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
    //    public static new MatrixMathNet Random(int d)
    //    {
    //        return Random(d, d);
    //    }

    //    #endregion StaticConstruction


    //    #endregion  // Construction

    //}  // class MatrixMathNet



}  // namespace IG.Num
