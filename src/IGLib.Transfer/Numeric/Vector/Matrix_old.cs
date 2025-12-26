// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

    // MATRICES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

//using MathNet.Numerics;
//using MathNet.Numerics.Distributions;
//using MathNet.Numerics.RandomSources;

//using Complex = MathNet.Numerics.Complex;

namespace IG.Old
{

    ///// <summary>Real matrix class.
    ///// Implemented as wrapper class around MathNet.Numerics.LinearAlgebra.Matrix class.
    ///// WARNING: 
    ///// This class is obsolete and should not be used any more. It is only there to support new development,
    ///// and will be removed in due time.</summary>
    ///// $A Igor Jan08 Jul10 Nov10;
    //[Serializable]
    //[Obsolete("This class is obsolete and should not be used. It is only here to assist newer developments, but will be removed in the future.")]
    //public class Matrix_old_to_delete : MatrixBase, IMatrix
    //{
    //    #region Construction

    //    protected Matrix_old_to_delete()
    //    {
    //        Base = null;
    //    }


    //    /// <summary>Constructs a matrix from another matrix by copying the provided matrix components 
    //    /// to the internal data structure.</summary>
    //    /// <param name="A">Matrix whose components are copied to the current matrix.</param>
    //    public Matrix_old_to_delete(IMatrix A)
    //    {
    //        if (A == null)
    //            throw new ArgumentNullException("Matrix to copy new matrix components from is not specified (null reference).");
    //        if (A.RowCount == 0)
    //            throw new ArgumentException("Matrix to create a new matrix from has 0 rows.");
    //        if (A.ColumnCount == 0)
    //            throw new ArgumentException("Matrix to create a new matrix from has 0 columns.");
    //        // Copy the array to a jagged array:
    //        double[][] B;
    //        B = new double[A.RowCount][];
    //        for (int i = 0; i < A.RowCount; ++i)
    //        {
    //            B[i] = new double[A.ColumnCount];
    //            for (int j = 0; j < A.ColumnCount; ++j)
    //                B[i][j] = A[i, j];
    //        }
    //        Base = new MathNet.Numerics.LinearAlgebra.Matrix(B);
    //    }

    //    /// <summary>Construct a matrix from MathNet.Numerics.LinearAlgebra.Matrix.
    //    /// Only a reference of A is copied.</summary>
    //    /// <param name="A">MathNet.Numerics.LinearAlgebra.Matrix from which a new matrix is created.</param>
    //    public Matrix_old_to_delete(MathNet.Numerics.LinearAlgebra.Matrix A)
    //    {
    //        Base = A;
    //    }

    //    /// <summary>Constructs an d1*d2 - dimensional matrix of zeros.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d1">Number of columns.</param>
    //    public Matrix_old_to_delete(int d1, int d2)
    //    {
    //        if (d1 <= 0)
    //            throw new ArgumentException("Can not create a matrix with number of rows less thatn 1. Provided number of rows: "
    //                + d1.ToString() + ".");
    //        if (d2 <= 0)
    //            throw new ArgumentException("Can not create a matrix with number of columns less thatn 1. Provided number of columns: "
    //                + d2.ToString() + ".");
    //        Base = new MathNet.Numerics.LinearAlgebra.Matrix(d1, d2);
    //    }

    //    /// <summary> Construct an numrows-by-d2 constant matrix with specified value for all elements.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    /// <param name="value">Value of all components.</param>
    //    public Matrix_old_to_delete(int d1, int d2, double val)
    //    {
    //        if (d1 <= 0)
    //            throw new ArgumentException("Can not create a matrix with number of rows less thatn 1. Provided number of rows: "
    //                + d1.ToString() + ".");
    //        if (d2 <= 0)
    //            throw new ArgumentException("Can not create a matrix with number of columns less thatn 1. Provided number of columns: "
    //                + d2.ToString() + ".");
    //        Base = new MathNet.Numerics.LinearAlgebra.Matrix(d1, d2, val);
    //    }

    //    /// <summary>Constructs a square matrix with specified diagonal values.</summary>
    //    /// <param name="d">Size of the square matrix.</param>
    //    /// <param name="val">Vector of diagonal values.</param>
    //    public Matrix_old_to_delete(IVector diagonal)
    //    {
    //        if (diagonal == null)
    //            throw new ArgumentNullException("Can not create a square diagonal matrix, vector of diagonal elements not specified (null reference).");
    //        int dim = diagonal.Length;
    //        Base = new MathNet.Numerics.LinearAlgebra.Matrix(dim, dim);
    //        for (int i = 0; i < dim; ++i)
    //            Base[i, i] = diagonal[i];
    //    }

