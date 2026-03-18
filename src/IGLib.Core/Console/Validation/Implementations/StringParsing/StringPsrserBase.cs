
using IGLib;
using System;

internal abstract class StringParserBase<T> : IStringParser<T>
{
    protected StringParserBase(IFormatProvider? formatProvider = null)
    {
        FormatProvider = formatProvider ?? throw new ArgumentNullException(nameof(formatProvider));
    }

    protected IFormatProvider FormatProvider { get; }

    public abstract bool TryParse(string text, out T value);
}

