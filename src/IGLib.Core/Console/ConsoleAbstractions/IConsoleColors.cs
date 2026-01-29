
using System;

namespace IGLib.ConsoleUtils
{

    public interface IConsoleColors
    {

        /// <summary>Gets or sets the foreground color of the console.</summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>Gets or sets the background color of the console.</summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>Sets the current console's foreground and background colors to their defaults.</summary>
        void ResetColor();

    }

}