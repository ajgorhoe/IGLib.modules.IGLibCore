
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IG.Lib;

namespace IG.Num
{



    /// <summary>Interface for 1d grid generators.</summary>
    /// <remarks>List(double) is usually a primary data sturcture for storing nodes, therefore methods returning reference to this are more efficient.</remarks>
    /// $A Igor Dec10;
    public interface IGridGenerator1d : ILockable
    {

        #region Data

        /// <summary>Gets or sets coordinate of the first generated node (usually the minimal generated coordinate).</summary>
        double CoordinateFirst { get; set; }

        /// <summary>Gets or sets coordinate of the last generated node (usually the maximal generated coordinate).</summary>
        double CoordinateLast { get; set; }
        
        /// <summary>Gets or sets teh number of nodes to be generated.</summary>
        int NumNodes { get; set; }

        /// <summary>Gets the number of intervals between the nodes.
        /// Simply <see cref="NumNodes"/>-1.</summary>
        int NumIntervals { get; }

        /// <summary>Flag indicating whether the grid is generated ready to use.
        /// If true then node positions are contained in an internal structure, such that they can be obtained
        /// simply by copying this structure (i.e. no need for re-calculation).
        /// REMARK: If you intend to do something with results on basis of the value of this flag, don't forget to enclose
        /// checking of the flag and your operation into the lock(....Lock){ ... } block. </summary>
        bool Calculated { get; }

        #endregion Data


        #region Operation

        /// <summary>Clears the results of grid generation (releases internal structures).
        /// The <see cref="Calculated"/> flag is set to false.</summary>
        void ClearResults();

        /// <summary>Performs grid generation and stores the generated nodes directly on the provided list.
        /// Unless necessary due to nature of generation, results are not stored internally on the current grid generator object.
        /// Because of this, the Calculated flag is normally not set after calling this function.
        /// WARNING: This method generates a gid even if it has already been generated and is up to date.</summary>
        /// <param name="nodeList">A list where node coordinates are stored.
        /// List is allocated or re-allocated if necessary.</param>
        void CalculateGrid(ref List<double> nodeList);

        
        /// <summary>Performs grid generation and stores the generated nodes and intervals between them directly on the provided lists.
        /// Unless necessary due to nature of generation, results are not stored internally on the current grid generator object.
        /// Because of this, the Calculated flag is normally not set after calling this function.
        /// WARNING: This method generates a grid even if it has already been generated and is up to date.</summary>
        /// <param name="nodeList">A list where positions of the generated grid are stored.</param>
        /// <param name="intervalLengthsList">A list where lengths of intervals between grid nodes are stored.</param>
        void CalculateGrid(ref List<double> nodeList, ref List<double> intervalLengthsList);
        

        /// <summary>Performs grid generation according to current settings. 
        /// The generated grid is stored in internal structures of the generator, from where it can be readily copied
        /// (e.g. by GetNodeTable() or by GetNodeList()), used in some other way (e.g. by GetIntervalLengthsList) 
        /// or just referenced (e.g. by GetNodeListReference()).
        /// If the grid has already been generated and it is up to date (according to parameters of the generator) then
        /// the grid is not re-calculated.</summary>
        void CalculateGrid();

        /// <summary>Returns the specified node of the generated grid.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="which">Specifies which node (counted from 0) is to be returned.</param>
        /// <returns>Position of the specified node of the grid.</returns>
        double GetNode(int which);

        /// <summary>Stores the node positions of the generated 1D grid into the specified table.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="result">Table where node positions of the generated grid are stored. Allocated or relocated if necessary.</param>
        void GetNodeTable(ref double[] result);

        /// <summary>Returns a table containing 1D node positions.
        /// A copy of list of node positions is always created and returned, therefore it is guaranteed that this table will not
        /// be used by some other object related to the grid generator.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        double[] GetNodeTable();
        
        /// <summary>Stores the node positions of the generated 1D grid into the specified list.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="result">List where node positions of the generated grid are stored. Allocated or relocated if necessary.</param>
        void GetNodeList(ref List<double> result);

        /// <summary>Returns a list containing 1D node positions.
        /// A copy of list of node positions is always created and returned, therefore it is guaranteed that this table will not
        /// be used by some other object related to the grid generator.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <returns></returns>
        List<double> GetNodeList();

