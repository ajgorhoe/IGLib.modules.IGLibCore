using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IG.Lib;

namespace IGLib.Commands
{
    public interface IGenericCommand: IIdentifiable, IStringIdentifiable
    {

        /// <summary>Executes the current command synchronously.
        /// <para>It is on command itself to interpret and validate its arguments, as well
        /// as the type of the value it returns, if any.</para></summary>
        /// <param name="parameters">Command parameters.</param>
        /// <returns>Result of command execution. Can also  return null (indicating that there 
        /// is no return value for this command).</returns>
        object Execute(params object[] parameters);

        /// <summary>Executes the current command asynchronously.
        /// <para>It is on command itself to interpret and validate its arguments, as well
        /// as the type of the value it returns, if any.</para></summary>
        /// <param name="parameters">Command parameters.</param>
        /// <returns>Task that can be avaited to obtain the result of command execution, which 
        /// can also be null (indicating that there is no return value for this command).</returns>
        Task<object> ExecuteAsync(params object[] parameters);


    }

}
