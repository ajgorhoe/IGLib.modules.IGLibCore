//// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using IG.Lib;

//namespace IG.Num
//{


//    ///// <summary>Uniform random number generator.</summary>
//    //public interface IRandomGenerator
//    //{

//    //    /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//    //    /// and LESS THAN 1.0.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
//    //    double NextDouble();

//    //    /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//    //    /// and LESS than the specified maximum.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to 0.0, and
//    //    /// LESS THAN THE SPECIFIED MAXIMUM.</returns>
//    //    double NextDouble(double maxValue);

//    //    /// <summary>A double-precision floating point number greater than or equal to the specified minimum, 
//    //    /// and LESS than the specified maximum.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to the specified minimum, 
//    //    /// and LESS than the specified maximum.</returns>
//    //    double NextDouble(double minValue, double maxValue);

//    //    /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//    //    /// and LESS OR EQUAL than 1.0.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
//    //    double NextDoubleInclusive();

//    //    /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//    //    /// and LESS OR EQUAL than the specified maximum.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to 0.0, and
//    //    /// LESS OR EQUAL THAN THE SPECIFIED MAXIMUM.</returns>
//    //    double NextDoubleInclusive(double maxvalue);

//    //    /// <summary>A double-precision floating point number greater than or equal to the specified minimum, 
//    //    /// and LESS OR EQUAL than the specified maximum.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to the specified minimum, 
//    //    /// and LESS OR EQUAL than the specified maximum.</returns>
//    //    double NextDoubleInclusive(double minValue, double maxValue);

//    //    /// <summary>Returns a nonnegative random number.</summary>
//    //    /// <returns>A 32-bit signed integer greater than or equal to zero and less than MaxValue.</returns>
//    //    int Next();

//    //    /// <summary>Returns a nonnegative random number LESS THAN the specified maximum.</summary>
//    //    /// <param name="maxValue">The EXCLUSIVE upper bound of the random number to be generated.
//    //    /// Must be greater than or equal to zero. </param>
//    //    /// <returns>A 32-bit signed integer greater than or equal to zero, and LESS THAN maxValue.
//    //    /// If maxValue equals zero, maxValue is returned.</returns>
//    //    int Next(int maxValue);

//    //    /// <summary>Returns a random number within a specified range (lower bound inclusive,
//    //    /// UPPER BOUND EXCLUSIVE).</summary>
//    //    /// <param name="minValue">The inclusive lower bound of the random number returned. </param>
//    //    /// <param name="maxValue">The EXCLUSIVE UPPER BOUND of the random number returned.
//    //    /// Must be greater than or equal to minValue. </param>
//    //    /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue.
//    //    /// If minValue equals maxValue, minValue  is returned.</returns>
//    //    int Next(int minValue, int maxValue);

//    //    /// <summary>Returns a nonnegative random number LESS OR EQUAL THAN the specified maximum.</summary>
//    //    /// <param name="maxValue">The INCLUSIVE upper bound of the random number to be generated.
//    //    /// Must be greater than or equal to zero. </param>
//    //    /// <returns>A 32-bit signed integer greater than or equal to zero, and LESS OR EQUAL than maxValue.
//    //    /// If maxValue equals zero, maxValue is returned.</returns>
//    //    int NextInclusive(int maxValue);

//    //    /// <summary>Returns a random number within a specified range (lower bound inclusive,
//    //    /// upper bound INCLUSIVE).</summary>
//    //    /// <param name="minValue">he inclusive lower bound of the random number returned. </param>
//    //    /// <param name="maxValue">The INCLUSIVE upper bound of the random number returned.
//    //    /// Must be greater than or equal to minValue.</param>
//    //    /// <returns>A 32-bit signed integer greater than or equal to minValue and LESS OR EQUAL than maxValue.
//    //    /// If minValue equals maxValue, minValue  is returned.</returns>
//    //    int NextInclusive(int minValue, int maxValue);

//    //    /// <summary>Fills the elements of a specified array of bytes with random numbers.</summary>
//    //    /// <param name="buffer">An array of bytes to contain random numbers. </param>
//    //    void NextBytes(byte[] buffer);

