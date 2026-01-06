#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Num;

namespace IG.Gr
{


    abstract public class Plotter
    {

    }



    //abstract public class GraphicObjectBase 
    //{
        
    //    /// <summary>Gets the parent object of this object (if there exists)</summary>
    //    public abstract GraphicObject Parent
    //    { 
    //        get;
    //        protected internal set;
    //    }


    //    /// <summary>Adds a graphic primitive to the object.</summary>
    //    public abstract void Add(GraphicPrimitive primitive);

    //    /// <summary>Adds a child graphic object to the current objects.</summary>
    //    /// <param name="grobject">Graphic object that is added as a child object.</param>
    //    public abstract void Add(GraphicObject grobject);


    //    // Graphic objects contained in the current object:

    //    ///// <summary>Returns a list of child objects of this graphic object.</summary>
    //    //public abstract List<GraphicObject> Children
    //    //{
    //    //    get;
    //    //    protected internal set;
    //    //}




    //    /// <summary>Gets the number of child objects of the current element 
    //    /// (descendants of higher leval are excluded).</summary>
    //    public abstract int NumDirectChildObjects
    //    { get; }

    //    /// <summary>Gets the total number of descendant objects.</summary>
    //    public abstract int NumObjects
    //    { get; }


    //    /// <summary>Returns tha direct child object No. i.</summary>
    //    /// <param name="i">Index of the child object to return.</param>
    //    /// <returns>The direct child object No i.</returns>
    //    public abstract GraphicObject GetDirectChildObject(int i);

    //    /// <summary>Returns the descendant graphic object with a specified index.
    //    /// Index runs over all descendant objects in such a way that deeper descendants have higher indices.</summary>
    //    /// <param name="i">Index of the descendant object.
    //    /// Objects that are deeper in the descendant tree have higher indices.</param>
    //    /// <returns>The requested object.</returns>
    //    /// <exception cref="Exception">Thrown if index is out of range.</exception>
    //    public abstract GraphicObject GetObject(int i);

    //    /// <summary>Adds a child object to the graphic object.</summary>
    //    public abstract int AddChildObject(GraphicObject grobject);



    //    // Graphic objects contained in the current object:

    //    ///// <summary>Returns a list of graphic primitives that form this graphic object 
    //    ///// (together with its child objects).</summary>
    //    //public abstract List<GraphicPrimitive> Primitives
    //    //{
    //    //    get;
    //    //    protected internal set;
    //    //}



    //    /// <summary>Gets the number of graphic primitives directly contained in this object.
    //    /// This does NOT include the primitive contained in descendant objects.</summary>
    //    public abstract int NumDirectPrimitives
    //    { get; }

    //    /// <summary>Gets the total number of graphic primitives contained in this object,
    //    /// which includes primitives of all descendant graphic objects.</summary>
    //    public abstract int NumPrimitives
    //    { get; }

    //    /// <summary>Returns the primitive No. i that is directly contained on this graphic object.
    //    /// Primitives contained in descendant objects are excluded.</summary>
    //    /// <param name="i">Zero-based index of the primitive.</param>
    //    /// <returns>The specified primitive that is directly contained in the current graphic object.</returns>
    //    public abstract GraphicPrimitive GetDirectPrimitive(int i);

    //    /// <summary>Returns the graphic primitive No. i that is directly contained in this graphic object.</summary>
    //    /// <param name="i">Zaro-based index of the primitive. Primitives that are contained in descendant objects
    //    /// of higher levels have greater indices.</param>
    //    /// <returns>The specified graphic primitive contained in the current graphic object or one of its descendants.</returns>
    //    public abstract GraphicPrimitive GetPrimitive(int i);


    //    /// <summary>Gets or sets the specified graphic primitive that is DIRECTLY contained in this graphic object.</summary>
    //    /// <param name="i">Index of the primitive.</param>
    //    public abstract GraphicPrimitive this[int i]
    //    {
    //        get;
    //        protected internal set;
    //    }

    //    // Grphical settings:

    //    /// <summary>Gets the primitive's internal graphic properties (such as color, etc.)</summary>
    //    public abstract GrPrimitiveSettings Settings
    //    {
    //        get;
    //        protected set;
    //    }

    //    /// <summary>Gets the primitive's internal graphic properties (such as color, etc.)</summary>
    //    public virtual GrPrimitiveSettings EffectiveSettings
    //    {
    //        get
    //        {
    //            GrPrimitiveSettings ReturnedString = Settings;
    //            if (ReturnedString == null && Parent != null)
    //                ReturnedString = Parent.EffectiveSettings;
    //        }
    //    }



    //}  // class GraphicObjectBase - to be deleted!


    // Graphic object:

    /// <summary>Graphic object, a tree - like collection of graphic primitives.</summary>
    public class GraphicObject
    {

        #region Data

        private GraphicObject _parent = null;


        private List<GraphicObject> _childobjects = new List<GraphicObject>();

        private List<GraphicPrimitive> _primitives = new List<GraphicPrimitive>();

        #endregion Data


