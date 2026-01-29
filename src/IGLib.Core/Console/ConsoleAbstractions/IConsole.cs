
using System;

namespace IGLib.ConsoleUtils
{

    public interface IConsole
    {

        /// <summary>Reads / receives and returns one line of text input (from a user or an orchestration tool).</summary>
        /// <returns>The string (a line of text, without the newline) that was provided as line of input.</returns>
        string? ReadLine();

        /// <summary>Writes / outputs a string to console's output.</summary>
        /// <param name="value"></param>
        void Write(string? value);

        /// <summary>Writes / outputs a line of text, appending the newline mark at the end.</summary>
        /// <param name="value">String to be written / output (a newline is appended internally).</param>
        void WriteLine(string? value = null);

        //// Common formatting overloads (optional, but Console-like)
        //void WriteLine(string format, params object?[] args);
        //void Write(string format, params object?[] args);
    }

}