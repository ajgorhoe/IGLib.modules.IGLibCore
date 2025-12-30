namespace IG.Forms
{
    partial class BrowserSimpleControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserSimpleControl));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusType = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusPath = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusError = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFile_OpenURL = new System.Windows.Forms.ToolStripMenuItem();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.googleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFile_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuFile_CloseDocument = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFile_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTools_Controls = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuTools_Stop = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuGelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelp_About = new System.Windows.Forms.ToolStripMenuItem();
            this.browser = new System.Windows.Forms.WebBrowser();
            this.ControlPnl = new System.Windows.Forms.Panel();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonForward = new System.Windows.Forms.Button();
            this.TopPnlHideBtn = new System.Windows.Forms.Button();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.txtAddressBar = new System.Windows.Forms.TextBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonHome = new System.Windows.Forms.Button();
            this.pnlOuter = new System.Windows.Forms.Panel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.ControlPnl.SuspendLayout();
            this.pnlOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusType,
            this.StatusPath,
            this.StatusError,
            this.StatusStatus});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 478);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(700, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "<<< Initialized >>>";
            // 
            // StatusType
            // 
            this.StatusType.Name = "StatusType";
            this.StatusType.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusType.Size = new System.Drawing.Size(79, 17);
            this.StatusType.Text = "<< Type >>";
            this.StatusType.ToolTipText = "This shows the type of the selected XML node in the tree view.";
            // 
            // StatusPath
            // 
            this.StatusPath.Name = "StatusPath";
            this.StatusPath.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusPath.Size = new System.Drawing.Size(77, 17);
            this.StatusPath.Text = "<< Path >>";
            this.StatusPath.ToolTipText = "This shows the path of the last selected XML node in the tree view.";
            // 
            // StatusError
            // 
            this.StatusError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.StatusError.Name = "StatusError";
            this.StatusError.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusError.Size = new System.Drawing.Size(34, 17);
            this.StatusError.Text = " OK";
            this.StatusError.ToolTipText = "This shows eventual error and warning messages.";
            // 
            // StatusStatus
            // 
            this.StatusStatus.ForeColor = System.Drawing.Color.Blue;
            this.StatusStatus.Name = "StatusStatus";
            this.StatusStatus.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusStatus.Size = new System.Drawing.Size(68, 17);
            this.StatusStatus.Text = "Initialized.";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.MenuTools,
            this.MenuGelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(700, 24);
            this.menuStrip.TabIndex = 3;
            this.menuStrip.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile_OpenURL,
            this.MenuFile_Open,
            this.toolStripSeparator1,
            this.printPreviewToolStripMenuItem,
            this.printToolStripMenuItem,
            this.toolStripSeparator2,
            this.MenuFile_CloseDocument,
            this.MenuFile_Close});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(37, 20);
            this.MenuFile.Text = "&File";
            this.MenuFile.ToolTipText = "Closes the current document, but not the form.";
            // 
            // MenuFile_OpenURL
            // 
            this.MenuFile_OpenURL.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeToolStripMenuItem,
            this.googleToolStripMenuItem,
            this.otherToolStripMenuItem});
            this.MenuFile_OpenURL.Name = "MenuFile_OpenURL";
            this.MenuFile_OpenURL.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.MenuFile_OpenURL.Size = new System.Drawing.Size(216, 22);
            this.MenuFile_OpenURL.Text = "Open &Location";
            this.MenuFile_OpenURL.ToolTipText = "Loads an XML document from a file.";
            this.MenuFile_OpenURL.Click += new System.EventHandler(this.MenuFile_OpenURL_Click);
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.homeToolStripMenuItem.Text = "&Home";
            this.homeToolStripMenuItem.Click += new System.EventHandler(this.homeToolStripMenuItem_Click);
            // 
            // googleToolStripMenuItem
            // 
            this.googleToolStripMenuItem.Name = "googleToolStripMenuItem";
            this.googleToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.googleToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.googleToolStripMenuItem.Text = "&Search";
            this.googleToolStripMenuItem.Click += new System.EventHandler(this.googleToolStripMenuItem_Click);
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.otherToolStripMenuItem.Text = "&Other...";
            this.otherToolStripMenuItem.Click += new System.EventHandler(this.otherToolStripMenuItem_Click);
            // 
            // MenuFile_Open
            // 
            this.MenuFile_Open.Name = "MenuFile_Open";
            this.MenuFile_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MenuFile_Open.Size = new System.Drawing.Size(216, 22);
            this.MenuFile_Open.Text = "&Open File";
            this.MenuFile_Open.ToolTipText = "Loads an XML document from a file.";
            this.MenuFile_Open.Click += new System.EventHandler(this.MenuFile_Open_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(213, 6);
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
            this.printPreviewToolStripMenuItem.Click += new System.EventHandler(this.printPreviewToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.printToolStripMenuItem.Text = "&Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(213, 6);
            // 
            // MenuFile_CloseDocument
            // 
            this.MenuFile_CloseDocument.Name = "MenuFile_CloseDocument";
            this.MenuFile_CloseDocument.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.MenuFile_CloseDocument.Size = new System.Drawing.Size(216, 22);
            this.MenuFile_CloseDocument.Text = "&Close Document";
            this.MenuFile_CloseDocument.Click += new System.EventHandler(this.MenuFile_CloseDocument_Click);
            // 
            // MenuFile_Close
            // 
            this.MenuFile_Close.Name = "MenuFile_Close";
            this.MenuFile_Close.Size = new System.Drawing.Size(216, 22);
            this.MenuFile_Close.Text = "Hide Window";
            this.MenuFile_Close.ToolTipText = "Closes the XML Viewer.";
            this.MenuFile_Close.Click += new System.EventHandler(this.MenuFile_Close_Click);
            // 
            // MenuTools
            // 
            this.MenuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuTools_Controls,
            this.toolStripSeparator5,
            this.MenuTools_Stop});
            this.MenuTools.Name = "MenuTools";
            this.MenuTools.Size = new System.Drawing.Size(48, 20);
            this.MenuTools.Text = "T&ools";
            // 
            // MenuTools_Controls
            // 
            this.MenuTools_Controls.Checked = true;
            this.MenuTools_Controls.CheckOnClick = true;
            this.MenuTools_Controls.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuTools_Controls.Name = "MenuTools_Controls";
            this.MenuTools_Controls.Size = new System.Drawing.Size(213, 22);
            this.MenuTools_Controls.Text = "Top &Control Panel";
            this.MenuTools_Controls.ToolTipText = resources.GetString("MenuTools_Controls.ToolTipText");
            this.MenuTools_Controls.CheckedChanged += new System.EventHandler(this.MenuTools_Controls_CheckStateChanged);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(210, 6);
            // 
            // MenuTools_Stop
            // 
            this.MenuTools_Stop.Name = "MenuTools_Stop";
            this.MenuTools_Stop.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MenuTools_Stop.Size = new System.Drawing.Size(213, 22);
            this.MenuTools_Stop.Text = "Stop Loading Page";
            // 
            // MenuGelp
            // 
            this.MenuGelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuHelp_About});
            this.MenuGelp.Name = "MenuGelp";
            this.MenuGelp.Size = new System.Drawing.Size(44, 20);
            this.MenuGelp.Text = "&Help";
            // 
            // MenuHelp_About
            // 
            this.MenuHelp_About.Name = "MenuHelp_About";
            this.MenuHelp_About.Size = new System.Drawing.Size(170, 22);
            this.MenuHelp_About.Text = "About this control";
            this.MenuHelp_About.Click += new System.EventHandler(this.MenuHelp_About_Click);
            // 
            // browser
            // 
            this.browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browser.Location = new System.Drawing.Point(0, 59);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(694, 389);
            this.browser.TabIndex = 2;
            this.browser.TabStop = false;
            this.browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            this.browser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
            // 
            // ControlPnl
            // 
            this.ControlPnl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ControlPnl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ControlPnl.Controls.Add(this.buttonBack);
            this.ControlPnl.Controls.Add(this.buttonForward);
            this.ControlPnl.Controls.Add(this.TopPnlHideBtn);
            this.ControlPnl.Controls.Add(this.buttonSubmit);
            this.ControlPnl.Controls.Add(this.txtAddressBar);
            this.ControlPnl.Controls.Add(this.buttonRefresh);
            this.ControlPnl.Controls.Add(this.buttonStop);
            this.ControlPnl.Controls.Add(this.buttonHome);
            this.ControlPnl.Location = new System.Drawing.Point(0, 0);
            this.ControlPnl.Name = "ControlPnl";
            this.ControlPnl.Size = new System.Drawing.Size(694, 62);
            this.ControlPnl.TabIndex = 14;
            // 
            // buttonBack
            // 
            this.buttonBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonBack.Location = new System.Drawing.Point(12, 4);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(42, 23);
            this.buttonBack.TabIndex = 4;
            this.buttonBack.Text = "<=";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonForward
            // 
            this.buttonForward.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonForward.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.buttonForward.Location = new System.Drawing.Point(60, 3);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(42, 23);
            this.buttonForward.TabIndex = 5;
            this.buttonForward.Text = "=>";
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.Click += new System.EventHandler(this.buttonForward_Click);
            // 
            // TopPnlHideBtn
            // 
            this.TopPnlHideBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TopPnlHideBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TopPnlHideBtn.ForeColor = System.Drawing.Color.Crimson;
            this.TopPnlHideBtn.Location = new System.Drawing.Point(665, 4);
            this.TopPnlHideBtn.Name = "TopPnlHideBtn";
            this.TopPnlHideBtn.Size = new System.Drawing.Size(23, 23);
            this.TopPnlHideBtn.TabIndex = 9;
            this.TopPnlHideBtn.Text = "x";
            this.TopPnlHideBtn.UseVisualStyleBackColor = true;
            this.TopPnlHideBtn.Click += new System.EventHandler(this.TopPnlHideBtn_Click);
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSubmit.Location = new System.Drawing.Point(634, 30);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(54, 29);
            this.buttonSubmit.TabIndex = 1;
            this.buttonSubmit.Text = "Submit";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // txtAddressBar
            // 
            this.txtAddressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddressBar.Location = new System.Drawing.Point(12, 33);
            this.txtAddressBar.Name = "txtAddressBar";
            this.txtAddressBar.Size = new System.Drawing.Size(616, 20);
            this.txtAddressBar.TabIndex = 0;
            this.txtAddressBar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAddressBar_KeyPress);
            this.txtAddressBar.Leave += new System.EventHandler(this.txtAddressBar_Leave);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.ForeColor = System.Drawing.Color.Purple;
            this.buttonRefresh.Location = new System.Drawing.Point(224, 4);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 8;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonStop.ForeColor = System.Drawing.Color.Red;
            this.buttonStop.Location = new System.Drawing.Point(108, 4);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(35, 23);
            this.buttonStop.TabIndex = 6;
            this.buttonStop.Text = "X";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonHome
            // 
            this.buttonHome.ForeColor = System.Drawing.Color.Purple;
            this.buttonHome.Location = new System.Drawing.Point(163, 4);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(55, 23);
            this.buttonHome.TabIndex = 7;
            this.buttonHome.Text = "Home";
            this.buttonHome.UseVisualStyleBackColor = true;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // pnlOuter
            // 
            this.pnlOuter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOuter.Controls.Add(this.browser);
            this.pnlOuter.Controls.Add(this.ControlPnl);
            this.pnlOuter.Location = new System.Drawing.Point(3, 27);
            this.pnlOuter.Name = "pnlOuter";
            this.pnlOuter.Size = new System.Drawing.Size(694, 448);
            this.pnlOuter.TabIndex = 15;
            // 
            // BrowserSimpleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlOuter);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip);
            this.Name = "BrowserSimpleControl";
            this.Size = new System.Drawing.Size(700, 500);
            this.VisibleChanged += new System.EventHandler(this.BrowserSimpleControl_VisibleChanged);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ControlPnl.ResumeLayout(false);
            this.ControlPnl.PerformLayout();
            this.pnlOuter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusType;
        private System.Windows.Forms.ToolStripStatusLabel StatusPath;
        private System.Windows.Forms.ToolStripStatusLabel StatusError;
        private System.Windows.Forms.ToolStripStatusLabel StatusStatus;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuFile_OpenURL;
        private System.Windows.Forms.ToolStripMenuItem MenuFile_Open;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuFile_Close;
        private System.Windows.Forms.ToolStripMenuItem MenuFile_CloseDocument;
        private System.Windows.Forms.ToolStripMenuItem MenuTools;
        private System.Windows.Forms.ToolStripMenuItem MenuTools_Controls;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem MenuTools_Stop;
        private System.Windows.Forms.ToolStripMenuItem MenuGelp;
        private System.Windows.Forms.ToolStripMenuItem MenuHelp_About;
        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.Panel ControlPnl;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonForward;
        private System.Windows.Forms.Button TopPnlHideBtn;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.TextBox txtAddressBar;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.Panel pnlOuter;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem googleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}