//    //}


//    ///// <summary>Default generator of uniformly distributed random numbers.
//    ///// Provides a global generator and a static function for generating new generators.
//    ///// Currently, the generator used is the system's generator built in C#.</summary>
//    //public class RandomGenerator : RandomGeneratorSystem, IRandomGenerator
//    //{

//    //    #region initialization

//    //    /// <summary>Initializes a new instance of random generator, using a time-dependent default seed value.</summary>
//    //    public RandomGenerator()
//    //        : base()
//    //    {  }

//    //    /// <summary>Initializes a new instance of random generator, using the specified seed value.</summary>
//    //    /// <param name="seed">A number used to calculate a starting value for the pseudo-random number sequence. 
//    //    /// If a negative number is specified, the absolute value of the number is used. </param>
//    //    public RandomGenerator(int seed)
//    //        : base(seed)
//    //    {  }

//    //    #endregion initialization

//    //    #region Global

//    //    static IRandomGenerator _global = null;

//    //    /// <summary>Global random generator. Initialized with time dependent seed.
//    //    /// Therefore, the generator should generate different sequences each time the application is run,
//    //    /// but it can nod be used to create deterministic sequences because seeding with a specified
//    //    /// value is not possible.
//    //    /// The returned generator is thread safe. It is initialized when first accessed.</summary>
//    //    public static IRandomGenerator Global
//    //    {
//    //        get
//    //        {
//    //            if (_global == null)
//    //            {
//    //                lock (_lock)
//    //                {
//    //                    if (_global == null)
//    //                    {
//    //                        _global = new RandGeneratorThreadSafe();
                            
//    //                    }
//    //                }
//    //            }
//    //            return _global;
//    //        }
//    //    }

//    //    #endregion Global

//    //    /// <summary>Creates and returns a new random generator initialized with a time dependent seed.
//    //    /// WARNING:
//    //    /// The returned generator is NOT THREAD SAFE. Use GetThreadSafe() for a thread safe generator.</summary>
//    //    public static IRandomGenerator Create()
//    //    { return new RandomGenerator(); }

//    //     /// <summary>Creates and returns a new random generator initialized with a specified seed.
//    //    /// The returned generator is NOT THREAD SAFE. Use GetThreadSafe(seed) for a thread safe generator.</summary>
//    //    public static IRandomGenerator Create(int seed)
//    //    { return new RandomGenerator(seed); }

//    //    /// <summary>Creates and returns a new random generator initialized with a time dependent seed.
//    //    /// The returned generator IS THREAD SAFE.</summary>
//    //    public static IRandomGenerator CreateThreadSafe()
//    //    { return new RandGeneratorThreadSafe(); }

//    //     /// <summary>Creates and returns a new random generator initialized with a specified seed.
//    //    /// The returned generator IS THREAD SAFE.</summary>
//    //    public static IRandomGenerator CreateThreadSafe(int seed)
//    //    { return new RandGeneratorThreadSafe(seed); }

//    //    protected static object _lock = new Object();

//    // }  // class Rand


//    /// <summary>Generator of uniformly distributed random numbers.
//    /// Based on the default random generator.
//    /// Instance members are thread safe!</summary>
//    public class RandGeneratorThreadSafe : RandomGeneratorSystem, IRandomGenerator, ILockable
//    {

//        #region initialization

//        /// <summary>Initializes a new instance of random generator, using a time-dependent default seed value.</summary>
//        public RandGeneratorThreadSafe()
//            : base()
//        { }

//        /// <summary>Initializes a new instance of random generator, using the specified seed value.</summary>
//        /// <param name="seed">A number used to calculate a starting value for the pseudo-random number sequence. 
//        /// If a negative number is specified, the absolute value of the number is used. </param>
//        public RandGeneratorThreadSafe(int seed)
//            : base()
//        { }

//        #endregion initialization

//        protected object _lock = new Object();

//        /// <summary>Gets an object used for locking of the current object.</summary>
//        public object Lock
//        {
//            get { return _lock; }
//        }

