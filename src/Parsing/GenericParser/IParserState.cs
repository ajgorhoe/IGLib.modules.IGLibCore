using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing.GenericParser
{

    public interface  IParserState
    {

        IParserState Clone();

        int Position { get; set; }


    }


}
