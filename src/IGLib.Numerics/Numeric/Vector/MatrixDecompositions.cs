
#nullable disable

// MATRIX DECOMPOSITIONS; This module is based on the MathNet external library.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using IG.Lib;

//using Matrix_MathNet = MathNet.Numerics.LinearAlgebra.Matrix;
//using LUDecomposition_MathNet = MathNet.Numerics.LinearAlgebra.LUDecomposition;
//using QRDecomposition_MathNet = MathNet.Numerics.LinearAlgebra.QRDecomposition;

using Matrix_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix;
using MatrixBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Matrix<double>;
using Vector_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Double.DenseVector;
using VectorBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Vector<double>;
using VectorComplexBase_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Vector<System.Numerics.Complex>;
using LUDecomposition_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Factorization.LU<double>;
// MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseLU;
using QRDecomposition_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Factorization.QR<double>;
// MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseQR;
using CholeskyDecomposition_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Factorization.Cholesky<double>;
// MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseCholesky;
using EigenValueDecomposition_MathNetNumerics =  MathNet.Numerics.LinearAlgebra.Factorization.Evd<double>;
    // MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseEvd;
using SingularValueDecomposition_MathNetNumerics = MathNet.Numerics.LinearAlgebra.Factorization.Svd<double>;
    // MathNet.Numerics.LinearAlgebra.Double.Factorization.DenseSvd;

using MathNet.Numerics.LinearAlgebra.Double;
using QRDecomposition_MathNetNumericx = MathNet.Numerics.LinearAlgebra.Double.Factorization;


namespace IG.Num
{


    /// <summary>Classes that can be used for solution of linear systems of equations.
    /// <para>This interface is mainly used for matrix dexompositions.</para></summary>
    /// $A Igor Apr12;
    public interface ILinearSolver: ILockable
    {
    
        /// <summary>Indicates whether the matrix of coefficients of a linear system represented 
        /// by the current decomposition object, is nonsingular.</summary>
        /// <returns>True if the decomposed matrix represented by the current object is snonsingular,
        /// false otherwise.</returns>
        bool IsNonSingular
        { get; }

        /// <summary>Calculates the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition, and stores it to the specified matrix.</summary>
        /// <param name="product">Matrix where re-calculated product of the decomposed matrix is stored.</param>
        void GetProduct(ref IMatrix product);

        /// <summary>Calculates and returns the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition.</summary>
        IMatrix GetProduct();

        /// <summary>Calculates inverse of the decomposed matrix represented by the current object, 
        /// and stores it in the specified matrix.</summary>
        /// <param name="inv">Matrix where calculated inverse is stored.</param>
        void Inverse(ref IMatrix inv);

        /// <summary>Calculates and returns inverse of the decomposed matrix represented by the current object.</summary>
        IMatrix Inverse();

        /// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        /// matrix whose colums are right-hand sides of equations to be solved. Solutions 
        /// are stored to the specified matrix.
        /// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        /// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        /// <param name="X">Matrix where results are stored (one column for each right-hand side).</param>
        /// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        /// <exception cref="System.SystemException">Matrix is singular.</exception>
        void Solve(IMatrix B, ref IMatrix X);

        /// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        /// matrix whose colums are right-hand sides of equations to be solved, and returns
        /// a matrix whose columns are solutions of the specified systems of equations.
        /// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        /// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        /// <returns>X so that L*U*X = B(piv,:)</returns>
        /// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        /// <exception cref="System.SystemException">Matrix is singular.</exception>
        IMatrix Solve(IMatrix B);

        /// <summary>Solves a system of linear equations A*x=b, and stores the solution in the specified vector.
        /// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        /// <param name="b">Vector of right-hand sides.</param>
        /// <param name="x">Vector where solution is stored.</param>
        void Solve(IVector b, ref IVector x);

        /// <summary>Solves a system of linear equations A*x=b, and returns the solution.
        /// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        /// <param name="b">Right-hand side vector.</param>
        /// <returns>Solution of the System such that L*U*x=s(piv,:).</returns>
        IVector Solve(IVector b);

    }  // interface ILinearSolver

    public abstract class LinearSolverBase 
    {


        #region ThreadLocking

        private object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.
        /// Do not use this object for locking in class' methods, for this you should use 
        /// InternalLock.</summary>
        public object Lock { get { return _mainLock; } }

        #endregion ThreadLocking


        #region Data

        protected internal int
            _rowCount = 0,  // number of rows of a decomposed matrix
            _columnCount = 0;  // number of columns of a decomposed matrix

        #endregion Data


        #region Operation


        #region ToOverride

        protected MatrixBase_MathNetNumerics _matrixSolution_MathNetNumerics;

        protected Matrix_MathNetNumerics _matrixRighthandSides_MathNetNumerics;

        /// <summary>Solves systems of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="righthandSides">Matrix that contains right-hand sides of the linear equations to be solved as its columns.</param>
        /// <returns>The Math.Net Numerics matrix whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="righthandSides"/> parameter.</returns>
        protected virtual MatrixBase_MathNetNumerics SolveMathNetNumerics(IMatrix righthandSides)
        {
            lock (Lock)
            {
                Matrix.Copy(righthandSides, ref _matrixRighthandSides_MathNetNumerics);
                _matrixSolution_MathNetNumerics = null;
                throw new ArgumentException("Solution with system of equations with right-hand sides in Math.Net Numerics matrix form is not implemented.");
                // return matrixSolution_MathNetNumerics;
            }
        }

        protected VectorBase_MathNetNumerics _vectorSolution_MathNetNumerics;
        
        protected Vector_MathNetNumerics _vectorRighthandSides_MathNetNumerics;

        /// <summary>Solves the system of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="rightHandSides">Vector of right-hand sides of the linear equations to be solved.</param>
        /// <returns>The Math.Net Numerics vector whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="rightHandSides"/> parameter.</returns>
        protected virtual VectorBase_MathNetNumerics SolveMathNetNumerics(IVector rightHandSides)
        {
            lock (Lock)
            {
                Vector.Copy(rightHandSides, ref _vectorRighthandSides_MathNetNumerics);
                _vectorSolution_MathNetNumerics = null;
                throw new ArgumentException("Solution with system of equations with right-hand sides in Math.Net Numerics vector form is not implemented.");
                // return vectorSolution_MathNetNumerics;
            }
        }

        #endregion ToOverride

        protected IVector auxB = null;

        protected IVector auxX = null;

        /// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        /// matrix whose colums are right-hand sides of equations to be solved. Solutions 
        /// are stored to the specified matrix.
        /// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        /// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        /// <param name="X">Matrix where results are stored (one column for each right-hand side).</param>
        /// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        /// <exception cref="System.SystemException">Matrix is singular.</exception>
        public virtual void Solve(IMatrix B, ref IMatrix X)
        {
            //MatrixBase_MathNetNumerics sol = SolveMathNetNumerics(X);
            //Matrix.Copy(sol, ref X);

            int d1 = this._rowCount;
            int d2 = this._columnCount;
            if (d1 != d2)
                throw new InvalidOperationException("Decomposed matrix is not a square matrix, solution with matrix right-hand sides not implemented.");
            int dim = this._rowCount;
            int numSystems = B.ColumnCount;
            if (B.RowCount != dim)
                throw new ArgumentException("Matrix of right-hand sides: number of rows "
                    + B.RowCount + " different than dimension of system matrix - " + dim + ".");
            if (X.RowCount != dim || X.ColumnCount != numSystems)
                MatrixBase.Resize(ref X, dim, numSystems);
            if (auxB == null)
                auxB = new Vector(dim);
            else if (auxB.Length != dim)
                auxB = new Vector(dim);
            if (auxX == null)
                auxX = new Vector(dim);
            else if (auxX.Length != dim)
                auxX = new Vector(dim);
            for (int whichSystem = 0; whichSystem < numSystems; ++whichSystem)
            {
                // Get column of B as right-hand sides:
                for (int j = 0; j < dim; ++j)
                    auxB[j] = B[j, whichSystem];
                // Solve the system with this vector:
                Solve(auxB, ref auxX);
                for (int j = 0; j < dim; ++j)
                    X[j, whichSystem] = auxX[j];
            }
        }




