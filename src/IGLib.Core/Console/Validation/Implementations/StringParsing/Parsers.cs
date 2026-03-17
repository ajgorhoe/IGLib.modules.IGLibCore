using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IGLib;


/// <summary>Dummy identity parser - just returns the original string when parsing a string value from a string.</summary>
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
    public bool TryParse(string text, out bool value)
    {
        return bool.TryParse(text, out value);
    }
}



