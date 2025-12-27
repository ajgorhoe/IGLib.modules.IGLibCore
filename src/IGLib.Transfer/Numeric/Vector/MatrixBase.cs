// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

    // MATRICES

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;


//$$
//using Matrix_MathNet = MathNet.Numerics.LinearAlgebra.Matrix;

using Matrix_MathNetNumercs = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix;
using MatrixBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Matrix<double>;

using IG.Lib;

namespace IG.Num
{



    //TODO: Define abstract class MatrixBase from which all matrix classes will inherit!

    /// <summary>Generic Matrix interface.</summary>
    /// $A Igor Sep08 Jan09;
    public interface IMatrix<ComponentType>
    {

        /// <summary>Gets the number of rows.</summary>
        int RowCount { get; }

        /// <summary>Gets the number of columns.</summary>
        int ColumnCount { get; }

        /// <summary>Gets total number of elements.
        /// <para>Warning: this is usually done by multiplying <see cref="RowCount"/> and <see cref="ColumnCount"/>,
        /// so it is not a priceless operation.</para></summary>
        int Count { get; }

        /// <summary>Gets or set the element indexed by <c>(i, j)</c> in the <c>Matrix</c>.</summary>
        /// <param name="i">Row index.</param>
        /// <param name="j">Column index.</param>
        ComponentType this[int i, int j] { get; set; }

        /// <summary>Gets or set the element indexed by the specified <c>flat index</c> in the <c>Matrix</c>.</summary>
        /// <param name="flatIndex">Flat (one dimensional) index that addresses matrix element.</param>
        ComponentType this[int flatIndex] { get; set; }

        /// <summary>Creates and returns a rectangular 2D array that contains a component-wise copy of the matrix.</summary>
        ComponentType[,] ToArray();

        /// <summary>Creates and returns a jagged 2D array that contains a component-wise copy of the matrix.</summary>
        ComponentType[][] ToJaggedArray();

        #region Operations

        /// <summary>Sets all components of the current matrix to 0.</summary>
        void SetZero();

        /// <summary>Negates the matrix.</summary>
        void Negate();

        /// <summary>Gets matrix trace (sum of diagonal terms).</summary>
        ComponentType Trace { get; }

        #region Norms

        /// <summary>Gets Forbenious (or Euclidean) norm of the matrix - square root of sum of squares of elements.</summary>
        double NormForbenius
        { get; }

        /// <summary>Gets Forbenious (or Euclidean) norm of the matrix - square root of sum of squares of elements.</summary>
        [Obsolete("Use NormForbenius instead!")]
        double NormEuclidean
        { get; }

        /// <summary>Gets Forbenious (or Euclidean) norm of the matrix - square root of sum of squares of elements.</summary>
        double Norm
        { get; }

        #endregion Norms

        #endregion Operations


    }  // interface IMatrix<T>


    /// <summary>Real matrix interface.</summary>
    /// $A Igor Sep08 Dec09;
    public interface IMatrix : IMatrix<double> 
    {

        ///// <summary>Creates and returns a copy of the current matrix.</summary>
        ///// <returns>A new copy of the current matrix. 
        ///// The copy is supposed to be of the same type as the current matrix.</returns>
        //MatrixBase GetCopyBase();

        ///// <summary>Creates and returns a new matrix with the specified dimensions, and of the same type as 
        ///// the current matrix.</summary>
        ///// <param name="rowCount">Number fo rows of the newly created matrix.</param>
        ///// <param name="columnCount">Number of columns of the newly created matrix.</param>
        ///// <returns>A newly created matrix of the specified dimensions and of the same type as the current matrix.</returns>
        //MatrixBase GetNewBase(int rowCount, int columnCount);

        ///// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
        ///// the current matrix.</summary>
        //MatrixBase GetNewBase();

        ///// <summary>Creates and returns a new vector with the specified dimension, 
        ///// and of the type that is consistent with the type of the current vector.</summary>
        ///// <param name="length">Dimension of the newly created vector.</param>
        ///// <returns>A newly created vector of the specified dimension and of the same type as the current vector.</returns>
        //VectorBase GetNewVectorBase(int length);

        /// <summary>Creates and returns a copy of the current matrix.</summary>
        /// <returns>A new copy of the current matrix. 
        /// The copy is supposed to be of the same type as the current matrix.</returns>
        IMatrix GetCopy();

        /// <summary>Creates and returns a new matrix with the specified dimensions, and of the same type as 
        /// the current matrix.</summary>
        /// <param name="rowCount">Number fo rows of the newly created matrix.</param>
        /// <param name="columnCount">Number of columns of the newly created matrix.</param>
        /// <returns>A newly created matrix of the specified dimensions and of the same type as the current matrix.</returns>
        IMatrix GetNew(int rowCount, int columnCount);

        /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
        /// the current matrix.</summary>
        IMatrix GetNew();

        /// <summary>Creates and returns a new vector with the specified dimension, 
        /// and of the type that is consistent with the type of the current vector.</summary>
        /// <param name="length">Dimension of the newly created vector.</param>
        /// <returns>A newly created vector of the specified dimension and of the same type as the current vector.</returns>
        IVector GetNewVector(int length);

        
        /// <summary>Sets all components of the current matrix to the specified value.</summary>
        /// <param name="elementValue">Value to which elements are set.</param>
        void SetConstant(double elementValue);

        /// <summary>Sets the current matrix to identity matrix.</summary>
        void SetIdentity();

        /// <summary>Sets the current matrix such that it contains random elements on the interval (0,1].</summary>
        void SetRandom();

        /// <summary>Sets the current matrix such that it contains random elements on the interval (0,1].</summary>
        /// <param name="rnd">Random generator used to generate matrix elements.</param>
        void SetRandom(IRandomGenerator rnd);
        
        /// <summary>Sets the current matrix to the diagonal matrix with all diagonal elements 
        /// equal to the specified value.
        /// Matrix does not need to be a square matrix.</summary>
        /// <param name="diagonalElement">Value of diagonal elements.</param>
        void SetDiagonal(double diagonalElement);

        /// <summary>Sets the current matrix to the diagonal matrix with diagonal element specified by a vector.</summary>
        /// <param name="diagonal">Vector of diagonal elements.</param>
        void SetDiagonal(IVector diagonal);

        /// <summary>Returns true if the current matrix is a square matrix, and false if not.</summary>
        bool IsSquare();

        /// <summary>Returns true if the current matrix is symmetric, and false if not.
        /// If the matrix is not a square matrix then false is returned.</summary>
        bool IsSymmetric();

        /// <summary>Returns true if the specified matrix is symmetric within some tolerance, and false if not.
        /// If the matrix is null then false is returned.</summary>
        /// <param name="relativeRMSTolerance">Tolerance on the ratio between RMS of differences between out of diagonal terms
        /// and their transposes and between RMS of out of diagonal terms, below which matrix is considered symmetric.</param>
        bool IsSymmetric(double relativeRMSTolerance);

        
         
        /// <summary>Returns a readable an easily string form of a matrix, accuracy and padding can be set.</summary>
        /// <param name="accuracy">Accuracy of matrix elments representations.</param>
        /// <param name="padding">Paddind of matrix elements.</param>
        /// <returns>A readable string representation in tabular form.</returns>
        string ToStringReadable(int accuracy = 4, int padding = 8);


        /// <summary>Returns a string representation of this matrix with newlines inserted after each row.
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        string ToStringNewlines();

        /// <summary>Returns a string representation of this matrix with newlines inserted after each row, 
        /// with the specified format for elements of the matrix.
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        /// <param name="elementFormat">Format specification for printing individual element.</param>
        string ToStringNewlines(string elementFormat);

        /// <summary>Returns string representation of the current matrix in the standard IGLib form
        /// (Mathematica-like format but with C representation of numbers).
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        string ToStringMath();

        /// <summary>Returns string representation of the current matrix in the standard IGLib form
        /// (Mathematica-like format but with C representation of numbers).
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        string ToString();

        /// <summary>Returns a string representation of the current matrix in a standard IGLib form
        /// (Mathematica-like format but with C representation of numbers), with the specified  
        /// format for elements of the matrix.</summary>
        /// <param name="elementFormat">Format specification for printing individual element.</param>
        string ToString(string elementFormat);

        /// <summary>Returns a string representation of the current matrix in a standard IGLib form
        /// (Mathematica-like format but with C representation of numbers), with the specified  
        /// format for elements of the matrix.</summary>
        /// <param name="elementFormat">Format specification for printing individual element.</param>
        string ToStringMath(string elementFormat);
        
        /// <summary>Returns an integer valued hash function of the current matrix object.
        /// <para>The returned value is calculated by the <see cref="Util.GetHashFunctionInt"/> method.</para></summary>
        /// <seealso cref="Util.GetHashFunctionInt"/>
        int GetHashFunctionInt();
        
        /// <summary>Returns a string valued hash function of the current matrix object.
        /// <para>The returned value is calculated by the <see cref="Util.GetHashFunctionString"/> method.</para></summary>
        /// <remarks>The returned string is always on the same length, and is based on the <see cref="ToString()"/> method.
        /// Therefore it is convenient for use in file or directory names that have one part related to a specific matrix.</remarks>
        /// <seealso cref="Util.GetHashFunctionString"/>
        string GetHashFunctionString();

    }  //  interface IMatrix


    /// <summary>Extension methods for IMatrix interface.</summary>
    /// $A Igor Apr11;
    public static class MatrixExtensions
    {

        #region ToString

        /// <summary>Easily readable string form of a matrix, accuracy and padding can be set.</summary>
        /// <param name="mat">Matrix to be changed to a string.</param>
        /// <param name="accuracy">Accuracy of matrix elments representations.</param>
        /// <param name="padding">Paddind of matrix elements.</param>
        /// <returns>A readable string representation in tabular form.</returns>
        public static string ToStringReadable(this IMatrix mat, int accuracy = 4, int padding = 8)
        {
            return MatrixBase.ToStringReadable(mat, accuracy, padding);
        }


        /// <summary>Returns string representation of the current matrix in the standard IGLib form. 
        /// Rows and elements are printed in comma separated lists in curly brackets.
        /// Extension method for IMatrix interface.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        public static string ToStringMath(this IMatrix mat)
        {
            return MatrixBase.ToStringMath(mat);
        }

        /// <summary>Returns string representation of the current matrix in the standard IGLib form, with the specified  
        /// format for elements of the matrix. 
        /// Rows and elements are printed in comma separated lists in curly brackets.
        /// Extension method for IMatrix interface.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        /// <param name="elementFormat">Elsment format.</param>
        public static string ToString(this IMatrix mat, string elementFormat)
        {
            return MatrixBase.ToString(mat, elementFormat);
        }

        /// <summary>Returns string representation of the current matrix in the standard IGLib form, with the specified  
        /// format for elements of the matrix. 
        /// Rows and elements are printed in comma separated lists in curly brackets.
        /// Extension method for IMatrix interface.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        /// <param name="elementFormat">Element format.</param>
        public static string ToStringMath(this IMatrix mat, string elementFormat)
        {
            return MatrixBase.ToStringMath(mat, elementFormat);
        }


        /// <summary>Returns a string representation of the current matrix with newlines inserted after each row.
        /// Rows and elements are printed in comma separated lists in curly brackets.
        /// Extension nethod for IMatrix interface.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        public static string ToStringNewlines(IMatrix mat)
        {
            return MatrixBase.ToStringNewlines(mat);
        }

        /// <summary>Returns a string representation of the current matrix with newlines inserted after each row, 
        /// with the specified format for elements of the matrix.
        /// Rows and elements are printed in comma separated lists in curly brackets.
        /// Extension nethod for IMatrix interface.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        /// <param name="elementFormat">Format specification for individual elemeents.</param>
        public static string ToStringNewlines(IMatrix mat, string elementFormat)
        {
            return MatrixBase.ToStringNewlines(mat, elementFormat);
        }

        
        /// <summary>Returns string representation of the current matrix in the standard IGLib form
        /// (Mathematica-like format but with C representation of numbers).
        /// Rows and elements are printed in comma separated lists in curly brackets.
        /// Extension nethod for IMatrix interface.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        public static string ToStringMath1(IMatrix mat)
        {
            return MatrixBase.ToStringMath(mat);
        }

        #endregion ToString



    } // MatrixExtensions


    /// <summary>Base class for matrices.</summary>
    /// $A Igor Jan08 Jul10 Nov10;
    public abstract class MatrixBase: IMatrix
    {

        #region Data

        /// <summary>Gets the number of rows of the current matrix.</summary>
        public abstract int RowCount
        {
            get;
        }

        /// <summary>Gets the number of columns of the current matrix.</summary>
        public abstract int ColumnCount
        {
            get;
        }

        /// <summary>Gets total number of elements.
        /// <para>Warning: this is usually done by multiplying <see cref="RowCount"/> and <see cref="ColumnCount"/>,
        /// so it is not a priceless operation.</para></summary>
        public virtual int Count { get { return RowCount + ColumnCount; } }

        ///// <summary>Gets or sets the specified component of the current matrix.</summary>
        ///// <param name="i">Row number of the component.</param>
        ///// <param name="j">Column number of the component.</param>
        //public abstract double this[int i, int j]
        //{
        //    get;
        //    set;
        //}


        /// <summary>Creates and returns a copy of the current matrix.</summary>
        /// <returns>A new copy of the current matrix. 
        /// The copy is supposed to be of the same type as the current matrix.</returns>
        public abstract MatrixBase GetCopyBase();

        /// <summary>Creates and returns a new matrix with the specified dimensions, and of the same type as 
        /// the current matrix.</summary>
        /// <param name="rowCount">Number fo rows of the newly created matrix.</param>
        /// <param name="columnCount">Number of columns of the newly created matrix.</param>
        /// <returns>A newly created matrix of the specified dimensions and of the same type as the current matrix.</returns>
        public abstract MatrixBase GetNewBase(int rowCount, int columnCount);

                
        /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
        /// the current matrix.</summary>
        public abstract MatrixBase GetNewBase();


        /// <summary>Creates and returns a new vector with the specified dimension, 
        /// and of the type that is consistent with the type of the current vector.</summary>
        /// <param name="length">Dimension of the newly created vector.</param>
        /// <returns>A newly created vector of the specified dimension and of the same type as the current vector.</returns>
        public abstract VectorBase GetNewVectorBase(int length);

        /// <summary>Creates and returns a copy of the current matrix.</summary>
        /// <returns>A new copy of the current matrix. 
        /// The copy is supposed to be of the same type as the current matrix.</returns>
        public virtual IMatrix GetCopy()
        { return GetCopyBase(); }

        /// <summary>Creates and returns a new matrix with the specified dimensions, and of the same type as 
        /// the current matrix.</summary>
        /// <param name="rowCount">Number fo rows of the newly created matrix.</param>
        /// <param name="columnCount">Number of columns of the newly created matrix.</param>
        /// <returns>A newly created matrix of the specified dimensions and of the same type as the current matrix.</returns>
        public virtual IMatrix GetNew(int rowCount, int columnCount)
        { return GetNewBase(rowCount, columnCount); }

        /// <summary>Creates and returns a new matrix with the same dimensions and of the same type as 
        /// the current matrix.</summary>
        public virtual IMatrix GetNew()
        { return GetNewBase(); }

        /// <summary>Creates and returns a new vector with the specified dimension, 
        /// and of the type that is consistent with the type of the current vector.</summary>
        /// <param name="length">Dimension of the newly created vector.</param>
        /// <returns>A newly created vector of the specified dimension and of the same type as the current vector.</returns>
        public virtual IVector GetNewVector(int length)
        { return GetNewVectorBase(length); }

        /// <summary>Creates and returns a rectangular 2D array that contains a component-wise copy of the matrix.</summary>
        public virtual double[,] ToArray()
        {
            double[,] ret = new double[RowCount, ColumnCount];
            for (int i = 0; i < RowCount; ++i)
                for (int j = 0; j < ColumnCount; ++j )
                {
                    ret[i, j] = this[i, j];
                }
            return ret;
        }

        /// <summary>Creates and returns a jagged 2D array that contains a component-wise copy of the matrix.</summary>
        public virtual double[][] ToJaggedArray()
        {
            double[][] ret = new double[RowCount][];
            for (int i = 0; i < RowCount; ++i)
            {
                ret[i] = new double[ColumnCount];
                for (int j = 0; j < ColumnCount; ++j)
                {
                    ret[i][j] = this[i, j];
                }
            }
            return ret;
        }

        /// <summary>Returns a new matrix that is transpose of the current matrix.</summary>
        public virtual IMatrix T
        {
            get
            {
                IMatrix ret = GetNewBase(ColumnCount, RowCount);
                Transpose(this, ret);
                return ret;
            }
        }

        #endregion Data


        #region Indices

        /// <summary>Gets or set the element indexed by a flat index in the <c>Matrix</c>.
        /// <remarks>This method just provides a mechanism of addressing elements by a single (flat) index in derived 
        /// classes where implementation adheres to addressing by a single index (e.g. where elements are
        /// stored in a two dimensional array). In other cases, this indexing operator will be overridden.</remarks></summary>
        /// <param name="flatIndex">Flat element index, i.e. one dimensional index of a matrix element
        /// when elements are expanded to one dimensional array.</param>
        public virtual double this[int flatIndex]
        {
            get
            {
                return this[flatIndex / ColumnCount, flatIndex % ColumnCount];
            }
            set 
            {
                this[flatIndex / ColumnCount, flatIndex % ColumnCount] = value;
            }
        }

        /// <summary>Gets or set the element indexed by <c>(i, j)</c> in the <c>Matrix</c>.
        /// <remarks>This method just provides a mechanism of addressing elements by two indices in derived 
        /// classes where implementation adheres to addressing by a single index (e.g. where elements are
        /// stored in a one dimensional array). In other cases, this indexing operator will be overridden.</remarks></summary>
        /// <param name="row">Row index of the element.</param>
        /// <param name="column">Column index of the element.</param>
        public virtual double this[int row, int column]
        {
            get
            {
                return this[row * ColumnCount + column];
            }
            set 
            {
                this[row * ColumnCount + column] = value;
            }
        }


        /// <summary>Returns flat index corresponding to the specified row and column indices of a matrix
        /// with specified dimensions.</summary>
        /// <param name="dim1">First index.</param>
        /// <param name="dim2">Second index.</param>
        /// <param name="row">Row number (0-based).</param>
        /// <param name="column">Coolumn number (0-based).</param>
        public static int Index(int dim1, int dim2, int row, int column)
        {
            return row * dim2 + column;
        }
        
        /// <summary>Calculates and returns flat index corresponding to the specified row and column indices of the 
        /// specified matrix.</summary>
        /// <param name="mat">Matrix for which flat index is calculated.</param>
        /// <param name="row">Row number (0-based).</param>
        /// <param name="column">Coolumn number (0-based).</param>
        public static int Index(IMatrix mat, int row, int column)
        {
            if (mat == null)
                throw new ArgumentException("Matrix where index is looked for is not specified (null reference).");
            return Index(mat.RowCount, mat.ColumnCount, row, column);
        }
       
        /// <summary>Calculates and returns flat index corresponding to the specified row and column indices of the 
        /// current matrix.</summary>
        /// <param name="row">Row number (0-based).</param>
        /// <param name="column">Coolumn number (0-based).</param>
        public virtual int Index(int row, int column)
        {
            return Index(this.RowCount, this.ColumnCount, row, column);
        }

        /// <summary>Returns (through output arguments) row and column indices corresponding to the
        /// specified flat index in a matrix with the specified dimensions.</summary>
        /// <param name="dim1">First dimension of the matrix (number of rows).</param>
        /// <param name="dim2">Second dimension of the matrix (number of columns).</param>
        /// <param name="flatIndex">Flat element index, i.e. one dimensional index of a matrix element
        /// when elements are expanded to one dimensional array.</param>
        /// <param name="row">Row index corresponding to the specified flat index.</param>
        /// <param name="column">Column index corresponding to the specified flat index.</param>
        public static void Indices(int dim1, int dim2, int flatIndex, out int row, out int column)
        {
            row = flatIndex / dim2;
            column = flatIndex % dim2;
        }

        /// <summary>Returns (through output arguments) row and column indices corresponding to the
        /// specified flat index in the specified matrix.</summary>
        /// <param name="mat">Matrix for which flat index is calculated.</param>
        /// <param name="flatIndex">Flat element index, i.e. one dimensional index of a matrix element
        /// obtained as if elements were expanded to one dimensional array.</param>
        /// <param name="row">Row index corresponding to the specified flat index.</param>
        /// <param name="column">Column index corresponding to the specified flat index.</param>
        public static void Indices(IMatrix mat, int flatIndex, out int row, out int column)
        {
            if (mat == null)
                throw new ArgumentException("Matrix where index is looked for is not specified (null reference).");
            Indices(mat.RowCount, mat.ColumnCount, flatIndex, out row, out column);
        }

        /// <summary>Returns (through output arguments) row and column indices corresponding to the
        /// specified flat index in the current matrix.</summary>
        /// <param name="flatIndex">Flat element index, i.e. one dimensional index of a matrix element
        /// obtained as if elements were expanded to one dimensional array.</param>
        /// <param name="row">Row index corresponding to the specified flat index.</param>
        /// <param name="column">Column index corresponding to the specified flat index.</param>
        public virtual void Indices(int flatIndex, out int row, out int column)
        {
            Indices(RowCount, ColumnCount, flatIndex, out row, out column);
        }


        #endregion Indices



        #region Operations

        #region SetValue

        /// <summary>Sets all components of the current matrix to 0.</summary>
        public virtual void SetZero()
        {
            for (int i = 0; i < RowCount; ++i)
                for (int j = 0; j < ColumnCount; ++j)
                    this[i, j] = 0.0;
        }

        /// <summary>Sets all components of the current matrix to the specified value.</summary>
        /// <param name="elementValue">Value to which elements are set.</param>
        public virtual void SetConstant(double elementValue)
        {
            for (int i = 0; i < RowCount; ++i)
                for (int j = 0; j < ColumnCount; ++j)
                    this[i, j] = elementValue;
        }

        /// <summary>Sets the current matrix to identity matrix.
        /// WARNING:
        /// Exception is thrown if a matrix is not square. For nonsquare matrices, use SetDiagonal(1.0)!</summary>
        public virtual void SetIdentity()
        {
            if (RowCount!=ColumnCount)
                throw new InvalidOperationException("Attempt to set a non-square matrix to identity matrix.");
            for (int i = 0; i < RowCount; ++i)
                for (int j = 0; j < ColumnCount; ++j)
                {
                    if (i == j)
                        this[i, j] = 1.0;
                    else
                        this[i, j] = 0.0;
                }
        }

        /// <summary>Sets the current matrix such that it contains random elements on the interval (0,1].</summary>
        public virtual void SetRandom()
        {
            SetRandom(this);
        }

        /// <summary>Sets the current matrix such that it contains random elements on the interval (0,1].</summary>
        /// <param name="rnd">Random generator used to generate matrix elements.</param>
        public virtual void SetRandom(IRandomGenerator rnd)
        {
            SetRandom(this, rnd);
        }


        /// <summary>Sets the current matrix to the diagonal matrix with all diagonal elements 
        /// equal to the specified value.
        /// Matrix does not need to be a square matrix.</summary>
        /// <param name="diagonalElement">Value of diagonal elements.</param>
        public virtual void SetDiagonal(double diagonalElement)
        {
            for (int i = 0; i < RowCount; ++i)
                for (int j = 0; j < ColumnCount; ++j)
                {
                    if (i == j)
                        this[i, j] = diagonalElement;
                    else
                        this[i, j] = 0.0;
                }
        }

        /// <summary>Sets the current matrix to the diagonal matrix with diagonal element specified by a vector.</summary>
        /// <param name="diagonal">Vector of diagonal elements.</param>
        public virtual void SetDiagonal(IVector diagonal)
        {
            SetDiagonal(this, diagonal);
        }

        #endregion SetValue


        #region QueryType

        /// <summary>Returns true if the current matrix is a square matrix, and false if not.</summary>
        public virtual bool IsSquare()
        {
            if (RowCount == ColumnCount)
                return true;
            return false;
        }

        /// <summary>Returns true if the current matrix is symmetric, and false if not.
        /// If the matrix is not a square matrix then false is returned.</summary>
        public virtual bool IsSymmetric()
        {
            return IsSymmetric(this);
        }

        /// <summary>Returns true if the specified matrix is symmetric within some tolerance, and false if not.
        /// If the matrix is null then false is returned.</summary>
        /// <param name="relativeRMSTolerance">Tolerance on the ratio between RMS of differences between out of diagonal terms
        /// and their transposes and between RMS of out of diagonal terms, below which matrix is considered symmetric.</param>
        public virtual bool IsSymmetric(double relativeRMSTolerance = 0.0)
        {
            return IsSymmetric(this, relativeRMSTolerance);
        }


        #endregion QueryType


        /// <summary>Gets matrix trace (sum of diagonal terms).</summary>
        public virtual double Trace
        {
            get
            {
                if (RowCount != ColumnCount)
                    throw new InvalidOperationException("Can not calculate trace of a non-square matrix.");
                double ret = 0.0;
                for (int i = 0; i < RowCount; ++i)
                    ret += this[i, i];
                return ret;
            }
        }

        /// <summary>Negates the current matrix.</summary>
        public virtual void Negate()
        {
            int i, j;
            for (i = 0; i < this.RowCount; ++i)
                for (j = 0; j < this.ColumnCount; ++j)
                    this[i, j] = -this[j, i];
        }

        /// <summary>Transposes the current matrix.
        /// WARNING: this operation can only be done on square matrices!</summary>
        public virtual void Transpose()
        {
            int i, j;
            double element;
            if (RowCount != ColumnCount)
                throw new InvalidOperationException("Non-square matrix can not be transposed in-place.");
            for (i = 1; i < this.RowCount; ++i)
                for (j = 0; j < i; ++j)
                {
                    element = this[i, j];
                    this[i, j] = this[j, i];
                    this[j, i] = element;
                }
        }



        #region Norms


        /// <summary>Gets Forbenious (or Euclidean) norm of the matrix - square root of sum of squares of elements.</summary>
        public virtual double NormForbenius
        {
            get
            {
                double ret = 0.0;
                int i, j;
                for (i = 0; i < RowCount; ++i)
                    for (j = 0; j < ColumnCount; ++j)
                        ret += this[i, j] * this[i, j];
                return Math.Sqrt(ret);
            }
        }

        /// <summary>Gets Forbenious (or Euclidean) norm of the matrix - square root of sum of squares of elements.</summary>
        [Obsolete("Use NormForbenius instead!")]
        public virtual double NormEuclidean
        {  get{ return NormForbenius; } }

        /// <summary>Gets Forbenious (or Euclidean) norm of the matrix - square root of sum of squares of elements.</summary>
        public virtual double Norm
        { get { return NormForbenius; } }



        #endregion Norms



        #region Operations.Auxiliary


        /// <summary>Returns the hash code (hash function) of the current matrix.</summary>
        /// <remarks>
        /// <para>This method calls the <see cref="MatrixBase.GetHashCode(IMatrix)"/> to calculate the 
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
        /// <remarks>This method calls the <see cref="MatrixBase.Equals(IMatrix, IMatrix)"/> to obtain the returned value, which is
        /// standard for all implementations of the <see cref="IMatrix"/> interface.
        /// <para>Overrides the <see cref="object.Equals(Object)"/> method.</para></remarks>
        public override bool Equals(Object obj)
        {
            return MatrixBase.Equals(this, obj as IMatrix);
        }

        /// <summary>Returns an integer valued hash function of the current matrix object.
        /// <para>The returned value is calculated by the <see cref="Util.GetHashFunctionInt"/> method.</para></summary>
        /// <seealso cref="Util.GetHashFunctionInt"/>
        public int GetHashFunctionInt()
        {
            return Util.GetHashFunctionInt(this);
        }

        /// <summary>Returns a string valued hash function of the current matrix object.
        /// <para>The returned value is calculated by the <see cref="Util.GetHashFunctionString"/> method.</para></summary>
        /// <remarks>The returned string is always of the same length, and is based on the <see cref="ToString()"/> method.
        /// Therefore it is convenient for use in file or directory names that have one part related to a specific matrix.</remarks>
        /// <seealso cref="Util.GetHashFunctionString"/>
        public string GetHashFunctionString()
        {
            return Util.GetHashFunctionString(this);
        }

        #endregion Operations.Auxiliary



        #endregion Operations



        #region StaticOperations


        private static MatrixStore _matrixStore;

        /// <summary>Gets the matrix store for recycling auxiliary matrices.
        /// <para>Matrix store is created only once, on the first access.</para></summary>
        public static MatrixStore MatrixStore
        {
            get
            {
                if (_matrixStore == null)
                {
                    lock (Util.LockGlobal)
                    {
                        if (_matrixStore == null)
                            _matrixStore = new MatrixStore(0, 0, false);
                    }
                }
                return _matrixStore;
            }
        }


        #region SetValueStatic


        /// <summary>Sets all components of the specified matrix to 0.</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        public static void SetZero(IMatrix mat)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to zero is not specified (null argument).");
            for (int i = 0; i < mat.RowCount; ++i)
                for (int j = 0; j < mat.ColumnCount; ++j)
                    mat[i, j] = 0.0;
        }

