
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;


namespace IG.Lib
{
    /// <summary>A simple string parser. Transferred from legacy IGLib as it was dependenccy in IG.Num namespace.
    /// <para>Warning:</para>
    /// <para>This class is obsolete, and there may be a better parser class created in the future. You can
    /// also take a look at StringParser class in legacy IGLib project, however that class is probably not 
    /// significantly better than this one.</para></summary>
    //[Obsolete("Replaced by the StringParser Class.")]
    public class Parser
    {

        static int RetFailed = -1;  // return value for failed string search or parse.

        #region Spaces

        public static int SkipSeparator(string str, int startpos, char separator)
        {
            int ret = -1;
            startpos = SkipSpaces(str, startpos);
            if (startpos >= 0)
            {
                ret = startpos;
                if (str[startpos] == separator)
                {
                    ++startpos;
                    ret = SkipSpaces(str,startpos);
                }
            }
            return ret;
        }

        /// <summary>Returns index of the first non-whitespace character in str from the specified position pos (counted from 0).</summary>
        /// <param name="str">String that is parsed.</param>
        /// <param name="startpos">Position where search begins (counted from 0).</param>
        /// <returns>Index of the character that stops the search.
        /// If string is null or empty or searchstarted after the last character, -1 is returned.</returns>
        public static int SkipSpaces(string str, int startpos)
        {
            int ret = startpos;
            try
            {
                if (string.IsNullOrEmpty(str))
                    return RetFailed;
                else if (startpos < 0)
                    return RetFailed;
                int length = str.Length;
                if (startpos >= length)
                    return RetFailed;
                int pos = startpos;
                char c = str[pos];
                while (pos < length && char.IsWhiteSpace(c))
                {
                    ++pos;
                    if (pos < length)
                        c = str[pos];
                }
                ret = pos;
            }
            catch { ret = RetFailed; }
            return ret;
        }

        #endregion Spaces



        #region Numbers

        // Reading an integer:

        /// <summary>Reads an integer from a string, starting at a specified position and skipping eventual leading spaces. 
        /// Returns the position of the first character after the read integer (or -1 if reading failed) and outputs the read number.</summary>
        /// <param name="number">Output parameter returning the number that was read.</param>
        /// <param name="str">String from which the number is read.</param>
        /// <param name="startpos">Starting position from which the number is searched for.</param>
        /// <returns></returns>
        public static int ReadInteger(ref int number, string str, int startpos)
        {
            long num = 0;
            int ret = ReadInteger(ref num, str, startpos);
            number = (int) num;
            return ret;
        }

        /// <summary>Reads an integer from a string, starting at a specified position and skipping eventual leading spaces. 
        /// Returns the position of the first character after the read integer (or -1 if reading failed) and outputs the read number.</summary>
        /// <param name="number">Output parameter returning the number that was read.</param>
        /// <param name="str">String from which the number is read.</param>
        /// <param name="startpos">Starting position from which the number is searched for.</param>
        /// <param name="skipspaces">Wheather spaces should be skipped.</param>
        /// <returns></returns>
        public static int ReadInteger(ref int number, string str, int startpos, bool skipspaces)
        {
            long num = 0;
            int ret = ReadInteger(ref num, str, startpos, skipspaces);
            number = (int) num;
            return ret;
        }

        /// <summary>Reads an integer from a string, starting at a specified position and skipping eventual leading spaces. 
        /// Returns the position of the first character after the read integer (or -1 if reading failed) and outputs the read number.</summary>
        /// <param name="number">Output parameter returning the number that was read.</param>
        /// <param name="str">String from which the number is read.</param>
        /// <param name="startpos">Starting position from which the number is searched for.</param>
        /// <returns></returns>
        static int ReadInteger(ref long number, string str, int startpos)
        {
            return ReadInteger(ref number, str, startpos, true /* skispaces */ );
        }

        /// <summary>Reads an integer from a string, starting at a specified position. 
        /// Returns the position of the first character after the read number (or -1 if reading failed) and outputs the read number.</summary>
        /// <param name="number">Output parameter returning the number that was read.</param>
        /// <param name="str">String from which the number is read.</param>
        /// <param name="startpos">Starting position from which number is searched for.</param>
        /// <param name="skipspaces">If true then leading spaces are ignored, otherwise operation fails if there is a whitespace
        /// at the starting position.</param>
        /// <returns></returns>
        public static int ReadInteger(ref long number, string str, int startpos, bool skipspaces)
        {
            int ret = -1, pos;
            if (skipspaces)
                startpos = SkipSpaces(str, startpos);
            if (startpos < 1)
                return -1;
            pos = SkipInteger(str, startpos, skipspaces);
            if (pos > 0)
            {
                string numstr = str.Substring(startpos, ret - startpos);
                number = long.Parse(numstr, NumberStyles.Integer,CultureInfo.InvariantCulture);
                ret = pos;
            }
            return ret;
        }

