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
//using IG.Forms;

//namespace IG.Forms
//{

//    /// <summary>Control for adjusting alignment.
//    /// <para>Alignment is get or set by the <see cref="Alignment"/> property.</para></summary>
//    /// <remarks><para>This is a dummy control used for various tests such as window positioning.</para></remarks>
//    /// $A Igor Oct09;
//    [Obsolete("Use AlignmentControl instesd.")]
//    public partial class AlignmentControlOld : UserControl
//    {
//        public AlignmentControlOld()
//        {
//            InitializeComponent();
//            Alignment = new Alignment();
//        }


//        /// <summary>Text displayed in the title label of the control.</summary>
//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
//        public string Title
//        {
//            get { return lblTitle11.Text; }
//            set { lblTitle11.Text = value; }
//        }

//        protected Alignment _alignment = new Alignment();

//        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
//        public Alignment Alignment
//        {
//            get {
//                if (rbHorizontalNone.Checked)
//                    _alignment.Horizontal = AlignmentHorizontal.None;
//                if (rbHorizontalLeft.Checked)
//                    _alignment.Horizontal = AlignmentHorizontal.Left;
//                if (rbHorizontalCentered.Checked)
//                    _alignment.Horizontal = AlignmentHorizontal.Centered;
//                if (rbHorizontalRight.Checked)
//                    _alignment.Horizontal = AlignmentHorizontal.Right;

//                if (rbVerticalNone.Checked)
//                    _alignment.Vertical = AlignmentVertical.None;
//                if (rbVerticalTop.Checked)
//                    _alignment.Vertical = AlignmentVertical.Top;
//                if (rbVerticalCentered.Checked)
//                    _alignment.Vertical = AlignmentVertical.Middle;
//                if (rbVerticalBottom.Checked)
//                    _alignment.Vertical = AlignmentVertical.Bottom;

//                return _alignment;
//            }
//            set
//            {
//                Alignment = value;
//                switch (value.Horizontal)
//                {
//                    case AlignmentHorizontal.None:
//                        rbHorizontalNone.Checked = true;
//                        break;
//                    case AlignmentHorizontal.Left:
//                        rbHorizontalLeft.Checked = true;
//                        break;
//                    case AlignmentHorizontal.Centered:
//                        rbHorizontalCentered.Checked = true;
//                        break;
//                    case AlignmentHorizontal.Right:
//                        rbHorizontalRight.Checked = true;
//                        break;
//                }
//                switch (value.Vertical)
//                {
//                    case AlignmentVertical.None:
//                        rbVerticalNone.Checked = true;
//                        break;
//                    case AlignmentVertical.Top:
//                        rbVerticalTop.Checked = true;
//                        break;
//                    case AlignmentVertical.Middle:
//                        rbVerticalCentered.Checked = true;
//                        break;
//                    case AlignmentVertical.Bottom:
//                        rbVerticalBottom.Checked = true;
//                        break;
//                }
//            }
//        }



//    } // class TestControl

//}
