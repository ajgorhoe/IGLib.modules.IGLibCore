
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{

    /// <summary>Generates 1D grids with equidistant or geometrically grown intervals.</summary>
    /// $A Igor Apr10 Dec10;
    public class GridGenerator1d : GridGenerator1dBase,
        IGridGenerator1d, ILockable
    {

        /// <summary>Creates a uniform 1D grid generator that generates two nodes at 0 and 1.</summary>
        public GridGenerator1d()
            : base()
        {
            MakeUniformUnscaled();
            NumNodes = 2;
        }

        /// <summary>Creates a 1D grid generator.
        /// Grid intervals can grow or fall in geometric series, either from the lower limit or from the center of the
        /// grid interval, and the grid can also be scaled.</summary>
        /// <param name="from">Lower bound of the generated 1D grid (can change when scaling is applied).</param>
        /// <param name="to">Upper bound of the generated 1D grid (can change when scaling is applied).</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        /// <param name="centered">Specifies whether the grid is centered in such a way that the intervals grow or fall 
        /// from the center towards the bounds.</param>
        /// <param name="growthFactor">Factor of interval growth.
        ///   Factor 1 means a uniform grid with equidistant intervals.
        ///   If factor is larger than 1 then intervals grow from the lower bound towards the upper bound.
        ///   (or from the center, in the case that <paramref name="centered"/> = true). If factor is less than
        ///   1 then intervals shrink in this direction.</param>
        /// <param name="scalingFactor">Factor by which the grid is scaled.</param>
        public GridGenerator1d(double from, double to, int numNodes, bool centered, double growthFactor,
             double scalingFactor)
        {
            MakeUniformUnscaled();
            this.CoordinateFirst = from;
            this.CoordinateLast = to;
            this.NumNodes = numNodes;
            this.Centered = centered;
            this.GrowthFactor = growthFactor;
            this.ScalingFactor = scalingFactor;
        }

        /// <summary>Creates a 1D grid generator.
        /// Grid intervals can grow or fall in geometric series, either from the lower limit or from the center of the
        /// grid interval.</summary>
        /// <param name="from">Lower bound of the generated 1D grid.</param>
        /// <param name="to">Upper bound of the generated 1D grid.</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        /// <param name="centered">Specifies whether the grid is cnetered in such a way that the intervals grow or fall 
        /// from the center towards the bounds.</param>
        /// <param name="growthFactor">Factor of interval growth.
        ///   Factor 1 means a uniform grid with equidistant intervals.
        ///   If factor is larger than 1 then intervals grow from the lower bound towards the upper bound.
        ///   (or from the center, in the case that <paramref name="centered"/> = true). If factor is less than
        ///   1 then intervals shrink in this direction.</param>
        public GridGenerator1d(double from, double to, int numNodes, bool centered, double growthFactor):  
            this(from, to, numNodes, centered, growthFactor, 1.0 /* scalingFactor */)
        {  }

        /// <summary>Creates a 1D grid generator.
        /// Grid intervals can grow or fall in geometrical series from the lower bound of the grid interval.</summary>
        /// <param name="from">Lower bound of the generated 1D grid.</param>
        /// <param name="to">Upper bound of the generated 1D grid.</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        /// <param name="growthFactor">Factor of interval growth.
        ///   Factor 1 means a uniform grid with equidistant intervals.
        ///   If factor is larger than 1 then intervals grow from the lower bound towards the upper bound.
        ///   If factor is less than 1 then intervals shrink in this direction.</param>
        public GridGenerator1d(double from, double to, int numNodes, double growthFactor):  
            this(from, to, numNodes, false /* centered */, growthFactor, 1.0 /* scalingFactor */)
        {  }

        /// <summary>Creates a 1D grid generator for uniform grid (equidistant intervals).</summary>
        /// <param name="from">Lower bound of the generated 1D grid.</param>
        /// <param name="to">Upper bound of the generated 1D grid.</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        public GridGenerator1d(double from, double to, int numNodes):  
            this(from, to, numNodes, false /* centered */, 1.0 /* growthFactor */, 1.0 /* scalingFactor */)
        {  }



        #region Data

        bool _centered = false;
        
        double _growthFactor = 1.0;

        double _scalingFactor = 1.0;

        /// <summary>Flag specifying whether intervals grow in both direction from the center of the grid interval.</summary>
        public bool Centered
        {
            get { lock (Lock) { return _centered; } }
            set { _centered = value; }
        }

        /// <summary>Quotient of lengths of two consecutive intervals of the generated grid.</summary>
        public double GrowthFactor
        {
            get { lock (Lock) { return _growthFactor; } }
            set { _growthFactor = value; }
        }

        /// <summary>Scaling factor by which the whole generated grid is scaled.
        /// This enables the generated grid to extend outside the prescribed lower and upper bound, or to be shrinked within this interval.
        /// WARNING: In most cases, scaling should be kept at 1.0.</summary>
        public double ScalingFactor
        {
            get { lock (Lock) { return _scalingFactor; } }
            set { _scalingFactor = value; }
        }


        /// <summary>Resets the parameters in such away that the generated grid is uniform (equidistant intervals)
        /// and with scaling factor 1.</summary>
        public void MakeUniformUnscaled()
        {
            Centered = false;
            GrowthFactor = 1.0;
            ScalingFactor = 1.0;
        }

        #endregion Data


        #region Operation

        /// <summary>Performs grid generation and stores the generated nodes directly on the provided list.
        /// Unless necessary due to nature of generation, results are not stored internally on the current grid generator object.
        /// Because of this, the Calculated flag is normally not set after calling this function.
        /// WARNING: This method generates a grid even if it has already been generated and is up to date.</summary>
        /// <param name="nodeList">A list where node coordinates are stored.
        /// List is allocated or re-allocated if necessary.</param>
        public override void CalculateGrid(ref List<double> nodeList)
        {
            lock (Lock)
            {
                CalculateGridUnitFactors(this.NumNodes, this.Centered, this.GrowthFactor,
                        this.ScalingFactor, ref _nodes);
                FitGridNodes(0.0, 1.0, CoordinateFirst, CoordinateLast, _nodes);
            }
        }


        #endregion Operation

    }



}