
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Lib
{


    /// <summary>Object store.
    /// Objects of the specified type can be stored here for later reuse (efficiency improvement).</summary>
    /// <typeparam name="T">Type of objects to be stored, must be a reference type.</typeparam>
    public interface IObjectStore<T> where T : class
    {

        /// <summary>Gets the current number of objects.</summary>
        int Count { get; }

        /// <summary>Gets or sets the maximal number of objects that can be stored by this object store.</summary>
        int MaxCount { get; set; }

        /// <summary>Returns true if the specified object is eligible for storing in this object storage,
        /// false if it is not.</summary>
        bool IsEligible(T obj);

        /// <summary>Stores the specified object if the object is eligible for storage in this store
        /// and if The maximal number of stored object will not be exceeded.</summary>
        /// <param name="obj">Object to be stored.</param>
        /// <returns>true if the object has actually been stored, false if not.</returns>
        bool TryStore(T obj);

        /// <summary>Stores the specified object in the store.
        /// Throws ArgumentException if the specified object is not eligible for storing in this store.
        /// If the maximum number of objects would be exceeded then nothing happens.</summary>
        /// <returns>true if the object has actually been stored, false if not.</returns>
        bool StoreEligible(T obj);

        /// <summary>Returns an object from this object store, or null if it is not possible to provide
        /// an eligible object. 
        /// If the store contains no objects, it tries to create and return a new eligible object.
        /// Should not throw an exception.</summary>
        T TryGet();

        /// <summary>Returns the last object from this object store, or null if there are no
        /// objects on it. 
        /// Dose not attempt to create a new object.</summary>
        T TryGetStored();
        
        /// <summary>Returns a non-null object that is eligible for storing in this object store.
        /// If the store itself does not contain any objects, an object is created anew, or exception
        /// is thrown if this is not possible.
        /// IMPORTANT:
        /// TryGet returns an object or null if the object can not be returned (does not throw an exception),
        /// and TryGetStored returns an eligible object only if any are stored, otherwise returns null.</summary>
        T GetEligible();


    }  // interface IObjectStore<T>


    /// <summary>Object store.
    /// Objects of the specified type can be stored here for later reuse (efficiency improvement).
    /// IMPORTANT:
    /// Override IsEligible(), NotEligibleMessage() and TryGetNew() and possibly TryStore() 
    /// methods in derived classes when applicable.</summary>
    /// <typeparam name="T">Type of objects to be stored, must be a reference type.</typeparam>
    public class ObjectStore<T> : IObjectStore<T>, ILockable where T: class
    {

        protected object _lock = new object();

        public object Lock { get { return _lock; } }

        protected List<T> _objects = new List<T>();

        /// <summary>Gets the current number of objects.</summary>
        public int Count
        {  
            get { return _objects.Count; }
            protected set 
            {
                throw new InvalidOperationException("Number of stored objects can not be specified explicitly.");
            }
        }

        int _maxCount = 0;  // 0 - unlimited

        /// <summary>Gets or sets the maximal number of objects that can be stored by this object store.</summary>
        public int MaxCount
        {
            get { return _maxCount; }
            set 
            {
                lock (_lock)
                {
                    if (value < 0)
                        throw new ArgumentException("Value less than 0 can not be assigned to MaxCount.",
                            "MaxCount");
                    _maxCount = value;
                    if (value > 0 && _objects.Capacity > value)
                        _objects.Capacity = value;
                }
            }
        }


        /// <summary>Returns true if the specified object is eligible for storing in this object storage,
        /// false if it is not.</summary>
        public virtual bool IsEligible(T obj)
        { if (obj!=null) return true; else return false; }

        /// <summary>Removes ineligible obects from the list.</summary>
        protected virtual void ClearIneligible()
        {
            int num = _objects.Count;
            int lastEligible = -1, lastChecked = 0;
            while (lastChecked < Count)
            {
                T obj = _objects[lastChecked];
                if (IsEligible(obj))
                {
                    ++lastEligible;
                    if (lastEligible != lastChecked)
                        _objects[lastEligible] = obj;
                }
                ++lastChecked;
            }
            if (lastEligible < num - 1)
            {
                Util.ResizeList(ref _objects, lastEligible + 1, null);
            }
        }


        /// <summary>Returns a message indicating why the specified object is not eligible for storage
        /// in the current store.</summary>
        protected virtual String NotEligibleMessage(object obj)
        {
            if (obj == null)
                return "Object is not specified (null reference).";
            return "";
        }


        /// <summary>Stores the specified object if the object is eligible for storage in this store
        /// and if The maximal number of stored object will not be exceeded.</summary>
        /// <param name="obj">Object to be stored.</param>
        /// <returns>true if the object has actually been stored, false if not.</returns>
        public virtual bool TryStore(T obj)
        {
            lock (_lock)
            {
                if (IsEligible(obj))
                {
                    if (MaxCount > 0 && Count >= MaxCount)
                        return false;
                    _objects.Add(obj);
                    return true;
                } else
                    return false;
            }
        }


        /// <summary>Stores the specified object in the store.
        /// WARNING:
        /// Throws ArgumentException if the specified object is not eligible for storing in this store.
        /// If the maximum number of objects would be exceeded then nothing happens.</summary>
        /// <returns>true if the object has actually been stored, false if not.</returns>
        public bool StoreEligible(T obj)
        {
            lock(_lock)
            {
                if (IsEligible(obj))
                {
                    if (MaxCount > 0 && Count >= MaxCount)
                        return false;
                    bool ret = TryStore(obj);
                    return ret;
                } else
                    throw new ArgumentException("Object is not eligible for storage in this class."
                        + Environment.NewLine + "Reason: " + NotEligibleMessage(obj));
            }
        }



        /// <summary>Returns an object from this object store, or null if it is not possible to provide
        /// an eligible object. 
        /// If the store contains no objects, it tries to create and return a new eligible object.
        /// Should not throw an exception.</summary>
        public T TryGet()
        {
            lock(_lock)
            {
                int lastIndex = Count-1;
                if (lastIndex >= 0)
                {
                    T ret = _objects[lastIndex];
                    _objects.RemoveAt(lastIndex);
                    return ret;
                } else
                {
                    return TryGetNew();
                }
            }
        }


        /// <summary>Returns the last object from this object store, or null if there are no
        /// objects on it. 
        /// Dose not attempt to create a new object.</summary>
        public T TryGetStored()
        {
            lock (_lock)
            {
                int lastIndex = Count - 1;
                if (lastIndex >= 0)
                {
                    T ret = _objects[lastIndex];
                    _objects.RemoveAt(lastIndex);
                    return ret;
                }
                else
                    return null;
            }
        }


        /// <summary>Returns a non-null object that is eligible for storing in this object store.
        /// If the store itself does not contain any objects, an object is created anew, or exception
        /// is thrown if this is not possible.
        /// IMPORTANT:
        /// TryGet returns an object or null if the object can not be returned (does not throw an exception),
        /// and TryGetStored returns an eligible object only if any are stored, otherwise returns null.</summary>
        public T GetEligible()
        {
            lock (_lock)
            {
                T ret = TryGet();
                if (ret == null)
                {
                    ret = GetNew();
                }
                return ret;
            }
        }


        /// <summary>Returns a newly created object eligible for storage, or null if such an object
        /// can not be created. This method should not throw an exception.</summary>
        protected virtual T TryGetNew()
        {
            return null;
        }


        /// <summary>Returns a newly xreated object eligible for storage in this object store.
        /// Exception is thrown if such an object can not be created.</summary>
        protected T GetNew()
        {
            T ret = TryGetNew();
            if (ret==null)
                throw new InvalidOperationException("Object storage: Could not create a new object.");
            return ret;
        }

    }  // class ObjectStore<T>


}
