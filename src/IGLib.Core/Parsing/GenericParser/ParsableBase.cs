// Copyright © Igor Grešovnik (2008 - present). License:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing
{


    public abstract class ParsableBase<CharType> : IParsable<CharType>
        where CharType : struct
    {
 

        /// <summary>Returns true if the specified characters are equal, false otherwise.</summary>
        /// <param name="ch1">The first character to be compared.</param>
        /// <param name="ch2">The second character to be compared.</param>
        /// <returns></returns>
        public abstract bool AreEqual(CharType ch1, CharType ch2);

        /// <inheritdoc/>
        public abstract int Length { get; }

        /// <inheritdoc/>
        public abstract CharType this[int i]
        {  get; }


        /// <summary>Returns a substring of <see cref="Parsable"/> starting at <paramref name="startIndex"/> and
        /// of length <paramref name="length"/></summary>
        /// <param name="startIndex">Starting position at which sub-string is taken.</param>
        /// <param name="length">Number of characters in the returned substring.</param>
        /// <returns>The substring corresponding to parameters.</returns>
        public virtual IEnumerable<CharType> SubString(int startIndex, int length)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex),
                    $"Starting index must be greater or equal to 0. Specified: {startIndex}");
            }
            if (startIndex >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex),
                    $"Starting index must be lesser or equal to parsable length. Specified: {startIndex}, length: {Length}");
            }
            int last = startIndex + length;
            if (last >= Length)
            {
                // prevent overflow:
                length -= last - (length - 1);
            }
            for (int i = 0; i < length ; i++)
            {
                yield return this[startIndex + i];
            }
        }


        public IEnumerable<CharType> SubString(int startIndex)
        {
            return SubString(startIndex, Length-startIndex);
        }


        /// <summary>Returns true if the specified character is at the specified position of the parsable.</summary>
        /// <param name="position">Positio ofr which character is verified.</param>
        /// <param name="ch">Character that is expected at <paramref name="position"/></param>
        /// <returns></returns>
        public virtual bool IsAt(int position, CharType ch)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position),
                    $"Starting index must be greater or equal to 0. Specified: {position}");
            }
            if (position >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position),
                    $"Starting index must be lesser or equal to parsable length. Specified: {position}, length: {Length}");
            }
            return AreEqual(this[position], ch); 
        }

        public virtual bool IsAt(int position, Func<CharType, bool> characterMatcher) 
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position),
                    $"Starting index must be greater or equal to 0. Specified: {position}");
            }
            if (position >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position),
                    $"Starting index must be lesser or equal to parsable length. Specified: {position}, length: {Length}");
            }
            return characterMatcher(this[position]);
        }

        public virtual bool IsAt(int position, IEnumerable<CharType> str) 
        {
            int index = 0;
            foreach (CharType ch in str)
            {
                if (index >= Length)
                    return false;
                CharType actual = this[index];
                if (!AreEqual(ch, actual))
                {
                    return false;
                }
            }
            return true;
        }

        public virtual bool IsAt(int position, Func<IEnumerable<CharType>, bool> stringMatcher, int stringLength)
        {
            return stringMatcher(SubString(position, stringLength));
        }


    }


}
