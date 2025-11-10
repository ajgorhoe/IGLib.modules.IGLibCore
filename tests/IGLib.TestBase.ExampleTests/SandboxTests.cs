
#nullable disable

#if true

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System.Collections.Generic;

using LearnCs.Lib;
using System;

namespace IGLib.Tests.Base
{

    /// <summary>This is a sandbox for testing code of interest.</summary>
    public class SandbboxTests : TestBase<SandbboxTests>
    {
        public SandbboxTests(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output property.
        }



        #region TestSamples

        [Fact]  
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

        [Theory]                 
        [InlineData(2, 8, 10)]  
        [InlineData(0, 0, 0)]
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



        [Theory]
        [InlineData(
            new double[] { 1.1, 1.2, 1.3 },
            new double[] { 2.1, 2.2, 2.3 },
            new double[] { 3.2, 3.4, 3.6 })]
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

        #endregion TestSamples

        #region NewExamples



        #endregion NewExamples


        #region Sandbox_Tests

        // ToDo: delete methods in  this region later, when not needed any more (do not delete the region itself)

        #region Sandbox_Tests.HowREPL_Works



        public class Submission_1
{
            public int a = 22;
        }

        public class Submission_2
        {
            public Submission_1 previous;
            public object Execute()
            {
                previous.a = 55;
                System.Console.WriteLine($"a = {previous.a}");
                return null;
            }
        }

        public class Submission_3
        {
            public Submission_2 previous;

            int a = 22;

            static class MyClass { public static int x = 100; }

            //double Pr => (() =>
            //{
            //    var ret = a + b;
            //    return (double)ret;
            //})();

            public object Execute()
            {

                System.Console.WriteLine($"  a = {a}");
                a = 55;
                System.Console.WriteLine($"  a = {a}");
                a = 66;
                System.Console.WriteLine($"  a = {a}");
                System.Console.WriteLine($"  Value of MyClass.x: {MyClass.x}");

                return null;
            }
        }



        public class Test
        {
            
            static double a = 1.2, b = 3.4;

            Func<double> F = (() =>
            {
                double sum = a + b;
                System.Console.WriteLine($"Computed {sum}");
                return sum;
            });

            double Pr1 => F();

            //public double Pr => (() =>
            //{
            //    var sum = a + b;
            //    System.Console.WriteLine($"Computed {sum}");
            //    return sum;
            //})();

        }




        #endregion Sandbox_Tests.HowREPL_Works





        #endregion Sandbox_Tests



    }

}

#endif // if false