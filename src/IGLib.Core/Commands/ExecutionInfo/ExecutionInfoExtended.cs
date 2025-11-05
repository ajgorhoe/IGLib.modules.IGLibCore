using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Commands
{


    /// <inheritdoc/>
    public class ExecutionInfoExtended : ExecutionInfoExtended<IGenericCommand, ExecutionInfoExtended>,
        IExecutionInfoExtended<IGenericCommand, ExecutionInfoExtended>
    {
        /// <inheritdoc/>
        public ExecutionInfoExtended(ICommandRunner<IGenericCommand, ExecutionInfoExtended> commandRunner = null) :
            base(commandRunner)
        {  }
    }

    /// <inheritdoc/>
    /// <summary>Execution info classes that implement this interface contain detailed information on command 
    /// execution.</summary>
    /// <see cref="IExecutionInfoExtended{CommandType, ExecutionInfoType}"/>
    /// <remarks>See the description of <see cref="IExecutionInfoExtended{CommandType, ExecutionInfoType}"/>
    /// for more details.</remarks>
    public class ExecutionInfoExtended<CommandType, ExecutionInfoType> : ExecutionInfo<CommandType, ExecutionInfoType>,
            IExecutionInfoExtended<CommandType, ExecutionInfoType>
        where CommandType: class, IGenericCommand
        where ExecutionInfoType : IExecutionInfo<CommandType, ExecutionInfoType>
    {

        /// <summary>Constructor for execution info object of the current type.</summary>
        /// <param name="commandRunner">Command runner that created the current execution info. Optional,
        /// default is null. This parameter is usually used by the command runner when creating this object,
        /// normally before invoking the command.</param>
        public ExecutionInfoExtended(ICommandRunner<CommandType, ExecutionInfoType> commandRunner = null)
        {
            Runner = commandRunner;
        }

        // Remarks:
        // CommandResult is inherited from ExecutionInfo<CommandType, ExecutionInfoType>.
        // Runner is inherited from ExecutionInfo<CommandType, ExecutionInfoType>.

        /// <inheritdoc/>
        public virtual string CommandName { get; set; } = null;

        /// <inheritdoc/>
        public virtual string CommandStringId { get; set; } = null;

        /// <inheritdoc/>
        public virtual CommandType Command { get; set; } = null;

        /// <inheritdoc/>
        public virtual bool IsCommandDefined { get { return Command != null; } }

        /// <inheritdoc/>
        public virtual bool HasExecutionStarted { get; set; } = false;

        /// <inheritdoc/>
        public virtual bool IsExecuting { get { return HasExecutionStarted && !HasExecutionCompleted; } }

        /// <inheritdoc/>
        public virtual bool HasExecutionCompleted { get; set; } = false; 

        /// <inheritdoc/>
        public virtual bool HasExecutedSuccessfully { get; set; } = false;

        /// <inheritdoc/>
        public virtual bool HasExecutedWithError
        {
            get { return (HasExecutionCompleted && (!HasExecutedSuccessfully)); }
        }

        /// <inheritdoc/>
        public virtual bool IsCommandUndefined { get { return (HasExecutionStarted && Command == null); } }

        /// <inheritdoc/>
        public virtual object[] CommandParameters { get; set; } = null;

        /// <inheritdoc/>
        public virtual Exception ExecutionException { get; set; } = null;

        /// <inheritdoc/>
        protected virtual string ErrorDescriptionUnknown { get; } = "Unknown error has occurred.";

        /// <inheritdoc/>
        protected virtual string ErrorDescriptionCommandNotDefinedPartial { get; } = "Command not defined: ";

        /// <inheritdoc/>
        protected virtual string ErrorDescriptionCommandNotDefined
        {
            get
            {
                return ErrorDescriptionCommandNotDefinedPartial + (CommandName == null? "?": CommandName);
            }
        }

        /// <inheritdoc/>
        protected virtual string ErrorDescriptionExecutionException
        {
            get
            {
                return $"Command execution has thrown exception ({ExecutionException.GetType().Name}: {ExecutionException.Message}).";
            }
        }

        private string _errorDescription = null;

        /// <inheritdoc/>
        public virtual string ExecutionErrorMesage {
            get
            {
                if (_errorDescription == null)
                {
                    if (HasExecutionCompleted && !HasExecutedSuccessfully)
                    {
                        if (IsCommandUndefined)
                        {
                            ExecutionErrorMesage = ErrorDescriptionCommandNotDefined;
                        } else if (ExecutionException!=null)
                        {
                            ExecutionErrorMesage = ErrorDescriptionExecutionException;
                        } else
                        {
                            ExecutionErrorMesage = ErrorDescriptionUnknown;
                        }
                    }
                }
                return _errorDescription;
            }
            set
            {
                _errorDescription = value;
            }
        }

    }


}
