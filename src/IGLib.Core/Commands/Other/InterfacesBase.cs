
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IG.Lib;

namespace IGLib.Core
{


    ///// <summary>Classes whose instances have unique integer IDs.</summary>
    //public interface IIdentifiable
    //{
    //    /// <summary>Returns unique ID (in the scope of a given type) of the current object.</summary>
    //    int Id { get; }
    //}

    ///// <summary>Classes whose instances have unique string IDs.</summary>
    //public interface IStringIdentifiable
    //{

    //    /// <summary>Unique string ID of the current object; often used in logs.</summary>
    //    string StringId { get; }

    //}

    ///// <summary>Lockable object, has a Lock property that returns object on which
    ///// lock must be performed in order to lock the object.</summary>
    //public interface ILockable
    //{
    //    object Lock { get; }
    //}


    public class IdentifiableLockableBase: ILockable, IIdentifiable, IStringIdentifiable
    {

        /// <summary>Static object used for global locking within the current process.
        /// <para>Can only be used for locking quick operations.</para></summary>
        public static object GlobalLock { get; set; } = new object();

        private static int _nextId = 0;

        /// <summary>Returns another ID that is unique for objects of the containing class its and derived classes.</summary>
        protected static int GetNextId()
        {
            lock (GlobalLock)
            {
                ++_nextId;
                return _nextId;
            }
        }

        
        #region ThreadLocking

        private object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.
        /// Do not use this object for locking in class' methods, for this you should use 
        /// InternalLock.</summary>
        public object Lock { get { return _mainLock; } }

        #endregion ThreadLocking

        #region IIdentifiable

        private int _id = GetNextId();

        /// <summary>Unique ID for objects of the currnet and derived classes.</summary>
        public int Id
        { get { return _id; } }

        protected virtual string GetDefaultStringId()
        {
            return GetType().Name + "_" + Id.ToString("D" + 5);
        }

        private string _stringId;

        /// <summary>Unique string ID of the current object; often used in logs.</summary>
        public virtual string StringId
        {
            get
            {
                if (_stringId == null)
                {
                    lock (Lock)
                    {
                        if (_stringId == null)
                            _stringId = GetDefaultStringId();
                    }
                }
                return _stringId;
            }
        }

        #endregion IIdentifiable

    }

}