        /// <summary>Returns a list containing generated 1D node positions.
        /// This method can return a reference to the internal list containing the generated node coordinated.
        /// This means that any future generation or other operation performed by the generator can override list contents.
        /// In order to use contents of the list thread safely, enclose this method call and all subsequent operations you 
        /// will perform on the returned list of generated nodes, within a lock((...).Lock){ ... } block.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <returns>A reference to a list of generated nodes. This is usually not a copy of the list but a reference to
        /// an internal structure that can be overridden by subsequent operations on the current grid generator.</returns>
        List<double> GetNodeListReference();


        /// <summary>Stores a table of interval lengths between the generated nodes into the specified array.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="result">Table where interval lengths between nodes of the generated grid are stored. Allocated or relocated if necessary.</param>
        void GetIntervalLengthsTable(ref double[] result);

        /// <summary>Generates and returns a table of interval lengths between the generated nodes.
        /// The returned table is a copy that will not be overridden by further operations on the grid generator.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <returns>A table containing lengths of intervals between the subsequent nodes generated by the current grid generator.</returns>
        double[] GetIntervalLengthsTable();

        /// <summary>Stores a table of interval lengths between the generated nodes into the specified list.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="result">List where interval lengths between nodes of the generated grid are stored. Allocated or relocated if necessary.</param>
        void GetIntervalLengthsList(ref List<double> result);

        /// <summary>Generates and returns a list of interval lengths between the generated nodes.
        /// The returned list is a copy that will not be overridden by further operations on the grid generator.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <returns>A table containing lengths of intervals between the subsequent nodes generated by the current grid generator.</returns>
        List<double> GetIntervalLengthsList();

        /// <summary>Returns the length of the specified interval between generated nodes of the grid.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="i">Consecutive number of the interval between nodes.</param>
        double GetIntervalLength(int i);

        #endregion Operation

    }  // interface IGridGenerator1d



    /// <summary>Base class for 1D grid generators.</summary>
    /// $A Igor Apr10; Dec10;
    public abstract class GridGenerator1dBase : IGridGenerator1d, ILockable
    {

        #region Construction

        protected GridGenerator1dBase() 
        {
            Calculated = false;
            CoordinateFirst = 0.0;
            CoordinateLast = 1.0;
        }

        #endregion Construction

        #region StaticConstruction

        /// <summary>Creates and returns a 1D grid generator for uniform grid (equidistant intervals).</summary>
        /// <param name="from">Lower bound of the generated 1D grid.</param>
        /// <param name="to">Upper bound of the generated 1D grid.</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        public static GridGenerator1d CreateUniform(double from, double to, int numNodes)
        { return new GridGenerator1d(from, to, numNodes); }
        
        /// <summary>Creates and returns a 1D grid generator.
        /// Grid intervals can grow or fall in geometrical series from the lower bound of the grid interval.</summary>
        /// <param name="from">Lower bound of the generated 1D grid.</param>
        /// <param name="to">Upper bound of the generated 1D grid.</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        /// <param name="growthFactor">Factor of interval growth.
        ///   Factor 1 means a uniform grid with equidistant intervals.
        ///   If factor is larger than 1 then intervals grow from the lower bound towards the upper bound.
        ///   If factor is less than 1 then intervals shrink in this direction.</param>
        public static GridGenerator1d CreateGeometric(double from, double to, int numNodes, double growthFactor)
        { return new GridGenerator1d(from, to, numNodes, growthFactor);  }
        
        /// <summary>Creates and returns a 1D grid generator.
        /// Grid intervals can grow or fall in geometric series from the center of the
        /// grid interval towards the bounds.</summary>
        /// <param name="from">Lower bound of the generated 1D grid.</param>
        /// <param name="to">Upper bound of the generated 1D grid.</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        /// <param name="growthFactor">Factor of interval growth.
        ///   Factor 1 means a uniform grid with equidistant intervals.
        ///   If factor is larger than 1 then intervals grow from the centertowards the bounds. If factor is less than
        ///   1 then intervals shrink in this direction.</param>
        public static GridGenerator1d CreateGeometricCentered(double from, double to, int numNodes, double growthFactor)
        { return new GridGenerator1d(from, to, numNodes, true /* centered */, growthFactor); }

        /// <summary>Creates and returns a 1D grid generator.
        /// Grid intervals can grow or fall in geometric series from the lower bound towards teh upper bound of the
        /// grid interval, and the grid can also be scaled.</summary>
        /// <param name="from">Lower bound of the generated 1D grid (can change when scaling is applied).</param>
        /// <param name="to">Upper bound of the generated 1D grid (can change when scaling is applied).</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        /// <param name="growthFactor">Factor of interval growth.
        ///   Factor 1 means a uniform grid with equidistant intervals.
        ///   If factor is larger than 1 then intervals grow from the lower bound towards the upper bound.
        ///   If factor is less than 1 then intervals shrink in this direction.</param>
        /// <param name="scalingFactor">Factor by which the grid is scaled.</param>
        public static GridGenerator1d CreateGeometricScaled(double from, double to, int numNodes, 
            double growthFactor, double scalingFactor)
        { return new GridGenerator1d(from, to, numNodes, false /* centered */, growthFactor, scalingFactor); }

