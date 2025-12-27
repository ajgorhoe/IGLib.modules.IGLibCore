// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

    // Scalar (representation of double) and 
    // Counter struct (representation of long)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

using IG.Lib;




namespace IG.Num
{



    /// <summary>Represents a real number.</summary>
    [Obsolete("This class might be removed in the future.")]
    public struct Scalar : IFormattable, IComparable<Scalar>, IComparer<Scalar>
    {

        /// <summary>Value of the scalar.</summary>
        public double Value;

        #region Constructors

        public Scalar(Scalar v) { Value=v.Value; }
        public Scalar(double v) { Value = v; }

        #endregion  // Constructors

        #region Initialization_Assigning

        /// <summary>Returns a scalar with value 1.</summary>
        public static Scalar One { get { return new Scalar(1.0); } }

        /// <summary>Returns a scalar with value 0.</summary>
        public static Scalar Zero { get { return new Scalar(0.0); } }

        /// <summary>Copies a scalar to the current scalar.</summary>
        public void Copy(Scalar s) { Value = s.Value; }

        /// <summary>Copies double to the current scalar.</summary>
        public void Copy(double d) { Value = d; }

        /// <summary>Copies a scalar to another (existing) scalar in place.</summary>
        /// <param name="a">Scalar that is copied.</param>
        /// <param name="res">Scalar where result is stored.</param>
        public static void Copy(Scalar a, ref Scalar res)
        {
            res.Value = a.Value;
        }

        /// <summary>Copies a number to an (existing) scalar in place.</summary>
        /// <param name="a">Number that is copied.</param>
        /// <param name="res">Scalar where result is stored.</param>
        public static void Copy(double a, ref Scalar res)
        {
            res.Value = a;
        }


        #endregion // Initialization_Assigning

        #region Numeric

        /// <summary>Creates a scalar that is not a number.</summary>
        static public Scalar NaN() { return new Scalar(double.NaN); }

        /// <summary>Creates a scalar that represents positive infinity.</summary>
        static public Scalar PositiveInfinity() { return new Scalar(double.PositiveInfinity); }

        /// <summary>Creates a scalar that represents negative infinity.</summary>
        static public Scalar NegativeInfinity() { return new Scalar(double.NegativeInfinity); }
 
        /// <summary>Creates a scalar that contains minimal representable value.</summary>
        static public Scalar MinValue() { return new Scalar(double.MinValue); }
 
        /// <summary>Creates a scalar that contains maximal representable value.</summary>
        static public Scalar MaxValue() { return new Scalar(double.MaxValue); }
 
        /// <summary>Creates a scalar that contains the smallest positive value greater than 0.</summary>
        static public Scalar Epsilon() { return new Scalar(double.Epsilon); }
       

        // Checking for exceptional values: 

        /// <summary>Indicates whether the current <c>Scalar</c> is not a number.</summary>
        public bool IsNaN { get { return double.IsNaN(Value); } }

        /// <summary>Indicates whether the current <c>Scalar</c> represents positive infinity.</summary>
        public bool IsPositiveInfinity { get { return double.IsPositiveInfinity(Value); } }

        /// <summary>Indicates whether the current <c>Scalar</c> represents negative infinity.</summary>
        public bool IsNegativeInfinity { get { return double.IsNegativeInfinity(Value); } }

        /// <summary>Indicates whether the current <c>Scalar</c> represents infinity.</summary>
        public bool IsInfinity { get { return double.IsInfinity(Value); } }


        #endregion Numeric


        #region Equality_Comparison_Hashing


        public int Compare(Scalar a, Scalar b)
        {
            if (a.Value < b.Value)
                return -1;
            else if (a.Value > b.Value)
                return 1;
            else if (a.Value == b.Value)
                return 0;
            else
                return 0;
        }

        /// <summary>Indicates whether <c>obj</c> is equal to this instance.</summary>
        public override bool Equals(object obj)
        {
            return (obj is Scalar) && this.Equals((Scalar)obj);
        }

        /// <summary>Indicates whether <c>a</c> is equal to this instance. </summary>
        public bool Equals(Scalar a) 
        {
            return !IsNaN && !a.IsNaN && (Value==a.Value);
        }

