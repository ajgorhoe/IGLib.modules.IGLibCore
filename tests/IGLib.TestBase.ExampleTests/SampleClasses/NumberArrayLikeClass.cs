using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnCs.Lib
{


    /// <summary>Abstract base class for an array-like structures (potentially multidimensional) of number 
    /// elements (with element types being number types like double, float, int, Complex, etc.)</summary>
    /// <typeparam name="ElementType">Type of elements of the array-like constructs represented by this base class.</typeparam>
    public abstract class NumberArrayLikeClass<ElementType> : INumberArrayLikeClass<ElementType> where ElementType : struct
    {

        /// <summary>Object that performs numerical operations on elements of the object (such as summation, 
        /// multiplication, division, conjugation, calculation of absolute  or conjugate value, or taking negative value)</summary>
        /// <remarks>This property has type <see cref="INumberOperations{NumberType}"/>. Interface type enables injection of any 
        /// type that implements the interface.</remarks>
        protected abstract INumberOperations<ElementType> NumberOperations { get; }

#if false
        ///// <summary>Returns the rank - the number of dimensions of the potentially multidimensional array-like structure 
        ///// of numbers, which contains data for the current object.</summary>
        //public abstract int Rank { get; }

        ///// <summary>Gets a number that represents the number of elements along the specified dimension of the 
        ///// abstracted multidimensional array that contains data for the object defined by the current class.</summary>
        ///// <param name="dimension">The consecutive number of the dimension (starting at 0) along which the corresponding
        ///// number of elements is returned.</param>
        //public abstract int GetLength(int dimension);

        ///// <summary>Gets the total number of elements in all the dimensions of the
        ///// array that contains the number elements of the current array-like class.</summary>
        //public abstract int Length { get; }
#endif 

    }

}
