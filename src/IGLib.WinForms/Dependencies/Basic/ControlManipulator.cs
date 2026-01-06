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



    /// <summary>Performs various operations on the specified controls:
    /// <para>  - Blinks the control.</para></summary>
    public class ControlManipulator
    {

        public ControlManipulator()
        { }

        /// <summary>Constructs control manipulators, initialized with the specified controls to be manipulated.</summary>
        /// <param name="controls"></param>
        public ControlManipulator(params Control[] controls): this()
        {
            AddControls(controls);
        }

        #region Data

        private Dictionary<Control, ControlPropertyStore> _controls = new Dictionary<Control, ControlPropertyStore>();

        /// <summary>Dictionary that contains controls, and for each control it contains a storage object that saves and restores
        /// control's state when necessary.</summary>
        protected Dictionary<Control, ControlPropertyStore> Controls
        {
            get { return _controls; }
        }

        /// <summary>Removes all manipulated controls from the object.</summary>
        public void RemoveAllControls()
        {
            Controls.Clear();
        }


        /// <summary>Removes the specified control from this object, if it contains it.</summary>
        /// <param name="control"></param>
        public void RemoveControl(Control control)
        {
            if (Controls.ContainsKey(control))
                Controls.Remove(control);
        }

        public void AddControl(Control control)
        {
            if (control != null)
            {
                Controls.Add(control, new ControlPropertyStore(control, true /* saveProperties */));
            }
        }

        public void AddControls(params Control[] controls)
        {
            foreach (Control ctrl in controls)
            {
                AddControl(ctrl);
            }
        }

        public void SaveProperties()
        {
            foreach (ControlPropertyStore store in Controls.Values)
            {
                store.SaveProperties();
            }
        }

        public void RestoreProperties()
        {
            foreach (ControlPropertyStore store in Controls.Values)
            {
                store.SaveProperties();
            }
        }


        private System.Windows.Forms.Timer _timer;

        /// <summary>Timer used for blinking, has pre-installed event handler. Created as necessary (azy evaluation).</summary>
        protected System.Windows.Forms.Timer BlinkTimer
        {
            get
            {
                if (_timer == null)
                {
                    lock (UtilForms.Lock)
                    {
                        if (_timer == null)
                        {
                            _timer = new System.Windows.Forms.Timer();
                            _timer.Tick += new System.EventHandler(BlinkTimer_Tick);
                        }
                    }
                }
                return _timer;
            }
        }

        #endregion Data



        #region Actions.Blink


        protected static Color DefaultBlinkColor = Color.Orange;

        public void Blink(int numBlinks = 2, double blinkIntervalSeconds = 0.2, params Control[] controls)
        {
            Blink(DefaultBlinkColor, numBlinks, blinkIntervalSeconds, controls);
        }

        public void Blink(Color blinkBackground, int numBlinks = 2, double blinkIntervalSeconds = 0.2, params Control[] controls)
        {
            if (Controls != null)
                AddControls(controls);
            this.BlinkBackColor = blinkBackground;
            this.NumBlinks = numBlinks;
            this.BlinkIntervalSeconds = blinkIntervalSeconds;
            this.StartBlinking();
        }

        /// <summary>Starts blinking the control.</summary>
        public void StartBlinking()
        {
            DoBlink = false;
            BlinkTimer.Stop();
            ApplyNormalColor();
            BlinkTimer.Interval = BlinkIntervalMs;
            NumPerformedBlinks = 0;
            DoBlink = true;
            BlinkTimer.Start();
            ApplyBlinkColor();  // this will also set _isBlinkOn to true
        }


        /// <summary>Interrups blinking process, if it is currently happening. It is ensured that controls' normal background will 
        /// be reset before timer is actually shut off.</summary>
        public void StopBlinking()
        {
            DoBlink = false;
            BlinkTimer.Stop();
            ApplyNormalColor();
        }

        

        private Color _blinkBackColor = Color.Orange;

        /// <summary>Background color used for blinking.</summary>
        public Color BlinkBackColor { get { return _blinkBackColor; } set { _blinkBackColor = value; } }


        private double _blinkIntervalSeconds = 0.2;

        /// <summary>Interval between color switches, in seconds.
        /// <para>This is actually the time of half of a blink (the time interval in which one of two colors is displayed.)</para></summary>
        public double BlinkIntervalSeconds
        { get { return _blinkIntervalSeconds; } set { _blinkIntervalSeconds = value; } }

        /// <summary>Interval between successive blinks, in milliseconds. Bound to <see cref="BlinkIntervalSeconds"/>, so these
        /// properties can be used interchangeably, dependent on which time unit you prefer to use.</summary>
        public int BlinkIntervalMs
        { get { return (int)(BlinkIntervalSeconds * 1000.0); } set { BlinkIntervalSeconds = (double)value / 1000.0; } }


        private int _numBlinks = 2;

        /// <summary>Number of blinks.</summary>
        public int NumBlinks
        {
            get { return _numBlinks; } set { _numBlinks = value; }
        }

        
        private bool _isBlinkOn = false;

        /// <summary>Indicates whether blinking is currently switched on.
        /// <para>When true, the <see cref="BlinkTimer"/> tick handlers will alternately switch background colors of controls 
        /// and cause blinking in this way.</para></summary>
        protected bool IssBlinkOn
        { get { return _isBlinkOn; } set { _isBlinkOn = value; } }

        protected int NumPerformedBlinks { get;  set; } 


        private bool _doBlink1 = false;

        protected bool DoBlink { get { return _doBlink1; } set { _doBlink1 = value; } }


        protected void ApplyBlinkColor()
        {
            foreach (Control c in Controls.Keys)
            {
                c.BackColor = BlinkBackColor;
            }
            IssBlinkOn = true;
        }

        protected void ApplyNormalColor()
        {
            foreach (ControlPropertyStore store in Controls.Values)
            {
                if (store.Control != null)
                    store.Control.BackColor = store.BackColor;  // restore the saved normal color of each control
            }
            IssBlinkOn = false;
        }


        /// <summary>Timer event handler that performes blining of control in the specified color.</summary>
        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            if (IssBlinkOn)
            {
                // If blink color is set, we must set back to normal in any case, even if _doblink is false
                ApplyNormalColor();
                ++NumPerformedBlinks;
                if (NumPerformedBlinks >= NumBlinks)
                {
                    DoBlink = false;
                    _timer.Stop();
                }

            } else
            {
                if (DoBlink)
                    ApplyBlinkColor();
                else
                    _timer.Stop(); // this allows to interrupt blinking
            }
        }



        #endregion Actions.Blink






    }  // Class ControlManipulator



}
