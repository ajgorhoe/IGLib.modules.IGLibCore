
#nullable disable

#if true

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;

using LearnCs.Lib;

namespace IGLib.Tests.Base
{

    /// <summary><para>This test class provides some examples of how to create unit tests in XUnit. It
    /// also shows how to inherit from the generic <see cref="TestBase{TestClass}"/> class.</para>
    /// <para>In particular, the examples show how to define sets of test parameters, even when these
    /// parameters are of class types and require creation of objects (the basic parameters definition
    /// via the <see cref="TheoryAttribute"/> and <see cref="InlineDataAttribute"/> only allow setting
    /// data with constant expressions or array of elements specified by constant expressions).</para>
    /// 
    /// <para>This test class inherits from <see cref="TestBase{ExampleTestClass}"/>, from which it inherits
    /// properties  <see cref="TestBase{TestClass}.Output"/> of type <see cref="ITestOutputHelper"/>, which can 
    /// be used to write on test's output. Only the simple <see cref="ITestOutputHelper.WriteLine(string)"/>
    /// and <see cref="ITestOutputHelper.WriteLine(string, object[])"/> methods are available for writing 
    /// to test Output.</para>
    /// <para>The <see cref="TestBase{TestClass}"/> also provides the property 
    /// <see cref="TestBase{TestClass}.Console"/> of type <see cref="ITestOutputHelper"/>, which contains
    /// a reference to the same object as <see cref="TestBase{TestClass}.Output"/>.</para></summary>
    public class ExampleTestClass : TestBase<ExampleTestClass>
    {
        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> property.</summary>
        /// <param name=""></param>
        public ExampleTestClass(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output property.
        }

        /// <summary><para>Typical form of a simple unit test. The [Fact] attribute in square brackets (type 
        /// <see cref="FactAttribute"/>) is used to tell that this is a test method, and it can be executed by
        /// the test environment when you run tests (this also enables the "Run Tests" item in the context
        /// menu when you right-click the test method name, enabling to quickly run the test).</para>
        /// <para>The <see cref="TestBase{TestClass}.Output"/> property inherited from the test class can
        /// be used to write additional information to the output of the test.</para>
        /// <para>Assertions are used to verify that the result match what is expected. In this case,
        /// "fluent assertions" are used, which are implemented as extension methods for various types. In 
        /// this example, the int.Should() extension method is used to do the assertion. If the assertion
        /// fails then the test would fail and this would be visible in Test Explorer. The test would also
        /// fail if unhandled exception is thrown in a test.</para>>
        /// <para>This fictional demonstrative example tests that the + operator correctly sums two integer
        /// numbers.</para></summary>
        [Fact]    // Attribute [Fact] marks this method as test method (spcifics of the XUnit framework)
        protected void TestDemo_Simple_Addition_Test()
        {
            Output.WriteLine($"Testing addition of two numbers of type int:");
            // ** Arrange:
            int a = 22;
            int b = 7;
            int expectedResult = 29;
            // ** Act:
            int result = a + b;
            Output.WriteLine($"Input: a={a}, b={b}, result vs. expected: {result} vs. {expectedResult}");
            // ** Assert:
            result.Should().Be(expectedResult);
        }

        /// <summary><para>This method shows how to use tests with parameters. Procedure is as follows:</para>
        /// <para>  * Add parameters to the test method. In this case, parameters <paramref name="a"/> and
        /// <paramref name="b"/> represent summands that will be added in each test, and the <paramref name="expectedResult"/>
        /// parameter states the expected result of summation for these summands, such that we can verify correctness.</para>
        /// <para>  * Add the attribute [Theory] to the test method (not [Fact], which is for non-parametric tests).</para>
        /// <para>  * Add one [InlineData(...)] attribute with listed parameter values for each set of
        /// parameters for which the test should be executed.</para></summary>
        [Theory]                 // Use [Theory] attribute instead of [Fact] for parameterized tests
        [InlineData(2, 8, 10)]   // Use [InlineData(...)] attribute to list sets of parameter values (each set means a separate test)
        [InlineData(0, 0, 0)]
        [InlineData(0, 22, 22)]
        [InlineData(14, 0, 14)]
        [InlineData(-3, 0, -3)]
        [InlineData(-4, -5, -9)]
        [InlineData(-2, 5, 3)]
        [InlineData(2, -6, -4)]
        protected void TestDemo_Addition_Test(int a, int b, int expectedResult)
        {
            Output.WriteLine($"Testing addition of two numbers of type int:");
            // Arrange
            // Act
            int result = a + b;  // perform the operation that we want to test
            // Write informative output to the test output
            Output.WriteLine($"Input: a={a}, b={b}, result vs. expected: {result} vs. {expectedResult}");
            // Assert
            result.Should().Be(expectedResult); // assert correctness of operation
        }



