
#nullable enable

using FluentAssertions;
using IGLib.Commands;
using IGLib.Tests.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Xunit;
using Xunit.Abstractions;
// Enable use of only a specific type without stating namespace:
using UtilTypes = IGLib.Types.Extensions.UtilTypes;

#if false  // please leave this block such that 
using static IGLib.Types.Extensions.UtilTypes;
using IGLib.Types.Extensions;
#endif

namespace IGLib.Types.Tests
{

    /// <summary>Tests of type utilities from <see cref="IGLib.Types.Extensions.UtilTypesSingle"/>, mainly type conversion tests.
    /// <para>The "using static" directive is used, such that utility methods can be called without
    /// stating the namespace and containing class, but they cannot be called as extension methods.</para>
    /// <para>This contains the most complete tests of <see cref="IGLib.Types.Extensions.UtilTypesSingle"/></para>
    /// <para>For tests with extension methods, see <see cref="UtilTypes_ExtensionMetodsTests"/></para></summary>
    public class UtilTypesTests : TestBase<UtilTypesTests>
    {


        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> property.</summary>
        /// <param name=""></param>
        public UtilTypesTests(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        { }


#if false  // change condition to true when testing effects of using directives!
        /// <summary>This contains a code block that demonstrates in what ways the methods from <see cref="UtilTypes"/>
        /// can be called, and what #using directives one needs for this. Part of the code should
        /// cause compiler errors.</summary>
        protected static void TestSyntax()
        {
            object o = (double) 2.0;

            // Needs:
            // using  IGLib.Types.Extensions
            bool isConvertible1 = IsConvertibleTo<int>(o);

            // Needs:
            // using static IGLib.Types.Extensions.UtilTypes;
            bool isConvertible2 = o.IsConvertibleTo<int>();

            // Needes either of:
            // using IGLib.Types.Extensions;  // but this also enables extension methods, which is not always wanted
            // using UtilTypes = IGLib.Types.Extensions.UtilTypes;
            bool isConvertible3 = UtilTypes.IsConvertibleTo<int>(o);

        }
#endif


        #region BasicUtilitiesTests


        [Theory]
        // Numeric types:
        [InlineData(typeof(byte), true)]
        [InlineData(typeof(sbyte), true)]
        [InlineData(typeof(short), true)]
        [InlineData(typeof(ushort), true)]
        [InlineData(typeof(int), true)]
        [InlineData(typeof(uint), true)]
        [InlineData(typeof(long), true)]
        [InlineData(typeof(ulong), true)]
        [InlineData(typeof(float), true)]
        [InlineData(typeof(double), true)]
        [InlineData(typeof(decimal), true)]
        // Types that are not numeric:
        [InlineData(typeof(DateTime), false)]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(char), false)]
        [InlineData(typeof(UtilTypes), false)]
        [InlineData(typeof(GenericCommandBase), false)]
        [InlineData(typeof(IGenericCommand), false)]
        [InlineData(typeof(System.IO.FileAccess), false)]
        [InlineData(typeof(DayOfWeek), false)]
        protected void IsNumericType_WorksCorrectly(Type type, bool shouldBeNumeric)
        {
            Console.WriteLine("Checking whether the specified type is numeric type.");
            Console.WriteLine($"Type checked: {type.Name}; should be numeric: {shouldBeNumeric}");
            // Act:
            bool isNumeric = UtilTypes.IsNumericType(type);
            Console.WriteLine($"Returned from {nameof(UtilTypes.IsNumericType)}: {isNumeric}");
            // Assert:
            isNumeric.Should().Be(shouldBeNumeric, because: $"this type is {(isNumeric ? "" : "NOT")} a numeric type");
        }

        /// <summary>Additional test dataset for <see cref="UtilTypes.IsOfNumericType(object?)"/></summary>
        public static TheoryData<object?, bool> Dataset_IsOfNumericType => new()
        {
            { (ushort?) 143, true },
            { (double?) 1.02e-26, true },
            { (double?) null, false }, // null should alwatys result in false
            { (decimal) 999.99m, true},
            { (decimal) -5.34m, true },
            { (System.IO.FileAccess) System.IO.FileAccess.Read, false },
            { new DateTime(2024, 1, 1), false },
        };

        [Theory]
        [MemberData(nameof(Dataset_IsOfNumericType))]
        // Null objects should return fale:
        [InlineData(null, false)]
        // Numeric types:
        [InlineData((byte)35, true)]
        [InlineData((sbyte)-47, true)]
        [InlineData((short)-87, true)]
        [InlineData((ushort)127, true)]
        [InlineData((int)-34, true)]
        [InlineData((uint)8932, true)]
        [InlineData((long)-879947543, true)]
        [InlineData((ulong)8483956343, true)]
        [InlineData((float)-1.23e6, true)]
        [InlineData((double)6.02e-23, true)]
        //[InlineData((decimal) 4.24m, true)]
        // Types that are not numeric:
        [InlineData((string)"ABC", false)]
        [InlineData((char)'x', false)]
        protected void IsOfNumericType_WorksCorrectly(object? o, bool shouldBeNumeric)
        {
            Console.WriteLine("Checking whether the specified object is of numeric type.");
            Console.WriteLine($"Object checked: {o}, type: {o?.GetType().Name ?? "<null>"}, should be numeric: {shouldBeNumeric}");
            // Act:
            bool isNumeric = UtilTypes.IsOfNumericType(o);
            Console.WriteLine($"Returned from {nameof(UtilTypes.IsNumericType)}: {isNumeric}");
            // Assert:
            isNumeric.Should().Be(shouldBeNumeric, because: $"this object is {(isNumeric ? "" : "NOT")} of numeric type");
        }


        IEnumerable<object> x = new object[] { (int)1, (int)2, (int)3 };

