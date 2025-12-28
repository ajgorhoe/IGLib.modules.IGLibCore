
#nullable disable

// GRAPHICAL OBJECTS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using IG.Lib;
using IG.Num;



namespace IG.Num
{


    /// <summary>Bounding box, defines minimum and maximum co-ordinates of domains, geometric objects and their groups.</summary>
    /// $A Igor Mar10 Nov10;
    public interface IBoundingBox
    {

        #region Data

        /// <summary>Gets dimension of the bounding box space.</summary>
        /// <remarks>Setter is not public. In order to change dimension, use the ChangeDimensionAndReset() method!</remarks>
        int Dimension
        {
            get;
        }


        /// <summary>Changes dimension of the current bounding box to the specified dimension
        /// and resets the bounding box. Existent bounds information is lost.</summary>
        /// <param name="newDimension">New dimenson of the bounding box.</param>
        void SetDimensionAndReset(int newDimension);


        /// <summary>Gets or sets the vector of minimal co-ordinates of the bounding box.
        /// <para>Getter always creates and returns a new vector whose elements are minimal components of the bounding box.</para>
        /// <para>Setter copies components rather than vector reference (if the vector assigned is null then 
        /// exception is thrown).
        /// When setting the vector, its dimension must match current dimension of the bounding box, 
        /// otherwise exception is thrown.</para></summary>
        /// <remarks>If dimension needs to be changed then one must set the Dimension property first. This 
        /// will also reset the bounding box, and a more transparent approach is to create a new bounding
        /// box.</remarks>
        IVector Min
        {
            get;
            set;
        }

        /// <summary>Gets or sets the vector of maximal co-ordinates of the bounding box.
        /// <para>Getter always creates and returns a new vector whose elements are maximal components of the bounding box.</para>
        /// <para>Setter copies components rather than vector reference (if the vector assigned is null then 
        /// exception is thrown). 
        /// When setting the vector, its dimension must match current dimension of the bounding box, 
        /// otherwise exception is thrown.</para></summary>
        /// <remarks>If dimension needs to be changed then one must set the Dimension property first. This 
        /// will also reset the bounding box, and a more transparent approach is to create a new bounding
        /// box.</remarks>
        IVector Max
        {
            get;
            set;
        }

        #endregion Data


        #region Operation

        /// <summary>Resets all components of vector of minimal coordinates to <see cref="BoundingBoxBase.UndefinedMin"/>,
        /// and all components of vector of maximal coordinates to <see cref="BoundingBoxBase.UndefinedMax"/>.
        /// <para>After this method is called, Update() called on any co-ordinate of the bounding box
        /// will set both bounds of that coordinate exactly to the value of coordinate passed as argument.</para></summary>
        /// <remarks>After reset, bounds on the reset coordinates become undefined.</remarks>
        void Reset();

        /// <summary>Resets the specified component of vector of minimal coordinates to <see cref="BoundingBoxBase.UndefinedMin"/>,
        /// and the same component of vector of maximal coordinates to <see cref="BoundingBoxBase.UndefinedMax"/>.
        /// <para>After this method is called, the first Update() called on this component  
        /// will set the bounds exactly to value that is passed as argument.</para></summary>
        /// <remarks>After reset, bounds on the reset coordinates become undefined.</remarks>
        void Reset(int componentIndex);

        /// <summary>Resets the specified minimal coordinate value to <see cref="BoundingBoxBase.UndefinedMin"/>.
        /// <para>After this method is called, the first update called on this coordinate component  
        /// will set its lower bound exactly to the value that is passed as argument.</para></summary>
        /// <remarks>After reset, specific bounds on the reset coordinates become undefined.</remarks>
        void ResetMin(int componentIndex);

        /// <summary>Resets the specified maximal coordinate value to <see cref="BoundingBoxBase.UndefinedMax"/>.
        /// <para>After this method is called, the first update called on this coordinate component  
        /// will set its upper bound exactly to the value that is passed as argument.</para></summary>
        /// <remarks>After reset, specific bounds on the reset coordinates become undefined.</remarks>
        void ResetMax(int componentIndex);

        /// <summary>Returns true if the speciifed co-ordinates lie outside of the bounding box,
        /// and false otherwise. 
        /// <para>If the specified coordinates are null then false is returned, if dimensions do not match then exception is thrown.</para></summary>
        /// <param name="coordinates">Coordinates that are tested for falling outside the bounding box.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        bool LiesOutside(IVector coordinates);

        /// <summary>Returns true if the speciifed co-ordinates lie outside of the bounding box,
        /// and false otherwise. If the specified coordinates are null then false is returned.</summary>
        /// <param name="coordinates">Coordinates that are tested for falling outside the bounding box.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        bool LiesOutside(params double[] coordinates);

        /// <summary>Returns true of the specified co-ordinate lies outside of the bounding box, and false otherwise.</summary>
        /// <param name="componentIndex">Index of co-ordinate component that is tested for lying out of the bounding box.</param>
        /// <param name="coordinate">Co-ordinate component that is tested for lying outside of the bounding box.</param>
        bool LiesOutside(int componentIndex, double coordinate);

        /// <summary>Returns true if the specified bounding box lise outside of the current bounding box,
        /// and false otherwise. If the specified bounding box is null then false is returned.</summary>
        /// <param name="bounds">Bounding box that is tested for falling outside the current bounding box.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        bool LiesOutside(IBoundingBox bounds);

        /// <summary>Updates the bounding box in such a way that the specified vector fits in it.
        /// If the specified vector is null then this method has no effect.</summary>
        /// <param name="coordinates">Vector that needs to fit into the bounding box after this call.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        void Update(IVector coordinates);
        
        /// <summary>Updates the bounding box in such a way that all specified vectors fit in it.
        /// <para>If any of the specified vector is null then this entry has no effect, and if 
        /// the array is null then the method call has no effect.</para></summary>
        /// <param name="points">Vector of coordinates that need to fit into the bounding box after this call.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        void Update(params IVector[] points);

        /// <summary>Updates the bounding box in such a way that a vector with the specified co-ordinates fits in it.
        /// <para>If the specified array of co-ordinate values is null then this method has no effect.</para>
        /// <para>Number of specified co-ordinates must match dimension of the bounding box.</para></summary>
        /// <param name="coordinates">Array of co/ordinate components of the vector to which the bounding box
        /// is adapted in such a way that the vector fits in the bounding box after the call.
        /// <para>Number of specified co-ordinates must match the dimension of the bounding box.</para></param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        void Update(params double[] coordinates);

        /// <summary>Updates the bounding box in such a way that the specified value of the specified 
        /// co-ordinate component (defined by co-ordinate index) fits in it.</summary>
        /// <param name="componentIndex">Index of the co-ordinate component that is updated.</param>
        /// <param name="coordinate">Co-ordinate value to which the bounding box is adapted 
        /// in such a way that this co-ordinate value fits inside it.</param>
        void Update(int componentIndex, double coordinate);

        /// <summary>Updates the bounding box in such a way that all specified values of the specified 
        /// co-ordinate component fit in it.
        /// <para>If the specified array of the coordinate values is null then this method has no effect.</para>
        /// <para>Update is performed for a particular coordinate of the bounding box, and all specified 
        /// values are taken into account.</para></summary>
        /// <param name="componentIndex">Index of co-ordinate component in which the bounding box is updated.</param>
        /// <param name="coordinates">Values of co-ordinate component to which the bounding box is adapted in such a way
        /// that these values fit inside it (in the specified co-ordinate direction).</param>
        void Update(int componentIndex, params double[] coordinates);

        /// <summary>Updates the bounding box in such a way that the specified other bounding box fits in it.
        /// If the specified bounding box is null then this method has no effect.</summary>
        /// <param name="bounds">Bounding box that needs to fit into bounding box after this call.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        void Update(IBoundingBox bounds);
        
        /// <summary>Updates the current bounding box in such a way that in all components, all specified 
        /// coordinates fit in it.
        /// This is for example useful to define a hypercube (equal minimum and maximum in all components),
        /// in this case just minimal and maximal bounds are specified as arguments.</summary>
        /// <param name="coordinates">Co-ordinates to which the bounding box is adapted in all axis directions in such a way
        /// that the specified co-ordinates fit inside it.</param>
        void UpdateAll(params double[] coordinates);
        
        /// <summary>Reduces (if necessary) the current bounding box in such a way that the specified bounding
        /// box contains it.
        /// <para>Minimal and maximal components of the current bounding box that don't fall in the specified
        /// bounding box are changed such that they lie just on the border of the bounding box, and others remain intact.</para></summary>
        /// <param name="outerBounds">Bounding box that specify the area within which the current bounding box must fit.
        /// Components that fall out of this bounding box are moved to the border of this bounding box (and the current
        /// bounding box shrinks because of this).</param>
        void Shrink(IBoundingBox outerBounds);

        /// <summary>Reduces (if necessary) the current bounding box in such a way that the specified component
        /// falls within the interval specified by the lower and upper bound.</summary>
        /// <param name="which">Index of the component along which the bounding box is reduced if nexessary.</param>
        /// <param name="minBound">Lower bound on the interval that is admissible in the specified coordinate direction.</param>
        /// <param name="maxBound">Upper bound on the interval that is admissible in the specified coordinate direction.</param>
        void Shrink(int which, double minBound, double maxBound);

        /// <summary>Symmetrically expands or shrinks the interval between the bounds for the specified component
        /// for the specified factor. 
        /// Factors greater than 1 mean expansion, factors lesser than 1 mean shrinkage, and fators lesser than 0 are invalid.</summary>
        /// <param name="componentIndex">Component of the coordinate to be expanded or shrinked.</param>
        /// <param name="factor">Factor by which the interval between bounds is expanded or shrinked. 
        /// Factors greater than 1 mean expansion, factors less than 1 mean shrinkage, and factors less than 0 are invalid (ArgumentException thrown).</param>
        void ExpandOrShrinkInterval(int componentIndex, double factor);
        
