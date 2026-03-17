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


internal sealed class ByteStringParser : IStringParser<byte>
{
    private readonly IFormatProvider _formatProvider;

    public ByteStringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out byte value)
    {
        return byte.TryParse(text, NumberStyles.Integer, _formatProvider, out value);
    }
}


internal sealed class SByteStringParser : IStringParser<sbyte>
{
    private readonly IFormatProvider _formatProvider;

    public SByteStringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out sbyte value)
    {
        return sbyte.TryParse(text, NumberStyles.Integer, _formatProvider, out value);
    }
}


internal sealed class Int16StringParser : IStringParser<short>
{
    private readonly IFormatProvider _formatProvider;

    public Int16StringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out short value)
    {
        return short.TryParse(text, NumberStyles.Integer, _formatProvider, out value);
    }
}


internal sealed class UInt16StringParser : IStringParser<ushort>
{
    private readonly IFormatProvider _formatProvider;

    public UInt16StringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out ushort value)
    {
        return ushort.TryParse(text, NumberStyles.Integer, _formatProvider, out value);
    }
}


internal sealed class Int32StringParser : IStringParser<int>
{
    private readonly IFormatProvider _formatProvider;

    public Int32StringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out int value)
    {
        return int.TryParse(text, NumberStyles.Integer, _formatProvider, out value);
    }
}


internal sealed class UInt32StringParser : IStringParser<uint>
{
    private readonly IFormatProvider _formatProvider;

    public UInt32StringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out uint value)
    {
        return uint.TryParse(text, NumberStyles.Integer, _formatProvider, out value);
    }
}



