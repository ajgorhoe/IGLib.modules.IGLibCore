using System;
using System.Globalization;

namespace IGLib;

internal static class StringParserProvider
{
    public static IStringParser<T> GetParser<T>(IFormatProvider formatProvider = null)
    {
        object parser = GetParser(typeof(T), formatProvider ?? CultureInfo.CurrentCulture);

        if (parser is IStringParser<T> typedParser)
            return typedParser;

        throw new InvalidOperationException(
            $"Internal parser provider error: parser for type '{typeof(T).FullName}' " +
            $"has incompatible runtime type '{parser.GetType().FullName}'.");
    }

    private static object GetParser(Type targetType, IFormatProvider formatProvider)
    {
        if (targetType == typeof(string))
            return new IdentityStringParser();

        if (targetType == typeof(bool))
            return new BooleanStringParser();

        if (targetType == typeof(byte))
            return new ByteStringParser(formatProvider);

        if (targetType == typeof(sbyte))
            return new SByteStringParser(formatProvider);








        throw new NotSupportedException(
            $"No built-in string parser is available for target type '{targetType.FullName}'.");
    }
}