        /// <summary>Symmetrically expands or shrinks the intervals between the bounds for all components
        /// for the specified factor, and takes care that minimal interval lengths are not 0. 
        /// Factors greater than 1 mean expansion, factors lesser than 1 mean shrinkage, and fators lesser than 0 are invalid.
        /// Remark: components where minimal bound is set above maximal are not treated specially.</summary>
        /// <param name="factor">Factor by which the interval between bounds is expanded or shrinked. 
        /// Factors greater than 1 mean expansion, factors less than 1 mean shrinkage, and factors less than 0 are invalid (ArgumentException thrown).</param>
        /// <param name="zeroIntervalLengthReplacemnt">Interval length that is taken for those components for which the current
        /// interval length equals 0 (i.e. lower bound equals upper bound).</param>
        void ExpandOrShrinkInterval(double factor, double zeroIntervalLengthReplacemnt);

        /// <summary>Symmetrically expands or shrinks the intervals between the bounds for all components
        /// for the specified factor. 
        /// Factors greater than 1 mean expansion, factors lesser than 1 mean shrinkage, and fators lesser than 0 are invalid.</summary>
        /// <param name="factor">Factor by which the interval between bounds is expanded or shrinked. 
        /// Factors greater than 1 mean expansion, factors less than 1 mean shrinkage, and factors less than 0 are invalid (ArgumentException thrown).</param>
        void ExpandOrShrinkInterval(double factor);

        /// <summary>Sets minimal value for the specified coponent of the bounding box.</summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="min">Minimal value of the component.</param>
        void SetMin(int componentIndex, double min);

        /// <summary>Sets maximal value for the specified coponent of the bounding box.</summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="max">Maximal value of the component.</param>
        void SetMax(int componentIndex, double max);

        /// <summary>Sets minimal and maximal value for the specified coponent of the bounding box.</summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="min">Minimal value of the component.</param>
        /// <param name="max">Maximal value of the component.</param>
        void SetBounds(int componentIndex, double min, double max);

        /// <summary>Returns the lower bound on the specified co-ordinate component of the bounding box.
        /// <para>If lower bound is not defined for this component then <see cref="double.MinValue"/> is returned.</para></summary>
        /// <param name="which">Component index.</param>
        double GetMin(int which);

        /// <summary>Returns the upper bound on the specified co-ordinate component of the bounding box.
        /// <para>If upper bound is not defined for this component then <see cref="double.MaxValue"/> is returned.</para></summary>
        /// <param name="which">Component index.</param>
        double GetMax(int which);

        /// <summary>Gets the vector of lower bounds on co-ordinates of the current bounding box and stores it in the 
        /// specified object. 
        /// <para>Components for which lower bounds are not defined are set to <see cref="double.MinValue"/>.</para></summary>
        /// <param name="min">Reference to a vector object where lower bounds (minimal co-ordinates) are stored.</param>
        void GetMin(ref IVector min);

        /// <summary>Gets the vector of upper bounds on co-ordinates of the current bounding box and stores it in the 
        /// specified object. 
        /// <para>Components for which upper bounds are not defined are set to <see cref="double.MaxValue"/>.</para></summary>
        /// <param name="max">Reference to a vector object where upper bounds (maximal co-ordinates) are stored.</param>
        void GetMax(ref IVector max);

        /// <summary>Returns interval length for the specified co-ordinate component (i.e. difference between the upper and lower
        /// bound), or 0 if either lower or upper bound is not defined for this component.</summary>
        /// <param name="componentIndex">Specifies the component for which interval length is returned.</param>
        double GetIntervalLength(int componentIndex);

        /// <summary>Gets the interval lengths for co-ordinates of the current bounding box (i.e. differences) 
        /// and stores them in the specified vector object. 
        /// <para>Elements for which either upper or lower bounds are not defined are set to 0.</para></summary>
        /// <param name="intervals">Reference to a vector object where interval lengths are stored.</param>
        void GetIntervalLengths(ref IVector intervals);
        
        /// <summary>Calculates relative coordinates, with respect to the current bounding box, that
        /// correspond to the specified absolute (or physical or actual) coordinates.</summary>
        /// <param name="absoluteCoordinates">Vector of absolute coordinates.</param>
        /// <param name="relativeCoordinates">Vector where corresponding relative coordinates are stored.</param>
        void GetRelativeCoordinates(IVector absoluteCoordinates, ref IVector relativeCoordinates);
        
        /// <summary>Calculates absolute (physical) coordinates, with respect to the current bounding box, that
        /// correspond to the specified relative coordinates.</summary>
        /// <param name="absoluteCoordinates">Vector of relative coordinates.</param>
        /// <param name="relativeCoordinates">Vector where corresponding absolute coordinates are stored.</param>
        void GetAbsoluteCoordinates(IVector relativeCoordinates, ref IVector absoluteCoordinates);

        /// <summary>Returns true if maximum value is defined for the specified component, false otherwise.</summary>
        /// <param name="componentIndex">Index of component that is queried.</param>
        bool IsMinDefined(int componentIndex);

        /// <summary>Returns true if maximum value is defined for the specified component, false otherwise.</summary>
        /// <param name="componentIndex">Index of component that is queried.</param>
        bool IsMaxDefined(int componentIndex);

        #endregion Operation 


        #region Utilities

        /// <summary>Creates a random point contained in the current bounding box by using the
        /// specified random number generator, and stores that point into the specified vector.
        /// The random point is generated according to a uniform distribution within the bounding box.</summary>
        /// <param name="point">Vector where generated random point is stored.</param>
        /// <param name="rand">Random number generator used for generation of the random point.</param>
        void GetRandomPoint(ref IVector point, IRandomGenerator rand);

        /// <summary>Creates a random point contained in the current bounding box by using the
        /// global random number generator, and stores that point into the specified vector.
        /// The random point is generated according to a uniform distribution within the bounding box.</summary>
        /// <param name="point">Vector where generated random point is stored.</param>
        void GetRandomPoint(ref IVector point);

        #endregion Utilities

    }  // interface IBoundingBox


    /// <summary>Base class for bounding boxes that define minimal and maximal co-ordinates of 
    /// objects, groups of objects, and regions in space.</summary>
    /// $A Igor Mar10 Nov10;
    public abstract class BoundingBoxBase : IBoundingBox
    {

        #region Construction

        /// <summary>Creates a bounding box of the specified dimension and initializes it in such a way that
        /// minimal co-ordinate components equal to <see cref="BoundingBoxBase.UndefinedMin"/> and maximal co-ordinate components 
        /// equal do <see cref="BoundingBoxBase.UndefinedMax"/>.</summary>
        /// <param name="dimension">Dimension of the space in which bounding box is defined.</param>
        public BoundingBoxBase(int dimension)
        {
            // Remark: Setting dimension also allocates internal vector of minimal and maximal values 
            // and resets them
            this.Dimension = dimension;
        }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified vector.</summary>
        /// <param name="coordinates">Vector of coordinates to which minimal and maximal co-ordines of the 
        /// bounding box are set.</param>
        public BoundingBoxBase(IVector coordinates)
            : this(coordinates.Length)
        {
            Update(coordinates);
        }

        /// <summary>Constructs a bounding box of the specified dimension and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified vectors.</summary>
        /// <param name="min">Vector of minimal coordinates.</param>
        /// <param name="max">Vector of maximal coordinates.</param>
        public BoundingBoxBase(IVector min, IVector max)
            : this(min.Length)
        {
            Update(min);
            Update(max);
        }

        /// <summary>Constructs a bounding box of the specified dimension and initializes it in such a way that
        /// its bounds correspond to those of the specified bounding box.
        /// <para>The specified bounding box must be different than null, otherwise exception is thrown.</para></summary>
        /// <param name="bounds">Bounds with which the constructed bounding box is initialized.</param>
        public BoundingBoxBase(IBoundingBox bounds)
            : this(bounds.Dimension)
        {
            Update(bounds);
        }


        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified array.</summary>
        /// <param name="coordinates">Array of coordinates to which minimal and maximal co-ordines of the 
        /// bounding box are set.</param>
        public BoundingBoxBase(double[] coordinates)
            : this(coordinates.Length)
        {
            for (int i = 0; i < Dimension; ++i)
            {
                MinAuxiliary[i] = coordinates[i];
                MaxAuxiliary[i] = coordinates[i];
            }
        }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified arrays.</summary>
        /// <param name="min">Array of components to which minimal co-ordines of the bounding box are set.</param>
        /// <param name="max">Array of components to which maximal co-ordines of the bounding box are set.</param>
        public BoundingBoxBase(double[] min, double[] max)
            : this(min.Length)
        {
            for (int i = 0; i < Dimension; ++i)
            {
                MinAuxiliary[i] = min[i];
                MaxAuxiliary[i] = max[i];
            }
        }

        #endregion Construction


        #region Data

        protected const double UndefinedMin = double.MinValue;
        protected const double UndefinedMax = double.MaxValue;

        IVector _minAux, _maxAux;

        /// <summary>Auxiliary property, gets or sets vector of minimal co-ordinates.
        /// <para>Setter just sets reference to the vector passed and does not create a copy.</para></summary>
        protected IVector MinAuxiliary
        {
            get { return _minAux; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Invalid attempt to assign null vector to bounding box minimal co-ordinates.");
                else if (value.Length == 0)
                    throw new ArgumentException("Bounding box minimum vector assigned with invalid dimension (" + value.Length
                        + ", should be " + Dimension + ".");
                _minAux = value;
                // Update bounding box dimension according to the value set. If dimension changes then 
                // internal vectors of minimal and maximal values will be re-allocated and reset automatically.
                Dimension = value.Length;
            }
        }

        /// <summary>Auxiliary property, gets or sets vector of maximal co-ordinates.
        /// <para>Setter just sets reference to the vector passed and does not create a copy.</para></summary>
        protected IVector MaxAuxiliary
        {
            get { return _maxAux; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Invalid attempt to assign null vector to bounding box minimal co-ordinates.");
                else if (value.Length == 0)
                    throw new ArgumentException("Bounding box minimum vector assigned with invalid dimension (" + value.Length
                        + ", should be " + Dimension + ".");
                _maxAux = value;
                Dimension = value.Length;
            }
        }



