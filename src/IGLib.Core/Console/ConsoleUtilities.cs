

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

using IGLib.ConsoleUtils;

using static IGLib.ConsoleUtils.SystemConsole;
using MyConsole = IGLib.ConsoleUtils.SystemConsole;


namespace IGLib.ConsoleUtils
{

    /// <summary>Various utilities that work with console abstractions (interfaces like <see cref="IConsole"/>,
    /// <see cref="IConsoleKeyInput"/>, <see cref="IConsoleStreams"/>, <see cref="IConsoleStreams"/>, etc.).</summary>
    public static class ConsoleUtilities
    {

        public static void Sandbox()
        {
            GlobalConsole.WriteLine("AAA");  // because of: using static IGLib.ConsoleUtils.SystemConsole
            MyConsole.GlobalConsole.WriteLine();  // because of: using MyConsole = IGLib.ConsoleUtils.SystemConsole

        }


        #region Helpers

        public static void OverWrite<T>(IList<T>? list, T newElementValue = default!)
        {
            if (list != null)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    list[i] = newElementValue!;
                }
            }
        }

        public static bool AreEqual<T>(IList<T> l1, IList<T> l2)
            where T: IEquatable<T>
        {
            if (l1 == null || l2 == null)
            {
                return l1 == l2;
            }
            if (l1.Count != l2.Count)
            {
                return false;
            }
            for (int i = 0; i < l1.Count; ++i)
            {
                if (!(l1[i].Equals(l2[i])))
                { return false; }
            }
            return true;
        }

        #endregion Helpers


        #region ReadValues


        /// <summary>Reads an integer from a console and assigns it to a variable.
        /// User can input a non-integer to see current content, or insert an empty string to leave the old content.</summary>
        /// <param name="value">Variable to which the inserted value is assigned.</param>
        /// <returns>true if a new value has been assigned, false otherwise.</returns>
        public static bool Read(ref int value)
        {
            bool ret = false;
            string? str = null;
            int i = 0;
            do
            {
                ++i;
                str = Console.ReadLine();
                if (string.IsNullOrEmpty(str))
                {
                    // Keep the old value and print it
                    Console.WriteLine("  = " + value.ToString());
                }
                else
                {
                    try
                    {
                        value = int.Parse(str);
                        ret = true;  // value has been changed
                        str = ""; // continue if successfully parsed
                    }
                    catch
                    {
                        if (str == "?")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Insert an integer,");
                            Console.WriteLine("  \"?\" for help,");
                            Console.WriteLine("  non-numeric string to show current value,");
                            Console.WriteLine("  <Enter> to keep the old value.");
                            Console.WriteLine();
                        }
                        // Inserted string is not a valid representation of the output type,
                        // print the old value and request a new one:
                        if (i > 1)
                            Console.WriteLine("Insert an integer, \"?\" for help.");
                        Console.WriteLine("  Current value: " + value.ToString());
                        Console.Write("  New value:     ");
                    }
                }
            } while (!string.IsNullOrEmpty(str));
            return ret;
        }  // Read (ref int)

        /// <summary>Reads a boolean from a console and assigns it to a variable.
        /// User can input a non-boolean to see current content, or insert an empty string to leave the old content.
        /// Eligible input to assign a new boolean value (strings are not case sensitive!):
        /// </summary>
        /// <param name="value">Variable to which the inserted value is assigned.</param>
        /// <returns>true if a new value has been assigned, false otherwise.</returns>
        public static bool Read(ref bool value)
        {
            bool wasAssigned = false;
            string? str = null;
            int i = 0;
            do
            {
                ++i;
                str = Console.ReadLine();
                if (string.IsNullOrEmpty(str))
                {
                    // Keep the old value and print it
                    Console.WriteLine("  = " + value.ToString());
                    return false;
                }
                else
                {
                    wasAssigned = bool.TryParse(str, out value);
                    if (wasAssigned)
                    {
                        return wasAssigned;
                    }
                    switch (str)
                    {
                        case "0":
                            value = false;
                            return true;
                        case "1":
                            value = true;
                            return true;
                        case "false":
                            value = false;
                            return true;
                        case "true":
                            value = true;
                            return true;
                        case "no":
                            value = false;
                            return true;
                        case "yes":
                            value = true;
                            return true;

                        case "n":
                            value = false;
                            return true;
                        case "y":
                            value = true;
                            return true;
                    }
                    if (str == "?")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Insert a boolean value,");
                        Console.WriteLine("  \"?\" for help,");
                        Console.WriteLine("  non-boolean string to show current value,");
                        Console.WriteLine("  <Enter> to keep the old value.");
                        Console.WriteLine("  Legal input: '0', '1', 'false', 'true', 'y', 'n', 'yes', 'no' (case insensitive).");
                        Console.WriteLine();
                    }
                    // Inserted string is not a valid representation of the output type,
                    // print the old value and request a new one:
                    if (i > 1)
                        Console.WriteLine("Insert a boolean, \"?\" for help.");
                    Console.WriteLine("  Current value: " + value.ToString());
                    Console.Write("  New value:     ");
                }
            } while (!wasAssigned);
            return wasAssigned;
        }  // Read (ref bool)

        /// <summary>Reads an integer (of type long) from a console and assigns it to a variable.
        /// User can input a non-integer to see current content, or insert an empty string to leave the old content.</summary>
        /// <param name="value">Variable to which the inserted value is assigned.</param>
        /// <returns>true if a new value has been assigned, false otherwise.</returns>
        public static bool Read(ref long value)
        {
            bool ret = false;
            string str = null;
            int i = 0;
            do
            {
                ++i;
                str = Console.ReadLine();
                if (string.IsNullOrEmpty(str))
                {
                    // Keep the old value and print it
                    Console.WriteLine("  = " + value.ToString());
                }
                else
                {
                    try
                    {
                        value = long.Parse(str);
                        ret = true;  // value has been changed
                        str = ""; // continue if successfully parsed
                    }
                    catch
                    {
                        if (str == "?")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Insert an integer (long),");
                            Console.WriteLine("  \"?\" for help,");
                            Console.WriteLine("  non-numeric string to show current value,");
                            Console.WriteLine("  <Enter> to keep the old value.");
                            Console.WriteLine();
                        }
                        // Inserted string is not a valid representation of the output type,
                        // print the old value and request a new one:
                        if (i > 1)
                            Console.WriteLine("Insert an integer (long), \"?\" for help.");
                        Console.WriteLine("  Current value: " + value.ToString());
                        Console.Write("  New value:     ");
                    }
                }
            } while (!string.IsNullOrEmpty(str));
            return ret;
        }  // Read (ref long)

        /// <summary>Reads a number of type double from a console and assigns it to a variable.
        /// User can input a non-integer to see current content, or insert an empty string to leave the old content.</summary>
        /// <param name="value">Variable to which the inserted value is assigned.</param>
        /// <returns>true if a new value has been assigned, false otherwise.</returns>
        public static bool ReadOld(ref double value)
        {
            bool ret = false;
            string? str = null;
            int i = 0;
            do
            {
                ++i;
                str = Console.ReadLine();
                if (string.IsNullOrEmpty(str))
                {
                    // Keep the old value and print it
                    Console.WriteLine("  = " + value.ToString());
                }
                else
                {
                    try
                    {
                        value = double.Parse(str);
                        ret = true;  // value has been changed
                        str = ""; // continue if successfully parsed
                    }
                    catch
                    {
                        if (str == "?")
                        {
                            Console.WriteLine();
                            Console.WriteLine("Insert a number (double precision),");
                            Console.WriteLine("  \"?\" for help,");
                            Console.WriteLine("  non-numeric string to show current value,");
                            Console.WriteLine("  <Enter> to keep the old value.");
                            Console.WriteLine();
                        }
                        // Inserted string is not a valid representation of the output type,
                        // print the old value and request a new one:
                        if (i > 1)
                            Console.WriteLine("Insert a number (double precision), \"?\" for help.");
                        Console.WriteLine("  Current value: " + value.ToString());
                        Console.Write("  New value:     ");
                    }
                }
            } while (!string.IsNullOrEmpty(str));
            return ret;
        }  // Read (ref double)



        /// <summary>Reads a number of type double from a console and assigns it to a variable.
        /// User can input a non-integer to see current content, or insert an empty string to leave the old content.</summary>
        /// <param name="value">Variable to which the inserted value is assigned.</param>
        /// <returns>True if a value has been provided by user, false if the old value is kept.</returns>
        public static bool Read(ref double value)
        {
            bool ret = false;
            string? str = null;
            int i = 0;
            do
            {
                ++i;
                str = Console.ReadLine();
                if (string.IsNullOrEmpty(str))
                {
                    // Keep the old value and print it
                    Console.WriteLine("  = " + value.ToString());
                    return false;
                }
                ret = double.TryParse(str, out value);
                if (ret)
                {
                    // A valid value has been provided by user; return
                    return ret;
                }
                if (str == "?")
                {
                    Console.WriteLine();
                    Console.WriteLine($"\nInsert a number of type {value.GetType().Name},");
                    Console.WriteLine("  ? for help,");
                    Console.WriteLine("  non-numeric string to show current value,");
                    Console.WriteLine("  <Enter> to keep the old value.");
                    Console.WriteLine();
                }
                if (i > 1 && str != "?")
                    Console.WriteLine($"Insert a number of type {value.GetType().Name}, ? for help.");
                Console.WriteLine("  Current value: " + value.ToString());
                Console.Write(    "  New value:     ");

                //{
                //    try
                //    {
                //        value = double.Parse(str);
                //        ret = true;  // value has been changed
                //        str = ""; // continue if successfully parsed
                //    }
                //    catch
                //    {
                //        if (str == "?")
                //        {
                //            Console.WriteLine();
                //            Console.WriteLine("Insert a number (double precision),");
                //            Console.WriteLine("  \"?\" for help,");
                //            Console.WriteLine("  non-numeric string to show current value,");
                //            Console.WriteLine("  <Enter> to keep the old value.");
                //            Console.WriteLine();
                //        }
                //        // Inserted string is not a valid representation of the output type,
                //        // print the old value and request a new one:
                //        if (i > 1)
                //            Console.WriteLine("Insert a number (double precision), \"?\" for help.");
                //        Console.WriteLine("  Current value: " + value.ToString());
                //        Console.Write("  New value:     ");
                //    }
                //}

            } while (!string.IsNullOrEmpty(str));
            return ret;
        }  // Read(ref double)





        #endregion ReadValues




        #region PasswordUtilities


        [Obsolete("Do not use this in production as storing passwords in strings is not secure.")]
        public static string ReadPasswordToString(char displayChar = '*', bool repeatForValidation = false,
            string insertionPrompt = "", string validationPrompt = "", string validationNotEqualPrompt = "")
        {
            List<char> pwd = new List<char>();

            pwd = ReadPassword(displayChar, repeatForValidation, insertionPrompt, validationPrompt,
                validationNotEqualPrompt);

            string ret = "";
            if (pwd != null)
            { 
                ret = new string([.. pwd]);
                OverWrite(pwd);
            }
            return ret;
        }

        public static List<char> ReadPassword(char displayChar = '*', bool repeatForValidation = true,
            string insertionPrompt = "", string validationPrompt = "", string validationNotEqualPrompt = "")
        {
            const string defaultInsertionPrompt = "Insert the password: ";
            const string defaultValidationPrompt = "Insert the password again: ";
            const string defaultValidationNotEqualPrompt = "\nInserted passwords are not equal. Please insert the passwords again.";
            if (string.IsNullOrEmpty(insertionPrompt))
            {
                insertionPrompt = defaultInsertionPrompt;
            }
            if (string.IsNullOrEmpty(validationPrompt))
            {
                validationPrompt = defaultValidationPrompt;
            }
            var password = new List<char>();
            ConsoleKeyInfo key;
            Console.Write(insertionPrompt);
            do
            {
                key = Console.ReadKey(true);

                // Handle Backspace
                if (key.Key == ConsoleKey.Backspace && password.Count > 0)
                {
                    password.RemoveAt(password.Count - 1);
                    Console.Write("\b \b"); // move back, overwrite with space, move back again
                }
                else if (!char.IsControl(key.KeyChar)) // ignore other control keys (like Tab or Escape)
                {
                    password.Add(key.KeyChar);
                    Console.Write(displayChar);
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine(); // move to the next line after Enter
            if (repeatForValidation)
            {
                var validationPassword = new List<char>();
                while (!AreEqual(password, validationPassword))
                {
                    validationPassword.Clear();
                    Console.Write(validationPrompt);
                    do
                    {
                        key = Console.ReadKey(true);

                        // Handle Backspace
                        if (key.Key == ConsoleKey.Backspace && validationPassword.Count > 0)
                        {
                            validationPassword.RemoveAt(validationPassword.Count - 1);
                            Console.Write("\b \b"); // move back, overwrite with space, move back again
                        }
                        else if (!char.IsControl(key.KeyChar)) // ignore other control keys (like Tab or Escape)
                        {
                            validationPassword.Add(key.KeyChar);
                            Console.Write(displayChar);
                        }
                    } while (key.Key != ConsoleKey.Enter);
                    Console.WriteLine(); // move to the next line after Enter
                }
                OverWrite(validationPassword, '\0');  // overwrite contents for security reasons
            }


            return password;
        }




        public static StringBuilder ReadPasswordStringBuilder(char displayChar = '*', bool repeatForValidation = true, 
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