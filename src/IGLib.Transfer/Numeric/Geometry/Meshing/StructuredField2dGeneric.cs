
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{



    /// <summary>Structured mesh geometry in 2 dimensions.</summary>
    /// <typeparam name="TCoord">Type of coordinate vector used by the mesh.</typeparam>
    /// $A Igor Jan08 Mar09 Oct10;
    public class StructuredMeshGeometry2d<TCoord> : StructuredField2d<TCoord>
    {
        /// <summary>Constructs a new empty 2D field (no elements contained) with no name and no description.</summary>
        public StructuredMeshGeometry2d()
            : this(0, 0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field (number of grid nodes in the second direction).</param>
        public StructuredMeshGeometry2d(int dim1, int dim2)
            : this(dim1, dim2, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field (number of grid nodes in the second direction).</param>
        /// <param name="fieldName">Name of the field.</param>
        public StructuredMeshGeometry2d(int dim1, int dim2, string fieldName)
            : this(dim1, dim2, fieldName, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field (number of grid nodes in the second direction).</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public StructuredMeshGeometry2d(int dim1, int dim2, string fieldName, string fieldDescription) :
            base(dim1, dim2, fieldName, fieldDescription)
        { }

        /// <summary>Array of coordinates.</summary>
        public virtual TCoord[] Coordinates
        {
            get { return Values; }
            set { Values = value; }
        }

    }


    /// <summary>Field where field elements are arranged in a 2 dimensional array.
    /// Usually represents a field over a structured 2D mesh.
    /// <para>Elements can be accessed either through a single index running over all elements of the array,
    /// or by 2 indices indexing elements in 2 basic directions of the grid. The first index ("x direction")
    /// ie least significant and the last index is most significant.</para></summary>
    /// <typeparam name="TElement">Type of elements in the field.</typeparam>
    /// <remarks>Indexing of elements of the structured 2D field has changed in 2011. Now the first index 
    /// (in x direction) is least significant and the last one is most significant, opposite to notation in matrices.</remarks>
    /// $A Igor Jan08 Mar09;
    public class StructuredField2d<TElement> : Field<TElement>
    {

        /// <summary>Constructs a new empty 2D field (no elements contained) with no name and no description.</summary>
        public StructuredField2d()
            : this(0, 0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        public StructuredField2d(int dim1, int dim2)
            : this(dim1, dim2, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension.</param>
        /// <param name="dim2">Second dimension.</param>
        /// <param name="fieldName">Name of the field.</param>
        public StructuredField2d(int dim1, int dim2, string fieldName)
            : this(dim1, dim2, fieldName, null /* description */)
        { }

        /// <summary>Constructs a new 2D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 2D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field (number of grid nodes in the second direction).</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public StructuredField2d(int dim1, int dim2, string fieldName, string fieldDescription) :
            base(dim1 * dim2, fieldName, fieldDescription)
        {
            SetDimensions(dim1, dim2);
        }


        /// <summary>Returns element of the field at specified indices.</summary>
        /// <param name="i1">The first index of an element in a 2D array (least significant).</param>
        /// <param name="i2">The second index of an element in a 2D array.</param>
        /// <remarks>Indexing of elements of the structured 2D field has changed in 2011. Now the first index 
        /// (in x direction) is least significant and the last one is most significant, opposite to notation in matrices.</remarks>
        /// $A Igor Jan08 Oct11;         
        public TElement this[int i1, int i2]
        {
            get { return _values[i1 + i2 * _d1]; }
            set { _values[i1 + i2 * _d1] = value; }
        }

        /// <summary>Returns the linear index of the component that is identified by 2 separate indices.
        /// <para>Field elements are stored in one dimensional array but they can be accesses by 2
        /// indices as they were part of 2 dimensional array.</para>
        /// <para>The last index (for "y direction") is most significant and the first one (in "x direction") is least significant.</para></summary>
        /// <param name="i1">The first index of an element in a 2D array (least significant).</param>
        /// <param name="i2">The second index of an element in a 2D array.</param>
        /// <remarks>Indexing of elements of the structured 2D field has changed in 2011. Now the first index 
        /// (in x direction) is least significant and the last one is most signifivant, opposite to notation in matrices.</remarks>
        /// $A Igor Jan08 Oct11; 
        public int GetIndex(int i1, int i2)
        {
            return i1 + i2 * _d1;
        }

        /// <summary>Converts linear (contiguous) index to 2 separate indices for 2 dimensions.
        /// <para>The last index (for "y direction") is most significant and the first one (in "x direction") is least significant.</para></summary>
        /// <param name="index">Overall contiguous index.</param>
        /// <param name="i1">The first index of an element in a 2D array (least significant)</param>
        /// <param name="i2">The second index of an element in a 2D array.</param>
        /// <remarks>Indexing of elements of the structured 2D field has changed in 2011. Now the first index 
        /// (in x direction) is least significant and the last one is most signifivant, opposite to notation in matrices.</remarks>
        /// $A Igor Jan08 Oct11; 
        public void getIndices(int index, out int i1, out int i2)
        {
            i2 = index / _d1;
            i1 = index - i2 * _d1;

        }

        private int _d1, _d2;

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
                    SetDimensions(_d1, _d2);  // this will reallocate array of coordinates
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
                    SetDimensions(_d1, _d2);  // this will reallocate array of coordinates
                }
            }
        }


        /// <summary>Sets all three dimensions of the current 2D structured field.</summary>
        /// <param name="d1">First dimension.</param>
        /// <param name="d2">Second dimension.</param>
        public virtual void SetDimensions(int d1, int d2)
        {
            this._d1 = d1; this._d2 = d2;
            Length = _d1 * _d2;  // this will reallocate array of values
        }


        #region Static2d3d
        // 2D structured fields embedded in 3D...

        /// <summary>Generates coordinates of a 2D structured grid embedded in 3D by mapping coordinates of a reference 
        /// grid by the specified scalar functions that map coordinate vectors from the reference domain to individual
        /// coordinate components in the actual domain.
        /// <para>Target field can be the same as the reference field (in this case coordinates of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a grid, 
        /// not just for transformation of coordinates.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="referenceField">Field containing reference coordinates that will be mapped to actual coordinates.
        /// <para>Dimensions must be set and array of vector  values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetField">Field for which transformed coordinates are generated. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of vector 
        /// values must be allocated before the function is called.</para>
        /// <para>This parameter can be the same field as <paramref name="referenceField"/>.</para></param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates of the actual grid.</param>
        public static void MapCoordinatesPlain(StructuredField2d<vec3> referenceField, StructuredField2d<vec3> targetField,
            IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            Field<vec3>.MapCoordinatesPlain(referenceField, targetField, fx, fy, fz);
        }

        /// <summary>Generates coordinates of a 2D structured grid embedded in 3D by mapping coordinates of a reference 
        /// grid by the specified scalar functions that map coordinate vectors from the reference domain to individual
        /// coordinate components in the actual domain.
        /// <para>Target field can be the same as the reference field (in this case coordinates of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a grid, 
        /// not just for transformation of coordinates.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="referenceField">Field containing reference coordinates that will be mapped to actual coordinates.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
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
        public static void MapCoordinates(StructuredField2d<vec3> referenceField, StructuredField2d<vec3> targetField,
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
            Field<vec3>.MapCoordinatesPlain(referenceField, targetField, fx, fy, fz);
        }

        /// <summary>Generates coordinates of a 2D structured grid embedded in 3D by mapping first two coordinate components of 
        /// reference grid nodes by the specified scalar functions of two variables. This method is specialized for transforming
        /// coordinates of a reference grid that lies in the XY plane.
        /// <para>Target field can be the same as the reference field (in this case coordinates of the reference field are 
        /// overwritten one by one).</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="referenceField">Field containing reference coordinates that will be mapped to actual coordinates.
        /// <para>Dimensions must be set and array of vector  values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetField">Field for which transformed coordinates are generated. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of vector 
        /// values must be allocated before the function is called.</para>
        /// <para>This parameter can be the same field as <paramref name="referenceField"/>.</para></param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates of the actual grid.</param>
        public static void MapCoordinatesReferencePlain(StructuredField2d<vec3> referenceField, StructuredField2d<vec3> targetField,
            IFunc2d fx, IFunc2d fy, IFunc2d fz)
        {
            int dimx = referenceField.Dim1, dimy = referenceField.Dim2;
            vec3 refCoords;
            for (int j = 0; j < dimy; ++j)
                for (int i = 0; i < dimx; ++i)
                {
                    refCoords = referenceField[i, j];
                    targetField[i, j] = new vec3(fx.Value(refCoords.x, refCoords.y),
                        fy.Value(refCoords.x, refCoords.y), fz.Value(refCoords.x, refCoords.y));
                }
        }


        /// <summary>Generates coordinates of a 2D structured grid embedded in 3D by mapping first two coordinate components of 
        /// reference grid nodes by the specified scalar functions of two variables. This method is specialized for transforming
        /// coordinates of a reference grid that lies in the XY plane.
        /// <para>Target field can be the same as the reference field (in this case coordinates of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="referenceField">Field containing reference coordinates that will be mapped to actual coordinates.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
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
        public static void MapCoordinatesReference(StructuredField2d<vec3> referenceField, StructuredField2d<vec3> targetField,
            IFunc2d fx, IFunc2d fy, IFunc2d fz)
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
            MapCoordinatesReferencePlain(referenceField, targetField, fx, fy, fz);
        }

        /// <summary>Generates scalar values for a 2D structured grid embedded in 3D space from the specified  field 
        /// of grid coordinates by a specified scalar function of 3 variables that maps coordinates to scalar values.
        /// <para>Function can be used for any mapping of nodal vector values to nodal scalar values of a 2D grid embedded in 3D, 
        /// not just for mapping of coordinates.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="referenceField">Field containing coordinates that will be mapped to scalar values.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetScalarField">Field in which the mapped scalars are stored. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of 
        /// values must be allocated before the function is called.</para></param>
        /// <param name="scalarMap">Function that maps node coordinates of the reference grid to scalar values of the target grid.</param>
        public static void MapCoordinatesToScalarsPlain(StructuredField2d<vec3> referenceField,
            StructuredField2d<double> targetScalarField, IFunc3d scalarMap)
        {
            Field<vec3>.MapCoordinatesToScalarsPlain(referenceField, targetScalarField, scalarMap);
        }


        /// <summary>Generates scalar values for a 2D structured grid embedded in 3D space from the specified  field 
        /// of grid coordinates by a specified scalar function of 3 variables that maps coordinates to scalar values.
        /// <para>Function can be used for any mapping of nodal vector values to nodal scalar values of a 2D grid embedded in 3D, 
        /// not just for mapping of coordinates.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="referenceField">Field containing coordinates that will be mapped to scalar values.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetScalarField">Field in which the mapped scalars are stored. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of 
        /// values must be allocated before the function is called.</para></param>
        /// <param name="scalarMap">Function that maps node coordinates of the reference grid to scalar values of the target grid.</param>
        public static void MapCoordinatesToScalars(StructuredField2d<vec3> referenceField,
            StructuredField2d<double> targetScalarField, IFunc3d scalarMap)
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
            Field<vec3>.MapCoordinatesToScalarsPlain(referenceField, targetScalarField, scalarMap);
        }


        /// <summary>Generates coordinates of a 2D regular grid embeddes in 3D, lying on the XY plane, with grid 
        /// directions parallel to first two coordinate axes and equidistantly arranged nodes in both directions.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        public static void GenerateCoordinatesPlain(StructuredField2d<vec3> field,
            double minxRef, double maxxRef,
            double minyRef, double maxyRef)
        {
            double dimx = field.Dim1, dimy = field.Dim2;
            double hx = 0, hy = 0, xRef, yRef;
            if (dimx > 1)
                hx = (maxxRef - minxRef) / (double)(dimx - 1);
            if (dimy > 1)
                hy = (maxyRef - minyRef) / (double)(dimy - 1);
            for (int j = 0; j < dimy; ++j)
                for (int i = 0; i < dimx; ++i)
                {
                    xRef = minxRef + i * hx;
                    yRef = minyRef + j * hy;
                    field[i, j] = new vec3(xRef, yRef, 0.0);
                }
        }

        /// <summary>Generates coordinates of a 2D regular grid embeddes in 3D, lying on the XY plane, with grid 
        /// directions parallel to first two coordinate axes and equidistantly arranged nodes in both directions.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        public static void GenerateCoordinates(StructuredField2d<vec3> field,
            double minxRef, double maxxRef, double minyRef, double maxyRef)
        {
            if (field == null)
                throw new ArgumentException("Field for which coordinates should be generated is not specified (null reference).");
            if (field.Dim1 < 1 || field.Dim2 < 1)
            {
                if (field.Dim1 < 1)
                    throw new ArgumentException("The first dimension of the field is less than 1 (" + field.Dim1 + ").");
                else if (field.Dim2 < 1)
                    throw new ArgumentException("The second dimension of the field is less than 1 (" + field.Dim2 + ").");
            }
            GenerateCoordinatesPlain(field, minxRef, maxxRef, minyRef, maxyRef);
        }


        /// <summary>Generates coordinates of a 2D structured grid embedded in 3D by mapping nodes of a regular
        /// 2D structured grid lying on the XY plane with grid directions parallel to the first two coordinate axes. 
        /// <para>First two coordinates of each reference grid node are mapped to the three coordinate components of the 
        /// generated grid.</para> 
        /// <para>The generated grid covers a bounded patch of a parametric surface in 3D specified by the three functions.</para>
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinatesPlain(StructuredField2d<vec3> field,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy, IFunc2d fz)
        {
            GenerateCoordinatesPlain(field, minxRef, maxxRef, minyRef, maxyRef);
            MapCoordinatesReferencePlain(field /* reference field */, field /* target field - the same as reference */,
                fx, fy, fz);
        }

        /// <summary>Generates coordinates of a 2D structured grid embedded in 3D by mapping nodes of a regular
        /// 2D structured grid lying on the XY plane with grid directions parallel to the first two coordinate axes. 
        /// <para>First two coordinates of each reference grid node are mapped to the three coordinate components of the 
        /// generated grid.</para> 
        /// <para>The generated grid covers a bounded patch of a parametric surface in 3D specified by the three functions.</para>
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinates(StructuredField2d<vec3> field,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy, IFunc2d fz)
        {
            GenerateCoordinates(field, minxRef, maxxRef, minyRef, maxyRef);
            MapCoordinatesReference(field /* reference field */, field /* target field - the same as reference */,
                fx, fy, fz);
        }

        #endregion Static2d3d


        #region Static2d
        // 2D structured fields in 2D...

        /// <summary>Generates coordinates of a 2D structured grid by mapping coordinates of a reference 
        /// grid by the specified scalar functions that map coordinate vectors from the reference domain to individual
        /// coordinate components in the actual domain.
        /// <para>Target field can be the same as the reference field (in this case coordinates of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a grid, 
        /// not just for transformation of coordinates.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="referenceField">Field containing reference coordinates that will be mapped to actual coordinates.
        /// <para>Dimensions must be set and array of vector  values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetField">Field for which transformed coordinates are generated. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of vector 
        /// values must be allocated before the function is called.</para>
        /// <para>This parameter can be the same field as <paramref name="referenceField"/>.</para></param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        public static void MapCoordinatesPlain(StructuredField2d<vec2> referenceField, StructuredField2d<vec2> targetField,
            IFunc2d fx, IFunc2d fy)
        {
            Field<vec2>.MapCoordinatesPlain(referenceField, targetField, fx, fy);
        }

        /// <summary>Generates coordinates of a 2D structured grid by mapping coordinates of a reference 
        /// grid by the specified scalar functions that map coordinate vectors from the reference domain to individual
        /// coordinate components in the actual domain.
        /// <para>Target field can be the same as the reference field (in this case coordinates of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a grid, 
        /// not just for transformation of coordinates.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="referenceField">Field containing reference coordinates that will be mapped to actual coordinates.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetField">Field for which coordinates are generated. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of vector 
        /// values must be allocated before the function is called.</para>
        /// <para>This parameter can be the same field as <paramref name="referenceField"/>.</para></param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates of the actual grid.</param>
        public static void MapCoordinates(StructuredField2d<vec2> referenceField, StructuredField2d<vec2> targetField,
            IFunc2d fx, IFunc2d fy)
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
            Field<vec2>.MapCoordinatesPlain(referenceField, targetField, fx, fy);
        }

        /// <summary>Generates scalar values for a 2D structured grid from the specified  field 
        /// of grid coordinates by a specified scalar function of 2 variables that maps coordinates to scalar values.
        /// <para>Function can be used for any mapping of nodal vector values to nodal scalar values of a 2D grid, not 
        /// just for mapping of coordinates.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="referenceField">Field containing coordinates that will be mapped to scalar values.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetScalarField">Field in which the mapped scalars are stored. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of 
        /// values must be allocated before the function is called.</para></param>
        /// <param name="scalarMap">Function that maps node coordinates of the reference grid to scalar values of the target grid.</param>
        public static void MapCoordinatesToScalarsPlain(StructuredField2d<vec2> referenceField,
            StructuredField2d<double> targetScalarField, IFunc2d scalarMap)
        {
            Field<vec2>.MapCoordinatesToScalarsPlain(referenceField, targetScalarField, scalarMap);
        }


        /// <summary>Generates scalar values for a 2D structured grid from the specified  field 
        /// of grid coordinates by a specified scalar function of 2 variables that maps coordinates to scalar values.
        /// <para>Function can be used for any mapping of nodal vector values to nodal scalar values of a 2D grid, not 
        /// just for mapping of coordinates.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="referenceField">Field containing coordinates that will be mapped to scalar values.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetScalarField">Field in which the mapped scalars are stored. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of 
        /// values must be allocated before the function is called.</para></param>
        /// <param name="scalarMap">Function that maps node coordinates of the reference grid to scalar values of the target grid.</param>
        public static void MapCoordinatesToScalars(StructuredField2d<vec2> referenceField,
            StructuredField2d<double> targetScalarField, IFunc2d scalarMap)
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
            Field<vec2>.MapCoordinatesToScalarsPlain(referenceField, targetScalarField, scalarMap);
        }


        /// <summary>Generates coordinates of a 2D regular grid, with grid 
        /// directions parallel to coordinate axes and equidistantly arranged nodes in both directions.
        /// <para>Numbers of nodes in each grid direction are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        public static void GenerateCoordinatesPlain(StructuredField2d<vec2> field,
            double minxRef, double maxxRef,
            double minyRef, double maxyRef)
        {
            double dimx = field.Dim1, dimy = field.Dim2;
            double hx = 0, hy = 0, xRef, yRef;
            if (dimx > 1)
                hx = (maxxRef - minxRef) / (double)(dimx - 1);
            if (dimy > 1)
                hy = (maxyRef - minyRef) / (double)(dimy - 1);
            for (int j = 0; j < dimy; ++j)
                for (int i = 0; i < dimx; ++i)
                {
                    xRef = minxRef + i * hx;
                    yRef = minyRef + j * hy;
                    field[i, j] = new vec2(xRef, yRef);
                }
        }

        /// <summary>Generates coordinates of a 2D regular grid, with grid 
        /// directions parallel to coordinate axes and equidistantly arranged nodes in both directions.
        /// <para>Numbers of nodes in each grid direction are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        public static void GenerateCoordinates(StructuredField2d<vec2> field,
            double minxRef, double maxxRef, double minyRef, double maxyRef)
        {
            if (field == null)
                throw new ArgumentException("Field for which coordinates should be generated is not specified (null reference).");
            if (field.Dim1 < 1 || field.Dim2 < 1)
            {
                if (field.Dim1 < 1)
                    throw new ArgumentException("The first dimension of the field is less than 1 (" + field.Dim1 + ").");
                else if (field.Dim2 < 1)
                    throw new ArgumentException("The second dimension of the field is less than 1 (" + field.Dim2 + ").");
            }
            GenerateCoordinatesPlain(field, minxRef, maxxRef, minyRef, maxyRef);
        }


        /// <summary>Generates coordinates of a 2D structured grid by mapping nodal coordinates of a regular
        /// 2D structured grid with grid directions parallel to coordinate axes. 
        /// <para>Coordinates of each reference grid node are mapped to the two coordinate components of the 
        /// generated grid.</para> 
        /// <para>The generated grid covers a bounded parametric patch in 2D specified by the two functions.</para>
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinatesPlain(StructuredField2d<vec2> field,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy)
        {
            GenerateCoordinatesPlain(field, minxRef, maxxRef, minyRef, maxyRef);
            Field<vec2>.MapCoordinatesPlain(field /* reference field */, field /* target field - the same as reference */,
                fx, fy);
        }

        /// <summary>Generates coordinates of a 2D structured grid by mapping nodal coordinates of a regular
        /// 2D structured grid with grid directions parallel to coordinate axes. 
        /// <para>Coordinates of each reference grid node are mapped to the two coordinate components of the 
        /// generated grid.</para> 
        /// <para>The generated grid covers a bounded parametric patch in 2D specified by the two functions.</para>
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinates(StructuredField2d<vec2> field,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy)
        {
            GenerateCoordinates(field, minxRef, maxxRef, minyRef, maxyRef);
            MapCoordinates(field /* reference field */, field /* target field - the same as reference */,
                fx, fy);
        }



        // vec3 vec3 IFunc3d   IFunc3d IFunc3d

        #endregion Static2d


    }  // class StructuredField2D<TElement>




}


