// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using IG.Num;


namespace IG.Lib
{


    public static class ColorTranslator
    {

        /// <summary>Provides partial replacement for the class <see cref="System.Drawing.ColorConverter"/>, which is not available
        /// in .NET Standard 2.0.</summary>
        /// <param name="htmlColorString"></param>
        /// <returns></returns>
        /// <remarks>See this link (last answer): 
        /// https://stackoverflow.com/questions/12155811/how-to-convert-hexadecimal-ffffff-to-system-drawing-color </remarks>
        public static Color FromHtml(string htmlColorString)
        {
            Color c = Color.FromArgb(int.Parse(htmlColorString.Replace("#", ""),
                         System.Globalization.NumberStyles.AllowHexSpecifier));
            return c;
        }

    }

#if NETSTANDARD2_0_OR_GREATER

#endif

    /// <summary><para>Color scale.</para>
    /// Mapping from scalar values to colors, defining continuous or discrete color scales.</summary>
    /// <remarks>This class was initially used in GUI building for fadeouts, but has now broader applicaton
    /// and is  used in all kins of graphical applications. This is also the reason that it is placed in the
    /// <see cref="IG.Lib"/> namespace. Discrete color values are added in 2011 for needs in graphic applications.</remarks>
    /// $A Igor Jul08 Oct11;
    public interface IColorScale
    {

#region ValuesTransformation

        /// <summary>Transforms the specified value from reference domain (interval [0,1]) to actual domain.</summary>
        /// <param name="referenceValue">Value in the reference domain.</param>
        /// <returns>Value in the actual domain corresponding to the specified value in the reference domain.</returns>
        double FromReference(double referenceValue);

        /// <summary>Transforms the specified value from actual domain to reference domain (interval [0,1]).</summary>
        /// <param name="actualValue">Value in the actual domain.</param>
        /// <returns>Value in the reference domain corresponding to the specified value in the actual domain.</returns>
        double ToReference(double actualValue);

#endregion ValuesTransformation


#region MainOperations

        /// <summary>Gets the flag indicating whether the color scale is discrete (with a finite 
        /// number of possible colors, as opposed to continuous).</summary>
        bool IsDiscrete
        { get; }

        /// <summary>Returns the color from the current collor scale that corresponds to the specified value.
        /// <para>Scale can be continuous or discrete (in the latter case <see cref="IsDiscrete"/> is true).</para>
        /// <para>To get a color from the scale corresponding to a value in the reference domain [0,1], use
        /// the <see cref="GetReferenceColor"/> method instead.</para></summary>
        /// <param name="value">Value for which the corresponding color is returned.</param>
        color GetColor(double value);

        /// <summary>Returns the color from the current color scale that corresponds to the specified value
        /// in the reference domain (interval [0,1]).
        /// <para>Scale can be continuous or discrete (in the latter case <see cref="IsDiscrete"/> is true).</para></summary>
        /// <param name="referenceValue">Value from the interval [0, 1] (the reference domain) for which the corresponding 
        /// color is returned.</param>
        color GetReferenceColor(double referenceValue);
        
#endregion MainOperations


#region ContinuousColors

        /// <summary>Returns the CONTINUOUS color from the current color scale that corresponds to the specified
        /// value in the REFERENCE domain (interval [0, 1]).
        /// <para>Scale can be continuous or discrete (in the latter case <see cref="IsDiscrete"/> is true).</para></summary>
        /// <param name="referenceValue">Value in the reference domain (interval [0, 1]) for which the corresponding
        /// color is returned.</param>
        color GetContinuousReferenceColor(double referenceValue);

        /// <summary>Returns the CONTINUOUS color from the current color scale that corresponds to the specified value.
        /// <para>Scale can be continuous or discrete (in the latter case <see cref="IsDiscrete"/> is true).</para></summary>
        /// <param name="value">Value  (in the actual domain) for which the corresponding color is returned.</param>
        color GetContinuousColor(double value);

#endregion ContinuousColors


#region DiscreteColors


        /// <summary>Number of cells in discrete color scale. 
        /// <para>Remarks:</para>
        /// <para>If less than 1 then the current object can not represent a discrete color scale,
        /// and the related methods such as should throw exception.</para>
        /// <para>Minimal value is 1. If p or less is assigned then an exception is thrown.</para></summary>
        int NumCells
        { get; set; }

        /// <summary>Returns the reference value (in the interval [0, 1])
        /// corresponding to the specified cell in the discrete color map. This value is used to
        /// obtain the uniform (discrete) color of the cell by the methods that produces continuous
        /// scale colors (continuous methods are usually basis for scale definitions).</summary>
        /// <param name="cellIndex">Index of the cell for which the corresponding characteristic value
        /// (usually in the middle of the cell interval) is returned.</param>
        double GetReferenceCellValue(int cellIndex);


        /// <summary>Returns the value corresponding to the specified cell in the discrete color map. 
        /// This value is used to obtain the uniform (discrete)  color of the cell by the methods 
        /// that produces continuous scale colors (continuous  methods are usually basis for scale 
        /// definitions).</summary>
        /// <param name="cellIndex">Index of the cell for which the corresponding characteristic value
        /// (usually in the middle of the cell interval) is returned.</param>
        double GetCellValue(int cellIndex);