        /// <summary>Creates and returns a 1D grid generator.
        /// Grid intervals can grow or fall in geometric series from the center of the
        /// grid interval, and the grid can also be scaled.</summary>
        /// <param name="from">Lower bound of the generated 1D grid (can change when scaling is applied).</param>
        /// <param name="to">Upper bound of the generated 1D grid (can change when scaling is applied).</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        /// <param name="growthFactor">Factor of interval growth.
        ///   Factor 1 means a uniform grid with equidistant intervals.
        ///   If factor is larger than 1 then intervals grow from the center towards the bounds.
        ///   If factor is less than 1 then intervals shrink in this direction.</param>
        /// <param name="scalingFactor">Factor by which the grid is scaled.</param>
        public static GridGenerator1d CreateGeometricCenteredScaled(double from, double to, int numNodes, 
            double growthFactor, double scalingFactor)
        { return new GridGenerator1d(from, to, numNodes, true /* centered */, growthFactor, scalingFactor); }


        /// <summary>Creates and returns a 1D grid generator where grid point positions are calculated by the specified function.</summary>
        /// <param name="from">Lower bound of the generated 1D grid.</param>
        /// <param name="to">Upper bound of the generated 1D grid.</param>
        /// <param name="numNodes">Number of nodes generated.</param>
        /// <param name="function">Function used for evaluation of grid points.</param>
        /// <param name="firstFunctionArgument">First argument (lower bound of the interval) where grid function is evaluated.</param>
        /// <param name="lastFunctionArgument">First argument (upper bound of the interval) where grid function is evaluated.</param>
        public static GridGenerator1dFunc CreateFunctionBased(double from, double to, int numNodes, 
            IRealFunction function, double firstFunctionArgument, double lastFunctionArgument)
        {
            return new GridGenerator1dFunc(from, to, numNodes, function, firstFunctionArgument, lastFunctionArgument);
        }

        #endregion StaticConstruction


        #region Data

        private object _lockObj = new object();

        /// <summary>Object used for locking of the current object (ILockable interface).</summary>
        public object Lock { get { return _lockObj; } }

        private double
            _first = 0.0,
            _last = 1.0;

        /// <summary>Gets or sets coordinate of the first generated node (usually the minimal generated coordinate).</summary>
        public virtual double CoordinateFirst 
        {
            get { lock (Lock) { return _first; } }
            set { lock (Lock) { _first = value; } } 
        }

        /// <summary>Gets or sets coordinate of the last generated node (usually the maximal generated coordinate).</summary>
        public virtual double CoordinateLast 
        {
            get { lock (Lock) { return _last; } }
            set { lock (Lock) { _last = value; } } 
        }


        int _numNodes = 2;

        /// <summary>Gets or sets teh number of nodes to be generated.</summary>
        public virtual int NumNodes 
        {
            get { lock (Lock) { return _numNodes; } }
            set { lock (Lock) { _numNodes = value; } }
        }


        /// <summary>Gets the number of intervals between the nodes.
        /// Simply <see cref="NumNodes"/>-1.</summary>
        public virtual int NumIntervals
        { get { lock (Lock) { return NumNodes - 1; } } }

        bool _calculated = false;

        /// <summary>Flag indicating whether the grid is generated ready to use.
        /// If true then node positions are contained in an internal structure, such that they can be obtained
        /// simply by copying this structure (i.e. no need for re-calculation).
        /// REMARK: If you intend to do something with results on basis of the value of this flag, don't forget to enclose
        /// checking of the flag and your operation into the lock(....Lock){ ... } block. </summary>
        public virtual bool Calculated 
        {
            get { lock (Lock) { return _calculated; } }
            protected set { lock (Lock) { _calculated = value; } }
        }


        /// <summary>Internally stored generated nodes.</summary>
        protected List<double> _nodes;


        #endregion Data


        #region AuxiliaryUtilities

