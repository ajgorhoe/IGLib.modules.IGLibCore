

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;



namespace IGLib.ConsoleUtils
{

    /// <summary>Various utilities that work with console abstractions (interfaces like <see cref="IConsole"/>,
    /// <see cref="IConsoleKeyInput"/>, <see cref="IConsoleStreams"/>, <see cref="IConsoleStreams"/>, etc.).</summary>
    public static class ConsoleUtilities
    {


        #region PasswordUtilities


        public static string ReadPassword(char displayChar = '*', bool repeatforValidation = true, 
            string insertionPrompt = "Insert the password: ", 
            string validationPrompt = "Insert the password again: ")
        {
            var password = new StringBuilder();
            ConsoleKeyInfo key;

            if (!string.IsNullOrEmpty(insertionPrompt))
            {
                Console.Write(insertionPrompt);
            }
            do
            {
                key = Console.ReadKey(true);

                // Handle Backspace
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b"); // Move back, overwrite with space, move back again
                }
                // Ignore other control keys (like Tab or Escape)
                else if (!char.IsControl(key.KeyChar))
                {
                    password.Append(key.KeyChar);
                    Console.Write(displayChar);
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine(); // Move to the next line after Enter
            return password.ToString();
        }



        #endregion PasswordUtilities


    }

}