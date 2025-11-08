

using IG.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Types.Extensions
{

    /// <summary>Provides some simple utilities for type conversions. These utilities do 
    /// NOT rely on reflection.</summary>
    public static class UtilTypes
    {

        /// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        /// are of the specified primitive (unmanaged, most commonly numeric) type (<typeparamref name="NumType"/>).
        /// <para>For example, if the collection elements are of declared type object and the first type parameter
        /// is <see cref="int"/> then true is returned if and only if all elements of the collection are actually
        /// of type int and are NOT null.</para></summary>
        /// <typeparam name="NumType">The type for which elements of the collection are checked. It must
        /// be an unmanaged type such as bool, int, char, byte, long, float, double, etc.</typeparam>
        /// <typeparam name="CollectionElementType">Declared type of collection elements.</typeparam>
        /// <param name="collection">The collection whose elements are checked for whether they are of a specified type.</param>
        /// <returns>True if all elements are of the specified type and are not null, false otherwise.</returns>
        public static bool IsNumberCollection<NumType, CollectionElementType>(IList<CollectionElementType> collection)
            where NumType: unmanaged
        {
            if (collection == null || collection.Count == 0)
                return false;
            for (int i = 0; i < collection.Count; ++i)
            {
                CollectionElementType item = collection[i];
                if (item == null)
                {
                    return false;  // if number types, they cannot be null
                }
                if (item is not NumType)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        /// are of the specified primitive (unmanaged, most commonly numeric) type (<typeparamref name="NumType"/>).
        /// <para>For example, if the collection elements are of declared type object and the first type parameter
        /// is <see cref="int"/> then true is returned if and only if all elements of the collection are actually
        /// of type int and are NOT null.</para></summary>
        /// <typeparam name="NumType">The type for which elements of the collection are checked. It must
        /// be an unmanaged type such as bool, int, char, byte, long, float, double, etc.</typeparam>
        /// <typeparam name="CollectionElementType">Declared type of collection elements.</typeparam>
        /// <param name="collection">The collection whose elements are checked for whether they are of a specified type.</param>
        /// <returns>True if all elements are of the specified type and are not null, false otherwise.</returns>
        public static bool IsNumberCollectionOf<NumType, CollectionElementType>(IEnumerable<CollectionElementType> collection)
            where NumType: unmanaged
        {
            foreach (CollectionElementType item in collection)
            {
                if (item == null)
                {
                    return false;  // if number types, they cannot be null
                }
                if (item is not NumType)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsIntCollectionOf<CollectionElementType>(IList<CollectionElementType> collection)
        { return IsNumberCollection<int, CollectionElementType>(collection); }

        public static bool IsLongCollection<CollectionElementType>(IList<CollectionElementType> collection)
        { return IsNumberCollection<long, CollectionElementType>(collection); }

        public static bool IsDoubleCollection<CollectionElementType>(IList<CollectionElementType> collection)
        { return IsNumberCollection<double, CollectionElementType>(collection); }

        public static bool IsIntCollection<CollectionElementType>(IEnumerable<CollectionElementType> collection)
        { return IsNumberCollectionOf<int, CollectionElementType>(collection); }

        public static bool IsLongCollection<CollectionElementType>(IEnumerable<CollectionElementType> collection)
        { return IsNumberCollectionOf<long, CollectionElementType>(collection); }

        public static bool IsDoubleCollection<CollectionElementType>(IEnumerable<CollectionElementType> collection)
        { return IsNumberCollectionOf<double, CollectionElementType>(collection); }

        public static int ToInt_Old(this object value, bool precise = false)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (value is int i)
                return i;
            if (value is string s)
                return int.Parse(s);
            if (value is IConvertible convertible)
            {
                int converted = convertible.ToInt32(null);
                if (!precise)
                    return converted;
                try
                {
                    // Convert back to the original type and compare.
                    object roundTrip = Convert.ChangeType(converted, value.GetType());
                    if (value.Equals(roundTrip))
                        return converted;
                }
                catch
                {
                    // Conversion back failed → definitely not precise
                }
            }
            throw new InvalidOperationException(
                $"Cannot precisely convert value {value} of type {value.GetType().Name} to int.");
        }


        public static bool IsConvertibleToInt(this object value, bool precise = true)
        {
            if (value == null) { return false; }
            if (value is int) { return true; }
            if (value is string val) { return int.TryParse(val, out int _); }
            if (value is IConvertible convertible)
            {
                try
                {
                    int converted = convertible.ToInt32(null);
                    if (!precise)
                        return true;
                    // Convert back to the original type and compare.
                    object roundTrip = Convert.ChangeType(converted, value.GetType());
                    if (value.Equals(roundTrip))
                        return true;
                }
                catch
                {
                    // Conversion back failed → definitely not precise
                }
            }
            return false;
        }


        /// <summary>
        /// Converts a value to an integer. If <paramref name="precise"/> is true, 
        /// only lossless conversions are accepted.
        /// Uses InvariantCulture for string and IConvertible conversions.
        /// </summary>
        [RequiresUnreferencedCode("Uses Convert.ChangeType, which may require metadata for dynamic conversions.")]
        public static int ToInt(this object value, bool precise = false, IFormatProvider? provider = null)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value), $"{nameof(UtilTypes)}.{nameof(ToInt)}: Value is null.");

            provider ??= CultureInfo.InvariantCulture;

            if (value is int ret)
                return ret;

            if (value is string s)
            {
                if (int.TryParse(s, NumberStyles.Integer, provider, out int parsed))
                    return parsed;
                throw new FormatException(
                    $"{nameof(UtilTypes)}.{nameof(ToInt)}: Cannot parse string '{s}' to int using {((CultureInfo)provider).Name} culture.");
            }

            if (value is IConvertible convertible)
            {
                int converted = convertible.ToInt32(provider);

                if (!precise)
                    return converted;

                // Check if conversion was lossless
                try
                {
                    object roundTrip = Convert.ChangeType(converted, value.GetType(), provider);
                    if (Equals(value, roundTrip))
                        return converted;
                }
                catch
                {
                    // Some types can't be round-tripped; fall through to exception
                }
            }

            throw new InvalidOperationException(
                $"{nameof(UtilTypes)}.{nameof(ToInt)}: Value {value} of type {value.GetType().Name} cannot be converted to int{(precise ? " precisely" : "")}.");
        }



        ///// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        ///// are of the specified primitive (unmanaged, most commonly numeric) type (<typeparamref name="ConvertedType"/>).
        ///// <para>For example, if the collection elements are of declared type object and the first type parameter
        ///// is <see cref="int"/> then true is returned if and only if all elements of the collection are actually
        ///// of type int and are NOT null.</para></summary>
        ///// <typeparam name="ConvertedType">The type for which elements of the collection are checked. It must
        ///// be an unmanaged type such as bool, int, char, byte, long, float, double, etc.</typeparam>
        ///// <typeparam name="CollectionElementType">Declared type of collection elements.</typeparam>
        ///// <param name="collection">The collection whose elements are checked for whether they are of a specified type.</param>
        ///// <returns>True if all elements are of the specified type and are not null, false otherwise.</returns>
        //public static bool IsConvertibleToCollectionOf<ConvertedType, CollectionElementType>(IList<CollectionElementType> collection)
        //    where ConvertedType : IConvertible
        //{
        //    if (collection == null || collection.Count == 0)
        //        return false;
        //    for (int i = 0; i < collection.Count; ++i)
        //    {
        //        CollectionElementType item = collection[i];
        //        if (item == null)
        //        {
        //            return false;  // if number types, they cannot be null
        //        }
        //        if (item is not ConvertedType)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        /// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        /// are of the specified primitive (unmanaged, most commonly numeric) type (<typeparamref name="ConvertedType"/>).
        /// <para>For example, if the collection elements are of declared type object and the first type parameter
        /// is <see cref="int"/> then true is returned if and only if all elements of the collection are actually
        /// of type int and are NOT null.</para></summary>
        /// <typeparam name="ConvertedType">The type for which elements of the collection are checked. It must
        /// be an unmanaged type such as bool, int, char, byte, long, float, double, etc.</typeparam>
        /// <typeparam name="CollectionElementType">Declared type of collection elements.</typeparam>
        /// <param name="collection">The collection whose elements are checked for whether they are of a specified type.</param>
        /// <returns>True if all elements are of the specified type and are not null, false otherwise.</returns>
        public static bool IsConvertibleToCollectionOf<ConvertedType, CollectionElementType>(IEnumerable<CollectionElementType> collection)
            where ConvertedType : IConvertible
        {
            foreach (CollectionElementType item in collection)
            {
                if (item == null)
                {
                    return false;  // if number types, they cannot be null
                }
                if (item is not ConvertedType)
                {
                    return false;
                }
            }
            return true;
        }








        public static TargetType ConvertTo<TargetType>(
            this object value,
            bool precise = false,
            IFormatProvider? provider = null)
            where TargetType : IConvertible
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value), $"{nameof(UtilTypes)}.{nameof(ConvertTo)}: Value is null.");

            provider ??= CultureInfo.InvariantCulture;

            Type targetType = typeof(TargetType);
            Type sourceType = value.GetType();

            // 1️⃣ Already correct type
            if (value is TargetType tVal)
                return tVal;

            // 2️⃣ String input → switch on known target types
            if (value is string s)
            {
                object parsed = targetType switch
                {
                    Type t when t == typeof(int)
                        => int.TryParse(s, NumberStyles.Integer, provider, out int i) ? i
                           : throw new FormatException($"Cannot parse '{s}' as int."),

                    Type t when t == typeof(double)
                        => double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out double d) ? d
                           : throw new FormatException($"Cannot parse '{s}' as double."),

                    Type t when t == typeof(float)
                        => float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out float f) ? f
                           : throw new FormatException($"Cannot parse '{s}' as float."),

                    Type t when t == typeof(decimal)
                        => decimal.TryParse(s, NumberStyles.Number, provider, out decimal dec) ? dec
                           : throw new FormatException($"Cannot parse '{s}' as decimal."),

                    Type t when t == typeof(bool)
                        => bool.TryParse(s, out bool b) ? b
                           : throw new FormatException($"Cannot parse '{s}' as bool."),

                    Type t when t == typeof(DateTime)
                        => DateTime.TryParse(s, provider, DateTimeStyles.None, out DateTime dt) ? dt
                           : throw new FormatException($"Cannot parse '{s}' as DateTime."),

                    Type t when t == typeof(Guid)
                        => Guid.TryParse(s, out Guid g) ? g
                           : throw new FormatException($"Cannot parse '{s}' as Guid."),

                    _ => Convert.ChangeType(s, targetType, provider)!
                };

                return (TargetType)parsed;
            }

            // 3️⃣ General numeric or convertible case
            if (value is IConvertible convertible)
            {
                try
                {
                    object converted = Convert.ChangeType(convertible, targetType, provider)!;

                    if (!precise)
                        return (TargetType)converted;

                    // Precision check for numeric types
                    if (IsNumericType(targetType) && IsNumericType(sourceType))
                    {
                        double dOriginal = Convert.ToDouble(value, provider);
                        double dConverted = Convert.ToDouble(converted, provider);
                        if (Math.Abs(dOriginal - dConverted) < double.Epsilon)
                            return (TargetType)converted;

                        throw new InvalidOperationException(
                            $"{nameof(UtilTypes)}.{nameof(ConvertTo)}: Conversion from {sourceType.Name} to {targetType.Name} loses precision.");
                    }

                    // Round-trip check
                    object roundTrip = Convert.ChangeType(converted, sourceType, provider)!;
                    if (Equals(value, roundTrip))
                        return (TargetType)converted;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"{nameof(UtilTypes)}.{nameof(ConvertTo)}: Cannot convert {sourceType.Name} to {targetType.Name}.", ex);
                }
            }

            throw new InvalidOperationException(
                $"{nameof(UtilTypes)}.{nameof(ConvertTo)}: Value {value} of type {sourceType.Name} cannot be converted to {targetType.Name}{(precise ? " precisely" : "")}.");
        }

        private static bool IsNumericType(Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte)
                || type == typeof(short) || type == typeof(ushort)
                || type == typeof(int) || type == typeof(uint)
                || type == typeof(long) || type == typeof(ulong)
                || type == typeof(float) || type == typeof(double)
                || type == typeof(decimal);
        }











        public static void Test()
        {
            object[] ObjectArrayOfIntDouble = [1, 2, 3, 1.11, 2.22];
            bool isIntCollection = IsIntCollectionOf(ObjectArrayOfIntDouble);
            bool isIntCollection1 = IsNumberCollection<int, object>(ObjectArrayOfIntDouble);
        }

    }

}
