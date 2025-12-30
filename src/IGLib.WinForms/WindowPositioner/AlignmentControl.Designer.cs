// Copyright (c) Igor Grešovnik, IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

namespace IG.Forms
{
    partial class AlignmentControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblHorizontal = new System.Windows.Forms.Label();
            this.rbHorizontalRight = new System.Windows.Forms.RadioButton();
            this.rbHorizontalCentered = new System.Windows.Forms.RadioButton();
            this.rbHorizontalLeft = new System.Windows.Forms.RadioButton();
            this.rbHorizontalNone = new System.Windows.Forms.RadioButton();
            this.lblVertical = new System.Windows.Forms.Label();
            this.rbVerticalBottom = new System.Windows.Forms.RadioButton();
            this.rbVerticalMiddle = new System.Windows.Forms.RadioButton();
            this.rbVerticalTop = new System.Windows.Forms.RadioButton();
            this.rbVerticalNone = new System.Windows.Forms.RadioButton();
            this.pnlOuter = new System.Windows.Forms.GroupBox();
            this.pnlVertical = new System.Windows.Forms.Panel();
            this.pnlHorizontal = new System.Windows.Forms.Panel();
            this.pnlOuter.SuspendLayout();
            this.pnlVertical.SuspendLayout();
            this.pnlHorizontal.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHorizontal
            // 
            this.lblHorizontal.AutoSize = true;
            this.lblHorizontal.Location = new System.Drawing.Point(3, 0);
            this.lblHorizontal.Name = "lblHorizontal";
            this.lblHorizontal.Size = new System.Drawing.Size(57, 13);
            this.lblHorizontal.TabIndex = 5;
            this.lblHorizontal.Text = "Horizontal:";
            // 
            // rbHorizontalRight
            // 
            this.rbHorizontalRight.AutoSize = true;
            this.rbHorizontalRight.Location = new System.Drawing.Point(6, 85);
            this.rbHorizontalRight.Name = "rbHorizontalRight";
            this.rbHorizontalRight.Size = new System.Drawing.Size(50, 17);
            this.rbHorizontalRight.TabIndex = 1;
            this.rbHorizontalRight.TabStop = true;
            this.rbHorizontalRight.Text = "Right";
            this.rbHorizontalRight.UseVisualStyleBackColor = true;
            this.rbHorizontalRight.CheckedChanged += new System.EventHandler(this.rbHorizontalRight_CheckedChanged);
            // 
            // rbHorizontalCentered
            // 
            this.rbHorizontalCentered.AutoSize = true;
            this.rbHorizontalCentered.Location = new System.Drawing.Point(6, 62);
            this.rbHorizontalCentered.Name = "rbHorizontalCentered";
            this.rbHorizontalCentered.Size = new System.Drawing.Size(68, 17);
            this.rbHorizontalCentered.TabIndex = 1;
            this.rbHorizontalCentered.TabStop = true;
            this.rbHorizontalCentered.Text = "Centered";
            this.rbHorizontalCentered.UseVisualStyleBackColor = true;
            this.rbHorizontalCentered.CheckedChanged += new System.EventHandler(this.rbHorizontalCentered_CheckedChanged);
            // 
            // rbHorizontalLeft
            // 
            this.rbHorizontalLeft.AutoSize = true;
            this.rbHorizontalLeft.Location = new System.Drawing.Point(6, 39);
            this.rbHorizontalLeft.Name = "rbHorizontalLeft";
            this.rbHorizontalLeft.Size = new System.Drawing.Size(43, 17);
            this.rbHorizontalLeft.TabIndex = 1;
            this.rbHorizontalLeft.TabStop = true;
            this.rbHorizontalLeft.Text = "Left";
            this.rbHorizontalLeft.UseVisualStyleBackColor = true;
            this.rbHorizontalLeft.CheckedChanged += new System.EventHandler(this.rbHorizontalLeft_CheckedChanged);
            // 
            // rbHorizontalNone
            // 
            this.rbHorizontalNone.AutoSize = true;
            this.rbHorizontalNone.Checked = true;
            this.rbHorizontalNone.Location = new System.Drawing.Point(6, 16);
            this.rbHorizontalNone.Name = "rbHorizontalNone";
            this.rbHorizontalNone.Size = new System.Drawing.Size(51, 17);
            this.rbHorizontalNone.TabIndex = 1;
            this.rbHorizontalNone.TabStop = true;
            this.rbHorizontalNone.Text = "None";
            this.rbHorizontalNone.UseVisualStyleBackColor = true;
            this.rbHorizontalNone.CheckedChanged += new System.EventHandler(this.rbHorizontalNone_CheckedChanged);
            // 
            // lblVertical
            // 
            this.lblVertical.AutoSize = true;
            this.lblVertical.Location = new System.Drawing.Point(3, 0);
            this.lblVertical.Name = "lblVertical";
            this.lblVertical.Size = new System.Drawing.Size(45, 13);
            this.lblVertical.TabIndex = 10;
            this.lblVertical.Text = "Vertical:";
            // 
            // rbVerticalBottom
            // 
            this.rbVerticalBottom.AutoSize = true;
            this.rbVerticalBottom.Location = new System.Drawing.Point(6, 85);
            this.rbVerticalBottom.Name = "rbVerticalBottom";
            this.rbVerticalBottom.Size = new System.Drawing.Size(58, 17);
            this.rbVerticalBottom.TabIndex = 2;
            this.rbVerticalBottom.TabStop = true;
            this.rbVerticalBottom.Text = "Bottom";
            this.rbVerticalBottom.UseVisualStyleBackColor = true;
            this.rbVerticalBottom.CheckedChanged += new System.EventHandler(this.rbVerticalBottom_CheckedChanged);
            // 
            // rbVerticalMiddle
            // 
            this.rbVerticalMiddle.AutoSize = true;
            this.rbVerticalMiddle.Location = new System.Drawing.Point(6, 62);
            this.rbVerticalMiddle.Name = "rbVerticalMiddle";
            this.rbVerticalMiddle.Size = new System.Drawing.Size(56, 17);
            this.rbVerticalMiddle.TabIndex = 2;
            this.rbVerticalMiddle.TabStop = true;
            this.rbVerticalMiddle.Text = "Middle";
            this.rbVerticalMiddle.UseVisualStyleBackColor = true;
            this.rbVerticalMiddle.CheckedChanged += new System.EventHandler(this.rbVerticalCentered_CheckedChanged);
            // 
            // rbVerticalTop
            // 
            this.rbVerticalTop.AutoSize = true;
            this.rbVerticalTop.Location = new System.Drawing.Point(6, 39);
            this.rbVerticalTop.Name = "rbVerticalTop";
            this.rbVerticalTop.Size = new System.Drawing.Size(44, 17);
            this.rbVerticalTop.TabIndex = 2;
            this.rbVerticalTop.TabStop = true;
            this.rbVerticalTop.Text = "Top";
            this.rbVerticalTop.UseVisualStyleBackColor = true;
            this.rbVerticalTop.CheckedChanged += new System.EventHandler(this.rbVerticalTop_CheckedChanged);
            // 
            // rbVerticalNone
            // 
            this.rbVerticalNone.AutoSize = true;
            this.rbVerticalNone.Checked = true;
            this.rbVerticalNone.Location = new System.Drawing.Point(6, 16);
            this.rbVerticalNone.Name = "rbVerticalNone";
            this.rbVerticalNone.Size = new System.Drawing.Size(51, 17);
            this.rbVerticalNone.TabIndex = 2;
            this.rbVerticalNone.TabStop = true;
            this.rbVerticalNone.Text = "None";
            this.rbVerticalNone.UseVisualStyleBackColor = true;
            this.rbVerticalNone.CheckedChanged += new System.EventHandler(this.rbVerticalNone_CheckedChanged);
            // 
            // pnlOuter
            // 
            this.pnlOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOuter.Controls.Add(this.pnlVertical);
            this.pnlOuter.Controls.Add(this.pnlHorizontal);
            this.pnlOuter.Location = new System.Drawing.Point(3, 3);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(178, 134);
            this.pnlOuter.TabIndex = 15;
            this.pnlOuter.TabStop = false;
            this.pnlOuter.Text = "Alignment:";
            // 
            // pnlVertical
            // 
            this.pnlVertical.Controls.Add(this.lblVertical);
            this.pnlVertical.Controls.Add(this.rbVerticalNone);
            this.pnlVertical.Controls.Add(this.rbVerticalBottom);
            this.pnlVertical.Controls.Add(this.rbVerticalTop);
            this.pnlVertical.Controls.Add(this.rbVerticalMiddle);
            this.pnlVertical.Location = new System.Drawing.Point(94, 16);
            this.pnlVertical.Name = "pnlVertical";
            this.pnlVertical.Size = new System.Drawing.Size(78, 113);
            this.pnlVertical.TabIndex = 2;
            // 
            // pnlHorizontal
            // 
            this.pnlHorizontal.Controls.Add(this.lblHorizontal);
            this.pnlHorizontal.Controls.Add(this.rbHorizontalNone);
            this.pnlHorizontal.Controls.Add(this.rbHorizontalLeft);
            this.pnlHorizontal.Controls.Add(this.rbHorizontalCentered);
            this.pnlHorizontal.Controls.Add(this.rbHorizontalRight);
            this.pnlHorizontal.Location = new System.Drawing.Point(3, 16);
            this.pnlHorizontal.Name = "pnlHorizontal";
            this.pnlHorizontal.Size = new System.Drawing.Size(85, 113);
            this.pnlHorizontal.TabIndex = 1;
            // 
            // AlignmentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlOuter);
            this.MinimumSize = new System.Drawing.Size(184, 140);
            this.Name = "AlignmentControl";
            this.Size = new System.Drawing.Size(184, 140);
            this.pnlOuter.ResumeLayout(false);
            this.pnlVertical.ResumeLayout(false);
            this.pnlVertical.PerformLayout();
            this.pnlHorizontal.ResumeLayout(false);
            this.pnlHorizontal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHorizontal;
        private System.Windows.Forms.RadioButton rbHorizontalRight;
        private System.Windows.Forms.RadioButton rbHorizontalCentered;
        private System.Windows.Forms.RadioButton rbHorizontalLeft;
        private System.Windows.Forms.RadioButton rbHorizontalNone;
        private System.Windows.Forms.Label lblVertical;
        private System.Windows.Forms.RadioButton rbVerticalBottom;
        private System.Windows.Forms.RadioButton rbVerticalMiddle;
        private System.Windows.Forms.RadioButton rbVerticalTop;
        private System.Windows.Forms.RadioButton rbVerticalNone;
        private System.Windows.Forms.GroupBox pnlOuter;
        private System.Windows.Forms.Panel pnlHorizontal;
        private System.Windows.Forms.Panel pnlVertical;
    }
}
