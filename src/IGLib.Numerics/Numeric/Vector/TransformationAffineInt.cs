using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{


    /*    WORKING PLAN FOR IMPLEMENTATION OF AFFINE TRANSFORMATIONS (WARNING: PERFORMANCE CRITICAL CODE!)
     * 
     * 1. General square affine transformations - THREAD UNSAFE
     * 2. Diagonal affine transformations - THREAD UNSAFE
     * 3. General square, THREAD SAFE
     * 4. Diagonal affine transformations - THREAD SAFE
     * 
     * 5. General Affine transformations - THREAD UNSAFE & SAFE
     * 
     * VIRTUAL FUNCTIONS only for creation of stuff (e.g. calculation of inverse transformation matrix, or direct transf. matrix from inverse) - because it's performance critical.
     * 
     * 
     * 
     */


    
    /// <summary>Affine Transformation.</summary>
    /// $A Igor Jul10;
    public interface IAffineTransformation
    {

        #region Data

        int DimensionOriginal
        { get; }

        int DimensionTransformed
        { get; }

        /// <summary>Get or set the transformation matrix.</summary>
        IMatrix TransformationMatrix
        {
            get;
            set;
        }

        /// <summary>Gets or sets inverse transformation matrix.</summary>
        IMatrix InverseTransformationMatrix
        {
            get;
            set;
        }

        /// <summary>Gets or sets the translation vector.</summary>
        IVector TranslationVector
        {
            get;
            set;
        }


        /// <summary>Copies transformation matrix to the specified storage matrix.</summary>
        /// <param name="store">Storage matrix that transformation matrix is copied to.</param>
        void CopyTransformationMatrix(ref IMatrix store);


        /// <summary>Copies inverse transformation matrix to the specified storage matrix.</summary>
        /// <param name="store">Storage matrix that inverse transformation matrix is copied to.</param>
        void CopyInverseTransformationMatrix(ref IMatrix store);


        #endregion Data


        #region Operation

        /// <summary>Transforms co-ordinates from the reference to the transformed space.
        /// WARNING: This is a plain version that does not check dimensions.</summary>
        /// <param name="original">Vector of original co-ordinates.</param>
        /// <param name="transformed">Vector where transformed co-ordinates are stored.</param>
        void TransformCoordinatesPlain(IVector original, IVector transformed);

        /// <summary>Transforms co-ordinates from the reference to the transformed space.</summary>
        /// <param name="original">Vector of original co-ordinates.</param>
        /// <param name="transformed">Vector where transformed co-ordinates are stored.</param>
        void TransformCoordinates(IVector original, ref IVector transformed);


        /// <summary>Backward transforms co-ordinates from the transformed to the reference space.</summary>
        /// <param name="transformed">Vector of transformed co-ordinates.</param>
        /// <param name="original">Vector where inverse-transformed original co-ordinates are stored.</param>
        void TransformBackCoordinatesPlain(IVector transformed, IVector original);

        /// <summary>Backward transforms co-ordinates from the transformed to the reference space.</summary>
        /// <param name="transformed">Vector of transformed co-ordinates.</param>
        /// <param name="original">Vector where inverse-transformed original co-ordinates are stored.</param>
        void TransformBackCoordinates(IVector transformed, ref IVector original);


        #region GradientTransformation

        /// <summary>Transforms gradient (or other covariant vector) from the reference to the transformed space.
        /// WARNING: This is a plain version that does not check dimensions.</summary>
        /// <param name="original">Original gradient-like vector.</param>
        /// <param name="result">Vector where resulting transformed vector is stored.</param>
        void TransformGradientPlain(IVector original, IVector result);

        /// <summary>Transforms gradient (or other covariant vector) from the reference to the transformed space.</summary>
        /// <param name="original">Original gradient-like vector.</param>
        /// <param name="result">Vector where resulting transformed vector is stored.</param>
        void TransformGradient(IVector original, ref IVector result);

        /// <summary>Backward transforms gradient (or other covariant vector) from the transformed to the reference space.</summary>
        /// <param name="transformed">Transformed gradient-like vector.</param>
        /// <param name="result">Vector where resulting backward transformed vector is stored.</param>
        void TransformBackGradientPlain(IVector transformed, IVector result);

        /// <summary>Backward transforms gradient (or other covariant vector) from the transformed to the reference space.</summary>
        /// <param name="transformed">Transformed gradient-like vector.</param>
        /// <param name="result">Vector where resulting backward transformed vector is stored.</param>
        void TransformBackGradient(IVector transformed, ref IVector result);

        #endregion GradientTransformatioon


        #region HessianTransformation

        /// <summary>Transforms Hessian (or other covariant matrix) from the reference to the transformed space.
        /// WARNING: This is a plain version that does not check dimensions.</summary>
        /// <param name="original">Original matrix.</param>
        /// <param name="result">Matrix where resulting transformed matrix is stored.</param>
        void TransformHessianPlain(IMatrix original, IMatrix result);

        /// <summary>Transforms Hessian (or other covariant matrix) from the reference to the transformed space.</summary>
        /// <param name="original">Original matrix.</param>
        /// <param name="result">Matrix where resulting transformed matrix is stored. Allocated/reallocated if necessary.</param>
        void TransformHessian(IMatrix original, ref IMatrix result);

        /// <summary>Backward transforms Hessian (or other covariant matrix) from the transformed to the reference space.</summary>
        /// <param name="transformed">Matrix in transformed coordinates.</param>
        /// <param name="result">Matrix where resulting backward-transformed matrix is stored.</param>
        void TransformBackHessianPlain(IMatrix transformed, IMatrix result);

        /// <summary>Backward transforms Hessian (or other covariant matrix) from the transformed to the reference space.</summary>
        /// <param name="transformed">Matrix in transformed coordinates.</param>
        /// <param name="result">Matrix where resulting backward-transformed matrix is stored. Allocated/reallocated if necessary.</param>
        void TransformBackHessian(IMatrix transformed, ref IMatrix result);
        #endregion HessianTransformation



        #endregion Operation

    }



}
