// Copyright © Igor Grešovnik (2008 - present); license:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

// This file provides alternative implementation of the HashCode.Combine{T1}(T1) method for
// legacy .NET Framework and earlier versions of .NET Coree (below .NET 8).

#if NET8_0_OR_GREATER
using System;

namespace IGLib.Base
{

    /// <summary>Contains methods that combines hashes of multipe objects into a single hash.
    /// <para>In .NET 8 or greater, this just calls <see cref="HashCode.Combine{T1}(T1)"/>.
    /// </para></summary>
    public static class HashCodeHelper
    {
        public static int Combine<T1>(T1 value1)
        {
            return HashCode.Combine(value1);
        }

        public static int Combine<T1, T2>(T1 value1, T2 value2)
        {
            return HashCode.Combine(value1, value2);
        }

        public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            return HashCode.Combine(value1, value2, value3);
        }

        // Add more overloads as needed
    }
}
#else
using System;

namespace IGLib.Base
{

    /// <summary>Contains methods that combines hashes of multipe objects into a single hash.
    /// <para>In .NET 8 or greater, this just calls `HashCode.Combine{T1}(T1)`, but for lower
    /// versions (including .NET Framework), the class provides its own implementation.
    /// </para></summary>
    public static class HashCodeHelper
    {
        public static int Combine<T1>(T1 value1)
        {
            return value1?.GetHashCode() ?? 0;
        }

        public static int Combine<T1, T2>(T1 value1, T2 value2)
        {
            int hash1 = value1?.GetHashCode() ?? 0;
            int hash2 = value2?.GetHashCode() ?? 0;

            return CombineHashes(hash1, hash2);
        }

        public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            int hash1 = value1?.GetHashCode() ?? 0;
            int hash2 = value2?.GetHashCode() ?? 0;
            int hash3 = value3?.GetHashCode() ?? 0;

            return CombineHashes(hash1, hash2, hash3);
        }

        // Add more overloads as needed

        private static int CombineHashes(params int[] hashes)
        {
            int hash = 17;

            foreach (int h in hashes)
            {
                hash = hash * 31 + h;
            }

            return hash;
        }
    }
}
#endif