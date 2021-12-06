using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing.GenericParser
{
    internal interface IParser<CharType>
        where CharType : struct
    {


        IParsable<CharType> Parsable { get; }

        IParserState State { get; }

        int Position { get; set; }

        CharType this [int index] { get; }

        CharType Next { get; }

        /// <summary>Returns a substring of <see cref="Parsable"/> starting at <paramref name="startIndex"/> and
        /// of length <paramref name="length"/></summary>
        /// <param name="startIndex">Starting position at which sub-string is taken.</param>
        /// <param name="length">Number of characters in the returned substring.</param>
        /// <returns>The substring corresponding to parameters.</returns>
        IEnumerable<CharType> SubString(int startIndex, int length);


        IEnumerable<CharType> SubString(int startIndex);


        bool IsAt(int position, CharType ch);

        bool IsNext(CharType ch);

        bool IsAt(int position, Func<CharType, bool> characterMatcher);

        bool IsNext(Func<CharType, bool> characterMatcher);

        bool IsAt(int position, IEnumerable<CharType> str);

        bool IsNext(IEnumerable<CharType> str);

        bool IsAt(Func<IEnumerable<CharType>, bool> stringMatcher);

        bool IsNext(Func<IEnumerable<CharType>, bool> stringMatcher);



    }
}
