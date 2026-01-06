#nullable disable

/*****************************/
/*                           */
/*    MISCELLANEOUS TOOLS    */
/*                           */
/*****************************/


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using System.Runtime.InteropServices;
using System.Drawing.Printing;
using System.Drawing.Imaging;



namespace IG.Forms
{
	/// <summary>
	/// Summary description for FaidMessage1.
	/// </summary>
    /// 


    delegate void VoidDelegate();  /// Reference to a function without arguments & return value.
    public delegate void FormDelegate(Form f);    // Reference to a function with a Form argument.
    public delegate void ControlDelegate(Control ct);    // Reference to a function with a Control argument.



    /// <summary>Various forms utilities.</summary>
    public class UtilForms //: System.Windows.Forms.Form
    {
        /// Common tools for Windows forms


        private static object _lock;

        /// <summary>Global lock object for this class and for containing assembly.</summary>
        public static object Lock
        {
            get
            {
                if (_lock == null)
                {
                    lock(IG.Lib.Util.LockGlobal)
                    {
                        if (_lock == null)
                            _lock = new object();
                    }
                }
                return _lock;
            }
        }


        #region Data.Auxiliary


        private static Font _defaultFont = null;

        /// <summary>Default font used in forms.</summary>
        public static Font DefaultFont
        {
            get
            {
                if (_defaultFont == null)
                {
                    lock (UtilForms.Lock)
                    {
                        if (_defaultFont == null)
                            _defaultFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                    }
                }
                return _defaultFont;
            }
            protected internal set
            {
                if (!object.ReferenceEquals(value, _defaultFont))
                {
                    _defaultFont = value;
                }
            }
        }

        #endregion Data.Auxiliary


        #region EVENTS


        /// <summary>Sends the specified keystorkes to the specified control.
        /// <para>Examples: "A", "B", "F1", "{ENTER}", "{DELETE}", "{END}", </para>
        /// <para>"^AB" - hold Ctrl while A is pressed, then press B </para>
        /// <para>"+(AB)" (hold Shift while A and B are pressed),</para>
        /// <para>"%{ENTER}" (hold Alt while Enter is pressed).</para>
        /// </summary>
        /// <param name="targetControl">Target control to which the keystrokes are sent.</param>
        /// <param name="keyCode">A string specifying keys or sequence thereof to be sent to the control.</param>
        /// <remarks><para>List of codes accepted:</para>
        /// <para> http://msdn.microsoft.com/en-us/library/system.windows.forms.sendkeys.aspx </para></remarks>
        public static void GenerateKeyPress(Control targetControl, string keyCode)
        {
            targetControl.Focus();
            SendKeys.SendWait(keyCode);
        }

        #endregion EVENTS


        #region GENERAL_UTILITIES

        /// <summary>Blinks the specified control with default number of blinks and blink interval.</summary>
        /// <param name="control">Control to be blinked.</param>
        public static void BlinkControl(Control control)
        {
            BlinkForm(control, 2, 60);
        }

        /// <summary>Blinks the specified control with the specified number of blinks and blink interval.</summary>
        /// <param name="numBlinks">Number of blinks (exchanges of normal color / blink color).</param>
        /// <param name="control">Control that is blinked.</param>
        public static void BlinkForm(Control control, int numBlinks, int blinkTimeMs)
        {
            new ControlManipulator(control).Blink(numBlinks, (double)blinkTimeMs / 1.0e3);
        }

        #endregion GENERAL_UTILITIES


    }  // class UtilForms





}