        #region ToOverride

        /// <summary>Creates and returns a new vector that is consistent with the definition of bounding box.</summary>
        /// <param name="dim">Dimension of vector space.</param>
        protected abstract IVector CreateVector(int dim);

        #endregion ToOverride


        int _dimension = 0;


        /// <summary>Gets dimension of the bounding box space.</summary>
        /// <remarks>Setter is not public. In order to change dimension, use the ChangeDimensionAndReset() method!
        /// This method can not be overridden.
        /// Setter is protected. If dimension is set to a new value then internal vectors of minimal
        /// and maximal values are allocated anew and reset.</remarks>
        public int Dimension
        {
            get { return _dimension; }
            protected set
            {
                if (value <= 0)
                    throw new ArgumentException("Dimension of bounding box space can not be less or equal to zero.");
                _dimension = value;
                if (MinAuxiliary == null)
                {
                    MinAuxiliary = CreateVector(_dimension);
                    ResetMin();
                }
                else if (MinAuxiliary.Length != _dimension)
                {
                    MinAuxiliary = CreateVector(_dimension);
                    ResetMin();
                }
                if (MaxAuxiliary == null)
                {
                    MaxAuxiliary = CreateVector(_dimension);
                    ResetMax();
                }
                else if (MaxAuxiliary.Length != _dimension)
                {
                    MaxAuxiliary = CreateVector(_dimension);
                    ResetMax();
                }
            }
        }

        /// <summary>Changes dimension of the current bounding box to the specified dimension
        /// and resets the bounding box. Existent bounds information is lost.</summary>
        /// <param name="newDimension">New dimenson of the bounding box.</param>
        public void SetDimensionAndReset(int newDimension)
        { 
            this.Dimension = newDimension; 
            this.Reset(); 
        }


        /// <summary>Gets or sets the vector of minimal co-ordinates of the bounding box.
        /// <para>Getter always creates and returns a new vector whose elements are minimal components of the bounding box.</para>
        /// <para>Setter copies components rather than vector reference (if the vector assigned is null then 
        /// exception is thrown).
        /// When setting the vector, its dimension must match current dimension of the bounding box, 
        /// otherwise exception is thrown.</para></summary>
        /// <remarks>If dimension needs to be changed then one must set the Dimension property first. This 
        /// will also reset the bounding box, and a more transparent approach is to create a new bounding
        /// box.</remarks>
        public virtual IVector Min
        {
            get
            {
                int dim = this.Dimension;
                IVector min = CreateVector(dim);
                for (int i = 0; i < dim; ++i)
                    min[i] = MinAuxiliary[i];
                return min;
            }
            set
            {
                int dim = this.Dimension;
                if (value == null)
                    throw new ArgumentNullException("Vector of minimal co-ordinates to be assigned was not specified (null reference).");
                if (value.Length != dim)
                    throw new ArgumentException("Dimension of assigned vector of minimal co-ordinates (" + value.Length
                        + ") does not match the bounding box dimension (" + dim + ").");
                for (int i = 0; i < dim; ++i)
                    MinAuxiliary[i] = value[i];
            }
        }

        /// <summary>Gets or sets the vector of maximal co-ordinates of the bounding box.
        /// <para>Getter always creates and returns a new vector whose elements are maximal components of the bounding box.</para>
        /// <para>Setter copies components rather than vector reference (if the vector assigned is null then 
        /// exception is thrown). 
        /// When setting the vector, its dimension must match current dimension of the bounding box, 
        /// otherwise exception is thrown.</para></summary>
        /// <remarks>If dimension needs to be changed then one must set the Dimension property first. This 
        /// will also reset the bounding box, and a more transparent approach is to create a new bounding
        /// box.</remarks>
        public virtual IVector Max
        {
            get
            {
                int dim = this.Dimension;
                IVector max = CreateVector(dim);
                for (int i = 0; i < dim; ++i)
                    max[i] = MaxAuxiliary[i];
                return max;
            }
            set
            {
                int dim = this.Dimension;
                if (value == null)
                    throw new ArgumentNullException("Vector of maximal co-ordinates to be assigned was not specified (null reference).");
                if (value.Length != dim)
                    throw new ArgumentException("Dimension of assigned vector of maximal co-ordinates (" + value.Length
                        + ") does not match the bounding box dimension (" + dim + ").");
                for (int i = 0; i < dim; ++i)
                    MaxAuxiliary[i] = value[i];
            }
        }

        #endregion Data


        #region Operation

        /// <summary>Sets all components of vector of minimal coordinates to <see cref="UndefinedMin"/>.</summary>
        protected void ResetMin()
        {
            if (MinAuxiliary != null)
                for (int i = 0; i < MinAuxiliary.Length; ++i)
                    MinAuxiliary[i] = UndefinedMin;
        }

        /// <summary>Sets all components of vector of maximal coordinates to <see cref="UndefinedMax"/>.</summary>
        protected void ResetMax()
        {
            if (MaxAuxiliary != null)
                for (int i = 0; i < MaxAuxiliary.Length; ++i)
                    MaxAuxiliary[i] = UndefinedMax;
        }


        /// <summary>Resets all components of vector of minimal coordinates to <see cref="BoundingBoxBase.UndefinedMin"/>,
        /// and all components of vector of maximal coordinates to <see cref="BoundingBoxBase.UndefinedMax"/>.
        /// <para>After this method is called, Update() called on any co-ordinate of the bounding box
        /// will set both bounds of that coordinate exactly to the value of coordinate passed as argument.</para></summary>
        /// <remarks>After reset, bounds on the reset coordinates become undefined.</remarks>
        public void Reset()
        {
            ResetMin();
            ResetMax();
        }

        /// <summary>Resets the specified component of vector of minimal coordinates to <see cref="UndefinedMin"/>,
        /// and the same component of vector of maximal coordinates to <see cref="UndefinedMax"/>.
        /// <para>After this method is called, the first Update() called on this component  
        /// will set the bounds exactly to value that is passed as argument.</para></summary>
        /// <remarks>After reset, bounds on the reset coordinates become undefined.</remarks>
        public void Reset(int componentIndex)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new IndexOutOfRangeException("Bounding box component reset: co-ordinate index " + componentIndex
                    + " is out of range (should be between 0 and " + (Dimension - 1) + ").");
            MinAuxiliary[componentIndex] = UndefinedMin;
            MaxAuxiliary[componentIndex] = UndefinedMax;
        }

