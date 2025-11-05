using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IG.Lib;

namespace IGLib.Commands
{

    /// <summary>Registers and invokes commands of type <see cref="IGenericCommand"/>.
    /// <para>Thread safe. Command registrations / unregistrations, executions, and other functions
    /// and properties of the object of this class can be safely called from parallel threads.</para></summary>
    public abstract class CommandRunner<CommandType, ExecutionInfoType> : CommandRunnerUnsafe<CommandType, ExecutionInfoType>,
        ICommandRunner<CommandType, ExecutionInfoType>, ILockable
        where CommandType : class, IGenericCommand
        where ExecutionInfoType : IExecutionInfoExtended<CommandType, ExecutionInfoType>
    {

        /// <summary>Constructor. Only parameterless constructors will normally exist for this and derived classes.</summary>
        public CommandRunner(): base()
        { }

        /// <summary>Allows to specify on the constructed command runner object that registration of null commands is allowed.</summary>
        /// <param name="isNullCommandRegistrationAllowed"></param>
        public CommandRunner(bool isNullCommandRegistrationAllowed) : this()
        {
            IsNullCommandRegistrationAllowed = isNullCommandRegistrationAllowed;
        }

        /// <summary>Object used for locking access to internal resources, necessary to call object's
        /// methods from multiple threads.</summary>
        public object Lock { get; } = new object();

        /// <inheritdoc/>
        public override ICommandRunner<CommandType, ExecutionInfoType> UpdateCommands(params ICommandUpdater<CommandType, ExecutionInfoType>[] updaters)
        {
            lock(Lock)
            {
                return base.UpdateCommands(updaters);
            }
        }

        /// <inheritdoc/>
        public override bool ContainsCommand(string commandName)
        {
            lock (Lock) { 
                return base.ContainsCommand(commandName);
            }
        }

        /// <inheritdoc/>
        public override CommandType GetCommand(string commandName)
        {
            lock (Lock)
            {
                return base.GetCommand(commandName);
            }
        }

        /// <inheritdoc/>
        public override string[] GetCommandNames()
        {
            lock (Lock)
            {
                return base.GetCommandNames();
            }
        }

        /// <inheritdoc/>
        public override int CommandsCount { 
            get 
            {
                lock (Lock)
                {
                    return base.CommandsCount;
                }
            }
        }

        /// <inheritdoc/>
        public override ICommandRunner<CommandType, ExecutionInfoType> AddCommand(string commandName, CommandType command)
        {
            lock (Lock) {
                return base.AddCommand(commandName, command);
            }
        }

        /// <inheritdoc/>
        public override ICommandRunner<CommandType, ExecutionInfoType> AddOrReplaceCommand(string commandName, CommandType command)
        {
            lock (Lock) {
                return base.AddOrReplaceCommand(commandName, command);
            }
        }

        /// <inheritdoc/>
        public override ICommandRunner<CommandType, ExecutionInfoType> AddCommandAlias(string originalCommandName, string aliasCommandName)
        {
            lock (Lock)
            {
                return base.AddCommandAlias(originalCommandName, aliasCommandName);
            }
        }

        /// <inheritdoc/>
        public override ICommandRunner<CommandType, ExecutionInfoType> RemoveCommand(string commandName)
        {
            lock (Lock) {
                return base.RemoveCommand(commandName);
            }
        }

        /// <inheritdoc/>
        public override ICommandRunner<CommandType, ExecutionInfoType> RemoveAllCommands()
        {
            lock (Lock) {
                return base.RemoveAllCommands();
            }
        }

        /// <inheritdoc/>
        public override async Task<ExecutionInfoType> ExecuteAsync(string commandName, params object[] commandArguments)
        {
            return await base.ExecuteAsync(commandName, commandArguments);
        }

        /// <inheritdoc/>
        public override async Task<ExecutionInfoType> ExecuteWhen(string commandName, object[] commandArguments,
            Func<bool> condition, Action<ExecutionInfoType> executeInCurrentContext,
            int waitBetweenChecksMs = 20, int delayMs = 0, int timeoutMs = 0)
        {
            return await base.ExecuteWhen(commandName,commandArguments, condition, executeInCurrentContext, 
                waitBetweenChecksMs, delayMs, timeoutMs);
        }

        /// <inheritdoc/>
        public override async Task<ExecutionInfoType> ExecuteWhen(string commandName,
            Func<bool> condition, Action<ExecutionInfoType> executeInCurrentContext,
            int waitBetweenChecksMs = 20, int delayMs = 0, int timeoutMs = 0)
        {
            return await base.ExecuteWhen(commandName, condition, executeInCurrentContext,
                waitBetweenChecksMs, delayMs, timeoutMs);
        }

        /// <inheritdoc/>
        public override ExecutionInfoType Execute(string commandName, params object[] commandArguments)
        {
            CommandType command = null;
            ExecutionInfoType executionInfo;
            lock (Lock)
            {
                executionInfo = CreateCommandExecutionInfo(commandName, command, commandArguments);
                command = GetCommand(commandName);
            }
            if (command != null)
            {
                try
                {
                    executionInfo.HasExecutionStarted = true;
                    executionInfo.CommandResult = command.Execute(commandArguments);
                    executionInfo.HasExecutedSuccessfully = true;
                }
                catch (Exception ex)
                {
                    executionInfo.HasExecutedSuccessfully = false;
                    executionInfo.ExecutionException = ex;
                }
                finally
                {
                    executionInfo.HasExecutionCompleted = true;
                }
            }
            return executionInfo;
        }

    }


}
