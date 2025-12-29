// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IG.Lib;
using IG.Forms;

namespace IG.Forms
{

    /// <summary>Control for testing window positioning.</summary>
    /// $A Igor xx Oct09;
    public partial class WindowPositionerControl : UserControl
    {


        /// <summary>Constructs a new positioner form with undefined positioner, positioned window and master window.</summary>
        public WindowPositionerControl(): this(null /* positioner */, null /* positionedWindow */, null /* masterWindow */)
        {  }

        
        /// <summary>Constructs a new positioner form with unspecified positioner and specified positioned window and master window.
        /// <para>Positioner is created automatically and initialized to default state.</para></summary>
        /// <param name="positionedWindow">Window that is positioned.</param>
        /// <param name="masterWindow">Master window.</param>
        public WindowPositionerControl(Form positionedWindow, Form masterWindow)
            : this(null /* positioner */, positionedWindow, masterWindow)
        {  }

        /// <summary>Constructs a new positioner form with unspecified positioner and master window, and specified positioned window.
        /// <para>Positioner is created automatically and initialized to default state.</para></summary>
        /// <param name="positionedWindow">Window that is positioned.</param>
        public WindowPositionerControl(Form positionedWindow)
            : this(null /* positioner */, positionedWindow, null /* masterWindow */)
        {  }


        /// <summary>Constructs a new positioner form with the specified positioner and undefined positioned window and master window.</summary>
        /// <param name="positioner">Positioner object that positions the window.</param>
        public WindowPositionerControl(WindowPositioner positioner)
            : this(positioner, null /* positionedWindow */, null /* masterWindow */)
        { }
        
        /// <summary>Constructs a new positioner form with the specified positioner and positioned window, and undefined master window.</summary>
        /// <param name="positioner">Positioner object that positions the window.</param>
        /// <param name="positionedWindow">Window that is positioned.</param>
        public WindowPositionerControl(WindowPositioner positioner, Form positionedWindow)
            : this(positioner, positionedWindow, null /* masterWindow */)
        {  }

        /// <summary>Constructs a new positioner form with the specified positioner, positioned window and master window.</summary>
        /// <param name="positioner">Positioner object that positions the window.</param>
        /// <param name="positionedWindow">Window that is positioned.</param>
        /// <param name="masterWindow">Master window.</param>
        public WindowPositionerControl(WindowPositioner positioner, Form positionedWindow, Form masterWindow)
        {
            this.Positioner = positioner;
            this.Window = positionedWindow;
            this.MasterWindow = MasterWindow;

            InitializeComponent();
            this.lblTitle.Text = Title;
            
            // LaunchWindows(); // This was moved to the Load event handler.
            
            //// Set up some parameters:
            //shiftMasterRel.MinimumValue = -10;
            //shiftMasterRel.MaximumValue = 10;
            //shiftMasterRel.InitialValueX = 0;
            //shiftMasterRel.InitialValueY = 0;
            //shiftMasterRel.Increment = 0.05;


            //shiftScreenRel.MinimumValue = -2;
            //shiftScreenRel.MaximumValue = 2;
            //shiftScreenRel.InitialValueX = 0;
            //shiftScreenRel.InitialValueY = 0;
            //shiftScreenRel.Increment = 0.05;

        }

        #region OperationData

        protected WindowPositioner _positioner;

        /// <summary>Window positioner.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public WindowPositioner Positioner
        {
            get
            {
                if (_positioner == null)
                {
                    Positioner = new WindowPositioner(Window, MasterWindow);
                }
                return _positioner;
            }
            set
            {
                if (value != _positioner)
                {
                    _positioner = value;
                    if (value != null)
                    {
                        if (Positioner.Window != null)
                            this.Window = Positioner.Window;
                        else
                            Positioner.Window = this._window;
                        if (Positioner.MasterWindow != null)
                            this.MasterWindow = Positioner.MasterWindow;
                        else
                            Positioner.MasterWindow = this._masterWindow;
                    }
                }
            }
        }


        protected Form _window;

        /// <summary>Window to be positioned.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Form Window
        {
            get
            {
                if (_window != null)
                    if (_window.IsDisposed)
                        Window = null;
                if (_window == null)
                {
                    TestForm w = new TestForm();
                    w.Text = "Positioned window. " + w.Title;
                    Window = w;
                }
                return _window;
            }
            set
            {
                if (value != _window)
                {
                    _window = value;
                    if (_positioner!=null)
                        Positioner.Window = value;
                }
            }
        }


        protected Form _masterWindow;

        /// <summary>Master window relative to which position is set.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Form MasterWindow
        {
            get
            {
                if (_masterWindow != null)
                    if (_masterWindow.IsDisposed)
                        MasterWindow = null;
                if (_masterWindow == null)
                {
                    TestForm w = new TestForm();
                    w.Text = "Master window. " + w.Title;
                    MasterWindow = w;
                }
                return _masterWindow;
            }
            set
            {
                if (value != _masterWindow)
                {
                    _masterWindow = value;
                    if (_positioner!=null)
                        Positioner.MasterWindow = value;
                }
            }
        }

        public string Title
        {
            get { return "Window Positioner"; }
        }

        #endregion OperationData


        #region Operation 

        /// <summary>Converts string to double and returns the value.</summary>
        /// <param name="str">String to be converted.</param>
        /// <param name="defaultValue">Value returned if conversion is not successful.</param>
        public double DoubleVal(string str, double defaultValue)
        {
            double ret;
            if (double.TryParse(str, out ret))
                return ret;
            else
                return defaultValue;
        }