        /// <summary>Returns the (discrete) color corresponding to the specified cell index.
        /// If the index is out of range then the lowest or the highest cell color is returned,
        /// dependent on whether the value is smaller than lower bound or greater than upper bound.</summary>
        /// <param name="cellIndex">Index of the discrete cell for which color is returned.</param>
        color GetCellColor(int cellIndex);

        /// <summary>Retuns index of the cell of the discrete color scale that corresponds to the specified 
        /// value in the reference domain (interval [0, 1]).</summary>
        /// <param name="referenceValue">Value in the reference domain (interval [0, 1]) for wihich index 
        /// of the discrete cell of the color scale is returned.</param>
        int GetReferenceCellIndex(double referenceValue);


        /// <summary>Retuns index of the cell of the discrete color scale that corresponds to the specified 
        /// value (in the actusl domain).</summary>
        /// <param name="value">Value in the reference domain (interval [0, 1]) for wihich index 
        /// of the discrete cell of the color scale is returned.</param>
        int GetCellIndex(double value);

        /// <summary>Returns a color from the DISCRETE color scale represented by the current object
        /// that corresponds to he specified value in the REFERENCE domain (interval [0, 1]).</summary>
        /// <param name="referenceValue">Value in the reference domain (interval [0, 1]) for which the 
        /// corresponding color from the discrete color scale is returned.</param>
        color GetDiscreteReferenceColor(double referenceValue);

        /// <summary>Returns a color from the DISCRETE color scale represented by the current object
        /// that corresponds to he specified value (in the actual domain).</summary>
        /// <param name="value">Value (in the actual domain) for which the 
        /// corresponding color from the discrete color scale is returned.</param>
        color GetDiscreteColor(double value);

#endregion DiscreteColors

    } // interface IColorScale 


    /// <summary><para>Color scale; linearly mapped values, discrete scaling functionality,
    /// value to color mappiong defined through interpolation of table of provided mark colors.</para>
    /// Provides mapping from scalar values to colors, defining continuous or discrete color scales.
    /// <para>NOT thread safe.</para></summary>
    /// <remarks><para>Mapping from scalar value to color is usually defined continuously on the reference interval
    /// of values ranging from 0 to 1.</para>
    /// <para>This class was initially used in GUI building for fadeouts, but has now broader applicaton
    /// and is  used in all kins of graphical applications. This is also the reason that it is placed in the
    /// <see cref="IG.Lib"/> namespace. Discrete color values are added in 2011 for needs in graphic applications.</para></remarks>
    /// <remarks>A set of colors define the continuous color scale. When a reference value changes from 0 to 1, the
    /// corresponding color on the color scale smoothly varies from the first to the last color and passes all
    /// eventual intermediate colros.
    /// <para>Discrete color scale is defined on basis of continuous scale in such a way that for whole subranges
    /// of values in the reference domain (interval [0, 1]) only a single color is picked. This color corresponds to
    /// a particular discrete value in the reference domain, usually in the middle of the subinterval in the 
    /// reference space that corresponds to a given cell of the color scale.</para>
    /// <para>Values our of range:</para>
    /// <para>For continuous scale, values out of range will produce colors that correspond to either 
    /// lower or upper bound of the range (dependent in which direction the range is exceeded).
    /// For discrete scale, either color of the lower or of the upper cell is produced.</para>
    /// <para>The same object provides methods for getting collors either from continuous or form discrete
    /// color scale. There are methods such as <see cref="ColorScaleBase.GetColor"/> or <see cref="ColorScaleBase.GetReferenceColor"/> that
    /// return colors form either scale, dependent on the current value of the <see cref="ColorScaleBase.IsDiscrete"/> property.</para>
    /// <para>See also:</para>
    /// <para>http://www.sapdesignguild.org/resources/glossary_color/ </para>
    /// </remarks>
    /// $A Igor Jul08 Oct11;
    public class ColorScale : ColorScaleBase, IColorScale
    {

#region Construction

        /// <summary>Constructor. Creates a continuous scale.
        /// <para>Discrete scale has 10 cells.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="definitionColors">A set of colors that define the color scale. Color of the continuous scale
        /// will smoothly transition through these values when parameter in the reference domain would run form 0 to 1.
        /// <para>There should be at least one definition color (In this case, the color scale will have a uniform color).</para></param>
        public ColorScale(double minValue, double maxValue, params color[] definitionColors):
            this(minValue, maxValue, 10 /* numCells */, false /* isDiscrete */, definitionColors)
        {  }

        /// <summary>Constructor. Creates a discrete scale with the specified number of cells.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Nuber of cells in the discrete scale.</param>
        /// <param name="definitionColors">A set of colors that define the color scale. Color of the continuous scale
        /// will smoothly transition through these values when parameter in the reference domain would run form 0 to 1.
        /// <para>There should be at least one definition color (In this case, the color scale will have a uniform color).</para></param>
        public ColorScale(double minValue, double maxValue, int numCells, params color[] definitionColors):
            this(minValue, maxValue, numCells, true /* isDiscrete */, definitionColors)
        {  }