        // Read a number:

        /// <summary>Reads a number from a string, starting at a specified position and skipping eventual leading spaces. 
        /// Returns the position of the first character after the read number (or -1 if reading failed) and outputs the read number.</summary>
        /// <param name="number">Output parameter returning the number that was read.</param>
        /// <param name="str">String from which the number is read.</param>
        /// <param name="startpos">Starting position from which the number is searched for.</param>
        /// <returns></returns>
        public static int ReadNumber(ref double number, string str, int startpos)
        {
            return ReadNumber(ref number, str, startpos, true /* skispaces */ );
        }

        /// <summary>Reads a number from a string, starting at a specified position. 
        /// Returns the position of the first character after the read number (or -1 if reading failed) and outputs the read number.</summary>
        /// <param name="number">Output parameter returning the number that was read.</param>
        /// <param name="str">String from which the number is read.</param>
        /// <param name="startpos">Starting position from which number is searched for.</param>
        /// <param name="skipspaces">If true then leading spaces are ignored, otherwise operation fails if there is a whitespace
        /// at the starting position.</param>
        /// <returns></returns>
        public static int ReadNumber(ref double number, string str, int startpos, bool skipspaces)
        {
            int ret = -1, pos;
            if (skipspaces) 
                startpos = SkipSpaces(str, startpos);
            if (startpos<1)
                return -1;
            pos = SkipNumber(str, startpos, skipspaces);
            if (pos > 0)
            {
                string numstr = str.Substring(startpos, pos - startpos);
                number = double.Parse(numstr,NumberStyles.Float,CultureInfo.InvariantCulture);
                ret = pos;
            }
            return ret;
        }


        // Finding the end of number representation:

        /// <summary>Returns index of the first character in str after the representation of an integer number.</summary>
        /// <param name="str">String that is parsed.</param>
        /// <param name="beginpos">Position where search begins (counted from 0).</param>
        /// <param name="skipspaces">Indicates whether to skip the eventual leading whitespaces.</param>
        /// <param name="failiffloatingpoint">If true then method fails if there is a floating point number representation.
        /// If false then the position of the decimal point is returned.</param>
        /// <returns>Index of the first character in string after the integer number representation (count form 0).
        /// -1 is returned if string is empty, if initial position is out of range or if there is no integer number
        /// representation at the beginning of the string.
        /// if failiffloatingpoint is true then -1 is returned also if there is a floating point number representation.</returns>
        public static int SkipInteger(string str, int beginpos, bool skipspaces, bool failiffloatingpoint)
        {
            int ret = beginpos;
            try
            {
                if (string.IsNullOrEmpty(str))
                    return RetFailed;
                else if (beginpos < 0)
                    return RetFailed;
                int length = str.Length;
                if (beginpos >= length)
                    return RetFailed;
                int pos = beginpos;
                bool 
                    anydigitpassed=false; // wheteher any digit has been passed
                if (skipspaces)
                    pos = SkipSpaces(str, pos);
                if (pos < 0)
                    return RetFailed;
                char c = str[pos];
                if (c=='+' || c=='-')
                {
                    ++pos;
                    if (pos < length)
                        c = str[pos];
                }
                if (char.IsDigit(c))
                    anydigitpassed = true;
                while (pos < length && char.IsDigit(c))
                {
                    ++pos;
                    if (pos < length)
                        c = str[pos];
                }
                if (c == '.' && failiffloatingpoint)
                    return RetFailed;  // the number representation is floating point.
                if (anydigitpassed)  // there must be at least 1 digit
                    ret = pos;
                else
                    ret = RetFailed;
                }
            catch { ret = RetFailed; }
            return ret;
        }


        /// <summary>Returns index of the first character in str after the representation of an integer number.
        /// Leading whitespace characters are ignored, i.e. number is looked for after all space characters.</summary>
        /// <param name="str">String that is parsed.</param>
        /// <param name="beginpos">Position where search begins (counted from 0).</param>
        /// <param name="skipspaces">Indicates whether to skip leading spaces.</param>
        /// <returns>Index of the first character in string after the integer number representation (count form 0).
        /// -1 is returned if string is empty, if initial position is out of range or if there is no number
        /// representation at the beginning of the string, or if there is a floating point number representation.</returns>
        public static int SkipInteger(string str, int beginpos, bool skipspaces)
        {
            return SkipInteger(str, beginpos, skipspaces, true /* failiffloatingpoint*/ );
        }


