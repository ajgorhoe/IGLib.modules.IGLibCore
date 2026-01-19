using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IGLib.Parsing
{
    
    public interface ICommandlineParser1
    {

        string[] CommandlineToArgs(string commandLine);

        string ArgsToCommandline(IEnumerable<string> commandlineArguments);

    }

}
