using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Forms
{

    /// <summary>Marker interface indicating that a form can be closed from the contained control.</summary>
    /// <remarks><para>This interface must be implemented by forms that can be closed by a control that is contained 
    /// in the form. It represents a kind of message to the contained forms that they are allowed to close the
    /// top level form.</para>
    /// <para>For example, a control that contains a close button can check if its top level form implements
    /// this interface, and closes its top level form only when the button is pressed.</para></remarks>
    public interface IClosableFromContainedForm
    {
        void Close();
    }

}
