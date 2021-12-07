using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing
{

    public interface  IParserState
    {

        /// <summary>Returns a clone of the current parser state.</summary>
        IParserState Clone();

        /// <summary>Copies internal parser state from the specified state.</summary>
        /// <param name="state"><see cref="IParserState"/> from which the current state is copied.</param>
        void CopyFrom(IParserState state);

        /// <summary>Current parser position.</summary>
        int Position { get; set; }


    }


}
