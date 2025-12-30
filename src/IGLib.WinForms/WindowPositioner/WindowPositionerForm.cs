// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IG.Lib;

namespace IG.Forms
{

    /// <summary>Windows form for testing window positioning./summary>
    /// $A Igor xx Oct09;
    public partial class WindowPositionerForm : Form,
        IG.Forms.IClosableFromContainedForm
    {

        
        #region Construction

        /// <summary>Constructs a new positioner form with undefined positioner, positioned window and master window.</summary>
        public WindowPositionerForm(): this(null /* positioner */, null /* positionedWindow */, null /* masterWindow */)
        {  }
        
        /// <summary>Constructs a new positioner form with unspecified positioner and specified positioned window and master window.
        /// <para>Positioner is created automatically and initialized to default state.</para></summary>
        /// <param name="positionedWindow">Window that is positioned.</param>
        /// <param name="masterWindow">Master window.</param>
        public WindowPositionerForm(Form positionedWindow, Form masterWindow)
            : this(null /* positioner */, positionedWindow, masterWindow)
        {  }

        /// <summary>Constructs a new positioner form with unspecified positioner and master window, and specified positioned window.
        /// <para>Positioner is created automatically and initialized to default state.</para></summary>
        /// <param name="positionedWindow">Window that is positioned.</param>
        public WindowPositionerForm(Form positionedWindow)
            : this(null /* positioner */, positionedWindow, null /* masterWindow */)
        {  }

        /// <summary>Constructs a new positioner form with the specified positioner and undefined positioned window and master window.</summary>
        /// <param name="positioner">Positioner object that positions the window.</param>
        public WindowPositionerForm(WindowPositioner positioner)
            : this(positioner, null /* positionedWindow */, null /* masterWindow */)
        { }
        
        /// <summary>Constructs a new positioner form with the specified positioner and positioned window, and undefined master window.</summary>
        /// <param name="positioner">Positioner object that positions the window.</param>
        /// <param name="positionedWindow">Window that is positioned.</param>
        public WindowPositionerForm(WindowPositioner positioner, Form positionedWindow)
            : this(positioner, positionedWindow, null /* masterWindow */)
        {  }

        /// <summary>Constructs a new positioner form with the specified positioner, positioned window and master window.</summary>
        /// <param name="positioner">Positioner object that positions the window.</param>
        /// <param name="positionedWindow">Window that is positioned.</param>
        /// <param name="masterWindow">Master window.</param>
        public WindowPositionerForm(WindowPositioner positioner, Form positionedWindow, Form masterWindow)
        {
            InitializeComponent();
            this.Text = Title;
            this.Positioner = positioner;
            this.Window = positionedWindow;
            this.MasterWindow = MasterWindow;
        }

        #endregion Construction



        public string Title
        {
            get { return "IGLib Window Positioner"; }
        }

        /// <summary>Provides direct access to the embedded positioner control.</summary>
        public WindowPositionerControl PositionerControl
        {
            get { return this.windowPositionerControl1; } 
        }


        /// <summary>Window positioner.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public WindowPositioner Positioner
        {
            get
            { return PositionerControl.Positioner;  }
            set { PositionerControl.Positioner = value; }
        }

        /// <summary>Window to be positioned.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Form Window
        {
            get { return PositionerControl.Window;  }
            set { PositionerControl.Window = value; }
        }


        protected Form _masterWindow;

        /// <summary>Master window relative to which position is set.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Form MasterWindow
        {
            get { return PositionerControl.MasterWindow; }
            set { PositionerControl.MasterWindow = value; }
        }
        
    }  // Class WindowPositionTesterForm

}