        ///// <summary>Copies all elements of the specified list to a target list.
        ///// After operation, target list contains all elements of the source list (only references are copied for objects)
        ///// in the same order. 
        ///// If the original list is null then target list can either be allocated (if it was allocated before the call) or not.
        ///// Target list is allocated or re-allocated as necessary.</summary>
        ///// <typeparam name="T">Type of elements contained in the list.</typeparam>
        ///// <param name="original">Original list.</param>
        ///// <param name="target">List that elements of the original list are copied to.</param>
        //protected static void CopyList<T>(List<T> original, ref List<T> target)
        //{
        //    if (original == null)
        //    {
        //        if (target != null)
        //            target.Clear();
        //    } else
        //    {
        //        int numEl = original.Count;
        //        if (target == null)
        //            target = new List<T>(original.Capacity);
        //        int numElTarget = target.Count;
        //        if (numElTarget > numEl)
        //        {
        //            target.RemoveRange(numEl, numElTarget - numEl);
        //            numElTarget = target.Count;
        //        }
        //        for (int i = 0; i < numEl; ++i)
        //        {
        //            if (i < numElTarget)
        //                target[i] = original[i];
        //            else
        //                target.Add(original[i]);
        //        }
        //    }
        //}


        ///// <summary>Copies all elements of the specified list to a target table.
        ///// After operation, target table contains all elements of the source list (only references are copied for objects)
        ///// in the same order. 
        ///// If the original list is null then target table will also become null.
        ///// Target table is allocated or re-allocated as necessary.</summary>
        ///// <typeparam name="T">Type of elements contained in the list.</typeparam>
        ///// <param name="original">Original list.</param>
        ///// <param name="target">Table that elements of the original list are copied to.</param>
        //protected static void CopyList<T>(List<T> original, ref T[] target)
        //{
        //    if (original == null)
        //    {
        //        if (target != null)
        //            target=null;
        //    }
        //    else
        //    {
        //        int numEl = original.Count;
        //        if (target == null)
        //            target = new T[numEl];
        //        else if (target.Count() != numEl)
        //            target = new T[numEl];
        //        for (int i = 0; i < numEl; ++i)
        //        {
        //            target[i] = original[i];
        //        }
        //    }
        //}


