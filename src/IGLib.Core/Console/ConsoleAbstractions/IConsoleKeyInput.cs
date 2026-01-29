

using System;

namespace IGLib.ConsoleUtils
{

    public interface IConsoleKeyInput
    {

        public interface IConsoleKeyInput
        {
            ConsoleKeyInfo ReadKey(bool intercept = false);

            bool KeyAvailable { get; }

        }
    }

}