        /// <summary>Constructor.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Nuber of cells in the discrete scale.</param>
        /// <param name="isDiscrete">Determinse whether the main method such as <see cref="ColorScaleBase.GetColor"/> and <see cref="ColorScaleBase.GetReferenceColor"/> 
        /// provide colors of continuous or discrete scale.</param>
        /// <param name="definitionColors">A set of colors that define the color scale. Color of the continuous scale
        /// will smoothly transition through these values when parameter in the reference domain would run form 0 to 1.
        /// <para>There should be at least one definition color (In this case, the color scale will have a uniform color).</para></param>
        public ColorScale(double minValue, double maxValue, int numCells, bool isDiscrete, params color[] definitionColors)
        {
            if (definitionColors == null)
                throw new ArgumentNullException("definitionColors", "Definition colors are not specified.");
            if (definitionColors.Length < 1)
                throw new ArgumentException("No definition colors are specified (empty array).");
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.IsDiscrete = isDiscrete;
            SetDefinitionColors(definitionColors);
        }

#endregion Construction 


#region DefinitionColors

        /// <summary>Table of definition colors.</summary>
        color[] _definitionColors;

        /// <summary>Number of specified colors that define the color scale.</summary>
        protected int _numDefinitionColors;

        /// <summary>Sets the definition colors that define the color scale.
        /// <para>Internal table to store these colors is allocated anew and colors are copied to the table.</para></summary>
        /// <param name="colors">Definition colors.</param>
        protected void SetDefinitionColors(color[] colors)
        {
            if(colors==null)
                throw new ArgumentNullException("colors","At least one color defining the color scale should be defined.");
            _numDefinitionColors = colors.Length;
            _definitionColors = new color[_numDefinitionColors];
            for (int i = 0; i < _numDefinitionColors; ++i)
                _definitionColors[i] = colors[i];
            UpdateAuxiliaryData();
        }

#endregion DefinitionColors

#region ValuesTransformation

        private double _minValue, _maxvalue;

        /// <summary>Lower bound for values for which color scale is defines.
        /// <para>To the values below this limit, the lowest-value color is assigned.</para></summary>
        public double MinValue
        {
            get { return _minValue; }
            set 
            { 
                _minValue = value;
                UpdateAuxiliaryData();
            }
        }

        /// <summary>Upper bound for values for which color scale is defines.
        /// <para>To the values above this limit, the highest-value color is assigned. </para></summary>
        public double MaxValue
        {
            get { return _maxvalue; }
            set
            {
                _maxvalue = value;
                UpdateAuxiliaryData();
            }
        }


        /// <summary>Difference between maximal and minimal value.</summary>
        protected double _intervalLength;

        /// <summary>Transforms the specified value from reference domain (interval [0,1]) to actual domain.</summary>
        /// <param name="referenceValue">Value in the reference domain.</param>
        /// <returns>Value in the actual domain corresponding to the specified value in the reference domain.</returns>
        public override double FromReference(double referenceValue)
        {
            return _minValue + referenceValue * _intervalLength;
        }

        /// <summary>Transforms the specified value from actual domain to reference domain (interval [0,1]).</summary>
        /// <param name="actualValue">Value in the actual domain.</param>
        /// <returns>Value in the reference domain corresponding to the specified value in the actual domain.</returns>
        public override double ToReference(double actualValue)
        {
            return (actualValue-_minValue)/(_intervalLength);
        }

#endregion ValuesTransformation


        /// <summary>Updates precalculated auxiliary data that are used for faster calculations.</summary>
        protected virtual void UpdateAuxiliaryData()
        {
            _intervalLength = _maxvalue - _minValue;
        }

#region ContinuousColors

        /// <summary>Returns the CONTINUOUS color from the current color scale that corresponds to the specified
        /// value in the REFERENCE domain (interval [0, 1]).
        /// <para>Scale can be continuous or discrete (in the latter case <see cref="ColorScaleBase.IsDiscrete"/> is true).</para></summary>
        /// <param name="referenceValue">Value in the reference domain (interval [0, 1]) for which the corresponding
        /// color is returned.</param>
        public override color GetContinuousReferenceColor(double referenceValue)
        {
            if (_numDefinitionColors < 1)
                throw new InvalidOperationException("No definition colors are specified.");
            else if (_numDefinitionColors == 1)
            {
                return _definitionColors[0];
            } else if (_numDefinitionColors == 2)
            {
                return color.Mixture(true /* clipComponents */, 1.0*(1.0-referenceValue), _definitionColors[0],
                    1.0*referenceValue, _definitionColors[1]); 
            }
            else if (_numDefinitionColors == 3)
            {
                if (referenceValue<=0.5)
                    return color.Mixture(true /* clipComponents */, 
                        (1.0-2.0*referenceValue), _definitionColors[0],
                        2*referenceValue, _definitionColors[1]); 
                else
                    return color.Mixture(true /* clipComponents */, 
                        (1.0-2.0*(referenceValue-0.5)), _definitionColors[1],
                        2.0*(referenceValue-0.5), _definitionColors[2]); 
            } else
            {
                // Remark: for smaller numvers of definiton colors the color is caluculated with special
                // procedures for faster calculation. General method called below is a bit slower.
                return color.Scale(_definitionColors, referenceValue);
            }
        }


#endregion ContinuousColors

#region DiscreteColors


