
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IG.Lib;

namespace IGLib.Commands
{


    /// <summary>Marker interface for execution into interfaces.</summary>
    public interface IExecutionInfo
    {  }

    /// <summary>Contains most basic information about command execution: the result of execution (<see cref="CommandResult"/>).
    /// and command runner that launched the command (<see cref="Runner"/>, which can be used for chaining 
    /// command invocations and other operations on command runner).</summary>
    /// <typeparam name="CommandType">Type of command class used with the current type of execution info. This needs
    /// to be a type parameter because of the <see cref="Runner"/> property, (interface <see cref="ICommandRunner{CommandType, 
    /// ExecutionInfoType}"/>, which needs this information). Also, derived interfaces may contain a reference to
    /// command being invoked, for which its type needs to be declared.</typeparam>
    /// <typeparam name="ExecutionInfoType">Actual (concrete) type of the execution info. This is necessary
    /// so that execution info can hold a command runner (interface <see cref="ICommandRunner{CommandType, ExecutionInfoType}"/>),
    /// used to chaining executions </typeparam>
    public interface IExecutionInfo<CommandType, ExecutionInfoType> : IExecutionInfo
        where CommandType : class, IGenericCommand
        where ExecutionInfoType : IExecutionInfo<CommandType, ExecutionInfoType>  // decide between this or just IExecutionInfo
    {

        /// <summary>The result of command execution, if command has been successfully executed, or null
        /// if execution has not been completed yet or it failed, or if the command does not provide a
        /// result.</summary>
        object CommandResult { get; set; }

        /// <summary>Command runner that has invoked the command. This enables chaining of further runner 
        /// actions after running the command, by obtaining the command runner object from execution info
        /// that is result of <see cref="ICommandRunner{CommandType, ExecutionInfoType}.Execute(string, object[])"/>.</summary>
        ICommandRunner<CommandType, ExecutionInfoType> Runner { get; set; }

    }


}