    //    /// <summary>Constructs a d*d square matrix with specified diagonal value.</summary>
    //    /// <param name="d">Size of the square matrix.</param>
    //    /// <param name="val">Diagonal value.</param>
    //    public Matrix_old_to_delete(int d, double val)
    //    {
    //        if (d <= 0)
    //            throw new ArgumentException("Can not create a square matrix with number of rows and columns less thatn 1. Provided number of rows: "
    //                + d.ToString() + ".");
    //        Base = new MathNet.Numerics.LinearAlgebra.Matrix(d, val);
    //    }

    //    /// <summary>Constructs a matrix from a jagged 2-D array, directly using the provided array as 
    //    /// internal data structure.</summary>
    //    /// <param name="A">Two-dimensional jagged array of doubles.</param>
    //    /// <exception cref="System.ArgumentException">All rows must have the same length.</exception>
    //    /// <seealso cref="Matrix.Create(double[][])"/>
    //    /// <seealso cref="Matrix.Create(double[,])"/>
    //    public Matrix_old_to_delete(double[][] A)
    //    {
    //        if (A == null)
    //            throw new ArgumentNullException("Jagged array to create a matrix from is not specified (null reference).");
    //        if (A.Length == 0)
    //            throw new ArgumentException("Jagged array to create a matrix from has 0 rows.");
    //        int numrows = A.Length;
    //        int numcols = A[0].Length;
    //        if (numcols == 0)
    //            throw new ArgumentException("Invalid jagged array to create a matrix from: The first row has 0 elements.");
    //        for (int i = 1; i < A.Length; ++i)
    //            if (A[i].Length != numcols)
    //                throw new ArgumentException("Invalid jagged array to create a matrix from: not all rows have the same length; caused by row No. " 
    //                    + i.ToString());

    //        Base = new MathNet.Numerics.LinearAlgebra.Matrix(A);
    //    }

    //    /// <summary>Constructs a matrix from a 2-D array by deep-copying the provided array 
    //    /// to the internal data structure.</summary>
    //    /// <param name="A">Two-dimensional array of doubles.</param>
    //    public Matrix_old_to_delete(double[,] A)
    //    {
    //        if (A == null)
    //            throw new ArgumentNullException("Array to create a matrix from is not specified (null reference).");
    //        if (A.GetLength(0) == 0)
    //            throw new ArgumentException("Array to create a matrix from has 0 rows.");
    //        if (A.GetLength(1) == 0)
    //            throw new ArgumentException("Array to create a matrix from has 0 columns.");
    //        // Copy the array to a jagged array:
    //        double[][] B;
    //        B = new double[A.GetLength(0)][];
    //        for (int i = 0; i < A.GetLength(0); ++i)
    //        {
    //            B[i] = new double[A.GetLength(1)];
    //            for (int j = 0; j < A.GetLength(1); ++j)
    //                B[i][j] = A[i, j];
    //        }
    //        Base = new MathNet.Numerics.LinearAlgebra.Matrix(B);
    //    }

    //    /// <summary>Construct a matrix from a one-dimensional packed array.</summary>
    //    /// <param name="vals">One-dimensional array of doubles, packed by columns (ala Fortran).</param>
    //    /// <param name="numrows">Number of rows.</param>
    //    /// <exception cref="System.ArgumentException">Array length must be a multiple of numrows.</exception>
    //    [Obsolete("This method may be unsupported in future versions.")]
    //    public Matrix_old_to_delete(double[] vals, int numrows)
    //    {
    //        Base = new MathNet.Numerics.LinearAlgebra.Matrix(vals, numrows);
    //    }


    //    #region StaticConstruction



    //    /// <summary>Constructs a matrix from a copy of a 2-D array by deep-copy.</summary>
    //    /// <param name="A">Two-dimensional array of doubles.</param>
    //    public static Matrix Create(double[][] A)
    //    {
    //        if (A == null)
    //            throw new ArgumentNullException("Jagged array to create a matrix from is not specified (null reference).");
    //        if (A.Length == 0)
    //            throw new ArgumentException("Jagged array to create a matrix from has 0 rows.");
    //        if (A[0].Length == 0)
    //            throw new ArgumentException("Jagged array to create a matrix from has 0 columns.");
    //        return new Matrix(MathNet.Numerics.LinearAlgebra.Matrix.Create(A));
    //    }

    //    /// <summary>Constructs a matrix from a copy of a 2-D array by deep-copy.</summary>
    //    /// <param name="A">Two-dimensional array of doubles.</param>
    //    public static Matrix Create(double[,] A)
    //    {
    //        if (A == null)
    //            throw new ArgumentNullException("Array to create a matrix from is not specified (null reference).");
    //        if (A.GetLength(0) == 0)
    //            throw new ArgumentException("Array to create a matrix from has 0 rows.");
    //        if (A.GetLength(1) == 0)
    //            throw new ArgumentException("Array to create a matrix from has 0 columns.");
    //        return new Matrix(MathNet.Numerics.LinearAlgebra.Matrix.Create(A));
    //    }

