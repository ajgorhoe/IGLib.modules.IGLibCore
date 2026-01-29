

using System;

namespace IGLib.ConsoleUtils
{

    /// <summary>A capability interface that supplements the <see cref="IConsole"/> interface with
    /// keyboard input capability.</summary>
    public interface IConsoleKeyInput
    {

        /// <summary>Waits for console keyboard input, obtains and returns information on the key pressed.</summary>
        /// <param name="intercept">If true then the key pressed is intercepted and is NOT displayed on the console.
        /// Default is false (keys that are read are also displayed in console).</param>
        /// <returns>An object that describes the ConsoleKey constant and Unicode character, if any, that correspond to the 
        /// pressed console key. The returned <see cref="ConsoleKeyInfo"/> object also describes, in a bitwise combination of 
        /// <see cref="ConsoleModifiers"/> values in the <see cref="ConsoleKeyInfo.Modifiers"/> property, whether one or more 
        /// Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</returns>
        ConsoleKeyInfo ReadKey(bool intercept = false);

        /// <summary>Gets a value indicating whether a key press is available in the console input. This check is often 
        /// performed before <see cref="ReadKey(bool)"/> is performed.</summary>        
        bool KeyAvailable { get; }

    }

}