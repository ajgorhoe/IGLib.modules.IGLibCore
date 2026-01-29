

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


        public static StringBuilder ReadPassword(char displayChar = '*', bool repeatForValidation = true, 
            string insertionPrompt = "Insert the password: ", 
            string validationPrompt = "Insert the password again: ")
        {
            var password = new StringBuilder();
            ConsoleKeyInfo key;

            if (string.IsNullOrEmpty(insertionPrompt))
            {
                insertionPrompt = "Insert the password: ";
            }
            if (string.IsNullOrEmpty(validationPrompt))
            {
                validationPrompt = "Insert the password again: ";
            }
            Console.Write(insertionPrompt);
            do
            {
                key = Console.ReadKey(true);

                // Handle Backspace
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b"); // move back, overwrite with space, move back again
                }
                else if (!char.IsControl(key.KeyChar)) // ignore other control keys (like Tab or Escape)
                {
                    password.Append(key.KeyChar);
                    Console.Write(displayChar);
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine(); // move to the next line after Enter
            if (repeatForValidation)
            {
                var validationPassword = new StringBuilder();
                while (validationPassword != password)
                {
                    validationPassword.Clear();
                    Console.Write(validationPrompt);
                    do
                    {
                        key = Console.ReadKey(true);

                        // Handle Backspace
                        if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                        {
                            password.Remove(password.Length - 1, 1);
                            Console.Write("\b \b"); // move back, overwrite with space, move back again
                        }
                        else if (!char.IsControl(key.KeyChar)) // ignore other control keys (like Tab or Escape)
                        {
                            password.Append(key.KeyChar);
                            Console.Write(displayChar);
                        }
                    } while (key.Key != ConsoleKey.Enter);
                    Console.WriteLine(); // move to the next line after Enter

                }
            }

            return password;
        }



        #endregion PasswordUtilities


    }

}