        /// <summary>Returns the reference value (in the interval [0, 1])
        /// corresponding to the specified cell in the discrete color map. This value is used to
        /// obtain the uniform (discrete) color of the cell by the methods that produces continuous
        /// scale colors (continuous methods are usually basis for scale definitions).</summary>
        /// <param name="cellIndex">Index of the cell for which the corresponding characteristic value
        /// (usually in the middle of the cell interval) is returned.</param>
        public override double GetReferenceCellValue(int cellIndex)
        {
            if (_numCells <= 0)
                throw new InvalidOperationException("Cell indices are not defined because number of cells is less than 1.");
            if (cellIndex < 0)
                return 0;
            else if (cellIndex >= _numCells)
                return 1.0;
            else
            {
                double _referenceCellInterval = 1.0 / NumCells;
                return ((double)cellIndex + 0.5) * _referenceCellInterval;
            }
        }


        /// <summary>Retuns index of the cell of the discrete color scale that corresponds to the specified 
        /// value in the reference domain (interval [0, 1]).</summary>
        /// <param name="referenceValue">Value in the reference domain (interval [0, 1]) for wihich index 
        /// of the discrete cell of the color scale is returned.</param>
        public override int GetReferenceCellIndex(double referenceValue)
        {
            if (referenceValue < 0)
                return -1;
            else if (referenceValue > 1)
                return NumCells;
            else
            {
                int ret = (int)Math.Floor(referenceValue * (double) _numCells);
                // correct for possible roundoff errors:
                if (ret == NumCells && referenceValue <= 1)
                    ret = _numCells - 1;
                return ret;
            }
        }

#endregion DiscreteColors


    } // class ColorScale



    /// <summary><para>Color scale.</para>
    /// Provides mapping from scalar values to colors, defining continuous or discrete color scales.
    /// <para>NOT thread safe.</para></summary>
    /// <remarks><para>Mapping from scalar value to color is usually defined continuously on the reference interval
    /// of values ranging from 0 to 1.</para>
    /// This class was initially used in GUI building for fadeouts, but has now broader applicaton
    /// and is  used in all kins of graphical applications. This is also the reason that it is placed in the
    /// <see cref="IG.Lib"/> namespace. Discrete color values are added in 2011 for needs in graphic applications.</remarks>
    /// $A Igor Jul08 Oct11;
    public abstract class ColorScaleBase: IColorScale
    {

#region ValuesTransformation 

        /// <summary>Transforms the specified value from reference domain (interval [0,1]) to actual domain.</summary>
        /// <param name="referenceValue">Value in the reference domain.</param>
        /// <returns>Value in the actual domain corresponding to the specified value in the reference domain.</returns>
        public abstract double FromReference(double referenceValue);

        /// <summary>Transforms the specified value from actual domain to reference domain (interval [0,1]).</summary>
        /// <param name="actualValue">Value in the actual domain.</param>
        /// <returns>Value in the reference domain corresponding to the specified value in the actual domain.</returns>
        public abstract double ToReference(double actualValue);

#endregion ValuesTransformation 


#region MainOperations

        private bool _isDiscrete = false;

        /// <summary>Gets the flag indicating whether the color scale is discrete (with a finite 
        /// number of possible colors, as opposed to continuous).</summary>
        public bool IsDiscrete
        {
            get { return _isDiscrete; }
            protected set { _isDiscrete = value; }
        }

        /// <summary>Returns the color from the current collor scale that corresponds to the specified value.
        /// <para>Scale can be continuous or discrete (in the latter case <see cref="IsDiscrete"/> is true).</para>
        /// <para>To get a color from the scale corresponding to a value in the reference domain [0,1], use
        /// the <see cref="GetReferenceColor"/> method instead.</para></summary>
        /// <param name="value">Value for which the corresponding color is returned.</param>
        public virtual color GetColor(double value)
        {
            if (IsDiscrete)
                return GetDiscreteColor(value);
            else
                return GetContinuousColor(value);
        }


        /// <summary>Returns the color from the current color scale that corresponds to the specified value
        /// in the reference domain (interval [0,1]).</summary>
        /// <para>Scale can be continuous or discrete (in the latter case <see cref="IsDiscrete"/> is true).</para>
        /// <param name="referenceValue">Value from the interval [0, 1] (the reference domain) for which the corresponding 
        /// color is returned.</param>
        public virtual color GetReferenceColor(double referenceValue)
        {
            if (IsDiscrete)
                return GetDiscreteReferenceColor(referenceValue);
            else
                return GetContinuousReferenceColor(referenceValue);
        }

#endregion MainOperations


#region ContinuousColors

        /// <summary>Returns the CONTINUOUS color from the current color scale that corresponds to the specified
        /// value in the REFERENCE domain (interval [0, 1]).
        /// <para>Scale can be continuous or discrete (in the latter case <see cref="IsDiscrete"/> is true).</para></summary>
        /// <param name="referenceValue">Value in the reference domain (interval [0, 1]) for which the corresponding
        /// color is returned.</param>
        public abstract color GetContinuousReferenceColor(double referenceValue);

        /// <summary>Returns the CONTINUOUS color from the current color scale that corresponds to the specified value.
        /// <para>Scale can be continuous or discrete (in the latter case <see cref="IsDiscrete"/> is true).</para></summary>
        /// <param name="value">Value  (in the actual domain) for which the corresponding color is returned.</param>
        public virtual color GetContinuousColor(double value)
        {
            return GetContinuousReferenceColor(ToReference(value));
        }

#endregion ContinuousColors


#region DiscreteColors

        protected int _numCells = 0;

