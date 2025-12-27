
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    /// <summary>Structured mesh in 3D.
    /// <para>Type used for co-ordinates is <see cref="vec3"/>.</para></summary>
    /// $A Igor Jan08 Mar09 Oct11;
    public class StructuredMesh3d : StructuredMesh3d<vec3,
        StructuredField3d<int>, int,
        StructuredField3d<double>,
        double, StructuredField3d<vec3>, vec3,
        StructuredField3d<mat3>, mat3>
    {

        #region Construction

        /// <summary>Constructs a new empty 3D (unconnected) unstructured grid (no elements contained) with no name and no description.</summary>
        public StructuredMesh3d()
            : this(0, 0, 0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 3D (unconnected) unstructured grid with the specified dimensions, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field.</param>
        /// <param name="dim3">Third dimension of the field.</param>
        public StructuredMesh3d(int dim1, int dim2, int dim3)
            : this(dim1, dim2, dim3, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 3D (unconnected) unstructured grid with the specified dimensions, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field.</param>
        /// <param name="dim3">Third dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        public StructuredMesh3d(int dim1, int dim2, int dim3, string fieldName)
            : this(dim1, dim2, dim3, fieldName, null /* description */)
        { }

        /// <summary>Constructs a new 3D (unconnected) unstructured grid with the specified dimensions, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field.</param>
        /// <param name="dim3">Third dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public StructuredMesh3d(int dim1, int dim2, int dim3, string fieldName, string fieldDescription) :
            base(dim1, dim2, dim3, fieldName, fieldDescription)
        { }


        /// <summary>Constructs a 3D (unconnected) unstructured grid based on regular grid with grid direction parallel to coordinate axes, with
        /// specified name and description.
        /// Array of elements is allocated.</summary>
        /// <param name="dimx">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dimy">Second dimension of the field.</param>
        /// <param name="dimz">Third dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        /// <param name="minx">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxx">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="miny">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxy">Maximal value of y coordinate of the generated grid.</param>
        /// <param name="minz">Minimal value of z coordinate of the generated grid.</param>
        /// <param name="maxz">Maximal value of z coordinate of the generated grid.</param>
        public StructuredMesh3d(int dimx, int dimy, int dimz,
            string fieldName, string fieldDescription, 
            double minx, double maxx, 
            double miny, double maxy,
            double minz, double maxz) :
            this(dimx, dimy, dimz, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            GenerateCoordinatesPlain(this,
                minx, maxx,
                miny, maxy,
                minz, maxz);
        }


        /// <summary>Constructs a 3D (unconnected) unstructured grid based on structured grid by transformation of co-ordinates of a regular equidistant
        /// grid.
        /// Array of elements is allocated.</summary>
        /// <param name="dimx">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dimy">Second dimension of the field.</param>
        /// <param name="dimz">Third dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        /// <param name="minzRef">Minimal value of z coordinate of the generated grid.</param>
        /// <param name="maxzRef">Maximal value of z coordinate of the generated grid.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates of the actual grid.</param>
        public StructuredMesh3d(int dimx, int dimy, int dimz,
                string fieldName, string fieldDescription,
                double minxRef, double maxxRef,
                double minyRef, double maxyRef,
                double minzRef, double maxzRef,
                IFunc3d fx, IFunc3d fy, IFunc3d fz) :
            this(dimx, dimy, dimz, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            GenerateCoordinatesPlain(this,
                minxRef, maxxRef,
                minyRef, maxyRef,
                minzRef, maxzRef, 
                fx, fy, fz);
        }

        #endregion Construction


        #region Generation

        /// <summary>Generates coordinates of a 3D regular grid with grid directions parallel to coordinate axes
        /// and equidistant nodes in all directions.</summary>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        /// <param name="minzRef">Minimal value of z coordinate of the generated grid.</param>
        /// <param name="maxzRef">Maximal value of z coordinate of the generated grid.</param>
        public void GenerateCoordinates(
            double minxRef, double maxxRef,
            double minyRef, double maxyRef,
            double minzRef, double maxzRef)
        {
            GenerateCoordinatesPlain(this,
                minxRef, maxxRef,
                minyRef, maxyRef,
                minzRef, maxzRef);
        }


        /// <summary>Generates coordinates of the structured mesh for a parametric volume according 
        /// to functions specifying the x, y, and z coordinates in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para></summary>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="minzRef">Lower bound for the third parameter in the reference coordinate system.</param>
        /// <param name="maxzRef">Upper bound for the third parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates of the actual grid.</param>
        public void GenerateCoordinates(double minxRef, double maxxRef, double minyRef, double maxyRef,
            double minzRef, double maxzRef, IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            GenerateCoordinatesPlain(this, minxRef, maxxRef, minyRef, maxyRef,
                minzRef, maxzRef, fx, fy, fz);
        }


        #endregion Generation


    } // class StructuredMesh3d

}


