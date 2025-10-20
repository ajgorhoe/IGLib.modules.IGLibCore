// Copyright © Igor Grešovnik (2008 - present); license:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IG.Lib
{


    /// <summary>Provides implementation of register of objects of the specified type.
    /// Also generates unique IDs for objects of this type.
    /// This class utilizes implementation of IIdentifiable and IRegisterable interfaces.
    /// Implementation notes for IRegistable:
    /// For implementation, use a static instance of this class, and an a nonstatic instance of 
    /// the IdProcy class (to generate and hold object's unique ID). subclass of this clas, 
    /// initialized by that static instance.
    /// Example implementation can be found in the ExampleInterfaceImplementation subclass of this class.</summary>
    public class ObjectRegister<T> :
                                IdGenerator, ILockable
                    where T : class, IIdentifiable  // TODO: check if T must really be IRegistrable!
    {

        /// <summary>Creates an object that generates unique IDs (in the scope of the current instance) 
        /// and provides registration of objects of the given type.</summary>
        public ObjectRegister()
            : base()
        { }

        /// <summary>Creates an object that generates unique IDs (in the scope of the current instance) 
        /// with the specified first ID generated, and provides registration of objects of the given type.</summary>
        /// <param name="startId">The first ID that will be generated with this object's GetNewId() method.</param>
        public ObjectRegister(int startId)
            : base(startId)
        { }

        protected SortedDictionary<int, T> _register = new SortedDictionary<int, T>();

        protected object _internalLock = new object();


        /// <summary>Registers the specified object with the specified key (ID).
        /// Warning: this method does not check whether the specified key actually corresponds to
        /// object's ID that is obtained by the object's IIdentifiable.Id property. Therefore it is private.</summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        private void Register(int id, T obj)
        {
            lock (_internalLock)
            {
                if (obj != null)
                    if (!IsRegistered(id))
                        _register.Add(id, obj);
            }
        }

        /// <summary>Adds the specified object to the register, using its Id property.
        /// If the object is already registered (i.e. the register already contains its Id)
        /// then </summary>
        public void Register(T obj)
        {
            lock (_internalLock)
            {
                if (obj != null)
                    Register(obj.Id, obj);
            }
        }


        /// <summary>Returns true if object with the specified ID is already registered with the current object,
        /// false otherwise.</summary>
        /// <param name="id">IIdentifiable object's ID, obtained by its ID property.</param>
        public bool IsRegistered(int id)
        {
            lock (_internalLock) { return _register.ContainsKey(id); }
        }


        /// <summary>Returns true if the specified object is registered with the current object, false otherwise.</summary>
        public bool IsRegistered(T obj)
        {
            lock (_internalLock)
            {
                if (obj != null)
                    return _register.ContainsKey(obj.Id);
                else
                    return false;
            }
        }

        /// <summary>Returns object that is registered with this object with the specified id, or null
        /// if such an object is not registered.</summary>
        public T GetRegisteredInstance(int id)
        {
            lock (_internalLock)
            {
                if (IsRegistered(id))
                    return _register[id];
                else return null;
            }
        }

        /// <summary>Unregisters object with the specified ID.
        /// WARNING:
        /// This method should only be used in finalization methods of objects that implement the 
        /// IRegisterable interface.</summary>
        /// <param name="id"></param>
        /// <returns>Reference of the object that has been</returns>
        public void Unregister(int id)
        {
            lock (_internalLock)
            {
                if (IsRegistered(id))
                    _register.Remove(id);
            }
        }

    }  // class Registerable




}
