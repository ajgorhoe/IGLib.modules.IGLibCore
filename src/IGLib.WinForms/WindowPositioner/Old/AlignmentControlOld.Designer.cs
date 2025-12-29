// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

namespace IG.Forms
{
    partial class AlignmentControlOld
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
            this.rbHorizontalNone = new System.Windows.Forms.RadioButton();
            this.pnlHorizontal = new System.Windows.Forms.Panel();
            this.rbHorizontalRight = new System.Windows.Forms.RadioButton();
            this.rbHorizontalCentered = new System.Windows.Forms.RadioButton();
            this.rbHorizontalLeft = new System.Windows.Forms.RadioButton();
            this.pnlVertical = new System.Windows.Forms.Panel();
            this.lblVertical = new System.Windows.Forms.Label();
            this.rbVerticalBottom = new System.Windows.Forms.RadioButton();
            this.rbVerticalCentered = new System.Windows.Forms.RadioButton();
            this.rbVerticalTop = new System.Windows.Forms.RadioButton();
            this.rbVerticalNone = new System.Windows.Forms.RadioButton();
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.lblTitle11 = new System.Windows.Forms.Label();
            this.pnlHorizontal.SuspendLayout();
            this.pnlVertical.SuspendLayout();
            this.pnlOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHorizontal
            // 
            this.lblHorizontal.AutoSize = true;
            this.lblHorizontal.Location = new System.Drawing.Point(3, 0);
            this.lblHorizontal.Name = "lblHorizontal";
            this.lblHorizontal.Size = new System.Drawing.Size(105, 13);
            this.lblHorizontal.TabIndex = 0;
            this.lblHorizontal.Text = "Horizontal alignment:";
            // 
            // rbHorizontalNone
            // 
            this.rbHorizontalNone.AutoSize = true;
            this.rbHorizontalNone.Location = new System.Drawing.Point(6, 16);
            this.rbHorizontalNone.Name = "rbHorizontalNone";
            this.rbHorizontalNone.Size = new System.Drawing.Size(51, 17);
            this.rbHorizontalNone.TabIndex = 1;
            this.rbHorizontalNone.TabStop = true;
            this.rbHorizontalNone.Text = "None";
            this.rbHorizontalNone.UseVisualStyleBackColor = true;
            // 
            // pnlHorizontal
            // 
            this.pnlHorizontal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHorizontal.Controls.Add(this.lblHorizontal);
            this.pnlHorizontal.Controls.Add(this.rbHorizontalRight);
            this.pnlHorizontal.Controls.Add(this.rbHorizontalCentered);
            this.pnlHorizontal.Controls.Add(this.rbHorizontalLeft);
            this.pnlHorizontal.Controls.Add(this.rbHorizontalNone);
            this.pnlHorizontal.Location = new System.Drawing.Point(16, 36);
            this.pnlHorizontal.Name = "pnlHorizontal";
            this.pnlHorizontal.Size = new System.Drawing.Size(125, 114);
            this.pnlHorizontal.TabIndex = 2;
            // 
            // rbHorizontalRight
            // 
            this.rbHorizontalRight.AutoSize = true;
            this.rbHorizontalRight.Location = new System.Drawing.Point(6, 85);
            this.rbHorizontalRight.Name = "rbHorizontalRight";
            this.rbHorizontalRight.Size = new System.Drawing.Size(50, 17);
            this.rbHorizontalRight.TabIndex = 4;
            this.rbHorizontalRight.TabStop = true;
            this.rbHorizontalRight.Text = "Right";
            this.rbHorizontalRight.UseVisualStyleBackColor = true;
            // 
            // rbHorizontalCentered
            // 
            this.rbHorizontalCentered.AutoSize = true;
            this.rbHorizontalCentered.Location = new System.Drawing.Point(6, 62);
            this.rbHorizontalCentered.Name = "rbHorizontalCentered";
            this.rbHorizontalCentered.Size = new System.Drawing.Size(68, 17);
            this.rbHorizontalCentered.TabIndex = 3;
            this.rbHorizontalCentered.TabStop = true;
            this.rbHorizontalCentered.Text = "Centered";
            this.rbHorizontalCentered.UseVisualStyleBackColor = true;
            // 
            // rbHorizontalLeft
            // 
            this.rbHorizontalLeft.AutoSize = true;
            this.rbHorizontalLeft.Location = new System.Drawing.Point(6, 39);
            this.rbHorizontalLeft.Name = "rbHorizontalLeft";
            this.rbHorizontalLeft.Size = new System.Drawing.Size(43, 17);
            this.rbHorizontalLeft.TabIndex = 2;
            this.rbHorizontalLeft.TabStop = true;
            this.rbHorizontalLeft.Text = "Left";
            this.rbHorizontalLeft.UseVisualStyleBackColor = true;
            // 
            // pnlVertical
            // 
            this.pnlVertical.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVertical.Controls.Add(this.lblVertical);
            this.pnlVertical.Controls.Add(this.rbVerticalBottom);
            this.pnlVertical.Controls.Add(this.rbVerticalCentered);
            this.pnlVertical.Controls.Add(this.rbVerticalTop);
            this.pnlVertical.Controls.Add(this.rbVerticalNone);
            this.pnlVertical.Location = new System.Drawing.Point(156, 37);
            this.pnlVertical.Name = "pnlVertical";
            this.pnlVertical.Size = new System.Drawing.Size(125, 114);
            this.pnlVertical.TabIndex = 2;
            // 
            // lblVertical
            // 
            this.lblVertical.AutoSize = true;
            this.lblVertical.Location = new System.Drawing.Point(3, 0);
            this.lblVertical.Name = "lblVertical";
            this.lblVertical.Size = new System.Drawing.Size(93, 13);
            this.lblVertical.TabIndex = 0;
            this.lblVertical.Text = "Vertical alignment:";
            // 
            // rbVerticalBottom
            // 
            this.rbVerticalBottom.AutoSize = true;
            this.rbVerticalBottom.Location = new System.Drawing.Point(6, 85);
            this.rbVerticalBottom.Name = "rbVerticalBottom";
            this.rbVerticalBottom.Size = new System.Drawing.Size(58, 17);
            this.rbVerticalBottom.TabIndex = 8;
            this.rbVerticalBottom.TabStop = true;
            this.rbVerticalBottom.Text = "Bottom";
            this.rbVerticalBottom.UseVisualStyleBackColor = true;
            // 
            // rbVerticalCentered
            // 
            this.rbVerticalCentered.AutoSize = true;
            this.rbVerticalCentered.Location = new System.Drawing.Point(6, 62);
            this.rbVerticalCentered.Name = "rbVerticalCentered";
            this.rbVerticalCentered.Size = new System.Drawing.Size(68, 17);
            this.rbVerticalCentered.TabIndex = 7;
            this.rbVerticalCentered.TabStop = true;
            this.rbVerticalCentered.Text = "Centered";
            this.rbVerticalCentered.UseVisualStyleBackColor = true;
            // 
            // rbVerticalTop
            // 
            this.rbVerticalTop.AutoSize = true;
            this.rbVerticalTop.Location = new System.Drawing.Point(6, 39);
            this.rbVerticalTop.Name = "rbVerticalTop";
            this.rbVerticalTop.Size = new System.Drawing.Size(44, 17);
            this.rbVerticalTop.TabIndex = 6;
            this.rbVerticalTop.TabStop = true;
            this.rbVerticalTop.Text = "Top";
            this.rbVerticalTop.UseVisualStyleBackColor = true;
            // 
            // rbVerticalNone
            // 
            this.rbVerticalNone.AutoSize = true;
            this.rbVerticalNone.Location = new System.Drawing.Point(6, 16);
            this.rbVerticalNone.Name = "rbVerticalNone";
            this.rbVerticalNone.Size = new System.Drawing.Size(51, 17);
            this.rbVerticalNone.TabIndex = 5;
            this.rbVerticalNone.TabStop = true;
            this.rbVerticalNone.Text = "None";
            this.rbVerticalNone.UseVisualStyleBackColor = true;
            // 
            // pnlOuter
            // 
            this.pnlOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOuter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlOuter.Controls.Add(this.lblTitle11);
            this.pnlOuter.Controls.Add(this.pnlVertical);
            this.pnlOuter.Controls.Add(this.pnlHorizontal);
            this.pnlOuter.Location = new System.Drawing.Point(18, 15);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(370, 174);
            this.pnlOuter.TabIndex = 3;
            // 
            // lblTitle11
            // 
            this.lblTitle11.AutoSize = true;
            this.lblTitle11.Location = new System.Drawing.Point(20, 9);
            this.lblTitle11.Name = "lblTitle11";
            this.lblTitle11.Size = new System.Drawing.Size(71, 13);
            this.lblTitle11.TabIndex = 3;
            this.lblTitle11.Text = "Alignment: 11";
            // 
            // AlignmentControlOld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlOuter);
            this.MinimumSize = new System.Drawing.Size(270, 141);
            this.Name = "AlignmentControlOld";
            this.Size = new System.Drawing.Size(403, 201);
            this.pnlHorizontal.ResumeLayout(false);
            this.pnlHorizontal.PerformLayout();
            this.pnlVertical.ResumeLayout(false);
            this.pnlVertical.PerformLayout();
            this.pnlOuter.ResumeLayout(false);
            this.pnlOuter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHorizontal;
        private System.Windows.Forms.RadioButton rbHorizontalNone;
        private System.Windows.Forms.Panel pnlHorizontal;
        private System.Windows.Forms.RadioButton rbHorizontalRight;
        private System.Windows.Forms.RadioButton rbHorizontalCentered;
        private System.Windows.Forms.RadioButton rbHorizontalLeft;
        private System.Windows.Forms.Panel pnlVertical;
        private System.Windows.Forms.Label lblVertical;
        private System.Windows.Forms.RadioButton rbVerticalBottom;
        private System.Windows.Forms.RadioButton rbVerticalCentered;
        private System.Windows.Forms.RadioButton rbVerticalTop;
        private System.Windows.Forms.RadioButton rbVerticalNone;
        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.Label lblTitle11;




    }
}