        /// <summary>Gets the hashcode of this <c>Scalar</c>.</summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }


        /// <summary>Compare this Scalar with another Scalar.</summary>
        /// <param name="other">The scalar to compare with.</param>
        public int CompareTo(Scalar other) { return Value.CompareTo(other); }

        #endregion  // Equality_Comparison_Hashing


        #region Operators

        // Standard operators are defined i na standard way. Binary operators also allow mixing with standard numeric types.

        public static Scalar operator + (Scalar s) { return new Scalar (s.Value); }
        public static Scalar operator - (Scalar s) { return new Scalar(-s.Value); }

        public static Scalar operator + (Scalar a, Scalar b) { return new Scalar(a.Value + b.Value); }
        public static Scalar operator - (Scalar a, Scalar b) { return new Scalar(a.Value - b.Value); }
        public static Scalar operator * (Scalar a, Scalar b) { return new Scalar(a.Value * b.Value); }
        public static Scalar operator / (Scalar a, Scalar b) { return new Scalar(a.Value / b.Value); }
        public static bool operator == (Scalar a, Scalar b) { return (a.Value == b.Value); }
        public static bool operator != (Scalar a, Scalar b) { return (a.Value != b.Value); }
        public static bool operator < (Scalar a, Scalar b) { return (a.Value < b.Value); }
        public static bool operator > (Scalar a, Scalar b) { return (a.Value > b.Value); }
        public static bool operator <= (Scalar a, Scalar b) { return (a.Value <= b.Value); }
        public static bool operator >= (Scalar a, Scalar b) { return (a.Value >= b.Value); }

        //public static Scalar operator + (Scalar a, double s) { return new Scalar(a.Value  + s); }
        //public static Scalar operator + (double a, Scalar s) { return new Scalar(a  + s.Value); }

        //public static Scalar operator - (Scalar a, double s) { return new Scalar(a.Value - s); }
        //public static Scalar operator - (double a, Scalar s) { return new Scalar(a - s.Value); }

        //public static Scalar operator * (Scalar a, double s) { return new Scalar(a.Value * s); }
        //public static Scalar operator * (double a, Scalar s) { return new Scalar(a * s.Value); }

        //public static Scalar operator / (Scalar a, double s) { return new Scalar(a.Value / s); }
        //public static Scalar operator /(double a, Scalar s) { return new Scalar(a / s.Value); }

        //public static bool operator == (Scalar a, double s) { return (a.Value == s); }
        //public static bool operator == (double a, Scalar s) { return (a == s.Value); }

        //public static bool operator != (Scalar a, double s) { return (a.Value != s); }
        //public static bool operator != (double a, Scalar s) { return (a != s.Value); }

        //public static bool operator < ( Scalar a, double s) { return (a.Value < s); }
        //public static bool operator < (double a, Scalar s) { return (a < s.Value); }

        //public static bool operator > (Scalar a, double s) { return (a.Value > s); }
        //public static bool operator > (double a, Scalar s) { return (a > s.Value); }

        //public static bool operator <= ( Scalar a, double s) { return (a.Value <= s); }
        //public static bool operator <= (double a, Scalar s) { return (a <= s.Value); }

        //public static bool operator >= (Scalar a, double s) { return (a.Value >= s); }
        //public static bool operator >= (double a, Scalar s) { return  (a >= s.Value); }

        /// <summary>Implicit conversion from Scalar to double.</summary>
        public static implicit operator double(Scalar a)
        { return a.Value; }

        /// <summary>Implicit conversion from double to Scalar.</summary>
        public static implicit operator Scalar(double a)
        { return new Scalar(a); }

        #endregion Operators

        #region Formatting

        /// <summary>Returns standard string representation of a scalar, 
        /// with decimal point and e for exponent, no 1000 separators, number of digits according to type accuracy.</summary>
        /// <returns>String representing te scalar.</returns>
        public override string ToString()
        {
            return Value.ToString(null,CultureInfo.InvariantCulture);
        }

