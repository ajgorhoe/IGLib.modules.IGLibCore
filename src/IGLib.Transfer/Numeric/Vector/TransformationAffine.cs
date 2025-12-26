using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;


namespace IG.Num
{



    /// <summary>Implementation of a general affine transformation where dimensions of the 
    /// original and transformed space can be different.</summary>
    /// $A Igor Jul10;
    /// TODO: implement ICloneable!
    public class AffineTransformation // : IAffineTransformation
    {

        #region Construction


        /// <summary>Constructs affine transformation with the specified transformation matrix and translation vector.
        /// WARNING:  NOT IMPLEMENTED YET.
        /// </summary>
        /// <param name="transformationMatrix">Transformation matrix of the affine transformation.</param>
        /// <param name="translationVector">Translation vector of the affine transformation.</param>
        public AffineTransformation(Matrix transformationMatrix, Vector translationVector)
        {
            throw new NotImplementedException("General Affine transformations are not yet implemented.");
        }
        
        #endregion Construction


        #region Data




        #endregion Data


    }

}
