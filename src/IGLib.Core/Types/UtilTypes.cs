
#nullable enable

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
        public static bool IsNumberCollection<NumType, CollectionElementType>(IList<CollectionElementType>? collection)
            where NumType : unmanaged
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
        public static bool IsNumberCollectionOf<NumType, CollectionElementType>(IEnumerable<CollectionElementType>? collection)
            where NumType : unmanaged
        {
            if (collection == null)
            {
                return true;
            }
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

        #region UnUsed

#if false

        public static bool IsIntCollectionOf<CollectionElementType>(IList<CollectionElementType>? collection)
        { return IsNumberCollection<int, CollectionElementType>(collection); }

        public static bool IsLongCollection<CollectionElementType>(IList<CollectionElementType>? collection)
        { return IsNumberCollection<long, CollectionElementType>(collection); }

        public static bool IsDoubleCollection<CollectionElementType>(IList<CollectionElementType>? collection)
        { return IsNumberCollection<double, CollectionElementType>(collection); }

        public static bool IsIntCollection<CollectionElementType>(IEnumerable<CollectionElementType>? collection)
        { return IsNumberCollectionOf<int, CollectionElementType>(collection); }

        public static bool IsLongCollection<CollectionElementType>(IEnumerable<CollectionElementType>? collection)
        { return IsNumberCollectionOf<long, CollectionElementType>(collection); }

        public static bool IsDoubleCollection<CollectionElementType>(IEnumerable<CollectionElementType>? collection)
        { return IsNumberCollectionOf<double, CollectionElementType>(collection); }

#endif   // if false / true / ...

        #endregion UnUsed


        #region SingleTypeConversion

        // WARNING: Method from this region may be removed at some point!

        [Obsolete("Methods for conversion to single specific type may be phased out where this can be done generically.")]
        [RequiresUnreferencedCode("Uses Convert.ChangeType, which may require metadata for dynamic conversions.")]
        public static bool IsConvertibleToInt(this object? value, bool precise = true, IFormatProvider? formatProvider = null)
        {

            formatProvider ??= CultureInfo.InvariantCulture;

            if (value == null) { return false; }
            if (value is int) { return true; }
            if (value is string val) { return int.TryParse(val, NumberStyles.Integer, formatProvider, out int _); }
            if (value is IConvertible convertible)
            {
                try
                {
                    int converted = convertible.ToInt32(formatProvider);
                    if (!precise)
                        return true;
                    // Convert back to the original type and compare.
                    object roundTrip = Convert.ChangeType(converted, value.GetType(), formatProvider);
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
        [Obsolete("Methods for conversion to single specific type may be phased out where this can be done generically.")]
        [RequiresUnreferencedCode("Uses Convert.ChangeType, which may require metadata for dynamic conversions.")]
        public static int ToInt(this object? value, bool precise = false, IFormatProvider? formatProvider = null)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value), $"{nameof(UtilTypes)}.{nameof(ToInt)}: Value is null.");

            formatProvider ??= CultureInfo.InvariantCulture;

            if (value is int ret)
                return ret;

            if (value is string s)
            {
                if (int.TryParse(s, NumberStyles.Integer, formatProvider, out int parsed))
                    return parsed;
                throw new FormatException(
                    $"{nameof(UtilTypes)}.{nameof(ToInt)}: Cannot parse string '{s}' to int using {((CultureInfo)formatProvider).Name} culture.");
            }

            if (value is IConvertible convertible)
            {
                int converted = convertible.ToInt32(formatProvider);

                if (!precise)
                    return converted;

                // Check if conversion was lossless
                try
                {
                    object roundTrip = Convert.ChangeType(converted, value.GetType(), formatProvider);
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

        #endregion SingleTypeConversion




        #region GenericConversionOfBaseTypes

        //public static object? ConvertToType2222(this object? value, Type targetType, bool precise = false, IFormatProvider? provider = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public static object? ConvertTo2222<TargetType>(this object? value, bool precise = false, IFormatProvider? provider = null)
        //    where TargetType : IConvertible
        //{
        //    return ConvertToType2222(value, typeof(TargetType), precise, provider);
        //}



        /// <summary>
        /// Converts the specified <paramref name="value"/> to the specified <paramref name="targetType"/>.
        /// Supports <see cref="Nullable{T}"/> target types. For string inputs, type-safe <c>TryParse</c> paths are used for well-known primitive types to avoid reflection.
        /// Uses <see cref="Convert.ChangeType(object, Type, IFormatProvider?)"/> as a fallback for other IConvertible types.
        /// </summary>
        /// <param name="value">The value to convert. May be <see langword="null"/> if the target type is nullable.</param>
        /// <param name="targetType">The target <see cref="Type"/>. Must implement <see cref="IConvertible"/> (or be a nullable of a type that does).</param>
        /// <param name="precise">
        /// When <see langword="true"/>, ensures that numeric conversions are lossless (no fractional/truncation or other information loss).
        /// When <see langword="false"/>, lossy conversions are allowed. Default is <see langword="false"/>.
        /// </param>
        /// <param name="provider">
        /// Optional <see cref="IFormatProvider"/> (for example, <see cref="CultureInfo.InvariantCulture"/>).  
        /// If <see langword="null"/>, <see cref="CultureInfo.InvariantCulture"/> is used.
        /// </param>
        /// <returns>
        /// The converted value boxed as <see cref="object"/> (or <see langword="null"/> when the target is nullable and <paramref name="value"/> was <see langword="null"/> or an empty/whitespace string).
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="targetType"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="targetType"/> does not implement <see cref="IConvertible"/>, or conversion is not possible (or not precise when <paramref name="precise"/> is true).</exception>
        /// <exception cref="FormatException">Thrown when parsing a string fails for known types (int, double, etc.).</exception>
        public static object? ConvertToType(this object? value, Type targetType, bool precise = false, IFormatProvider? provider = null)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            provider ??= CultureInfo.InvariantCulture;

            // Handle nullable target types
            Type? underlying = Nullable.GetUnderlyingType(targetType);
            bool targetIsNullable = underlying != null;
            Type effectiveTarget = underlying ?? targetType;

            // If source is null: return null for nullable target, otherwise fail
            if (value is null)
                return targetIsNullable ? null : 
                    throw new InvalidOperationException($"{nameof(UtilTypes)}.{nameof(ConvertToType)
                    }: Cannot convert null to non-nullable target type {targetType.Name}.");

            // If effective target doesn't implement IConvertible -> fail
            if (!typeof(IConvertible).IsAssignableFrom(effectiveTarget))
                throw new InvalidOperationException($"{nameof(UtilTypes)}.{nameof(ConvertToType)}: Target type {effectiveTarget.Name} does not implement IConvertible.");

            Type sourceType = value.GetType();

            // If the value already matches the target (assignable), return it (handles reference types)
            if (effectiveTarget.IsAssignableFrom(sourceType))
                return value;

            // If value is string -> use explicit TryParse for known types (no reflection)
            if (value is string s)
            {
                // treat empty/whitespace as null for nullable target types
                if (string.IsNullOrWhiteSpace(s) && targetIsNullable)
                    return null;

                if (effectiveTarget == typeof(int))
                {
                    if (int.TryParse(s, NumberStyles.Integer, provider, out var i)) return i;
                    throw new FormatException($"Cannot parse '{s}' as int.");
                }

                if (effectiveTarget == typeof(long))
                {
                    if (long.TryParse(s, NumberStyles.Integer, provider, out var l)) return l;
                    throw new FormatException($"Cannot parse '{s}' as long.");
                }

                if (effectiveTarget == typeof(short))
                {
                    if (short.TryParse(s, NumberStyles.Integer, provider, out var sh)) return sh;
                    throw new FormatException($"Cannot parse '{s}' as short.");
                }

                if (effectiveTarget == typeof(byte))
                {
                    if (byte.TryParse(s, NumberStyles.Integer, provider, out var by)) return by;
                    throw new FormatException($"Cannot parse '{s}' as byte.");
                }

                if (effectiveTarget == typeof(uint))
                {
                    if (uint.TryParse(s, NumberStyles.Integer, provider, out var ui)) return ui;
                    throw new FormatException($"Cannot parse '{s}' as uint.");
                }

                if (effectiveTarget == typeof(ulong))
                {
                    if (ulong.TryParse(s, NumberStyles.Integer, provider, out var ul)) return ul;
                    throw new FormatException($"Cannot parse '{s}' as ulong.");
                }

                if (effectiveTarget == typeof(float))
                {
                    if (float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out var f)) return f;
                    throw new FormatException($"Cannot parse '{s}' as float.");
                }

                if (effectiveTarget == typeof(double))
                {
                    if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out var d)) return d;
                    throw new FormatException($"Cannot parse '{s}' as double.");
                }

                if (effectiveTarget == typeof(decimal))
                {
                    if (decimal.TryParse(s, NumberStyles.Number, provider, out var dec)) return dec;
                    throw new FormatException($"Cannot parse '{s}' as decimal.");
                }

                if (effectiveTarget == typeof(bool))
                {
                    if (bool.TryParse(s, out var b)) return b;
                    throw new FormatException($"Cannot parse '{s}' as bool.");
                }

                if (effectiveTarget == typeof(DateTime))
                {
                    if (DateTime.TryParse(s, provider, DateTimeStyles.None, out var dt)) return dt;
                    throw new FormatException($"Cannot parse '{s}' as DateTime.");
                }

                if (effectiveTarget == typeof(Guid))
                {
                    if (Guid.TryParse(s, out var g)) return g;
                    throw new FormatException($"Cannot parse '{s}' as Guid.");
                }

                if (effectiveTarget == typeof(char))
                {
                    if (char.TryParse(s, out var ch)) return ch;
                    throw new FormatException($"Cannot parse '{s}' as Char.");
                }

                // Fallback for other IConvertible types: use Convert.ChangeType
                try
                {
                    return Convert.ChangeType(s, effectiveTarget, provider)!;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"{nameof(UtilTypes)}.{nameof(ConvertToType)}: Cannot convert string '{s}' to {effectiveTarget.Name}.", ex);
                }
            }

            // If value implements IConvertible => general conversion via Convert.ChangeType
            if (value is IConvertible)
            {
                try
                {
                    object converted = Convert.ChangeType(value, effectiveTarget, provider)!;

                    if (!precise)
                        return converted;

                    // Precise mode: attempt round-trip to ensure no information loss
                    try
                    {
                        object roundTrip = Convert.ChangeType(converted, sourceType, provider)!;
                        if (Equals(value, roundTrip))
                            return converted;

                        throw new InvalidOperationException($"{nameof(UtilTypes)}.{nameof(ConvertToType)}: Conversion from {sourceType.Name} to {effectiveTarget.Name} loses precision.");
                    }
                    catch (InvalidCastException)
                    {
                        // If round-trip cannot be performed because sourceType is not convertible back, consider it not precise
                        throw new InvalidOperationException($"{nameof(UtilTypes)}.{nameof(ConvertToType)}: Conversion to {effectiveTarget.Name} cannot be verified as precise (round-trip failed).");
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"{nameof(UtilTypes)}.{nameof(ConvertToType)}: Cannot convert {sourceType.Name} to {effectiveTarget.Name}.", ex);
                }
            }

            // Not convertible
            throw new InvalidOperationException($"{nameof(UtilTypes)}.{nameof(ConvertToType)}: Value of type {sourceType.Name} cannot be converted to {effectiveTarget.Name}.");
        }

        /// <summary>
        /// Generic wrapper that calls <see cref="ConvertToType(object?, Type, bool, IFormatProvider?)"/> and casts the result to <typeparamref name="TargetType"/>.
        /// </summary>
        public static TargetType? ConvertTo<TargetType>(this object? value, bool precise = false, IFormatProvider? provider = null)
            where TargetType : IConvertible
        {
            object? result = ConvertToType(value, typeof(TargetType), precise, provider);
            if (result is null) return default;
            return (TargetType)result;
        }


        /// <summary>Returns true if <paramref name="type"/> is a numerical type (supporting arythmetic 
        /// operations), false otherwise.</summary>
        /// <param name="type">Type for which th query is performed.</param>
        private static bool IsNumericType(Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte)
                || type == typeof(short) || type == typeof(ushort)
                || type == typeof(int) || type == typeof(uint)
                || type == typeof(long) || type == typeof(ulong)
                || type == typeof(float) || type == typeof(double)
                || type == typeof(decimal);
        }






        #endregion GenericConversionOfBaseTypes




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
        public static bool IsConvertibleToCollectionOf<ConvertedType, CollectionElementType>(
                IEnumerable<CollectionElementType>? collection)
            where ConvertedType : IConvertible
        {
            if (collection == null)
            {
                return true;
            }
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

        public static void Test()
        {
            object[] ObjectArrayOfIntDouble = [1, 2, 3, 1.11, 2.22];
            // bool isIntCollection = IsIntCollectionOf(ObjectArrayOfIntDouble);
            bool isIntCollection1 = IsNumberCollection<int, object>(ObjectArrayOfIntDouble);
        }

    }

}