        /// <summary>Number of cells in discrete color scale. 
        /// <para>Remarks:</para>
        /// <para>If less than 1 then the current object can not represent a discrete color scale,
        /// and the related methods such as should throw exception.</para>
        /// <para>Minimal value is 1. If p or less is assigned then an exception is thrown.</para></summary>
        public int NumCells
        {
            get { return _numCells; }
            set
            {
                if (value < 1) throw new InvalidOperationException("Can not set number of cells smaller than 1.");
                _numCells = value;
            }
        }

        /// <summary>Returns the reference value (in the interval [0, 1])
        /// corresponding to the specified cell in the discrete color map. This value is used to
        /// obtain the uniform (discrete) color of the cell by the methods that produces continuous
        /// scale colors (continuous methods are usually basis for scale definitions).</summary>
        /// <param name="cellIndex">Index of the cell for which the corresponding characteristic value
        /// (usually in the middle of the cell interval) is returned.</param>
        public abstract double GetReferenceCellValue(int cellIndex);


        /// <summary>Returns the value corresponding to the specified cell in the discrete color map. 
        /// This value is used to obtain the uniform (discrete)  color of the cell by the methods 
        /// that produces continuous scale colors (continuous  methods are usually basis for scale 
        /// definitions).</summary>
        /// <param name="cellIndex">Index of the cell for which the corresponding characteristic value
        /// (usually in the middle of the cell interval) is returned.</param>
        public virtual double GetCellValue(int cellIndex)
        {
            return FromReference(GetReferenceCellValue(cellIndex));
        }

        /// <summary>Returns the (discrete) color corresponding to the specified cell index.
        /// If the index is out of range then the lowest or the highest cell color is returned,
        /// dependent on whether the value is smaller than lower bound or greater than upper bound.</summary>
        /// <param name="cellIndex">Index of the discrete cell for which color is returned.</param>
        public virtual color GetCellColor(int cellIndex)
        {
            return GetContinuousReferenceColor(GetReferenceCellValue(cellIndex));
        }

        /// <summary>Retuns index of the cell of the discrete color scale that corresponds to the specified 
        /// value in the reference domain (interval [0, 1]).</summary>
        /// <param name="referenceValue">Value in the reference domain (interval [0, 1]) for wihich index 
        /// of the discrete cell of the color scale is returned.</param>
        public abstract int GetReferenceCellIndex(double referenceValue);


        /// <summary>Retuns index of the cell of the discrete color scale that corresponds to the specified 
        /// value (in the actusl domain).</summary>
        /// <param name="value">Value in the reference domain (interval [0, 1]) for wihich index 
        /// of the discrete cell of the color scale is returned.</param>
        public virtual int GetCellIndex(double value)
        {
            return GetReferenceCellIndex(ToReference(value));
        }

        /// <summary>Returns a color from the DISCRETE color scale represented by the current object
        /// that corresponds to he specified value in the REFERENCE domain (interval [0, 1]).</summary>
        /// <param name="referenceValue">Value in the reference domain (interval [0, 1]) for which the 
        /// corresponding color from the discrete color scale is returned.</param>
        public virtual color GetDiscreteReferenceColor(double referenceValue)
        {
            return GetCellColor(GetReferenceCellIndex(referenceValue));
        }


        /// <summary>Returns a color from the DISCRETE color scale represented by the current object
        /// that corresponds to he specified value (in the actual domain).</summary>
        /// <param name="value">Value (in the actual domain) for which the 
        /// corresponding color from the discrete color scale is returned.</param>
        public virtual color GetDiscreteColor(double value)
        {
            return GetDiscreteReferenceColor(ToReference(value));
        }

#endregion DiscreteColors

#region Static


        /// <summary>Creates and returns a continuous color scale that runs through the specified colors.
        /// <para>The first specified color corresponds to the lowest and the last one to the highest values. 
        /// Other colors (if specified) are positioned between, and colors for intermediate values are 
        /// interpolated between two neighboring colors. Only one color can also be specified, in this case
        /// the whole scale has the same color.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="scaleColors">Colors that determine the color scale. T</param>
        public static ColorScale Create(double minValue, double maxValue, params color[] scaleColors)
        {
            return new ColorScale(minValue, maxValue, scaleColors);
        }

        /// <summary>Creates and returns a discrete color scale that runs through the specified colors.
        /// <para>The first specified color corresponds to the lowest and the last one to the highest values. 
        /// Other colors (if specified) are positioned between, and colors for intermediate values are 
        /// interpolated between two neighboring colors. Only one color can also be specified, in this case
        /// the whole scale has the same color.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of cells in the color scale.</param>
        /// <param name="scaleColors">Colors that determine the color scale.</param>
        public static ColorScale Create(double minValue, double maxValue, int numCells, 
            params color[] scaleColors)
        {
            return new ColorScale(minValue, maxValue, numCells, scaleColors);
        }

#region FixedScales


        /// <summary>Creates and returns a continuous color scale that runs from blue through 
        /// red till yellow color.
        /// <para>This is intended as default color scale for graphical representations. The advantge is
        /// that color brightness varies monotonically () and relatively significantly) form the lower to
        /// the upper bounds, while hue varies significantly, too.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        public static ColorScale CreateDefault(double minValue, double maxValue)
        {
            color col1 = new color(0, 0, 0.5); // dark blue
            color col2 = new color(0, 0.75, 0); // medium green
            color col3 = new color(0, 0, 1); // light red
            color col4 = new color(0, 1, 1); // yellow
            return new ColorScale(minValue, maxValue,
                col1, col2, col3, col4);
        }

