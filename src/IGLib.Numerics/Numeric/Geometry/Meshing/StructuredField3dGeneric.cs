
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    /// <summary>Structured mesh geometry in 3 dimensions.</summary>
    /// <typeparam name="TCoord">Type of coordinate vector used by the mesh.</typeparam>
    /// $A Igor Jan08 Mar09;
    public class StructuredMeshGeometry3d<TCoord> : StructuredField3d<TCoord>
    {
        /// <summary>Constructs a new empty 3D field (no elements contained) with no name and no description.</summary>
        public StructuredMeshGeometry3d()
            : this(0, 0, 0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 3D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field.</param>
        /// <param name="dim3">Third dimension of the field.</param>
        public StructuredMeshGeometry3d(int dim1, int dim2, int dim3)
            : this(dim1, dim2, dim3, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 3D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field.</param>
        /// <param name="dim3">Third dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        public StructuredMeshGeometry3d(int dim1, int dim2, int dim3, string fieldName)
            : this(dim1, dim2, dim3, fieldName, null /* description */)
        { }

        /// <summary>Constructs a new 3D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field.</param>
        /// <param name="dim3">Third dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public StructuredMeshGeometry3d(int dim1, int dim2, int dim3, string fieldName, string fieldDescription) :
            base(dim1, dim2, dim3, fieldName, fieldDescription)
        { }

        /// <summary>Array of coordinates.</summary>
        public virtual TCoord[] Coordinates
        {
            get { return Values; }
            set { Values = value; }
        }

    }


    /// <summary>Field where field elements are arranged in a 3 dimensional array.
    /// Usually represents a field over a structured 3D mesh.
    /// <para>Elements can be accessed either through a single index running over all elements of the array,
    /// or by 3 indices indexing elements in 3 basic directions of the grid. The first index ("x direction")
    /// ie least significant and the last index is most significant.</para></summary>
    /// <typeparam name="TElement">Type of elements in the field.</typeparam>
    /// <remarks>Indexing of elements of the structured 3D field has changed in 2011. Now the first index 
    /// (in x direction) is least significant and the last one is most signifivant, opposite to notation in matrices.</remarks>
    /// $A Igor Jan08 Mar09;
    public class StructuredField3d<TElement> : Field<TElement>
    {

        /// <summary>Constructs a new empty 3D field (no elements contained) with no name and no description.</summary>
        public StructuredField3d()
            : this(0, 0, 0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 3D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension.</param> <param name="dim2">Second dimension.</param> <param name="dim3">Third dimension.</param> 
        public StructuredField3d(int dim1, int dim2, int dim3)
            : this(dim1, dim2, dim3, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 3D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension.</param> <param name="dim2">Second dimension.</param> <param name="dim3">Third dimension.</param> 
        /// <param name="fieldName">Name of the field.</param>
        public StructuredField3d(int dim1, int dim2, int dim3, string fieldName)
            : this(dim1, dim2, dim3, fieldName, null /* description */)
        { }

        /// <summary>Constructs a new 3D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field (number of grid nodes in the second direction).</param>
        /// <param name="dim3">Third dimension of the field (number of grid nodes in the third direction).</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public StructuredField3d(int dim1, int dim2, int dim3, string fieldName, string fieldDescription) :
            base(dim1 * dim2 * dim3, fieldName, fieldDescription)
        {
            SetDimensions(dim1, dim2, dim3);
        }


        /// <summary>Returns element of the field at specified indices.</summary>
        /// <param name="i1">The first index of an element in a 3D array (least significant).</param>
        /// <param name="i2">The second index of an element in a 3D array.</param>
        /// <param name="i3">The third index of an element in a 3D array (most significant).</param>
        /// <remarks>Indexing of elements of the structured 3D field has changed in 2011. Now the first index 
        /// (in x direction) is least significant and the last one is most signifivant, opposite to notation in matrices.</remarks>
        /// $A Igor Jan08 Oct11;         
        public TElement this[int i1, int i2, int i3]
        {
            get { return _values[i1 + i2 * _d1 + i3 * _d2 * _d1]; }
            set { _values[i1 + i2 * _d1 + i3 * _d2 * _d1] = value; }
        }

        /// <summary>Returns the linear index of the component that is identified by 3 separate indices.
        /// <para>Field elements are stored in one dimensional array but they can be accesses by 3
        /// indices as they were part of 3 dimensional array.</para>
        /// <para>The last index (for "z direction") is most significant and the first one (in "x direction") is least significant.</para></summary>
        /// <param name="i1">The first index of an element in a 3D array (least significant).</param>
        /// <param name="i2">The second index of an element in a 3D array.</param>
        /// <param name="i3">The third index of an element in a 3D array (most significant).</param>
        /// <remarks>Indexing of elements of the structured 3D field has changed in 2011. Now the first index 
        /// (in x direction) is least significant and the last one is most signifivant, opposite to notation in matrices.</remarks>
        /// $A Igor Jan08 Oct11; 
        public int GetIndex(int i1, int i2, int i3)
        {
            return i1 + i2 * _d1 + i3 * _d2 * _d1;
        }

        /// <summary>Converts linear (contiguous) index to 3 separate indices for 3 dimensions.
        /// <para>The last index (for "z direction") is most significant and the first one (in "x direction") is least significant.</para></summary>
        /// <param name="index">Overall contiguous index.</param>
        /// <param name="i1">The first index of an element in a 3D array (least significant)</param>
        /// <param name="i2">The second index of an element in a 3D array.</param>
        /// <param name="i3">The third index of an element in a 3D array (most significant).</param>
        /// <remarks>Indexing of elements of the structured 3D field has changed in 2011. Now the first index 
        /// (in x direction) is least significant and the last one is most signifivant, opposite to notation in matrices.</remarks>
        /// $A Igor Jan08 Oct11; 
        public void getIndices(int index, out int i1, out int i2, out int i3)
        {
            i3 = index / (_d2 * _d1);
            index -= i3 * (_d2 * _d1);
            i2 = index / _d1;
            i1 = index - i2 * _d1;

        }

        private int _d1, _d2, _d3;

        /// <summary>Returns the first dimenson of the structured mesh (number of mesh points 
        /// in the first direction).
        /// <para>Dependencies are handled by a call to the <see cref="SetDimensions"/> method when the property is set.</para></summary>
        public int Dim1
        {
            get { return _d1; }
            set
            {
                if (value != _d1)
                {
                    _d1 = value;
                    SetDimensions(_d1, _d2, _d3);  // this will reallocate array of coordinates
                }
            }
        }

        /// <summary>Returns the second dimenson of the structured mesh (number of mesh points 
        /// in the second direction).
        /// <para>Dependencies are handled by a call to the <see cref="SetDimensions"/> method when the property is set.</para></summary>
        public int Dim2
        {
            get { return _d2; }
            set
            {
                if (value != _d2)
                {
                    _d2 = value;
                    SetDimensions(_d1, _d2, _d3);  // this will reallocate array of coordinates
                }
            }
        }

        /// <summary>Gets or sets the second dimenson of the structured mesh (number of mesh points 
        /// in the second direction).
        /// <para>Dependencies are handled by a call to the <see cref="SetDimensions"/> method when the property is set.</para></summary>
        public int Dim3
        {
            get { return _d3; }
            set
            {
                if (value != _d3)
                {
                    Length = _d1 * _d2 * _d3;
                    _d3 = value;
                    SetDimensions(_d1, _d2, _d3);  // this will reallocate array of coordinates
                }
            }
        }

        /// <summary>Sets all three dimensions of the current 3D structured field.</summary>
        /// <param name="d1">First dimension.</param>
        /// <param name="d2">Second dimension.</param>
        /// <param name="d3">Third dimension.</param>
        public virtual void SetDimensions(int d1, int d2, int d3)
        {
            this._d1 = d1; this._d2 = d2; this._d3 = d3;
            Length = _d1 * _d2 * _d3;  // this will reallocate array of values
        }

        
        #region Static3d
        // 3D structured fields, vectors and coordinates represented by the vec3 struct...

        /// <summary>Generates coordinates of a 3D structured grid for a parametric volume from a reference grid according 
        /// to functions specifying x, y, and z coordinates in terms of three scalar functions of 3 variables that map 
        /// reference coordinates to components of the mapped coordinates of the grid.
        /// <para>Target field can be the same as the reference field (in this case coordinates of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a grid, 
        /// not just for transformation of coordinates.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="referenceField">Field containing reference coordinates that will be mapped to actual coordinates.
        /// <para>Dimensions must be set and array of vector  values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetField">Field for which coordinates are generated. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of vector 
        /// values must be allocated before the function is called.</para>
        /// <para>This parameter can be the same field as <paramref name="referenceField"/>.</para></param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates of the actual grid.</param>
        public static void MapCoordinatesPlain(StructuredField3d<vec3> referenceField, StructuredField3d<vec3> targetField,
            IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            Field<vec3>.MapCoordinatesPlain(referenceField, targetField, fx, fy, fz);
            //int dimx = referenceField.Dim1, dimy = referenceField.Dim2, dimz = referenceField.Dim3;
            //vec3 refCoords;
            //for (int k = 0; k < dimz; ++k)
            //    for (int j = 0; j < dimy; ++j)
            //        for (int i = 0; i < dimx; ++i)
            //        {
            //            refCoords = referenceField[i, j, k];
            //            targetField[i, j, k] = new vec3(fx.Value(refCoords),
            //                fy.Value(refCoords), fz.Value(refCoords));
            //        }
        }


        /// <summary>Generates coordinates of a 3D structured grid for a parametric volume from a reference grid according 
        /// to functions specifying x, y, and z coordinates in terms of three scalar functions of 3 variables that map 
        /// reference coordinates to components of the mapped coordinates of the grid.
        /// <para>Target field can be the same as the reference field (in this case coordinates of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a grid, 
        /// not just for transformation of coordinates.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="referenceField">Field containing reference coordinates that will be mapped to actual coordinates.
        /// <para>Dimensions must be set and array of vector  values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetField">Field for which coordinates are generated. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of vector 
        /// values must be allocated before the function is called.</para>
        /// <para>This parameter can be the same field as <paramref name="referenceField"/>.</para></param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates of the actual grid.</param>
        public static void MapCoordinates(StructuredField3d<vec3> referenceField, StructuredField3d<vec3> targetField,
            IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            if (referenceField == null)
                throw new ArgumentException("Reference field is not specified (null reference).");
            if (targetField == null)
                throw new ArgumentException("Target field is not specified (null reference).");
            if (targetField.Dim1 != referenceField.Dim1)
                throw new ArgumentException("The first dimension of the target field (" + targetField.Dim1 +
                    ") does not match the first dimension of the reference field (" + referenceField.Dim1 + ").");
            if (targetField.Dim2 != referenceField.Dim2)
                throw new ArgumentException("The second dimension of the target field (" + targetField.Dim2 +
                    ") does not match the second dimension of the reference field (" + referenceField.Dim2 + ").");
            if (targetField.Dim3 != referenceField.Dim3)
                throw new ArgumentException("The third dimension of the target field (" + targetField.Dim3 +
                    ") does not match the first dimension of the reference field (" + referenceField.Dim3 + ").");
            Field<vec3>.MapCoordinatesPlain(referenceField, targetField, fx, fy, fz);
        }


        /// <summary>Generates scalar values for a 3D structured grid from the specified field of grid coordinates by
        /// a specified scalar function of 3 variables that maps reference coordinates to scalar values.
        /// <para>Function can be used for any mapping of nodal vector values to nodal scalar values of a grid, 
        /// not just for mapping of coordinates.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="referenceField">Field containing coordinates that will be mapped to scalar values.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetScalarField">Field in which the mapped scalars are stored. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of 
        /// values must be allocated before the function is called.</para></param>
        /// <param name="scalarMap">Function that maps node coordinates of the reference grid to scalar values of the target grid.</param>
        public static void MapCoordinatesToScalarsPlain(StructuredField3d<vec3> referenceField,
            StructuredField3d<double> targetScalarField, IFunc3d scalarMap)
        {
            Field<vec3>.MapCoordinatesToScalars(referenceField, targetScalarField, scalarMap);
            //int dimx = referenceField.Dim1, dimy = referenceField.Dim2, dimz = referenceField.Dim3;
            //vec3 refCoords;
            //for (int k = 0; k < dimz; ++k)
            //    for (int j = 0; j < dimy; ++j)
            //        for (int i = 0; i < dimx; ++i)
            //        {
            //            refCoords = referenceField[i, j, k];
            //            targetScalarField[i, j, k] = scalarMap.Value(refCoords);
            //        }
        }


        /// <summary>Generates scalar values for a 3D structured grid from the specified field of grid coordinates by
        /// a specified scalar function of 3 variables that maps reference coordinates to scalar values.
        /// <para>Function can be used for any mapping of nodal vector values to nodal scalar values of a grid, 
        /// not just for mapping of coordinates.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="referenceField">Field containing coordinates that will be mapped to scalar values.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetScalarField">Field in which the mapped scalars are stored. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of 
        /// values must be allocated before the function is called.</para></param>
        /// <param name="scalarMap">Function that maps node coordinates of the reference grid to scalar values of the target grid.</param>
        public static void MapCoordinatesToScalars(StructuredField3d<vec3> referenceField,
            StructuredField3d<double> targetScalarField, IFunc3d scalarMap)
        {
            if (referenceField == null)
                throw new ArgumentException("Reference field is not specified (null reference).");
            if (targetScalarField == null)
                throw new ArgumentException("Target field is not specified (null reference).");
            if (targetScalarField.Dim1 != referenceField.Dim1)
                throw new ArgumentException("The first dimension of the target field (" + targetScalarField.Dim1 +
                    ") does not match the first dimension of the reference field (" + referenceField.Dim1 + ").");
            if (targetScalarField.Dim2 != referenceField.Dim2)
                throw new ArgumentException("The second dimension of the target field (" + targetScalarField.Dim2 +
                    ") does not match the second dimension of the reference field (" + referenceField.Dim2 + ").");
            if (targetScalarField.Dim3 != referenceField.Dim3)
                throw new ArgumentException("The third dimension of the target field (" + targetScalarField.Dim3 +
                    ") does not match the first dimension of the reference field (" + referenceField.Dim3 + ").");
            Field<vec3>.MapCoordinatesToScalars(referenceField, targetScalarField, scalarMap);

        }



        /// <summary>Generates coordinates (unconnected) unstructured grid based on a 3D regular grid with grid directions parallel to coordinate axes
        /// and equidistantly arranged nodes in all directions.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        /// <param name="minzRef">Minimal value of z coordinate of the generated grid.</param>
        /// <param name="maxzRef">Maximal value of z coordinate of the generated grid.</param>
        public static void GenerateCoordinatesPlain(StructuredField3d<vec3> field,
            double minxRef, double maxxRef,
            double minyRef, double maxyRef,
            double minzRef, double maxzRef)
        {
            double dimx = field.Dim1, dimy = field.Dim2, dimz = field.Dim3;
            double hx = 0, hy = 0, hz = 0, xRef, yRef, zRef;
            if (dimx > 1)
                hx = (maxxRef - minxRef) / (double)(dimx - 1);
            if (dimy > 1)
                hy = (maxyRef - minyRef) / (double)(dimy - 1);
            if (dimz > 1)
                hz = (maxzRef - minzRef) / (double)(dimz - 1);
            for (int k = 0; k < dimz; ++k)
                for (int j = 0; j < dimy; ++j)
                    for (int i = 0; i < dimx; ++i)
                    {
                        xRef = minxRef + i * hx;
                        yRef = minyRef + j * hy;
                        zRef = minzRef + k * hz;
                        field[i, j, k] = new vec3(xRef, yRef, zRef);
                    }
        }



        /// <summary>Generates coordinates of unstructured grid based on a 3D regular grid with grid directions parallel to coordinate axes
        /// and equidistantly arranged nodes in all directions.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        /// <param name="minzRef">Minimal value of z coordinate of the generated grid.</param>
        /// <param name="maxzRef">Maximal value of z coordinate of the generated grid.</param>
        public static void GenerateCoordinates(StructuredField3d<vec3> field,
            double minxRef, double maxxRef, double minyRef, double maxyRef, double minzRef, double maxzRef)
        {
            if (field == null)
                throw new ArgumentException("Field for which coordinates should be generated is not specified (null reference).");
            if (field.Dim1 < 1 || field.Dim2 < 1 || field.Dim3 < 1)
            {
                if (field.Dim1 < 1)
                    throw new ArgumentException("The first dimension of the field is less than 1 (" + field.Dim1 + ").");
                else if (field.Dim2 < 1)
                    throw new ArgumentException("The second dimension of the field is less than 1 (" + field.Dim2 + ").");
                else
                    throw new ArgumentException("The third dimension of the field is less than 1 (" + field.Dim3 + ").");
            }
            GenerateCoordinatesPlain(field, minxRef, maxxRef, minyRef, maxyRef, minzRef, maxzRef);
        }



        /// <summary>Generates coordinates of unstructured grid based on a 3D structured grid of a parametric volume according 
        /// to functions specifying the x, y, and z coordinates in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="minzRef">Lower bound for the third parameter in the reference coordinate system.</param>
        /// <param name="maxzRef">Upper bound for the third parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinatesPlain(StructuredField3d<vec3> field,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            double minzRef, double maxzRef, IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            GenerateCoordinatesPlain(field, minxRef, maxxRef, minyRef, maxyRef, minzRef, maxzRef);
            Field<vec3>.MapCoordinatesPlain(field /* reference field */, field /* target field - the same as reference */,
                fx, fy, fz);
        }


        /// <summary>Generates coordinates of a 3D structured grid of a parametric volume according 
        /// to functions specifying the x, y, and z coordinates in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="minzRef">Lower bound for the third parameter in the reference coordinate system.</param>
        /// <param name="maxzRef">Upper bound for the third parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinates(StructuredField3d<vec3> field,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            double minzRef, double maxzRef, IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            GenerateCoordinates(field, minxRef, maxxRef, minyRef, maxyRef, minzRef, maxzRef);
            MapCoordinates(field /* reference field */, field /* target field - the same as reference */,
                fx, fy, fz);
        }


        #endregion Static3d



    }  // class StructuredField3D<TElement>




}