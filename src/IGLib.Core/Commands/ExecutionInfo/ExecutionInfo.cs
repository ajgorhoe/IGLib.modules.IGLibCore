
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Commands
{

    /// <inheritdoc/>
    public class ExecutionInfo : ExecutionInfo<IGenericCommand, ExecutionInfo>,
        IExecutionInfo<IGenericCommand, ExecutionInfo>
    {
        /// <inheritdoc/>
        public ExecutionInfo(ICommandRunner<IGenericCommand, ExecutionInfo> commandRunner = null) :
            base(commandRunner)
        { }
    }

    /// <inheritdoc/>
    public class ExecutionInfo<CommandType, ExecutionInfoType> : IExecutionInfo<CommandType, ExecutionInfoType>
        where CommandType: class, IGenericCommand
        where ExecutionInfoType: IExecutionInfo<CommandType, ExecutionInfoType>
    {

        /// <summary>Constructor for execution info object of the current type.</summary>
        /// <param name="commandRunner">Command runner that created the current execution info. Optional,
        /// default is null. This parameter is usually used by the command runner when creating this object,
        /// normally before invoking the command.</param>
        public ExecutionInfo(ICommandRunner<CommandType, ExecutionInfoType> commandRunner = null)
        {
            Runner = commandRunner;
        }

        /// <inheritdoc/>
        public virtual object CommandResult { get; set; } = null;

        /// <inheritdoc/>
        public virtual ICommandRunner<CommandType, ExecutionInfoType> Runner { get; set; }


    }

}