        /// <summary>Calculates factors for a table of values between two points, and stores
        /// them to factors.</summary>
        /// <param name="numElements">Number of sampling points (elements). It should be greater than 1.</param>
        /// <param name="centered">if 0 then factors run from 0 to 1 (for a table is from the
        /// specified starting till hte end point), otherwise factors run from -1 to 1
        /// (for tables centered in the starting point, ending in the end point and 
        /// starting in the reflected end point across the start point).</param>
        /// <param name="growthFactor">Factor by which length of successive intervals is increases
        /// to obtain table with intervals growing in geometric order. If the specified
        /// factor is 0 then it is set to 1.</param>
        /// <param name="scalingFactor">additional factor by which each factor is mutiplied.
        ///  If centered!=0 and growthfactor>1 then intervals fall from -1 to 0 and 
        /// grow from 0 to 1.</param>
        /// <param name="factors">Ouptput - list where factors are stored.</param>
        /// $A Igor Apr10;
        public static void CalculateGridUnitFactors(int numElements, bool centered, double growthFactor,
             double scalingFactor, ref List<double> factors)
        {
            int i, elementIndex;
            bool odd;
            double sum, currentFactor, intervalLength;
            if (numElements < 2)
            {
                numElements = 2;
                throw new ArgumentException("Wrong number of elements in a table (" + numElements
                    + "), should be at least 2.\n");
            }
            if (growthFactor == 0.0)
                growthFactor = 1.0;
            if (scalingFactor == 0.0)
                scalingFactor = 1.0;
            if (growthFactor < 0)
            {
                growthFactor *= -1.0;
                scalingFactor *= -1.0;
            }
            Util.ResizeList(ref factors, numElements, 0.0);
            if (centered)
            {
                // Factors from -1 to 1, interval shrinkage from -1 to 0 and interval growth
                // from 0 to 1 (for growthfactor>1):
                if (numElements % 2 == 0)
                    odd = false;
                else
                    odd = true;
                if (odd)
                {
                    // Odd number of elements, factor 0 is included; calculate normalizing
                    // factor for the first interval length:
                    intervalLength = 1.0;
                    sum = 0.0;
                    for (i = 1; i <= (numElements - 1) / 2; ++i)
                    {
                        sum += 2 * intervalLength;
                        intervalLength /= growthFactor;
                    }
                    // Calculate factors: 
                    currentFactor = -1.0;
                    intervalLength = 2.0 / sum;
                    elementIndex = 0;
                    for (i = 1; i <= (numElements - 1) / 2; ++i)  // faling interval lengths before 0
                    {
                        ++elementIndex;
                        factors[elementIndex - 1] = currentFactor;
                        currentFactor += intervalLength;
                        intervalLength /= growthFactor;
                    }
                    ++elementIndex; //centered, the next interval is the same 
                    currentFactor = factors[elementIndex - 1] = 0;
                    for (i = 1; i <= (numElements - 1) / 2; ++i)  // growing interval length after 0
                    {
                        ++elementIndex;
                        intervalLength *= growthFactor;
                        currentFactor += intervalLength;
                        factors[elementIndex - 1] = currentFactor;
                    }
                }
                else
                {
                    // Even number of elements, 0 is excluded; calculate normalizing factor
                    // for the first interval length
                    intervalLength = 1.0;
                    sum = 0.0;
                    for (i = 1; i <= (numElements / 2) - 1; ++i)
                    {
                        sum += 2 * intervalLength;
                        intervalLength /= growthFactor;
                    }
                    sum += intervalLength;


                    //sum = 0.0;
                    //currentFactor = -1.0;
                    //intervalLength = 2.0 / sum;
                    //elementIndex = 0;
                    //for (i = 1; i <= (numElements) / 2; ++i)  // faling interval length before 0
                    //{
                    //    factors[elementIndex] = currentFactor;
                    //    currentFactor += intervalLength;
                    //    intervalLength /= growthFactor;
                    //    ++elementIndex;
                    //}
                    ////intervalLength *= growthFactor;
                    //for (i = 1; i <= (numElements) / 2; ++i)  // growing interval length after 0
                    //{
                    //    factors[elementIndex - 1] = currentFactor;
                    //    intervalLength *= growthFactor;
                    //    currentFactor += intervalLength;
                    //    ++elementIndex;
                    //}


                    /* Calculate factors: */
                    currentFactor = -1.0;
                    intervalLength = 2.0 / sum;
                    elementIndex = 0;
                    for (i = 1; i <= (numElements) / 2; ++i)  // faling interval length before 0
                    {
                        factors[elementIndex] = currentFactor;
                        currentFactor += intervalLength;
                        intervalLength /= growthFactor;
                        ++elementIndex;
                    }
                    intervalLength *= growthFactor;
                    for (i = 1; i <= (numElements) / 2; ++i)  // growing interval length after 0
                    {
                        factors[elementIndex] = currentFactor;
                        intervalLength *= growthFactor;
                        currentFactor += intervalLength;
                        ++elementIndex;
                    }
                }
            }
            else  // not centered
            {
                // Calculate the normalizing factor for the first interval: 
                intervalLength = 1.0;
                sum = 0;
                for (i = 1; i < numElements; ++i)
                {
                    sum += intervalLength;
                    intervalLength *= growthFactor;
                }
                // Calculate the factors: 
                intervalLength = 1.0 / sum;
                currentFactor = 0.0;
                elementIndex = 0;
                for (i = 1; i <= numElements; ++i)
                {
                    ++elementIndex;
                    factors[elementIndex - 1] = currentFactor;
                    currentFactor += intervalLength;
                    intervalLength *= growthFactor;
                }
            }
            // Scale the factors: 
            if (scalingFactor != 1)
            {
                for (i = 1; i <= numElements; ++i)
                    factors[i - 1] *= scalingFactor;
            }
        }  // CalculateTableUnitFactors

