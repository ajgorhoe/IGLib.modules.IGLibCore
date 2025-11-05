using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Commands
{

    public class CommandRunnerExtendedUnsafe : CommandRunnerUnsafe<IGenericCommand, ExecutionInfoExtended>
    {

        /// <summary>Constructor. Only parameterless constructors will normally exist for this and derived classes.</summary>
        public CommandRunnerExtendedUnsafe(): base()
        {  }


        /// <summary>Creation method used to create the <see cref="ExecutionInfoType"/> to hold information on the
        /// particular execution of the command. Such an object is created before execution is delegated to the appropriate
        /// <see cref="CommandType"/> object (e.g., in the <see cref="Execute(string, object[])"/>) method.</summary>
        /// <param name="commandName"></param>
        /// <param name="command"></param>
        /// <param name="commandArguments"></param>
        /// <returns></returns>
        protected override ExecutionInfoExtended CreateCommandExecutionInfo(string commandName, IGenericCommand command,
            params object[] commandArguments)
        {
            ExecutionInfoExtended ret = new ExecutionInfoExtended(this);
            string commandId;
            if (command != null)
            {
                commandId = command.StringId;
            }
            else
            {
                commandId = UndefinedCommandId;
            }
            ret.CommandName = commandName;
            ret.CommandStringId = commandId;
            ret.Command = command;
            ret.CommandParameters = commandArguments;
            return ret;
        }


    }


}
