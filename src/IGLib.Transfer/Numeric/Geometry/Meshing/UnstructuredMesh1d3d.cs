// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{



    /// <summary>Unstructured mesh in 1D embedded in 3D space.
    /// <para>Used e.g. for representation of curves in 3D.</para>
    /// <para>Type used for co-ordinates is <see cref="vec3"/>.</para>
    /// <para>Contains collections of collections of index, scalar, vector and tensor fields that are created on demand.</para></summary>
    /// $A Igor Jan08 Mar09;
    public class UnstructuredMesh1d3d : UnstructuredMesh1d<vec3,
        Field<int>, int,
        Field<double>, double,
        Field<vec3>, vec3,
        Field<mat3>, mat3>
    {

        #region Construction

        /// <summary>Constructs a new empty 1D (unconnected) unstructured grid (no elements contained) with no name and no description.</summary>
        public UnstructuredMesh1d3d()
            : this(0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 1D (unconnected) unstructured grid with the specified size, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 1D structured grid.</para></summary>
        /// <param name="length">Length of the field (number of grid nodes).</param>
        public UnstructuredMesh1d3d(int length)
            : this(length, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 1D (unconnected) unstructured grid with the specified size, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 1D structured grid.</para></summary>
        /// <param name="length">Length of the field (number of grid nodes).</param>
        /// <param name="fieldName">Name of the field.</param>
        public UnstructuredMesh1d3d(int length, string fieldName)
            : this(length, fieldName, null /* description */)
        { }

        /// <summary>Constructs a new 1D (unconnected) unstructured grid with the specified size, name and description.
        /// Array of elements is allocated.
        /// <para>Elements of the field are arranged in a 1D structured grid.</para></summary>
        /// <param name="length">Length of the field (number of grid nodes).</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public UnstructuredMesh1d3d(int length, string fieldName, string fieldDescription) :
            base(length, fieldName, fieldDescription)
        { }


        /// <summary>Constructs a (unconnected) unstructured grid based on 1D regular grid with grid direction parallel to coordinate axes, with
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
        public UnstructuredMesh1d3d(int dimx, int dimy,
            string fieldName, string fieldDescription,
            double minx, double maxx,
            double miny, double maxy) :
            this(dimx * dimy, fieldName, fieldDescription)
        {
            //// Dimensions, name and description were set by the base constructor, now just set the coordinates:
            //Field<vec3>.GenerateCoordinatesPlain(this, dimx, dimy,
            //    minx, maxx,
            //    miny, maxy);
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            Field<vec3>.GenerateCoordinates1dPlain(this, dimx,
                minx, maxx);
        }


        /// <summary>Constructs an (unconnected) unstructured grid of nodes based on 1D structured 
        /// grid obtained by transformation of co-ordinates of a regular equidistant grid. Array of 
        /// elements is allocated.</summary>
        /// <param name="dimx">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates of the actual grid.</param>
        public UnstructuredMesh1d3d(int dimx,
                string fieldName, string fieldDescription,
                double minxRef, double maxxRef,
                IRealFunction fx, IRealFunction fy, IRealFunction fz) :
            this(dimx, fieldName, fieldDescription)
        {
            // Dimensions, name and description were set by the base constructor, now just set the coordinates:
            Field<vec3>.GenerateCoordinates1dPlain(this, dimx, 
                minxRef, maxxRef,
                fx, fy, fz);
        }

        #endregion Construction


        #region Generation

        /// <summary>Generates an (unconnected) unstructured grid of nodes based on a 1D regular grid 
        /// with grid directions parallel to coordinate axes and equidistant nodes in all directions.</summary>
        /// <param name="dimx">Dimension (number of points) along the only direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        public void GenerateCoordinates(
            int dimx, 
            double minxRef, double maxxRef)
        {
            SetLength(dimx);
            Field<vec3>.GenerateCoordinates1dPlain(this, dimx, 
                minxRef, maxxRef);
        }


        /// <summary>Generates coordinates of an (unconnected) unstructured grid of points based on coordinates 
        /// of a structured mesh for a parametric volume according  to functions specifying the x, y, and z coordinates 
        /// in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by mapping individual coordinates of a regular equidistant grid
        /// from the reference system.</para></summary>
        /// <param name="dimx">Dimention (number of points) in the first direction.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        public void GenerateCoordinates(int dimx, double minxRef, double maxxRef,
            IRealFunction fx, IRealFunction fy, IRealFunction fz)
        {
            SetLength(dimx);
            Field<vec3>.GenerateCoordinates1dPlain(this, dimx, minxRef, maxxRef,
                fx, fy, fz);
        }


        #endregion Generation


    } // class UnstructuredMesh1d




}


