using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnCs.Lib
{


    /// <summary>Implementation of <see cref="INumberOperations{ElementType}"/> for type <see cref="Complex"/>.</summary>
    /// <inheritdoc/>
    public class NumberOperationsComplex : INumberOperations<Complex>
    {

        /// <inheritdoc/>
        public Complex AddNumbers(Complex a, Complex b)
        {
            return a + b;
        }

        /// <inheritdoc/>
        public Complex SubtractNumbers(Complex a, Complex b)
        {
            return (a - b);
        }

        /// <inheritdoc/>
        public Complex MultiplyNumbers(Complex a, Complex b)
        {
            return a * b;
        }

        /// <inheritdoc/>
        public Complex DivideNumbers(Complex a, Complex b)
        {
            return a / b;
        }

        /// <inheritdoc/>
        public Complex GetModulusNumber(Complex a, Complex b)
        {
            return 0;
        }

        /// <inheritdoc/>
        public bool AreEqual(Complex a, Complex b)
        {
            return (a == b);
        }

        /// <inheritdoc/>
        public Complex NegativeNumber(Complex a)
        {
            return -a;
        }

        /// <inheritdoc/>
        public Complex ConjugateNumber(Complex a)
        {
            return a.Conjugate;
        }

        /// <inheritdoc/>
        public Complex Absolute(Complex a)
        {
            return a.AbsoluteValue; // double is implicitly converted to Complex
        }

        /// <inheritdoc/>
        public Complex AbsoluteSquare(Complex a)
        {
            return a * a;
        }

        /// <inheritdoc/>
        public Complex Sqrt(Complex a)
        {
            return Complex.Pow(a, 1, 2);
        }

        /// <inheritdoc/>
        public Complex Root(Complex a, int n)
        {
            return Complex.Pow(a, 1, n);
        }

        /// <inheritdoc/>
        public Complex Pow(Complex a, int n)
        {
            return Complex.Pow(a, n);
        }

        /// <summary>Gets a global instance of <see cref="NumberOperationsComplex"/> that can be used anywhere
        /// it is needed. Because the class is thread safe, this same instance can be used in any number of places.</summary>
        public static NumberOperationsComplex Instance { get; } = new NumberOperationsComplex();

    }


}
