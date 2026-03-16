using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib
{


    public static class StringParserProvider
    {

        public static IStringParser<T> GetParser<T>()
        {
            Type type = typeof(T);



            throw new NotSupportedException($"Type {type.FullName} is not supported.");

        }


    }

}