    //    /// <summary>Construct a complex matrix from a set of real column vectors.</summary>
    //    public static Matrix CreateFromColumns(IList<Vector> columnVectors)
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
    //        return new Matrix(newData);
    //    }

    //    /// <summary>Construct a complex matrix from a set of real row vectors.</summary>
    //    public static Matrix CreateFromRows(IList<Vector> rowVectors)
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
    //        return new Matrix(newData);
    //    }


    //    // TODO: add a couple of other creations from lists of doubles, from submatirices, etc.!



    //    /// <summary>Creates a d1*d2 identity matrix.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    /// <returns>An d1*d2 matrix with ones on the diagonal and zeros elsewhere.</returns>
    //    public static Matrix Identity(int d1, int d2)
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
    //        return new Matrix(data);
    //    }

    //    /// <summary>Creates a square identity matrix of dimension d*d.</summary>
    //    /// <param name="d">Matrix dimension.</param>
    //    /// <returns>A d*d identity matrix.</returns>
    //    public static Matrix Identity(int d)
    //    {
    //        return Matrix.Identity(d, d);
    //    }

    //    /// <summary>Creates a d1*d2 matrix filled with 0.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    public static Matrix Zeros(int d1, int d2)
    //    {
    //        return new Matrix(d1, d2,0.0);
    //    }

    //    /// <summary>creates a square d*d matrix filled with 0.</summary>
    //    /// <param name="d">Number of rows and columns.</param>
    //    public static Matrix Zeros(int d)
    //    {
    //        return new Matrix(d, d,0.0);
    //    }

    //    /// <summary>Creates a d1*d2 matrix filled with 1.</summary>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    public static Matrix Ones(int d1, int d2)
    //    {
    //        return new Matrix(d1, d2,1.0);
    //    }

    //    /// <summary>Generates a square d*d matrix filled with 1.</summary>
    //    /// <param name="d1">Number of rows and columns.</param>
    //    public static Matrix Ones(int d)
    //    {
    //        return new Matrix(d, d, 1.0);
    //    }

    //    /// <summary>Creates a new diagonal d1*d2 matrix based on the diagonal vector.</summary>
    //    /// <param name="diagonalVector">The values of the matrix diagonal.</param>
    //    /// <param name="d1">Number of rows.</param>
    //    /// <param name="d2">Number of columns.</param>
    //    /// <returns>A d1*d2 matrix with the values from the diagonal vector on the diagonal and zeros elsewhere.</returns>
    //    public static Matrix Diagonal(IVector<double> diagonalVector, int d1, int d2)
    //    {
    //        if (diagonalVector == null)
    //            throw new ArgumentNullException("Vector to create a diagonal matrix from is not specified (nul reference).");
    //        if (diagonalVector.Length==0)
    //            throw new ArgumentException("Vector to create a diagonal matrix from has 0 elements.");
    //        if (d1 < diagonalVector.Length || d2 < diagonalVector.Length)
    //            throw new ArgumentException("Vector to create a diagonal matrix from is too large.");
    //        if (d1>diagonalVector.Length && d2>diagonalVector.Length)
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
    //        return new Matrix(data);
    //    }

    //    /// <summary>Creates a new square diagonal matrix based on the diagonal vector.</summary>
    //    /// <param name="diagonalVector">The values of the matrix diagonal.</param>
    //    /// <returns>A square matrix with the values from the diagonal vector on the diagonal and zeros elsewhere.</returns>
    //    public static Matrix Diagonal(IVector<double> diagonalVector)
    //    {
    //        return Matrix.Diagonal(diagonalVector, diagonalVector.Length, diagonalVector.Length);
    //    }


    //    ///// <summary>Creates a d1*d2 matrix with random elements.</summary>
    //    ///// <param name="d1">Number of rows.</param>
    //    ///// <param name="d2">Number of columns.</param>
    //    ///// <param name="randomDistribution">Continuous Random Distribution or Source.</param>
    //    ///// <returns>An d1*d2 matrix with elements distributed according to the provided distribution.</returns>
    //    //public static Matrix Random(int d1, int d2, IContinuousGenerator randomDistribution)
    //    //{
    //    //    double[][] data = new double[d1][];
    //    //    for (int i = 0; i < d1; i++)
    //    //    {
    //    //        double[] col = new double[d2];
    //    //        for (int j = 0; j < d2; j++)
    //    //        {
    //    //            col[j] = randomDistribution.NextDouble();
    //    //        }