        /// <summary>Calculates factors for a table of values between two points, and stores
        /// them to a list. Factors are calculated according to values of a function of one variable, 
        /// evaluated in equidistant points on the specified interval, and scaled such that they lie 
        /// between 0 and 1. It is caller's responsibility to provide a monotonous function, otherwise 
        /// some factors can lie outside the [0,1] interval and are not ordered.</summary>
        /// <param name="func">A real-valued function of a real variable that defines distribution of factors. The calculated 
        /// factors are function values </param>
        /// <param name="numElements">Number of sampling points (elements). It should be greater than 1.</param>
        /// <param name="firstFunctionArgument">Lower bound of function arguments.</param>
        /// <param name="lastFunctionArgument">Upper bound for function arguments.</param>
        /// <param name="factors">Ouptput - list where factors are stored.</param>
        /// $A Igor Apr10;
        public static void CalculateGridUnitFactors(int numElements, IRealFunction func, 
            double firstFunctionArgument, double lastFunctionArgument, ref List<double> factors)
        {
            if (numElements < 2)
            {
                numElements = 2;
                throw new ArgumentException("Wrong number of elements in a grid (" + numElements
                    + "), should be at least 2.\n");
            }
            if (func == null)
                throw new ArgumentNullException("Function for evaluation of grid factors is not defined.");
            else if (!func.ValueDefined)
                throw new ArgumentException("Function for evaluation of grid factors doesn't have its value defined. Function name. "
                    + func.Name + ".");
            else if (firstFunctionArgument == lastFunctionArgument)
                throw new ArgumentException("Ïnterval on which grid distribution function is evaluated hay zero length.");
            Util.ResizeList(ref factors, numElements, 0.0);
            double firstValue = factors[0] = func.Value(firstFunctionArgument);
            double lastValue = factors[numElements - 1] = func.Value(lastFunctionArgument);
            double valueScalingFactor = 1.0/ ( (lastValue-firstValue) * (double) (numElements-1) );
            double intervalLength = 1.0*(lastFunctionArgument-firstFunctionArgument)/(double) (numElements - 1);
            for (int i=1; i<numElements-1; ++i)
                factors[i] = func.Value(firstFunctionArgument + (double) i * intervalLength);
            for (int i=0; i<numElements; ++i)
                factors[i] = (factors[i]-firstFunctionArgument) * (double) i * valueScalingFactor;
        }  // CalculateTableUnitFactors


        /// <summary>Calculates lengths of subsequent intervals defined by the first list, 
        /// and stores it to the second list.</summary>
        /// <param name="nodes">List containing nodes that define intervals.</param>
        /// <param name="lengths">List where interval lengths are stored.</param>
        public static void CalculateIntervalLenghts(List<double> nodes, ref List<double> lengths)
        {
            if (nodes.Count < 1)
            {
                lengths = null;
                return;
            }
            Util.ResizeList(ref lengths, nodes.Count - 1, 0.0, true);
            for (int i = 1; i < nodes.Count; ++i)
            {
                double l = nodes[i] - nodes[i - 1];
                lengths[i - 1] = l;
            }
        }

        /// <summary>Calculates length ratios between subsequent intervals defined by the first list, 
        /// ans stores it to the second list.</summary>
        /// <param name="nodes">List containing nodes that define intervals.</param>
        /// <param name="lengthRatios">List where interval length ratios are stored.
        /// This list has two elements less than the original list.</param>
        public static void CalculateIntervalLenghRatios(List<double> nodes, ref List<double> lengthRatios)
        {
            List<double> lengths = null;
            CalculateIntervalLenghts(nodes, ref lengths);
            if (lengths == null)
            {
                lengthRatios = null;
                return;
            }
            if (lengths.Count < 1)
            {
                lengthRatios = null;
                return;
            }
            Util.ResizeList(ref lengthRatios, lengths.Count - 1, 0.0, true);
            for (int i = 1; i < lengths.Count; ++i)
                lengthRatios[i - 1] = lengths[i] / lengths[i - 1];
        }


        /// <summary>Examples for calculating table factors.</summary>
        public static void ExampleTableFactors()
        {
            Console.WriteLine("\nAnalysisTable.ExampleTableFactors():\n");
            List<double> factors = null;
            List<double> lengths = null;
            List<double> lengthRatios = null;
            int numElements = 4;
            bool centered = false;
            double growthFactor = 2.0;
            double scalingFactor = 1.0;

            centered = false;
            numElements = 2;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

            centered = false;
            numElements = 3;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

            centered = false;
            numElements = 4;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

            centered = false;
            numElements = 7;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

            centered = false;
            numElements = 8;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

            centered = true;
            numElements = 2;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

            centered = true;
            numElements = 3;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

            centered = true;
            numElements = 4;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

            centered = true;
            numElements = 5;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

            centered = true;
            numElements = 6;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

            centered = true;
            numElements = 8;
            CalculateGridUnitFactors(numElements, centered, growthFactor,
                scalingFactor, ref factors);
            CalculateIntervalLenghts(factors, ref lengths);
            CalculateIntervalLenghRatios(factors, ref lengthRatios);
            Console.WriteLine("\n" + (centered ? "Centered table: " : "Ordinary table:"));
            Console.WriteLine("Num. elements:  " + numElements);
            Console.WriteLine("Growth factor:  " + growthFactor);
            Console.WriteLine("Scaling factor: " + scalingFactor);
            Console.WriteLine("Factors: ");
            Console.WriteLine(Util.ListToString(factors));
            Console.WriteLine("Length ratios: ");
            Console.WriteLine(Util.ListToString(lengthRatios));

        }

