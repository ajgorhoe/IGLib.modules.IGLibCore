//// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//using IG.Lib;

//namespace IG.Forms 
//{

//    /// <summary>Test form, used only to test if control can be inclded in the form..</summary>
//    /// <remarks><para>This is a dummy control used for various tests such as window positioning.</para></remarks>
//    /// $A Igor Oct09;
//    [Obsolete("This form is not used.")]
//    public partial class AlignmentTestForm : Form, IIdentifiable,
//        IG.Forms.IClosableFromContainedForm
//    {
//        public AlignmentTestForm()
//        {
//            InitializeComponent();
//            this.Text = Title;
//        }


//        public string Title
//        {
//            get { return "IGLib's Test form No. " + Id; }
//        }

//        #region IIdentifiable

//        /// <summary>Unique ID for objects of the currnet and derived classes.
//        /// <para>Returns the ID of the embedded test control (of type <see cref="AlignmentControlOld"/>).</para></summary>
//        public int Id
//        { get { return testControl1.Id; } }

//        #endregion IIdentifiable


//    }  // Class TestForm
//}