        /// <summary>Returns a string representation of a scalar in the specified way.</summary>
        /// <param name="format">the format string.</param>
        /// <param name="formatProvider">The format provider used.</param>
        /// <returns>String representation of the scalar.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Value.ToString(format, formatProvider);
        }

        #endregion Formatting


        #region Parsing

        // Static methods:

        /// <summary>Reads a scalar from a string, starting at the beginning and skipping any leading spaces. 
        /// Returns the position of the first character after the read scalar (or -1 if reading failed) and outputs the read scalar.</summary>
        /// <param name="result">Output parameter returning the scalar that was read.</param>
        /// <param name="str">String from which the scalar is read.</param>
        /// <returns>Position right after the read scalar, or -1 if reading was not successful.</returns>
        public static int Read(ref Scalar result, string str)
        {
            return Read(ref result, str, 0 /* startpos */, true /* skipspaces */);
        }

        /// <summary>Reads a scalar from a string, starting at a specified position and skipping any leading spaces. 
        /// Returns the position of the first character after the read scalar (or -1 if reading failed) and outputs the read scalar.</summary>
        /// <param name="result">Output parameter returning the scalar that was read.</param>
        /// <param name="str">String from which the scalar is read.</param>
        /// <param name="startpos">Starting position from which scalar is searched for.</param>
        /// <returns>Position right after the read scalar, or -1 if reading was not successful.</returns>
        public static int Read(ref Scalar result, string str, int startpos)
        {
            return Read(ref result, str, startpos, true /* skipspaces */);
        }


        /// <summary>Reads a scalar from a string, starting at a specified position. 
        /// Returns the position of the first character after the read scalar (or -1 if reading failed) and outputs the read scalar.</summary>
        /// <param name="result">Output parameter returning the scalar that was read.</param>
        /// <param name="str">String from which the scalar is read.</param>
        /// <param name="startpos">Starting position from which scalar is searched for.</param>
        /// <param name="skipspaces">If true then leading spaces are ignored, otherwise operation fails if there is a whitespace
        /// at the starting position.</param>
        /// <returns>Position right after the read scalar, or -1 if reading was not successful.</returns>
        public static int Read(ref Scalar result, string str, int startpos, bool skipspaces)
        {
            double d = 0;
            int ret = Parser.ReadNumber(ref d, str, startpos, skipspaces);
            result.Copy(d);
            return ret;
        }


        /// <summary>Parses a scalar from a string and returns it.
        /// Leading and trailing spaces are ignored, but other characters are not.
        /// FormatException is thrown if the string does not represent a scalar.</summary>
        /// <param name="str">String that is parsed.</param>
        /// <returns>The scalar represented by the parsed string.</returns>
        public static Scalar Parse(string str)
        {
            Scalar s = new Scalar(0d);
            int pos = Read(ref s,str);
            if (pos < 0)
                throw new FormatException("String does not represent a scalar. Culture must be neutral.");
            else
            {
                pos = Parser.SkipSpaces(str,pos);
                if (pos >= 0 && pos < str.Length)
                    throw new FormatException("Non-space characters follow the scalar.");
            }
            return s;
        }

        /// <summary>Parses a scalar from a string and returns it.
        /// Leading and trailing spaces are ignored, but other characters are not.
        /// false is returned if parsing was not successful, but no exception is thrown in this case.</summary>
        /// <param name="str">String that is parsed.</param>
        /// <param name="result">Variable where result of the operation is stored.</param>
        /// <returns>True if parsing was successful (and the scalar was actually read), false otherwise.</returns>
        public static bool TryParse(string str, ref Scalar result)
        {
            bool ret = false;
            try
            {
                Scalar s = new Scalar(0d);
                int pos = Read(ref s,str);
                if (pos > 0)
                {
                    pos = Parser.SkipSpaces(str, pos);
                    if (!(pos >= 0 && pos < str.Length))
                    {
                        // No characters other than spaces follow scalar representation, parsing was successful:
                        result = s;
                        ret = true;
                    }
                }
            }
            catch { }
            return ret;
        }

        // Instance methods:

