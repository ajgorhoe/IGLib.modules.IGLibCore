using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IGLib
{


    internal static class StringParserProvider
    {
        public static IStringParser<T> GetParser<T>(IFormatProvider? formatProvider = null)
        {
            object parser = GetParser(typeof(T), formatProvider ?? CultureInfo.CurrentCulture);

            return (IStringParser<T>)parser;
        }

        private static object GetParser(Type targetType, IFormatProvider formatProvider)
        {
            if (targetType == typeof(string))
                return new IdentityStringParser();

            if (targetType == typeof(bool))
                return new BooleanStringParser();

            if (targetType == typeof(int))
                return new Int32StringParser(formatProvider);

            if (targetType == typeof(decimal))
                return new DecimalStringParser(formatProvider);



            throw new NotSupportedException(
                $"No built-in string parser is available for target type '{targetType.FullName}'.");
        }

    }

}