        /// <summary>Creates and returns a discrete color scale that runs from blue through 
        /// red till yellow color.
        /// <para>This is intended as default color scale for graphical representations. The advantge is
        /// that color brightness varies monotonically () and relatively significantly) form the lower to
        /// the upper bounds, while hue varies significantly, too.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        public static ColorScale CreateDefault(double minValue, double maxValue, int numCells)
        {
            color col1 = new color(0, 0, 0.5); // dark blue
            color col2 = new color(0, 0.75, 0); // medium green
            color col3 = new color(0, 0, 1); // light red
            color col4 = new color(0, 1, 1); // yellow
            return new ColorScale(minValue, maxValue, numCells,
                col1, col2, col3, col4);
        }


        /// <summary>Creates and returns a continuous color scale that runs from violet till red color and
        /// passes rainbow colors in their natural order (red-orange-yellow-green-blue-indigo-violet).
        /// <para>Indigo is included.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        public static ColorScale CreateRainbowFull(double minValue, double maxValue)
        {
            color col1 = System.Drawing.Color.Violet;
            color col2 = System.Drawing.Color.Indigo;
            color col3 = System.Drawing.Color.Blue;
            color col4 = System.Drawing.Color.Green;
            color col5 = System.Drawing.Color.Yellow;
            color col6 = System.Drawing.Color.Orange;
            color col7 = System.Drawing.Color.Red;
            return new ColorScale(minValue, maxValue,
                col1, col2, col3, col4, col5, col6, col7);
        }

        /// <summary>Creates and returns a discrete color scale that runs from violet till red color and
        /// passes rainbow colors in their natural order (red-orange-yellow-green-blue-indigo-violet).
        /// <para>Indigo is included.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        public static ColorScale CreateRainbowFull(double minValue, double maxValue, int numCells)
        {
            color col1 = System.Drawing.Color.Violet;
            color col2 = System.Drawing.Color.Indigo;
            color col3 = System.Drawing.Color.Blue ;
            color col4 = System.Drawing.Color.Green;
            color col5 = System.Drawing.Color.Yellow;
            color col6 = System.Drawing.Color.Orange;
            color col7 = System.Drawing.Color.Red;
            return new ColorScale(minValue, maxValue, numCells,
                col1, col2, col3, col4, col5, col6, col7);
        }

        /// <summary>Creates and returns a continuous color scale that runs from violet till red color and
        /// passes rainbow colors in their natural order (red-orange-yellow-green-blue-violet).
        /// <para>Indigo is excluded from the scale.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        public static ColorScale CreateRainbow(double minValue, double maxValue)
        {
            color col1 = System.Drawing.Color.Violet;
            color col2 = System.Drawing.Color.Blue;
            color col3 = System.Drawing.Color.Green;
            color col4 = System.Drawing.Color.Yellow;
            color col5 = System.Drawing.Color.Orange;
            color col6 = System.Drawing.Color.Red;
            return new ColorScale(minValue, maxValue,
                col1, col2, col3, col4, col5, col6);
        }

        /// <summary>Creates and returns a discrete color scale that runs from violet till red color and
        /// passes rainbow colors in their natural order (red-orange-yellow-green-blue-indigo-violet).
        /// <para>Indigo is excluded from the scale.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        public static ColorScale CreateRainbow(double minValue, double maxValue, int numCells)
        {
            color col1 = System.Drawing.Color.Violet;
            color col2 = System.Drawing.Color.Blue;
            color col3 = System.Drawing.Color.Green;
            color col4 = System.Drawing.Color.Yellow;
            color col5 = System.Drawing.Color.Orange;
            color col6 = System.Drawing.Color.Red;
            return new ColorScale(minValue, maxValue, numCells,
                col1, col2, col3, col4, col5, col6);
        }


#region ForColorBlind

        /* Links:
         * http://en.wikipedia.org/wiki/Protanopia#Dichromacy
         * http://www.archimedes-lab.org/colorblindnesstest.html
         * http://www.archimedes-lab.org/colorblindnesstest.html
         * http://colorschemedesigner.com/
         * http://msdn.microsoft.com/en-us/library/bb263953%28v=vs.85%29.aspx#hess1009_topic4
         * http://www.iamcal.com/toys/colors/
         * http://colorvisiontesting.com/color8.htm
         * */


        /// <summary>Creates and returns a continuous color scale that is adapted to color blind people in general.
        /// <para>Colors follow as #E69F00 - #56B4E9 - #2B9F78 - #F0E442 - #0072B2 - #D55E00 - CC79A7.</para>
        /// <para>This scale should be well distinguished by normal sighted persons as well as color vision deficients 
        /// with different kinds of color blindness.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <remarks><para>See also:</para>
        /// <para>http://www.archimedes-lab.org/colorblindnesstest.html </para>
        /// <para>http://en.wikipedia.org/wiki/Color_blindness </para></remarks>
        public static ColorScale CreateColorBlind(double minValue, double maxValue)
        {
            // Main color marks in the scale (numbers in comments represent hue in degrees):
            color col1 = ColorTranslator.FromHtml("#E69F00");  // Orange, 41°
            color col2 = ColorTranslator.FromHtml("#56B4E9");  // Sky blue, 202°
            color col3 = ColorTranslator.FromHtml("#2B9F78");  // Bluish green, 160°
            color col4 = ColorTranslator.FromHtml("#F0E442");  // Yellow, 56°
            color col5 = ColorTranslator.FromHtml("#0072B2");  // Blue, 202°
            color col6 = ColorTranslator.FromHtml("#D55E00");  // Vermillon, 27°
            color col7 = ColorTranslator.FromHtml("#CC79A7");  // Reddish purple, 336°
         return new ColorScale(minValue, maxValue,
                col1, col2, col3, col4, col5, col6, col7);
        }

