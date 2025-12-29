namespace IG.Forms
{
    partial class BrowserSimpleForm
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
            this.browserForm = new IG.Forms.BrowserSimpleControl();
            this.SuspendLayout();
            // 
            // browserForm
            // 
            this.browserForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserForm.FileToOpen = "d:\\users\\igor\\0000\\tmp\\index.html";
            this.browserForm.HomeUrl = "http://www2.arnes.si/~ljc3m2/igor/iglib/";
            this.browserForm.Location = new System.Drawing.Point(0, 0);
            this.browserForm.Name = "browserForm";
            this.browserForm.SearchUrl = "http://www.google.com";
            this.browserForm.Size = new System.Drawing.Size(684, 462);
            this.browserForm.TabIndex = 0;
            // 
            // BrowserSimpleWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 462);
            this.Controls.Add(this.browserForm);
            this.Icon = IG.Forms.Properties.Resources.ig;
            this.Name = "BrowserSimpleWindow";
            this.Text = "BrowserSimpleWindow";
            this.VisibleChanged += new System.EventHandler(this.BrowserSimpleWindow_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public BrowserSimpleControl browserForm;
    }
}