        /// <summary>Resets the specified minimal coordinate value to <see cref="UndefinedMin"/>.
        /// <para>After this method is called, the first update called on this coordinate component  
        /// will set its lower bound exactly to the value that is passed as argument.</para></summary>
        /// <remarks>After reset, specific bounds on the reset coordinates become undefined.</remarks>
        public void ResetMin(int componentIndex)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new IndexOutOfRangeException("Bounding box minimal component reset: co-ordinate index " + componentIndex
                    + " is out of range (should be between 0 and " + (Dimension - 1) + ").");
            MinAuxiliary[componentIndex] = UndefinedMin;
        }

        /// <summary>Resets the specified maximal coordinate value to <see cref="UndefinedMax"/>.
        /// <para>After this method is called, the first update called on this coordinate component  
        /// will set its upper bound exactly to the value that is passed as argument.</para></summary>
        /// <remarks>After reset, specific bounds on the reset coordinates become undefined.</remarks>
        public void ResetMax(int componentIndex)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new IndexOutOfRangeException("Bounding box maximal component reset: co-ordinate index " + componentIndex
                    + " is out of range (should be between 0 and " + (Dimension - 1) + ").");
            MaxAuxiliary[componentIndex] = UndefinedMax;
        }

        /// <summary>Returns true if the speciifed co-ordinates lie outside of the bounding box,
        /// and false otherwise. 
        /// <para>If the specified coordinates are null then false is returned, if dimensions do not match then exception is thrown.</para></summary>
        /// <param name="coordinates">Coordinates that are tested for falling outside the bounding box.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        public bool LiesOutside(IVector coordinates)
        {
            if (coordinates == null)
                return false;
            else
            {
                if (coordinates.Length != Dimension)
                    throw new ArgumentException("Dimension of tested co-ordinates (" + coordinates
                        + ") does not match the bounding box dimention (" + Dimension + ").");
                for (int i = 0; i < Dimension; ++i)
                {
                    if (coordinates[i] < MinAuxiliary[i])
                        return true;
                    if (coordinates[i] > MaxAuxiliary[i])
                        return true;
                }
            }
            return false;
        }

        /// <summary>Returns true if the speciifed co-ordinates lie outside of the bounding box,
        /// and false otherwise. If the specified coordinates are null then false is returned.</summary>
        /// <param name="coordinates">Coordinates that are tested for falling outside the bounding box.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        public bool LiesOutside(params double[] coordinates)
        {
            if (coordinates == null)
                return false;
            else
            {
                if (coordinates.Length != Dimension)
                    throw new ArgumentException("Number of tested co-ordinates (" + coordinates
                        + ") do not match the bounding box dimention (" + Dimension + ").");
                for (int i = 0; i < Dimension; ++i)
                {
                    if (coordinates[i] < MinAuxiliary[i])
                        return true;
                    if (coordinates[i] > MaxAuxiliary[i])
                        return true;
                }
            }
            return false;
        }

        /// <summary>Returns true of the specified co-ordinate lies outside of the bounding box, and false otherwise.</summary>
        /// <param name="componentIndex">Index of co-ordinate component that is tested for lying out of the bounding box.</param>
        /// <param name="coordinate">Co-ordinate component that is tested for lying outside of the bounding box.</param>
        public bool LiesOutside(int componentIndex, double coordinate)
        {
            if (componentIndex != Dimension)
                throw new IndexOutOfRangeException("Bounding box component update: co-ordinate index " + componentIndex
                    + " is out of range (should be between 0 and " + (Dimension - 1) + ").");
            if (coordinate < MinAuxiliary[componentIndex])
                return true;
            if (coordinate > MaxAuxiliary[componentIndex])
                return true;
            return false;
        }

        /// <summary>Returns true if the specified bounding box lise outside of the current bounding box,
        /// and false otherwise. If the specified bounding box is null then false is returned.</summary>
        /// <param name="bounds">Bounding box that is tested for falling outside the current bounding box.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        public bool LiesOutside(IBoundingBox bounds)
        {
            if (bounds == null)
                return false;
            for (int i = 0; i < Dimension; ++i)
            {
                if (bounds.GetMin(i) < MinAuxiliary[i])
                    return true;
                if (bounds.GetMax(i) > MaxAuxiliary[i])
                    return true;
            }
            return false;
        }

        /// <summary>Updates the bounding box in such a way that the specified vector fits in it.
        /// If the specified vector is null then this method has no effect.</summary>
        /// <param name="coordinates">Vector that needs to fit into the bounding box after this call.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        public void Update(IVector coordinates)
        {
            if (coordinates != null)
            {
                int dim = this.Dimension;
                if (coordinates.Length != dim)
                    throw new ArgumentException("Dimension of updating co-ordinates (" + coordinates
                        + ") does not match the bounding box dimention (" + dim + ").");
                for (int i = 0; i < dim; ++i)
                {
                    if (coordinates[i] < MinAuxiliary[i] || MinAuxiliary[i]==UndefinedMin)
                        MinAuxiliary[i] = coordinates[i];
                    if (coordinates[i] > MaxAuxiliary[i] || MaxAuxiliary[i]==UndefinedMax)
                        MaxAuxiliary[i] = coordinates[i];
                }
            }
        }

        /// <summary>Updates the bounding box in such a way that all specified vectors fit in it.
        /// <para>If any of the specified vector is null then this entry has no effect, and if 
        /// the array is null then the method call has no effect.</para></summary>
        /// <param name="points">Vector of coordinates that need to fit into the bounding box after this call.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        public void Update(params IVector[] points)
        {
            if (points!=null)
                for (int i = 0; i < points.Length; ++i)
                {
                    Update(points[i]);
                }
        }

        /// <summary>Updates the bounding box in such a way that a vector with the specified co-ordinates fits in it.
        /// <para>If the specified array of co-ordinate values is null then this method has no effect.</para>
        /// <para>Number of specified co-ordinates must match dimension of the bounding box.</para></summary>
        /// <param name="coordinates">Array of co/ordinate components of the vector to which the bounding box
        /// is adapted in such a way that the vector fits in the bounding box after the call.
        /// <para>Number of specified co-ordinates must match the dimension of the bounding box.</para></param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        public void Update(params double[] coordinates)
        {
            if (coordinates != null)
            {
                int dim = this.Dimension;
                if (coordinates.Length != dim)
                    throw new ArgumentException("Dimension of updating co-ordinates (" + coordinates
                        + ") does not match the bounding box dimention (" + dim + ").");
                for (int i = 0; i < dim; ++i)
                {
                    if (coordinates[i] < MinAuxiliary[i] || MinAuxiliary[i] == UndefinedMin)
                        MinAuxiliary[i] = coordinates[i];
                    if (coordinates[i] > MaxAuxiliary[i] || MaxAuxiliary[i] == UndefinedMax)
                        MaxAuxiliary[i] = coordinates[i];
                }
            }
        }

        /// <summary>Updates the bounding box in such a way that the specified value of the specified 
        /// co-ordinate component (defined by co-ordinate index) fits in it.</summary>
        /// <param name="componentIndex">Index of the co-ordinate component that is updated.</param>
        /// <param name="coordinate">Co-ordinate value to which the bounding box is adapted 
        /// in such a way that this co-ordinate value fits inside it.</param>
        public void Update(int componentIndex, double coordinate)
        {
            if (componentIndex<0 || componentIndex >= Dimension)
                throw new IndexOutOfRangeException("Bounding box component update: co-ordinate index " + componentIndex
                    + " is out of range (should be between 0 and " + (Dimension - 1) + ").");
            if (coordinate < MinAuxiliary[componentIndex] || MinAuxiliary[componentIndex] == UndefinedMin)
                MinAuxiliary[componentIndex] = coordinate;
            if (coordinate > MaxAuxiliary[componentIndex] || MaxAuxiliary[componentIndex] == UndefinedMax)
                MaxAuxiliary[componentIndex] = coordinate;
        }

        /// <summary>Updates the bounding box in such a way that all specified values of the specified 
        /// co-ordinate component fit in it.
        /// <para>If the specified array of the coordinate values is null then this method has no effect.</para>
        /// <para>Update is performed for a particular coordinate of the bounding box, and all specified 
        /// values are taken into account.</para></summary>
        /// <param name="componentIndex">Index of co-ordinate component in which the bounding box is updated.</param>
        /// <param name="coordinates">Values of co-ordinate component to which the bounding box is adapted in such a way
        /// that these values fit inside it (in the specified co-ordinate direction).</param>
        public void Update(int componentIndex, params double[] coordinates)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new IndexOutOfRangeException("Bounding box component update: co-ordinate index " + componentIndex
                    + " is out of range (should be between 0 and " + (Dimension - 1) + ").");
            for (int i = 0; i < coordinates.Length; ++i)
            {
                double coordinate = coordinates[i];
                if (coordinate < MinAuxiliary[componentIndex] || MinAuxiliary[componentIndex] == UndefinedMin)
                    MinAuxiliary[componentIndex] = coordinate;
                if (coordinate > MaxAuxiliary[componentIndex] || MaxAuxiliary[componentIndex] == UndefinedMax)
                    MaxAuxiliary[componentIndex] = coordinate;
            }
        }

        /// <summary>Updates the bounding box in such a way that the specified other bounding box fits in it.
        /// If the specified bounding box is null then this method has no effect.
        /// <para>WARNING: Components of the bounding box that are not defined can not result in any changes 
        /// in the current bounding box..</para></summary>
        /// <param name="bounds">Bounding box that needs to fit into bounding box after this call.</param>
        /// <exception cref="ArgumentException">Throws exception when dimensions don't match.</exception>
        public void Update(IBoundingBox bounds)
        {
            if (bounds != null)
            {
                int dim = this.Dimension;
                for (int i = 0; i < dim; ++i)
                {
                    if (bounds.IsMinDefined(i))
                        Update(i, bounds.GetMin(i));
                    if (bounds.IsMaxDefined(i))
                        Update(i, bounds.GetMax(i));
                }
            }
        }

        /// <summary>Updates the current bounding box in such a way that in all components, all specified 
        /// coordinates fit in it.
        /// This is for example useful to define a hypercube (equal minimum and maximum in all components),
        /// in this case just minimal and maximal bounds are specified as arguments.</summary>
        /// <param name="coordinates">Co-ordinates to which the bounding box is adapted in all axis directions in such a way
        /// that the specified co-ordinates fit inside it.</param>
        public void UpdateAll(params double[] coordinates)
        {
            for (int i = 0; i < Dimension; ++i)
                Update(i, coordinates);
        }

        /// <summary>Reduces (if necessary) the current bounding box in such a way that the specified bounding
        /// box contains it.
        /// <para>Minimal and maximal components of the current bounding box that don't fall in the specified
        /// bounding box are changed such that they lie just on the border of the bounding box, and others remain intact.</para></summary>
        /// <param name="outerBounds">Bounding box that specify the area within which the current bounding box must fit.
        /// Components that fall out of this bounding box are moved to the border of this bounding box (and the current
        /// bounding box shrinks because of this).</param>
        public void Shrink(IBoundingBox outerBounds)
        {
            if (outerBounds != null)
            {
                int dim = this.Dimension;
                if (dim != outerBounds.Dimension)
                    throw new ArgumentException("The specified maximal bounding box is of incompatible dimension, "
                        + outerBounds.Dimension + " instead of " + this.Dimension + ".");
                for (int i = 0; i < dim; ++i)
                {
                    if (outerBounds.IsMinDefined(i))
                        if (this.MinAuxiliary[i] < outerBounds.GetMin(i)) // || this.MinAuxiliary[i] == UndefinedMin)
                            this.MinAuxiliary[i] = outerBounds.GetMin(i);
                    if (outerBounds.IsMaxDefined(i))
                        if (this.MaxAuxiliary[i] > outerBounds.GetMax(i)) // || this.MaxAuxiliary[i] == UndefinedMax)
                            this.MaxAuxiliary[i] = outerBounds.GetMax(i);
                }
            }
        }

        /// <summary>Reduces (if necessary) the current bounding box in such a way that the specified component
        /// falls within the interval specified by the lower and upper bound.</summary>
        /// <param name="which">Index of the component along which the bounding box is reduced if nexessary.</param>
        /// <param name="minBound">Lower bound on the interval that is admissible in the specified coordinate direction.</param>
        /// <param name="maxBound">Upper bound on the interval that is admissible in the specified coordinate direction.</param>
        public void Shrink(int which, double minBound, double maxBound)
        {
            if (which < 0 || which >= this.Dimension)
                throw new IndexOutOfRangeException("Component index " + which + " out of range, should be between 0 and " + (this.Dimension-1) + ".");
            if (this.MinAuxiliary[which] < minBound) // || !IsMinDefined(which))
                this.MinAuxiliary[which] = minBound;
            if (this.MaxAuxiliary[which] > maxBound) // || !IsMaxDefined(which))
                this.MaxAuxiliary[which] = maxBound;
        }

        /// <summary>Symmetrically expands or shrinks the interval between the bounds for the specified component
        /// for the specified factor. 
        /// <para>Factors greater than 1 mean expansion, factors lesser than 1 mean shrinkage, and fators lesser than 0 are invalid.</para>
        /// <para>If lower or upper bound of the specified component is not defined then this method has no effect.</para></summary>
        /// <param name="componentIndex">Component of the coordinate to be expanded or shrinked.</param>
        /// <param name="factor">Factor by which the interval between bounds is expanded or shrinked. 
        /// Factors greater than 1 mean expansion, factors less than 1 mean shrinkage, and factors less than 0 are invalid (ArgumentException thrown).</param>
        public void ExpandOrShrinkInterval(int componentIndex, double factor)
        {
            if (componentIndex<0 || componentIndex >= Dimension)
                throw new IndexOutOfRangeException("Bounding box expansion or shrinkage: co-ordinate index " + componentIndex
                    + " is out of range (should be between 0 and " + (Dimension - 1) + ").");
            if (factor < 0)
                throw new ArgumentException("Expansion / shrinkage factor " + factor + " is less than 0.");
            if (IsMinDefined(componentIndex) && IsMaxDefined(componentIndex))
            {
                double min = this.MinAuxiliary[componentIndex];
                double max = this.MaxAuxiliary[componentIndex];
                double halfIntervalLength = 0.5 * (max - min);
                double center = 0.5*(max+min);
                MinAuxiliary[componentIndex] = center - factor*halfIntervalLength;
                MaxAuxiliary[componentIndex] = center + factor*halfIntervalLength;
            }
        }

        /// <summary>Symmetrically expands or shrinks the intervals between the bounds for all components
        /// for the specified factor. 
        /// Factors greater than 1 mean expansion, factors lesser than 1 mean shrinkage, and fators lesser than 0 are invalid.</summary>
        /// <param name="factor">Factor by which the interval between bounds is expanded or shrinked. 
        /// Factors greater than 1 mean expansion, factors less than 1 mean shrinkage, and factors less than 0 are invalid (ArgumentException thrown).</param>
        public void ExpandOrShrinkInterval(double factor)
        {
            if (factor < 0)
                throw new ArgumentException("Expansion / shrinkage factor " + factor + " is less than 0.");
            for (int componentIndex = 0; componentIndex < this.Dimension; ++componentIndex)
            {
                ExpandOrShrinkInterval(componentIndex, factor);
            }
        }

        /// <summary>Symmetrically expands or shrinks the intervals between the bounds for all components
        /// for the specified factor, and takes care that minimal interval lengths are not 0. 
        /// Factors greater than 1 mean expansion, factors lesser than 1 mean shrinkage, and fators lesser than 0 are invalid.
        /// Remark: components where minimal bound is set above maximal are not treated specially.</summary>
        /// <param name="factor">Factor by which the interval between bounds is expanded or shrinked. 
        /// Factors greater than 1 mean expansion, factors less than 1 mean shrinkage, and factors less than 0 are invalid (ArgumentException thrown).</param>
        /// <param name="zeroIntervalLengthReplacemnt">Interval length that is taken for those components for which the current
        /// interval length equals 0 (i.e. lower bound equals upper bound).</param>
        public void ExpandOrShrinkInterval(double factor, double zeroIntervalLengthReplacemnt)
        {
            if (factor < 0)
                throw new ArgumentException("Expansion / shrinkage factor " + factor + " is less than 0.");
            for (int componentIndex = 0; componentIndex < this.Dimension; ++componentIndex)
            {
                if (IsMinDefined(componentIndex) && IsMaxDefined(componentIndex))
                {
                    double min = this.MinAuxiliary[componentIndex];
                    double max = this.MaxAuxiliary[componentIndex];
                    if (min == max)
                    {
                        MinAuxiliary[componentIndex] = min - 0.5 * zeroIntervalLengthReplacemnt;
                        MaxAuxiliary[componentIndex] = max + 0.5 * zeroIntervalLengthReplacemnt;
                    }
                    else
                    {
                        double halfIntervalLength = 0.5 * (max - min);
                        double center = 0.5 * (max + min);
                        MinAuxiliary[componentIndex] = center - factor * halfIntervalLength;
                        MaxAuxiliary[componentIndex] = center + factor * halfIntervalLength;
                    }
                }
            }
        }

        /// <summary>Sets minimal value for the specified coponent of the bounding box.</summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="min">Minimal value of the component.</param>
        public void SetMin(int componentIndex, double min)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new ArgumentException("Component index out of range when setting bounding box minimal co-ordinate." + Environment.NewLine
                    + "  Index: " + componentIndex + ", should be between 0 and " + (Dimension - 1) + ".");
            MinAuxiliary[componentIndex] = min;
        }

        /// <summary>Sets maximal value for the specified coponent of the bounding box.</summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="max">Maximal value of the component.</param>
        public void SetMax(int componentIndex, double max)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new ArgumentException("Component index out of range when setting bounding box minimal co-ordinate." + Environment.NewLine
                    + "  Index: " + componentIndex + ", should be between 0 and " + (Dimension - 1) + ".");
            MaxAuxiliary[componentIndex] = max;
        }

        /// <summary>Sets minimal and maximal value for the specified coponent of the bounding box.</summary>
        /// <param name="componentIndex">Component index.</param>
        /// <param name="min">Minimal value of the component.</param>
        /// <param name="max">Maximal value of the component.</param>
        public void SetBounds(int componentIndex, double min, double max)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new ArgumentException("Component index out of range when setting bounding box minimal and maximal co-ordinates." + Environment.NewLine
                    + "  Index: " + componentIndex + ", should be between 0 and " + (Dimension - 1) + ".");
            MinAuxiliary[componentIndex] = min;
            MaxAuxiliary[componentIndex] = max;
        }

        /// <summary>Returns the lower bound on the specified co-ordinate component of the bounding box.
        /// <para>If lower bound is not defined for this component then <see cref="double.MinValue"/> is returned.</para></summary>
        /// <param name="componentIndex">Component index.</param>
        public double GetMin(int componentIndex)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new ArgumentException("Component index out of range when getting bounding box minimal co-ordinate." + Environment.NewLine
                    + "  Index: " + componentIndex + ", should be between 0 and " + (Dimension - 1) + ".");
            double ret = MinAuxiliary[componentIndex];
            if (ret != UndefinedMin)
                return ret;
            else 
                return double.MinValue;  // lower bound on component not defined
        }

        /// <summary>Returns the upper bound on the specified co-ordinate component of the bounding box.
        /// <para>If upper bound is not defined for this component then <see cref="double.MaxValue"/> is returned.</para></summary>
        /// <param name="componentIndex">Component index.</param>
        public double GetMax(int componentIndex)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new ArgumentException("Component index out of range when getting bounding box maximal co-ordinate." + Environment.NewLine
                    + "  Index: " + componentIndex + ", should be between 0 and " + (Dimension - 1) + ".");
            double ret = MaxAuxiliary[componentIndex];
            if (ret != UndefinedMax)
                return ret;
            else
                return double.MaxValue;  // upper bound on component not defined
        }


        /// <summary>Gets the vector of lower bounds on co-ordinates of the current bounding box and stores it in the 
        /// specified object. 
        /// <para>Components for which lower bounds are not defined are set to <see cref="double.MinValue"/>.</para></summary>
        /// <param name="min">Reference to a vector object where lower bounds (minimal co-ordinates) are stored.</param>
        public void GetMin(ref IVector min)
        {
            int dim = this.Dimension;
            bool resize = false;
            if (min == null)
                resize = true;
            else if (min.Length != dim)
                resize = true;
            if (resize)
                Vector.Resize(ref min, dim);
            for (int i = 0; i < dim; ++i)
            {
                double minComp = this.MinAuxiliary[i];
                if (minComp == UndefinedMin)
                    min[i] = double.MinValue;
                else
                    min[i] = minComp;
                
            }
        }

        /// <summary>Gets the vector of upper bounds on co-ordinates of the current bounding box and stores it in the 
        /// specified object. 
        /// <para>Components for which upper bounds are not defined are set to <see cref="double.MaxValue"/>.</para></summary>
        /// <param name="max">Reference to a vector object where upper bounds (maximal co-ordinates) are stored.</param>
        public void GetMax(ref IVector max)
        {
            int dim = this.Dimension;
            bool resize = false;
            if (max == null)
                resize = true;
            else if (max.Length != dim)
                resize = true;
            if (resize)
                Vector.Resize(ref max, dim);
            for (int i = 0; i < dim; ++i)
            {
                double maxComp = this.MaxAuxiliary[i];
                if (maxComp == UndefinedMax)
                    max[i] = double.MaxValue;
                else
                    max[i] = maxComp;
            }
        }

        /// <summary>Returns interval length for the specified co-ordinate component (i.e. difference between the upper and lower
        /// bound), or 0 if either lower or upper bound is not defined for this component.</summary>
        /// <param name="componentIndex">Specifies the component for which interval length is returned.</param>
        public double GetIntervalLength(int componentIndex)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new ArgumentException("Component index out of range when getting bounding box component interval length." + Environment.NewLine
                    + "  Index: " + componentIndex + ", should be between 0 and " + (Dimension - 1) + ".");
            double min = MinAuxiliary[componentIndex];
            double max = MaxAuxiliary[componentIndex];
            if (min != UndefinedMin && max != UndefinedMax)
                return (max-min);
            else
                return 0;  // lower bound on component not defined
        }

        /// <summary>Gets the interval lengths for co-ordinates of the current bounding box (i.e. differences) 
        /// and stores them in the specified vector object. 
        /// <para>Elements for which either upper or lower bounds are not defined are set to 0.</para></summary>
        /// <param name="intervals">Reference to a vector object where interval lengths are stored.</param>
        public void GetIntervalLengths(ref IVector intervals)
        {
            bool resize = false;
            int dim = this.Dimension;
            if (intervals == null)
                resize = true;
            else if (intervals.Length != dim)
                resize = true;
            if (resize)
                Vector.Resize(ref intervals, dim);
            for (int i = 0; i < dim; ++i)
            {
                double min = MinAuxiliary[i];
                double max = MaxAuxiliary[i];
                if (min != UndefinedMin && max != UndefinedMax)
                    intervals[i] =  (max - min);
                else
                    intervals[i] = 0;  // lower bound on component not defined
            }
        }


        /// <summary>Calculates relative coordinates, with respect to the current bounding box, that
        /// correspond to the specified absolute (or physical or actual) coordinates.</summary>
        /// <param name="absoluteCoordinates">Vector of absolute coordinates.</param>
        /// <param name="relativeCoordinates">Vector where corresponding relative coordinates are stored.</param>
        public void GetRelativeCoordinates(IVector absoluteCoordinates, ref IVector relativeCoordinates)
        {
            int dimension = this.Dimension;
            if (absoluteCoordinates == null)
                throw new ArgumentException("Vector of absolute coordinates is not specified (null reference).");
            else if (absoluteCoordinates.Length != dimension)
                throw new ArgumentException("Vector of absolute coordinates has wrong dimension " + absoluteCoordinates.Length
                    + ", should be " + dimension);
            bool resize = false;
            if (relativeCoordinates == null)
                resize = true;
            else if (relativeCoordinates.Length != dimension)
                resize = true;
            if (resize)
                Vector.Resize(ref relativeCoordinates, dimension);
            for (int i = 0; i < dimension; ++i)
            {
                if (!IsMinDefined(i))
                    throw new InvalidOperationException("Lower bound is not defined for component " + i + ".");
                if (!IsMaxDefined(i))
                    throw new InvalidOperationException("Uper bound is not defined for component " + i + ".");
                double absoluteCoord = absoluteCoordinates[i];
                double min = GetMin(i);
                double max = GetMax(i);
                if (max == min)
                    throw new InvalidOperationException("Lower bound equals upper bound for componene " + i + ".");
                relativeCoordinates[i] = (absoluteCoord - min) / (max - min);
            }
        }


        /// <summary>Calculates absolute (physical) coordinates, with respect to the current bounding box, that
        /// correspond to the specified relative coordinates.</summary>
        /// <param name="absoluteCoordinates">Vector of relative coordinates.</param>
        /// <param name="relativeCoordinates">Vector where corresponding absolute coordinates are stored.</param>
        public void GetAbsoluteCoordinates(IVector relativeCoordinates, ref IVector absoluteCoordinates)
        {
            int dimension = this.Dimension;
            if (relativeCoordinates == null)
                throw new ArgumentException("Vector of relative coordinates is not specified (null reference).");
            else if (relativeCoordinates.Length != dimension)
                throw new ArgumentException("Vector of relative coordinates has wrong dimension " + relativeCoordinates.Length
                    + ", should be " + dimension);
            bool resize = false;
            if (absoluteCoordinates == null)
                resize = true;
            else if (absoluteCoordinates.Length != dimension)
                resize = true;
            if (resize)
                Vector.Resize(ref absoluteCoordinates, dimension);
            for (int i = 0; i < dimension; ++i)
            {
                if (!IsMinDefined(i))
                    throw new InvalidOperationException("Lower bound is not defined for component " + i + ".");
                if (!IsMaxDefined(i))
                    throw new InvalidOperationException("Uper bound is not defined for component " + i + ".");
                double relativeCoord = relativeCoordinates[i];
                double min = GetMin(i);
                double max = GetMax(i);
                if (max == min)
                    throw new InvalidOperationException("Lower bound equals upper bound for componene " + i + ".");
                absoluteCoordinates[i] = min + (relativeCoord * (max - min));
            }
            throw new NotImplementedException();
        }

        /// <summary>Returns true if maximum value is defined for the specified component, false otherwise.</summary>
        /// <param name="componentIndex">Index of component that is queried.</param>
        public bool IsMinDefined(int componentIndex)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new ArgumentException("Component index out of range when testing whether minimum is defined." + Environment.NewLine
                    + "  Index: " + componentIndex + ", should be between 0 and " + (Dimension - 1) + ".");
            return MinAuxiliary[componentIndex] != UndefinedMin;
        }

        /// <summary>Returns true if maximum value is defined for the specified component, false otherwise.</summary>
        /// <param name="componentIndex">Index of component that is queried.</param>
        public bool IsMaxDefined(int componentIndex)
        {
            if (componentIndex < 0 || componentIndex >= Dimension)
                throw new ArgumentException("Component index out of range when testing whether maximum is defined." + Environment.NewLine
                    + "  Index: " + componentIndex + ", should be between 0 and " + (Dimension - 1) + ".");
            return MaxAuxiliary[componentIndex] != UndefinedMax;
        }

        #endregion Operation


        #region Misc

        public override string  ToString()
        {
 	         StringBuilder sb = new StringBuilder();
            sb.AppendLine("{ ");
            sb.AppendLine("  " + MinAuxiliary.ToString() + ", ");
            sb.AppendLine("  " + MaxAuxiliary.ToString());
            sb.AppendLine("} ");
            return sb.ToString();
        }

        #endregion Misc


        #region Utilities

        /// <summary>Creates a random point contained in the current bounding box by using the
        /// global random number generator, and stores that point into the specified vector.
        /// The random point is generated according to a uniform distribution within the bounding box.</summary>
        /// <param name="point">Vector where generated random point is stored.</param>
        public void GetRandomPoint(ref IVector point)
        {
            GetRandomPoint(ref point, RandomGenerator.Global);
        }

        /// <summary>Creates a random point contained in the current bounding box by using the
        /// specified random number generator, and stores that point into the specified vector.
        /// The random point is generated according to a uniform distribution within the bounding box.</summary>
        /// <param name="point">Vector where generated random point is stored.</param>
        /// <param name="rand">Random number generator used for generation of the random point.</param>
        public void GetRandomPoint(ref IVector point, IRandomGenerator rand)
        {
            int dim = this.Dimension;
            Vector.Resize(ref point, dim);
            for (int k = 0; k < dim; ++k)
            {
                if (!this.IsMinDefined(k))
                    throw new InvalidOperationException("Can not define a random point within bounding box: "
                        + Environment.NewLine + "  minimum for element No. " + k + " is not defined.");
                if (!this.IsMaxDefined(k))
                    throw new InvalidOperationException("Can not define a random point within bounding box: "
                        + Environment.NewLine + "  maximum for element No. " + k + " is not defined.");
                point[k] = rand.NextDoubleInclusive(this.GetMin(k), this.GetMax(k));
            }
        }

        #endregion Misc


        #region StaticUtilities

        /// <summary>Copies state of the specified bounding box to anotherv(target) bounding box.
        /// <para>The target bounding box must be allocated and of the same dimension as the original bounding box.</para></summary>
        /// <param name="original">Bounding box to be copied.</param>
        /// <param name="copy">Bounding box where copy of the original is stored.</param>
        /// <exception cref="ArgumentException">When dimensions don't match or one of the counding boxes is null.</exception>
        public void Copy(IBoundingBox original, IBoundingBox copy)
        {
            if (original == null)
                throw new ArgumentException("The bounding box to be copied is not specified.");
            else if (copy == null)
                throw new ArgumentException("Bounding box to hold a copy is not specified.");
            else if (original.Dimension != copy.Dimension)
                throw new ArgumentException("Dimensions of the original bounding box the one to hold a copy do not match.");
            else
            {
                copy.Reset();
                copy.Update(original);
            }
        }

        /// <summary>Creates and returns a bounding box with specified dimension and minimum and maximum bounds equal in all components.</summary>
        /// <param name="dimension">Dimension of the bounding box.</param>
        /// <param name="min">Minimal bound for all components.</param>
        /// <param name="max">Maximal bound for all components.</param>
        public static BoundingBox Create(int dimension, double min, double max)
        {
            BoundingBox ret = new BoundingBox(dimension);
            for (int i = 0; i < dimension; ++i)
                ret.Update(i, min, max);
            return ret;
        }


        /// <summary>Creates and returns a minimal bounding box that contains all the specified points.</summary>
        /// <param name="points">Points (vectors) defining the created bounding box.
        /// Some vectors in this array can be null, but all non-null vector must have the same dimension.</param>
        public static BoundingBox Create(params IVector[] points)
        {
            BoundingBox ret = null;
            if (points == null)
                throw new ArgumentNullException("Points defining the bounding box not specified (null array).");
            else if (points.Length < 1)
                throw new ArgumentException("Points defining the bounding box not specified (zero length array).");
            else
            {
                int dimension = 0;
                for (int i = 0; i < points.Length; ++i)
                {
                    IVector vec = points[i];
                    if (vec != null)
                    {
                        if (dimension == 0)
                        {
                            dimension = vec.Length;
                            if (dimension != 0)
                                ret = new BoundingBox(dimension);
                        }
                        if (vec.Length != dimension)
                        {
                            throw new ArgumentException("Points that define bounding box do not have equal dimensions.");
                        }
                        ret.Update(vec);
                    }
                }
                if (dimension < 1)
                    throw new ArgumentException("Invalid array of points.");
            }
            return ret;
        }


        /// <summary>Mapping from one bounding box to another.
        /// Returns a single component mapped by a mapping that maps the original to the target
        /// bounding box. If one of the bounding boxes is not specified then the performed mapping
        /// is identity mapping and does not change the value.</summary>
        /// <param name="original">Original bounding box.</param>
        /// <param name="target">Target bounding box, mapping maps the originl bounding box to this one.</param>
        /// <param name="whichComponent">Index of component that is mapped.</param>
        /// <param name="value">Value that is mapped (individual component of an element of the vector space).</param>
        /// <returns>The mapped component.</returns>
        public static double Map(IBoundingBox original, IBoundingBox target, int whichComponent, double value)
        {
            if (original == null || target == null)
                return value;
            else if (original.Dimension != target.Dimension)
                throw new ArgumentException("Bounding box defining scaling and shifting map do not have consistent dimensions, "
                    + original.Dimension + " vs. " + target.Dimension + ".");
            else if (whichComponent < 0 || whichComponent >= original.Dimension)
                throw new ArgumentOutOfRangeException("Coordinate index " + whichComponent +
                    " out of range, should b ebetween 0 and " + (whichComponent - 1) + ".");
            else
            {
                double minOrig = original.GetMin(whichComponent);
                double maxOrig = original.GetMax(whichComponent);
                double minTarget = target.GetMin(whichComponent);
                double maxTarget = target.GetMax(whichComponent);
                return minTarget + (value - minOrig) * (maxTarget - minTarget) / (maxOrig - minOrig);
            }
        }


        /// <summary>Mapping from one bounding box to another.
        /// Maps a vector from the original to the target space. Mapped vector is stored
        /// to a specified vector.
        /// If one of the bounding boxes is not specified then the performed mapping
        /// is identity mapping and does not change the value.</summary>
        /// <param name="original">Original bounding box.</param>
        /// <param name="target">Target bounding box, mapping maps the originl bounding box to this one.</param>
        /// <param name="value">Vector that is mapped.</param>
        /// <param name="result">Vecto where the mapped vectr is stored.</param>
        public static void Map(IBoundingBox original, IBoundingBox target, IVector value, ref IVector result)
        {
            if (original == null || target == null)
                Vector.Copy(value, ref result);
            else if (original.Dimension != target.Dimension)
                throw new ArgumentException("Bounding boxes defining scaling and shifting map do not have consistent dimensions, "
                    + original.Dimension + " vs. " + target.Dimension + ".");
            else if (value.Length != original.Dimension)
                throw new ArgumentException("Dimension of mapped vector, " + value.Length +
                    ", does not match bounding box dimension " + original.Dimension + ".");
            else
            {
                if (result == null)
                    result = new Vector(value.Length);
                else if (result.Length != value.Length)
                    Vector.Resize(ref result, value.Length);
                for (int whichComponent = 0; whichComponent < value.Length; ++whichComponent)
                {
                    double minOrig = original.GetMin(whichComponent);
                    double maxOrig = original.GetMax(whichComponent);
                    double minTarget = target.GetMin(whichComponent);
                    double maxTarget = target.GetMax(whichComponent);
                    result[whichComponent] = minTarget + (value[whichComponent] - minOrig)
                        * (maxTarget - minTarget) / (maxOrig - minOrig);
                }
            }
        }


        #endregion StaticUtilities



    } // abstract class BoundingBoxBase


    /// <summary>Bounding box, defines lower and upper bounds on vector quantities such as coordinates of
    /// geometrix objects, bounds of domains, etc.</summary>
    public class BoundingBox: BoundingBoxBase, IBoundingBox
    {
        
        #region Construction

        /// <summary>Creates a bounding box of the specified dimension and initializes it in such a way that
        /// minimal co-ordinate components equal to <see cref="BoundingBoxBase.UndefinedMin"/> and maximal co-ordinate components 
        /// equal do <see cref="BoundingBoxBase.UndefinedMax"/>.</summary>
        /// <param name="dimension">Dimension of the space in which bounding box is defined.</param>
        public BoundingBox(int dimension): base(dimension)
        {  }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified vector.</summary>
        /// <param name="coordinates">Vector of coordinates to which minimal and maximal co-ordines of the 
        /// bounding box are set.</param>
        public BoundingBox(IVector coordinates)
            : base(coordinates)
        {  }

        /// <summary>Creates a bounding box of the specified dimension and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified vectors.</summary>
        /// <param name="min">Vector of minimal coordinates.</param>
        /// <param name="max">Vector of macimal coordinates.</param>
        public BoundingBox(IVector min, IVector max)
            : base(min, max)
        {  }
        
        /// <summary>Constructs a bounding box of the specified dimension and initializes it in such a way that
        /// its bounds correspond to those of the specified bounding box.
        /// <para>The specified bounding box must be different than null, otherwise exception is thrown.</para></summary>
        /// <param name="bounds">Bounds with which the constructed bounding box is initialized.</param>
        public BoundingBox(IBoundingBox bounds)
            : base(bounds)
        {  }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified array.</summary>
        /// <param name="coordinates">Array of coordinates to which minimal and maximal co-ordines of the 
        /// bounding box are set.</param>
        public BoundingBox(double[] coordinates)
            : base(coordinates)
        {  }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified arrays.</summary>
        /// <param name="min">Array of components to which minimal co-ordines of the bounding box are set.</param>
        /// <param name="max">Array of components to which maximal co-ordines of the bounding box are set.</param>
        public BoundingBox(double[] min, double[] max)
            : base(min, max)
        {  }

        #endregion Construction

        #region Data

        #region ToOverride

        /// <summary>Creates and returns a new vector that is consistent with the definition of bounding box.</summary>
        /// <param name="dim">Dimension of vector space.</param>
        protected override IVector CreateVector(int dim)
        {
            return new Vector(dim);
        }

        #endregion ToOverride

        #endregion Data

    }  // class BoundingBox


    /// <summary>A 3D bounding box.</summary>
    public class BoundingBox3d : BoundingBoxBase, IBoundingBox
    {

        #region Construction

        
        /// <summary>Creates a 3D bounding box with unspecified bounds;
        /// minimal co-ordinate components equal to <see cref="BoundingBoxBase.UndefinedMin"/> and maximal co-ordinate components 
        /// equal do <see cref="BoundingBoxBase.UndefinedMax"/>.</summary>
        public BoundingBox3d()
            : base(3)
        { }
        
        /// <summary>Creates a 3D bounding box with the specified bounds.</summary>
        /// <param name="minX">Lower bound in the first coordinate.</param>
        /// <param name="maxX">Upper bound in the first coordinate.</param>
        /// <param name="minY">Lower bound in the second coordinate.</param>
        /// <param name="maxY">Upper bound in the second coordinate.</param>
        /// <param name="minZ">Lower bound in the third coordinate.</param>
        /// <param name="maxZ">Upper bound in the third coordinate.</param>
        public BoundingBox3d(double minX, double maxX, double minY, double maxY, double minZ, double maxZ)
            : this()
        { 
            Update(0 /* componentIndex */, minX, maxX);
            Update(1 /* componentIndex */, minY, maxY);
            Update(2 /* componentIndex */, minZ, maxZ);
        }

        ///// <summary>Creates a bounding box of the specified dimension and initializes it in such a way that
        ///// minimal co-ordinate components equal to <see cref="UndefinedMin"/> and maximal co-ordinate components 
        ///// equal do <see cref="UndefinedMax"/>.</summary>
        ///// <param name="dimension">Dimension of the space in which bounding box is defined.</param>
        //protected BoundingBox3d(int dimension)
        //    : base(dimension)
        //{ }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified vector.</summary>
        /// <param name="coordinates">Vector of coordinates to which minimal and maximal co-ordines of the 
        /// bounding box are set.</param>
        public BoundingBox3d(IVector coordinates)
            : base(coordinates)
        { }

        /// <summary>Creates a bounding box of the specified dimension and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified vectors.</summary>
        /// <param name="min">Vector of minimal coordinates.</param>
        /// <param name="max">Vector of macimal coordinates.</param>
        public BoundingBox3d(IVector min, IVector max)
            : base(min, max)
        { }
                
        /// <summary>Constructs a 3D bounding box of the specified dimension and initializes it in such a way that
        /// its bounds correspond to those of the specified bounding box.
        /// <para>The specified bounding box must be different than null, otherwise exception is thrown.</para></summary>
        /// <param name="bounds">Bounds with which the constructed bounding box is initialized.</param>
        public BoundingBox3d(IBoundingBox bounds)
            : base(bounds)
        {  }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified array.</summary>
        /// <param name="coordinates">Array of coordinates to which minimal and maximal co-ordines of the 
        /// bounding box are set.</param>
        public BoundingBox3d(double[] coordinates)
            : base(coordinates)
        { }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified arrays.</summary>
        /// <param name="min">Array of components to which minimal co-ordines of the bounding box are set.</param>
        /// <param name="max">Array of components to which maximal co-ordines of the bounding box are set.</param>
        public BoundingBox3d(double[] min, double[] max)
            : base(min, max)
        { }

        #endregion Construction


        #region Data

        /// <summary>Gets the lower bound in the first coordinate.</summary>
        public double MinX { get { return this.MinAuxiliary[0]; } }

        /// <summary>Gets the upper bound in the first coordinate.</summary>
        public double MaxX { get { return this.MaxAuxiliary[0]; } }

        /// <summary>Gets the lower bound in the second coordinate.</summary>
        public double MinY { get { return this.MinAuxiliary[1]; } }

        /// <summary>Gets the upper bound in the second coordinate.</summary>
        public double MaxY { get { return this.MaxAuxiliary[1]; } }

        /// <summary>Gets the lower bound in the third coordinate.</summary>
        public double MinZ { get { return this.MinAuxiliary[2]; } }

        /// <summary>Gets the upper bound in the third coordinate.</summary>
        public double MaxZ { get { return this.MaxAuxiliary[2]; } }

        #endregion Data

        #region Operation

        /// <summary>Updates the bounding box in such a way that the specified co-ordinates fit in it.</summary>
        /// <param name="coordinates">Vector of oordinates for which the bounding box is updated in such a way that they fit in it.</param>
        public void Update(vec3 coordinates)
        {
            Update(coordinates.x, coordinates.y, coordinates.z);
        }


        /// <summary>Updates the bounding box in such a way that all specified points fit in it.
        /// <para>If the array of points is empty or null then this function has no effect.</para></summary>
        /// <param name="points">Coordinate vectors of points for which the bounding box is updated in such a way that they fit in it.</param>
        public void Update(params vec3[] points)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Length; ++i)
                    Update(points[i]);
            }
        }

        /// <summary>Updates the bounding box in such a way that the specified co-ordinates fit in it.
        /// <para>If the specified coordinates are null then this method has no effect.</para></summary>
        /// <param name="coordinates">Vector of oordinates for which the bounding box is updated in such a way that they fit in it.</param>
        public void Update(Vector3d coordinates)
        {
            if (coordinates!=null)
                Update(coordinates.X, coordinates.Y, coordinates.Z);
        }

        /// <summary>Updates the bounding box in such a way that all specified points fit in it.
        /// <para>If the specified array of points is empty or null then this function has no effect. 
        /// Elements of the array that are null are ignored.</para></summary>
        /// <param name="points">Coordinate vectors of points for which the bounding box is updated in such a way that they fit in it.</param>
        public void Update(params Vector3d[] points)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Length; ++i)
                    Update(points[i]);
            }
        }

        #endregion Operation

        #region ToOverride

        /// <summary>Creates and returns a new vector that is consistent with the definition of bounding box.</summary>
        /// <param name="dim">Dimension of vector space.</param>
        protected override IVector CreateVector(int dim)
        {
            if (dim != 3)
                throw new ArgumentException("invalid dimension " + dim + ": dimension of vectors or arrays in 3D bounding box should be 3.");
            return new Vector3d(0.0);
        }

        #endregion ToOverride


    }  // class BoundingBox3d


    /// <summary>A 2D bounding box.</summary>
    public class BoundingBox2d : BoundingBoxBase, IBoundingBox
    {

        #region Construction

        /// <summary>Creates a 2D bounding box with unspecified bounds;
        /// minimal co-ordinate components equal to <see cref="BoundingBoxBase.UndefinedMin"/> and maximal co-ordinate components 
        /// equal do <see cref="BoundingBoxBase.UndefinedMax"/>.</summary>
        public BoundingBox2d()
            : base(2)
        {  }
                
        /// <summary>Creates a 2D bounding box with the specified bounds.</summary>
        /// <param name="minX">Lower bound in the first coordinate.</param>
        /// <param name="maxX">Upper bound in the first coordinate.</param>
        /// <param name="minY">Lower bound in the second coordinate.</param>
        /// <param name="maxY">Upper bound in the second coordinate.</param>
        public BoundingBox2d(double minX, double maxX, double minY, double maxY)
            : this()
        { 
            Update(0 /* componentIndex */, minX, maxX);
            Update(1 /* componentIndex */, minY, maxY);
        }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified vector.</summary>
        /// <param name="coordinates">Vector of coordinates to which minimal and maximal co-ordines of the 
        /// bounding box are set.</param>
        public BoundingBox2d(IVector coordinates)
            : base(coordinates)
        {
            if (coordinates!=null)
                if (coordinates.Length != 2)
                    throw new ArgumentException("Dimension of the specified vector should be 2.");
        }

        /// <summary>Creates a bounding box of the specified dimension and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified vectors.</summary>
        /// <param name="min">Vector of minimal coordinates.</param>
        /// <param name="max">Vector of macimal coordinates.</param>
        public BoundingBox2d(IVector min, IVector max)
            : base(min, max)
        {  }
                
        /// <summary>Constructs a 2D bounding box of the specified dimension and initializes it in such a way that
        /// its bounds correspond to those of the specified bounding box.
        /// <para>The specified bounding box must be different than null, otherwise exception is thrown.</para></summary>
        /// <param name="bounds">Bounds with which the constructed bounding box is initialized.</param>
        public BoundingBox2d(IBoundingBox bounds)
            : base(bounds)
        {  }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified array.</summary>
        /// <param name="coordinates">Array of coordinates to which minimal and maximal co-ordines of the 
        /// bounding box are set.</param>
        public BoundingBox2d(double[] coordinates)
            : base(coordinates)
        { }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified arrays.</summary>
        /// <param name="min">Array of components to which minimal co-ordines of the bounding box are set.</param>
        /// <param name="max">Array of components to which maximal co-ordines of the bounding box are set.</param>
        public BoundingBox2d(double[] min, double[] max)
            : base(min, max)
        { }

        #endregion Construction

        #region Data

        /// <summary>Gets the lower bound in the first coordinate.</summary>
        public double MinX { get { return this.MinAuxiliary[0]; } }

        /// <summary>Gets the upper bound in the first coordinate.</summary>
        public double MaxX { get { return this.MaxAuxiliary[0]; } }

        /// <summary>Gets the lower bound in the second coordinate.</summary>
        public double MinY { get { return this.MinAuxiliary[1]; } }

        /// <summary>Gets the upper bound in the second coordinate.</summary>
        public double MaxY { get { return this.MaxAuxiliary[1]; } }

        #endregion Data

        #region Operation

        /// <summary>Updates the bounding box in such a way that the specified co-ordinates fit in it.</summary>
        /// <param name="coordinates">Vector of oordinates for which the bounding box is updated in such a way that they fit in it.</param>
        public void Update(vec2 coordinates)
        {
            Update(coordinates.x, coordinates.y);
        }


        /// <summary>Updates the bounding box in such a way that all specified points fit in it.
        /// <para>If the array of points is empty or null then this function has no effect.</para></summary>
        /// <param name="points">Coordinate vectors of points for which the bounding box is updated in such a way that they fit in it.</param>
        public void Update(params vec2[] points)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Length; ++i)
                    Update(points[i]);
            }
        }

        /// <summary>Updates the bounding box in such a way that the specified co-ordinates fit in it.
        /// <para>If the specified coordinates are null then this method has no effect.</para></summary>
        /// <param name="coordinates">Vector of oordinates for which the bounding box is updated in such a way that they fit in it.</param>
        public void Update(Vector2d coordinates)
        {
            if (coordinates != null)
                Update(coordinates.X, coordinates.Y);
        }

        /// <summary>Updates the bounding box in such a way that all specified points fit in it.
        /// <para>If the specified array of points is empty or null then this function has no effect. 
        /// Elements of the array that are null are ignored.</para></summary>
        /// <param name="points">Coordinate vectors of points for which the bounding box is updated in such a way that they fit in it.</param>
        public void Update(params Vector2d[] points)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Length; ++i)
                    Update(points[i]);
            }
        }

        #endregion Operation

        #region ToOverride

        /// <summary>Creates and returns a new vector that is consistent with the definition of bounding box.</summary>
        /// <param name="dim">Dimension of vector space.</param>
        protected override IVector CreateVector(int dim)
        {
            if (dim != 2)
                throw new ArgumentException("invalid dimension " + dim + ": dimension of vectors or arrays in 2D bounding box should be 2.");
            return new Vector2d(0.0);
        }

        #endregion ToOverride


    }  // class BoundingBox2d


    /// <summary>An 1D bounding box.</summary>
    public class BoundingBox1d : BoundingBoxBase, IBoundingBox
    {

        #region Construction

        /// <summary>Creates an 1D bounding box with unspecified bounds;
        /// minimal co-ordinate components equal to <see cref="BoundingBoxBase.UndefinedMin"/> and maximal co-ordinate components 
        /// equal do <see cref="BoundingBoxBase.UndefinedMax"/>.</summary>
        public BoundingBox1d()
            : base(1)
        {  }
                
        /// <summary>Creates an 1D bounding box with the specified bounds.</summary>
        /// <param name="minX">Lower bound in the first coordinate.</param>
        /// <param name="maxX">Upper bound in the first coordinate.</param>
        public BoundingBox1d(double minX, double maxX)
            : this()
        { 
            Update(0 /* componentIndex */, minX, maxX);
        }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified vector.</summary>
        /// <param name="coordinates">Vector of coordinates to which minimal and maximal co-ordines of the 
        /// bounding box are set.</param>
        public BoundingBox1d(IVector coordinates)
            : base(coordinates)
        {
            if (coordinates!=null)
                if (coordinates.Length != 1)
                    throw new ArgumentException("Dimension of the specified vector should be 1.");
        }

        /// <summary>Creates a bounding box of the specified dimension and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified vectors.</summary>
        /// <param name="min">Vector of minimal coordinates.</param>
        /// <param name="max">Vector of macimal coordinates.</param>
        public BoundingBox1d(IVector min, IVector max)
            : base(min, max)
        {  }
                
        /// <summary>Constructs a 1D bounding box of the specified dimension and initializes it in such a way that
        /// its bounds correspond to those of the specified bounding box.
        /// <para>The specified bounding box must be different than null, otherwise exception is thrown.</para></summary>
        /// <param name="bounds">Bounds with which the constructed bounding box is initialized.</param>
        public BoundingBox1d(IBoundingBox bounds)
            : base(bounds)
        {  }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified array.</summary>
        /// <param name="coordinates">Array of coordinates to which minimal and maximal co-ordines of the 
        /// bounding box are set.</param>
        public BoundingBox1d(double[] coordinates)
            : base(coordinates)
        { }

        /// <summary>Creates a bounding box and initializes it in such a way that
        /// minimal and maximal co-ordinate components equal to components of the spacified arrays.</summary>
        /// <param name="min">Array of components to which minimal co-ordines of the bounding box are set.</param>
        /// <param name="max">Array of components to which maximal co-ordines of the bounding box are set.</param>
        public BoundingBox1d(double[] min, double[] max)
            : base(min, max)
        { }

        #endregion Construction

        #region Data


        /// <summary>Gets the lower bound in the first coordinate.</summary>
        public double MinX { get { return this.MinAuxiliary[0]; } }

        /// <summary>Gets the upper bound in the first coordinate.</summary>
        public double MaxX { get { return this.MaxAuxiliary[0]; } }


        /// <summary>Gets the lower bound in the first coordinate.</summary>
        public double MinValue { get { return this.MinAuxiliary[0]; } }

        /// <summary>Gets the upper bound in the first coordinate.</summary>
        public double MaxValue { get { return this.MaxAuxiliary[0]; } }

        #endregion Data


        #region ToOverride

        /// <summary>Creates and returns a new vector that is consistent with the definition of bounding box.</summary>
        /// <param name="dim">Dimension of vector space.</param>
        protected override IVector CreateVector(int dim)
        {
            if (dim != 1)
                throw new ArgumentException("Invalid dimension " + dim + ": dimension of vectors or arrays in 1D bounding box should be 1.");
            return new Vector(1);
        }

        #endregion ToOverride


    }  // class BoundingBox1d



}