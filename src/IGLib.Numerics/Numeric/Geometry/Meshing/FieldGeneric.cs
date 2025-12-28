
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{
    /// <summary>A generic field of values of any type.
    /// Values must be allocated at once.</summary>
    /// <typeparam name="TElement">Type of elements of the field.</typeparam>
    /// $A Igor Jan08 Mar09;
    public class Field<TElement>
    {

        /// <summary>Constructs a new empty field (no elements contained) with no name and no description.</summary>
        public Field()
            : this(0, null, null)
        { }

        /// <summary>Creates a new field with the specified number of elements.</summary>
        /// <param name="numElements">Number of elements of teh field.</param>
        public Field(int numElements)
            : this(numElements, null /* fieldName */, null /* fieldName */)
        { }

        /// <summary>Constructs a new field with the specified number of elements and name.</summary>
        /// <param name="numElements">Number of elements.</param>
        /// <param name="fieldName">Name of the field.</param>
        public Field(int numElements, string fieldName)
            : this(numElements, fieldName, null)
        { }


        /// <summary>Constructs a new field with the specified number of elements, name and description.
        /// Table of elements is allocated.</summary>
        /// <param name="numElements">Number of elements.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public Field(int numElements, string fieldName, string fieldDescription)
        {
            this.Values = new TElement[numElements];
            this.Name = fieldName;
            this.Description = fieldDescription;
        }

        private string _name;

        /// <summary>Name of the field.</summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        private string _description;

        /// <summary>Description of the field.</summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        
        protected TElement[] _values;

        /// <summary>Array of field values.</summary>
        public virtual TElement[] Values
        {
            get { return _values; }
            protected set 
            { 
                _values = value;
                if (value!=null)
                    _length = value.Length;
                else
                    _length = 0;
            }
        }

        private int _length;

        /// <summary>Number of elements.</summary>
        public int Length
        {
            get { return _length; }
            protected set {
                _length = value;
                if (value != Values.Length)
                {
                    if (value<0)
                        throw new ArgumentException("Number of field elements can not be less than 0.");
                    _values = new TElement[value];
                }
            }
        }

        /// <summary>Sets the length of the current field to the specified value.</summary>
        /// <param name="newLength">New length (number of elements) of the field.</param>
        public virtual void SetLength(int newLength)
        {
            this.Length = newLength;
        }

        /// <summary>Access to elements through element index.</summary>
        /// <param name="index">Index running from 0 to <see cref="Length"/></param>
        public TElement this[int index]
        {
            get { return _values[index]; }
            set { _values[index] = value; }
        }

        /// <summary>Returns element of the field at the specified index.</summary>
        /// <param name="index">Index for which field element is returned.</param>
        public TElement GetValue(int index)
        {
            return _values[index];
        }

        /// <summary>Sets the element at the specified index to the specified value.</summary>
        /// <param name="index">Indec of the element to be set.</param>
        /// <param name="value">Value that is assigned to the specified element.</param>
        public void SetValue(int index, TElement value)
        {
            _values[index] = value;
        }

        #region Static


        /// <summary>Updates the specified component of the specified bounds (object of type <see cref="IBoundingBox"/>) in such a way that
        /// all elements of the specified field will fit within the bounds.
        /// <para>Call has no effect if the field is null. Bounding box object must be allocated and of the dimension
        /// greater that the specified coordinate index.</para></summary>
        /// <param name="field">Field according to whose elements the bounds are updated.</param>
        /// <param name="bounds">Bounding box object that is updated in such a way that all elements of the field fit in it.</param>
        /// <param name="componentIndex">Index of component of bounding box that is updated.</param>
        public static void UpdateBounds(Field<double> field, IBoundingBox bounds, int componentIndex)
        {
            if (field != null)
            {
                if (bounds == null)
                    throw new ArgumentNullException("bounds", "Can not update bounds, bounding box object not specified (null reference).");
                double dimension = bounds.Dimension;
                if (dimension <= componentIndex)
                    throw new ArgumentException("Dimension of the specified bounding box (" + dimension 
                        + ") is not greater than index of component to be updated (" + componentIndex + ").");
                for (int i = 0; i < field.Length; ++i)
                {
                    double element = field[i];
                    bounds.Update(componentIndex, element);
                }
            }
        }


        /// <summary>Updates the specified bounds (object of type <see cref="IBoundingBox"/>) in such a way that
        /// all non-null vectors on the specified field will fit within the bounds.
        /// <para>Call has no effect if the field is null. Bounding box object must be allocated and of the same dimension
        /// as elements of the field.</para></summary>
        /// <typeparam name="TEl">Type of elements of the field, must be <see cref="IVector"/>.</typeparam>
        /// <param name="field">Field according to whose elements the bounds are updated.</param>
        /// <param name="bounds">Bounding box object that is updated in such a way that all elements of the field fit in it.</param>
        public static void UpdateBounds<TEl>(Field<TEl> field, IBoundingBox bounds)
            where TEl : IVector
        {
            if (field != null)
            {
                if (bounds == null)
                    throw new ArgumentNullException("bounds", "Can not update bounds, bounding box object not specified (null reference).");
                double dimension = bounds.Dimension;
                for (int i = 0; i < field.Length; ++i)
                {
                    TEl element = field[i];
                    if (element != null)
                    {
                        if (element.Length != dimension)
                            throw new InvalidOperationException("Incompatible dimension of vector element " + i + "when updating bounds: "
                                + element.Length + " instead of " + dimension + ".");
                        bounds.Update(element);
                    }
                }
            }
        }

        #endregion Static

        #region Static3d
        // 3D fields where vectors and coordinates represented by the vec3 struct...

        /// <summary>Updates the specified bounds (object of type <see cref="IBoundingBox"/>) in such a way that
        /// all 3D vector elements of type <see cref="vec3"/> of the specified field will fit within the bounds.
        /// <para>Call has no effect if the field is null. Bounding box object must be allocated and of dimension 3.</para></summary>
        /// <param name="field">Field according to whose 3D vector elements the bounds are updated.</param>
        /// <param name="bounds">Bounding box object that is updated in such a way that all elements of the field fit in it.
        /// Must be of dimension 3.</param>
        public static void UpdateBounds(Field<vec3> field, IBoundingBox bounds)
        {
            if (field != null)
            {
                if (bounds == null)
                    throw new ArgumentNullException("bounds", "Can not update bounds, bounding box object not specified (null reference).");
                double dimension = bounds.Dimension;
                if (dimension != 3)
                    throw new ArgumentException("Dimension of the bounding box should be 3 for updating it with 3D vector field. Actual dimension: " + dimension);
                for (int i = 0; i < field.Length; ++i)
                {
                    vec3 element = field[i];
                    bounds.Update(element.x, element.y, element.z);
                }
            }
        }


        /// <summary>Generates coordinates of a set of unstructured 3D points (point cloud) from a reference set according 
        /// to 3 scalar functions of 3 variables that map coordinates of the reference points to components of the 
        /// mapped points.
        /// <para>Target field can be the same as the reference field (in this case vectors of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a set of nodes, 
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
        public static void MapCoordinatesPlain(Field<vec3> referenceField, Field<vec3> targetField,
            IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            int length = referenceField.Length;
            vec3 refCoords;
            for (int i = 0; i < length; ++i)
            {
                refCoords = referenceField[i];
                targetField[i] = new vec3(fx.Value(refCoords),
                    fy.Value(refCoords), fz.Value(refCoords));
            }
        }


        /// <summary>Generates coordinates of a set of unstructured points (point cloud) from a reference set according 
        /// to 3 scalar functions of 3 variables that map coordinates of the reference points to components of the
        /// mapped points.
        /// <para>Target field can be the same as the reference field (in this case vectors of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a set of nodes, 
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
        public static void MapCoordinates(Field<vec3> referenceField, Field<vec3> targetField,
            IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            if (referenceField == null)
                throw new ArgumentException("Reference field is not specified (null reference).");
            if (targetField == null)
                throw new ArgumentException("Target field is not specified (null reference).");
            if (targetField.Length != referenceField.Length)
                throw new ArgumentException("The length of the target field (" + targetField.Length +
                    ") does not match the length of the reference field (" + referenceField.Length + ").");
            MapCoordinatesPlain(referenceField, targetField, fx, fy, fz);
        }

        /// <summary>Generates coordinates of a set of unstructured 3D points (point cloud) from a reference set according 
        /// to 3 scalar functions of 2 variables that map coordinates of the reference points to components of the 
        /// mapped points. Mapping functions act only on the first two coordinates of each point.
        /// <para>This function is usually used for mapping coordinates of 2D structures embedded in 3D space.</para>
        /// <para>Target field can be the same as the reference field (in this case vectors of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a set of nodes, 
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
        public static void MapCoordinates2dPlain(Field<vec3> referenceField, Field<vec3> targetField,
            IFunc2d fx, IFunc2d fy, IFunc2d fz)
        {
            int length = referenceField.Length;
            vec3 refCoords;
            for (int i = 0; i < length; ++i)
            {
                refCoords = referenceField[i];
                targetField[i] = new vec3(fx.Value(refCoords[0], refCoords[1]),
                    fy.Value(refCoords[0], refCoords[1]), fz.Value(refCoords[0], refCoords[1]));
            }
        }


        /// <summary>Generates coordinates of a set of unstructured points (point cloud) from a reference set according 
        /// to 3 scalar functions of 2 variables that map the first two coordinates of the reference points to components of the
        /// mapped points. 
        /// <para>This function is usually used for mapping coordinates of 2D structures embedded in 3D space.</para>
        /// <para>Target field can be the same as the reference field (in this case vectors of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a set of nodes, 
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
        public static void MapCoordinates2d(Field<vec3> referenceField, Field<vec3> targetField,
            IFunc2d fx, IFunc2d fy, IFunc2d fz)
        {
            if (referenceField == null)
                throw new ArgumentException("Reference field is not specified (null reference).");
            if (targetField == null)
                throw new ArgumentException("Target field is not specified (null reference).");
            if (targetField.Length != referenceField.Length)
                throw new ArgumentException("The length of the target field (" + targetField.Length +
                    ") does not match the length of the reference field (" + referenceField.Length + ").");
            MapCoordinates2dPlain(referenceField, targetField, fx, fy, fz);
        }

        /// <summary>Generates coordinates of a set of unstructured 3D points (point cloud) from a reference set according 
        /// to 3 functions of 1 variable that map the first coordinate of the reference points to components of the 
        /// mapped points.
        /// <para>This function is usually used for mapping coordinates of 1D structures embedded in 3D space.</para>
        /// <para>Target field can be the same as the reference field (in this case vectors of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a set of nodes, 
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
        public static void MapCoordinates1dPlain(Field<vec3> referenceField, Field<vec3> targetField,
            IRealFunction fx, IRealFunction fy, IRealFunction fz)
        {
            int length = referenceField.Length;
            vec3 refCoords;
            for (int i = 0; i < length; ++i)
            {
                refCoords = referenceField[i];
                targetField[i] = new vec3(fx.Value(refCoords.x),
                    fy.Value(refCoords.x), fz.Value(refCoords.x));
            }
        }


        /// <summary>Generates coordinates of a set of unstructured 3D points (point cloud) from a reference set according 
        /// to 3 functions of 1 variable that map the first coordinate of the reference points to components of the 
        /// mapped points.
        /// <para>This function is usually used for mapping coordinates of 1D structures embedded in 3D space.</para>
        /// <para>Target field can be the same as the reference field (in this case vectors of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a set of nodes, 
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
        public static void MapCoordinates1d(Field<vec3> referenceField, Field<vec3> targetField,
            IRealFunction fx, IRealFunction fy, IRealFunction fz)
        {
            if (referenceField == null)
                throw new ArgumentException("Reference field is not specified (null reference).");
            if (targetField == null)
                throw new ArgumentException("Target field is not specified (null reference).");
            if (targetField.Length != referenceField.Length)
                throw new ArgumentException("The length of the target field (" + targetField.Length +
                    ") does not match the length of the reference field (" + referenceField.Length + ").");
            MapCoordinates1dPlain(referenceField, targetField, fx, fy, fz);
        }


        /// <summary>Generates scalar values for a 3D unstructured set fo points from the specified field of grid coordinates by
        /// a specified scalar function of 3 variables that maps coordinates to scalar values.
        /// <para>Function can be used for any mapping of nodal vector values to nodal scalar values of an unstructured grid, 
        /// not just for mapping of coordinates.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="referenceField">Field containing coordinates that will be mapped to scalar values.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetScalarField">Field in which the mapped scalars are stored. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of 
        /// values must be allocated before the function is called.</para></param>
        /// <param name="scalarMap">Function that maps node coordinates of the reference grid to scalar values of the target grid.</param>
        public static void MapCoordinatesToScalarsPlain(Field<vec3> referenceField,
            Field<double> targetScalarField, IFunc3d scalarMap)
        {
            int length = referenceField.Length;
            vec3 refCoords;
            for (int i = 0; i < length; ++i)
            {
                refCoords = referenceField[i];
                targetScalarField[i] = scalarMap.Value(refCoords);
            }
        }


        /// <summary>Generates scalar values for a 3D unstructured set fo points from the specified field of grid coordinates by
        /// a specified scalar function of 3 variables that maps coordinates to scalar values.
        /// <para>Function can be used for any mapping of nodal vector values to nodal scalar values of an unstructured grid, 
        /// not just for mapping of coordinates.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="referenceField">Field containing coordinates that will be mapped to scalar values.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetScalarField">Field in which the mapped scalars are stored. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of 
        /// values must be allocated before the function is called.</para></param>
        /// <param name="scalarMap">Function that maps node coordinates of the reference grid to scalar values of the target grid.</param>
        public static void MapCoordinatesToScalars(Field<vec3> referenceField,
            Field<double> targetScalarField, IFunc3d scalarMap)
        {
            if (referenceField == null)
                throw new ArgumentException("Reference field is not specified (null reference).");
            if (targetScalarField == null)
                throw new ArgumentException("Target field is not specified (null reference).");
            if (targetScalarField.Length != referenceField.Length)
                throw new ArgumentException("The length of the target field (" + targetScalarField.Length +
                    ") does not match the length of the reference field (" + referenceField.Length + ").");
            MapCoordinatesToScalarsPlain(referenceField, targetScalarField, scalarMap);
        }

        #endregion Static3d

        #region Static3dFromStructured

        /// <summary>Generates coordinates of a 3D unstructured grid from a regular grid with grid directions parallel to coordinate axes
        /// and equidistantly arranged nodes in all directions.
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
        /// <param name="dimz">Number of nodes in z direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        /// <param name="minzRef">Minimal value of z coordinate of the generated grid.</param>
        /// <param name="maxzRef">Maximal value of z coordinate of the generated grid.</param>
        public static void GenerateCoordinatesPlain(Field<vec3> field,
            int dimx, int dimy, int dimz,
            double minxRef, double maxxRef,
            double minyRef, double maxyRef,
            double minzRef, double maxzRef)
        {
            double hx = 0, hy = 0, hz = 0, xRef, yRef, zRef;
            if (dimx > 1)
                hx = (maxxRef - minxRef) / (double)(dimx - 1);
            if (dimy > 1)
                hy = (maxyRef - minyRef) / (double)(dimy - 1);
            if (dimz > 1)
                hz = (maxzRef - minzRef) / (double)(dimz - 1);
            int index = 0;
            for (int k = 0; k < dimz; ++k)
                for (int j = 0; j < dimy; ++j)
                    for (int i = 0; i < dimx; ++i)
                    {
                        xRef = minxRef + i * hx;
                        yRef = minyRef + j * hy;
                        zRef = minzRef + k * hz;
                        field[index] = new vec3(xRef, yRef, zRef);
                        ++index;
                    }
        }


        /// <summary>Generates coordinates of a 3D unstructured grid from a regular grid with grid directions parallel to coordinate axes
        /// and equidistantly arranged nodes in all directions.
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
        /// <param name="dimz">Number of nodes in z direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        /// <param name="minzRef">Minimal value of z coordinate of the generated grid.</param>
        /// <param name="maxzRef">Maximal value of z coordinate of the generated grid.</param>
        public static void GenerateCoordinates(Field<vec3> field,
            int dimx, int dimy, int dimz,
            double minxRef, double maxxRef, double minyRef, double maxyRef, double minzRef, double maxzRef)
        {
            if (field == null)
                throw new ArgumentException("Field for which coordinates should be generated is not specified (null reference).");
            if (dimx < 1 || dimy < 1 || dimz < 1)
            {
                if (dimx < 1)
                    throw new ArgumentException("The first dimension of the field is less than 1 (" + dimx + ").");
                else if (dimy < 1)
                    throw new ArgumentException("The second dimension of the field is less than 1 (" + dimy + ").");
                else
                    throw new ArgumentException("The third dimension of the field is less than 1 (" + dimz + ").");
            }
            GenerateCoordinatesPlain(field, dimx, dimy, dimz, minxRef, maxxRef, minyRef, maxyRef, minzRef, maxzRef);
        }


        /// <summary>Generates coordinates of a 3D unstructured grid of a parametric volume according 
        /// to functions specifying the x, y, and z coordinates in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
        /// <param name="dimz">Number of nodes in z direction.</param>
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
        public static void GenerateCoordinatesPlain(Field<vec3> field,
            int dimx, int dimy, int dimz,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            double minzRef, double maxzRef, IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            GenerateCoordinatesPlain(field, dimx, dimy, dimz, minxRef, maxxRef, minyRef, maxyRef, minzRef, maxzRef);
            Field<vec3>.MapCoordinatesPlain(field /* reference field */, field /* target field - the same as reference */,
                fx, fy, fz);
        }


        /// <summary>Generates coordinates of a 3D unstructured grid of a parametric volume according 
        /// to functions specifying the x, y, and z coordinates in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
        /// <param name="dimz">Number of nodes in z direction.</param>
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
        public static void GenerateCoordinates(Field<vec3> field,
            int dimx, int dimy, int dimz,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            double minzRef, double maxzRef, IFunc3d fx, IFunc3d fy, IFunc3d fz)
        {
            GenerateCoordinates(field, dimx, dimy, dimz, minxRef, maxxRef, minyRef, maxyRef, minzRef, maxzRef);
            MapCoordinates(field /* reference field */, field /* target field - the same as reference */,
                fx, fy, fz);
        }

        #endregion Static3dFromStructured


        #region Static2d
        // 2D fields where vectors and coordinates represented by the vec2 struct...

        /// <summary>Updates the specified bounds (object of type <see cref="IBoundingBox"/>) in such a way that
        /// all 2D vector elements of type <see cref="vec2"/> of the specified field will fit within the bounds.
        /// <para>Call has no effect if the field is null. Bounding box object must be allocated and of dimension 3.</para></summary>
        /// <param name="field">Field according to whose 2D vector elements the bounds are updated.</param>
        /// <param name="bounds">Bounding box object that is updated in such a way that all elements of the field fit in it.
        /// Must be of dimension 2.</param>
        public static void UpdateBounds(Field<vec2> field, IBoundingBox bounds)
        {
            if (field != null)
            {
                if (bounds == null)
                    throw new ArgumentNullException("bounds", "Can not update bounds, bounding box object not specified (null reference).");
                double dimension = bounds.Dimension;
                if (dimension != 2)
                    throw new ArgumentException("Dimension of the bounding box should be 2 for updating it with 2D vector field. Actual dimension: " + dimension);
                for (int i = 0; i < field.Length; ++i)
                {
                    vec2 element = field[i];
                    bounds.Update(element.x, element.y);
                }
            }
        }

        /// <summary>Generates coordinates of a set of unstructured 3D points (point cloud) from a reference set according 
        /// to 2 scalar functions of 2 variables that map coordinates of the reference points to components of the 
        /// mapped points.
        /// <para>Target field can be the same as the reference field (in this case vectors of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a set of nodes, 
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
        public static void MapCoordinatesPlain(Field<vec2> referenceField, Field<vec2> targetField,
            IFunc2d fx, IFunc2d fy)
        {
            int length = referenceField.Length;
            vec2 refCoords;
            for (int i = 0; i < length; ++i)
            {
                refCoords = referenceField[i];
                targetField[i] = new vec2(fx.Value(refCoords), fy.Value(refCoords));
            }
        }


        /// <summary>Generates coordinates of a set of unstructured points (point cloud) from a reference set according 
        /// to 2 scalar functions of 2 variables that map coordinates of the reference points to components of the
        /// mapped points.
        /// <para>Target field can be the same as the reference field (in this case vectors of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a set of nodes, 
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
        public static void MapCoordinates(Field<vec2> referenceField, Field<vec2> targetField,
            IFunc2d fx, IFunc2d fy)
        {
            if (referenceField == null)
                throw new ArgumentException("Reference field is not specified (null reference).");
            if (targetField == null)
                throw new ArgumentException("Target field is not specified (null reference).");
            if (targetField.Length != referenceField.Length)
                throw new ArgumentException("The length of the target field (" + targetField.Length +
                    ") does not match the length of the reference field (" + referenceField.Length + ").");
            MapCoordinatesPlain(referenceField, targetField, fx, fy);
        }

        /// <summary>Generates coordinates of a set of unstructured 2D points (point cloud) from a reference set according 
        /// to 2 functions of 1 variables that map the first coordinate of the reference points to components of the 
        /// mapped points.
        /// <para>This function is usually used for mapping coordinates of 2D structures embedded in 3D.</para>
        /// <para>Target field can be the same as the reference field (in this case vectors of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a set of nodes, 
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
        public static void MapCoordinates1dPlain(Field<vec2> referenceField, Field<vec2> targetField,
            IRealFunction fx, IRealFunction fy)
        {
            int length = referenceField.Length;
            vec2 refCoords;
            for (int i = 0; i < length; ++i)
            {
                refCoords = referenceField[i];
                targetField[i] = new vec2(fx.Value(refCoords.x),
                    fy.Value(refCoords.x));
            }
        }


        /// <summary>Generates coordinates of a set of unstructured 2D points (point cloud) from a reference set according 
        /// to 2 functions of 1 variables that map the first coordinate of the reference points to components of the 
        /// mapped points.
        /// <para>This function is usually used for mapping coordinates of 2D structures embedded in 3D.</para>
        /// <para>Target field can be the same as the reference field (in this case vectors of the reference field are 
        /// overwritten one by one).</para>
        /// <para>Function can be used for transformation of any nodal vector values of a set of nodes, 
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
        public static void MapCoordinates1d(Field<vec2> referenceField, Field<vec2> targetField,
            IRealFunction fx, IRealFunction fy)
        {
            if (referenceField == null)
                throw new ArgumentException("Reference field is not specified (null reference).");
            if (targetField == null)
                throw new ArgumentException("Target field is not specified (null reference).");
            if (targetField.Length != referenceField.Length)
                throw new ArgumentException("The length of the target field (" + targetField.Length +
                    ") does not match the length of the reference field (" + referenceField.Length + ").");
            MapCoordinates1dPlain(referenceField, targetField, fx, fy);
        }



        /// <summary>Generates scalar values for a 2D unstructured set fo points from the specified field of grid coordinates by
        /// a specified scalar function of 2 variables that maps coordinates to scalar values.
        /// <para>Function can be used for any mapping of nodal vector values to nodal scalar values of an unstructured set of nodes, 
        /// not just for mapping of coordinates.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="referenceField">Field containing coordinates that will be mapped to scalar values.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetScalarField">Field in which the mapped scalars are stored. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of 
        /// values must be allocated before the function is called.</para></param>
        /// <param name="scalarMap">Function that maps node coordinates of the reference grid to scalar values of the target grid.</param>
        public static void MapCoordinatesToScalarsPlain(Field<vec2> referenceField,
            Field<double> targetScalarField, IFunc2d scalarMap)
        {
            int length = referenceField.Length;
            vec2 refCoords;
            for (int i = 0; i < length; ++i)
            {
                refCoords = referenceField[i];
                targetScalarField[i] = scalarMap.Value(refCoords);
            }
        }


        /// <summary>Generates scalar values for a 2D unstructured set fo points from the specified field of grid coordinates by
        /// a specified scalar function of 2 variables that maps coordinates to scalar values.
        /// <para>Function can be used for any mapping of nodal vector values to nodal scalar values of an unstructured set of nodes, 
        /// not just for mapping of coordinates.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="referenceField">Field containing coordinates that will be mapped to scalar values.
        /// <para>Dimensions must be set and array of vector values (coordinates) must be allocated 
        /// before the function is called.</para></param>
        /// <param name="targetScalarField">Field in which the mapped scalars are stored. 
        /// <para>Dimensions must be set and consistent with dimensions of the reference field, and array of 
        /// values must be allocated before the function is called.</para></param>
        /// <param name="scalarMap">Function that maps node coordinates of the reference grid to scalar values of the target grid.</param>
        public static void MapCoordinatesToScalars(Field<vec2> referenceField,
            Field<double> targetScalarField, IFunc2d scalarMap)
        {
            if (referenceField == null)
                throw new ArgumentException("Reference field is not specified (null reference).");
            if (targetScalarField == null)
                throw new ArgumentException("Target field is not specified (null reference).");
            if (targetScalarField.Length != referenceField.Length)
                throw new ArgumentException("The length of the target field (" + targetScalarField.Length +
                    ") does not match the length of the reference field (" + referenceField.Length + ").");
            MapCoordinatesToScalarsPlain(referenceField, targetScalarField, scalarMap);
        }

        #endregion Static2d


        #region Static2dFromStructured

        /// <summary>Generates coordinates of a 3D unstructured grid from a regular grid with grid directions parallel to coordinate axes
        /// and equidistantly arranged nodes in all directions.
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        public static void GenerateCoordinatesPlain(Field<vec2> field,
            int dimx, int dimy,
            double minxRef, double maxxRef,
            double minyRef, double maxyRef)
        {
            double hx = 0, hy = 0, xRef, yRef;
            if (dimx > 1)
                hx = (maxxRef - minxRef) / (double)(dimx - 1);
            if (dimy > 1)
                hy = (maxyRef - minyRef) / (double)(dimy - 1);
            int index = 0;
            for (int j = 0; j < dimy; ++j)
                for (int i = 0; i < dimx; ++i)
                {
                    xRef = minxRef + i * hx;
                    yRef = minyRef + j * hy;
                    field[index] = new vec2(xRef, yRef);
                    ++index;
                }
        }

        /// <summary>Generates coordinates of a 3D unstructured grid from a regular grid with grid directions parallel to coordinate axes
        /// and equidistantly arranged nodes in all directions.
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        public static void GenerateCoordinates(Field<vec2> field,
            int dimx, int dimy,
            double minxRef, double maxxRef, double minyRef, double maxyRef)
        {
            if (field == null)
                throw new ArgumentException("Field for which coordinates should be generated is not specified (null reference).");
            if (dimx < 1 || dimy < 1)
            {
                if (dimx < 1)
                    throw new ArgumentException("The first dimension of the field is less than 1 (" + dimx + ").");
                else if (dimy < 1)
                    throw new ArgumentException("The second dimension of the field is less than 1 (" + dimy + ").");
            }
            GenerateCoordinatesPlain(field, dimx, dimy, minxRef, maxxRef, minyRef, maxyRef);
        }


        /// <summary>Generates coordinates of a 3D unstructured grid of a parametric volume according 
        /// to functions specifying the x, y, and z coordinates in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinatesPlain(Field<vec2> field,
            int dimx, int dimy,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy)
        {
            GenerateCoordinatesPlain(field, dimx, dimy, minxRef, maxxRef, minyRef, maxyRef);
            Field<vec2>.MapCoordinatesPlain(field /* reference field */, field /* target field - the same as reference */,
                fx, fy);
        }

        /// <summary>Generates coordinates of a 3D unstructured grid of a parametric volume according 
        /// to functions specifying the x, y, and z coordinates in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="minyRef">Lower bound for the second parameter in the reference coordinate system.</param>
        /// <param name="maxyRef">Upper bound for the second parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinates(Field<vec2> field,
            int dimx, int dimy,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy)
        {
            GenerateCoordinates(field, dimx, dimy, minxRef, maxxRef, minyRef, maxyRef);
            MapCoordinates(field /* reference field */, field /* target field - the same as reference */,
                fx, fy);
        }

        #endregion Static2dFromStructured

        #region Static2d3dFromStructured

        /// <summary>Generates coordinates of a 2D unstructured grid embedded in 3D from a regular grid 
        /// with grid directions parallel to the first two coordinate axes
        /// and equidistantly arranged nodes in all directions.
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        public static void GenerateCoordinates2dPlain(Field<vec3> field,
            int dimx, int dimy,
            double minxRef, double maxxRef,
            double minyRef, double maxyRef)
        {
            double hx = 0, hy = 0, xRef, yRef;
            if (dimx > 1)
                hx = (maxxRef - minxRef) / (double)(dimx - 1);
            if (dimy > 1)
                hy = (maxyRef - minyRef) / (double)(dimy - 1);
            int index = 0;
            for (int j = 0; j < dimy; ++j)
                for (int i = 0; i < dimx; ++i)
                {
                    xRef = minxRef + i * hx;
                    yRef = minyRef + j * hy;
                    field[index] = new vec3(xRef, yRef, 0);
                    ++index;
                }
        }

        /// <summary>Generates coordinates of a 2D unstructured grid embedded in 3D from a regular grid 
        /// with grid directions parallel to the first two coordinate axes
        /// and equidistantly arranged nodes in all directions.
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        /// <param name="minyRef">Minimal value of y coordinate of the generated grid.</param>
        /// <param name="maxyRef">Maximal value of y coordinate of the generated grid.</param>
        public static void GenerateCoordinates2d(Field<vec3> field,
            int dimx, int dimy,
            double minxRef, double maxxRef, double minyRef, double maxyRef)
        {
            if (field == null)
                throw new ArgumentException("Field for which coordinates should be generated is not specified (null reference).");
            if (dimx < 1 || dimy < 1)
            {
                if (dimx < 1)
                    throw new ArgumentException("The first dimension of the field is less than 1 (" + dimx + ").");
                else if (dimy < 1)
                    throw new ArgumentException("The second dimension of the field is less than 1 (" + dimy + ").");
            }
            GenerateCoordinates2dPlain(field, dimx, dimy, minxRef, maxxRef, minyRef, maxyRef);
        }


        /// <summary>Generates coordinates of a 3D unstructured grid of a parametric volume according 
        /// to functions specifying the x, y, and z coordinates in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
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
        public static void GenerateCoordinates2dPlain(Field<vec3> field,
            int dimx, int dimy,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy, IFunc2d fz)
        {
            GenerateCoordinates2dPlain(field, dimx, dimy, minxRef, maxxRef, minyRef, maxyRef);
            Field<vec3>.MapCoordinates2dPlain(field /* reference field */, field /* target field - the same as reference */,
                fx, fy, fz);
        }

        /// <summary>Generates coordinates of a 3D unstructured grid of a parametric volume according 
        /// to functions specifying the x, y, and z coordinates in terms of three scalar functions of 3 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="dimy">Number of nodes in y direction.</param>
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
        public static void GenerateCoordinates2d(Field<vec3> field,
            int dimx, int dimy,
            double minxRef, double maxxRef, double minyRef, double maxyRef,
            IFunc2d fx, IFunc2d fy, IFunc2d fz)
        {
            GenerateCoordinates2d(field, dimx, dimy, minxRef, maxxRef, minyRef, maxyRef);
            MapCoordinates2d(field /* reference field */, field /* target field - the same as reference */,
                fx, fy, fz);
        }

        #endregion Static2d3dFromStructured


        #region Static1d3dFromStructured

        /// <summary>Generates coordinates of a 1D unstructured grid from a regular grid with grid direction parallel to X coordinate axes
        /// and equidistantly arranged nodes.
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        public static void GenerateCoordinates1dPlain(Field<vec3> field,
            int dimx,
            double minxRef, double maxxRef)
        {
            double hx = 0, xRef;
            if (dimx > 1)
                hx = (maxxRef - minxRef) / (double)(dimx - 1);
            int index = 0;
            for (int i = 0; i < dimx; ++i)
            {
                xRef = minxRef + i * hx;
                field[index] = new vec3(xRef, 0, 0);
                ++index;
            }
        }


        /// <summary>Generates coordinates of a 1D unstructured grid from a regular grid with grid direction parallel to X coordinate axis
        /// and equidistantly arranged nodes.
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        public static void GenerateCoordinates1d(Field<vec3> field,
            int dimx,
            double minxRef, double maxxRef)
        {
            if (field == null)
                throw new ArgumentException("Field for which coordinates should be generated is not specified (null reference).");
            if (dimx < 1)
            {
                if (dimx < 1)
                    throw new ArgumentException("The first dimension of the field is less than 1 (" + dimx + ").");
            }
            if (dimx != field.Length)
                throw new ArgumentException("The specified number of nodes " + dimx + " does not correspond to field length" + field.Length + ".");
            GenerateCoordinates1dPlain(field, dimx, minxRef, maxxRef);
        }


        /// <summary>Generates coordinates of a 1D unstructured grid of a parametric curve according 
        /// to functions specifying the x, y, and z coordinates in terms of three functions of 1 variable.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinates1dPlain(Field<vec3> field,
            int dimx,
            double minxRef, double maxxRef, IRealFunction fx, IRealFunction fy, IRealFunction fz)
        {
            GenerateCoordinates1dPlain(field, dimx, minxRef, maxxRef);
            Field<vec3>.MapCoordinates1dPlain(field /* reference field */, field /* target field - the same as reference */,
                fx, fy, fz);
        }


        /// <summary>Generates coordinates of a 1D unstructured grid of a parametric curve according 
        /// to functions specifying the x, y, and z coordinates in terms of three functions of 1 variables.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fz">Function that maps node coordinates of the reference grid to the third 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinates1d(Field<vec3> field,
            int dimx,
            double minxRef, double maxxRef, IRealFunction fx, IRealFunction fy, IRealFunction fz)
        {
            GenerateCoordinates1d(field, dimx, minxRef, maxxRef);
            MapCoordinates1d(field /* reference field */, field /* target field - the same as reference */,
                fx, fy, fz);
        }

        
        #endregion Static1d3dFromStructured


        #region Static1d2dFromStructured


        /// <summary>Generates coordinates of a 1D unstructured grid embedded in 2D space from a regular grid with grid direction parallel to X coordinate axes
        /// and equidistantly arranged nodes.
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Maximal value of x coordinate of the generated grid.</param>
        public static void GenerateCoordinates1dPlain(Field<vec2> field,
            int dimx,
            double minxRef, double maxxRef)
        {
            double hx = 0, xRef;
            if (dimx > 1)
                hx = (maxxRef - minxRef) / (double)(dimx - 1);
            int index = 0;
            for (int i = 0; i < dimx; ++i)
            {
                xRef = minxRef + i * hx;
                field[index] = new vec2(xRef, 0);
                ++index;
            }
        }


        /// <summary>Generates coordinates of a 1D unstructured grid embedded in 2D space from a regular grid with grid direction parallel to X coordinate axis
        /// and equidistantly arranged nodes.
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="minxRef">Minimal value of x coordinate of the generated grid.</param>
        /// <param name="maxxRef">Minimal value of x coordinate of the generated grid.</param>
        public static void GenerateCoordinates(Field<vec2> field,
            int dimx,
            double minxRef, double maxxRef)
        {
            if (field == null)
                throw new ArgumentException("Field for which coordinates should be generated is not specified (null reference).");
            if (dimx < 1)
            {
                if (dimx < 1)
                    throw new ArgumentException("The first dimension of the field is less than 1 (" + dimx + ").");
            }
            GenerateCoordinates1dPlain(field, dimx, minxRef, maxxRef);
        }


        /// <summary>Generates coordinates of a 1D unstructured grid embedded in 2D space of a parametric curve according 
        /// to functions specifying the x, and y coordinates in terms of 2 functions of 1 variable.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>This is plain version of the method that does not check consistency of dimension or existence of objects.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinates1dPlain(Field<vec2> field,
            int dimx,
            double minxRef, double maxxRef, IRealFunction fx, IRealFunction fy)
        {
            GenerateCoordinates1dPlain(field, dimx, minxRef, maxxRef);
            Field<vec2>.MapCoordinates1dPlain(field /* reference field */, field /* target field - the same as reference */,
                fx, fy);
        }


        /// <summary>Generates coordinates of a 1D unstructured grid embedded in 2D space of a parametric curve according 
        /// to functions specifying the x, and y coordinates in terms of 2 functions of 1 variable.
        /// <para>Coordinates are obtained by transforming individual coordinates of a regular equidistant grid
        /// from the reference system.</para>.
        /// <para>Numbers of nodes in each grid directions are specified by current dimensions of the grid.</para>
        /// <para>Consistency of dimensions and existence of objects are checked and exceptions are thrown when checks fail.</para></summary>
        /// <param name="field">Field for which coordinates are generated. Dimensions must be set
        /// and array of vector values allocated before this function is called.</param>
        /// <param name="dimx">Number of nodes in x direction.</param>
        /// <param name="minxRef">Lower bound for the first parameter in the reference coordinate system.</param>
        /// <param name="maxxRef">Upper bound for the first parameter in the reference coordinate system.</param>
        /// <param name="fx">Function that maps node coordinates of the reference grid to the first 
        /// node coordinates' component of the actual grid.</param>
        /// <param name="fy">Function that maps node coordinates of the reference grid to the second 
        /// node coordinates' component of the actual grid.</param>
        public static void GenerateCoordinates1d(Field<vec2> field,
            int dimx,
            double minxRef, double maxxRef, IRealFunction fx, IRealFunction fy)
        {
            GenerateCoordinates1dPlain(field, dimx, minxRef, maxxRef);
            MapCoordinates1d(field /* reference field */, field /* target field - the same as reference */,
                fx, fy);
        }

        #endregion Static1d2dFromStructured




        #region Exampless

        public static void Example()
        {
            Field<double> ff = new Field<double>(100, "ExampleField");
            ExampleClassScalarField sf = new ExampleClassScalarField(20, "Example field.");
            ff = sf;
        }
        
        /// <summary>Example of a field class.</summary>
        public class ExampleClassScalarField : Field<double>
        {

            public ExampleClassScalarField()
                : this(0, null, null)
            { }

            public ExampleClassScalarField(int numElements, string fieldName)
                : this(numElements, fieldName, null)
            { }

            public ExampleClassScalarField(int numElements, string fieldName, string fieldDescription) :
                base(numElements, fieldName, fieldDescription)
            { }

            /// <summary>Array of field values.</summary>
            public override double[] Values
            {
                get { return _values; }
                protected set
                {
                    _values = value;
                    if (value != null)
                        _length = value.Length;
                    else
                        _length = 0;

                }
            }


        }

        #endregion Examples


    } // class Field<T>



    


}
