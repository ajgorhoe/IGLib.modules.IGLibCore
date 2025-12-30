// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

    // GENERAL MESH REPRESENTATIONS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Num;

namespace IG.Gr
{

    /// <summary>Base class for all mesh classes.</summary>
    public abstract class Mesh
    {

        /// <summary>Returns a list of node numbers.
        /// This property must be implemented in a concrete derived class in such a way that it 
        /// returns a non/null list, odherwise GetNodeNuber() and SetNodeNumber() must be overridden.</summary>
        protected internal abstract List<int> NodeNumbers
        {
            get;
            protected set;
        }

        public virtual int GetNodeNumber(int i)
        {
            if (NodeNumbers == null)
                throw new Exception("A list of node numbers does not exist.");
            if (i<0 || i>=NodeNumbers.Count)
            throw new ArgumentException("Index " + i.ToString() + " is out of range (0 to " 
                + (NodeNumbers.Count-1).ToString() + ").");
            return NodeNumbers[i];
        }

        protected internal virtual void SetNodeNumber(int gridpoint, int nodenumber)
        {
            if (NodeNumbers == null)
            {
                int num = NumNodes;
                NodeNumbers = new List<int>(num);
                for (int i=0;i<num;++i)
                    NodeNumbers[i] =i;  // initialize node numbers to correspond grid point indices.
                // throw new Exception("A list of node numbers does not exist.");
            }
            if (gridpoint < 0 || gridpoint >= NodeNumbers.Count)
            throw new ArgumentException("Index " + gridpoint.ToString() + " is out of range (0 to " 
                + (NodeNumbers.Count-1).ToString() + ").");
            NodeNumbers[gridpoint] = nodenumber;

        }
        
        /// <summary>Returns number of grid points.</summary>
        public abstract int NumNodes
        {
            get;
        }

        /// <summary>Gets a value indicating whether the mesh has faces.</summary>
        public abstract bool HasFaces
        { get; }

        /// <summary>Returns the number of faces that the mesh has.</summary>
        public abstract int NumFaces
        { get; }


        /// <summary>Gets a value indicating whether the mesh has volumes.</summary>
        public abstract bool HasVolumes
        { get; }

        /// <summary>Returns the number of volumes that the mesh has.</summary>
        public abstract int NumVolumes
        { get; }




    }

    public abstract class Mesh2D : Mesh
    {
    }

    public abstract class Mesh3D : Mesh
    {

        /// <summary>Returns the table of grid co-ordinates.
        /// If this property is not properly implemented in the concrete subclasses (e.g. it simply
        /// returns null) then methods GetGridCoordinate() and SetGridCoordinate() and the indexer
        /// must be re-implemented.</summary>
        protected abstract vec3[] GridCoordinates
        {
            get;
        }


        /// <summary>Returns coordinates of the specified grid point.
        /// Indices run contiguously from 0 on (they do not correspond to node numbers, which can be arbitrarily arranged).</summary>
        /// <param name="i">Zero-based (contiguous) index of the grid point.</param>
        /// <returns>Coordinate of the grid/point.</returns>
        public virtual vec3 GetGridCoordinate(int i)
        {
            return this[i];
        }

        /// <summary>Sets coordinates of the specified grid point to the provided values.
        /// Indices run contiguously from 0 on (they do not correspond to node numbers, which can be arbitrarily arranged).</summary>
        /// <param name="i">Zero-based (contiguous) index of the grid point.</param>
        /// <param name="coordinates">Co-ordinates that are assigned to the grid point.</param>
        /// <returns></returns>
        public virtual void SetGridCoordinate(int i, vec3 coordinates)
        {
            this[i] = coordinates;
        }

        /// <summary>Gets or sets co-ordinates of the specified grid point.
        /// Indices run contiguously from 0 on (they do not correspond to node numbers, which can be arbitrarily arranged).</summary>
        /// <param name="i">Zero-based (contiguous) index of the grid point.</param>
        public virtual vec3 this[int i]
        {
            get
            {
                if (GridCoordinates == null)
                    throw new Exception("A table of grid co-ordinates does not exist.");
                if (i < 0 || i >= GridCoordinates.Length)
                    throw new ArgumentException("Grid point index " + i.ToString() + " is out of range (0 to "
                        + (GridCoordinates.Length - 1).ToString() + ").");
                return GridCoordinates[i];
            }
            set
            {
                if (GridCoordinates == null)
                    throw new Exception("A table of grid co-ordinates does not exist.");
                if (i < 0 || i >= GridCoordinates.Length)
                    throw new ArgumentException("Grid point index " + i.ToString() + " is out of range (0 to "
                        + (GridCoordinates.Length - 1).ToString() + ").");
                GridCoordinates[i] = value;
            }
        }


