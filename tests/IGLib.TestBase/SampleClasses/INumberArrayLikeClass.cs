namespace LearnCs.Lib
{

    /// <summary>Interface for an array-like structures (potentially multidimensional) of number 
    /// elements (with element types being number types like double, float, int, Complex, etc.)</summary>
    /// <typeparam name="ElementType">Type of elements of the array-like constructs represented by this interface.</typeparam>

    public interface INumberArrayLikeClass<ElementType>
        where ElementType : struct
    {


#if false
        ///// <summary>Returns the rank - the number of dimensions of the potentially multidimensional array-like structure 
        ///// of numbers, which contains data for the current object.</summary>
        //int Rank { get; }

        ///// <summary>Gets a number that represents the number of elements along the specified dimension of the 
        ///// abstracted multidimensional array that contains data for the object defined by the current class.</summary>
        ///// <param name="dimension">The consecutive number of the dimension (starting at 0) along which the corresponding
        ///// number of elements is returned.</param>
        //int GetLength(int dimension);

        ///// <summary>Gets the total number of elements in all the dimensions of the
        ///// array that contains the number elements of the current array-like class.</summary>
        //int Length { get; }
#endif

    }
}