        /// <summary>Returns index of the first character in str after the representation of an integer number.
        /// Leading whitespace characters are ignored, i.e. number is looked for after all space characters.</summary>
        /// <param name="str">String that is parsed.</param>
        /// <param name="beginpos">Position where search begins.</param>
        /// <returns>Index of the first character in string after the integer number representation (count form 0).
        /// -1 is returned if string is empty, if initial position is out of range or if there is no number
        /// representation at the beginning of the string, or if there is a floating point number representation.</returns>
        public static int SkipInteger(string str, int beginpos)
        {
            return SkipInteger(str, beginpos, true  /* skipspaces */ );
        }



        /// <summary>Returns index of the first character in str after the representation of a number 
        /// (either floating point or integer).</summary>
        /// <param name="str">String that is parsed.</param>
        /// <param name="beginpos">Position where search begins (counted from 0).</param>
        /// <param name="skipspaces">Indicates whether to skip the eventual leading whitespaces.</param>
        /// <returns>Index of the first character in string after the number representation (count form 0).
        /// -1 is returned if string is empty, if initial position is out of range or if there is no number
        /// representation at the beginning of the string.</returns>
        public static int SkipNumber(string str, int beginpos, bool skipspaces)
        {
            int ret = beginpos;
            try
            {
                if (string.IsNullOrEmpty(str))
                    return RetFailed;
                else if (beginpos < 0)
                    return RetFailed;
                int length = str.Length;
                if (beginpos >= length)
                    return RetFailed;
                int pos = beginpos;
                bool
                    anydigitpassed = false, // wheteher any digit has been passed
                    decimalpointpassed = false,  // whether a decimal piont has been passed
                    exponentpassed=false;  // whether an exponent has been passed
                if (skipspaces)
                    pos = SkipSpaces(str, pos);
                if (pos < 0)
                    return RetFailed;
                char c = str[pos];
                if (c == '+' || c == '-')  // skip the sign
                {
                    ++pos;
                    if (pos < length)
                        c = str[pos];
                }
                if (char.IsDigit(c))
                    anydigitpassed = true;
                else if (c == '.')
                    decimalpointpassed = true;
                while (pos < length && char.IsDigit(c))
                {
                    ++pos;
                    if (pos < length)
                        c = str[pos];
                }
                if (c == '.')
                {
                    if (decimalpointpassed)
                    {
                        // Decimal point has alreaddy been passed, therefore this one is not a part of a number.
                        // The number is in legal form only if any digit has also been passed:
                        if (anydigitpassed)
                            return pos;
                        else
                            return -1;
                    }
                    else
                    {
                        decimalpointpassed = true;
                        ++pos;
                        c = str[pos];
                        while (pos < length && char.IsDigit(c))
                        {
                            ++pos;
                            if (pos < length)
                                c = str[pos];
                        }
                    }
                }
                if ( pos<length && (decimalpointpassed || anydigitpassed)  // exponent can folloe just a decimal point
                    && c=='e' || c=='E')
                {
                    // Check whether there is an exponent; if yes then set position after the exponent.
                    int posexp = pos+1;
                    char cexp;
                    if (posexp < length)
                    {
                        cexp = str[posexp];
                        if (cexp == '+' || cexp == '-')
                        {
                            ++posexp;
                            if (posexp < length)
                                cexp = str[posexp];
                        }
                        if (char.IsDigit(cexp))
                        {
                            exponentpassed = true;
                            // e or E introduced an exponent, therefore we find its last digit:
                            while (posexp < length && char.IsDigit(cexp))
                            {
                                ++posexp;
                                if (posexp < length)
                                    cexp = str[posexp];
                            }
                            // set the position after the exponent:
                            pos = posexp;
                            c = cexp;
                        }
                    }
                }
                if (anydigitpassed || (decimalpointpassed && exponentpassed))  // there must be at least one digit, or decimal point and exponent
                    ret = pos;
                else
                    ret = RetFailed;
            }
            catch { ret = RetFailed; }
            return ret;
        }

        #endregion  // Numbers


