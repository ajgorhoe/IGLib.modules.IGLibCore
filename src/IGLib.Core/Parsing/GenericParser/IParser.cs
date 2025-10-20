// Copyright © Igor Grešovnik (2008 - present). License:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing
{

    /// <summary>Interface for imperative parser of string-like constructs.
    /// <para>Main implementation is <see cref="StringParser"/>.</para></summary>
    /// <typeparam name="CharType">Type of basic component of parsed "strings". Strings this can be general
    /// array-like structures (where position is defined) composed of elements of type.</typeparam>
    public interface IParser<CharType>
        where CharType : struct
    {


        //IParsable<CharType> Parsable { get; }

        //IParserState State { get; }

        /// <summary>Current position of the parser.</summary>
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


        /// <summary>Returns true if the specified character is at the specified position of the parsable.</summary>
        /// <param name="position">Positio ofr which character is verified.</param>
        /// <param name="ch">Character that is expected at <paramref name="position"/></param>
        /// <returns></returns>
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
