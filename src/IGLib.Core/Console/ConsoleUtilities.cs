

using IGLib.ConsoleAbstractions.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static IGLib.ConsoleAbstractions.SystemConsole;
using static IGLib.ParsingUtils;
using MyConsole = IGLib.ConsoleAbstractions.SystemConsole;


namespace IGLib.ConsoleAbstractions
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
            where T : IEquatable<T>
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
                    Console.WriteLine($"  [{string.Join(", ", BooleanTrueStrings)}] for True ({(IsBooleanStringsCaseSensitive ? "case sensitive" : "not case sensitive")}),");
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
                valueProvided = double.TryParse(userInput, NumberStyles.Number, Global.DefaultFormatProvider, out value);
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
                Console.Write("  New value:     ");
            } while (!string.IsNullOrEmpty(userInput));
            return valueProvided;
        }



        /// <summary>Calls <see cref="Read(IConsole, ref NumericType, IFormatProvider?)"/> on <see cref="GlobalConsole"/>.</summary>
        public static bool Read<NumericType>(ref NumericType value, IFormatProvider? formatProvider = null)
            where NumericType : struct, IConvertible
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
            where NumericType : struct  // , IConvertible
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
                valueProvided = TryParse<NumericType>(userInput, out value, formatProvider);
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
                Console.Write("  New value:     ");
            } while (!string.IsNullOrEmpty(userInput));
            return valueProvided;
        }



        #endregion ReadValues



        #region PasswordUtilities



        /// <summary>Reads a password inserted by the user via the global console <see cref="GlobalConsole"/> in a secure-ish way.
        /// <para>This method just calls the <see cref="ReadPasswordChars(char)"/> on the <see cref="GlobalConsole"/>.</para></summary>
        public static char[] ReadPasswordChars(char displayChar = '*')
        {
            return ReadPasswordChars(GlobalConsole, displayChar = '*');
        }

        /// <summary>Reads a password inserted by the user via console in a secure-ish way, and returns the password read as a 
        /// character array.
        /// <para>Security:</para>
        /// <para>* Characters are not displayed as user types in the password. Instead, either a substitue character (<paramref name="displayChar"/>)
        /// is displayed for each character, or nothing is displayed (if parameter equals '\0').</para>
        /// <para>  * Password is read character by character usinf <see cref="IConsoleKeyInput.ReadKey(bool)"/> with intercept parameter
        /// set to true.</para>
        /// <para>  * Make sure that the implementation of <see cref="IConsoleWithKeyInput"/> that you are using actually respects not
        /// not echoing characters input by the user.</para>
        /// <para>* Mutual buffer is used internally to store the input password, converted to character array.</para>
        /// <para>* The content of internal buffer that stores inserted password is cleared character by character before return.</para>
        /// <para>* However,  the caller is responsible for also clearing the returned character array after the password has been used, 
        /// e.g. by calling <see cref="Array.Clear(Array)"/>.</para>
        /// <para>IMPORTANT: for security reasons, clear contents of the returned array immediately after it is not needed
        /// any more, using <see cref="Array.Clear(Array)"/>.</para></summary>
        /// <param name="console">Console object (abatracted) from which the password is read.</param>
        /// <param name="displayChar">Character to display for each entered character. Set to '\0' to not display anything.
        /// Defaults to asterisk ('*').</param>
        /// <returns>Charracter array containing the password inserted by the user.
        /// <para>IMPORTANT: after the returned array is used, clear it by calling <see cref="Array.Clear(Array)"/> on it.</para></returns>
        public static char[] ReadPasswordChars(IConsoleWithKeyInput console, char displayChar = '*')
        {
            var buffer = new List<char>(40);

            try
            {
                while (true)
                {
                    var key = console.ReadKey(intercept: true);

                    switch (key.Key)
                    {
                        case ConsoleKey.Enter:
                            console.WriteLine();
                            return FinalizePassword(buffer);

                        case ConsoleKey.Backspace:
                            if (buffer.Count > 0)
                            {
                                buffer[buffer.Count - 1] = '\0';  // overwrite the last character for security
                                buffer.RemoveAt(buffer.Count - 1);
                                if (displayChar != '\0')
                                    console.Write("\b \b");
                            }
                            break;

                        case ConsoleKey.Escape:
                            // Escape removes the current input, but it does not cancel the whole process;
                            // User can start over, or press <Enter> to leave with an empty password.
                            int bufferCount = buffer.Count;
                            for (int i = 0; i < bufferCount; ++i)
                            {
                                buffer[buffer.Count - 1] = '\0';  // overwrite the last character for security
                                buffer.RemoveAt(buffer.Count - 1);
                                if (displayChar != '\0')
                                    console.Write("\b \b");
                            }
                            break;

                        // ignore navigation and modifier keys
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.Home:
                        case ConsoleKey.End:
                        case ConsoleKey.PageUp:
                        case ConsoleKey.PageDown:
                        case ConsoleKey.Insert:
                        case ConsoleKey.Delete:
                        case ConsoleKey.Tab:
                            break;

                        default:
                            char c = key.KeyChar;

                            if (!char.IsControl(c))
                            {
                                buffer.Add(c);

                                if (displayChar != '\0')
                                    console.Write(displayChar);
                            }
                            break;
                    }
                }
            }
            finally
            {
                //if (cancelHandler != null)
                //    Console.CancelKeyPress -= cancelHandler;
            }
        }

        /// <summary>Reads a password inserted by the user via the <see cref="System.Console"/> in a secure-ish way.
        /// <para>This method is similar to <see cref="ReadPasswordChars(char)"/>, except that it implments some additional
        /// behavior that can be implemented on the classic console, the <see cref="System.Console"/>.</para>
        /// <para>Specifics adaptes for the <see cref="System.Console"/> include registration and deregistration of the
        /// event handler for cancellation.</para></summary>
        public static char[] ReadPasswordCharsBySystemConsole(char displayChar = '*')
        {
            var buffer = new List<char>(40);

            ConsoleCancelEventHandler? cancelHandler = null;

            try
            {
                // Handle Ctrl+C safely
                cancelHandler = (sender, e) =>
                {
                    e.Cancel = true;                 // prevent process termination
                    ClearBuffer(buffer);
                    Console.WriteLine();
                    throw new OperationCanceledException("Password entry cancelled.");
                };

                Console.CancelKeyPress += cancelHandler;

                while (true)
                {
                    var key = Console.ReadKey(intercept: true);

                    switch (key.Key)
                    {
                        case ConsoleKey.Enter:
                            Console.WriteLine();
                            return FinalizePassword(buffer);

                        case ConsoleKey.Backspace:
                            if (buffer.Count > 0)
                            {
                                buffer[buffer.Count - 1] = '\0';
                                buffer.RemoveAt(buffer.Count - 1);

                                if (displayChar != '\0')
                                    Console.Write("\b \b");
                            }
                            break;

                        case ConsoleKey.Escape:
                            ClearBuffer(buffer);
                            Console.WriteLine();
                            // Alternative might be this: throw new OperationCanceledException("Password entry cancelled.");
                            Console.WriteLine(" << Cancelled. >>");
                            return FinalizePassword(buffer);

                        // ignore navigation and modifier keys
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.Home:
                        case ConsoleKey.End:
                        case ConsoleKey.PageUp:
                        case ConsoleKey.PageDown:
                        case ConsoleKey.Insert:
                        case ConsoleKey.Delete:
                        case ConsoleKey.Tab:
                            break;

                        default:
                            char c = key.KeyChar;

                            if (!char.IsControl(c))
                            {
                                buffer.Add(c);

                                if (displayChar != '\0')
                                    Console.Write(displayChar);
                            }
                            break;
                    }
                }
            }
            finally
            {
                if (cancelHandler != null)
                    Console.CancelKeyPress -= cancelHandler;
            }
        }

        private static char[] FinalizePassword(List<char> buffer)
        {
            char[] result = buffer.ToArray();
            ClearBuffer(buffer);
            return result;
        }

        private static void ClearBuffer(List<char> buffer)
        {
            for (int i = 0; i < buffer.Count; i++)
                buffer[i] = '\0';
            buffer.Clear();
        }



        #region OlderUtilities


        public static List<char> ReadPasswordOld(char displayChar = '*', bool repeatForValidation = true,
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


        public static StringBuilder ReadPasswordStringBuilderOld(char displayChar = '*', bool repeatForValidation = true,
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


        [Obsolete("Do not use this method as storing passwords in strings is not secure.")]
        public static string ReadPasswordToStringOld(char displayChar = '*', bool repeatForValidation = false,
            string insertionPrompt = "", string validationPrompt = "", string validationNotEqualPrompt = "")
        {
            List<char> pwd = new List<char>();

            pwd = ReadPasswordOld(displayChar, repeatForValidation, insertionPrompt, validationPrompt,
                validationNotEqualPrompt);

            string ret = "";
            if (pwd != null)
            {
                ret = new string([.. pwd]);
                OverWrite(pwd);
            }
            return ret;
        }


        #endregion OlderUtilities



        #endregion PasswordUtilities


    }

}