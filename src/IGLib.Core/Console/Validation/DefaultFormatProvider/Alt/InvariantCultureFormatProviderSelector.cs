using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IGLib.Alt;

public sealed class InvariantCultureFormatProviderSelector : IDefaultFormatProviderSelector
{
    public IFormatProvider DefaultFormatProvider => CultureInfo.InvariantCulture;

}
