// Copyright © Igor Grešovnik (2008 - present). License:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing
{


    public static class StringExtensions
    {


        /// <summary>Splits string according to function telling whether a given character should be used
        /// as splitting point.</summary>
        /// <param name="str">String to be split.</param>
        /// <param name="splitControlFunc">Function telling for a given character whether it should be split
        /// the string or not.</param>
        /// <returns></returns>
        public static IEnumerable<string> Split(this string str,
                                            Func<char, bool> splitControlFunc)
        {
            int nextChunk = 0;

            for (int whichChar = 0; whichChar < str.Length; whichChar++)
            {
                if (splitControlFunc(str[whichChar]))
                {
                    yield return str.Substring(nextChunk, whichChar - nextChunk);
                    nextChunk = whichChar + 1;
                }
            }
            yield return str.Substring(nextChunk);
        }


        /// <summary>Returns a string where the leading and trailing quotes have been removed, if any.</summary>
        /// <param name="str"></param>
        /// <param name="quote">Character that defines the quote.</param>
        /// <returns>Bare string <paramref name="str"/> not enclosed in quotes (i.e., if it was enclosed before), 
        /// with quote character is defined by <paramref name="quote"/>.</returns>
        public static string TrimMatchingQuotes(this string str, char quote)
        {
            if ((str.Length >= 2) &&
                (str[0] == quote) && (str[str.Length - 1] == quote))
                return str.Substring(1, str.Length - 2);

            return str;
        }


    }


    /// <summary>
    /// <para>Implementation that follows this:</para>
    /// https://stackoverflow.com/questions/298830/split-string-containing-command-line-parameters-into-string-in-c-sharp
    /// </summary>
    public class CommandLineParserSimple
    {


        public static IEnumerable<string> SplitCommandLine(string commandLine)
        {
            bool inQuotes = false;

            return commandLine.Split(c =>
            {
                if (c == '\"')
                    inQuotes = !inQuotes;

                return !inQuotes && c == ' ';
            })
                              .Select(arg => arg.Trim().TrimMatchingQuotes('\"'))
                              .Where(arg => !string.IsNullOrEmpty(arg));
        }

        public static void Test(string cmdLine, params string[] args)
        {
            string[] split = SplitCommandLine(cmdLine).ToArray();

            Debug.Assert(split.Length == args.Length);

            for (int n = 0; n < split.Length; n++)
                Debug.Assert(split[n] == args[n]);
        }

        public static void PerformSomeTests()
        {
            Test(@"/src:""C:\tmp\Some Folder\Sub Folder"" /users:""abcdefg@hijkl.com"" tasks:""SomeTask,Some Other Task"" -someParam",
             @"/src:""C:\tmp\Some Folder\Sub Folder""", @"/users:""abcdefg@hijkl.com""", @"tasks:""SomeTask,Some Other Task""", @"-someParam");
        }

    }


}
