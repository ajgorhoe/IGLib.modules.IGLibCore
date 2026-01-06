#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IG.Lib;

namespace IG.Forms
{

    /// <summary>Control for setting horizontal and vertical alignment of items.</summary>
    /// $A Igor Oct09;
    public partial class AlignmentControl : UserControl
    {
        public AlignmentControl()
        {
            InitializeComponent();
        }

        #region Events

        /// <summary>Event that occurs when the value of alignment changes.</summary>
        public event EventHandler ValueChanged;

        /// <summary>Triggers the <see cref="ValueChanged"/> event.</summary>
        protected void OnValueChanged(object sender, EventArgs eventArgs)
        {
            if (ValueChanged != null)
            {
                Alignment alignment = this.Alignment;
                ValueChanged(sender, eventArgs);
            }
        }

        #endregion Events



        /// <summary>Text displayed in the title label of the control.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Title
        {
            get { return pnlOuter.Text; }
            set { pnlOuter.Text = value; }
        }


        protected Alignment _alignment = new Alignment();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Alignment Alignment
        {
            get
            {
                if (rbHorizontalNone.Checked)
                    _alignment.Horizontal = AlignmentHorizontal.None;
                if (rbHorizontalLeft.Checked)
                    _alignment.Horizontal = AlignmentHorizontal.Left;
                if (rbHorizontalCentered.Checked)
                    _alignment.Horizontal = AlignmentHorizontal.Centered;
                if (rbHorizontalRight.Checked)
                    _alignment.Horizontal = AlignmentHorizontal.Right;

                if (rbVerticalNone.Checked)
                    _alignment.Vertical = AlignmentVertical.None;
                if (rbVerticalTop.Checked)
                    _alignment.Vertical = AlignmentVertical.Top;
                if (rbVerticalMiddle.Checked)
                    _alignment.Vertical = AlignmentVertical.Middle;
                if (rbVerticalBottom.Checked)
                    _alignment.Vertical = AlignmentVertical.Bottom;

                return _alignment;
            }
            set
            {
                _alignment = value;
                switch (value.Horizontal)
                {
                    case AlignmentHorizontal.None:
                        rbHorizontalNone.Checked = true;
                        break;
                    case AlignmentHorizontal.Left:
                        rbHorizontalLeft.Checked = true;
                        break;
                    case AlignmentHorizontal.Centered:
                        rbHorizontalCentered.Checked = true;
                        break;
                    case AlignmentHorizontal.Right:
                        rbHorizontalRight.Checked = true;
                        break;
                }
                switch (value.Vertical)
                {
                    case AlignmentVertical.None:
                        rbVerticalNone.Checked = true;
                        break;
                    case AlignmentVertical.Top:
                        rbVerticalTop.Checked = true;
                        break;
                    case AlignmentVertical.Middle:
                        rbVerticalMiddle.Checked = true;
                        break;
                    case AlignmentVertical.Bottom:
                        rbVerticalBottom.Checked = true;
                        break;
                }
            }
        }

        /// <summary>Common things for event handlers executing when any of the radio buttons changes checked status.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnyControl_CheckChanged(object sender, EventArgs e)
        {
            OnValueChanged(sender, e);
        }

        private void rbHorizontalNone_CheckedChanged(object sender, EventArgs e)
        {
            AnyControl_CheckChanged(sender, e);
        }

        private void rbHorizontalLeft_CheckedChanged(object sender, EventArgs e)
        {
            OnValueChanged(sender, e);
        }

        private void rbHorizontalCentered_CheckedChanged(object sender, EventArgs e)
        {
            OnValueChanged(sender, e);
        }

        private void rbHorizontalRight_CheckedChanged(object sender, EventArgs e)
        {
            OnValueChanged(sender, e);
        }

        private void rbVerticalNone_CheckedChanged(object sender, EventArgs e)
        {
            OnValueChanged(sender, e);
        }

        private void rbVerticalTop_CheckedChanged(object sender, EventArgs e)
        {
            OnValueChanged(sender, e);
        }

        private void rbVerticalCentered_CheckedChanged(object sender, EventArgs e)
        {
            OnValueChanged(sender, e);
        }

        private void rbVerticalBottom_CheckedChanged(object sender, EventArgs e)
        {
            OnValueChanged(sender, e);
        }



    }  // class AlignmentControl
}
