using IG.Lib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IG.Forms
{
    public partial class BrowserSimpleControl : UserControl, ILockable
    {

        public BrowserSimpleControl()
        {
            InitializeComponent();
            this.ActiveControl = txtAddressBar;
            // string s = Util.MutexGlobalName;
            buttonBack.Enabled = false;
            buttonForward.Enabled = false;
            buttonStop.Enabled = false;
            FetchBrowserInitialPosition();
        }


        #region ILockable

        private object _mainLock = new object();

        /// <summary>This object's central lock object to be used by other object.
        /// Do not use this object for locking in class' methods, for this you should use 
        /// InternalLock.</summary>
        public object Lock { get { return _mainLock; } }

        #endregion ILockable

        #region Operation

        private bool _browserPositionRemembered = false;
        private int _browserYInitialPosition = 0;
        private int _controlPanelHeigt = 0;
        private int _statusStripHeight = 0;


        /// <summary>Obtains browser y position and height of the panel containing controls.</summary>
        protected virtual void FetchBrowserInitialPosition()
        {
            if (!_browserPositionRemembered && this.Visible)
            {   
                _browserYInitialPosition = browser.Location.Y;
                _controlPanelHeigt = ControlPnl.Height;
                _statusStripHeight = this.Height - statusStrip1.Location.Y;
                _browserPositionRemembered = true;
            }
        }

        /// <summary>Sets the browser y position within the form to its original position.</summary>
        protected virtual void SetBrowserPositionOriginal()
        {
            FetchBrowserInitialPosition();
            browser.Location = new Point(browser.Location.X, _browserYInitialPosition);
            browser.Height = statusStrip1.Location.Y - browser.Location.Y;
        }

        /// <summary>Sets browser y position for the situation where browser controls are not visible.</summary>
        protected virtual void SetBrowserPositionWithoutControls()
        {
            FetchBrowserInitialPosition();
            browser.Location = new Point(browser.Location.X, _browserYInitialPosition - _controlPanelHeigt);
            browser.Height = statusStrip1.Location.Y - browser.Location.Y;
        }

        public virtual void OpenLocation(string location)
        {
            if (InvokeRequired)
            {
                this.Invoke((Action) delegate {
                    OpenLocation(location);
                } );
            }
            else
            {
                browser.Navigate(location);
            }
        }

        public const string DefaultHomeUrl = Util.IGLibUrl;

        protected string _homeUrl = DefaultHomeUrl;

        /// <summary>Home page Url.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string HomeUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_homeUrl))
                {
                    _homeUrl = DefaultHomeUrl;
                }
                return _homeUrl;
            }
            set
            {
                _homeUrl = value;
            }
        }

        public const string DefaultSearchUrl = "http://www.google.com";

        protected string _searchUrl = DefaultSearchUrl;

        /// <summary>Home page Url.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string SearchUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_searchUrl))
                {
                    _searchUrl = DefaultHomeUrl;
                }
                return _searchUrl;
            }
            set
            {
                _searchUrl = value;
            }
        }

        public const string DefaultFileToOpen = "d:/users/igor/0000/tmp/index.html";

        protected string _fileToOpen;

        /// <summary>File to be opened in the file open dialog box.
        /// <para>Used for setting initial directory for the dialog box. Each time this property is
        /// reset to the last file path picked from the dialog box.</para></summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string FileToOpen
        {
            get
            {
                if (string.IsNullOrEmpty(_fileToOpen))
                {
                    if (!string.IsNullOrEmpty(DefaultFileToOpen))
                        _fileToOpen = DefaultFileToOpen.Replace('/', Path.DirectorySeparatorChar); ;
                }
                return _fileToOpen;
            }
            set
            {
                _fileToOpen = value;
            }
        }
        /// <summary>Reports the error that occurred in the current window control.</summary>
        /// <param name="ex">Exception that was thrown because of the error.</param>
        protected void ReportError(Exception ex)
        {
            ReportError(ex.Message);
        }

        /// <summary>Reports the error that occurred in the current window control.</summary>
        /// <param name="errorMessage">Error message that describes the error.</param>
        protected void ReportError(string errorMessage)
        {
            Console.WriteLine(Environment.NewLine + "ERROR: " + errorMessage + Environment.NewLine);
            ////Creating a fading message, which can be canceled explicitly (e.g. by pressing mouse button 3:)
            //string msgtitle = "Error!";
            //string msgtext = "The following error occurred: " + errorMessage + Environment.NewLine;
            //int showTime = 4000;
            //FadingMessage fm = new FadingMessage(msgtitle, msgtext, showTime);
        }

        /// <summary>Sets browser URL to the text from the address bar.</summary>
        private void UpdateUrlToAdressBar()
        {
            try
            {
                UpdateUrl(txtAddressBar.Text);
            }
            catch (Exception ex)
            {
                ReportError(ex);
                throw;
            }
        }


        /// <summary>Sets browser URL to the specified text.</summary>
        /// <param name="urlString">String representing the url to which address must be changed.</param>
        private void UpdateUrl(string urlString)
        {
            try
            {
                if (string.IsNullOrEmpty(urlString))
                    return;
                Uri url = new Uri(urlString);
                urlString = url.ToString();
                string currentUrl;
                if (browser.Url != null)
                    currentUrl = browser.Url.ToString();
                else
                    currentUrl = null;
                if (urlString != currentUrl)
                {
                    txtAddressBar.Text = urlString;
                    url = new Uri(urlString); 
                    browser.Url = url;
                }
            }
            catch (Exception ex)
            {
                ReportError(ex);
                throw;
            }

        }

        #endregion Operation

        private void txtAddressBar_Leave(object sender, EventArgs e)
        {
            try
            {
            UpdateUrlToAdressBar();
            }
            catch (Exception ex)
            {
                ReportError(ex);
                throw;
            }
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("buttonHome_Click begins...");
                UpdateUrl(HomeUrl);
                Console.WriteLine("buttonHome_Click ends...");
            }
            catch (Exception ex)
            {
                ReportError(ex);

                Console.WriteLine("buttonHome_Click error...");


                throw;
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            browser.GoBack();
            txtAddressBar.Text = browser.Url.ToString();
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            browser.GoForward();
            txtAddressBar.Text = browser.Url.ToString();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                browser.Refresh();
            }
            catch (Exception ex)
            {
                ReportError(ex);
                throw;
            }
        }

        private void txtAddressBar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                browser.Navigate(txtAddressBar.Text);
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            browser.Navigate(txtAddressBar.Text);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            browser.Stop();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            txtAddressBar.Text = browser.Url.ToString();
            buttonStop.Enabled = false;
            if (browser.CanGoBack == true)
                buttonBack.Enabled = true;
            else
                buttonBack.Enabled = false;
            if (browser.CanGoForward)
                buttonForward.Enabled = true;
            else
                buttonForward.Enabled = false;
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            buttonStop.Enabled = true;
        }


        private void webBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
            if (browser.CanGoBack == true)
            {
                buttonBack.Enabled = true;
            }
            else
            {
                buttonBack.Enabled = false;
            }
        }

        private void webBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
            if (browser.CanGoForward == true)
            {
                buttonForward.Enabled = true;
            }
            else
            {
                buttonForward.Enabled = false;
            }
        }


        private void HideTopPanel()
        {
            ControlPnl.Visible = false;
            SetBrowserPositionWithoutControls();
        }

        private void ShowTopPanel()
        {
            ControlPnl.Visible = true;
            SetBrowserPositionOriginal();
        }

        private void TopPnlHideBtn_Click(object sender, EventArgs e)
        {
            // HideTopPanel();
            MenuTools_Controls.Checked = false;
        }

        private void MenuTools_Controls_CheckStateChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem a = (ToolStripMenuItem) sender;
            if (a.Checked)
                ShowTopPanel();
            else
                HideTopPanel();
        }

        private void BrowserSimpleControl_VisibleChanged(object sender, EventArgs e)
        {
            this.ActiveControl = txtAddressBar;
        }

        private void MenuFile_OpenURL_Click(object sender, EventArgs e)
        {
            this.ActiveControl = txtAddressBar;
            string text = "< Type the URL here! >";
            int length = text.Length;
            txtAddressBar.Text = text;
            this.txtAddressBar.Select(0, length - 1);
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Navigate(HomeUrl);
        }

        private void googleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Navigate(SearchUrl);
        }

        private void otherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ActiveControl = txtAddressBar;
            string text = "< Type the URL here! >";
            int length = text.Length;
            txtAddressBar.Text = text;
            this.txtAddressBar.Select(0, length);
        }

        private void MenuFile_Open_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowReadOnly = true;
            if (!string.IsNullOrEmpty(FileToOpen))
                openFileDialog.InitialDirectory = Path.GetDirectoryName(FileToOpen);
            DialogResult result = openFileDialog.ShowDialog();
            if (result!=DialogResult.Cancel && result!=DialogResult.Abort)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    FileToOpen = filePath;
                    browser.Navigate(filePath);
                }
                catch(Exception ex)
                {
                    ReportError(ex);
                }
            }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.ShowPrintPreviewDialog();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Print();
        }



        private void MenuFile_Close_Click(object sender, EventArgs e)
        {
            if (this.TopLevelControl == this.Parent)
            {
                this.TopLevelControl.Visible = false;
            }
        }

        private void MenuFile_CloseDocument_Click(object sender, EventArgs e)
        {
            browser.Navigate("");
        }

        private void MenuHelp_About_Click(object sender, EventArgs e)
        {
            string title = "About";
            string text = 
                "Simple embeddable web browser." 
                + Environment.NewLine +Environment.NewLine
                + "By Igor Grešovnik, 2008."
                + Environment.NewLine
                + "Part of GLib Forms."
                + Environment.NewLine
                + Util.IGLibUrl;
            if (MessageBox.Show(text, title, MessageBoxButtons.OK) == DialogResult.Yes)
            {
            }
        }


    }
}
