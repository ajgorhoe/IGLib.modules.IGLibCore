

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

using IGLib.ConsoleUtils;

using static IGLib.ConsoleUtils.SystemConsole;
using MyConsole = IGLib.ConsoleUtils.SystemConsole;
using System.ComponentModel;


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

        #region Constants

        public static string[] BooleanTrueStrings { get; internal set; } = ["true", "1", "yes", "y"];

        public static string[] BooleanFalseStrings { get; internal set; } = ["false", "0", "no", "n"];

        public static bool IsBooleanStringsCaseSensitive { get; internal set; } = false;

        public static bool IsBooleanAnyIntegerAccepted { get; internal set; } = true;

        public static bool IsTruePredefinedString(string? inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return false;
            }
            foreach (string str in BooleanTrueStrings)
            {
                if (IsBooleanStringsCaseSensitive)
                {
                    if (inputString == str)
                    { return true; }
                } 
                else
                {
                    if (str?.ToLower() == inputString?.ToLower())
                    { return true; }
                }
            }
            return false;
        }

        public static bool IsFalsePredefinedString(string? inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return false;
            }
            foreach (string str in BooleanFalseStrings)
            {
                if (IsBooleanStringsCaseSensitive)
                {
                    if (inputString == str)
                    { return true; }
                } 
                else
                {
                    if (str?.ToLower() == inputString?.ToLower())
                    { return true; }
                }
            }
            return false;
        }


        #endregion Constants

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


        /// <summary>Calls <see cref="Read(IConsole, ref bool, IFormatProvider?)"/> on <see cref="GlobalConsole"/>.</summary>
        public static bool Read(ref bool value, IFormatProvider? formatProvider = null)
        {
            return Read(GlobalConsole, ref value, formatProvider);
        }

        /// <summary>Reads a boolean value from console input and assigns it to the specified boolean variable.
        /// User can input a non-boolean string to see current content, or insert an empty string to leave the old content,
        /// or input a question mark for help.</summary>
        /// <param name="console">Console object (abatracted) from which the value is read.</param>
        /// <param name="value">Variable to which the read value is assigned.</param>
        /// <param name="formatProvider">Format provider used when parsing the value from the string input via <paramref name="console"/>.
        /// Default is <see cref="Global.DefaultFormatProvider"/>.</param>
        /// <returns>True if a value has been provided explicitly by the user, false if not (and the old value is kept).</returns>
        public static bool Read(IConsole console, ref bool value, IFormatProvider? formatProvider = null)
        {
            if (formatProvider == null)
            {
                formatProvider = IGLib.Global.DefaultFormatProvider;
            }
            bool initialValue = value;
            bool valueProvided = false;
            string? userInput = null;
            int i = 0;
            do
            {
                ++i;
                userInput = Console.ReadLine();
                if (string.IsNullOrEmpty(userInput))
                {
                    // Keep the old value and print it
                    Console.WriteLine("    = " + value.ToString());
                    return false;
                }
                valueProvided = bool.TryParse(userInput, out value);
                if (valueProvided)
                {
                    // A valid value has been provided by user; return
                    return valueProvided;
                }
                value = initialValue; // need to restore because TryParse modifies it
                if (IsTruePredefinedString(userInput))
                {
                    value = true;
                    return true;
                }
                if (IsFalsePredefinedString(userInput))
                {
                    value = false;
                    return true;
                }
                if (IsBooleanAnyIntegerAccepted)
                {
                    long intValue;
                    bool isInteger = long.TryParse(userInput, NumberStyles.Integer, Global.DefaultFormatProvider, out intValue);
                    if (isInteger)
                    {
                        value = (intValue != 0);
                        return true;
                    }
                }

                if (userInput == "?")
                {
                    Console.WriteLine($"\n  Insert a value of type {value.GetType().Name},");
                    Console.WriteLine($"  ? for help,");
                    Console.WriteLine($"  non-boolean string to show current value,");
                    Console.WriteLine($"  <Enter> to keep the current value ({value}),");
                    Console.WriteLine($"  [{string.Join(", ", BooleanFalseStrings)}] for False),");
                    Console.WriteLine($"  [{string.Join(", ", BooleanTrueStrings)}] for True ({(IsBooleanStringsCaseSensitive? "case sensitive" : "not case sensitive")}),");
                    if (IsBooleanAnyIntegerAccepted)
                    {
                        Console.WriteLine($"  0 for False, any other integer value for True.\n");
                    }
                    else
                    {
                        Console.WriteLine($"  arbitrary integer values are not accepted");
                    }
                }
                else
                {
                    Console.WriteLine($"Insert a value of type {value.GetType().Name}, ? for help.");
                }
                Console.WriteLine("  Current value: " + value.ToString());
                Console.Write("  New value:     ");
            } while (!string.IsNullOrEmpty(userInput));
            return valueProvided;
        }








        /// <summary>Calls <see cref="Read(IConsole, ref double, IFormatProvider?)"/> on <see cref="GlobalConsole"/>.</summary>
        public static bool Read(ref double value, IFormatProvider? formatProvider = null)
        {
            return Read(GlobalConsole, ref value, formatProvider);
        }

        /// <summary>Reads a number of type double from console input in a user friendy way  and assigns it to the specified variable.
        /// User can input a non-numerical string to see current value, or input an empty string to leave the old value,
        /// or input a question mark (`?`) do display help.</summary>
        /// <param name="console">Console object (abatracted) from which the value is read.</param>
        /// <param name="value">Variable to which the read value is assigned.</param>
        /// <param name="formatProvider">Format provider used when parsing the value from the string input via <paramref name="console"/>.
        /// Default is <see cref="Global.DefaultFormatProvider"/>.</param>
        /// <returns>True if a value has been provided explicitly by the user, false if not (and the old value is kept).</returns>
        public static bool Read(IConsole console, ref double value, IFormatProvider? formatProvider = null)
        {
            if (formatProvider == null)
            {
                formatProvider = Global.DefaultFormatProvider;
            }
            double initialValue = value;
            bool valueProvided = false;
            string? userInput = null;
            int i = 0;
            do
            {
                ++i;
                userInput = Console.ReadLine();
                if (string.IsNullOrEmpty(userInput))
                {
                    // Keep the old value and print it
                    Console.WriteLine("    = " + value.ToString());
                    return false;
                }
                valueProvided = double.TryParse(userInput, NumberStyles.Integer, Global.DefaultFormatProvider, out value);
                if (valueProvided)
                {
                    // A valid value has been provided by user; return
                    return valueProvided;
                }
                value = initialValue; // need to restore because TryParse modifies the value
                if (userInput == "?")
                {
                    Console.WriteLine($"\n  Insert a number of type {value.GetType().Name},");
                    Console.WriteLine($"  ? for help,");
                    Console.WriteLine($"  non-numeric string to show current value,");
                    Console.WriteLine($"  <Enter> to keep the current value ({value}).\n");
                }
                else
                {
                    Console.WriteLine($"Insert a number of type {value.GetType().Name}, ? for help.");
                }
                Console.WriteLine("  Current value: " + value.ToString());
                Console.Write(    "  New value:     ");
            } while (!string.IsNullOrEmpty(userInput));
            return valueProvided;
        }







        internal static bool TryParse<NumericType>(IConsole console, IFormatProvider formatProvider, out NumericType value)
            where NumericType : struct
        {
            //value = default;
            //return false;
            throw new NotImplementedException("Generic TryParse is not implemented.");
        }

        /// <summary>Calls <see cref="Read(IConsole, ref NumericType, IFormatProvider?)"/> on <see cref="GlobalConsole"/>.</summary>
        internal static bool Read<NumericType>(ref NumericType value, IFormatProvider? formatProvider = null)
            where NumericType : struct
        {
            return Read(GlobalConsole, ref value, formatProvider);
        }

        /// <summary>Reads a number of type double from console input in a user friendy way  and assigns it to the specified variable.
        /// User can input a non-numerical string to see current value, or input an empty string to leave the old value,
        /// or input a question mark (`?`) do display help.</summary>
        /// <param name="console">Console object (abatracted) from which the value is read.</param>
        /// <param name="value">Variable to which the read value is assigned.</param>
        /// <param name="formatProvider">Format provider used when parsing the value from the string input via <paramref name="console"/>.
        /// Default is <see cref="Global.DefaultFormatProvider"/>.</param>
        /// <returns>True if a value has been provided explicitly by the user, false if not (and the old value is kept).</returns>
        public static bool Read<NumericType>(IConsole console, ref NumericType value, IFormatProvider? formatProvider = null)
            where NumericType : struct
        {
            if (formatProvider == null)
            {
                formatProvider = Global.DefaultFormatProvider;
            }
            NumericType initialValue = value;
            bool valueProvided = false;
            string? userInput = null;
            int i = 0;
            do
            {
                ++i;
                userInput = Console.ReadLine();
                if (string.IsNullOrEmpty(userInput))
                {
                    // Keep the old value and print it
                    Console.WriteLine("    = " + value.ToString());
                    return false;
                }
                valueProvided = TryParse<NumericType>(console, Global.DefaultFormatProvider, out value);
                if (valueProvided)
                {
                    // A valid value has been provided by user; return
                    return valueProvided;
                }
                value = initialValue; // need to restore because TryParse modifies the value
                if (userInput == "?")
                {
                    Console.WriteLine($"\n  Insert a number of type {value.GetType().Name},");
                    Console.WriteLine($"  ? for help,");
                    Console.WriteLine($"  non-numeric string to show current value,");
                    Console.WriteLine($"  <Enter> to keep the current value ({value}).\n");
                }
                else
                {
                    Console.WriteLine($"Insert a number of type {value.GetType().Name}, ? for help.");
                }
                Console.WriteLine("  Current value: " + value.ToString());
                Console.Write(    "  New value:     ");
            } while (!string.IsNullOrEmpty(userInput));
            return valueProvided;
        }





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