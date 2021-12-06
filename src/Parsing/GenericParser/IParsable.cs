using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing
{

    /// <summary>Interface for parsable strings and their genetalizations.</summary>
    /// <typeparam name="CharType"></typeparam>
    public interface IParsable<CharType>
        where CharType: struct
    {

        int Length { get; }

        CharType this[int i]
        { get; set; }

        

    }


}
