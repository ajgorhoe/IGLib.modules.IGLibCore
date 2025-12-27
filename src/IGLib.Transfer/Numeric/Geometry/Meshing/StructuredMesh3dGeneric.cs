
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{


    /// <summary>Generic 3D structured mesh with collections of named scalar, vector, tensor and index fields.
    /// <para>Fields are created on demand (lazy evaluation).</para></summary>
    /// <typeparam name="TCoord">Type of coordinates for description of mesh geometry.</typeparam>
    /// <typeparam name="TIndexField">Type of contained index fields.</typeparam>
    /// <typeparam name="TIndex">Type of elements of contained index fields.</typeparam>
    /// <typeparam name="TScalarField">Type of contained scalar fields.</typeparam>
    /// <typeparam name="TScalar">Type of elements of contained scalar fields.</typeparam>
    /// <typeparam name="TVectorField">Type of contained vector fields.</typeparam>
    /// <typeparam name="TVector">Type of elements of contained vector fields.</typeparam>
    /// <typeparam name="TTensorField">Type of contained tensor fields.</typeparam>
    /// <typeparam name="TTensor">Type of elements of contained tensor fields.</typeparam>
    /// $A Igor Jan08 Mar09;
    public class StructuredMesh3d<TCoord, TIndexField, TIndex, TScalarField, TScalar, TVectorField, TVector, TTensorField, TTensor> :
        StructuredMeshGeometry3d<TCoord>
            where TIndexField : StructuredField3d<TIndex>, new()
            where TScalarField : StructuredField3d<TScalar>, new()
            where TVectorField: StructuredField3d<TVector>, new()
            where TTensorField: StructuredField3d<TTensor>, new()
    {

        /// <summary>Constructs a new empty 3D field (no elements contained) with no name and no description.</summary>
        public StructuredMesh3d()
            : this(0, 0, 0, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 3D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field.</param>
        /// <param name="dim3">Third dimension of the field.</param>
        public StructuredMesh3d(int dim1, int dim2, int dim3)
            : this(dim1, dim2, dim3, null /* fieldName */, null /* description */)
        { }

        /// <summary>Constructs a new 3D field with the specified dimensions, name and description.
        /// Table of elements is allocated.
        /// <para>Elements of the field are arranged in a 3D structured grid.</para></summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field.</param>
        /// <param name="dim3">Third dimension of the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        public StructuredMesh3d(int dim1, int dim2, int dim3, string fieldName)
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
        public StructuredMesh3d(int dim1, int dim2, int dim3, string fieldName, string fieldDescription) :
            base(dim1, dim2, dim3, fieldName, fieldDescription)
        { }



        #region IndexFields

        private FieldCollection3d<TIndexField, TIndex> _indexFields;

        /// <summary>Gets the collection of index fields of the current mesh.
        /// <para>Created on first access (lazy evaluation).</para></summary>
        /// <remarks>Property is protected since the collection should not be accessed directly.</remarks>
        protected FieldCollection3d<TIndexField, TIndex> IndexFields
        {
            get
            {
                if (_indexFields == null)
                    _indexFields = new FieldCollection3d<TIndexField, TIndex>();
                return _indexFields;
            }
        }

        private TIndexField _activeIndexField;

        /// <summary>Gets active index field. Has protected setter.
        /// <para>If there is currently no active field then one is created.</para></summary>
        public TIndexField ActiveIndexField
        {
            get
            {
                if (_activeIndexField == null)
                {
                    if (IndexFields.ActiveField == null)
                    {
                        TIndexField @field = IndexFields.CreateField(Dim1, Dim2, Dim3, IndexFields.DefauletFieldName,
                            "Automatically generated default index field on the collection on 3D structured mesh.");
                        IndexFields.AddField(field);
                        IndexFields.ActiveFieldName = field.Name;
                        ActiveIndexField = field;
                    }
                    if (_activeIndexField == null)
                        throw new InvalidOperationException("Can not create and set active index field on the structured 3D mesh.");
                }
                return _activeIndexField;
            }
            protected set { _activeIndexField = value; }
        }

        /// <summary>Gets or sets name of the currently active index field. This determines the currently active index field.</summary>
        public string ActiveIndexFieldName
        {
            get { return IndexFields.ActiveFieldName; }
            set
            {
                _activeIndexField = null;
                IndexFields.ActiveFieldName = value;
            }
        }

        /// <summary>Returns index field with the specified name.</summary>
        /// <param name="name">Name of the index field to be returned.</param>
        public TIndexField GetIndexField(string name)
        {
            return IndexFields[name];
        }

        /// <summary>Returns the index field with the specified index.
        /// <para>Warning: numbering of index fields changes when fields are added or removed, 
        /// therefore this method should only be called within the time (after the index of a 
        /// particular field is known) when fields were not added or removed.</para></summary>
        /// <param name="index">Consecutive index of the index field to be returned.</param>
        public TIndexField GetIndexField(int index)
        {
            return IndexFields[index];
        }

        /// <summary>Gets the number of index fields.</summary>
        public int NumIndexFields
        {
            get
            {
                if (_indexFields == null)
                    return 0;
                return _indexFields.Count;
            }
        }


        /// <summary>Creates and returns a new index field of dimensions that match dimensions of the current structured mesh, 
        /// and with specified name and description.</summary>
        /// <param name="fieldName">Name of the created field (can be null).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        public virtual TIndexField CreateIndexField(string fieldName, string fieldDescription)
        {
            TIndexField ret = new TIndexField();
            ret.SetDimensions(Dim1, Dim2, Dim3);
            if (fieldName != null)
                ret.Name = fieldName;
            if (fieldDescription != null)
                ret.Description = fieldDescription;
            return ret;
        }


        /// <summary>Creates a new index field of dimensions that match dimensions of the current 3D structuredmesh, and specified 
        /// with name and description, and adds it to the current collection.</summary>
        /// <param name="fieldName">Name of the created field (normally it shouldn't be null though this is legal).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        /// <returns>The created field that is added to the collection.</returns>
        public TIndexField AddIndexField(string fieldName, string fieldDescription)
        {
            TIndexField ret = this.CreateIndexField(fieldName, fieldDescription);
            IndexFields.AddField(ret);
            return ret;
        }

        /// <summary>Adds the specified index field to the collection of index fields under the specified name.
        /// <para>If the field already has a name then its name is replaced by the specified name, 
        /// unless the specified name is null.</para></summary>
        /// <param name="field">Field to be added to the collection.</param>
        /// <param name="name">Name under which the field is added to the collection.</param>
        public void AddIndexField(TIndexField field, string name)
        {
            if (field == null)
                throw new ArgumentNullException("field", "Field to be added is not specified (null argument).");
            if (name != null)
                field.Name = name;
            AddIndexField(field);
        }

        /// <summary>Adds the specified index field to the collection of index fields.</summary>
        /// <param name="field">Field to be added.</param>
        public void AddIndexField(TIndexField field)
        {
            if (field == null)
                throw new ArgumentNullException("field", "Field to be added is not specified (null argument).");
            // Check consistancy of dimensions of the added field:
            if (field.Dim1 != this.Dim1 || field.Dim2 != this.Dim2 || field.Dim3!=this.Dim3)
            {
                if (field.Length == 0)
                    field.SetDimensions(this.Dim1, this.Dim2, this.Dim3);
                else
                    throw new ArgumentException("Dimensions of the field to be added (" + field.Dim1 + ", " + field.Dim2 + ", " + field.Dim3
                        + ") do not match dimension of the mesh (" + this.Dim1 + ", " + this.Dim2 + ", " + this.Dim3 + ").");
            }
            _activeIndexField = null; // since this might be dependent on the performed operation
            IndexFields.AddField(field);
        }

        /// <summary>Removed the field with the specified name from the current field collection.</summary>
        /// <param name="fieldName">Name of the field to be removed.</param>
        public void RemoveIndexField(string fieldName)
        {
            _activeIndexField = null;  // since this might be dependent on the performed operation
            IndexFields.RemoveField(fieldName);

        }

        #endregion IndexFields


        #region ScalarFields

        private FieldCollection3d<TScalarField, TScalar> _scalarFields;

        /// <summary>Gets the collection of scalar fields of the current mesh.
        /// <para>Created on first access (lazy evaluation).</para></summary>
        /// <remarks>Property is protected since the collection should not be accessed directly.</remarks>
        protected FieldCollection3d<TScalarField, TScalar> ScalarFields
        {
            get
            {
                if (_scalarFields == null)
                    _scalarFields = new FieldCollection3d<TScalarField, TScalar>();
                return _scalarFields;
            }
        }

        private TScalarField _activeScalarField;

        /// <summary>Gets active scalar field. Has protected setter.
        /// <para>If there is currently no active field then one is created.</para></summary>
        public TScalarField ActiveScalarField
        {
            get
            {
                if (_activeScalarField == null)
                {
                    if (ScalarFields.ActiveField == null)
                    {
                        TScalarField @field = ScalarFields.CreateField(Dim1, Dim2, Dim3, ScalarFields.DefauletFieldName, 
                            "Automatically generated default scalar field on the collection on 3D structured mesh.");
                        ScalarFields.AddField(field);
                        ScalarFields.ActiveFieldName = field.Name;
                        ActiveScalarField = field;
                    }
                    if (_activeScalarField == null)
                        throw new InvalidOperationException("Can not create and set active scalar field on the structured 3D mesh.");
                }
                return _activeScalarField;
            }
            protected set { _activeScalarField = value; }
        }

        /// <summary>Gets or sets name of the currently active scalar field. This determines the currently active scalar field.</summary>
        public string ActiveScalarFieldName
        {
            get { return ScalarFields.ActiveFieldName; }
            set 
            {
                _activeScalarField = null;
                ScalarFields.ActiveFieldName = value;
            }
        }

        /// <summary>Returns scalar field with the specified name.</summary>
        /// <param name="name">Name of the scalar field to be returned.</param>
        public TScalarField GetScalarField(string name)
        {
            return ScalarFields[name];
        }

        /// <summary>Returns the scalar field with the specified index.
        /// <para>Warning: numbering of scalar fields changes when fields are added or removed, 
        /// therefore this method should only be called within the time (after the index of a 
        /// particular field is known) when fields were not added or removed.</para></summary>
        /// <param name="index">Consecutive index of the scalar field to be returned.</param>
        public TScalarField GetScalarField(int index)
        {
            return ScalarFields[index];
        }

        /// <summary>Gets the number of scalar fields.</summary>
        public int NumScalarFields
        {
            get
            {
                if (_scalarFields == null)
                    return 0;
                return _scalarFields.Count;
            }
        }


        /// <summary>Creates and returns a new scalar field of dimensions that match dimensions of the current structured mesh, 
        /// and with specified name and description.</summary>
        /// <param name="fieldName">Name of the created field (can be null).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        public virtual TScalarField CreateScalarField(string fieldName, string fieldDescription)
        {
            TScalarField ret = new TScalarField();
            ret.SetDimensions(Dim1, Dim2, Dim3);
            if (fieldName != null)
                ret.Name = fieldName;
            if (fieldDescription != null)
                ret.Description = fieldDescription;
            return ret;
        }


        /// <summary>Creates a new scalar field of dimensions that match dimensions of the current 3D structuredmesh, and specified 
        /// with name and description, and adds it to the current collection.</summary>
        /// <param name="fieldName">Name of the created field (normally it shouldn't be null though this is legal).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        /// <returns>The created field that is added to the collection.</returns>
        public TScalarField AddScalarField(string fieldName, string fieldDescription)
        {
            TScalarField ret = this.CreateScalarField(fieldName, fieldDescription);
            ScalarFields.AddField(ret);
            return ret;
        }

        /// <summary>Adds the specified scalar field to the collection of scalar fields under the specified name.
        /// <para>If the field already has a name then its name is replaced by the specified name, 
        /// unless the specified name is null.</para></summary>
        /// <param name="field">Field to be added to the collection.</param>
        /// <param name="name">Name under which the field is added to the collection.</param>
        public void AddScalarField(TScalarField field, string name)
        {
            if (field == null)
                throw new ArgumentNullException("field", "Field to be added is not specified (null argument).");
            if (name != null)
                field.Name = name;
            AddScalarField(field);
        }

        /// <summary>Adds the specified scalar field to the collection of scalar fields.</summary>
        /// <param name="field">Field to be added.</param>
        public void AddScalarField(TScalarField field)
        {
            if (field == null)
                throw new ArgumentNullException("field", "Field to be added is not specified (null argument).");
            // Check consistancy of dimensions of the added field:
            if (field.Dim1 != this.Dim1 || field.Dim2 != this.Dim2 || field.Dim3 != this.Dim3)
            {
                if (field.Length == 0)
                    field.SetDimensions(this.Dim1, this.Dim2, this.Dim3);
                else
                    throw new ArgumentException("Dimensions of the field to be added (" + field.Dim1 + ", " + field.Dim2 + ", " + field.Dim3
                        + ") do not match dimension of the mesh (" + this.Dim1 + ", " + this.Dim2 + ", " + this.Dim3 + ").");
            }
            _activeScalarField = null; // since this might be dependent on the performed operation
            ScalarFields.AddField(field);
        }

        /// <summary>Removed the field with the specified name from the current field collection.</summary>
        /// <param name="fieldName">Name of the field to be removed.</param>
        public void RemoveScalarField(string fieldName)
        {
            _activeScalarField = null;  // since this might be dependent on the performed operation
            ScalarFields.RemoveField(fieldName);

        }

        #endregion ScalarFields


        #region VectorFields

        private FieldCollection3d<TVectorField, TVector> _vectorFields;

        /// <summary>Gets the collection of vector fields of the current mesh.
        /// <para>Created on first access (lazy evaluation).</para></summary>
        /// <remarks>Property is protected since the collection should not be accessed directly.</remarks>
        protected FieldCollection3d<TVectorField, TVector> VectorFields
        {
            get
            {
                if (_vectorFields == null)
                    _vectorFields = new FieldCollection3d<TVectorField, TVector>();
                return _vectorFields;
            }
        }

        private TVectorField _activeVectorField;

        /// <summary>Gets active vector field. Has protected setter.
        /// <para>If there is currently no active field then one is created.</para></summary>
        public TVectorField ActiveVectorField
        {
            get
            {
                if (_activeVectorField == null)
                {
                    if (VectorFields.ActiveField == null)
                    {
                        TVectorField @field = VectorFields.CreateField(Dim1, Dim2, Dim3, VectorFields.DefauletFieldName,
                            "Automatically generated default vector field on the collection on 3D structured mesh.");
                        VectorFields.AddField(field);
                        VectorFields.ActiveFieldName = field.Name;
                        ActiveVectorField = field;
                    }
                    if (_activeVectorField == null)
                        throw new InvalidOperationException("Can not create and set active vector field on the structured 3D mesh.");
                }
                return _activeVectorField;
            }
            protected set { _activeVectorField = value; }
        }

        /// <summary>Gets or sets name of the currently active vector field. This determines the currently active vector field.</summary>
        public string ActiveVectorFieldName
        {
            get { return VectorFields.ActiveFieldName; }
            set
            {
                _activeVectorField = null;
                VectorFields.ActiveFieldName = value;
            }
        }

        /// <summary>Returns vector field with the specified name.</summary>
        /// <param name="name">Name of the vector field to be returned.</param>
        public TVectorField GetVectorField(string name)
        {
            return VectorFields[name];
        }

        /// <summary>Returns the vector field with the specified index.
        /// <para>Warning: numbering of vector fields changes when fields are added or removed, 
        /// therefore this method should only be called within the time (after the index of a 
        /// particular field is known) when fields were not added or removed.</para></summary>
        /// <param name="index">Consecutive index of the vector field to be returned.</param>
        public TVectorField GetVectorField(int index)
        {
            return VectorFields[index];
        }

        /// <summary>Gets the number of vector fields.</summary>
        public int NumVectorFields
        {
            get
            {
                if (_vectorFields == null)
                    return 0;
                return _vectorFields.Count;
            }
        }


        /// <summary>Creates and returns a new vector field of dimensions that match dimensions of the current structured mesh, 
        /// and with specified name and description.</summary>
        /// <param name="fieldName">Name of the created field (can be null).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        public virtual TVectorField CreateVectorField(string fieldName, string fieldDescription)
        {
            TVectorField ret = new TVectorField();
            ret.SetDimensions(Dim1, Dim2, Dim3);
            if (fieldName != null)
                ret.Name = fieldName;
            if (fieldDescription != null)
                ret.Description = fieldDescription;
            return ret;
        }


        /// <summary>Creates a new vector field of dimensions that match dimensions of the current 3D structuredmesh, and specified 
        /// with name and description, and adds it to the current collection.</summary>
        /// <param name="fieldName">Name of the created field (normally it shouldn't be null though this is legal).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        /// <returns>The created field that is added to the collection.</returns>
        public TVectorField AddVectorField(string fieldName, string fieldDescription)
        {
            TVectorField ret = this.CreateVectorField(fieldName, fieldDescription);
            VectorFields.AddField(ret);
            return ret;
        }

        /// <summary>Adds the specified vector field to the collection of vector fields under the specified name.
        /// <para>If the field already has a name then its name is replaced by the specified name, 
        /// unless the specified name is null.</para></summary>
        /// <param name="field">Field to be added to the collection.</param>
        /// <param name="name">Name under which the field is added to the collection.</param>
        public void AddVectorField(TVectorField field, string name)
        {
            if (field == null)
                throw new ArgumentNullException("field", "Field to be added is not specified (null argument).");
            if (name != null)
                field.Name = name;
            AddVectorField(field);
        }

        /// <summary>Adds the specified vector field to the collection of vector fields.</summary>
        /// <param name="field">Field to be added.</param>
        public void AddVectorField(TVectorField field)
        {
            if (field == null)
                throw new ArgumentNullException("field", "Field to be added is not specified (null argument).");
            // Check consistancy of dimensions of the added field:
            if (field.Dim1 != this.Dim1 || field.Dim2 != this.Dim2 || field.Dim3 != this.Dim3)
            {
                if (field.Length == 0)
                    field.SetDimensions(this.Dim1, this.Dim2, this.Dim3);
                else
                    throw new ArgumentException("Dimensions of the field to be added (" + field.Dim1 + ", " + field.Dim2 + ", " + field.Dim3
                        + ") do not match dimension of the mesh (" + this.Dim1 + ", " + this.Dim2 + ", " + this.Dim3 + ").");
            }
            _activeVectorField = null; // since this might be dependent on the performed operation
            VectorFields.AddField(field);
        }

        /// <summary>Removed the field with the specified name from the current field collection.</summary>
        /// <param name="fieldName">Name of the field to be removed.</param>
        public void RemoveVectorField(string fieldName)
        {
            _activeVectorField = null;  // since this might be dependent on the performed operation
            VectorFields.RemoveField(fieldName);

        }

        #endregion VectorFields


        #region TensorFields

        private FieldCollection3d<TTensorField, TTensor> _tensorFields;

        /// <summary>Gets the collection of tensor fields of the current mesh.
        /// <para>Created on first access (lazy evaluation).</para></summary>
        /// <remarks>Property is protected since the collection should not be accessed directly.</remarks>
        protected FieldCollection3d<TTensorField, TTensor> TensorFields
        {
            get
            {
                if (_tensorFields == null)
                    _tensorFields = new FieldCollection3d<TTensorField, TTensor>();
                return _tensorFields;
            }
        }

        private TTensorField _activeTensorField;

        /// <summary>Gets active tensor field. Has protected setter.
        /// <para>If there is currently no active field then one is created.</para></summary>
        public TTensorField ActiveTensorField
        {
            get
            {
                if (_activeTensorField == null)
                {
                    if (TensorFields.ActiveField == null)
                    {
                        TTensorField @field = TensorFields.CreateField(Dim1, Dim2, Dim3, TensorFields.DefauletFieldName,
                            "Automatically generated default tensor field on the collection on 3D structured mesh.");
                        TensorFields.AddField(field);
                        TensorFields.ActiveFieldName = field.Name;
                        ActiveTensorField = field;
                    }
                    if (_activeTensorField == null)
                        throw new InvalidOperationException("Can not create and set active tensor field on the structured 3D mesh.");
                }
                return _activeTensorField;
            }
            protected set { _activeTensorField = value; }
        }

        /// <summary>Gets or sets name of the currently active tensor field. This determines the currently active tensor field.</summary>
        public string ActiveTensorFieldName
        {
            get { return TensorFields.ActiveFieldName; }
            set
            {
                _activeTensorField = null;
                TensorFields.ActiveFieldName = value;
            }
        }

        /// <summary>Returns tensor field with the specified name.</summary>
        /// <param name="name">Name of the tensor field to be returned.</param>
        public TTensorField GetTensorField(string name)
        {
            return TensorFields[name];
        }

        /// <summary>Returns the tensor field with the specified index.
        /// <para>Warning: numbering of tensor fields changes when fields are added or removed, 
        /// therefore this method should only be called within the time (after the index of a 
        /// particular field is known) when fields were not added or removed.</para></summary>
        /// <param name="index">Consecutive index of the tensor field to be returned.</param>
        public TTensorField GetTensorField(int index)
        {
            return TensorFields[index];
        }

        /// <summary>Gets the number of tensor fields.</summary>
        public int NumTensorFields
        {
            get
            {
                if (_tensorFields == null)
                    return 0;
                return _tensorFields.Count;
            }
        }


        /// <summary>Creates and returns a new tensor field of dimensions that match dimensions of the current structured mesh, 
        /// and with specified name and description.</summary>
        /// <param name="fieldName">Name of the created field (can be null).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        public virtual TTensorField CreateTensorField(string fieldName, string fieldDescription)
        {
            TTensorField ret = new TTensorField();
            ret.SetDimensions(Dim1, Dim2, Dim3);
            if (fieldName != null)
                ret.Name = fieldName;
            if (fieldDescription != null)
                ret.Description = fieldDescription;
            return ret;
        }


        /// <summary>Creates a new tensor field of dimensions that match dimensions of the current 3D structuredmesh, and specified 
        /// with name and description, and adds it to the current collection.</summary>
        /// <param name="fieldName">Name of the created field (normally it shouldn't be null though this is legal).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        /// <returns>The created field that is added to the collection.</returns>
        public TTensorField AddTensorField(string fieldName, string fieldDescription)
        {
            TTensorField ret = this.CreateTensorField(fieldName, fieldDescription);
            TensorFields.AddField(ret);
            return ret;
        }

        /// <summary>Adds the specified tensor field to the collection of tensor fields under the specified name.
        /// <para>If the field already has a name then its name is replaced by the specified name, 
        /// unless the specified name is null.</para></summary>
        /// <param name="field">Field to be added to the collection.</param>
        /// <param name="name">Name under which the field is added to the collection.</param>
        public void AddTensorField(TTensorField field, string name)
        {
            if (field == null)
                throw new ArgumentNullException("field", "Field to be added is not specified (null argument).");
            if (name != null)
                field.Name = name;
            AddTensorField(field);
        }

        /// <summary>Adds the specified tensor field to the collection of tensor fields.</summary>
        /// <param name="field">Field to be added.</param>
        public void AddTensorField(TTensorField field)
        {
            if (field == null)
                throw new ArgumentNullException("field", "Field to be added is not specified (null argument).");
            // Check consistancy of dimensions of the added field:
            if (field.Dim1 != this.Dim1 || field.Dim2 != this.Dim2 || field.Dim3 != this.Dim3)
            {
                if (field.Length == 0)
                    field.SetDimensions(this.Dim1, this.Dim2, this.Dim3);
                else
                    throw new ArgumentException("Dimensions of the field to be added (" + field.Dim1 + ", " + field.Dim2 + ", " + field.Dim3
                        + ") do not match dimension of the mesh (" + this.Dim1 + ", " + this.Dim2 + ", " + this.Dim3 + ").");
            }
            _activeTensorField = null; // since this might be dependent on the performed operation
            TensorFields.AddField(field);
        }

        /// <summary>Removed the field with the specified name from the current field collection.</summary>
        /// <param name="fieldName">Name of the field to be removed.</param>
        public void RemoveTensorField(string fieldName)
        {
            _activeTensorField = null;  // since this might be dependent on the performed operation
            TensorFields.RemoveField(fieldName);

        }

        #endregion TensorFields


    } // class StructuredMesh3d<...>


}
