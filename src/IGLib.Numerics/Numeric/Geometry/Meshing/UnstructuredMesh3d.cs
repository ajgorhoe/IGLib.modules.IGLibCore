
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    /// <summary>Unstructured mesh in 3D.
    /// <para>Type used for co-ordinates is <see cref="vec3"/>.</para>
    /// <para>Contains collections of collections of index, scalar, vector and tensor fields that are created on demand.</para></summary>
    /// $A Igor Jan08 Mar09;
    public class UnstructuredMesh3d : UnstructuredMesh3d<vec3,
        Field<int>, int,
        Field<double>, double,
        Field<vec3>, vec3,
        Field<mat3>, mat3>
    {

        #region Construction

        /// <summary>Constructs a new empty 3D (unconnected) unstructured grid (no elements contained) with no name and no description.</summary>
        public UnstructuredMesh3d()
            : this(0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 3D (unconnected) unstructured grid with the specified size, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="length">Length of the field (number of grid nodes).</param>
        public UnstructuredMesh3d(int length)
            : this(length, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 3D (unconnected) unstructured grid with the specified size, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="length">Length of the field (number of grid nodes).</param>
        /// <param name="fieldName">Name of the field.</param>
        public UnstructuredMesh3d(int length, string fieldName)
            : this(length, fieldName, null /* description */)
        { }

        /// <summary>Constructs a new 3D (unconnected) unstructured grid with the specified size, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
         /// <param name="length">Length of the field (number of grid nodes).</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public UnstructuredMesh3d(int length, string fieldName, string fieldDescription) :
            base(length, fieldName, fieldDescription)
        { }


        /// <summary>Constructs a (unconnected) unstructured grid based on 3D regular grid with grid direction parallel to coordinate axes, with
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
        public UnstructuredMesh3d(int dimx, int dimy, int dimz,
            string fieldName, string fieldDescription,
            double minx, double maxx,
            double miny, double maxy,
            double minz, double maxz) :
            this(dimx * dimy * dimz, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            Field<vec3>.GenerateCoordinatesPlain(this, dimx, dimy, dimz,
                minx, maxx,
                miny, maxy,
                minz, maxz);
        }


        /// <summary>Constructs an (unconnected) unstructured grid of nodes based on 3D structured 
        /// grid obtained by transformation of co-ordinates of a regular equidistant grid. Array of 
        /// elements is allocated.</summary>
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
        public UnstructuredMesh3d(int dimx, int dimy, int dimz,
                string fieldName, string fieldDescription,
                double minxRef, double maxxRef,
                double minyRef, double maxyRef,
                double minzRef, double maxzRef,
                IFunc3d fx, IFunc3d fy, IFunc3d fz) :
            this(dimx * dimy * dimz, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            Field<vec3>.GenerateCoordinatesPlain(this, dimx, dimy, dimz,
                minxRef, maxxRef,
                minyRef, maxyRef,
                minzRef, maxzRef,
                fx, fy, fz);
        }

        #endregion Construction


        #region Generation

        /// <summary>Generates an (unconnected) unstructured grid of nodes based on a 3D regular grid 
        /// with grid directions parallel to coordinate axes and equidistant nodes in all directions.</summary>
        /// <param name="dimx">First dimension (number of points generated in the first tirection).</param>
        /// <param name="dimy">First dimension (number of points generated in the second tirection).</param>
        /// <param name="dimz">First dimension (number of points generated in the third tirection).</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        /// <param name="minzRef">Minimal value of z coordinate of the generated grid.</param>
        /// <param name="maxzRef">Maximal value of z coordinate of the generated grid.</param>
        public void GenerateCoordinates(
            int dimx, int dimy, int dimz,
            double minxRef, double maxxRef,
            double minyRef, double maxyRef,
            double minzRef, double maxzRef)
        {
            SetLength(dimx * dimy * dimz);
            Field<vec3>.GenerateCoordinatesPlain(this, dimx, dimy, dimz,
                minxRef, maxxRef,
                minyRef, maxyRef,
                minzRef, maxzRef);
        }


        /// <summary>Generates coordinates of an (unconnected) unstructured grid of points based on coordinates 
        /// of a structured mesh for a parametric volume according  to functions specifying the x, y, and z coordinates 
        /// in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by mapping individual coordinates of a regular equidistant grid
        /// from the reference system.</para></summary>
        /// <param name="dimx">First dimension (number of points generated in the first tirection).</param>
        /// <param name="dimy">First dimension (number of points generated in the second tirection).</param>
        /// <param name="dimz">First dimension (number of points generated in the third tirection).</param>
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
        public void GenerateCoordinates(int dimx, int dimy, int dimz, double minxRef, double maxxRef, double minyRef, double maxyRef,
            double minzRef, double maxzRef, IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            SetLength(dimx * dimy * dimz);
            Field<vec3>.GenerateCoordinatesPlain(this, dimx, dimy, dimz, minxRef, maxxRef, minyRef, maxyRef,
                minzRef, maxzRef, fx, fy, fz);
        }


        #endregion Generation


    } // class UnstructuredMesh3d

}