        /// <summary>Returns index of the first character in str after the representation of a number 
        /// (either floating point or integer).</summary>
        /// <param name="str">String that is parsed.</param>
        /// <param name="beginpos">Position where search begins (counted from 0).</param>
        /// <returns>Index of the first character in string after the number representation (count form 0).
        /// -1 is returned if string is empty, if initial position is out of range or if there is no number
        /// representation at the beginning of the string (after leading spaces).</returns>
        public static int SkipNumber(string str, int beginpos)
        {
            return SkipNumber(str, beginpos, true  /* skipspaces */ );
        }


        public static int SkipToCharacter(string str, int beginpos, char target)
        {
            int ret = beginpos;



            ret = 0;
            if (ret==0)
            {
                ret = -1;
                throw new Exception("SkipToCharacter Not implemented yet.");
            }
            return ret;
        }


        #region Lists

        /// <summary>Reads a list of numbers of type double from result stirng startng at specified position, 
        /// and returns position after the list.
        /// Numbers must be separated by a specified separator (with spaces allowed between them) and embedded within specified brackets.</summary>
        /// <param name="tab">Output parameter where read-in list is written.</param>
        /// <param name="str">String from which the list is read.</param>
        /// <param name="startpos">Starting positio nwhere reading begins.</param>
        /// <param name="open">Opening bracket of the list.</param>
        /// <param name="close">Closing bracket of the list.</param>
        /// <param name="separator">Separator that separates the numbers.</param>
        /// <param name="skipspaces">If true then leading spaces are skipped first, otherwise starting position must be
        /// exactly at the opening bracket of the list.</param>
        /// <returns>Returns position of the first character after the list.</returns>
        public static int ReadDoubleList(ref List<double> tab, string str, int startpos, 
            char open, char close, char separator, bool skipspaces)
        {
            int ret = -1;
            if(tab==null)
                tab=new List<double>();
            else
                tab.RemoveRange(0,tab.Count); // tab.reemove
            // Skip leading spaces if instructed:
            if (skipspaces)
                startpos = SkipSpaces(str, startpos);
            if (startpos<0)
                return -1;
            else if (str[startpos]!=open)
                return -1;
            else
            {
                ++startpos;  // now we are inside the list, get position to the first number and start reading elements:
                startpos = SkipSpaces(str, startpos);
                if (startpos < 0)
                    return -1;
                else if (str[startpos] == close)
                {
                    // We have an empty list:
                    ++startpos;
                    return startpos;
                }
                bool stop = false;
                double val=0;
                int num = 0;
                while (!stop)
                {
                    // Read next number:
                    startpos = ReadNumber(ref val, str, startpos);
                    if (startpos < 0) // error in format
                        return -1;
                    else
                    {
                        // Value read, add it to the table
                        ++num;
                        tab.Add(val);
                        // Skip spaces and check whether there is already the closing separator that reads the list:
                        startpos = SkipSpaces(str, startpos);
                        if (startpos < 0)
                            return -1;
                        else if (str[startpos]==close)
                        {
                            // We have a closing bracket - we reached end of the list. Set position immediately after the list
                            // and return position:
                            ++startpos;
                            ret=startpos;
                            return ret;
                        } else
                        {
                            // Not finished yet; skip the separator and prepare for reading the next value:
                            startpos = SkipSeparator(str,startpos,separator);
                            if (startpos<0)
                                return -1;
                        }
                    }
                }
            }
            return ret;
        }