        ///// <summary>Calculates inverse of the matrix from its specified Cholesky-decomposed matrix.</summary>
        ///// <param name="CholeskyMatrix">Matrix containing the Cholesky decomposition of the original matrix.</param>
        ///// <param name="B">Matrix whose columns are right-hand sides of equations to be solved.</param>
        ///// <param name="auxX">Auxiliary vector of the same dimension as dimensions of the decomposed matrix.
        ///// Reallocated if necessary.</param>
        ///// <param name="X">Matrix where result will be stored. Reallocated if necessary.</param>
        ///// $A Igor Dec14;
        //private static void CholeskySolve_to_delete(IMatrix CholeskyMatrix, IMatrix B, ref IVector auxX, ref IMatrix X)
        //{
        //    if (CholeskyMatrix == null)
        //        throw new ArgumentException("Matrix containing Cholesky decomposition is not specified (null reference).");
        //    int dim = CholeskyMatrix.RowCount;
        //    if (B == null)
        //        throw new ArgumentException("Matrix of right-hand sides is not specified (null reference).");
        //    if (B.RowCount != dim)
        //        throw new ArgumentException("Matrix of right-hand sides does not have as many rows as there are equations.");
        //    int numSystems = B.ColumnCount;
        //    if (CholeskyMatrix.ColumnCount != dim)
        //        throw new ArgumentException("Matrix containing LU decomposition is not a square matrix.");
        //    if (auxX == null)
        //        auxX = CholeskyMatrix.GetNewVector(dim);
        //    if (auxX.Length != dim)
        //        auxX = CholeskyMatrix.GetNewVector(dim);
        //    if (X == null)
        //        X = CholeskyMatrix.GetNew(dim, numSystems);
        //    if (X.RowCount != dim || X.ColumnCount != dim)
        //        X = CholeskyMatrix.GetNew(dim, numSystems);
        //    if (object.ReferenceEquals(CholeskyMatrix, X) || object.ReferenceEquals(CholeskyMatrix, B) || object.ReferenceEquals(B, X))
        //        throw new ArgumentException("Input matrix the same as result matrix. Can not be done in place to such extent.");
        //    for (int whichSystem = 0; whichSystem < numSystems; ++whichSystem)
        //    {
        //        // Get column of B as right-hand sides:
        //        for (int j = 0; j < dim; ++j)
        //            auxX[j] = B[j, whichSystem];
        //        // Solve the system with this vector:
        //        CholeskySolve(CholeskyMatrix, auxX, ref auxX);
        //        for (int j = 0; j < dim; ++j)
        //            X[j, whichSystem] = auxX[j];
        //    }
        //}



        /// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        /// matrix whose colums are right-hand sides of equations to be solved, and returns
        /// a matrix whose columns are solutions of the specified systems of equations.
        /// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        /// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        /// <returns>X so that L*U*X = B(piv,:)</returns>
        /// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        /// <exception cref="System.SystemException">Matrix is singular.</exception>
        public IMatrix Solve(IMatrix B)
        {
            IMatrix ret = null;
            Solve(B, ref ret);
            return ret;
        }

        /// <summary>Solves a system of linear equations A*x=b, and stores the solution in the specified vector.
        /// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        /// <param name="b">Vector of right-hand sides.</param>
        /// <param name="x">Vector where solution is stored.</param>
        public virtual void Solve(IVector b, ref IVector x)
        {
            VectorBase_MathNetNumerics sol = SolveMathNetNumerics(b);
            Vector.Copy(sol, ref x);
        }

        /// <summary>Solves a system of linear equations A*x=b, and returns the solution.
        /// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        /// <param name="b">Right-hand side vector.</param>
        /// <returns>Solution of the System such that L*U*x=s(piv,:).</returns>
        public IVector Solve(IVector b)
        {
            IVector ret = null;
            Solve(b, ref ret);
            return ret;
        }



        #endregion Operation

    }


    /// <summary>LU decomposition of a matrix.
    /// <para>Objects of this class are immutable. Decomposition is calculated at initialization,
    /// and the decomposed matrix can not be replaced later.</para></summary>
    /// <remarks><para>
    /// For an m-by-n matrix A with m >= n, the LU decomposition is an m-by-n
    /// unit lower triangular matrix L, an n-by-n upper triangular matrix U,
    /// and a permutation vector pivot of length m so that A(piv,:) = L*U.
    /// <c> If m &lt; n, then L is m-by-m and U is m-by-n. </c>
    /// The LU decomposition with pivoting always exists, even if the matrix is
    /// singular, so the constructor will never fail.  The primary use of the
    /// LU decomposition is in the solution of square systems of simultaneous
    /// linear equations.  This will fail if IsNonSingular() returns false.
    /// </para></remarks>
    /// $A Igor Nov08 Apr12;
    [Serializable]
    public class LUDecomposition : LinearSolverBase, ILinearSolver, ILockable
    {

        protected internal LUDecomposition_MathNetNumerics Base = null;

        #region Construction

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public LUDecomposition(Matrix_MathNetNumerics A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to be decomposed is not specified (null reference).");
            if (A.RowCount <= 0 || A.ColumnCount <= 0)
                throw new ArgumentException("Inconsistent dimensions.");
            _rowCount = A.RowCount;
            _columnCount = A.ColumnCount;
            Base = A.LU(); // new LUDecomposition_MathNetNumerics(A);
        }

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public LUDecomposition(Matrix A)
            : this(A.CopyMathNetNumerics)
        {  }

        #endregion Construction


        /// <summary>Indicates whether the matrix of coefficients of a linear system is nonsingular.</summary>
        /// <returns><c>true</c> if U, and hence A, is nonsingular.</returns>
        public bool IsNonSingular
        {
            get 
            { 
                if (Base == null) 
                    throw new Exception("Invalid data.");
                return Base.Determinant != 0;  // Base.IsNonSingular;
            }
        }

