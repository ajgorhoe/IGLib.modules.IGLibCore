// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

// GRAPHICAL OBJECTS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;
using IG.Num;



namespace IG.Gr
{

    /// <summary>Specifies the type of the graphic primitive.</summary>
    [Flags]
    public enum GraphicPrimitiveType
    {
        Undefined = 0, 
        Point = 1, 
        Line = 2, 
        Trigangle = 4, 
        Quadrilateral = 8
    }


    /// <summary>Graphic set that contain graphic primitives and other graphic sets.</summary>
    /// $A Igor Mar10 Nov10;
    public interface IGraphicSet
    {

        /// <summary>Gets the parent group, if any.</summary>
        IGraphicSet Parent
        { get; }
    }



    public interface IGraphicPrimitive
    {





    }








}