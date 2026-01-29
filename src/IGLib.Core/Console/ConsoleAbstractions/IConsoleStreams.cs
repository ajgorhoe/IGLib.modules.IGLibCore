

using System;
using System.IO;

namespace IGLib.ConsoleUtils
{

    public interface IConsoleStreams
    {

        /// <summary>A <see cref="TextReader"/> that reepresents the current console's input stream.</summary>
        TextReader In { get; }

        /// <summary>A <see cref="TextWriter"/> that reepresents the current console's output stream.</summary>
        TextWriter Out { get; }

        /// <summary>A <see cref="TextWriter"/> that reepresents the current console's error output stream.</summary>
        TextWriter Error { get; }

    }

}