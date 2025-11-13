
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
        public static bool IsNumberCollectionOf<NumType, CollectionElementType>(IEnumerable<CollectionElementType>? collection)
            where NumType: unmanaged
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

        public static object? ConvertToType(this object? value, Type targetType, bool precise = false, IFormatProvider? provider = null)
        {
            throw new NotImplementedException();
        }

        public static object? ConvertTo<TargetType>(this object? value, bool precise = false, IFormatProvider? provider = null)
            where TargetType : IConvertible
        {
            return ConvertToType(value, typeof(TargetType), precise, provider);
        }



        /// <summary>Converts the specified <paramref name="value"/> to the target type <typeparamref name="TargetType"/>.</summary>
        /// <typeparam name="TargetType">The destination type that implements <see cref="IConvertible"/>.</typeparam>
        /// <param name="value">The value to convert. May be any <see cref="object"/> implementing <see cref="IConvertible"/>,
        /// which means basic types like byte, char, int, double, string, long, float, unsigned int, and similar.</param>
        /// <param name="precise">
        /// When <see langword="true"/>, the conversion must not lose precision; conversions such as 2.3 → 2 will throw an exception.
        /// When <see langword="false"/>, lossy conversions are allowed. The default is <see langword="false"/>.
        /// </param>
        /// <param name="provider">
        /// An optional <see cref="IFormatProvider"/> (such as <see cref="CultureInfo.InvariantCulture"/>) that controls formatting 
        /// for string and numeric conversions. If <see langword="null"/>, <see cref="CultureInfo.InvariantCulture"/> is used.</param>
        /// <returns>The converted value as <typeparamref name="TargetType"/>.</returns>
        /// <remarks>
        /// <para>
        /// This method performs conversion using type-safe and trim-friendly logic:
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If <paramref name="value"/> is already of <typeparamref name="TargetType"/>, it is returned directly.
        /// </description></item>
        /// <item><description>
        /// If <paramref name="value"/> is a <see cref="string"/>, built-in <c>TryParse</c> methods are used for well-known primitive types (e.g. <see cref="int"/>, <see cref="double"/>, <see cref="bool"/>).
        /// </description></item>
        /// <item><description>
        /// For other <see cref="IConvertible"/> types, <see cref="Convert.ChangeType(object, Type, IFormatProvider?)"/> is used.
        /// </description></item>
        /// <item><description>
        /// When <paramref name="precise"/> is <see langword="true"/>, numeric conversions that lose information will throw <see cref="InvalidOperationException"/>.
        /// </description></item>
        /// </list>
        /// <para>This implementation does not use reflection and is safe for trimming and AOT compilation.</para>
        /// Safe for trimming and AOT compilation when converting between built-in types that implement <see cref="IConvertible"/>.
        /// Converting arbitrary user-defined types with <see cref="Convert.ChangeType(object, Type, IFormatProvider?)"/> 
        /// may not be safe for trimming.</remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
        /// <exception cref="FormatException">Thrown when parsing a string fails.</exception>
        /// <exception cref="InvalidOperationException">Thrown when conversion is not supported or cannot be performed precisely.</exception>
        public static TargetType ConvertTo_1111<TargetType>(this object? value, bool precise = false, IFormatProvider? provider = null)
            where TargetType : IConvertible
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value), $"{nameof(UtilTypes)}.{nameof(ConvertTo)}: Value is null.");

            provider ??= CultureInfo.InvariantCulture;

            Type targetType = typeof(TargetType);
            Type sourceType = value.GetType();

            if (Nullable.GetUnderlyingType(targetType) is Type underlying)
            {
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    return default!;
                targetType = underlying;
            }


            // 1. Already correct type:
            if (value is TargetType tVal)
                return tVal;

            // 2️. String input → switch on known target types
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

            // 3️. General numeric or convertible case
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
