// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using IG.Lib;

namespace IG.Gr
{



    public interface IGrsettings
    {
        color Color
        { get; set; }

    }

    /// <summary>Line Settings.</summary>
    /// $A Igor xx;
    public class GrLinesettings
    {
        public GrLinesettings()
        { 
            Color = new color(System.Drawing.Color.Black); 
        }

        protected color _color;

        /// <summary>Line color.</summary>
        color Color
        { get { return _color; } set { _color = value; } }


    }

    /// <summary>Fill Settings.</summary>
    /// $A Igor xx Oct 11;
    public class GrFillSettings
    {
        public GrFillSettings()
        { 
            Color = new color(System.Drawing.Color.LightBlue); 
        }

        protected color _color;

        /// <summary>Surface color.</summary>
        color Color
        { get { return _color; } set { _color = value; } }


    }

 
    /// <summary>Text Settings.</summary>
    /// $A Igor xx Oct11;
   public class GrTextSettings
    {

        public GrTextSettings()
        { 
            Color = new color(System.Drawing.Color.Brown); 
        }

        protected color _color;

        
        /// <summary>Font color.</summary>
       public color Color
        { get { return _color; } set { _color = value; } }


    }


 
    /// <summary>Point Settings.</summary>
    /// $A Igor xx Oct11;
    public class GrPointSettings
    {

        public GrPointSettings()
        { 
            Color = new color(System.Drawing.Color.Red); 
        }

        protected color _color;

        /// <summary>Point color.</summary>
        public color Color
        { get { return _color; } set { _color = value; } }


    }


 
    /// <summary>Settings for graphic primitive.</summary>
    /// $A Igor xx Oct 11;
    public class GrPrimitiveSettings
    {

        public GrPrimitiveSettings()
        { 
            Color = new color(System.Drawing.Color.LightGreen); 
        }

        protected color _color;

        public color Color
        { get { return _color; } set { _color = value; } }

    }


}