        /// <summary>Creates and returns a discrete color scale that is adapted to color blind people in general.
        /// <para>Colors follow as #E69F00 - #56B4E9 - #2B9F78 - #F0E442 - #0072B2 - #D55E00 - CC79A7.</para>
        /// <para>This scale should be well distinguished by normal sighted persons as well as color vision deficients 
        /// with different kinds of color blindness.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        /// <remarks><para>See also:</para>
        /// <para>http://www.archimedes-lab.org/colorblindnesstest.html </para>
        /// <para>http://en.wikipedia.org/wiki/Color_blindness </para></remarks>
        public static ColorScale CreateColorBlind(double minValue, double maxValue, int numCells)
        {
            // Main color marks in the scale (numbers in comments represent hue in degrees):
            color col1 = ColorTranslator.FromHtml("#E69F00");  // Orange, 41°
            color col2 = ColorTranslator.FromHtml("#56B4E9");  // Sky blue, 202°
            color col3 = ColorTranslator.FromHtml("#2B9F78");  // Bluish green, 160°
            color col4 = ColorTranslator.FromHtml("#F0E442");  // Yellow, 56°
            color col5 = ColorTranslator.FromHtml("#0072B2");  // Blue, 202°
            color col6 = ColorTranslator.FromHtml("#D55E00");  // Vermillon, 27°
            color col7 = ColorTranslator.FromHtml("#CC79A7");  // Reddish purple, 336°
            return new ColorScale(minValue, maxValue, numCells,
                col1, col2, col3, col4, col5, col6, col7);
        }


        // TODO: Check if you can find better scales for different kinds of color blindness!

        /// <summary>Creates and returns a continuous color scale that is adapted to people with protanopia.
        /// <para>Colors follow as orange-yellow-blue-violet.</para>
        /// <para>Protanopia is a color vision deficiency caused by absence of red retinal 
        /// photoreceptors. In this dichromacy, red appears dark.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <remarks><para>See also:</para>
        /// <para>http://en.wikipedia.org/wiki/Color_blindness </para></remarks>
        public static ColorScale CreateColorBlindProtanopia(double minValue, double maxValue)
        {
            color col1 = System.Drawing.Color.Orange;
            color col2 = System.Drawing.Color.Yellow;
            color col3 = System.Drawing.Color.Blue;
            color col4 = System.Drawing.Color.Violet;
            return new ColorScale(minValue, maxValue,
                col1, col2, col3, col4);
        }

        /// <summary>Creates and returns a discrete color scale that is adapted to people with protanopia.
        /// <para>Colors follow as orange-yellow-blue-violet.</para>
        /// <para>Protanopia is a color vision deficiency caused by absence of red retinal 
        /// photoreceptors. In this dichromacy, red appears dark.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        /// <remarks><para>See also:</para>
        /// <para>http://en.wikipedia.org/wiki/Color_blindness </para></remarks>
        public static ColorScale CreateColorBlindProtanopia(double minValue, double maxValue, int numCells)
        {
            color col1 = System.Drawing.Color.Orange;
            color col2 = System.Drawing.Color.Yellow;
            color col3 = System.Drawing.Color.Blue;
            color col4 = System.Drawing.Color.Violet;
            return new ColorScale(minValue, maxValue, numCells,
                col1, col2, col3, col4);
        }

        /// <summary>Creates and returns a continuous color scale that is adapted to people with deuteranopia.
        /// <para>Colors follow as red - yellow - blue.</para>
        /// <para>Deuteranopia is a color vision deficiency caused by absence of green retinal 
        /// photoreceptors and afects red-green discrimination.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <remarks><para>See also:</para>
        /// <para>http://en.wikipedia.org/wiki/Color_blindness </para></remarks>
        public static ColorScale CreateColorBlindDeuteranopia(double minValue, double maxValue)
        {
            color col1 = System.Drawing.Color.Red;
            color col2 = System.Drawing.Color.Yellow;
            color col3 = System.Drawing.Color.Blue;
            return new ColorScale(minValue, maxValue,
                col1, col2, col3);
        }

        /// <summary>Creates and returns a discrete color scale that is adapted to people with deuteranopia.
        /// <para>Colors follow as red - yellow - blue.</para>
        /// <para>Deuteranopia is a color vision deficiency caused by absence of green retinal 
        /// photoreceptors and afects red-green discrimination.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        /// <remarks><para>See also:</para>
        /// <para>http://en.wikipedia.org/wiki/Color_blindness </para></remarks>
        public static ColorScale CreateColorBlindDeuteranopia(double minValue, double maxValue, int numCells)
        {
            color col1 = System.Drawing.Color.Red;
            color col2 = System.Drawing.Color.Yellow;
            color col3 = System.Drawing.Color.Blue;
            return new ColorScale(minValue, maxValue, numCells,
                col1, col2, col3);
        }

