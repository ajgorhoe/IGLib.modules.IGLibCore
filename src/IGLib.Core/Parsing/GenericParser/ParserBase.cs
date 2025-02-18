using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing
{
    public abstract class ParserBase<CharType> //: IParser<CharType>
        where CharType : struct
    {


        public ParserBase(IParsable<CharType> parsable, IParserState paserState)
        {
            Parsable = parsable;
            State = paserState;
        }

        protected virtual IParsable<CharType> Parsable { get; set; }

        protected virtual IParserState State { get; set; }

        public virtual int Length => Parsable.Length;
        
        public virtual int Position => State.Position;

        public virtual CharType this [int index] => Parsable[index];

        public virtual CharType NextChar => Parsable[State.Position];

        /// <summary>Returns a substring of <see cref="Parsable"/> starting at <paramref name="startIndex"/> and
        /// of length <paramref name="length"/></summary>
        /// <param name="startIndex">Starting position at which sub-string is taken.</param>
        /// <param name="length">Number of characters in the returned substring.</param>
        /// <returns>The substring corresponding to parameters.</returns>
        public virtual IEnumerable<CharType> SubString(int startIndex, int length) => Parsable.SubString(startIndex, length);


        public virtual IEnumerable<CharType> SubString(int startIndex) => SubString(startIndex, Length - startIndex - 1);


        /// <summary>Returns true if the specified character is at the specified position of the parsable.</summary>
        /// <param name="position">Positio ofr which character is verified.</param>
        /// <param name="ch">Character that is expected at <paramref name="position"/></param>
        /// <returns></returns>
        public virtual bool IsAt(int position, CharType ch) => Parsable.IsAt(position, ch);

        public virtual bool IsNext(CharType ch) => IsAt(Position, ch);

        public virtual bool IsAt(int position, Func<CharType, bool> characterMatcher) => Parsable.IsAt(position, characterMatcher);

        public virtual bool IsNext(Func<CharType, bool> characterMatcher) => IsAt(Position, characterMatcher);

        public virtual bool IsAt(int position, IEnumerable<CharType> str) => Parsable.IsAt(position, str);

        public virtual bool IsNext(IEnumerable<CharType> str) => IsAt(Position, str);

        public virtual bool IsAt(int position, Func<IEnumerable<CharType>, bool> stringMatcher, int stringLength) 
            => Parsable.IsAt(position, stringMatcher, stringLength);

        public virtual bool IsNext(Func<IEnumerable<CharType>, bool> stringMatcher, int stringLength) => 
            IsAt(Position, stringMatcher, stringLength);



    }

}
