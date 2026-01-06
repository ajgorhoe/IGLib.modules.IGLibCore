#nullable disable

// GRAPHICAL OBJECTS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using IG.Lib;
using IG.Num;



namespace IG.Gr
{


    /// <summary>Base class for graphic sets that contain groups of graphic primitives.</summary>
    /// $A Igor Mar10 Nov10;
    public abstract class GraphicSetBase
    {

        private IGraphicSet _parent;

        public IGraphicSet Parent
        {
            get { return _parent; }
            protected set { _parent = value; }
        }


        /// <summary>Gets number of child graphic sets directly contained in the current graphic set.</summary>
        public int NumGraphicSets
        {
            get { return _childPrimitives.Count; }
        }

        private List<IGraphicSet> _childSets = new List<IGraphicSet>();

        /// <summary>Returns the specified child graphic set contained in the current graphic set.</summary>
        /// <param name="i">Index of the child graphic set.</param>
        public IGraphicSet GetChildSet(int i)
        {
            return _childSets[i];
        }

        private List<IGraphicPrimitive> _childPrimitives = new List<IGraphicPrimitive>();

        /// <summary>Gets number of child graphic primitives directly contained in the current graphic set.</summary>
        public int NumGraphicPrimitives
        {
            get { return _childPrimitives.Count; }
        }

        /// <summary>Returns the specified child graphic primitive contained in the current graphic set.</summary>
        /// <param name="i">Index of the child graphic primitive.</param>
        public IGraphicPrimitive GetChildPrimitive(int i)
        {
            return _childPrimitives[i];
        }


        BoundingBox3d _bounds = new BoundingBox3d();


        IBoundingBox Bounds
        {
            get { return _bounds; }
        }


        // TODO:
        // implement updating according to different things!
        //

        //public virtual void RecalculateBounds(IGraphicPrimitive primitive)
        //{
        //    if (primitive != null)
        //        if (primitive.coordinates!=null)

        //}

        /// <summary>Recalculates the bounding box of the current graphic set.</summary>
        /// <param name="recursive">If true then bounds of sub-sets are re-calculated recursively.</param>
        public virtual void ReCalculateBounds(bool recursive)
        {
            if (recursive)
            {
                // TODO: implement this!
                for (int i = 0; i < NumGraphicPrimitives; ++i)
                    ; //  GetChildSet(i).RecalculateBounds(_childPrimitives[i]);
            }
        }

        public virtual void RecalculateBounds()
        { ReCalculateBounds(true /* recursive */ ); }


        #region RenderingProperties

        #endregion RenderingProperties


    }


    

    /// <summary>Base class for graphic primitives.</summary>
    /// $A Igor Mar10 Nov10;
    public abstract class GraphicPrimitiveBase: IGraphicPrimitive
    {

        private IGraphicSet _parent = null;

        public virtual IGraphicSet Parent
        {
            get { return _parent; }
            protected internal set { _parent = value; }
        }

        protected vec3[] _coord;
        protected color[] _lineColors;
        protected color[] _fillColors;
        protected GraphicPrimitiveType _type;

        #region Geeneral

        /// <summary>Returns flags enumeration indicating the type of the graphic primitive.</summary>
        public abstract GraphicPrimitiveType Type
        { get; }

        #endregion General


        #region Coordinates

        /// <summary>Gets number of co-ordinates of the graphic primitive.</summary>
        public virtual int NumCoordinates
        { get { return _coord.Length; } }

        /// <summary>Returns the specified co-ordinates of the graphic primitive.</summary>
        /// <param name="which">Index of primitive's node whose co-ordinates are returned.</param>
        public virtual vec3 GetCoordinate(int which)
        {
            if (which < 0 || which >= NumCoordinates)
                throw new IndexOutOfRangeException("Graphic primitive co-ordinate index out of bounds: "
                    + which + ", should be between 0 and " + (NumCoordinates - 1) + ".");
            return _coord[which];
        }

        /// <summary>Sets the specified co-ordinates of the graphic primitive.</summary>
        /// <param name="which">Index of the primitive's node whose co-ordinates are set.</param>
        /// <param name="coord"></param>
        public virtual void SetCoordinate(int which, vec3 coord)
        {
            if (which < 0 || which >= NumCoordinates)
                throw new IndexOutOfRangeException("Graphic primitive co-ordinate index out of bounds: "
                    + which + ", should be between 0 and " + (NumCoordinates - 1) + ".");
            _coord[which] = coord;
        }

        public virtual void UpdateBoundingBox(BoundingBox bounds)
        {
            if (bounds!=null && _coord!=null)
                for (int i=0; i<_coord.Length; ++i)
                {
                    // TODO: implement this!
                    // bounds.Update(_coord[i]);
                }
        }

        public virtual void UpdateBoundingBoxTransformed()
        {
        }

        #endregion Coordinates

        #region GraphicProperties

