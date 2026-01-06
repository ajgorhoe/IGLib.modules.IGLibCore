#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using IG.Lib;
using IG.Num;
using System.Drawing;

namespace IG.Forms
{



    /// <summary>Stores properties of a specified control, and restores them to on arbitrary controls.
    /// <para>Can be used to copy properties from one to several controls, or to temporary store control properties for
    /// some manipulation that is performed on controls (such as blinking of control background, temporary moving and size changes).</para></summary>
    /// <remarks>
    /// <para>This class operates in two main modes.</para>
    /// <para>In the first mode, desired properties of the control are stored (i.e., partial snapshot of the specified control state is made), 
    /// and are later restored to either the same control or to any number of specified controls.</para>
    /// <para>In the second mode, the control reference is stored in the class, and the class is then used for saving and restoring properties
    /// of this particular control. Mixted modes are also possible.</para>
    /// </remarks>
    public class ControlPropertyStore
    {


        /// <summary>Constructs a new storege object for control properties, and innitializes it with the specified control.
        /// <para>Contol's main properties are saved immediately, if not specified differently by the <paramref name="saveProperties"/> parameter.</para></summary>
        /// <param name="c">Control whose properties are handled. If not null then properties are saved.</param>
        /// <param name="saveProperties">Specifies whetherr control's properties are stored immediately. Default is true.</param>
        /// 
        public ControlPropertyStore(Control c, bool saveProperties = true) : this()
        {
            this.Control = c;  // this will automatically save control's properties
        }

        protected ControlPropertyStore()
        {
            BackColor = Color.LightGray;
            ForeColor = Color.White;
            Font = UtilForms.DefaultFont;
            Text = "<< Text not set. >>";
        }

        private Control _control;

        /// <summary>Sets the control that is taken care of by this object, and immediately saves its main properties if 
        /// this is specified by the <paramref name="saveProperties"/> parameter.</summary>
        /// <param name="control">Sontrol that is taken are of by this class.</param>
        /// <param name="saveProperties">Indicates whether control's properties should also be saved immediately.</param>
        public void SetControl(Control control, bool saveProperties = true)
        {
            if (control != _control)
            {
                _control = control;
            }
            if (_control != null && saveProperties)
                SaveProperties(_control);
        }

        /// <summary>Control that is manipulated.
        /// <para>Setter also saves control's properties immediately.</para></summary>
        public Control Control
        {
            get { return _control; }
            set
            {
                SetControl(value, true /* saveProperties */);
            }
        }



        #region StoredProperties

        public Color BackColor { get; set; }

        public Color ForeColor { get; set; }

        public Font Font { get; set; }

        public string Text { get; set; }




        #endregion StoreedProperties


        #region SaveAndRestore

        /// <summary>Saves all main properties of the specified control.</summary>
        /// <param name="c">Control. If null then internally stored control is taken, if that is also null then <see cref="InvalidOperationException"/> is thrown.</param>
        public virtual void SaveProperties(Control c = null)
        {
            if (c == null)
                c = Control;
            if (c == null)
                throw new InvalidOperationException("Control is not specified, neither through argument nor internally.");
            SaveAppearance(c);
            // SavePosition(c);
        }

        /// <summary>Restores all main properties of the specified control.</summary>
        /// <param name="c">Control. If null then internally stored control is taken, if that is also null then <see cref="InvalidOperationException"/> is thrown.</param>
        public virtual void restoreProperties(Control c = null)
        {
            if (c == null)
                c = Control;
            if (c == null)
                throw new InvalidOperationException("Control is not specified, neither through argument nor internally.");
            RestoreAppearance(c);
            // RestorePosition(c);
        }

        /// <summary>Saves main appearance related properties of the specified control.</summary>
        /// <param name="c">Control. If null then internally stored control is taken, if that is also null then <see cref="InvalidOperationException"/> is thrown.</param>
        public virtual void SaveAppearance(Control c = null)
        {
            if (c == null)
                c = Control;
            if (c == null)
                throw new InvalidOperationException("Control is not specified, neither through argument nor internally.");
            BackColor = c.BackColor;
            ForeColor = c.ForeColor;
            Font = c.Font;
            Text = c.Text;
        }

        /// <summary>Restores main appearance related properties of the specified control.</summary>
        /// <param name="c">Control. If null then internally stored control is taken, if that is also null then <see cref="InvalidOperationException"/> is thrown.</param>
        public virtual void RestoreAppearance(Control c = null)
        {
            if (c == null)
                c = Control;
            if (c == null)
                throw new InvalidOperationException("Control is not specified, neither through argument nor internally.");
            c.BackColor = BackColor;
            c.ForeColor = ForeColor;
            c.Font = Font;
            c.Text = Text;
        }

        /// <summary>Saves main position related properties on the specified control (to the values stored in the current object).</summary>
        /// <param name="c">Control. If null then internally stored control is taken, if that is also null then <see cref="InvalidOperationException"/> is thrown.</param>
        public void SavePosition(Control c = null)
        {
            if (c == null)
                c = Control;
            if (c == null)
                throw new InvalidOperationException("Control is not specified, neither through argument nor internally.");

            throw new NotImplementedException("Saving position not yet implemented.");
        }

        /// <summary>Restores main position related properties on the specified control (to the values stored in the current object).</summary>
        /// <param name="c">Control. If null then internally stored control is taken, if that is also null then <see cref="InvalidOperationException"/> is thrown.</param>
        public void RestorePosition(Control c = null)
        {
            if (c == null)
                c = Control;
            if (c == null)
                throw new InvalidOperationException("Control is not specified, neither through argument nor internally.");

            throw new NotImplementedException("Restoring position not yet implemented.");
        }

        #endregion SaveAndRestore


    }




}