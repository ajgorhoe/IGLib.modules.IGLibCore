using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLibCore.BasicUtils
{

    /// <summary>Classes that contain unique application-level ID. This helps identify objects passed around in tests.</summary>
    public interface IGloballyIdentifiable
    {
        int ObjectId { get; }
    }

    /// <summary>Base class for classes whose objects have a unique application-wide ID.</summary>
    public abstract class GloballyIdentifiableBase
    {
        private static int _nextId = 0;
        private static object _lock = new object();
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

        /// <summary>Application-wide unique object ID (unique across objects of all derived types).</summary>
        public int ObjectId { get; } = NextGlobalId;
    }

}