        /// <summary>Returns the specified line color of the graphic primitive.</summary>
        /// <param name="which">Index of primitive's line whose color is set.</param>
        public virtual Color GetLineColor(int which)
        {
            if (which < 0)
                throw new IndexOutOfRangeException("Invalid line color index: " + which);
            if (_lineColors == null)
                return Color.Blue;
            else if (which >= _lineColors.Length)
            {
                if (_lineColors.Length < 1)
                    return Color.Blue;
                else
                    return _lineColors[0];
            }
            else
                return _lineColors[which];
        }

        /// <summary>Sets the specified line color of the graphic primitive.</summary>
        /// <param name="which">Index of primitive's line whose color is set.</param>
        /// <param name="color">Color to be set.</param>
        public virtual void SetLineColor(int which, Color color)
        {
            if (which < 0)
                throw new IndexOutOfRangeException("Invalid line color index: " + which);
            if (_lineColors == null)
            {
                Color[] c = new Color[which+1];
                c[which] = color;
            } else if (which >= _lineColors.Length)
            {
                Color[] c = new Color[which + 1];
                for (int i = 0; i < _lineColors.Length; ++i)
                    c[i] = _lineColors[i];
                c[which] = color;
            }
            else
                _lineColors[which] = color;
        }

        /// <summary>Returns average line color of the graphic primitive.</summary>
        public virtual color GetLineColor()
        {
            double R=0, G=0, B=0, opacity=0;
            for (int i=0; i<_lineColors.Length; ++i)
            {
                opacity += _lineColors[i].Opacity;
                R += _lineColors[i].R;
                G += _lineColors[i].G;
                B += _lineColors[i].B;
            }
            opacity /= _lineColors.Length;
            R /= _lineColors.Length;
            G /= _lineColors.Length;
            B /= _lineColors.Length;
            return new color(R, G, B, opacity);
        }

        /// <summary>Sets all line colors of the graphic primitive to the specified value.</summary>
        public virtual void SetLineColor(Color color)
        {
            if (_lineColors == null)
                _lineColors = new color[1];
            else if (_lineColors.Length < 1)
                _lineColors = new color[1];
            for (int i = 0; i < _lineColors.Length; ++i)
                _lineColors[i] = color;
        }


        /// <summary>Returns the specified fill color of the graphic primitive.</summary>
        /// <param name="which">Index of primitive's fill whose color is set.</param>
        public virtual Color GetFillColor(int which)
        {
            if (which < 0)
                throw new IndexOutOfRangeException("Invalid fill color index: " + which);
            if (_fillColors == null)
                return Color.Blue;
            else if (which >= _fillColors.Length)
            {
                if (_fillColors.Length < 1)
                    return Color.Blue;
                else
                    return _fillColors[0];
            }
            else
                return _fillColors[which];
        }

        /// <summary>Sets the specified fill color of the graphic primitive.</summary>
        /// <param name="which">Index of primitive's fill whose color is set.</param>
        /// <param name="color">Color to be set.</param>
        public virtual void SetFillColor(int which, Color color)
        {
            if (which < 0)
                throw new IndexOutOfRangeException("Invalid fill color index: " + which);
            if (_fillColors == null)
            {
                Color[] c = new Color[which + 1];
                c[which] = color;
            }
            else if (which >= _fillColors.Length)
            {
                Color[] c = new Color[which + 1];
                for (int i = 0; i < _fillColors.Length; ++i)
                    c[i] = _fillColors[i];
                c[which] = color;
            }
            else
                _fillColors[which] = color;
        }

        /// <summary>Returns average fill color of the graphic primitive.</summary>
        public virtual Color GetFillColor()
        {
            double R = 0, G = 0, B = 0, opacity = 0;
            for (int i = 0; i < _fillColors.Length; ++i)
            {
                opacity += _fillColors[i].Opacity;
                R += _fillColors[i].R;
                G += _fillColors[i].G;
                B += _fillColors[i].B;
            }
            opacity /= _fillColors.Length;
            R /= _fillColors.Length;
            G /= _fillColors.Length;
            B /= _fillColors.Length;
            return new color(R, G, B, opacity);
        }

        /// <summary>Sets all fill colors of the graphic primitive to the specified value.</summary>
        public virtual void SetFillColor(Color color)
        {
            if (_fillColors == null)
                _fillColors = new color[1];
            else if (_fillColors.Length < 1)
                _fillColors = new color[1];
            for (int i = 0; i < _fillColors.Length; ++i)
                _fillColors[i] = color;
        }

        #endregion GraphicProperties


    }  // abstract class GrPrimitiveBase


    /// <summary>Base class for graphic line primitive.</summary>
    public abstract class GraphicPointBase: GraphicPrimitiveBase
    {

    }


    /// <summary>Base class for graphic line primitive.</summary>
    public abstract class GraphicLineBase : GraphicPrimitiveBase
    {


    }


    /// <summary>Base class for graphic line primitive.</summary>
    public abstract class GraphicTriangleBase : GraphicPrimitiveBase
    {


    }


    /// <summary>Base class for graphic line primitive.</summary>
    public abstract class GraphicQuadriLateralBase : GraphicPrimitiveBase
    {


    }



}