//        #region IRandom Members

//        /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//        /// and LESS THAN 1.0.</summary>
//        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
//        public override double NextDouble()
//        { lock (_lock) { return base.NextDouble(); } }

//        /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//        /// and LESS than the specified maximum.</summary>
//        /// <returns>A double-precision floating point number greater than or equal to 0.0, and
//        /// LESS THAN THE SPECIFIED MAXIMUM.</returns>
//        public override double NextDouble(double maxValue)
//        { lock (_lock) { return base.NextDouble(maxValue); } }

//        /// <summary>A double-precision floating point number greater than or equal to the specified minimum, 
//        /// and LESS THAN the specified maximum.</summary>
//        /// <returns>A double-precision floating point number greater than or equal to the specified minimum, 
//        /// and LESS than the specified maximum.</returns>
//        public override double NextDouble(double minValue, double maxValue)
//        { lock (_lock) { return base.NextDouble(minValue, maxValue); } }

//        /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//        /// and LESS OR EQUAL than 1.0.</summary>
//        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
//        public override double NextDoubleInclusive()
//        { lock (_lock) { return base.NextDoubleInclusive(); } }

//        /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//        /// and LESS OR EQUAL than the specified maximum.</summary>
//        /// <returns>A double-precision floating point number greater than or equal to 0.0, and
//        /// LESS OR EQUAL THAN THE SPECIFIED MAXIMUM.</returns>
//        public override double NextDoubleInclusive(double maxValue)
//        { lock (_lock) { return base.NextDoubleInclusive(maxValue); } }

//        /// <summary>A double-precision floating point number greater than or equal to the specified minimum, 
//        /// and LESS OR EQUAL than the specified maximum.</summary>
//        /// <returns>A double-precision floating point number greater than or equal to the specified minimum, 
//        /// and LESS OR EQUAL than the specified maximum.</returns>
//        public override double NextDoubleInclusive(double minValue, double maxValue)
//        { lock (_lock) { return base.NextDoubleInclusive(minValue, maxValue); } }

//        /// <summary>Returns a nonnegative random number.</summary>
//        /// <returns>A 32-bit signed integer greater than or equal to zero and less than MaxValue.</returns>
//        public override int Next()
//        { lock (_lock) { return base.Next(); } }

//        /// <summary>Returns a nonnegative random number LESS THAN the specified maximum.</summary>
//        /// <param name="maxValue">The EXCLUSIVE upper bound of the random number to be generated.
//        /// Must be greater than or equal to zero. </param>
//        /// <returns>A 32-bit signed integer greater than or equal to zero, and LESS THAN maxValue.
//        /// If maxValue equals zero, maxValue is returned.</returns>
//        public override int Next(int maxValue)
//        { lock (_lock) { return base.Next(maxValue); } }

//        /// <summary>Returns a random number within a specified range (lower bound inclusive,
//        /// UPPER BOUND EXCLUSIVE).</summary>
//        /// <param name="minValue">The inclusive lower bound of the random number returned. </param>
//        /// <param name="maxValue">The EXCLUSIVE UPPER BOUND of the random number returned.
//        /// Must be greater than or equal to minValue. </param>
//        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue.
//        /// If minValue equals maxValue, minValue  is returned.</returns>
//        public override int Next(int minValue, int maxValue)
//        { lock (_lock) { return base.Next(minValue, maxValue); } }

//        /// <summary>Returns a nonnegative random number LESS OR EQUAL the specified maximum.</summary>
//        /// <param name="maxValue">The INCLUSIVE upper bound of the random number to be generated.
//        /// Must be greater than or equal to zero. </param>
//        /// <returns>A 32-bit signed integer greater than or equal to zero, and LESS OR EQUAL than maxValue.
//        /// If maxValue equals zero, maxValue is returned.</returns>
//        public override int NextInclusive(int maxValue)
//        { lock (_lock) { return base.NextInclusive(maxValue); } }

