// Copyright © Igor Grešovnik (2008 - present); license:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IG.Lib
{

    /// <summary>Classes whose instances have unique integer IDs.</summary>
    public interface IIdentifiable
    {
        /// <summary>Returns unique ID (in the scope of a given type) of the current object.</summary>
        int Id { get; }
    }

}
