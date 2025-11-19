
#nullable enable

using FluentAssertions;
using IGLib.Commands;
using IGLib.Tests.Base;
using Microsoft.VisualBasic;
using NUnit.Framework.Internal;
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
        /// <para>2. Target type (redundant when testing generic methods - MUST BE int here) (Type)</para>
        /// <para>3. Whether PRECISE conversion is required (bool)</para>
        /// <para>4. Whether conversion is expected to be be SUCCESFUL (bool)</para>
        /// <para>5. Expected values of converted elemennts (object[])</para></summary>
        public static TheoryData<IEnumerable?, Type, bool, bool, object?[]?> Dataset_CovertToListOf_Int =>
            new ()
            {
                // PRECISE conversions are required:
                { (object?[])[(int)1, (int)2, (int)3], TypeInt, true, true, [1, 2, 3] },
                { (object?[])[(double)1.23, (int)2 ], TypeInt, true, false, null },  // mixed numeric type elements
                { (object?[])[(double)1, (double)2.45 ], TypeInt, true, false, null },  // mixed numeric type elements
                { (object?[])[], TypeInt, true, false, null },  // empty enumerable
                { null, TypeInt, true, false, null },  // null enumerable
                { (object?[])["str", (int) 1, (int) 2], TypeInt, true, false, null },  // mixed non-numeric and numeric elements
                { (object?[])[(int) 1, (int) 2, "str"], TypeInt, true, false, null },  // mixed non-numeric and numeric elements
                { (object?[])[(int) 1, "str", (int) 2], TypeInt, true, false, null },  // mixed non-numeric and numeric elements
                { (object?[])[null!, (int) 1, (int) 2], TypeInt, true, false, null },  // includes null elements
                { (object?[])[(int) 1, (int) 2, null!], TypeInt, true, false, null },  // includes null elements
                { (object?[])[(int) 1, null!, (int) 2], TypeInt, true, false, null },  // includes null elements
                { new List<object?>() { -25, 433, 9238, -5 }, TypeInt, true, true, [-25, 433, 9238, -5] },
                { new List<object?>() {  }, TypeInt, true, false, null },
                { new List<object?>() { 1, 2, "xy" }, TypeInt, true, false, null },
                { new List<object?>() { 1, 2, null! }, TypeInt, true, false, null },
                { new List<object?>() { 1, 2, 3, 4.55 }, TypeInt, true, false, null },
                // APPROXIMATE conversions are ALLOWED:
                { (object?[])[(int)1, (int)2, (int)3], TypeInt, false, true, [1, 2, 3] },  // same result as with precise
                { (object?[])[(double)1.23], TypeInt, false, true, null },   // conversion succeeds while it fails with precise
                { (object?[])[(float)1.23], TypeInt, false, true, null },   // conversion succeeds while it fails with precise
                { (object?[])[(string)"1.23"], TypeInt, false, false, null },   // string conversion must be precise with precise
                { (object?[])[(string)"15"], TypeInt, false, true, null },   // precise string conversion succeeds
                { (object?[])[int.MaxValue + (long) 1], TypeInt, false, false, null }  // owerflow
            };



        [Theory]
        [MemberData(nameof(Dataset_CovertToListOf_Int))]
        protected void ConvertToListOf_Int_WorksCorrectly(IEnumerable? enumerable, Type targetType, 
            bool precise, bool shouldBeConvertible, object?[]? expectedResult)
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
            Console.WriteLine($"Is precise conversioin required:      {precise}");
            Console.WriteLine($"Conversion should be possible:        {shouldBeConvertible}");
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
            bool wasConverionSuccessful = false;
            try
            {
                result = UtilTypes.ConvertToListOf<int>(enumerable, precise: precise);
                wasConverionSuccessful = true;
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
            }
            Console.WriteLine($"Was conversion successful: {wasConverionSuccessful}");
            // Assert:
            wasConverionSuccessful.Should().Be(shouldBeConvertible, because: $"collection elements should {
                (shouldBeConvertible ? "" : "NOT")} be convertible to the specified type.");
            if (wasConverionSuccessful && expectedResult != null && expectedResult.Length > 0)
            {
                result.Should().NotBeNull();
                result.Count.Should().Be(expectedResult.Length, because: $"number of elements after conversion should be {
                    expectedResult.Length}.");
                if (expectedResult != null && expectedResult.Length >= 0 && shouldBeConvertible)
                {
                    for (int i = 0; i < expectedResult!.Length; ++i)
                    {
                        expectedResult[i].Should().Be(result[i], because: $"element {i} should be {
                            expectedResult[i]} but it is {result[i]}.");
                    }
                }
            }
        }


        [Theory]
        // Dataset for conversion to int:
        [MemberData(nameof(Dataset_CovertToListOf_Int))]  // Conversion to int from various types
        protected void IsConvertibleToCollectionOf_Int_WorksCorrectly(IEnumerable? enumerable, Type targetType,
            bool precise, bool shouldBeConvertible, object?[]? expectedResult)
        {
            Type internalType = typeof(int);
            Console.WriteLine($"Testing conversion of an object collection to a list of elements of type {internalType.Name}.");
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
            Console.WriteLine($"Target element type after conversion: {internalType.Name}");
            Console.WriteLine($"Is precise conversioin required:      {precise}");
            Console.WriteLine($"Conversion should be possible:        {shouldBeConvertible}");
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
            Console.WriteLine($"\nThe collection should be convertible to collection of type {targetType.Name}: {shouldBeConvertible}\n");
            if (targetType!=internalType)
            {
                Console.WriteLine($"Warning:\n  Target type from dataset ({targetType.Name}) does not match the internal target type ({
                    internalType.Name}) of this method.\n  Possibly a wrong dataset is used.");
            }
            targetType.Should().Be(internalType, because: $"PRECOND: the target typee for this test method should be {internalType.Name}");
            // Act:
            bool result = false;
            bool wasExceptionThrown = false;
            try
            {
                result = UtilTypes.IsConvertibleToCollectionOf<int>(enumerable, precise: precise);
                Console.WriteLine($"Result of {nameof(UtilTypes.IsConvertibleToCollectionOfType)}: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.GetType().Name} was thrown when trying to establish convertibility.\n  Message:{ex.Message}\n");
                wasExceptionThrown = true;
            }
            // Assert:
            wasExceptionThrown.Should().BeFalse(because: $"{nameof(UtilTypes.IsConvertibleToCollectionOfType)} should not throw exceptions");
            result.Should().Be(shouldBeConvertible);
            if (wasExceptionThrown && expectedResult != null && expectedResult.Length > 0)
            {
                result.Should().Be(shouldBeConvertible, because: $"the collection should {(shouldBeConvertible ? "" : "NOT")} be convertible to collection of objects of type {targetType.Name}");
            }
        }



        // TARGET TYPE defined AS Type PARAMETER


        /// <summary>Test parameters dataset for testing conversion of collection elements to
        /// list of type specified via parameter. Pretty comprehensive.
        /// <para>Dataset parameters:</para>
        /// <para>1. Collection to be converted (IEnumerable?)</para>
        /// <para>2. Target type (Type)</para>
        /// <para>3. Whether PRECISE conversion is required (bool)</para>
        /// <para>4. Whether conversion is expected to be be SUCCESFUL (bool)</para>
        /// <para>5. Expected values of converted elemennts (object[])</para></summary>
        public static TheoryData<IEnumerable?, Type, bool, bool, object?[]?> Dataset_CovertToListOfType =>
            new()
            {
                // NULL or EMPTY COLLECTION:
                // Null collections cannot be converted:
                { null, typeof(int), true, false, [] },
                { null, typeof(int), false, false, [] },
                // Empty collections cannot be converted:
                { new List<object>(), typeof(int), true, false, [] },
                { new List<object>(), typeof(int), false, false, [] },
                // Collections with null elements cannot be converted:
                { (object?[])[1, "23", (string?) null], typeof(string), false, false, ["1", "23", (string?)null] },
                { (object?[])[1, "23", (double?)null], typeof(int), false, false, [23, (int?) null] },

                // INTEGER type to INTEGER type:
                // byte to int: should work for any value
                { (object?[])[(byte)0, (byte)2, (byte)255], typeof(int), true, true, [0, 2, 255] },
                // char to int: should work for any value
                { (object?[])[(char)0, (char)2, char.MaxValue], typeof(int), true, true, [0, 2, (int)char.MaxValue] },
                // uint to int:
                { (object?[])[(uint)0, (uint)2, (uint)int.MaxValue], typeof(int), true, true, [0, 2, (int)int.MaxValue] },
                { (object?[])[uint.MaxValue], typeof(int), true, false, [uint.MaxValue] },  // overflow!
                // long to int: 
                { (object?[])[(long)int.MinValue, (long)-10, (long)0, (long)15, (long)int.MaxValue], 
                    typeof(int), true, true, [int.MinValue, -10, 0, 15, int.MaxValue] },
                { (object?[])[(long)int.MinValue -1], typeof(int), true, false, [(long)int.MinValue - 1] },  // umderflow!
                { (object?[])[(long)int.MaxValue +1], typeof(int), true, false, [(long)int.MaxValue + 1] },  // owerflow!
                // ulong to int:
                { (object?[])[(ulong)0, (ulong)2, (ulong)int.MaxValue], typeof(int), true, true, [0, 2, int.MaxValue] },
                // long to char:
                { (object?[])[(long)0, (long)26, (long)char.MaxValue], typeof(char), true, true, [(char)0, (char)26, char.MaxValue] },
                { (object?[])[(long)-1], typeof(char), true, false, [(int)-1] },  // char cannot be negative
                { (object?[])[(long)char.MaxValue + (long)1], typeof(char), true, false, [(int)char.MaxValue + (int)1] },  // owerflow
                // INTEGER type to FLOATING POINT type:
                { (object?[])[-132, 0, 815], typeof(double), true, true, [-132.0, 0.0, 815.0] },
                { (object?[])[-132, 0, 815], typeof(float), true, true, [-132.0f, 0.0f, 815.0f] },
                { (object?[])[ulong.MaxValue], typeof(float), true, false, [ulong.MaxValue] },  // overflow
                // FLOATING POINT type to INTEGER type:
                { (object?[])[(double)int.MinValue - 1.0], typeof(int), true, false, [(long)int.MinValue - 1.0] },  // underflow
                { (object?[])[(double)int.MaxValue + 1.0], typeof(int), true, false, [(long)int.MinValue - 1.0] },  // overflow
                { (object?[])[(double)-132, (double)0, (double)815], typeof(int), true, true, [-132, 0, 815] },
                { (object?[])[(float)-132, (float)0, (float)815], typeof(int), true, true, [-132, 0, 815] },
                // precise conversion fails when the floating point value has non-integer part:
                { (object?[])[(double)1253.28], typeof(int), true, false, [(int)1253] },  // conversion is not precise, fails
                { (object?[])[(double)1253.28], typeof(char), true, false, [(char)1253] },  // conversion is not precise, fails
                { (object?[])[(double)-5.9], typeof(int), true, false, [-6] },  // conversion is not precise, fails
                // approximate conversions with the same data should succeed when precise is not required:
                { (object?[])[(double)12.28], typeof(int), false, true, [(int)12] },  // conversion is not precise, succeeds when precise is not required
                { (object?[])[(double)12.28], typeof(char), false, false, [(char)12] },  // conversion is not precise, succeeds when precise is not required
                { (object?[])[(double)-5.9], typeof(int), false, true, [(int)-6] },  // conversion is not precise, succeeds when precise is not required
                // BOOLEAN TO other types conversions:
                { (object?[])[true, false], typeof(int), true, true, [1, 0] },
                { (object?[])[true, false], typeof(byte), true, true, [(byte)1, (byte)0] },
                { (object?[])[true, false], typeof(long), true, true, [(long)1, (long)0] },
                { (object?[])[true, false], typeof(ulong), true, true, [(ulong)1, (ulong)0] },
                { (object?[])[false, true], typeof(double), true,  true, [0.0, 1.0] },
                { (object?[])[false, true], typeof(float), true,  true, [0.0f, 1.0f] },
                { (object?[])[true, false], typeof(char), true, false, [(char)1, (char)0] },  // boolean cannot be converted to char
                { (object?[])[true, false], typeof(char), false, false, [(char)1, (char)0] },  // boolean cannot be converted to char
                { (object?[])[true, false], typeof(string), true, true, ["True", "False"] },
                // type TO BOOLEAN conversions:
                // int to boolean:
                // Conversion of 0 and 1 to boolean works both as approximate and precise:
                { (object?[])[0, 1], typeof(bool), true,  true, [false, true] }, 
                { (object?[])[0, 1], typeof(bool), false,  true, [false, true] },  
                // Conversion of int values other than 0 or 1 only works as not precise:
                { (object?[])[10, -11], typeof(bool), true, false, [true, true] },
                { (object?[])[10, -11], typeof(bool), false, true, [true, true] },  // not precise works, all non-0 converted to true
                // byte to boolean:
                // Conversion of 0 and 1 to boolean works both as approximate and precise:
                { (object?[])[(byte)0, (byte)1], typeof(bool), true,  true, [false, true] }, 
                { (object?[])[(byte)0, (byte)1], typeof(bool), false,  true, [false, true] },  
                // Conversion of byte values other than 0 or 1 only works as not precise:
                { (object?[])[(byte)10, (byte)150], typeof(bool), true, false, [true, true] },
                { (object?[])[(byte)10, (byte)150], typeof(bool), false, true, [true, true] },  // not precise works, all non-0 converted to true
                // double to boolean:
                // Conversion of 0 and 1 to boolean works both as approximate and precise:
                { (object?[])[(double)0.0, (double)1.0], typeof(bool), true,  true, [false, true] }, 
                { (object?[])[(double)0.0, (double)1.0], typeof(bool), false,  true, [false, true] },  
                // Conversion of double values other than 0.0 or 1.0 only works as not precise:
                { (object?[])[(double)10.0, (double)-150.0], typeof(bool), true, false, [true, true] },
                { (object?[])[(double)10.0, (double)-150.0], typeof(bool), false, true, [true, true] },  // not precise works, all non-0 converted to true
                { (object?[])[(double)0.0034, (double)1.0e-6], typeof(bool), true, false, [true, true] },
                { (object?[])[(double)10.034, (double)1.0e-6], typeof(bool), false, true, [true, true] },  // not precise works, all non-0 converted to true
                // string to boolean:
                // precise conversion of ANY CAPITALIZATION of "true" and "false" to boolean works:
                { (object?[])["False", "True"], typeof(bool), true,  true, [false, true] }, 
                { (object?[])["false", "true"], typeof(bool), true,  true, [false, true] }, 
                { (object?[])["FALSE", "TRUE"], typeof(bool), true,  true, [false, true] }, 
                { (object?[])["falSE", "truE"], typeof(bool), true,  true, [false, true] }, 
                // conversion of strings "y"/"n", "yes"/"no" or "Yes"/"No" to boolean does not work:
                { (object?[])["n", "y"], typeof(bool), false,  false, [false, true] }, 
                { (object?[])["No", "Yes"], typeof(bool), false,  false, [false, true] }, 
                { (object?[])["no", "yes"], typeof(bool), false,  false, [false, true] }, 
                // STRING to INTEGER type conversions:
                { (object?[])["-6", "0", "255"], typeof(int), true, true, [-6, 0, 255] },
                { (object?[])["8.12"], typeof(int), false, false, [8] },  // floating point is not legal with conversion to integer types
                { (object?[])["0", "25", "255"], typeof(byte), true, true, [(byte) 0, (byte) 25, (byte)255] },
                { (object?[])["260"], typeof(byte), true, false, [260] },  // overflow
                { (object?[])["-5"], typeof(byte), true, false, [-5] },  // byte cannot be negative
                // INTEGER type to STRING type conversions:
                { (object?[])[-6, 0, 255], typeof(string), true, true, ["-6", "0", "255"] },
                { (object?[])[(byte)0, (byte)25, (byte)255], typeof(string), true, true, ["0", "25", "255"] },
                { (object?[])[long.MinValue], typeof(string), true, true, [(long.MinValue).ToString()] },
                // STRING to FLOATING POINT type conversions:
                { (object?[])["-6.2", "0.5", "255.44", "-2.6e5"], typeof(double), true, true, [-6.2, 0.5, 255.44, -2.6e5] },
                { (object?[])["-6.2", "0.5", "255.44", "-2.6e5"], typeof(float), true, true, [-6.2f, 0.5f, 255.44f, -2.6e5f] },
#if NETCOREAPP  // for data below behavior is different in .NET Framework
                { (object?[])["1.23e90"], typeof(float), false, true, [(float)1.23e90] },  // overflow, returns float infinity
                { (object?[])["1.23e90"], typeof(float), true, true, [(float)1.23e90] },  // overflow, returns float infinity
#endif
                // FLOATING POINT type to STRING conversions:
                { (object?[])[-6.2, 0.5, 255.44, -2.6e5], typeof(string), true, true, ["-6.2", "0.5", "255.44", "-260000"] },
                { (object?[])[(float)-6.2, (float)0.5, (float)255.44, (float)-2.6e5], typeof(string), true, true, ["-6.2", "0.5", "255.44", "-260000"] },
                // MIXED TYPE conversions (elements of collection are of mixed types):
                // Conversion succeeds if all element conversions to specific type succeed.
                // Conversion to int:
                { (object?[])["-6", 0.49, (byte)87, -2.6e5], typeof(int), false, true, [-6, 0, 87, -260000] },  // imprecise conversion succeeds
                { (object?[])["-6", 0.49, (byte)87, -2.6e5], typeof(int), true, false, [-6, 0, 87, -260000] },  // precise conversion fails
                { (object?[])["-6", (double)0.0, (byte)87, -2.6e5], typeof(int), true, true, [-6, 0, 87, -260000] },  // precise conversion also succeeds on this data
                { (object?[])[true, (char) 22, false], typeof(int), true, true, [1, 22, 0] },  // imprecise conversion succeeds
                // Conversion to double:
                { (object?[])["-6.21e15", (double)0.0, (byte)87, (int)-260000], typeof(double), true, true, [-6.21e15, 0.0, 87.0, -2.6e5] },
                { (object?[])[(byte) 52, (uint)43, (long)1243], typeof(double), true, true, [52.0, 43.0, 1243.0] },
                { (object?[])["-2.53", (float)18.5e14f, true, false], typeof(double), true, true, [-2.53, (double)18.5e14f, 1, 0] },
                // Conversion to string:
                { (object?[])[true, 28, 37.45, (short) -36], typeof(string), true, true, ["True", "28", "37.45", "-36"] },
                { (object?[])[false, 'x', 'y', "xy", 58.6], typeof(string), true, true, ["False", "x", "y", "xy", "58.6"] },
                // Conversion to boolean:
                { (object?[])["true", "False", 28, (double)0.0, (short) -36, 1.5], typeof(bool), false, true, 
                    [true, false, true, false, true, true] },  // works for imprecise conversion
                { (object?[])["true", "False", 28, (double)0.0, (short) -36, 1.5], typeof(bool), true, false, 
                    [true, false, true, false, true, true] },  // does not work for precise conversion
                { (object?[])["true", "False", 1, (double)0.0, (short) 1, 1.0], typeof(bool), true, true, 
                    [true, false, true, false, true, true] },  // this also works for precise conversion (numerical values are only 0 or 1)
                // NULLABLE VALUE TYPES:
                { (object?[])[(double?) 2.0, (byte?) 15], typeof(int), true, true, [(int)2, (int)15] },
                { (object?[])[(double?) null, (byte?) 15], typeof(int), true, false, [(int)2, null] },  // collection should not contain null values
                { (object?[])[(double?) 18.0, "15", (long?)((long)int.MaxValue + 1)], typeof(long), true, true, 
                    [(long)18, (long)15, (long)int.MaxValue + 1] },  // collection should not contain null values
                // PRECISE vs. IMPRECISE conversions:
                // When precise parameter is true, conversions that lose precision are not allowed and will fail;
                // precise conversion is specified by the 2nd boolean parameter:
                { (object?[])[(double)5.2], typeof(int), true, false, [5] },  // precise conversion fails
                { (object?[])[(double)8.0], typeof(bool), true, false, [true] },  // precise conversion fails
                { (object?[])[(int)9], typeof(bool), true, false, [true] },  // precise conversion fails
                // When precise parameter is false, legitimate conversions that lose precision are allowed and will succeed:
                { (object?[])[(double)5.2], typeof(int), false, true, [5] },  // precise conversion fails
                { (object?[])[(double)8.0], typeof(bool), false, true, [true] },  // precise conversion fails
                { (object?[])[(int)9], typeof(bool), false, true, [true] },  // precise conversion fails
            };



        [Theory]
        // Dataset for conversion to int:
        [MemberData(nameof(Dataset_CovertToListOf_Int))]  // Conversion to int from various types
        [MemberData(nameof(Dataset_CovertToListOfType))]  // Conversions between different types, comprehensive
        protected void ConvertToListOfType_WorksCorrectly(IEnumerable? enumerable, Type targetType,
            bool precise, bool shouldBeConvertible, object?[]? expectedResult)
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
            Console.WriteLine($"Is precise conversioin required:      {precise}");
            Console.WriteLine($"Conversion should be possible:        {shouldBeConvertible}");
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
            Console.WriteLine("");
            // Act:
            List<object?>? result = null;
            bool wasConverionSuccessful = false;
            try
            {
                result = UtilTypes.ConvertToListOfType(enumerable, targetType, precise: precise);
                wasConverionSuccessful = true;
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
            }
            Console.WriteLine($"Was conversion successful: {wasConverionSuccessful}");
            // Assert:
            wasConverionSuccessful.Should().Be(shouldBeConvertible, because: $"collection elements should {(shouldBeConvertible ? "" : "NOT")} be convertible to the specified type.");
            if (wasConverionSuccessful && expectedResult != null && expectedResult.Length > 0)
            {
                result.Should().NotBeNull();
                result.Count.Should().Be(expectedResult.Length, because: $"number of elements after conversion should be {expectedResult.Length}.");
                if (expectedResult != null && expectedResult.Length >= 0 && shouldBeConvertible)
                {
                    for (int i = 0; i < expectedResult!.Length; ++i)
                    {
                        if (result!= null)
                        {
                            result[i]!.GetType().Should().Be(targetType, because: $"resulting elements should be of type {targetType.Name}"); 
                        }
                        result![i].Should().Be(expectedResult[i], because: $"element {i} should be {expectedResult[i]} but it is {(result[i] == null ? "null" : result[i])}.");
                    }
                }
            }
        }


        [Theory]
        // Dataset for conversion to int:
        [MemberData(nameof(Dataset_CovertToListOf_Int))]  // Conversion to int from various types
        [MemberData(nameof(Dataset_CovertToListOfType))]  // Conversions between different types, comprehensive
        protected void IsConvertibleToCollectionOfType_WorksCorrectly(IEnumerable? enumerable, Type targetType,
            bool precise, bool shouldBeConvertible, object?[]? expectedResult)
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
            Console.WriteLine($"Is precise conversioin required:      {precise}");
            Console.WriteLine($"Conversion should be possible:        {shouldBeConvertible}");
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
            Console.WriteLine($"\nThe collection should be convertible to collection of type {targetType.Name}: {shouldBeConvertible}\n");
            // Act:
            bool result = false;
            bool wasExceptionThrown = false;
            try
            {
                result = UtilTypes.IsConvertibleToCollectionOfType(enumerable, targetType, precise: precise);
                Console.WriteLine($"Result of {nameof(UtilTypes.IsConvertibleToCollectionOfType)}: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.GetType().Name} was thrown when trying to establish convertibility.\n  Message:{ex.Message}\n");
                wasExceptionThrown = true;
            }
            // Assert:
            wasExceptionThrown.Should().BeFalse(because: $"{nameof(UtilTypes.IsConvertibleToCollectionOfType)} should not throw exceptions");
            result.Should().Be(shouldBeConvertible);
            if (wasExceptionThrown && expectedResult != null && expectedResult.Length > 0)
            {
                result.Should().Be(shouldBeConvertible, because: $"the collection should {
                    (shouldBeConvertible? "": "NOT")} be convertible to collection of objects of type {targetType.Name}");
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
        // Conversion of double, imprecise - ROUNDED:
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
        // Conversion of double, imprecise - ROUNDED:
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
        // Conversion of double, imprecise - ROUNDED:
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
        // Conversion of double, imprecise - ROUNDED:
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