//        /// <summary>Returns a random number within a specified range (lower bound inclusive,
//        /// upper bound INCLUSIVE).</summary>
//        /// <param name="minValue">he inclusive lower bound of the random number returned. </param>
//        /// <param name="maxValue">The INCLUSIVE upper bound of the random number returned.
//        /// Must be greater than or equal to minValue.</param>
//        /// <returns>A 32-bit signed integer greater than or equal to minValue and LESS OR EQUAL than maxValue.
//        /// If minValue equals maxValue, minValue  is returned.</returns>
//        public override int NextInclusive(int minValue, int maxValue)
//        { lock (_lock) { return base.NextInclusive(minValue, maxValue); } }

//        /// <summary>Fills the elements of a specified array of bytes with random numbers.</summary>
//        /// <param name="buffer">An array of bytes to contain random numbers. </param>
//        public override void NextBytes(byte[] buffer)
//        { lock (_lock) { base.NextBytes(buffer); } }

//        #endregion

//    }  // class RandomSystemThreadSafe


//    ///// <summary>Generator of uniformly distributed random numbers.
//    ///// Based on system random generator.
//    ///// WARNING: Instance members are not guaranteed to be thread safe!</summary>
//    //public class RandomGeneratorSystem : System.Random, IRandomGenerator
//    //{

//    //    #region initialization

//    //    /// <summary>Initializes a new instance of random generator, using a time-dependent default seed value.</summary>
//    //    public RandomGeneratorSystem() : base()
//    //    {  }

//    //    /// <summary>Initializes a new instance of random generator, using the specified seed value.</summary>
//    //    /// <param name="seed">A number used to calculate a starting value for the pseudo-random number sequence. 
//    //    /// If a negative number is specified, the absolute value of the number is used. </param>
//    //    public RandomGeneratorSystem(int seed) : base()
//    //    {  }

//    //    #endregion initialization


//    //    #region IRandom Members

//    //    /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//    //    /// and LESS THAN 1.0.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
//    //    public override double NextDouble()
//    //    {
//    //        return base.NextDouble();
//    //    }

//    //    /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//    //    /// and LESS than the specified maximum.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to 0.0, and
//    //    /// LESS THAN THE SPECIFIED MAXIMUM.</returns>
//    //    public virtual double NextDouble(double maxValue)
//    //    {
//    //        double ret = maxValue * NextDouble();
//    //        if (ret == maxValue && maxValue!=0) // take care of exclusivity (mind rounding errors)
//    //            return NextDouble(maxValue);
//    //        return ret;
//    //    }

//    //    /// <summary>A double-precision floating point number greater than or equal to the specified minimum, 
//    //    /// and LESS THAN the specified maximum.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to the specified minimum, 
//    //    /// and LESS than the specified maximum.</returns>
//    //    public virtual double NextDouble(double minValue, double maxValue)
//    //    {
//    //        double ret = minValue + NextDouble()*(maxValue-minValue);
//    //        if (ret == maxValue && maxValue != minValue) // take care of exclusivity (mind rounding errors)
//    //            return NextDouble(minValue, maxValue);
//    //        return ret;
//    //    }

//    //    /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//    //    /// and LESS OR EQUAL than 1.0.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
//    //    public virtual double NextDoubleInclusive()
//    //    {
//    //        double ret = NextDouble()*(1.0+1.0e-10);
//    //        if (ret > 1)
//    //            return NextDoubleInclusive();
//    //        return ret;
//    //    }

//    //    /// <summary>A double-precision floating point number greater than or equal to 0.0, 
//    //    /// and LESS OR EQUAL than the specified maximum.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to 0.0, and
//    //    /// LESS OR EQUAL THAN THE SPECIFIED MAXIMUM.</returns>
//    //    public virtual double NextDoubleInclusive(double maxValue)
//    //    {
//    //        double ret = maxValue * NextDoubleInclusive();
//    //        if (ret > maxValue) // take care of exclusivity (mind rounding errors)
//    //            return NextDoubleInclusive(maxValue);
//    //        return ret;
//    //    }

