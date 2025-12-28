
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    /// <summary>Unstructured mesh in 2D.
    /// <para>Type used for co-ordinates is <see cref="vec2"/>.</para>
    /// <para>Contains collections of collections of index, scalar, vector and tensor fields that are created on demand.</para></summary>
    /// $A Igor Jan08 Mar09;
    public class UnstructuredMesh2d : UnstructuredMesh2d<vec2,
        Field<int>, int,
        Field<double>, double,
        Field<vec2>, vec2,
        Field<mat2>, mat2>
    {

        #region Construction

        /// <summary>Constructs a new empty 2D (unconnected) unstructured grid (no elements contained) with no name and no description.</summary>
        public UnstructuredMesh2d()
            : this(0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 2D (unconnected) unstructured grid with the specified size, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="length">Length of the field (number of grid nodes).</param>
        public UnstructuredMesh2d(int length)
            : this(length, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 2D (unconnected) unstructured grid with the specified size, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="length">Length of the field (number of grid nodes).</param>
        /// <param name="fieldName">Name of the field.</param>
        public UnstructuredMesh2d(int length, string fieldName)
            : this(length, fieldName, null /* description */)
        { }

        /// <summary>Constructs a new 2D (unconnected) unstructured grid with the specified size, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="length">Length of the field (number of grid nodes).</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public UnstructuredMesh2d(int length, string fieldName, string fieldDescription) :
            base(length, fieldName, fieldDescription)
        { }


        /// <summary>Constructs a (unconnected) unstructured grid based on 2D regular grid with grid direction parallel to coordinate axes, with
        /// specified name and description.
        /// Array of elements is allocated.</summary>
        /// <param name="dimx">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dimy">Second dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        /// <param name="minx">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxx">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="miny">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxy">Maximal value of y coordinate of the generated grid.</param>
        public UnstructuredMesh2d(int dimx, int dimy, 
            string fieldName, string fieldDescription,
            double minx, double maxx,
            double miny, double maxy) :
            this(dimx * dimy, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            Field<vec2>.GenerateCoordinatesPlain(this, dimx, dimy, 
                minx, maxx,
                miny, maxy);
        }


        /// <summary>Constructs an (unconnected) unstructured grid of nodes based on 2D structured 
        /// grid obtained by transformation of co-ordinates of a regular equidistant grid. Array of 
        /// elements is allocated.</summary>
        /// <param name="dimx">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dimy">Second dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second  
        /// node coordinates of the actual grid.</param>
        public UnstructuredMesh2d(int dimx, int dimy,
                string fieldName, string fieldDescription,
                double minxRef, double maxxRef,
                double minyRef, double maxyRef,
                IFunc2d fx, IFunc2d fy) :
            this(dimx * dimy, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            Field<vec2>.GenerateCoordinatesPlain(this, dimx, dimy,
                minxRef, maxxRef,
                minyRef, maxyRef,
                fx, fy);
        }

        #endregion Construction


        #region Generation

        /// <summary>Generates an (unconnected) unstructured grid of nodes based on a 2D regular grid 
        /// with grid directions parallel to coordinate axes and equidistant nodes in all directions.</summary>
        /// <param name="dimx">Dimenson (number of points) in the first generation direction.</param>
        /// <param name="dimy">Dimenson (number of points) in the second generation direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        public void GenerateCoordinates(
            int dimx, int dimy, 
            double minxRef, double maxxRef,
            double minyRef, double maxyRef)
        {
            SetLength(dimx * dimy);
            Field<vec2>.GenerateCoordinatesPlain(this, dimx, dimy,
                minxRef, maxxRef,
                minyRef, maxyRef);
        }


        /// <summary>Generates coordinates of an (unconnected) unstructured grid of points based on coordinates 
        /// of a structured mesh for a parametric volume according  to functions specifying the x, y, and z coordinates 
        /// in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by mapping individual coordinates of a regular equidistant grid
        /// from the reference system.</para></summary>
        /// <param name="dimx">Dimenson (number of points) in the first generation direction.</param>
        /// <param name="dimy">Dimenson (number of points) in the second generation direction.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        public void GenerateCoordinates(int dimx, int dimy, double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy)
        {
            SetLength(dimx * dimy);
            Field<vec2>.GenerateCoordinatesPlain(this, dimx, dimy, minxRef, maxxRef, minyRef, maxyRef,
                fx, fy);
        }


        #endregion Generation


    } // class UnstructuredMesh2d

}


