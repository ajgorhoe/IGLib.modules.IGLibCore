
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

namespace IGLib.Commands.Tests
{

    /// <summary>Tests for generic commands (implementations of <see cref="IGenericCommand"/>).</summary>
    public class GenericCommandTests : TestBase<GenericCommandTests>
    {


        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> and <see cref="TestBase{TestClassType}.Console"/> properties.</summary>
        /// <param name=""></param>
        public GenericCommandTests(ITestOutputHelper output) :  base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output and Console property.
        }


        #region CommandCreation

        [Fact]
        protected void CommandCreation_CommandsHaveDistinctIDsGrowingWithCreationTime()
        {
            // Arrange:
            Console.WriteLine("Testing that created commands have distinct IDs and that IDs grow with time of creation:\n");
            // Act:
            IGenericCommand cmd1 = new TestCmd1();
            Console.WriteLine($"Created command {nameof(cmd1)} of type {cmd1?.GetType()?.Name}, ID = {cmd1?.Id}");
            IGenericCommand cmd2 = new TestCmd2();
            Console.WriteLine($"Created command {nameof(cmd2)} of type {cmd2?.GetType()?.Name}, ID = {cmd2?.Id}");
            IGenericCommand cmd3 = new TestCmd3();
            Console.WriteLine($"Created command {nameof(cmd3)} of type {cmd3?.GetType()?.Name}, ID = {cmd3?.Id}");
            cmd1.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd1)}");
            cmd2.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd2)}");
            cmd3.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd3)}");
            // Assert:
            cmd1.Id.Should().NotBe(cmd2.Id, because: "cmd1 and cmd2 are different instances and must have different IDs");
            cmd1.Id.Should().NotBe(cmd3.Id, because: "cmd1 and cmd3 are different instances and must have different IDs");
            cmd2.Id.Should().NotBe(cmd3.Id, because: "cmd2 and cmd3 are different instances and must have different IDs");
            cmd2.Id.Should().BeGreaterThan(cmd1.Id, because: "cmd2 was created later than cmd1 and should have greater ID.");
            cmd3.Id.Should().BeGreaterThan(cmd2.Id, because: "cmd2 was created later than cmd1 and should have greater ID.");
        }

        [Fact]
        protected void CommandCreation_CommandsOfSameTypeHaveDistinctIDsGrowingWithCreationTime()
        {
            // Arrange:
            int numCreatedCommands = 200;
            Console.WriteLine($"Testing that of {numCreatedCommands} created commands of the same type, \nall have distinct IDs that grow with time of creation:\n");
            // Act:
            List<IGenericCommand> commands = new List<IGenericCommand>();
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("IDs of created commands:");
            for (int i = 0; i < numCreatedCommands; ++i)
            {
                commands.Add(new TestCmd1());
                commands[i].Should().NotBeNull(because: $"PRECOND: It must be possible to create a command of type {nameof(TestCmd1)}");
                if (i % 10 == 0)
                {
                    sb.AppendLine();
                }
                sb.Append($"[{i}]: {commands[i].Id}; ");
            }
            Console.WriteLine(sb.ToString());
            // Assert:
            for (int i = 0; i < numCreatedCommands-1; ++i)
            {
                commands[i+1].Id.Should().BeGreaterThan(commands[i].Id, because: $"Command IDs should be in ascending order with respect to order of creation");
            }
        }

        [Fact]
        protected void CommandCreation_CommandsOfDifferentTypesHaveDistinctIDsGrowingWithCreationTime()
        {
            // Arrange:
            int numCreatedCommands = 200;
            Console.WriteLine($"Testing that of {numCreatedCommands} created commands of different types, all have distinct IDs that grow with time of creation:\n");
            // Act:
            List<IGenericCommand> commands = new List<IGenericCommand>();
            StringBuilder sb = new StringBuilder();
            Console.WriteLine("IDs of created commands:");
            IGenericCommand cmd;
            int numCommandTypes = 6;
            for (int i = 0; i < numCreatedCommands; ++i)
            {
                switch ((i) % numCommandTypes)
                {
                    case 0: cmd = new TestCmd1(); break;
                    case 1: cmd = new TestCmd2(); break;
                    case 2: cmd = new TestCmd3(); break;
                    case 3: cmd = new TestCmd4(); break;
                    default: cmd = new TestCmd5(); break;  // appears twice more often
                }
                commands.Add(cmd);
                commands[i].Should().NotBeNull(because: $"PRECOND: It must be possible to create a command of type {nameof(TestCmd1)}");
                if (i % 10 == 0)
                {
                    sb.AppendLine();
                }
                sb.Append($"[{i}]: {commands[i].Id}; ");
            }
            Console.WriteLine(sb.ToString());
            // Assert:
            for (int i = 0; i < numCreatedCommands-1; ++i)
            {
                commands[i+1].Id.Should().BeGreaterThan(commands[i].Id, because: $"Command IDs should be in ascending order with respect to order of creation");
            }
        }

        [Fact]
        protected void CommandCreation_CommandsOfDifferentTypesDerivedFromCommandBaseHaveConsistentProperties()
        {
            // Arrange:
            Console.WriteLine($"Testing that created commands of different types based on {nameof(GenericCommandBase)}\n  have consistent properties:\n");
            // Act:
            IGenericCommand cmd1 = new TestCmd1();
            Console.WriteLine($"Created command {nameof(cmd1)} of type {cmd1?.GetType()?.Name}, ID = {cmd1?.Id}");
            Console.WriteLine($"  StringId: {cmd1?.StringId}, Description: {cmd1?.Description}");
            IGenericCommand cmd2 = new TestCmd2();
            Console.WriteLine($"Created command {nameof(cmd2)} of type {cmd2?.GetType()?.Name}, ID = {cmd2?.Id}");
            Console.WriteLine($"  StringId: {cmd2?.StringId}, Description: {cmd2?.Description}");
            IGenericCommand cmd3 = new TestCmd3();
            Console.WriteLine($"Created command {nameof(cmd3)} of type {cmd3?.GetType()?.Name}, ID = {cmd3?.Id}");
            Console.WriteLine($"  StringId: {cmd3?.StringId}, Description: {cmd3?.Description}");
            cmd1.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd1)}");
            cmd2.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd2)}");
            cmd3.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd3)}");
            // Assert:
            cmd1.StringId.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have StringId defined by default.");
            cmd2.StringId.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have StringId defined by default.");
            cmd3.StringId.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have StringId defined by default.");
            
            cmd1.Description.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have Description defined by default.");
            cmd2.Description.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have Description defined by default.");
            cmd3.Description.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have Description defined by default.");
            
            cmd1.StringId.Should().NotBe(cmd2.StringId, because: "cmd1 and cmd2 are different instances and must have different StringId-s");
            cmd1.StringId.Should().NotBe(cmd3.StringId, because: "cmd1 and cmd3 are different instances and must have different StringId-s");
            cmd2.StringId.Should().NotBe(cmd3.StringId, because: "cmd2 and cmd3 are different instances and must have different StringId-s");
            cmd1.StringId.Should().Contain(cmd1.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.StringId)}.");
            cmd2.StringId.Should().Contain(cmd2.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.StringId)}.");
            cmd3.StringId.Should().Contain(cmd3.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.StringId)}.");
            cmd1.StringId.Should().Contain(cmd1.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.StringId)}.");
            cmd2.StringId.Should().Contain(cmd2.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.StringId)}.");
            cmd3.StringId.Should().Contain(cmd3.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.StringId)}.");

            cmd1.Description.Should().NotBe(cmd2.Description, because: "cmd1 and cmd2 are different instances and must have different descriptions");
            cmd1.Description.Should().NotBe(cmd3.Description, because: "cmd1 and cmd3 are different instances and must have different descriptions");
            cmd2.Description.Should().NotBe(cmd3.Description, because: "cmd2 and cmd3 are different instances and must have different descriptions");
            cmd1.Description.Should().Contain(cmd1.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.Description)}.");
            cmd2.Description.Should().Contain(cmd2.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.Description)}.");
            cmd3.Description.Should().Contain(cmd3.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.Description)}.");
            cmd1.Description.Should().Contain(cmd1.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.Description)}.");
            cmd2.Description.Should().Contain(cmd2.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.Description)}.");
            cmd3.Description.Should().Contain(cmd3.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.Description)}.");
        }

        [Fact]
        protected void CommandCreation_CommandsOfSameTypeDerivedFromCommandBaseHaveConsistentProperties()
        {
            // Arrange:
            Console.WriteLine($"Testing that created commands of the same type based on {nameof(GenericCommandBase)}\n  have consistent properties:\n");
            // Act:
            IGenericCommand cmd1 = new TestCmd1();
            Console.WriteLine($"Created command {nameof(cmd1)} of type {cmd1?.GetType()?.Name}, ID = {cmd1?.Id}");
            Console.WriteLine($"  StringId: {cmd1?.StringId}, Description: {cmd1?.Description}");
            IGenericCommand cmd2 = new TestCmd2();
            Console.WriteLine($"Created command {nameof(cmd2)} of type {cmd2?.GetType()?.Name}, ID = {cmd2?.Id}");
            Console.WriteLine($"  StringId: {cmd2?.StringId}, Description: {cmd2?.Description}");
            IGenericCommand cmd3 = new TestCmd3();
            Console.WriteLine($"Created command {nameof(cmd3)} of type {cmd3?.GetType()?.Name}, ID = {cmd3?.Id}");
            Console.WriteLine($"  StringId: {cmd3?.StringId}, Description: {cmd3?.Description}");
            cmd1.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd1)}");
            cmd2.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd2)}");
            cmd3.Should().NotBeNull(because: $"PRECOND: It must be able to create a command of type {nameof(TestCmd3)}");
            // Assert:
            cmd1.StringId.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have StringId defined by default.");
            cmd2.StringId.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have StringId defined by default.");
            cmd3.StringId.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have StringId defined by default.");

            cmd1.Description.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have Description defined by default.");
            cmd2.Description.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have Description defined by default.");
            cmd3.Description.Should().NotBeNull(because: $"Commands based on {nameof(GenericCommandBase)} must have Description defined by default.");

            cmd1.StringId.Should().NotBe(cmd2.StringId, because: "cmd1 and cmd2 are different instances and must have different StringId-s");
            cmd1.StringId.Should().NotBe(cmd3.StringId, because: "cmd1 and cmd3 are different instances and must have different StringId-s");
            cmd2.StringId.Should().NotBe(cmd3.StringId, because: "cmd2 and cmd3 are different instances and must have different StringId-s");
            cmd1.StringId.Should().Contain(cmd1.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.StringId)}.");
            cmd2.StringId.Should().Contain(cmd2.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.StringId)}.");
            cmd3.StringId.Should().Contain(cmd3.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.StringId)}.");
            cmd1.StringId.Should().Contain(cmd1.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.StringId)}.");
            cmd2.StringId.Should().Contain(cmd2.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.StringId)}.");
            cmd3.StringId.Should().Contain(cmd3.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.StringId)}.");

            cmd1.Description.Should().NotBe(cmd2.Description, because: "cmd1 and cmd2 are different instances and must have different descriptions");
            cmd1.Description.Should().NotBe(cmd3.Description, because: "cmd1 and cmd3 are different instances and must have different descriptions");
            cmd2.Description.Should().NotBe(cmd3.Description, because: "cmd2 and cmd3 are different instances and must have different descriptions");
            cmd1.Description.Should().Contain(cmd1.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.Description)}.");
            cmd2.Description.Should().Contain(cmd2.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.Description)}.");
            cmd3.Description.Should().Contain(cmd3.GetType().Name, because: $"Commands based on {nameof(GenericCommandBase)} must contain type name in default {nameof(GenericCommandBase.Description)}.");
            cmd1.Description.Should().Contain(cmd1.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.Description)}.");
            cmd2.Description.Should().Contain(cmd2.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.Description)}.");
            cmd3.Description.Should().Contain(cmd3.Id.ToString(), because: $"Commands based on {nameof(GenericCommandBase)} must contain {nameof(GenericCommandBase.Id)} in {nameof(GenericCommandBase.Description)}.");
        }

        #endregion CommandCreation


    }



}

