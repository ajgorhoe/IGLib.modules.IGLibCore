

// AGGREGATE CONSOLE INTERFACES (the core interface supplemented by various combinations of
// capability interfaces).

using System;

namespace IGLib.ConsoleUtils
{

    /// <summary>Full console interface, supplements the basic <see cref="IConsole"/> with capability
    /// interfaces <see cref="IConsoleKeyInput"/>, <see cref="IConsoleColors"/>, <see cref="IConsoleScreen"/>.</summary>
    public interface IFullConsole : IConsole, IConsoleKeyInput, IConsoleColors, IConsoleScreen 
    { }



}