// Copyright © Igor Grešovnik (2008 - present). License:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing
{
    public class ParsableFromString: ParsableBase<char>, IParsable<char>
    {

        public override bool AreEqual(char ch1, char ch2)
        {
            return ch1 == ch2;
        }

        public override int Length => Sb.Length;

        public ParsableFromString(string parsedString)
        {
            Sb = new StringBuilder(parsedString);
        }

        public override char this[int i] { get => Sb[i]; }

        /// <summary>Holds the copy of parsed string iinternally; <see cref="StringBuilder"/> is
        /// used instead of <see cref="string"/> for faster performance.</summary>
        protected StringBuilder Sb { get; set; }

    }

}
