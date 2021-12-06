using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing.CommansLineParsing
{


    public abstract class ParsableBase<CharType> : IParsable<CharType>
        where CharType : struct
    {

        public abstract CharType this[int i]
        {  get; set; }
        //{
        //    return default(CharType);
        //}

    

    }


}
