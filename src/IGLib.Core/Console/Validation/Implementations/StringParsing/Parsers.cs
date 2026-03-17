using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib
{

    internal sealed class IdentityStringParser : IStringParser<string>
    {
        public bool TryParse(string text, out string value)
        {
            value = text;
            return true;
        }
    }

    internal sealed class BooleanStringParser : IStringParser<bool>
    {
        public bool TryParse(string text, out bool value) =>
            bool.TryParse(text, out value);
    }

}
