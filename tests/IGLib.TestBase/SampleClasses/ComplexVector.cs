using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnCs.Lib
{


    /// <summary>Vector whose elements are of type double.
    /// <para>See: https://en.wikipedia.org/wiki/Euclidean_vector</para>
    /// <para>https://en.wikipedia.org/wiki/Vector_(mathematics_and_physics)</para></summary>
    public class ComplexVector : VectorBase<Complex, ComplexVector>, // Class ComplexVector is concrete non-generic class and sets
                                                                     // Complex and ComplexVector as concrete types in inherited generic class VectorBase<ElementType, VectorRetType>
        IVector<Complex>,   
        IVector<Complex, ComplexVector>
    {

        #region Constructors

        /// <summary>Only to support serialization / deserialization.</summary>
        public ComplexVector() : base()
        { }

        public ComplexVector(int dim) : base(dim)  // Constructor ComplexVector calls constructor on the base class.
        { }

        public ComplexVector(Complex[] elements, bool copyElements) : base(elements, copyElements)
        { }

        public ComplexVector(params Complex[] elements) : base(elements) 
        { }

        #endregion Constructors


        #region TypeConversions

        /// <summary>Converts array of double (<see cref="double[]"/>) to a complex vector (<see cref="ComplexVector"/>).
        /// <para>Each component of the array is converted to a complex number via implicit conversion from 
        /// <see cref="double"/> to <see cref="Complex"/>.</para></summary>
        /// <param name="array"></param>
        public static implicit operator ComplexVector(double[] array)
        {
            ComplexVector v = new ComplexVector(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                v[i] = array[i];  // make use of implicit conversion from (double, double) to Complex.
            }
            return v;
        }


        /// <summary>Converts array of 2-tuples of double (<see cref="(double, double)[]"/>) to a complex vector (<see cref="ComplexVector"/>).
        /// <para>Each component of the array is a 2-tuple <see cref="(double, double)"/>, which is converted to 
        /// a complex number via implicit conversion.</para></summary>
        /// <param name="array"></param>
        public static implicit operator ComplexVector((double, double)[] array)
        {
            ComplexVector v = new ComplexVector(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                v[i] = array[i];  // make use of implicit conversion from (double, double) to Complex.
            }
            return v;
        }

        /// <summary>Converts array of arrays of double (<see cref="double[][]"/>) to a complex vector (<see cref="ComplexVector"/>).
        /// <para>Each component of the outer one-dimensional array is again a 1D array, which is converted to 
        /// a complex number, thus it must have at most 2 elements (for real and imaginary part), but can have less or be null
        /// (see in <see cref="Complex"/> the operator Complex(double[]).</para></summary>
        /// <param name="array">Array of arrays , each of which represents a complex number, and should not have mroe than 2 elements.</param>
        public static implicit operator ComplexVector(double[][] array)
        {
            ComplexVector v = new ComplexVector(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                v[i] = (Complex) array[i];  // make use of explicit conversion from double[] to Complex.
            }
            return v;
        }

        #endregion TypeConversions


        #region NumberArrayStructureGeneric_Inherited

        /// <inheritdoc/>
        /// <summary>This property can now be implemented because the concrete type of elements is known.</summary>
        protected override INumberOperations<Complex> NumberOperations { get; } = NumberOperationsComplex.Instance;

        #region NumberOperationsOnElements

        // ToDo: check the situation in the new version and remove or update the comments.

        // These methods are inherited from NumberArrayStructureGeneric<ElementType> and are reused completely:

        // protected virtual ElementType Add(ElementType a, ElementType b)

        // protected virtual ElementType Subtract(ElementType a, ElementType b)

        // protected virtual ElementType Multiply(ElementType a, ElementType b)

        // protected virtual ElementType Divide(ElementType a, ElementType b)

        // protected virtual ElementType GetModulus(ElementType a, ElementType b)

        // protected virtual ElementType Negative(ElementType a)

        // protected virtual ElementType Conjugate(ElementType a)

        #endregion NumberOperationsOnElements

        #endregion NumberArrayStructureGeneric_Inherited

        #region VectorSpecific

        /// <inheritdoc/>
        public override ComplexVector CreateVector(int dim)
        {
            return new ComplexVector(dim);
        }

        #endregion VectorSpecific

    }

}