        /// <summary>Reads a list of lists of numbers of type double from result stirng startng at specified position, 
        /// and returns position after the list.
        /// Numbers and sub-lists must be separated by a specified separator (with spaces allowed between them) and embedded within specified brackets.</summary>
        /// <param name="tab">Output parameter where read-in list is written.</param>
        /// <param name="str">String from which the list is read.</param>
        /// <param name="startpos">Starting positio nwhere reading begins.</param>
        /// <param name="open">Opening bracket of the list and sub-lists.</param>
        /// <param name="close">Closing bracket of the list and sub-lists.</param>
        /// <param name="separator">Separator that separates the numbers and sub-lists.</param>
        /// <param name="skipspaces">If true then leading spaces are skipped first, otherwise starting position must be
        /// exactly at the opening bracket of the list.</param>
        /// <returns>Returns position of the first character after the list.</returns>
        public static int ReadDoubleList(ref List<List<double>> tab, string str, int startpos,
            char open, char close, char separator, bool skipspaces)
        {
            int ret = -1;
            if (tab == null)
                tab = new List<List<double>>();
            else
                tab.RemoveRange(0, tab.Count); // tab.reemove
            // Skip leading spaces if instructed:
            if (skipspaces)
                startpos = SkipSpaces(str, startpos);
            if (startpos < 0)
                return -1;
            else if (str[startpos] != open)
                return -1;
            else
            {
                ++startpos;  // now we are inside the list, get position to the first number and start reading elements:
                startpos = SkipSpaces(str, startpos);
                if (startpos < 0)
                    return -1;
                else if (str[startpos] == close)
                {
                    // We have an empty list:
                    ++startpos;
                    return startpos;
                }
                bool stop = false;
                // double val = 0;
                int num = 0;
                while (!stop)
                {
                    // Read next sub-list:
                    List<double> sublist = null;
                    startpos = ReadDoubleList(ref sublist, str, startpos, open, close, separator, true /* skipspaces */ );
                    if (startpos < 0) // error in format
                        return -1;
                    else
                    {
                        // Sub-list read, add it to the table
                        ++num;
                        tab.Add(sublist);
                        // Skip spaces and check whether there is already the closing separator that reads the list:
                        startpos = SkipSpaces(str, startpos);
                        if (startpos < 0)
                            return -1;
                        else if (str[startpos] == close)
                        {
                            // We have a closing bracket - we reached end of the list. Set position immediately after the list
                            // and return position:
                            ++startpos;
                            ret = startpos;
                            return ret;
                        }
                        else
                        {
                            // Not finished yet; skip the separator and prepare for reading the next value:
                            startpos = SkipSeparator(str, startpos, separator);
                            if (startpos < 0)
                                return -1;
                        }
                    }
                }
            }
            return ret;
        } //  ReadDoubleList(ref List<List<double>>, ... )





        #endregion // Lists


        #region Brackets

        /// <summary>Returns position of the first character that has the specified nesting messagelevel with respect to 
        /// open and close brackets and the current messagelevel.
        /// Nesting messagelevel begins with 0 and is increased with every open bracked and decreased with any closed bracket.</summary>
        /// <param name="str">String that is parsed.</param>
        /// <param name="beginpos">Position where search begins (counted from 0).</param>
        /// <param name="open">The opening bracket.</param>
        /// <param name="close">The closing bracket.</param>
        /// <param name="targetlevel">Level that must be reached. Can be either a positive or negative integer.
        /// If messagelevel is 0 then initial position is returned.</param>
        /// <returns>Positon of the first character from startpos (inclusively) with a given nesting messagelevel.
        /// Returns -1 if such character can not be found, if string is null or empty or if initial position is out of range.</returns>
        int SkipToBracketLevel(string str, int beginpos, char open, char close,int targetlevel)
        {
            int ret=beginpos;
            try
            {
                if (string.IsNullOrEmpty(str))
                    return RetFailed;
                else if (beginpos < 0)
                    return RetFailed;
                int length = str.Length;
                if (beginpos >= length)
                    return RetFailed;
                int pos = beginpos;
                char c = str[pos];
                int level=0;
                while (pos < length && level!=targetlevel)
                {
                    if (c==open)
                        ++level;
                    else if (c==close)
                        --level;
                    ++pos;
                    if (pos < length)
                        c = str[pos];
                }
                if (pos<length)
                    ret=pos;
                else
                    return RetFailed;
            }
            catch { ret = RetFailed; }
            return ret;
        }


        #endregion   // Brackets


        public static string ListToString(List<double> l)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < l.Count; ++i)
            {
                sb.Append(l[i].ToString(CultureInfo.InvariantCulture));
                if (i < l.Count - 1)
                    sb.Append(", ");
                //else
                //    sb.Append("}");
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static string ListToString(List<List<double>> l)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for (int i = 0; i < l.Count; ++i)
            {
                sb.Append(ListToString(l[i]));
                if (i < l.Count - 1)
                    sb.Append(", ");
                //else
                //    sb.Append("}");
            }
            sb.Append("}");
            return sb.ToString();
        }


        public static void Examples()
        {
            string str = @"{ 0.1, 0.2, 0.3} 
                    {{}, {2.1, 2.2, 2.3}, {3.1, 3.2}} ";
            List<double> dl = null;
            List<List<double>> ddl = null;
            int startpos = 0;
            startpos = ReadDoubleList(ref dl, str, startpos, '{', '}', ',', true);
            Console.WriteLine();
            Console.WriteLine("Parsed string: " + str);
            Console.WriteLine("First list (single): ");
            Console.WriteLine(ListToString(dl));
            if (startpos >= 0)
            {
                startpos = ReadDoubleList(ref ddl,  str, startpos,  '{', '}', ',', true);
                Console.WriteLine("Second list (double): ");
                Console.WriteLine(ListToString(ddl));
            }
        }


    }  //class Parser

}  // namespace IG.Lib

