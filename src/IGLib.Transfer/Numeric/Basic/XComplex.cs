// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

    // COMPLEX NUMBERS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace IG.Num
{

        // TODO: Implement parsing and formatting of complex numbers!


    ///// <summary>Extended complex struct.
    ///// This should not be used!!!</summary>
    ///// $A Igor Sept09;
    //[Obsolete("Do not use this struct!")]
    //public struct XComplex : ICloneable, IEquatable<XComplex>, IComparable<XComplex>
    //{

    //    /// <summary>MathNet.Numerics.Complex base of the Xcomplex struct.</summary>
    //    public MathNet.Numerics.Complex Base;


    //    #region Constructors_Constants

    //    #region Constructors

    //    /// <summary>Constructs an extended complex struct from <c>Complex</c>.</summary>
    //    /// <param name="s">The complex number used in construction.</param>
    //    public XComplex(MathNet.Numerics.Complex b)
    //    { Base = b; }

    //    /// <summary>Constructs an extended complex struct from real and imaginary part.</summary>
    //    /// <param name="real">Real part of the complex number.</param>
    //    /// <param name="imaginary">Imaginary part of the complex number.</param>
    //    public XComplex(double real, double imaginary)
    //    {
    //        Base = new MathNet.Numerics.Complex(real, imaginary);
    //    }

    //    /// <summary>Constructs an extended complex struct that is a real number (imaginary part equals 0).</summary>
    //    /// <param name="real">Real part of the complex number.</param>
    //    public XComplex(double real)
    //    {
    //        Base = new MathNet.Numerics.Complex(real, 0.0);
    //    }

    //    #endregion // Constructors

    //    #region Copy

    //    // Method that return a clone of the current complex number:

    //    public Object Clone()
    //    {
    //        return new XComplex(Real, Imaginary);
    //    }

    //    // Methods that return a clone of a specified complex number:

    //    /// <summary>Returns a clone of the specified complex number.</summary>
    //    /// <param name="x">Complex number whose copy is returned as <c>XComplex</c>.</param>
    //    public static XComplex Clone(XComplex x)
    //    {
    //        return new XComplex(x);
    //    }

    //    /// <summary>Returns a clone of the specified complex number.</summary>
    //    /// <param name="x">>Complex number whose copy is returned as <c>XComplex</c>.</param>
    //    public static XComplex Clone(MathNet.Numerics.Complex x)
    //    {
    //        return new XComplex(x);
    //    }

    //    // Methods that copy to the current complex number:

    //    /// <summary>Copies a complex number to the current complex number.</summary>
    //    /// <param name="x">Complex number whose copy is stored in the current <c>XComplex</c>.</param>
    //    public void Copy(XComplex x)
    //    {
    //        Base = x.Base;
    //    }

    //    /// <summary>Copies a complex number of type Complex to the current complex number o type XComplex.</summary>
    //    /// <param name="x">Complex number whose copy is stored in the current <c>XComplex</c>.</param>
    //    public void Copy(MathNet.Numerics.Complex x)
    //    {
    //        Base = x;
    //    }

    //    /// <summary>Copies a pair of numbers to the current complex number of type <c>XComplex</c>.</summary>
    //    /// <param name="real">New real part of the current <c>XComplex</c>.</param>
    //    /// <param name="imaginary">New imaginary part of the current <c>XComplex</c>.</param>
    //    public void Copy(double real, double imaginary)
    //    {
    //        Base = new MathNet.Numerics.Complex(real, imaginary);
    //    }

    //    /// <summary>Copies a real number to the current complex number of type <c>XComplex</c>.</summary>
    //    /// <param name="x">Real number whose copy is stored in the current <c>XComplex</c>.</param>
    //    public void Copy(double real)
    //    {
    //        Base = new MathNet.Numerics.Complex(real, 0.0);
    //    }

    //    // Static methods that copy to the specified complex number:

    //    /// <summary>Copier a complex number to <c>XComplex</c> and returns it.</summary>
    //    /// <param name="x">A complex number that is copied.</param>
    //    /// <returns>Complex number that is a copy of the argument.</returns>
    //    public static XComplex Copy(MathNet.Numerics.Complex x, ref XComplex res)
    //    {
    //        res.Base = x;
    //        return res;
    //    }

    //    /// <summary>Copies a complex number to <c>XComplex</c> and returns it.</summary>
    //    /// <param name="x">A complex number that is copied.</param>
    //    /// <returns>Complex number that is a copy of the argument.</returns>
    //    public static XComplex Copy(XComplex x, ref XComplex res)
    //    {
    //        res = x;
    //        return res;
    //    }

    //    /// <summary>Copies a complex number represented by real and imaginary part to <c>XComplex</c>.</summary>
    //    /// <param name="real">Real part of the copied complex number.</param>
    //    /// <param name="imaginary">Imaginary part of the copied complex number.</param>
    //    /// <returns>Complex number that is represented by the arguments.</returns>
    //    public static XComplex Copy(double real, double imaginary,ref XComplex res)
    //    {
    //        res = new XComplex(real, imaginary);
    //        return res;
    //    }

    //    /// <summary>Copies a complex number that has only a real part to <c>XComplex</c>.</summary>
    //    /// <param name="real">Real part of the copied complex number.</param>
    //    /// <returns>Complex number that is represented by the argument.</returns>
    //    public static XComplex Copy(double real,ref XComplex res)
    //    {
    //        res = new XComplex(real, 0.0);
    //        return res;
    //    }

    //    // Other ways of creating a complex number:

    //    /// <summary>Constructs a <c>Complex</c> from its real and imaginary parts.</summary>
    //    public static XComplex FromRealImaginary(double real, double imag)
    //    { return new XComplex(real, imag); }

    //    /// <summary>Constructs a <c>Complex</c> from its modulus and argument. </summary>
    //    /// <param name="modulus">Must be non-negative.</param>
    //    /// <param name="argument">Real number.</param>
    //    public static XComplex FromModulusArgument(double modulus, double argument )
    //    {
    //        return Clone(MathNet.Numerics.Complex.FromModulusArgument(modulus, argument));
    //    }

    //    #endregion Copy

    //    #region Constants


    //    /// <summary>Constructs a complex number with random real and imaginary value.</summary>
    //    /// <param name="realRandomDistribution">Continuous random distribution or source for the real part.</param>
    //    /// <param name="imagRandomDistribution">Continuous random distribution or source for the imaginary part.</param>
    //    public static XComplex Random(
    //            MathNet.Numerics.Distributions.IContinuousGenerator realRandomDistribution, 
    //            MathNet.Numerics.Distributions.IContinuousGenerator imagRandomDistribution)
    //    {
    //        return Clone(MathNet.Numerics.Complex.Random(realRandomDistribution,imagRandomDistribution));
    //    }

    //    /// <summary>Constructs a complex number with random real and imaginary value.</summary>
    //    /// <param name="randomDistribution">Continuous random distribution or source for the real and imaginary parts.</param>
    //    public static XComplex Random(MathNet.Numerics.Distributions.IContinuousGenerator randomDistribution)
    //    {
    //        return Clone(MathNet.Numerics.Complex.Random(randomDistribution)); 
    //    }

    //    /// <summary>Constructs a complex number with random modulus and argument.</summary>
    //    /// <param name="modulusRandomDistribution">Continuous random distribution or source for the modulus.</param>
    //    /// <param name="argumentRandomDistribution">Continuous random distribution or source for the argument.</param>
    //    public static XComplex RandomPolar(
    //            MathNet.Numerics.Distributions.IContinuousGenerator modulusRandomDistribution,
    //            MathNet.Numerics.Distributions.IContinuousGenerator argumentRandomDistribution)
    //    {
    //        return Clone(MathNet.Numerics.Complex.RandomPolar(modulusRandomDistribution,argumentRandomDistribution));
    //    }

    //    /// <summary>Constructs a complex number on the unit circle with random argument.</summary>
    //    /// <param name="argumentRandomDistribution">Continuous random distribution or source for the argument.</param>
    //    public static XComplex RandomUnitCircle(MathNet.Numerics.Distributions.IContinuousGenerator argumentRandomDistribution)
    //    {
    //        return Clone(MathNet.Numerics.Complex.RandomUnitCircle(argumentRandomDistribution));
    //    }


    //    /// <summary>Represents the complex zero value (a constant).</summary>
    //    public static XComplex Zero
    //    {
    //        get { return new XComplex(0d, 0d); }
    //    }

    //    /// <summary>Indicates whether the <c>XComplex</c> is zero.</summary>
    //    public bool IsZero
    //    {
    //        get { return Base.IsZero; }
    //    }

    //    /// <summary>Represents the complex multiplication unit (a constant).</summary>
    //    public static XComplex One
    //    {
    //        get { return new XComplex(1d, 0d); }
    //    }

    //    /// <summary>Indicates whether the <c>XComplex</c> is one.</summary>
    //    public bool IsOne
    //    {
    //        get { return Base.IsOne; }
    //    }

    //    /// <summary>Represents the imaginary unit number (a constant).</summary>
    //    public static XComplex I
    //    {
    //        get { return new XComplex(0d, 1d); }
    //    }

    //    /// <summary>Indicates whether the <c>XComplex</c> is the imaginary unit.</summary>
    //    public bool IsI
    //    {
    //        get { return Base.IsI; }
    //    }

    //    /// <summary>Represents a value that is not a number (a constant).</summary>
    //    public static XComplex NaN
    //    {
    //        get { return new XComplex(double.NaN, double.NaN); }
    //    }

    //    /// <summary>Indicates whether the provided <cX>Complex</c> evaluates to a value that is not a number.</summary>
    //    public bool IsNaN
    //    {
    //        get { return Base.IsNaN; }
    //    }

    //    /// <summary>Represents the infinity value (a constant).</summary>
    //    /// <remarks>
    //    /// The semantic associated to this value is a <c>Complex</c> of infinite real and imaginary part.</remarks>
    //    public static XComplex Infinity
    //    {
    //        get { return new XComplex(double.PositiveInfinity, double.PositiveInfinity); }
    //    }

    //    /// <summary>Indicates whether the provided <c>Complex</c> evaluates to an infinite value.</summary>
    //    /// <remarks>True if it either evaluates to a complex infinity or to a directed infinity.</remarks>
    //    public bool IsInfinity
    //    {
    //        get { return Base.IsInfinity; }
    //    }

    //    /// <summary>Indicates the provided <c>Complex</c> is real.</summary>
    //    public bool IsReal
    //    {
    //        get { return Base.IsReal; }
    //    }

    //    /// <summary>
    //    /// Indicates the provided <c>Complex</c> is real and not negative, that is >= 0.</summary>
    //    public bool IsRealNonNegative
    //    {
    //        get { return Base.IsRealNonNegative; }
    //    }

    //    /// <summary>Indicates the provided <c>Complex</c> is imaginary.</summary>
    //    public bool IsImaginary
    //    {
    //        get { return Base.IsImaginary; }
    //    }

    //    #endregion  // Constants

    //    #endregion  // Constructors_Constants

    //    /// <summary>Converts this <c>XComplex</c> to MathNes.Numerics.Complex.</summary>
    //    /// <returns></returns>
    //    public MathNet.Numerics.Complex ToComplex()
    //    {
    //        return Base;
    //    }


    //    #region Cartesian and Polar Components

    //    /// <summary>Gets or sets the rear part of this complex number.</summary>
    //    public double Real
    //    {
    //        get { return Base.Real; }
    //        set { Base.Real = value; }
    //    }
    //    /// <summary>Gets or sets the rear part of this complex number.</summary>
    //    public double Re
    //    {
    //        get { return Base.Real; }
    //        set { Base.Real = value; }
    //    }

    //    /// <summary>Gets or sets the imaginary part of this complex number.</summary>
    //    public double Imag
    //    {
    //        get { return Base.Imag; }
    //        set { Base.Imag = value; }
    //    }

    //    /// <summary>Gets or sets the imaginary part of this complex number.</summary>
    //    public double Imaginary
    //    {
    //        get { return Base.Imag; }
    //        set { Base.Imag = value; }
    //    }

    //    /// <summary>Gets or sets the imaginary part of this complex number.</summary>
    //    public double Im
    //    {
    //        get { return Base.Imag; }
    //        set { Base.Imag = value; }
    //    }


    //    /// <summary>Gets or sets the modulus of this Complex number.</summary>
    //    /// <exception cref="ArgumentOutOfRangeException">Thrown if an attempt is made to set a negative modulus.</exception>
    //    /// <remarks>
    //    /// If this Complex is zero when the modulus is set, the Complex is assumed to be positive real with an argument of zero.
    //    /// </remarks>
    //    /// <seealso cref="Argument"/>
    //    public double Modulus
    //    {
    //        get{ return Base.Modulus; }
    //        set { Base.Modulus = value; }
    //    }


    //    /// <summary>Gets or sets the squared modulus of this Complex number.</summary>
    //    /// <exception cref="ArgumentOutOfRangeException">Thrown if an attempt is made to set a negative modulus.</exception>
    //    /// <remarks>
    //    /// If this complex number is zero when the modulus is set, the number is assumed to be positive real with an argument of zero.
    //    /// </remarks>
    //    /// <seealso cref="Argument"/>
    //    public double ModulusSquared
    //    {
    //        get{ return Base.ModulusSquared; }
    //        set { Base.ModulusSquared = value; }
    //    }


    //    /// <summary>Gets or sets the argument of this complex number.</summary>
    //    /// <remarks>Argument always returns a value bigger than negative Pi and smaller or equal to Pi. 
    //    /// If this complex number is zero, the Complexis assumed to be positive real with an argument of zero.
    //    /// </remarks>
    //    public double Argument
    //    {
    //        get { return Base.Argument; }
    //        set { Base.Argument = value; }
    //    }


    //    /// <summary>Gets the unity of this complex (same argument, but on the unit circle; exp(I*arg))</summary>
    //    public XComplex Sign
    //    {
    //        get { return Clone(Base.Sign); }
    //    }

    //    /// <summary>Real part (protected) - just for similarity with <c>MathNet.Numerics.Complex</c>.</summary>
    //    private double real { get { return Base.Real; } set { Base.Real = value; } }
    //    /// <summary>Imaginary part (protected) - just for similarity with <c>MathNet.Numerics.Complex</c>.</summary>
    //    private double imag { get { return Base.Imag; } set { Base.Imag = value; } }

    //    #endregion // Cartesian and Polar Components


    //    /// <summary>Gets or sets the conjugate of this complex number.</summary>
    //    /// <remarks>a.Conjugate = s; is equivalent to a = s.Conjugate.</remarks>
    //    public XComplex Conjugate
    //    {
    //        get { return new XComplex(Re, -Im); }
    //        set { this = value.Conjugate; }
    //    }

    //    #region Formatting_Parsing

    //    public override string ToString()
    //    {
    //        return "{" + Re.ToString(null, CultureInfo.InvariantCulture)
    //            + ", " + Im.ToString(null, CultureInfo.InvariantCulture) +"}";
    //    }

    //    #endregion  // Formatting_Parsing


    //    #region Equality_Comparison_Hashing


    //    /// <summary>Compares two complex numbers by size.</summary>
    //    /// <param name="a"></param>
    //    /// <param name="s"></param>
    //    /// <returns></returns>
    //    public int CompareBySize(XComplex a, XComplex b)
    //    {
    //        double sa = a.Abs(), sb = b.Abs();
    //        if (sa < sb)
    //            return -1;
    //        else if (sa > sb)
    //            return 1;
    //        else if (sa == sb)
    //            return 0;
    //        else
    //            return 0;
    //    }

    //    /// <summary>Function for comparing two compex numbers 
    //    /// - throws an exception because complex numbers can not be ordered.</summary>
    //    public int Compare(XComplex a, XComplex b)
    //    {
    //        throw new NotImplementedException("Comparison is not implemented because complex numbers are not ordable.");
    //    }

    //    /// <summary>Function for comparing two compex numbers 
    //    /// - throws an exception because complex numbers can not be ordered.</summary>
    //    public int Compare(object a, object b)
    //    {
    //        throw new NotImplementedException("Comparison is not implemented because complex numbers are not ordable.");
    //    }


    //    /// <summary>Indicates whether <c>obj</c> is equal to this instance.</summary>
    //    public override bool Equals(object obj)
    //    {
    //        return (obj is XComplex) && this.Equals((XComplex) obj);
    //    }

    //    /// <summary>Indicates whether <c>a</c> is equal to this instance. </summary>
    //    public bool Equals(XComplex a)
    //    {
    //        return !IsNaN && !a.IsNaN && (Re == a.Re && Im == a.Im);
    //    }

    //    /// <summary>Gets the hashcode of this <c>Scalar</c>.</summary>
    //    public override int GetHashCode()
    //    {
    //        return Base.GetHashCode();
    //    }


    //    /// <summary>Compare this Scalar with another Scalar.</summary>
    //    /// <param name="s">The scalar to compare with.</param>
    //    public int CompareTo(XComplex other) { return Base.CompareTo(other.Base); }

    //    #endregion  // Equality_Comparison_Hashing


    //    #region Operators


    //    public static bool operator ==(XComplex a, XComplex b)
    //    {  return a.Base==b.Base;   }

    //    public static bool operator !=(XComplex a, XComplex b)
    //    {
    //        return a.Base!=b.Base;
    //    }

    //    public static XComplex operator +(XComplex a)
    //    {
    //        return a;
    //    }

    //    public static XComplex operator -(XComplex a)
    //    {
    //        return new XComplex(-a.real, -a.imag);
    //    }

    //    public static XComplex operator +(XComplex a, XComplex b)
    //    {
    //        return new XComplex(a.real + b.real, a.imag + b.imag);
    //    }

    //    public static XComplex operator -(XComplex a, XComplex b)
    //    {
    //        return new XComplex(a.real - b.real, a.imag - b.imag);
    //    }

    //    public static XComplex operator +(XComplex a, double b)
    //    {
    //        return new XComplex(a.real + b, a.imag);
    //    }

    //    public static XComplex operator -(XComplex a, double b)
    //    {
    //        return new XComplex(a.real - b, a.imag);
    //    }

    //    public static XComplex operator +(double a, XComplex b)
    //    {
    //        return new XComplex(b.real + a, b.imag);
    //    }

    //    public static XComplex operator -(double a, XComplex b)
    //    {
    //        return new XComplex(a - b.real, -b.imag);
    //    }

    //    public static XComplex operator *(XComplex a, XComplex b)
    //    {
    //        return Clone(a.Base * b.Base);
    //    }

    //    public static XComplex operator *(double a, XComplex b)
    //    {
    //        return new XComplex(b.real * a, b.imag * a);
    //    }

    //    public static XComplex operator *(XComplex a, double b)
    //    {
    //        return new XComplex(a.real * b, a.imag * b);
    //    }

    //    public static XComplex operator /(XComplex a, XComplex b)
    //    {
    //        return Clone(a.Base/b.Base);
    //    }

    //    public static XComplex operator /(double a, XComplex b)
    //    {
    //        return Clone(a/b.Base);
    //    }

    //    public static XComplex operator /(XComplex a, double b)
    //    {
    //        return Clone(a.Base/b);
    //    }

    //    /// <summary>Implicit conversion from a real double to a real <c>XComplex</c>.</summary>
    //    public static implicit operator XComplex(double number)
    //    {
    //        return new XComplex(number, 0d);
    //    }

    //    /// <summary>Implicit conversion of <c>Complex</c> to <c>XComplex</c>.</summary>
    //    public static implicit operator XComplex(MathNet.Numerics.Complex c)
    //    {
    //        return new XComplex(c);
    //    }

    //    /// <summary>Implicit conversion from <c>Complex</c> to <c>XComplex</c>.</summary>
    //    public static implicit operator MathNet.Numerics.Complex(XComplex c)
    //    {
    //        return c.Base;
    //    }

    //    #endregion  // Operators


    //    #region Functions

    //    #region Miscellaneous


    //    /// <summary>Returns the absolute value (or modulus) of the complex number.</summary>
    //    public double Abs()
    //    {
    //        return Math.Sqrt(Im*Im+Re*Re);
    //    }

    //    #endregion  // Mescellaneous


    //    #region Trigonometric Functions

    //    /// <summary>Trigonometric Sine f this <c>XComplex</c>.</summary>
    //    public XComplex Sine()
    //    {
    //        return Base.Sine();
    //    }

    //    /// <summary>Trigonometric Cosine of this <c>XComplex</c>.</summary>
    //    public XComplex Cosine()
    //    {
    //        return Base.Cosine();
    //    }

    //    /// <summary>Trigonometric Tangent of this <c>XComplex</c>.</summary>
    //    public XComplex Tangent()
    //    {
    //        return Base.Tangent();
    //    }

    //    /// <summary>Trigonometric Cotangent of this <c>XComplex</c>.</summary>
    //    public XComplex Cotangent()
    //    {
    //        return Base.Cotangent();
    //    }

    //    /// <summary>Trigonometric Secant of this <c>XComplex</c>.</summary>
    //    public XComplex Secant()
    //    {
    //        return Base.Secant();
    //    }

    //    /// <summary>Trigonometric Cosecant of this <c>XComplex</c>.</summary>
    //    public XComplex Cosecant()
    //    {
    //        return Base.Cosecant();
    //    }

    //    #endregion  // Trigonometric Functions


    //    #region Trigonometric Arcus Functions

    //    /// <summary>Trigonometric Arcus Sine of this <c>XComplex</c>.</summary>
    //    public XComplex InverseSine()
    //    {
    //        return Base.InverseSine();
    //    }

    //    /// <summary>Trigonometric Arcus Cosine of this <c>XComplex</c>.</summary>
    //    public XComplex InverseCosine()
    //    {
    //        return Base.InverseCosine();
    //    }

    //    /// <summary>Trigonometric Arcus Tangent of this <c>XComplex</c>.</summary>
    //    public XComplex InverseTangent()
    //    {
    //        return Base.InverseTangent();
    //    }

    //    /// <summary>Trigonometric Arcus Cotangent of this <c>XComplex</c>.</summary>
    //    public XComplex InverseCotangent()
    //    {
    //        return Base.InverseCotangent();
    //    }

    //    /// <summary>Trigonometric Arcus Secant of this <c>CXomplex</c>.</summary>
    //    public XComplex InverseSecant()
    //    {
    //        return Base.InverseSecant();
    //    }

    //    /// <summary>Trigonometric Arcus Cosecant of this <c>XComplex</c>.</summary>
    //    public XComplex InverseCosecant()
    //    {
    //        return Base.InverseCosecant();
    //    }

    //    #endregion  // Trigonometric Arcus Functions


    //    #region Trigonometric Hyperbolic Functions


    //    /// <summary>Trigonometric Hyperbolic Sine  of this <c>XComplex</c>.</summary>
    //    public XComplex HyperbolicSine()
    //    {
    //        return Base.HyperbolicSine();
    //    }

    //    /// <summary>Trigonometric Hyperbolic Cosine of this <c>XComplex</c>.</summary>
    //    public XComplex HyperbolicCosine()
    //    {
    //        return Base.HyperbolicCosine();
    //    }


    //    /// <summary>Trigonometric Hyperbolic Tangent of this <c>XComplex</c>.</summary>
    //    public XComplex HyperbolicTangent()
    //    {
    //        return Base.HyperbolicTangent();
    //    }


    //    /// <summary>Trigonometric Hyperbolic Cotangent of this <c>XComplex</c>.</summary>
    //    public XComplex HyperbolicCotangent()
    //    {
    //        return Base.HyperbolicCotangent();
    //    }



    //    /// <summary>Trigonometric Hyperbolic Secant of this <c>XComplex</c>.</summary>
    //    public XComplex HyperbolicSecant()
    //    {
    //        return Base.HyperbolicSecant();
    //    }

    //    /// <summary>Trigonometric Hyperbolic Cosecant of this <c>XComplex</c>.</summary>
    //    public XComplex HyperbolicCosecant()
    //    {
    //        return Base.HyperbolicCosecant();
    //    }

    //    #endregion  // Trigonometric Hyperbolic Functions


    //    #region Trigonometric Hyperbolic Area Functions

    //    /// <summary> Trigonometric Hyperbolic Area Sine of this <c>XComplex</c>.</summary>
    //    public XComplex InverseHyperbolicSine()
    //    {
    //        return Base.InverseHyperbolicSine();
    //    }


    //    /// <summary>Trigonometric Hyperbolic Area Cosine of this <c>XComplex</c>.</summary>
    //    public XComplex InverseHyperbolicCosine()
    //    {
    //        return Base.InverseHyperbolicCosine();
    //    }


    //    /// <summary>Trigonometric Hyperbolic Area Tangent of this <c>XComplex</c>.</summary>
    //    public XComplex InverseHyperbolicTangent()
    //    {
    //        return Base.InverseHyperbolicTangent();
    //    }


    //    /// <summary>Trigonometric Hyperbolic Area Cotangent of this <c>XComplex</c>.</summary>
    //    public XComplex InverseHyperbolicCotangent()
    //    {
    //        return Base.InverseHyperbolicCotangent();
    //    }


    //    /// <summary>Trigonometric Hyperbolic Area Secant of this <c>XComplex</c>.</summary>
    //    public XComplex InverseHyperbolicSecant()
    //    {
    //        return Base.InverseHyperbolicSecant();
    //    }

        
    //    /// <summary>Trigonometric Hyperbolic Area Cosecant of this <c>XComplex</c>.</summary>
    //    public XComplex InverseHyperbolicCosecant()
    //    {
    //        return Base.InverseHyperbolicCosecant();
    //    }

    //    #endregion  // Trigonometric Hyperbolic Area Functions


    //    #region Exponential Functions


    //    /// <summary>Exponential of this <c>XComplex</c>.</summary>
    //    public XComplex Exponential()
    //    {
    //        return Base.Exponential();
    //    }


    //    /// <summary>Natural Logarithm of this <c>XComplex</c>.</summary>
    //    public XComplex NaturalLogarithm()
    //    {
    //        return Base.NaturalLogarithm();
    //    }


    //    /// <summary>Raise this <c>XComplex</c> to a given value.</summary>
    //    public XComplex Power(XComplex exponent)
    //    {
    //        return Base.Power(exponent);
    //    }


    //    /// <summary>Raise  this <c>XComplex</c> to the inverse of the given value.</summary>
    //    public XComplex Root(XComplex rootexponent)
    //    {
    //        return Base.Root(rootexponent);
    //    }



    //    /// <summary>The Square (power 2) of this <c>XComplex</c>.</summary>
    //    public XComplex Square()
    //    {
    //        return Base.Square();
    //    }


    //    /// <summary>The Square Root (power 1/2) of this <c>XComplex</c>.</summary>
    //    public XComplex SquareRoot()
    //    {
    //        return Base.SquareRoot();
    //    }

    //    #endregion  // Exponential Functions

    //    #endregion  // Functions


    //    public static void Examples()
    //    {
    //        XComplex xa = new XComplex(1, 2), xb = new XComplex(3, 2) ;
    //        MathNet.Numerics.Complex a, b;
    //        a = xa;
    //        b = xb;
    //        Console.WriteLine("a = " + a.ToString());
    //        Console.WriteLine("b = " + b.ToString());
    //        Console.WriteLine("a * b = " + (a * b).ToString());
    //        Console.WriteLine("xa = " + xa.ToString());
    //        Console.WriteLine("xb = " + xb.ToString());
    //        Console.WriteLine("xa * xb = " + (xa * xb).ToString());



    //    }

    //}  // class XComplex




    ///// <summary>Extension of the type MathNet.Numerics.Complex by using extension methods.</summary>
    //public static class ComplexExtension
    //{
    //    public static XComplex ToXComplex (this MathNet.Numerics.Complex c)
    //    {
    //        return XComplex.Clone(c);
    //    }
    //}  // class ComplexExtension






}











