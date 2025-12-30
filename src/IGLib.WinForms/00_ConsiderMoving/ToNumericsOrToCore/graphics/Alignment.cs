using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IG.Lib
{

    /// <summary>Defines horizontal alignment of some object.</summary>
    /// $A Igor xx;
    public enum AlignmentHorizontal
    {
        None = 0,  // none or undefined.
        Left = 1,
        Right = 2,
        Centered = 4
    }

    /// <summary>Defines vertical Alignment of some object.</summary>
    /// $A Igor xx;
    public enum AlignmentVertical
    {
        None = 0,  // none or undefined.
        Bottom = 1,
        Top = 2,
        Middle = 4
    }

    /// <summary>Defines alignment (vertical and horizontal) of some object.</summary>
    /// $A Igor xx;
    public struct Alignment
    {

        #region Construction

        /// <summary>Construct the alignment structure with the specified horizontal and vertical alignment.</summary>
        /// <param name="horizontal">Horizontal alignment.</param>
        /// <param name="vertical">Vertical alignment.</param>
        public Alignment(AlignmentHorizontal horizontal, AlignmentVertical vertical)
        {
            this.Horizontal = horizontal;
            this.Vertical = vertical;
        }

        /// <summary>Constructs the alignment structure with specified horizontal alignement and with vertical 
        /// alignment set to <see cref="Alignment.DefaultVerticalAlignment"/>.</summary>
        /// <param name="horizontal">Horizontal alignment.</param>
        public Alignment(AlignmentHorizontal horizontal) :
            this(horizontal, Alignment.DefaultVerticalAlignment) { }

        /// <summary>Constructs the alignment structure with specified vertical alignement and with horizontal 
        /// alignment set to <see cref="Alignment.DefaultHorizontalAlignment"/>.</summary>
        /// <param name="vertical">Vertical alignment.</param>
        public Alignment(AlignmentVertical vertical) :
            this(Alignment.DefaultHorizontalAlignment, vertical) { }

        #endregion Construction


        /// <summary>Horizontal alignment.
        /// <para>Default is <see cref="Alignment.DefaultHorizontalAlignment"/>.</para></summary>
        public AlignmentHorizontal Horizontal;

        /// <summary>Vertical Alignment.
        /// <para>Default is <see cref="Alignment.DefaultVerticalAlignment"/>.</para></summary>
        public AlignmentVertical Vertical;


        /// <summary>Default horizontal alignment.</summary>
        public const AlignmentHorizontal DefaultHorizontalAlignment = AlignmentHorizontal.None;

        /// <summary>Default vertical alignment.</summary>
        public const AlignmentVertical DefaultVerticalAlignment = AlignmentVertical.None;

    } // struct Alignment


}