        /// <summary>Grid stretching to fit the specified interval bounds.
        /// Modifies position of the generated 1d grid node in such a way that it fits the new grid interval.</summary>
        /// <param name="minCurrent">Lower bound of the interval containing the current grid.</param>
        /// <param name="maxCurrent">Upper bound of the interval containing the current grid.</param>
        /// <param name="minTarget">Lower bound of the interval containing the fitted (stretched) grid.</param>
        /// <param name="maxTarget">Lower bound of the interval containing the fitted (stretched) grid.</param>
        /// <param name="node">Position of node to be modified according to grid stretching.</param>
        public static void FitGridNode(double minCurrent, double maxCurrent,
            double minTarget, double maxTarget, ref double node)
        {
            node = minTarget + (node-minCurrent)*(maxTarget-minTarget)/(maxCurrent-minCurrent);
        }

        /// <summary>Grid stretching to fit the specified interval bounds.
        /// Modifies positions of the generated 1d grid nodes contained in the specified list 
        /// in such a way that it fits the new grid interval.</summary>
        /// <param name="minCurrent">Lower bound of the interval containing the current grid.</param>
        /// <param name="maxCurrent">Upper bound of the interval containing the current grid.</param>
        /// <param name="minTarget">Lower bound of the interval containing the fitted (stretched) grid.</param>
        /// <param name="maxTarget">Lower bound of the interval containing the fitted (stretched) grid.</param>
        /// <param name="nodes">List containing positions of node to be modified according to grid stretching.</param>
        public static void FitGridNodes(double minCurrent, double maxCurrent,
            double minTarget, double maxTarget, List<double> nodes)
        {
            if (nodes != null)
                for (int i = 0; i < nodes.Count; ++i)
                {
                    double nodePos = nodes[i];
                    FitGridNode(minCurrent, maxCurrent, minTarget, maxTarget, ref nodePos);
                    nodes[i] = nodePos;
                }
        }

        #endregion AuxiliaryUtilities


        #region Operation



        /// <summary>Clears the results of grid generation (releases internal structures).
        /// The <see cref="Calculated"/> flag is set to false.</summary>
        public virtual void ClearResults()
        {
            lock (Lock)
            {
                Calculated = false;
                _nodes = null;
            }
        }

        /// <summary>Performs grid generation and stores the generated nodes directly on the provided list.
        /// Unless necessary due to nature of generation, results are not stored internally on the current grid generator object.
        /// Because of this, the Calculated flag is normally not set after calling this function.
        /// WARNING: This method generates a grid even if it has already been generated and is up to date.</summary>
        /// <param name="nodeList">A list where node coordinates are stored.
        /// List is allocated or re-allocated if necessary.</param>
        public abstract void CalculateGrid(ref List<double> nodeList);


        /// <summary>Performs grid generation and stores the generated nodes and intervals between them directly on the provided lists.
        /// Unless necessary due to nature of generation, results are not stored internally on the current grid generator object.
        /// Because of this, the Calculated flag is normally not set after calling this function.
        /// WARNING: This method generates a grid even if it has already been generated and is up to date.</summary>
        /// <param name="nodeList">A list where positions of the generated grid are stored.</param>
        /// <param name="intervalLengthsList">A list where lengths of intervals between grid nodes are stored.</param>
        public virtual void CalculateGrid(ref List<double> nodeList, ref List<double> intervalLengthsList)
        {
            lock (Lock)
            {
                CalculateGrid(ref nodeList);
                CalculateIntervalLenghts(nodeList, ref intervalLengthsList);
            }
        }


