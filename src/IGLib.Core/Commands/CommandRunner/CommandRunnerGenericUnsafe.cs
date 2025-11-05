using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Commands
{

    /// <summary>Registers and invokes commands of type <see cref="IGenericCommand"/>.
    /// <para>NOT thread safe. Command registrations and executions should be called from a single thread, 
    /// unless a custom resource locking is implemented. Use <see cref="CommandRunner{CommandType, ExecutionInfoType}"/>
    /// if you need a thread safe equivalent.</para></summary>
    public abstract class CommandRunnerUnsafe<CommandType, ExecutionInfoType> : ICommandRunner<CommandType, ExecutionInfoType>
        where CommandType : class, IGenericCommand
        where ExecutionInfoType : IExecutionInfoExtended<CommandType, ExecutionInfoType>
    {

        /// <summary>Constructor. Only parameterless constructors will normally exist for this and derived classes.</summary>
        public CommandRunnerUnsafe()
        { }

        /// <summary>Allows to specify on the constructed command runner object that registration of null commands is allowed.</summary>
        /// <param name="isNullCommandRegistrationAllowed"></param>
        public CommandRunnerUnsafe(bool isNullCommandRegistrationAllowed) : this()
        {
            IsNullCommandRegistrationAllowed = isNullCommandRegistrationAllowed;
        }

        /// <summary>Keeps track of registered commands.</summary>
        protected Dictionary<string, CommandType> Commands { get; } = new Dictionary<string, CommandType>();

        /// <summary>Creation method used to create the <see cref="ExecutionInfoType"/> to hold information on the
        /// particular execution of the command. Such an object is created before execution is delegated to the appropriate
        /// <see cref="CommandType"/> object (e.g., in the <see cref="Execute(string, object[])"/>) method.</summary>
        /// <param name="commandName"></param>
        /// <param name="command"></param>
        /// <param name="commandArguments"></param>
        /// <returns></returns>
        protected abstract ExecutionInfoType CreateCommandExecutionInfo(string commandName, CommandType command,
            params object[] commandArguments);

        /// <summary>Whether null may be passed as argument to methods that perform command registration.</summary>
        public bool IsNullCommandRegistrationAllowed { get; protected set; } = false;

        /// <inheritdoc/>
        public virtual ICommandRunner<CommandType, ExecutionInfoType> UpdateCommands(params ICommandUpdater<CommandType, ExecutionInfoType>[] updaters)
        {
            if (updaters != null)
            {
                foreach (var updater in updaters)
                {
                    updater.UpdateCommands(this);
                }
                
            }
            return this;
        }

        /// <summary>Returns true if the command named <paramref name="commandName"/> is rgistered on the current 
        /// command runner object, false if not.</summary>
        /// <param name="commandName">Command name whose existence is queried.</param>
        public virtual bool ContainsCommand(string commandName)
        {
            if (commandName == null)
                return false;
            return Commands.ContainsKey(commandName);
        }

        /// <summary>Returns the command (object of type <see cref="CommandType"/>) that is registered under the specified <paramref name="commandName"/>.</summary>
        /// <param name="commandName">Name unter which the demanded command object is registered.</param>
        public virtual CommandType GetCommand(string commandName)
        {
            CommandType command;
            if (Commands.TryGetValue(commandName, out command))
                return command;
            return null;
        }

        /// <summary>Returns command names for all commands that are registered on the current command runner.</summary>
        public virtual string[] GetCommandNames()
        {
            return Commands.Keys.ToArray();
        }

        /// <summary>Gets the number of commands that are currently registered under distinctive names.</summary>
        public virtual int CommandsCount { get { return Commands.Count; } }

        /// <summary>Adds the specified command (parameter <paramref name="command"/>) to the current command runner, 
        /// and registers it under the name <paramref name="commandName"/>.
        /// <para>A single command object can be registered under several names.</para>
        /// <para>This method is meant for registereing a command with name that is not yet used, and its overrides may launch warning or throw exceptions if <paramref name="commandName"/>
        /// is already used. To avoid doubt, use the <see cref="AddOrReplaceCommand(string, CommandType)"/> instead.</para></summary>
        /// <param name="commandName">Name of the command under which <paramref name="command"/> is added.</param>
        /// <param name="command">Command that is added under the specified name.</param>
        public virtual ICommandRunner<CommandType, ExecutionInfoType> AddCommand(string commandName, CommandType command)
        {
            if (command == null)
            {
                if (!IsNullCommandRegistrationAllowed)
                {
                    throw new ArgumentException($"Command to be registerred under name \"{commandName}\" is not specified (null reference).");
                }
            }
            if (string.IsNullOrEmpty(commandName))
            {
                string commandId = "<< unspecified >>";
                if (command != null)
                    commandId = command.StringId;
                throw new ArgumentException($"Command name can not be null or empty string (encountered when adding command {command.StringId}).", nameof(commandName));
            }
            if (ContainsCommand(commandName))
            {
                var cmd = GetCommand(commandName);
                if (cmd != null)
                {
                    throw new InvalidOperationException(
                        $"Command {commandName} cannot be added, already registered with the {cmd.GetType().Name} command class.");
                }
            }
            Commands[commandName] = command;
            return this;
        }

        /// <summary>Registers the specified command (parameter <paramref name="command"/>) on the current command 
        /// runner under the name <paramref name="commandName"/>.
        /// <para>If another command object had already been registered under this name, the definition is changed silently.</para></summary>
        /// <param name="commandName">Name of the command under which <paramref name="command"/> gets registered.</param>
        /// <param name="command">Command that is registered under the specified name (<paramref name="commandName"/>).</param>
        public virtual ICommandRunner<CommandType, ExecutionInfoType> AddOrReplaceCommand(string commandName, CommandType command)
        {
            if (!string.IsNullOrEmpty(commandName) && Commands.ContainsKey(commandName))
            {
                Commands.Remove(commandName);
            }
            AddCommand(commandName, command);
            return this;
        }

        /// <summary>Adds an alias to the existing command registered under <paramref name="originalCommandName"/>, and registeres 
        /// the corresponding command object under the new command, <paramref name="aliasedCommandName"/>.</summary>
        /// <param name="originalCommandName">Name under the command is already rehistered.</param>
        /// <param name="aliasedCommandName">Command name under which the command name will be registered in addition to the original name.</param>
        public virtual ICommandRunner<CommandType, ExecutionInfoType> AddCommandAlias(string originalCommandName, string aliasedCommandName)
        {
            if (!Commands.ContainsKey(originalCommandName))
            {
                throw new ArgumentException($"Can not alias the command {originalCommandName}: command does not exist.");
            }
            AddOrReplaceCommand(aliasedCommandName, GetCommand(originalCommandName));
            return this;
        }

        /// <summary>Removes the command registered under <paramref name="commandName"/> from the system.</summary>
        /// <param name="commandName">Name that is unregistered.</param>
        public virtual ICommandRunner<CommandType, ExecutionInfoType> RemoveCommand(string commandName)
        {
            if (!string.IsNullOrEmpty(commandName) && Commands.ContainsKey(commandName))
            {
                Commands.Remove(commandName);
            }
            return this;
        }

        /// <summary>Removes all registered commands from the current command runner.</summary>
        public virtual ICommandRunner<CommandType, ExecutionInfoType> RemoveAllCommands()
        {
            Commands.Clear();
            return this;
        }

        /// <summary>Placeholder - command Id for commands that are not defined.</summary>
        public virtual string UndefinedCommandId { get; protected set; } = "<< Undefined. >>";
        
        /// <summary>Executes the specified command asynchronously.</summary>
        /// <param name="commandName">Name of the command to be executed.</param>
        /// <param name="commandArguments">Parameters passed to the executed command.</param>
        /// <returns>Task that can be awaited, whose result contains command execution information (<see cref="ExecutionInfoType"/>) for the executed command.</returns>
        public virtual async Task<ExecutionInfoType> ExecuteAsync(string commandName, params object[] commandArguments)
        {
            Task<ExecutionInfoType> executiontask = Task.Factory.StartNew<ExecutionInfoType>(() =>
            {
                return Execute(commandName, commandArguments);
            });
            return await executiontask;
        }

        /// <summary>Executes the specified command when the specified condition is fulfilled, on a pool thread. Condition is checked 
        /// periodically, with eventual delay.</summary>
        /// <param name="commandName">Name of the command to be executed.</param>
        /// <param name="commandArguments">Command arguments.</param>
        /// <param name="condition">Delegates that evaluates condition for execution of command.</param>
        /// <param name="executeInCurrentContext">Task that is executed in the current thread context after the command is executed.
        /// Task receives the task that executed the command as input parameter, through wihich it can access the command info as task result,
        /// and result of command through this <see cref="ExecutionInfoType"/> object.</param>
        /// <param name="waitBetweenChecksMs">Time span, in milliseconds, that is waited between subsequent checks of condition.</param>
        /// <param name="delayMs">Time, in millisecond, by which checking the condition is delayed.</param>
        /// <param name="timeoutMs">Task through which the command's execution info (object of type <see cref="ExecutionInfoType"/>) is accessed.</param>
        /// <returns></returns>
        public virtual async Task<ExecutionInfoType> ExecuteWhen(string commandName, object[] commandArguments,
            Func<bool> condition, Action<ExecutionInfoType> executeInCurrentContext,
            int waitBetweenChecksMs = 20, int delayMs = 0, int timeoutMs = 0)
        {
            if (delayMs > 0)
                await Task.Delay(delayMs);
            while (!condition())
            {
                await Task.Delay(waitBetweenChecksMs);
            }
            Task<ExecutionInfoType> executionTask = Task.Factory.StartNew<ExecutionInfoType>(() =>
            {
                return Execute(commandName, commandArguments);
            });
            if (executeInCurrentContext == null)
            {
                Task<ExecutionInfoType> taskCurrentContex = executionTask.ContinueWith((previousTask) =>
                {
                    return previousTask.Result;
                }, TaskScheduler.FromCurrentSynchronizationContext());
                return await taskCurrentContex;
            }
            else
            {
                return await executionTask;
            }
        }

        /// <summary>Similar to <see cref="ExecuteAsync(string, object[])"/>, with som default values.</summary>
        public virtual async Task<ExecutionInfoType> ExecuteWhen(string commandName,
            Func<bool> condition, Action<ExecutionInfoType> executeInCurrentContext,
            int waitBetweenChecksMs = 20, int delayMs = 0, int timeoutMs = 0)
        {
            return await ExecuteWhen(commandName, null, condition, executeInCurrentContext, waitBetweenChecksMs, delayMs, timeoutMs);
        }

        /// <summary>Synchronously executes the command identified by <paramref name="commandName"/> and returns information
        /// about its execution in form of <see cref="ExecutionInfoType"/>.</summary>
        /// <param name="commandName">Name of the command to be executed, used to identify the <see cref="CommandType"/> object
        /// to which the actual execution is delegated.</param>
        /// <param name="commandArguments">Parameters that aree passed to execution of the command (optional).</param>
        /// <returns>An <see cref="ExecutionInfoType"/> object containing all the relevant inforamtion regarding the particular
        /// act of command execution, which includes execution status (completion status, error status), parameters, results, the 
        /// <see cref="CommandType"/> object to which execution was delegated, etc.</returns>
        public virtual ExecutionInfoType Execute(string commandName, params object[] commandArguments)
        {
            CommandType command = GetCommand(commandName);
            ExecutionInfoType executionInfo = CreateCommandExecutionInfo(commandName, command, commandArguments);
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