        /// <summary>Gets the parent object of this object (if there exists).</summary>
        public virtual GraphicObject Parent
        {
            get { return _parent; }
            protected internal set { _parent = value; }
        }

        /// <summary>Adds a graphic primitive to the object.</summary>
        public virtual void Add(GraphicPrimitive primitive)
        {
            _primitives.Add(primitive);
        }

        /// <summary>Adds a child graphic object to the current objects.</summary>
        /// <param name="grobject">Graphic object that is added as a child object.</param>
        public virtual void Add(GraphicObject grobject)
        {
            _childobjects.Add(grobject);
        }

        /// <summary>Gets the number of child objects of the current element 
        /// (descendants of higher leval are excluded).</summary>
        public virtual int NumDirectChildObjects
        { get { return _childobjects.Count; } }

        /// <summary>Gets the total number of descendant objects.</summary>
        public virtual int NumObjects
        {
            get
            {
                int numchildren = _childobjects.Count;
                int ret = numchildren;
                for (int i = 0; i < numchildren; ++i)
                    ret += _childobjects[i].NumObjects;  // total number of cild's children (induces recursion)
                return ret;
            }
        }


        /// <summary>Returns tha direct child object No. i.</summary>
        /// <param name="i">Index of the child object to return.</param>
        /// <returns>The direct child object No i.</returns>
        /// <exception cref="IndexOutOfRangeException">When index is out of range.</exception>
        public virtual GraphicObject GetDirectChildObject(int i)
        {
            if (i < 0 || i >= _childobjects.Count)
                throw new IndexOutOfRangeException("Index " + i.ToString() +" is out of range (0 to " + 
                    (_childobjects.Count-1).ToString() + ").");
            return _childobjects[i];
        }

        /// <summary>Adds a child object to the graphic object.</summary>
        public virtual void AddChildObject(GraphicObject grobject)
        {
            _childobjects.Add(grobject);
        }


        /// <summary>Gets the number of graphic primitives directly contained in this object.
        /// This does NOT include the primitive contained in descendant objects.</summary>
        public virtual int NumDirectPrimitives
        { get { return _primitives.Count; } }

        /// <summary>Gets the total number of graphic primitives contained in this object,
        /// which includes primitives of all descendant graphic objects.</summary>
        public virtual int NumPrimitives
        {
            get
            {
                int ret = _primitives.Count;
                for (int i = 0; i < _childobjects.Count; ++i)
                    ret += _childobjects[i].NumPrimitives;  // total number of primitives on child object; induces recursion
                return ret;
            }
        }

        /// <summary>Returns the primitive No. i that is directly contained on this graphic object.
        /// Primitives contained in descendant objects are excluded.</summary>
        /// <param name="i">Zero-based index of the primitive.</param>
        /// <returns>The specified primitive that is directly contained in the current graphic object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When index is out of range.</exception>
        public virtual GraphicPrimitive GetDirectPrimitive(int i)
        {
            if (i < 0 || i >= _primitives.Count)
                throw new ArgumentOutOfRangeException("Index " + i.ToString() +" is out of range (0 to " + 
                    (_primitives.Count-1).ToString() + ").");
            return _primitives[i];
        }


        // Graphic settinigs:


        private GrPrimitiveSettings _settings = null;

        /// <summary>Gets the primitive's internal graphic properties (such as color, etc.)</summary>
        public virtual GrPrimitiveSettings Settings
        {
            get
            { return _settings; }
            protected set { _settings = value; }
        }


        public virtual GrPrimitiveSettings EffectiveSettings
        {
            get
            {
                GrPrimitiveSettings ret = Settings;
                if (ret == null && Parent != null)
                    ret = Parent.EffectiveSettings;
                return ret;
            }
        }




    }  // class GraphicObject






    /// <summary>Abstract base class for all graphic primitives such as lines, facets, etc.</summary>
    public abstract class GraphicPrimitive
    {

        private GraphicObject _parent = null;  // graphic object that directly contains this primitive.

        private GrPrimitiveSettings _settings = null;

        /// <summary>Returns the graphic object that directly contains this primitive.</summary>
        public virtual GraphicObject Parent
        {
            get { return _parent; }
            protected internal set { _parent = value; }
        }

        /// <summary>Get or set the co-ordinate No. i of this graphic primitive.</summary>
        public abstract int NumCoordinates
        {  get;  }

        /// <summary>Returns the co-ordinate No. i of the graphic primitive.</summary>
        public abstract vec3 this[int i]
        { get; protected internal set; }

        ///// <summary>Reyurns the center of mass of all co-ordinates of this primitive.</summary>
        //public abstract Coord3D Center
        //{ get; }


        /// <summary>Reyurns the center of mass of all co-ordinates of this primitive.</summary>
        public virtual vec3 Center
        {
            get
            {
                vec3 ret = new vec3(0.0, 0.0, 0.0);
                int num = NumCoordinates;
                for (int i = 0; i < num; ++i)
                {
                    vec3 current = this[i];
                    ret += current;
                    num += 1;
                }
                ret /= num;
                return ret;
            }
        }