    //    //        data[i] = col;
    //    //    }
    //    //    return new Matrix(data);
    //    //}

    //    ///// <summary>Creates a d*d square matrix with random elements.</summary>
    //    ///// <param name="d">Number of rows and columns.</param>
    //    ///// <param name="randomDistribution">Continuous Random Distribution or Source.</param>
    //    ///// <returns>An d*d matrix with elements distributed according to the provided distribution.</returns>
    //    //public static Matrix Random(int d, IContinuousGenerator randomDistribution)
    //    //{
    //    //    return Random(d, d, randomDistribution);
    //    //}

    //    ///// <summary>Generates a d1*d2 matrix with standard-distributed random elements.</summary>
    //    ///// <param name="d1">Number of rows.</param>
    //    ///// <param name="d2">Number of columns.</param>
    //    ///// <returns>A d1*d2 matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
    //    //public static Matrix Random(int d1, int d2)
    //    //{
    //    //    return Random(d1, d2, new StandardDistribution());
    //    //}

    //    ///// <summary>Generates a d*d square matrix with standard-distributed random elements.</summary>
    //    ///// <param name="d">Number of rows and columns.</param>
    //    ///// <returns>A d*d square matrix with uniformly distributed random elements in <c>[0, 1)</c> interval.</returns>
    //    //public static Matrix Random(int d)
    //    //{
    //    //    return Random(d, d);
    //    //}

    //    #endregion StaticConstruction


    //    #endregion  // Construction


    //    #region Data


    //    private MathNet.Numerics.LinearAlgebra.Matrix _base = null;

    //    private int _rowCount, _columnCount;

    //    // protected double[][] _components;

    //    protected internal MathNet.Numerics.LinearAlgebra.Matrix Base
    //    {
    //        get { return _base; }
    //        protected set 
    //        { 
    //            _base = value;
    //            if (value == null)
    //            {
    //                RowCountSetter = 0; 
    //                ColumnCountSetter = 0;
    //                //_components = null;
    //            } else
    //            {
    //                RowCountSetter = value.RowCount;
    //                ColumnCountSetter = value.ColumnCount;

    //                // _components = Base._data;
    //                // REMARK:
    //                // Here the internal data structure for matrix components should be assigned
    //                // the data structure of the Base. However, this is currently not possible due
    //                // to protection level of Base._data. Problem is that even inherited classes can
    //                // not access this field (otherwise, an internal class could be created that inherits
    //                // from Mat.Net matrix and exposes the field such that it would be seen here.)
    //            }
    //        }
    //    }


    //    /// <summary>Gets the first dimension (number of rows) of the matrix.</summary>
    //    public override int RowCount
    //    {
    //        get { return _rowCount; }
    //    }

    //    /// <summary>Gets the second dimension (number of columns) of the matrix.</summary>
    //    public override int ColumnCount
    //    {
    //        get { return _columnCount; }
    //    }

    //    /// <summary>Sets the first dimension (number of rows) of the matrix.
    //    /// This setter must be used very restrictively - only in setters that can change matrix dimensions.
    //    /// Setter is defined separately from getter because the base class' property does not define
    //    /// a getter.</summary>
    //    protected int RowCountSetter { set { _rowCount = value; } }

    //    /// <summary>Sets the first dimension (number of rows) of the matrix.
    //    /// This setter must be used very restrictively - only in setters that can change matrix dimensions.
    //    /// Setter is defined separately from getter because the base class' property does not define
    //    /// a getter.</summary>
    //    protected int ColumnCountSetter { set { _columnCount = value; } }


    //    /// <summary>Gets or set the element indexed by <c>(i, j)</c> in the <c>Matrix</c>.</summary>
    //    /// <param name="i">Row index.</param>
    //    /// <param name="j">Column index.</param>
    //    /// <remarks>Component access is currently a bit slower because it works indirectly through the
    //    /// base matrix' access. This could be corrected if we could assign the components array
    //    /// _base._data to some internal variable, but there is no way to access the component array
    //    /// of a Math.Net matrix due to its protection level (which is currently default, i.e. private,
    //    /// but should be at least protected in order to solve this problem).</remarks>
    //    public override double this[int i, int j]
    //    { get { return _base[i, j]; } set { _base[i, j] = value; } }