        /// <summary>Reads in the value from a string, starting at the beginning and skipping any leading spaces. 
        /// Returns the position of the first character after the read scalar (or -1 if reading failed) and outputs the read scalar.</summary>
        /// <param name="str">String from which the scalar is read.</param>
        /// <returns>Position right after the read scalar, or -1 if reading was not successful.</returns>
        public int read(string str)
        {
            return Read(ref this, str);
        }

        /// <summary>Reads in the value from a string, starting at a specified position and skipping any leading spaces. 
        /// Returns the position of the first character after the read scalar (or -1 if reading failed) and outputs the read scalar.</summary>
        /// <param name="str">String from which the scalar is read.</param>
        /// <param name="startpos">Starting position from which scalar is searched for.</param>
        /// <returns>Position right after the read scalar, or -1 if reading was not successful.</returns>
        public int Read(string str, int startpos)
        {
            return Read(ref this, str, startpos);
        }


        /// <summary>Reads in the value from a string, starting at a specified position. 
        /// Returns the position of the first character after the read scalar (or -1 if reading failed) and outputs the read scalar.</summary>
        /// <param name="str">String from which the scalar is read.</param>
        /// <param name="startpos">Starting position from which scalar is searched for.</param>
        /// <param name="skipspaces">If true then leading spaces are ignored, otherwise operation fails if there is a whitespace
        /// at the starting position.</param>
        /// <returns>Position right after the read scalar, or -1 if reading was not successful.</returns>
        public int Read(string str, int startpos, bool skipspaces)
        {
            return Read(ref this, str, startpos, skipspaces);
        }


        /// <summary>Parses a scalar from a string and sets the value to the parsed scalar.
        /// Leading and trailing spaces are ignored, but other characters are not.
        /// FormatException is thrown if the string does not represent a scalar.</summary>
        /// <param name="str">String that is parsed.</param>
        public void Copy(string str)
        {
            Scalar s;
            s = Parse(str);
            this.Copy(s);
        }

        /// <summary>Parses a scalar from a string and sets the value to the parsed scalar.
        /// Leading and trailing spaces are ignored, but other characters are not.
        /// false is returned if parsing was not successful, but no exception is thrown in this case.</summary>
        /// <param name="str">String that is parsed.</param>
        /// <returns>True if parsing was successful (and the scalar was actually read), false otherwise.</returns>
        public bool TryCopy(string str)
        {
            return TryParse(str, ref this);
        }

        #endregion Parsing




