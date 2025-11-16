
#nullable enable

using IG.Lib;
using System;
using System.Collections;
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


        #region BasicUtilities



        /// <summary>Returns true if <paramref name="type"/> is a numerical type (supporting arythmetic 
        /// operations), false otherwise.</summary>
        /// <param name="type">Type for which th query is performed.</param>
        public static bool IsNumericType(Type type)
        {
            return type == typeof(byte) || type == typeof(sbyte)
                || type == typeof(short) || type == typeof(ushort)
                || type == typeof(int) || type == typeof(uint)
                || type == typeof(long) || type == typeof(ulong)
                || type == typeof(float) || type == typeof(double)
                || type == typeof(decimal);
        }


        /// <summary>Returns true if <paramref name="o"/> is a numerical type (supporting arythmetic 
        /// operations), false otherwise.</summary>
        /// <param name="o">Object for which the query is performed.</param>
        public static bool IsOfNumericType(this object? o)
        {
            if (o == null) return false;
            Type type = o.GetType();
            Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            return IsNumericType(underlyingType);
        }

        /// <summary>Checks whether all elements of <paramref name="collection"/> are of numeric types.</summary>
        /// <param name="collection">Collection whose eleements are checked for whether being of numerical types.</param>
        /// <returns>False if the collection <paramref name="collection"/> is null, if it is an empty
        /// collection, if any of its elements are null, or if any of its elements is not of numeric type
        /// (as determined by the <see cref="IsNumericType(Type)"/> method).</returns>
        public static bool IsCollectionOfNumericType(this IEnumerable<object?>? collection)
        {
            return (collection is not null && collection.Any() && collection.All(
                (object? item) => IsOfNumericType(item)));
        }

        public static bool IsCollectionOfNumericType(this IEnumerable? collection)
        {
            if (collection is null || !collection.GetEnumerator().MoveNext())
            { return false; }
            bool ret = false;
            foreach (object? item in collection)
            {
                if (item == null)
                { return false; }
                if (!ret)
                { ret = true; }  // collection has non-null items.
                if (!IsNumericType(item.GetType()))
                { return false; }
            }
            return ret;
        }


        // QUERY TYPE is GENERIC:

        /// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        /// are of the specified type (<typeparamref name="QueryType"/>), false otherwise. It alsso returns 
        /// false if the collection is null or empty or any element is null.</summary>
        /// <typeparam name="QueryType">The type for which elements of the collection are checked.</typeparam>
        /// <typeparam name="ElementType">Declared type of collection elements.</typeparam>
        /// <param name="collection">The collection whose elements are checked for whether they are of a specified type.</param>
        /// <returns>True if all elements are of the specified type and are not null, false otherwise.</returns>
        public static bool IsCollectionOfType<QueryType, ElementType>(this IEnumerable<ElementType>? collection)
        {
            return (collection is not null && collection.Any() && collection.All(
                (ElementType item) => item is QueryType));
        }

        /// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        /// are of the specified type (<typeparamref name="QueryType"/>), false otherwise. It also returns
        /// false if the collection is null or empty or any of its elements is null.</summary>
        /// <typeparam name="QueryType">The type for which elements of the collection are checked.</typeparam>
        /// <param name="collection">The collection whose elements are checked for whether they are of a specified type.</param>
        /// <returns>False if collection is null, if it is empty, or if any element is not of type <typeparamref name="QueryType"/>;
        /// true otherwise (if all elements are of the specified type).</returns>
        public static bool IsCollectionOfType<QueryType>(this IEnumerable<object?>? collection)
        {
            return (collection is not null && collection.Any() && collection.All(
                (object? item) => item is QueryType));
        }

        /// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        /// are of the specified type (<typeparamref name="QueryType"/>), false otherwise. It also returns
        /// false if the collection is null or empty or any of its elements is null.</summary>
        /// <typeparam name="QueryType">The type for which elements of the collection are checked.</typeparam>
        /// <param name="collection">The collection whose elements are checked for whether they are of a specified type.</param>
        /// <returns>False if collection is null, if it is empty, or if any element is not of type <typeparamref name="QueryType"/>;
        /// true otherwise (if all elements are of the specified type).</returns>
        public static bool IsCollectionOfType<QueryType>(this IEnumerable? collection)
        {
            if (collection is null || !collection.GetEnumerator().MoveNext())
            { return false; }
            bool ret = false;
            foreach (object? item in collection)
            {
                if (item == null)
                { return false; }
                if (!ret)
                { ret = true; }  // collection has non-null items.
                if (item is not QueryType)
                { return false; }
            }
            return ret;

            //return (collection is not null && collection.Any() && collection.All(
            //    (object? item) => item is QueryType));
        }

        // QUERY TYPE is NOT GENERIC:


        /// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        /// are of the specified type (<paramref name="queryType"/>), false otherwise. It alsso returns 
        /// false if the collection is null or empty or any of its elements are null.</summary>
        /// <typeparam name="QueryType">The type for which elements of the collection are queried.</typeparam>
        /// <typeparam name="ElementType">Declared type of collection elements.</typeparam>
        /// <param name="collection">The collection whose elements are checked for whether they are of a specified type.</param>
        /// <returns>True if all elements are of the specified type and are not null, false otherwise.</returns>
        public static bool IsCollectionOfType<ElementType>(this IEnumerable<ElementType>? collection, Type queryType)
        {
            return collection is not null && collection.Any() && collection.All(
                (ElementType item) => item != null && queryType.IsAssignableFrom(item.GetType()));
        }

        /// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        /// are of the specified type (<paramref name="queryType"/>), false otherwise. It alsso returns 
        /// false if the collection is null or empty or any of its elements are null.</summary>
        /// <typeparam name="QueryType">The type for which elements of the collection are queried.</typeparam>
        /// <param name="collection">The collection whose elements are checked for whether they are of a specified type.</param>
        /// <returns>True if all elements are of the specified type and are not null, false otherwise.</returns>
        public static bool IsCollectionOfType(this IEnumerable? collection, Type queryType)
        {
            if (collection is null || !collection.GetEnumerator().MoveNext())
            { return false; }
            bool ret = false;
            foreach(object? item in collection)
            {
                if (item == null)
                { return false; }
                if (!ret)
                { ret = true; }  // collection has non-null items.
                if (!queryType.IsAssignableFrom(item.GetType()))
                { return false; }
            }
            return ret;
        }

        ///// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        ///// are of the specified type (<paramref name="queryType"/>), false otherwise. It alsso returns 
        ///// false if the collection is null or empty or any of its elements are null.</summary>
        ///// <typeparam name="QueryType">The type for which elements of the collection are queried.</typeparam>
        ///// <typeparam name="ElementType">Declared type of collection elements.</typeparam>
        ///// <param name="collection">The collection whose elements are checked for whether they are of a specified type.</param>
        ///// <returns>True if all elements are of the specified type and are not null, false otherwise.</returns>
        //public static bool IsCollectionOfType(this IEnumerable<object?>? collection, Type queryType)
        //{
        //    return collection is not null && collection.Any() && collection.All(
        //        (object? item) => item != null && queryType.IsAssignableFrom(item.GetType()));
        //}




        #endregion BasicUtilities



        #region CollectionConversions




        /// <summary>Returns true if all members of the specified collection (<paramref name="collection"/>) 
        /// can be converted to the specified basic type (<typeparamref name="TargetType"/>, which must
        /// implement the <see cref="IConvertible"/> interface) by the 
        /// <see cref="ConvertTo{TargetType}(object?, bool, IFormatProvider?)"/> method.
        /// <para>False is returned if <paramref name="collection"/> is null or empty, if any of its elements
        /// is null, or any of its elements are not convertible to <typeparamref name="TargetType"/> (i.e., the
        /// <see cref="IsConvertibleTo{TargetType}(object?, bool, IFormatProvider?)"/> returns false on those
        /// elements); otherwise, false is returned.</para></summary>
        /// <typeparam name="TargetType">The type for which elements of the collection are queried. It must be a 
        /// basic type implementing <see cref="IConvertible"/> interface, such as bool, int, char, byte, long,
        /// float, double, string, etc.</typeparam>
        /// <param name="collection">The collection whose elements are checked for whether they can be 
        /// converted to the specified type.</param>
        /// <returns>False if <paramref name="collection"/> is null or empty or any element is null or
        /// any element cannot be converted to the specified type (<typeparamref name="TargetType"/>)
        /// by the <see cref="ConvertTo{TargetType}(object?, bool, IFormatProvider?)"/> method.</returns>
        public static bool IsConvertibleToCollectionOf<TargetType>(IEnumerable? collection, 
                bool precise = false, IFormatProvider? formatProvider = null)
            where TargetType : IConvertible
        {
            if (collection is null || !collection.GetEnumerator().MoveNext())
            { return false; }
            bool ret = false;
            foreach (object? item in collection)
            {
                if (item == null)
                {
                    return false;  // if null, we cannot reason about cnvertibility
                }
                if (!ret)
                {
                    ret = true;  // collection has a non-null element; if all elements OK, true will be returned 
                }
                if (!IsConvertibleTo<TargetType>(item, precise, formatProvider))
                {
                    return false;
                }
            }
            return ret;
        }

        /// <summary>Converts elements of <paramref name="collection"/> to the specified type (<typeparamref name="TargetType"/>)
        /// and returns a list of converted elements, or throws exception if any element conversion fails.
        /// Returns null if the collection is null or empty.
        /// <para>Elements are converted by <see cref="ConvertTo{TargetType}(object?, bool, IFormatProvider?)"/>.</para></summary>
        /// <typeparam name="TargetType">Type to whch elements of the collection are converted. Must
        /// implement the <see cref="IConvertible"/> interface.</typeparam>
        /// <param name="collection">Collection whose elements are converted.</param>
        /// <param name="precise">If true then conversions are only performed when precise conversions are
        /// possible, otherwise exception is thrown. This is important for conversions form floating point
        /// to integer types or to floating point types with smaller precision.</param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static List<TargetType?>? ConvertToListOf<TargetType>(IEnumerable? collection,
                bool precise = false, IFormatProvider? formatProvider = null)
            where TargetType : IConvertible
        {
            if (collection is null || !collection.GetEnumerator().MoveNext())
            { return null; }
            List<TargetType?>? returned = null;
            foreach (object? item in collection)
            {
                if (item == null)
                {
                    return null;  // if null, we cannot reason about cnvertibility
                }
                TargetType? convertedElement = UtilTypes.ConvertTo<TargetType>(item, precise, formatProvider);
                if (returned == null)
                {
                    // Create the list if not yet created - as late as possible, to avoid unnecessary creation
                    returned = new List<TargetType?>();
                }
                returned.Add(convertedElement);
            }
            return returned;
        }





        #endregion CollectionConversions




        #region GenericConversionOfBaseTypes


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
        /// <param name="provider">Optional <see cref="IFormatProvider"/>. If <see langword="null"/>, then 
        /// <see cref="CultureInfo.InvariantCulture"/> is used.</param>
        /// <returns>The converted value boxed as <see cref="object"/> (or <see langword="null"/> when the target is nullable
        /// and <paramref name="value"/> was <see langword="null"/> or an empty/whitespace string).</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="targetType"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="targetType"/> does not implement <see cref="IConvertible"/>, or conversion is not possible (or not precise when <paramref name="precise"/> is true).</exception>
        /// <exception cref="FormatException">Thrown when parsing a string fails for known types (int, double, etc.).</exception>
        /// <remarks><para>Tests for this method (may not show up in CodeLens due to indirect calls):</para>
        /// <para>* IGLib.Types.Tests.UtilTypesTests.ConvertTo_Generic_WorksCorrectlyFor_Int(...)</para>
        /// <para>* etc. (tested for some other types)</para>
        /// <para>Tests of generic method also test correctness of this method because they rely on it.</para></remarks>
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

        /// <summary>Returns true if the specified object (<paramref name="value"/>) can be converted to the specific
        /// type (<paramref name="targetType"/>), false otherwise.
        /// <para>Parameters have the same meaning as in <see cref="ConvertToType(object?, Type, bool, IFormatProvider?)"/>.</para></summary>
        /// <remarks>Tests for this method:
        /// <para>* IGLib.Types.Tests.UtilTypesTests.ConvertTo_Generic_WorksCorrectlyFor_Int(...)</para>
        /// <para>* etc. (tested for some other types)</para>
        /// <para>Tests of generic method also test correctness of this method because they rely on it.</para></remarks>
        /// <remarks><para>Tests for this method (may not show up in CodeLens):</para>
        /// <para>* IGLib.Types.Tests.UtilTypesTests.ConvertTo_Generic_WorksCorrectlyFor_Int(...)</para>
        /// <para>* etc. (tested for some other types)</para>
        /// <para>Tests of generic method also test correctness of this method because they rely on it.</para></remarks>
        public static bool IsConvertibleToType(this object? value, Type targetType, bool precise = false, IFormatProvider? formatProvider = null)
        {
            try
            {
                object? converted = ConvertToType(value, targetType, precise, formatProvider);
            }
            catch
            {  
                return false; 
            }
            // If no exception was thrown then ovject is convertivle to TargetType
            return true;
        }

        /// <summary>
        /// Generic wrapper that calls <see cref="ConvertToType(object?, Type, bool, IFormatProvider?)"/> and casts the result to <typeparamref name="TargetType"/>.
        /// </summary>
        /// <remarks><para>Tests for this method (may not show up in CodeLens due to indirect calls):</para>
        /// <para>* IGLib.Types.Tests.UtilTypesTests.ConvertTo_Generic_WorksCorrectlyFor_Int(...)</para>
        /// <para>* etc. (tested for some other types)</para>
        /// <para>Tests of generic method also test correctness of this method because they rely on it.</para></remarks>
        public static TargetType? ConvertTo<TargetType>(this object? value, bool precise = false, IFormatProvider? provider = null)
            where TargetType : IConvertible
        {
            object? result = ConvertToType(value, typeof(TargetType), precise, provider);
            if (result is null) return default;
            return (TargetType)result;
        }

        /// <summary>Returns true if the specified object (<paramref name="value"/>) can be converted to the specific
        /// type (<typeparamref name="TargetType"/>), false otherwise.
        /// <para>Parameters have the same meaig as in <see cref="ConvertTo{TargetType}(object?, bool, IFormatProvider?)"/>.</para></summary>
        /// <remarks><para>Tests for this method (may not show up in CodeLens):</para>
        /// <para>* IGLib.Types.Tests.UtilTypesTests.ConvertTo_Generic_WorksCorrectlyFor_Int(...)</para>
        /// <para>* etc. (tested for some other types)</para></remarks>
        public static bool IsConvertibleTo<TargetType>(this object? value, bool precise = false, IFormatProvider? formatProvider = null)
            where TargetType : IConvertible
        {
            try
            {
                TargetType? converted = ConvertTo<TargetType>(value, precise, formatProvider);
            }
            catch
            {  
                return false; 
            }
            // If no exception was thrown then ovject is convertivle to TargetType
            return true;
        }


        #endregion GenericConversionOfBaseTypes



    }

}
