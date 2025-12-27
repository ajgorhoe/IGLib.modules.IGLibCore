
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{



    /// <summary>Collection of structured 3D fields of the same type.  Fields are identified by their names.
    /// <para>In principle, contained fields can be of different dimensions.</para></summary>
    /// <typeparam name="TField">Type of fields contained in the collection. Must be a 3D structured field (<see cref="StructuredField3d{TElement}"/>).</typeparam>
    /// <typeparam name="TElement">Type of elements of the fields that can be contained in the collection.</typeparam>
    /// <remarks>Completely generic definition was introduced in 2009.
    /// Before that, several types were used for specific field collections.</remarks>
    /// $A Igor Apr09;
    public class FieldCollection3d<TField, TElement> : FieldCollection<TField, TElement>
        where TField : StructuredField3d<TElement>, new()
    {

            /// <summary>Constructor.</summary>
        public FieldCollection3d()
            : base()
        {  }

        /// <summary>Call to this method is invalid for the current type. Call the method with three dimensions instead.</summary>
        public override TField  CreateField(int numElements, string name, string description)
        {
            throw new InvalidOperationException("Can not use this method on collection of 3D structured fields because all 3 indices must be specified when creating a field.");
 	         //return base.CreateField(numElements, name, description);
        }

        /// <summary>Creates a new 3D structured field of specified dimensions and with specified name and description.</summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field (number of grid nodes in the second direction).</param>
        /// <param name="dim3">Third dimension of the field (number of grid nodes in the third direction).</param>
        /// <param name="fieldName">Name of the created field (can be null).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        public virtual TField CreateField(int dim1, int dim2, int dim3, string fieldName, string fieldDescription)
        {
            TField ret = new TField();
            if (dim1 != 0 || dim2 != 0 || dim3 != 0)
                ret.SetDimensions(dim1, dim2, dim3);
            if (fieldName != null)
                ret.Name = fieldName;
            if (fieldDescription != null)
                ret.Description = fieldDescription;
            return ret;
        }

        /// <summary>>Call to this method is invalid for the current type. Call the method with three dimensions instead.</summary>
        public override void AddField(int numElements, string fieldName, string fieldDescription)
        {
            throw new InvalidOperationException("This method may not be called on structured fields because all dimensions must be specified when creating such a field.");
        }

        /// <summary>Creates a new field with specified dimensions and name and description, and adds it to the current collection.</summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field (number of grid nodes in the second direction).</param>
        /// <param name="dim3">Third dimension of the field (number of grid nodes in the third direction).</param>
        /// <param name="fieldName">Name of the created field (normally it shouldn't be null though this is legal).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        public void AddField(int dim1, int dim2, int dim3, string fieldName, string fieldDescription)
        {
            AddField(CreateField(dim1, dim2, dim3, fieldName, fieldDescription));
        }

    }  // class FieldCollection3d<TField, TElement>



    /// <summary>Collection of structured 2D fields of the same type.  Fields are identified by their names.
    /// <para>In principle, contained fields can be of different dimensions.</para></summary>
    /// <typeparam name="TField">Type of fields contained in the collection. Must be a 2D structured field (<see cref="StructuredField2d{TElement}"/>).</typeparam>
    /// <typeparam name="TElement">Type of elements of the fields that can be contained in the collection.</typeparam>
    /// <remarks>Completely generic definition was introduced in 2009.
    /// Before that, several types were used for specific field collections.</remarks>
    /// $A Igor Apr09;
    public class FieldCollection2d<TField, TElement> : FieldCollection<TField, TElement>
        where TField : StructuredField2d<TElement>, new()
    {

        /// <summary>Constructor.</summary>
        public FieldCollection2d()
            : base()
        { }

        /// <summary>Call to this method is invalid for the current type. Call the method with three dimensions instead.</summary>
        public override TField CreateField(int numElements, string name, string description)
        {
            throw new InvalidOperationException("Can not use this method on collection of 2D structured fields because both 2 indices must be specified when creating a field.");
            //return base.CreateField(numElements, name, description);
        }

        /// <summary>Creates a new 2D structured field of specified dimensions and with specified name and description.</summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field (number of grid nodes in the second direction).</param>
        /// <param name="fieldName">Name of the created field (can be null).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        public virtual TField CreateField(int dim1, int dim2, string fieldName, string fieldDescription)
        {
            TField ret = new TField();
            if (dim1 != 0 || dim2 != 0)
                ret.SetDimensions(dim1, dim2);
            if (fieldName != null)
                ret.Name = fieldName;
            if (fieldDescription != null)
                ret.Description = fieldDescription;
            return ret;
        }

        /// <summary>>Call to this method is invalid for the current type. Call the method with three dimensions instead.</summary>
        public override void AddField(int numElements, string fieldName, string fieldDescription)
        {
            throw new InvalidOperationException("This method may not be called on structured fields because all dimensions must be specified when creating such a field.");
        }

        /// <summary>Creates a new field with specified dimensions and name and description, and adds it to the current collection.</summary>
        /// <param name="dim1">First dimension of the field (number of grid nodes in the first direction).</param>
        /// <param name="dim2">Second dimension of the field (number of grid nodes in the second direction).</param>
        /// <param name="fieldName">Name of the created field (normally it shouldn't be null though this is legal).</param>
        /// <param name="fieldDescription">Description of the created field (can be null).</param>
        public void AddField(int dim1, int dim2, string fieldName, string fieldDescription)
        {
            AddField(CreateField(dim1, dim2, fieldName, fieldDescription));
        }

    }  // class FieldCollection2d<TField, TElement>



    /// <summary>Collection of fields of the same type. Fields are identified by their names.
    /// <para>In principle, contained fields can be of different dimensions.</para></summary>
    /// <typeparam name="TField">Type of fields contained in the collection.</typeparam>
    /// <typeparam name="TElement">Type of elements of the fields that can be contained in the collection.</typeparam>
    /// <remarks>Completely generic definition was introduced in 2009.
    /// Before that, several types were used for specific field collections.</remarks>
    /// $A Igor Apr09;
    public class FieldCollection<TField, TElement>
        where TField: Field<TElement>, new()
    {

        public const string DefaultFieldNameConst = "default";

        /// <summary>Gets name that is used for automatically created fields.</summary>
        public string DefauletFieldName
        { get { return DefaultFieldNameConst; } }

        /// <summary>Constructor.</summary>
        public FieldCollection()
        {  }

        /// <summary>Creates a new field of specified length and with specified name and description (the latter two can be null).</summary>
        /// <param name="numElements">Field length (number of elements).</param>
        /// <param name="name">Field name (can be null).</param>
        /// <param name="description">Description of the field (can be null).</param>
        public virtual TField CreateField(int numElements, string name, string description)
        {
            TField ret = new TField();
            if (numElements > 0)
                ret.SetLength(numElements);
            if (name != null)
                ret.Name = name;
            if (description != null)
                ret.Description = description;
            return ret;
        }
        
        private SortedList<string, TField> _fields = new SortedList<string, TField>();

        protected SortedList<string, TField> Fields
        {
            get { return _fields; }
        }

        private string _activeFieldName;

        /// <summary>Gets or sets name of the active field.
        /// <para>Active field will be accessed throughthe <see cref="ActiveField"/> property. 
        /// When setting name of the active field, that property is not assigned, but it gets
        /// assigned on the first get access.</para></summary>
        public string ActiveFieldName
        {
            get { return _activeFieldName; }
            set {
                if (value != _activeFieldName)
                {
                    NullifyActiveField();
                    _activeFieldName = value;
                }
            }
        }

        private TField _activeField;

        /// <summary>Gets the currently active field.</summary>
        public TField ActiveField
        {
            get
            {
                if (_activeField == null)
                {
                    if (_fields.ContainsKey(_activeFieldName))
                        _activeField = _fields[_activeFieldName];
                }
                return _activeField;
            }
        }

        /// <summary>Sets the variable holding currently active field to null.
        /// <para>If active field name is specified then active field will still be correctly
        /// obtained (lazy evaluation).</para></summary>
        protected void NullifyActiveField()
        {
            _activeField = null;
        }


        /// <summary>Returns names of all the fields contained in the current field collection.</summary>
        public string[] GetFieldNames()
        {
            return _fields.Keys.ToArray();
        }

        /// <summary>Gets the field that has a specified name.</summary>
        /// <param name="fieldName">Name of the field to be returned.</param>
        public TField this[string fieldName]
        {
            get { if (!_fields.ContainsKey(fieldName)) return null; else return _fields[fieldName]; }
        }

        /// <summary>Get the field that corresponds to the specified index.
        /// <para>This is seldom used, usually fields will be accessed through field names.</para>
        /// <para>Warning: when adding fields, index of a field may change, therefore this kind of 
        /// access should only be used wen there is no adding or removing of fields.</para></summary>
        /// <param name="index">Index of the field.</param>
        public TField this[int index]
        {
            get { return _fields.Values[index]; }
        }

        /// <summary>Gets the number of fields contained in the collection.</summary>
        public int Count
        {
            get { return _fields.Count; }
        }


        /// <summary>Creates a new field of the specified length and adds it to the current collection.</summary>
        /// <param name="numElements">Length of the field that is created and added (number of elements).</param>
        /// <param name="fieldName">Name of the field. In principle, it should be different tan null.</param>
        /// <param name="fieldDescription">Optional description of the field. It can be null.</param>
        public virtual void AddField(int numElements, string fieldName, string fieldDescription)
        {
            AddField(CreateField(numElements, fieldName, fieldDescription));
        }

        /// <summary>Adds the specified to the collection under a specified name.
        /// <para>If the field already has a name then its name is replaced by the specified name, 
        /// unlsee the specified name is null.</para></summary>
        /// <param name="field">Field to be added to the collection.</param>
        /// <param name="name">Name under which the field is added to the current collection.</param>
        public void AddField(TField field, string name)
        {
            if (field == null)
                throw new ArgumentNullException("field", "Field to be added is not specified (null argument).");
            if (name!=null)
                field.Name = name;
            AddField(field);
        }

        /// <summary>Adds the specified field to the urrent field collection.</summary>
        /// <param name="field">Field to be added.</param>
        public void AddField(TField field)
        {
            if (field == null)
                throw new ArgumentNullException("field", "Field to be added is not specified (null argument).");
            string name = field.Name;
            if (name == _activeFieldName)
                NullifyActiveField();   // becaue active field reference by name might have changed.
            _fields.Add(name, field);
            if (_activeFieldName == null)
            {
                ActiveFieldName = field.Name;
            }
        }
        
        /// <summary>Removed the field with the specified name from the current field collection.</summary>
        /// <param name="fieldName">Name of the field to be removed.</param>
        public void RemoveField(string fieldName)
        {
            if (fieldName == _activeFieldName)
                NullifyActiveField();
            if (_fields.ContainsKey(fieldName))
                _fields.Remove(fieldName);
        }


    }  // class FieldCollection<TField, TElement>


}