        /// <summary>Creates and returns a continuous color scale that is adapted to people with tritanopia.
        /// <para>Colors follow as red - yellow - blue.</para>
        /// <para>Tritanopia is a color vision deficiency caused by absence of blue retinal 
        /// photoreceptors.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <remarks><para>See also:</para>
        /// <para>http://en.wikipedia.org/wiki/Color_blindness </para></remarks>
        public static ColorScale CreateColorBlindTritanopia(double minValue, double maxValue)
        {
            color col1 = System.Drawing.Color.Red;
            color col2 = System.Drawing.Color.Yellow;
            color col3 = System.Drawing.Color.Blue;
            return new ColorScale(minValue, maxValue,
                col1, col2, col3);
        }

        /// <summary>Creates and returns a discrete color scale that is adapted to people with tritanopia.
        /// <para>Colors follow as red - yellow - blue.</para>
        /// <para>Tritanopia is a color vision deficiency caused by absence of blue retinal 
        /// photoreceptors.</para></summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        /// <remarks><para>See also:</para>
        /// <para>http://en.wikipedia.org/wiki/Color_blindness </para></remarks>
        public static ColorScale CreateColorBlindTritanopia(double minValue, double maxValue, int numCells)
        {
            color col1 = System.Drawing.Color.Red;
            color col2 = System.Drawing.Color.Yellow;
            color col3 = System.Drawing.Color.Blue;
            return new ColorScale(minValue, maxValue, numCells,
                col1, col2, col3);
        }


#endregion ForColorBlind



        /// <summary>Creates and returns a continuous color scale with various hues of gray.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        public static ColorScale CreateGray(double minValue, double maxValue)
        {
            return new ColorScale(minValue, maxValue,
                new color(0.1, 0.1, 0.1), new color(0.9, 0.9, 0.9));
        }

        /// <summary>Creates and returns a discrete color scale with various hues of gray.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        public static ColorScale CreateGray(double minValue, double maxValue, int numCells)
        {
            return new ColorScale(minValue, maxValue, numCells,
                new color(0.1, 0.1, 0.1), new color(0.9, 0.9, 0.9));
        }



        /// <summary>Creates and returns a continuous color scale that ranges from dark blue to red.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        public static ColorScale CreateBlueRed(double minValue, double maxValue)
        {
            return new ColorScale(minValue, maxValue,
                new color(0, 0, 0.4), new color(0, 1, 0 ));
        }

        /// <summary>Creates and returns a discrete color scale that ranges from dark blue to red.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        public static ColorScale CreateBlueRed(double minValue, double maxValue, int numCells)
        {
            return new ColorScale(minValue, maxValue, numCells,
                new color(0, 0, 0.4), new color(0, 1, 0 ));
        }



        /// <summary>Creates and returns a continuous color scale that ranges from dark blue to yellow.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        public static ColorScale CreateBlueYellow(double minValue, double maxValue)
        {
            return new ColorScale(minValue, maxValue,
                new color(0, 0, 0.4), new color(1, 1, 0 ));
        }

        /// <summary>Creates and returns a discrete color scale that ranges from dark blue to yellow.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        public static ColorScale CreateBlueYellow(double minValue, double maxValue, int numCells)
        {
            return new ColorScale(minValue, maxValue, numCells,
                new color(0, 0, 0.4), new color(1, 1, 0 ));
        }



        /// <summary>Creates and returns a continuous color scale that runs from blue through 
        /// red till yellow color.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        public static ColorScale CreateBlueRedYellow(double minValue, double maxValue)
        {
            return new ColorScale(minValue, maxValue,
                System.Drawing.Color.Blue, System.Drawing.Color.Red, System.Drawing.Color.Yellow);
        }

        /// <summary>Creates and returns a discrete color scale that runs from blue through 
        /// red till yellow color.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        public static ColorScale CreateBlueRedYellow(double minValue, double maxValue, int numCells)
        {
            return new ColorScale(minValue, maxValue, numCells,
                System.Drawing.Color.Blue, System.Drawing.Color.Red, System.Drawing.Color.Yellow);
        }


        /// <summary>Creates and returns a continuous color scale that runs from blue through 
        /// green till red color.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        public static IColorScale CreateBlueGreenYellow(double minValue, double maxValue)
        {
            return new ColorScale(minValue, maxValue,
                System.Drawing.Color.Blue, System.Drawing.Color.Green, System.Drawing.Color.Red);
        }

        /// <summary>Creates and returns a discrete color scale that runs from blue through 
        /// green till red color.</summary>
        /// <param name="minValue">Lower bound of the range of values for which color scale is defined.</param>
        /// <param name="maxValue">Upper bound of the range of values for which color scale is defined.</param>
        /// <param name="numCells">Number of discrete cells of the color scale.</param>
        public static ColorScale CreateBlueGreenRed(double minValue, double maxValue, int numCells)
        {
            return new ColorScale(minValue, maxValue, numCells,
                System.Drawing.Color.Blue, System.Drawing.Color.Green, System.Drawing.Color.Red);
        }

#endregion FixedScales


#endregion Static


    }  // abstract class ColorScaleBase

}