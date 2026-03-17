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


            throw new NotImplementedException();
            // throw new NotSupportedException($"Type {type.FullName} is not supported.");
        }


    }

}
