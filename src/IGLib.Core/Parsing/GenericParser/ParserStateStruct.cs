// Copyright © Igor Grešovnik (2008 - present). License:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing
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
