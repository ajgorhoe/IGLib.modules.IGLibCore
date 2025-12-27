
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;


namespace IG.Num
{


    /// <summary>Affine transformation with diagonal tansformation matrix.</summary>
    /// $A Igor dec10;
    /// TODO: IMPLEMENT THIS CORRECTLY!
    /// Currently this is just general square affine transformation with adapted constructors!!!
    /// TODO: implement ICloneable!
    public class AffineTransformationDiagonal : AffineTransformationSquare, 
        IAffineTransformation, ILockable
    {
        

        /// <summary>Constructs diagonal affine transformation with the specified dimension of the original (reference)
        /// and transformed (target) space.</summary>
        /// <param name="dimension">Dimension of the original and transformed space.</param>
        public AffineTransformationDiagonal(int dimension)
        {
            if (dimension < 1)
                throw new ArgumentException("Can not create affine transformation with space dimension less than 1.");
            this.Dimension = dimension;
        }


        /// <summary>Constructs affine transformation with the specified transformation matrix and translation vector.</summary>
        /// <param name="diagonalOfTransfMat">Transformation matrix of the affine transformation.</param>
        /// <param name="translationVec">Translation vector of the affine transformation.</param>
        public AffineTransformationDiagonal(IVector diagonalOfTransfMat, IVector translationVec) 
        {
            int dim = 0;
            if (diagonalOfTransfMat != null)
                dim = diagonalOfTransfMat.Length;
            else if (translationVec != null)
                dim = translationVec.Length;
            this.TransformationMatrix = new Matrix(diagonalOfTransfMat);
            this.TranslationVector = translationVec;
        }








    }  // AffineTransformationDiagonal





    /// <summary>Implementation of a general affine transformation.</summary>
    /// $A Igor Dec10;
    /// TODO:  Delete this class later, it is used only as template for copying some methods!
    public class AffineTransformationDiagonal0_TO_DELETE // :  IAffineTransformation, ILockable  // TODO:  uncomment the interface!
    {

        #region Construction

        ///// <summary>Constructs affine transformation with the specified dimensions of the original (reference)
        ///// and transformed (target) space.</summary>
        ///// <param name="dimOriginal">Dimension of the original space.</param>
        ///// <param name="dimTransformed">Dimension of the reference space.</param>
        //public AffineTransformationDiagonal(int dimOriginal, int dimTransformed)
        //    // : base(dimOriginal, dimTransformed)
        //{
        //    DimensionOriginal = dimOriginal;
        //    DimensionTransformed = dimTransformed;
        //}


        /// <summary>Constructs affine transformation with the specified transformation matrix and translation vector.</summary>
        /// <param name="transformationMatrixDiagonal">Transformation matrix of the affine transformation.</param>
        /// <param name="translationVector">Translation vector of the affine transformation.</param>
        public AffineTransformationDiagonal0_TO_DELETE(IVector transformationMatrixDiagonal, IVector translationVector) // :
            // base(transformationMatrix, translationVector)
        {
            this.TransformationMatrixDiagonal = transformationMatrixDiagonal;
            this.TranslationVector = translationVector;
        }

        #endregion Construction


        #region Data


        private object _lockObj = new object();

        /// <summary>Object used for locking (implementation of ILockable interface).</summary>
        /// <remarks>Due to performance reasons, locking is not within methods of this class, even if method use 
        /// internal fields. Thread safety must be achieved by explicit locking.</remarks>
        public object Lock { get { return _lockObj; } }

        private int _dimension;

        /// <summary>Dimension of the original (reference) space.</summary>
        /// <remarks>Dimensions of original and transformed space are equal for this class because 
        /// transformation matrix is diagonal.</remarks>
        public int DimensionOriginal
        {
            get { return _dimension; }
            protected set { _dimension = value; }
        }

        /// <summary>Dimension of the transformed space.</summary>
        /// <remarks>Dimensions of original and transformed space are equal for this class because 
        /// transformation matrix is diagonal.</remarks>
        public int DimensionTransformed
        {
            get { return _dimension; }
            protected set { _dimension = value; }
        }



        private IVector _transfMatDiagonal;

        private IVector _translationVector;

        private IMatrix _transfMat;

        /// <summary>Vector of diagonal elements of the transformation matrix (since transformation matrix is diagonal).</summary>
        public IVector TransformationMatrixDiagonal
        {
            get { return _transfMatDiagonal; }
            set 
            { 
                _transfMatDiagonal = value;
                if (value == null)
                    TransformationMatrix = null;
                else 
                {
                    DimensionOriginal = DimensionTransformed = value.Length;
                    int dim = value.Length;
                    if (TransformationMatrix.RowCount != value.Length || TransformationMatrix.ColumnCount != value.Length)
                        TransformationMatrix = null;
                    if (TransformationMatrix != null)
                    {
                        TransformationMatrix.SetZero();
                        for (int i = 0; i<dim; ++i)
                            for (int j=0; j<dim; ++j)
                            {
                                if (i!=j)
                                    TransformationMatrix[i, j] = 0;
                                else
                                    TransformationMatrix[i, j] = value[i];
                            }
                    }
                }
            }
        }

        /// <summary>Translation vector of Affine transformation.</summary>
        public IVector TranslationVector
        {
            get { return _translationVector; }
            set 
            { 
                _translationVector = value;
                if (value!=null)
                    DimensionOriginal = DimensionTransformed = value.Length;
            }
        }

        public IMatrix TransformationMatrix
        {
            get
            {
                if (_transfMat == null)
                {
                    if (TransformationMatrixDiagonal != null)
                    {
                        int dim = TransformationMatrixDiagonal.Length;
                        _transfMat = TransformationMatrixDiagonal.GetNewMatrix();
                        for (int i=0; i<dim; ++i)
                            for (int j = 0; j < dim; ++j)
                            {
                                if (i == j)
                                    _transfMat[i, i] = TransformationMatrixDiagonal[i];
                                else
                                    _transfMat[i,j] = 0;
                            }
                    }
                }
                return _transfMat;
            }
            set
            {
                if (value == null)
                {
                    _transfMat = value;
                    TransformationMatrixDiagonal = null;
                } else
                {
                    if (value.RowCount != value.ColumnCount)
                    {
                        throw new ArgumentException("Transformation matrix for diagonal Affine transformation is not square.");
                    }
                    // Check that the matrix  has onlydiagonal elements different thatn null!
                    for (int i = 0; i < value.RowCount; ++i)
                    {
                        for (int j = 0; j < i; ++i)
                        {
                            if (value[i, j] != 0.0)
                                throw new ArgumentException("Transformation matrix of diagonal Affine transformation is not diagonal. "
                                    + Environment.NewLine + "  Firts non-zero component: [" + i + ", " + j + "].");
                            if (value[j, i] != 0.0)
                                throw new ArgumentException("Transformation matrix of diagonal Affine transformation is not diagonal. "
                                    + Environment.NewLine + "  Firts non-zero component: [" + j + ", " + i + "].");
                        }
                    }
                    _transfMat = value;
                    for (int i = 0; i < value.RowCount; ++i)
                    {
                        // Set the diagonal component:
                        TransformationMatrixDiagonal[i] = value[i, i];
                    }
                }
            }
        }

        #endregion Data

        #region Operation


        #endregion Operation


    }  // AffineTransformationDiagonal0_TO_DELETE

}
