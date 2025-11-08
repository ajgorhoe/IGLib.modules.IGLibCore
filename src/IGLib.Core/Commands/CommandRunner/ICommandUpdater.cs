
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Commands
{

    /// <summary>Marker interface for command updaters.</summary>
    /// <seealso cref="ICommandUpdater{CommandType, ExecutionInfoType}"/>
    public interface ICommandUpdater
    {  }

    /// <summary>Interface for command updater classes, which install command on command runners (interface 
    /// <see cref="ICommandUpdater{CommandType, ExecutionInfoType}"/>).
    /// <para>When implementing this interface, in its <see cref="UpdateCommands(ICommandRunner{CommandType, ExecutionInfoType})"/>
    /// mechod, put the code that updates the state of the command runner upon which the updater acts; this can include adding named
    /// commands, creating aliases for commands, removing commands, etc. In general, the updater may also need to instantiate commands
    /// that it adds to the runner, unless the commands are created elsewhere and injected.</para>
    /// <para>Once the updater is created, it can update an runner by directly calling its <see cref="UpdateCommands(ICommandRunner{CommandType, ExecutionInfoType})"/>
    /// method on it. However, this can also be done by passing the commandupdater as argument to the runner's
    ///  method, which will then take care of calling the updating method on the runner.</para>
    ///  <para>The updater enables separation of two concerns: creation and installation of commands, which is taken care of the domain
    ///  that provides certain commsnds (the command producing domain of the software); and executing (consuming) the commands,
    ///  which is done by a separate command runner class, in any part of the software that needs to have access to commands
    ///  from the producingg domain. This can serve as method for connecting very different parts of the software in a decoupled
    ///  way, without these parts needing to know a any details about each other.</para></summary>
    /// <typeparam name="CommandType">Type of command objects used by command runners that the current updater updates.</typeparam>
    /// <typeparam name="ExecutionInfoType">Type of execution info objects used by command runners that the current updater updated.</typeparam>
    public interface ICommandUpdater<CommandType, ExecutionInfoType>:
            ICommandUpdater
        where CommandType : class, IGenericCommand
        where ExecutionInfoType : IExecutionInfo<CommandType, ExecutionInfoType> 
    {

        void UpdateCommands(ICommandRunner<CommandType, ExecutionInfoType> commandRunner);

    }


}
