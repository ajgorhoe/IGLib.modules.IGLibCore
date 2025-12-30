namespace IG.Forms
{
    partial class FadingMessage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FadingMessage));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.BodyPnl = new System.Windows.Forms.Panel();
            this.lblMEssage = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.PnlStatus = new System.Windows.Forms.Panel();
            this.labelBottom = new System.Windows.Forms.Label();
            this.menuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuCopyMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLaunchThreadParallel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLaunchSameThread = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExample = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExampleMassive = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIdentifyThread = new System.Windows.Forms.ToolStripMenuItem();
            this.BodyPnl.SuspendLayout();
            this.PnlStatus.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Location = new System.Drawing.Point(299, 1);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 26);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(3, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(140, 15);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "< Ctrl-right-click to close >";
            // 
            // BodyPnl
            // 
            this.BodyPnl.AutoSize = true;
            this.BodyPnl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BodyPnl.Controls.Add(this.lblMEssage);
            this.BodyPnl.Controls.Add(this.lblTitle);
            this.BodyPnl.Location = new System.Drawing.Point(8, 11);
            this.BodyPnl.Name = "BodyPnl";
            this.BodyPnl.Size = new System.Drawing.Size(349, 153);
            this.BodyPnl.TabIndex = 4;
            // 
            // lblMEssage
            // 
            this.lblMEssage.AutoSize = true;
            this.lblMEssage.Font = new System.Drawing.Font("Times New Roman", 10.5F);
            this.lblMEssage.Location = new System.Drawing.Point(0, 67);
            this.lblMEssage.Margin = new System.Windows.Forms.Padding(0);
            this.lblMEssage.MinimumSize = new System.Drawing.Size(100, 40);
            this.lblMEssage.Name = "lblMEssage";
            this.lblMEssage.Padding = new System.Windows.Forms.Padding(3);
            this.lblMEssage.Size = new System.Drawing.Size(349, 86);
            this.lblMEssage.TabIndex = 0;
            this.lblMEssage.Text = resources.GetString("lblMEssage.Text");
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Blue;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(5);
            this.lblTitle.Size = new System.Drawing.Size(272, 67);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Fading Message Title.\r\nCan be mutiline. Top position of label\r\nis adjusted to tit" +
    "le height.";
            // 
            // PnlStatus
            // 
            this.PnlStatus.AutoSize = true;
            this.PnlStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PnlStatus.Controls.Add(this.btnCancel);
            this.PnlStatus.Controls.Add(this.lblStatus);
            this.PnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PnlStatus.Location = new System.Drawing.Point(8, 196);
            this.PnlStatus.Name = "PnlStatus";
            this.PnlStatus.Size = new System.Drawing.Size(363, 31);
            this.PnlStatus.TabIndex = 5;
            // 
            // labelBottom
            // 
            this.labelBottom.AutoSize = true;
            this.labelBottom.Location = new System.Drawing.Point(10, 166);
            this.labelBottom.Name = "labelBottom";
            this.labelBottom.Size = new System.Drawing.Size(177, 13);
            this.labelBottom.TabIndex = 6;
            this.labelBottom.Text = "Label for forcing lower window edge";
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopyMessage,
            this.menuClose,
            this.menuLaunchThreadParallel,
            this.menuIdentifyThread});
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(269, 92);
            this.menuMain.Opening += new System.ComponentModel.CancelEventHandler(this.menuMain_Opening);
            // 
            // menuCopyMessage
            // 
            this.menuCopyMessage.Name = "menuCopyMessage";
            this.menuCopyMessage.Size = new System.Drawing.Size(268, 22);
            this.menuCopyMessage.Text = "Copy Message Text (Ctrl - C)";
            this.menuCopyMessage.Click += new System.EventHandler(this.menuCopyMessage_Click);
            // 
            // menuClose
            // 
            this.menuClose.Name = "menuClose";
            this.menuClose.Size = new System.Drawing.Size(268, 22);
            this.menuClose.Text = "Close this Message (Ctrl-Right-Click)";
            this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // menuLaunchThreadParallel
            // 
            this.menuLaunchThreadParallel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLaunchSameThread,
            this.menuExample,
            this.menuExampleMassive});
            this.menuLaunchThreadParallel.Name = "menuLaunchThreadParallel";
            this.menuLaunchThreadParallel.Size = new System.Drawing.Size(268, 22);
            this.menuLaunchThreadParallel.Text = "Launch Test Message";
            this.menuLaunchThreadParallel.Click += new System.EventHandler(this.menuLaunchMessageParallel_Click);
            // 
            // menuLaunchSameThread
            // 
            this.menuLaunchSameThread.Name = "menuLaunchSameThread";
            this.menuLaunchSameThread.Size = new System.Drawing.Size(218, 22);
            this.menuLaunchSameThread.Text = "Launch In the Same Thread";
            this.menuLaunchSameThread.Click += new System.EventHandler(this.menuLaunchSameThread_Click);
            // 
            // menuExample
            // 
            this.menuExample.Name = "menuExample";
            this.menuExample.Size = new System.Drawing.Size(218, 22);
            this.menuExample.Text = "Demo (Example) Launch";
            this.menuExample.Click += new System.EventHandler(this.menuExample_Click);
            // 
            // menuExampleMassive
            // 
            this.menuExampleMassive.Name = "menuExampleMassive";
            this.menuExampleMassive.Size = new System.Drawing.Size(218, 22);
            this.menuExampleMassive.Text = "Massive Launch Demo";
            this.menuExampleMassive.Click += new System.EventHandler(this.menuExampleMassive_Click);
            // 
            // menuIdentifyThread
            // 
            this.menuIdentifyThread.Name = "menuIdentifyThread";
            this.menuIdentifyThread.Size = new System.Drawing.Size(268, 22);
            this.menuIdentifyThread.Text = "Identify Message";
            this.menuIdentifyThread.Click += new System.EventHandler(this.menuIdentifyThread_Click);
            // 
            // FadingMessage
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(379, 235);
            this.ControlBox = false;
            this.Controls.Add(this.BodyPnl);
            this.Controls.Add(this.PnlStatus);
            this.Controls.Add(this.labelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(40, 20);
            this.Name = "FadingMessage";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FadeMessage_FormClosing);
            this.Load += new System.EventHandler(this.FadeMessage_Load);
            this.Disposed += new System.EventHandler(this.FadeMessage_Disposed);
            this.BodyPnl.ResumeLayout(false);
            this.BodyPnl.PerformLayout();
            this.PnlStatus.ResumeLayout(false);
            this.PnlStatus.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion




        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel BodyPnl;
        private System.Windows.Forms.Label lblMEssage;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel PnlStatus;
        private System.Windows.Forms.Label labelBottom;
        private System.Windows.Forms.ContextMenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuCopyMessage;
        private System.Windows.Forms.ToolStripMenuItem menuClose;
        private System.Windows.Forms.ToolStripMenuItem menuLaunchThreadParallel;
        private System.Windows.Forms.ToolStripMenuItem menuIdentifyThread;
        private System.Windows.Forms.ToolStripMenuItem menuLaunchSameThread;
        private System.Windows.Forms.ToolStripMenuItem menuExample;
        private System.Windows.Forms.ToolStripMenuItem menuExampleMassive;
    }
}
