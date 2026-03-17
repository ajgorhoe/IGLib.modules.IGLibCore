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
            Type targetType = typeof(T);
            IFormatProvider provider = formatProvider ?? CultureInfo.CurrentCulture;





            throw new NotSupportedException(
                $"No built-in string parser is available for target type '{targetType.FullName}'.");
        }


    }

}
