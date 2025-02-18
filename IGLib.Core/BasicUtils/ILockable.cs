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