        /// <summary>Sets all components of the specified matrix to the specified value.</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="elementValue">Value to which elements are set.</param>
        public static void SetConstant(IMatrix mat, double elementValue)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to zero is not specified (null argument).");
            for (int i = 0; i < mat.RowCount; ++i)
                for (int j = 0; j < mat.ColumnCount; ++j)
                    mat[i, j] = elementValue;
        }

        /// <summary>Sets the specified matrix to identity matrix.
        /// WARNING: 
        /// Works only for square matrices (exception is thrown if matrix is not square). For nonsquare matrices, use SetDiagonal(1.0)!</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        public static void SetIdentity(IMatrix mat)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to zero is not specified (null argument).");
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to identity not specified (null matrix).");
            if (mat.RowCount != mat.ColumnCount)
                throw new InvalidOperationException("Attempt to set a non-square matrix to identity matrix.");
            for (int i = 0; i < mat.RowCount; ++i)
                for (int j = 0; j < mat.ColumnCount; ++j)
                {
                    if (i == j)
                        mat[i, j] = 1.0;
                    else
                        mat[i, j] = 0.0;
                }
        }

        /// <summary>Sets the current matrix to the diagonal matrix with diagonal element specified by a vector.</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="diagonal">Vector of diagonal elements.</param>
        public static void SetDiagonal(IMatrix mat, IVector diagonal)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to zero is not specified (null argument).");
            if (diagonal == null)
                throw new ArgumentNullException("Vector of diagonal matrix elements is not specified (null reference).");
            if (diagonal.Length < mat.RowCount && diagonal.Length < mat.ColumnCount || 
                diagonal.Length > mat.RowCount && diagonal.Length > mat.ColumnCount)
                throw new Exception("Vector of diagonal elements has incorrect dimension " + diagonal.Length
                    + " for a matrix with dimensions " + mat.RowCount + "x" + mat.ColumnCount + ".");
            for (int i = 0; i < mat.RowCount; ++i)
                for (int j = 0; j < mat.ColumnCount; ++j)
                {
                    if (i == j)
                        mat[i, j] = diagonal[i];
                    else
                        mat[i, j] = 0;
                }
        }

        /// <summary>Sets the current matrix to the diagonal matrix with all diagonal elements equal to the specified
        /// value.
        /// Matrix does not need to be a square matrix.</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="diagonalElement">Value of diagonal elements.</param>
        public static void SetDiagonal(IMatrix mat, double diagonalElement)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to random matrix is not specified (null argument).");
            for (int i = 0; i < mat.RowCount; ++i)
                for (int j = 0; j < mat.ColumnCount; ++j)
                {
                    if (i == j)
                        mat[i, j] = diagonalElement;
                    else
                        mat[i, j] = 0.0;
                }
        }
        
        /// <summary>Sets the specified matrix such that it contains random elements on the interval (0,1].</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        public static void SetRandom(IMatrix mat)
        {
            IRandomGenerator rnd = RandomGenerator.Global;  // new RandGeneratorThreadSafe(1111);
            SetRandom(mat, rnd);
        }

        /// <summary>Sets the current matrix such that it contains random elements on the interval (0,1].</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="rnd">Random generator used to generate matrix elements. If null then the default (global) random generator iis taken.</param>
        public static void SetRandom(IMatrix mat, IRandomGenerator rnd = null)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to zero is not specified (null argument).");
            if (rnd == null)
                rnd = RandomGenerator.Global;
            int d1 = mat.RowCount, d2 = mat.ColumnCount;
            double element;
            for (int i = 0; i < d1; ++i)
                for (int j = 0; j < d2; ++j)
                {
                    do
                    {
                        element = rnd.NextDouble();
                    } while (element == 0.0);
                    mat[i, j] = element;
                }
        }

        /// <summary>Sets the specified matrix such that it is symmetric and contains random elements on the interval (0,1].</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        public static void SetRandomSymmetric(IMatrix mat)
        {
            IRandomGenerator rnd = RandomGenerator.Global; // new RandGeneratorThreadSafe(1111);
            SetRandomSymmetric(mat, rnd);
        }

        /// <summary>Sets the specified matrix such that it is symmetric and contains random elements on the interval (0,1].</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="rnd">Random generator used to generate matrix elements. If null then the default (global) random generator iis taken.</param>
        public static void SetRandomSymmetric(IMatrix mat, IRandomGenerator rnd)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to symmetric random matrix is not specified (null argument).");
            if (rnd == null)
                rnd = RandomGenerator.Global;
            int d1 = mat.RowCount, d2 = mat.ColumnCount;
            if (d1 != d2)
                throw new ArgumentException("Matrix to be set to random symmetric matrix is not square (dimensions: " 
                    + d1 + "x" + d2 + ")");
            double element;
            for (int i = 0; i < d1; ++i)
            {
                for (int j = 0; j <= i; ++j)
                {
                    do
                    {
                        element = rnd.NextDouble();
                    } while (element == 0.0);
                    mat[i, j] = element;
                    if (i != j)
                        mat[j, i] = element;
                }
            }
        }

        /// <summary>Sets the specified matrix such that it is antisymmetric and contains random elements on the interval (0,1].</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        public static void SetRandomAntiSymmetric(IMatrix mat)
        {
            IRandomGenerator rnd = RandomGenerator.Global; // new RandGeneratorThreadSafe(1111);
            SetRandomAntiSymmetric(mat, rnd);
        }

        /// <summary>Sets the specified matrix such that it is antisymmetric and contains random elements on the interval (0,1].
        /// <para>Matrix will have zero elements on the diagonal.</para></summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="rnd">Random generator used to generate matrix elements. If null then the default (global) random generator iis taken.</param>
        public static void SetRandomAntiSymmetric(IMatrix mat, IRandomGenerator rnd)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to antisymmetric random matrix is not specified (null argument).");
            if (rnd == null)
                rnd = RandomGenerator.Global;
            int d1 = mat.RowCount, d2 = mat.ColumnCount;
            if (d1 != d2)
                throw new ArgumentException("Matrix to be set to random antisymmetric matrix is not square (dimensions: "
                    + d1 + "x" + d2 + ")");
            double element;
            for (int i = 0; i < d1; ++i)
            {
                mat[i, i] = 0.0;
                for (int j = 0; j < i; ++j)
                {
                    do
                    {
                        element = rnd.NextDouble();
                    } while (element == 0.0);
                    mat[i, j] = element;
                    mat[j, i] = -element;
                }
            }
        }

        /// <summary>Sets the specified matrix such that it is lower triangular and contains random elements on the interval (0,1].</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        public static void SetRandomLowerTriangular(IMatrix mat)
        {
            IRandomGenerator rnd = RandomGenerator.Global; // new RandGeneratorThreadSafe(1111);
            SetRandomLowerTriangular(mat, rnd);
        }

        /// <summary>Sets the specified matrix such that it is lower triangular and contains random elements on the interval (0,1].</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="rnd">Random generator used to generate matrix elements. If null then the default (global) random generator iis taken.</param>
        public static void SetRandomLowerTriangular(IMatrix mat, IRandomGenerator rnd)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to lower triangular random matrix is not specified (null argument).");
            if (rnd == null)
                rnd = RandomGenerator.Global;
            int d1 = mat.RowCount, d2 = mat.ColumnCount;
            double element;
            for (int i = 0; i < d1; ++i)
            {
                for (int j = 0; j < d2; ++j)
                {
                    if (j > i)
                        mat[i, j] = 0.0;
                    else
                    {
                        do
                        {
                            element = rnd.NextDouble();
                        } while (element == 0.0);
                        mat[i, j] = element;
                    }
                }
            }
        }

        /// <summary>Sets the specified matrix such that it is upper triangular and contains random elements on the interval (0,1].</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        public static void SetRandomUpperTriangular(IMatrix mat)
        {
            IRandomGenerator rnd = RandomGenerator.Global; // new RandGeneratorThreadSafe(1111);
            SetRandomUpperTriangular(mat, rnd);
        }

        /// <summary>Sets the specified matrix such that it is upper triangular and contains random elements on the interval (0,1].</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="rnd">Random generator used to generate matrix elements. If null then the default (global) random generator iis taken.</param>
        public static void SetRandomUpperTriangular(IMatrix mat, IRandomGenerator rnd)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to lower triangular random matrix is not specified (null argument).");
            if (rnd == null)
                rnd = RandomGenerator.Global;
            int d1 = mat.RowCount, d2 = mat.ColumnCount;
            double element;
            for (int i = 0; i < d1; ++i)
            {
                for (int j = 0; j < d2; ++j)
                {
                    if (j < i)
                        mat[i, j] = 0.0;
                    else
                    {
                        do
                        {
                            element = rnd.NextDouble();
                        } while (element == 0.0);
                        mat[i, j] = element;
                    }
                }
            }
        }

        /// <summary>Sets the specified matrix such that it is positive definite and contains random elements on the interval (0,1].</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        public static void SetRandomSymmetricPositiveDefinite(IMatrix mat)
        {
            IRandomGenerator rnd = RandomGenerator.Global; // new RandGeneratorThreadSafe(1111);
            SetRandomSymmetricPositiveDefinite(mat, rnd);
        }

        /// <summary>Sets the specified matrix such that it is positive definite and contains random 
        /// elements.</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="rnd">Random generator used to generate matrix elements.</param>
        /// <remarks><para>This method is relativley slow because of multiplication of two matrices.
        /// For quicker method use <see cref="SetRandomPositiveDiagonallyDominantSymmetric(IMatrix, IRandomGenerator, double)"/>.</para>
        /// <para>Matrix is created in such a way that a random lower triangular matrix with positive diagonal elements
        /// is created first, then it is multiplied by its transpose.</para>
        /// <para>It seems that generation of positive definite matrices in this way is not stable when elements of the 
        /// lower triangular matrix are random on the interval [0,1). For this reason, 
        /// 1 is added to all diagonal elements of the lower triangular matrix.</para></remarks>
        public static void SetRandomSymmetricPositiveDefinite(IMatrix mat, IRandomGenerator rnd) 
        {
            if (mat == null)
                throw new ArgumentException("Matrix to be set to random positive definite matrix is not specified (null argument).");
            int d1 = mat.RowCount, d2 = mat.ColumnCount;
            if (d1 != d2)
                throw new ArgumentException("Matrix to be set to random positive definite matrix is not square (dimensions: "
                    + d1 + "x" + d2 + ")");
            IMatrix L = null, LT = null;
            // Try to get auxiliary matrices from the matrix store:
            L = MatrixStore.TryGet();
            LT = MatrixStore.TryGet();
            if (L == LT && LT!=null)
                throw new ArgumentException("Matrix L and LT are the same.");
            // Resize if necessary:
            Resize(ref L, d1, d1);
            Resize(ref LT, d1, d1);
            // Set lower triangular matrix and its transpose:
            SetRandomLowerTriangular(L, rnd);
            for (int i = 0; i < d1; ++i) // ensure that diagonal elements are positive:
            {
                 L[i, i] += 1.0;
            }
            Transpose(L, ref LT);
            // Set matrix as product of lower and upper triangular matrix:
            Multiply(L, LT, mat);
            // Try to put auxiliary matrixes back to matrix store:
            MatrixStore.TryStore(L as Matrix);
            MatrixStore.TryStore(LT as Matrix);
        }

        /// <summary>Sets the specified matrix such that it is has random elements and is diagonally dominant with positive diagonal 
        /// elements, i.e. any diagonal element is greater by absolute value than sum of absolute values of nondiagonal 
        /// elements in the corresponding column.</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="rnd">Random generator used to generate matrix elements. If null then global ranom generator is taken.</param>
        /// <param name="dominancyFactor">Factor such that any diagonal element is by absolute value at least by this
        /// factor greater than the sum of absolute values of nondiagonal elements in the corresponding column.</param>
        public static void SetRandomPositiveDiagonallyDominant(IMatrix mat, IRandomGenerator rnd = null, double dominancyFactor = 100.0)
        {
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to lower triangular random matrix is not specified (null argument).");
            if (rnd == null)
                rnd = RandomGenerator.Global;
            int d1 = mat.RowCount, d2 = mat.ColumnCount;
            double element;
            if (dominancyFactor < 0)
                dominancyFactor = -dominancyFactor;
            else if (dominancyFactor == 0)
                dominancyFactor = 1.0;
            if (dominancyFactor < 1.0)
                dominancyFactor = 1.0 / dominancyFactor;
            double outDiagonalFactor = 0.5 * (1.0 / ((double)Math.Max(d1, d2) * dominancyFactor));
            for (int i = 0; i < d1; ++i)
            {
                for (int j = 0; j < d2; ++j)
                {
                    if (i == j)
                    {
                        element = rnd.NextDouble();
                        if (element < 0.5)
                            element = 1.0 - element;
                        mat[i, i] = element;
                    } else
                    {
                        mat[i, j] = rnd.NextDouble() * outDiagonalFactor;
                        mat[j, i] = rnd.NextDouble() * outDiagonalFactor;
                    }
                }
            }
        }

        /// <summary>Sets the specified matrix such that it is has random elements and is diagonally dominant with positive diagonal
        /// elements, i.e. any diagonal element is greater by absolute value than sum of absolute values of nondiagonal 
        /// elements in the corresponding column.</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="dominancyFactor">The average ratio between absolute value of any diagonal term and the sum of absolute values of 
        /// out of diagonal terms in the same column. Should be greater than 1.</param>
        public static void SetRandomPositiveDiagonallyDominant(IMatrix mat, double dominancyFactor)
        {
            SetRandomPositiveDiagonallyDominant(mat, null /* rnd */, dominancyFactor);
        }

        /// <summary>Sets the specified matrix such that it is has random elements and is symmetric diagonally dominant with positive diagonal 
        /// elements, i.e. any diagonal element is greater by absolute value than sum of absolute values of nondiagonal 
        /// elements in the corresponding column.</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="rnd">Random generator used to generate matrix elements. If null then global random generator is taken.</param>
        /// <param name="dominancyFactor">Factor such that any diagonal element is by absolute value at least by this
        /// factor greater than the sum of absolute values of nondiagonal elements in the corresponding column.</param>
        public static void SetRandomPositiveDiagonallyDominantSymmetric(IMatrix mat, IRandomGenerator rnd = null, double dominancyFactor = 100.0)
        {
            SetRandomPositiveDiagonallyDominant(mat, rnd, dominancyFactor);
            SymmetricPart(mat, mat);
        }

        /// <summary>Sets the specified matrix such that it is has random elements and is symmetric diagonally dominant with positive diagonal
        /// elements, i.e. any diagonal element is greater by absolute value than sum of absolute values of nondiagonal 
        /// elements in the corresponding column.</summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="dominancyFactor">The average ratio between absolute value of any diagonal term and the sum of absolute values of 
        /// out of diagonal terms in the same column. Should be greater than 1.</param>
        public static void SetRandomPositiveDiagonallyDominantSymmetric(IMatrix mat, double dominancyFactor)
        {
            SetRandomPositiveDiagonallyDominantSymmetric(mat, null /* rnd */, dominancyFactor);
        }


        // Generation of random invertible or positive definite matrices:


         
        /// <summary>Sets the specified QUADRATIC matrix such that it is has random elements and is nonsingular.
        /// <para>Matrix elements are generated from a product of random lower triangular matrix with 1 on diagonal
        /// and a random upper triangular matrix with elements on the diagonal betweeen 1 and 2, both with nondiagonal terms
        /// between 0 (inclusive) and 1.</para>
        /// <para>Determminant of the generated matrix is returned.</para></summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="rnd">Random generator used to generate matrix elements. If null then global random generator is taken.</param>
        /// <returns>Determinant of the generated matrix, so it can be used in checks.</returns>
        public static double SetRandomInvertible(IMatrix mat, IRandomGenerator rnd = null)
        {
            double determinant = 1.0;
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to lower triangular random matrix is not specified (null argument).");
            if (mat.RowCount != mat.ColumnCount)
                throw new InvalidOperationException("Random invertible matrix can not be generated for non-square matrices.");
            if (rnd == null)
                rnd = RandomGenerator.Global;
            int d1 = mat.RowCount, d2 = mat.ColumnCount;
            IMatrix lower = new Matrix(d1, d2);
            IMatrix upper = new Matrix(d1, d2);
            for (int row = 0; row < d1; ++row)
                for (int col = 0; col < d2; ++col)
                {
                    if (col < row)
                    {
                        // below diagonal
                        lower[row, col] = rnd.NextDouble();
                        upper[row, col] = 0.0;
                    }
                    else if (col > row)
                    {
                        // above diagonal
                        lower[row, col] = 0.0;
                        upper[row, col] = rnd.NextDouble();
                    }
                    else
                    {
                        // diagonal elements
                        double element = 1.0 + rnd.NextDouble();
                        determinant *= element;
                        lower[row, col] = 1.0;
                        upper[row, col] = element;
                    }
                }
            MultiplyPlain(lower, upper, mat);
            return determinant;
        }



        /// <summary>Sets the specified QUADRATIC matrix such that it is has random elements and is a symmetric positive definite matrix.
        /// <para>Matrix elements are generated from a product of random lower triangular matrix with diagonal elements betweeen 1 and 2,
        /// and below diagonal elements between 0 (inclusive) and 1, and its transpose (i.e. from random Cholesky factors).</para>
        /// <para>Determminant of the generated matrix is returned.</para></summary>
        /// <param name="mat">Matrix whose components are set.</param>
        /// <param name="rnd">Random generator used to generate matrix elements. If null then global random generator is taken.</param>
        /// <returns>Determinant of the generated matrix, so it can be used in checks.</returns>
        public static double SetRandomPositiveDefiniteSymmetric(IMatrix mat, IRandomGenerator rnd = null)
        {
            double determinant = 1.0;
            if (mat == null)
                throw new ArgumentNullException("Matrix to be set to random symmetric positive definite is not specified (null argument).");
            if (mat.RowCount != mat.ColumnCount)
                throw new InvalidOperationException("Random symmetric positive definite matrix can not be generated for non-square matrices.");
            if (rnd == null)
                rnd = RandomGenerator.Global;
            int d1 = mat.RowCount, d2 = mat.ColumnCount;
            IMatrix lower = new Matrix(d1, d2);
            for (int row = 0; row < d1; ++row)
                for (int col = 0; col < d2; ++col)
                {
                    if (col < row)
                    {
                        // below diagonal
                        lower[row, col] = rnd.NextDouble();
                    }
                    else if (col > row)
                    {
                        // above diagonal
                        lower[row, col] = 0.0;
                    } else
                    {
                        // diagonal elements
                        double element = 1.0 + rnd.NextDouble();
                        determinant *= element;
                        lower[row, col] = element;
                    }
                }
            determinant *= determinant;  // because thre are 2 factors with the same determinant
            MultiplyTranspMatPlain(lower, lower, mat);
            return determinant;
        }



        #endregion SetValueStatic


        #region QueryTypeStatic

        /// <summary>Returns true if the specified matrix is a square matrix, and false if not.
        /// If the matrix is null then false is returned.</summary>
        /// <param name="mat">Matrix that is tested for being square.</param>
        public static bool IsSquare(IMatrix mat)
        {
            if (mat != null)
                if (mat.RowCount == mat.ColumnCount)
                    return true;
            return false;
        }

        /// <summary>Returns true if the specified matrix is symmetric, and false if not.
        /// If the matrix is null then false is returned.</summary>
        /// <param name="mat">Matrix that is tested for being symmetric.</param>
        public static bool IsSymmetric(IMatrix mat)
        {
            if (IsSquare(mat))
            {
                for (int i = 0; i<mat.RowCount; ++i)
                    for (int j = 0; j < i; ++j)
                    {
                        if (mat[i,j]!=mat[j,i])
                            return false;
                    }
                return true;
            }
            return false;
        }

        /// <summary>Returns true if the specified matrix is symmetric within some tolerance, and false if not.
        /// If the matrix is null then false is returned.</summary>
        /// <param name="mat">Matrix that is tested for being symmetric.</param>
        /// <param name="relativeRMSTolerance">Tolerance on the ratio between RMS of differences between out of diagonal terms
        /// and their transposes and between RMS of out of diagonal terms, below which matrix is considered symmetric.</param>
        public static bool IsSymmetric(IMatrix mat, double relativeRMSTolerance)
        {
            double smallValue = 1.0e-60;
            if (IsSquare(mat))
            {
                if (relativeRMSTolerance<0)
                    relativeRMSTolerance = - relativeRMSTolerance;
                double sumSquares = 0.0, sumSquareDifs = 0.0;
                for (int i = 0; i < mat.RowCount; ++i)
                    for (int j = 0; j < i; ++j)
                    {
                        double m_ij = mat[i, j];
                        double m_ji = mat[j, i];
                        sumSquares += m_ij*m_ij + m_ji*m_ji;
                        sumSquareDifs += 2.0 * (m_ij - m_ji) * (m_ij - m_ji);
                    }
                if (Math.Sqrt(sumSquareDifs / (sumSquares + smallValue)) < relativeRMSTolerance)
                    return true;
            }
            return false;
        }

        #endregion QueryTypeStatic


        /// <summary>Compares two matrices and returns -1 if the first matrix is smaller than the second one,
        /// 0 if matrices are equal, and 1 if the first matrix is greater.
        /// Matrix that is null is considered smaller than a matrix that is not null. Two null matrices are considered equal.
        /// Matrix with smaller dimension is considered smaller than a matrix with greater dimension.
        /// Matrices with equal dimensions ar compared by elements. The first element that is different decides
        /// which matrix is considered greater.</summary>
        /// <param name="m1">First matrix to be compared.</param>
        /// <param name="m2">Second matrix to be compared.</param>
        /// <returns>-1 if the first matrix is smaller than the second one, 0 if matrices are equal, and 1 if the second is greater.</returns>
        /// <remarks>This comparison does not have any mathematical meaning, it is just used for sotting of matrices in data structures.</remarks>
        public static int Compare(IMatrix m1, IMatrix m2)
        {
            if (m1 == null)
            {
                if (m2 == null)
                    return 0;
                else
                    return -1;
            }
            else if (m2 == null)
            {
                return 1;
            }
            else if (m1.RowCount < m2.RowCount)
                return -1;
            else if (m1.RowCount > m2.RowCount)
                return 1;
            else if (m1.ColumnCount < m2.ColumnCount)
                return -1;
            else if (m1.ColumnCount > m2.ColumnCount)
                return 1;
            else
            {
                int dim1 = m1.RowCount;
                int dim2 = m1.ColumnCount;
                for (int i = 0; i < dim1; ++i)
                    for (int j=0; j<dim2; ++j)
                    {
                        double el1 = m1[i,j];
                        double el2 = m2[i,j];
                        if (el1 < el2)
                            return -1;
                        else if (el1 > el2)
                            return 1;
                    }
                return 0;  // all elements are equal
            }
        }



        /// <summary>Resizes, if necessary, the specified matrix according to the required dimensions.
        /// If the matrix is initially null then a new matrix is created. If in this case a template matrix is specified
        /// then the newly created matrix will be of the same type as that template matrix, because it is created by 
        /// the GetNew() method on that matrix.
        /// If dimensions of the initial matrix do not match the required dim., then matrix is resized. 
        /// If the specified matrix dimension is less or equal to 0 then matrix is resized with the same dimensions as 
        /// those of the template matirx. If in this case the template matrix is null, an exception is thrown.
        /// WARNINGS:
        /// Components are NOT preserved and have in general undefined values after operation is performed.
        /// If matrix and template are both null then the type of nawly created matrix is Matrix.</summary>
        /// <param name="mat">Matrix that is resized.</param>
        /// <param name = "template">Matrix that is taken as template (for type of a newly created matrix or for dimensions if
        /// they are not specified).</param>
        /// <param name="rowCount">If greater than 0 then it specifies the number of rows to which matrix is resized.</param>
        /// <param name="columnCount">If greater than 0 then it specifies the number of columns to which matrix is resized.</param>
        public static void Resize(ref IMatrix mat, IMatrix template, int rowCount, int columnCount)
        {
            if (rowCount <= 0 || columnCount <=0)
            {
                if (template == null)
                    throw new ArgumentNullException("Matrix dimensions after resize are not specified (arguments less or equal to 0) and template is not specified.");
                else
                {
                    rowCount = template.RowCount;
                    columnCount = template.ColumnCount;
                }
            }
            if (mat == null)
            {
                if (template != null)
                    mat = template.GetNew(rowCount, columnCount);
                else
                    mat = new Matrix(rowCount, columnCount);
            }
            else if (mat.RowCount != rowCount || mat.ColumnCount!=columnCount)
            {
                mat = mat.GetNew(rowCount, columnCount);
            }
        }

        /// <summary>Resizes, if necessary, the specified matrix according to the required dimensions.
        /// If the matrix is initially null then a new matrix (of type Matrix) is created.
        /// If dimensions of the initial matrix do not match the required dimensions, then matrix is resized. 
        /// Components are NOT preserved and have in general undefined values after operation is performed.
        /// WARNING: 
        /// If the matrix is initially null then the type of the newly created matrix is Matrix.</summary>
        /// <param name="mat">VMatrix that is resized.</param>
        /// <param name="rowCount">Dimension to which matrix is resized (if less than 1 then exception is thrown).</param>
        /// <param name="columnCount">Dimension to which matrix is resized (if less than 1 then exception is thrown).</param>
        public static void Resize(ref IMatrix mat, int rowCount, int columnCount)
        {
            if (rowCount <= 0 || columnCount <=0)
                throw new ArgumentNullException("Matrix dimensions after resize are not specified (argument less or equal to 0).");
            if (mat == null)
                mat = new Matrix(rowCount, columnCount);
            else if (mat.RowCount!=rowCount || mat.ColumnCount!=columnCount)
                mat = mat.GetNew(rowCount, columnCount);
        }


        /// <summary>Resizes, if necessary, the specified matrix according to the dimensions of the specified template matrix.
        /// If the matrix is initially null then a new matrix is created. In this case the newly created matrix will 
        /// be of the same type as that template matrix, because it is created by  the GetNew() method on that matrix.
        /// If dimensions of the initial matrix do not match the dimensions of the template matrix, then matrix is resized. 
        /// If the template matrix is null, then an exception is thrown.
        /// WARNINGS:
        /// Components are NOT preserved and have in general undefined values after operation is performed.
        /// If matrix and template are null then the type of newly created matrix is Matrix.</summary>
        /// <param name="mat">Matrix that is resized.</param>
        /// <param name = "template">Matrix that is taken as template (for type of a newly created matrix or for dimensions).</param>
        public static void Resize(ref IMatrix mat, IMatrix template)
        {
            int rowCount, columnCount;
            if (template == null)
                throw new ArgumentNullException("Template matrix is not specified in vector resize.");
            else
            {
                rowCount = template.RowCount;
                columnCount = template.ColumnCount;
            }
            if (mat == null)
            {
                mat = template.GetNew(rowCount, columnCount);
            } else if (mat.RowCount != rowCount || mat.ColumnCount!=columnCount)
            {
                mat = mat.GetNew(rowCount, columnCount);
            }
        }


        /// <summary>Copies components of a matrix to another matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of the copied matrix and result storage must match.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where copy will be stored. Dimensions must match dimensions of original.</param>
        public static void CopyPlain(IMatrix a, IMatrix result)
        {
            int d1 = a.RowCount;
            int d2 = a.ColumnCount;
            int i, j;
            for (i = 0; i < d1; ++i)
                for (j = 0; j < d2; ++j)
                    result[i, j] = a[i, j];
        }

        /// <summary>Copies components of a matrix to another matrix.
        /// WARNING: dimensions of the copied matrix and result storage must match.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where copy will be stored. Dimensions must match dimensions of original.</param>
        public static void Copy(IMatrix a, IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix to be copied is not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result matrix for copy operation is not specified (null reference).");
            else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                throw new ArgumentException("Dimension mismatch at copying matrices." + Environment.NewLine
                    + "  Original matrix: " + a.RowCount + "x" + a.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                int d1 = a.RowCount;
                int d2 = a.ColumnCount;
                int i, j;
                for (i = 0; i < d1; ++i)
                    for (j = 0; j < d2; ++j)
                        result[i, j] = a[i, j];
            }
        }

        /// <summary>Copies components of a matrix to another matrix.
        /// Resulting matrix is allocated or reallocated if necessary.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where copy is stored.</param>
        public static void Copy(IMatrix a, ref IMatrix result)
        {
            if (a == null)
                result = null;
            else
            {
                int d1 = a.RowCount;
                int d2 = a.ColumnCount;
                if (result == null)
                    result = a.GetNew(d1, d2);
                else if (d1 != result.RowCount || d2 != result.ColumnCount)
                    result = a.GetNew(d1, d2);
                int i, j;
                for (i = 0; i < d1; ++i)
                    for (j = 0; j < d2; ++j)
                        result[i,j] = a[i, j];
            }
        }


        /// <summary>Copies components of a matrix to another matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of the copied matrix and result storage must match.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where copy will be stored. Dimensions must match dimensions of original.</param>
        public static void CopyPlain(MatrixBase_MathNetNumerics a, IMatrix result)
        {
            int d1 = a.RowCount;
            int d2 = a.ColumnCount;
            int i, j;
            for (i = 0; i < d1; ++i)
                for (j = 0; j < d2; ++j)
                    result[i, j] = a[i, j];
        }

        /// <summary>Copies components of a matrix to another matrix.
        /// WARNING: dimensions of the copied matrix and result storage must match.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where copy will be stored. Dimensions must match dimensions of original.</param>
        public static void Copy(MatrixBase_MathNetNumerics a, IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix to be copied is not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result matrix for copy operation is not specified (null reference).");
            else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                throw new ArgumentException("Dimension mismatch at copying matrices." + Environment.NewLine
                    + "  Original matrix: " + a.RowCount + "x" + a.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                int d1 = a.RowCount;
                int d2 = a.ColumnCount;
                int i, j;
                for (i = 0; i < d1; ++i)
                    for (j = 0; j < d2; ++j)
                        result[i, j] = a[i, j];
            }
        }

        /// <summary>Copies components of a matrix to another matrix.
        /// Resulting matrix is allocated or reallocated if necessary.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where copy is stored.</param>
        public static void Copy(MatrixBase_MathNetNumerics a, ref IMatrix result)
        {
            if (a == null)
                result = null;
            else
            {
                int d1 = a.RowCount;
                int d2 = a.ColumnCount;
                if (result == null)
                    result = new Matrix(d1, d2);
                else if (d1 != result.RowCount || d2 != result.ColumnCount)
                    result = new Matrix(d1, d2);
                int i, j;
                for (i = 0; i < d1; ++i)
                    for (j = 0; j < d2; ++j)
                        result[i,j] = a[i, j];
            }
        }


        /// <summary>Copies components of a matrix to another matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of the copied matrix and result storage must match.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where copy will be stored. Dimensions must match dimensions of original.</param>
        public static void CopyPlain(IMatrix a, Matrix_MathNetNumercs result)
        {
            int d1 = a.RowCount;
            int d2 = a.ColumnCount;
            int i, j;
            for (i = 0; i < d1; ++i)
                for (j = 0; j < d2; ++j)
                    result[i, j] = a[i, j];
        }

        /// <summary>Copies components of a matrix to another matrix.
        /// WARNING: dimensions of the copied matrix and result storage must match.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where copy will be stored. Dimensions must match dimensions of original.</param>
        public static void Copy(IMatrix a, Matrix_MathNetNumercs result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix to be copied is not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result matrix for copy operation is not specified (null reference).");
            else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                throw new ArgumentException("Dimension mismatch at copying matrices." + Environment.NewLine
                    + "  Original matrix: " + a.RowCount + "x" + a.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                int d1 = a.RowCount;
                int d2 = a.ColumnCount;
                int i, j;
                for (i = 0; i < d1; ++i)
                    for (j = 0; j < d2; ++j)
                        result[i, j] = a[i, j];
            }
        }

        /// <summary>Copies components of a matrix to another matrix.
        /// Resulting matrix is allocated or reallocated if necessary.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where copy is stored.</param>
        public static void Copy(IMatrix a, ref Matrix_MathNetNumercs result)
        {
            if (a == null)
                result = null;
            else
            {
                int d1 = a.RowCount;
                int d2 = a.ColumnCount;
                if (result == null)
                    result = new Matrix_MathNetNumercs(d1, d2);
                else if (d1 != result.RowCount || d2 != result.ColumnCount)
                    result = new Matrix_MathNetNumercs(d1, d2);
                int i, j;
                for (i = 0; i < d1; ++i)
                    for (j = 0; j < d2; ++j)
                        result[i, j] = a[i, j];
            }
        }


        /// <summary>Stores a negative matrix of the operand in another matrix.
        /// Can be done in-place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of the operand and result storage must match.</summary>
        /// <param name="a">Operand.</param>
        /// <param name="result">Matrix where result is stored. Dimensions must match dimensions of operand.</param>
        public static void NegatePlain(IMatrix a, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = -a[i, j];
        }

        /// <summary>Stores a negative matrix of the operand in another matrix.
        /// Can be done in-place.
        /// WARNING: dimensions of the operand and result storage must match.</summary>
        /// <param name="a">Operand.</param>
        /// <param name="result">Matrix where result is stored. Dimensions must match dimensions of operand.</param>
        public static void Negate(IMatrix a, IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix to be negated is not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result matrix for negation is not specified (null reference).");
            else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix negation." + Environment.NewLine
                    + "  Operand: " + a.RowCount + "x" + a.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                int i, j;
                for (i = 0; i < a.RowCount; ++i)
                    for (j = 0; j < a.ColumnCount; ++j)
                        result[i, j] = -a[i, j];
            }
        }

        /// <summary>Stores a negative matrix of the operand in another matrix.
        /// Can be done in-place.
        /// Resulting matrix is allocated or reallocated if necessary.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where result of negation is be stored.</param>
        public static void Negate(IMatrix a, ref IMatrix result)
        {
            if (a == null)
                result = null;
            else
            {
                if (result == null)
                    result = a.GetNew(a.RowCount, a.ColumnCount);
                else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                    result = a.GetNew(a.RowCount, a.ColumnCount);
                int i, j;
                for (i = 0; i < a.RowCount; ++i)
                    for (j = 0; j < a.ColumnCount; ++j)
                        result[i, j] = -a[i, j];
            }
        }



        /// <summary>Stores transpose of the operand in another matrix.
        /// Can be done in-place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of the operand and result storage must match.</summary>
        /// <param name="a">Operand.</param>
        /// <param name="result">Matrix where result is stored. Dimensions must match dimensions of operand.</param>
        public static void TransposePlain(IMatrix a, IMatrix result)
        {
            if (a == result)
            {
                // Perform transposition in-place:
                if (a.RowCount != a.ColumnCount)
                {
                    throw new ArgumentException("Matrix transpose can not be performed in-place for non-square matrices. "
                        + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
                }
                int i, j;
                double element;
                for (i = 0; i < result.RowCount; ++i)
                {
                    result[i, i] = a[i, i];
                    for (j = 0; j < i; ++j)
                    {
                        element = a[i, j];
                        a[i, j] = a[j, i];
                        a[j, i] = element;
                    }
                }
            } else
            {
                int i, j;
                for (i = 0; i < result.RowCount; ++i)
                    for (j = 0; j < result.ColumnCount; ++j)
                    {
                        result[i, j] = a[j, i];
                    }
            }
        }

        /// <summary>Stores transpose of the operand in another matrix.
        /// Can be done in-place.
        /// WARNING: dimensions of the operand and result storage must match.</summary>
        /// <param name="a">Operand.</param>
        /// <param name="result">Matrix where result is stored. Dimensions must match dimensions of operand.</param>
        public static void Transpose(IMatrix a, IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix to be transposed is not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result matrix for transposition is not specified (null reference).");
            else if (a.RowCount != result.ColumnCount || a.ColumnCount != result.RowCount)
                throw new ArgumentException("Dimension mismatch at matrix transpose." + Environment.NewLine
                    + "  Operand: " + a.RowCount + "x" + a.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                if (a == result)
                {
                    // Perform transposition in-place:
                    if (a.RowCount != a.ColumnCount)
                    {
                        throw new ArgumentException("Matrix transpose can not be performed in-place for non-square matrices. "
                            + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
                    }
                    int i, j;
                    double element;
                    for (i = 0; i < result.RowCount; ++i)
                    {
                        result[i, i] = a[i, i];
                        for (j = 0; j < i; ++j)
                        {
                            element = a[i, j];
                            a[i, j] = a[j, i];
                            a[j, i] = element;
                        }
                    }
                }
                else
                {
                    int i, j;
                    for (i = 0; i < result.RowCount; ++i)
                        for (j = 0; j < result.ColumnCount; ++j)
                        {
                            result[i, j] = a[j, i];
                        }
                }
            }
        }

        /// <summary>Stores transpose of the matrix operand in another matrix.
        /// Can be done in-place.
        /// Resulting matrix is allocated or reallocated if necessary.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="result">Matrix where result of negation is be stored.</param>
        public static void Transpose(IMatrix a, ref IMatrix result)
        {
            if (a == null)
                result = null;
            else
            {
                if (result == null)
                    result = a.GetNew(a.ColumnCount, a.RowCount);
                else if (a.RowCount != result.ColumnCount || a.ColumnCount != result.RowCount)
                    result = a.GetNew(a.ColumnCount, a.RowCount);
                if (a == result)
                {
                    // Perform transposition in-place:
                    if (a.RowCount != a.ColumnCount)
                    {
                        throw new ArgumentException("Matrix transpose can not be performed in-place for non-square matrices. "
                            + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
                    }
                    int i, j;
                    double element;
                    for (i = 0; i < result.RowCount; ++i)
                    {
                        result[i, i] = a[i, i];
                        for (j = 0; j < i; ++j)
                        {
                            element = a[i, j];
                            a[i, j] = a[j, i];
                            a[j, i] = element;
                        }
                    }
                }
                else
                {
                    int i, j;
                    for (i = 0; i < result.RowCount; ++i)
                        for (j = 0; j < result.ColumnCount; ++j)
                        {
                            result[i, j] = a[j, i];
                        }
                }
            }
        }


        /// <summary>Calculates symmetric and antisymmetric part of the specified matrix (symmetric part 
        /// is stored in the first result argument but is calculated last, such that it overrides antisymmetric 
        /// when both arguments point to the same matrix object).
        /// <para>Can be done in-place.</para>
        /// <para>When both symmetric and antisymmetric result arguments point to the same matrix, symmetric
        /// part is stored in the matrix.</para>
        /// <para>This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of the operand and result storage must match.</para>
        /// <para>WARNING: If this <paramref name="antiSym"/> points to the same matrix object as <paramref name="sym"/> then symmetric part is
        /// stored in this matrix (because it overrides antisymmetric part).</para></summary>
        /// <param name="a">Matrix whose symmetric and antisymmetric part are calculated.</param>
        /// <param name="sym">Matrix where symmetric part of <paramref name="a"/> is stored.</param>
        /// <param name="antiSym">Matrix where antisymmetric part of <paramref name="a"/> is stored.
        /// <para>WARNING: If this argument points to the same matrix object as <paramref name="sym"/> then symmetric part is 
        /// stored in this matrix (because it overrides antisymmetric part).</para>
        /// </param>
        public static void SymmetrizePlain(IMatrix a, IMatrix sym, IMatrix antiSym)
        {
            int dim = a.RowCount;
            for (int i = 0; i < dim; ++i)
            {
                // Diagonal elements:
                antiSym[i, i] = 0;
                sym[i, i] = a[i, i];
                double elAntiSym, elSym;
                for (int j = 0; j < i; ++j)
                {
                    elAntiSym = 0.5 * (a[i, j] - a[j, i]);
                    elSym = 0.5 * (a[i, j] + a[j, i]);
                    antiSym[i, j] = elAntiSym;
                    antiSym[j, i] = -elAntiSym;
                    sym[i, j] = elSym;
                    sym[j, i] = elSym;
                }
            }
        }  // SymmetrizePlain(...)

        /// <summary>Calculates symmetric and antisymmetric part of the specified matrix (symmetric part 
        /// is stored in the first result argument but is calculated last, such that it overrides antisymmetric 
        /// when both arguments point to the same matrix object).
        /// <para>Can be done in-place.</para>
        /// <para>When both symmetric and antisymmetric result arguments point to the same matrix, exception is thrown.</para>
        /// <para>Dimensions of the operand and result storage must match, otherwise exception is thrown.</para></summary>
        /// <param name="a">Matrix whose symmetric and antisymmetric part are calculated.</param>
        /// <param name="sym">Matrix where symmetric part of <paramref name="a"/> is stored.</param>
        /// <param name="antiSym">Matrix where antisymmetric part of <paramref name="a"/> is stored.
        /// <para>WARNING: If this argument points to the same matrix object as <paramref name="sym"/> then exception is thrown.</para>
        /// </param>
        public static void Symmetrize(IMatrix a, IMatrix sym, IMatrix antiSym)
        {
            if (a == null)
                throw new ArgumentException("Matrix to be symmetrized is null.");
            if (sym == null)
                throw new ArgumentException("Symmetric result matrix is null.");
            else if (antiSym == null)
            {
                antiSym = sym;
            }
            if (sym == antiSym)
                throw new ArgumentException("Argument to store symmetric part points to the same matrix object as argument to store antisymmetric part.");
            if (a.RowCount != a.ColumnCount)
                throw new ArgumentException("Symmetrization can not be done for non-square matrices. "
                        + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
            if (sym.RowCount != a.RowCount || sym.ColumnCount != a.ColumnCount)
                throw new ArgumentException("Dimensinos of symmetric result matrix do not match dimensions of the original.");
            if (antiSym.RowCount != a.RowCount || antiSym.ColumnCount != a.ColumnCount)
                throw new ArgumentException("Dimensinos of antisymmetric result matrix do not match dimensions of the original.");
            int dim = a.RowCount;
            for (int i = 0; i < dim; ++i)
            {
                // Diagonal elements:
                antiSym[i, i] = 0;
                sym[i, i] = a[i, i];
                double elAntiSym, elSym;
                for (int j = 0; j < i; ++j)
                {
                    elAntiSym = 0.5 * (a[i, j] - a[j, i]);
                    elSym = 0.5 * (a[i, j] + a[j, i]);
                    antiSym[i, j] = elAntiSym;
                    antiSym[j, i] = -elAntiSym;
                    sym[i, j] = elSym;
                    sym[j, i] = elSym;
                }
            }
        }  // SymmetrizePlain(...)

        /// <summary>Stores symmetrized and antisymmetirzed matrix obtained from the specified matrix.
        /// Resulting matrix is allocated or reallocated if necessary.</summary>
        /// <param name="a">Original matrix.</param>
        /// <param name="sym">Matrix where result of symmetrization is stored.</param>
        /// <param name="antiSym">Matrix where result of antisymmetrization is stored.</param>
        public static void Symmetrize(IMatrix a, ref IMatrix sym, ref IMatrix antiSym)
        {
            if (a == null)
                throw new ArgumentException("Matrix to be symmetrized is null.");
            if (a.RowCount != a.ColumnCount)
                throw new ArgumentException("Symmetrization can not be done for non-square matrices. "
                        + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
            int dim = a.RowCount;
            bool allocSym = false, allocAntiSym = false;
            if (sym == null)
                allocSym = true;
            else if (sym.RowCount!=dim || sym.ColumnCount != dim)
                allocSym = true;
            if (antiSym == null)
                allocAntiSym = true;
            else
            {
                if (antiSym == sym)
                {
                    allocAntiSym = true;
                    antiSym = null;
                    if (sym == null)
                    {
                        // Not only that symmetric and antisymmetric storage variables point to the same matrix, but
                        // references to the same variables are passed via these arguments. Both parts can therefore
                        // not be decoupled by allocation, therefore exception is thrown.
                        throw new ArgumentException("The same variable was passed as argument for storing both symmetric and antisymmetric part of the matrix.");
                    }
                }
                if (antiSym.RowCount != dim || antiSym.ColumnCount != dim)
                    allocAntiSym = true;
            }
            if (allocSym)
                sym = a.GetNew();
            if (allocAntiSym)
                antiSym = a.GetNew();
            for (int i = 0; i < dim; ++i)
            {
                // Diagonal elements:
                antiSym[i, i] = 0;
                sym[i, i] = a[i, i];
                double elAntiSym, elSym;
                for (int j = 0; j < i; ++j)
                {
                    elAntiSym = 0.5 * (a[i, j] - a[j, i]);
                    elSym = 0.5 * (a[i, j] + a[j, i]);
                    antiSym[i, j] = elAntiSym;
                    antiSym[j, i] = -elAntiSym;
                    sym[i, j] = elSym;
                    sym[j, i] = elSym;
                }
            }
        }  // SymmetrizePlain(...)



        /// <summary>Stores symmetric part of a square matrix operand in another matrix.
        /// Symmetrization is performed by averaging of non-diagonal terms and their transposed terms.
        /// Can be done in-place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of the operand and result storage must match.</summary>
        /// <param name="a">Operand, must be a square matrix.</param>
        /// <param name="result">Matrix where result is stored. Dimensions must match dimensions of operand.</param>
        public static void SymmetricPartPlain(IMatrix a, IMatrix result)
        {
            // Perform symmetrization in-place:
            if (a.RowCount != a.ColumnCount)
            {
                throw new ArgumentException("Matrix symmetrization can not be performed for non-square matrices. "
                    + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
            }
            int i, j;
            double element;
            for (i = 0; i < result.RowCount; ++i)
            {
                result[i, i] = a[i, i];
                for (j = 0; j < i; ++j)
                {
                    element = 0.5*(a[i, j]+a[j,i]);
                    result[i, j] = element;
                    result[j, i] = element;
                }
            }
        }  // SymmetricPartPlain

        /// <summary>Stores symmetric part of a square matrix operand in another matrix.
        /// Symmetrization is performed by averaging of non-diagonal terms and their transposed terms.
        /// Can be done in-place.
        /// WARNING: dimensions of the operand and result storage must match.</summary>
        /// <param name="a">Operand, must be a square matrix.</param>
        /// <param name="result">Matrix where result is stored. Dimensions must match dimensions of operand.</param>
        public static void SymmetricPart(IMatrix a, IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix to be symmetrized is not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result matrix for symmetrization is not specified (null reference).");
            else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix symmetrization." + Environment.NewLine
                    + "  Operand: " + a.RowCount + "x" + a.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else if (a.RowCount != a.ColumnCount)
            {
                throw new ArgumentException("Matrix symmetrization can not be performed in-place for non-square matrices. "
                    + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
            }
            int i, j;
            double element;
            for (i = 0; i < result.RowCount; ++i)
            {
                result[i, i] = a[i, i];
                for (j = 0; j < i; ++j)
                {
                    element = 0.5 * (a[i, j] + a[j, i]);
                    result[i, j] = element;
                    result[j, i] = element;
                }
            }
        }  // SymmetricPart

        /// <summary>Stores symmetric part of a square matrix operand in another matrix.
        /// Symmetrization is performed by averaging of non-diagonal terms and their transposed terms.
        /// Can be done in-place.
        /// Resulting matrix is allocated or reallocated if necessary.</summary>
        /// <param name="a">Original matrix, must be a square matrix.</param>
        /// <param name="result">Matrix where result of negation is be stored.</param>
        public static void SymmetricPart(IMatrix a, ref IMatrix result)
        {
            if (a == null)
                result = null;
            else if (a.RowCount != a.ColumnCount)
            {
                throw new ArgumentException("Matrix symmetrization can not be performed in-place for non-square matrices. "
                    + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
            } else if (result == null)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            int i, j;
            double element;
            for (i = 0; i < result.RowCount; ++i)
            {
                result[i, i] = a[i, i];
                for (j = 0; j < i; ++j)
                {
                    element = 0.5 * (a[i, j] + a[j, i]);
                    result[i, j] = element;
                    result[j, i] = element;
                }
            }
        }  // SymmetricPart

        /// <summary>Stores antisymmetric part of a square matrix operand in another matrix.
        /// Antisymmetrization is performed by subtracting non-diagonal terms and their transposed terms,
        /// division by 2 and storing result in one matrix element and its negative value in another element.
        /// Can be done in-place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of the operand and result storage must match.</summary>
        /// <param name="a">Operand; must be a square matrix.</param>
        /// <param name="result">Matrix where result is stored. Dimensions must match dimensions of operand.</param>
        public static void AntisymmetricPartPlain(IMatrix a, IMatrix result)
        {
            // Perform antisymmetrization in-place:
            int i, j, d1 = a.RowCount, d2 = a.ColumnCount;
            double element;
            if (d1 != d2)
            {
                throw new ArgumentException("Matrix antisymmetrization can not be performed for non-square matrices. "
                    + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
            }
            for (i = 0; i < d1; ++i)
            {
                result[i, i] = 0;
                for (j = 0; j < i; ++j)
                {
                    element = 0.5*(a[i, j]-a[j,i]);
                    result[i, j] = element;
                    result[j, i] = -element;
                }
            }
        }  // SymmetricPartPlain

        /// <summary>Stores antisymmetric part of a square matrix operand in another matrix.
        /// Antisymmetrization is performed by subtracting non-diagonal terms and their transposed terms,
        /// division by 2 and storing result in one matrix element and its negative value in another element.
        /// Can be done in-place.
        /// WARNING: dimensions of the operand and result storage must match.</summary>
        /// <param name="a">Operand, must be a square matrix.</param>
        /// <param name="result">Matrix where result is stored. Dimensions must match dimensions of operand.</param>
        public static void AntisymmetricPart(IMatrix a, IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix to be antisymmetrized is not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result matrix for symmetrization is not specified (null reference).");
            else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix symmetrization." + Environment.NewLine
                    + "  Operand: " + a.RowCount + "x" + a.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            // Perform antisymmetrization in-place:
            int i, j, d1 = a.RowCount, d2 = a.ColumnCount;
            double element;
            if (d1 != d2)
            {
                throw new ArgumentException("Matrix antisymmetrization can not be performed for non-square matrices. "
                    + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
            }
            for (i = 0; i < d1; ++i)
            {
                result[i, i] = 0;
                for (j = 0; j < i; ++j)
                {
                    element = 0.5 * (a[i, j] - a[j, i]);
                    result[i, j] = element;
                    result[j, i] = -element;
                }
            }
        }  // AntisymmetricPart

        /// <summary>Stores antisymmetric part of a square matrix operand in another matrix.
        /// Antisymmetrization is performed by subtracting non-diagonal terms and their transposed terms,
        /// division by 2 and storing result in one matrix element and its negative value in another element.
        /// Can be done in-place.
        /// Resulting matrix is allocated or reallocated if necessary.</summary>
        /// <param name="a">Original matrix, must be a square matrix.</param>
        /// <param name="result">Matrix where result of negation is be stored.</param>
        public static void AntisymmetricPart(IMatrix a, ref IMatrix result)
        {
            if (a == null)
            {
                result = null;
                return;
            } else if (a.RowCount != a.ColumnCount)
            {
                throw new ArgumentException("Matrix antisymmetrization can not be performed in-place for non-square matrices. "
                    + Environment.NewLine + "  Matrix dimensions: " + a.RowCount + "x" + a.ColumnCount + ".");
            }
            else if (result == null)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            else if (a.RowCount != result.RowCount || a.ColumnCount != result.ColumnCount)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            int i, j;
            double element;
            for (i = 0; i < result.RowCount; ++i)
            {
                result[i, i] = 0;
                for (j = 0; j < i; ++j)
                {
                    element = 0.5 * (a[i, j] - a[j, i]);
                    result[i, j] = element;
                    result[j, i] = -element;
                }
            }
        }  // AntisymmetricPart



        /// <summary>Sums two matrices and stores the result in the specified result matrix.
        /// Operation can be performed in place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored. Dimensions must match dimensions of operands.</param>
        public static void AddPlain(IMatrix a, IMatrix b, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = a[i, j] + b[i, j];
        }


        /// <summary>Sums two matrices and stores the result in the specified result matrix.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored. Dimensions must match dimensions of operands.</param>
        public static void Add(IMatrix a, IMatrix b, IMatrix result)
        {
            if (a == null || b==null)
                throw new ArgumentNullException("Matrix summation: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix summation is not specified (null reference).");
            else if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount
                || result.RowCount!=a.RowCount || result.ColumnCount !=a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix summation. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                int i, j;
                for (i = 0; i < a.RowCount; ++i)
                    for (j = 0; j < a.ColumnCount; ++j)
                        result[i, j] = a[i, j] + b[i,j];
            }
        }

        /// <summary>Sums two matrices and stores the result in the specified result matrix.
        /// Operation can be performed in place.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then teh result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void Add(IMatrix a, IMatrix b, ref IMatrix result)
        {
            if (a == null || b==null)
                throw new ArgumentNullException("Matrix summation: operand not specified (null reference).");
            else if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix summation. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.RowCount,a.ColumnCount);
            else if (result.RowCount!=a.RowCount || result.ColumnCount !=a.ColumnCount)
                result = a.GetNew(a.RowCount,a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = a[i, j] + b[i, j];
        }

        /// <summary>Subtracts two matrices and stores the result in the specified result matrix.
        /// Operation can be performed in place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void SubtractPlain(IMatrix a, IMatrix b, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = a[i, j] - b[i, j];
        }

        /// <summary>Subtracts two matrices and stores the result in the specified result matrix.
        /// Operation can be performed in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void Subtract(IMatrix a, IMatrix b, IMatrix result)
        {
            if (a == null || b==null)
                throw new ArgumentNullException("Matrix subtraction: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix subtraction is not specified (null reference).");
            else if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount
                || result.RowCount!=a.RowCount || result.ColumnCount !=a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix subtraction. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                int i, j;
                for (i = 0; i < a.RowCount; ++i)
                    for (j = 0; j < a.ColumnCount; ++j)
                        result[i, j] = a[i, j] - b[i,j];
            }
        }

        /// <summary>Subtracts two matrices and stores the result in the specified result matrix.
        /// Operation can be performed in place.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then teh result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void Subtract(IMatrix a, IMatrix b, ref IMatrix result)
        {
            if (a == null || b==null)
                throw new ArgumentNullException("Matrix subtraction: operand not specified (null reference).");
            else if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix subtraction. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.RowCount,a.ColumnCount);
            else if (result.RowCount!=a.RowCount || result.ColumnCount !=b.ColumnCount)
                result = a.GetNew(a.RowCount,a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = a[i, j] - b[i,j];
        }


        #region ArrayOperations

        /// <summary>Element-by-element multiplication.
        /// This operation can be performed in place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void ArrayMultiplyPlain(IMatrix a, IMatrix b, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = a[i, j] * b[i, j];
        }

        /// <summary>Element-by-element multiplication.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void ArrayMultiply(IMatrix a, IMatrix b, IMatrix result)
        {
            if (a == null || b==null)
                throw new ArgumentNullException("Matrix array multiplication: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix array multiplication is not specified (null reference).");
            else if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount
                || result.RowCount!=a.RowCount || result.ColumnCount !=a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix array multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                int i, j;
                for (i = 0; i < a.RowCount; ++i)
                    for (j = 0; j < a.ColumnCount; ++j)
                        result[i, j] = a[i, j] * b[i,j];
            }
        }

        /// <summary>Element-by-element multiplication.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then teh result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void ArrayMultiply(IMatrix a, IMatrix b, ref IMatrix result)
        {
            if (a == null || b==null)
                throw new ArgumentNullException("Matrix array multiplication: operand not specified (null reference).");
            else if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix array multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.RowCount,a.ColumnCount);
            else if (result.RowCount!=a.RowCount || result.ColumnCount !=b.ColumnCount)
                result = a.GetNew(a.RowCount,a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = a[i, j] * b[i,j];
        }

        /// <summary>Element-by-element division.
        /// This operation can be performed in place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void ArrayDividePlain(IMatrix a, IMatrix b, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = a[i, j] / b[i, j];
        }

        /// <summary>Element-by-element division.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void ArrayDivide(IMatrix a, IMatrix b, IMatrix result)
        {
            if (a == null || b==null)
                throw new ArgumentNullException("Matrix array division: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix array division is not specified (null reference).");
            else if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount
                || result.RowCount!=a.RowCount || result.ColumnCount !=a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix array division. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                int i, j;
                for (i = 0; i < a.RowCount; ++i)
                    for (j = 0; j < a.ColumnCount; ++j)
                        result[i, j] = a[i, j] / b[i,j];
            }
        }

        /// <summary>Element-by-element division.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then teh result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void ArrayDivide(IMatrix a, IMatrix b, ref IMatrix result)
        {
            if (a == null || b==null)
                throw new ArgumentNullException("Matrix array division: operand not specified (null reference).");
            else if (a.RowCount != b.RowCount || a.ColumnCount != b.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix array division. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.RowCount,a.ColumnCount);
            else if (result.RowCount!=a.RowCount || result.ColumnCount !=b.ColumnCount)
                result = a.GetNew(a.RowCount,a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = a[i, j] / b[i,j];
        }

        /// <summary>Element-by-element raise to power.
        /// This operation can be performed in place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="power">Power to which elements of the matrix are raised.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void ArrayPowerPlain(IMatrix a, double power, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = Math.Pow(a[i, j], power);
        }

        /// <summary>Element-by-element raise to power.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="power">Power to which elements of the matrix are raised.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void ArrayPower(IMatrix a, double power, IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix array raise to power: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix array raise to power is not specified (null reference).");
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix array raise to power. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                int i, j;
                for (i = 0; i < a.RowCount; ++i)
                    for (j = 0; j < a.ColumnCount; ++j)
                        result[i, j] = Math.Pow(a[i, j], power);
            }
        }

        /// <summary>Element-by-element raise to power.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then teh result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="power">Power to which elements of the matrix are raised.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void ArrayPower(IMatrix a, double power, ref IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix array raise to power: operand not specified (null reference).");
            else if (result == null)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = Math.Pow(a[i, j], power);
        }

        /// <summary>Element-by-element mapping of an arbitrary function.
        /// This operation can be performed in place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="mapping">Mapping applied to each element.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void ArrayMapPlain(IMatrix a, Converter<double, double> mapping, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = mapping(a[i,j]);
        }

        /// <summary>Element-by-element mapping of an arbitrary function.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="mapping">Mapping applied to each element.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void ArrayMap(IMatrix a, Converter<double, double> mapping, IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix array raise to power: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix array raise to power is not specified (null reference).");
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix array raise to power. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else
            {
                int i, j;
                for (i = 0; i < a.RowCount; ++i)
                    for (j = 0; j < a.ColumnCount; ++j)
                        result[i, j] = mapping(a[i, j]);
            }
        }

        /// <summary>Element-by-element mapping of an arbitrary function.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then teh result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="mapping">Mapping applied to each element.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void ArrayMap(IMatrix a, Converter<double, double> mapping, ref IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix array raise to power: operand not specified (null reference).");
            else if (result == null)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                    result[i, j] = mapping(a[i, j]);
        }

        #endregion ArrayOperations


        #region MatrixProducts


        // MATRIX * MATRIX

        /// <summary>R=A*B. 
        /// Multiplies two matrices and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyPlain(IMatrix a, IMatrix b, IMatrix result)
        {
            int i, j, k;
            double element;
            if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication can not be done in place.");
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < b.ColumnCount; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < a.ColumnCount; ++k)
                        element += a[i, k] * b[k, j];
                    result[i, j] = element;
                }
        }

        /// <summary>R=A*B. 
        /// Multiplies two matrices and stores the result in the specified result matrix.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void Multiply(IMatrix a, IMatrix b, IMatrix result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix multiplication is not specified (null reference).");
            else if (a.ColumnCount != b.RowCount 
                || result.RowCount != a.RowCount || result.ColumnCount != b.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j, k;
            double element;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < b.ColumnCount; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < a.ColumnCount; ++k)
                        element += a[i, k] * b[k, j];
                    result[i, j] = element;
                }
        }

        /// <summary>R=A*B. 
        /// Multiplies two matrices and stores the result in the specified result matrix.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void Multiply(IMatrix a, IMatrix b, ref IMatrix result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication can not be done in place.");
            else if (a.ColumnCount != b.RowCount)
                throw new ArgumentException("Dimension mismatch at matrix  multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.RowCount, b.ColumnCount);
            else if (result.RowCount != a.RowCount || result.ColumnCount != b.ColumnCount)
                result = a.GetNew(a.RowCount, b.ColumnCount);
            int i, j, k;
            double element;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < b.ColumnCount; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < a.ColumnCount; ++k)
                        element += a[i, k] * b[k, j];
                    result[i, j] = element;
                }
        }



        // TRANSPOSED MATRIX * MATRIX

        /// <summary>R=A^T*B
        /// Calculates product of transposed matrix and a matrix, and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyTranspMatPlain(IMatrix a, IMatrix b, IMatrix result)
        {
            int i, j, k;
            double element;
            if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Transposed matrix by matrix multiplication can not be done in place.");
            for (i = 0; i < a.ColumnCount; ++i)  // first matrix transposed!
                for (j = 0; j < b.ColumnCount; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < a.RowCount; ++k)  // first matrix transposed!
                        element += a[k, i] * b[k, j];  // first matrix transposed!
                    result[i, j] = element;
                }
        }

        /// <summary>R=A^T*B
        /// Multiplies two matrices and stores the result in the specified result matrix.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyTranspMat(IMatrix a, IMatrix b, IMatrix result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Transposed matrix by matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Transposed matrix by matrix  multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Transposed matrix by matrix multiplication: result storage for matrix multiplication is not specified (null reference).");
            else if (a.RowCount != b.RowCount  // first matrix transposed!
                || result.RowCount != a.ColumnCount || result.ColumnCount != b.ColumnCount)
                throw new ArgumentException("Dimension mismatch at transposed matrix by matrix  multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j, k;
            double element;
            for (i = 0; i < a.ColumnCount; ++i)  // first matrix transposed!
                for (j = 0; j < b.ColumnCount; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < a.RowCount; ++k)  // first matrix transposed!
                        element += a[k, i] * b[k, j];  // first matrix transposed!
                    result[i, j] = element;
                }
        }

        /// <summary>R=A^T*B
        /// Multiplies two matrices and stores the result in the specified result matrix.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyTranspMat(IMatrix a, IMatrix b, ref IMatrix result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Transposed matrix by matrix  multiplication: operand not specified (null reference).");
            else if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Transposed matrix by matrix  multiplication can not be done in place.");
            else if (a.RowCount != b.RowCount)  // first matrix transposed!
                throw new ArgumentException("Dimension mismatch at transposed matrix by matrix  multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.ColumnCount, b.ColumnCount);  // first matrix transposed!
            else if (result.RowCount != a.ColumnCount || result.ColumnCount != b.ColumnCount)  // first matrix transposed!
                result = a.GetNew(a.ColumnCount, b.ColumnCount);  // first matrix transposed!
            int i, j, k;
            double element;
            for (i = 0; i < a.ColumnCount; ++i)  // first matrix transposed!
                for (j = 0; j < b.ColumnCount; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < a.RowCount; ++k)  // first matrix transposed!
                        element += a[k, i] * b[k, j];  // first matrix transposed!
                    result[i, j] = element;
                }
        }

        /// <summary>Tests product A^T*B.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestMultiplyTranspMat(double tolerance, bool printReports)
        {
            int d1 = 2, d2 = 4, d3 = 3;
            IMatrix result = null, referenceResult = null;
            IMatrix A = new Matrix(d2, d1); A.SetRandom();
            IMatrix B = new Matrix(d2, d3); B.SetRandom();
            IMatrix A_T = null; Matrix.Transpose(A, ref A_T);
            MultiplyTranspMat(A, B, ref result);
            Multiply(A_T, B, ref referenceResult);
            bool ret = CheckTestResult(result, referenceResult, tolerance, printReports);
            return ret;
        }



        // MATRIX * TRANSPOSED MATRIX

        /// <summary>R=A*B^T. 
        /// Multiplies a matrix by transpose of another matrix and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyMatTranspPlain(IMatrix a, IMatrix b, IMatrix result)
        {
            int i, j, k;
            double element;
            if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Matrix by transposed matrix  multiplication can not be done in place.");
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < b.RowCount; ++j)  // second matrix transposed!
                {
                    element = 0.0;
                    for (k = 0; k < a.ColumnCount; ++k)
                        element += a[i, k] * b[j, k];  // second matrix transposed!
                    result[i, j] = element;
                }
        }

        /// <summary>R=A*B^T. 
        /// Multiplies a matrix by transpose of another matrix and stores the result in the specified result matrix.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyMatTransp(IMatrix a, IMatrix b, IMatrix result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Matrix by transposed matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Matrix by transposed matrix multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix by transposed matrix multiplication is not specified (null reference).");
            else if (a.ColumnCount != b.ColumnCount  // second matrix transposed!
                || result.RowCount != a.RowCount || result.ColumnCount != b.RowCount)
                throw new ArgumentException("Dimension mismatch at matrix by transposed matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j, k;
            double element;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < b.RowCount; ++j)  // second matrix transposed!
                {
                    element = 0.0;
                    for (k = 0; k < a.ColumnCount; ++k)
                        element += a[i, k] * b[j, k];  // second matrix transposed!
                    result[i, j] = element;
                }
        }

        /// <summary>R=A*B^T. 
        /// Multiplies a matrix by transpose of another matrix and stores the result in the specified result matrix.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyMatTransp(IMatrix a, IMatrix b, ref IMatrix result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Matrix by transposed matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Matrix by transposed matrix multiplication can not be done in place.");
            else if (a.ColumnCount != b.ColumnCount)  // second matrix transposed!
                throw new ArgumentException("Dimension mismatch at matrix by transposed matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.RowCount, b.RowCount);  // second matrix transposed!
            else if (result.RowCount != a.RowCount || result.ColumnCount != b.RowCount)  // second matrix transposed!
                result = a.GetNew(a.RowCount, b.RowCount);
            int i, j, k;
            double element;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < b.RowCount; ++j)  // second matrix transposed!
                {
                    element = 0.0;
                    for (k = 0; k < a.ColumnCount; ++k)
                        element += a[i, k] * b[j, k];  // second matrix transposed!
                    result[i, j] = element;
                }
        }

        /// <summary>Tests product A*B^T.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestMultiplyMatTransp(double tolerance, bool printReports)
        {
            int d1 = 2, d2 = 4, d3 = 3;
            IMatrix result = null, referenceResult = null;
            IMatrix A = new Matrix(d1, d2);  A.SetRandom();
            IMatrix B = new Matrix(d3, d2); B.SetRandom();
            IMatrix B_T = null; Matrix.Transpose(B, ref B_T);
            MultiplyMatTransp(A, B, ref result);
            Multiply(A, B_T, ref referenceResult);
            bool ret = CheckTestResult(result, referenceResult, tolerance, printReports);
            return ret;
        }



        // TRANSPOSED MATRIX * TRANSPOSED MATRIX

        /// <summary>R=A^T*B^T. 
        /// Multiplies transposed matrix by another transposed matrix, and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyTranspTranspPlain(IMatrix a, IMatrix b, IMatrix result)
        {
            int i, j, k;
            double element;
            if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Transposed matrix by transposed matrix multiplication can not be done in place.");
            for (i = 0; i < a.ColumnCount; ++i)  // both matrices transposed!
                for (j = 0; j < b.RowCount; ++j)  // both matrices transposed!
                {
                    element = 0.0;
                    for (k = 0; k < a.RowCount; ++k)  // both matrices transposed!
                        element += a[k, i] * b[j, k];  // both matrices transposed!
                    result[i, j] = element;
                }
        }

        /// <summary>R=A^T*B^T. 
        /// Multiplies transposed matrix by another transposed matrix, and stores the result in the specified result matrix.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyTranspTransp(IMatrix a, IMatrix b, IMatrix result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Transposed matrix by transposed matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Transposed matrix by transposed matrix multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for transposed matrix by transposed matrix multiplication is not specified (null reference).");
            else if (a.RowCount != b.ColumnCount  // both matrices transposed!
                || result.RowCount != a.ColumnCount || result.ColumnCount != b.RowCount)  // both matrices transposed!
                throw new ArgumentException("Dimension mismatch at transposed matrix by transposed matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j, k;
            double element;
            for (i = 0; i < a.ColumnCount; ++i)  // both matrices transposed!
                for (j = 0; j < b.RowCount; ++j)  // both matrices transposed!
                {
                    element = 0.0;
                    for (k = 0; k < a.RowCount; ++k)  // both matrices transposed!
                        element += a[k, i] * b[j, k];  // both matrices transposed!
                    result[i, j] = element;
                }
        }

        /// <summary>R=A^T*B^T. 
        /// Multiplies transposed matrix by another transposed matrix, and stores the result in the specified result matrix.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyTranspTransp(IMatrix a, IMatrix b, ref IMatrix result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Transposed matrix by transposed matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result)
                throw new ArgumentException("Result the same as operand. Transposed matrix by transposed matrix multiplication can not be done in place.");
            else if (a.RowCount != b.ColumnCount)  // both matrices transposed!
                throw new ArgumentException("Dimension mismatch at transposed matrix by transposed matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.ColumnCount, b.RowCount);  // both matrices transposed!
            else if (result.RowCount != a.ColumnCount || result.ColumnCount != b.RowCount)  // both matrices transposed!
                result = a.GetNew(a.ColumnCount, b.RowCount);  // both matrices transposed!
            int i, j, k;
            double element;
            for (i = 0; i < a.ColumnCount; ++i)  // both matrices transposed!
                for (j = 0; j < b.RowCount; ++j)  // both matrices transposed!
                {
                    element = 0.0;
                    for (k = 0; k < a.RowCount; ++k)  // both matrices transposed!
                        element += a[k, i] * b[j, k];  // both matrices transposed!
                    result[i, j] = element;
                }
        }

        /// <summary>Tests product A^T*B^T.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestMultiplyTranspTransp(double tolerance, bool printReports)
        {
            int d1 = 2, d2 = 4, d3 = 3;
            IMatrix result = null, referenceResult = null;
            IMatrix A = new Matrix(d2, d1); A.SetRandom();
            IMatrix B = new Matrix(d3, d2); B.SetRandom();
            IMatrix A_T = null; Matrix.Transpose(A, ref A_T);
            IMatrix B_T = null; Matrix.Transpose(B, ref B_T);
            MultiplyTranspTransp(A, B, ref result);
            Multiply(A_T, B_T, ref referenceResult);
            bool ret = CheckTestResult(result, referenceResult, tolerance, printReports);
            return ret;
        }


        // MATRIX * MATRIX * MATRIX

        /// <summary>R=A*B*C. 
        /// Multiplies three matrices and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyPlain(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            // Store dimensions of involved matrix factors:
            int 
                m = 
                a.RowCount,
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.ColumnCount;
            if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication can not be done in place.");
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[l, j];
                        element += a[i, k] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>R=A*B*C. 
        /// Multiplies three matrices and stores the result in the specified result matrix.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void Multiply(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            if (a == null || b == null || c==null)
                throw new ArgumentNullException("Tripple matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result || c==result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for tripple matrix multiplication is not specified (null reference).");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.RowCount,
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.ColumnCount;
            if (a.ColumnCount!=n || c.RowCount!=o || result.RowCount!=m || result.ColumnCount!=p)
                throw new ArgumentException("Dimension mismatch at tripple matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[l, j];
                        element += a[i, k] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>R=A*B*C. 
        /// Multiplies three matrices and stores the result in the specified result matrix.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void Multiply(IMatrix a, IMatrix b, IMatrix c, ref IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Tripple matrix multiplication can not be done in place.");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.RowCount,
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.ColumnCount;
            if (a.ColumnCount != n || c.RowCount != o)
                throw new ArgumentException("Dimension mismatch at tripple matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount + ".");
            if (result == null)
                result=a.GetNew(m,p);
            else if (result.RowCount != m || result.ColumnCount != p)
                result=a.GetNew(m,p);
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[l, j];
                        element += a[i, k] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>Tests product A*B*C.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestMultiply3(double tolerance, bool printReports)
        {
            int d1 = 2, d2 = 4, d3 = 3, d4 = 5;
            IMatrix result = null, referenceResult = null;
            IMatrix A = new Matrix(d1, d2); A.SetRandom();
            IMatrix B = new Matrix(d2, d3); B.SetRandom();
            IMatrix C = new Matrix(d3, d4); C.SetRandom();
            Multiply(A, B, C, ref result);
            IMatrix AB = null;
            Multiply(A, B, ref AB);
            Multiply(AB, C, ref referenceResult);
            bool ret = CheckTestResult(result, referenceResult, tolerance, printReports);
            return ret;
        }


        // TRANSPOSED MATRIX * MATRIX * TRANSPOSED MATRIX

        /// <summary>R=A^T*B*C^T. 
        /// Multiplies three matrices (transposed first argument, second argument, and transposed third argument) 
        /// and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of all arguments must be consistent, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand, transpose of the first matrix factor.</param>
        /// <param name="b">Second operand, the second matrix factor.</param>
        /// <param name="c">Third operand, transpose of the third matrix factor.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must be consistent with dimensions of operands.</param>
        public static void MultiplyTranspMatTranspPlain(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Tripple matrix multiplication (A^T*B*C^T) can not be done in place.");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.ColumnCount,  // transpose!
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.RowCount;  // transpose!
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[j, l];
                        element += a[k, i] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>R=A^T*B*C^T. 
        /// Multiplies three matrices (transposed first argument, second argument, and transposed third argument) 
        /// and stores the result in the specified result matrix.
        /// WARNING: dimensions of all arguments must be consistent, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyTranspMatTransp(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix multiplication (A^T*B*C^T): operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Tripple matrix (A^T*B*C^T) multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for tripple matrix multiplication (A^T*B*C^T) is not specified (null reference).");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.ColumnCount,  // transpose!
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.RowCount;  // transpose!
            if (a.RowCount != n || c.ColumnCount != o   // transposed a and b!
                || result.RowCount != m || result.ColumnCount != p)
                throw new ArgumentException("Dimension mismatch at tripple matrix multiplication (A^T*B*C^T). " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[j, l];
                        element += a[k, i] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>R=A^T*B*C^T. 
        /// Multiplies three matrices (transposed first argument, second argument, and transposed third argument) 
        /// and stores the result in the specified result matrix.
        /// WARNING: dimensions of operands must be consistent, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where result is stored. Allocated or reallocated if necessary.</param>
        public static void MultiplyTranspMatTransp(IMatrix a, IMatrix b, IMatrix c, ref IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix multiplication (A^T*B*C^T): operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Tripple matrix multiplication (A^T*B*C^T) can not be done in place.");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.ColumnCount,  // transpose!
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.RowCount;  // transpose!
            if (a.RowCount != n || c.ColumnCount != o)   // transposed a and b!
                throw new ArgumentException("Dimension mismatch at tripple matrix multiplication (A^T*B*C^T). " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount + ".");
            if (result == null)
                result = a.GetNew(m, p);
            else if (result.RowCount != m || result.ColumnCount != p)
                result = a.GetNew(m, p);
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[j, l];
                        element += a[k, i] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>Tests product A^T*B*C^T.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestMultiplyTranspMatTransp(double tolerance, bool printReports)
        {
            int d1 = 2, d2 = 4, d3 = 3, d4 = 5;
            IMatrix result = null, referenceResult = null;
            IMatrix A = new Matrix(d2, d1); A.SetRandom();
            IMatrix B = new Matrix(d2, d3); B.SetRandom();
            IMatrix C = new Matrix(d4, d3); C.SetRandom();
            MultiplyTranspMatTransp(A, B, C, ref result);
            IMatrix A_T = null;  Matrix.Transpose(A, ref A_T);
            IMatrix C_T = null;  Matrix.Transpose(C, ref C_T);
            IMatrix prod = null;
            Multiply(A_T, B, ref prod);
            Multiply(prod, C_T, ref referenceResult);
            bool ret = CheckTestResult(result, referenceResult, tolerance, printReports);
            return ret;
        }


        // TRANSPOSED MATRIX * MATRIX * MATRIX

        /// <summary>R=A^T*B*C. 
        /// Multiplies three matrices (transposed first argument, second argument, and third argument) 
        /// and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of all arguments must be consistent, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand, transpose of the first matrix factor.</param>
        /// <param name="b">Second operand, the second matrix factor.</param>
        /// <param name="c">Third operand, transpose of the third matrix factor.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must be consistent with dimensions of operands.</param>
        public static void MultiplyTranspMatMatPlain(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Tripple matrix multiplication (A^T*B*C) can not be done in place.");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.ColumnCount,  // transpose!
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.ColumnCount;  // transpose!
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[l, j];
                        element += a[k, i] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>R=A^T*B*C. 
        /// Multiplies three matrices (transposed first argument, second argument, and third argument) 
        /// and stores the result in the specified result matrix.
        /// WARNING: dimensions of all arguments must be consistent, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyTranspMatMat(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix multiplication (A^T*B*C): operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Tripple matrix (A^T*B*C) multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for tripple matrix multiplication (A^T*B*C) is not specified (null reference).");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.ColumnCount,  // transpose!
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.ColumnCount;  // transpose!
            if (a.RowCount != n || c.RowCount != o   // transposed a and b!
                || result.RowCount != m || result.ColumnCount != p)
                throw new ArgumentException("Dimension mismatch at tripple matrix multiplication (A^T*B*C). " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[l, j];
                        element += a[k, i] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>R=A^T*B*C. 
        /// Multiplies three matrices (transposed first argument, second argument, and third argument) 
        /// and stores the result in the specified result matrix.
        /// WARNING: dimensions of operands must be consistent, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where result is stored. Allocated or reallocated if necessary.</param>
        public static void MultiplyTranspMatMat(IMatrix a, IMatrix b, IMatrix c, ref IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix multiplication (A^T*B*C): operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Tripple matrix multiplication (A^T*B*C) can not be done in place.");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.ColumnCount,  // transpose!
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.ColumnCount;  // transpose!
            if (a.RowCount != n || c.RowCount != o)   // transposed a and b!
                throw new ArgumentException("Dimension mismatch at tripple matrix multiplication (A^T*B*C). " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount + ".");
            if (result == null)
                result = a.GetNew(m, p);
            else if (result.RowCount != m || result.ColumnCount != p)
                result = a.GetNew(m, p);
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[l, j];
                        element += a[k, i] * element_bc;
                    }
                    result[i, j] = element;
                }
        }


        /// <summary>Tests product A^T*B*C.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestMultiplyTranspMatMat(double tolerance, bool printReports)
        {
            int d1 = 2, d2 = 4, d3 = 3, d4 = 5;
            IMatrix result = null, referenceResult = null;
            IMatrix A = new Matrix(d2, d1); A.SetRandom();
            IMatrix B = new Matrix(d2, d3); B.SetRandom();
            IMatrix C = new Matrix(d3, d4); C.SetRandom();
            MultiplyTranspMatMat(A, B, C, ref result);
            IMatrix A_T = null; Matrix.Transpose(A, ref A_T);
            IMatrix prod = null;
            Multiply(A_T, B, ref prod);
            Multiply(prod, C, ref referenceResult);
            bool ret = CheckTestResult(result, referenceResult, tolerance, printReports);
            return ret;
        }


        // MATRIX * MATRIX * TRANSPOSED MATRIX

        /// <summary>R=A*B*C^T. 
        /// Multiplies three matrices (first argumet, second argumet, and transposed third argument) 
        /// and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyMatMatTranspPlain(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            // Store dimensions of involved matrix factors:
            int
                m = a.RowCount,
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.RowCount;
            if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication can not be done in place.");
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[j, l];
                        element += a[i, k] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>R=A*B*C^T. 
        /// Multiplies three matrices (first argumet, second argumet, and transposed third argument) 
        /// and stores the result in the specified result matrix.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyMatMatTransp(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix-matrix-transpose multiplication: operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for tripple matrix-matrix-transpose multiplication is not specified (null reference).");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.RowCount,
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.RowCount;
            if (a.ColumnCount != n || c.ColumnCount != o || result.RowCount != m || result.ColumnCount != p)
                throw new ArgumentException("Dimension mismatch at tripple matrix-matrix-transpose multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[j, l];
                        element += a[i, k] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>R=A*B*C^T. 
        /// Multiplies three matrices (first argumet, second argumet, and transposed third argument) 
        /// and stores the result in the specified result matrix.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyMatMatTransp(IMatrix a, IMatrix b, IMatrix c, ref IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Tripple matrix-matrix-transpose multiplication can not be done in place.");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.RowCount,
                n = b.RowCount,
                o = b.ColumnCount,
                p = c.RowCount;
            if (a.ColumnCount != n || c.ColumnCount != o)
                throw new ArgumentException("Dimension mismatch at tripple matrix-matrix-transpose multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount + ".");
            if (result == null)
                result = a.GetNew(m, p);
            else if (result.RowCount != m || result.ColumnCount != p)
                result = a.GetNew(m, p);
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[k, l] * c[j, l];
                        element += a[i, k] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>Tests product A*B*C^T.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestMultiplyMatMatTransp(double tolerance, bool printReports)
        {
            int d1 = 2, d2 = 4, d3 = 3, d4 = 5;
            IMatrix result = null, referenceResult = null;
            IMatrix A = new Matrix(d1, d2); A.SetRandom();
            IMatrix B = new Matrix(d2, d3); B.SetRandom();
            IMatrix C = new Matrix(d4, d3); C.SetRandom();
            MultiplyMatMatTransp(A, B, C, ref result);
            IMatrix C_T = null;  Transpose(C, ref C_T);
            IMatrix AB = null; 
            Multiply(A, B, ref AB);
            Multiply(AB, C_T, ref referenceResult);
            bool ret = CheckTestResult(result, referenceResult, tolerance, printReports);
            return ret;
        }

        // MATRIX * TRANSPOSED MATRIX * MATRIX

        /// <summary>R=A*B^T*C. 
        /// Multiplies three matrices (first argument, transposed second argument, and third argument) 
        /// and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyMatTranspMatPlain(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            // Store dimensions of involved matrix factors:
            int
                m =
                a.RowCount,
                n = b.ColumnCount,
                o = b.RowCount,
                p = c.ColumnCount;
            if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication can not be done in place.");
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[l, k] * c[l, j];
                        element += a[i, k] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>R=A*B^T*C. 
        /// Multiplies three matrices (first argument, transposed second argument, and third argument) 
        /// and stores the result in the specified result matrix.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyMatTranspMat(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for tripple matrix multiplication is not specified (null reference).");
            // Store dimensions of involved matrix factors:
            int
                m = a.RowCount,
                n = b.ColumnCount,
                o = b.RowCount,
                p = c.ColumnCount;
            if (a.ColumnCount != n || c.RowCount != o || result.RowCount != m || result.ColumnCount != p)
                throw new ArgumentException("Dimension mismatch at tripple matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[l, k] * c[l, j];
                        element += a[i, k] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>R=A*B^T*C. 
        /// Multiplies three matrices (first argument, transposed second argument, and third argument) 
        /// and stores the result in the specified result matrix.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyMatTranspMat(IMatrix a, IMatrix b, IMatrix c, ref IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix multiplication: operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Tripple matrix multiplication can not be done in place.");
            // Store dimensions of involved matrix factors:
            int
                m =
                a.RowCount,
                n = b.ColumnCount,
                o = b.RowCount,
                p = c.ColumnCount;
            if (a.ColumnCount != n || c.RowCount != o)
                throw new ArgumentException("Dimension mismatch at tripple matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount + ".");
            if (result == null)
                result = a.GetNew(m, p);
            else if (result.RowCount != m || result.ColumnCount != p)
                result = a.GetNew(m, p);
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[l, k] * c[l, j];
                        element += a[i, k] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>Tests product A*B^T*C.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestMultiplyMatTranspMat(double tolerance, bool printReports)
        {
            int d1 = 2, d2 = 4, d3 = 3, d4 = 5;
            IMatrix result = null, referenceResult = null;
            IMatrix A = new Matrix(d1, d2); A.SetRandom();
            IMatrix B = new Matrix(d3, d2); B.SetRandom();
            IMatrix C = new Matrix(d3, d4); C.SetRandom();
            MultiplyMatTranspMat(A, B, C, ref result);
            IMatrix B_T = null;  Transpose(B, ref B_T);
            IMatrix ABT = null;
            Multiply(A, B_T, ref ABT);
            Multiply(ABT, C, ref referenceResult);
            bool ret = CheckTestResult(result, referenceResult, tolerance, printReports);
            return ret;
        }


        // TRANSPOSED MATRIX * TRANSPOSED MATRIX * TRANSPOSED MATRIX

        /// <summary>R=A^T*B^T*C^T. 
        /// Multiplies three matrices and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyTranspTranspTranspPlain(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            // Store dimensions of involved matrix factors:
            int
                m = a.ColumnCount,
                n = b.ColumnCount,
                o = b.RowCount,
                p = c.RowCount;
            if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication (A^T*B^T*C^T) can not be done in place.");
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[l, k] * c[j, l];
                        element += a[k, i] * element_bc;
                    }
                    result[i, j] = element;
                }
        }


        /// <summary>R=A^T*B^T*C^T. 
        /// Multiplies three matrices and stores the result in the specified result matrix.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyTranspTranspTransp(IMatrix a, IMatrix b, IMatrix c, IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix multiplication (A^T*B^T*C^T): operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Matrix multiplication (A^T*B^T*C^T) can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for tripple matrix multiplication (A^T*B^T*C^T) is not specified (null reference).");
            // Store dimensions of involved matrix factors:
            int
                m = a.ColumnCount,
                n = b.ColumnCount,
                o = b.RowCount,
                p = c.RowCount;
            if (a.RowCount != n || c.ColumnCount != o || result.RowCount != m || result.ColumnCount != p)
                throw new ArgumentException("Dimension mismatch at tripple matrix multiplication (A^T*B^T*C^T). " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[l, k] * c[j, l];
                        element += a[k, i] * element_bc;
                    }
                    result[i, j] = element;
                }
        }


        /// <summary>R=A^T*B^T*C^T. 
        /// Multiplies three matrices and stores the result in the specified result matrix.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// WARNING: This operation can not be performed in place.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="c">Third operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyTranspTranspTransp(IMatrix a, IMatrix b, IMatrix c, ref IMatrix result)
        {
            if (a == null || b == null || c == null)
                throw new ArgumentNullException("Tripple matrix multiplication (A^T*B^T*C^T): operand not specified (null reference).");
            else if (a == result || b == result || c == result)
                throw new ArgumentException("Result the same as operand. Tripple matrix multiplication (A^T*B^T*C^T) can not be done in place.");
            // Store dimensions of involved matrix factors:
            int
                m = a.ColumnCount,
                n = b.ColumnCount,
                o = b.RowCount,
                p = c.RowCount;
            if (a.RowCount != n || c.ColumnCount != o)
                throw new ArgumentException("Dimension mismatch at tripple matrix multiplication (A^T*B^T*C^T). " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + b.RowCount + "x" + b.ColumnCount
                    + ",  third operand: " + c.RowCount + "x" + c.ColumnCount + ".");
            if (result == null)
                result = a.GetNew(m, p);
            else if (result.RowCount != m || result.ColumnCount != p)
                result = a.GetNew(m, p);
            int i, j, k, l;
            double element, element_bc;
            for (i = 0; i < m; ++i)
                for (j = 0; j < p; ++j)
                {
                    element = 0.0;
                    for (k = 0; k < n; ++k)
                    {
                        element_bc = 0.0;
                        for (l = 0; l < o; ++l)
                            element_bc += b[l, k] * c[j, l];
                        element += a[k, i] * element_bc;
                    }
                    result[i, j] = element;
                }
        }

        /// <summary>Tests product A^T*B^T*C^T.</summary>
        /// <param name="tolerance">Tolerance (on norm of the difference) for the test to pass</param>
        /// <param name="printReports">Specifies whether to print reports or not.</param>
        /// <returns>true if the test passes, false if not.</returns>
        private static bool TestMultiplyTranspTranspTransp(double tolerance, bool printReports)
        {
            int d1 = 2, d2 = 4, d3 = 3, d4 = 5;
            IMatrix result = null, referenceResult = null;
            IMatrix A = new Matrix(d2, d1); A.SetRandom();
            IMatrix B = new Matrix(d3, d2); B.SetRandom();
            IMatrix C = new Matrix(d4, d3); C.SetRandom();
            MultiplyTranspTranspTransp(A, B, C, ref result);
            IMatrix A_T = null; Transpose(A, ref A_T);
            IMatrix B_T = null; Transpose(B, ref B_T);
            IMatrix C_T = null; Transpose(C, ref C_T);
            IMatrix ATBT = null;
            Multiply(A_T, B_T, ref ATBT);
            Multiply(ATBT, C_T, ref referenceResult);
            bool ret = CheckTestResult(result, referenceResult, tolerance, printReports);
            return ret;
        }





        #endregion MatrixProducts


        #region MatrixVectorProducts

        // MATRIX * VECTOR

        /// <summary>R=A*b. 
        /// Multiplies a matrix with a vector and stores the result in the specified result vector.
        /// Operation can not be performed in place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Vector where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyPlain(IMatrix a, IVector b, IVector result)
        {
            int i, k;
            if (b == result)
                throw new ArgumentException("Result the same as operand. Matrix-vector multiplication can not be done in place.");
            double element;
            for (i = 0; i < a.RowCount; ++i)
            {
                element = 0.0;
                for (k = 0; k < a.ColumnCount; ++k)
                    element += a[i, k] * b[k];
                result[i] = element;
            }
        }

        /// <summary>R=A*b. 
        /// Multiplies a matrix with a vector and stores the result in the specified result vector.
        /// Operation can not be performed in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Vector where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void Multiply(IMatrix a, IVector b, IVector result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Matrix by vector multiplication: operand not specified (null reference).");
            else if (b == result)
                throw new ArgumentException("Result the same as operand. Matrix-vector multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix multiplication is not specified (null reference).");
            else if (a.ColumnCount != b.Length
                || result.Length != a.RowCount)
                throw new ArgumentException("Dimension mismatch at matrix by vector multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand dim.: " + b.Length 
                    + ", result dim: " + result.Length + ".");
            int i, k;
            double element;
            for (i = 0; i < a.RowCount; ++i)
                {
                    element = 0.0;
                    for (k = 0; k < a.ColumnCount; ++k)
                        element += a[i, k] * b[k];
                    result[i] = element;
                }
        }

        /// <summary>R=A*b. 
        /// Multiplies matrix by scalar and stores the result in the specified result matrix.
        /// Operation can not be performed in place.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void Multiply(IMatrix a, IVector b, ref IVector result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Matrix by vector multiplication: operand not specified (null reference).");
            else if (b == result)
                throw new ArgumentException("Result the same as operand. Matrix-vector multiplication can not be done in place.");
            else if (a.ColumnCount != b.Length)
                throw new ArgumentException("Dimension mismatch at matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand dim.: " + b.Length + ".");
            else if (result == null)
                result = a.GetNewVector(a.RowCount);
            else if (result.Length != a.RowCount)
                result = a.GetNewVector(a.RowCount);
            int i, k;
            double element;
            for (i = 0; i < a.RowCount; ++i)
            {
                element = 0.0;
                for (k = 0; k < a.ColumnCount; ++k)
                    element += a[i, k] * b[k];
                result[i] = element;
            }
        }

        // TRANSPOSED MATRIX * VECTOR

        /// <summary>R=A^T*b. 
        /// Multiplies a transposed matrix with a vector and stores the result in the specified result vector.
        /// Operation can not be performed in place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Vector where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyTranspVecPlain(IMatrix a, IVector b, IVector result)
        {
            int i, k;
            if (b == result)
                throw new ArgumentException("Result the same as operand. Matrix-vector multiplication can not be done in place.");
            double element;
            for (i = 0; i < a.ColumnCount; ++i)
            {
                element = 0.0;
                for (k = 0; k < a.RowCount; ++k)
                    element += a[k, i] * b[k];
                result[i] = element;
            }
        }

        /// <summary>R=A^T*b. 
        /// Multiplies a transposed matrix with a vector and stores the result in the specified result vector.
        /// Operation can not be performed in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Vector where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyTranspVec(IMatrix a, IVector b, IVector result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Matrix by vector multiplication: operand not specified (null reference).");
            else if (b == result)
                throw new ArgumentException("Result the same as operand. Matrix-vector multiplication can not be done in place.");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix multiplication is not specified (null reference).");
            else if (a.RowCount != b.Length
                || result.Length != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix by vector multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand dim.: " + b.Length 
                    + ", result dim: " + result.Length + ".");
            int i, k;
            double element;
            for (i = 0; i < a.ColumnCount; ++i)
            {
                element = 0.0;
                for (k = 0; k < a.RowCount; ++k)
                    element += a[k, i] * b[k];
                result[i] = element;
            }
        }

        /// <summary>R=A^T*b. 
        /// Multiplies transposed matrix by scalar and stores the result in the specified result matrix.
        /// Operation can not be performed in place.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyTranspVec(IMatrix a, IVector b, ref IVector result)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Matrix by vector multiplication: operand not specified (null reference).");
            else if (b == result)
                throw new ArgumentException("Result the same as operand. Matrix-vector multiplication can not be done in place.");
            else if (a.ColumnCount != b.Length)
                throw new ArgumentException("Dimension mismatch at matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand dim.: " + b.Length + ".");
            else if (result == null)
                result = a.GetNewVector(a.ColumnCount);
            else if (result.Length != a.ColumnCount)
                result = a.GetNewVector(a.ColumnCount);
            int i, k;
            double element;
            for (i = 0; i < a.ColumnCount; ++i)
            {
                element = 0.0;
                for (k = 0; k < a.RowCount; ++k)
                    element += a[k, i] * b[k];
                result[i] = element;
            }
        }

        // TRANSPOSED VECTOR * MATRIX * VECTOR

        /// <summary>R=a^T*M*b. 
        /// Left-multiplies a matrix with a vector (transposed), right multiplies the result vith another vector, 
        /// and returns the final result.
        /// Can be performed in place.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="left">Left vector factor (transpose vector as one row matrix).</param>
        /// <param name="a">Matrix factor.</param>
        /// <param name="right">Right factor (vector as one column matrix).</param>
        public static double MultiplyPlain(IVector left, IMatrix a, IVector right)
        {
            int i, j;
            double ret = 0.0;
            for (i = 0; i < a.RowCount; ++i)
            {
                double rightElement = 0.0;
                for (j = 0; j < a.ColumnCount; ++j)
                    rightElement += a[i, j] * right[j];
                ret += left[i]*rightElement;
            }
            return ret;
        }

        /// <summary>R=a^T*M*b. 
        /// Left-multiplies a matrix with a vector (transposed), right multiplies the result vith another vector, 
        /// and returns the final result.
        /// Can be performed in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="left">Left factor (vector as one row matrix).</param>
        /// <param name="a">Matrix factor.</param>
        /// <param name="right">Right factor (vector as one column matrix).</param>
        public static double Multiply(IVector left, IMatrix a, IVector right)
        {
            if (a == null || left==null || right == null)
                throw new ArgumentNullException("Vector-matrix-vector multiplication: operand not specified (null reference).");
            else if (a.RowCount!=left.Length ||  a.ColumnCount != right.Length)
                throw new ArgumentException("Dimension mismatch at matrix by vector multiplication. " + Environment.NewLine
                    + "  Left vector operand: " + left.Length
                    + ", Matrix operand: " + a.RowCount + "x" + a.ColumnCount
                    + ", Right vector operand: " + right.Length + ".");
            int i, j;
            double ret = 0.0;
            for (i = 0; i < a.RowCount; ++i)
            {
                double rightElement = 0.0;
                for (j = 0; j < a.ColumnCount; ++j)
                    rightElement += a[i, j] * right[j];
                ret += left[i] * rightElement;
            }
            return ret;
        }

        /// <summary>R=a^T*M*b. 
        /// Left-multiplies a matrix with a vector (transposed), right multiplies the result vith another vector, 
        /// and returns the final result.
        /// Can be performed in place.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="left">Left factor (vector as one row matrix).</param>
        /// <param name="a">Matrix factor.</param>
        /// <param name="right">Right factor (vector as one column matrix).</param>
        /// <param name="result">Vector where result of operation is stored. Reallocated if necessary.</param>
        public static double Multiply(IVector left, IMatrix a, IVector right, ref IVector result)
        {
            if (a == null || left == null || right == null)
                throw new ArgumentNullException("Vector-matrix-vector multiplication: operand not specified (null reference).");
            else if (a.RowCount != left.Length || a.ColumnCount != right.Length)
                throw new ArgumentException("Dimension mismatch at matrix by vector multiplication. " + Environment.NewLine
                    + "  Left vector operand: " + left.Length
                    + ", Matrix operand: " + a.RowCount + "x" + a.ColumnCount
                    + ", Right vector operand: " + right.Length + ".");
            int i, j;
            double ret = 0.0;
            for (i = 0; i < a.RowCount; ++i)
            {
                double rightElement = 0.0;
                for (j = 0; j < a.ColumnCount; ++j)
                    rightElement += a[i, j] * right[j];
                ret += left[i] * rightElement;
            }
            return ret;
        }


        #endregion MatrixVectorProducts


        #region MatrixScalarProducts


        /// <summary>Multiplies matrix by scalar and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// This operation can be performed in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyPlain(IMatrix a, double b, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i,j]*b;
                }
        }

        /// <summary>Multiplies matrix by scalar and stores the result in the specified result matrix.
        /// This operation can be performed in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void Multiply(IMatrix a, double b, IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix-scalar multiplication: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix-scalar multiplication is not specified (null reference).");
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix-scalar multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: scalar"
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i,j]*b;
                }
       }

        /// <summary>Multiplies matrix by scalar and stores the result in the specified result matrix.
        /// This operation can be performed in place.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void Multiply(IMatrix a, double b, ref IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix-scalar multiplication: operand not specified (null reference).");
            else if (result == null)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i,j]*b;
                }
        }




        /// <summary>Divides matrix by scalar and stores the result in the specified result matrix.
        /// This is a plain version of the method that does not perform any consistency checks.
        /// This operation can be performed in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void DividePlain(IMatrix a, double b, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] * b;
                }
        }

        /// <summary>Divides matrix by scalar and stores the result in the specified result matrix.
        /// This operation can be performed in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void Divide(IMatrix a, double b, IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix-scalar multiplication: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix-scalar multiplication is not specified (null reference).");
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix-scalar multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: scalar"
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] * b;
                }
        }

        /// <summary>Divides matrix by scalar and stores the result in the specified result matrix.
        /// This operation can be performed in place.
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="b">Second operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void Divide(IMatrix a, double b, ref IMatrix result)
        {
            if (a == null)
                throw new ArgumentNullException("Matrix-scalar multiplication: operand not specified (null reference).");
            else if (result == null)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] * b;
                }
        }

        #endregion MatrixScalarProducts


        #region DiagonalMatrixProducts


        // DIAGONAL * MATRIX

        /// <summary>Left-multiplies a matrix with a diagonal matrix, and stores the result in the specified result matrix.
        /// R=diag(d)*A
        /// This is a plain version of the method that does not perform any consistency checks.
        /// The operation can be done in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="diag">First operand (a vector representing a diagonal matrix).</param>
        /// <param name="a">First operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyDiagonalPlain(IVector diag, IMatrix a, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] * diag[i];
                }
        }

        /// <summary>Right-multiplies a matrix with a diagonal matrix, and stores the result in the specified result matrix.
        /// R=diag(d)*A
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="diag">First operand (a vector representing a diagonal matrix).</param>
        /// <param name="a">First operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyDiagonal(IVector diag, IMatrix a, IMatrix result)
        {
            if (a == null || diag == null)
                throw new ArgumentNullException("Diagonal - matrix multiplication: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for diagonal - matrix multiplication is not specified (null reference).");
            else if (diag.Length != a.RowCount
                || result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at diagonal - matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + diag.Length
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] * diag[i];
                }
        }

        /// <summary>Right-multiplies a matrix with a diagonal matrix, and stores the result in the specified result matrix.
        /// R=diag(d)*A
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="diag">First operand (a vector representing a diagonal matrix).</param>
        /// <param name="a">First operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyDiagonal(IVector diag, IMatrix a, ref IMatrix result)
        {
            if (a == null || diag == null)
                throw new ArgumentNullException("Diagonal - matrix multiplication: operand not specified (null reference).");
            else if (diag.Length != a.RowCount)
                throw new ArgumentException("Dimension mismatch at diagonal - matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + diag.Length
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] * diag[i];
                }
        }



        // INVERSE DIAGONAL * MATRIX

        /// <summary>Left-multiplies a matrix with inverse of a diagonal matrix, and stores the result in the specified result matrix.
        /// R=diag(d)^-1*A
        /// This is a plain version of the method that does not perform any consistency checks.
        /// The operation can be done in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="diag">First operand (a vector representing a diagonal matrix).</param>
        /// <param name="a">Second operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyInverseDiagonalPlain(IVector diag, IMatrix a, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] / diag[i];
                }
        }

        /// <summary>Right-multiplies a matrix with inverse of a diagonal matrix, and stores the result in the specified result matrix.
        /// R=diag(d)^-1*A
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="diag">First operand (a vector representing a diagonal matrix).</param>
        /// <param name="a">First operand.</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyInverseDiagonal(IVector diag, IMatrix a, IMatrix result)
        {
            if (a == null || diag == null)
                throw new ArgumentNullException("Inverse diagonal - matrix multiplication: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for inverse diagonal - matrix multiplication is not specified (null reference).");
            else if (diag.Length != a.RowCount
                || result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at inverse diagonal - matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + diag.Length
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] / diag[i];
                }
        }

        /// <summary>Right-multiplies a matrix with inverse of a diagonal matrix, and stores the result in the specified result matrix.
        /// R=diag(d)^-1*A
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="diag">First operand (a vector representing a diagonal matrix).</param>
        /// <param name="a">First operand.</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyInverseDiagonal(IVector diag, IMatrix a, ref IMatrix result)
        {
            if (a == null || diag == null)
                throw new ArgumentNullException("Inverse diagonal - matrix multiplication: operand not specified (null reference).");
            else if (diag.Length != a.RowCount)
                throw new ArgumentException("Dimension mismatch at inverse diagonal - matrix multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + diag.Length
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] / diag[i];
                }
        }




        // MATRIX * DIAGONAL

        /// <summary>Right-multiplies a matrix with a diagonal matrix, and stores the result in the specified result matrix.
        /// R=A*diag(d)
        /// This is a plain version of the method that does not perform any consistency checks.
        /// The operation can be done in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="diag">Second operand (a vector representing a diagonal matrix).</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyDiagonalPlain(IMatrix a, IVector diag, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] * diag[j];
                }
        }

        /// <summary>Right-multiplies a matrix with a diagonal matrix, and stores the result in the specified result matrix.
        /// R=A*diag(d)
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="diag">Second operand (a vector representing a diagonal matrix).</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyDiagonal(IMatrix a, IVector diag, IMatrix result)
        {
            if (a == null || diag == null)
                throw new ArgumentNullException("Matrix - diagonal multiplication: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix -diagonal multiplication is not specified (null reference).");
            else if (diag.Length!=a.ColumnCount
                || result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix - diagonal multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + diag.Length 
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] * diag[j];
                }
        }

        /// <summary>Right-multiplies a matrix with a diagonal matrix, and stores the result in the specified result matrix.
        /// R=A*diag(d)
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="diag">Second operand (a vector representing a diagonal matrix).</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyDiagonal(IMatrix a, IVector diag, ref IMatrix result)
        {
            if (a == null || diag == null)
                throw new ArgumentNullException("Matrix - diagonal multiplication: operand not specified (null reference).");
            else if (diag.Length != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix - diagonal multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + diag.Length
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] * diag[j];
                }
        }



        // MATRIX * INVERSE DIAGONAL

        /// <summary>Right-multiplies a matrix with inverse of a diagonal matrix, and stores the result in the specified result matrix.
        /// R=A*diag(d)^-1
        /// This is a plain version of the method that does not perform any consistency checks.
        /// The operation can be done in place.
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="diag">Second operand (a vector representing a diagonal matrix).</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyInverseDiagonalPlain(IMatrix a, IVector diag, IMatrix result)
        {
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] / diag[j];
                }
        }

        /// <summary>Right-multiplies a matrix with inverse of a diagonal matrix, and stores the result in the specified result matrix.
        /// R=A*diag(d)^-1
        /// WARNING: dimensions of matrices must match, otherwise an exception is thrown.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="diag">Second operand (a vector representing a diagonal matrix).</param>
        /// <param name="result">Matrix where the result is stored. Dimensions must match dimensions of operands.</param>
        public static void MultiplyInverseDiagonal(IMatrix a, IVector diag, IMatrix result)
        {
            if (a == null || diag == null)
                throw new ArgumentNullException("Matrix - inverse diagonal multiplication: operand not specified (null reference).");
            else if (result == null)
                throw new ArgumentNullException("Result storage for matrix - inverse diagonal multiplication is not specified (null reference).");
            else if (diag.Length!=a.ColumnCount
                || result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix - inverse diagonal multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + diag.Length 
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] / diag[j];
                }
        }

        /// <summary>Right-multiplies a matrix with inverse of a diagonal matrix, and stores the result in the specified result matrix.
        /// R=A*diag(d)^-1
        /// WARNING: dimensions of operands must match, otherwise an exception is thrown.
        /// If dimensions of the result do not match then the result is re-allocated.</summary>
        /// <param name="a">First operand.</param>
        /// <param name="diag">Second operand (a vector representing a diagonal matrix).</param>
        /// <param name="result">Matrix where result is stored.</param>
        public static void MultiplyInverseDiagonal(IMatrix a, IVector diag, ref IMatrix result)
        {
            if (a == null || diag == null)
                throw new ArgumentNullException("Matrix - inverse diagonal multiplication: operand not specified (null reference).");
            else if (diag.Length != a.ColumnCount)
                throw new ArgumentException("Dimension mismatch at matrix - inverse diagonal multiplication. " + Environment.NewLine
                    + "  First operand: " + a.RowCount + "x" + a.ColumnCount
                    + ",  second operand: " + diag.Length
                    + ", result: " + result.RowCount + "x" + result.ColumnCount + ".");
            else if (result == null)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            else if (result.RowCount != a.RowCount || result.ColumnCount != a.ColumnCount)
                result = a.GetNew(a.RowCount, a.ColumnCount);
            int i, j;
            for (i = 0; i < a.RowCount; ++i)
                for (j = 0; j < a.ColumnCount; ++j)
                {
                    result[i, j] = a[i, j] / diag[j];
                }
        }



        #endregion DiagonalMatrixProducts




        #region Auxiliary

        /// <summary>Returns hash code of the specified matrix.</summary>
        /// <param name="mat">Matrix whose hath code is returned.</param>
        /// <remarks>This method should be used when overriding the GetHashCode() in  vector classes, 
        /// in order to unify calculation of hash code over different vector classes.</remarks>
        public static int GetHashCode(IMatrix mat)
        {
            if (mat == null)
                return int.MaxValue;
            int dim1 = mat.RowCount;
            int dim2 = mat.ColumnCount;
            if (dim1 < 1 || dim2 < 1)
            {
                return int.MaxValue - 1 - Math.Abs(dim1 * dim2) - Math.Abs(dim2);
            }
            int ret = 0;
            for (int i = 0; i < dim1; i++)
            {
                for (int j = 0; j < dim2; j++)
                {
                    ret ^= mat[i, j].GetHashCode(); ;
                }
            }
            return ret;
        }

        /// <summary>Returns true if the specified matrices are equal, false if not.</summary>
        /// <param name="m1">The first of the two matrices that are checked for equality.</param>
        /// <param name="m2">The second of the two matrices that are checked for equality.</param>
        /// <remarks>
        /// <para>This method should be used when overriding the Equals() method in  matrix classes, 
        /// in order to unify the equality check over different matrix classes.</para>
        /// <para>If both matrices are null or both have one dimension less than 1 and the other dimension the same 
        /// then then they considered equal.</para>
        /// <para>This method is consistent with the <see cref="MatrixBase.Compare"/> method, i.e. it returns the 
        /// same value as the expression <see cref="MatrixBase.Compare"/>(<paramref name="m1"/>, <paramref name="m2"/>==0).</para>
        /// </remarks>
        public static bool Equals(IMatrix m1, IMatrix m2)
        {
            if (m1 == null)
            {
                if (m2 == null)
                    return true;
                else
                    return false;
            }
            else
            {
                int dim1 = m1.RowCount;
                int dim2 = m1.ColumnCount;
                if (m2.RowCount != dim1 || m2.ColumnCount!=dim2)
                    return false;
                else
                {
                    for (int i = 0; i < dim1; i++)
                        for (int j=0; j<dim2; j++)
                    {
                        if (m1[i, j] != m2[i, j])
                            return false;
                    }
                    return true;
                }
            }
        }

        /// <summary>Returns an integer valued hash function of the specified matrix object.
        /// <para>The returned value is calculated by the <see cref="Util.GetHashFunctionInt"/> method.</para></summary>
        /// <param name="mat">Matrix object whose hash function is calculated and returned.</param>
        /// <seealso cref="Util.GetHashFunctionInt"/>
        public static int GetHashFunctionInt(IMatrix mat)
        {
            return Util.GetHashFunctionInt(mat);
        }

        /// <summary>Returns a string valued hash function of the specified matrix object.
        /// <para>The returned value is calculated by the <see cref="Util.GetHashFunctionString"/> method.</para></summary>
        /// <param name="mat">Matrix object whose hash function is calculated and returned.</param>
        /// <seealso cref="Util.GetHashFunctionString"/>
        public static string GetHashFunctionString(IMatrix mat)
        {
            return Util.GetHashFunctionString(mat);
        }


        #endregion Auxiliary


        #endregion StaticOperations




        #region InputOutput



        #region StaticInputOutput



        ///// <summary>Returns string representation of the specified matrix in the standard IGLib form. 
        ///// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        ///// <param name="mat">Matrix whose string representation is returned.</param>
        //public static string ToString(IMatrix mat)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (mat == null)
        //        sb.Append("{}");
        //    else
        //    {
        //        int d1 = mat.RowCount - 1;
        //        int d2 = mat.ColumnCount - 1;
        //        sb.Append('{');
        //        for (int i = 0; i <= d1; ++i)
        //        {
        //            sb.Append("{");
        //            for (int j = 0; j <= d2; ++j)
        //            {
        //                sb.Append(mat[i, j].ToString(null, CultureInfo.InvariantCulture));
        //                if (j < d2)
        //                    sb.Append(", ");
        //            }
        //            sb.Append("}");
        //            if (i < d1)
        //                sb.Append(", ");
        //        }
        //        sb.Append("}");
        //    }
        //    return sb.ToString();
        //}


        /// <summary>Returns a readable string form of a matrix, accuracy and padding can be set.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        /// <param name="accuracy">Accuracy of matrix elments representations.</param>
        /// <param name="padding">Padding of matrix elements.</param>
        /// <returns>A readable string representation in tabular form, with the specified accuracy and padding of elements.</returns>
        public static string ToStringReadable(IMatrix mat, int accuracy = 4, int padding = 8)
        {
            if (mat == null)
                return Util.NullRepresentationString;
            string accStr = "F" + accuracy.ToString();
            StringBuilder sb = new StringBuilder();

            // string s = "";
            for (int i = 0; i < mat.RowCount; ++i)
            {
                for (int j = 0; j < mat.ColumnCount; ++j)
                    sb.Append(mat[i, j].ToString(accStr).PadLeft(padding) + " ");
                sb.AppendLine();
            }
            return sb.ToString();
        }



        /// <summary>Returns a string representation of the specified matrix with newlines inserted after each row.
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        public static string ToStringNewlines(IMatrix mat)
        {
            StringBuilder sb = new StringBuilder();
            if (mat == null)
                sb.Append("{}");
            else
            {
                int d1 = mat.RowCount - 1;
                int d2 = mat.ColumnCount - 1;
                sb.Append('{' + Environment.NewLine);
                for (int i = 0; i <= d1; ++i)
                {
                    sb.Append("  {");
                    for (int j = 0; j <= d2; ++j)
                    {
                        sb.Append(mat[i, j].ToString(null, CultureInfo.InvariantCulture));
                        if (j < d2)
                            sb.Append(", ");
                    }
                    sb.Append("}");
                    if (i < d1)
                        sb.Append(", " + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);
            }
            return sb.ToString();
        }


        /// <summary>Returns a string representation of the specified matrix with newlines inserted after each row, 
        /// with the specified format for elements of the matrix.
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        /// <param name="elementFormat">Format specification for printing individual element.</param>
        public static string ToStringNewlines(IMatrix mat, string elementFormat)
        {
            StringBuilder sb = new StringBuilder();
            if (mat == null)
                sb.Append("{}");
            else
            {
                int d1 = mat.RowCount - 1;
                int d2 = mat.ColumnCount - 1;
                sb.Append('{' + Environment.NewLine);
                for (int i = 0; i <= d1; ++i)
                {
                    sb.Append("  {");
                    for (int j = 0; j <= d2; ++j)
                    {
                        sb.Append(mat[i, j].ToString(elementFormat, CultureInfo.InvariantCulture));
                        if (j < d2)
                            sb.Append(", ");
                    }
                    sb.Append("}");
                    if (i < d1)
                        sb.Append(", " + Environment.NewLine);
                }
                sb.Append("}" + Environment.NewLine);
            }
            return sb.ToString();
        }

        /// <summary>Returns string representation of the current matrix in the standard IGLib form
        /// (Mathematica-like format but with C representation of numbers).
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        public static string ToString(IMatrix mat)
        {
            return ToStringMath(mat);
        }

        /// <summary>Returns string representation of the current matrix in the standard IGLib form
        /// (Mathematica-like format but with C representation of numbers).
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        public static string ToStringMath(IMatrix mat)
        {
            StringBuilder sb = new StringBuilder();
            if (mat == null)
                sb.Append("{}");
            else
            {
                int d1 = mat.RowCount - 1;
                int d2 = mat.ColumnCount - 1;
                sb.Append('{');
                for (int i = 0; i <= d1; ++i)
                {
                    sb.Append("{");
                    for (int j = 0; j <= d2; ++j)
                    {
                        sb.Append(mat[i, j].ToString(null, CultureInfo.InvariantCulture));
                        if (j < d2)
                            sb.Append(", ");
                    }
                    sb.Append("}");
                    if (i < d1)
                        sb.Append(", ");
                }
                sb.Append("}");
            }
            return sb.ToString();
        }

        /// <summary>Returns string representation of the current matrix in the standard IGLib form
        /// (Mathematica-like format but with C representation of numbers), with the specified  
        /// format for elements of the matrix.
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        /// <param name="elementFormat">Format specification for printing individual element.</param>
        public static string ToString(IMatrix mat, string elementFormat)
        {
            return ToStringMath(mat, elementFormat);
        }

        /// <summary>Returns string representation of the current matrix in the standard IGLib form
        /// (Mathematica-like format but with C representation of numbers), with the specified  
        /// format for elements of the matrix.
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        /// <param name="mat">Matrix whose string representation is returned.</param>
        /// <param name="elementFormat">Format specification for printing individual element.</param>
        public static string ToStringMath(IMatrix mat, string elementFormat)
        {
            StringBuilder sb = new StringBuilder();
            if (mat == null)
                sb.Append("{}");
            else
            {
                int d1 = mat.RowCount - 1;
                int d2 = mat.ColumnCount - 1;
                sb.Append('{');
                for (int i = 0; i <= d1; ++i)
                {
                    sb.Append("{");
                    for (int j = 0; j <= d2; ++j)
                    {
                        sb.Append(mat[i, j].ToString(elementFormat, CultureInfo.InvariantCulture));
                        if (j < d2)
                            sb.Append(", ");
                    }
                    sb.Append("}");
                    if (i < d1)
                        sb.Append(", ");
                }
                sb.Append("}");
            }
            return sb.ToString();
        }


#if false

        /// <summary>Saves (serializes) the specified matrix to the specified JSON file.
        /// File is owerwritten if it exists.</summary>
        /// <param name="mat">Object that is saved to a file.</param>
        /// <param name="filePath">Path to the file in which object is is saved.</param>
        public static void SaveJson(IMatrix mat, string filePath)
        {
            throw new NotImplementedException();
            //SaveJson(mat, filePath, false /* append */ );
        }


        /// <summary>Saves (serializes) the specified matrix to the specified JSON file.</summary>
        /// <param name="mat">Object that is saved to a file.</param>
        /// <param name="filePath">Path to the file in which object is is saved.</param>
        /// <param name="append">Specifies whether serialized data is appended at the end of the file
        /// in the case that the file already exists.</param>
        public static void SaveJson(IMatrix mat, string filePath, bool append)
        {
            throw new NotImplementedException();
            //MatrixDtoBase dtoOriginal = new MatrixDtoBase();
            //dtoOriginal.CopyFrom(mat);
            //ISerializer serializer = new SerializerJson();
            //serializer.Serialize<MatrixDtoBase>(dtoOriginal, filePath, append);
        }

        /// <summary>Restores (deserializes) a matrix object from the specified file in JSON format.</summary>
        /// <param name="filePath">File from which object data is restored.</param>
        /// <param name="matRestored">Object that is restored by deserialization.</param>
        public static void LoadJson(string filePath, ref IMatrix matRestored)
        {
            throw new NotImplementedException();
            //ISerializer serializer = new SerializerJson();
            //MatrixDtoBase dtoRestored = serializer.DeserializeFile<MatrixDtoBase>(filePath);
            //dtoRestored.CopyTo(ref matRestored);
        }

#endif

        #endregion StaticInputOutput




        /// <summary>Returns string representation of the current matrix in the standard IGLib form. 
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        public override string ToString()
        {
            return ToString(this);
        }

        /// <summary>Returns a readable an easily string form of a matrix, accuracy and padding can be set.</summary>
        /// <param name="accuracy">Accuracy of matrix elments representations.</param>
        /// <param name="padding">Paddind of matrix elements.</param>
        /// <returns>A readable string representation in tabular form.</returns>
        public virtual string ToStringReadable(int accuracy = 4, int padding = 8)
        {
            return MatrixBase.ToStringReadable(this, accuracy, padding);
        }

        /// <summary>Returns a string representation of this matrix with newlines inserted after each row.
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        public virtual string ToStringNewlines()
        {
            return ToStringNewlines(this);
        }

        /// <summary>Returns a string representation of this matrix with newlines inserted after each row, 
        /// with the specified format for elements of the matrix.
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        /// <param name="elementFormat">Format specification for printing individual element.</param>
        public virtual string ToStringNewlines(string elementFormat)
        {
            return ToStringNewlines(this, elementFormat);
        }


        /// <summary>Returns string representation of the current matrix in the standard IGLib form
        /// (Mathematica-like format but with C representation of numbers).
        /// Rows and elements are printed in comma separated lists in curly brackets.</summary>
        public virtual string ToStringMath()
        {
            return ToStringMath(this);
        }


        /// <summary>Returns a string representation of the current matrix in a standard IGLib form
        /// (Mathematica-like format but with C representation of numbers), with the specified  
        /// format for elements of the matrix.</summary>
        /// <param name="elementFormat">Format specification for printing individual element.</param>
        public virtual string ToString(string elementFormat)
        {
            return ToString(this, elementFormat);
        }

        /// <summary>Returns a string representation of the current matrix in a standard IGLib form
        /// (Mathematica-like format but with C representation of numbers), with the specified  
        /// format for elements of the matrix.</summary>
        /// <param name="elementFormat">Format specification for printing individual element.</param>
        public virtual string ToStringMath(string elementFormat)
        {
            return ToStringMath(this, elementFormat);
        }
        

        #endregion InputOutput


        #region Operators_Overloading


        /// <summary>Unary plus, returns the operand.</summary>
        public static MatrixBase operator +(MatrixBase m)
        {
            if (m == null)
                return null;
            else
                return m.GetCopyBase();
        }

        /// <summary>Unary negation, returns the negative operand.</summary>
        public static MatrixBase operator -(MatrixBase m)
        {
            if (m == null)
                return null;
            else
            {
                MatrixBase ret = m.GetCopyBase();
                ret.Negate();
                return ret;
            }
        }

        /// <summary>Matrix addition.</summary>
        public static MatrixBase operator +(MatrixBase a, MatrixBase b)
        {
            MatrixBase ret = a.GetCopyBase();
            Add(a, b, ret);
            return ret;
        }


        /// <summary>Matrix subtraction.</summary>
        public static MatrixBase operator -(MatrixBase a, MatrixBase b)
        {
            MatrixBase ret = a.GetCopyBase();
            Subtract(a, b, ret);
            return ret;
        }



        /// <summary>Product of two matrices.</summary>
        public static MatrixBase operator *(MatrixBase a, MatrixBase b)
        {
            if (a == null)
                return null;
            else if (b == null)
                return null;
            MatrixBase ret = a.GetNewBase(a.RowCount, b.ColumnCount);
            Multiply(a, b, ret);
            return ret;
        }

        /// <summary>Product of a matrix and a vector.</summary>
        public static VectorBase operator *(MatrixBase a, VectorBase b)
        {
            VectorBase ret = b.GetNewBase();
            Multiply(a, b, ret);
            return ret;
        }

        /// <summary>Product of a matrix by a scalar.</summary>
        public static MatrixBase operator *(MatrixBase a, double b)
        {
            MatrixBase ret = a.GetNewBase();
            MatrixBase.Multiply(a, b, ret);
            return ret;
        }

        /// <summary>Product of a matrix by a scalar.</summary>
        public static MatrixBase operator *(double a, MatrixBase b)
        {
            MatrixBase ret = b.GetNewBase();
            Multiply(b, a, ret);
            return ret;
        }

        /// <summary>Matrix subtraction.</summary>
        public static MatrixBase operator /(MatrixBase a, double b)
        {
            MatrixBase ret = a.GetNewBase();
            Divide(a, b, ret);
            return ret;
        }



        #endregion  // Operators_Overloading



        #region LuDecomposition



        /// <summary>Simpler but slower (compared to <see cref="Determinant"/>) implementation of determinant
        /// calculation of an arbitrary square real matrix.
        /// <para>Auxiliary variable are allocated internally by the function, which makes it slower with respect to
        /// <see cref="Determinant"/> where auxiliary parameters can be provided by the caller.</para></summary>
        /// <param name="A">Matrix whose determinant is calculated.</param>
        /// <returns>The calculated determinant of the specified matrix.</returns>
        public static double DeterminantSlow(IMatrix A)
        {
            int[] permutations = null;
            IMatrix LU = null;
            return Determinant(A, ref permutations, ref LU);
        }

        /// <summary>Calculates and returns determinant of a real-valued square matrix.
        /// <para>This function is efficient when auxiliary parameters <paramref name="auxPermutations"/> and <paramref name="auxLU"/>
        /// are provided that are already initialized with the same dimensions as the matrix whose determinant is calculated.</para></summary>
        /// <param name="A">Matrix whose determinant is to be calculated.</param>
        /// <param name="auxPermutations">Auxiliary array where row permutations of LU decomposition of <paramref name="A"/> will be stored. 
        /// For best performance, caller should pass an array that is alredy initialized with the same dimensions.</param>
        /// <param name="auxLU">Auxiliary matrix where LU decomposition of <paramref name="A"/> will be stored. For best performance,
        /// caller should pass a matrix that is alredy initialized with the same dimensions.</param>
        /// <returns>The calculated determinant of the specified matrix.</returns>
        public static double Determinant(IMatrix A, ref int[] auxPermutations, ref IMatrix auxLU)
        {
            if (A == null)
                throw new ArgumentException("Matrix whose determinant is calculateed is not specified (nulll reference).");
            else if (A.RowCount != A.ColumnCount || A.RowCount < 1)
                throw new ArgumentException("Invalid dimensions of matrix whose determinant is calculated. Should be square with dimension greater than 0.");
            int toggle = 0;
            LuDecompose(A, out toggle, ref auxPermutations, ref auxLU);
            return LuDeterminant(auxLU, toggle);
        }


        /// <summary>Calculates thr Doolittle LU decomposition with partial pivoting (LUP) of a square real matrix.
        /// <para>Throws <see cref="InvalidOperationException"/> if the matrix is singular.</para>
        /// <para>http://en.wikipedia.org/wiki/LU_decomposition#Doolittle_algorithm </para></summary>
        /// <param name="A">Square matrix whose decomposition is calculated.</param>
        /// <param name="toggle">Attains value 1 for even number of row swaps, or -1 for odd number of row swaps
        /// (important for calculation of matrix determinant).</param>
        /// <param name="perm">Array where the performed row permutations are stored. Reallocated if necessary.</param>
        /// <param name="result">Matrix where the resulting LU decomposition is stored. Reallocated if necessary.
        /// <para>Result is lower triangular matrix L with 1s on diagonal (stored below diagonal), and upper triangular matrix.</para></param>
        /// $A Igor Dec14;
        public static void LuDecompose(IMatrix A, out int toggle, ref int[] perm, ref IMatrix result)
        {
            if (A == null)
                throw new ArgumentException("Matrix to LU-decompose is not specified (null reference).");
            int dim = A.RowCount;
            if (dim != A.ColumnCount)
                throw new ArgumentException("Matrix to LU-decompose is a non-square mattrix");
            if (perm == null)
                perm = new int[dim];
            else if (perm.Length != dim)
                perm = new int[dim];
            if (object.ReferenceEquals(A, result))
                throw new ArgumentException("Result matrix is the same as inpuut matrix. Can not be done in place.");
            MatrixBase.Copy(A, ref result);  // prepare a copy to work on, resize performed if necessary
            for (int i = 0; i < dim; ++i) { perm[i] = i; } // prepare row permutation array (initialize to identity)

            toggle = 1; // toggle will tracks row swaps. +1 for even, -1 for odd, use by determinant calculation

            for (int j = 0; j < dim - 1; ++j)
            {
                // Iterate over columns: 
                double colMax = Math.Abs(result[j, j]); // find largest value in column j
                int rowMaxCol = j;
                for (int i = j + 1; i < dim; ++i)
                {
                    if (result[i, j] > colMax)
                    {
                        colMax = result[i, j];
                        rowMaxCol = i;
                    }
                }
                if (rowMaxCol != j) // if the largest value is not on pivot, swap rows
                {
                    for (int iCol = 0; iCol < dim; ++iCol)
                    {
                        double rowEl = result[rowMaxCol, iCol];
                        result[rowMaxCol, iCol] = result[j, iCol];
                        result[j, iCol] = rowEl;
                    }
                    int tmp = perm[rowMaxCol]; // swap permutation array entries correspondingly
                    perm[rowMaxCol] = perm[j];
                    perm[j] = tmp;

                    toggle = -toggle; // adjust the toggle
                }
                if (Math.Abs(result[j, j]) < 1.0E-20) // if diagonal element after swap is zero,throw exception
                    throw new InvalidOperationException("Zero diagonal term in LU decomposition, matrix is singular.");

                for (int i = j + 1; i < dim; ++i)
                {
                    result[i, j] /= result[j, j];
                    for (int k = j + 1; k < dim; ++k)
                    {
                        result[i, k] -= result[i, j] * result[j, k];
                    }
                }
            } // loop over columns

        } // LuDecompose

        /// <summary>Solves a system of equations with the specified LU decomposition with already permuted b.
        /// <para>Helper method. Before it is used, right-hand side vector must be permuted according to row permutations
        /// contained in LU decomposition.</para>
        /// <para>Parameters must be of correct dimensions. No reallocations are performed.</para>
        /// <para>No consistency checks are performed on arguments.</para></summary>
        /// <param name="luMatrix">Matrix containing LU decomposition of the originall system matrix.</param>
        /// <param name="b">Vector containing right-hand sides permuted according to permutations of LU decomposition.</param>
        /// <param name="x">Vector where permuted results are stored.</param>
        /// $A Igor Dec14;
        protected static void LuSolveNoPermutationsPlain(IMatrix luMatrix, IVector b, IVector x)
        {
            int n = luMatrix.RowCount;
            VectorBase.CopyPlain(b, x);
            for (int i = 1; i < n; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= luMatrix[i, j] * x[j];
                x[i] = sum;
            }

            x[n - 1] /= luMatrix[n - 1, n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < n; ++j)
                    sum -= luMatrix[i, j] * x[j];
                x[i] = sum / luMatrix[i, i];
            }
        }

        /// <summary>Solves a system of equations with the specified right-hand sides and the specified LU decomposition 
        /// of the system matrix.</summary>
        /// <param name="luMatrix">Matrix containing LU decomposition of the original matrix with permuted rows.</param>
        /// <param name="perm">Permutation array containing information about row permutations performed during LU decomposition.</param>
        /// <param name="b">Vextor of right-hand sides.</param>
        /// <param name="auxVec">Auxiliary vector of the same dimension as the system matrix.</param>
        /// <param name="x">Vector where the solution is stored.</param>
        /// $A Igor Dec14;
        public static void LuSolve(IMatrix luMatrix, int[] perm, IVector b, ref IVector auxVec, ref IVector x)
        {
            if (luMatrix == null)
                throw new ArgumentException("Matrix containing LU decomposition is not specified (null reference).");
            if (perm == null)
                throw new ArgumentException("Permutation array is not specified.");
            int dim = luMatrix.RowCount;
            if (luMatrix.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LU decomposition is not a square matrix.");
            if (perm.Length != dim)
                throw new ArgumentException("Length of permutation array in does not correspond to dimension of the LU decomposed matrix.");
            if (x == null)
                x = luMatrix.GetNewVector(dim);
            if (x.Length != dim)
                x = luMatrix.GetNewVector(dim);
            if (auxVec == null)
                auxVec = luMatrix.GetNewVector(dim);
            if (auxVec.Length != dim)
                auxVec = luMatrix.GetNewVector(dim);
            if (object.ReferenceEquals(b, x))
                throw new ArgumentException("Input and output vectors are the same. Can not be done in place.");
            else if (object.ReferenceEquals(b, auxVec) || object.ReferenceEquals(x, auxVec))
                throw new ArgumentException("Auxiliary vector the same as input or output. Needs separate instance.");
            // Permute b according to perm & store it in auxVec:
            for (int i = 0; i < dim; ++i)
                auxVec[i] = b[perm[i]];
            // Solve the system with permuted right-hand sides by calling the helper function:
            LuSolveNoPermutationsPlain(luMatrix, auxVec, x);
        }


        /// <summary>Calculates inverse of the matrix from its specified LU decomposition.</summary>
        /// <param name="luMatrix">Matrix containing the LU decomposition of the original matrix (with partial pivoting).</param>
        /// <param name="perm">Array containing information of row permutations from the LU decomposition procedure.</param>
        /// <param name="B">Matrix whose columns are right-hand sides of equations to be solved.</param>
        /// <param name="auxVec">Auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="auxRight">Auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="auxX">Another auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="X">Matrix where result will be stored. Reallocated if necessary.</param>
        /// $A Igor Dec14;
        public static void LuSolve(IMatrix luMatrix, int[] perm, IMatrix B, ref IVector auxVec, ref IVector auxRight,
            ref IVector auxX, ref IMatrix X)
        {
            if (luMatrix == null)
                throw new ArgumentException("Matrix containing LU decomposition is not specified (null reference).");
            if (perm == null)
                throw new ArgumentException("Permutation array is not specified.");
            int dim = luMatrix.RowCount;
            if (B == null)
                throw new ArgumentException("Matrix of right-hand sides is not specified (null reference).");
            if (B.RowCount != dim)
                throw new ArgumentException("Matrix of right-hand sides does not have as many rows as there are equations.");
            int numSystems = B.ColumnCount;
            if (luMatrix.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LU decomposition is not a square matrix.");
            if (perm.Length != dim)
                throw new ArgumentException("Length of permutation array in does not correspond to dimension of the LU decomposed matrix.");
            if (auxVec == null)
                auxVec = luMatrix.GetNewVector(dim);
            if (auxVec.Length != dim)
                auxVec = luMatrix.GetNewVector(dim);
            if (auxRight == null)
                auxRight = luMatrix.GetNewVector(dim);
            if (auxRight.Length != dim)
                auxRight = luMatrix.GetNewVector(dim);
            if (auxX == null)
                auxX = luMatrix.GetNewVector(dim);
            if (auxX.Length != dim)
                auxX = luMatrix.GetNewVector(dim);
            if (X == null)
                X = luMatrix.GetNew(dim, numSystems);
            if (X.RowCount != dim || X.ColumnCount != numSystems)
                X = luMatrix.GetNew(dim, numSystems);
            if (object.ReferenceEquals(luMatrix, X) || object.ReferenceEquals(luMatrix, B) || object.ReferenceEquals(B, X))
                throw new ArgumentException("Input matrix the same as result matrix. Can not be done in place.");
            if (object.ReferenceEquals(auxVec, auxRight) || object.ReferenceEquals(auxVec, auxX) || object.ReferenceEquals(auxRight, auxX))
                throw new ArgumentException("Auxiliary vectors are the same references. Need separate instances.");
            for (int whichSystem = 0; whichSystem < numSystems; ++whichSystem)
            {
                // Get column of B as right-hand sides:
                for (int j = 0; j < dim; ++j)
                    auxRight[j] = B[j, whichSystem];
                // Solve the system with this vector:
                LuSolve(luMatrix, perm, auxRight, ref auxVec, ref auxX);
                for (int j = 0; j < dim; ++j)
                    X[j, whichSystem] = auxX[j];
            }
        }

        /// <summary>Calculates inverse of the matrix from its specified LU decomposition.</summary>
        /// <param name="luMatrix">Matrix containing the LU decomposition of the original matrix (with partial pivoting).</param>
        /// <param name="perm">Array containing information of row permutations from the LU decomposition procedure.</param>
        /// <param name="auxRight">Auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="auxX">Another auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="res">Matrix where result will be stored. Reallocated if necessary.</param>
        /// $A Igor Dec14;
        public static void LuInverse(IMatrix luMatrix, int[] perm, ref IVector auxRight, ref IVector auxX, ref IMatrix res)
        {
            if (luMatrix == null)
                throw new ArgumentException("Matrix containing LU decomposition is not specified (null reference).");
            if (perm == null)
                throw new ArgumentException("Permutation array is not specified.");
            int dim = luMatrix.RowCount;
            if (luMatrix.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LU decomposition is not a square matrix.");
            if (perm.Length != dim)
                throw new ArgumentException("Length of permutation array in does not correspond to dimension of the LU decomposed matrix.");
            if (auxRight == null)
                auxRight = luMatrix.GetNewVector(dim);
            if (auxRight.Length != dim)
                auxRight = luMatrix.GetNewVector(dim);
            if (auxX == null)
                auxX = luMatrix.GetNewVector(dim);
            if (auxX.Length != dim)
                auxX = luMatrix.GetNewVector(dim);
            if (res == null)
                res = luMatrix.GetNew(dim, dim);
            if (res.RowCount != dim || res.ColumnCount != dim)
                res = luMatrix.GetNew(dim, dim);
            if (object.ReferenceEquals(luMatrix, res))
                throw new ArgumentException("Input matrix the same as result matrix. Can not be done in place.");
            if (object.ReferenceEquals(auxRight, auxX))
                throw new ArgumentException("Auxiliary vectors are the same references. Need separate instances.");
            for (int i = 0; i < dim; ++i)
            {
                for (int j = 0; j < dim; ++j)
                {
                    if (i == perm[j])
                        auxRight[j] = 1.0;
                    else
                        auxRight[j] = 0.0;
                }
                LuSolveNoPermutationsPlain(luMatrix, auxRight, auxX);
                for (int j = 0; j < dim; ++j)
                    res[j, i] = auxX[j];
            }
        }

        /// <summary>Calculates and returns matrix determinant form its specified LU decomposition.</summary>
        /// <param name="luMatrix">Matrix containing the LU decomposition of the matrix whose determinant is to be calculated.</param>
        /// <param name="toggle">Toggle that contains information about number of row permutations when building LU decomposittion,
        /// 1 for even and -1 for odd number of permutations.</param>
        /// <returns>Determinant calculated from matrix LU decomposition.</returns>
        /// $A Igor Dec14;
        public static double LuDeterminant(IMatrix luMatrix, int toggle)
        {
            if (luMatrix == null)
                throw new ArgumentException("Determinant calculation: Matrix containing LU decomposition is not specified.");
            int dim = luMatrix.RowCount;
            if (luMatrix.ColumnCount != dim)
                throw new ArgumentException("Determinant calculation: Matrix containing LU decomposition is not a square matrix.");
            if (toggle != -1 && toggle != 1)
                throw new ArgumentException("Determinant calculation: toggle should be either -1 or 1.");
            double result = toggle;
            for (int i = 0; i < dim; ++i)
                result *= luMatrix[i, i];
            return result;
        }


        /// <summary>Extracts the lower part of the Doolittle specified LU decomposition (1s on diagonal, 0s in above diagonal)
        /// and stores it in the specified result matrix.
        /// <para>Although operatioin can be done in place, it is not allowed for input and output matrix to be the same.
        /// Reason is that the operation always done in in combination (with extracting all parts).</para></summary>
        /// <param name="matLu">Matrix containng the LU decomposition of some matrix.</param>
        /// <param name="result">Matrix where the lower part of the specified matrix is stored.</param>
        /// $A Igor Dec14;
        public static void LuExtractLower(IMatrix matLu, ref IMatrix result)
        {
            if (matLu == null)
                throw new ArgumentException("Marix containing LU decomposed original is not specified (null reference).");
            int dim = matLu.RowCount;
            if (matLu.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LU decomposition is not a square matrix.");
            if (result == null)
                result = matLu.GetNew(dim, dim);
            if (result.RowCount != dim || result.ColumnCount != dim)
                result = matLu.GetNew(dim, dim);
            if (object.ReferenceEquals(matLu, result))
                throw new ArgumentException("Input and result matrix are the same, which is not allowed.");
            for (int i = 0; i < dim; ++i)
            {
                for (int j = 0; j < dim; ++j)
                {
                    if (i == j)
                        result[i, j] = 1.0;
                    else if (i > j)
                        result[i, j] = matLu[i, j];
                    else
                        result[i, j] = 0.0;
                }
            }
        }

        /// <summary>Extracts the upper part of the specified Doolittle LU decomposition 0s below diagonal)
        /// and stores it in the specified result matrix.
        /// <para>Although operatioin can be done in place, it is not allowed for input and output matrix to be the same.
        /// Reason is that the operation always done in in combination (with extracting all parts).</para></summary>
        /// <param name="matLu">Matrix containng the LU decomposition of some matrix.</param>
        /// <param name="result">Matrix where the upper part of the specified matrix is stored.</param>
        /// $A Igor Dec14;
        public static IMatrix LuExtractUpper(IMatrix matLu, ref IMatrix result)
        {
            if (matLu == null)
                throw new ArgumentException("Marix containing LU decomposed original is not specified (null reference).");
            int dim = matLu.RowCount;
            if (matLu.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LU decomposition is not a square matrix.");
            if (result == null)
                result = matLu.GetNew(dim, dim);
            if (result.RowCount != dim || result.ColumnCount != dim)
                result = matLu.GetNew(dim, dim);
            if (object.ReferenceEquals(matLu, result))
                throw new ArgumentException("Input and result matrix are the same, which is not allowed.");
            for (int i = 0; i < dim; ++i)
            {
                for (int j = 0; j < dim; ++j)
                {
                    if (i <= j)
                        result[i, j] = matLu[i, j];
                    else
                        result[i, j] = 0.0;
                }
            }
            return result;
        }

        /// <summary>Calculates and stores permutation matrix that correspond to the specified permutation array.
        /// <para>Used only for testing.</para></summary>
        /// <param name="perm">Permutation array, contains information on how matrix rows were permted.</param>
        /// <param name="res">Matrix where the corresponding permutation matrix is sttored. Reallocated if necessary.</param>
        /// $A Igor Dec14;
        public static void PermutationArrayToMatrix(int[] perm, ref IMatrix res)
        {
            if (perm == null)
                throw new ArgumentException("Permutation array not specified (null reference).");
            if (res == null)
                throw new ArgumentException("Matrix where permutation matrix is to be stored is not specified (null referrence).");
            int dim = perm.Length;
            if (dim == 0)
                throw new ArgumentException("Length of permutation array is 0.");
            if (res.RowCount != dim || res.ColumnCount != dim)
                res = res.GetNew(dim, dim);
            MatrixBase.SetZero(res);
            // Convert Doolittle perm array to corresponding perm matrix
            for (int i = 0; i < dim; ++i)
                res[i, perm[i]] = 1.0;
        }

        /// <summary>Permutes rows of the specified aquare matrix according to the specified permutations array.
        /// <para>Mainly used for testing.</para></summary>
        /// <param name="A">Matrix to be Permuted.</param>
        /// <param name="permutations">Array that contains information on row permutations that must be performed.</param>
        /// <param name="result">Matrix where resultt is stored, i.e. the original matrix with permuted rows.</param>
        /// $A Igor Dec14;
        public static void Permute(IMatrix A, int[] permutations, ref IMatrix result)
        {
            if (A == null)
                throw new ArgumentException("Matrix whose rows should be permuted is not specified (null reference).");
            int numRows = A.RowCount;
            int numColumns = A.ColumnCount;
            if (result == null)
                result = A.GetNew(numRows, numColumns);
            else if (result.RowCount != numRows || result.ColumnCount != numColumns)
                result = A.GetNew(numRows, numColumns);
            if (object.ReferenceEquals(A, result))
                throw new ArgumentException("Result matrix is the same as original.");
            for (int row = 0; row < numRows; ++row)
            {
                for (int col = 0; col < numColumns; ++col)
                {
                    result[row, col] = A[permutations[row], col];
                }
            }
        } // Permute()

        /// <summary>Unpermutes the product of the specified DooLittle LU decomposition according to permutations array.
        /// <para>The method can be used for reversing any row permutation performed on a sqyare matrix.</para></summary>
        /// <param name="Apermuted">Matrix containing the permuted LU decomposition of some decomposed original matrix. 
        /// Must be a square matrix.</param>
        /// <param name="permutations">Array that contains information on row permutations that were perfomed during the LU decomposition.</param>
        /// <param name="auxArray">Auxilliary array, should have the same dimension as <paramref name="permutations"/>.</param>
        /// <param name="result">Matrix where resultt is stored, i.e. the unpermuted LU product.</param>
        /// $A Igor Dec14;
        public static void UnPermute(IMatrix Apermuted, int[] permutations, ref int[] auxArray, ref IMatrix result)
        {
            if (Apermuted == null)
                throw new ArgumentException("Matrix whose rows should be permuted is not specified (null reference).");
            int numRows = Apermuted.RowCount;
            int numColumns = Apermuted.ColumnCount;
            if (result == null)
                result = Apermuted.GetNew(numRows, numColumns);
            else if (result.RowCount != numRows || result.ColumnCount != numColumns)
                result = Apermuted.GetNew(numRows, numColumns);
            if (auxArray == null)
                auxArray = new int[numRows];
            else if (auxArray.Length != numRows)
                auxArray = new int[numRows];
            if (object.ReferenceEquals(permutations, auxArray))
                throw new ArgumentException("Auxiliary array is the same as the permutations array.");
            if (object.ReferenceEquals(Apermuted, result))
                throw new ArgumentException("Result matrix is the same as the permuted matrix.");
            for (int i = 0; i < permutations.Length; ++i)
                auxArray[permutations[i]] = i;
            for (int row = 0; row < numRows; ++row)
            {
                for (int col = 0; col < numColumns; ++col)
                {
                    result[row, col] = Apermuted[auxArray[row], col];
                }
            }
        } // UnPermute()


        #region LuDecomposition.Tests

        /// <summary>Performs a test of calculatons performed via LU decomposition of a matrix.
        /// Calculation times and error extents are measured and reported (if specified).
        /// <para>If relative errors are below the specified tolerance, true is returned, otherwise false is returned.</para></summary>
        /// <param name="dim">Dimension of the problems generated for tests.</param>
        /// <param name="numRepetitions">Number of repetitions of the tests. If greater than 1 then tests are repeated
        /// with inputs generated anew each time.</param>
        /// <param name="tol">Tolerance on relative errors of test results.</param>
        /// <param name="outputLevel">Level of output. If 0 then no reports are launched to the console.</param>
        /// <param name="randomGenerator">Random generator used for generation of test inputs.</param>
        /// <param name="A">System matrix used in the test. If specified (i.e., not null) then this matrix is LU decomposed and
        /// used in the first repetition of tests instead of a randomly generated matrix. In this case, its dimension
        /// overrides (when not the same) the specified dimension <paramref name="dim"/> of test matrices and vectors.
        /// If there are more than one repetitions (parameter <paramref name="numRepetitions"/>) then subsequent repetitions
        /// still use randomly generated inputs. If specified (i.e., not null) then this vector is  used in the first repetition 
        /// of tests instead of a a randomly generated vector. Similar rules apply as for <paramref name="A"/>.</param>
        /// <param name="b">Vector of right-hand sides used in the test.</param>
        /// <returns>True if all tests passed successfully (i.e., if errors are below the specified tolerance).</returns>
        /// $A Igor Dec14;
        public static bool TestLuDecomposition(int dim, int numRepetitions = 1, double tol = 1e-6, int outputLevel = 0,
            IRandomGenerator randomGenerator = null, IMatrix A = null, IVector b = null)
        {
            bool passed = true;
            if (tol <= 0)
                tol = 1.0e-6;
            double smallNumber = 1e-20;  // e.g. for guarding division by 0
            StopWatch1 t = new StopWatch1();
            try
            {
                double determinant = 0;
                if (randomGenerator == null)
                    randomGenerator = RandomGenerator.Global;
                if (numRepetitions < 1)
                    numRepetitions = 1;
                if (A != null)
                {
                    dim = A.RowCount;
                    determinant = DeterminantSlow(A);  // reference calculation of determinant (since it is not provided in this case)
                }
                else if (b != null)
                    dim = b.Length;
                if (outputLevel >= 1)
                {
                    if (numRepetitions < 2)
                        Console.WriteLine(Environment.NewLine + "Test of LU decomposition..." + Environment.NewLine);
                    else
                        Console.WriteLine(Environment.NewLine + "Test of LU decomposition (" + numRepetitions + " repetitions)..."
                            + Environment.NewLine);
                }
                for (int repetition = 0; repetition < numRepetitions; ++repetition)
                {
                    if (numRepetitions > 1 && outputLevel >= 1)
                    {
                        Console.WriteLine(Environment.NewLine + "REPETITION No. " + repetition + " of the LU decomposition test..." + Environment.NewLine);
                    }
                    t.Start();
                    if (A == null || repetition > 1)
                    {
                        A = new Matrix(dim, dim);
                        determinant = MatrixBase.SetRandomInvertible(A, randomGenerator);
                    }
                    if (b == null || repetition > 1)
                    {
                        b = new Vector(dim);
                        VectorBase.SetRandom(b);
                    }
                    if (A.RowCount != dim || A.ColumnCount != dim || b.Length != dim)
                        throw new ArgumentException("Provided matrix and/or right/hand sides are not of correct dimensions.");
                    t.Stop();
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Preparation of test input done in " + t.Time + "s (CPU: ", t.CpuTime + ").");
                        Console.WriteLine("Dimension of the matrix for LU testing decomposition: " + dim + "." + Environment.NewLine);
                    }
                    IMatrix LU = new Matrix(dim, dim);
                    int[] permutations = new int[dim];
                    int toggle = 0;
                    t.Start();
                    LuDecompose(A, out toggle, ref permutations, ref LU);
                    t.Stop();
                    // Check the product:
                    IMatrix product = null;
                    IMatrix diffMat = null;
                    IMatrix lower = null;
                    IMatrix upper = null;
                    LuExtractLower(LU, ref lower);
                    LuExtractUpper(LU, ref upper);
                    Multiply(lower, upper, ref product);
                    IMatrix restored = null;
                    int[] auxArray1 = null;
                    UnPermute(product, permutations, ref auxArray1, ref restored);  // use a custom method with the perm array to unscramble LU
                    MatrixBase.Subtract(restored, A, ref diffMat);
                    double relativeError = diffMat.NormForbenius / (A.NormForbenius + smallNumber);
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("LU decompositin calculated in "
                            + t.Time.ToString("G2") + "s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: product of lower and upperr does not match original, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Product of factors: relative error = " + relativeError.ToString("G2"));
                    }
                    // Check solution of system of equations: 
                    IVector x = null;
                    IVector auxVec1 = null;
                    t.Start();
                    LuSolve(LU, permutations, b, ref auxVec1, ref x);
                    t.Stop();
                    IVector testVec = null;
                    IVector diffVec = null;
                    MatrixBase.Multiply(A, x, ref testVec);
                    VectorBase.Subtract(testVec, b, ref diffVec);
                    relativeError = diffVec.Norm / (b.Norm + smallNumber);
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Equations solved (just back substitution) in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: solution of system of equations not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Solution of system of equations: relative error = " + relativeError.ToString("G2"));
                    }
                    // Check calculation of determinant:

                    t.Start();
                    double detCalc = LuDeterminant(LU, toggle);
                    t.Stop();
                    relativeError = Math.Abs(detCalc - determinant) / (Math.Abs(determinant) + smallNumber);
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Determinant caluclated from decomposition in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: calculation of determinant not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Calculation of determinant from decomposition: relative error = " + relativeError.ToString("G2"));
                    }
                    // Check calculation of inverse matrix (directly):
                    IMatrix inverseMat = null;
                    IVector auxVec2 = null;
                    t.Start();
                    LuInverse(LU, permutations, ref auxVec1, ref auxVec2, ref inverseMat);
                    t.Stop();
                    IMatrix identityMat = Matrix.Identity(dim);
                    MatrixBase.Multiply(A, inverseMat, ref product);
                    Matrix.Subtract(product, identityMat, ref diffMat);
                    relativeError = diffMat.NormForbenius / (Math.Sqrt((double)dim));
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Matrix inverse directly from decomposition in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: calculation of matrix inverse not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Matrix inverse directly from decomposition: relative error = " + relativeError.ToString("G2"));
                    }
                    // Check calculation of inverse matrix (through solutions of equations with identity right-hand side):
                    IVector auxVec3 = null;
                    MatrixBase.SetZero(inverseMat);
                    t.Start();
                    LuSolve(LU, permutations, identityMat, ref auxVec1, ref auxVec2, ref auxVec3, ref inverseMat);
                    t.Stop();
                    MatrixBase.Multiply(A, inverseMat, ref product);
                    Matrix.Subtract(product, identityMat, ref diffMat);
                    relativeError = diffMat.NormForbenius / (Math.Sqrt((double)dim));
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Matrix inverse through equation solving in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: calculation of matrix inverse through equation solving not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Matrix inverse through equation solving: relative error = " + relativeError.ToString("G2"));
                    }
                } // loop over repetitions
            }
            catch (Exception ex)
            {
                passed = false;
                if (outputLevel >= 0)
                {
                    Console.WriteLine(Environment.NewLine + "ERROR: Exception throwin in the LU test." +
                        Environment.NewLine + "  Message: " + ex.Message);
                }
            }
            if (outputLevel >= 1)
            {
                Console.WriteLine(Environment.NewLine + "Total time spent for test operations: "
                    + t.TotalTime.ToString("G4") + " s (CPU: " + t.TotalCpuTime.ToString("G4") + " s)." + Environment.NewLine);
                if (passed)
                    Console.WriteLine(Environment.NewLine + "Test of LU decomposition completed SUCCESSFULLY." + Environment.NewLine);
                else
                    Console.WriteLine(Environment.NewLine + "Test of LU decomposition completed with ERRORS." + Environment.NewLine);
            }
            return passed;
        }  // TestLuDecomposition()

        /// <summary>Demonstration of usae of LU decomposition.</summary>
        /// $A Igor Dec14;
        public static void TestLuDecompositionDemo()
        {
            Console.WriteLine(Environment.NewLine + "Begin LU decomposition demo." + Environment.NewLine);

            int dim = 4;
            IMatrix m = new Matrix(dim, dim);
            m[0, 0] = 3.0; m[0, 1] = 7.0; m[0, 2] = 2.0; m[0, 3] = 5.0;
            m[1, 0] = 1.0; m[1, 1] = 8.0; m[1, 2] = 4.0; m[1, 3] = 2.0;
            m[2, 0] = 2.0; m[2, 1] = 1.0; m[2, 2] = 9.0; m[2, 3] = 3.0;
            m[3, 0] = 5.0; m[3, 1] = 4.0; m[3, 2] = 7.0; m[3, 3] = 1.0;

            Console.WriteLine("System matrix = " + Environment.NewLine + m.ToStringReadable());

            int[] perm = new int[dim];
            int toggle;
            IMatrix luMatrix = m.GetNew(dim, dim);
            LuDecompose(m, out toggle, ref perm, ref luMatrix);

            IMatrix lower = null;
            IMatrix upper = null;

            LuExtractLower(luMatrix, ref lower);
            LuExtractUpper(luMatrix, ref upper);

            Console.WriteLine("The (combined) LUP decomposition of m is" + Environment.NewLine + luMatrix.ToStringReadable());
            Console.WriteLine("The decomposition permutation array is: " + perm.ToString());
            Console.WriteLine("The decomposition toggle value is: " + toggle);
            Console.WriteLine(Environment.NewLine + "The lower part of LU is " + Environment.NewLine + lower.ToStringReadable());
            Console.WriteLine("The upper part of LU is " + Environment.NewLine + upper.ToStringReadable());

            IVector auxRight = null;
            IVector auxX = null;

            // Cslvulsyion of inverse:
            IMatrix inverse = null;
            LuInverse(luMatrix, perm, ref auxRight, ref auxX, ref inverse);
            Console.WriteLine("Inverse of m computed from its decomposition is " + Environment.NewLine + inverse.ToStringReadable());
            IMatrix prod = m.GetNew();
            MatrixBase.Multiply(inverse, m, ref prod);
            // Console.WriteLine("Product of matrix and its inverse is\n" + MatrixAsString(prod));
            // Test inverse correctness:
            IMatrix diff = m.GetNew();
            MatrixBase.SetIdentity(diff);
            MatrixBase.Subtract(prod, diff, ref diff);
            Console.WriteLine("Error norm for matrix inverse: " + (diff.NormForbenius).ToString() + Environment.NewLine);

            // Calculate inverse in another way - by solbving multiple systems of equations with right-hand 
            //  sides defined by identity matrix:

            inverse = null;
            IVector auxVec = null;
            IMatrix B = m.GetNew(dim, dim);
            MatrixBase.SetIdentity(B);
            LuSolve(luMatrix, perm, B, ref auxVec, ref auxRight, ref auxX, ref inverse);
            Console.WriteLine("Inverse of m computed by explicit equation solving (identity right-hand) is " + Environment.NewLine + inverse.ToStringReadable());
            prod = null;
            MatrixBase.Multiply(inverse, m, ref prod);
            // Test inverse correctness:
            MatrixBase.SetIdentity(diff);
            MatrixBase.Subtract(prod, diff, ref diff);
            Console.WriteLine("Inverse by solving with matrix right-hand sides, error norm: " + (diff.NormForbenius).ToString() + Environment.NewLine);

            double det = LuDeterminant(luMatrix, toggle);
            Console.WriteLine("Determinant of m computed via decomposition = " + det.ToString("F1"));

            double[] bArray = new double[] { 49.0, 30.0, 43.0, 52.0 };

            IVector b = new Vector(bArray);
            Console.WriteLine(Environment.NewLine + "Right-hand side vector: " + b.ToStringMath());

            Console.WriteLine("Solving system via decomposition...");
            IVector x = null;
            LuSolve(luMatrix, perm, b, ref auxRight, ref x);

            Console.WriteLine(Environment.NewLine + "Solution is x = " + x.ToStringMath());

            // Insight in matrix decomposition concepts:
            IMatrix lu = null;
            Multiply(lower, upper, ref lu);
            IMatrix orig = null;
            int[] auxArray = null;
            UnPermute(lu, perm, ref auxArray, ref orig);  // use a custom method with the perm array to unscramble LU
            MatrixBase.Subtract(orig, m, ref diff);
            if (diff.NormForbenius < 0.000000001)
                Console.WriteLine("\nProduct of L and U successfully unpermuted using perm array.");
            else
                Console.WriteLine("\nPruduct of L and U unpermuted UNSUCCESSFULLY using perm array.");

            IMatrix permMatrix = m.GetNew(dim, dim);
            PermutationArrayToMatrix(perm, ref permMatrix); // convert the perm array to a matrix
            MatrixBase.Multiply(permMatrix, lu, ref orig); // another way to unscramble
            MatrixBase.Subtract(orig, m, ref diff);
            if (diff.NormForbenius < 0.000000001)
                Console.WriteLine("\nProduct of L and U successfully unpermuted using perm matrix.");
            else
                Console.WriteLine("\nPruduct of L and U unpermuted UNSUCCESSFULLY using permutation matrix.");

            Console.WriteLine("\nEnd of matrix decomposition demo.\n");
            // Console.ReadLine();
        }  // TestLuDecompositionDemo()

        #endregion LuDecomposition.Tests


        #endregion LuDecomposition


        #region LdltDecomposition


        /// <summary>Calculates LDLT decomposition of a real symmetric square matrix.
        /// <para>L is lower triangular matrix with 1s on diagonal, and D is a diagonal matrix.</para>
        /// <para>Decomposition can be done in place.</para>
        /// <para>Can be done in place (input and result matrices can reference the same object).</para></summary>
        /// <param name="A">Matrix whose decomposition is calculated.</param>
        /// <param name="tol">Tolerance for detection of singularity (must be a small positive number greater or equal to 0).</param>
        /// <param name="result">Matrix where the result of calculation is stored.</param>
        /// $A Igor Dec14;
        public static bool LdltDecompose(IMatrix A, ref IMatrix result, double tol = 1e-12)
        {
            if (tol <= 0)
                tol = 1.0e-12;
            if (A == null)
                throw new ArgumentException("Matrix to be LDLT decomposed is not specified (null reference).");
            int dim = A.RowCount;
            if (A.ColumnCount != dim)
                throw new ArgumentException("Matrix to be LDLT decomposed is not a square matrix.");
            if (result == null)
                result = A.GetNew(dim, dim);
            if (result.RowCount != dim || result.ColumnCount != dim)
                result = A.GetNew(dim, dim);

            if (!object.ReferenceEquals(A, result))
                MatrixBase.CopyPlain(A, result);
            for (int j = 0; j < dim; j++)
            {
                double dj = result[dim * j + j];
                for (int k = 0; k < j; k++)
                {
                    double ajk = result[j * dim + k];
                    dj -= ajk * ajk * result[dim * k + k];
                }

                result[dim * j + j] = dj;
                if (Math.Abs(dj) < tol)
                    return false;

                double djInv = 1.0 / dj;

                for (int i = j + 1; i < dim; i++)
                {
                    double lij = result[i * dim + j];
                    for (int k = 0; k < j; k++)
                        lij -= result[i * dim + k] * result[j * dim + k] * result[k * dim + k];

                    double element = lij * djInv;
                    result[i * dim + j] = element;
                    result[j * dim + i] = element;  // also populate symmetric elment
                }
            }
            //for (int i = 0; i < dim; i++)
            //{
            //    // Symmetrically populate above diagonal:
            //    for (int j = i + 1; j < dim; ++j)
            //    {
            //        result[i, j] = result[j, i];
            //    }
            //}

            return true;
        }

        /// <summary>Solves a system of eauations with the specified LDLT decomposition of a real symmetric square matrix.
        /// <para>Used in conjunction with the <see cref="LdltDecompose"/> method for calculation of decomposition.</para>
        /// <para>Can be done in place (input and result vectors can reference the same object).</para></summary>
        /// <param name="decomposed">Decomposed original matrix (obtained by <see cref="LdltDecompose"/>).
        /// <para>Matrix is in form of 1D flat arrat with row-wise element arrangement.</para></param>
        /// <param name="b">Vector of the right-hand sides of the system of equations.</param>
        /// <param name="x">Vector where result is stored.</param>
        /// $A Igor Dec14;
        public static void LdltSolve(IMatrix decomposed, IVector b, ref IVector x)
        {
            if (decomposed == null)
                throw new ArgumentException("LDLT decomposed matrix not specified (null reference).");
            int dim = decomposed.RowCount;
            if (decomposed.ColumnCount != dim)
                throw new ArgumentException("Decomposed matrix is not a square matrix.");
            if (b == null)
                throw new ArgumentException("Vector of right-hand sides is not specified (null reference).");
            if (b.Length != dim)
                throw new ArgumentException("Vector of right-hand sides is of inconsistent dimentions.");
            if (x == null)
                x = b.GetNew(dim);
            else if (x.Length != dim)
                x = b.GetNew(dim);
            if (!object.ReferenceEquals(x, b))
                VectorBase.Copy(b, x);
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < i; j++)
                    x[i] -= x[j] * decomposed[i * dim + j];
            }

            for (int i = 0; i < dim; i++)
                x[i] /= decomposed[i * dim + i];

            for (int i = dim; i >= 0; i--)
            {
                for (int j = i + 1; j < dim; j++)
                    x[i] -= x[j] * decomposed[j * dim + i];
            }
        }


        /// <summary>Calculates inverse of the matrix from its specified LDLT-decomposed matrix.</summary>
        /// <param name="ldltMatrix">Matrix containing the LDLT decomposition of the original matrix.</param>
        /// <param name="B">Matrix whose columns are right-hand sides of equations to be solved.</param>
        /// <param name="auxX">Auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="X">Matrix where result will be stored. Reallocated if necessary.</param>
        /// $A Igor Dec14;
        public static void LdltSolve(IMatrix ldltMatrix, IMatrix B, ref IVector auxX, ref IMatrix X)
        {
            if (ldltMatrix == null)
                throw new ArgumentException("Matrix containing LDLT decomposition is not specified (null reference).");
            int dim = ldltMatrix.RowCount;
            if (B == null)
                throw new ArgumentException("Matrix of right-hand sides is not specified (null reference).");
            if (B.RowCount != dim)
                throw new ArgumentException("Matrix of right-hand sides does not have as many rows as there are equations.");
            int numSystems = B.ColumnCount;
            if (ldltMatrix.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LU decomposition is not a square matrix.");
            if (auxX == null)
                auxX = ldltMatrix.GetNewVector(dim);
            if (auxX.Length != dim)
                auxX = ldltMatrix.GetNewVector(dim);
            if (X == null)
                X = ldltMatrix.GetNew(dim, numSystems);
            if (X.RowCount != dim || X.ColumnCount != numSystems)
                X = ldltMatrix.GetNew(dim, numSystems);
            if (object.ReferenceEquals(ldltMatrix, X) || object.ReferenceEquals(ldltMatrix, B) || object.ReferenceEquals(B, X))
                throw new ArgumentException("Input matrix the same as result matrix. Can not be done in place to such extent.");
            for (int whichSystem = 0; whichSystem < numSystems; ++whichSystem)
            {
                // Get column of B as right-hand sides:
                for (int j = 0; j < dim; ++j)
                    auxX[j] = B[j, whichSystem];
                // Solve the system with this vector:
                LdltSolve(ldltMatrix, auxX, ref auxX);
                for (int j = 0; j < dim; ++j)
                    X[j, whichSystem] = auxX[j];
            }
        }

        /// <summary>Calculates inverse of the matrix from its specified LDLT decomposition.</summary>
        /// <param name="ldltMatrix">Matrix containing the LDLT decomposition of the original matrix (with partial pivoting).</param>
        /// <param name="auxX">Another auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="res">Matrix where result will be stored. Reallocated if necessary.</param>
        /// $A Igor Dec14;
        public static void LdltInverse(IMatrix ldltMatrix, ref IVector auxX, ref IMatrix res)
        {
            if (ldltMatrix == null)
                throw new ArgumentException("Matrix containing LDLT decomposition is not specified (null reference).");
            int dim = ldltMatrix.RowCount;
            if (ldltMatrix.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LDLT decomposition is not a square matrix.");
            if (auxX == null)
                auxX = ldltMatrix.GetNewVector(dim);
            if (auxX.Length != dim)
                auxX = ldltMatrix.GetNewVector(dim);
            if (res == null)
                res = ldltMatrix.GetNew(dim, dim);
            if (res.RowCount != dim || res.ColumnCount != dim)
                res = ldltMatrix.GetNew(dim, dim);
            if (object.ReferenceEquals(ldltMatrix, res))
                throw new ArgumentException("Input matrix the same as result matrix. Can not be done in place to this extent.");
            for (int whichSystem = 0; whichSystem < dim; ++whichSystem)
            {
                // Get column of B as right-hand sides:
                for (int j = 0; j < dim; ++j)
                {
                    // make right-hand vector contain a column of identity matrix:
                    if (j != whichSystem)
                        auxX[j] = 0.0;
                    else
                        auxX[j] = 1.0;
                }
                // Solve the system with this vector:
                LdltSolve(ldltMatrix, auxX, ref auxX);
                for (int j = 0; j < dim; ++j)
                    res[j, whichSystem] = auxX[j];
            }
        }



        /// <summary>Calculates and returns determinant of a square symmetric matrix form its specified LDLT decomposition.</summary>
        /// <param name="ldltMatrix">Matrix containing the LDLT decomposition of the matrix whose determinant is to be calculated.</param>
        /// <returns>Determinant calculated from the precalculated LDLT decomposition of a symmetric matrix.</returns>
        /// $A Igor Dec14;
        public static double LdltDeterminant(IMatrix ldltMatrix)
        {
            if (ldltMatrix == null)
                throw new ArgumentException("Determinant calculation: Matrix containing LDLT decomposition is not specified.");
            int dim = ldltMatrix.RowCount;
            if (ldltMatrix.ColumnCount != dim)
                throw new ArgumentException("Determinant calculation: Matrix containing LDLT decomposition is not a square matrix.");
            double result = 1.0;
            for (int i = 0; i < dim; ++i)
                result *= ldltMatrix[i, i];
            return result;
        }


        /// <summary>Extracts the lower part of the specified LDLT decomposition (1s on diagonal, 0s above diagonal)
        /// and stores it in the specified result matrix.
        /// <para>Although operatioin can be done in place, it is not allowed for input and output matrix to be the same.
        /// Reason is that the operation always done in in combination (with extracting all parts).</para></summary>
        /// <param name="matLdlt">Matrix containng the LDLT decomposition of some matrix.</param>
        /// <param name="result">Matrix where the lower part of the decomposition is stored.</param>
        /// $A Igor Dec14;
        public static void LdltExtractLower(IMatrix matLdlt, ref IMatrix result)
        {
            if (matLdlt == null)
                throw new ArgumentException("Marix containing LDLT decomposed original is not specified (null reference).");
            int dim = matLdlt.RowCount;
            if (matLdlt.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LDLT decomposition is not a square matrix.");
            if (result == null)
                result = matLdlt.GetNew(dim, dim);
            if (result.RowCount != dim || result.ColumnCount != dim)
                result = matLdlt.GetNew(dim, dim);
            if (object.ReferenceEquals(matLdlt, result))
                throw new ArgumentException("Input and result matrix are the same, which is not allowed.");
            for (int i = 0; i < dim; ++i)
            {
                for (int j = 0; j < dim; ++j)
                {
                    if (i == j)
                        result[i, j] = 1.0;
                    else if (i > j)
                        result[i, j] = matLdlt[i, j];
                    else
                        result[i, j] = 0.0;
                }
            }
        }

        /// <summary>Extracts the upper part of the specified LDLT decomposition (1s on diagonal, 0s in below diagonal)
        /// and stores it in the specified result matrix.
        /// <para>Although operatioin can be done in place, it is not allowed for input and output matrix to be the same.
        /// Reason is that the operation always done in in combination (with extracting all parts).</para></summary>
        /// <param name="matLdlt">Matrix containng the LDLT decomposition of some matrix.</param>
        /// <param name="result">Matrix where the upper part of the specified matrix is stored.</param>
        /// $A Igor Dec14;
        public static IMatrix LdltExtractUpper(IMatrix matLdlt, ref IMatrix result)
        {
            if (matLdlt == null)
                throw new ArgumentException("Marix containing LDLT decomposed original is not specified (null reference).");
            int dim = matLdlt.RowCount;
            if (matLdlt.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LDLT decomposition is not a square matrix.");
            if (result == null)
                result = matLdlt.GetNew(dim, dim);
            if (result.RowCount != dim || result.ColumnCount != dim)
                result = matLdlt.GetNew(dim, dim);
            if (object.ReferenceEquals(matLdlt, result))
                throw new ArgumentException("Input and result matrix are the same, which is not allowed.");
            for (int i = 0; i < dim; ++i)
            {
                for (int j = 0; j < dim; ++j)
                {
                    if (i == j)
                        result[i, j] = 1.0;
                    else if (i < j)
                        result[i, j] = matLdlt[j, i];  // take from lower part; with current implementation, upper part can also be used.
                    else
                        result[i, j] = 0.0;
                }
            }
            return result;
        }

        /// <summary>Extracts the diagonal part of the specified LDLT decomposition 
        /// and stores it in the specified result matrix.
        /// <para>Although operatioin can be done in place, it is not allowed for input and output matrix to be the same.
        /// Reason is that the operation always done in in combination (with extracting all parts).</para></summary>
        /// <param name="matLdlt">Matrix containng the LDLT decomposition of some matrix.</param>
        /// <param name="result">Matrix where the upper part of the specified matrix is stored.</param>
        /// $A Igor Dec14;
        public static IMatrix LdltExtractDiagonal(IMatrix matLdlt, ref IMatrix result)
        {
            if (matLdlt == null)
                throw new ArgumentException("Marix containing LDLT decomposed original is not specified (null reference).");
            int dim = matLdlt.RowCount;
            if (matLdlt.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LDLT decomposition is not a square matrix.");
            if (result == null)
                result = matLdlt.GetNew(dim, dim);
            if (result.RowCount != dim || result.ColumnCount != dim)
                result = matLdlt.GetNew(dim, dim);
            if (object.ReferenceEquals(matLdlt, result))
                throw new ArgumentException("Input and result matrix are the same, which is not allowed.");
            for (int i = 0; i < dim; ++i)
            {
                for (int j = 0; j < dim; ++j)
                {
                    if (i == j)
                        result[i, j] = matLdlt[i, j];
                    else
                        result[i, j] = 0.0;
                }
            }
            return result;
        }


        #region LdltDecomposition.Tests


        /// <summary>Performs a test of calculatons performed via LDLT decomposition of a matrix.
        /// Calculation times and error extents are measured and reported (if specified).
        /// <para>If relative errors are below the specified tolerance, true is returned, otherwise false is returned.</para></summary>
        /// <param name="dim">Dimension of the problems generated for tests.</param>
        /// <param name="numRepetitions">Number of repetitions of the tests. If greater than 1 then tests are repeated
        /// with inputs generated anew each time.</param>
        /// <param name="tol">Tolerance on relative errors of test results.</param>
        /// <param name="outputLevel">Level of output. If 0 then no reports are launched to the console.</param>
        /// <param name="randomGenerator">Random generator used for generation of test inputs.</param>
        /// <param name="A">System matrix used in the test. If specified (i.e., not null) then this matrix is LDLT decomposed and
        /// used in the first repetition of tests instead of a randomly generated matrix. In this case, its dimension
        /// overrides (when not the same) the specified dimension <paramref name="dim"/> of test matrices and vectors.
        /// If there are more than one repetitions (parameter <paramref name="numRepetitions"/>) then subsequent repetitions
        /// still use randomly generated inputs. If specified (i.e., not null) then this vector is  used in the first repetition 
        /// of tests instead of a a randomly generated vector. Similar rules apply as for <paramref name="A"/>.</param>
        /// <param name="b">Vector of right-hand sides used in the test.</param>
        /// <returns>True if all tests passed successfully (i.e., if errors are below the specified tolerance).</returns>
        /// $A Igor Dec14;
        public static bool TestLdltDecomposition(int dim, int numRepetitions = 1, double tol = 1e-6, int outputLevel = 0,
            IRandomGenerator randomGenerator = null, IMatrix A = null, IVector b = null)
        {
            bool passed = true;
            if (tol <= 0)
                tol = 1.0e-6;
            double smallNumber = 1e-20;  // e.g. for guarding division by 0
            StopWatch1 t = new StopWatch1();
            try
            {
                double determinant = 0;
                if (randomGenerator == null)
                    randomGenerator = RandomGenerator.Global;
                if (numRepetitions < 1)
                    numRepetitions = 1;
                if (A != null)
                {
                    dim = A.RowCount;
                    if (!MatrixBase.IsSymmetric(A, tol))
                        throw new ArgumentException("The specified matrix for testing LDLT decomposition is not symmetric.");
                    determinant = DeterminantSlow(A);
                }
                else if (b != null)
                    dim = b.Length;
                if (outputLevel >= 1)
                {
                    if (numRepetitions < 2)
                        Console.WriteLine(Environment.NewLine + "Test of LDLT decomposition..." + Environment.NewLine);
                    else
                        Console.WriteLine(Environment.NewLine + "Test of LDLT decomposition (" + numRepetitions + " repetitions)..."
                            + Environment.NewLine);
                }
                for (int repetition = 0; repetition < numRepetitions; ++repetition)
                {
                    if (numRepetitions > 1 && outputLevel >= 1)
                    {
                        Console.WriteLine(Environment.NewLine + "REPETITION No. " + repetition + " of the LDLT decomposition test..." + Environment.NewLine);
                    }
                    t.Start();
                    if (A == null || repetition > 1)
                    {
                        A = new Matrix(dim, dim);
                        determinant = MatrixBase.SetRandomPositiveDefiniteSymmetric(A, randomGenerator);
                    }
                    if (b == null || repetition > 1)
                    {
                        b = new Vector(dim);
                        VectorBase.SetRandom(b);
                    }
                    if (A.RowCount != dim || A.ColumnCount != dim || b.Length != dim)
                        throw new ArgumentException("Provided matrix and/or right/hand sides are not of correct dimensions.");
                    t.Stop();
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Preparation of test input done in " + t.Time + "s (CPU: " + t.CpuTime + ").");
                        Console.WriteLine("Dimension of the matrix for LDLT testing decomposition: " + dim + "." + Environment.NewLine);
                    }
                    IMatrix LDLT = new Matrix(dim, dim);
                    t.Start();
                    LdltDecompose(A, ref LDLT);
                    t.Stop();
                    // Check the product:
                    IMatrix product = null;
                    IMatrix auxMat = null;
                    IMatrix diffMat = null;
                    IMatrix lower = null;
                    IMatrix upper = null;
                    IMatrix diagonal = null;
                    LdltExtractLower(LDLT, ref lower);
                    LdltExtractUpper(LDLT, ref upper);
                    LdltExtractDiagonal(LDLT, ref diagonal);
                    //MatrixBase.Multiply(lower, diagonal, upper, ref product);
                    //Console.WriteLine(Environment.NewLine + "Dedomposed matrices: \nLDLT: \n"
                    //    + LDLT.ToStringReadable() + "lower: \n" + lower.ToStringReadable() + "upper: \n" + upper.ToStringReadable()
                    //    + "diagonal: \n" + diagonal.ToStringReadable());
                    MatrixBase.Multiply(lower, diagonal, ref auxMat);
                    MatrixBase.Multiply(auxMat, upper, ref product);
                    //Console.WriteLine(Environment.NewLine + "Dedomposed matrices: \nLDLT: \n"
                    //    + LDLT.ToStringReadable() + "lower: \n" + lower.ToStringReadable() + "upper: \n" + upper.ToStringReadable()
                    //    + "diagonal: \n" + diagonal.ToStringReadable()
                    //    + "\nfirst product: \n" + auxMat.ToStringReadable() + "second product: \n" + product.ToStringReadable()
                    //    + "original: \n" + A.ToStringReadable());
                    MatrixBase.Subtract(product, A, ref diffMat);
                    // IMatrix restored = null;
                    double relativeError = diffMat.NormForbenius / (A.NormForbenius + smallNumber);
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("LDLT decompositin calculated in "
                            + t.Time.ToString("G2") + "s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: product of lower and upperr does not match original, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Product of factors: relative error = " + relativeError.ToString("G2"));
                    }
                    // Check soution of system of equations: 
                    IVector x = null;
                    IVector auxVec1 = null;
                    t.Start();
                    LdltSolve(LDLT, b, ref x);
                    t.Stop();
                    IVector testVec = null;
                    IVector diffVec = null;
                    MatrixBase.Multiply(A, x, ref testVec);
                    VectorBase.Subtract(testVec, b, ref diffVec);
                    relativeError = diffVec.Norm / (b.Norm + smallNumber);
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Equations solved (just back substitution) in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: soLDLTtion of system of equations not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Solution of system of equations: relative error = " + relativeError.ToString("G2"));
                    }

                    // Check calculation of determinant:
                    t.Start();
                    double detCalc = LdltDeterminant(LDLT);
                    t.Stop();
                    relativeError = Math.Abs(detCalc - determinant) / (Math.Abs(determinant) + smallNumber);
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Determinant caLDLTclated from decomposition in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: calculation of determinant not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Calculation of determinant from decomposition: relative error = " + relativeError.ToString("G2"));
                    }
                    // Check calculation of inverse matrix (directly):
                    IMatrix inverseMat = null;
                    t.Start();
                    LdltInverse(LDLT, ref auxVec1, ref inverseMat);
                    t.Stop();
                    IMatrix identityMat = new Matrix(dim, dim); MatrixBase.SetIdentity(identityMat);
                    MatrixBase.Multiply(A, inverseMat, ref product);
                    Matrix.Subtract(product, identityMat, ref diffMat);
                    relativeError = diffMat.NormForbenius / (Math.Sqrt((double)dim));
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Matrix inverse directly from decomposition in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: calculation of matrix inverse not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Matrix inverse directly from decomposition: relative error = " + relativeError.ToString("G2"));
                    }
                    // Check calculation of inverse matrix (through solutions of equations with identity right-hand side):
                    MatrixBase.SetZero(inverseMat);
                    t.Start();
                    LdltSolve(LDLT, identityMat, ref auxVec1, ref inverseMat);
                    t.Stop();
                    MatrixBase.Multiply(A, inverseMat, ref product);
                    Matrix.Subtract(product, identityMat, ref diffMat);
                    relativeError = diffMat.NormForbenius / (Math.Sqrt((double)dim));
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Matrix inverse through equation solving in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: calculation of matrix inverse through equation solving not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Matrix inverse through equation solving: relative error = " + relativeError.ToString("G2"));
                    }
                } // loop over repetitions
            }
            catch (Exception ex)
            {
                passed = false;
                if (outputLevel >= 0)
                {
                    Console.WriteLine(Environment.NewLine + "ERROR: Exception throwin in the LDLT test." +
                        Environment.NewLine + "  Message: " + ex.Message);
                }
            }
            if (outputLevel >= 1)
            {
                Console.WriteLine(Environment.NewLine + "Total time spent for test operations: "
                    + t.TotalTime.ToString("G4") + " s (CPU: " + t.TotalCpuTime.ToString("G4") + " s)." + Environment.NewLine);
                if (passed)
                    Console.WriteLine(Environment.NewLine + "Test of LDLT decomposition completed SUCCESSFULLY." + Environment.NewLine);
                else
                    Console.WriteLine(Environment.NewLine + "Test of LDLT decomposition completed with ERRORS." + Environment.NewLine);
            }
            return passed;
        }

        #endregion LdltDecomposition.Tests


        #endregion LdltDecomposition



        #region CholeskyDecomposition


        /// <summary>Calculates Cholesky decomposition of a real symmetric square matrix.
        /// <para>L is lower triangular matrix with 1s on diagonal, and D is a diagonal matrix.</para>
        /// <para>Decomposition can be done in place.</para>
        /// <para>Can be done in place (input and result matrices can reference the same object).</para></summary>
        /// <param name="A">Matrix whose decomposition is calculated.</param>
        /// <param name="tol">Tolerance for detection of singularity (must be a small positive number greater or equal to 0).</param>
        /// <param name="result">Matrix where the result of calculation is stored.</param>
        /// $A Igor Dec14;
        public static bool CholeskyDecompose(IMatrix A, ref IMatrix result, double tol = 1e-12)
        {
            if (tol <= 0)
                tol = 1.0e-12;
            if (A == null)
                throw new ArgumentException("Matrix to be Cholesky decomposed is not specified (null reference).");
            int dim = A.RowCount;
            if (A.ColumnCount != dim)
                throw new ArgumentException("Matrix to be Cholesky decomposed is not a square matrix.");
            if (result == null)
                result = A.GetNew(dim, dim);
            if (result.RowCount != dim || result.ColumnCount != dim)
                result = A.GetNew(dim, dim);

            if (!object.ReferenceEquals(A, result))
                MatrixBase.CopyPlain(A, result);

            for (int i = 0; i < dim; i++)
            {
                // Working on the i-th column:
                // Determine the i-th diagonal element:
                double q = A[i, i];
                for (int j = 0; j < i; j++)
                {
                    double el = result[i, j];
                    q -= el * el;
                }
                if (q < tol)
                    throw new InvalidOperationException("Matrix whose Cholesky decomposition is calculated is singular.");
                q = Math.Sqrt(q);
                result[i, i] = q;
                // Determine lower entries:
                for (int j = i + 1; j < dim; j++)
                {
                    double p = A[j, i];
                    for (int k = 0; k < i; k++)
                    {
                        p -= result[i, k] * result[j, k];
                    }
                    double el = p / q;
                    result[j, i] = el;
                    result[i, j] = el;
                }
            }

            return true;
        }

        /// <summary>Solves a system of eauations with the specified Cholesky decomposition of a real symmetric square matrix.
        /// <para>Used in conjunction with the <see cref="CholeskyDecompose"/> method for calculation of decomposition.</para>
        /// <para>Can be done in place (input and result vectors can reference the same object).</para></summary>
        /// <param name="decomposed">Decomposed original matrix (obtained by <see cref="CholeskyDecompose"/>).
        /// <para>Matrix is in form of 1D flat arrat with row-wise element arrangement.</para></param>
        /// <param name="b">Vector of the right-hand sides of the system of equations.</param>
        /// <param name="x">Vector where result is stored.</param>
        /// $A Igor Dec14;
        public static void CholeskySolve(IMatrix decomposed, IVector b, ref IVector x)
        {
            if (decomposed == null)
                throw new ArgumentException("Cholesky decomposed matrix not specified (null reference).");
            int dim = decomposed.RowCount;
            if (decomposed.ColumnCount != dim)
                throw new ArgumentException("Decomposed matrix is not a square matrix.");
            if (b == null)
                throw new ArgumentException("Vector of right-hand sides is not specified (null reference).");
            if (b.Length != dim)
                throw new ArgumentException("Vector of right-hand sides is of inconsistent dimentions.");
            if (x == null)
                x = b.GetNew(dim);
            else if (x.Length != dim)
                x = b.GetNew(dim);
            if (!object.ReferenceEquals(x, b))
                VectorBase.Copy(b, x);


            // Determine Ly = b
            IVector y = decomposed.GetNewVector(dim);
            for (int i = 0; i < dim; i++)
            {
                y[i] = b[i];
                for (int j = 0; j < i; j++)
                {
                    y[i] -= decomposed[j, i] * y[j];
                }
                y[i] = y[i] / decomposed[i, i];
            }

            // Determine L^T x = y
            // double[] z = new double[dim];

            // Re-use y for z, since we don't need a value after it's set
            for (int i = (dim - 1); i >= 0; i--)
            {
                x[i] = y[i];
                for (int j = (dim - 1); j > i; j--)
                {
                    x[i] -= decomposed[i, j] * x[j];
                }
                x[i] = x[i] / decomposed[i, i];
            }



        }

        /// <summary>Calculates inverse of the matrix from its specified Cholesky-decomposed matrix.</summary>
        /// <param name="CholeskyMatrix">Matrix containing the Cholesky decomposition of the original matrix.</param>
        /// <param name="B">Matrix whose columns are right-hand sides of equations to be solved.</param>
        /// <param name="auxX">Auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="X">Matrix where result will be stored. Reallocated if necessary.</param>
        /// $A Igor Dec14;
        public static void CholeskySolve(IMatrix CholeskyMatrix, IMatrix B, ref IVector auxX, ref IMatrix X)
        {
            if (CholeskyMatrix == null)
                throw new ArgumentException("Matrix containing Cholesky decomposition is not specified (null reference).");
            int dim = CholeskyMatrix.RowCount;
            if (B == null)
                throw new ArgumentException("Matrix of right-hand sides is not specified (null reference).");
            if (B.RowCount != dim)
                throw new ArgumentException("Matrix of right-hand sides does not have as many rows as there are equations.");
            int numSystems = B.ColumnCount;
            if (CholeskyMatrix.ColumnCount != dim)
                throw new ArgumentException("Matrix containing LU decomposition is not a square matrix.");
            if (auxX == null)
                auxX = CholeskyMatrix.GetNewVector(dim);
            if (auxX.Length != dim)
                auxX = CholeskyMatrix.GetNewVector(dim);
            if (X == null)
                X = CholeskyMatrix.GetNew(dim, numSystems);
            if (X.RowCount != dim || X.ColumnCount != numSystems)
                X = CholeskyMatrix.GetNew(dim, numSystems);
            if (object.ReferenceEquals(CholeskyMatrix, X) || object.ReferenceEquals(CholeskyMatrix, B) || object.ReferenceEquals(B, X))
                throw new ArgumentException("Input matrix the same as result matrix. Can not be done in place to such extent.");
            for (int whichSystem = 0; whichSystem < numSystems; ++whichSystem)
            {
                // Get column of B as right-hand sides:
                for (int j = 0; j < dim; ++j)
                    auxX[j] = B[j, whichSystem];
                // Solve the system with this vector:
                CholeskySolve(CholeskyMatrix, auxX, ref auxX);
                for (int j = 0; j < dim; ++j)
                    X[j, whichSystem] = auxX[j];
            }
        }

        /// <summary>Calculates inverse of the matrix from its specified Cholesky decomposition.</summary>
        /// <param name="CholeskyMatrix">Matrix containing the Cholesky decomposition of the original matrix (with partial pivoting).</param>
        /// <param name="auxX">Another auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="res">Matrix where result will be stored. Reallocated if necessary.</param>
        /// $A Igor Dec14;
        public static void CholeskyInverse(IMatrix CholeskyMatrix, ref IVector auxX, ref IMatrix res)
        {
            if (CholeskyMatrix == null)
                throw new ArgumentException("Matrix containing Cholesky decomposition is not specified (null reference).");
            int dim = CholeskyMatrix.RowCount;
            if (CholeskyMatrix.ColumnCount != dim)
                throw new ArgumentException("Matrix containing Cholesky decomposition is not a square matrix.");
            if (auxX == null)
                auxX = CholeskyMatrix.GetNewVector(dim);
            if (auxX.Length != dim)
                auxX = CholeskyMatrix.GetNewVector(dim);
            if (res == null)
                res = CholeskyMatrix.GetNew(dim, dim);
            if (res.RowCount != dim || res.ColumnCount != dim)
                res = CholeskyMatrix.GetNew(dim, dim);
            if (object.ReferenceEquals(CholeskyMatrix, res))
                throw new ArgumentException("Input matrix the same as result matrix. Can not be done in place to this extent.");
            for (int whichSystem = 0; whichSystem < dim; ++whichSystem)
            {
                // Get column of B as right-hand sides:
                for (int j = 0; j < dim; ++j)
                {
                    // make right-hand vector contain a column of identity matrix:
                    if (j != whichSystem)
                        auxX[j] = 0.0;
                    else
                        auxX[j] = 1.0;
                }
                // Solve the system with this vector:
                CholeskySolve(CholeskyMatrix, auxX, ref auxX);
                for (int j = 0; j < dim; ++j)
                    res[j, whichSystem] = auxX[j];
            }
        }

        /// <summary>Calculates and returns determinant of a square symmetric matrix form its specified Cholesky decomposition.</summary>
        /// <param name="CholeskyMatrix">Matrix containing the Cholesky decomposition of the matrix whose determinant is to be calculated.</param>
        /// <returns>Determinant calculated from the precalculated Cholesky decomposition of a symmetric matrix.</returns>
        /// $A Igor Dec14;
        public static double CholeskyDeterminant(IMatrix CholeskyMatrix)
        {
            if (CholeskyMatrix == null)
                throw new ArgumentException("Determinant calculation: Matrix containing Cholesky decomposition is not specified.");
            int dim = CholeskyMatrix.RowCount;
            if (CholeskyMatrix.ColumnCount != dim)
                throw new ArgumentException("Determinant calculation: Matrix containing Cholesky decomposition is not a square matrix.");
            double result = 1.0;
            for (int i = 0; i < dim; ++i)
                result *= CholeskyMatrix[i, i];
            result *= result;  // there are two factors with the same diagonal
            return result;
        }

        /// <summary>Extracts the lower part of the specified Cholesky decomposition (0s above diagonal)
        /// and stores it in the specified result matrix.
        /// <para>Although operatioin can be done in place, it is not allowed for input and output matrix to be the same.
        /// Reason is that the operation always done in in combination (with extracting all parts).</para></summary>
        /// <param name="matCholesky">Matrix containng the Cholesky decomposition of some matrix.</param>
        /// <param name="result">Matrix where the lower part of the decomposition is stored.</param>
        /// $A Igor Dec14;
        public static void CholeskyExtractLower(IMatrix matCholesky, ref IMatrix result)
        {
            if (matCholesky == null)
                throw new ArgumentException("Marix containing Cholesky decomposed original is not specified (null reference).");
            int dim = matCholesky.RowCount;
            if (matCholesky.ColumnCount != dim)
                throw new ArgumentException("Matrix containing Cholesky decomposition is not a square matrix.");
            if (result == null)
                result = matCholesky.GetNew(dim, dim);
            if (result.RowCount != dim || result.ColumnCount != dim)
                result = matCholesky.GetNew(dim, dim);
            if (object.ReferenceEquals(matCholesky, result))
                throw new ArgumentException("Input and result matrix are the same, which is not allowed.");
            for (int i = 0; i < dim; ++i)
            {
                for (int j = 0; j < dim; ++j)
                {
                    if (i >= j)
                        result[i, j] = matCholesky[i, j];
                    else
                        result[i, j] = 0.0;
                }
            }
        }

        /// <summary>Extracts the upper part of the specified Cholesky decomposition (0s in below diagonal)
        /// and stores it in the specified result matrix.
        /// <para>Although operatioin can be done in place, it is not allowed for input and output matrix to be the same.
        /// Reason is that the operation always done in in combination (with extracting all parts).</para></summary>
        /// <param name="matCholesky">Matrix containng the Cholesky decomposition of some matrix.</param>
        /// <param name="result">Matrix where the upper part of the specified matrix is stored.</param>
        /// $A Igor Dec14;
        public static IMatrix CholeskyExtractUpper(IMatrix matCholesky, ref IMatrix result)
        {
            if (matCholesky == null)
                throw new ArgumentException("Marix containing Cholesky decomposed original is not specified (null reference).");
            int dim = matCholesky.RowCount;
            if (matCholesky.ColumnCount != dim)
                throw new ArgumentException("Matrix containing Cholesky decomposition is not a square matrix.");
            if (result == null)
                result = matCholesky.GetNew(dim, dim);
            if (result.RowCount != dim || result.ColumnCount != dim)
                result = matCholesky.GetNew(dim, dim);
            if (object.ReferenceEquals(matCholesky, result))
                throw new ArgumentException("Input and result matrix are the same, which is not allowed.");
            for (int i = 0; i < dim; ++i)
            {
                for (int j = 0; j < dim; ++j)
                {
                    if (i <= j)
                        result[i, j] = matCholesky[j, i];  // take from lower part; with current implementation, upper part can also be used.
                    else
                        result[i, j] = 0.0;
                }
            }
            return result;
        }


        #region CholeskyDecomposition.Tests


        /// <summary>Performs test of calculatons performed via Cholesky decomposition of a matrix.
        /// Calculation times and error extents are measured and reported (if specified so).
        /// <para>If relative errors are below the specified tolerance, true is returned, otherwise false is returned.</para></summary>
        /// <param name="dim">Dimension of the problems generated for tests.</param>
        /// <param name="numRepetitions">Number of repetitions of the tests. If greater than 1 then tests are repeated
        /// with inputs generated anew each time.</param>
        /// <param name="tol">Tolerance on relative errors of test results.</param>
        /// <param name="outputLevel">Level of output. If 0 then no reports are launched to the console.</param>
        /// <param name="randomGenerator">Random generator used for generation of test inputs.</param>
        /// <param name="A">System matrix used in the test. If specified (i.e., not null) then this matrix is Cholesky decomposed and
        /// used in the first repetition of tests instead of a randomly generated matrix. In this case, its dimension
        /// overrides (when not the same) the specified dimension <paramref name="dim"/> of test matrices and vectors.
        /// If there are more than one repetitions (parameter <paramref name="numRepetitions"/>) then subsequent repetitions
        /// still use randomly generated inputs. If specified (i.e., not null) then this vector is  used in the first repetition 
        /// of tests instead of a a randomly generated vector. Similar rules apply as for <paramref name="A"/>.</param>
        /// <param name="b">Vector of right-hand sides used in the test.</param>
        /// <returns>True if all tests passed successfully (i.e., if errors are below the specified tolerance).</returns>
        /// $A Igor Dec14;
        public static bool TestCholeskyDecomposition(int dim, int numRepetitions = 1, double tol = 1e-6, int outputLevel = 0,
            IRandomGenerator randomGenerator = null, IMatrix A = null, IVector b = null)
        {
            bool passed = true;
            if (tol <= 0)
                tol = 1.0e-6;
            double smallNumber = 1e-20;  // e.g. for guarding division by 0
            StopWatch1 t = new StopWatch1();
            try
            {
                double determinant = 0;
                if (randomGenerator == null)
                    randomGenerator = RandomGenerator.Global;
                if (numRepetitions < 1)
                    numRepetitions = 1;
                if (A != null)
                {
                    dim = A.RowCount;
                    if (!MatrixBase.IsSymmetric(A, tol))
                        throw new ArgumentException("The specified matrix for testing Cholesky decomposition is not symmetric.");
                    determinant = DeterminantSlow(A);
                }
                else if (b != null)
                    dim = b.Length;
                if (outputLevel >= 1)
                {
                    if (numRepetitions < 2)
                        Console.WriteLine(Environment.NewLine + "Test of Cholesky decomposition..." + Environment.NewLine);
                    else
                        Console.WriteLine(Environment.NewLine + "Test of Cholesky decomposition (" + numRepetitions + " repetitions)..."
                            + Environment.NewLine);
                }
                for (int repetition = 0; repetition < numRepetitions; ++repetition)
                {
                    if (numRepetitions > 1 && outputLevel >= 1)
                    {
                        Console.WriteLine(Environment.NewLine + "REPETITION No. " + repetition + " of the Cholesky decomposition test..." + Environment.NewLine);
                    }
                    t.Start();
                    if (A == null || repetition > 1)
                    {
                        A = new Matrix(dim, dim);
                        determinant = MatrixBase.SetRandomPositiveDefiniteSymmetric(A, randomGenerator);
                    }
                    if (b == null || repetition > 1)
                    {
                        b = new Vector(dim);
                        VectorBase.SetRandom(b);
                    }
                    if (A.RowCount != dim || A.ColumnCount != dim || b.Length != dim)
                        throw new ArgumentException("Provided matrix and/or right/hand sides are not of correct dimensions.");
                    t.Stop();
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Preparation of test input done in " + t.Time + "s (CPU: " + t.CpuTime + ").");
                        Console.WriteLine("Dimension of the matrix for Cholesky testing decomposition: " + dim + "." + Environment.NewLine);
                    }
                    IMatrix Cholesky = new Matrix(dim, dim);
                    t.Start();
                    CholeskyDecompose(A, ref Cholesky);
                    t.Stop();
                    // Check the product:
                    IMatrix product = null;
                    IMatrix diffMat = null;
                    IMatrix lower = null;
                    IMatrix upper = null;
                    CholeskyExtractLower(Cholesky, ref lower);
                    CholeskyExtractUpper(Cholesky, ref upper);
                    MatrixBase.Multiply(lower, upper, ref product);
                    //Console.WriteLine(Environment.NewLine + "Dedomposed matrices: \nCholesky: \n"
                    //    + Cholesky.ToStringReadable() + "lower: \n" + lower.ToStringReadable() + "upper: \n" + upper.ToStringReadable());
                    //Console.WriteLine(Environment.NewLine + "Dedomposed matrices: \nCholesky: \n"
                    //    + Cholesky.ToStringReadable() + "lower: \n" + lower.ToStringReadable() + "upper: \n" + upper.ToStringReadable()
                    //    + "product: \n" + product.ToStringReadable()
                    //    + "original: \n" + A.ToStringReadable());
                    MatrixBase.Subtract(product, A, ref diffMat);
                    // IMatrix restored = null;
                    double relativeError = diffMat.NormForbenius / (A.NormForbenius + smallNumber);
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Cholesky decompositin calculated in "
                            + t.Time.ToString("G2") + "s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: product of lower and upperr does not match original, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Product of factors: relative error = " + relativeError.ToString("G2"));
                    }
                    // Check soution of system of equations: 
                    IVector x = null;
                    IVector auxVec1 = null;
                    t.Start();
                    CholeskySolve(Cholesky, b, ref x);
                    t.Stop();
                    IVector testVec = null;
                    IVector diffVec = null;
                    MatrixBase.Multiply(A, x, ref testVec);
                    VectorBase.Subtract(testVec, b, ref diffVec);
                    relativeError = diffVec.Norm / (b.Norm + smallNumber);
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Equations solved (just back substitution) in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: soCholeskytion of system of equations not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Solution of system of equations: relative error = " + relativeError.ToString("G2"));
                    }

                    // Check calculation of determinant:
                    t.Start();
                    double detCalc = CholeskyDeterminant(Cholesky);
                    t.Stop();
                    relativeError = Math.Abs(detCalc - determinant) / (Math.Abs(determinant) + smallNumber);
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Determinant caCholeskyclated from decomposition in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: calculation of determinant not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Calculation of determinant from decomposition: relative error = " + relativeError.ToString("G2"));
                    }
                    // Check calculation of inverse matrix (directly):
                    IMatrix inverseMat = null;
                    t.Start();
                    CholeskyInverse(Cholesky, ref auxVec1, ref inverseMat);
                    t.Stop();
                    IMatrix identityMat = new Matrix(dim, dim); MatrixBase.SetIdentity(identityMat);
                    MatrixBase.Multiply(A, inverseMat, ref product);
                    Matrix.Subtract(product, identityMat, ref diffMat);
                    relativeError = diffMat.NormForbenius / (Math.Sqrt((double)dim));
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Matrix inverse directly from decomposition in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: calculation of matrix inverse not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Matrix inverse directly from decomposition: relative error = " + relativeError.ToString("G2"));
                    }
                    // Check calculation of inverse matrix (through solutions of equations with identity right-hand side):
                    MatrixBase.SetZero(inverseMat);
                    t.Start();
                    CholeskySolve(Cholesky, identityMat, ref auxVec1, ref inverseMat);
                    t.Stop();
                    MatrixBase.Multiply(A, inverseMat, ref product);
                    Matrix.Subtract(product, identityMat, ref diffMat);
                    relativeError = diffMat.NormForbenius / (Math.Sqrt((double)dim));
                    if (relativeError > tol)
                        passed = false;
                    if (outputLevel >= 0)
                    {
                        Console.WriteLine("Matrix inverse through equation solving in "
                            + t.Time.ToString("G2") + " s (CPU: " + t.CpuTime.ToString("G2") + " s).");
                        if (relativeError > tol)
                            Console.WriteLine(Environment.NewLine + "ERROR: calculation of matrix inverse through equation solving not correct, relative error = "
                                + relativeError + Environment.NewLine);
                        Console.WriteLine("Matrix inverse through equation solving: relative error = " + relativeError.ToString("G2"));
                    }
                } // loop over repetitions
            }
            catch (Exception ex)
            {
                passed = false;
                if (outputLevel >= 0)
                {
                    Console.WriteLine(Environment.NewLine + "ERROR: Exception throwin in the Cholesky test." +
                        Environment.NewLine + "  Message: " + ex.Message);
                }
            }
            if (outputLevel >= 1)
            {
                Console.WriteLine(Environment.NewLine + "Total time spent for test operations: "
                    + t.TotalTime.ToString("G4") + " s (CPU: " + t.TotalCpuTime.ToString("G4") + " s)." + Environment.NewLine);
                if (passed)
                    Console.WriteLine(Environment.NewLine + "Test of Cholesky decomposition completed SUCCESSFULLY." + Environment.NewLine);
                else
                    Console.WriteLine(Environment.NewLine + "Test of Cholesky decomposition completed with ERRORS." + Environment.NewLine);
            }
            return passed;
        }



        /// <summary>Demonstration of usae of Cholesky decomposition.</summary>
        /// $A Igor Dec14;
        public static void TestCholeskyDecompositionDemo()
        {
            Console.WriteLine(Environment.NewLine + "Begin Cholesky decomposition demo." + Environment.NewLine);

            int dim = 3;
            IMatrix m = new Matrix(dim, dim);
            m[0, 0] = 4; m[0, 1] = 12; m[0, 2] = -16;
            m[1, 0] = 12; m[1, 1] = 37; m[1, 2] = -43;
            m[2, 0] = -16; m[2, 1] = -43; m[2, 2] = 98;

            Console.WriteLine("System matrix = " + Environment.NewLine + m.ToStringReadable());

            int[] perm = new int[dim];
            IMatrix CholeskyMatrix = m.GetNew(dim, dim);
            CholeskyDecompose(m, ref CholeskyMatrix);

            IMatrix lower = null;
            IMatrix upper = null;

            CholeskyExtractLower(CholeskyMatrix, ref lower);
            CholeskyExtractUpper(CholeskyMatrix, ref upper);

            Console.WriteLine("The (combined) Cholesky decomposition of m is" + Environment.NewLine + CholeskyMatrix.ToStringReadable());
            Console.WriteLine(Environment.NewLine + "The lower part of Cholesky is " + Environment.NewLine + lower.ToStringReadable());
            Console.WriteLine("The upper part of Cholesky is " + Environment.NewLine + upper.ToStringReadable());

            IVector auxX = null;

            // Calculation of inverse:
            IMatrix inverse = null;
            CholeskyInverse(CholeskyMatrix, ref auxX, ref inverse);
            Console.WriteLine("Inverse of m computed from its decomposition is " + Environment.NewLine + inverse.ToStringReadable());
            IMatrix prod = m.GetNew();
            MatrixBase.Multiply(inverse, m, ref prod);
            // Console.WriteLine("Product of matrix and its inverse is\n" + MatrixAsString(prod));
            // Test inverse correctness:
            IMatrix diff = m.GetNew();
            MatrixBase.SetIdentity(diff);
            MatrixBase.Subtract(prod, diff, ref diff);
            Console.WriteLine("Error norm for matrix inverse: " + (diff.NormForbenius).ToString() + Environment.NewLine);

            // Calculate inverse in another way - by solbving multiple systems of equations with right-hand 
            //  sides defined by identity matrix:

            inverse = null;
            IMatrix B = m.GetNew(dim, dim);
            MatrixBase.SetIdentity(B);
            CholeskySolve(CholeskyMatrix, B, ref auxX, ref inverse);
            Console.WriteLine("Inverse of m computed by explicit equation solving (identity right-hand) is " + Environment.NewLine + inverse.ToStringReadable());
            prod = null;
            MatrixBase.Multiply(inverse, m, ref prod);
            // Test inverse correctness:
            MatrixBase.SetIdentity(diff);
            MatrixBase.Subtract(prod, diff, ref diff);
            Console.WriteLine("Inverse by solving with matrix right-hand sides, error norm: " + (diff.NormForbenius).ToString() + Environment.NewLine);

            double det = CholeskyDeterminant(CholeskyMatrix);
            Console.WriteLine("Determinant of m computed via decomposition = " + det.ToString("F1"));

            double[] bArray = new double[] { 49.0, 30.0, 43.0 };

            IVector b = new Vector(bArray);
            Console.WriteLine(Environment.NewLine + "Right-hand side vector: " + b.ToStringMath());

            Console.WriteLine("Solving system via decomposition...");
            IVector x = null;
            CholeskySolve(CholeskyMatrix, b, ref x);

            Console.WriteLine(Environment.NewLine + "Solution is x = " + x.ToStringMath());

            // Insight in matrix decomposition concepts:
            IMatrix Cholesky = null;
            Multiply(lower, upper, ref Cholesky);
            IMatrix orig = null;
            int[] auxArray = null;
            UnPermute(Cholesky, perm, ref auxArray, ref orig);  // use a custom method with the perm array to unscramble Cholesky
            MatrixBase.Subtract(orig, m, ref diff);
            if (diff.NormForbenius < 0.000000001)
                Console.WriteLine("\nProduct of L and U successfully unpermuted using perm array.");
            else
                Console.WriteLine("\nPruduct of L and U unpermuted UNSUCCESSFULLY using perm array.");

            IMatrix permMatrix = m.GetNew(dim, dim);
            PermutationArrayToMatrix(perm, ref permMatrix); // convert the perm array to a matrix
            MatrixBase.Multiply(permMatrix, Cholesky, ref orig); // another way to unscramble
            MatrixBase.Subtract(orig, m, ref diff);
            if (diff.NormForbenius < 0.000000001)
                Console.WriteLine("\nProduct of L and U successfully unpermuted using perm matrix.");
            else
                Console.WriteLine("\nPruduct of L and U unpermuted UNSUCCESSFULLY using permutation matrix.");

            Console.WriteLine("\nEnd of matrix decomposition demo.\n");
            // Console.ReadLine();
        }  // TestCholeskyDecompositionDemo()




        #endregion CholeskyDecomposition.Tests


        #endregion CholeskyDecomposition


        #region QrDecompositon


        /// <summary>Calculates QR decomposition of a real invertible matrix by using Gramm-Schmidt orthogonalization.
        /// <para>Q is orthogonal matrix (dot product of distinct columns is 0 and that of the same columns is 1), 
        /// and R is upper triangular.</para>
        /// <para>Decomposition can NOT be done in place.</para></summary>
        /// <param name="A">Matrix whose decomposition is calculated.</param>
        /// <param name="resQ">Matrix where the orthogonal factor Q is stored.</param>
        /// <param name="resR">Matrix where the uppker triangular factor R is stored.</param>
        /// <param name="tol">Tolerance for detection of singularity (must be a small positive number greater or equal to 0).</param>
        /// <remarks>See also:
        /// <para>http://en.wikipedia.org/wiki/QR_decomposition#Using_the_Gram.E2.80.93Schmidt_process</para></remarks>
        /// $A Igor Mar15;
        public static bool QrDecomposeGrammSchmidt(IMatrix A, ref IMatrix resQ, ref IMatrix resR, double tol = 1e-12)
        {
            if (tol <= 0)
                tol = 1.0e-12;
            if (A == null)
                throw new ArgumentException("Matrix to be QR decomposed is not specified (null reference).");
            int dim = A.RowCount;
            if (A.ColumnCount != dim)
                throw new ArgumentException("Matrix to be QR decomposed is not a square matrix.");
            if (resQ == null)
                resQ = A.GetNew(dim, dim);
            if (resQ.RowCount != dim || resQ.ColumnCount != dim)
                resQ = A.GetNew(dim, dim);
            if (resR == null)
                resR = A.GetNew(dim, dim);
            if (resR.RowCount != dim || resR.ColumnCount != dim)
                resR = A.GetNew(dim, dim);

            for (int whichCol = 0; whichCol < dim; ++ whichCol)
            {
                // Copy column from A to Q:
                for (int i = 0; i < dim; ++ i)
                    resQ[i, whichCol] = A[i, whichCol];
                for (int whichBasis = 0; whichBasis < whichCol; ++ whichBasis)
                {
                    double dotProd = 0;
                    // Calculate inner product between orthonormed vector No. whichProj and the current column vector:
                    for (int i = 0; i < dim; ++i)
                        dotProd += resQ [i, whichBasis] * resQ[i, whichCol];
                    // from the current column vector, subtract its projection on the orthonormed vector:
                    for (int i = 0; i < dim; ++i)
                    {
                        resQ[i, whichCol] -= dotProd * resQ[i, whichBasis];
                    }
                }
                // Finally, normalize the newly calculated column of Q:
                double norm = 0.0;
                for (int i = 0; i < dim; ++ i)
                {
                    double el = resQ[i, whichCol];
                    norm += el * el;
                }
                norm = Math.Sqrt(norm);
                if (norm < tol)
                    throw new InvalidOperationException("Orthogonalization failed in QR decomposition, rank deficiency (tolerance: " + tol + ").");
                double normReciprocal = 1.0 / norm;
                for (int i = 0; i < dim; ++i)
                {
                    resQ[i, whichCol] *= normReciprocal;
                }
                // Now that we have Q, calculate R: 
                for (int row = 0; row < dim; ++row)
                {
                    for (int col = 0; col < dim; ++col)
                    {
                        if (row > col)
                            resR[row, col] = 0;
                        else
                        {
                            double dotProd = 0;
                            for (int i = 0; i < dim; ++i)
                            {
                                dotProd += resQ[i, row] * A[i, col];
                            }
                            resR[row, col] = dotProd;
                        }
                    }
                }
                
            }

                
            return true;
        }


        /// <summary>Calculates QR decomposition of a real invertible matrix.
        /// <para>Q is orthogonal matrix (dot product of distinct columns is 0 and that of the same columns is 1), 
        /// and R is upper triangular.</para>
        /// <para>Decomposition can NOT be done in place.</para></summary>
        /// <param name="A">Matrix whose decomposition is calculated.</param>
        /// <param name="resQ">Matrix where the orthogonal factor Q is stored.</param>
        /// <param name="resR">Matrix where the uppker triangular factor R is stored.</param>
        /// <param name="tol">Tolerance for detection of singularity (must be a small positive number greater or equal to 0).</param>
        /// <remarks>See also:
        /// <para>http://en.wikipedia.org/wiki/QR_decomposition</para></remarks>
        /// $A Igor Mar15;
        public static bool QrDecompose(IMatrix A, ref IMatrix resQ, ref IMatrix resR, double tol = 1e-12)
        {
            return QrDecomposeGrammSchmidt(A, ref resQ, ref resR, tol);
        }

        
        /// <summary>Solves a system of eauations with the specified QR decomposition of a real symmetric square matrix.
        /// <para>Used in conjunction with the <see cref="QrDecompose"/> method for calculation of decomposition.</para>
        /// <para>Can NOT be done in place (input and result vectors can reference the same object).</para></summary>
        /// <param name="factorQ">First factor of QR decomposition (orthogonal Q, obtained by <see cref="QrDecompose"/>).</param>
        /// <param name="factorR">Second factor of QR decomposition (upper triangular R, obtained by <see cref="QrDecompose"/>).</param>
        /// <param name="b">Vector of the right-hand sides of the system of equations.</param>
        /// <param name="x">Vector where result is stored.</param>
        /// $A Igor Mar15;
        public static void QrSolve(IMatrix factorQ, IMatrix factorR, IVector b, ref IVector x)
        {
            if (factorQ == null)
                throw new ArgumentException("Factor Q of the QR decomposed matrix not specified (null reference).");
            int dim = factorQ.RowCount;
            if (factorQ.ColumnCount != dim)
                throw new ArgumentException("Factor Q of the QR decomposed matrix is not a square matrix.");
            if (factorR == null)
                throw new ArgumentException("Factor R of the QR decomposed matrix not specified (null reference).");
            if (factorR.RowCount != dim)
                throw new ArgumentException("Factor R of the QR decomposed matrix has wrong number of rows, " 
                    + factorR.RowCount + " instead of " + dim + ".");
            if (factorR.ColumnCount != dim)
                throw new ArgumentException("Factor R of the QR decomposed matrix is not a square matrix.");
            if (b == null)
                throw new ArgumentException("Vector of right-hand sides is not specified (null reference).");
            if (b.Length != dim)
                throw new ArgumentException("Vector of right-hand sides is of inconsistent dimentions, " + b.Length + " instead of " + dim + ".");
            if (x == null)
                x = b.GetNew(dim);
            else if (x.Length != dim)
                x = b.GetNew(dim);
            if (object.ReferenceEquals(b, x))
                throw new ArgumentException("Vectors of right-hand sides and solution are the same (reference equality), QR solve can not be done in place.");

            IVector auxVec = x;

            //if (auxVec == null)
            //    auxVec = b.GetNew(dim);
            //else if (auxVec.Length != dim)
            //    auxVec = b.GetNew(dim);
            //if (object.ReferenceEquals(b, auxVec))
            //    throw new ArgumentException("The auxiliary vector and vector of right-hand sides are the same vectors (reference equality).");

            // Multiply by Q transposed form the left to obtain a new equation with system 
            // matrix R and new rightphand sides auxVec:
            MatrixBase.MultiplyTranspVecPlain(factorQ, b, auxVec);

            // Solve the upper triangular system with system matrix R and right-hand sides auxVec:
            for (int row = dim - 1; row >= 0; --row)
            {
                double xx = auxVec[row];
                for (int col = row + 1; col < dim; ++ col)
                {
                    xx -= factorR[row, col] * x[col];
                }
                xx /= factorR[row, row];
                x[row] = xx;
            }

        }


        /// <summary>Calculates inverse of the matrix from its specified LDLT-decomposed matrix.</summary>
        /// <param name="factorQ">First factor of QR decomposition (orthogonal Q, obtained by <see cref="QrDecompose"/>).</param>
        /// <param name="factorR">Second factor of QR decomposition (upper triangular R, obtained by <see cref="QrDecompose"/>).</param>
        /// <param name="B">Matrix whose columns are right-hand sides of equations to be solved.</param>
        /// <param name="auxB">Auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="auxX">Another auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary. May not be the same as <paramref name="auxB"/>.</param>
        /// <param name="X">Matrix where result will be stored. Reallocated if necessary.</param>
        /// $A Igor Dec14;
        public static void QrSolve(IMatrix factorQ, IMatrix factorR, IMatrix B, ref IVector auxB, ref IVector auxX, ref IMatrix X)
        {
            if (factorQ == null)
                throw new ArgumentException("Factor Q of the QR decomposed matrix not specified (null reference).");
            int dim = factorQ.RowCount;
            if (factorQ.ColumnCount != dim)
                throw new ArgumentException("Factor Q of the QR decomposed matrix is not a square matrix.");
            if (factorR == null)
                throw new ArgumentException("Factor R of the QR decomposed matrix not specified (null reference).");
            if (factorR.RowCount != dim)
                throw new ArgumentException("Factor R of the QR decomposed matrix has wrong number of rows, "
                    + factorR.RowCount + " instead of " + dim + ".");
            if (factorR.ColumnCount != dim)
                throw new ArgumentException("Factor R of the QR decomposed matrix is not a square matrix.");
            if (B == null)
                throw new ArgumentException("Matrix of right-hand sides is not specified (null reference).");
            if (B.RowCount != dim)
                throw new ArgumentException("Matrix of right-hand sides does not have as many rows as there are equations.");
            int numSystems = B.ColumnCount;
            if (auxB == null)
                auxB = factorQ.GetNewVector(dim);
            if (auxB.Length != dim)
                auxB = factorQ.GetNewVector(dim);
            if (X == null)
                X = factorQ.GetNew(dim, numSystems);
            if (X.RowCount != dim || X.ColumnCount != numSystems)
                X = factorQ.GetNew(dim, numSystems);
            if (object.ReferenceEquals(factorQ, X) || object.ReferenceEquals(factorQ, B) || object.ReferenceEquals(B, X)
                || object.ReferenceEquals (factorR, X) || object.ReferenceEquals(factorR, B))
                throw new ArgumentException("Input matrix the same as result matrix. Can not be done in place to such extent.");
            if (object.ReferenceEquals(auxB, auxX))
                throw new ArgumentException("Auxiliary vectors for single right-hand sides and solutions are the same (reference equality).");
            for (int whichSystem = 0; whichSystem < numSystems; ++whichSystem)
            {
                // Get column of B as right-hand sides:
                for (int j = 0; j < dim; ++j)
                    auxB[j] = B[j, whichSystem];
                // Solve the system with this vector:
                QrSolve(factorQ, factorR, auxB, ref auxX);
                for (int j = 0; j < dim; ++j)
                    X[j, whichSystem] = auxX[j];
            }
        }

        /// <summary>Calculates inverse of the matrix from its specified LDLT decomposition.</summary>
        /// <param name="factorQ">Matrix containing the Q factor of the QR decomposition of the original matrix.</param>
        /// <param name="factorR">Matrix containing the R factor of the QR decomposition of the original matrix.</param>
        /// <param name="auxB">Auxiliary vector of the same dimension as dimensions of the decomposed matrix.</param>
        /// <param name="auxX">Another auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        /// Reallocated if necessary.</param>
        /// <param name="res">Matrix where result will be stored. Reallocated if necessary.</param>
        /// $A Igor Dec14;
        public static void QrInverse(IMatrix factorQ, IMatrix factorR, ref IVector auxB, ref IVector auxX, ref IMatrix res)
        {
            if (factorQ == null)
                throw new ArgumentException("Factor Q of the QR decomposed matrix not specified (null reference).");
            int dim = factorQ.RowCount;
            if (factorQ.ColumnCount != dim)
                throw new ArgumentException("Factor Q of the QR decomposed matrix is not a square matrix.");
            if (factorR == null)
                throw new ArgumentException("Factor R of the QR decomposed matrix not specified (null reference).");
            if (factorR.RowCount != dim)
                throw new ArgumentException("Factor R of the QR decomposed matrix has wrong number of rows, "
                    + factorR.RowCount + " instead of " + dim + ".");
            if (factorR.ColumnCount != dim)
                throw new ArgumentException("Factor R of the QR decomposed matrix is not a square matrix.");
            if (auxB == null)
                auxB = factorQ.GetNewVector(dim);
            if (auxB.Length != dim)
                auxB = factorQ.GetNewVector(dim);
            if (auxX == null)
                auxX = factorQ.GetNewVector(dim);
            if (auxX.Length != dim)
                auxX = factorQ.GetNewVector(dim);
            if (res == null)
                res = factorQ.GetNew(dim, dim);
            if (res.RowCount != dim || res.ColumnCount != dim)
                res = factorQ.GetNew(dim, dim);
            if (object.ReferenceEquals(factorQ, res) || object.ReferenceEquals(factorR, res) )
                throw new ArgumentException("Input matrix the same as result matrix. Can not be done in place to such extent.");
            if (object.ReferenceEquals(auxB, auxX))
                throw new ArgumentException("Auxiliary vectors for single right-hand sides and solutions are the same (reference equality).");

            for (int whichSystem = 0; whichSystem < dim; ++whichSystem)
            {
                // Get column of B as right-hand sides:
                for (int j = 0; j < dim; ++j)
                {
                    // make right-hand vector contain a column of identity matrix:
                    if (j != whichSystem)
                        auxB[j] = 0.0;
                    else
                        auxB[j] = 1.0;
                }
                // Solve the system with this vector:
                QrSolve(factorQ, factorR, auxB, ref auxX);
                for (int j = 0; j < dim; ++j)
                    res[j, whichSystem] = auxX[j];
            }
        }



        #endregion QrDecomposition

        #region Testing


        /// <summary>Checks whether the difference between matrix result of the tested operation and 
        /// some reference result is within the specified tolerance.</summary>
        /// <param name="result">Matrix that is the result of the tested operation.</param>
        /// <param name="referenceResult">Reference result obtained in a different way.</param>
        /// <param name="tolerance">Tolerance on norm of the difference. Must be greater than 0.
        /// If norm is less or equal to the tolerance then the test passes.</param>
        /// <param name="printReports">If true then short reports are printed to console.</param>
        /// <returns>true if norm of the difference between the result matrix and the reference result is less or
        /// equal to tolerance.</returns>
        protected static bool CheckTestResult(IMatrix result, IMatrix referenceResult, double tolerance, bool printReports)
        {
            if (tolerance<=0)
                throw new ArgumentException("Invalid tolerance " + tolerance + ", should not be less or equal to 0!");
            IMatrix diff = null;
            Subtract(result, referenceResult, ref diff);
            double normDiff = diff.NormForbenius;
            bool passed = true;
            if (normDiff <= tolerance)
            {
                if (printReports)
                    Console.WriteLine("Test PASSED. Norm of difference: " + normDiff);
            } else 
            {
                passed = false;
                if (printReports)
                {
                    Console.WriteLine("ERROR: Test NOT passed. Norm of difference: " + normDiff + ", tolerance: " + tolerance);
                    Console.WriteLine("Difference between the result of operation and reference result: ");
                    Console.WriteLine(diff.ToString());
                }
            }
            return passed;
        }

        /// <summary>Checks whether the difference between matrix result of the tested operation and 
        /// some reference result is within the specified tolerance.</summary>
        /// <param name="result">Matrix that is the result of the tested operation.</param>
        /// <param name="referenceResult">Reference result obtained in a different way.</param>
        /// <param name="tolerance">Tolerance on norm of the difference. Must be greater than 0.
        /// If norm is less or equal to the tolerance then the test passes.</param>
        /// <param name="printReports">If true then short reports are printed to console.</param>
        /// <returns>true if norm of the difference between the result matrix and the reference result is less or
        /// equal to tolerance.</returns>
        protected static bool CheckTestResult(IVector result, IVector referenceResult, double tolerance, bool printReports)
        {
            if (tolerance <= 0)
                throw new ArgumentException("Invalid tolerance " + tolerance + ", should not be less or equal to 0!");
            IVector diff = null;
            Vector.Subtract(result, referenceResult, ref diff);
            double normDiff = diff.Norm2;
            bool passed = true;
            if (normDiff <= tolerance)
            {
                if (printReports)
                    Console.WriteLine("Test PASSED. Norm of difference: " + normDiff);
            }
            else
            {
                passed = false;
                if (printReports)
                {
                    Console.WriteLine("ERROR: Test NOT passed. Norm of difference: " + normDiff + ", tolerance: " + tolerance);
                    Console.WriteLine("Difference between the result of operation and reference result: ");
                    Console.WriteLine(diff.ToString());
                }
            }
            return passed;
        }



        /// <summary>Tests various matrix and vector products with fixed tolerance of 1.0E-6.</summary>
        /// <param name="printReports">Specifies whether to print short reports to console or not.</param>
        /// <returns>True if all tests have passed, and false if there is an error.</returns>
        public static bool TestMatrixProducts(bool printReports)
        {
            return TestMatrixProducts(1.0e-6, printReports);
        }

        /// <summary>Tests various matrix and vector products, without printing reports.</summary>
        /// <param name="tolerance">Tolerance for difference between product and test expression below which any individual test passes.
        /// Must be greater than 0.</param>
       /// <returns>True if all tests have passed, and false if there is an error.</returns>
        public static bool TestMatrixProducts(double tolerance)
        {
            return TestMatrixProducts(tolerance, false);
        }

        /// <summary>Tests various matrix and vector products with fixed tolerance of 1.0E-6 and without printing reports.</summary>
        /// <returns>True if all tests have passed, and false if there is an error.</returns>
        public static bool TestMatrixProducts()
        {
            return TestMatrixProducts(1.0e-6, false);
        }

        /// <summary>Tests various matrix and vector products.</summary>
        /// <param name="tolerance">Tolerance for difference between product and test expression below which any individual test passes.
        /// Must be greater than 0.</param>
        /// <param name="printReports">Specifies whether to print short reports to console or not.</param>
        /// <returns>True if all tests have passed, and false if there is an error.</returns>
        public static bool TestMatrixProducts(double tolerance, bool printReports)
        {
            if (tolerance <= 0)
                throw new ArgumentException("Invalid tolerance " + tolerance + ", should not be less or equal to 0!");
            bool passed = true, passedThis;
            int numPassed = 0, numFailed = 0;
            if (printReports)
                Console.WriteLine(Environment.NewLine, "Tests of matrix products:");

            if (printReports)
                Console.WriteLine(Environment.NewLine + "MultiplyTranspMat: ");
            passedThis = TestMultiplyTranspMat(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;

            if (printReports) 
                Console.WriteLine(Environment.NewLine + "MultiplyMatTransp: ");
            passedThis = TestMultiplyMatTransp(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;

            if (printReports) 
                Console.WriteLine(Environment.NewLine + "MultiplyTranspTransp: ");
            passedThis = TestMultiplyTranspTransp(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;

            if (printReports) 
                Console.WriteLine(Environment.NewLine + "Multiply3: ");
            passedThis = TestMultiply3(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;

            if (printReports)
                Console.WriteLine(Environment.NewLine + "MultiplyTranspMatTransp: ");
            passedThis = TestMultiplyTranspMatTransp(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;

            if (printReports)
                Console.WriteLine(Environment.NewLine + "MultiplyTranspMatMat: ");
            passedThis = TestMultiplyTranspMatMat(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;

            if (printReports)
                Console.WriteLine(Environment.NewLine + "MultiplyMatMatTransp: ");
            passedThis = TestMultiplyMatMatTransp(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;

            if (printReports)
                Console.WriteLine(Environment.NewLine + "MultiplyMatTranspMat: ");
            passedThis = TestMultiplyMatTranspMat(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;

            if (printReports)
                Console.WriteLine(Environment.NewLine + "MultiplyTranspTranspTransp: ");
            passedThis = TestMultiplyTranspTranspTransp(tolerance, printReports);
            passed = passed && passedThis;
            if (passedThis) ++numPassed; else ++numFailed;

            if (printReports)
            {
                if (passed)
                    Console.WriteLine(Environment.NewLine + "All tests have passed." + Environment.NewLine);
                else
                    Console.WriteLine(Environment.NewLine + "WARNING: Some tests have NOT PASSED (" 
                        +  numFailed + " of " + (numPassed +  numFailed) + " failed)." + Environment.NewLine);
            }
            return passed;
        }

        
        /// <summary>A test method, just prints some output.</summary>
        public static void TestStaticMethodCommon()
        {
            Console.WriteLine("TestStaticMethodCommon from the MatrixBase class.");
            TestStaticMethodSpecific();
        }

        /// <summary>A test method, just prints some output.</summary>
        public static void TestStaticMethodSpecific()
        {
            Console.WriteLine("TestStaticMethod from MatrixBase.");
        }

        /// <summary>Performs test of converson between double indexing and flat indexing of matric elements,.
        /// Returns true if successful, false othwrwise.</summary>
        /// <param name="dim1">Number of rows.</param>
        /// <param name="dim2">Number of columns.</param>
        /// <param name="providedMatrix">If not null, this matrix will be taken for testing indices.</param>
        /// <returns>true if successful, false if not.</returns>
        public static bool TestIndices(int dim1 = 3, int dim2 = 4, IMatrix providedMatrix = null)
        {
            Console.WriteLine(Environment.NewLine + "Test of matrix index conversions ...");
            bool ret = true;
            IMatrix mat = null;
            if (providedMatrix == null)
                mat = new Matrix(dim1, dim2);
            else
                mat = providedMatrix;
            MatrixBase.SetRandom(mat);
            int iFlat = 0;
            for (int row = 0; row < mat.RowCount; ++row)
                for (int col = 0; col < mat.ColumnCount; ++col)
                {
                    double element = mat[row, col];
                    int flatIndex = MatrixBase.Index(mat, row, col);
                    double elementFlat = mat[flatIndex];
                    int rowCalc, colCalc;
                    MatrixBase.Indices(mat, flatIndex, out rowCalc, out colCalc);
                    Console.WriteLine(row + ", " + col + ": flat = " + flatIndex + ", restored = ("
                        + rowCalc + ", " + colCalc + "), el. = " + elementFlat);
                    if (flatIndex != iFlat)
                    {
                        ret = false;
                        Console.WriteLine("  Error (" + row + ", " + col + "): Calculated flat index (" + flatIndex 
                            + ") different from actual one (" + flatIndex + ").");
                    }
                    if (rowCalc != row || colCalc != col)
                    {
                        ret = false;
                        Console.WriteLine("  Error (" + row + ", " + col + "): Calculated indices from flat form (" 
                            + rowCalc + ", " + colCalc + ") don't match original indices.");
                    }
                    if (flatIndex != iFlat)
                    {
                        ret = false;
                        Console.WriteLine("  Error (" + row + ", " + col + "): Calculated flat index (" + flatIndex
                            + ") different from actual one (" + flatIndex + ").");
                    }
                    ++ iFlat;
                }
            Console.WriteLine("... test done " + (ret?"successfully.":" with ERRORS.") + Environment.NewLine);
            return ret;
        }


        #endregion Testing


    }  // abstract class MatrixBase


    /// <summary>Matrix store.
    /// <para>Stores matrix objects for reuse.</para>
    /// <para>Can be used for storage fo matrices with specific dimension (default) or for torage of any non-null matrices.</para></summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MatrixStore<T> : ObjectStore<T>, ILockable
        where T: class, IMatrix
    {

        /// <summary>Constructs a new matrix store of unspecified dimensions.</summary>
        protected MatrixStore(): base()
        {  }

        /// <summary>Constructs a new matrix store for matrices with the specified dimensions.</summary>
        /// <param name="rowCount">Number of rows of stored matrices.</param>
        /// <param name="columnCount">Number of columns of stored matrices.</param>
        public MatrixStore(int rowCount, int columnCount)
            : this()
        {
            this._rowCount = rowCount;
            this._columnCount = columnCount;
        }

        /// <summary>Constructs a new matrix store for matrices with the specified dimensions.
        /// <para>If <param name="constrainDimensions"></param> is false then store can be used for matrices with any dimensions.</para></summary>
        /// <param name="rowCount">Number of rows of stored matrices.</param>
        /// <param name="columnCount">Number of columns of stored matrices.</param>
        public MatrixStore(int rowCount, int columnCount, bool constrainDimensions)
            : this(rowCount, columnCount)
        {
            this._constrainDimensions = constrainDimensions;
        }

        /// <summary>Ilf true then only matrices with matching dimensions are eligible for storing.
        /// <para>Default is true.</para></summary>
        protected bool _constrainDimensions = true;

        /// <summary>Ilf true then only matrices with matching dimensions are eligible for storing.</summary>
        public bool ConstrainDimensions
        {
            get { lock (Lock) { return _constrainDimensions; } }
            set { lock (Lock) { 
                bool checkEligible = false;
                if (_constrainDimensions == false && value == true)
                    checkEligible = true;
                _constrainDimensions = value;
                if (checkEligible)
                    ClearIneligible();
            } }
        }

        protected int _rowCount, _columnCount;

        /// <summary>Number of rows for matrices to be stored.
        /// <para>If <see cref="ConstrainDimensions"/> is true then only matrices that match dimensions are eligible for storage.
        /// Otherwise, dimensions are only important for creation of new matrices.</para></summary>
        public int RowCount
        {
            get { lock (Lock) { return RowCount; } }
            set 
            {
                lock (Lock)
                {
                    bool checkEligible = false;
                    if (value != _rowCount && _constrainDimensions)
                        checkEligible = true;
                    _rowCount = value;
                    if (checkEligible)
                        ClearIneligible();
                }
            }
        }

        /// <summary>Number of columns for matrices to be stored.
        /// <para>If <see cref="ConstrainDimensions"/> is true then only matrices that match dimensions are eligible for storage.
        /// Otherwise, dimensions are only important for creation of new matrices.</para></summary>
        public int ColumnCount
        {
            get { lock (Lock) { return _columnCount; } }
            set
            {
                lock (Lock)
                {
                    bool checkEligible = false;
                    if (value != _columnCount && _constrainDimensions)
                        checkEligible = true;
                    _columnCount = value;
                    if (checkEligible)
                        ClearIneligible();
                }
            }
        }

        /// <summary>Returns true if the specified matrix is eligible for storage in the current store, false if not.</summary>
        /// <param name="mat">Matrix whose eligibility is checked.</param>
        public override bool IsEligible(T mat)
        {
            lock (Lock)
            {
                bool ret = true;
                if (_constrainDimensions)
                {
                    if (mat == null)
                        ret = false;
                    else if (mat.RowCount != _rowCount || mat.ColumnCount != _columnCount)
                        ret = false;
                }
                return ret;
            }
        }

    }  // MatrixStore<T>


    /// <summary>Matrix store.
    /// <para>Stores matrix objects for reuse.</para>
    /// <para>Can be used for storage fo matrices with specific dimension (default) or for torage of any non-null matrices.</para></summary>
    public class MatrixStore : MatrixStore<Matrix>, ILockable
    {

        /// <summary>Constructs a new matrix store of unspecified dimensions.</summary>
        protected MatrixStore()
            : base()
        { }

        /// <summary>Constructs a new matrix store for matrices with the specified dimensions.</summary>
        /// <param name="rowCount">Number of rows of stored matrices.</param>
        /// <param name="columnCount">Number of columns of stored matrices.</param>
        public MatrixStore(int rowCount, int columnCount)
            : base(rowCount, columnCount)
        {  }

        /// <summary>Constructs a new matrix store for matrices with the specified dimensions.
        /// <para>If <param name="constrainDimensions"></param> is false then store can be used for matrices with any dimensions.</para></summary>
        /// <param name="rowCount">Number of rows of stored matrices.</param>
        /// <param name="columnCount">Number of columns of stored matrices.</param>
        public MatrixStore(int rowCount, int columnCount, bool constrainDimensions)
            : base(rowCount, columnCount, constrainDimensions)
        {  }

        /// <summary>Returns a newly created object eligible for storage, or null if such an object
        /// can not be created. This method should not throw an exception.</summary>
        protected override Matrix TryGetNew()
        {
            if (_rowCount > 0 && _columnCount > 0)
            {
                return new Matrix(_rowCount, _columnCount);
            }
            else
                return null;
        }

    } // class MatrixStore


} 


