
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{

    /// <summary>Generates 1D grids with equidistant or geometrically grown intervals.</summary>
    /// $A Igor Apr10 Dec10;
    public class GridGenerator1dFunc : GridGenerator1dBase,
        IGridGenerator1d, ILockable
    {

        /// <summary>Creates a uniform 1D grid generator that generates two nodes at 0 and 1.</summary>
        public GridGenerator1dFunc()
            : base()
        {
            MakeUniform();
            NumNodes = 2;
        }

        /// <summary>Creates a 1D grid generator where grid point positions are calculated by the specified function.</summary>
        /// <param name="from">Lower bound of the generated 1D grid.</param>
        /// <param name="to">Upper bound of the generated 1D grid.</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        /// <param name="function">Function used for evaluation of grid points.</param>
        /// <param name="firstFunctionArgument">First argument (lower bound of the interval) where grid function is evaluated.</param>
        /// <param name="lastFunctionArgument">First argument (upper bound of the interval) where grid function is evaluated.</param>
        public GridGenerator1dFunc(double from, double to, int numNodes, IRealFunction function, double firstFunctionArgument, double lastFunctionArgument)
        {
            this.CoordinateFirst = from;
            this.CoordinateLast = to;
            this.Function = function;
            this.FunctionArgumentFirst = firstFunctionArgument;
            this.FunctionArgumentLast = lastFunctionArgument;
        }




        #region Data

        //bool _centered = false;

        //double _growthFactor = 1.0;

        //double _scalingFactor = 1.0;

        ///// <summary>Flag specifying whether intervals grow in both direction from the center of the grid interval.</summary>
        //public bool Centered
        //{
        //    get { lock (Lock) { return _centered; } }
        //    set { _centered = value; }
        //}

        ///// <summary>Quotient of lengths of two consecutive intervals of the generated grid.</summary>
        //public double GrowthFactor
        //{
        //    get { lock (Lock) { return _growthFactor; } }
        //    set { _growthFactor = value; }
        //}

        ///// <summary>Scaling factor by which the whole generated grid is scaled.
        ///// This enables the generated grid to extend outside the prescribed lower and upper bound, or to be shrinked within this interval.
        ///// WARNING: In most cases, scaling should be kept at 1.0.</summary>
        //public double ScalingFactor
        //{
        //    get { lock (Lock) { return _scalingFactor; } }
        //    set { _scalingFactor = value; }
        //}


        IRealFunction _func;

        public IRealFunction Function
        {
            get { return _func; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("Function for evaluation of grid points is not specified (null argument in setter).");
                _func = value;
            }
        }

        double _funcArgFirst = 0.0;
        double _funcArgLast = 0.0;

        /// <summary>First argument where grid generation function is evaluated.</summary>
        public double FunctionArgumentFirst
        {
            get { return _funcArgFirst; }
            set { _funcArgFirst = value; }
        }

        /// <summary>Last argument where grid generation function is evaluated.</summary>
        public double FunctionArgumentLast
        {
            get { return _funcArgLast; }
            set { _funcArgLast = value; }
        }

        /// <summary>Resets the parameters in such away that the generated grid is uniform (equidistant intervals)
        /// and with scaling factor 1.</summary>
        public void MakeUniform()
        {
            Function = Func.GetIdentity();
            FunctionArgumentFirst = 0.0;
            FunctionArgumentLast = 1.0;
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
                CalculateGridUnitFactors(this.NumNodes, this.Function, this.FunctionArgumentFirst, this.FunctionArgumentLast, ref _nodes);
                FitGridNodes(0.0, 1.0, CoordinateFirst, CoordinateLast, _nodes);
            }

        }

        #endregion Operation

    }



}