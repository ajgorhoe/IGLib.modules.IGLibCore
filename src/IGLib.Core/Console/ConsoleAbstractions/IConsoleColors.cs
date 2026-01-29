
using System;

namespace IGLib.ConsoleUtils
{

    /// <summary>Capability interface for console abstractions, supplements <see cref="IConsole"/> with the ability
    /// to set or obtain the foreground and background color.</summary>
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