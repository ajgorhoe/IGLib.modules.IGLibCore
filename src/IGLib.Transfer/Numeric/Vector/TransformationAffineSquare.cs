using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{

    // TODO:
    // For efficiency reasons, it might be better to skip the base function and implement directly
    // AffineTransformation and AffineTransformationDiagonal 
    // (in this way, virtual methods can be avoided, which means some less overhead)


    /// <summary>Invertible Affine transformation with square transformation matrix.</summary>
    /// $A Igor Jul10 Dec10;
    /// //TODO: implement IAffine transfoermation!
    public abstract class AffineTransformationSquare : IAffineTransformation, ILockable
    {

        #region Construction

        protected AffineTransformationSquare() {  }

        /// <summary>Constructs affine transformation with the specified dimensions of the original (reference)
        /// and transformed (target) space.</summary>
        /// <param name="dimension">Dimension of the original space.</param>
        public AffineTransformationSquare(int dimension)
        {
            if (dimension < 1)
                throw new ArgumentException("Can not create affine transformation with space dimension less than 1.");
            this.Dimension = dimension;
        }

        /// <summary>Constructs affine transformation with the specified transformation matrix and translation vector.
        /// Either of the transformation matrix or translation vector can be null, but not both.</summary>
        /// <param name="transformationMatrix">Transformation matrix of the affine transformation.</param>
        /// <param name="translationVector">Translation vector of the affine transformation.</param>
        /// <param name="inverseSpecified">Whether inverse is specified.</param>
        public AffineTransformationSquare(Matrix transformationMatrix, Vector translationVector, bool inverseSpecified)
        {
            if (transformationMatrix == null && translationVector == null)
                throw new ArgumentNullException("Neither transformation matrix nor translation vector not defined (null reference).");
            else if (transformationMatrix != null)
            {
                if (transformationMatrix.RowCount != transformationMatrix.ColumnCount)
                    throw new ArgumentException("Transformation matrix specified is not a square matrix. Dimensions: "
                        + transformationMatrix.RowCount + "x" + transformationMatrix.ColumnCount + ".");
                else if (translationVector.Length != transformationMatrix.RowCount)
                    throw new ArgumentException("Translation vector has different dimension (" + translationVector.Length
                        + ") than transformation matrix (" + transformationMatrix.RowCount + "x" + transformationMatrix.ColumnCount + ")." );
            }
            this.TransformationMatrix = transformationMatrix;
            this.TranslationVector = translationVector;
        }


        /// <summary>Constructs affine transformation with the specified transformation matrix and translation vector.
        /// Either of the transformation matrix or translation vector can be null, but not both.</summary>
        /// <param name="transformationMatrix">Transformation matrix of the affine transformation.</param>
        /// <param name="translationVector">Translation vector of the affine transformation.</param>
        public AffineTransformationSquare(Matrix transformationMatrix, Vector translationVector) :
            this(transformationMatrix, translationVector, false /* inverseSpecified */ )
        {
            if (transformationMatrix == null && translationVector == null)
                throw new ArgumentNullException("Neither transformation matrix nor translation vector not defined (null reference).");
            else if (transformationMatrix != null)
            {
                if (transformationMatrix.RowCount != transformationMatrix.ColumnCount)
                    throw new ArgumentException("Transformation matrix specified is not a square matrix. Dimensions: "
                        + transformationMatrix.RowCount + "x" + transformationMatrix.ColumnCount + ".");
                else if (translationVector.Length != transformationMatrix.RowCount)
                    throw new ArgumentException("Translation vector has different dimension (" + translationVector.Length
                        + ") than transformation matrix (" + transformationMatrix.RowCount + "x" + transformationMatrix.ColumnCount + ")." );
            }
            this.TransformationMatrix = transformationMatrix;
            this.TranslationVector = translationVector;
        }


        #endregion Construction

        #region Data

        private object _lockObj = new object();

        /// <summary>Object used for locking of the current object (ILockable interface).</summary>
        public object Lock { get { return _lockObj; } }

        private int _spaceDimension;

        /// <summary>Dimension of the original (reference) space.</summary>
        public int DimensionOriginal
        {
            get { return _spaceDimension; }
            protected set
            {
                if (value <= 0)
                    throw new ArgumentException("Space dimension in square affine transformation can not be less or equal to 0.");
                _spaceDimension = value;
            }
        }

        /// <summary>Dimension of the transformed space.</summary>
        public int DimensionTransformed
        {
            get { return _spaceDimension; }
            protected set
            {
                if (value <= 0)
                    throw new ArgumentException("Space dimension in square affine transformation can not be less or equal to 0.");
                _spaceDimension = value;
            }
        }

        /// <summary>Dimension of the original and transformed space (which are equal for this class).</summary>
        public int Dimension
        { 
            get { return _spaceDimension; }
            protected set 
            {
                if (value <= 0)
                    throw new ArgumentException("Space dimension in square affine transformation can not be less or equal to 0.");
                _spaceDimension = value;
            }
        }

        private bool _calcTransformationMatrix;

        /// <summary>Flag indicating whethet the transformation matrix is calculated or not.</summary>
        public bool CalculatedTransformationMatrix
        {
            get { return _calcTransformationMatrix; }
            protected set { _calcTransformationMatrix = value; }
        }

        private IMatrix _transfMatrix;

        /// <summary>Gets or sets transformation matrix that transforms vector of co-ordinates from 
        /// the original to the reference space.</summary>
        public virtual IMatrix TransformationMatrix
        {
            get 
            {
                if (!CalculatedTransformationMatrix)
                {
                    lock (Lock)
                    {
                        CalculateTransformationMatrix();
                    }
                }
                return _transfMatrix;
            }
            set 
            {
                lock (Lock)
                {
                    _transfMatrix = value;
                    CalculatedInverseTransformationMatrix = false;
                    if (value != null)
                        if (value.RowCount != Dimension)
                        {
                            // Dimension has changed!
                            TranslationVector = null;
                            Dimension = value.RowCount;
                        }
                    CalculatedTransformationMatrix = true;
                }
            }
        }


        private bool _calcInverseTransformationMatrix;

        /// <summary>Flag indicating whethet the transformation matrix is calculated or not.</summary>
        public bool CalculatedInverseTransformationMatrix
        {
            get { return _calcInverseTransformationMatrix; }
            protected set { _calcInverseTransformationMatrix = value; }
        }



        /// <summary>Gets or sets inverse of the transformation matrix that transforms vector of co-ordinates from 
        /// the original to the reference space.</summary>
        public virtual IMatrix InverseTransformationMatrix
        {
            get
            {
                if (!CalculatedInverseTransformationMatrix)
                {
                    lock (Lock)
                    {
                        CalculateInverseTransformationMatrix();
                    }
                }
                return _transfMatrix;
            }
            set
            {
                lock (Lock)
                {
                    _transfMatrix = value;
                    CalculatedTransformationMatrix = false;
                    if (value != null)
                        if (value.RowCount != Dimension)
                        {
                            // Dimension has changed!
                            TranslationVector = null;
                            Dimension = value.RowCount;
                        }
                    CalculatedInverseTransformationMatrix = true;
                }
            }
        }


        protected Vector _auxVec;

        protected Matrix _identityMat;

        protected Matrix _auxMat;

        /// <summary>Initializes auxiliary vector.</summary>
        protected void InitAuxVec()
        {
            // Initialization of the auxiliary vector:
            if (_auxVec == null)
                _auxVec = new Vector(Dimension);
            else if (_auxVec.Length != Dimension)
                _auxVec = new Vector(Dimension);
        }

        /// <summary>Initializes the auxiliary identity matrix.</summary>
        protected void InitIdentityMat()
        {
            bool resize=false;
            if (_identityMat == null)
                resize=true;
            else if (_identityMat.RowCount != Dimension)
                resize=true;
            if (resize)
            {
                _identityMat = new Matrix(Dimension, Dimension);
                for (int i=0; i<Dimension; ++i)
                    for (int j = 0; j < Dimension; ++j)
                    {
                        if (i == j)
                            _identityMat[i, j] = 1;
                        else _identityMat[i, j] = 0;
                    }
            }
        }


        /// <summary>Initializes the auxiliary matrix.</summary>
        protected void InitAuxMat()
        {
            bool resize = false;
            if (_auxMat == null)
                resize = true;
            else if (_auxMat.RowCount != Dimension)
                resize = true;
            if (resize)
            {
                _auxMat = new Matrix(Dimension, Dimension);
            }
        }


        /// <summary>Calculates the transformation matrix from the inverse transformation matrix.</summary>
        protected virtual void CalculateTransformationMatrix()
        {
            if (!CalculatedInverseTransformationMatrix)
                throw new InvalidOperationException("Can not calculate the transformation matrix: Inverse transformation matrix is not calculated.");
            IMatrix mat = InverseTransformationMatrix;
            if (mat == null)
            {
                TransformationMatrix = null;
                CalculatedTransformationMatrix = true;
            }
            else
            {
                InitIdentityMat();
                InitAuxMat();
                Matrix.CopyPlain(mat, _auxMat);
                LUDecomposition decomp = new LUDecomposition(_auxMat);
                TransformationMatrix = decomp.Solve(_identityMat);
                CalculatedTransformationMatrix = true;
            }
        }


        /// <summary>Calculates the inverse transformation matrix from the direct transformation matrix.</summary>
        protected virtual void CalculateInverseTransformationMatrix()
        {
            if (!CalculatedTransformationMatrix)
                throw new InvalidOperationException("Can not calculate the transformation matrix: Inverse transformation matrix is not calculated.");
            IMatrix mat = TransformationMatrix;
            if (mat == null)
            {
                TransformationMatrix = null;
                CalculatedInverseTransformationMatrix = true;
            }
            else
            {
                InitIdentityMat();
                InitAuxMat();
                Matrix.CopyPlain(mat, _auxMat);
                LUDecomposition decomp = new LUDecomposition(_auxMat);
                InverseTransformationMatrix = decomp.Solve(_identityMat);
                CalculatedInverseTransformationMatrix = true;
            }
        }



        private IVector _translation;

        /// <summary>Gets or sets the translation vector of the affine transformation.</summary>
        public virtual IVector TranslationVector
        {
            get { return _translation; }
            set 
            { 
                _translation = value;
                if (value!=null)
                {
                    if (value.Length!=Dimension)
                    {
                        // Dimension has changed!
                        CalculatedTransformationMatrix = false;
                        CalculatedInverseTransformationMatrix = false;
                        Dimension=value.Length;
                    }
                }
            }
        }


        /// <summary>Copies transformation matrix to the specified storage matrix.</summary>
        /// <param name="store">Storage matrix that transformation matrix is copied to.</param>
        public virtual void CopyTransformationMatrix(ref IMatrix store)
        {
            Matrix.Copy(this.TransformationMatrix, ref store);
        }


        /// <summary>Copies inverse transformation matrix to the specified storage matrix.</summary>
        /// <param name="store">Storage matrix that inverse transformation matrix is copied to.</param>
        public virtual void CopyInverseTransformationMatrix(ref IMatrix store)
        {
            Matrix.Copy(this.InverseTransformationMatrix, ref store);
        }

       #endregion Data

        #region Operation

        #region CoordinatesTransformation

        /// <summary>Transforms co-ordinates from the reference to the transformed space.
        /// WARNING: This is a plain version that does not check dimensions.</summary>
        /// <param name="original">Vector of original co-ordinates.</param>
        /// <param name="result">Vector where transformed co-ordinates are stored.</param>
        public void TransformCoordinatesPlain(IVector original, IVector result)
        {
            IMatrix transfMat = TransformationMatrix;
            if (transfMat != null)
                Matrix.MultiplyPlain(transfMat, original, result);
            else
                Vector.CopyPlain(original, result);
            IVector transVec = TranslationVector;
            if (transVec != null)
                Vector.AddPlain(result, transVec, result);
        }

        /// <summary>Transforms co-ordinates from the reference to the transformed space.</summary>
        /// <param name="original">Vector of original co-ordinates.</param>
        /// <param name="result">Vector where transformed co-ordinates are stored.</param>
        public void TransformCoordinates(IVector original, ref IVector result)
        {
            if (original == null)
                throw new ArgumentNullException("Vector of original co-ordinates not specified.");
            else if (original.Length != Dimension)
                throw new ArgumentException("Vector of original co-ordinates is not of correct dimension - "
                    + original.Length + " instead of " + Dimension + ".");
            if (result == null)
                result = original.GetNew();
            else if (result.Length != Dimension)
                result = original.GetNew();
            TransformCoordinatesPlain(original, result);
        }


        /// <summary>Backward transforms co-ordinates from the transformed to the reference space.</summary>
        /// <param name="transformed">Vector of transformed co-ordinates.</param>
        /// <param name="result">Vector where inverse-transformed original co-ordinates are stored.</param>
        public void TransformBackCoordinatesPlain(IVector transformed, IVector result)
        {
            IVector transVec = TranslationVector;
            IMatrix ivntransfMat = InverseTransformationMatrix;
            if (transVec != null)
            {
                if (ivntransfMat!=null)
                {
                    // TODO: try to avoid this step, e.g. make sure that _auxVec is always initialized when inverse transformation 
                    // matrix is defined!
                    InitAuxVec();
                    Vector.SubtractPlain(transformed, transVec, _auxVec);
                    Matrix.MultiplyPlain(ivntransfMat, _auxVec, result);

                } else
                    Vector.SubtractPlain(transformed, transVec, result);
            } else
            {
                // transVec == null
                if (ivntransfMat != null)
                    Matrix.MultiplyPlain(ivntransfMat, transformed, result);
                else
                    Vector.CopyPlain(transformed, result);  // transformation matrix and translation vector not defined - identity transformation.
            }
        }

        /// <summary>Backward transforms co-ordinates from the transformed to the reference space.</summary>
        /// <param name="transformed">Vector of transformed co-ordinates.</param>
        /// <param name="result">Vector where inverse-transformed original co-ordinates are stored.</param>
        public void TransformBackCoordinates(IVector transformed, ref IVector result)
        {
            if (transformed == null)
                throw new ArgumentNullException("Vector of transformed co-ordinates not specified.");
            else if (transformed.Length != Dimension)
                throw new ArgumentException("Vector of transformed co-ordinates is not of correct dimension - "
                    + transformed.Length + " instead of " + Dimension + ".");
            if (result == null)
                result = transformed.GetNew();
            else if (result.Length != Dimension)
                result = transformed.GetNew();
            TransformBackCoordinatesPlain(transformed, result);
        }

        #endregion CoordinatesTransformation


        #region GradientTransformation

        /// <summary>Transforms gradient (or other covariant vector) from the reference to the transformed space.
        /// WARNING: This is a plain version that does not check dimensions.</summary>
        /// <param name="original">Original gradient-like vector.</param>
        /// <param name="result">Vector where resulting transformed vector is stored.</param>
        public void TransformGradientPlain(IVector original, IVector result)
        {
            IMatrix invTransfMat = InverseTransformationMatrix;
            if (invTransfMat != null)
                Matrix.MultiplyTranspVecPlain(invTransfMat, original, result);
                // Matrix.MultiplyPlain(invTransfMat, original, result);
            else
                Vector.CopyPlain(original, result);
        }

        /// <summary>Transforms gradient (or other covariant vector) from the reference to the transformed space.</summary>
        /// <param name="original">Original gradient-like vector.</param>
        /// <param name="result">Vector where resulting transformed vector is stored.</param>
        public void TransformGradient(IVector original, ref IVector result)
        {
            if (original == null)
                throw new ArgumentNullException("Original covariant (gradient-like) vector not specified.");
            else if (original.Length != Dimension)
                throw new ArgumentException("Original covariant (gradient-like) vector is not of correct dimension - "
                    + original.Length + " instead of " + Dimension + ".");
            if (result == null)
                result = original.GetNew();
            else if (result.Length != Dimension)
                result = original.GetNew();
            TransformGradientPlain(original, result);
        }

        /// <summary>Backward transforms gradient (or other covariant vector) from the transformed to the reference space.</summary>
        /// <param name="transformed">Transformed gradient-like vector.</param>
        /// <param name="result">Vector where resulting backward transformed vector is stored.</param>
        public void TransformBackGradientPlain(IVector transformed, IVector result)
        {
            IMatrix transfMat = TransformationMatrix;
            if (transfMat != null)
                Matrix.MultiplyTranspVecPlain(transfMat, transformed, result);
                // Matrix.MultiplyPlain(transfMat, transformed, result);
            else
                Vector.CopyPlain(transformed, result);  // transformation matrix and translation vector not defined - identity transformation.
        }

        /// <summary>Backward transforms gradient (or other covariant vector) from the transformed to the reference space.</summary>
        /// <param name="transformed">Transformed gradient-like vector.</param>
        /// <param name="result">Vector where resulting backward transformed vector is stored.</param>
        public void TransformBackGradient(IVector transformed, ref IVector result)
        {
            if (transformed == null)
                throw new ArgumentNullException("Transformed covariant (gradient-like) vector not specified.");
            else if (transformed.Length != Dimension)
                throw new ArgumentException("Transformed covariant (gradient-like) vector is not of correct dimension - "
                    + transformed.Length + " instead of " + Dimension + ".");
            if (result == null)
                result = transformed.GetNew();
            else if (result.Length != Dimension)
                result = transformed.GetNew();
            TransformBackGradientPlain(transformed, result);
        }

        #endregion GradientTransformatioon


        #region HessianTransformation

        /// <summary>Transforms Hessian (or other covariant matrix) from the reference to the transformed space.
        /// WARNING: This is a plain version that does not check dimensions.</summary>
        /// <param name="original">Original matrix.</param>
        /// <param name="result">Matrix where resulting transformed matrix is stored.</param>
        public void TransformHessianPlain(IMatrix original, IMatrix result)
        {
            IMatrix invTransfMat = InverseTransformationMatrix;
            if (invTransfMat != null)
                Matrix.MultiplyTranspMatMatPlain(invTransfMat, original, invTransfMat, result);
                // Matrix.Multiply(invTransfMat, original, invTransfMat, result);
            else
                Matrix.CopyPlain(original, result);
        }

        /// <summary>Transforms Hessian (or other covariant matrix) from the reference to the transformed space.</summary>
        /// <param name="original">Original matrix.</param>
        /// <param name="result">Matrix where resulting transformed matrix is stored. Allocated/reallocated if necessary.</param>
        public void TransformHessian(IMatrix original, ref IMatrix result)
        {
            if (original == null)
                throw new ArgumentNullException("Original covariant (Hessian-like) matrix not specified.");
            else if (original.RowCount != Dimension || original.ColumnCount!=Dimension)
                throw new ArgumentException("Original covariant (Hessian-like) matrix is not of correct dimension - "
                    + original.RowCount + "x" + original.ColumnCount + " instead of " + Dimension + "x" + Dimension + ".");
            if (result == null)
                result = original.GetNew();
            else if (result.RowCount != Dimension || result.ColumnCount != Dimension)
                result = original.GetNew();
            TransformHessianPlain(original, result);
        }

        /// <summary>Backward transforms Hessian (or other covariant matrix) from the transformed to the reference space.</summary>
        /// <param name="transformed">Matrix in transformed coordinates.</param>
        /// <param name="result">Matrix where resulting backward-transformed matrix is stored.</param>
        public void TransformBackHessianPlain(IMatrix transformed, IMatrix result)
        {
            IMatrix transfMat = TransformationMatrix;
            if (transfMat != null)
                Matrix.MultiplyTranspMatMatPlain(transfMat, transformed, transfMat, result);
                // Matrix.MultiplyPlain(transfMat, transformed, transfMat, result);
            else
                Matrix.CopyPlain(transformed, result);  // transformation matrix and translation vector not defined - identity transformation.
        }

        /// <summary>Backward transforms Hessian (or other covariant matrix) from the transformed to the reference space.</summary>
        /// <param name="transformed">Matrix in transformed coordinates.</param>
        /// <param name="result">Matrix where resulting backward-transformed matrix is stored. Allocated/reallocated if necessary.</param>
        public void TransformBackHessian(IMatrix transformed, ref IMatrix result)
        {
            if (transformed == null)
                throw new ArgumentNullException("Transformed covariant (Hessian-like) matrix not specified.");
            else if (transformed.RowCount != Dimension || transformed.ColumnCount!=Dimension)
                throw new ArgumentException("Transformed covariant (Hessian-like) matrix is not of correct dimension - "
                    + transformed.RowCount + "x" + transformed.ColumnCount + " instead of " + Dimension + "x" + Dimension + ".");
            if (result == null)
                result = transformed.GetNew();
            else if (result.RowCount != Dimension || result.ColumnCount!=Dimension)
                result = transformed.GetNew();
            TransformBackHessianPlain(transformed, result);
        }

        #endregion HessianTransformation


        #endregion Operation



    }  // class AffineTransformationSquare




}