        /// <summary>Examples of using the Scalar class</summary>
        static public void Examples()
        {

            Scalar a = new Scalar(5.58058390863867695);
            int ib = 10;
            // + can be combined with type int, although it is only defined for double (implicit type conversion!):
            Console.WriteLine("a = " + a.ToString() + ", ib = " + ib.ToString() + ", a + ib = " + (a+ib).ToString());

            Console.ReadLine();
        }


    }  // struct Scalar



    /// <summary>Represents a real number.</summary>
    public struct Counter : IFormattable, IComparable<Counter>, IComparer<Counter>
    {

        /// <summary>Value of the counter.</summary>
        public long Value;

        #region Constructors

        public Counter(Counter v) { Value = v.Value; }
        // public Counter(long v) { Value = v; }

        #endregion  // Constructors

        #region Initialization_Assigning

        /// <summary>Returns a counter with value 1.</summary>
        public static Counter One { get { return new Counter(1); } }

        /// <summary>Returns a counter with value 0.</summary>
        public static Counter Zero { get { return new Counter(0); } }

        /// <summary>Copies a counter to the current counter.</summary>
        public void Copy(Counter s) { Value = s.Value; }

        /// <summary>Copies double to the current counter.</summary>
        public void Copy(long l) { Value = l; }

        /// <summary>Copies a counter to another (existing) counter in place.</summary>
        /// <param name="a">Counter that is copied.</param>
        /// <param name="res">Counter where result is stored.</param>
        public static void Copy(Counter a, ref Counter res)
        {
            res.Value = a.Value;
        }

        /// <summary>Copies a number to an (existing) counter in place.</summary>
        /// <param name="a">Number that is copied.</param>
        /// <param name="res">Counter where result is stored.</param>
        public static void Copy(long a, ref Counter res)
        {
            res.Value = a;
        }


        #endregion // Initialization_Assigning

        #region Numeric


        /// <summary>Creates a counter that contains minimal representable value.</summary>
        static public Counter MinValue() { return new Counter(long.MinValue); }

        /// <summary>Creates a counter that contains maximal representable value.</summary>
        static public Counter MaxValue() { return new Counter(long.MaxValue); }

        // Checking for exceptional values: 

        /// <summary>Indicates whether the current <c>Counter</c> is not a number.</summary>
        public bool IsNaN { get { return double.IsNaN(Value); } }

        /// <summary>Indicates whether the current <c>Counter</c> represents positive infinity.</summary>
        public bool IsPositiveInfinity { get { return double.IsPositiveInfinity(Value); } }

        /// <summary>Indicates whether the current <c>Counter</c> represents negative infinity.</summary>
        public bool IsNegativeInfinity { get { return double.IsNegativeInfinity(Value); } }

        /// <summary>Indicates whether the current <c>Counter</c> represents infinity.</summary>
        public bool IsInfinity { get { return double.IsInfinity(Value); } }


        #endregion Numeric


        #region Equality_Comparison_Hashing


        public int Compare(Counter a, Counter b)
        {
            if (a.Value < b.Value)
                return -1;
            else if (a.Value > b.Value)
                return 1;
            else if (a.Value == b.Value)
                return 0;
            else
                return 0;
        }

        /// <summary>Indicates whether <c>obj</c> is equal to this instance.</summary>
        public override bool Equals(object obj)
        {
            return (obj is Counter) && this.Equals((Counter)obj);
        }

        /// <summary>Indicates whether <c>a</c> is equal to this instance. </summary>
        public bool Equals(Counter a)
        {
            return !IsNaN && !a.IsNaN && (Value == a.Value);
        }

        /// <summary>Gets the hashcode of this <c>Counter</c>.</summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }


        /// <summary>Compare this Counter with another Counter.</summary>
        /// <param name="other">The counter to compare with.</param>
        public int CompareTo(Counter other) { return Value.CompareTo(other); }

        #endregion  // Equality_Comparison_Hashing


        #region Operators

        // Standard operators are defined i na standard way. Binary operators also allow mixing with standard numeric types.

        public static Counter operator +(Counter s) { return new Counter(s.Value); }
        public static Counter operator -(Counter s) { return new Counter(-s.Value); }

        public static Counter operator +(Counter a, Counter b) { return new Counter(a.Value + b.Value); }
        public static Counter operator -(Counter a, Counter b) { return new Counter(a.Value - b.Value); }
        public static Counter operator *(Counter a, Counter b) { return new Counter(a.Value * b.Value); }
        public static Counter operator /(Counter a, Counter b) { return new Counter(a.Value / b.Value); }
        public static bool operator ==(Counter a, Counter b) { return (a.Value == b.Value); }
        public static bool operator !=(Counter a, Counter b) { return (a.Value != b.Value); }
        public static bool operator <(Counter a, Counter b) { return (a.Value < b.Value); }
        public static bool operator >(Counter a, Counter b) { return (a.Value > b.Value); }
        public static bool operator <=(Counter a, Counter b) { return (a.Value <= b.Value); }
        public static bool operator >=(Counter a, Counter b) { return (a.Value >= b.Value); }

        //public static Counter operator +(Counter a, long s) { return new Counter(a.Value + s); }
        //public static Counter operator +(long a, Counter s) { return new Counter(a + s.Value); }

        //public static Counter operator -(Counter a, long s) { return new Counter(a.Value - s); }
        //public static Counter operator -(long a, Counter s) { return new Counter(a - s.Value); }

        //public static Counter operator *(Counter a, long s) { return new Counter(a.Value * s); }
        //public static Counter operator *(long a, Counter s) { return new Counter(a * s.Value); }

        //public static Counter operator /(Counter a, long s) { return new Counter(a.Value / s); }
        //public static Counter operator /(long a, Counter s) { return new Counter(a / s.Value); }

        //public static bool operator ==(Counter a, long s) { return (a.Value == s); }
        //public static bool operator ==(long a, Counter s) { return (a == s.Value); }

        //public static bool operator !=(Counter a, long s) { return (a.Value != s); }
        //public static bool operator !=(long a, Counter s) { return (a != s.Value); }

        //public static bool operator <(Counter a, long s) { return (a.Value < s); }
        //public static bool operator <(long a, Counter s) { return (a < s.Value); }

        //public static bool operator >(Counter a, long s) { return (a.Value > s); }
        //public static bool operator >(long a, Counter s) { return (a > s.Value); }

        //public static bool operator <=(Counter a, long s) { return (a.Value <= s); }
        //public static bool operator <=(long a, Counter s) { return (a <= s.Value); }

        //public static bool operator >=(Counter a, long s) { return (a.Value >= s); }
        //public static bool operator >=(long a, Counter s) { return (a >= s.Value); }

        /// <summary>Explicit conversion from <c>Counter</c> to long.</summary>
        public static implicit operator long(Counter a)
        { return a.Value; }

        /// <summary>Implicit conversion from long to <c>Counter</c>.</summary>
        public static implicit operator Counter(long a)
        { return new Counter(a); }

        #endregion Operators

        #region Formatting

        /// <summary>Returns standard string representation of a counter, 
        /// with decimal point and e for exponent, no 1000 separators, number of digits according to type accuracy.</summary>
        /// <returns>String representing te counter.</returns>
        public override string ToString()
        {
            return Value.ToString(null, CultureInfo.InvariantCulture);
        }

        /// <summary>Returns a string representation of a counter in the specified way.</summary>
        /// <param name="format">the format string.</param>
        /// <param name="formatProvider">The format provider used.</param>
        /// <returns>String representation of the counter.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Value.ToString(format, formatProvider);
        }

        #endregion Formatting


        #region Parsing

        // Static methods:

        /// <summary>Reads a counter from a string, starting at the beginning and skipping any leading spaces. 
        /// Returns the position of the first character after the read counter (or -1 if reading failed) and outputs the read counter.</summary>
        /// <param name="result">Output parameter returning the counter that was read.</param>
        /// <param name="str">String from which the counter is read.</param>
        /// <returns>Position right after the read counter, or -1 if reading was not successful.</returns>
        public static int Read(ref Counter result, string str)
        {
            return Read(ref result, str, 0 /* startpos */, true /* skipspaces */);
        }

        /// <summary>Reads a counter from a string, starting at a specified position and skipping any leading spaces. 
        /// Returns the position of the first character after the read counter (or -1 if reading failed) and outputs the read counter.</summary>
        /// <param name="result">Output parameter returning the counter that was read.</param>
        /// <param name="str">String from which the counter is read.</param>
        /// <param name="startpos">Starting position from which counter is searched for.</param>
        /// <returns>Position right after the read counter, or -1 if reading was not successful.</returns>
        public static int Read(ref Counter result, string str, int startpos)
        {
            return Read(ref result, str, startpos, true /* skipspaces */);
        }


        /// <summary>Reads a counter from a string, starting at a specified position. 
        /// Returns the position of the first character after the read counter (or -1 if reading failed) and outputs the read counter.</summary>
        /// <param name="result">Output parameter returning the counter that was read.</param>
        /// <param name="str">String from which the counter is read.</param>
        /// <param name="startpos">Starting position from which counter is searched for.</param>
        /// <param name="skipspaces">If true then leading spaces are ignored, otherwise operation fails if there is a whitespace
        /// at the starting position.</param>
        /// <returns>Position right after the read counter, or -1 if reading was not successful.</returns>
        public static int Read(ref Counter result, string str, int startpos, bool skipspaces)
        {
            long l = 0;
            int ret = Parser.ReadInteger(ref l, str, startpos, skipspaces);
            result.Copy(l);
            return ret;
        }


        /// <summary>Parses a counter from a string and returns it.
        /// Leading and trailing spaces are ignored, but other characters are not.
        /// FormatException is thrown if the string does not represent a counter.</summary>
        /// <param name="str">String that is parsed.</param>
        /// <returns>The counter represented by the parsed string.</returns>
        public static Counter Parse(string str)
        {
            Counter s = new Counter(0);
            int pos = Read(ref s, str);
            if (pos < 0)
                throw new FormatException("String does not represent a counter. Culture must be neutral.");
            else
            {
                pos = Parser.SkipSpaces(str, pos);
                if (pos >= 0 && pos < str.Length)
                    throw new FormatException("Non-space characters follow the counter.");
            }
            return s;
        }

        /// <summary>Parses a counter from a string and returns it.
        /// Leading and trailing spaces are ignored, but other characters are not.
        /// false is returned if parsing was not successful, but no exception is thrown in this case.</summary>
        /// <param name="str">String that is parsed.</param>
        /// <param name="result">Variable where result is stored.</param>
        /// <returns>True if parsing was successful (and the counter was actually read), false otherwise.</returns>
        public static bool TryParse(string str, ref Counter result)
        {
            bool ret = false;
            try
            {
                Counter s = new Counter(0);
                int pos = Read(ref s, str);
                if (pos > 0)
                {
                    pos = Parser.SkipSpaces(str, pos);
                    if (!(pos >= 0 && pos < str.Length))
                    {
                        // No characters other than spaces follow counter representation, parsing was successful:
                        result = s;
                        ret = true;
                    }
                }
            }
            catch { }
            return ret;
        }

        // Instance methods:

        /// <summary>Reads in the value from a string, starting at the beginning and skipping any leading spaces. 
        /// Returns the position of the first character after the read counter (or -1 if reading failed) and outputs the read counter.</summary>
        /// <param name="str">String from which the counter is read.</param>
        /// <returns>Position right after the read counter, or -1 if reading was not successful.</returns>
        public int read(string str)
        {
            return Read(ref this, str);
        }

        /// <summary>Reads in the value from a string, starting at a specified position and skipping any leading spaces. 
        /// Returns the position of the first character after the read counter (or -1 if reading failed) and outputs the read counter.</summary>
        /// <param name="str">String from which the counter is read.</param>
        /// <param name="startpos">Starting position from which counter is searched for.</param>
        /// <returns>Position right after the read counter, or -1 if reading was not successful.</returns>
        public int Read(string str, int startpos)
        {
            return Read(ref this, str, startpos);
        }


        /// <summary>Reads in the value from a string, starting at a specified position. 
        /// Returns the position of the first character after the read counter (or -1 if reading failed) and outputs the read counter.</summary>
        /// <param name="str">String from which the counter is read.</param>
        /// <param name="startpos">Starting position from which counter is searched for.</param>
        /// <param name="skipspaces">If true then leading spaces are ignored, otherwise operation fails if there is a whitespace
        /// at the starting position.</param>
        /// <returns>Position right after the read counter, or -1 if reading was not successful.</returns>
        public int Read(string str, int startpos, bool skipspaces)
        {
            return Read(ref this, str, startpos, skipspaces);
        }


        /// <summary>Parses a counter from a string and sets the value to the parsed counter.
        /// Leading and trailing spaces are ignored, but other characters are not.
        /// FormatException is thrown if the string does not represent a counter.</summary>
        /// <param name="str">String that is parsed.</param>
        public void Copy(string str)
        {
            Counter s;
            s = Parse(str);
            this.Copy(s);
        }

        /// <summary>Parses a counter from a string and sets the value to the parsed counter.
        /// Leading and trailing spaces are ignored, but other characters are not.
        /// false is returned if parsing was not successful, but no exception is thrown in this case.</summary>
        /// <param name="str">String that is parsed.</param>
        /// <returns>True if parsing was successful (and the counter was actually read), false otherwise.</returns>
        public bool TryCopy(string str)
        {
            return TryParse(str, ref this);
        }

        #endregion Parsing




        /// <summary>Examples of using the Counter class</summary>
        static public void Examples()
        {

            Counter a = new Counter(5);
            int ib = 10;
            // + can be combined with type int, although it is only defined for double (implicit type conversion!):
            Console.WriteLine("a = " + a.ToString() + ", ib = " + ib.ToString() + ", a + ib = " + (a + ib).ToString());

            Console.ReadLine();
        }


    }  // struct Counter

}  // namespace