//    //    /// <summary>A double-precision floating point number greater than or equal to the specified minimum, 
//    //    /// and LESS OR EQUAL than the specified maximum.</summary>
//    //    /// <returns>A double-precision floating point number greater than or equal to the specified minimum, 
//    //    /// and LESS OR EQUAL than the specified maximum.</returns>
//    //    public virtual double NextDoubleInclusive(double minValue, double maxValue)
//    //    {
//    //        double ret = minValue + NextDoubleInclusive() * (maxValue - minValue);
//    //        if (ret > maxValue) // take care of exclusivity (mind rounding errors)
//    //            return NextDoubleInclusive(minValue, maxValue);
//    //        return ret;
//    //    }

//    //    /// <summary>Returns a nonnegative random number.</summary>
//    //    /// <returns>A 32-bit signed integer greater than or equal to zero and less than MaxValue.</returns>
//    //    public override int Next()
//    //    {
//    //        return base.Next();
//    //    }

//    //    /// <summary>Returns a nonnegative random number LESS THAN the specified maximum.</summary>
//    //    /// <param name="maxValue">The EXCLUSIVE upper bound of the random number to be generated.
//    //    /// Must be greater than or equal to zero. </param>
//    //    /// <returns>A 32-bit signed integer greater than or equal to zero, and LESS THAN maxValue.
//    //    /// If maxValue equals zero, maxValue is returned.</returns>
//    //    public override int Next(int maxValue)
//    //    {
//    //        return base.Next(maxValue);
//    //    }

//    //    /// <summary>Returns a random number within a specified range (lower bound inclusive,
//    //    /// UPPER BOUND EXCLUSIVE).</summary>
//    //    /// <param name="minValue">The inclusive lower bound of the random number returned. </param>
//    //    /// <param name="maxValue">The EXCLUSIVE UPPER BOUND of the random number returned.
//    //    /// Must be greater than or equal to minValue. </param>
//    //    /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue.
//    //    /// If minValue equals maxValue, minValue  is returned.</returns>
//    //    public override int Next(int minValue, int maxValue)
//    //    {
//    //        return base.Next(minValue, maxValue);
//    //    }

//    //    /// <summary>Returns a nonnegative random number LESS OR EQUAL the specified maximum.</summary>
//    //    /// <param name="maxValue">The INCLUSIVE upper bound of the random number to be generated.
//    //    /// Must be greater than or equal to zero. </param>
//    //    /// <returns>A 32-bit signed integer greater than or equal to zero, and LESS OR EQUAL than maxValue.
//    //    /// If maxValue equals zero, maxValue is returned.</returns>
//    //    public virtual int NextInclusive(int maxValue)
//    //    {
//    //        if (maxValue==Int32.MaxValue)
//    //            throw new ArgumentException("Maximum integer value for random generation must be less than "
//    //                + Int32.MaxValue);
//    //        return base.Next(maxValue + 1);
//    //    }

//    //    /// <summary>Returns a random number within a specified range (lower bound inclusive,
//    //    /// upper bound INCLUSIVE).</summary>
//    //    /// <param name="minValue">he inclusive lower bound of the random number returned. </param>
//    //    /// <param name="maxValue">The INCLUSIVE upper bound of the random number returned.
//    //    /// Must be greater than or equal to minValue.</param>
//    //    /// <returns>A 32-bit signed integer greater than or equal to minValue and LESS OR EQUAL than maxValue.
//    //    /// If minValue equals maxValue, minValue  is returned.</returns>
//    //    public virtual int NextInclusive(int minValue, int maxValue)
//    //    {
//    //        if (maxValue==Int32.MaxValue)
//    //            throw new ArgumentException("Maximum integer value for random generation must be less than "
//    //                + Int32.MaxValue);
//    //        return Next(minValue, maxValue + 1);
//    //    }

//    //    /// <summary>Fills the elements of a specified array of bytes with random numbers.</summary>
//    //    /// <param name="buffer">An array of bytes to contain random numbers. </param>
//    //    public override void NextBytes(byte[] buffer)
//    //    {
//    //        base.NextBytes(buffer);
//    //    }

//    //    #endregion


//    //}  // class RandomSystem



//}