        /// <summary>Returns the lower triangular factor.</summary>
        public Matrix L
        {
            get 
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.L);
                
            }
        }

        /// <summary>Returns the upper triangular factor.</summary>
        public Matrix U
        {
            get 
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.U);
            }
        }

        protected int[] _pivot;

        /// <summary>Returns the integer pivot permutation vector of LU decomposition.</summary>
        public int[] Pivot
        {
            get 
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data.");
                if (_pivot == null)
                {
                    int dim = Base.P.Dimension;
                    _pivot = new int[_rowCount];
                    for (int i = 0; i < _rowCount; ++i)
                    {
                        if (i < dim)
                            _pivot[i] = Base.P[i];
                        else
                            _pivot[i] = i;
                    }
                }
                return _pivot;
            }
        }

        /// <summary>Calculates the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition, and stores it to the specified matrix.</summary>
        /// <param name="product">Matrix where re-calculated product of the decomposed matrix is stored.</param>
        public void GetProduct(ref IMatrix product)
        {
            MatrixBase_MathNetNumerics prod = Base.L * Base.U;
            prod.PermuteRows(Base.P.Inverse());
            Matrix.Copy(prod, ref product);
        }


        /// <summary>Calculates and returns the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition.</summary>
        public IMatrix GetProduct()
        {
            IMatrix ret = null;
            GetProduct(ref ret);
            return ret;
        }

        protected double _determinant;

        protected bool _determinantCalculated = false;

        /// <summary>Returns the determinant.</summary>
        /// <returns>det(A)</returns>
        /// <exception cref="System.ArgumentException">Matrix must be square.</exception>
        public double Determinant
        {
            get
            {
                lock (Lock)
                {
                    if (Base == null)
                        throw new Exception("Invalid data.");
                    if (!_determinantCalculated)
                    {
                        _determinant = Base.Determinant;
                        _determinantCalculated = true;
                    }
                    return _determinant;
                }
            }
        }

        /// <summary>Calculates inverse of the decomposed matrix represented by the current object, 
        /// and stores it in the specified matrix.</summary>
        /// <param name="inv">Matrix where calculated inverse is stored.</param>
        public void Inverse(ref IMatrix inv)
        {
            if (Base == null)
                throw new Exception("Invalid data.");
            MatrixBase_MathNetNumerics  inverse = Base.Inverse();
            Matrix.Copy(inverse, ref inv);
        }

        /// <summary>Calculates and returns inverse of the decomposed matrix represented by the current object.</summary>
        public IMatrix Inverse()
        {
            IMatrix ret = null;
            Inverse(ref ret);
            return ret;
        }
        
        #region ToOverride

        /// <summary>Solves systems of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="righthandSides">Matrix that contains right-hand sides of the linear equations to be solved as its columns.</param>
        /// <returns>The Math.Net Numerics matrix whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="righthandSides"/> parameter.</returns>
        protected override MatrixBase_MathNetNumerics SolveMathNetNumerics(IMatrix righthandSides)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data: base object is null.");
                if (righthandSides == null)
                    throw new ArgumentNullException("Matrix of right-hand sides is not specified (null argument).");
                if (righthandSides.RowCount != this._rowCount)
                    throw new ArgumentException("Matrix of rihght-hand sides has wrong dimension, number of rows (" 
                        + righthandSides.RowCount + ") does not match number of equations (" + this._rowCount + ").");
                if (righthandSides.ColumnCount < 1)
                    throw new ArgumentException("Matrix of right-hand sides of systems of linear equations does not have any columns.");
                Matrix.Copy(righthandSides, ref _matrixRighthandSides_MathNetNumerics);
                _matrixSolution_MathNetNumerics = Base.Solve(_matrixRighthandSides_MathNetNumerics);
                return _matrixSolution_MathNetNumerics;
            }
        }


        /// <summary>Solves the system of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="rightHandSides">Vector of right-hand sides of the linear equations to be solved.</param>
        /// <returns>The Math.Net Numerics vector whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="rightHandSides"/> parameter.</returns>
        protected override VectorBase_MathNetNumerics SolveMathNetNumerics(IVector rightHandSides)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data: base object is null.");
                if (rightHandSides == null)
                    throw new ArgumentNullException("Vector of right-hand sides is not specified (null argument).");
                if (rightHandSides.Length != this._rowCount)
                    throw new InvalidOperationException("Vector of rihght-hand sides has wrong dimension (" 
                        + rightHandSides.Length + ") that does not match number of equations (" + this._rowCount + ").");
                Vector.Copy(rightHandSides, ref _vectorRighthandSides_MathNetNumerics);
                _vectorSolution_MathNetNumerics = Base.Solve(_vectorRighthandSides_MathNetNumerics);
                return _vectorSolution_MathNetNumerics;
            }
        }

        #endregion ToOverride


        ///// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        ///// matrix whose colums are right-hand sides of equations to be solved. Solutions 
        ///// are stored to the specified matrix.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        ///// <param name="X">Matrix where results are stored (one column for each right-hand side).</param>
        ///// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        ///// <exception cref="System.SystemException">Matrix is singular.</exception>
        //public virtual void Solve(Matrix B, ref IMatrix X)
        //{
        //    if (Base == null)
        //        throw new Exception("Invalid data.");
        //    if (B == null)
        //        throw new ArgumentNullException("Matrix of right-hand sides is not specified.");
        //    if (B.CopyMathNetNumerics == null)
        //        throw new ArgumentException("Matrix of right-hand sides: invalid data.");
        //    MatrixBase_MathNetNumerics sol = Base.Solve(B.CopyMathNetNumerics);
        //    Matrix.Copy(sol, ref X);
        //}

        ///// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        ///// matrix whose colums are right-hand sides of equations to be solved, and returns
        ///// a matrix whose columns are solutions of the specified systems of equations.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        ///// <returns>X so that L*U*X = B(piv,:)</returns>
        ///// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        ///// <exception cref="System.SystemException">Matrix is singular.</exception>
        //public IMatrix Solve(Matrix B)
        //{
        //    IMatrix ReturnedString = null;
        //    Solve(B, ref ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Solves a system of linear equations A*x=b, and stores the solution in the specified vector.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="b">Vector of right-hand sides.</param>
        ///// <param name="x">Vector where solution is stored.</param>
        //public virtual void Solve(Vector b, ref IVector x)
        //{
        //    if (b == null)
        //        throw new ArgumentNullException("Right-hand side vector is not specified (null reference)");
        //    if (b.Length != _rowCount)
        //        throw new Exception("Inconsistent dimension of the right-hand side vector.");
        //    VectorBase_MathNetNumerics sol = Base.Solve(b.CopyMathNetNumerics);
        //    Vector.Copy(sol, ref x);
        //}

        ///// <summary>Solves a system of linear equations A*x=b, and returns the solution.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="b">Right-hand side vector.</param>
        ///// <returns>Solution of the System such that L*U*x=s(piv,:).</returns>
        ////public IVector Solve(Vector b)
        //{
        //    IVector ReturnedString = null;
        //    Solve(b, ref ReturnedString);
        //    return ReturnedString;
        //}

    }  // class LUDecomposition


    /// <summary>QR decomposition of a matrix.
    /// <para>Objects of this class are immutable. Decomposition is calculated at initialization,
    /// and the decomposed matrix can not be replaced later.</para></summary>
    /// <remarks><para>
    /// Any real square matrix A may be decomposed as A = QR where Q is an orthogonal matrix 
    /// (its columns are orthogonal unit vectors meaning QTQ = I) and R is an upper triangular matrix 
    /// (also called right triangular matrix).
    /// </para></remarks>
    /// $A Igor Nov08 Apr12;
    [Serializable]
    public class QRDecomposition : LinearSolverBase, ILinearSolver, ILockable
    {

        protected internal QRDecomposition_MathNetNumerics Base = null;

        #region Construction

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public QRDecomposition(Matrix_MathNetNumerics A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to be decomposed is not specified (null reference).");
            if (A.RowCount <= 0 || A.ColumnCount <= 0)
                throw new ArgumentException("Inconsistent dimensions.");
            _rowCount = A.RowCount;
            _columnCount = A.ColumnCount;
            Base = A.QR(); // new QRDecomposition_MathNetNumerics(A);
        }

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public QRDecomposition(Matrix A)
            : this(A.CopyMathNetNumerics)
        { }

        #endregion Construction


        /// <summary>Indicates whether the matrix of coefficients of a linear system is nonsingular.</summary>
        /// <returns><c>true</c> if U, and hence A, is nonsingular.</returns>
        public bool IsNonSingular
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return Base.Determinant != 0;  // Base.IsNonSingular;
            }
        }


        /// <summary>Calculates the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition, and stores it to the specified matrix.</summary>
        /// <param name="product">Matrix where re-calculated product of the decomposed matrix is stored.</param>
        public void GetProduct(ref IMatrix product)
        {
            MatrixBase_MathNetNumerics prod = Base.Q * Base.R;
            Matrix.Copy(prod, ref product);
        }


        /// <summary>Calculates and returns the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition.</summary>
        public IMatrix GetProduct()
        {
            IMatrix ret = null;
            GetProduct(ref ret);
            return ret;
        }


        protected double _determinant;

        protected bool _determinantCalculated = false;

        /// <summary>Returns the determinant.</summary>
        /// <returns>det(A)</returns>
        /// <exception cref="System.ArgumentException">Matrix must be square.</exception>
        public double Determinant
        {
            get
            {
                lock (Lock)
                {
                    if (Base == null)
                        throw new Exception("Invalid data.");
                    if (!_determinantCalculated)
                    {
                        _determinant = Base.Determinant;
                        _determinantCalculated = true;
                    }
                    return _determinant;
                }
            }
        }

        MatrixBase_MathNetNumerics _inverse;

        /// <summary>Calculates inverse of the decomposed matrix represented by the current object, 
        /// and stores it in the specified matrix.</summary>
        /// <param name="inv">Matrix where calculated inverse is stored.</param>
        public void Inverse(ref IMatrix inv)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                if (_rowCount != _columnCount)
                    throw new InvalidOperationException("Can not calculate matric inverse: number of rows is diffeernt than number of columns.");
                if (_inverse == null)
                {
                    //Ainv = Base.Inverse();
                    MatrixBase_MathNetNumerics identity = new Matrix_MathNetNumerics(_rowCount, _rowCount);
                    for (int i = 0; i < _rowCount; ++i)
                        for (int j = 0; j < _rowCount; ++j)
                        {
                            if (i == j)
                                identity[i, j] = 1;
                            else
                                identity[i, j] = 0;
                        }
                    _inverse = Base.Solve(identity);
                }
            }
            Matrix.Copy(_inverse, ref inv);
        }


        /// <summary>Calculates and returns inverse of the decomposed matrix represented by the current object.</summary>
        public IMatrix Inverse()
        {
            IMatrix ret = null;
            Inverse(ref ret);
            return ret;
        }


        #region ToOverride

        /// <summary>Solves systems of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="righthandSides">Matrix that contains right-hand sides of the linear equations to be solved as its columns.</param>
        /// <returns>The Math.Net Numerics matrix whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="righthandSides"/> parameter.</returns>
        protected override MatrixBase_MathNetNumerics SolveMathNetNumerics(IMatrix righthandSides)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data: base object is null.");
                if (righthandSides == null)
                    throw new ArgumentNullException("Matrix of right-hand sides is not specified (null argument).");
                if (righthandSides.RowCount != this._rowCount)
                    throw new ArgumentException("Matrix of rihght-hand sides has wrong dimension, number of rows ("
                        + righthandSides.RowCount + ") does not match number of equations (" + this._rowCount + ").");
                if (righthandSides.ColumnCount < 1)
                    throw new ArgumentException("Matrix of right-hand sides of systems of linear equations does not have any columns.");
                Matrix.Copy(righthandSides, ref _matrixRighthandSides_MathNetNumerics);
                _matrixSolution_MathNetNumerics = Base.Solve(_matrixRighthandSides_MathNetNumerics);
                return _matrixSolution_MathNetNumerics;
            }
        }


        /// <summary>Solves the system of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="rightHandSides">Vector of right-hand sides of the linear equations to be solved.</param>
        /// <returns>The Math.Net Numerics vector whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="rightHandSides"/> parameter.</returns>
        protected override VectorBase_MathNetNumerics SolveMathNetNumerics(IVector rightHandSides)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data: base object is null.");
                if (rightHandSides == null)
                    throw new ArgumentNullException("Vector of right-hand sides is not specified (null argument).");
                if (rightHandSides.Length != this._rowCount)
                    throw new InvalidOperationException("Vector of rihght-hand sides has wrong dimension ("
                        + rightHandSides.Length + ") that does not match number of equations (" + this._rowCount + ").");
                Vector.Copy(rightHandSides, ref _vectorRighthandSides_MathNetNumerics);
                _vectorSolution_MathNetNumerics = Base.Solve(_vectorRighthandSides_MathNetNumerics);
                return _vectorSolution_MathNetNumerics;
            }
        }

        #endregion ToOverride



        ///// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        ///// matrix whose colums are right-hand sides of equations to be solved. Solutions 
        ///// are stored to the specified matrix.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        ///// <param name="X">Matrix where results are stored (one column for each right-hand side).</param>
        ///// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        ///// <exception cref="System.SystemException">Matrix is singular.</exception>
        //public virtual void Solve(Matrix B, ref IMatrix X)
        //{
        //    if (Base == null)
        //        throw new Exception("Invalid data.");
        //    if (B == null)
        //        throw new ArgumentNullException("Matrix of right-hand sides is not specified.");
        //    if (B.CopyMathNetNumerics == null)
        //        throw new ArgumentException("Matrix of right-hand sides: invalid data.");
        //    MatrixBase_MathNetNumerics sol = Base.Solve(B.CopyMathNetNumerics);
        //    Matrix.Copy(sol, ref X);
        //}

        ///// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        ///// matrix whose colums are right-hand sides of equations to be solved, and returns
        ///// a matrix whose columns are solutions of the specified systems of equations.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        ///// <returns>X so that L*U*X = B(piv,:)</returns>
        ///// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        ///// <exception cref="System.SystemException">Matrix is singular.</exception>
        //public IMatrix Solve(Matrix B)
        //{
        //    IMatrix ReturnedString = null;
        //    Solve(B, ref ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Solves a system of linear equations A*x=b, and stores the solution in the specified vector.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="b">Vector of right-hand sides.</param>
        ///// <param name="x">Vector where solution is stored.</param>
        //public virtual void Solve(Vector b, ref IVector x)
        //{
        //    if (b == null)
        //        throw new ArgumentNullException("Right-hand side vector is not specified (null reference)");
        //    if (b.Length != _rowCount)
        //        throw new Exception("Inconsistent dimension of the right-hand side vector.");
        //    VectorBase_MathNetNumerics sol = Base.Solve(b.CopyMathNetNumerics);
        //    Vector.Copy(sol, ref x);
        //}

        ///// <summary>Solves a system of linear equations A*x=b, and returns the solution.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="b">Right-hand side vector.</param>
        ///// <returns>Solution of the System such that L*U*x=s(piv,:).</returns>
        //public IVector Solve(Vector b)
        //{
        //    IVector ReturnedString = null;
        //    Solve(b, ref ReturnedString);
        //    return ReturnedString;
        //}

    }  // class QRDecomposition


    /// <summary>Cholesky decomposition of a matrix. Available for symmetric positive definite matrices.
    /// <para>Objects of this class are immutable. Decomposition is calculated at initialization,
    /// and the decomposed matrix can not be replaced later.</para></summary>
    /// <remarks><para>For a symmetric, positive definite matrix A, the Cholesky factorization
    /// is an lower triangular matrix L so that A = L*L'. </para>
    /// <para>The computation of the Cholesky factorization is done at construction time. If the matrix is not symmetric
    /// or positive definite, the constructor will throw an exception.
    /// </para></remarks>
    /// $A Igor Nov08 Apr12;
    [Serializable]
    public class CholeskyDecomposition : LinearSolverBase, ILinearSolver, ILockable
    {

        protected internal CholeskyDecomposition_MathNetNumerics Base = null;

        #region Construction

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public CholeskyDecomposition(Matrix_MathNetNumerics A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to be decomposed is not specified (null reference).");
            if (A.RowCount <= 0 || A.ColumnCount <= 0)
                throw new ArgumentException("Inconsistent dimensions.");
            _rowCount = A.RowCount;
            _columnCount = A.ColumnCount;
            Base = A.Cholesky();// new CholeskyDecomposition_MathNetNumerics(A);
        }

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public CholeskyDecomposition(IG.Num.Matrix A)
            : this(A.CopyMathNetNumerics)
        { }

        #endregion Construction

        
        /// <summary>Indicates whether the matrix of coefficients of a linear system is nonsingular.</summary>
        /// <returns><c>true</c> if U, and hence A, is nonsingular.</returns>
        public bool IsNonSingular
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return Base.Determinant != 0;  // Base.IsNonSingular;
            }
        }


        /// <summary>Calculates the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition, and stores it to the specified matrix.</summary>
        /// <param name="product">Matrix where re-calculated product of the decomposed matrix is stored.</param>
        public void GetProduct(ref IMatrix product)
        {
            MatrixBase_MathNetNumerics prod = Base.Factor*Base.Factor.Transpose();
            Matrix.Copy(prod, ref product);
        }


        /// <summary>Calculates and returns the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition.</summary>
        public IMatrix GetProduct()
        {
            IMatrix ret = null;
            GetProduct(ref ret);
            return ret;
        }


        protected double _determinant;

        protected bool _determinantCalculated = false;

        /// <summary>Returns the determinant.</summary>
        /// <returns>det(A)</returns>
        /// <exception cref="System.ArgumentException">Matrix must be square.</exception>
        public double Determinant
        {
            get
            {
                lock (Lock)
                {
                    if (Base == null)
                        throw new Exception("Invalid data.");
                    if (!_determinantCalculated)
                    {
                        _determinant = Base.Determinant;
                        _determinantCalculated = true;
                    }
                    return _determinant;
                }
            }
        }

        MatrixBase_MathNetNumerics _inverse;

        /// <summary>Calculates inverse of the decomposed matrix represented by the current object, 
        /// and stores it in the specified matrix.</summary>
        /// <param name="inv">Matrix where calculated inverse is stored.</param>
        public void Inverse(ref IMatrix inv)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                if (_rowCount != _columnCount)
                    throw new InvalidOperationException("Can not calculate matric inverse: number of rows is diffeernt than number of columns.");
                if (_inverse == null)
                {
                    //Ainv = Base.Inverse();
                    MatrixBase_MathNetNumerics identity = new Matrix_MathNetNumerics(_rowCount, _rowCount);
                    for (int i = 0; i < _rowCount; ++i)
                        for (int j = 0; j < _rowCount; ++j)
                        {
                            if (i == j)
                                identity[i, j] = 1;
                            else
                                identity[i, j] = 0;
                        }
                    _inverse = Base.Solve(identity);
                }
            }
            Matrix.Copy(_inverse, ref inv);
        }


        /// <summary>Calculates and returns inverse of the decomposed matrix represented by the current object.</summary>
        public IMatrix Inverse()
        {
            IMatrix ret = null;
            Inverse(ref ret);
            return ret;
        }

        #region ToOverride

        /// <summary>Solves systems of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="righthandSides">Matrix that contains right-hand sides of the linear equations to be solved as its columns.</param>
        /// <returns>The Math.Net Numerics matrix whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="righthandSides"/> parameter.</returns>
        protected override MatrixBase_MathNetNumerics SolveMathNetNumerics(IMatrix righthandSides)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data: base object is null.");
                if (righthandSides == null)
                    throw new ArgumentNullException("Matrix of right-hand sides is not specified (null argument).");
                if (righthandSides.RowCount != this._rowCount)
                    throw new ArgumentException("Matrix of rihght-hand sides has wrong dimension, number of rows ("
                        + righthandSides.RowCount + ") does not match number of equations (" + this._rowCount + ").");
                if (righthandSides.ColumnCount < 1)
                    throw new ArgumentException("Matrix of right-hand sides of systems of linear equations does not have any columns.");
                Matrix.Copy(righthandSides, ref _matrixRighthandSides_MathNetNumerics);
                _matrixSolution_MathNetNumerics = Base.Solve(_matrixRighthandSides_MathNetNumerics);
                return _matrixSolution_MathNetNumerics;
            }
        }


        /// <summary>Solves the system of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="rightHandSides">Vector of right-hand sides of the linear equations to be solved.</param>
        /// <returns>The Math.Net Numerics vector whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="rightHandSides"/> parameter.</returns>
        protected override VectorBase_MathNetNumerics SolveMathNetNumerics(IVector rightHandSides)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data: base object is null.");
                if (rightHandSides == null)
                    throw new ArgumentNullException("Vector of right-hand sides is not specified (null argument).");
                if (rightHandSides.Length != this._rowCount)
                    throw new InvalidOperationException("Vector of rihght-hand sides has wrong dimension ("
                        + rightHandSides.Length + ") that does not match number of equations (" + this._rowCount + ").");
                Vector.Copy(rightHandSides, ref _vectorRighthandSides_MathNetNumerics);
                _vectorSolution_MathNetNumerics = Base.Solve(_vectorRighthandSides_MathNetNumerics);
                return _vectorSolution_MathNetNumerics;
            }
        }

        #endregion ToOverride


        ///// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        ///// matrix whose colums are right-hand sides of equations to be solved. Solutions 
        ///// are stored to the specified matrix.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        ///// <param name="X">Matrix where results are stored (one column for each right-hand side).</param>
        ///// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        ///// <exception cref="System.SystemException">Matrix is singular.</exception>
        //public virtual void Solve(Matrix B, ref IMatrix X)
        //{
        //    if (Base == null)
        //        throw new Exception("Invalid data.");
        //    if (B == null)
        //        throw new ArgumentNullException("Matrix of right-hand sides is not specified.");
        //    if (B.CopyMathNetNumerics == null)
        //        throw new ArgumentException("Matrix of right-hand sides: invalid data.");
        //    MatrixBase_MathNetNumerics sol = Base.Solve(B.CopyMathNetNumerics);
        //    Matrix.Copy(sol, ref X);
        //}

        ///// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        ///// matrix whose colums are right-hand sides of equations to be solved, and returns
        ///// a matrix whose columns are solutions of the specified systems of equations.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        ///// <returns>X so that L*U*X = B(piv,:)</returns>
        ///// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        ///// <exception cref="System.SystemException">Matrix is singular.</exception>
        //public IMatrix Solve(Matrix B)
        //{
        //    IMatrix ReturnedString = null;
        //    Solve(B, ref ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Solves a system of linear equations A*x=b, and stores the solution in the specified vector.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="b">Vector of right-hand sides.</param>
        ///// <param name="x">Vector where solution is stored.</param>
        //public virtual void Solve(Vector b, ref IVector x)
        //{
        //    if (b == null)
        //        throw new ArgumentNullException("Right-hand side vector is not specified (null reference)");
        //    if (b.Length != _rowCount)
        //        throw new Exception("Inconsistent dimension of the right-hand side vector.");
        //    VectorBase_MathNetNumerics sol = Base.Solve(b.CopyMathNetNumerics);
        //    Vector.Copy(sol, ref x);
        //}

        ///// <summary>Solves a system of linear equations A*x=b, and returns the solution.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="b">Right-hand side vector.</param>
        ///// <returns>Solution of the System such that L*U*x=s(piv,:).</returns>
        //public IVector Solve(Vector b)
        //{
        //    IVector ReturnedString = null;
        //    Solve(b, ref ReturnedString);
        //    return ReturnedString;
        //}

    }  // class QRDecomposition


    /// <summary>Eigenvalue decomposition of a matrix.
    /// <para>Calculates eigenvectors and eigenvalues of a real matrix.</para>
    /// <para>Objects of this class are immutable. Decomposition is calculated at initialization,
    /// and the decomposed matrix can not be replaced later.</para></summary>
    /// <remarks>
    /// If A is symmetric, then A = V*D*V' where the eigenvalue matrix D is
    /// diagonal and the eigenvector matrix V is orthogonal.
    /// I.e. A = V*D*V' and V*VT=I.
    /// If A is not symmetric, then the eigenvalue matrix D is block diagonal
    /// with the real eigenvalues in 1-by-1 blocks and any complex eigenvalues,
    /// lambda + i*mu, in 2-by-2 blocks, [lambda, mu; -mu, lambda].  The
    /// columns of V represent the eigenvectors in the sense that A*V = V*D,
    /// i.e. A.Multiply(V) equals V.Multiply(D).  The matrix V may be badly
    /// conditioned, or even singular, so the validity of the equation
    /// A = V*D*Inverse(V) depends upon V.Condition().
    /// </remarks>
    /// $A Igor Feb09;
    [Serializable]
    public class EigenValueDecomposition : LinearSolverBase, ILinearSolver, ILockable
    {
        //protected internal int
        //    _rowCount = 0,  // number of rows of a decomposed matrix
        //    _columnCount = 0;  // number of columns of a decomposed matrix

        protected internal EigenValueDecomposition_MathNetNumerics Base = null;

        #region Construction

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public EigenValueDecomposition(Matrix_MathNetNumerics A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to be decomposed is not specified (null reference).");
            if (A.RowCount <= 0 || A.ColumnCount <= 0)
                throw new ArgumentException("Inconsistent dimensions.");
            _rowCount = A.RowCount;
            _columnCount = A.ColumnCount;
            Base = A.Evd(); // new EigenValueDecomposition_MathNetNumerics(A);
        }

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public EigenValueDecomposition(Matrix A)
            : this(A.CopyMathNetNumerics)
        { }

        #endregion Construction


        /// <summary>Indicates whether the matrix of coefficients of a linear system is nonsingular.</summary>
        /// <returns><c>true</c> if U, and hence A, is nonsingular.</returns>
        public bool IsNonSingular
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return Base.IsFullRank;
            }
        }

        /// <summary>Returns the right eigenvectors.</summary>
        public Matrix Eigenvectors
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.EigenVectors);

            }
        }


        /// <summary>Returns the eigen values as a an array of complex numbers.</summary>
        /// <returns>The eigen values.</returns>
        public System.Numerics.Complex[] EigenValues
        {
            get
            {
                VectorComplexBase_MathNetNumerics v = Base.EigenValues;
                int dim = v.Count;
                System.Numerics.Complex[] ret = new System.Numerics.Complex[dim];
                for (int i = 0; i < dim; ++i)
                    ret[i] = v[i];
                return ret;
            }
        }



        /// <summary>Calculates the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition, and stores it to the specified matrix.</summary>
        /// <param name="product">Matrix where re-calculated product of the decomposed matrix is stored.</param>
        public void GetProduct(ref IMatrix product)
        {
            MatrixBase_MathNetNumerics prod = Base.EigenVectors * Base.D * Base.EigenVectors.Transpose();
            Matrix.Copy(prod, ref product);
        }


        /// <summary>Calculates and returns the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition.</summary>
        public IMatrix GetProduct()
        {
            IMatrix ret = null;
            GetProduct(ref ret);
            return ret;
        }

        protected double _determinant;

        protected bool _determinantCalculated = false;

        /// <summary>Returns the determinant.</summary>
        /// <returns>det(A)</returns>
        /// <exception cref="System.ArgumentException">Matrix must be square.</exception>
        public double Determinant
        {
            get
            {
                lock (Lock)
                {
                    if (Base == null)
                        throw new Exception("Invalid data.");
                    if (!_determinantCalculated)
                    {
                        _determinant = Base.Determinant;
                        _determinantCalculated = true;
                    }
                    return _determinant;
                }
            }
        }


        MatrixBase_MathNetNumerics _inverse;

        /// <summary>Calculates inverse of the decomposed matrix represented by the current object, 
        /// and stores it in the specified matrix.</summary>
        /// <param name="inv">Matrix where calculated inverse is stored.</param>
        public void Inverse(ref IMatrix inv)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                if (_rowCount != _columnCount)
                    throw new InvalidOperationException("Can not calculate matric inverse: number of rows is diffeernt than number of columns.");
                if (_inverse == null)
                {
                    //Ainv = Base.Inverse();
                    MatrixBase_MathNetNumerics identity = new Matrix_MathNetNumerics(_rowCount, _rowCount);
                    for (int i = 0; i < _rowCount; ++i)
                        for (int j = 0; j < _rowCount; ++j)
                        {
                            if (i == j)
                                identity[i, j] = 1;
                            else
                                identity[i, j] = 0;
                        }
                    _inverse = Base.Solve(identity);
                }
            }
            Matrix.Copy(_inverse, ref inv);
        }

        /// <summary>Calculates and returns inverse of the decomposed matrix represented by the current object.</summary>
        public IMatrix Inverse()
        {
            IMatrix ret = null;
            Inverse(ref ret);
            return ret;
        }

        #region ToOverride

        /// <summary>Solves systems of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="righthandSides">Matrix that contains right-hand sides of the linear equations to be solved as its columns.</param>
        /// <returns>The Math.Net Numerics matrix whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="righthandSides"/> parameter.</returns>
        protected override MatrixBase_MathNetNumerics SolveMathNetNumerics(IMatrix righthandSides)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data: base object is null.");
                if (righthandSides == null)
                    throw new ArgumentNullException("Matrix of right-hand sides is not specified (null argument).");
                if (righthandSides.RowCount != this._rowCount)
                    throw new ArgumentException("Matrix of rihght-hand sides has wrong dimension, number of rows ("
                        + righthandSides.RowCount + ") does not match number of equations (" + this._rowCount + ").");
                if (righthandSides.ColumnCount < 1)
                    throw new ArgumentException("Matrix of right-hand sides of systems of linear equations does not have any columns.");
                Matrix.Copy(righthandSides, ref _matrixRighthandSides_MathNetNumerics);
                _matrixSolution_MathNetNumerics = Base.Solve(_matrixRighthandSides_MathNetNumerics);
                return _matrixSolution_MathNetNumerics;
            }
        }


        /// <summary>Solves the system of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="rightHandSides">Vector of right-hand sides of the linear equations to be solved.</param>
        /// <returns>The Math.Net Numerics vector whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="rightHandSides"/> parameter.</returns>
        protected override VectorBase_MathNetNumerics SolveMathNetNumerics(IVector rightHandSides)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data: base object is null.");
                if (rightHandSides == null)
                    throw new ArgumentNullException("Vector of right-hand sides is not specified (null argument).");
                if (rightHandSides.Length != this._rowCount)
                    throw new InvalidOperationException("Vector of rihght-hand sides has wrong dimension ("
                        + rightHandSides.Length + ") that does not match number of equations (" + this._rowCount + ").");
                Vector.Copy(rightHandSides, ref _vectorRighthandSides_MathNetNumerics);
                _vectorSolution_MathNetNumerics = Base.Solve(_vectorRighthandSides_MathNetNumerics);
                return _vectorSolution_MathNetNumerics;
            }
        }

        #endregion ToOverride


        ///// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        ///// matrix whose colums are right-hand sides of equations to be solved. Solutions 
        ///// are stored to the specified matrix.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        ///// <param name="X">Matrix where results are stored (one column for each right-hand side).</param>
        ///// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        ///// <exception cref="System.SystemException">Matrix is singular.</exception>
        //public virtual void Solve(Matrix B, ref IMatrix X)
        //{
        //    if (Base == null)
        //        throw new Exception("Invalid data.");
        //    if (B == null)
        //        throw new ArgumentNullException("Matrix of right-hand sides is not specified.");
        //    if (B.CopyMathNetNumerics == null)
        //        throw new ArgumentException("Matrix of right-hand sides: invalid data.");
        //    MatrixBase_MathNetNumerics sol = Base.Solve(B.CopyMathNetNumerics);
        //    Matrix.Copy(sol, ref X);
        //}

        ///// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        ///// matrix whose colums are right-hand sides of equations to be solved, and returns
        ///// a matrix whose columns are solutions of the specified systems of equations.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        ///// <returns>X so that L*U*X = B(piv,:)</returns>
        ///// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        ///// <exception cref="System.SystemException">Matrix is singular.</exception>
        //public IMatrix Solve(Matrix B)
        //{
        //    IMatrix ReturnedString = null;
        //    Solve(B, ref ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Solves a system of linear equations A*x=b, and stores the solution in the specified vector.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="b">Vector of right-hand sides.</param>
        ///// <param name="x">Vector where solution is stored.</param>
        //public virtual void Solve(Vector b, ref IVector x)
        //{
        //    if (b == null)
        //        throw new ArgumentNullException("Right-hand side vector is not specified (null reference)");
        //    if (b.Length != _rowCount)
        //        throw new Exception("Inconsistent dimension of the right-hand side vector.");
        //    VectorBase_MathNetNumerics sol = Base.Solve(b.CopyMathNetNumerics);
        //    Vector.Copy(sol, ref x);
        //}

        ///// <summary>Solves a system of linear equations A*x=b, and returns the solution.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="b">Right-hand side vector.</param>
        ///// <returns>Solution of the System such that L*U*x=s(piv,:).</returns>
        //public IVector Solve(Vector b)
        //{
        //    IVector ReturnedString = null;
        //    Solve(b, ref ReturnedString);
        //    return ReturnedString;
        //}

    }  // class EigenValueDecomposition


    /// <summary>Singular value decomposition of a matrix.
    /// <para>Calculates eigenvectors and eigenvalues of a real matrix.</para>
    /// <para>Objects of this class are immutable. Decomposition is calculated at initialization,
    /// and the decomposed matrix can not be replaced later.</para></summary>
    /// <remarks><para>
    /// Suppose M is an m-by-n matrix whose entries are real numbers. 
    /// Then there exists a factorization of the form M = UΣVT where:
    /// - U is an m-by-m unitary matrix;
    /// - Σ is m-by-n diagonal matrix with nonnegative real numbers on the diagonal;
    /// - VT denotes transpose of V, an n-by-n unitary matrix; 
    /// Such a factorization is called a singular-value decomposition of M. A common convention is to order the diagonal 
    /// entries Σ(i,i) in descending order. In this case, the diagonal matrix Σ is uniquely determined 
    /// by M (though the matrices U and V are not). The diagonal entries of Σ are known as the singular values of M.
    /// </para></remarks>
    /// $A Igor Feb09;
    [Serializable]
    public class SingularValueDecomposition : LinearSolverBase, ILinearSolver, ILockable
    {
        //protected internal int
        //    _rowCount = 0,  // number of rows of a decomposed matrix
        //    _columnCount = 0;  // number of columns of a decomposed matrix

        protected internal SingularValueDecomposition_MathNetNumerics Base = null;

        #region Construction

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public SingularValueDecomposition(Matrix_MathNetNumerics A)
        {
            if (A == null)
                throw new ArgumentNullException("Matrix to be decomposed is not specified (null reference).");
            if (A.RowCount <= 0 || A.ColumnCount <= 0)
                throw new ArgumentException("Inconsistent dimensions.");
            _rowCount = A.RowCount;
            _columnCount = A.ColumnCount;
            Base = A.Svd(); //new SingularValueDecomposition_MathNetNumerics(A, true /* computeVectors */);
        }

        /// <summary>Constructor.</summary>
        /// <param name="A">Matrix to be decomposed.</param>
        public SingularValueDecomposition(Matrix A)
            : this(A.CopyMathNetNumerics)
        { }

        #endregion Construction
        

        /// <summary>Indicates whether the matrix of coefficients of a linear system is nonsingular.</summary>
        /// <returns><c>true</c> if U, and hence A, is nonsingular.</returns>
        public bool IsNonSingular
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                if (_rowCount != _columnCount)
                    throw new InvalidOperationException("Singularity can not be checked for a non-square matrix.");
                return Base.Rank == Math.Min(_rowCount, _columnCount);
            }
        }

        /// <summary>Returns the left singular vectors.</summary>
        public Matrix LeftSingularVectors
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.U);
            }
        }


        /// <summary>Returns the singular values.</summary>
        public Vector SingularValues
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Vector(Base.S);
            }
        }


        /// <summary>Returns the right singular vectors.</summary>
        public Matrix RightSingularVectors
        {
            get
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                return new Matrix(Base.VT);
            }
        }

        

        /// <summary>Calculates the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition, and stores it to the specified matrix.</summary>
        /// <param name="product">Matrix where re-calculated product of the decomposed matrix is stored.</param>
        public void GetProduct(ref IMatrix product)
        {
            MatrixBase_MathNetNumerics prod = Base.U * Base.W * Base.VT;
            Matrix.Copy(prod, ref product);
        }

        /// <summary>Calculates and returns the product (i.e. the original matrix of coefficients of a linear system
        /// of equations) of the current decomposition.</summary>
        public IMatrix GetProduct()
        {
            IMatrix ret = null;
            GetProduct(ref ret);
            return ret;
        }

        protected double _determinant;

        protected bool _determinantCalculated = false;

        /// <summary>Returns the determinant.</summary>
        /// <returns>det(A)</returns>
        /// <exception cref="System.ArgumentException">Matrix must be square.</exception>
        public double Determinant
        {
            get
            {
                lock (Lock)
                {
                    if (Base == null)
                        throw new Exception("Invalid data.");
                    if (!_determinantCalculated)
                    {
                        _determinant = Base.Determinant;
                        _determinantCalculated = true;
                    }
                    return _determinant;
                }
            }
        }

        MatrixBase_MathNetNumerics _inverse;

        /// <summary>Calculates inverse of the decomposed matrix represented by the current object, 
        /// and stores it in the specified matrix.</summary>
        /// <param name="inv">Matrix where calculated inverse is stored.</param>
        public void Inverse(ref IMatrix inv)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new Exception("Invalid data.");
                if (_rowCount != _columnCount)
                    throw new InvalidOperationException("Can not calculate matric inverse: number of rows is diffeernt than number of columns.");
                if (_inverse == null)
                {
                    //Ainv = Base.Inverse();
                    MatrixBase_MathNetNumerics identity = new Matrix_MathNetNumerics(_rowCount, _rowCount);
                    for (int i = 0; i < _rowCount; ++i)
                        for (int j = 0; j < _rowCount; ++j)
                        {
                            if (i == j)
                                identity[i, j] = 1;
                            else
                                identity[i, j] = 0;
                        }
                    _inverse = Base.Solve(identity);
                }
            }
            Matrix.Copy(_inverse, ref inv);
        }

        /// <summary>Calculates and returns inverse of the decomposed matrix represented by the current object.</summary>
        public IMatrix Inverse()
        {
            IMatrix ret = null;
            Inverse(ref ret);
            return ret;
        }

        #region ToOverride

        /// <summary>Solves systems of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="righthandSides">Matrix that contains right-hand sides of the linear equations to be solved as its columns.</param>
        /// <returns>The Math.Net Numerics matrix whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="righthandSides"/> parameter.</returns>
        protected override MatrixBase_MathNetNumerics SolveMathNetNumerics(IMatrix righthandSides)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data: base object is null.");
                if (righthandSides == null)
                    throw new ArgumentNullException("Matrix of right-hand sides is not specified (null argument).");
                if (righthandSides.RowCount != this._rowCount)
                    throw new ArgumentException("Matrix of rihght-hand sides has wrong dimension, number of rows ("
                        + righthandSides.RowCount + ") does not match number of equations (" + this._rowCount + ").");
                if (righthandSides.ColumnCount < 1)
                    throw new ArgumentException("Matrix of right-hand sides of systems of linear equations does not have any columns.");
                Matrix.Copy(righthandSides, ref _matrixRighthandSides_MathNetNumerics);
                _matrixSolution_MathNetNumerics = Base.Solve(_matrixRighthandSides_MathNetNumerics);
                return _matrixSolution_MathNetNumerics;
            }
        }


        /// <summary>Solves the system of linear equations with the specified right-hand sides and the current matrix 
        /// decomposition by using the Math.Net Numerics library, and returns the result in Math.Net matrix form.</summary>
        /// <param name="rightHandSides">Vector of right-hand sides of the linear equations to be solved.</param>
        /// <returns>The Math.Net Numerics vector whose columns contains solutions to the systems of linear equations whose 
        /// right-hand sides are specified by yhe <paramref name="rightHandSides"/> parameter.</returns>
        protected override VectorBase_MathNetNumerics SolveMathNetNumerics(IVector rightHandSides)
        {
            lock (Lock)
            {
                if (Base == null)
                    throw new InvalidOperationException("Invalid data: base object is null.");
                if (rightHandSides == null)
                    throw new ArgumentNullException("Vector of right-hand sides is not specified (null argument).");
                if (rightHandSides.Length != this._rowCount)
                    throw new InvalidOperationException("Vector of rihght-hand sides has wrong dimension ("
                        + rightHandSides.Length + ") that does not match number of equations (" + this._rowCount + ").");
                Vector.Copy(rightHandSides, ref _vectorRighthandSides_MathNetNumerics);
                _vectorSolution_MathNetNumerics = Base.Solve(_vectorRighthandSides_MathNetNumerics);
                return _vectorSolution_MathNetNumerics;
            }
        }

        #endregion ToOverride


        ///// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        ///// matrix whose colums are right-hand sides of equations to be solved. Solutions 
        ///// are stored to the specified matrix.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        ///// <param name="X">Matrix where results are stored (one column for each right-hand side).</param>
        ///// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        ///// <exception cref="System.SystemException">Matrix is singular.</exception>
        //public virtual void Solve(Matrix B, ref IMatrix X)
        //{
        //    if (Base == null)
        //        throw new Exception("Invalid data.");
        //    if (B == null)
        //        throw new ArgumentNullException("Matrix of right-hand sides is not specified.");
        //    if (B.CopyMathNetNumerics == null)
        //        throw new ArgumentException("Matrix of right-hand sides: invalid data.");
        //    MatrixBase_MathNetNumerics sol = Base.Solve(B.CopyMathNetNumerics);
        //    Matrix.Copy(sol, ref X);
        //}

        ///// <summary>Solves A*X = B (a set of linear systems of equations), where B is the 
        ///// matrix whose colums are right-hand sides of equations to be solved, and returns
        ///// a matrix whose columns are solutions of the specified systems of equations.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="B">A Matrix with as many rows as A and any number of columns (right-hand sides).</param>
        ///// <returns>X so that L*U*X = B(piv,:)</returns>
        ///// <exception cref="System.ArgumentException">Matrix row dimensions must agree.</exception>
        ///// <exception cref="System.SystemException">Matrix is singular.</exception>
        //public IMatrix Solve(Matrix B)
        //{
        //    IMatrix ReturnedString = null;
        //    Solve(B, ref ReturnedString);
        //    return ReturnedString;
        //}

        ///// <summary>Solves a system of linear equations A*x=b, and stores the solution in the specified vector.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="b">Vector of right-hand sides.</param>
        ///// <param name="x">Vector where solution is stored.</param>
        //public virtual void Solve(Vector b, ref IVector x)
        //{
        //    if (b == null)
        //        throw new ArgumentNullException("Right-hand side vector is not specified (null reference)");
        //    if (b.Length != _rowCount)
        //        throw new Exception("Inconsistent dimension of the right-hand side vector.");
        //    VectorBase_MathNetNumerics sol = Base.Solve(b.CopyMathNetNumerics);
        //    Vector.Copy(sol, ref x);
        //}

        ///// <summary>Solves a system of linear equations A*x=b, and returns the solution.
        ///// <para>Decomposed matrix of coefficients A is represented by the current object.</para></summary>
        ///// <param name="b">Right-hand side vector.</param>
        ///// <returns>Solution of the System such that L*U*x=s(piv,:).</returns>
        //public IVector Solve(Vector b)
        //{
        //    IVector ReturnedString = null;
        //    Solve(b, ref ReturnedString);
        //    return ReturnedString;
        //}

    }  // class SingularValueDecomposition


}
