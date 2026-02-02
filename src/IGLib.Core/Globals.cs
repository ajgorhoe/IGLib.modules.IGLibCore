using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IGLib
{

    public static class Global
    {

        /// <summary>Specifies the default <see cref="IFormatProvider"/> to be used in various APIs, such as
        /// console utilities for reading numbers and boolean values. (see e.g. class 
        /// <see cref="IGLib.ConsoleUtils.ConsoleUtilities"/>).</summary>
        public static IFormatProvider DefaultFormatProvider { get; internal set; } 
            = CultureInfo.InvariantCulture;


    }


}
