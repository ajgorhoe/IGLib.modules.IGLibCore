
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IG.Lib;

namespace IGLib.Commands
{

    /// <summary>Contains extended information about command execution. Beside the execution context  (command 
    /// runner) and command result available on the simpler <see cref="IExecutionInfo{CommandType, ExecutionInfoType}"/>, 
    /// this also includes detailed information on execution status  (whether execution has started or is completed,
    /// whether it completed with errors, etc.).</summary>
    /// <typeparam name="CommandType">Type of the generic command object whose execution information is contained
    /// by this class.</typeparam>
    public interface IExecutionInfoExtended<CommandType, ExecutionInfoType> : IExecutionInfo<CommandType, ExecutionInfoType>
        where CommandType : class, IGenericCommand
        where ExecutionInfoType : IExecutionInfo<CommandType, ExecutionInfoType>  // decide between this or just IExecutionInfo
    {

        /// <summary>Name by which the command was accessed in its context (usually the command runner that
        /// invoked it).</summary>
        string CommandName { get; set; }


        /// <summary>String ID of the command. For commands that correctly implement the <see cref="IStringIdentifiable"/>
        /// interface, this should uniquely indentify command object.</summary>
        string CommandStringId { get; set; }

        /// <summary>Whether command with the name <see cref="CommandName"/> is defined in its context (usually
        /// the command runner that tried to invoke it).</summary>
        bool IsCommandDefined { get; }

        /// <summary>Reference of the executed command.</summary>
        CommandType Command { get; set; }

        /// <summary>Parameters with which the command is invoked.</summary>
        object[] CommandParameters { get; set; }

        /// <summary>Whether execution of the command has been started already.</summary>
        bool HasExecutionStarted { get; set; }

        /// <summary>Whether execution of the command has completed (either successfully or with
        /// error).</summary>
        bool HasExecutionCompleted { get; set; }

        /// <summary>Whether the command is currently being executed.</summary>
        bool IsExecuting { get; }

        /// <summary>Whether the command has been executed successfully.</summary>
        bool HasExecutedSuccessfully { get; set; }

        /// <summary>True if command execution has been completed, but with error.</summary>
        bool HasExecutedWithError { get; }

        /// <summary>Contains eventual exception that has been thrown during command execution.</summary>
        Exception ExecutionException { get; set; }

        /// <summary>Eventual error message indicating why execution of the command has failed.</summary>
        string ExecutionErrorMesage { get; set; }

    }


}
