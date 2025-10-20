// Copyright © Igor Grešovnik (2008 - present); license:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IG.Lib
{

    /// <summary>Lockable object, has a Lock property that returns object on which
    /// lock must be performed in order to lock the object.</summary>
    public interface ILockable
    {
        object Lock { get; }
    }

}