        /// <summary>This method demonstrates how to use tests with array parameters. It tests whether summation of arrays
        /// is performed correctly. For demonstration purposes, we summate two arrays in the body of the test, while
        /// in a real-life scenarios this would be performed bt the method that we want to test.
        /// <para></para></summary>
        /// <param name="arr1">The first array summand.</param>
        /// <param name="arr2">The second array summand.</param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData(
            new double[] { 1.1, 1.2, 1.3 },
            new double[] { 2.1, 2.2, 2.3 },
            new double[] { 3.2, 3.4, 3.6 })]
        [InlineData(
            new double[] { 10, 20, 30 },
            new double[] { 11, 12, 13 },
            new double[] { 21, 32, 43 })]
        //// Two datasets with wrong expected results; uncomment them to see how tests would fail:
        //[InlineData(
        //    new double[] { 4, 6 },
        //    new double[] { 10, 8 },
        //    new double[] { 14, 555 })]  // wrong expected result (wrong elements); test will fail for this dataset
        //[InlineData(
        //    new double[] { 4, 6 },
        //    new double[] { 10, 8 },
        //    new double[] { 14, 14, 22 })]  // wrong expected result (wrong length); test will fail for this dataset
        protected void TestDemo_Array_Addition_Test(double[] arr1, double[] arr2, double[] expected)
        {
            // Arrange:
            double[] result = new double[arr1.Length];
            // Act:
            // Perform array summation (normally we would call a method for this, but it's just a demo):
            for (int i = 0; i < arr1.Length; i++)
            { 
                result[i] = arr1[i] + arr2[i];
            }
            // Assert:
            // Assert on what we expect the result to be:
            result.Should().NotBeNull(because: "(precondition) the result should be properly allocated for the operation.");
            result.Length.Should().Be(expected.Length, because: "(precondition) the result should be allocated with correct length");
            for (int i = 0; i < arr1.Length; i++)
            { 
                //
                result[i].Should().BeApproximately(expected[i], 1e-6, because: "each element od the result array should be the sum of elements of added array at the same indices");
            }
        }


        /// <summary>This test example has many things commented out, and it is to demonstrate the difficulties associated with 
        /// having parameters of the test methods that are higher-dimensional arrays like rectangular or jagged arrays. In 
        /// order to achieve this, one needs to first define a method that returns a collection of parameters, and then
        /// define define a test method having the attributes </summary>
        /// <param name="array"></param>
        [Theory]
        [InlineData(
            new double[] { 1.1, 1.2, 1.3 }
            // , new double[][] { new double[] { 1.1, 1.2, 1.3 } }
            )]
        [InlineData(
            new double[] { 1.2344, 1.2, 1.3 }
            // , new double[][] { new double[] { 1.1, 1.2, 1.3 } }
            )]
        protected void TestDemo_Array2dRectangular_Example(double[] array) // the following does not work: , double[][] jaggedaArray
        {
#if false
            // Various ways to initialize arrays, to try using them in definition of inline parameters
            //double[] oneDim = { 1.1, 1.2 };
            //double[] oneDim2 = [2.1, 1.2];
            //double[,] rectangular = new double[,] { { 1.1, 1.2, 1.3 } };
            //double[,] rectangular2 = new double[,] { { 1.1, 1.2, 1.3 } };
            //double[][] jagged = new double[][] { new double[] { 1.1, 1.2, 1.3 } };
            //double[][] jagged2 = { new double[] { 1.1, 1.2, 1.3 } };
            //double[][] jagged3 = { new double[] { 1.1, 1.2, 1.3 }, new double[] { 1.1, 1.2, 1.3 } };
            //double[][] jagged4 = [[1.1, 1.2, 1.3], new double[] { 1.1, 1.2, 1.3 }];
            //double[][] jagged5 = [[1.1, 1.2, 1.3], [1.1, 1.2, 1.3]];
#endif
            array.Should().NotBeNullOrEmpty();
        }


        // Datasets for tests defined by static public methods returning IEnumerable<object[]>:

        /// <summary>Returns a collection (<see cref="IEnumerable{object[]}"/>) containing a single parameters set for testing 
        /// addition of two <see cref="ComplexVector"/> objects.</summary>
        public static IEnumerable<object[]> Dataset_OperatorPlus_1()
        {
            ComplexVector a = (double[])[1, 2, 3];
            ComplexVector b = (double[])[4, 5, 6];
            ComplexVector r = (double[])[5, 7, 9];
            yield return new object[] { a, b, r };
        }

        /// <summary>Another method returning a collection with a single parameters set.</summary>
        public static IEnumerable<object[]> Dataset_OperatorPlus_2()
        {
            ComplexVector a = (double[])[1.1, 1.2, 1.3];
            ComplexVector b = (double[])[2.1, 2.2, 2.3];
            ComplexVector r = (double[])[3.2, 3.4, 3.6];
            yield return new object[] { a, b, r };
        }

        /// <summary>Yet another method returning a collection with a single parameters set.</summary>
        public static IEnumerable<object[]> Dataset_OperatorPlus_3()
        {
            ComplexVector a = ((double, double)[])[(1.1, 1), (1.2, 2), (1.3, 3)];
            ComplexVector b = ((double, double)[])[(2.1, 4), (2.2, 5), (2.3, 6)];
            ComplexVector r = ((double, double)[])[(3.2, 5), (3.4, 7), (3.6, 9)];
            yield return new object[] { a, b, r };
        }

        /// <summary>Returns a collection of sets of parameters for testing addition of two <see cref="ComplexVector"/>
        /// objects, as <see cref="IEnumerable{object[]}"/>.</summary>
        public static IEnumerable<object[]> Dataset_OperatorPlus()
        {
            ComplexVector a;
            ComplexVector b;
            ComplexVector r;

            a = (double[])[1, 2, 3];
            b = (double[])[4, 5, 6];
            r = (double[])[5, 7, 9];
            yield return new ComplexVector[] { a, b, r };

            a = (double[])[1.1, 1.2, 1.3];
            b = (double[])[2.1, 2.2, 2.3];
            r = (double[])[3.2, 3.4, 3.6];
            yield return new object[] { a, b, r };

            a = ((double, double)[])[(1.1, 1), (1.2, 2), (1.3, 3)];
            b = ((double, double)[])[(2.1, 4), (2.2, 5), (2.3, 6)];
            r = ((double, double)[])[(3.2, 5), (3.4, 7), (3.6, 9)];
            yield return new object[] { a, b, r };
        }

        /// <summary>Example of a parametric tests that has parameters of arbitrary types (in this case, all three 
        /// parameters are of type ComplexVector, but the same approach works for arbitrary parameter types).
        /// <para>When we have sets for parameters that are not of simple types (for which literals can be used 
        /// to represent their values) and are not simple 1D arrays with such elements, we cannot use attributes 
        /// <see cref="InlineDataAttribute"/> to pass parameter sets to test methods.</para>
        /// <para>Solution is to use public static methods that return collection of parameter sets in form of
        /// <see cref="IEnumerable{[]}"/>. This example shows how to do this.</para>
        /// <para>We define a public static method that returns the collection (<see cref="IEnumerable{T}"/>) of 
        /// parameter sets as object arrays, <see cref="object[]"/>. In this case, the method is named <see cref="Dataset_OperatorPlus"/>.
        /// We then add the <see cref="TheoryAttribute"/> attribute to the method + one or more <see cref="MemberDataAttribute"/>
        /// attributes, in which we specified name of the methods that must be called to obtain parameter sets.</para></summary>
        /// <param name="aV">1st parameter of the test (the first summand).</param>
        /// <param name="bV">2nd parameter fo the test (the second summand).</param>
        /// <param name="expectedV">3rd parameter of the test (the expected result).</param>
        [Theory]
        [MemberData(nameof(Dataset_OperatorPlus))]
        protected void TestDemo_AddComplexVectors_Test(ComplexVector aV, ComplexVector bV, ComplexVector expectedV)
        {
            double operationTolerance = 1e-6;
            Output.WriteLine($"Testing vector summation via instance method...");
            // Arrange:
            ComplexVector a = aV;
            ComplexVector b = bV;
            ComplexVector expectedResult = expectedV; // as ComplexVector
            ComplexVector result = new ComplexVector(a.Dim);
            double tolerance = operationTolerance * (a.Norm() + b.Norm()).Re;
            Output.WriteLine($"First summand:   {a}");
            Output.WriteLine($"Second summand:  {b}");
            Output.WriteLine($"Expected result: {expectedResult}");
            Output.WriteLine($"Tolerance: {tolerance}");
            // Act
            a.AddVector(b, result);
            Output.WriteLine($"Result:   {result}");
            Output.WriteLine($"Expected: {expectedResult}");
            // Assert:
            result.Should().NotBeNull();
            result.Dim.Should().Be(expectedResult.Dim);
            for (int i = 0; i < result.Dim; ++i)
            {
                result[i].Re.Should().BeApproximately(expectedResult[i].Re, tolerance);
            }
        }


        /// <summary>This test is similar as the previous one (<see cref="TestDemo_AddComplexVectors_Test(ComplexVector, ComplexVector, ComplexVector)"/>),
        /// except that parameter sets to be tested are specified by listing several methods that generate test parameter sets.</summary>
        [Theory]
        [MemberData(nameof(Dataset_OperatorPlus_1))]
        [MemberData(nameof(Dataset_OperatorPlus_2))]
        [MemberData(nameof(Dataset_OperatorPlus_3))]
        protected void TestDemo_AddComplexVectors_WithSeparateParameterSetMethods_Test(ComplexVector aV, ComplexVector bV, ComplexVector expectedV)
        {
            // call the same method as above, just with different datasets:
            TestDemo_AddComplexVectors_Test(aV, bV, expectedV);
        }

        #region NewExamples

        #endregion NewExamples


        #region Sandbox_Tests

        // ToDo: delete methods in  this region later, when not needed any more (do not delete the region itself)



        #endregion Sandbox_Tests



    }

}

#endif // if false