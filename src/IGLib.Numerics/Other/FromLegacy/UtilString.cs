
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using IG.Num;

namespace IG.Lib
{

    /// <summary>Represents a character type, supports bitwise flag combination.</summary>
    [Flags]
    public enum CharType
    { None = 0, CapitalLetter = 1, SmallLetter = 2, Numeric = 4,
        All = CapitalLetter | SmallLetter | Numeric };

    /// <summary>Various string operations, random strings, random characters, etc.</summary>
    /// $A Igor xx;
    public class UtilStr: UtilCsv
    {
        

        private static int _maxStringLength = 100;

        /// <summary>Get or sets maximal length for generated strings (must be GREATER THAN 0).</summary>
        public static int MaxStringLength
        {
            get { return _maxStringLength; }
            set 
            { 
                if (value>0)
                    _maxStringLength = value;
                else throw new ArgumentException("Maximal string length should be greater than 0!");
            }
        }

        /// <summary>Returns true if the specified arrays are equal (i.e. all corresponding values are equal or 
        /// both arrays are null), false if not.</summary>
        /// <param name="originals">First array of strings to be compared.</param>
        /// <param name="compared">Second array of strings to be compared.</param>
        public static bool ArraysEqual<ElementType>(ElementType[] originals, ElementType[] compared)
            where ElementType: IComparable<ElementType>
        {
            if (originals == null)
            {
                if (compared == null)
                    return true;
            }
            else if (compared == null)
                return false;
            else
            {
                if (originals.Length != compared.Length)
                    return false;
                for (int i = 0; i < originals.Length; ++i)
                {
                    if (!EqualityComparer<ElementType>.Default.Equals(originals[i], compared[i]))
                        return false;
                }
            }
            return true;
        }


        /// <summary>Returns true if the specified string arrays are equal (i.e. all corresponding values are equal or 
        /// both arrays are null), false if not.</summary>
        /// <param name="originals">First array of strings to be compared.</param>
        /// <param name="compared">Second array of strings to be compared.</param>
        public static bool StringArraysEqual(string[] originals, string[] compared)
        {
            if (originals == null)
            {
                if (compared == null)
                    return true;
            }
            else if (compared == null)
                return false;
            else
            {
                if (originals.Length != compared.Length)
                    return false;
                for (int i = 0; i < originals.Length; ++i)
                {
                    if (originals[i] != compared[i])
                        return false;
                }
            }
            return true;
        }


        #region ParameterLists


        /// <summary>Returns a string array of parameters represented by the specified comma separated list in string form.</summary>
        /// <param name="listString">List of parameters in string form, comma separated and with possible whitespaces between them.</param>
        /// <returns></returns>
        public static string[] GetParametersArrayPlain(string listString)
        {
            if (listString == null)
                return null;
            else
            {
                string [] parNames = listString.Split(',');
                for (int i=0; i<parNames.Length; ++i)
                {
                    parNames[i] = parNames[i].Trim();
                }
                return parNames;
            }
        }