        // TODO:
        // Implement:
        // Coordinates of a given node (numbered in non-contiguos way)

    }



    /// <summary>Surface mesh in 3 dimensions.</summary>
    public abstract class SurfceMesh3D: Mesh3D
    {

        public override bool HasVolumes 
        { get { return false; } }

        public override int  NumVolumes
        { get { return 0; } }
    }




    public class StructuredSurfaceMesh3D :  SurfceMesh3D
    {

        #region Construction

        private StructuredSurfaceMesh3D() // argument-less constructor is not allowed.
        { }


        /// <summary>Constructs a structured surface mesh in 3 dimensions.
        /// Grid co-ordinates are initialized to 0.</summary>
        /// <param name="num1">Number of points in the first grid direction.</param>
        /// <param name="num2">Number of points in the second grid direction.</param>
        public StructuredSurfaceMesh3D(int num1, int num2)
        {
            _numx = num1;
            _numy = num2;
            int numgridpoints = _numx * _numy;
            _gridCoordinates = new vec3[numgridpoints];
            for (int i=0; i< numgridpoints; ++i)
                _gridCoordinates[i] = new vec3(0.0, 0.0, 0.0);
            _nodeNumbers = new List<int>();
            for (int i=0; i< numgridpoints; ++i)
                NodeNumbers[i] = i;
        }

        /// <summary>Construct a structured surface mesh in 3 dimensions.
        /// Complete is constructed by translations of the origin by linear combinations of two base vectors
        /// with integer factors.</summary>
        /// <param name="origin">Origin of the mesh.</param>
        /// <param name="basevector1">The first base step of the mesh.</param>
        /// <param name="basevector2">The second base step of the mesh.</param>
        /// <param name="num1">Number of points in the first grid direction.</param>
        /// <param name="num2">Number of points in the second grid direction.</param>
        public StructuredSurfaceMesh3D(vec3 origin, vec3 basevector1, vec3 basevector2, int num1, int num2)
        {
            _numx = num1;
            _numy = num2;
            int numgridpoints = _numx*_numy;
            _gridCoordinates = new vec3[numgridpoints];
            for (int i=0; i< _numx; ++i)
                for (int j = 0; j < _numy; ++j)
                {
                    _gridCoordinates[i*_numx + j] = origin + i * basevector1 + j * basevector2;
                }
            _nodeNumbers = new List<int>();
            for (int i=0; i< numgridpoints; ++i)
                NodeNumbers[i] = i;
        }


        #endregion  // Construction

        #region // Data

        private int
            _numx = 0,
            _numy = 0;

        private vec3[] _gridCoordinates = null;  // Basic data structure

        protected override vec3[] GridCoordinates
        {
            get { return _gridCoordinates; }
        }

        private List<int> _nodeNumbers = null;

        protected internal override List<int> NodeNumbers
        {
            get { return _nodeNumbers; }
            protected set { _nodeNumbers = value; }
        }

        #endregion  // Data



        /// <summary>Returns the number of grid points.</summary>
        public override int NumNodes
        {
            get
            {
                return _numx*_numy;
            }
        }


        /// <summary>Gets a value indicating whether the mesh has faces.</summary>
        public override bool HasFaces
        { get { return true; } }

        /// <summary>Returns the number of faces that the mesh has.</summary>
        public override int NumFaces
        { get { return _numx * _numy; } }

        public virtual vec3 this[int i, int j]
        {
            get
            {
                if (GridCoordinates == null)
                    throw new Exception("A table of grid co-ordinates does not exist.");
                if (i < 0 || i >= _numx)
                    throw new ArgumentException("First 2D grid point index " + i.ToString() + " is out of range (0 to "
                        + (_numx - 1).ToString() + ").");
                if (j < 0 || j >= _numx)
                    throw new ArgumentException("Second 2D grid point index " + j.ToString() + " is out of range (0 to "
                        + (_numy - 1).ToString() + ").");
                return GridCoordinates[i*_numx+j];
            }
            set
            {
                if (GridCoordinates == null)
                    throw new Exception("A table of grid co-ordinates does not exist.");
                if (i < 0 || i >= _numx)
                    throw new ArgumentException("First 2D grid point index " + i.ToString() + " is out of range (0 to "
                        + (_numx - 1).ToString() + ").");
                if (j < 0 || j >= _numx)
                    throw new ArgumentException("Second 2D grid point index " + j.ToString() + " is out of range (0 to "
                        + (_numy - 1).ToString() + ").");
                GridCoordinates[i*_numx+j] = value;
            }
        }


    }  // class StructuredSurfaceMesh3D



}  // namespace IG.Gr
