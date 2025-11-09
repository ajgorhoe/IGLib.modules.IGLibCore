// Copyright © Igor Grešovnik (2008 - present); license:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

#nullable disable

<<<<<<< HEAD

=======
>>>>>>> dev/25_11_04_MovingCommandsFromSandbox
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IG.Lib
{

    /// <summary>Classes whose instances have unique string IDs.</summary>
    public interface IStringIdentifiable
    {

        /// <summary>Unique string ID of the current object; often used in logs.</summary>
        string StringId { get; }

    }

}
