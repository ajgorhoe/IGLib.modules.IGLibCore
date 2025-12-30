// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

namespace IG.Forms
{
    partial class WindowShiftControlText
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
            this.pnlOuter = new System.Windows.Forms.GroupBox();
            this.txtWindowShiftRelY = new System.Windows.Forms.TextBox();
            this.txtWindowShiftRelX = new System.Windows.Forms.TextBox();
            this.lblWindowRelY = new System.Windows.Forms.Label();
            this.lblWindowShiftRelX = new System.Windows.Forms.Label();
            this.pnlOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlOuter
            // 
            this.pnlOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOuter.Controls.Add(this.txtWindowShiftRelY);
            this.pnlOuter.Controls.Add(this.txtWindowShiftRelX);
            this.pnlOuter.Controls.Add(this.lblWindowRelY);
            this.pnlOuter.Controls.Add(this.lblWindowShiftRelX);
            this.pnlOuter.Location = new System.Drawing.Point(3, 3);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(124, 71);
            this.pnlOuter.TabIndex = 4;
            this.pnlOuter.TabStop = false;
            this.pnlOuter.Text = "Window relative shift:";
            // 
            // txtWindowShiftRelY
            // 
            this.txtWindowShiftRelY.Location = new System.Drawing.Point(27, 45);
            this.txtWindowShiftRelY.Name = "txtWindowShiftRelY";
            this.txtWindowShiftRelY.Size = new System.Drawing.Size(91, 20);
            this.txtWindowShiftRelY.TabIndex = 2;
            this.txtWindowShiftRelY.Click += new System.EventHandler(this.txtWindowShiftRelY_Enter);
            this.txtWindowShiftRelY.Enter += new System.EventHandler(this.txtWindowShiftRelY_Enter);
            this.txtWindowShiftRelY.Validated += new System.EventHandler(this.txtWindowShiftRelY_Validated);
            // 
            // txtWindowShiftRelX
            // 
            this.txtWindowShiftRelX.Location = new System.Drawing.Point(27, 19);
            this.txtWindowShiftRelX.Name = "txtWindowShiftRelX";
            this.txtWindowShiftRelX.Size = new System.Drawing.Size(91, 20);
            this.txtWindowShiftRelX.TabIndex = 1;
            this.txtWindowShiftRelX.Click += new System.EventHandler(this.txtWindowShiftRelX_Enter);
            this.txtWindowShiftRelX.Enter += new System.EventHandler(this.txtWindowShiftRelX_Enter);
            this.txtWindowShiftRelX.Validated += new System.EventHandler(this.txtWindowShiftRelX_Validated);
            // 
            // lblWindowRelY
            // 
            this.lblWindowRelY.AutoSize = true;
            this.lblWindowRelY.Location = new System.Drawing.Point(6, 48);
            this.lblWindowRelY.Name = "lblWindowRelY";
            this.lblWindowRelY.Size = new System.Drawing.Size(15, 13);
            this.lblWindowRelY.TabIndex = 0;
            this.lblWindowRelY.Text = "y:";
            // 
            // lblWindowShiftRelX
            // 
            this.lblWindowShiftRelX.AutoSize = true;
            this.lblWindowShiftRelX.Location = new System.Drawing.Point(6, 22);
            this.lblWindowShiftRelX.Name = "lblWindowShiftRelX";
            this.lblWindowShiftRelX.Size = new System.Drawing.Size(15, 13);
            this.lblWindowShiftRelX.TabIndex = 0;
            this.lblWindowShiftRelX.Text = "x:";
            // 
            // WindowShiftControlText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlOuter);
            this.MinimumSize = new System.Drawing.Size(132, 80);
            this.Name = "WindowShiftControlText";
            this.Size = new System.Drawing.Size(132, 80);
            this.pnlOuter.ResumeLayout(false);
            this.pnlOuter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox pnlOuter;
        private System.Windows.Forms.Label lblWindowRelY;
        private System.Windows.Forms.Label lblWindowShiftRelX;
        private System.Windows.Forms.TextBox txtWindowShiftRelX;
        private System.Windows.Forms.TextBox txtWindowShiftRelY;




    }
}
