//// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//using IG.Lib;
//using IG.Num;
//using IG.Forms;

//namespace IG.Forms
//{

//    /// <summary>Control for setting window positions, relative positions, sizes, positions, shifts, etc.</summary>
//    /// <remarks><para>This is a dummy control used for various tests such as window positioning.</para></remarks>
//    /// $A Igor Oct09;
//    public partial class WindowShiftControlNumeric : UserControl
//    {
//        public WindowShiftControlNumeric()
//        {
//            InitializeComponent();
//            // this.lblTitle.Text = Title;
//        }


//        #region Events


//        /// <summary>Event that occurs when the value of window position (or shift, size, relative position, etc.) kept by this control changes.</summary>
//        public event EventHandler ValueChanged;

//        /// <summary>Triggers the <see cref="ValueChanged"/> event.</summary>
//        protected void OnValueChanged(object sender, EventArgs eventArgs)
//        {
//            if (ValueChanged!=null)
//            {
//                vec2 val = this.Shift;
//                ValueChanged(sender, eventArgs);
//            }
//        }

//        #endregion Events

//        /// <summary>Title text that is written on the outer group box.</summary>
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
//        public string Title
//        {
//            get { return pnlOuter.Text; }
//            set { pnlOuter.Text = value; }
//        }

//        protected vec2 _shift = new vec2(0.0, 0.0);

//        /// <summary>Gets or sets the resulting shift stored in 2D vector structure of type <see cref="vec2"/></summary>
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
//        public vec2 Shift
//        {
//            get
//            {
//                _shift.x = (double)txtWindowShiftRelX.Value; // double.Parse(txtWindowShiftRelX.Text);
//                _shift.y = (double)txtWindowShiftRelY.Value; // double.Parse(txtWindowShiftRelY.Text);
//                return _shift;
//            }
//            set
//            {
//                _shift = value;
//                txtWindowShiftRelX.Value = (decimal)value.x;  // value.x.ToString();
//                txtWindowShiftRelY.Value = (decimal)value.y;  // value.y.ToString();
//                OnValueChanged(this, null);
//            }
//        }

//        protected double _initialValueX;

//        /// <summary>Initial value of the first component of <see cref="Shift"/>.</summary>
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
//        public double InitialValueX
//        {
//            get
//            {
//                _initialValueX = (double)txtWindowShiftRelX.Value;
//                return _initialValueX;
//            }
//            set
//            {
//                _initialValueX = value;
//                txtWindowShiftRelX.Value = (decimal) value;
//            }
//        }

//        protected double _initialValueY;

//        /// <summary>Initial value of the second component of <see cref="Shift"/>.</summary>
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
//        public double InitialValueY
//        {
//            get
//            {
//                _initialValueY = (double)txtWindowShiftRelY.Value;
//                return _initialValueY;
//            }
//            set
//            {
//                _initialValueY = value;
//                txtWindowShiftRelY.Value = (decimal)value;
//            }
//        }


//        protected double _minValue;

//        /// <summary>Minimal value of components of <see cref="Shift"/>.</summary>
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
//        public double MinimumValue
//        {
//            get
//            {
//                _minValue = M.min(
//                    (double) txtWindowShiftRelX.Minimum, 
//                    (double) txtWindowShiftRelY.Minimum);
//                return _minValue;
//            }
//            set
//            {
//                _minValue = value;
//                txtWindowShiftRelX.Minimum = (decimal) value;
//                txtWindowShiftRelY.Minimum = (decimal) value;
//            }
//        }


//        protected double _maxValue;

//        /// <summary>Maximal value of components of <see cref="Shift"/>.</summary>
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
//        public double MaximumValue
//        {
//            get
//            {
//                _maxValue =  M.max(
//                    (double) txtWindowShiftRelX.Maximum,
//                    (double)txtWindowShiftRelY.Maximum);
//                return _maxValue;
//            }
//            set
//            {
//                _maxValue = value;
//                txtWindowShiftRelX.Maximum = (decimal)value;
//                txtWindowShiftRelY.Maximum = (decimal)value;
//            }
//        }



//        protected double _increment;

//        /// <summary>Increment for text controls for components of <see cref="Shift"/>.</summary>
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
//        public double Increment
//        {
//            get
//            {
//                _increment = M.min(
//                    (double) txtWindowShiftRelX.Increment,
//                    (double)txtWindowShiftRelY.Increment);
//                return _increment;
//            }
//            set
//            {
//                _increment = value;
//                txtWindowShiftRelX.Increment = (decimal)value;
//                txtWindowShiftRelY.Increment = (decimal)value;
//            }
//        }


//        private void txtWindowShiftRelX_Validated(object sender, EventArgs e)
//        {
//            double val;
//            if (double.TryParse(txtWindowShiftRelX.Text, out val))
//            {
//                _shift.x = val;
//                OnValueChanged(this, null);
//            } else
//            {
//                txtWindowShiftRelX.Value = (decimal)_shift.x;
//            }
//        }

//        private void txtWindowShiftRelY_Validated(object sender, EventArgs e)
//        {
//            double val;
//            if (double.TryParse(txtWindowShiftRelY.Text, out val))
//            {
//                _shift.y = val;
//                OnValueChanged(this, null);
//            } else
//            {
//                txtWindowShiftRelY.Value = (decimal)_shift.y;
//            }
//        }

//        private void txtWindowShiftRelX_Enter(object sender, EventArgs e)
//        {
//            txtWindowShiftRelX.Select(0, 100);
//        }

//        private void txtWindowShiftRelY_Enter(object sender, EventArgs e)
//        {
//            txtWindowShiftRelY.Select(0, 100);
//        }



//    } // class WindowShiftControlNumeric

//}
