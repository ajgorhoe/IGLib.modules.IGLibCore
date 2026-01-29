
using System;

namespace IGLib.ConsoleUtils
{

    public interface IConsoleScreen
    {

        void Clear();

        void SetCursorPosition(int left, int top);

        int CursorLeft { get; set; }

        int CursorTop { get; set; }

    }

}