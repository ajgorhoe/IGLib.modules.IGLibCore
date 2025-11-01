using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnCs.Lib
{

    
    /// <summary>This interface provides methods for performing basic number operations (especially the arithmetic
    /// operations) for the specified number type (<typeparamref name="NumberType"/>).</summary>
    /// <typeparam name="NumberType">Type of numbers for which basic number operations are provided.</typeparam>
    public interface INumberOperations<NumberType>
        where NumberType : struct
    {

        /// <summary>Calculates and returns sum of parameters <paramref name="a"/> and <paramref name="b"/>.</summary>
        /// <param name="a">First sumand.</param>
        /// <param name="b">Second sumand.</param>
        /// <returns>The sum of <paramref name="a"/> and <paramref name="b"/>.</returns>
        NumberType AddNumbers(NumberType a, NumberType b);
        
        /// <summary>Calculates and returns difference of parameters <paramref name="a"/> and <paramref name="b"/>.</summary>
        /// <param name="a">First operand from which <paramref name="b"/> is subtracted.</param>
        /// <param name="b">Second operand that is subtracted from <paramref name="a"/>.</param>
        /// <returns>The difference of <paramref name="a"/> and <paramref name="b"/>.</returns>
        NumberType SubtractNumbers(NumberType a, NumberType b);
        
        /// <summary>Calculates and returns product of parameters <paramref name="a"/> and <paramref name="b"/>.</summary>
        /// <param name="a">First factor.</param>
        /// <param name="b">Second factor.</param>
        /// <returns>The product of <paramref name="a"/> and <paramref name="b"/>.</returns>
        NumberType MultiplyNumbers(NumberType a, NumberType b);
        
        /// <summary>Calculates and returns quotient of parameters <paramref name="a"/> and <paramref name="b"/>.</summary>
        /// <param name="a">Numerator (divident).</param>
        /// <param name="b">Denominator (divisor).</param>
        /// <returns>The quotient of <paramref name="a"/> and <paramref name="b"/>.</returns>
        NumberType DivideNumbers(NumberType a, NumberType b);
        
        /// <summary>Calculates and returns modulus of division of parameters <paramref name="a"/> and <paramref name="b"/>.
        /// This operation has an important meaning for integer types, while for floating-pont and complex types it
        /// always returns 0.</summary>
        /// <param name="a">Numerator (divident).</param>
        /// <param name="b">Denominator (divisor).</param>
        /// <returns>The modulus of division of <paramref name="a"/> by <paramref name="b"/>.</returns>
        NumberType GetModulusNumber(NumberType a, NumberType b);

        /// <summary>Returns if number <paramref name="a"/> is equal to number <see cref="b"/>, false otherwise.</summary>
        /// <param name="a">First number to be compared.</param>
        /// <param name="b">Second number to be compared.</param>
        /// <returns>True if <paramref name="a"/> and <paramref name="b"/> are equal, false otherwise.</returns>
        bool AreEqual(NumberType a, NumberType b);

        /// <summary>Returns negative value of parameter <paramref name="a"/>.</summary>
        /// <param name="a">Number whose negative value is returned.</param>
        /// <returns>Negative value of <paramref name="a"/>.</returns>
        NumberType NegativeNumber(NumberType a);

        /// <summary>Returns conjugate value of parameter <paramref name="a"/>. This operation has an important
        /// meaning for complex numbers (e.g., <typeparamref name="NumberType"/> = <see cref="Complex"/>), while
        /// for usual number types it returns the parameter itself (it is identity).</summary>
        /// <param name="a">Number conjugate value is returned.</param>
        /// <returns>Conjugate value of <paramref name="a"/>: its complex conjugate in case of complex numbers,
        /// and parameter itself for other number types like various integer or floating point numbers.</returns>
        NumberType ConjugateNumber(NumberType a);

        /// <summary>Returns magnitude, or absolute value, of <paramref name="a"/></summary>
        /// <param name="a">Number of which the absolute value (magnitude) is returned.</param>
        /// <returns>Absolute value (magnitude) of <paramref name="a"/>.</returns>
        NumberType Absolute(NumberType a);

        /// <summary>Returns square of the absolute value of <paramref name="a"/>.</param>
        /// <returns>Square of absolute value (magnitude) of <paramref name="a"/>.</returns>
        NumberType AbsoluteSquare(NumberType a);

        /// <summary>Returns square root of <paramref name="a"/>.</param>
        /// <returns>Square root of <paramref name="a"/>.</returns>
        NumberType Sqrt(NumberType a);

        /// <summary>Returns n-th root (root with index <paramref name="n"/>) of <paramref name="a"/>.</param>
        /// <returns>Root with index <paramref name="n"/> of <paramref name="a"/>.</returns>
        NumberType Root(NumberType a, int n);

        /// <summary>Returns <paramref name="a"/> raised to the integer power <paramref name="n"/>.</param>
        /// <returns><paramref name="a"/> raised to <paramref name="n"/>.</returns>
        NumberType Pow(NumberType a, int n);

    }

}
