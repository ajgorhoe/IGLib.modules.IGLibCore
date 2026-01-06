#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using IG.Num;


namespace IG.Lib
{


    /// <summary>Extended color representation. 
    /// Stores RGB components and opacity as double numbers ranging fom 0 to 1.
    /// Implicit conversions to and from <see cref="System.Drawing.Color"/> and form <see cref="System.Drawing.KnownColor"/>
    /// are provided.</summary>
    /// $A Igor xx;
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public struct color
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {

        #region Constructors

        /// <summary>Creates a new color with specified red, green and blue components and opacity.</summary>
        /// <param name="r">Red component of the created color (range 0 to 1).</param>
        /// <param name="g">Green component of the created color (range 0 to 1).</param>
        /// <param name="b">Blue component of the created color (range 0 to 1).</param>
        /// <param name="opacity">Opacity of the created color (range 0 - completely transparent, 
        /// 1 - completely opaque).</param>
        public color(double r, double g, double b, double opacity)
        {
            _R = r;
            _G = g;
            _B = b;
            _opacity = opacity;
        }

        /// <summary>Creates a new color with specified red, green and blue components. 
        /// Opacity is set to 1.</summary>
        /// <param name="r">Red component of the created color (range 0 to 1).</param>
        /// <param name="g">Green component of the created color (range 0 to 1).</param>
        /// <param name="b">Blue component of the created color (range 0 to 1).</param>
        public color(double r, double g, double b)
        {
            _R = r;
            _G = g;
            _B = b;
            _opacity = 1.0;
        }

        /// <summary>Creates a new color that corresponds (as closely as possible) to the specified color.</summary>
        /// <param name="color">Color that is copied to the created color.</param>
        public color(System.Drawing.Color color)
        {
            _R = ((double)color.R) / MaxIntComponentD;
            _G = ((double)color.G) / MaxIntComponentD;
            _B = ((double)color.B) / MaxIntComponentD;
            _opacity = ((double)color.A) / MaxIntComponentD;
        }

        /// <summary>Creates a new color that corresponds to the specified known (system) color enumerated by the 
        /// <see cref="System.Drawing.KnownColor"/> enumerator, such as <see cref="KnownColor.Menu"/> or <see cref="KnownColor.WindowFrame"/>.</summary>
        /// <param name="knownColor">Color name.</param>
#if false
        // REMARK: This was enclosed in "#if false" because KnownColor and Color.FromKnownColor
        // are not availale in .NET Standard 2.0.

        public color(System.Drawing.KnownColor knownColor)
            : this(System.Drawing.Color.FromKnownColor(knownColor))
        { }
#endif

        /// <summary>Creates a new color that corresponds to the specified
        /// color.</summary>
        /// <param name="color">Color that is copied to the created color.</param>
        public color(color color)
        {
            _R = color.R;
            _G = color.G;
            _B = color.B;
            _opacity = color.Opacity;
        }

        /// <summary>Creates a new color that corresponds to the specified color name (e.g. "white", "blue", etc.).</summary>
        /// <param name="name">Color name.</param>
        public color(string name) :
            this(System.Drawing.Color.FromName(name))
        { }


#endregion Constructors

#region Data

        private double _R, _G, _B, _opacity;

        /// <summary>Maximal integer component of the standard <see cref="System.Drawing.Color"/> struct.</summary>
        public const int MaxIntComponent = 255;

        /// <summary>Maximal component of the standard <see cref="System.Drawing.Color"/> struct as double.</summary>
        public const double MaxIntComponentD = MaxIntComponent;

        /// <summary>Red component of the color (range from 0 to 1).</summary>
        public double R
        { get { return _R; } set { _R = value; } }

        /// <summary>Green component of the color (range from 0 to 1).</summary>
        public double G
        { get { return _G; } set { _G = value; } }

        /// <summary>Blue component of the color (range from 0 to 1).</summary>
        public double B
        { get { return _B; } set { _B = value; } }

        /// <summary>Opacity of the color (range from 0 - completely transparent - to 1 - completely opaque).</summary>
        public double Opacity
        { get { return _opacity; } set { _opacity = value; } }

#endregion Data

#region ColorConversions

        /// <summary>Gets or sets the red RGB component as integer in the range 0 to 255.
        /// Getter clips the returned value if it would exceed the range.
        /// Setter does not perform clipping.</summary>
        public int IntR
        {
            get
            {
                int ret = (int)(R * MaxIntComponentD);
                if (ret < 0)
                    ret = 0;
                else if (ret > MaxIntComponent)
                    ret = MaxIntComponent;
                return ret;
            }
            set { R = ((double)value) / MaxIntComponentD; }
        }

        /// <summary>Gets or sets the green RGB component as integer in the range 0 to 255.
        /// Getter clips the returned value if it would exceed the range.
        /// Setter does not perform clipping.</summary>
        public int IntG
        {
            get
            {
                int ret = (int)(G * MaxIntComponentD);
                if (ret < 0)
                    ret = 0;
                else if (ret > MaxIntComponent)
                    ret = MaxIntComponent;
                return ret;
            }
            set { G = ((double)value) / MaxIntComponentD; }
        }

        /// <summary>Gets or sets the blue RGB component as integer in the range 0 to 255.
        /// Getter clips the returned value if it would exceed the range.
        /// Setter does not perform clipping.</summary>
        public int IntB
        {
            get
            {
                int ret = (int)(B * MaxIntComponentD);
                if (ret < 0)
                    ret = 0;
                else if (ret > MaxIntComponent)
                    ret = MaxIntComponent;
                return ret;
            }
            set { B = ((double)value) / MaxIntComponentD; }
        }

        /// <summary>Gets or sets the Opacity as integer in the range 0 to 255.
        /// Getter clips the returned value if it would exceed the range.
        /// Setter does not perform clipping.</summary>
        public int IntOpacity
        {
            get
            {
                int ret = (int)(Opacity * MaxIntComponentD);
                if (ret < 0)
                    ret = 0;
                else if (ret > MaxIntComponent)
                    ret = MaxIntComponent;
                return ret;
            }
            set { Opacity = ((double)value) / MaxIntComponentD; }
        }

        /// <summary>Gets the red RGB component of the current color as float number in the range 0 to 1.
        /// No clipping is performed.</summary>
        public float FloatR
        {
            get { return (float)R; }
        }

        /// <summary>Gets the green RGB component of the current color as float number in the range 0 to 1.
        /// No clipping is performed.</summary>
        public float FloatG
        {
            get { return (float)G; }
        }

        /// <summary>Gets the blue RGB component of the current color as float number in the range 0 to 1.
        /// No clipping is performed.</summary>
        public float FloatB
        {
            get { return (float)B; }
        }

        /// <summary>Gets the opacity of the current color as float number in the range 0 to 1.
        /// No clipping is performed.</summary>
        public float FloatOpacity
        {
            get { return (float)Opacity; }
        }

        /// <summary>Gets the hue-saturation-brightnes (HSB) value for hue of the current color (in the range 0 to 1).</summary>
        public double Hue
        {
            get
            {
                System.Drawing.Color color = this;
                return color.GetHue();
            }
        }

        /// <summary>Gets the hue-saturation-brightnes (HSB) value for hue of the current color (in the range 0 to 1).</summary>
        public double Saturation
        {
            get
            {
                System.Drawing.Color color = this;
                return color.GetSaturation();
            }
        }

        /// <summary>Gets the hue-saturation-brightnes (HSB) value for brightness of the current color (in the range 0 to 1).</summary>
        public double Brightness
        {
            get
            {
                System.Drawing.Color color = this;
                return color.GetBrightness();
            }
        }

#endregion ColorConversions

#region Conversions

        public double ConvertComponent(int component)
        {
            return (double)component / MaxIntComponentD;
        }

        /// <summary>Converts <see cref="color"/> to standard <see cref="System.Drawing.Color"/></summary>
        /// <param name="col">Color to be converted.</param>
        /// <returns><see cref="System.Drawing.Color"/> value corresponding to the specified color.</returns>
        public static implicit operator System.Drawing.Color(color col)
        {
            int
                r = (int)(col.R * MaxIntComponentD),
                g = (int)(col.G * MaxIntComponentD),
                b = (int)(col.B * MaxIntComponentD),
                a = (int)(col.Opacity * MaxIntComponentD);
            if (r < 0)
                r = 0;
            else if (r > MaxIntComponent)
                r = MaxIntComponent;
            if (g < 0)
                g = 0;
            else if (g > MaxIntComponent)
                g = MaxIntComponent;
            if (b < 0)
                b = 0;
            else if (b > MaxIntComponent)
                b = MaxIntComponent;
            if (a < 0)
                a = 0;
            else if (a > MaxIntComponent)
                a = MaxIntComponent;
            return System.Drawing.Color.FromArgb(a, r, g, b);
        }

        /// <summary>Converts standard color representation <see cref="System.Drawing.Color"/> to 
        /// extended representation <see cref="color"/>.</summary>
        /// <param name="col">Color to be converted.</param>
        /// <returns><see cref="color"/> value corresponding to the specified color.</returns>
        public static implicit operator color(System.Drawing.Color col)
        {
            return new color(col);
        }

#if false
        // REMARK: This was enclosed in "#if false" because KnownColor and Color.FromKnownColor
        // are not availale in .NET Standard 2.0.

        /// <summary>Converts the <see cref="System.Drawing.KnownColor"/> enumerator to 
        /// extended representation <see cref="Color"/>.</summary>
        /// <param name="knownColorEnum">Value that is converted.</param>
        /// <returns><see cref="Color"/> value corresponding to the specified color.</returns>
        public static implicit operator color(System.Drawing.KnownColor knownColorEnum)
        {
            System.Drawing.Color col = System.Drawing.Color.FromKnownColor(knownColorEnum);
            return new color(knownColorEnum);
        }
#endif

#endregion Conversions

#region Static

        /// <summary>Returns average of the specified colors.
        /// <para>The calculated color components are clipped to their prescribed range.</para></summary>
        /// <param name="colors">Colors to be averaged.</param>
        /// <returns>An average color whose components are averages over RGB components of all specified colors.</returns>
        public static color Average(params color[] colors)
        {
            return Average(true /* clipComponents */, colors);
        }


        /// <summary>Returns average of the specified colors.
        /// <para>Depending on the specified flag, clipping of calculated color components to the prescribed range is
        /// performed or not.</para></summary>
        /// <param name="clipComponents">Whether the resulting color components are clipped to the prescribed range or not.</param>
        /// <param name="colors">Colors to be averaged.</param>
        /// <returns>An average color whose components are averages over components of all specified colors.</returns>
        public static color Average(bool clipComponents, params color[] colors)
        {
            double r = 0, g = 0, b = 0, opacity = 0;
            int length = 0;
            if (colors != null)
                length = colors.Length;
            for (int i = 0; i < length; ++i)
            {
                color current = colors[i];
                r += current.R;
                g += current.G;
                b += current.B;
                opacity += current.Opacity;
            }
            r /= (double)length;
            g /= (double)length;
            b /= (double)length;
            if (clipComponents)
            {
                if (r < 0) r = 0;
                if (r > 1) r = 1;
                if (g < 0) g = 0;
                if (g > 1) g = 1;
                if (b < 0) b = 0;
                if (b > 1) b = 1;
                if (opacity < 0) opacity = 0;
                if (opacity > 1) opacity = 1;
            }
            return new color(r, g, b, opacity);
        }



        /// <summary>Returns additive mixture (linear combination) of two colors.
        /// <para>The calculated color components are clipped to their prescribed range.</para>
        /// <para>The sum of weights will usually be normalized 1, but this is not necessary. 
        /// The method itself does not provide normalization.</para></summary>
        /// <param name="weight1">Weighting factor for the first color.</param>
        /// <param name="color1">The first color to be mixed.</param>
        /// <param name="weight2">Weighting factor for the second color.</param>
        /// <param name="color2">The second color to be mixed.</param>
        /// <returns>A mixed color whose components are linear combinations of RGB components of the specified two colors.</returns>
        public static color Mixture(double weight1, color color1, double weight2, color color2)
        {
            return Mixture(true /* clipComponents */, weight1, color1, weight2, color2);
        }

        /// <summary>Returns additive mixture (linear combination) of two colors.
        /// <para>Depending on the specified flag, clipping of calculated color components to the prescribed range is
        /// performed or not.</para>
        /// <para>The sum of weights will usually be normalized 1, but this is not necessary. 
        /// The method itself does not provide normalization.</para></summary>
        /// <param name="clipComponents">Whether the resulting color components are clipped to the prescribed range or not.</param>
        /// <param name="weight1">Weighting factor for the first color.</param>
        /// <param name="color1">The first color to be mixed.</param>
        /// <param name="weight2">Weighting factor for the second color.</param>
        /// <param name="color2">The second color to be mixed.</param>
        /// <returns>A mixed color whose components are linear combinations of RGB components of the specified two colors.</returns>
        public static color Mixture(bool clipComponents, double weight1, color color1, double weight2, color color2)
        {
            double r = 0, g = 0, b = 0, opacity = 0;
            r += weight1 * color1.R + weight2 * color2.R;
            g += weight1 * color1.G + weight2 * color2.G;
            b += weight1 * color1.B + weight2 * color2.B;
            opacity += weight1 * color1.Opacity + weight2 * color2.Opacity;
            if (clipComponents)
            {
                if (r < 0) r = 0;
                if (r > 1) r = 1;
                if (g < 0) g = 0;
                if (g > 1) g = 1;
                if (b < 0) b = 0;
                if (b > 1) b = 1;
                if (opacity < 0) opacity = 0;
                if (opacity > 1) opacity = 1;
            }
            return new color(r, g, b, opacity);
        }



        /// <summary>Returns additive mixture (linear combination) of the specified colors.
        /// <para>The calculated color components are clipped to their prescribed range.</para>
        /// <para>The sum of specified weights will usually be normalized 1, but this is not necessary. 
        /// The method itself does not provide normalization.</para></summary>
        /// <param name="weights">Weights vorresponding to colors that are mixed. Usually the sum of weights will be 1.</param>
        /// <param name="colors">Colors to be additively mixed.</param>
        /// <returns>A mixed color whose components are linear combinations of RGB components of the specified colors.</returns>
        public static color Mixture(double[] weights, color[] colors)
        {
            return Mixture(true /* clipComponents */, weights, colors);
        }


        /// <summary>Returns additive mixture (linear combination) of the specified colors.
        /// <para>Depending on the specified flag, clipping of calculated color components to the prescribed range is
        /// performed or not.</para>
        /// <para>The sum of specified weights will usually be normalized 1, but this is not necessary. 
        /// The method itself does not provide normalization.</para></summary>
        /// <param name="clipComponents">Whether the resulting color components are clipped to the prescribed range or not.</param>
        /// <param name="weights">Weights vorresponding to colors that are mixed. Usually the sum of weights will be 1.</param>
        /// <param name="colors">Colors to be additively mixed.</param>
        /// <returns>A mixed color whose components are linear combinations of RGB components of the specified colors.</returns>
        public static color Mixture(bool clipComponents, double[] weights, color[] colors)
        {
            double r = 0, g = 0, b = 0, opacity = 0;
            int length = 0;
            if (colors != null)
            {
                length = colors.Length;
                if (weights == null)
                    throw new ArgumentException("Weights for color mixing are not specified (null reference), although colors are.");
                if (weights.Length != length)
                    throw new ArgumentException("Number of weights (" + weights.Length + ") is different than number of colors ("
                        + length + ").");
            }
            for (int i = 0; i < length; ++i)
            {
                color current = colors[i];
                double weight = weights[i];
                r += weight*current.R;
                g += weight * current.G;
                b += weight * current.B;
                opacity += weight * current.Opacity;
            }
            if (clipComponents)
            {
                if (r < 0) r = 0;
                if (r > 1) r = 1;
                if (g < 0) g = 0;
                if (g > 1) g = 1;
                if (b < 0) b = 0;
                if (b > 1) b = 1;
                if (opacity < 0) opacity = 0;
                if (opacity > 1) opacity = 1;
            }
            return new color(r, g, b, opacity);
        }


#region ColorScales


        /// <summary>Returns a color corresponding to the specified value (from the specified range) where color 
        /// scale begins with the first specified color (lowest value) and smoothly changes to the second specified color (highest value).
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="col1">Collor corresponding to the lowest value of the scale.</param>
        /// <param name="col2">Collor corresponding to the highest value of the scale.</param>
        /// <param name="minValue">Lowest value for the color.</param>
        /// <param name="maxValue">Highest value for the color.</param>
        /// <param name="value">Value for which the corresponding color is returned.</param>
        public static color Scale(color col1, color col2, double minValue, double maxValue, double value)
        {
            return Scale(col1, col2, (value - minValue) / (maxValue - minValue));
        }

        /// <summary>Returns a color corresponding to the specified value (ranging from 0 to 1) where color 
        /// scale begins with the first specified color (at 0) and smoothly changes to the second specified color (at 1).
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="col1">Color corresponding to the lowest value of the scale.</param>
        /// <param name="col2">Color corresponding to the highest value of the scale.</param>
        /// <param name="value">Value for which the corresponding color is returned. Should be between 0 or 1.</param>
        public static color Scale(color col1, color col2, double value)
        {
            if (value < 0) value = 0;
            if (value > 1) value = 1;
            double red = 0, green = 0, blue = 0, opacity = 0;
            red = (1.0 - value) * col1.R + value * col2.R;
            green = (1.0 - value) * col1.G + value * col2.G;
            blue = (1.0 - value) * col1.B + value * col2.B;
            opacity = (1.0 - value) * col1.Opacity + value * col2.Opacity;
            if (red < 0) red = 0;
            if (red > 1) red = 1;
            if (green < 0) green = 0;
            if (green > 1) green = 1;
            if (blue < 0) blue = 0;
            if (blue > 1) blue = 1;
            if (opacity < 0) opacity = 0;
            if (opacity > 1) opacity = 1;
            return new color(red, green, blue, opacity);
        }


        /// <summary>Returns a color corresponding to the specified value (within the specified range) where color 
        /// scale begins with the first specified color (at the lower bound of the range) and smoothly changes through
        /// other colors up to the last specified color (at the higher bound of the range).
        /// <para>If the value is out of range then color for lower or upper end of scale is returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="colors">Table of colors that are used in color scale. The first color of the table 
        /// corresponds to the lower bound of the range, and the last one corresponds to the upper bound of
        /// the range. Colors between correspond to equally spaced values between, and colors between these
        /// values continuously change between two neighboring colors from the table.</param>
        /// <param name="minValue">Lower bound of the range of values for which colors are assigned.</param>
        /// <param name="maxValue">Upper bound of the range of values for which colors are assigned.</param>
        /// <param name="value">Value for which the corresponding color is returned. Should be between 0 or 1.</param>
        public color Scale(color[] colors, double minValue, double maxValue, double value)
        {
            return Scale(colors, (value - minValue) / (maxValue - minValue));
        }


        /// <summary>Returns a color corresponding to the specified value (ranging from 0 to 1) where color 
        /// scale begins with the first specified color (at 0) and smoothly changes through other colors up
        /// to the last specified color (at 1).
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="colors">Table of colors that are used in color scale. The first color of the table 
        /// corresponds to the lower bound of the range, and the last one corresponds to the upper bound of
        /// the range. Colors between correspond to equally spaced values between, and colors between these
        /// values continuously change between two neighboring colors from the table.</param>
        /// <param name="value">Value for which the corresponding color is returned. Should be between 0 or 1.</param>
        public static color Scale(color[] colors, double value)
        {
            if (colors==null)
                throw new ArgumentNullException("colors", "Colors not specified (null reference).");
            int num = colors.Length;
            if (num == 0)
                throw new ArgumentException("There are no colors specified (empty array).");
            else if (num == 1)
                return colors[0];
            double h = 1.0 / ((double)num - 1.0);  // Length of interval between two marker colors
            double hFactor = value / h;
            int cellIndex = (int) Math.Floor(hFactor);
            if (cellIndex < 0)
                return colors[0];
            else if (cellIndex >= (num - 1))
                return colors[num - 1];
            else
            {
                double cellValue = hFactor - (double) cellIndex;  // running from 0 to 1 within a cell
                color col1 = colors[cellIndex];
                color col2 = colors[cellIndex + 1];
                if (cellValue < 0) cellValue = 0;
                if (cellValue > 1) cellValue = 1;
                double red = 0, green = 0, blue = 0, opacity = 0;
                red = (1.0 - cellValue) * col1.R + cellValue * col2.R;
                green = (1.0 - cellValue) * col1.G + cellValue * col2.G;
                blue = (1.0 - cellValue) * col1.B + cellValue * col2.B;
                opacity = (1.0 - cellValue) * col1.Opacity + cellValue * col2.Opacity;
                if (red < 0) red = 0;
                if (red > 1) red = 1;
                if (green < 0) green = 0;
                if (green > 1) green = 1;
                if (blue < 0) blue = 0;
                if (blue > 1) blue = 1;
                if (opacity < 0) opacity = 0;
                if (opacity > 1) opacity = 1;
                return new color(red, green, blue, opacity);
            }
        }


        /// <summary>Returns a color corresponding to the specified value (from the specified range) where color 
        /// scale begins with black and smoothly changes to white.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether the value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="minValue">Minimal value of the scale.</param>
        /// <param name="maxValue">Maximal value of the scale.</param>
        /// <param name="value">Value for which the corresponding color is returned.</param>
        public static color ScaleGray(double minValue, double maxValue, double value)
        {
            return ScaleGray((value - minValue) / (maxValue - minValue));
        }

        /// <summary>Returns a color corresponding to the specified value (ranging from 0 to 1) where color 
        /// scale begins with black and smoothly changes to white.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="value">Value for which the corresponding color is returned. Range of the scale is between 0 or 1.</param>
        public static color ScaleGray(double value)
        {
            return Scale(new color(0, 0, 0), new color(1, 1, 1), value);
        }

        /// <summary>Returns a color corresponding to the specified value (from the specified range) where color 
        /// scale begins with black and smoothly changes to red.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether the value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="minValue">Minimal value of the scale.</param>
        /// <param name="maxValue">Maximal value of the scale.</param>
        /// <param name="value">Value for which the corresponding color is returned.</param>
        public static color ScaleRed(double minValue, double maxValue, double value)
        {
            return ScaleRed((value - minValue) / (maxValue - minValue));
        }

        /// <summary>Returns a color corresponding to the specified value (ranging from 0 to 1) where color 
        /// scale begins with black and smoothly changes to red.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="value">Value for which the corresponding color is returned. Range of the scale is between 0 or 1.</param>
        public static color ScaleRed(double value)
        {
            return Scale(new color(0, 0, 0), new color(1, 0, 0), value);
        }

        /// <summary>Returns a color corresponding to the specified value (from the specified range) where color 
        /// scale begins with black and smoothly changes to green.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether the value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="minValue">Minimal value of the scale.</param>
        /// <param name="maxValue">Maximal value of the scale.</param>
        /// <param name="value">Value for which the corresponding color is returned.</param>
        public static color ScaleGreen(double minValue, double maxValue, double value)
        {
            return ScaleGreen((value - minValue) / (maxValue - minValue));
        }

        /// <summary>Returns a color corresponding to the specified value (ranging from 0 to 1) where color 
        /// scale begins with black and smoothly changes to green.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="value">Value for which the corresponding color is returned. Range of the scale is between 0 or 1.</param>
        public static color ScaleGreen(double value)
        {
            return Scale(new color(0, 0, 0), new color(0, 1, 0), value);
        }

        /// <summary>Returns a color corresponding to the specified value (from the specified range) where color 
        /// scale begins with black and smoothly changes to blue.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether the value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="minValue">Minimal value of the scale.</param>
        /// <param name="maxValue">Maximal value of the scale.</param>
        /// <param name="value">Value for which the corresponding color is returned.</param>
        public static color ScaleBlue(double minValue, double maxValue, double value)
        {
            return ScaleBlue((value - minValue) / (maxValue - minValue));
        }

        /// <summary>Returns a color corresponding to the specified value (ranging from 0 to 1) where color 
        /// scale begins with black and smoothly changes to blue.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="value">Value for which the corresponding color is returned. Range of the scale is between 0 or 1.</param>
        public static color ScaleBlue(double value)
        {
            return Scale(new color(0, 0, 0), new color(0, 0, 1), value);
        }

        /// <summary>Returns a color corresponding to the specified value (from the specified range) where color 
        /// scale begins with black and smoothly changes to blue.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether the value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="minValue">Minimal value of the scale.</param>
        /// <param name="maxValue">Maximal value of the scale.</param>
        /// <param name="value">Value for which the corresponding color is returned.</param>
        public static color ScaleYellow(double minValue, double maxValue, double value)
        {
            return ScaleYellow((value - minValue) / (maxValue - minValue));
        }

        /// <summary>Returns a color corresponding to the specified value (ranging from 0 to 1) where color 
        /// scale begins with black and smoothly changes to blue.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="value">Value for which the corresponding color is returned. Range of the scale is between 0 or 1.</param>
        public static color ScaleYellow(double value)
        {
            return Scale(new color(0, 0, 0), System.Drawing.Color.Yellow, value);
        }



        /// <summary>Returns a color corresponding to the specified value (from the specified range) where color 
        /// scale begins with blue, then changes to red and finally to yellow.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="minValue">Minimal value of the scale.</param>
        /// <param name="maxValue">Maximal value of the scale.</param>
        /// <param name="value">Value for which the corresponding color is returned.</param>
        public static color ScaleBlueRedYellow(double minValue, double maxValue, double value)
        {
            return ScaleBlueRedYellow((value - minValue) / (maxValue - minValue));
        }

        /// <summary>Returns a color corresponding to the specified value (ranging from 0 to 1) where color 
        /// scale begins with blue, then changes to red and finally to yellow.
        /// <para>If the value is out of range then colors for lower or upper end of scale are returned,
        /// dependent on whether value is smaller than the lower bound or larger than the upper bound.</para></summary>
        /// <param name="value">Value for which the corresponding color is returned. Range of the scale is between 0 or 1.</param>
        public static color ScaleBlueRedYellow(double value)
        {
            if (value < 0) value = 0;
            if (value > 1) value = 1;
            double red = 0, green = 0, blue = 0;
            blue = 1.0 - 2 * value;
            red = 2 * value;
            green = 2 * (value - 0.5);
            if (red < 0) red = 0;
            if (red > 1) red = 1;
            if (green < 0) green = 0;
            if (green > 1) green = 1;
            if (blue < 0) blue = 0;
            if (blue > 1) blue = 1;
            return new color(red, green, blue);
        }


#endregion ColorScales

#endregion Static

    }

}