    //    ///// <summary>Resizes the current matrix to the specified dimensions.</summary>
    //    ///// <param name="newdimension1">New first dimension of the matrix.</param>
    //    ///// <param name="newdimension2">New second dimension of the matrix.</param>
    //    //protected internal virtual void Resize(int newdimension1, int newdimension2)
    //    //{
    //    //    if (newdimension1 < 1)
    //    //        throw new ArgumentException("Matrix first dimension must be greater than 0.");
    //    //    if (newdimension2 < 1)
    //    //        throw new ArgumentException("Matrix second dimension must be greater than 0.");
    //    //    if (RowCount != newdimension1 || ColumnCount != newdimension2)
    //    //    {
    //    //        MathNet.Numerics.LinearAlgebra.Matrix OldBase = Base;
    //    //        Base = new MathNet.Numerics.LinearAlgebra.Matrix(newdimension1, newdimension2, 0.0);
    //    //        if (Base != null && OldBase != null)
    //    //        {
    //    //            for (int i = 0; i < Base.RowCount && i < OldBase.RowCount; ++i)
    //    //                for (int j = 0; j < Base.ColumnCount && j < OldBase.ColumnCount; ++j)
    //    //                    Base[i, j] = OldBase[i, j];
    //    //        }
    //    //    }
    //    //}


    //    /// <summary>Creates and returns a copy of the current matrix.</summary>
    //    /// <returns>A new copy of the current matrix. 
    //    /// The copy is supposed to be of the same type as the current matrix.</returns>
    //    public Matrix GetCopyThis()
    //    {
    //        int numRows = this.RowCount;
    //        int numColumns = this.ColumnCount;
    //        Matrix ReturnedString = new Matrix(numRows, numColumns);
    //        for (int i = 0; i < numRows; ++i)
    //            for (int j = 0; j < numColumns; ++j)
    //                ReturnedString[i, j] = this[i, j];
    //        return ReturnedString;
    //    }

    //    /// <summary>Creates and returns a copy of the current matrix.</summary>
    //    /// <returns>A new copy of the current matrix. 
    //    /// The copy is supposed to be of the same type as the current matrix.</returns>
    //    public override MatrixBase GetCopyBase()
    //    {
    //        return GetCopyThis();
    //    }


    //    /// <summary>Creates and returns a new matrix with the specified dimensions, and of the same type as 
    //    /// the current matrix.</summary>
    //    /// <param name="rowCount">Number fo rows of the newly created matrix.</param>
    //    /// <param name="columnCount">Number of columns of the newly created matrix.</param>
    //    /// <returns>A newly created matrix of the specified dimensions and of the same type as the current matrix.</returns>
    //    public virtual Matrix GetNewThis(int rowCount, int columnCount)
    //    {
    //        return new Matrix(rowCount, columnCount);
    //    }

    //    /// <summary>Creates and returns a new matrix with the specified dimensions, and of the same type as 
    //    /// the current matrix.</summary>
    //    /// <param name="rowCount">Number fo rows of the newly created matrix.</param>
    //    /// <param name="columnCount">Number of columns of the newly created matrix.</param>
    //    /// <returns>A newly created matrix of the specified dimensions and of the same type as the current matrix.</returns>
    //    public override MatrixBase GetNewBase(int rowCount, int columnCount)
    //    {
    //        return new Matrix(rowCount, columnCount);
    //    }

    //    /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
    //    /// the current matrix.</summary>
    //    public Matrix GetNewThis()
    //    {
    //        return new Matrix(this.RowCount, this.ColumnCount);
    //    }

    //    /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
    //    /// the current matrix.</summary>
    //    public override MatrixBase GetNewBase()
    //    {
    //        return new Matrix(this.RowCount, this.ColumnCount);
    //    }


    //    /// <summary>Creates and returns a new vector with the specified dimension, 
    //    /// and of the type that is consistent with the type of the current matrix.</summary>
    //    /// <param name="length">Dimension of the newly created vector.</param>
    //    /// <returns>A newly created vector of the specified dimension and of the same type as the current matrix.</returns>
    //    public Vector GetNewVectorThis(int length)
    //    {
    //        return new Vector(length);
    //    }


    //    /// <summary>Creates and returns a new vector with the specified dimension, 
    //    /// and of the type that is consistent with the type of the current matrix.</summary>
    //    /// <param name="length">Dimension of the newly created vector.</param>
    //    /// <returns>A newly created vector of the specified dimension and of the same type as the current matrix.</returns>
    //    public override VectorBase GetNewVectorBase(int length)
    //    {
    //        return GetNewVectorThis(length);
    //    }

    //    #endregion Data


    //    #region Conversions

    //    // TODO: implement this region!

    //    #endregion  // Conversions


    //    #region Submatrix_Operations

    //    // TODO: implement this region!


    //    #endregion  // Submatrix_Operations


    //    #region Helpers_Infrastructure

    //    // TODO: implement this region!


    //    #endregion  Helpers_Infrastructure


    //    #region Operations