        /// <summary>Returns a string representation of the list of parameters, 
        /// separated by commas but not embedded in any braces.</summary>
        /// <param name="parameters">Array of parameters from which a comma separated list string is formed.</param>
        public static string GetParametersStringPlain(string[] parameters)
        {
            StringBuilder sb = new StringBuilder();
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; ++i)
                {
                    sb.Append(parameters[i]);
                    if (i < parameters.Length - 1)
                        sb.Append(", ");
                }
            }
            return sb.ToString();
        }


        /// <summary>Returns a string array of parameters represented by the specified comma separated list in string form.</summary>
        /// <param name="linesString">List of parameters in string form, comma separated and with possible whitespaces between them.</param>
        /// <param name="doTrim">Specifies whether parameter names should be trimmed.</param>
        public static string[] GetLines(string linesString, bool doTrim = true)
        {
            if (linesString == null)
                return null;
            else
            {
                string[] parNames = linesString.Split('\n');
                if (doTrim)
                {
                    for (int i = 0; i < parNames.Length; ++i)
                    {
                        parNames[i] = parNames[i].Trim();
                    }
                }
                return parNames;
            }
        }

        /// <summary>Returns a string representation of the list of lines, 
        /// separated by newlines.</summary>
        /// <param name="lines">Array of parameters from which a comma separated list string is formed.</param>
        public static string GetLinesStringPlain(string[] lines)
        {
            StringBuilder sb = new StringBuilder();
            if (lines != null)
            {
                for (int i = 0; i < lines.Length; ++i)
                {
                    sb.Append(lines[i]);
                    if (i < lines.Length - 1)
                        sb.Append(Environment.NewLine);
                }
            }
            return sb.ToString();
        }



        #endregion ParameterLists

        #region CommandLine


        /// <summary>Assembles and returns the commandline string that corresponds to the specified command 
        /// name and arguments.</summary>
        /// <param name="CommandName">Command name.</param>
        /// <param name="CommandArguments">Array of command arguments.</param>
        public static string GetCommandLine(string CommandName, string[] CommandArguments)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CommandName);
            if (CommandArguments != null)
            {
                for (int i = 0; i < CommandArguments.Length; ++i)
                {
                    sb.Append(" " + CommandArguments[i]);
                }
            }
            return sb.ToString();
        }

        /// <summary>Assembles and returns the commandline string that corresponds to the specified command 
        /// name and arguments.</summary>
        /// <param name="commandNameAndArguments">Array containing command names (0-th element) and its arguments.</param>
        public static string GetCommandLine(string[] commandNameAndArguments)
        {
            if (commandNameAndArguments == null)
                throw new ArgumentException("Command name and arguments not specified (null string array).");
            if (commandNameAndArguments.Length < 1)
                throw new ArgumentException("Command name not specified (string array has no elements).");
            else if (string.IsNullOrEmpty(commandNameAndArguments[0]))
                throw new ArgumentException("Command name not specified (null or empty string).");
            StringBuilder sb = new StringBuilder();
            sb.Append(commandNameAndArguments[0]);
            for (int i = 1; i < commandNameAndArguments.Length; ++i)
                sb.Append(" " + commandNameAndArguments[i]);
            return sb.ToString();
        }

        /// <summary>Parses a command line and extracts arguments from it.
        /// Arguments can be separated according to usual rules for command-line arguments:
        /// spaces are separators, there can be arbitraty number of spaces, and if we want an
        /// argument to contain spaces, we must enclose it in double quotes.
        /// Command line can also contain the command name followed by arguments. In this case it is treated in the same way, and
        /// command can be obtained simply as the first string in the returned array.</summary>
        /// <param name="commandLine">Command line that is split to individual arguments.
        /// Command line can also contain a command, which is treated equally.</param>
        /// <returns>An array of arguments.</returns>
        public static string[] GetArgumentsArray(string commandLine)
        {
            List<string> _aux = null;
            List<string> _args = null;
            //lock (Util.LockGlobal)
            //{
            GetArguments(commandLine, ref _args, ref _aux);
            return _args.ToArray();
            //}
        }

        /// <summary>Parses a command line and extracts arguments from it.
        /// Arguments can be separated according to usual rules for command-line arguments:
        /// spaces are separators, there can be arbitraty number of spaces, and if we want an
        /// argument to contain spaces, we must enclose it in double quotes.
        /// Command line can also contain the command name followed by arguments. In this case it is treated in the same way, and
        /// command can be obtained simply as the first string in the returned array.</summary>
        /// <param name="commandLine">Command line that is split to individual arguments.
        /// Command line can also contain a command, which is treated equally.</param>
        /// <param name="ret">List in which the parsed arguments are stored.</param>
        public static void GetArguments(string commandLine, ref List<string> ret)
        {
            List<string> aux = null;
            GetArguments(commandLine, ref ret, ref aux);
        }

        /// <summary>Parses a command line and extracts arguments from it.
        /// Arguments can be separated according to usual rules for command-line arguments:
        /// spaces are separators, there can be arbitraty number of spaces, and if we want an
        /// argument to contain spaces, we must enclose it in double quotes.
        /// Command line can also contain the command name followed by arguments. In this case it is treated in the same way, and
        /// command can be obtained simply as the first string in the returned array.</summary>
        /// <param name="commandLine">Command line that is split to individual arguments.
        /// Command line can also contain a command, which is treated equally.</param>
        /// <param name="ret">List in which the parsed arguments are stored.</param>
        /// <param name="aux">Auxiliary list for storing intermediate results.</param>
        private static void GetArguments(string commandLine, ref List<string> ret, ref List<string> aux)
        {
            if (ret == null)
                ret = new List<string>();
            else
                ret.Clear();
            if (string.IsNullOrEmpty(commandLine))
            {
                return;
            }
            if (aux == null)
                aux = new List<string>();
            else
                aux.Clear();
            char[] separatorsQuote = new char[] { '\"' };
            while (!string.IsNullOrEmpty(commandLine))
            {
                commandLine = commandLine.Trim();
                string[] tab;
                if (commandLine[0] == '\"')
                {
                    commandLine = commandLine.Substring(1);
                    tab = commandLine.Split(separatorsQuote, 2);
                }
                else
                {
                    tab = commandLine.Split(null, 2);
                }
                commandLine = null;
                if (tab != null)
                    if (tab.Length > 0)
                    {
                        ret.Add(tab[0]);
                        if (tab.Length > 1)
                            commandLine = tab[1];
                    }
            }
        }


        #endregion CommmandLine


        #region RandomString
        
        /// <summary>Creates and returns a valid random unicode string. All possible uncode characters may be 
        /// contained in the generated string, EXCEPT the surogate characters in the range  0xD800-0xDFFF.</summary>
        /// <remarks><para>Unicode surrogate characters in the range 0xD800-0xDFFF (55296-57343) are not valid 
        /// on their own. They must appear as a pair (0xD800-0xDBFF first, 0xDC00-0xDFFF second) in order to be 
        /// valid (in the UTF-16 encoding scheme). Alone, they will be treated as invalid characters and decoded 
        /// to 0xFFFD (65533). C# uses UTF-16 to represent its strings.</para>
        /// <para>See also: http://stackoverflow.com/questions/12127843/generating-a-random-string </para></remarks>
        /// <param name="stringLength">Length of the returned string.</param>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        public static string RandomUnicodeString(int stringLength, IRandomGenerator rndgen = null)
        {
            return RandomUnicodeString(stringLength, true /* excludeSurrogate */, rndgen);
        }

        /// <summary>Creates and returns a valid random unicode string. All legal uncode characters may be 
        /// contained in the generated string.
        /// <para>If surrogates are not excluded then it is not guaranteed that the returned string is og prescribed length.</para></summary>
        /// <remarks><para>Unicode surrogate characters in the range 0xD800-0xDFFF (55296-57343) are not valid 
        /// on their own. They must appear as a pair (0xD800-0xDBFF first, 0xDC00-0xDFFF second) in order to be 
        /// valid (in the UTF-16 encoding scheme). Alone, they will be treated as invalid characters and decoded 
        /// to 0xFFFD (65533). C# uses UTF-16 to represent its strings.</para>
        /// <para>See also: http://stackoverflow.com/questions/12127843/generating-a-random-string </para></remarks>
        /// <param name="stringLength">Length of the returned string.</param>
        /// <param name="excludeSurrogates">If true then surrogate characters will not be included in the generated string.</param>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        public static string RandomUnicodeString(int stringLength, bool excludeSurrogates = true, IRandomGenerator rndgen = null)
        {
            if (rndgen == null)
                rndgen = RandomGenerator.Global;
            StringBuilder plainText = new StringBuilder();
            for (int j = 0; j < stringLength; ++j)
            {
                char next = (char) rndgen.Next(char.MinValue, char.MaxValue + 1);
                if (next >= 0xD800 && next <= 0xDFFF)
                {
                    // Surrogate character generated:
                    if (excludeSurrogates)
                    {
                        // We don't include surrogate characters. Keep generating until non-surrogate is obtained.
                        while(next >= 0xD800 && next <= 0xDFFF)
                            next = (char) rndgen.Next(char.MinValue, char.MaxValue + 1);
                        plainText.Append(next);
                    } else
                    {
                        // We include surrogate characters. These are only valid in proper sequences of two characters:
                        next = (char)rndgen.Next(0xD800, 0xDBFF + 1);
                        plainText.Append(next);
                        next = (char)rndgen.Next(0xDC00, 0xDFFF + 1);
                        plainText.Append(next);
                    }
                } else
                    plainText.Append(next);
            }
            byte[] x = Encoding.Unicode.GetBytes(plainText.ToString());
            string result = Encoding.Unicode.GetString(x);
            return result;
        }


        /// <summary>Returns a random character that is a capital letter (A-Z)</summary>
        /// <param name="rndgen">Randomg generator used. If null or unspecified then global random generator is used.</param>
        public static char RandomCharCapitalLetter(IRandomGenerator rndgen = null)
        {
            if (rndgen == null)
                rndgen = RandomGenerator.Global;
            return Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rndgen.NextDouble() + (int)'A')));
        }

        /// <summary>Returns a random character that is a small letter (a-z)</summary>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        public static char RandomCharSmallLetter(IRandomGenerator rndgen = null)
        {
            if (rndgen == null)
                rndgen = RandomGenerator.Global;
            return Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rndgen.NextDouble() + (int)'a')));
        }

        /// <summary>Returns a random numeric character (0-9)</summary>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        public static char RandomCharNumeric(IRandomGenerator rndgen = null)
        {
            if (rndgen == null)
                rndgen = RandomGenerator.Global;
            return Convert.ToChar(Convert.ToInt32(Math.Floor(10 * rndgen.NextDouble() + (int)'0')));
        }


        /// <summary>Returns a random character whose type is specified by type flags.
        /// Type flags can be combined by bitwise operations.</summary>
        /// <param name="typeflags">Flags specifying the permitted types of the returned character.</param>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        /// <returns>The randomly generated character.</returns>
        public static char RandomChar(CharType typeflags, IRandomGenerator rndgen = null)
        {
            char ret = ' ';
            if (rndgen == null)
                rndgen = RandomGenerator.Global;
            // typeflags defining whether certain types of characters are included:
            bool
                small = (typeflags & CharType.SmallLetter) != 0,
                capital = (typeflags & CharType.CapitalLetter) != 0,
                numeric = (typeflags & CharType.Numeric) != 0;
            // Interval limits and weights for drawig lots:
            double
                smallint = 0.0, smallwt = 3.0,
                capitalint = 0.0, capitalwt = 3.0,
                numericint = 0.0, numericwt = 1.0,
                max = 0.0;
            // Define intervals for drawing lots:
            if (small)
            {
                smallint = max + smallwt;
                max = smallint;
            }
            if (capital)
            {
                capitalint = max + capitalwt;
                max = capitalint;
            }
            if (numeric)
            {
                numericint = max + numericwt;
                max = numericint;
            }
            if (max == 0.0)
                throw new ArgumentException("Flags for random character type do not define any type.");
            // Normalize intervals:
            if (small)
                smallint /= max;
            if (capital)
                capitalint /= max;
            if (numeric)
                numericint /= max;
            // Select character type randomly:
            double r = rndgen.NextDouble();
            if (small && r <= smallint)
                ret = RandomCharSmallLetter();
            else if (capital && r <= capitalint)
                ret = RandomCharCapitalLetter();
            else if (numeric && r <= numericint)
                ret = RandomCharNumeric();
            else
                throw new Exception("Could not generate a random character of the specified kind.");
            return ret;
        }

        /// <summary>Returns a random character that is either a capital letter (A-Y) or a small letter (a-z).
        /// Type flags can be combined by bitwise operations.</summary>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        /// <returns>The randomly generated character.</returns>
        public static char RandomChar(IRandomGenerator rndgen = null)
        {
            if (rndgen == null)
                rndgen = RandomGenerator.Global;
            return RandomChar(CharType.CapitalLetter | CharType.SmallLetter, rndgen);
        }

        /// <summary>Returns a randomly generated string of a specified length whose characters are of specified type(result).
        /// It can also be specified that the first character is a letter.</summary>
        /// <param name="length">Length of the string.</param>
        /// <param name="flags">Flags that determine permitted types of characters contained in the string.</param>
        /// <param name="firstletter">If true then first character of the generated string will be a letter.
        /// Warning: when the value is true, make sure that either capital or small letters are permitted character type.</param>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        /// <returns>The generated random string.</returns>
        public static string RandomString(int length, CharType flags, bool firstletter, IRandomGenerator rndgen = null)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; ++i)
            {
                if (i == 0 && firstletter)
                    sb.Append(RandomChar(flags & (CharType.SmallLetter | CharType.CapitalLetter), rndgen));
                else
                    sb.Append(RandomChar(flags, rndgen));
            }
            return sb.ToString();
        }

        /// <summary>Returns a randomly generated string of a specified length whose characters are of specified type(result).</summary>
        /// <param name="length">Length of the string.
        /// It is not guaranteed that the first character is a letter.</param>
        /// <param name="flags">Flags that determine permitted types of characters contained in the string.</param>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        /// <returns>The generated random string.</returns>
        public static string RandomString(int length, CharType flags, IRandomGenerator rndgen = null)
        {
            return RandomString(length, flags, false  /* firstletter */, rndgen);
        }

        /// <summary>Returns a randomly generated string composed of alphanumeric characters.
        /// It can be specified that the first character is a letter.</summary>
        /// <param name="length">Length of the string.</param>
        /// <param name="firstletter">If true then first character of the generated string will be a letter.</param>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        /// <returns>The generated random string.</returns>
        public static string RandomString(int length, bool firstletter, IRandomGenerator rndgen = null)
        {
            return RandomString(length, CharType.CapitalLetter | CharType.SmallLetter | CharType.Numeric /* typeflags */,
                firstletter, rndgen);
        }

        /// <summary>Returns a randomly generated string of a specified length containing alphanumeric characters.
        /// The first character is a letter.</summary>
        /// <param name="length">Length of the string.</param>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        /// <returns>The generated random string.</returns>
        public static string RandomString(int length, IRandomGenerator rndgen = null)
        {
            return RandomString(length, CharType.CapitalLetter | CharType.SmallLetter | CharType.Numeric /* typeflags */,
                true /* firstletter */, rndgen);
        }


        /// <summary>Returns a randomly generated string of a random length containing alphanumeric characters.
        /// The first character is a letter.
        /// Length is between 1 and the value specified by the MaxStringLength property.</summary>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        /// <returns>The generated random string.</returns>
        public static string RandomString(IRandomGenerator rndgen = null)
        {
            if (rndgen == null)
                rndgen = RandomGenerator.Global;
            int length = 1 + rndgen.Next(MaxStringLength);
            return RandomString(length, rndgen);
        }


        /// <summary>Creates and returns a random string consisting of only the letters contained in the specified string.</summary>
        /// <remarks>Cryptographically secure random numbers are used to select characters for the password from 
        /// the array of allowed characters.</remarks>
        /// <param name="stringLength">Length of the generated password.</param>
        /// <param name="allowedChars">String containing allowed characters of the returned string.
        /// <para>By default (if parameter is null or empty string), these are digits and lower- and upper- case English letters.</para></param>
        /// <param name="rndgen">Random generator used. If null or unspecified then global random generator is used.</param>
        /// <returns>A random  password of specified length.</returns>
        public static string RandomString(int stringLength, string allowedChars = null, IRandomGenerator rndgen = null)
        {
            if (string.IsNullOrEmpty(allowedChars))
                allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789";
            Byte[] randomBytes = new Byte[stringLength];
            char[] chars = new char[stringLength];
            int allowedCharCount = allowedChars.Length;
            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rndgen.Next(0, allowedCharCount)];
            }
            return new string(chars);
        }


        #endregion RandomString


        #region Queries.Characters

        /// <summary>Returns true if the specified character is an ASCII characters, and false otherwise.
        /// <para>Characters that qualify: from 0 to 255.</para></summary>
        /// <param name="ch">Character that is querried.</param>
        public static bool IsAscii(char ch)
        {
            return (ch >= 0 || ch <= 255);
        }
        /// <summary>Returns true if the specified character is an ASCII letter, and false otherwise.
        /// <para>Characters that qualify: from 'a' to 'z' and from 'A' to 'Z'.</para></summary>
        /// <param name="ch">Character that is querried.</param>
        public static bool IsAsciiLetter(char ch)
        {
            return (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z');
        }

        /// <summary>Returns true if the specified character is an ASCII digit, and false otherwise.
        /// <para>Characters that qualify: from '0' to '9'.</para></summary>
        /// <param name="ch">Character that is querried.</param>
        public static bool IsAsciiDigit(char ch)
        {
            return (ch >= '0' && ch <= '9');
        }

        /// <summary>Returns true if the specified character is either an ASCII letter or an ASCII 
        /// digit, and false otherwise.
        /// <para>Characters that qualify: from 'a' to 'z', from 'A' to 'Z' and from '0' to '9'.</para></summary>
        /// <param name="ch">Character that is querried.</param>
        public static bool IsAsciiLetterOrDigit(char ch)
        {
            return (ch >= 'a' && ch <= 'z' || ch >= 'A' && ch <= 'Z' || ch >= '0' && ch <= '9');
        }


        #endregion Queries.Characters


        #region Queries

        /// <summary>Returns true if the string corresponding to the specified array of characters is a
        /// legal standard representation of a variable name, and false otherwise.</summary>
        /// <param name="str">Array representation of the string that is queried.</param>
        public static bool IsVarName(char[] str)
        {
            if (str == null)
                return false;
            int length = str.Length;
            if (length == 0)
                return false;
            else
            {
                if (length == 0 && str[0] == '_')
                    return false;  // variable name can not be just "_".
                if ( !( str[0]=='_' || IsAsciiLetter(str[0]) ) )
                    return false;
                for (int i = 1; i < length; ++i)
                {
                    if (!(str[0] == '_' || IsAsciiLetterOrDigit(str[0])))
                        return false;
                }
                return true;
            }
        }

        /// <summary>Returns true if the specified string is a
        /// legal standard representation of a variable name, and false otherwise.</summary>
        /// <param name="str">String that is queried.</param>
        public static bool IsVariableName(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            else
                return IsVarName(str.ToCharArray());
        }

        /// <summary>Returns true if the string corresponding to the specified array of characters is an 
        /// ASCII string (i.e. it contains only ASCII characters - from 0 to 255), and false otherwise.
        /// <para>True is returned for null or empty strings.</para></summary>
        /// <param name="str">Array representation of the string that is queried.</param>
        public static bool IsAsciiString(char[] str)
        {
            if (str == null)
                return true;
            int length = str.Length;
            if (length == 0)
                return true;
            else
            {
                for (int i = 0; i < str.Length; ++i)
                {
                    if (!IsAscii(str[i]))
                        return false;
                }
                return true;
            }
        }


        /// <summary>Returns true if the string specified string is an ASCII string (i.e. it contains only 
        /// ASCII characters - from 0 to 255), and false otherwise.</summary>
        /// <param name="str">String that is queried.</param>
        public static bool IsAsciiString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            else
                return IsAsciiString(str.ToCharArray());
        }


        #endregion Queries


        #region TypeConversions

        /// <summary>Converts a string representation of a boolean setting to boolean.
        /// <para>Strings "true", "yes" and "on" (regardless of capitalization) or non-zero integer representations
        /// result to true, "false", "no", "off" or zero integer representations result in false.</para></summary>
        /// <param name="strsetting">String representation of the specific setting.</param>
        /// <exception cref="InvalidDataException"> if the string is not one of recognized representation of a boolean.</exception>
        /// <returns>Boolean value corresponding to the setting.</returns>
        public static bool ToBoolean(string strsetting)
        {
            bool ret = false;
            if (!string.IsNullOrEmpty(strsetting))
            {
                bool interpreted = false;
                strsetting = strsetting.ToLower();
                if (strsetting == "true" || strsetting == "yes" || strsetting == "on")
                {
                    ret = true;
                    interpreted = true;
                }
                else if (strsetting == "false" || strsetting == "no" || strsetting == "off")
                {
                    ret = false;
                    interpreted = true;
                }
                else
                {
                    try
                    {
                        int i = 0;
                        i = int.Parse(strsetting);
                        interpreted = true;
                        if (i != 0)
                            ret = true;
                    }
                    catch { }
                }
                if (!interpreted)
                    throw new InvalidDataException("Could not interpret the following boolean representing string: "
                        + strsetting + ".");
            }
            return ret;
        }

        /// <summary>Returns a boolean value indicating whether the specified string can represent a boolean value.
        /// <para>Strings "true", "yes" and "on" (regardless of capitalization) or non-zero integer representations
        /// result to true, "false", "no", "off" or zero integer representations result in false.</para>
        /// <para>This method is NOT CONSISTENT with <see cref="bool.TryParse"/> but with <see cref="UtilStr.ToBoolean"/>.</para></summary>
        /// <param name="str">String that is checked if it can represent a boolean value.</param>
        /// <returns></returns>
        public static bool IsBoolean(string str)
        {
            
            bool val;
            bool ret = false;
            try
            {
                val = ToBoolean(str);
                ret = true;
            }
            catch { }
            return ret;
        }


        /// <summary>Converts a string representation of an integer setting to an integer value.
        /// If the setting is not defined then 0 is returned.</summary>
        /// <param name="strsetting">String representation of the specific setting.</param>
        /// <returns>Long integer value corresponding to the setting.</returns>
        public static long ToInt(string strsetting)
        {
            return ToInt(strsetting, 0 /* defaultvalue */);
        }

        /// <summary>Converts a string representation of an integer setting to an integer value.</summary>
        /// <param name="strsetting">String representation of the specific setting.</param>
        /// <param name="defaultvalue">Default value returned in the case that the setting is not defined.</param>
        /// <returns>Long integer value corresponding to the setting.</returns>
        public static long ToInt(string strsetting, long defaultvalue)
        {
            long ret = defaultvalue;
            if (!string.IsNullOrEmpty(strsetting))
                ret = long.Parse(strsetting);
            return ret;
        }

        /// <summary>Returns a boolean value indicating whether the specified string is a valid 
        /// representation of an integer number.</summary>
        /// <param name="str">String that is checked.</param>
        public static bool IsInt(string str)
        {
            int val;
            return int.TryParse(str, out val);
        }

        /// <summary>Converts a string representation of a double setting to an integer value.
        /// If the setting is not defined then 0 is returned.</summary>
        /// <param name="strsetting">String representation of the value.</param>
        /// <returns>Long integer value corresponding to the setting.</returns>
        public static double ToDouble(string strsetting)
        {
            return ToDouble(strsetting, 0 /* defaultvalue */);
        }

        /// <summary>Converts a string representation of a double value to a number of type double.</summary>
        /// <param name="strsetting">String representation of the value.</param>
        /// <param name="defaultvalue">Default value returned in the case that the string is null or empty.</param>
        /// <returns>Long integer value corresponding to the setting.</returns>
        public static double ToDouble(string strsetting, long defaultvalue)
        {
            double ret = defaultvalue;
            if (!string.IsNullOrEmpty(strsetting))
                ret = double.Parse(strsetting);
            return ret;
        }


        /// <summary>Returns a boolean value indicating whether the specified string is a valid 
        /// representation of a number (of type double).</summary>
        /// <param name="str">String that is checked.</param>
        public static bool IsDouble(string str)
        {
            double val;
            return double.TryParse(str, out val);
        }


        #endregion TypeConversions


        #region FileOperations

        /// <summary>Loads complete file contents into a stiring and returnes that string.</summary>
        /// <param name="filePath">Path to the file that is red into a string.</param>
        public static string Load(string filePath)
        {
            string ret = null;
            using (StreamReader sr = new StreamReader(filePath))
            {
                ret  = sr.ReadToEnd();
            }
            return ret;
        }


        /// <summary>Loads complete file contents into the specified stiring.</summary>
        /// <param name="filePath">Path to the file that is red into a string.</param>
        /// <param name="readString">String variable where file contents is stored.</param>
        public static void Load(string filePath, ref string readString)
        {
            readString = Load(filePath);
        }


        /// <summary>Saves the specified string to a specified file.
        /// If the specified file does not exists then it is created anew if possible. 
        /// A boolean argument specifis whether to overwrite existing files or to append the string at the end of the file.</summary>
        /// <param name="str">String to be saved to a file.</param>
        /// <param name="filePath">Path to the file where string is to be saved.</param>
        /// <param name="append">If true then the string is appended at the end of the file in the case that the file already exists.
        /// If false then the file is overwritten in the case taht it already exists.</param>
        public static void Save(string str, string filePath, bool append)
        {
            using (StreamWriter sw = new StreamWriter(filePath, append))
            {
                sw.Write(str);
            }
        }

        /// <summary>Saves the specified string to a specified file.
        /// If the file already exists then is content is overwritten.
        /// If the file does not yet exist then it is created anew.</summary>
        /// <param name="str">String to be written to a file.</param>
        /// <param name="filePath">Path to the file where string is written.</param>
        public static void Save(string str, string filePath)
        { Save(str, filePath, false /* append */ ); }

        /// <summary>Saves the specified string to a specified file.
        /// If the file already exists then string is appended at the end of the current file contents.
        /// If the file does not yet exist then it is created anew.</summary>
        /// <param name="str">String to be written to a file.</param>
        /// <param name="filePath">Path to the file where string is written.</param>
        public static void Append(string str, string filePath)
        { Save(str, filePath, true /* append */ ); }



        #endregion FileOperations


    }  // class Str
}
