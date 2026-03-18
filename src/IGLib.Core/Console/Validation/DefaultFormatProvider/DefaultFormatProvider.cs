using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib
{

    public class DefaultFormatProviderSelector
    {

        /// <summary>Returns the global <see cref="IDefaultFormatProviderSelector"/> that is used to provide
        /// the <see cref="IFormatProvider"/> when one is not specified, and also the the
        /// <see cref="IDefaultFormatProviderSelector"/> is not specified.</summary>
        public static IDefaultFormatProviderSelector Global { get; private set; } = 

        public static IFormatProvider GetFormatProvider(IFormatProvider? specifiedProvider,
            IDefaultFormatProviderSelector? selector)
        {
            return (specifiedProvider ?? (selector?? Global).DefaultFormatProvider);
        }

    }

}