    //    /// <summary>Copies components of a matrix to another matrix.
    //    /// WARNING: dimensions of the specified result matrix must match dimensions of the original matrix.
    //    /// REMARK: This method is implemented because it is more efficient than the corresponding
    //    /// method in MatrixBase (due to matched types).</summary>
    //    /// <param name="a">Original matrix.</param>
    //    /// <param name="result">Materix where copy will be stored. Dimensions must match dimensions of a.</param>
    //    public static void Copy(Matrix a, Matrix result)
    //    {
    //        if (a == null)
    //            throw new ArgumentNullException("Matrix to be copied is not specified (null reference).");
    //        else if (result == null)
    //            throw new ArgumentNullException("Result matrix for copy operation is not specified (null reference).");
    //        else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
    //            throw new ArgumentException("Dimension mismatch at copying matrices.");
    //        else
    //        {
    //            for (int i = 0; i < a.RowCount; ++i)
    //                for (int j = 0; j < a.ColumnCount; ++j)
    //                    result[i, j] = a[i, j];
    //        }
    //    }

    //    /// <summary>Copies components of a matrix to another matrix.
    //    /// Resulting matrix is allocated or reallocated if necessary.
    //    /// REMARK: This method is implemented because it is more efficient than the corresponding
    //    /// method in MatrixBase (due to matched types).</summary>
    //    /// <param name="a">Original matrix.</param>
    //    /// <param name="result">Materix where copy will be stored. Dimensions must match dimensions of a.</param>
    //    public static void Copy(Matrix a, ref Matrix result)
    //    {
    //        if (a == null)
    //            result = null;
    //        else
    //        {
    //            if (result == null)
    //                result = a.GetNewThis(a.RowCount, a.ColumnCount);
    //            else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
    //                result = a.GetNewThis(a.RowCount, a.ColumnCount);
    //            for (int i = 0; i < a.RowCount; ++i)
    //                for (int j = 0; j < a.ColumnCount; ++j)
    //                    result[i,j] = a[i, j];
    //        }
    //    }


    //    /// <summary>Returns a new matrix that is transpose of the current matrix.
    //    /// Re-implemented here for efficiency (hides the base class property).</summary>
    //    public new Matrix T
    //    {
    //        get
    //        {
    //            Matrix ReturnedString = GetNewThis(ColumnCount, RowCount);
    //            Transpose(this, ReturnedString);
    //            return ReturnedString;
    //        }
    //    }



    //    #region Products

    //    /// <summary>Matrix multiplication by a scalar.</summary>
    //    /// <param name="m1">Matrix that is multiplied.</param>
    //    /// <param name="s2">Scalar with which matrix is multiplied.</param>
    //    /// <param name="result">Matrix where result is stored.</param>
    //    public static void Multiply(Matrix m1, double s2, ref Matrix result)
    //    {
    //        if (m1 == null)
    //            throw new ArgumentNullException("The first operand is not specidied (null reference).");
    //        if (m1 != result)
    //            Copy(m1, ref result);
    //        IMatrix resultAux = result;
    //        bool reallocate = false;
    //        if (result == null)
    //            reallocate = true;
    //        else if (result.RowCount != m1.RowCount || result.ColumnCount != m1.ColumnCount)
    //            reallocate = true;
    //        if (reallocate)
    //            result = new Matrix(m1.RowCount, m1.ColumnCount);
    //        MatrixBase.Multiply(m1, s2, result);
    //    }


    //    /// <summary>Kronecker product, A(m*n)*B(p*q)=C(mp*nq)
    //    /// Result is a block matrix with as many blocks as there are elements of the first matrix,
    //    /// each block being a product of the second matrix by an element of the first matrix.
    //    /// Both operands must be defined (non-null).</summary>
    //    /// <param name="m1">First operand.</param>
    //    /// <param name="m2">Second operand.</param>
    //    /// <param name="result"></param>
    //    public static void KroneckerProduct(Matrix_old_to_delete m1, Matrix_old_to_delete m2, Matrix_old_to_delete result)
    //    {
    //        if (m1 == null)
    //            throw new ArgumentNullException("The first operand is not specidied (null reference).");
    //        if (m2 == null)
    //            throw new ArgumentNullException("The second operand is not specidied (null reference).");
    //        if (m1.RowCount <= 0 || m1.ColumnCount<=0 || m2.RowCount<=0 || m2.ColumnCount<=0)
    //            throw new ArgumentException("Inconsistent dimensions of operands.");
    //        if (m1 == result)
    //            throw new ArgumentException("Result matrix is the same as the first operand, this is not allowed in matrix multiplication.");
    //        if (m2 == result)
    //            throw new ArgumentException("Result matrix is the same as the seconf operand, this is not allowed in matrix multiplication.");
    //        if (result == null)
    //            result = new Matrix_old_to_delete();
    //        result.Base = MathNet.Numerics.LinearAlgebra.Matrix.KroneckerProduct(m1.Base, m2.Base);
    //    }


