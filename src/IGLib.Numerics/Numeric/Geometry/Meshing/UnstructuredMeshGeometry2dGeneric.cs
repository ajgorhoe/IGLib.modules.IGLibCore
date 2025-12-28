
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Num
{

    /// <summary>Geometry and topology of an unstructured 2D mesh.
    /// <param>Presents both connected meshes and unconnected sets of points.</param></summary>
    /// <typeparam name="TCoord">Type of nodal coordinates.</typeparam>
    /// $A Igor Jan08 Mar09;
    public class UnstructuredMeshGeometry2d<TCoord> : Field<TCoord>
    {


        /// <summary>Constructs a new empty unstructured grid (no elements contained) with no name and no description.</summary>
        public UnstructuredMeshGeometry2d()
            : this(0, null, null)
        { }

        /// <summary>Creates a new unstructured grid with the specified number of elements.</summary>
        /// <param name="numElements">Number of elements of the coordinate field.</param>
        public UnstructuredMeshGeometry2d(int numElements)
            : this(numElements, null /* fieldName */, null /* fieldName */)
        { }

        /// <summary>Constructs a new unstructured grid with the specified number of elements and name.</summary>
        /// <param name="numElements">Number of elements.</param>
        /// <param name="fieldName">Name of the field.</param>
        public UnstructuredMeshGeometry2d(int numElements, string fieldName)
            : this(numElements, fieldName, null)
        { }


        /// <summary>Constructs a new unstructured grid with the specified number of elements, name and description.
        /// Table of elements is allocated.</summary>
        /// <param name="numElements">Number of elements.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldDescription">Field description.</param>
        public UnstructuredMeshGeometry2d(int numElements, string fieldName, string fieldDescription) :
            base(numElements, fieldName, fieldDescription)
        { }


        // TODO: add handling of connectivity!



    }  // class UnstructuredMeshGeometry2d<TCoord>


}


