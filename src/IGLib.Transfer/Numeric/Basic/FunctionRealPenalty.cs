// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{

    /// <summary>Interface that must be satisfied by penalty functions.
    /// Penalty functions have small values where argument is less than 0, and grow 
    /// fast where their argument is positive.</summary>
    /// $A Igor xx;
    public interface IRealFunctionPenalty : IRealFunction
    {

        /// <summary>Whether penalty function has finite support (meaning that it is 0
        /// for all arguments less than some specific value).</summary>
        bool IsFiniteSupport { get; }

        /// <summary>True if absolute value is differentiable, false otherwise.
        /// Differentiability of absolute value at 0 is important for penalty functions used for
        /// equality constraints.</summary>
        bool IsAbsoluteDifferentiable { get; }

        /// <summary>True if absolute value is twoce differentiable, false otherwise.
        /// Differentiability of absolute value at 0 is important for penalty functions used for
        /// equality constraints.</summary>
        bool IsAbsoluteTwiceDifferentiable { get; }

        /// <summary>Whether maximal value for which penalty function is zero can be set.</summary>
        bool CanSetMaxZero { get; }

        /// <summary>Whether the gap can be set.</summary>
        bool CanSetBarrierLength { get; }

        /// <summary>Whether the height can be set.</summary>
        bool CanSetBarrierHeight { get; }

        /// <summary>Maximal value for which penalty function is zero.</summary>
        double MaxZero { get; set; }

        /// <summary>Gap - characteristic length of transition area on which penalty function 
        /// grows for about (or sometimes exactly, especially in case of finite support) 
        /// characteristic height.</summary>
        double BarrierLength { get; set; }

        /// <summary>Characteristic heitht of transition area, usually value of the penalty function
        /// at the end of transition area.</summary>
        double BarrierHeight { get; set; }

    }  // interface IRealFunctionPenalty


    

    // TODO: Implement power functions that are products of power and exponential function!
    public partial class Func
    {


        // Power penalty function:


        /// <summary>Creates and returns a new power penalty function consisting of sticked together 
        /// constant zero-valued function and a power function with positive integer exponent.
        /// Formula: hh*((x - xx0)/dd)^pp
        /// where: 
        ///   dd:  Characteristic barrier length. Length of the interval on which function grows
        ///          from 0 to characteristic height.  
        ///   hh:  Characteristic barrier height. Value of the function at transition point
        ///          plus characteristic length.
        ///   xx0: Transition point where function starts to be non-zero.
        ///   pp:  Power. Must be greater than 0; for 2 first derivative is continuous
        /// in transition points, for 3 second derivative is also continuous, etc.
        /// </summary>
        /// <param name="barrierLength">Characteristic barrier length. Length of the interval on which function grows
        /// from 0 to characteristic height.</param>
        /// <param name="barrierHeght">Characteristic barrier height. Value of the function at transition point
        /// plus characteristic length.</param>
        /// <param name="power">Power. Must be greater than 0, for 2 first derivative is continuous
        /// in transition points, for 3 second derivative is also continuous, etc.</param>
        public static PenaltyPower GetPenaltyPower(double barrierLength, double barrierHeght, int power)
        {
            return new PenaltyPower(barrierLength, barrierHeght, power);
        }

        /// <summary>Creates and returns a new power penalty function consisting of sticked together 
        /// constant zero-valued function and a power function with positive integer exponent.
        /// Formula: hh*((x - xx0)/dd)^pp
        /// where: 
        ///   dd:  Characteristic barrier length. Length of the interval on which function grows
        ///          from 0 to characteristic height.  
        ///   hh:  Characteristic barrier height. Value of the function at transition point
        ///          plus characteristic length.
        ///   xx0: Transition point where function starts to be non-zero.
        ///   pp:  Power. Must be greater than 0; for 2 first derivative is continuous
        /// in transition points, for 3 second derivative is also continuous, etc.
        /// </summary>
        /// <param name="barrierLength">Characteristic barrier length. Length of the interval on which function grows
        /// from 0 to characteristic height.</param>
        /// <param name="barrierHeght">Characteristic barrier height. Value of the function at transition point
        /// plus characteristic length.</param>
        /// <param name="zeroEnd">Transition point where function starts to be non-zero.</param>
        /// <param name="power">Power. Must be greater than 0, for 2 first derivative is continuous
        /// in transition points, for 3 second derivative is also continuous, etc.</param>
        public static PenaltyPower GetPenaltyPower(double barrierLength, double barrierHeght, 
                double zeroEnd, int power)
        {
            return new PenaltyPower(barrierLength, barrierHeght, zeroEnd, power);
        }


        /// <summary>Penalty function consisting of sticked together constant zero-valued
        /// function and a power function with positive integer exponent.
        /// Formula: hh*((x - xx0)/dd)^pp
        /// where: 
        ///   dd:  Characteristic barrier length. Length of the interval on which function grows
        ///          from 0 to characteristic height.  
        ///   hh:  Characteristic barrier height. Value of the function at transition point
        ///          plus characteristic length.
        ///   xx0: Transition point where function starts to be non-zero.
        ///   pp:  Power. Must be greater than 0; for 2 first derivative is continuous
        /// in transition points, for 3 second derivative is also continuous, etc.
        /// </summary>
        public class PenaltyPower : RealFunction, IRealFunctionPenalty, IRealFunction
        {

            #region Initialization

            /// <summary>Creates a new penalty function consisting of sticked together constant zero-valued
            /// function and a power function with positive integer exponent.
            /// Transition pint where function starts to be non-zero is at negative characteristic length.</summary>
            /// <param name="length">Characteristic barrier length. Length of the interval on which function grows
            /// from 0 to characteristic height.</param>
            /// <param name="height">Characteristic barrier height. Value of the function at transition point
            /// plus characteristic length.</param>
            /// <param name="power">Power. Must be greater than 0, for 2 first derivative is continuous
            /// in transition points, for 3 second derivative is also continuous, etc.</param>
            public PenaltyPower(double length, double height, int power)
                : this(length, height, -length /* zeroEnd */, power)
            { }

            /// <summary>Creates a new penalty function consisting of sticked together constant zero-valued
            /// function and a power function with positive integer exponent.</summary>
            /// <param name="length">Characteristic barrier length. Length of the interval on which function grows
            /// from 0 to characteristic height.</param>
            /// <param name="height">Characteristic barrier height. Value of the function at transition point
            /// plus characteristic length.</param>
            /// <param name="zeroEnd">Transition point where function starts to be non-zero.</param>
            /// <param name="power">Power. Must be greater than 0, for 2 first derivative is continuous
            /// in transition points, for 3 second derivative is also continuous, etc.</param>
            public PenaltyPower(double length, double height, double zeroEnd, int power)
            {
                this.dd = length;
                this.hh = height;
                this.xx0 = zeroEnd;
                this.pp = power;
            }


            #endregion Initialization


            #region Data

            double
                dd,  // characteristic length on which function grows from about 0 to characteristic height
                hh,  // characteristic height
                xx0,  // maximal x where function value is still 0
                pp;  // power

            #endregion Data


            #region IRealFunctionPenalty Members


            /// <summary>Whether penalty function has finite support (meaning that it is 0
            /// for all arguments less than some specific value).</summary>
            public bool IsFiniteSupport
            { get { return true; } }

            /// <summary>True if absolute value is differentiable, false otherwise.
            /// Differentiability of absolute value at 0 is important for penalty functions used for
            /// equality constraints.</summary>
            public bool IsAbsoluteDifferentiable
            { get { if (pp >= 2 && xx0 == 0) return true; else return false; } }

            /// <summary>True if absolute value is twoce differentiable, false otherwise.
            /// Differentiability of absolute value at 0 is important for penalty functions used for
            /// equality constraints.</summary>
            public bool IsAbsoluteTwiceDifferentiable
            { get { if (pp >= 3 && xx0 == 0) return true; else return false; } }

            /// <summary>Whether maximal value for which penalty function is zero can be set.</summary>
            public bool CanSetMaxZero
            { get { return true; } }

            /// <summary>Whether the gap can be set.</summary>
            public bool CanSetBarrierLength
            { get { return true; } }

            /// <summary>Whether the height can be set.</summary>
            public bool CanSetBarrierHeight
            { get { return true; } }

            /// <summary>Maximal value for which penalty function is zero.</summary>
            public double MaxZero
            {
                get { return xx0; }
                set { xx0 = value; }
            }

            /// <summary>Gap - characteristic length of transition area on which penalty function 
            /// grows for about (or sometimes exactly, especially in case of finite support) 
            /// characteristic height.</summary>
            public double BarrierLength
            {
                get { return dd; }
                set { dd = value; }
            }

            /// <summary>Characteristic heitht of transition area, usually value of the penalty function
            /// at the end of transition area.</summary>
            public double BarrierHeight
            {
                get { return hh; }
                set { hh = value; }
            }

            #endregion IRealFunctionPenalty Members


            #region IRealFunction Stuff

            protected override double RefValue(double x)
            {
                if (x <= xx0)
                    return 0;
                else
                    return hh * Math.Pow(((x - xx0) / dd), pp);
            }

            public override bool ValueDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for value defined for function " + Name + ".");
                }
            }

            protected override double RefDerivative(double x)
            {
                if (x <= xx0)
                    return 0;
                else return (hh * pp * Math.Pow(((x - xx0) / dd), (-1 + pp)) ) / dd;
            }

            public override bool DerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for derivative defined for function " + Name + ".");
                }
            }

            protected override double RefSecondDerivative(double x)
            {
                if (x <= xx0)
                    return 0;
                else
                {
                    return (hh * (-1 + pp) * pp * Math.Pow(((x - xx0) / dd), (-2 + pp))) / (dd * dd) ;
                }
            }

            public override bool SecondDerivativeDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for second derivative defined for function " + Name + ".");
                }
            }


            protected override double RefDerivative(double x, int order)
            {
                if (order == 0)
                    return RefValue(x);
                else if (order == 1)
                    return RefDerivative(x);
                else if (order == 2)
                    return RefSecondDerivative(x);
                else
                    throw new NotImplementedException("Defivatives higher than 2nd order are not defined for function "
                            + Name + ".");
            }

            public override bool HigherDerivativeDefined(int order)
            {
                if (order <= 0)
                    throw new ArgumentException("Can not get a flag for defined derivative of nonpositive order. Function: "
                        + Name + ".");
                if (order > 2)
                    return false;
                else
                    return true;
            }

            protected internal override void setHighestDerivativeDefined(int order)
            {
                throw new InvalidOperationException("Can not set value of highest derivative defined. Function:  "
                  + Name + ".");
            }


            /// <summary>Returns integral for the function where x0 = 0.</summary>
            private double Int0(double tt)
            {
                if (tt < 0)
                    return 0;
                else
                    return hh * (tt * Math.Pow((tt / dd), pp)) / (1 + pp);
            }
            

            protected override double RefIntegral(double x)
            {
                return Int0(x - xx0);
            }

            public override bool IntegralDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for integral defined for function " + Name + ".");
                }
            }

            protected override double RefInverse(double y)
            {
                if (y < 0)
                    throw new ArgumentException("Inverse not defined for y<0, function: " + Name + ".");
                else if (y == 0)
                    return xx0;
                else
                    return xx0 + dd * Math.Pow((y / hh), 1.0/pp);
            }

            public override bool InverseDefined
            {
                get { return true; }
                protected internal set
                {
                    throw new InvalidOperationException(
                        "Can not set a flag for inverse defined for function " + Name + ".");
                }
            }


            #endregion IRealFunction Stuff


        }  // class PenaltyPower



    }  // partial class Func

}