        /// <summary>Performs grid generation according to current settings. 
        /// The generated grid is stored in internal structures of the generator, from where it can be readily copied
        /// (e.g. by GetNodeTable() or by GetNodeList()), used in some other way (e.g. by GetIntervalLengthsList) 
        /// or just referenced (e.g. by GetNodeListReference()).
        /// If the grid has already been generated and it is up to date (according to parameters of the generator) then
        /// the grid is not re-calculated.</summary>
        public virtual void CalculateGrid()
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid(ref _nodes);
                Calculated = true;
            }
        }

        /// <summary>Returns the specified node of the generated grid.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="which">Specifies which node (counted from 0) is to be returned.</param>
        /// <returns>Position of the specified node of the grid.</returns>
        public virtual double GetNode(int which)
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid();
                if (which < 0 || which >= NumNodes)
                    throw new ArgumentException("Node index (" + which + ") out of range , should be between 0 and " + NumNodes + ".");
                return _nodes[which];
            }

        }

        /// <summary>Stores the node positions of the generated 1D grid into the specified table.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="result">Table where node positions of the generated grid are stored. Allocated or relocated if necessary.</param>
        public virtual void GetNodeTable(ref double[] result)
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid();
                Util.CopyList<double>(_nodes, ref result);
            }
        }

        /// <summary>Returns a table containing 1D node positions.
        /// A copy of list of node positions is always created and returned, therefore it is guaranteed that this table will not
        /// be used by some other object related to the grid generator.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        public virtual double[] GetNodeTable()
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid();
                double[] ret = null;
                Util.CopyList<double>(_nodes, ref ret);
                return ret;
            }
        }

        /// <summary>Stores the node positions of the generated 1D grid into the specified list.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="result">List where node positions of the generated grid are stored. Allocated or relocated if necessary.</param>
        public virtual void GetNodeList(ref List<double> result)
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid();
                Util.CopyList<double>(_nodes, ref result);
            }
        }

        /// <summary>Returns a list containing 1D node positions.
        /// A copy of list of node positions is always created and returned, therefore it is guaranteed that this table will not
        /// be used by some other object related to the grid generator.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <returns></returns>
        public virtual List<double> GetNodeList()
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid();
                List<double> ret = null;
                Util.CopyList<double>(_nodes, ref ret);
                return ret;
            }
        }

        /// <summary>Returns a list containing generated 1D node positions.
        /// This method can return a reference to the internal list containing the generated node coordinated.
        /// This means that any future generation or other operation performed by the generator can override list contents.
        /// In order to use contents of the list thread safely, enclose this method call and all subsequent operations you 
        /// will perform on the returned list of generated nodes, within a lock((...).Lock){ ... } block.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <returns>A reference to a list of generated nodes. This is usually not a copy of the list but a reference to
        /// an internal structure that can be overridden by subsequent operations on the current grid generator.</returns>
        public virtual List<double> GetNodeListReference()
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid();
                return _nodes;
            }
        }


        /// <summary>Stores a table of interval lengths between the generated nodes into the specified array.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="result">Array where interval lengths between nodes of the generated grid are stored. Allocated or relocated if necessary.</param>
        public virtual void GetIntervalLengthsTable(ref double[] result)
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid();
                List<double> lengths = null;
                CalculateIntervalLenghts(_nodes, ref lengths);
                Util.CopyList(lengths, ref result);
            }
        }

        /// <summary>Generates and returns a table of interval lengths between the generated nodes.
        /// The returned table is a copy that will not be overridden by further operations on the grid generator.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <returns>A table containing lengths of intervals between the subsequent nodes generated by the current grid generator.</returns>
        public virtual double[] GetIntervalLengthsTable()
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid();
                List<double> lengths = null;
                double[] ret = null;
                CalculateIntervalLenghts(_nodes, ref lengths);
                Util.CopyList(lengths, ref ret);
                return ret;
            }
        }

        /// <summary>Stores a table of interval lengths between the generated nodes into the specified list.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="result">List where interval lengths between nodes of the generated grid are stored. Allocated or relocated if necessary.</param>
        public virtual void GetIntervalLengthsList(ref List<double> result)
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid();
                CalculateIntervalLenghts(_nodes, ref result);
            }
        }

        /// <summary>Generates and returns a list of interval lengths between the generated nodes.
        /// The returned list is a copy that will not be overridden by further operations on the grid generator.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <returns>A table containing lengths of intervals between the subsequent nodes generated by the current grid generator.</returns>
        public virtual List<double> GetIntervalLengthsList()
        {
            lock (Lock)
            {
                if (!Calculated)
                    CalculateGrid();
                List<double> lengths = null;
                CalculateIntervalLenghts(_nodes, ref lengths);
                return lengths;
            }
        }

        /// <summary>Returns the length of the specified interval between generated nodes of the grid.
        /// If necessary, grid is generated first in order to obtain the correct data.</summary>
        /// <param name="which">Consecutive number of the interval between nodes.</param>
        public virtual double GetIntervalLength(int which)
        {
            lock (Lock)
            {
                if (which < 0 || which > NumIntervals)
                    throw new ArgumentException("Interval index (" + which + ") out of range , should be between 0 and " + NumIntervals + ".");
                if (!Calculated)
                    CalculateGrid();
                return _nodes[which + 1] - _nodes[which];
            }
        }


        #endregion Operation

    } // abstract class GridGenerator1dBase





}