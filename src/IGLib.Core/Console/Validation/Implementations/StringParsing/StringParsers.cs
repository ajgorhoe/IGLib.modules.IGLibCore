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


internal sealed class Int32StringParser : StringParserBase<int>
{
    public Int32StringParser(IFormatProvider formatProvider)
        : base(formatProvider)
    {
    }

    public override bool TryParse(string text, out int value)
    {
        return int.TryParse(text, NumberStyles.Integer, FormatProvider, out value);
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


internal sealed class Int64StringParser : IStringParser<long>
{
    private readonly IFormatProvider _formatProvider;

    public Int64StringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out long value)
    {
        return long.TryParse(text, NumberStyles.Integer, _formatProvider, out value);
    }
}

internal sealed class UInt64StringParser : IStringParser<ulong>
{
    private readonly IFormatProvider _formatProvider;

    public UInt64StringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out ulong value)
    {
        return ulong.TryParse(text, NumberStyles.Integer, _formatProvider, out value);
    }
}


internal sealed class SingleStringParser : IStringParser<float>
{
    private readonly IFormatProvider _formatProvider;

    public SingleStringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out float value)
    {
        return float.TryParse(
            text,
            NumberStyles.Float | NumberStyles.AllowThousands,
            _formatProvider,
            out value);
    }
}


internal sealed class DoubleStringParser : IStringParser<double>
{
    private readonly IFormatProvider _formatProvider;

    public DoubleStringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out double value)
    {
        return double.TryParse(
            text,
            NumberStyles.Float | NumberStyles.AllowThousands,
            _formatProvider,
            out value);
    }
}


internal sealed class DecimalStringParser : IStringParser<decimal>
{
    private readonly IFormatProvider _formatProvider;

    public DecimalStringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out decimal value)
    {
        return decimal.TryParse(
            text,
            NumberStyles.Number,
            _formatProvider,
            out value);
    }
}


internal sealed class DateTimeStringParser : IStringParser<DateTime>
{
    private readonly IFormatProvider _formatProvider;

    public DateTimeStringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out DateTime value)
    {
        return DateTime.TryParse(
            text,
            _formatProvider,
            DateTimeStyles.None,
            out value);
    }
}


internal sealed class DateTimeOffsetStringParser : IStringParser<DateTimeOffset>
{
    private readonly IFormatProvider _formatProvider;

    public DateTimeOffsetStringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out DateTimeOffset value)
    {
        return DateTimeOffset.TryParse(
            text,
            _formatProvider,
            DateTimeStyles.None,
            out value);
    }
}


internal sealed class TimeSpanStringParser : IStringParser<TimeSpan>
{
    private readonly IFormatProvider _formatProvider;

    public TimeSpanStringParser(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public bool TryParse(string text, out TimeSpan value)
    {
        return TimeSpan.TryParse(text, _formatProvider, out value);
    }
}


internal sealed class GuidStringParser : IStringParser<Guid>
{
    public bool TryParse(string text, out Guid value)
    {
        return Guid.TryParse(text, out value);
    }
}

