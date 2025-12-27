
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    /// <summary>2D structured mesh in 3D.
    /// <para>Grid is 2D but it is embedded in 3D space. Type used for co-ordinates is <see cref="vec3"/>.</para></summary>
    /// $A Igor Jan08 Mar09 Oct11;
    public class StructuredMesh2d3d : StructuredMesh2d<vec3, 
        StructuredField2d<int>, int, 
        StructuredField2d<double>, 
        double, StructuredField2d<vec3>, vec3, 
        StructuredField2d<mat3>, mat3>
    {

        #region Construction

        /// <summary>Constructs a new empty 2D field (no elements contained) with no name and no description.</summary>
        public StructuredMesh2d3d()
            : this(0, 0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">First dimension of the field (number of grid nodes in the first direction).</param>
        public StructuredMesh2d3d(int dim1, int dim2)
            : this(dim1, dim2, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="fieldName">Name of the field.</param>
        public StructuredMesh2d3d(int dim1, int dim2, string fieldName)
            : this(dim1, dim2, fieldName, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public StructuredMesh2d3d(int dim1, int dim2, string fieldName, string fieldDescription) :
            base(dim1, dim2, fieldName, fieldDescription)
        { }


        /// <summary>Constructs a 2D regular grid in 3D on the XY plane with grid directions parallel to coordinate axes, 
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
        public StructuredMesh2d3d(int dimx, int dimy,
            string fieldName, string fieldDescription, 
            double minx, double maxx, 
            double miny, double maxy) :
            base(dimx, dimy, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            GenerateCoordinates(this,
                minx, maxx,
                miny, maxy);
        }


        /// <summary>Constructs a 2D structured grid embedded in 3D by transformation of co-ordinates of a regular 
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
        public StructuredMesh2d3d(int dimx, int dimy,
                string fieldName, string fieldDescription,
                double minxRef, double maxxRef,
                double minyRef, double maxyRef,
                IFunc2d fx, IFunc2d fy, IFunc2d fz) :
            base(dimx, dimy, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            GenerateCoordinatesPlain(this, minxRef, maxxRef, minyRef, maxyRef,
                fx, fy, fz);
        }

        #endregion Construction


        #region Generation

        /// <summary>Generates coordinates of a 2D regular grid in 3D on the XY plane with grid directions 
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


        /// <summary>Generates coordinates of the structured 2D mesh embedded in 3D for a parametric surface according 
        /// to functions specifying the x, y, and z coordinates in terms of three scalar functions of 2 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// lying on XY plane from the reference system.</para></summary>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps first two node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps first two node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        /// <param name="fz">Function that maps first two node coordinates of the reference grid to the third 
        /// node coordinates of the actual grid.</param>
        public void GenerateCoordinates(double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy, IFunc2d fz)
        {
            GenerateCoordinatesPlain(this, minxRef, maxxRef, minyRef, maxyRef,
                fx, fy, fz);
        }


        #endregion Generation



    }  // class StructuredMesh2d3d


}