       /// <summary>Converts string to double and returns the value (or 0 if conversion was not successful).</summary>
        public double DoubleVal(string str) { return DoubleVal(str, 0.0); }

        /// <summary>Copies data from the form to positioner.</summary>
        public void CopyDataToPositioner()
        {
            if (Positioner != null)
            {
                // General data:
                Positioner.MasterWeight = DoubleVal(txtMasterWeight.Value.ToString());
                Positioner.WindowAlignment = alignWindow.Alignment;
                Positioner.ShiftAbsolute = shiftWindowAbsolute.Shift;
                Positioner.WindowShiftRelative = shiftWindowRel.Shift;
                // Relative to master window:
                Positioner.MasterWindowShiftRelative = shiftMasterRel.Shift;
                Positioner.MasterWindowAlignment = alignMaster.Alignment;
                // Relative to screen:
                Positioner.ScreenShiftRelative = shiftScreenRel.Shift;
                Positioner.ScreenAlignment = alignScreen.Alignment;
                Positioner.MovementPauseSeconds = double.Parse(txtPause.Text);
            }
        }

        public void CopyDataFromPositioner()
        {
                
                // General data:
                txtMasterWeight.Value = (decimal) Positioner.MasterWeight;
                alignWindow.Alignment = Positioner.WindowAlignment;
                shiftWindowAbsolute.Shift = Positioner.ShiftAbsolute;
                shiftWindowRel.Shift = Positioner.WindowShiftRelative;
                // Relative to master window:
                shiftMasterRel.Shift = Positioner.MasterWindowShiftRelative;
                alignMaster.Alignment = Positioner.MasterWindowAlignment;
                // Relative to screen:
                shiftScreenRel.Shift = Positioner.ScreenShiftRelative;
                alignScreen.Alignment = Positioner.ScreenAlignment;
                txtPause.Text = Positioner.MovementPauseSeconds.ToString();
        }

        public void LaunchWindows()
        {
            Form master = MasterWindow;
            Form positioned = Window;
            master.Location = new Point(
                Positioner.ScreenBounds.Left + Positioner.ScreenBounds.Width / 3,
                Positioner.ScreenBounds.Top + Positioner.ScreenBounds.Height / 3);

            positioned.Location = new Point(
                Positioner.ScreenBounds.Left + Positioner.ScreenBounds.Width / 2,
                Positioner.ScreenBounds.Top + Positioner.ScreenBounds.Height / 2);
            try
            {
                master.Show();
            }
            catch { }
            try
            {
                positioned.Show();
            }
            catch { }
            try
            {
                master.Location = new Point(
                    Positioner.ScreenBounds.Left + Positioner.ScreenBounds.Width / 3,
                    Positioner.ScreenBounds.Top + Positioner.ScreenBounds.Height / 3);
                positioned.Location = new Point(
                    Positioner.ScreenBounds.Left + Positioner.ScreenBounds.Width / 2,
                    Positioner.ScreenBounds.Top + Positioner.ScreenBounds.Height / 2);
            }
            catch { }
            MakeWindowsVisible();
        }

        public void MakeWindowsVisible()
        {
            try { MasterWindow.Activate(); UtilForms.BlinkControl(MasterWindow); }
            catch { }
            try { Window.Activate(); UtilForms.BlinkControl(Window); }
            catch { }
            this.Focus();
        }
        /// <summary>Launches the master window and the window to be positioned.</summary>
        private void btnLaunchWindows_Click(object sender, EventArgs e)
        {
            LaunchWindows();
        }

        /// <summary>Launches the master window and the window to be positioned.</summary>
        private void btnPositionWindows_Click(object sender, EventArgs e)
        {
            if (Positioner != null)
            {
                CopyDataToPositioner();
                Positioner.PositionWindow();
                MakeWindowsVisible();
            }
        }


        private void btnRememberPosition_Click(object sender, EventArgs e)
        {
            if (Positioner != null)
            {
                Positioner.SetToCurrentPosition();
                CopyDataFromPositioner();
            }
        }

        private void btnStickToMaster_Click(object sender, EventArgs e)
        {
            CopyDataToPositioner();
            Positioner.StickToMaster();
        }

        private void btnUnStickFromMaster_Click(object sender, EventArgs e)
        {
            CopyDataToPositioner();
            Positioner.UnStickFromMaster();
        }

        private void txtMasterWeight_Enter(object sender, EventArgs e)
        {
            txtMasterWeight.Select(0, 100);
        }

        private void txtPause_Validated(object sender, EventArgs e)
        {
            double val;
            if (double.TryParse(txtPause.Text, out val))
            {
                if (Positioner != null)
                    Positioner.MovementPauseSeconds = val;
            } else
            {
                val = 0.001;
                if (Positioner != null)
                    val = Positioner.MovementPauseSeconds;
                UtilForms.BlinkControl(txtPause);
                txtPause.Text = val.ToString();
            }
        }

        private void txtPause_Enter(object sender, EventArgs e)
        {
            txtPause.SelectAll();
        }

        private void chkRememberPositions_CheckedChanged(object sender, EventArgs e)
        {
            if (Positioner != null)
            {
                CopyDataToPositioner();
                if (chkRememberPositions.Checked)
                    Positioner.RememberPosition();
                else
                    Positioner.StopRememberPosition();
            }
        }

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            CopyDataFromPositioner();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CopyDataToPositioner();
        }


        #endregion Operation 

        private void WindowPositionerControl_Load(object sender, EventArgs e)
        {
            if (_window == null && _masterWindow == null)
                LaunchWindows();
        }



    } // class WindowPositionTesterControl



}
