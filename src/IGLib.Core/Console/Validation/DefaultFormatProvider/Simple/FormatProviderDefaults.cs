
using System;
using System.Globalization;

namespace IGLib;

public static class FormatProviderDefaults
{
    public static IFormatProvider Global { get; private set; } = CultureInfo.InvariantCulture;

    public static void SetGlobal(IFormatProvider formatProvider)
    {
        if (formatProvider == null)
            throw new ArgumentNullException(nameof(formatProvider));

        Global = formatProvider;
    }

    public static IFormatProvider Resolve(IFormatProvider? specifiedFormatProvider)
        => specifiedFormatProvider ?? Global;
}

