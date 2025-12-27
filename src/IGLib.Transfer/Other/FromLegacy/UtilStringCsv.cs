// Copyright (c) Igor Grešovnik (2009), IGLib license; http://www2.arnes.si/~ljc3m2/igor/ioptlib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace IG.Lib
{



    /// <summary>Auxiliary utilities for dealing with files in CSV and other delimited formats.
    /// <para>CSV - related utilities were moved here from the <see cref="UtilStr"/> class on May 2013.</para></summary>
    /// $A Igor xx May13;
    public class UtilCsv
    {

        /// <summary>The default separator in the CSV files - comma (",").</summary>
        public const string DefaultCsvSeparator = ",";

        public static string _csvSeparator = DefaultCsvSeparator;

        public static string CsvSeparator
        {
            get { return _csvSeparator; }
            set { _csvSeparator = value; }
        }

        /// <summary>Returns true if the specified line in a 2D jagged array of
        /// string cell values (arranged in rows) is empty or does not exist, 
        /// otherwise false is returned.</summary>
        /// <param name="values">A 2D jagged array that contains a table of tables (rows) of string cell values. 
        /// Such array corresponds a table of rows of cells stored in CSV format.</param>
        /// <param name="rowIndex">Index of the line that is tested for being empty or not.</param>
        /// $A Igor Oct08;
        public static bool CsvIsEmptyRow(string[][] values, int rowIndex)
        {
            if (values != null)
                if (rowIndex < values.Length)
                {
                    string[] line = values[rowIndex];
                    if (line.Length > 1)
                        return false;
                    else if (line.Length == 1)
                        if (line[0] != null)
                            if (line[0].Length > 0)
                                return false;

                }
            return true;
        }


        /// <summary>Returns the value of the specified cell of a 2D jagged array of
        /// string cell values (arranged in rows) at the specified row and column,
        /// or null if such a value does not exist.</summary>
        /// <param name="values">A 2D jagged array that contains a table of tables (rows) of string cell values. 
        /// Such array corresponds a table of rows of cells stored in CSV format.</param>
        /// <param name="rowIndex">Row number.</param>
        /// <param name="columnIndex">Column number.</param>
        /// <returns></returns>
        /// $A Igor Oct08;
        public static string CsvGetValue(string[][] values, int rowIndex, int columnIndex)
        {
            if (values != null)
                if (values.Length > rowIndex)
                    if (values[rowIndex] != null)
                        if (values[rowIndex].Length > columnIndex)
                            return values[rowIndex][columnIndex];
            return null;
        }

        /// <summary>Returns index of thr column that contains data with the specified name in a 2D jagged array of values.
        /// It is assumed that the first non-empty row (subarray) contains names of data columns.
        /// -1 is returned if the column with the specified field name is not found.
        /// Column (or field) names are assumed to be case insensitive.</summary>
        /// <param name="values">A 2D jagged array that contains a table of tables (rows) of string cell values. 
        /// Such array corresponds a table of rows of cells stored in CSV format.</param>
        /// <param name="fieldName">Name of the data column (field) whose column index is searched for.</param>
        /// <returns>Index of data column with the specified name. Column (or field) names are assumed 
        /// to be contained in the first nonempty sub-array (row).</returns>
        /// $A Igor Oct08;
        public static int CsvColumnIndex(string[][] values, string fieldName)
        {
            return CsvColumnIndex(values, fieldName, false /* caseSensitive */);
        }

        /// <summary>Returns index of thr column that contains data with the specified name in a 2D jagged array of values.
        /// It is assumed that the first non-empty row (subarray) contains names of data columns.
        /// -1 is returned if the column with the specified field name is not found.</summary>
        /// <param name="values">A 2D jagged array that contains a table of tables (rows) of string cell values. 
        /// Such array corresponds a table of rows of cells stored in CSV format.</param>
        /// <param name="fieldName">Name of the data column (field) whose column index is searched for.</param>
        /// <param name="caseSensitive">Whether column (field) names are case sensitive or not.</param>
        /// <returns>Index of data column with the specified name. Column (or field) names are assumed 
        /// to be contained in the first nonempty subarray (row).</returns>
        /// $A Igor Oct08;
        public static int CsvColumnIndex(string[][] values, string fieldName, bool caseSensitive)
        {
            if (values != null) if (values.Length > 0)
                {
                    int rowNum = 0;
                    bool foundNonEmpty = false;
                    while (rowNum < values.Length && !foundNonEmpty)
                    {
                        if (values[rowNum] != null)
                            if (values[rowNum].Length > 0)
                            {
                                if (values[rowNum].Length > 1)
                                    foundNonEmpty = true;
                                else if (values[rowNum][0] != null)
                                    if (values[rowNum][0].Length > 0)
                                        foundNonEmpty = true;
                            }
                        if (!foundNonEmpty)
                            ++rowNum;
                    }
                    if (foundNonEmpty)
                    {
                        if (caseSensitive)
                        {
                            for (int columnNum = 0; columnNum < values[rowNum].Length; ++columnNum)
                                if (values[rowNum][columnNum] == fieldName)
                                    return columnNum;
                        }
                        else
                        {
                            fieldName = fieldName.ToLower();
                            for (int columnNum = 0; columnNum < values[rowNum].Length; ++columnNum)
                                if (values[rowNum][columnNum].ToLower() == fieldName)
                                    return columnNum;
                        }
                    }
                }
            return -1;
        }

        /// <summary>Converts a CSV string to a 2D jagged array of string values and returns it.
        /// Constant <see cref="UtilCsv.DefaultCsvSeparator"/>) is assumed to be a separator in the CSV format.</summary>
        /// <param name="csvString">A CSV string representing data in CSV format.</param>
        /// <returns>A 2D jagged array that contains a table of string cell values of the CSV string that is parsed.
        /// Each Subarray contains a single row of cells values.</returns>
        /// $A Igor Oct08;
        public static string[][] FromCsvString(string csvString)
        {
            return FromCsvString(csvString, DefaultCsvSeparator /* separator */);
        } 

        /// <summary>Converts a CSV string to a 2D jagged array of string values and returns it.</summary>
        /// <param name="csvString">A CSV string representing data in CSV format.</param>
        /// <param name="separator">Separator that is used in CSV format (usually , or ;).
        /// If null or empty string then Constant <see cref="UtilCsv.DefaultCsvSeparator"/> is taken.</param>
        /// <returns>A 2D jagged array that contains a table of string cell values of the CSV string that is parsed.
        /// Each Subarray contains a single row of cells values.</returns>
        /// $A Igor Oct08;
        public static string[][] FromCsvString(string csvString, string separator)
        {
            string[][] ret = null;
            if (string.IsNullOrEmpty(csvString))
                return null;
            if (string.IsNullOrEmpty(separator))
                separator = DefaultCsvSeparator;
            // Extract lines first:
            char[] lineSeparator = new char[] { '\n' };
            string[] lines = csvString.Split(lineSeparator);

            //char[] lineSeparator = Environment.NewLine.ToCharArray();
            //string[] lines = csvString.Split(lineSeparator);
            //if (lines.Length == 1)
            //{
            //    // We must take into account the case when lines are not separated by
            //    // system's newline:
            //    char[] lineSeparator1 = new char[]{'\n'};
            //    string[] lines1 = csvString.Split(lineSeparator1);
            //    if (lines1.Length > 1)
            //    {
            //        char[] trimChars = new char[]{'\r'};
            //        for (int i = 0; i < lines1.Length; ++i)
            //        {
            //            if (lines1[i] != null)
            //                lines1[i] = lines1[i].Trim(trimChars);
            //        }
            //        lines = lines1;
            //    }
            //}
            // From lines, extract individual table cells and store them:
            ret = new string[lines.Length][];
            char[] separatorChar = separator.ToCharArray();
            char[] quoteTrimChars = new char[] { '\"' };
            List<string> strList = null;
            for (int i = 0; i < lines.Length; ++i)
            {
                string currentLine = lines[i];
                if (currentLine != null)
                    currentLine = currentLine.TrimEnd('\r');
                if (string.IsNullOrEmpty(currentLine))
                    ret[i] = new string[0]; // an empty line;
                else
                {
                    string[] cells = currentLine.Split(separatorChar);
                    bool containsDoubleQuote = false;
                    if (cells != null)
                        for (int j = 0; j < cells.Length; ++j)
                        {
                            string str = cells[j];
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (cells[j].First() == '\"') // (cells[j].First() == '\"')
                                {
                                    containsDoubleQuote = true;
                                    break;
                                }
                            }
                        }
                    if (!containsDoubleQuote)
                        ret[i] = cells;
                    else
                    {
                        // Split line contains strings that begin with double quotes.
                        // This meand that contents between the double quotes should not be 
                        // split, and we must glue such strings together again, adding deliminator
                        // character that caused the split:
                        if (strList == null)
                            strList = new List<string>();
                        strList.Clear();
                        int j = 0;
                        while (j < cells.Length)
                        {
                            if (!string.IsNullOrEmpty(cells[j]))
                            {
                                if (cells[j].First() == '\"')
                                {
                                    if (cells[j].Last() == '\"')
                                    {
                                        // Everything OK, there were no deliminator characters within
                                        // the string contained in double quotes:
                                        strList.Add(cells[j]);
                                    }
                                    else
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        sb.Append(cells[j].TrimStart(quoteTrimChars));
                                        ++j;
                                        while (j < cells.Length)
                                        {
                                            if (cells[j].Last() == '\"')
                                            {
                                                sb.Append(cells[j].TrimEnd(quoteTrimChars));
                                                break;
                                            }
                                            else
                                            {
                                                sb.Append(cells[j]);
                                                sb.Append(separator);
                                            }
                                            ++j;
                                        }
                                        strList.Add(sb.ToString());
                                    }
                                }
                                else
                                    strList.Add(cells[j]);
                            } else
                                strList.Add(cells[j]);
                            ++j;
                        }
                        ret[i] = strList.ToArray();
                    }
                }
            }
            return ret;
        }  // FromCsvStringcsvString, separator

        /// <summary>Convertsa a 2D jagged array of string values to a string in CSV format.</summary>
        /// <param name="values">dD table of string values that are written in CSV format.</param>
        /// <param name="separator">Separator that is used to separate cell values.
        /// If null or empty string then Constant <see cref="UtilCsv.DefaultCsvSeparator"/> is taken.</param>
        /// <remarks>If the value of any cell contains separators then it is embedded
        /// in double quotes.</remarks>
        /// $A Igor Oct08;
        public static string ToCsvString(string[][] values, string separator)
        {
            StringBuilder sb = new StringBuilder();
            if (values != null)
            {
                if (string.IsNullOrEmpty(separator))
                    separator = DefaultCsvSeparator;
                for (int i = 0; i < values.Length; ++i)
                {
                    string[] lineValues = values[i];
                    if (lineValues != null)
                    {
                        for (int j = 0; j < lineValues.Length; ++j)
                        {
                            string val = lineValues[j];
                            if (val != null)
                            {
                                if (val.Contains(separator))
                                {
                                    sb.Append("\"");
                                    sb.Append(val);
                                    sb.Append("\"");
                                }
                                else
                                    sb.Append(val);
                            }
                            if (j < lineValues.Length - 1)
                                sb.Append(separator);
                        }
                    }
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        } // ToCsvString(values, separator)

        /// <summary>Returns a string that represents a table of string values.
        /// Each value is embedded in double quotes, values are separated by commas, and
        /// tables of values are separated by newlines.</summary>
        /// <param name="values">a 2D jagged array of string values that is converted to a
        /// string.</param>
        /// <param name="valueSeparator">Separator that separates individual values in a row.
        /// If not specified then Constant <see cref="UtilCsv.DefaultCsvSeparator"/> is taken.</param>
        /// <returns>String containing values in double quotes, separated by commas and 
        /// arrays of values separated by newlines, that can, for example, be printed to 
        /// a console. An additional newline is added after the last array of values.</returns>
        /// $A Igor Oct08;
        protected static string TableToString(string[][] values, string valueSeparator)
        {
            if (string.IsNullOrEmpty(valueSeparator))
                valueSeparator = DefaultCsvSeparator;
            string lineSeparator = Environment.NewLine;
            StringBuilder sb = new StringBuilder();
            if (values != null)
            {
                for (int i = 0; i < values.Length; ++i)
                {
                    if (values[i] != null)
                    {
                        for (int j = 0; j < values[i].Length; ++j)
                        {
                            sb.Append(values[i][j]);
                            if (j < values[i].Length - 1)
                                sb.Append(valueSeparator);
                        }
                    }
                    sb.Append(lineSeparator);
                }
            }
            return sb.ToString();
        } // TableToString(values)

        /// <summary>Reads contents of a CSV file and returns a 2D jagged array of strigg values contained in the file.</summary>
        /// <param name="filePath">Path to the CSV file that is read and parsed.</param>
        /// <param name="separator">Separator that is used in the CSV file. If not specified (null or empty string) then Constant <see cref="UtilCsv.DefaultCsvSeparator"/> is assumed.</param>
        /// <returns>A 2D jagged array that contains a table of string cell values of the CSV string that is parsed.
        /// Each Subarray contains a single row of cells values.</returns>
        /// $A Igor Oct08;
        public static string[][] LoadCsv(string filePath, string separator)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                if (filePath == null)
                    throw new ArgumentException("CSV file path not specified (null string).");
                else
                    throw new ArgumentException("CSV File path not specified (empty string).");
            }
            if (!File.Exists(filePath))
                throw new ArgumentException("CSV file does not exist: " + filePath);
            string csvString = null;
            UtilStr.Load(filePath, ref csvString);
            return FromCsvString(csvString, separator);
        }

        /// <summary>Reads contents of a CSV file and returns a 2D jagged array of strigg values contained in the file.
        /// Constant <see cref="UtilCsv.DefaultCsvSeparator"/> is assumed as separator in the CSV file.</summary>
        /// <param name="filePath">Path to the CSV file that is read and parsed.</param>
        /// <returns>A 2D jagged array that contains a table of string cell values of the CSV string that is parsed.
        /// Each Subarray contains a single row of cells values.</returns>
        /// $A Igor Oct08;
        public static string[][] LoadCsv(string filePath)
        {
            return LoadCsv(filePath, DefaultCsvSeparator /* separator */ );
        }

        /// <summary>Saves a 2D jagged array of string cell values into a CSV file.</summary>
        /// <param name="filePath">Path to the file into which contents is written.</param>
        /// <param name="values">A 2D jagged array of string cell values. Each outer element contains one row of values in CSV.</param>
        /// <param name="separator">Separator that is used in CSV format.</param>
        /// <param name="append">If true then the CSV string is appended to the existent file if 
        /// the file already exists. Otherwise, existend files are overwritten.</param>
        public static void SaveCsv(string filePath, string[][] values, string separator, bool append)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                if (filePath == null)
                    throw new ArgumentException("CSV file path not specified (null string).");
                else
                    throw new ArgumentException("CSV File path not specified (empty string).");
            }
            string csvString = ToCsvString(values, separator);
            UtilStr.Save(csvString, filePath, append);
        }

        /// <summary>Saves a 2D jagged array of string cell values into a CSV file.
        /// If the file already exists then its contents are overwritten.</summary>
        /// <param name="filePath">Path to the file into which contents is written.</param>
        /// <param name="values">A 2D jagged array of string cell values. Each outer element contains one row of values in CSV.</param>
        /// <param name="separator">Separator that is used in CSV format.</param>
        /// $A Igor Oct08;
        public static void SaveCsv(string filePath, string[][] values, string separator)
        {
            SaveCsv(filePath, values, separator, false /* append */);
        }

        /// <summary>Saves a 2D jagged array of string cell values into a CSV file.
        /// Constant <see cref="UtilCsv.DefaultCsvSeparator"/> is assumed to be a separator for the CSV format.</summary>
        /// <param name="filePath">Path to the file into which contents is written.</param>
        /// <param name="values">A 2D jagged array of string cell values. Each outer element contains one row of values in CSV.</param>
        /// <param name="append">If true then the CSV string is appended to the existent file if 
        /// the file already exists. Otherwise, existend files are overwritten.</param>
        public static void SaveCsv(string filePath, string[][] values, bool append)
        {
            SaveCsv(filePath, values, DefaultCsvSeparator /* separator */, append);
        }

        /// <summary>Saves a 2D jagged array of string cell values into a CSV file.
        /// If the file already exists then its contents are overwritten.
        /// Constant <see cref="UtilCsv.DefaultCsvSeparator"/> is assumed to be a separator for the CSV format.</summary>
        /// <param name="filePath">Path to the file into which contents is written.</param>
        /// <param name="values">A 2D jagged array of string cell values. Each outer element contains one row of values in CSV.</param>
        /// $A Igor Oct08;
        public static void SaveCsv(string filePath, string[][] values)
        {
            SaveCsv(filePath, values, DefaultCsvSeparator /* separator */, false /* append */);
        }

        /// <summary>Tests conversion of a 2D jagged array of string values to a CSV formatted string and back.
        /// Returns true if the array restored by conversion of the original array to a CSV string
        /// and back from a CSV string to a jagged array is the same (within inevitable ambiguities of the 
        /// CSV format) as the original array.</summary>
        /// <param name="values">Original 2D jagged array of values arranged by rows, which is converted to
        /// a CSV string and back.</param>
        /// <param name="separator">Separator that is used in CSV format (usually this will be "," or ";", sometimes "\t").
        /// If not specified (null or empty string) then the Constant <see cref="UtilCsv.DefaultCsvSeparator"/> is assumed.</param>
        /// <param name="printResults">If true then detailed results of the test are printed to a console, 
        /// indicating all individual differences between the original and restored data. It is also indicated
        /// whether the individual differences are considered errors (if not then a string describing a difference
        /// begins with "WARNING").</param>
        /// <returns>True if the test has passed, i.e. the restored data obtained by conversion to 
        /// a CSV string and back is the same (within allowed discrepancies) than original data.</returns>
        public static bool TestCsvStringConversion(string[][] values, string separator,
            bool printResults)
        {
            bool ret = true;
            if (printResults)
            {
                Console.WriteLine();
                Console.WriteLine("Testing conversion to/from CSV.");
                Console.WriteLine("Table of strings to be converted:");
                Console.WriteLine(TableToString(values, DefaultCsvSeparator));
                Console.WriteLine();
            }
            string csvString = ToCsvString(values, separator);
            string[][] valuesRestored = FromCsvString(csvString, separator);
            if (values == null)
            {
                if (valuesRestored != null)
                {
                    bool thisOk = true;
                    if (valuesRestored.Length > 1)
                    {
                        ret = thisOk = false;
                    }
                    else if (valuesRestored.Length == 1)
                    {
                        if (valuesRestored[0].Length > 1)
                            ret = thisOk = false;
                        else if (valuesRestored[0].Length == 1)
                            if (valuesRestored[0][0] != null)
                                if (valuesRestored[0][0].Length > 0)
                                    ret = thisOk = false;
                    }
                    if (printResults)
                    {
                        if (thisOk)
                            Console.WriteLine("WARNING: Original 2D array of values is null but restored is not.");
                        else
                            Console.WriteLine("ERROR: Original 2D array of values is null but restored is not.");
                        Console.WriteLine("  Length of restored array: " + valuesRestored.Length);
                    }
                }
            }
            else
            {
                if (valuesRestored == null)
                {
                    bool thisOk = true;
                    if (values.Length > 1)
                        thisOk = ret = false;
                    else if (values.Length == 1)
                    {
                        if (values[0].Length > 1)
                            thisOk = ret = false;
                        else if (values[0].Length == 1)
                            if (values[0][0] != null)
                                if (values[0][0].Length > 0)
                                    thisOk = ret = false;
                    }
                    if (printResults)
                    {
                        if (thisOk)
                            Console.WriteLine("WARNING: restored 2D array of values is null.");
                        else
                            Console.WriteLine("ERROR: restored 2D array of values is null.");
                        Console.WriteLine("  Length of original array: " + values.Length);
                    }
                }
                else
                {
                    // Neither original nor restored array is null.
                    if (values.Length != valuesRestored.Length)
                    {
                        bool thisOk = true;
                        if (Math.Abs(values.Length - valuesRestored.Length) > 1)
                            thisOk = ret = false;
                        if (printResults)
                        {
                            if (thisOk)
                                Console.WriteLine("WARNING: lengths of original and restored array are different.");
                            else
                                Console.WriteLine("ERROR: lengths of original and restored array are different.");
                            Console.WriteLine("  Original: " + values.Length + ", restored: " + valuesRestored.Length);
                        }
                    }
                    for (int i = 0; i < values.Length; ++i)
                    {
                        if (i >= valuesRestored.Length)
                        {
                            bool thisOk = true;
                            if (values[i].Length > 1)
                                thisOk = ret = false;
                            else if (values[i].Length == 1)
                            {
                                if (values[i][0] != null)
                                    if (values[i][0].Length > 0)
                                        thisOk = ret = false;
                            }
                            if (printResults)
                            {
                                if (thisOk)
                                    Console.WriteLine("WARNING: Line no. " + i + " does not exist in the restored array.");
                                else
                                    Console.WriteLine("ERROR: Line no. " + i + " does not exist in the restored array.");
                            }
                        }
                        if (values[i] == null)
                        {
                            if (valuesRestored[i] != null)
                            {
                                bool thisOk = true;
                                if (valuesRestored[i].Length > 1)
                                    thisOk = ret = false;
                                else if (valuesRestored[i].Length == 1)
                                    if (valuesRestored[i][0] != null)
                                        if (valuesRestored[i][0].Length > 0)
                                            thisOk = ret = false;
                                if (thisOk)
                                    Console.WriteLine("WARNING: Original row No. " + i + " is null lbut restored is not.");
                                else
                                    Console.WriteLine("ERROR: Original row No. " + i + " is null lbut restored is not.");
                                Console.WriteLine("  Length of restored line: " + valuesRestored[i].Length);
                            }
                        }
                        else
                        {
                            // values[i]!=null
                            if (valuesRestored[i] == null)
                            {
                                bool thisOk = true;
                                if (values[i].Length > 1)
                                    thisOk = ret = false;
                                else if (values[i].Length == 1)
                                    if (values[i][0] != null)
                                        if (values[i][0].Length > 0)
                                            thisOk = ret = false;
                                if (printResults)
                                {
                                    if (thisOk)
                                        Console.WriteLine("WARNING: Restored line No. " + i + " is null but original is not.");
                                    else
                                        Console.WriteLine("ERROR: Restored line No. " + i + " is null but original is not.");
                                }
                            }
                            else
                            {
                                if (valuesRestored[i].Length != values[i].Length)
                                {
                                    ret = false;
                                    if (printResults)
                                    {
                                        Console.WriteLine("ERROR: Restored line No. " + i + " has different length than original.");
                                        Console.WriteLine("  Original: " + values[i].Length + ", restored: "
                                            + valuesRestored.Length);
                                    }
                                }
                                for (int j = 0; j < values[i].Length; ++j)
                                {
                                    if (j >= valuesRestored[i].Length)
                                    {
                                        ret = false;
                                        if (printResults)
                                        {
                                            Console.WriteLine("ERROR: restored value is missing.");
                                            Console.WriteLine("  line: " + i + ", column: " + j);
                                        }
                                    }
                                    else
                                    {
                                        string cell = values[i][j];
                                        string cellRestored = valuesRestored[i][j];
                                        if (cell != cellRestored)
                                        {
                                            ret = false;
                                            if (printResults)
                                            {
                                                Console.WriteLine("ERROR: difference in original and restored cell.");
                                                Console.WriteLine("  line: " + i + ", column: " + j);
                                                Console.WriteLine("  original: \"" + cell + "\"");
                                                Console.WriteLine("  restored: \"" + cellRestored + "\"");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (printResults)
            {
                if (ret)
                    Console.WriteLine("Test successful, restored values are equal than original.");
                else
                    Console.WriteLine("Test NOT successful, ERRORS occurred.");
                Console.WriteLine();
            }
            return ret;
        }  // TestCsv(values, separator, printResults)


        /// <summary>Example of using CSV format utilities.</summary>
        /// $A Igor Oct08;
        public static void ExampleCsv()
        {
            string separator = DefaultCsvSeparator;
            string[][] values = new string[][] {
                new string[] {"00","01\".","03,,."},
                new string[] {"10","11","12"},
                new string[] {},
               new string[] {"30","31","32","33"},
                null,
                new string[] {"50","51"}
            };
            TestCsvStringConversion(values, separator, true);
        }  // ExampleCsv()


    }  // class CSV



}
