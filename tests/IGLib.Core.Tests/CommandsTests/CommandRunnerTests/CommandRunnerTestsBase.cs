
#nullable disable

using FluentAssertions;
using IGLib.Tests.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace IGLib.Commands.Tests
{

    /// <summary>Base test class for classes that test different variants of <see cref="CommandRunnerExtendedUnsafe"/>.
    /// <para>Tests that are the same for all ariants of these classes should be defined in this class.
    /// Other tests should be defined in specific classes.</para>
    /// <para>For each specific class, redefine the method <see cref="CreateRunner"/>, which is used to
    /// create initialized instance of a specific <typeparamref name="TCommandRunner"/> class.</para></summary>
    /// <typeparam name="TCommandRunner">Type of command runner that is tested. This must be concretized in 
    /// specific tests.</typeparam>
    /// <remarks>This class could be made concrete if <see cref="CreateRunner()"/> was made concrete. However,
    /// we don't want this because we don't wand this class to actually execute tests. By class being
    /// abstract, test runner will not actually run the tests for this class, but only for its derived classes.</remarks>
    public abstract class CommandRunnerTestsBase<TExecutionInfo, TCommandRunner> : 
        TestBase<CommandRunnerTestsBase<TExecutionInfo, TCommandRunner>>
        where TCommandRunner: ICommandRunner<IGenericCommand, TExecutionInfo>
        // where TCommand: IGenericCommand
        where TExecutionInfo: IExecutionInfo<IGenericCommand, TExecutionInfo>
    {
        
        
        /// <summary>This constructor, when called by the test framework, will bring in an object 
        /// of type <see cref="ITestOutputHelper"/>, which will be used to write on the tests' output,
        /// accessed through the base class's <see cref="Output"/> property.</summary>
        /// <param name=""></param>
        public CommandRunnerTestsBase(ITestOutputHelper output) :
            base(output)  // calls base class's constructor
        {
            // Remark: the base constructor will assign output parameter to the Output and Console property.
        }

        protected abstract TCommandRunner CreateRunner();
        //{
        //    return new();
        //}

        [Fact]
        protected virtual void Registration_CommandsAreRegisteredCorrectly()
        {
            // Arrange:
            TCommandRunner runner = CreateRunner();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: Runner must not contain any commands.");
            string cmdName1 = TestConst.Cmd1;
            TestCmd1 cmdObj1 = new TestCmd1();
            // Act:
            // Add first command:
            runner.AddCommand(cmdName1, cmdObj1);
            // Assert:
            runner.CommandsCount.Should().Be(1, because: "After adding one command, Runner must contain exactly one command.");
            runner.GetCommandNames().Should().Contain(cmdName1, because: 
                "After adding command named cmdName1, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj1, because: $"The correct command should be registered under name `${cmdName1}`");
            // Arrrange (second command):
            string cmdName2 = TestConst.Cmd2;
            TestCmd2 cmdObj2 = new TestCmd2();
            // Act:
            // Add second command:
            runner.AddCommand(cmdName2, cmdObj2);
            // Addert:
            runner.CommandsCount.Should().Be(2, because: "After adding the second command, the number of commands contained should be 2.");
            runner.GetCommandNames().Should().Contain(cmdName2, because:  "After adding command named cmdName2, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName2).Should().Be(cmdObj2, because: $"The correct command should be registered under name `${cmdName2}`");
            // Registration of new command does not affect previously registered commands:
            runner.GetCommandNames().Should().Contain(cmdName1, because: "Registration of a new command must not affect previously registered commands.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj1, because: $"Previously registered command under name `${cmdName1}` must remain unchanged.");
        }

        [Fact]
        protected virtual void Registration_CommandsCanBeReplaced()
        {
            // Arrange
            TCommandRunner runner = CreateRunner();
            string cmdName1 = TestConst.Cmd1;
            TestCmd1 cmdObj1 = new TestCmd1();
            runner.AddCommand(cmdName1, cmdObj1);
            string cmdName2 = TestConst.Cmd2;
            TestCmd2 cmdObj2 = new TestCmd2();
            runner.AddCommand(cmdName2, cmdObj2);
            runner.CommandsCount.Should().Be(2, because: "PRECOND: Two commands must be registered before testing replacement.");
            runner.GetCommandNames().Should().Contain(cmdName1, because: $"PRECOND: Command {cmdName1} should be registered.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj1, because: $"PRECOND: Command object registered under {cmdName1} should be correct.");
            runner.GetCommandNames().Should().Contain(cmdName2, because: $"PRECOND: Command {cmdName2} should be registered.");
            runner.GetCommand(cmdName2).Should().Be(cmdObj2, because: $"PRECOND: Command object registered under {cmdName2} should be correct.");
            // Act:
            // Replace the first command by another cmdObj3:
            Command3 cmdObj3 = new Command3();
            runner.AddOrReplaceCommand(cmdName1, cmdObj3);
            // Assert:
            // Number of registered commands must remain the same:
            runner.CommandsCount.Should().Be(2, because: "Replacement of a command must not change the number of registered commands.");
            // cmdName1 remains a registered command, but with a different command object:
            runner.GetCommandNames().Should().Contain(cmdName1, because: "After replacement, the command name must remain registered.");
            runner.GetCommand(cmdName1).Should().NotBe(cmdObj1, because: "After replacement, the command object must be different from the original one.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj3, because: $"After replacement, the command object registered under {cmdName1} must be the newly registered one.");
            // Replacement of one command must not affect registration status of the other  commands:
            runner.GetCommandNames().Should().Contain(cmdName2, because: "Replacement of one command must not affect other commands.");
            runner.GetCommand(cmdName2).Should().Be(cmdObj2, because: $"Replacement of one command must not affect other commands; command object under {cmdName2} must remain unchanged.");
            // Act - second replacement:
            // Replace the second command by command object cmdObj4:
            Command4 cmdObj4 = new Command4();
            runner.AddOrReplaceCommand(cmdName2, cmdObj4);
            // Assert:
            // Number of registered commands must remain the same:
            runner.CommandsCount.Should().Be(2, because: "Replacement of a command must not change the number of registered commands.");
            // cmdName2 remains a registered command, but with a different command object:
            runner.GetCommandNames().Should().Contain(cmdName2, because: $"After replacement, the command name {cmdName2} must remain registered.");
            runner.GetCommand(cmdName2).Should().NotBe(cmdObj2, because: $"After replacement, the command object under {cmdName2} must be different from the original one.");
            runner.GetCommand(cmdName2).Should().Be(cmdObj4, because: $"After replacement, the command object registered under {cmdName2} must be the newly registered one.");
            // Again, replacement should not affect other commands:
            runner.GetCommand(cmdName1).Should().Be(cmdObj3, because: $"Replacement of one command must not affect other commands; command object under {cmdName1} must remain unchanged.");
        }

        [Fact]
        protected virtual void Registration_CommandAliasesCanBeDefined()
        {
            // Arrange:
            // Register two commands on a newly created invoker and verify the state:
            TCommandRunner runner = CreateRunner();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: Initially, command runner must not contain any commands.");
            string cmdName1 = TestConst.Cmd1;
            TestCmd1 cmdObj1 = new TestCmd1();
            runner.AddCommand(cmdName1, cmdObj1);
            string cmdName2 = TestConst.Cmd2;
            TestCmd2 cmdObj2 = new TestCmd2();
            runner.AddCommand(cmdName2, cmdObj2);
            // Verify that command runner state is correct after adding two commands:
            runner.CommandsCount.Should().Be(2, because: "PRECOND: After adding two commands, command runner must contain exactly two commands.");
            runner.GetCommandNames().Should().Contain(cmdName1, because: "PRECOND: After adding command named cmdName1, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj1, because: $"PRECOND: The correct command should be registered under name `${cmdName1}`");
            runner.GetCommandNames().Should().Contain(cmdName2, because: "PRECOND: After adding command named cmdName2, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName2).Should().Be(cmdObj2, because: $"PRECOND: The correct command should be registered under name `${cmdName2}`");
            // Act:
            // Define aliases for both registered commands; aliases will reuse the same command instances (not only class)
            // as the original commands: 
            string cmdName1a = TestConst.Cmd1a;
            string cmdName2a = TestConst.Cmd2a;
            runner.AddCommandAlias(cmdName1, cmdName1a);
            runner.AddCommandAlias(cmdName2, cmdName2a);
            // Assert:
            runner.CommandsCount.Should().Be(4, because: "After adding two command aliases, the total number of registered commands must be 4.");
            runner.ContainsCommand(cmdName1a).Should().BeTrue(because: "Aliases must be considered properly registered command names.");
            runner.ContainsCommand(cmdName2a).Should().BeTrue(because: "Aliases must be considered properly registered command names.");
            runner.GetCommand(cmdName1a).Should().Be(cmdObj1, because: "Command instance registered under new name (alias) must be the same as that registered under original name.");
            runner.GetCommand(cmdName2a).Should().Be(cmdObj2, because: "Command instance registered under new name (alias) must be the same as that registered under original name.");
        }

        [Fact]
        protected virtual void Registration_CommandsCanBeUnRegistered()
        {
            // Arrange:
            // On a newly created invoker, register two commands and verify the state:
            TCommandRunner runner = CreateRunner();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: Initially, command runner must not contain any commands.");
            string cmdName1 = TestConst.Cmd1;
            TestCmd1 cmdObj1 = new TestCmd1();
            runner.AddCommand(cmdName1, cmdObj1);
            string cmdName2 = TestConst.Cmd2;
            TestCmd2 cmdObj2 = new TestCmd2();
            runner.AddCommand(cmdName2, cmdObj2);
            // Verify that command runner state is correct after adding two commands:
            runner.CommandsCount.Should().Be(2, because: "PRECOND: After adding two commands, command runner must contain exactly two commands.");
            runner.GetCommandNames().Should().Contain(cmdName1, because: "PRECOND: After adding command named cmdName1, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj1, because: $"PRECOND: The correct command should be registered under name `${cmdName1}`");
            runner.GetCommandNames().Should().Contain(cmdName2, because: "PRECOND: After adding command named cmdName2, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName2).Should().Be(cmdObj2, because: $"PRECOND: The correct command should be registered under name `${cmdName2}`");
            // Act:
            // remove the first command:
            runner.RemoveCommand(cmdName1);
            // Assert:
            runner.CommandsCount.Should().Be(1, because: "After removing one command, exactly one command must remain registered.");
            runner.GetCommandNames().Should().NotContain(cmdName1, because: $"After removing command named {cmdName1}, it must no longer be contained in the list of registered command names.");
            // Getcommand() must return null if command with specific name is not registered:
            runner.GetCommand(cmdName1).Should().BeNull(because: $"After removing command named {cmdName1}, GetCommand() must return null for that name.");
            // Removal of one command must not affect registration status of other commands:
            runner.GetCommandNames().Should().Contain(cmdName2, because: "Removal of one command must not affect other registered commands.");
            runner.GetCommand(cmdName2).Should().Be(cmdObj2, because: $"Removal of one command must not affect other registered commands; command object under {cmdName2} must remain unchanged.");
            // Arrange:
            // Also remove the second command:
            runner.RemoveCommand(cmdName2);
            // Assert:
            runner.CommandsCount.Should().Be(0, because: "After removing both commands, no commands must remain registered.");
            runner.GetCommandNames().Should().NotContain(cmdName2, because: $"After removing command named {cmdName2}, it must no longer be contained in the list of registered command names.");
            runner.GetCommand(cmdName2).Should().BeNull(because: $"After removing command named {cmdName2}, GetCommand() must return null for that name.");
        }


        [Fact]
        protected virtual void Registration_AllCommandsCanBeUnRegistered()
        {
            // Arrange:
            // On a newly created invoker, register two commands and verify the state:
            TCommandRunner runner = CreateRunner();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: Initially, command runner must not contain any commands.");
            string cmdName1 = TestConst.Cmd1;
            TestCmd1 cmdObj1 = new TestCmd1();
            runner.AddCommand(cmdName1, cmdObj1);
            string cmdName2 = TestConst.Cmd2;
            TestCmd2 cmdObj2 = new TestCmd2();
            runner.AddCommand(cmdName2, cmdObj2);
            // Verify that command runner state is correct after adding two commands:
            runner.CommandsCount.Should().Be(2, because: "PRECOND: After adding two commands, command runner must contain exactly two commands.");
            runner.GetCommandNames().Should().Contain(cmdName1, because: "PRECOND: After adding command named cmdName1, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj1, because: $"PRECOND: The correct command should be registered under name `${cmdName1}`");
            runner.GetCommandNames().Should().Contain(cmdName2, because: "PRECOND: After adding command named cmdName2, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName2).Should().Be(cmdObj2, because: $"PRECOND: The correct command should be registered under name `${cmdName2}`");
            // Act:
            // Unregister all commands by a single call:
            runner.RemoveAllCommands();
            // Assert:
            runner.CommandsCount.Should().Be(0, because: "After removing all commands, no commands must remain registered.");
            runner.GetCommandNames().Should().NotContain(cmdName1, because: $"After removing all commands, command named {cmdName1} must no longer be contained in the list of registered command names.");
            runner.GetCommand(cmdName1).Should().BeNull(because: $"After removing all commands, GetCommand() must return null for name {cmdName1}.");
            runner.GetCommandNames().Should().NotContain(cmdName2, because: $"After removing all commands, command named {cmdName2} must no longer be contained in the list of registered command names.");
            runner.GetCommand(cmdName2).Should().BeNull(because: $"After removing all commands, GetCommand() must return null for name {cmdName2}.");
        }

        [Fact]
        protected virtual void Registration_CommandsCanBeRegisteredInBatchesAtDifferentLevels()
        {
            // Arrange:
            // On a newly created invoker, register the first batch of commands via 
            // CommandUpdaterBaseLevel, which should install 2 commands with command objects:
            TCommandRunner runner = CreateRunner();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: Initially, command runner must not contain any commands.");
            // Act:
            // Register first batch of commands:
            runner.UpdateCommands(new CommandUpdaterBaseLevel<TExecutionInfo>());
            // Assert:
            // Verify number and names of commands installed in a batch via CommandUpdaterBaseLevel:
            runner.CommandsCount.Should().Be(2, because: "After applying CommandUpdaterBaseLevel, exactly two commands must be registered.");
            runner.ContainsCommand(TestConst.Cmd1).Should().BeTrue(because: "After applying CommandUpdaterBaseLevel, command named Cmd1 must be registered.");
            runner.ContainsCommand(TestConst.Cmd2).Should().BeTrue(because: "After applying CommandUpdaterBaseLevel, command named Cmd2 must be registered.");
            runner.ContainsCommand(TestConst.Cmd3).Should().BeFalse(because: "After applying CommandUpdaterBaseLevel, command named Cmd3 must NOT be registered.");
            runner.ContainsCommand(TestConst.Cmd4).Should().BeFalse(because: "After applying CommandUpdaterBaseLevel, command named Cmd4 must NOT be registered.");
            // Verify types of the two commands installed:
            runner.GetCommand(TestConst.Cmd1).GetType().Should().Be<TestCmd1>(because: $"Command registered under name {TestConst.Cmd1} must be of type TestCmd1.");
            runner.GetCommand(TestConst.Cmd2).GetType().Should().Be<TestCmd2>(because: $"Command registered under name {TestConst.Cmd2} must be of type TestCmd2.");
            // Act - second batch registration:
            // Applying the other batch will re-registered the first two commands and register
            // another two commands in addition:
            runner.UpdateCommands(new CommandUpdaterHigherLevel<TExecutionInfo>());
            // Assert:
            runner.CommandsCount.Should().Be(4, because: "After applying CommandUpdaterHigherLevel, exactly four commands must be registered.");
            runner.ContainsCommand(TestConst.Cmd1).Should().BeTrue(because: "After applying CommandUpdaterHigherLevel, command named Cmd1 must be registered.");
            runner.ContainsCommand(TestConst.Cmd2).Should().BeTrue(because: "After applying CommandUpdaterHigherLevel, command named Cmd2 must be registered.");
            // Another two commands must now also be registered, under names Cmd3 and Cmd 4:
            runner.ContainsCommand(TestConst.Cmd3).Should().BeTrue(because: "After applying CommandUpdaterHigherLevel, command named Cmd3 must also be registered.");
            runner.ContainsCommand(TestConst.Cmd4).Should().BeTrue(because: "After applying CommandUpdaterHigherLevel, command named Cmd4 must also be registered.");
            runner.GetCommand(TestConst.Cmd1).GetType().Should().Be<TestCmd1>(because: $"Command registered under name {TestConst.Cmd1} must be of type TestCmd1.");
            runner.GetCommand(TestConst.Cmd2).GetType().Should().Be<TestCmd2>(because: $"Command registered under name {TestConst.Cmd2} must be of type TestCmd2.");
            runner.GetCommand(TestConst.Cmd3).GetType().Should().Be<Command3>(because: $"Command registered under name {TestConst.Cmd3} must be of type Command3.");
            runner.GetCommand(TestConst.Cmd4).GetType().Should().Be<Command4>(because: $"Command registered under name {TestConst.Cmd4} must be of type Command4.");
            // Arrange & act - reset and use only second updater:
            //// If the command runner is reset to the initial state (all commands removed) and only
            //// the second updater is applied for batch registration, the result will be the same because
            //// via inheritance, the second updater also registered commands that would be registered
            //// by the first updater; this is useful for definition of commands at different project
            //// levels:
            //// Reset command runner and verify:
            runner.RemoveAllCommands();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: After removing all commands, no commands must remain registered.");
            // Act:
            //// Update commands by using the CommandUpdaterHigherLevel; all 4 commands should be registered:
            runner.UpdateCommands(new CommandUpdaterHigherLevel<TExecutionInfo>());
            runner.CommandsCount.Should().Be(4, because: "After updating the command runner by CommandUpdaterHigherLevel, 4 commands shouuuld be registered.");
            runner.ContainsCommand(TestConst.Cmd1).Should().BeTrue(because: $"After applying CommandUpdaterHigherLevel, command named {TestConst.Cmd1} must be registered.");
            runner.ContainsCommand(TestConst.Cmd2).Should().BeTrue(because: $"After applying CommandUpdaterHigherLevel, command named {TestConst.Cmd2} must be registered.");
            runner.ContainsCommand(TestConst.Cmd3).Should().BeTrue(because: $"After applying CommandUpdaterHigherLevel, command named {TestConst.Cmd3} must be registered.");
            runner.ContainsCommand(TestConst.Cmd4).Should().BeTrue(because: $"After applying CommandUpdaterHigherLevel, command named {TestConst.Cmd4} must be registered.");
            // Verify types of all 4 registered commands:
            runner.GetCommand(TestConst.Cmd1).GetType().Should().Be<TestCmd1>(because: $"Command registered under name {TestConst.Cmd1} must be of type TestCmd1.");
            runner.GetCommand(TestConst.Cmd2).GetType().Should().Be<TestCmd2>(because: $"Command registered under name {TestConst.Cmd2} must be of type TestCmd2.");
            runner.GetCommand(TestConst.Cmd3).GetType().Should().Be<Command3>(because: $"Command registered under name {TestConst.Cmd3} must be of type Command3.");
            runner.GetCommand(TestConst.Cmd4).GetType().Should().Be<Command4>(because: $"Command registered under name {TestConst.Cmd4} must be of type Command4.");
        }

        [Fact]
        protected virtual void Execution_RegisteredCommandsAreExecutedCorrectly()
        {
            // Arrange:
            // On a newly created invoker, register the first batch of commands via 
            // CommandUpdaterBaseLevel, which should install 2 commands with command objects:
            TCommandRunner runner = CreateRunner();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: Initially, command runner must not contain any commands.");
            string cmdName1 = TestConst.Cmd1;
            TestCmd1 cmdObj1 = new TestCmd1();
            runner.AddCommand(cmdName1, cmdObj1);
            string cmdName2 = TestConst.Cmd2;
            TestCmd2 cmdObj2 = new TestCmd2();
            runner.AddCommand(cmdName2, cmdObj2);
            // Verify that command runner state is correct after adding two commands:
            runner.CommandsCount.Should().Be(2, because: "PRECOND: After adding two commands, command runner must contain exactly two commands.");
            runner.GetCommandNames().Should().Contain(cmdName1, because: "PRECOND: After adding command named cmdName1, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj1, because: $"PRECOND: The correct command should be registered under name `${cmdName1}`");
            runner.GetCommandNames().Should().Contain(cmdName2, because: "PRECOND: After adding command named cmdName2, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName2).Should().Be(cmdObj2, because: $"PRECOND: The correct command should be registered under name `${cmdName2}`");
            // Execution count must be 0 for all commands, because nothing has been executed yet:
            cmdObj1.NumExecutions.Should().Be(0, because: "PRECOND: Execution count must be 0 for all commands, because nothing has been executed yet.");
            cmdObj2.NumExecutions.Should().Be(0, because: "PRECOND: Execution count must be 0 for all commands, because nothing has been executed yet.");
            // Act:
            // Execute command named cmdName1 (this will delegate execution to cmdObj1):
            runner.Execute(cmdName1);
            // Assert:
            // Check that the correct command has actually beeen executed, and no other:
            cmdObj1.NumExecutions.Should().Be(1, because: "After executing command named cmdName1, its execution count must be 1.");
            cmdObj2.NumExecutions.Should().Be(0, because: "After executing command named cmdName1, execution count of other commands must remain 0.");
            // Act:
            // Execute command named cmdName2 (this will delegate execution to cmdObj2):
            runner.Execute(cmdName2);
            // Check that the correct command has actually beeen executed (via execution count 
            // implemented in test command objects):
            cmdObj1.NumExecutions.Should().Be(1, because: "After executing both commands, execution count of cmdObj1 must be 1.");
            cmdObj2.NumExecutions.Should().Be(1, because: "After executing both commands, execution count of cmdObj2 must be 1.");
            // Act:
            // Execute command named cmdName1 twice in a row and check that the associated cmdObj1 has actually been
            // executed twice (three times in total, together with the previous execution):
            runner.Execute(cmdName1);
            runner.Execute(cmdName1);

            cmdObj1.NumExecutions.Should().Be(3, because: "After additional two executions, execution count of cmdObj1 must be 3.");
            cmdObj2.NumExecutions.Should().Be(1, because: "Execution count of cmdObj2 must remain 1.");
        }

        [Fact]
        protected virtual void Execution_MetaDataContainsDetailedInformationOnExecution()
        {
            // Arrange:
            // On a newly created invoker, register the first batch of commands via 
            // CommandUpdaterBaseLevel, which should install 2 commands with command objects:
            TCommandRunner runner = CreateRunner();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: Initially, command runner must not contain any commands.");
            string cmdName1 = TestConst.Cmd1;
            TestCmd1 cmdObj1 = new TestCmd1();
            runner.AddCommand(cmdName1, cmdObj1);
            string cmdName2 = TestConst.Cmd2;
            TestCmd2 cmdObj2 = new TestCmd2();
            runner.AddCommand(cmdName2, cmdObj2);
            // Verify that command runner state is correct after adding two commands:
            runner.CommandsCount.Should().Be(2, because: "PRECOND: After adding two commands, command runner must contain exactly two commands.");
            runner.GetCommandNames().Should().Contain(cmdName1, because: "PRECOND: After adding command named cmdName1, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj1, because: $"PRECOND: The correct command should be registered under name `${cmdName1}`");
            runner.GetCommandNames().Should().Contain(cmdName2, because: "PRECOND: After adding command named cmdName2, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName2).Should().Be(cmdObj2, because: $"PRECOND: The correct command should be registered under name `${cmdName2}`");
            // Execution count must be 0 for all commands, because nothing has been executed yet:
            cmdObj1.NumExecutions.Should().Be(0, because: "PRECOND: Execution count must be 0 for all commands, because nothing has been executed yet.");
            cmdObj2.NumExecutions.Should().Be(0, because: "PRECOND: Execution count must be 0 for all commands, because nothing has been executed yet.");
            // Act:
            TExecutionInfo executionInfo;
            // Execute command named cmdName1 (this will delegate execution to cmdObj1):
            executionInfo = runner.Execute(cmdName1);
            // Check that the correct command has actually beeen executed, and no other:
            cmdObj1.NumExecutions.Should().Be(1, because: "PRECOND: After executing command named cmdName1, its execution count must be 1.");
            cmdObj2.NumExecutions.Should().Be(0, because: "PRECOND: After executing command named cmdName1, execution count of other commands must remain 0.");
            // Assert:
            // Check that execution status is OK:
            if (executionInfo is ExecutionInfoExtended executionInfoExtended)
            {
                Console.WriteLine($"Execution info is of type {typeof(ExecutionInfoExtended).Name}, verifying correctness of contained fields...");
                executionInfoExtended.HasExecutedSuccessfully.Should().BeTrue(because: "Successful execution must be noted in execution metadata.");
                executionInfoExtended.HasExecutionCompleted.Should().BeTrue(because: "Completed execution must be noted in execution metadata.");
                executionInfoExtended.CommandResult.Should().BeNull(because: "Command does not return any result, so CommandResult must be null.");
                executionInfoExtended.ExecutionException.Should().BeNull(because: "Successful execution must not have any associated exception.");
                executionInfoExtended.Command.Should().Be(cmdObj1, because: "Execution metadata must contain correct command object.");
                executionInfoExtended.CommandStringId.Should().Be(cmdObj1.StringId, because: "Execution metadata must contain correct command ID.");
                executionInfoExtended.CommandName.Should().Be(cmdName1, because: "Execution metadata must contain correct command name.");
                Console.WriteLine("  ... executionInfoExtended verified.");
            } else
            {
                // ToDo: verify correctness for othef know types (currently, there should be just two!)
                Console.WriteLine($"Execution info is of type {executionInfo.GetType().Name};  fields vere not verified.");

            }
        }

        [Fact]
        protected virtual void Execution_ExceptionsThrownByCommandObjectsAreCaughtAndProperlyNoted()
        {
            // Arrange:
            // On a newly created invoker, register the first batch of commands via 
            // CommandUpdaterBaseLevel, which should install 2 commands with command objects:
            TCommandRunner runner = CreateRunner();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: Initially, command runner must not contain any commands.");
            string cmdName1 = TestConst.Cmd1;
            TestCmd1 cmdObj1 = new TestCmd1();
            runner.AddCommand(cmdName1, cmdObj1);
            // Verify that command runner state is correct after adding the command:
            runner.CommandsCount.Should().Be(1, because: "PRECOND: After adding a single command, command runner must contain exactly one command.");
            runner.GetCommandNames().Should().Contain(cmdName1, because: "PRECOND: After adding command named cmdName1, it must be contained in the list of registered command names.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj1, because: $"PRECOND: The correct command should be registered under name `${cmdName1}`");
            // Instruct command ocject to throw exception when executed, and execute the command:
            cmdObj1.ExceptionsToThrowCount = 1;
            // Act:
            TExecutionInfo executionInfo = runner.Execute(cmdName1);
            cmdObj1.NumExecutions.Should().Be(1, because: "PRECOND: After executing command named cmdName1, its execution count must be 1.");
            // Assert:
            // Check that thrown exception by command object is properly noted in execution metadata:
            if (executionInfo is ExecutionInfoExtended executionInfoExtended)
            {
                Console.WriteLine($"Execution info is of type {typeof(ExecutionInfoExtended).Name}, verifying correctness of contained fields...");
                executionInfoExtended.HasExecutionCompleted.Should().BeTrue(because: "After execution with exception thrown, HasExecutionCompleted must be true.");
                executionInfoExtended.HasExecutedSuccessfully.Should().BeFalse(because: "After execution with exception thrown, HasExecutedSuccessfully must be false.");
                executionInfoExtended.HasExecutedWithError.Should().BeTrue(because: "After execution with exception thrown, HasExecutedWithError must be true.");
                executionInfoExtended.ExecutionException.Should().NotBeNull(because: "After execution with exception thrown, ExecutionException must contain the exception thrown.");
            }
            else
            {
                Console.WriteLine($"Execution info is of type {executionInfo.GetType().Name};  fields vere not verified.");
                Console.WriteLine("Posibly, for this type of execution info, this test is not relevant.");
            }
        }

        [Fact]
        protected virtual void Execution_AttemptToExecuteUnregisteredCommandIsProperlyNoted()
        {
            // Arrange:
            TCommandRunner runner = CreateRunner();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: Initially, command runner must not contain any commands.");
            string cmdName1 = TestConst.Cmd1;
            // Act:
            // Attempt to execute command named cmdName1 when not registered on the command runner:
            TExecutionInfo executionInfo = runner.Execute(cmdName1);
            if (executionInfo is ExecutionInfoExtended executionInfoExtended)
            {
                executionInfoExtended.IsCommandDefined.Should().BeFalse(because: "After attempting to execute unregistered command, IsCommandDefined must be false.");
                executionInfoExtended.HasExecutedSuccessfully.Should().BeFalse(because: "After attempting to execute unregistered command, HasExecutedSuccessfully must be false.");
                executionInfoExtended.HasExecutionStarted.Should().BeFalse(because: "After attempting to execute unregistered command, HasExecutionStarted must be false.");
                executionInfoExtended.Command.Should().BeNull(because: "After attempting to execute unregistered command, Command must be null.");
            }
            else
            {
                Console.WriteLine($"Execution info is of type {executionInfo.GetType().Name};  fields vere not verified.");
                Console.WriteLine("Posibly, for this type of execution info, this test is not relevant.");
            }
        }

        [Fact]
        protected virtual void Execution_CommandsWithParametersAndReturnValueAreExecutedCorrectly()
        {
            // Arrange:
            // On a newly created invoker, register the first batch of commands via 
            // CommandUpdaterBaseLevel, which should install 2 commands with command objects:
            TCommandRunner runner = CreateRunner();
            string cmdNameSProd = TestConst.CmdSum;
            IGenericCommand cmdObjSum = new TestCmdProd();
            runner.AddCommand(cmdNameSProd, cmdObjSum);
            runner.GetCommandNames().Should().Contain(cmdNameSProd, because: $"PRECOND: Command {cmdNameSProd} should be registered.");
            runner.GetCommand(cmdNameSProd).GetType().Should().Be<TestCmdProd>(because: $"PRECOND: Command TestCmdProd should be registered under command name {cmdNameSProd}.");
            double a = 7;
            double b = 3;
            double c = 2;
            // Act:
            TExecutionInfo executionInfo = runner.Execute(cmdNameSProd, a, b, c);
            // Assert:
            if (executionInfo is ExecutionInfoExtended executionInfoExtended)
            {
                executionInfoExtended.HasExecutedSuccessfully.Should().BeTrue(because: "Command execution must be successful.");
            } else
            {
                Console.WriteLine($"Execution info is of type {executionInfo.GetType().Name};  some fields vere not verified.");
                Console.WriteLine("Posibly, for this type of execution info, these fields are not relevant.");
            }
            
            executionInfo.CommandResult.Should().NotBeNull(because: "Result should not be null for a command that returns something.");
                double result = (double)executionInfo.CommandResult;
                // Verify the result, which should be product of command arguments:
                result.Should().Be(a * b * c, because: "The result must be a product of command parameters in this case.");
            // Arrange:
            // Use the same command again, with shorter syntax and with different parameter values:
            a = 238.33; b = 43.825; c = 566.21;
            // Act and assert:
            runner.Execute(cmdNameSProd, a, b, c).CommandResult.Should().Be(a * b * c, because: "The result must again be a product of command parameters.");
        }


        // This test is for testing, uncomment attribute to run. [Fact]
        protected virtual void Execution_ExecutionIsFastEnough_100()
        {
            const int numTestExecutions = 100;
            const int minExecutionsPerSecond = 100_000;
            Execution_ExecutionIsFastEnough(numTestExecutions, minExecutionsPerSecond);
        }

        // This test is for testing, uncomment attribute to run. [Fact]
        protected virtual void Execution_ExecutionIsFastEnough_10_000()
        {
            const int numTestExecutions = 10_000;
            const int minExecutionsPerSecond = 100_000;
            Execution_ExecutionIsFastEnough(numTestExecutions, minExecutionsPerSecond);
        }

        /// <summary>Speed test.
        /// <para>WARNING:</para>
        /// <para>Sometimes some of these tests are failing, seemingly due to some internal reasons of the framework.
        /// Therefore, tests are only enabled in debug mode, until this is resolved. For now it seems that issues
        /// might be caused by Console.WriteLine(...).</para></summary>
        /// <param name="numTestExecutions">Number of invocations performed; recommended values in the order of ten thousand or more.</param>
        /// <param name="minExecutionsPerSecond">The minimum required invocations per second for the test to pass.</param>
        [Theory]
        [InlineData(100,        100_000)]
        [InlineData(10_000,     100_000)]
        #if DEBUG
        //[InlineData(100_000,  100_000)]
        #endif
        protected virtual void Execution_ExecutionIsFastEnough(int numTestExecutions, int minExecutionsPerSecond)
        {
            int numWarmupExecutions = 10;
            Console.WriteLine($"Testing {typeof(TCommandRunner).Name} execution speed ({numTestExecutions} executions,\n  {
                numWarmupExecutions} for warmup, min per second: {minExecutionsPerSecond}):\n");
            TCommandRunner runner = CreateRunner();
            runner.CommandsCount.Should().Be(0, because: "PRECOND: No commands should be registered initially.");
            string cmdName1 = TestConst.Cmd1;
            TestCmd1 cmdObj1 = new TestCmd1();
            runner.AddCommand(cmdName1, cmdObj1);
            runner.ContainsCommand(cmdName1).Should().BeTrue(because: $"PRECOND: Command name {cmdName1} should be registered.");
            runner.GetCommand(cmdName1).Should().BeOfType<TestCmd1>(because: $"PRECOND: Command of type {typeof(TestCmd1).Name} should be registered under {cmdName1}.");
            runner.GetCommand(cmdName1).Should().Be(cmdObj1, because: $"PRECOND: Command object {nameof(cmdObj1)} should be registered under {cmdName1}.");
            cmdObj1.NumExecutions.Should().Be(0, because: $"PRECOND: Number of executions registered on the command object {nameof(cmdObj1)} should initially be 0.");
            cmdObj1.NoConsoleOutput = true;
            TExecutionInfo executionInfo;
            // Warmup: perform a few invocations to "warm up" any caches, JIT compilation, etc.:
            for (int i = 0; i < numWarmupExecutions; ++i)
            {
                executionInfo = runner.Execute(cmdName1);
            }
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for (int i = 0; i < numTestExecutions; ++i)
            {
                executionInfo = runner.Execute(cmdName1);
            }
            sw.Stop();
            double elapsedSeconds = sw.Elapsed.TotalSeconds;
            double operationsPerSecond = numTestExecutions / (elapsedSeconds);

            cmdObj1.NumExecutions.Should().Be(numTestExecutions + numWarmupExecutions,
                because: "PRECOND: The actuall number of executions must correspond to the sum of number of warmup executions and number of executions for speed test.");
            // Assert:
            operationsPerSecond.Should().BeGreaterThanOrEqualTo(minExecutionsPerSecond, because: $"The number of executions per second should be greater or equal to {minExecutionsPerSecond} (required minimum).");

            //try
            //{
                // Strange: if outputs below are uncommented, some instances of the test usually fail
                Console.WriteLine($"{numTestExecutions} command executions performed in {elapsedSeconds * (double)1_000} ms."
                    + Environment.NewLine);
                Console.WriteLine($"Achieved executions per second: {(operationsPerSecond / (double)1e6)} M" + Environment.NewLine);

                // Addition - comparison with direect execution of commands:
                // Comparing execution of commands directly, without using CommandRunner:
                var cmd1 = runner.GetCommand(cmdName1) as TestCmd1;
                Console.WriteLine("\n\nMeasuring direct execution of command object without CommandRunner overhead...");
                object result;
                for (int i = 0; i < numWarmupExecutions; ++i)
                {
                    result = cmd1.Execute(null);
                }
                sw.Restart();
                for (int i = 0; i < numTestExecutions; ++i)
                {
                    result = cmd1.Execute(null);
                }
                sw.Stop();
                double elapsedSecondsRunner = elapsedSeconds;
                elapsedSeconds = sw.Elapsed.TotalSeconds;
                operationsPerSecond = numTestExecutions / (elapsedSeconds);
                Console.WriteLine($"{numTestExecutions} DIRECT command executions performed in {elapsedSeconds * (double)1_000} ms."
                    + Environment.NewLine);
                Console.WriteLine($"Achieved executions per second: {(operationsPerSecond / (double)1e6)} M" + Environment.NewLine);
                Console.WriteLine($"Direct execution is {elapsedSecondsRunner / elapsedSeconds}  times faster.");
            // }
            //catch (Exception ex)
            //{
            //    try
            //    {
            //        Console.WriteLine($"\n\nWARNING: {ex.GetType().Name} thrown in the auxiliary block below the test (not relevant for the test itself.)");
            //        Console.WriteLine($"  Exception message: {ex.Message}");
            //    }
            //    catch { }
            //}
        }


    }

}

