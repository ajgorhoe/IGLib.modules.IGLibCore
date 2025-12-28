
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    /// <summary>2D structured mesh.
    /// <para>Type used for co-ordinates and vectors is <see cref="vec2"/>.</para></summary>
    /// $A Igor Jan08 Mar09 Oct11;
    public class UnStructuredMesh2d : StructuredMesh2d<vec2,
        StructuredField2d<int>, int,
        StructuredField2d<double>,
        double, StructuredField2d<vec2>, vec2,
        StructuredField2d<mat2>, mat2>
    {

        #region Construction

        /// <summary>Constructs a new empty 2D mesh (no elements contained) with no name and no description.</summary>
        public UnStructuredMesh2d()
            : this(0, 0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">First dimension of the field (number of grid nodes in the first direction).</param>
        public UnStructuredMesh2d(int dim1, int dim2)
            : this(dim1, dim2, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="fieldName">Name of the field.</param>
        public UnStructuredMesh2d(int dim1, int dim2, string fieldName)
            : this(dim1, dim2, fieldName, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public UnStructuredMesh2d(int dim1, int dim2, string fieldName, string fieldDescription) :
            base(dim1, dim2, fieldName, fieldDescription)
        { }


        /// <summary>Constructs a 2D regular grid with grid directions parallel to coordinate axes, 
        /// with specified name and description.
        /// Array of elements is allocated.</summary>
        /// <param name="dimx">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dimy">Second dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        /// <param name="minx">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxx">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="miny">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxy">Maximal value of y coordinate of the generated grid.</param>
        public UnStructuredMesh2d(int dimx, int dimy,
            string fieldName, string fieldDescription,
            double minx, double maxx,
            double miny, double maxy) :
            this(dimx, dimy, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            GenerateCoordinates(this,
                minx, maxx,
                miny, maxy);
        }


        /// <summary>Constructs a 2D structured grid by transformation of co-ordinates of a regular 
        /// equidistant grid in XY plane.
        /// Array of elements is allocated.</summary>
        /// <param name="dimx">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dimy">Second dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        /// <param name="fx">Function that maps first two node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps first two node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        /// <param name="fz">Function that maps first two node coordinates of the reference grid to the third 
        /// node coordinates of the actual grid.</param>
        public UnStructuredMesh2d(int dimx, int dimy,
                string fieldName, string fieldDescription,
                double minxRef, double maxxRef,
                double minyRef, double maxyRef,
                IFunc2d fx, IFunc2d fy, IFunc2d fz) :
            this(dimx, dimy, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            GenerateCoordinatesPlain(this, minxRef, maxxRef, minyRef, maxyRef,
                fx, fy);
        }

        #endregion Construction


        #region Generation

        /// <summary>Generates coordinates of a 2D regular grid with grid directions 
        /// parallel to coordinate axes and equidistant nodes in all directions.</summary>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        public void GenerateCoordinates(
            double minxRef, double maxxRef,
            double minyRef, double maxyRef)
        {
            GenerateCoordinatesPlain(this, minxRef, maxxRef, minyRef, maxyRef);
        }


        /// <summary>Generates coordinates of the structured 2D mesh by mapping a reference mesh with equidistant
        /// nodes and mesh directions parallel to the coordinate axes.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para></summary>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps first two node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps first two node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        public void GenerateCoordinates(double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy)
        {
            GenerateCoordinatesPlain(this, minxRef, maxxRef, minyRef, maxyRef,
                fx, fy);
        }


        #endregion Generation



    }  // class StructuredMesh2d


}