    //    /// <summary>Kronecker or tensor product, A(m*n)*B(p*q)=C(mp*nq)
    //    /// Result is a block matrix with as many blocks as there are elements of the first matrix,
    //    /// each block being a product of the second matrix by an element of the first matrix.
    //    /// Both operands must be defined (non-null).</summary>
    //    /// <param name="m1">First operand.</param>
    //    /// <param name="m2">Second operand.</param>
    //    /// <param name="result"></param>
    //    public static void KroneckerMultiply(Matrix_old_to_delete m1, Matrix_old_to_delete m2, Matrix_old_to_delete result)
    //    {
    //        KroneckerProduct(m1, m2, result);
    //    }

    //    /// <summary>Kronecker or tensor product, A(m*n)*B(p*q)=C(mp*nq)
    //    /// Result is a block matrix with as many blocks as there are elements of the first matrix,
    //    /// each block being a product of the second matrix by an element of the first matrix.
    //    /// Both operands must be defined (non-null).</summary>
    //    /// <param name="m1">First operand.</param>
    //    /// <param name="m2">Second operand.</param>
    //    /// <param name="result"></param>
    //    public static void TensorMultiply(Matrix_old_to_delete m1, Matrix_old_to_delete m2, Matrix_old_to_delete result)
    //    {
    //        KroneckerMultiply(m1, m2, result);
    //    }



    //    #endregion  // Products


    //    #region Norms


    //    /// <summary>Gets the Forbenius norm of the matrix, the square root of sum of squares of all elements.</summary>
    //    public override double NormForbenius
    //    {
    //        get
    //        {
    //            if (Base == null)
    //                return 0.0;
    //            else
    //                return Base.NormF();
    //        }
    //    }

    //    /// <summary>Gets the Forbenius norm of the matrix, the square root of sum of squares of all elements.</summary>
    //    public override double NormEuclidean
    //    {
    //        get
    //        {
    //            return NormForbenius;
    //        }
    //    }

    //    /// <summary>Gets the Forbenius norm of the matrix, the square root of sum of squares of all elements.</summary>
    //    public override double Norm
    //    {
    //        get
    //        {
    //            return NormForbenius;
    //        }
    //    }

    //    /// <summary>Gets the one norm of the matrix, the maximum column sum of absolute values.</summary>
    //    public virtual double Norm1
    //    {
    //        get
    //        {
    //            if (Base == null)
    //                return 0.0;
    //            else
    //                return Base.Norm1();
    //        }
    //    }

    //    /// <summary>Gets the two norm of the matrix, i.e. its maximal singular value.</summary>
    //    public virtual double Norm2
    //    {
    //        get
    //        {
    //            if (Base == null)
    //                return 0.0;
    //            else
    //                return Base.Norm2();
    //        }
    //    }

    //    /// <summary>Gets the infinity norm of the matrix, the maximum row sum of absolute values.</summary>
    //    public virtual double NormInf
    //    {
    //        get
    //        {
    //            if (Base == null)
    //                return 0.0;
    //            else
    //                return Base.NormInf();
    //        }
    //    }


    //    #endregion Norms



    //    #endregion  // Operations


    //    #region Decompositions

    //    // TODO: implement this region!


    //    #endregion  // Decompositions


    //    #region Linear_Algebra

    //    // TODO: implement this region!  (inverse, determinant, condition number, rank, etc.)


    //    #endregion  // Linear_Algebra


    //    //#region Operators_Overloading


    //    ///// <summary>Unary plus, returns the operand.</summary>
    //    //public static Matrix operator +(Matrix m)
    //    //{
    //    //    if (m == null)
    //    //        return null;
    //    //    else
    //    //        return m.GetCopyThis();
    //    //}

    //    ///// <summary>Unary negation, returns the negative operand.</summary>
    //    //public static Matrix operator -(Matrix v)
    //    //{
    //    //    Matrix ReturnedString = v.GetCopyThis();
    //    //    Matrix.Negate(v, ReturnedString);
    //    //    return ReturnedString;
    //    //}

    //    ///// <summary>Matrix addition.</summary>
    //    //public static Matrix operator +(Matrix a, Matrix b)
    //    //{
    //    //    Matrix ReturnedString = a.GetCopyThis();
    //    //    Matrix.Add(a, b, ReturnedString);
    //    //    return ReturnedString;
    //    //}

    //    /////// <summary>Addition of a scalar to each component of a matrix.</summary>
    //    ////public static Matrix operator +(Matrix a, double s)
    //    ////{
    //    ////    Matrix ReturnedString = null;
    //    ////    Matrix.Add(a, s, ref ReturnedString);
    //    ////    return ReturnedString;
    //    ////}

