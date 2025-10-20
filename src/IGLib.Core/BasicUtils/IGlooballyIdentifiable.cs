// Copyright © Igor Grešovnik (2008 - present); license:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IG.Lib
{

    /// <summary>Classes that contain unique application-level ID. This helps identify objects passed around in tests.</summary>
    public interface IGloballyIdentifiable
    {
        int ObjectId { get; }
    }

    /// <summary>Base class for classes whose objects have a unique process-wide ID.
    /// <para>The <see cref="NextGlobalId"/> static properrty of the class can be used to obtain a process-wide
    /// unique ID for other purposes, such as unique variable names uses in scripts.</para></summary>
    public abstract class GloballyIdentifiableBase
    {
        private static int _nextId = 0;
        private static object _lock = new object();
        
        /// <summary>Gets the next integer ID that is unique i nthe scope of the process.</summary>
        public static int NextGlobalId
        {
            get
            {
                lock (_lock)
                {
                    return (++_nextId);
                }
            }
        }

        /// <summary>Process-wide unique object ID. This ID is unique across all objects of classes inheriting from
        /// <see cref="GloballyIdentifiableBase"/>, created within the current process.</summary>
        public int ObjectId { get; } = NextGlobalId;
    }

}
