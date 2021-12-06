using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing.GenericParser
{
    public struct ParserStateStruct : IParserState
    {


        public IParserState Clone()
        {
            return new ParserStateStruct {
                Position = Position
            };
        }

        public void CopyFrom(IParserState state)
        {
            throw new NotImplementedException();
        }

        int _position;

        public int Position { get { return _position; } set { _position = value; } }

    }

}
