// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

namespace IG.Forms
{
    partial class WindowPositionerControl
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.btnPositionWindows = new System.Windows.Forms.Button();
            this.btnLaunchWindows = new System.Windows.Forms.Button();
            this.shiftWindowAbsolute = new IG.Forms.WindowShiftControlText();
            this.shiftWindowRel = new IG.Forms.WindowShiftControlText();
            this.alignWindow = new IG.Forms.AlignmentControl();
            this.lblMasterWeight = new System.Windows.Forms.Label();
            this.txtMasterWeight = new System.Windows.Forms.NumericUpDown();
            this.grpMaster = new System.Windows.Forms.GroupBox();
            this.chkRememberPositions = new System.Windows.Forms.CheckBox();
            this.txtPause = new System.Windows.Forms.TextBox();
            this.lblPause = new System.Windows.Forms.Label();
            this.btnRememberPosition = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnStickToMaster = new System.Windows.Forms.Button();
            this.alignMaster = new IG.Forms.AlignmentControl();
            this.shiftMasterRel = new IG.Forms.WindowShiftControlText();
            this.grpScreen = new System.Windows.Forms.GroupBox();
            this.alignScreen = new IG.Forms.AlignmentControl();
            this.shiftScreenRel = new IG.Forms.WindowShiftControlText();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnRefreshData = new System.Windows.Forms.Button();
            this.grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMasterWeight)).BeginInit();
            this.grpMaster.SuspendLayout();
            this.grpScreen.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Modern No. 20", 20.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Blue;
            this.lblTitle.Location = new System.Drawing.Point(3, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(240, 29);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Window positioner";
            // 
            // grpGeneral
            // 
            this.grpGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGeneral.Controls.Add(this.btnPositionWindows);
            this.grpGeneral.Controls.Add(this.btnLaunchWindows);
            this.grpGeneral.Controls.Add(this.shiftWindowAbsolute);
            this.grpGeneral.Controls.Add(this.shiftWindowRel);
            this.grpGeneral.Controls.Add(this.alignWindow);
            this.grpGeneral.Controls.Add(this.lblMasterWeight);
            this.grpGeneral.Controls.Add(this.txtMasterWeight);
            this.grpGeneral.Location = new System.Drawing.Point(3, 50);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(500, 158);
            this.grpGeneral.TabIndex = 1;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General";
            // 
            // btnPositionWindows
            // 
            this.btnPositionWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPositionWindows.Location = new System.Drawing.Point(352, 129);
            this.btnPositionWindows.Name = "btnPositionWindows";
            this.btnPositionWindows.Size = new System.Drawing.Size(127, 23);
            this.btnPositionWindows.TabIndex = 6;
            this.btnPositionWindows.Text = "Position Windows";
            this.btnPositionWindows.UseVisualStyleBackColor = true;
            this.btnPositionWindows.Click += new System.EventHandler(this.btnPositionWindows_Click);
            // 
            // btnLaunchWindows
            // 
            this.btnLaunchWindows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLaunchWindows.Location = new System.Drawing.Point(352, 98);
            this.btnLaunchWindows.Name = "btnLaunchWindows";
            this.btnLaunchWindows.Size = new System.Drawing.Size(127, 23);
            this.btnLaunchWindows.TabIndex = 5;
            this.btnLaunchWindows.Text = "Launch Windows";
            this.btnLaunchWindows.UseVisualStyleBackColor = true;
            this.btnLaunchWindows.Click += new System.EventHandler(this.btnLaunchWindows_Click);
            // 
            // shiftWindowAbsolute
            // 
            this.shiftWindowAbsolute.Increment = 1D;
            this.shiftWindowAbsolute.InitialValueX = 0D;
            this.shiftWindowAbsolute.InitialValueY = 0D;
            this.shiftWindowAbsolute.Location = new System.Drawing.Point(9, 58);
            this.shiftWindowAbsolute.MaximumValue = 4000D;
            this.shiftWindowAbsolute.MinimumSize = new System.Drawing.Size(132, 80);
            this.shiftWindowAbsolute.MinimumValue = -4000D;
            this.shiftWindowAbsolute.Name = "shiftWindowAbsolute";
            this.shiftWindowAbsolute.Size = new System.Drawing.Size(132, 80);
            this.shiftWindowAbsolute.TabIndex = 2;
            this.shiftWindowAbsolute.Title = "Absolute shift:";
            // 
            // shiftWindowRel
            // 
            this.shiftWindowRel.Increment = 0.05D;
            this.shiftWindowRel.InitialValueX = 0D;
            this.shiftWindowRel.InitialValueY = 0D;
            this.shiftWindowRel.Location = new System.Drawing.Point(337, 12);
            this.shiftWindowRel.MaximumValue = 8D;
            this.shiftWindowRel.MinimumSize = new System.Drawing.Size(132, 80);
            this.shiftWindowRel.MinimumValue = -8D;
            this.shiftWindowRel.Name = "shiftWindowRel";
            this.shiftWindowRel.Size = new System.Drawing.Size(156, 80);
            this.shiftWindowRel.TabIndex = 4;
            this.shiftWindowRel.Title = "Window relative shift:";
            // 
            // alignWindow
            // 
            this.alignWindow.Location = new System.Drawing.Point(147, 12);
            this.alignWindow.MinimumSize = new System.Drawing.Size(184, 140);
            this.alignWindow.Name = "alignWindow";
            this.alignWindow.Size = new System.Drawing.Size(184, 140);
            this.alignWindow.TabIndex = 3;
            this.alignWindow.Title = "Positioned window alignment:";
            // 
            // lblMasterWeight
            // 
            this.lblMasterWeight.AutoSize = true;
            this.lblMasterWeight.Location = new System.Drawing.Point(6, 16);
            this.lblMasterWeight.Name = "lblMasterWeight";
            this.lblMasterWeight.Size = new System.Drawing.Size(115, 13);
            this.lblMasterWeight.TabIndex = 1;
            this.lblMasterWeight.Text = "Master window weight:";
            // 
            // txtMasterWeight
            // 
            this.txtMasterWeight.DecimalPlaces = 2;
            this.txtMasterWeight.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.txtMasterWeight.Location = new System.Drawing.Point(9, 32);
            this.txtMasterWeight.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMasterWeight.Name = "txtMasterWeight";
            this.txtMasterWeight.Size = new System.Drawing.Size(70, 20);
            this.txtMasterWeight.TabIndex = 1;
            this.txtMasterWeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMasterWeight.Click += new System.EventHandler(this.txtMasterWeight_Enter);
            this.txtMasterWeight.Enter += new System.EventHandler(this.txtMasterWeight_Enter);
            // 
            // grpMaster
            // 
            this.grpMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMaster.Controls.Add(this.chkRememberPositions);
            this.grpMaster.Controls.Add(this.txtPause);
            this.grpMaster.Controls.Add(this.lblPause);
            this.grpMaster.Controls.Add(this.btnRememberPosition);
            this.grpMaster.Controls.Add(this.button1);
            this.grpMaster.Controls.Add(this.btnStickToMaster);
            this.grpMaster.Controls.Add(this.alignMaster);
            this.grpMaster.Controls.Add(this.shiftMasterRel);
            this.grpMaster.Location = new System.Drawing.Point(3, 214);
            this.grpMaster.Name = "grpMaster";
            this.grpMaster.Size = new System.Drawing.Size(500, 169);
            this.grpMaster.TabIndex = 1;
            this.grpMaster.TabStop = false;
            this.grpMaster.Text = "Master Window";
            // 
            // chkRememberPositions
            // 
            this.chkRememberPositions.AutoSize = true;
            this.chkRememberPositions.Location = new System.Drawing.Point(352, 146);
            this.chkRememberPositions.Name = "chkRememberPositions";
            this.chkRememberPositions.Size = new System.Drawing.Size(140, 17);
            this.chkRememberPositions.TabIndex = 11;
            this.chkRememberPositions.Text = "Remember Continuously";
            this.chkRememberPositions.UseVisualStyleBackColor = true;
            this.chkRememberPositions.CheckedChanged += new System.EventHandler(this.chkRememberPositions_CheckedChanged);
            // 
            // txtPause
            // 
            this.txtPause.Location = new System.Drawing.Point(352, 121);
            this.txtPause.Name = "txtPause";
            this.txtPause.Size = new System.Drawing.Size(103, 20);
            this.txtPause.TabIndex = 10;
            this.txtPause.Text = "0.001";
            this.txtPause.Click += new System.EventHandler(this.txtPause_Enter);
            this.txtPause.Enter += new System.EventHandler(this.txtPause_Enter);
            this.txtPause.Validated += new System.EventHandler(this.txtPause_Validated);
            // 
            // lblPause
            // 
            this.lblPause.AutoSize = true;
            this.lblPause.Location = new System.Drawing.Point(349, 105);
            this.lblPause.Name = "lblPause";
            this.lblPause.Size = new System.Drawing.Size(106, 13);
            this.lblPause.TabIndex = 9;
            this.lblPause.Text = "Movement pause (s):";
            // 
            // btnRememberPosition
            // 
            this.btnRememberPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRememberPosition.Location = new System.Drawing.Point(352, 19);
            this.btnRememberPosition.Name = "btnRememberPosition";
            this.btnRememberPosition.Size = new System.Drawing.Size(127, 23);
            this.btnRememberPosition.TabIndex = 6;
            this.btnRememberPosition.Text = "Remember Position";
            this.btnRememberPosition.UseVisualStyleBackColor = true;
            this.btnRememberPosition.Click += new System.EventHandler(this.btnRememberPosition_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(352, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Unstick From Master";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnUnStickFromMaster_Click);
            // 
            // btnStickToMaster
            // 
            this.btnStickToMaster.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStickToMaster.Location = new System.Drawing.Point(352, 48);
            this.btnStickToMaster.Name = "btnStickToMaster";
            this.btnStickToMaster.Size = new System.Drawing.Size(127, 23);
            this.btnStickToMaster.TabIndex = 6;
            this.btnStickToMaster.Text = "Stick to Master";
            this.btnStickToMaster.UseVisualStyleBackColor = true;
            this.btnStickToMaster.Click += new System.EventHandler(this.btnStickToMaster_Click);
            // 
            // alignMaster
            // 
            this.alignMaster.Location = new System.Drawing.Point(162, 19);
            this.alignMaster.MinimumSize = new System.Drawing.Size(184, 140);
            this.alignMaster.Name = "alignMaster";
            this.alignMaster.Size = new System.Drawing.Size(184, 140);
            this.alignMaster.TabIndex = 8;
            this.alignMaster.Title = "Alignment w.r. master:";
            // 
            // shiftMasterRel
            // 
            this.shiftMasterRel.Increment = 0.05D;
            this.shiftMasterRel.InitialValueX = 0D;
            this.shiftMasterRel.InitialValueY = 0D;
            this.shiftMasterRel.Location = new System.Drawing.Point(9, 19);
            this.shiftMasterRel.MaximumValue = 8D;
            this.shiftMasterRel.MinimumSize = new System.Drawing.Size(132, 80);
            this.shiftMasterRel.MinimumValue = -8D;
            this.shiftMasterRel.Name = "shiftMasterRel";
            this.shiftMasterRel.Size = new System.Drawing.Size(147, 80);
            this.shiftMasterRel.TabIndex = 7;
            this.shiftMasterRel.Title = "Shift relative to master:";
            // 
            // grpScreen
            // 
            this.grpScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpScreen.Controls.Add(this.alignScreen);
            this.grpScreen.Controls.Add(this.shiftScreenRel);
            this.grpScreen.Location = new System.Drawing.Point(3, 389);
            this.grpScreen.Name = "grpScreen";
            this.grpScreen.Size = new System.Drawing.Size(500, 161);
            this.grpScreen.TabIndex = 1;
            this.grpScreen.TabStop = false;
            this.grpScreen.Text = "Screen";
            // 
            // alignScreen
            // 
            this.alignScreen.Location = new System.Drawing.Point(162, 19);
            this.alignScreen.MinimumSize = new System.Drawing.Size(184, 140);
            this.alignScreen.Name = "alignScreen";
            this.alignScreen.Size = new System.Drawing.Size(184, 140);
            this.alignScreen.TabIndex = 10;
            this.alignScreen.Title = "Alignment w.r. screen:";
            // 
            // shiftScreenRel
            // 
            this.shiftScreenRel.Increment = 0.05D;
            this.shiftScreenRel.InitialValueX = 0D;
            this.shiftScreenRel.InitialValueY = 0D;
            this.shiftScreenRel.Location = new System.Drawing.Point(9, 19);
            this.shiftScreenRel.MaximumValue = 8D;
            this.shiftScreenRel.MinimumSize = new System.Drawing.Size(132, 80);
            this.shiftScreenRel.MinimumValue = -8D;
            this.shiftScreenRel.Name = "shiftScreenRel";
            this.shiftScreenRel.Size = new System.Drawing.Size(147, 80);
            this.shiftScreenRel.TabIndex = 9;
            this.shiftScreenRel.Title = "Shift relative to screen:";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(355, 556);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(127, 23);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "Update Positioner";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshData.Location = new System.Drawing.Point(222, 556);
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(127, 23);
            this.btnRefreshData.TabIndex = 11;
            this.btnRefreshData.Text = "Refresh Data";
            this.btnRefreshData.UseVisualStyleBackColor = true;
            this.btnRefreshData.Click += new System.EventHandler(this.btnRefreshData_Click);
            // 
            // WindowPositionerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRefreshData);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.grpScreen);
            this.Controls.Add(this.grpMaster);
            this.Controls.Add(this.grpGeneral);
            this.Controls.Add(this.lblTitle);
            this.MinimumSize = new System.Drawing.Size(506, 582);
            this.Name = "WindowPositionerControl";
            this.Size = new System.Drawing.Size(506, 582);
            this.Load += new System.EventHandler(this.WindowPositionerControl_Load);
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMasterWeight)).EndInit();
            this.grpMaster.ResumeLayout(false);
            this.grpMaster.PerformLayout();
            this.grpScreen.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.GroupBox grpMaster;
        private System.Windows.Forms.GroupBox grpScreen;
        private System.Windows.Forms.Label lblMasterWeight;
        private System.Windows.Forms.NumericUpDown txtMasterWeight;
        private IG.Forms.AlignmentControl alignWindow;
        private AlignmentControl alignMaster;
        private AlignmentControl alignScreen;
        private WindowShiftControlText shiftWindowRel;
        private WindowShiftControlText shiftWindowAbsolute;
        private WindowShiftControlText shiftMasterRel;
        private WindowShiftControlText shiftScreenRel;
        private System.Windows.Forms.Button btnLaunchWindows;
        private System.Windows.Forms.Button btnPositionWindows;
        private System.Windows.Forms.Button btnStickToMaster;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtPause;
        private System.Windows.Forms.Label lblPause;
        private System.Windows.Forms.Button btnRememberPosition;
        private System.Windows.Forms.CheckBox chkRememberPositions;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnRefreshData;
        //private WindowShiftControlRelative windowShiftControlRelative1;
        // private AlignmentControl alignmentControlindow;

    }  // class WindowPositionTesterControl

}
