
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Commands
{



    /// <summary>A marker interface for command runners, such as those implementing <see cref="ICommandRunner{CommandType, ExecutionInfoType}"/>.</summary>
    public interface ICommandRunner {  }


    /// <summary>Generic interface for command runners.
    /// <para>The part of the interface that only depends on the<typeparamref name="CommandType"/> generic
    /// parameter is separated out in the <see cref="ICommandRunner{CommandType, ExecutionInfoType}"/> interface. The
    /// rest of the intefface is defined here.</para>
    /// </summary>
    /// <typeparam name="CommandType">Type of command objects that through which commands are executed.</typeparam>
    /// <typeparam name="ExecutionInfoType">Type of the execution info, used by command runners to pass more or
    /// less detailed information on command execution (command runner that executed the command, command result, 
    /// execution status, etc.).</typeparam>
    public interface ICommandRunner<CommandType, ExecutionInfoType>:
            ICommandRunner
        where CommandType : class, IGenericCommand
        where ExecutionInfoType : IExecutionInfo<CommandType, ExecutionInfoType>
    {

        #region CommandExecution

        /// <summary>Executes the specified command asynchronously.</summary>
        /// <param name="commandName">Name of the command to be executed.</param>
        /// <param name="commandArguments">Parameters passed to the executed command.</param>
        /// <returns>Task that can be awaited, whose result contains command execution information (<see cref="IExecutionInfo"/>) for the executed command.</returns>
        Task<ExecutionInfoType> ExecuteAsync(string commandName, params object[] commandArguments);


        /// <summary>Synchronously executes the command identified by <paramref name="commandName"/> and returns information
        /// about its execution in form of <see cref="IExecutionInfo"/>.</summary>
        /// <param name="commandName">Name of the command to be executed, used to identify the <see cref="IGenericCommand"/> object
        /// to which the actual execution is delegated.</param>
        /// <param name="commandArguments">Parameters that aree passed to execution of the command (optional).</param>
        /// <returns>An <see cref="IExecutionInfo"/> object containing all the relevant inforamtion regarding the particular
        /// act of command execution, which includes execution status (completion status, error status), parameters, results, the 
        /// <see cref="IGenericCommand"/> object to which execution was delegated, etc.</returns>
        ExecutionInfoType Execute(string commandName, params object[] commandArguments);

        #endregion CommandExecution


        #region CommandRegistration

        /// <summary>Returns true if the command named <paramref name="commandName"/> is rgistered on the current 
        /// command runner object, false if not.</summary>
        /// <param name="commandName">Command name whose existence is queried.</param>
        bool ContainsCommand(string commandName);

        /// <summary>Returns the command (object of type <see cref="CommandType"/>) that is registered under the 
        /// specified <paramref name="commandName"/>.</summary>
        /// <param name="commandName">Name unter which the demanded command object is registered.</param>
        /// <returns>The command object that corresponds to <paramref name="commandName"/>, or null if there
        /// is no such command.</returns>
        CommandType GetCommand(string commandName);

        /// <summary>Returns command names for all commands that are registered on the current command runner.</summary>
        string[] GetCommandNames();

        /// <summary>Returns the number of commands that are currently registered under distinctive names.</summary>
        int CommandsCount { get; }

        /// <summary>Adds the specified command (parameter <paramref name="command"/>) to the current command 
        /// runner, and registers it under the name <paramref name="commandName"/>.
        /// <para>A single command object can be registered under several names.</para>
        /// <para>This method is meant for registereing a command with name that is not yet used, and its overrides 
        /// may launch warning or throw exceptions if <paramref name="commandName"/>
        /// is already used. To avoid doubt, use the <see cref="AddOrReplaceCommand(string, CommandType)"/> instead.</para></summary>
        /// <param name="commandName">Name of the command under which <paramref name="command"/> is added.</param>
        /// <param name="command">Command that is added under the specified name.</param>
        ICommandRunner<CommandType, ExecutionInfoType> AddCommand(string commandName, CommandType command);

        /// <summary>Registers the specified command (parameter <paramref name="command"/>) on the current command 
        /// runner under the name <paramref name="commandName"/>.
        /// <para>If another command object had already been registered under this name, the definition is 
        /// changed silently.</para></summary>
        /// <param name="commandName">Name of the command under which <paramref name="command"/> gets registered.</param>
        /// <param name="command">Command that is registered under the specified name (<paramref name="commandName"/>).</param>
        ICommandRunner<CommandType, ExecutionInfoType> AddOrReplaceCommand(string commandName, CommandType command);

        /// <summary>Adds an alias for the existing command registered under <paramref name="originalCommandName"/>, and registers 
        /// the corresponding command object under the new command name, <paramref name="aliasCommandName"/>.</summary>
        /// <param name="originalCommandName">Name under which the command is already registered.</param>
        /// <param name="aliasedCommandName">Command name under which the command name will become registered in 
        /// addition to the original name.</param>
        ICommandRunner<CommandType, ExecutionInfoType> AddCommandAlias(string originalCommandName, string aliasCommandName);


        /// <summary>Removes the command registered under <paramref name="commandName"/> from the system.</summary>
        /// <param name="commandName">Name that is unregistered.</param>
        ICommandRunner<CommandType, ExecutionInfoType> RemoveCommand(string commandName);

        /// <summary>Removes all registered commands from the current command runner.</summary>
        ICommandRunner<CommandType, ExecutionInfoType> RemoveAllCommands();

        /// <summary>Uses the specified command updater(s) in order to update commmands (typically, add new commannds, or aliases) on 
        /// the current command runner.</summary>
        /// <param name="updaters">Command updaters (of type <see cref="ICommandUpdater{CommandType, ExecutionInfoType}"/>) that will 
        /// update commands on the current command runner. Updaters are used one by one to update the current <see cref="ICommandRunner"/>
        /// by calling updaters' <see cref="ICommandUpdater{CommandType, ExecutionInfoType}.UpdateCommands(ICommandRunner{CommandType, ExecutionInfoType})"/>
        /// method. Updaters can be null or empty array, in which case nothing happens on the current command runner.</param>
        ICommandRunner<CommandType, ExecutionInfoType> UpdateCommands(params ICommandUpdater<CommandType, ExecutionInfoType>[] updaters);

        #endregion CommanndRegistration



    }




}
