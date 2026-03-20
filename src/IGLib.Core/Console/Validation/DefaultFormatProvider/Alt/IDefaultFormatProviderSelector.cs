using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IGLib.Alt;

public interface IDefaultFormatProviderSelector
{
    IFormatProvider DefaultFormatProvider { get; }
}