        /// <summary>Gets the primitive's internal graphic properties (such as color, etc.)</summary>
        public virtual GrPrimitiveSettings Settings
        {
            get
            { return _settings; }
            protected set { _settings = value; }
        }

        public virtual GrPrimitiveSettings EffectiveSettings
        {
            get 
            {  
                GrPrimitiveSettings ret = Settings;
                if (ret == null && Parent != null)
                    ret = Parent.EffectiveSettings;
                return ret;
            }
        }



    }  // GraphicPrimitive


    /// <summary>GraphicPrimitive that has a protected Coordinates[] array.</summary>
    public abstract class GraphicPrimitiveSimple: GraphicPrimitive
    {

        protected abstract vec3[] Coordinates
        {
            get;
        }

        public override int NumCoordinates
        {
            get 
            { 
                if (Coordinates!=null)
                    return Coordinates.Length;
                return 0;
            } 
        }

        /// <summary>Get or set the co-ordinate No. i of the graphic primitive.</summary>
        public override vec3 this[int i]
        {
            get 
            { 
                if (Coordinates == null)
                    throw new Exception ("Graphic primitive co-ordinates are not defined.");
                if (i<0 || i>=NumCoordinates)
                    throw new ArgumentException("Index " + i.ToString() + " is out of range [0, " 
                        + (NumCoordinates-1).ToString() + "].");
                return Coordinates[i]; }
            protected internal set 
            { 
                if (Coordinates == null)
                    throw new Exception ("Graphic primitive co-ordinates are not defined.");
                if (i<0 || i>=NumCoordinates)
                    throw new ArgumentException("Index " + i.ToString() + " is out of range [0, " 
                        + (NumCoordinates-1).ToString() + "].");
                Coordinates[i] = value; 
            }
        }




    }  // class GraphicPrimitiveSimple





    public class PointPrimitive : GraphicPrimitiveSimple
    {

        #region Data

        private vec3[] _coord = new vec3[1];

        protected override vec3[] Coordinates
        { get { return _coord; } }

        #endregion  // Data

    }

    public class TextPrimitive : GraphicPrimitiveSimple
    {

        #region Data

        private vec3[] _coord = new vec3[1];

        protected override vec3[] Coordinates
        { get { return _coord; } }

        #endregion  // Data

    }

    public class LinePrimitive : GraphicPrimitiveSimple
    {

        #region Data

        private vec3[] _coord = new vec3[1];

        protected override vec3[] Coordinates
        { get { return _coord; } }

        #endregion  // Data

    }

    public class TrilateralFacePrimitive : GraphicPrimitiveSimple
    {

        #region Data

        private vec3[] _coord = new vec3[1];

        protected override vec3[] Coordinates
        { get { return _coord; } }

        #endregion  // Data

    }

    public class QuadriLateralFacePrimitive : GraphicPrimitiveSimple
    {

        #region Data

        private vec3[] _coord = new vec3[1];

        protected override vec3[] Coordinates
        { get { return _coord; } }

        #endregion  // Data

    }


    public abstract class SurfaceGridPrimitive : GraphicPrimitive
    {
    }


    public class StructuredSurfaceGridPrimitive : SurfaceGridPrimitive
    {

        #region Construction

        protected StructuredSurfaceGridPrimitive()  // Default constructor not allowed.
        {  }

        /// <summary>Constructs a structured surface grid primitive in 3 dimensions.
        /// Grid co-ordinates are initialized to 0.</summary>
        /// <param name="num1">Number of points in the first grid direction.</param>
        /// <param name="num2">Number of points in the second grid direction.</param>
        public StructuredSurfaceGridPrimitive(int num1, int num2)
        {
            _mesh = new StructuredSurfaceMesh3D(num1, num2);
        }

        /// <summary>Construct a structured surface grid primitive in 3 dimensions.
        /// Complete is constructed by translations of the origin by linear combinations of two base vectors
        /// with integer factors.</summary>
        /// <param name="origin">Origin of the mesh.</param>
        /// <param name="basevector1">The first base step of the mesh.</param>
        /// <param name="basevector2">The second base step of the mesh.</param>
        /// <param name="num1">Number of points in the first grid direction.</param>
        /// <param name="num2">Number of points in the second grid direction.</param>
        public StructuredSurfaceGridPrimitive(vec3 origin, vec3 basevector1, vec3 basevector2, int num1, int num2)
        {
            _mesh = new StructuredSurfaceMesh3D(origin, basevector1, basevector2, num1, num2);
        }

        #endregion  Construction

        // private Coord3D[,] grid;  // Basic data structure


        StructuredSurfaceMesh3D _mesh = null;



        public override int NumCoordinates
        {
            get
            {
                return _mesh.NumNodes;
            }
        }

        public override vec3 this[int i]
        {
            get 
            { 
                return _mesh[i];
            }
            protected internal set 
            { 
                _mesh[i]=value;
            }
        }


    }  // class SurfaceGridQuadStructured






}
