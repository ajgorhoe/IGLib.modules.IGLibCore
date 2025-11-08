// Copyright © Igor Grešovnik (2008 - present). License:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// WARNING:
// This source files was copied from iglib_base;
// TODO:
// Consolidate later!
// Remark: The file should not change with respct to original, it shoulld be possible to just change the reference.

namespace IG.Num
{

    /// <summary>Gaussian random number generator.</summary>
    public class RandomGaussian
    {
        void Test()
        {
            IRandomGenerator rnd = RandomGenerator.Global;
        }

        public RandomGaussian(IRandomGenerator random = null)
        {
            _random = random ?? new RandomGeneratorSystem();
        }

        #region Global

        protected static object _lockStatic = new object();

        protected static RandomGaussian _global;

        /// <summary>Global Gaussian random generator.</summary>
        public static RandomGaussian Global
        {
            get
            {
                lock (_lockStatic)
                {
                    if (_global == null)
                        _global = new RandomGaussian(RandomGenerator.Global);
                    return _global;
                }
            }

        }

        #endregion Global

        private bool _hasDeviate;
        private double _storedDeviate;
        private readonly IRandomGenerator _random;

        /// <summary>
        /// Obtains normally (Gaussian) distributed random numbers, using the Box-Muller
        /// transformation.  This transformation takes two uniformly distributed deviates
        /// within the unit circle, and transforms them into two independently
        /// distributed normal deviates.
        /// </summary>
        /// <param name="mean">The mean of the distribution.  Default is zero.</param>
        /// <param name="sigma">The standard deviation of the distribution.  Default is one.</param>
        /// <returns></returns>
        public double NextGaussian(double mean = 0, double sigma = 1)
        {
            if (sigma <= 0)
                throw new ArgumentOutOfRangeException("sigma", "Must be greater than zero.");

            if (_hasDeviate)
            {
                _hasDeviate = false;
                return _storedDeviate * sigma + mean;
            }

            double v1, v2, rSquared;
            do
            {
                // two random values between -1.0 and 1.0
                v1 = 2 * _random.NextDouble() - 1;
                v2 = 2 * _random.NextDouble() - 1;
                rSquared = v1 * v1 + v2 * v2;
                // ensure within the unit circle
            } while (rSquared >= 1 || rSquared == 0);

            // calculate polar tranformation for each deviate
            var polar = Math.Sqrt(-2 * Math.Log(rSquared) / rSquared);
            // store first deviate
            _storedDeviate = v2 * polar;
            _hasDeviate = true;
            // return second deviate
            return v1 * polar * sigma + mean;
        }
    }



}