    //    /////// <summary>Addition of a scalar to each component of a matrix.</summary>
    //    ////public static Matrix operator +(double a, Matrix s)
    //    ////{
    //    ////    Matrix ReturnedString = null;
    //    ////    Matrix.Add(s, a, ref ReturnedString);
    //    ////    return ReturnedString;
    //    ////}


    //    ///// <summary>Matrix subtraction.</summary>
    //    //public static Matrix operator -(Matrix a, Matrix b)
    //    //{
    //    //    Matrix ReturnedString = a.GetCopyThis();
    //    //    Matrix.Subtract(a, b, ReturnedString);
    //    //    return ReturnedString;
    //    //}

    //    /////// <summary>Subtraction of scalar from each component of a matrix.</summary>
    //    ////public static Matrix operator -(Matrix a, double s)
    //    ////{
    //    ////    Matrix ReturnedString = null;
    //    ////    Matrix.Subtract(a, s, ref ReturnedString);
    //    ////    return ReturnedString;
    //    ////}


    //    ///// <summary>Product of two matrices.</summary>
    //    //public static IMatrix operator *(Matrix a, Matrix b)
    //    //{
    //    //    IMatrix ReturnedString = null;
    //    //    Multiply(a, b, ref ReturnedString);
    //    //    return ReturnedString;
    //    //}

    //    ///// <summary>Product of a matrix and a vector.</summary>
    //    //public static IVector operator *(Matrix a, Vector b)
    //    //{
    //    //    IVector ReturnedString = null;
    //    //    Multiply(a, b, ref ReturnedString);
    //    //    return ReturnedString;
    //    //}

    //    ///// <summary>Product of a matrix by a scalar.</summary>
    //    //public static IMatrix operator *(Matrix a, double b)
    //    //{
    //    //    Matrix ReturnedString = null;
    //    //    Matrix.Multiply(a, b, ref ReturnedString);
    //    //    return ReturnedString;
    //    //}

    //    ///// <summary>Product of a matrix by a scalar.</summary>
    //    //public static IMatrix operator *(double a, Matrix b)
    //    //{
    //    //    Matrix ReturnedString = null;
    //    //    Matrix.Multiply(b, a, ref ReturnedString);
    //    //    return ReturnedString;
    //    //}

    //    ///// <summary>Matrix subtraction.</summary>
    //    //public static IMatrix operator /(Matrix a, double b)
    //    //{
    //    //    IMatrix ReturnedString = null;
    //    //    Matrix.Divide(a, b, ref ReturnedString);
    //    //    return ReturnedString;
    //    //}



    //    //#endregion  // Operators_Overloading



    //    #region Formatting_Parsing


    //    /// <summary>Returns a string representation of this vector in a readable form.</summary>
    //    public virtual string ToStringBase()
    //    {
    //        if (Base == null)
    //            return "[[]]";
    //        else
    //            return Base.ToString();
    //    }


    //    #endregion  // Formatting_Parsing


    //    #region Auxiliary

 
    //    /// <summary>Overrides the GetHashCode() appropriately.</summary>
    //    /// <returns></returns>
    //    public override int GetHashCode()
    //    {
    //        if (RowCount < 1 || ColumnCount < 1)
    //            return RowCount.GetHashCode() ^ ColumnCount.GetHashCode();
    //        else
    //        {
    //            int ReturnedString = 0;
    //            for (int i=0; i < RowCount; ++i)
    //                for (int j=0; j < ColumnCount; ++j)
    //                {
    //                    ReturnedString ^= this[i, j].GetHashCode(); ;
    //                }
    //            return ReturnedString;
    //        }
    //    }

    //    /// <summary>Overrides the Equals() appropriately.</summary>
    //    public override bool Equals(Object obj)
    //    {
    //        IMatrix m = obj as IMatrix;
    //        if (m == null)
    //            return false;
    //        else if (this.RowCount != m.RowCount || this.ColumnCount != m.ColumnCount)
    //            return false;
    //        else
    //        {
    //            for (int i = 0; i < RowCount; ++i)
    //                for (int j = 0; j < ColumnCount; ++j)
    //                {
    //                    if (this[i,j]!=m[i,j])
    //                        return false;
    //                }

    //        }
    //        return true;
    //    }

    //    #endregion Auxiliary




    //    /// <summary>A test method, just prints some output.</summary>
    //    public static new void TestStaticMethodSpecific()
    //    {
    //        Console.WriteLine("TestStaticMethod from the Matrix class.");
    //    }




    //}  // class Matrix

}  // namespace IG.Num
