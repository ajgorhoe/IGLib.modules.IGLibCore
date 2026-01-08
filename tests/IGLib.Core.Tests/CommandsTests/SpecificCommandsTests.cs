
#nullable disable

using Xunit;
using FluentAssertions;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using IGLib.Tests.Base;
using IGLib.Commands.Tests;
using System.Text;
using System.Linq;
using IG.Lib;

namespace IGLib.Commands.Tests
{

    /// <summary>Tests for specific generic commands (implementations of <see cref="IGenericCommand"/>).</summary>
    public class SpecificCommandsTests : TestBase<SpecificCommandsTests>
    {


        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> and <see cref="TestBase{TestClassType}.Console"/> properties.</summary>
        /// <param name=""></param>
        public SpecificCommandsTests(ITestOutputHelper output) :  base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output and Console property.
        }


        #region SpecificCommands

        #region CommandSum

        [Theory]
        [InlineData(null, 0)]  // null parameters
        [InlineData(new object[] {  }, 0)]  // empty array
        // double parameters:
        [InlineData(new object[] { 5.4 }, 5.4)]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, 6)]
        [InlineData(new object[] { 1.43, 64.33, 4.56 }, 1.43 + 64.33 + 4.56)]
        // int parameters:
        [InlineData(new object[] { (int)5 }, 5)]
        [InlineData(new object[] { (int)5, (int)6, (int)7 }, 18)]
        // string parameters:
        [InlineData(new object[] { "5", "6", "7" }, 18)]
        [InlineData(new object[] { "5.35", "6", "7" }, 18.35)]
        // boolean parameters:
        [InlineData(new object[] { true },  1.0)]
        [InlineData(new object[] { false }, 0.0)]
        [InlineData(new object[] { true, false, true, true }, 3.0)]

        // mixed types parameters:
        [InlineData(new object[] { 5.0, (int)3, 2.5, (int)4 }, 14.5)]
        [InlineData(new object[] { 5.0, "3", "2.5", (int)4 }, 14.5)]
        
        // parameters with nulls:
        [InlineData(new object[] { null, 5.0, null, (int)3 }, 8.0)]
        [InlineData(new object[] { null, 5, 6, 3 }, 5 + 6 + 3)]
        [InlineData(new object[] { 5, 6, 3, null }, 5 + 6 + 3)]

        // cannot convert parameters:
        [InlineData(new object[] { "abc", "def" }, 0.0, true, typeof(FormatException))]
        [InlineData(new object[] { 'a' }, 0.0, true, typeof(InvalidOperationException))] 

        protected void SpecificCommand_CommandSum_WorksCorrectly(object[] parameters, double expectedResult, 
            bool shouldThrow = false, Type expectedExceptionType = null)
        {
            // Arrange:
            Console.WriteLine($"Test of command CommandSum:");
            Console.WriteLine("Parameters:");
            if (parameters == null)
            {
                Console.WriteLine("  null");
            } else if (parameters.Length == 0)
            {
                Console.WriteLine("  Empty array.");
            } else
            {
                for (int i = 0; i < parameters.Length; ++i)
                {
                    Console.WriteLine($"  [{i}]: {parameters[i]}, type: {parameters[i]?.GetType()}");
                }
            }
            Console.WriteLine($"Expected result: {expectedResult}");
            if (shouldThrow)
            {
                Console.WriteLine("An EXCEPTION is expected to be thrown during execution.");
                if (expectedExceptionType != null)
                {
                    Console.WriteLine($"  Expected exception type: {expectedExceptionType.Name}");
                }
            }
            Console.WriteLine("");
            IGenericCommand cmd = new CommandSum();
            cmd.Should().NotBeNull(because: $"PRECOND: It must be possible to create command of type CommandSum.");
            bool wasExceptionThrown = false;
            Type exceptionType = null;
            object result = null;
            // Act:
            try
            {
                result = cmd.Execute(parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION {ex.GetType().Name} thrown: {ex.Message}");
                wasExceptionThrown = true;
                exceptionType = ex.GetType();
            }
            Console.WriteLine($"Obtained result: {result}, type: {result?.GetType()}");
            // Assert:
            if (!wasExceptionThrown)
            {
                result.Should().BeOfType<double>(because: "CommandSum must return a double value.");
                double? dResultNullable = (double?)result;
                dResultNullable.Should().NotBeNull(because: "CommandSum must return a non-null double value.");
                double dResult = dResultNullable.Value;
                dResult.Should().Be(expectedResult);
            }
            wasExceptionThrown.Should().Be(shouldThrow, because: 
                shouldThrow ? "an exception was expected to be thrown." : "no exception was expected to be thrown");
            if (wasExceptionThrown && expectedExceptionType != null)
            {
                exceptionType.Should().Be(expectedExceptionType, because: 
                    $"the exception type must be {expectedExceptionType.Name}");
            }
        }

        [Fact]
        protected void SpecificCommand_CommandSum_HasConsistentPropertiesAtCreation()
        {
            // Arrange:
            Console.WriteLine($"Testing that command CommandSum has correct properties after creation:");
            // Act:
            IGenericCommand cmd = new CommandSum();
            // Assert:
            cmd.Should().NotBeNull(because: "it must be possible to create an instance of command");
            cmd.StringId.Should().NotBeNull(because: "created command should have a proper StringId");
            cmd.StringId.Should().Contain(cmd.Id.ToString(), because: "the StringId property should contain the unique ID of the command object");
            cmd.StringId.Should().Contain(cmd.GetType().Name, because: "the StringId property should contain the name of the type");
            cmd.Description.Should().NotBeNullOrEmpty(because: "this command should have a proper description");
            cmd.Description.Length.Should().BeGreaterThan(40, because: "the description should be reasonably long to provide useful information");
            cmd.Description.Should().Contain("sum", because: "the description should mention that the command computes sum of its arguments");
            cmd.Description.Should().Contain("sum", because: "the description should mention that the command computes sum of its arguments");
            cmd.Description.Should().Contain("return", because: "the description should describe the returned value");
            cmd.Description.Should().Contain("return", because: "the description should describe the returned value");
            cmd.Description.Should().Contain("arguments", because: "the description should mention the meaning of arguments");
            cmd.Description.Should().Contain("exception", because: "the description should mention situations where exception is thrown");
        }

        [Fact]
        protected void SpecificCommand_CommandSum_CanBeAssignedDescription()
        {
            // Arrange:
            Console.WriteLine($"Testing that a different Description can be assigned to CommandSum:");
            string newDescription = "This is a new description for this command, which should replace the default one.";
            // Act:
            IGenericCommand cmd = new CommandSum(description: newDescription);
            cmd.Should().NotBeNull(because: "PRECOND: it must be possible to create an instance of command");
            Console.WriteLine($"Command's Description: {cmd.Description}");
            // Assert:
            cmd.Description.Should().NotBeNull(because: "non-null description was assigned to the command");
            cmd.Description.Should().Be(newDescription, because: "the assigned description should be the one stored in the command");
        }

        [Fact]
        protected void SpecificCommand_CommandSum_CanBeAssignedDescriptionUrl()
        {
            // Arrange:
            Console.WriteLine($"Testing that a different DescriptionUrl can be assigned to CommandSum:");
            string newDescriptionUrl = "http://www.example.com/CommandManual#MyCommand";
            // Act:
            GenericCommandBase cmd = new CommandSum(descriptionUrl: newDescriptionUrl);
            cmd.Should().NotBeNull(because: "PRECOND: it must be possible to create an instance of command");
            Console.WriteLine($"Command's DescriptionUrl: {cmd.DescriptionUrl}");
            // Assert:
            cmd.DescriptionUrl.Should().NotBeNull(because: "non-null description was assigned to the command");
            cmd.DescriptionUrl.Should().Be(newDescriptionUrl, because: "the assigned DescriptionUrl should be the one stored in the command");
        }

        #endregion CommandSum



        #region CommandProduct

        [Theory]
        [InlineData(null,             1)]  // null parameters
        [InlineData(new object[] { }, 1)]  // empty array
        // double parameters:
        [InlineData(new object[] { 5.4 }, 5.4)]
        [InlineData(new object[] { 1.0, 2.0, 3.0, 4.0 }, 24.0)]
        [InlineData(new object[] { 1.43, 64.33, 4.56 }, 1.43 * 64.33 * 4.56)]
        // int parameters:
        [InlineData(new object[] { (int)5 }, 5)]
        [InlineData(new object[] { (int)5, (int)6, (int)7 }, 5 * 6 * 7)]
        // string parameters:
        [InlineData(new object[] { "5", "6", "7" }, 5 * 6 * 7)]
        [InlineData(new object[] { "5.35", "6", "7" }, 5.35 * 6 * 7)]
        // boolean parameters:
        [InlineData(new object[] { true }, 1.0)]
        [InlineData(new object[] { false }, 0.0)]
        [InlineData(new object[] { true, false, true, true }, 0.0)] // because of false, which converts to 0.0
        [InlineData(new object[] { true, true, true, true }, 1.0)]  // because all elements are truw, which converts to 1.0

        // mixed types parameters:
        [InlineData(new object[] { 5.0, (int)3, 2.5, (int)4 }, 5 * 3 * 2.5 * 4)]
        [InlineData(new object[] { 5.0, "3", "2.5", (int)4 }, 5 * 3 * 2.5 * 4)]

        // parameters with nulls:
        [InlineData(new object[] { null, 5.0, null, (int)3 }, 5.0 * 3)]
        [InlineData(new object[] { null, 5, 6, 3 }, 5 * 6 * 3)]
        [InlineData(new object[] { 5, 6, 3, null }, 5 * 6 * 3)]

        // cannot convert parameters:
        [InlineData(new object[] { "abc", "def" }, 0.0, true, typeof(FormatException))]
        [InlineData(new object[] { 'a' }, 0.0, true, typeof(InvalidOperationException))]
        protected void SpecificCommand_CommandProduct_WorksCorrectly(object[] parameters, double expectedResult, 
            bool shouldThrow = false, Type expectedExceptionType = null)
        {
            // Arrange:
            Console.WriteLine($"Test of command CommandProduct:");
            Console.WriteLine("Parameters:");
            if (parameters == null)
            {
                Console.WriteLine("  null");
            } else if (parameters.Length == 0)
            {
                Console.WriteLine("  Empty array.");
            } else
            {
                for (int i = 0; i < parameters.Length; ++i)
                {
                    Console.WriteLine($"  [{i}]: {parameters[i]}, type: {parameters[i]?.GetType()}");
                }
            }
            Console.WriteLine($"Expected result: {expectedResult}");
            if (shouldThrow)
            {
                Console.WriteLine("An EXCEPTION is expected to be thrown during execution.");
                if (expectedExceptionType != null)
                {
                    Console.WriteLine($"  Expected exception type: {expectedExceptionType.Name}");
                }
            }
            Console.WriteLine("");
            IGenericCommand cmd = new CommandProduct();
            cmd.Should().NotBeNull(because: $"PRECOND: It must be possible to create command of type CommandSum.");
            bool wasExceptionThrown = false;
            Type exceptionType = null;
            object result = null;
            // Act:
            try
            {
                result = cmd.Execute(parameters);
                Console.WriteLine($"Obtained result: {result}, type: {result?.GetType()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION {ex.GetType().Name} thrown: {ex.Message}");
                wasExceptionThrown = true;
                exceptionType = ex.GetType();
            }
            // Assert:
            if (!wasExceptionThrown)
            {
                result.Should().BeOfType<double>(because: "CommandSum must return a double value.");
                double? dResultNullable = (double?)result;
                dResultNullable.Should().NotBeNull(because: "CommandSum must return a non-null double value.");
                double dResult = dResultNullable.Value;
                dResult.Should().Be(expectedResult);
            }
            wasExceptionThrown.Should().Be(shouldThrow, because: 
                shouldThrow ? "an exception was expected to be thrown." : "no exception was expected to be thrown");
            if (wasExceptionThrown && expectedExceptionType != null)
            {
                exceptionType.Should().Be(expectedExceptionType, because: 
                    $"the exception type must be {expectedExceptionType.Name}");
            }
        }

        [Fact]
        protected void SpecificCommand_CommandProduct_HasConsistentPropertiesAtCreation()
        {
            // Arrange:
            Console.WriteLine($"Testing that command CommandProduct has correct properties after creation:");
            // Act:
            IGenericCommand cmd = new CommandProduct();
            // Assert:
            cmd.Should().NotBeNull(because: "it must be possible to create an instance of command");
            cmd.StringId.Should().NotBeNull(because: "created command should have a proper StringId");
            cmd.StringId.Should().Contain(cmd.Id.ToString(), because: "the StringId property should contain the unique ID of the command object");
            cmd.StringId.Should().Contain(cmd.GetType().Name, because: "the StringId property should contain the name of the type");
            cmd.Description.Should().NotBeNullOrEmpty(because: "this command should have a proper description");
            cmd.Description.Length.Should().BeGreaterThan(40, because: "the description should be reasonably long to provide useful information");
            cmd.Description.Should().Contain("product", because: "the description should mention that the command computes product of its arguments");
            cmd.Description.Should().Contain("product", because: "the description should mention that the command computes product of its arguments");
            cmd.Description.Should().Contain("return", because: "the description should describe the returned value");
            cmd.Description.Should().Contain("return", because: "the description should describe the returned value");
            cmd.Description.Should().Contain("arguments", because: "the description should mention the meaning of arguments");
            cmd.Description.Should().Contain("exception", because: "the description should mention situations where exception is thrown");
        }

        [Fact]
        protected void SpecificCommand_CommandProduct_CanBeAssignedDescription()
        {
            // Arrange:
            Console.WriteLine($"Testing that a different Description can be assigned to CommandProduct:");
            string newDescription = "This is a new description for this command, which should replace the default one.";
            // Act:
            IGenericCommand cmd = new CommandProduct(description: newDescription);
            cmd.Should().NotBeNull(because: "PRECOND: it must be possible to create an instance of command");
            Console.WriteLine($"Command's Description: {cmd.Description}");
            // Assert:
            cmd.Description.Should().NotBeNull(because: "non-null description was assigned to the command");
            cmd.Description.Should().Be(newDescription, because: "the assigned description should be the one stored in the command");
        }

        [Fact]
        protected void SpecificCommand_CommandProduct_CanBeAssignedDescriptionUrl()
        {
            // Arrange:
            Console.WriteLine($"Testing that a different DescriptionUrl can be assigned to CommandProduct:");
            string newDescriptionUrl = "http://www.example.com/CommandManual#MyCommand";
            // Act:
            GenericCommandBase cmd = new CommandProduct(descriptionUrl: newDescriptionUrl);
            cmd.Should().NotBeNull(because: "PRECOND: it must be possible to create an instance of command");
            Console.WriteLine($"Command's DescriptionUrl: {cmd.DescriptionUrl}");
            // Assert:
            cmd.DescriptionUrl.Should().NotBeNull(because: "non-null description was assigned to the command");
            cmd.DescriptionUrl.Should().Be(newDescriptionUrl, because: "the assigned DescriptionUrl should be the one stored in the command");
        }

        #endregion CommandProduct



        #region CommandAverage

        [Theory]
        [InlineData(null,             double.NaN)]  // null parameters
        [InlineData(new object[] { }, double.NaN)]  // empty array
        // double parameters:
        [InlineData(new object[] { 5.4 }, 5.4)]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, 6 / 3)]
        [InlineData(new object[] { 1.43, 64.33, 4.56 }, (1.43 + 64.33 + 4.56) / 3.0)]
        // int parameters:
        [InlineData(new object[] { (int)5 }, 5)]
        [InlineData(new object[] { (int)5, (int)6, (int)7 }, 18 / 3)]
        // string parameters:
        [InlineData(new object[] { "5", "6", "7" }, 18 / 3)]
        [InlineData(new object[] { "5.35", "6", "7" }, 18.35 / 3)]
        // boolean parameters:
        [InlineData(new object[] { true }, 1.0)]
        [InlineData(new object[] { false }, 0.0)]
        [InlineData(new object[] { true, false, true, true }, 3.0 / 4.0)]

        // mixed types parameters:
        [InlineData(new object[] { 5.0, (int)3, 2.5, (int)4 }, 14.5 / 4)]
        [InlineData(new object[] { 5.0, "3", "2.5", (int)4 }, 14.5 / 4)]

        // parameters with nulls:
        [InlineData(new object[] { null, 5.0, null, (int)3 }, 8.0 / 2)]
        [InlineData(new object[] { null, 5, 6, 3 }, (double)(5 + 6 + 3) / 3)]
        [InlineData(new object[] { 5, 6, 3, null }, (double)(5 + 6 + 3) / 3)]

        // cannot convert parameters:
        [InlineData(new object[] { "abc", "def" }, 0.0, true, typeof(FormatException))]
        [InlineData(new object[] { 'a' }, 0.0, true, typeof(InvalidOperationException))]
        protected void SpecificCommand_CommandAverage_WorksCorrectly(object[] parameters, double expectedResult,
            bool shouldThrow = false, Type expectedExceptionType = null)
        {
            // Arrange:
            Console.WriteLine($"Test of command CommandAverage:");
            Console.WriteLine("Parameters:");
            if (parameters == null)
            {
                Console.WriteLine("  null");
            }
            else if (parameters.Length == 0)
            {
                Console.WriteLine("  Empty array.");
            }
            else
            {
                for (int i = 0; i < parameters.Length; ++i)
                {
                    Console.WriteLine($"  [{i}]: {parameters[i]}, type: {parameters[i]?.GetType()}");
                }
            }
            Console.WriteLine($"Expected result: {expectedResult}");
            if (shouldThrow)
            {
                Console.WriteLine("An EXCEPTION is expected to be thrown during execution.");
                if (expectedExceptionType != null)
                {
                    Console.WriteLine($"  Expected exception type: {expectedExceptionType.Name}");
                }
            }
            Console.WriteLine("");
            IGenericCommand cmd = new CommandAverage();
            cmd.Should().NotBeNull(because: $"PRECOND: It must be possible to create command of this type.");
            // Act:
            bool wasExceptionThrown = false;
            Type exceptionType = null;
            object result = null;
            try
            {
                result = cmd.Execute(parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EXCEPTION {ex.GetType().Name} thrown: {ex.Message}");
                wasExceptionThrown = true;
                exceptionType = ex.GetType();
            }
            Console.WriteLine($"Obtained result: {result}, type: {result?.GetType()}");
            // Assert:
            if (!wasExceptionThrown)
            {
                result.Should().BeOfType<double>(because: "CommandAverage must return a double value.");
                double? dResultNullable = (double?)result;
                dResultNullable.Should().NotBeNull(because: "CommandAverage must return a non-null double value.");
                double dResult = dResultNullable.Value;
                dResult.Should().Be(expectedResult);
            }
            wasExceptionThrown.Should().Be(shouldThrow, because:
                shouldThrow ? "an exception was expected to be thrown." : "no exception was expected to be thrown");
            if (wasExceptionThrown && expectedExceptionType != null)
            {
                exceptionType.Should().Be(expectedExceptionType, because:
                    $"the exception type must be {expectedExceptionType.Name}");
            }
        }

        [Fact]
        protected void SpecificCommand_CommandAverage_HasConsistentPropertiesAtCreation()
        {
            // Arrange:
            Console.WriteLine($"Testing that command CommandAverage has correct properties after creation:");
            // Act:
            IGenericCommand cmd = new CommandAverage();
            // Assert:
            cmd.Should().NotBeNull(because: "it must be possible to create an instance of command");
            Console.WriteLine($"Command ID: {cmd.Id}");
            Console.WriteLine($"Command StringId: {cmd.StringId}");
            Console.WriteLine($"Command's Description: {cmd.StringId}");
            Console.WriteLine($"Command's DescriptionUrl: {cmd.StringId}");
            cmd.StringId.Should().NotBeNull(because: "created command should have a proper StringId");
            cmd.StringId.Should().Contain(cmd.Id.ToString(), because: "the StringId property should contain the unique ID of the command object");
            cmd.StringId.Should().Contain(cmd.GetType().Name, because: "the StringId property should contain the name of the type");
            cmd.Description.Should().NotBeNullOrEmpty(because: "this command should have a proper description");
            cmd.Description.Length.Should().BeGreaterThan(40, because: "the description should be reasonably long to provide useful information");
            cmd.Description.Should().Contain("average", because: "the description should mention that the command computes average of its arguments");
            cmd.Description.Should().Contain("average", because: "the description should mention that the command computes average of its arguments");
            cmd.Description.Should().Contain("return", because: "the description should describe the returned value");
            cmd.Description.Should().Contain("return", because: "the description should describe the returned value");
            cmd.Description.Should().Contain("arguments", because: "the description should mention the meaning of arguments");
            cmd.Description.Should().Contain("exception", because: "the description should mention situations where exception is thrown");
        }

        [Fact]
        protected void SpecificCommand_CommandAverage_CanBeAssignedDescription()
        {
            // Arrange:
            Console.WriteLine($"Testing that a different Description can be assigned to CommandAverage:");
            string newDescription = "This is a new description for this command, which should replace the default one.";
            // Act:
            IGenericCommand cmd = new CommandAverage(description: newDescription);
            cmd.Should().NotBeNull(because: "PRECOND: it must be possible to create an instance of command");
            Console.WriteLine($"Command's Description: {cmd.Description}");
            // Assert:
            cmd.Description.Should().NotBeNull(because: "non-null description was assigned to the command");
            cmd.Description.Should().Be(newDescription, because: "the assigned description should be the one stored in the command");
        }

        [Fact]
        protected void SpecificCommand_CommandAverage_CanBeAssignedDescriptionUrl()
        {
            // Arrange:
            Console.WriteLine($"Testing that a different DescriptionUrl can be assigned to CommandAverage:");
            string newDescriptionUrl = "http://www.example.com/CommandManual#MyCommand";
            // Act:
            GenericCommandBase cmd = new CommandAverage(descriptionUrl: newDescriptionUrl);
            cmd.Should().NotBeNull(because: "PRECOND: it must be possible to create an instance of command");
            Console.WriteLine($"Command's DescriptionUrl: {cmd.DescriptionUrl}");
            // Assert:
            cmd.DescriptionUrl.Should().NotBeNull(because: "non-null description was assigned to the command");
            cmd.DescriptionUrl.Should().Be(newDescriptionUrl, because: "the assigned DescriptionUrl should be the one stored in the command");
        }

        #endregion CommandAverage


        #endregion SpecificCommands


    }




}