        /// <summary>Test datases for <see cref="UtilTypes.IsCollectionOfNumericType(IEnumerable?)"/> </summary>
        public static TheoryData<IEnumerable<object?>?, bool> Dataset_IsCollectionOfNumericType => 
            new TheoryData<IEnumerable<object?>?, bool>
            {
                { [(int)1, (int)2, (int)3], true },
                { [(double)1.23, (int)2 ], true },  // mixed numeric type elements
                { [], false },  // empty enumerable
                { null!, false },  // null enumerable
                { ["str", (int) 1, (int) 2], false },  // mixed non-numeric and numeric elements
                { [(int) 1, (int) 2, "str"], false },  // mixed non-numeric and numeric elements
                { [(int) 1, "str", (int) 2], false },  // mixed non-numeric and numeric elements
                { [null!, (int) 1, (int) 2], false },  // includes null elements
                { [(int) 1, (int) 2, null!], false },  // includes null elements
                { [(int) 1, null!, (int) 2], false },  // includes null elements
                { new List<object>() { 1, 2, 3 }, true },
                { new List<object>() {  }, false },
                { new List<object>() { 1, 2, "xy" }, false },
                { new List<object>() { 1, 2, null! }, false },
                { new List<object>() { 1, 2, 3, 4.55 }, true },
            };

