#nullable disable

using IG.Lib;
using System.ComponentModel;
using System.Windows.Forms;

namespace IG.Forms
{

    /// <summary>Test control.</summary>
    /// <remarks><para>This is a dummy control used for various tests such as window positioning.</para></remarks>
    /// $A Igor Oct09;
    public partial class TestControl : UserControl, IIdentifiable
    {
        public TestControl()
        {
            InitializeComponent();
            // this.lblTitle.Text = Title;
            this.Title = "Test control No. " + this.Id;
        }

        private string _title = null;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Title
        {
            get { return _title; }
            set {
                if (!string.IsNullOrEmpty(value))
                {
                    this._title = value; this.lblTitle.Text = Title;
                }
            }
        }

        #region IIdentifiable

        private static int _nextId = 0;

        /// <summary>Returns another ID that is unique for objects of the containing class 
        /// its and derived classes.</summary>
        protected static int GetNextId()
        {
            lock (Util.LockGlobal)
            {
                ++_nextId;
                return _nextId;
            }
        }

        private int _id = GetNextId();

        /// <summary>Unique ID for objects of the currnet and derived classes.</summary>
        public int Id
        { get { return _id; } }

        #endregion IIdentifiable



    } // class TestControl

}
