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

    /// <summary>Objects of this type have a unique ID (unique for all objects of a given type)
    /// and can be registered in the register of existing objects of the specified type.
    /// Implementation notes: 
    /// Registerable.Example contains an example of how to do that, or can even be inherited
    /// to provide all functionality automatically (but this may not be feasible because multiple 
    /// inheritance is not supported, and a class already inherits form another one).</summary>
    public interface IRegisterable<T> : IIdentifiable
            where T : class, IIdentifiable
    {

        /// <summary>Gets the object register where the current object can be registered.</summary>
        ObjectRegister<T> ObjectRegister { get; }

        /// <summary>Registers the current object.
        /// Subsequent calls (after the first one) have no effect.</summary>
        void Register();

        /// <summary>Returns true if the current object is registered, false if not.</summary>
        /// <returns></returns>
        bool IsRegistered();

        /// <summary>Unregisters the current object if it is currently registered. 
        /// Can be performed several times, in this case only the first call may have effect.</summary>
        void Unregister();

    }  // interface IRegisterable<T>


}