        [Theory]
        [MemberData(nameof(Dataset_IsCollectionOfNumericType))]
        protected void IsCollectionOfNumericType_WorksCorrectly(IEnumerable<object?>? enumerable, bool shouldBeNumeric)
        {
            Console.WriteLine("Checking whether the specified collection contains only elements of numeric types.");
            Console.WriteLine("Collection checked: ");
            if (enumerable == null)
            {
                Console.WriteLine("  null");
            }
            else if (!enumerable.Any())
            {
                Console.WriteLine("  empty collection");
            } else
            {
                int i = 0;
                foreach (object? item in enumerable)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}");
                    ++i;
                }
            }
            Console.WriteLine($"Collection should be numeric: {shouldBeNumeric}");
            // Act:
            bool isNumeric = UtilTypes.IsCollectionOfNumericType(enumerable);
            Console.WriteLine($"Returned from {nameof(UtilTypes.IsCollectionOfNumericType)}: {isNumeric}");
            // Assert:
            isNumeric.Should().Be(shouldBeNumeric, because: $"this collection is {(isNumeric ? "" : "NOT")} of numeric types");
        }

        [Theory]
        [MemberData(nameof(Dataset_IsCollectionOfNumericType))]
        protected void IsCollectionOfNumericType_NongenericCollection_WorksCorrectly(IEnumerable? enumerable, bool shouldBeNumeric)
        {
            Console.WriteLine("Checking whether the specified collection contains only elements of numeric types.");
            Console.WriteLine("Collection checked: ");
            if (enumerable == null)
            {
                Console.WriteLine("  null");
            }
            else if (!enumerable.GetEnumerator().MoveNext())
            {
                Console.WriteLine("  empty collection");
            }
            else
            {
                int i = 0;
                foreach (object? item in enumerable)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}");
                    ++i;
                }
            }
            Console.WriteLine($"Collection should be numeric: {shouldBeNumeric}");
            // Act:
            bool isNumeric = UtilTypes.IsCollectionOfNumericType(enumerable);
            Console.WriteLine($"Returned from {nameof(UtilTypes.IsCollectionOfNumericType)}: {isNumeric}");
            // Assert:
            isNumeric.Should().Be(shouldBeNumeric, because: $"this collection is {(isNumeric ? "" : "NOT")} of numeric types");
        }


        public static TheoryData<IEnumerable<object?>?, bool> Dataset_IsCollectionOf_Int =>
            new TheoryData<IEnumerable<object?>?, bool>
            {
                { [(int)1, (int)2, (int)3], true },
                { [(double)1.23, (int)2 ], false },  // mixed numeric type elements
                { [(double)1, (int)2.45 ], false },  // mixed numeric type elements
                { [], false },  // empty enumerable
                { null, false },  // null enumerable
                { ["str", (int) 1, (int) 2], false },  // mixed non-numeric and numeric elements
                { [(int) 1, (int) 2, "str"], false },  // mixed non-numeric and numeric elements
                { [(int) 1, "str", (int) 2], false },  // mixed non-numeric and numeric elements
                { [null!, (int) 1, (int) 2], false },  // includes null elements
                { [(int) 1, (int) 2, null!], false },  // includes null elements
                { [(int) 1, null!, (int) 2], false },  // includes null elements
                { new List<object?>() { -25, 433, 9238, -5 }, true },
                { new List<object?>() {  }, false },
                { new List<object?>() { 1, 2, "xy" }, false },
                { new List<object?>() { 1, 2, null! }, false },
                { new List<object?>() { 1, 2, 3, 4.55 }, false },
            };


        [Theory]
        [MemberData(nameof(Dataset_IsCollectionOf_Int))]
        protected void IsCollectionOfType_Generic2Param_WorksCorrectlyForInt(IEnumerable<object?>? enumerable, bool shouldBeOfSpecifiedType)
        {
            Console.WriteLine("Checking whether the specified collection contains only elements of the specific genric type (int).");
            Console.WriteLine("Collection checked: ");
            if (enumerable == null)
            {
                Console.WriteLine("  null");
            }
            else if (!enumerable.Any())
            {
                Console.WriteLine("  empty collection");
            }
            else
            {
                int i = 0;
                foreach (object? item in enumerable)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}");
                    ++i;
                }
            }
            Console.WriteLine($"Collection should be of type int: {shouldBeOfSpecifiedType}");
            // Act:
            bool isOfSpecifiedType = UtilTypes.IsCollectionOfType<int, object?>(enumerable);
            Console.WriteLine($"Returned from {nameof(UtilTypes.IsCollectionOfType)}: {isOfSpecifiedType}");
            // Assert:
            isOfSpecifiedType.Should().Be(shouldBeOfSpecifiedType, because: $"this collection is {(isOfSpecifiedType ? "" : "NOT")} of the specified type");
        }


        [Theory]
        [MemberData(nameof(Dataset_IsCollectionOf_Int))]
        protected void IsCollectionOfType_Generic_WorksCorrectlyForInt(IEnumerable<object?>? enumerable, bool shouldBeOfSpecifiedType)
        {
            Console.WriteLine("Checking whether the specified collection contains only elements of the specific genric type (int).");
            Console.WriteLine("Collection checked: ");
            if (enumerable == null)
            {
                Console.WriteLine("  null");
            }
            else if (!enumerable.Any())
            {
                Console.WriteLine("  empty collection");
            }
            else
            {
                int i = 0;
                foreach (object? item in enumerable)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}");
                    ++i;
                }
            }
            Console.WriteLine($"Collection should be of type int: {shouldBeOfSpecifiedType}");
            // Act:
            bool isOfSpecifiedType = UtilTypes.IsCollectionOfType<int>(enumerable);
            Console.WriteLine($"Returned from {nameof(UtilTypes.IsCollectionOfType)}: {isOfSpecifiedType}");
            // Assert:
            isOfSpecifiedType.Should().Be(shouldBeOfSpecifiedType, because: $"this collection is {(isOfSpecifiedType ? "" : "NOT")} of the specified type");
        }



        [Theory]
        [MemberData(nameof(Dataset_IsCollectionOf_Int))]
        protected void IsCollectionOfType_Generic_NongenericCollection_WorksCorrectlyForInt(IEnumerable<object?>? enumerable, bool shouldBeOfSpecifiedType)
        {
            Console.WriteLine("Checking whether the specified collection contains only elements of the specific genric type (int).");
            Console.WriteLine("Collection checked: ");
            if (enumerable == null)
            {
                Console.WriteLine("  null");
            }
            else if (!enumerable.Any())
            {
                Console.WriteLine("  empty collection");
            }
            else
            {
                int i = 0;
                foreach (object? item in enumerable)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}");
                    ++i;
                }
            }
            Console.WriteLine($"Collection should be of type int: {shouldBeOfSpecifiedType}");
            // Act:
            bool isOfSpecifiedType = UtilTypes.IsCollectionOfType<int>(enumerable);
            Console.WriteLine($"Returned from {nameof(UtilTypes.IsCollectionOfType)}: {isOfSpecifiedType}");
            // Assert:
            isOfSpecifiedType.Should().Be(shouldBeOfSpecifiedType, because: $"this collection is {(isOfSpecifiedType ? "" : "NOT")} of the specified type");
        }




        // Query type id NOT generic

        [Theory]
        [MemberData(nameof(Dataset_IsCollectionOf_Int))]
        protected void IsCollectionOfType_GenericElementType_WorksCorrectlyForInt(IEnumerable<object?>? enumerable, bool shouldBeOfSpecifiedType)
        {
            Console.WriteLine("Checking whether the specified collection contains only elements of the specific genric type (int).");
            Console.WriteLine("Collection checked: ");
            if (enumerable == null)
            {
                Console.WriteLine("  null");
            }
            else if (!enumerable.Any())
            {
                Console.WriteLine("  empty collection");
            }
            else
            {
                int i = 0;
                foreach (object? item in enumerable)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}");
                    ++i;
                }
            }
            Console.WriteLine($"Collection should be of type int: {shouldBeOfSpecifiedType}");
            // Act:
            bool isOfSpecifiedType = UtilTypes.IsCollectionOfType<object?>(enumerable, typeof(int));
            Console.WriteLine($"Returned from {nameof(UtilTypes.IsCollectionOfType)}: {isOfSpecifiedType}");
            // Assert:
            isOfSpecifiedType.Should().Be(shouldBeOfSpecifiedType, because: $"this collection is {(isOfSpecifiedType ? "" : "NOT")} of the specified type");
        }


        [Theory]
        [MemberData(nameof(Dataset_IsCollectionOf_Int))]
        protected void IsCollectionOfType_WorksCorrectly(IEnumerable? enumerable, bool shouldBeOfSpecifiedType)
        {
            Console.WriteLine("Checking whether the specified collection contains only elements of the specific genric type (int).");
            Console.WriteLine("Collection checked: ");
            if (enumerable == null)
            {
                Console.WriteLine("  null");
            }
            else if (!enumerable.GetEnumerator().MoveNext())
            {
                Console.WriteLine("  empty collection");
            }
            else
            {
                int i = 0;
                foreach (object? item in enumerable)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}");
                    ++i;
                }
            }
            Console.WriteLine($"Collection should be of type int: {shouldBeOfSpecifiedType}");
            // Act:
            bool isOfSpecifiedType = UtilTypes.IsCollectionOfType(enumerable, typeof(int));
            Console.WriteLine($"Returned from {nameof(UtilTypes.IsCollectionOfType)}: {isOfSpecifiedType}");
            // Assert:
            isOfSpecifiedType.Should().Be(shouldBeOfSpecifiedType, because: $"this collection is {(isOfSpecifiedType ? "" : "NOT")} of the specified type");
        }









        #endregion BasicUtilitiesTests



        #region CollectionTypeConversions

        // GENERIC definition of TARGET TYPE


        public static Type TypeInt { get; } = typeof(int);
        public static Type TypeString { get; } = typeof(string);
        public static Type TypeDouble { get; } = typeof(double);


        /// <summary>Test parameters dataset for testing conversion of collection elements to
        /// list of type int (can be used for other typees, too, because of redundant parameters).
        /// <para>Dataset parameters:</para>
        /// <para>1. Collection to be converted (IEnumerable?)</para>
        /// <para>2. Target type (redundant when testing generic methods) (Type)</para>
        /// <para>3. Whether PRECISE conversion is required (bool)</para>
        /// <para>4. Whether conversion is expected to be be SUCCESFUL (bool)</para>
        /// <para>5. Expected values of converted elemennts (object[])</para></summary>
        public static TheoryData<IEnumerable?, Type, bool, bool, int[]?> Dataset_CovertToListOf_Precise_Int =>
            new ()
            {
                // PRECISE conversions are required:
                { (object?[])[(int)1, (int)2, (int)3], TypeInt, true, true, [1, 2, 3] },
                { (object?[])[(double)1.23, (int)2 ], TypeInt, true, false, null },  // mixed numeric type elements
                { (object?[])[(double)1, (double)2.45 ], TypeInt, true, false, null },  // mixed numeric type elements
                //{ (object?[])[], TypeInt, true, false, null },  // empty enumerable
                { null, TypeInt, true, false, null },  // null enumerable
                { (object?[])["str", (int) 1, (int) 2], TypeInt, true, false, null },  // mixed non-numeric and numeric elements
                { (object?[])[(int) 1, (int) 2, "str"], TypeInt, true, false, null },  // mixed non-numeric and numeric elements
                { (object?[])[(int) 1, "str", (int) 2], TypeInt, true, false, null },  // mixed non-numeric and numeric elements
                //{ (object?[])[null!, (int) 1, (int) 2], TypeInt, true, false, null },  // includes null elements
                { (object?[])[(int) 1, (int) 2, null!], TypeInt, true, false, null },  // includes null elements
                { (object?[])[(int) 1, null!, (int) 2], TypeInt, true, false, null },  // includes null elements
                { new List<object?>() { -25, 433, 9238, -5 }, TypeInt, true, true, [-25, 433, 9238, -5] },
                //{ new List<object?>() {  }, TypeInt, true, false, null },
                { new List<object?>() { 1, 2, "xy" }, TypeInt, true, false, null },
                { new List<object?>() { 1, 2, null! }, TypeInt, true, false, null },
                { new List<object?>() { 1, 2, 3, 4.55 }, TypeInt, true, false, null },
                // APPROXIMATE conversions are ALLOWED:
            };





        [Theory]
        [MemberData(nameof(Dataset_CovertToListOf_Precise_Int))]
        protected void ConvertToListOf_Precise_Int_WorksCorrectly(IEnumerable? enumerable, Type targetType, 
            bool precise, bool shouldBeConvertible, int[]? expectedResult)
        {
            Console.WriteLine("Testing conversion of an object collection to a list of elements of the specified type.");
            Console.WriteLine("Collection converted: ");
            if (enumerable == null)
            {
                Console.WriteLine("  null");
            }
            else if (!enumerable.GetEnumerator().MoveNext())
            {
                Console.WriteLine("  empty collection");
            }
            else
            {
                int i = 0;
                foreach (object? item in enumerable)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}, type: {item?.GetType().Name ?? "/"}");
                    ++i;
                }
            }
            Console.WriteLine($"Target element type after conversion: {targetType.Name}");
            Console.WriteLine($"Conversion should be possible: {shouldBeConvertible}");
            Console.WriteLine("Expected conversion result: ");
            if (expectedResult == null)
            {
                Console.WriteLine("  null");
            }
            else if (expectedResult.Length == 0)
            {
                Console.WriteLine("  empty array");
            }
            else
            {
                int i = 0;
                foreach (object? item in expectedResult)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}, type: {item?.GetType().Name ?? "/"}");
                    ++i;
                }
            }
            // Act:
            List<int>? result = null;
            bool wasConverionSuccessful = true;
            try
            {
                result = UtilTypes.ConvertToListOf<int>(enumerable, precise: precise);
                Console.WriteLine($"Result of conversion via {nameof(UtilTypes.ConvertToListOf)}:");
                if (result == null)
                {
                    Console.WriteLine("  null");
                }
                else if (result.Count == 0)
                {
                    Console.WriteLine("  empty array");
                }
                else
                {
                    int i = 0;
                    foreach (object? item in result)
                    {
                        Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}, type: {item?.GetType().Name ?? "/"}");
                        ++i;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.GetType().Name} thrown when trying to convert a collection of objects.\n  Message:{ex.Message}");
                wasConverionSuccessful = false;
            }
            // Assert:
            wasConverionSuccessful.Should().Be(shouldBeConvertible, because: $"collection elements should {
                (shouldBeConvertible ? "" : "NOT")} be convertible to the specified type.");
            if (wasConverionSuccessful && expectedResult != null && expectedResult.Length > 0)
            {
                result.Should().NotBeNull();
                result.Count.Should().Be(expectedResult.Length, because: $"number of elements after conversion should be {
                    expectedResult.Length}.");
                if (expectedResult != null && expectedResult.Length >= 0)
                {
                    for (int i = 0; i < expectedResult!.Length; ++i)
                    {

                        result![i].Should().Be(expectedResult[i], because: $"element {i} should be {
                            (expectedResult[i] == null ? "null" : expectedResult[i])} but it is {
                            (result[i] == null ? "null":result[i])}.");
                    }
                }
            }
        }














        // TARGET TYPE defined AS Type PARAMETER


        [Theory]
        // Dataset for conversion to int:
        [MemberData(nameof(Dataset_CovertToListOf_Precise_Int))]
        protected void ConvertToListOfType_Precise_Int_WorksCorrectly(IEnumerable? enumerable, Type targetType,
            bool precise, bool shouldBeConvertible, int[]? expectedResult)
        {
            Console.WriteLine("Testing conversion of an object collection to a list of elements of the specified type.");
            Console.WriteLine("Collection converted: ");
            if (enumerable == null)
            {
                Console.WriteLine("  null");
            }
            else if (!enumerable.GetEnumerator().MoveNext())
            {
                Console.WriteLine("  empty collection");
            }
            else
            {
                int i = 0;
                foreach (object? item in enumerable)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}, type: {item?.GetType().Name ?? "/"}");
                    ++i;
                }
            }
            Console.WriteLine($"Target element type after conversion: {targetType.Name}");
            Console.WriteLine($"Conversion should be possible: {shouldBeConvertible}");
            Console.WriteLine("Expected conversion result: ");
            if (expectedResult == null)
            {
                Console.WriteLine("  null");
            }
            else if (expectedResult.Length == 0)
            {
                Console.WriteLine("  empty array");
            }
            else
            {
                int i = 0;
                foreach (object? item in expectedResult)
                {
                    Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}, type: {item?.GetType().Name ?? "/"}");
                    ++i;
                }
            }
            // Act:
            List<object?>? result = null;
            bool wasConverionSuccessful = true;
            try
            {
                result = UtilTypes.ConvertToListOfType(enumerable, targetType, precise: precise);
                Console.WriteLine($"Result of conversion via {nameof(UtilTypes.ConvertToListOf)}:");
                if (result == null)
                {
                    Console.WriteLine("  null");
                }
                else if (result.Count == 0)
                {
                    Console.WriteLine("  empty array");
                }
                else
                {
                    int i = 0;
                    foreach (object? item in result)
                    {
                        Console.WriteLine($"  [{i}] : {item?.ToString() ?? "null"}, type: {item?.GetType().Name ?? "/"}");
                        ++i;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().Name} thrown when trying to convert a collection of objects.\n  Message:{ex.Message}");
                wasConverionSuccessful = false;
            }
            // Assert:
            wasConverionSuccessful.Should().Be(shouldBeConvertible, because: $"collection elements should {(shouldBeConvertible ? "" : "NOT")} be convertible to the specified type.");
            if (wasConverionSuccessful && expectedResult != null && expectedResult.Length > 0)
            {
                result.Should().NotBeNull();
                result.Count.Should().Be(expectedResult.Length, because: $"number of elements after conversion should be {expectedResult.Length}.");
                if (expectedResult != null && expectedResult.Length >= 0)
                {
                    for (int i = 0; i < expectedResult!.Length; ++i)
                    {

                        result![i].Should().Be(expectedResult[i], because: $"element {i} should be {(expectedResult[i] == null ? "null" : expectedResult[i])} but it is {(result[i] == null ? "null" : result[i])}.");
                    }
                }
            }
        }







        #endregion CollectionTypeConversions



        #region GeneralConversion_Generic_Tests


        /// <summary>Common base method for testing both the method for conversion to another basic type (<see cref="UtilTypes.ConvertTo{TargetType}(object?, bool, IFormatProvider?)"/>)
        /// and the method that tells whether an object can be converted to another basic type (<see cref="UtilTypes.IsConvertibleTo{TargetType}(object?, bool, IFormatProvider?)"/>).
        /// Parameter <paramref name="testConversion"/> specifies whether the conversion method (when true), 
        /// or the method that verifies whether conversion is possible, is tested (<see cref="UtilTypes.IsConvertibleTo{TargetType}(object?, bool, IFormatProvider?)"/>).</summary>
        /// <param name="testConversion">Tf true then the conversion method is tested (<see cref="UtilTypes.ConvertTo{TargetType}(object?, bool, IFormatProvider?)"/>),
        /// while if false, the method that tels whether the object can be converted to target type is tested.</param>
        /// <param name="converted">The object that is to be converted, or for which we query whether the object can be 
        /// converted to targat type.</param>
        /// <param name="precise">Whether conversion should be precise. For exemple, when true, and double is converted to int,
        /// conversion can succeed only when the double number is an integer (does not have the decimal part).</param>
        /// <param name="expectedResult">The expected result of conversion.</param>
        /// <param name="failureExpected">Whether the conversion should fail.</param>
        /// <param name="comment">Optional comment, which will be output and helps in checking and interpreting the results.</param>
        protected void ConvertToTypeGenericTestsCommon<TargetType>(bool testConversion, object? converted, bool precise, int expectedResult,
            bool failureExpected = false, string? comment = null)
            where TargetType : IConvertible
        {
            if (testConversion)
            {
                Console.WriteLine($"Testing conversion of an object to type int:");
            }
            else
            {
                Console.WriteLine($"Testing the method telling whether an object can be converted to int:");
            }
            Console.WriteLine($"Object to be converted: {converted}, type: {converted?.GetType().Name}");
            Console.WriteLine($"Precise conversion required: {(precise ? "yes" : "no")}.");
            Console.WriteLine("");
            if (failureExpected)
            {
                Console.WriteLine("Conversion is expected to fail.");
            }
            else
            {
                Console.WriteLine($"Expected result of conversion: {expectedResult}");
            }
            if (!string.IsNullOrEmpty(comment))
            {
                Console.WriteLine($"Additional comment: \n  {comment}");
            }
            Console.WriteLine("");
            // Arrange:
            // Act:
            bool exceptionThrown = false;
            Console.WriteLine("Result of conversion:");
            try
            {
                TargetType? result = UtilTypes.ConvertTo<TargetType>(converted, precise: precise);
                Console.WriteLine($"Result of conversion: {result}, expected: {expectedResult}");
                if (testConversion)
                {
                    result.Should().Be(expectedResult);
                }
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Console.WriteLine($"{ex.GetType().Name} thrown: {ex.Message}");
            }
            if (testConversion)
            {
                if (failureExpected)
                {
                    exceptionThrown.Should().BeTrue(because: "this conversion attempt is expected to throw an exception");
                }
                else
                {
                    exceptionThrown.Should().BeFalse(because: "this conversion should be performed without exceptions");
                }
            }
            if (!testConversion)
            {
                bool isConvertible = false;
                bool isConvertibleThrewException = false;
                try
                {
                    isConvertible = UtilTypes.IsConvertibleTo<TargetType>(converted, precise: precise);
                    Console.WriteLine($"{nameof(UtilTypes.IsConvertibleTo)} returned {isConvertible}.");
                    if (!testConversion)
                    {
                        isConvertible.Should().Be(!failureExpected, because: $"tesult of {nameof(UtilTypes.IsConvertibleTo)} is expected to be {!failureExpected}");
                    }
                }
                catch (Exception)
                {
                    isConvertibleThrewException = true;
                }
                if (!testConversion)
                {
                    isConvertibleThrewException.Should().BeFalse(because: "IsConvertible should not throw an exception in any case");
                }
            }
        }


        #region GeneralConversion_Generic_Tests__For_Int


        /// <summary>Tests whether <see cref="UtilTypes.ConvertTo{TargetType}(object?, bool, IFormatProvider?)"/> 
        /// works correctly for generic target type <see cref="int"/>.</summary>
        /// <param name="converted">Object that is converted to target type.</param>
        /// <param name="precise">Whether conversion should be precise (in this case, conversion fails for values that
        /// cannot be converted precisely, e.g. double to int where the original double has nonzero decimal part).</param>
        /// <param name="expectedResult">Expected result of conversion, agains which assertions are made.</param>
        /// <param name="failureExpected">Whether conversion should fail for the current input data.</param>
        /// <param name="comment">Optional comment (written to output for user inforrmation).</param>
        [Theory]
        // ****
        // Conversion of string:
        // ****
        [InlineData("2", false, 2)]
        [InlineData("-2", false, -2)]
        [InlineData("25_000", false, 25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
        [InlineData("-25_000", false, -25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
        [InlineData("25,000", false, 25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
        [InlineData("-25,000", false, -25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
        // String presentations are floating point instead of integers, conversion throws exception:
        [InlineData("2.0", false, 2, true, "floating point, not an int")]
        [InlineData("-2.0", false, -2, true, "floating point, not an int")]
        // Converson of string, OUT OF RANGE:
        [InlineData("2147483649", false, 0, true, "greater than max int")]
        [InlineData("-2147483650", false, 0, true, "less than min int")]
        // String representations are hexadecimal numbers - not supported:
        [InlineData("0x1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        [InlineData("1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        // ****
        // Conversion of double:
        // ****
        [InlineData((double)2.0, false, 2)]
        [InlineData((double)-2.0, false, -2)]
        // Conversion of double, inprecise - ROUNDED:
        [InlineData((double)2.4999, false, 2, false, "should be rounded below (precise not requested)")]
        [InlineData((double)2.5001, false, 3, false, "should be rounded above (precise not requested)")]
        [InlineData((double)-2.4999, false, -2, false, "should be rounded above (precise not requested)")]
        [InlineData((double)-2.5001, false, -3, false, "should be rounded below (precise not requested)")]
        // Converson of double, OUT OF RANGE:
        [InlineData((double)int.MaxValue + 2.0, false, 0, true, "greater than max int")]
        [InlineData((double)int.MinValue - 2.0, false, 0, true, "less than min int")]
        // Conversion of double, precise conversion required:
        [InlineData((double)2.0, true, 2, false, "precise conversion requested, can be done")]
        [InlineData((double)-2.0, true, -2, false, "precise conversion requested, can be done")]
        [InlineData((double)2.1, true, 2, true, "precise conversion requested, cannot be done because of decimal part")]
        [InlineData((double)-2.1, true, -2, true, "precise conversion requested, cannot be done because of decimal part")]
        // ****
        // Conversion of long:
        // ****
        [InlineData((long)2, false, 2)]
        [InlineData((long)-2, false, -2)]
        // Converson of long, OUT OF RANGE:
        [InlineData((long)int.MaxValue + 2, false, 0, true, "greater than max int")]
        [InlineData((long)int.MinValue - 2, false, 0, true, "less than min int")]
        // ****
        // Conversion of unsigned int:
        // ****
        [InlineData((uint)2, false, 2)]
        // Converson of unsigned int, OUT OF RANGE:
        [InlineData((uint)int.MaxValue + 2, false, 0, true, "greater than max int")]
        // Conversion of bool:
        // ****
        [InlineData((bool)true, false, 1)]
        [InlineData((bool)false, false, 0)]
        // ****
        // Conversion of char:
        // ****
        [InlineData((char)'x', false, (int)'x', false, "conversion of ASCII character to int")]
        [InlineData((char)2836, false, 2836, false, "conversion of non-ASCII character to int")]
        protected void ConvertTo_Generic_WorksCorrectlyFor_Int(object? converted, bool precise, int expectedResult,
            bool failureExpected = false, string? comment = null)
        {
            ConvertToTypeGenericTestsCommon<int>(true, converted, precise, expectedResult, failureExpected, comment);
        }


        /// <summary>Tests whether <see cref="UtilTypes.IsConvertibleTo{TargetType}(object?, bool, IFormatProvider?)"/> 
        /// works correctly for generic target type <see cref="int"/>.</summary>
        /// <param name="converted">Object that is converted to target type.</param>
        /// <param name="precise">Whether conversion should be precise (in this case, conversion fails for values that
        /// cannot be converted precisely, e.g. double to int where the original double has nonzero decimal part).</param>
        /// <param name="expectedResult">Expected result of conversion, agains which assertions are made.</param>
        /// <param name="failureExpected">Whether conversion should fail for the current input data.</param>
        /// <param name="comment">Optional comment (written to output for user inforrmation).</param>
        [Theory]
        // ****
        // Conversion of string:
        // ****
        [InlineData("2", false, 2)]
        [InlineData("-2", false, -2)]
        [InlineData("25_000", false, 25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
        [InlineData("-25_000", false, -25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
        [InlineData("25,000", false, 25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
        [InlineData("-25,000", false, -25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
        // String presentations are floating point instead of integers, conversion throws exception:
        [InlineData("2.0", false, 2, true, "floating point, not an int")]
        [InlineData("-2.0", false, -2, true, "floating point, not an int")]
        // Converson of string, OUT OF RANGE:
        [InlineData("2147483649", false, 0, true, "greater than max int")]
        [InlineData("-2147483650", false, 0, true, "less than min int")]
        // String representations are hexadecimal numbers - not supported:
        [InlineData("0x1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        [InlineData("1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        // ****
        // Conversion of double:
        // ****
        [InlineData((double)2.0, false, 2)]
        [InlineData((double)-2.0, false, -2)]
        // Conversion of double, inprecise - ROUNDED:
        [InlineData((double)2.4999, false, 2, false, "should be rounded below (precise not requested)")]
        [InlineData((double)2.5001, false, 3, false, "should be rounded above (precise not requested)")]
        [InlineData((double)-2.4999, false, -2, false, "should be rounded above (precise not requested)")]
        [InlineData((double)-2.5001, false, -3, false, "should be rounded below (precise not requested)")]
        // Converson of double, OUT OF RANGE:
        [InlineData((double)int.MaxValue + 2.0, false, 0, true, "greater than max int")]
        [InlineData((double)int.MinValue - 2.0, false, 0, true, "less than min int")]
        // Conversion of double, precise conversion required:
        [InlineData((double)2.0, true, 2, false, "precise conversion requested, can be done")]
        [InlineData((double)-2.0, true, -2, false, "precise conversion requested, can be done")]
        [InlineData((double)2.1, true, 2, true, "precise conversion requested, cannot be done because of decimal part")]
        [InlineData((double)-2.1, true, -2, true, "precise conversion requested, cannot be done because of decimal part")]
        // ****
        // Conversion of long:
        // ****
        [InlineData((long)2, false, 2)]
        [InlineData((long)-2, false, -2)]
        // Converson of long, OUT OF RANGE:
        [InlineData((long)int.MaxValue + 2, false, 0, true, "greater than max int")]
        [InlineData((long)int.MinValue - 2, false, 0, true, "less than min int")]
        // ****
        // Conversion of unsigned int:
        // ****
        [InlineData((uint)2, false, 2)]
        // Converson of unsigned int, OUT OF RANGE:
        [InlineData((uint)int.MaxValue + 2, false, 0, true, "greater than max int")]
        // Conversion of bool:
        // ****
        [InlineData((bool)true, false, 1)]
        [InlineData((bool)false, false, 0)]
        // ****
        // Conversion of char:
        // ****
        [InlineData((char)'x', false, (int)'x', false, "conversion of ASCII character to int")]
        [InlineData((char)2836, false, 2836, false, "conversion of non-ASCII character to int")]
        protected void IsConvertibleTo_Generic_WorksCorrectlyFor_Int(object? converted, bool precise, int expectedResult,
            bool failureExpected = false, string? comment = null)
        {
            ConvertToTypeGenericTestsCommon<int>(false, converted, precise, expectedResult, failureExpected, comment);
        }


        #endregion GeneralConversion_Generic_Tests__For_Int


        #endregion GeneralConversion_Generic_Tests




        #region GeneralConversion_Tests

        // Tests nongeneric general conversion methods
        // Only test for one type (int) because more extensive testing should be done for
        // generic methods (which also test non-generic ones because generic methods call
        // nongeneric ones to do the job)


        /// <summary>Common base method for testing both the method for conversion to another basic type (<see cref="UtilTypes.ConvertToType(object?, Type, bool, IFormatProvider?)"/>)
        /// and the method that tells whether an object can be converted to another basic type (<see cref="UtilTypes.IsConvertibleToType(object?, Type, bool, IFormatProvider?)"/>).
        /// Parameter <paramref name="testConversion"/> specifies whether the conversion method (when true), 
        /// or the method that verifies whether conversion is possible, is tested (<see cref="UtilTypes.IsConvertibleToType(object?, Type, bool, IFormatProvider?)"/>).</summary>
        /// <param name="testConversion">Tf true then the conversion method is tested (<see cref="UtilTypes.ConvertToType(object?, Type, bool, IFormatProvider?)"/>),
        /// while if false, the method that tels whether the object can be converted to target type is tested.</param>
        /// <param name="targetType">The required target type after conversion.</param>
        /// <param name="converted">The object that is to be converted, or for which we query whether the object can be 
        /// converted to targat type.</param>
        /// <param name="precise">Whether conversion should be precise. For exemple, when true, and double is converted to int,
        /// conversion can succeed only when the double number is an integer (does not have the decimal part).</param>
        /// <param name="expectedResult">The expected result of conversion.</param>
        /// <param name="failureExpected">Whether the conversion should fail.</param>
        /// <param name="comment">Optional comment, which will be output and helps in checking and interpreting the results.</param>
        protected void ConvertToTypeTestsCommon(bool testConversion, Type targetType, object? converted, bool precise, int expectedResult,
            bool failureExpected = false, string? comment = null)
        {
            if (testConversion)
            {
                Console.WriteLine($"Testing conversion of an object to type int:");
            }
            else
            {
                Console.WriteLine($"Testing the method telling whether an object can be converted to int:");
            }
            Console.WriteLine($"Object to be converted: {converted}, type: {converted?.GetType().Name}");
            Console.WriteLine($"Precise conversion required: {(precise ? "yes" : "no")}.");
            Console.WriteLine("");
            if (failureExpected)
            {
                Console.WriteLine("Conversion is expected to fail.");
            }
            else
            {
                Console.WriteLine($"Expected result of conversion: {expectedResult}");
            }
            if (!string.IsNullOrEmpty(comment))
            {
                Console.WriteLine($"Additional comment: \n  {comment}");
            }
            Console.WriteLine("");
            // Arrange:
            // Act:
            bool exceptionThrown = false;
            Console.WriteLine("Result of conversion:");
            try
            {
                object? result = UtilTypes.ConvertToType(converted, targetType, precise: precise);
                Console.WriteLine($"Result of conversion: {result}, expected: {expectedResult}");
                if (testConversion)
                {
                    result.Should().Be(expectedResult);
                }
            }
            catch (Exception ex)
            {
                exceptionThrown = true;
                Console.WriteLine($"{ex.GetType().Name} thrown: {ex.Message}");
            }
            if (testConversion)
            {
                if (failureExpected)
                {
                    exceptionThrown.Should().BeTrue(because: "this conversion attempt is expected to throw an exception");
                }
                else
                {
                    exceptionThrown.Should().BeFalse(because: "this conversion should be performed without exceptions");
                }
            }
            if (!testConversion)
            {
                bool isConvertible = false;
                bool isConvertibleThrewException = false;
                try
                {
                    isConvertible = UtilTypes.IsConvertibleToType(converted, targetType, precise: precise);
                    Console.WriteLine($"{nameof(UtilTypes.IsConvertibleTo)} returned {isConvertible}.");
                    if (!testConversion)
                    {
                        isConvertible.Should().Be(!failureExpected, because: $"tesult of {nameof(UtilTypes.IsConvertibleTo)} is expected to be {!failureExpected}");
                    }
                }
                catch (Exception)
                {
                    isConvertibleThrewException = true;
                }
                if (!testConversion)
                {
                    isConvertibleThrewException.Should().BeFalse(because: "IsConvertible should not throw an exception in any case");
                }
            }
        }


        #region GeneralConversion_Tests__For_Int


        /// <summary>Tests whether <see cref="UtilTypes.ConvertToType(object?, Type, bool, IFormatProvider?)"/> 
        /// works correctly for generic target type <see cref="int"/>.</summary>
        /// <param name="converted">Object that is converted to target type.</param>
        /// <param name="precise">Whether conversion should be precise (in this case, conversion fails for values that
        /// cannot be converted precisely, e.g. double to int where the original double has nonzero decimal part).</param>
        /// <param name="expectedResult">Expected result of conversion, agains which assertions are made.</param>
        /// <param name="failureExpected">Whether conversion should fail for the current input data.</param>
        /// <param name="comment">Optional comment (written to output for user inforrmation).</param>
        [Theory]
        // ****
        // Conversion of string:
        // ****
        [InlineData("2", false, 2)]
        [InlineData("-2", false, -2)]
        [InlineData("25_000", false, 25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
        [InlineData("-25_000", false, -25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
        [InlineData("25,000", false, 25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
        [InlineData("-25,000", false, -25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
        // String presentations are floating point instead of integers, conversion throws exception:
        [InlineData("2.0", false, 2, true, "floating point, not an int")]
        [InlineData("-2.0", false, -2, true, "floating point, not an int")]
        // Converson of string, OUT OF RANGE:
        [InlineData("2147483649", false, 0, true, "greater than max int")]
        [InlineData("-2147483650", false, 0, true, "less than min int")]
        // String representations are hexadecimal numbers - not supported:
        [InlineData("0x1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        [InlineData("1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        // ****
        // Conversion of double:
        // ****
        [InlineData((double)2.0, false, 2)]
        [InlineData((double)-2.0, false, -2)]
        // Conversion of double, inprecise - ROUNDED:
        [InlineData((double)2.4999, false, 2, false, "should be rounded below (precise not requested)")]
        [InlineData((double)2.5001, false, 3, false, "should be rounded above (precise not requested)")]
        [InlineData((double)-2.4999, false, -2, false, "should be rounded above (precise not requested)")]
        [InlineData((double)-2.5001, false, -3, false, "should be rounded below (precise not requested)")]
        // Converson of double, OUT OF RANGE:
        [InlineData((double)int.MaxValue + 2.0, false, 0, true, "greater than max int")]
        [InlineData((double)int.MinValue - 2.0, false, 0, true, "less than min int")]
        // Conversion of double, precise conversion required:
        [InlineData((double)2.0, true, 2, false, "precise conversion requested, can be done")]
        [InlineData((double)-2.0, true, -2, false, "precise conversion requested, can be done")]
        [InlineData((double)2.1, true, 2, true, "precise conversion requested, cannot be done because of decimal part")]
        [InlineData((double)-2.1, true, -2, true, "precise conversion requested, cannot be done because of decimal part")]
        // ****
        // Conversion of long:
        // ****
        [InlineData((long)2, false, 2)]
        [InlineData((long)-2, false, -2)]
        // Converson of long, OUT OF RANGE:
        [InlineData((long)int.MaxValue + 2, false, 0, true, "greater than max int")]
        [InlineData((long)int.MinValue - 2, false, 0, true, "less than min int")]
        // ****
        // Conversion of unsigned int:
        // ****
        [InlineData((uint)2, false, 2)]
        // Converson of unsigned int, OUT OF RANGE:
        [InlineData((uint)int.MaxValue + 2, false, 0, true, "greater than max int")]
        // Conversion of bool:
        // ****
        [InlineData((bool)true, false, 1)]
        [InlineData((bool)false, false, 0)]
        // ****
        // Conversion of char:
        // ****
        [InlineData((char)'x', false, (int)'x', false, "conversion of ASCII character to int")]
        [InlineData((char)2836, false, 2836, false, "conversion of non-ASCII character to int")]
        protected void ConvertToType_WorksCorrectlyFor_Int(object? converted, bool precise, int expectedResult,
            bool failureExpected = false, string? comment = null)
        {
            ConvertToTypeTestsCommon(true, typeof(int), converted, precise, expectedResult, failureExpected, comment);
        }


        /// <summary>Tests whether <see cref="UtilTypes.IsConvertibleToType(object?, Type, bool, IFormatProvider?)"/> 
        /// works correctly for generic target type <see cref="int"/>.</summary>
        /// <param name="converted">Object that is converted to target type.</param>
        /// <param name="precise">Whether conversion should be precise (in this case, conversion fails for values that
        /// cannot be converted precisely, e.g. double to int where the original double has nonzero decimal part).</param>
        /// <param name="expectedResult">Expected result of conversion, agains which assertions are made.</param>
        /// <param name="failureExpected">Whether conversion should fail for the current input data.</param>
        /// <param name="comment">Optional comment (written to output for user inforrmation).</param>
        [Theory]
        // ****
        // Conversion of string:
        // ****
        [InlineData("2", false, 2)]
        [InlineData("-2", false, -2)]
        [InlineData("25_000", false, 25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
        [InlineData("-25_000", false, -25_000, true, "_ thousands separator cannot be used in string converted to numbers")]
        [InlineData("25,000", false, 25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
        [InlineData("-25,000", false, -25_000, true, ", (comma) thousands separator cannot be used in string converted to numbers")]
        // String presentations are floating point instead of integers, conversion throws exception:
        [InlineData("2.0", false, 2, true, "floating point, not an int")]
        [InlineData("-2.0", false, -2, true, "floating point, not an int")]
        // Converson of string, OUT OF RANGE:
        [InlineData("2147483649", false, 0, true, "greater than max int")]
        [InlineData("-2147483650", false, 0, true, "less than min int")]
        // String representations are hexadecimal numbers - not supported:
        [InlineData("0x1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        [InlineData("1a", false, 26, true, "hexadecimal number format with 0x prefix - not supported")]
        // ****
        // Conversion of double:
        // ****
        [InlineData((double)2.0, false, 2)]
        [InlineData((double)-2.0, false, -2)]
        // Conversion of double, inprecise - ROUNDED:
        [InlineData((double)2.4999, false, 2, false, "should be rounded below (precise not requested)")]
        [InlineData((double)2.5001, false, 3, false, "should be rounded above (precise not requested)")]
        [InlineData((double)-2.4999, false, -2, false, "should be rounded above (precise not requested)")]
        [InlineData((double)-2.5001, false, -3, false, "should be rounded below (precise not requested)")]
        // Converson of double, OUT OF RANGE:
        [InlineData((double)int.MaxValue + 2.0, false, 0, true, "greater than max int")]
        [InlineData((double)int.MinValue - 2.0, false, 0, true, "less than min int")]
        // Conversion of double, precise conversion required:
        [InlineData((double)2.0, true, 2, false, "precise conversion requested, can be done")]
        [InlineData((double)-2.0, true, -2, false, "precise conversion requested, can be done")]
        [InlineData((double)2.1, true, 2, true, "precise conversion requested, cannot be done because of decimal part")]
        [InlineData((double)-2.1, true, -2, true, "precise conversion requested, cannot be done because of decimal part")]
        // ****
        // Conversion of long:
        // ****
        [InlineData((long)2, false, 2)]
        [InlineData((long)-2, false, -2)]
        // Converson of long, OUT OF RANGE:
        [InlineData((long)int.MaxValue + 2, false, 0, true, "greater than max int")]
        [InlineData((long)int.MinValue - 2, false, 0, true, "less than min int")]
        // ****
        // Conversion of unsigned int:
        // ****
        [InlineData((uint)2, false, 2)]
        // Converson of unsigned int, OUT OF RANGE:
        [InlineData((uint)int.MaxValue + 2, false, 0, true, "greater than max int")]
        // Conversion of bool:
        // ****
        [InlineData((bool)true, false, 1)]
        [InlineData((bool)false, false, 0)]
        // ****
        // Conversion of char:
        // ****
        [InlineData((char)'x', false, (int)'x', false, "conversion of ASCII character to int")]
        [InlineData((char)2836, false, 2836, false, "conversion of non-ASCII character to int")]
        protected void IsConvertibleToType_WorksCorrectlyFor_Int(object? converted, bool precise, int expectedResult,
            bool failureExpected = false, string? comment = null)
        {
            ConvertToTypeTestsCommon(false, typeof(int), converted, precise, expectedResult, failureExpected, comment);
        }


        #endregion GeneralConversion_Tests__For_Int


        #endregion GeneralConversion_Tests




    }

}

