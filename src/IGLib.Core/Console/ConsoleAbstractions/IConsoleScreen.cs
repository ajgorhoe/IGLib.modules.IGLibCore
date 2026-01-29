
using System;

namespace IGLib.ConsoleUtils
{

    public interface IConsoleScreen
    {
        /// <summary>Clears the console buffer and corresponding console window.</summary>
        void Clear();

        /// <summary>Sets the position of the cursor.</summary>
        /// <param name="left">The column position of the cursor. Columns are numbered from left to right starting at 0.</param>
        /// <param name="top">The row position of the cursor. Rows are numbered from top to bottom starting at 0.</param>
        void SetCursorPosition(int left, int top);

        /// <summary>Gets or sets the column position of the cursor within the buffer area.</summary>
        int CursorLeft { get; set; }

        /// <summary>Gets or sets the row position of the cursor within the buffer area.</summary>
        int CursorTop { get; set; }

    }

}