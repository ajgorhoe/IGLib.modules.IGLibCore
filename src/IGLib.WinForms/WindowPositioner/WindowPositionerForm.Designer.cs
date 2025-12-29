// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

namespace IG.Forms
{
    partial class WindowPositionerForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserSimpleForm));

            this.windowPositionerControl1 = new IG.Forms.WindowPositionerControl();
            this.SuspendLayout();
            // 
            // windowPositionTester1
            // 
            this.windowPositionerControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.windowPositionerControl1.Location = new System.Drawing.Point(0, 0);
            this.windowPositionerControl1.MinimumSize = new System.Drawing.Size(506, 570);
            this.windowPositionerControl1.Name = "windowPositionTester1";
            this.windowPositionerControl1.Size = new System.Drawing.Size(510, 606);
            this.windowPositionerControl1.TabIndex = 0;
            // 
            // WindowPositionTesterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 605);
            this.Controls.Add(this.windowPositionerControl1);
            this.MinimumSize = new System.Drawing.Size(525, 643);
            this.Icon = IG.Forms.Properties.Resources.ig;
            this.Name = "WindowPositionTesterForm";
            this.Text = "IGLib - Window positioner";
            this.ResumeLayout(false);

        }

        #endregion

        protected WindowPositionerControl windowPositionerControl1 = null;

    }
}