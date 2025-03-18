using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace LearnCs.Lib
{
    /// <summary>Represents a complex number (https://en.wikipedia.org/wiki/Complex_number).</summary>
    public struct Complex
    {

        #region Constructors

        /// <summary>Creates a complex number from the specified real and imaginary part.</summary>
        /// <param name="realPart">Real part of the complex number.</param>
        /// <param name="imaginaryPart">Imaginary part of the complex number.</param>
        public Complex(double realPart, double imaginaryPart)
        {
            Re = realPart;
            Im = imaginaryPart;
        }


        ///// <summary>Creates a default complex number, which is 0 + 0 i.</summary>
        //public Complex() : this(0.0, 0.0)  // call another constructor from this class with 2 parameters to do the job
        //{
        //    // No need to set components, this is done by the default (parameter-less) constructor that is 
        //    // called from the current constructor (": this(0.0, 0.0)")
        //    //Re = 0.0;
        //    //Im = 0.0;
        //}

        /// <summary>Creates a complex number from a real number. Real numbers are subset of complex numbers that
        /// don't have imaginary part.</summary>
        /// <param name="realNumber">The real number from which the current complex number is constructed.</param>
        public Complex(double realNumber)
        {
            Re = realNumber;
            Im = 0.0;
        }

        /// <summary>Creates a complex number from a 2-value tuple of doubles. The first value represents the 
        /// real and the second value represents the imaginary part.</summary>
        /// <param name="complexTuple"></param>
        public Complex((double, double) complexTuple)
        {
            Re = complexTuple.Item1;
            Im = complexTuple.Item2;
        }

        /// <summary>Returns a complex number initialized by polar coordinates.</summary>
        /// <param name="r">The absolute number of the complex number, or its radius in polar coordinates.</param>
        /// <param name="phi">The argument (or angle) of the complex number in polar coordinates.</param>
        public static Complex FromPolarCoordinates(double r, double phi)
        {
            return new Complex(r * Math.Cos(phi), r * Math.Sin(phi));
        }

        #endregion Constructors


        #region TypeConversionOperators


        /// <summary>Implicit conversion from double to complex.</summary>
        /// <param name="d">Number of type double that is converted to complex.</param>
        public static implicit operator Complex(double d)
        {
            return new Complex(d, 0);
        }

        /// <summary>Implicit conversion from a tuple of two numbers of type double to a complex number.</summary>
        /// <param name="doubleTuple">Tuple of two double numbers that is converted to complex.</param>
        public static implicit operator Complex((double, double) doubleTuple)
        {
            return new Complex(doubleTuple.Item1, doubleTuple.Item2);
        }

        /// <summary>Explicit conversion of <see cref="double[]"/> to a complex number. Used in particular in 
        /// parameterized test methods that have collections of complex numbers as parameters.</summary>
        /// <param name="components"></param>
        public static explicit operator Complex(double[] components)
        {
            if (components == null || components.Length == 0)
            {
                return new Complex(0.0, 0.0);
            }
            if (components.Length > 2)
            {
                throw new InvalidCastException($"Array double[] to Complex conversion: array length {components.Length}, should be 0 to 2.");
            }
            if (components.Length == 2)
            {
                return new Complex(components[0], components[1]);
            }
            if (components.Length == 1)
            {
                return new Complex(components[0]);
            }
            throw new InvalidCastException($"Array double[] to Complex conversion: Unexpected situation, unknown case occurred."
                + $"\n  Check the implementation of class {nameof(Complex)}, operator Complex(double[]).");
        }

        /// <summary>Explicit conversoin from complex to double. Will need cast operator.
        /// <para>Conversion cannot be done in all cases (i.e., when imaginary part is nonzero, complex number 
        /// can not be represented by double)</para> </summary>
        /// <param name="c">Complex number that is converted to type double.</param>
        /// <exception cref="InvalidOperationException">When imaginary part is different than zero.</exception>
        public static explicit operator double(Complex c)
        {
            if (c.Im != 0)
            {
                throw new InvalidOperationException($"Complex number {c} cannot be converted to double because its imaginary part is not zero.");
            }
            return c.Re;
        }

        /// <summary>Explicit conversion from complex to (double, double). Will need a cast operator.
        /// <para>The first item of the tuple becomes the real part and the second item becomes the 
        /// imaginary part of <paramref name="c"/>.</para></summary>
        /// <param name="c">Complex number that is converted to a (double, double) tuple.</param>
        public static explicit operator (double, double)(Complex c)
        {
            return (c.Re, c.Im);
        }

        /// <summary>Explicit conversion from complex to double[]. Will need a cast operator.
        /// <para>Element 0 of the array becomes the real part and the element 1 becomes the 
        /// imaginary part of <paramref name="c"/>.</para></summary>
        /// <param name="c">Complex number that is converted to array of type double.</param>
        public static explicit operator double[](Complex c)
        {
            return new double[] { c.Re, c.Im };
        }

        #endregion TypeConversionOperators



        #region Properties

        // Basic data of the complex number consists of its real and imaginary parts.
        // These are represented by Re and Im, the only two independent properties of type Complex (all other properties depend on these two).

        /// <summary>Underlying field for the property <see cref="Re"/></summary>
        private double _re;

        /// <summary>Underlying field for the property <see cref="Im"/></summary>
        private double _im;

        /// <summary>Gets the real part of the complex number.
        /// <para>This property is read only because we want type Complex to be immutable (after assignment, we cannot modify content).</para></summary>
        /// <remarks>WARNING:
        /// <para>If Re is defined without the underlying field (field added implicitly by the compiler) then it is not serialized, 
        /// and serialization/deserialization will not work, even if it is equipped with the <see cref="JsonIncludeAttribute"/>.</para></remarks>
        public double Re { get { return _re; } set { _re = value; } }

        /// <summary>Gets the imaginary part of the current complex number.
        /// <para>This property is read only.</para></summary>
        /// <remarks>WARNING:
        /// <para>If Re is defined wighout the underlying field (field added implicitly by the compiler) then it is not serialized, 
        /// and serialization/deserialization will not work, even if it is equipped with the <see cref="JsonIncludeAttribute"/>.</para></remarks>
        public double Im { get { return _im; } set { _im = value; } }


        // WARNING:
        // Derived property must be marked with the [JsonIgnore] attribute, otherwise, serialization will result in 
        // infinite depth of the object.

        // R and Fi (absolute value (modulus, magnitude) and argument) are derived properties:

        /// <summary>Gets absolute value (or magnitude) of the current complex number.</summary>
        public double AbsoluteValue =>  //   // Expression body (=> notation) can be used for simple properties
            Math.Sqrt(Re * Re + Im * Im);


        /// <summary>Gets the modulus (or magnitude, or absolute value) of the current complex number. This is the
        /// radius of the polar form of a complex number.</summary>
        [JsonIgnore]
        public double R => Math.Sqrt(Re * Re + Im * Im);
        // Remark: expression boy was used to replace the following longer notation:
        //{
        //    get
        //    {
        //        return Math.Sqrt(Re * Re + Im * Im);
        //    }
        //}

        /// <summary>Gets the argument of the current complex number. This is the angle parameter of the polar coordinates 
        /// of a complex number.</summary>
        [JsonIgnore]
        public double Phi => (Math.Atan2(Im, Re));

        // We also introduce some aliases of R and Fi, because different users can be more comfortable by using
        // different names for these parameters:

        /// <summary>Alias for the property <see cref="R"/>.</summary>
        [JsonIgnore]
        public double Magnitude => R;

        /// <summary>Alias of the property <see cref="R"/>.</summary>
        [JsonIgnore]
        public double Modulus => R;

        /// <summary>Alias for the property <see cref="Phi"/>. Avoid using this alias.</summary>
        [JsonIgnore]
        public double Argument => Phi;

        /// <summary>Please do not use, this property might be deprecated in the future.
        /// <para>Alias for the property <see cref="Phi"/>.</para></summary>
        [JsonIgnore]
        public double Fi => Phi;

        /// <summary>Gets the negative value of the current complex number.</summary>
        [JsonIgnore]
        public Complex Negative => new Complex(-Re, -Im);

        /// <summary>Gets the complex conjugate of the current complex number.</summary>
        [JsonIgnore]
        public Complex Conjugate => new Complex(Re, -Im);


        // ToDo: create tests for the property below!

        /// <summary>True if the current complex number is a real number (i.e., it can be 
        /// converted to double, i.e., its imaginary part is 0). False otherwise.</summary>
        [JsonIgnore]
        public bool IsReal => Im == 0;


        // ToDo: create tests for the property below! Also test implicit cast to long.

        /// <summary>True if the current complex number is an integer number (it can be
        /// converted to long, i.e., its imaginary part is 0, its real par does not have the 
        /// decimal part but only the integer part, and it is within the range of long).</summary>
        [JsonIgnore]
        public bool IsInteger => IsReal && Math.Round(Re) == Re
            && Re >= long.MinValue && Re <= long.MaxValue;


        // ToDo: create tests for the property below!
        // Also cast implicit cast to long, for both succeeded and failed cases

        /// <summary>True if the current complex number is an integer number that fits into int (it 
        /// can be converted to int, i.e., its imaginary part is 0, its real par does not have the 
        /// decimal part but only the integer part, and it is within the range of int).</summary>
        [JsonIgnore]
        public bool IsInt => IsReal && Math.Round(Re) == Re
            && Re >= int.MinValue && Re <= int.MaxValue;


        // ToDo: create tests for the method below! Also test implicit cast to long.

        /// <summary>Returns true if the current complex number is approximately a real number (i.e., its imaginary
        /// part is less or equal to the specified <paramref name="tolerance"/>).</summary>
        /// <param name="tolerance">Tolerance, must be less than 0.05. The number is regarded approximately a real
        /// number if the absolute the absolute value of its imaginary part is less or equal to tolerance.</param>
        /// <returns>True if the current complex number is approximately a real number (meaning that its imaginary part 
        /// is negligible), within <paramref name="tolerance"/>.</returns>
        /// <exception cref="ArgumentException">When the specified <paramref name="tolerance"/> is negative or it is 
        /// not less than 0.05.</exception>
        public bool IsApproximatelyReal(double tolerance = 0.0)
        {
            if (tolerance > 0.05 || tolerance < 0)
            {
                throw new ArgumentException($"Tolerance {tolerance} is not valid in {nameof(IsInteger)}({nameof(tolerance)}); it must be non-negative and less than 0.05.", nameof(tolerance));
            }
            return Math.Abs(Im) <= tolerance;
        }

        // ToDo: create tests for the method below! Also test implicit cast to long.

        /// <summary>Returns true if the current complex number is approximately an integer (within <paramref name="tolerance"/>)
        /// and can be approximately converted to long.</summary>
        /// <param name="tolerance">Tolerance, must be less than 0.05. The number is regarded approximately an
        /// integer if the absolute difference between its real part <see cref="Re"/> and the nearest integer
        /// is less than tolerance, and if the absolute value of the imaginary part is less than tolerance.</param>
        /// <returns>True if the current complex number is approximately an integer, within <paramref name="tolerance"/>.</returns>
        /// <exception cref="ArgumentException">When the specified <paramref name="tolerance"/> is negative or it is 
        /// not less than 0.05.</exception>
        public bool IsApproximatelyInteger(double tolerance  = 0.0)
        {
            if (tolerance > 0.05 || tolerance < 0)
            {
                throw new ArgumentException($"Tolerance {tolerance} is not valid in {nameof(IsInteger)}({nameof(tolerance)
                    }); it must be non-negative and less than 0.05.", nameof(tolerance));
            }
            return Math.Abs(Math.Round(Re) - Re) <= tolerance  // real part is almost an integer, within tolerance
                && Math.Abs(Im) <= tolerance  // imaginary part almost zero
                && Re >= long.MinValue && Re <= long.MaxValue;
        }

        // ToDo: create tests for the method below! Also test implicit cast to long.

        /// <summary>Similar to <see cref="IsApproximatelyInteger(double)"/>, except that the real part <see cref="Re"/>
        /// must also be within the range of the type int.</summary>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool IsApproximatelyInt(double tolerance = 0.0)
        {
            if (tolerance > 0.05 || tolerance < 0)
            {
                throw new ArgumentException($"Tolerance {tolerance} is not valid in {nameof(IsInteger)}({nameof(tolerance)
                    }); it must be non-negative and less than 0.05.", nameof(tolerance));
            }
            return Math.Abs(Math.Round(Re) - Re) <= tolerance  // real part is almost an integer, within tolerance
                && Math.Abs(Im) <= tolerance  // imaginary part almost zero
                && Re >= int.MinValue && Re <= int.MaxValue;
        }


        // ToDo: add comments and create tests for the method below!

        public double ToReal(double tolerance = 0.0)
        {
            if (!IsApproximatelyReal(tolerance))
            {
                throw new ArgumentException($"The current complex number {this} cannot be converted to a real number within the tolerance of {tolerance}.");
            }
            return Math.Round(Re);
        }

        // ToDo: add comments and create tests for the method below!

        public double ToDouble(double tolerance = 0.0) => ToReal(tolerance);

        public long ToInteger(double tolerance = 0.0)
        {
            if (!IsApproximatelyInteger(tolerance))
            {
                throw new ArgumentException($"The current complex number {this} cannot be converted to an integer number within the tolerance of {tolerance}.");
            }
            return (long)Re;
        }

        public long ToLong(double tolerance = 0.0) => ToInteger(tolerance);

        // ToDo: add comments and create tests for the method below!

        public long ToInt(double tolerance = 0.0)
        {
            if (!IsApproximatelyInt(tolerance))
            {
                throw new ArgumentException($"The current complex number {this} cannot be converted to integer number of type int int within the tolerance of {tolerance}.");
            }
            return (int)Re;
        }


        #endregion Properties


        #region ComplexConstants

        // Complex constants are important complex numbers, such as complex 0 or imaginary unit.
        // They are implemented by readonly static properties.

        /// <summary>Gets the complex 0 (a constant).</summary>
        public static Complex Zero { get; } = (0, 0);

        /// <summary>Gets the complex 1 (a constant).</summary>
        public static Complex One { get; } = (1.0, 0);

        /// <summary>Gets the real-valued constant Pi (= <see cref="Math.PI"/>), the ratio of 
        /// circumference and diameter of a circle, as complex number.</summary>
        public static Complex PI { get; } = (Math.PI, 0);

        /// <summary>Gets the real-valued constant E (= <see cref="Math.E"/>), the basis of 
        /// natural logarithm, as complex number.</summary>
        public static Complex E { get; } = (Math.E, 0);

        /// <summary>Gets the imaginary unit i.</summary>
        public static Complex I { get; } = new Complex(0, 1);

        /// <summary>Double constant 0.0.
        /// <para>Note there is an implicit conversion from double to Complex.</para></summary>
        public const double ConstZero = 0;

        /// <summary>Double constant 1.0.
        /// <para>Note there is an implicit conversion from double to Complex.</para></summary>
        public const double ConstOne = 1;

        /// <summary>Double constant Pi, the ratio of circumference and diameter of the 
        /// circle (the same as <see cref="Math.PI"/>).
        /// <para>Note there is an implicit conversion from double to Complex.</para></summary>
        public const double ConstPI = Math.PI;

        /// <summary>Double constant E, the basis of natural logarithm (the same as <see cref="Math.E"/>).
        /// <para>Note there is an implicit conversion from double to Complex.</para></summary>
        public const double ConstE = Math.E;

        #endregion ComplexConstants


        #region ArithmeticOperationsStatic 

        /// <summary>Ads two complex numbers and returns the result.</summary>
        public static Complex Add(Complex c1, Complex c2)
        {
            return new Complex(c1.Re + c2.Re, c1.Im + c2.Im);// a + b = (x + y i) + (u + v i) = (x + u) + (y + v) i
        }

        /// <summary>Subtracts <paramref name="c2"/> from <paramref name="c1"/> and returns the result.</summary>
        public static Complex Subtract(Complex c1, Complex c2)
        {
            return new Complex(c1.Re - c2.Re, c1.Im - c2.Im); // a - b = (x + y i) - (u + v i) = (x - u) + (y - v) i
        }

        /// <summary>Multiplies <paramref name="c1"/> by <paramref name="c2"/> and returns the result.</summary>
        public static Complex Multiply(Complex c1, Complex c2) // i^2 = -1
        {
            // c1 * c2 = (a + b i) * (c + d i) = ac + ad i + b i c + bd -1 = ac - bd + (ad + bc) i
            //return new Complex(c1.Re + c1.Im, c2.Re * c2.Im);
            return new Complex(c1.Re * c2.Re - c1.Im * c2.Im, c1.Re * c2.Im + c1.Im * c2.Re);
            //throw new NotImplementedException();
        }

        /// <summary>Divides <paramref name="c1"/> by <paramref name="c2"/> and returns the result.</summary>
        public static Complex Divide(Complex c1, Complex c2)//!!!
        {
            // a / c = (u + v i ) / (x + y i) = ((u x + v y) + (v x - u y) i) / (x^2 + y^2)
            //return new Complex(c1.Re + c1.Im, c2.Re + c2.Im);
            double denominator = c2.Re * c2.Re + c2.Im * c2.Im;
            return new Complex((c1.Re * c2.Re + c1.Im * c2.Im) / denominator, (c1.Im * c2.Re - c1.Re * c2.Im) / denominator);
        }

        /// <summary>Returns positive value of <paramref name="c"/>.</summary>
        public static Complex GetPositive(Complex c)
        {
            return c;
        }

        /// <summary>Returns negative value of <paramref name="c"/>.</summary>
        public static Complex GetNegative(Complex c)
        {
            return new Complex(-c.Re, -c.Im);
        }

        /// <summary>Returns complex conjugate of <paramref name="c"/>.</summary>
        public static Complex GetConjugate(Complex c)
        {
            return new Complex(c.Re, -c.Im); // c = x + i y => conjugate(c) = x - i y
        }

        /// <summary>Returns true or false of <paramref name= "a, b"/>.<summary> //!!!
        public static bool AreEqual(Complex a, Complex b)
        {
            return a.Re == b.Re && a.Im == b.Im;
        }

        ///<summary>Returns true or false of <paramref name="a, b"/>.<summary>
        public static bool AreNotEqual(Complex a, Complex b)
        {
            return a.Re != b.Re || a.Im != b.Im;
        }

        #endregion ArithmeticOperationsStatic


        #region ArithmeticOperationsInstanceMethods 

        /// <summary>Ads <paramref name="c"/> to the current complex number and returns the result.</summary>
        public Complex Add(Complex c)
        {
            // Remark: one can use "this" to refer to the current complex number, similar as in classes.
            return Complex.Add(this, c);
        }

        /// <summary>Subtracts <paramref name="c"/> from the curret complex number and returns the result.</summary>
        public Complex Subtract(Complex c)
        {
            return Complex.Subtract(this, c);
            //throw new NotImplementedException();
        }

        /// <summary>Multiplies the current complex number by <paramref name="c"/> and returns the result.</summary>
        public Complex Multiply(Complex c)
        {
            return Complex.Multiply(this, c);
        }

        /// <summary>Divides the current complex number by <paramref name="c"/> and returns the result.</summary>
        public Complex Divide(Complex c)
        {
            return Divide(this, c);
        }

        /// <summary>Returns (positive) value of the current complex number.</summary>
        public Complex GetPositive()
        {
            return GetPositive(this);
        }

        /// <summary>Returns negative value of the current complex number.</summary>
        public Complex GetNegative()
        {
            return GetNegative(this);
        }

        /// <summary>Returns negative value of the current complex number.</summary>
        public Complex GetConjugate()
        {
            return GetConjugate(this);
        }


        /// <summary>Returns boolean value, true or false, from comparison equality between two complex numbers.</summary> // !!!
        public Complex AreEqual()
        {
            return AreEqual();
        }


        /// <summary>Returns boolean value, true or false, from comparison inequality between two complex numbers.</summary> //!!!
        public Complex AreNotEqual()
        {
            return AreNotEqual();
        }

        #endregion ArithmeticOperationsInstanceMethods


        #region ArithmeticAndLogicOperatorsOverloaded


        /// <summary>Defines the binary operator + for summation of two complex numbers.</summary>
        public static Complex operator +(Complex c1, Complex c2)
        {
            // We use already defined static method for summation of complex numbers:
            //return Add(a, b);
            // return (Complex)(c1.Re + c2.Re, c1.Im + c2.Im); // cast in case under conversion type operator is EXPLICIT
            // (public static explicit operator Complex((double, double) doubleTuple))
            return new Complex(c1.Re + c2.Re, c1.Im + c2.Im);
        }

        /// <summary>Defines the binary operator - for subtraction of two complex numbers.</summary>
        public static Complex operator -(Complex c1, Complex c2)
        {
            //return Subtract(a, b);
            return new Complex(c1.Re - c2.Re, c1.Im - c2.Im);

            //throw new NotImplementedException();
        }

        /// <summary>Complex multiplication.</summary>
        public static Complex operator *(Complex c1, Complex c2)
        {
            //return Multiply(a, b);
            return new Complex(c1.Re * c2.Re - c1.Im * c2.Im, c1.Re * c2.Im + c1.Im * c2.Re);
        }

        /// <summary>Complex division (quotient of thow complex numbers).</summary>
        public static Complex operator /(Complex c1, Complex c2)
        {
            //return Divide(a,b);
            double denominator = c2.Re * c2.Re + c2.Im * c2.Im;
            return new Complex((c1.Re * c2.Re + c1.Im * c2.Im) / denominator, (c1.Im * c2.Re - c1.Re * c2.Im) / denominator);
        }

        /// <summary>Defines the unary + operator (just returns the same complex number).</summary>
        public static Complex operator +(Complex a)
        {
            return new Complex(a.Re, a.Im);
        }

        /// <summary>Defines the unary - operator for changing sign of a complex number.</summary>
        public static Complex operator -(Complex a)
        {
            return new Complex(-a.Re, -a.Im);
        }

        /// <summary>Complex comparison.</summary>
        public static bool operator ==(Complex a, Complex b)
        {
            return a.Re == b.Re && a.Im == b.Im;
        }

        /// <summary>Complex comparison.</summary>
        public static bool operator !=(Complex a, Complex b)
        {
            return a.Re != b.Re || a.Im != b.Im;
        }

        #endregion ArithmeticAndLogicOperatorsOverloaded


        #region ElementaryFunctionsStatic

        /// <summary>returns exponential function (where exponent is the Euler's number, base of natural logarithm).
        /// Defined for all complex numbers.</summary>
        public static Complex Exp(Complex x)
        {
            double factor = Math.Exp(x.Re);
            return new Complex(factor * Math.Cos(x.Im), factor * Math.Sin(x.Im));
        }

        /// <summary>Returns natural (base e) logarithm of the complex number <paramref name="x"/>.
        /// <para>Natural logarithm is a multivalued function. Principal branch is taken here.</para></summary>
        public static Complex Ln(Complex x)
        {
            return new Complex(Math.Log(x.AbsoluteValue), x.Argument);
        }

        // ToDo: create tests for Log!
        /// <summary>Returns logarithm base 10 of the complex number <paramref name="x"/>.
        /// <para>Note: logarithm is a multivalued function. Principal branch is taken here.</para></summary>
        public static Complex Log(Complex x)
        {
            return Ln(x) / Ln(10.0);
        }


        // ToDo: create tests for Log2!
        /// <summary>Returns logarithm base 2 of the complex number <paramref name="x"/>.
        /// <para>Note: logarithm is a multivalued function. Principal branch is taken here.</para></summary>
        public static Complex Log2(Complex x)
        {
            return Ln(x) / Ln(2.0);
        }


        public static Complex Pow(Complex x, int power)
        {
            double r = Math.Pow(x.R, power);
            double phi = x.Phi * power;
            return FromPolarCoordinates(r, phi);
        }

        public static Complex Pow(Complex x, int power, int rootIndex)
        {
            double r = Math.Pow(x.R, (double)power / (double)rootIndex);
            return FromPolarCoordinates(r, x.Phi * (double)power / (double)rootIndex);
        }

        public static Complex Pow(Complex x, Complex power)
        {
            return Exp(power * Ln(x));
        }

        /// <summary>Returns the root with index <see cref="n"/> of a complex number <paramref name="x"/>.</summary>
        /// <param name="x">Complex number whose n-th root is returned.</param>
        /// <param name="n">Root index (e.g. 2 for square root, 3 for cubic root, etc.).</param>
        /// <returns></returns>
        public static Complex Root(Complex x, int n)
        {
            return FromPolarCoordinates(Math.Pow(x.R, (double)1 / n), x.Phi / n);
        }

        /// <summary>Returns square root of <paramref name="x"/>.</summary>
        public static Complex Sqrt(Complex x)
        {
            return FromPolarCoordinates(Math.Sqrt(x.R), x.Phi / 2);
        }

        // ToDo: Create tests for CubicRoot()!
        /// <summary>Returns cubic root of <paramref name="x"/>.</summary>
        public static Complex CubicRoot(Complex x)
        {
            return Root(x, 3);
        }

        public static Complex Sin(Complex x)
        {
            return new Complex(
                Math.Sin(x.Re) * 0.5 * (Math.Exp(x.Im) + Math.Exp(-x.Im)),
                Math.Cos(x.Re) * 0.5 * (Math.Exp(x.Im) - Math.Exp(-x.Im))
                );
        }

        public static Complex Cos(Complex x)
        {
            return new Complex(
                +Math.Cos(x.Re) * 0.5 * (Math.Exp(x.Im) + Math.Exp(-x.Im)),
                -Math.Sin(x.Re) * 0.5 * (Math.Exp(x.Im) - Math.Exp(-x.Im))
                );
        }

        public static Complex Tan(Complex x)
        {
            return Sin(x) / Cos(x);
        }

        public static Complex Cot(Complex x)
        {
            return Cos(x) / Sin(x);
        }

        public static Complex Sinh(Complex x)
        {
            return 0.5 * (Exp(x) - Exp(-x));
        }

        public static Complex Cosh(Complex x)
        {
            return 0.5 * (Exp(x) + Exp(-x));
        }

        public static Complex Tanh(Complex x)
        {
            return Sinh(x) / Cosh(x);
        }

        public static Complex Coth(Complex x)
        {
            return Cosh(x) / Sinh(x);
        }

        #endregion ElementaryFunctionsStatiic



        #region ElementaryFunctionsInstance


        public Complex Exp()
        {
            return Complex.Exp(this);
            //double factor = Math.Exp(Re);
            //return new Complex(factor * Math.Cos(Im), factor * Math.Sin(Im));
        }

        public Complex Ln()
        {
            return Complex.Ln(this);
            //return new Complex(Math.Log(AbsoluteValue), Argument);
        }

        //public Complex Log()
        //{
        //    return Complex.Log(this);
        //}


        public Complex Pow(int power)
        {
            return Pow(this, power);
            //double r = Math.Pow(R, power);
            //double phi = Phi * power;
            //return FromPolarCoordinates(r, phi);
            //throw new NotImplementedException();
        }

        public Complex Pow(int n, int rootIndex)
        {
            return Complex.Pow(this, n, rootIndex);
        }

        public Complex Pow(Complex z)
        {
            return Complex.Pow(this, z);
        }

        public Complex Sin()
        {
            return Complex.Sin(this);
        }

        public Complex Cos()
        {
            // ToDo: implement this method and take care of unit tests
            throw new NotImplementedException();
        }

        public Complex Tan()
        {
            // ToDo: implement this method and take care of unit tests
            throw new NotImplementedException();
        }

        public Complex Cot()
        {
            // ToDo: implement this method and take care of unit tests
            throw new NotImplementedException();
        }

        public Complex Atan()
        {
            // ToDo: implement this method and take care of unit tests
            throw new NotImplementedException();
        }

        public Complex Sinh()
        {
            // ToDo: implement this method and take care of unit tests
            throw new NotImplementedException();
        }

        public Complex Cosh()
        {
            // ToDo: implement this method and take care of unit tests
            throw new NotImplementedException();
        }

        public Complex Tanh()
        {
            // ToDo: implement this method and take care of unit tests
            throw new NotImplementedException();
        }

        public Complex Coth()
        {
            // ToDo: implement this method and take care of unit tests
            throw new NotImplementedException();
        }

        #endregion ElementaryFunctionsInstance



        #region HashCodeAndEqualsOverride
        // This contains overrides of the Equals() and GetHashCode() methods.

        /// <summary>Overrides object.equals method. Returns true if <paramref name="obj"/> is also
        /// Complex and it has the same value, otherwise it returns false.</summary>
        /// <param name="obj">Object that is compared with the current complex number for equality.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Complex))
            {
                return false;
            }
            Complex compared = (Complex)obj;
            return Re == compared.Re && Im == compared.Im; ;
        }

        /// <summary>Override of <see cref="object.GetHashCode()"/> suitable for complex numbers;</summary>
        /// <returns>An integer hash code of the current complex number, suitable for use in hash tables.</returns>
        public override int GetHashCode()
        {
            return IGLib.Base.HashCodeHelper.Combine(Re, Im);
        }

        /// <summary>A candidate for <see cref="object.GetHashCode()"/> override. See also:
        /// <para>https://stackoverflow.com/questions/1646807/quick-and-simple-hash-code-combinations</para>
        /// <para>https://stackoverflow.com/questions/638761/gethashcode-override-of-object-containing-generic-array</para>
        /// <para>https://stackoverflow.com/questions/299304/why-does-javas-hashcode-in-string-use-31-as-a-multiplier</para></summary>
        /// <returns></returns>
        private int Get_HashCode1()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Re.GetHashCode();
                hash = hash * 31 + Im.GetHashCode();
                return hash;
            }
        }

        /// <summary>A candidate for <see cref="object.GetHashCode()"/> override.
        /// <para>See: https://stackoverflow.com/questions/1646807/quick-and-simple-hash-code-combinations</para></summary>
        private int Get_HashCode2()
        {
            return IGLib.Base.HashCodeHelper.Combine(Re, Im); // Use framework library, allows up to 8 parameters
        }

        /// <summary>A candidate for <see cref="object.GetHashCode()"/> override.
        /// <para>See: https://stackoverflow.com/questions/1646807/quick-and-simple-hash-code-combinations</para></summary>
        private int Get_HashCode3()
        {
            //var hash = new HashCode();
            //hash.Add(this.Re);
            //hash.Add(this.Im);
            //return hash.ToHashCode();
            return IGLib.Base.HashCodeHelper.Combine(this.Re, this.Im);
        }

        /// <summary>A candidate for <see cref="object.GetHashCode()"/> override. Leverages framework's implementation
        /// for tuples.</summary>
        private int Get_HashCode5()
        {
            (double, double) tuple = (Re, Im);
            return tuple.GetHashCode();
        }

        #endregion HashCodeAndEqualsOverride


        #region StringConversions

        /// <summary>Returns a readable string representation of the current complex number in the form
        /// (Re + Im i).
        /// <para>Examples:</para>
        /// <para>(0 + 0 i), (5 + 3 i), (1.3 + 4.2 i), (-23.45e-6 + 1.1e-5 i)</para></summary>
        /// <returns></returns>
        public string ToStringReadable()
        {
            // return $"({Re} {(Im >= 0? "+ ": "")} {Im} i)";
            if (Im < 0)
            {
                // In case of negative imaginary part, we change separating + into separating - and output negative imaginary part to string.
                // This has the same meaning but avoids writing the number as (3.2 + -8.1) and write it as (3.2 - 8.1).
                return $"({Re} - {-Im} i)";
            }
            else
            {
                return $"({Re} + {Im} i)";
            }
        }

        ///<summary>Returns a canonical string representation of the current complex number in the form
        /// (Re, Im).
        /// <para>Examples:</para>
        /// <para>(0, 0), (5, 3), (1.3, 4.2), (-23.45e-6, 1.1e-5)</para></summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({Re}, {Im})";
        }

        /// <summary>Returns a short form string representation of a complex number.</summary>
        public string ToStringShort()
        {
            if (Im == 0)
            {
                return $"{Re}";
            }
            if (Re == 0)
            {
                return $"({Im} i)";
            }
            return $"({Re}+{Im}i)";
        }

        // To consider: would we try to allow the following notation?
        // <para>String representation must be in form "(Re + Im i)", e.g.:</para>
        // <para>"(2.5 + 4.21 i)", "(12.5 - 1.22 i)", "(5.4 + -2.5 i)", "(3.3)", "(12.4 i)" </para>
        // <para>Whitespace characters don't matter. The following forms are also legitimate:</para>
        // <para>"  ( 2.5 + 4.21 i ) ", "(12.5-1.22i)", "(5.4+-2.5 i)", "  (  3.3 )", "(12.4i)"</para></summary>

        /// <summary>Parses complex number from its string representation.
        /// <para>String representaition must be in form "(Re + Im)", e.g.:</para>
        /// <para>"(2.5 , 4.21)", "(12.5 , -1.22)", "(5.4 , -2.5)", "(3, 35)", "(12, -4.56)" </para>
        /// <para>Whitespace characters don't matter. The following forms are also legitimate:</para>
        /// <para>"  ( 2.5 , 4.21  ) ", "(12.5, -1.22)", "(5.4, -2.5 )", "  (  3.3, 88 )", "(12, 45 )"</para></summary>
        /// <param name="s">String to be parsed.</param>
        /// <param name="result">The variable where the result will be stored. (0 , 0 ) is stored if not successful.</param>
        /// <returns>True if parsing was successful and meaningful value is writen to <paramref name="result"/>,
        /// false if not (in which case (0 , 0 ) is written to <paramref name="result"/>).</returns>
        public static bool TryParse(string s, out Complex result)
        {
            try
            {
                string sl = s;
                sl = sl.Trim();
                sl = sl.Trim(new char[] { '(', ')' });
                sl = sl.Trim();
                string[] sTab = sl.Split(',');

                if (sTab == null)
                {
                    throw new FormatException($"Invalid string representing complex number: could not extract two numbers (string.Split resulted in null).");
                }
                if (sTab.Length != 2)
                {
                    throw new FormatException($"Invalid string represeting complex number: could not extract two numbers (string.Split resulted in {sTab.Length} strings instead of 2).");
                }

                string s1 = sTab[0];
                string s2 = sTab[1];

                s1 = s1.Trim();
                s2 = s2.Trim();

                double re = double.Parse(s1);
                double im = double.Parse(s2);
                result = new Complex(re, im);
                return true;
            }
            catch (Exception e)
            {
                // handle exception:
                //throw; 
                Console.WriteLine(e.ToString());
            }
            result = 0; // works because implicit conversion from number types (from double) to complex exsists (converted to (0, 0)).
            return false;

            //throw new NotImplementedException();
        }

        /// <summary>Parses complex number from its string representation.
        /// <para>String representation of various forms are allowed and whitespaces are ignored.</para>
        /// <para>Basic representations (Re and Im are for real and imaginary values and can be replaced 
        /// by any number representation understood by <see cref="double.Parse(string)"/>; minimal spaces 
        /// are included except where space contributes to readability):</para>
        /// <para>  "Re" - just the real part, no brackets around it (parsable by <see cref="double.Parse(string)"/>), e.g. " -2.3e6 "</para>
        /// <para>  "(Re)" - just the real part with brackets around it, e.g. " ( -2.3e6 ) "</para>
        /// <para>  "Im i" - just the imaginary part followed by "i", no brackets around it, e.g. "-2.3e6i",  " -2.3e6 i ",  "-2.3e6i "</para>
        /// <para>  "Im*i" - just the imaginary part followed by "*i", no brackets around it (parsable by <see cref="double.Parse(string)"/>), e.g. "-2.3e6 i"</para>
        /// <para>    Remark: cannot omit "i" when there is only the imaginary part.</para>
        /// <para>  "(Im i)" - just the imaginary part followed by "i" with brackets around it, e. g. " (-2.3e6i)", " (-2.3e6 i ) ",  "(-2.3e6i ) "</para>
        /// <para>    Remark: cannot omit "i" when there is only the imaginary part.</para>
        /// <para>  "(Im*i)" - just the imaginary part followed by "*i", inside brackets, e. g. " (-2.3e6*i)", " ( -2.3e6 * i ) ",  "( -2.3e6 *i) "</para>
        /// <para>  "Re, Im" - comma separated real and imaginary part, e.g. " -2.3e6, 7.48 ", "-2.3e6,7.48  "</para>
        /// <para>  "Re, Im i" - comma separated real part and imaginary part followed by "i", e.g. " -2.3e6, 7.48i ", "-2.3e6,7.48 i"</para>
        /// <para>  "Re, Im*i" - comma separated real part and imaginary part followed by "*i", e.g. " -2.3e6, 7.48*i ", "-2.3e6,7.48 * i"</para>
        /// <para>  "(Re, Im i)" - comma separated real and imaginary part, inside brackets, e.g. " ( -2.3e6, 7.48i) ", "(-2.3e6,7.48 i )"</para>
        /// <para>  "Re+Im i" - + separated real part and imaginary part followed by "i", e.g. " -2.3e6+ 7.48i ", " -2.3e6 + 7.48 i "</para>
        /// <para>  "Re+Im*i" - + separated real part and imaginary part followed by "*i", e.g. " -2.3e6+ 7.48*i ", " -2.3e6 +7.48 * i"</para>
        /// <para>  "Re-Im i" - - separated real part and imaginary part followed by "i", e.g. " -2.3e6- -7.48i ", " -2.3e6 --7.48 i ", " -2.3e6 - -7.48 i"</para>
        /// <para>  "Re-Im*i" - - separated real part and imaginary part followed by "*i", e.g. " -2.3e6- -7.48*i ", " -2.3e6 --7.48 *i ", " -2.3e6 - -7.48 * i"</para>
        /// <para>  "(Re+Im i)" - + separated real part and imaginary part followed by "i", inside brackets, e.g. " ( -2.3e6+ 7.48i ) ", "(-2.3e6 + 7.48 i)"</para>
        /// <para>  "(Re+Im*i)" - + separated real part and imaginary part followed by "*i", inside brackets, e.g. "(-2.3e6+7.48*i)", " ( -2.3e6 +7.48 * i )"</para>
        /// <para>  "(Re+Im i)" - - separated real part and imaginary part followed by "i", inside brackets, e.g. "( -2.3e6- -7.48i )", "(-2.3e6 --7.48 i)", " (-2.3e6 - -7.48 i)"</para>
        /// <para>  "(Re+Im*i)" - - separated real part and imaginary part followed by "*i", inside brackets, e.g. " ( -2.3e6- -7.48*i ) ", "(-2.3e6 --7.48 *i)", "( -2.3e6 - -7.48 * i)"</para>
        /// <para>Practical examples of different representations:</para>
        /// <para>Comma separated real and imaginary part, with or without round brackets, with or without 
        /// 'i' for imaginary unit (optionally following *'):</para>
        /// <para>"(Re, Im)": "(2.5 , 4.21)", "( 12.5,-1.22  )", "( 5.4,-2.5)", "(   3, 35 )", "(12e-5, -4.56e-10)" </para>
        /// <para>"Re, Im ": "2.5 , 4.21", " 12.5,-1.22  ", " 5.4,-2.5", "   3, 35 ", "12e-5, -4.56e-10" </para>
        /// <para>"(Re, Im i)": "(2.5 , 4.21 i)", "( 12.5,-1.22i  )", "( 5.4,-2.5 * i)", "(   3, 35 *i )", "(12e-5, -4.56e-10*i)" </para>
        /// <para>"Re, Im i": "2.5 , 4.21 i", " 12.5,-1.22 *i ", " 5.4,-2.5i", "   3, 35 * i", "12e-5, -4.56e-10*i" </para>
        /// <para>Purely real numbers:</para>
        /// <para>"  ( 2.5   ) ", "(12.5)", "(5.4)", "  (  3.3 )", "(12.3e4)", "( 0.1e-6 )"</para>
        /// <para>"   2.5    ", "12.5", "5.4", "    3.3 ", "12.3e4", " 0.1e-6 "</para>
        /// <para>Purely imaginary numbers:</para>
        /// <para>"  ( 2.5 i  ) ", "(12.5i)", "(5.4 * i)", "  (  3.3* i)", "(12.3e4*i)", "( 0.1e-6   *  i)"</para>
        /// <para>"   2.5  i  ", "12.5i", "5.4" * i"    3.3* i", "12.3e4*i", " 0.1e-6   *   i  "</para>
        /// </summary>
        /// <param name="s">String to be parsed.</param>
        /// <returns>If parsing was successful, the complex number represented by <paramref name="s"/>. Otherwise, 
        /// <see cref="InvalidOleVariantTypeException"/> is thrown.</returns>
        /// <exception cref="FormatException">When the string is not formatted according to rules and a complex
        /// number could not be successfully inferred from the intended string representation.</exception>
        public static Complex ParseRobust(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("");
            }
            // Get rid of all leading and trailing whitespace:
            s = s.Trim();
            // In thee trimmed string, get rid of leading and trailing brackets, if they appear in pair open/closed:
            if (s[0] == '(' && s[s.Length - 1] == ')')
            {
                s = s.Substring(1, s.Length - 2).Trim();
            }
            else if (s[0] == '(' || s[s.Length - 1] == ')')
            {
                // There is either a leading or a trailing round bracket, but not both in the correct order; this indicates an error:
                if (s[0] == '(')
                {
                    throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: The parsed string contains the open round bracket but no matching closed bracket.");
                }
                else
                {
                    throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: The parsed string contains the closed round bracket but no matching open bracket.");
                }
            }
            // Check for nested open/closed brackets:
            if (s[0] == '(' || s[s.Length - 1] == ')')
            {
                if (s[0] == '(' && s[s.Length - 1] == ')')
                {
                    throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: The parsed string contains nested round brackets, which is currently not allowed in representation of a complex number.");
                }
                if (s[0] == '(')
                {
                    throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: The parsed string contains a nested open round bracket but no matching closed bracket.");
                }
                else
                {
                    throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: The parsed string contains a nested closed round bracket but no matching open bracket.");
                }
            }
            string sOriginalTrimmed = s;
            // After trimming round brackets, trim eventual leading and trailing whitespace again:
            s = s.Trim();
            // After trimming round brackets, check that there are no nested round brackets:
            // By now, we should have either just one floating point number (possibly with i) or two floating point numbers separated with comma,
            // and there should be no trailing spaces or round brackets:

            if (!s.Contains(','))
            {
                try
                {
                    // s does not contain comma, meaning that the number either has only a real or only an imaginary part.
                    // Try with the possibility of only the real part first:
                    double real;
                    bool success1 = double.TryParse(s, out real);
                    if (success1)
                    {
                        return new Complex(real, 0);
                    }
                    // If it did not work, try with the posibility of only the imaginary part, which has 'i' in it (and possibly '*' on the left):
                    if (s[s.Length - 1] == 'i')
                    {
                        // pure imaginary part needs to contain 'i' at the end.
                        s = s.Substring(0, s.Length - 1);
                        // After removal of the trailing 'i', trim whitespaces again:
                        s = s.TrimEnd();
                        // Optionally, the string can also contain '*' before 'i':
                        if (s[s.Length - 1] == '*')
                        {
                            // remove the '*' and trim whitespace again:
                            s = s.Substring(0, s.Length - 1);
                            s.TrimEnd();
                        }
                        // By now only the number representing teh imaginary part should remain:
                        double imaginary;
                        success1 = double.TryParse(s, out imaginary);
                        if (success1)
                        {
                            return new Complex(0, imaginary);
                        }
                        throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: The parsed string does not contain a comma (',') and contains 'i', but it does not appear to contain a well formed pure pure imaginary number..");
                    }
                    throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: The parsed string does not contain a comma (','), but it does not appear to contain neither a well formed pure real nor pure imaginary number.");
                }
                catch
                {
                    // s does not contain a pure real or pure imaginary number.
                    // There was no comma in the original s, but we would still like to allow for the possibility that '+' or '-'
                    // is used between real and imaginary part. What we do is find all appearances of '+' and '-' and try which is
                    // the largest substring starting at the beginnning of s, which can be parsed into double. This is the real part
                    // and the rest shoulld contain the imaginary part, which should necessarily end with "i" in this case:
                    s = sOriginalTrimmed;  // resttore the initial trimmed string
                    int nextSeparatorIndex = 0;
                    int identifiedSeparatorIndex = -1;
                    double realPart = 0;
                    double imaginaryPart = 0;
                    double parsed = 0;
                    string strRe = null;
                    string strIm = null;
                    bool changeSign = false;
                    do
                    {
                        nextSeparatorIndex = s.IndexOfAny(new char[] { '+', '-' }, nextSeparatorIndex);
                        if (nextSeparatorIndex >= 0)
                        {
                            // We have the next potential separator, either '+' or '-'. Verify whether this can actually be the separator,
                            // which means that the substring before it contains a number.
                            string substring = s.Substring(0, nextSeparatorIndex);
                            bool success2 = double.TryParse(substring, out parsed);
                            if (success2)
                            {
                                realPart = parsed;
                                strRe = substring;
                                changeSign = (s[nextSeparatorIndex] == '-');
                                identifiedSeparatorIndex = nextSeparatorIndex;
                            }
                            ++nextSeparatorIndex;
                        }

                    } while (nextSeparatorIndex >= 0);
                    if (identifiedSeparatorIndex < 0)
                    {
                        throw new InvalidAsynchronousStateException($"{nameof(Complex)}.{nameof(ParseRobust)}: Malfolmed string: contains no ',', does not represent pure real or imaginary number, and does not contain real and imaginary partt separated by '+' or '-'.");
                    }
                    if (identifiedSeparatorIndex > 0)
                    {
                        // Real part was identified and we have the separator ('+' or '-') that separates it from imaginary part;
                        // Try ti parse the imaginary part.
                        if (identifiedSeparatorIndex == s.Length - 1)
                        {
                            throw new InvalidAsynchronousStateException($"{nameof(Complex)}.{nameof(ParseRobust)}: Malformed string, no number follows the last '{s[identifiedSeparatorIndex]}' character.");
                        }
                        int startIndex = identifiedSeparatorIndex + 1;
                        strIm = s.Substring(identifiedSeparatorIndex + 1, s.Length - identifiedSeparatorIndex - 1);
                        strIm = strIm.Trim();
                        if (strIm[strIm.Length - 1] == 'i')
                        {
                            // pure imaginary part needs to contain 'i' at the end.
                            strIm = strIm.Substring(0, strIm.Length - 1);
                            // After removal of the trailing 'i', trim whitespaces again:
                            strIm = strIm.TrimEnd();
                            // Optionally, the string can also contain '*' before 'i':
                            if (strIm[strIm.Length - 1] == '*')
                            {
                                // remove the '*' and trim whitespace again:
                                strIm = strIm.Substring(0, strIm.Length - 1);
                                strIm.TrimEnd();
                            }
                            // By now only the pure number representing teh imaginary part should remain:
                            bool success3 = double.TryParse(strIm, out imaginaryPart);
                            if (success3)
                            {
                                if (changeSign)
                                { imaginaryPart = -imaginaryPart; }
                                return new Complex(realPart, imaginaryPart);
                            }
                        }
                        throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: Malformed string separated by '{s[identifiedSeparatorIndex]}', the imaginary part could not be retrieved.");
                    }
                    // Re-throw the original exception from the previous case where pure real or puree imaginary number was tried to be identified:
                    throw;
                }
            }  // string does not contain ','

            // String contains ',' as separator between the reeal and imaginary part.
            // Split the string and extract both parts:
            string[] parts = s.Split(',');
            if (parts.Length < 2)
            {
                throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: A string containing ',' as separator does not contain both the real and the imaginary part.");
            }
            if (parts.Length > 2)
            {
                throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: A string containing ',' as separator contains more than two parts.");
            }
            double re = 0, im = 0;
            bool success = double.TryParse(parts[0], out re);
            if (!success)
            {
                throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: Parsing of real part in a comma-separated string failed.");
            }
            string part1 = parts[1];
            part1 = part1.Trim();
            if (part1[part1.Length - 1] == 'i')
            {
                part1 = part1.TrimEnd('i');
                part1 = part1.TrimEnd();
                part1 = part1.TrimEnd('*');
                part1 = part1.TrimEnd();
            }
            success = double.TryParse(part1, out im);
            if (!success)
            {
                throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: Parsing of imaginary part in a comma-separated string failed.");
            }
            return new Complex(re, im);
            throw new FormatException($"{nameof(Complex)}.{nameof(ParseRobust)}: Malformed complex number, unkidentified error.");
        }

        /// <summary>Parses a complex number (of type <see cref="Complex"/>) from the string <paramref name="s"/>,
        /// stores the result in <paramref name="result"/>, and returns true if parsing was successful and false if not.
        /// This metthod calls <see cref="ParseRobust(string)"/> to do the job, therefore parsing is the same, just that
        /// this method does not throw an exception if parsing does not succeed, but rather returns a value indicating 
        /// success or failure. See documentation for <see cref="ParseRobust(string)"/> for formats that can be parsed.</summary>
        /// <param name="s">The string from which a complex number is parsed.</param>
        /// <param name="result">Variable where the parsed complex number is tored. 0 + 0 i is stored if parsig is not successful.</param>
        /// <returns>True if parsing is successful, false if not (i.i., the string does not contain a meaningful representation 
        /// of a complx number in a supported forms).</returns>
        public static bool TryParseRobust(string s, out Complex result)
        {
            try
            {
                result = ParseRobust(s);
                return true;
            }
            catch(Exception)
            {
                result = 0;
                return false;
            }
        }



        /// <summary>Parses complex number from its string representation and returns it. Exception is thrown if the
        /// value cannot be parsed from the string.</summary>
        /// <param name="s">String representation of the complex number from which the value will be parsed.</param>
        /// <returns>The complex number whose value has been parsed from the string.<paramref name="s"/>.</returns>
        /// <exception cref="FormatException">When the string <paramref name="s"/> does not contain a known representation
        /// of a complex number that could be parsed. Use <see cref="TryParse(string, out Complex)"/> to avoid thrown an 
        /// exception, and provide success information in the returned value instead.</exception>
        public static Complex Parse(string s)
        {
            Complex result;
            bool success = TryParse(s, out result);
            if (success)
            {
                return result;
            }
            throw new FormatException($"Cannot parse a complex number from string \"{s}\".");
        }

        #endregion StringConversions


    }  // end: class Complex

}

