// Copyright © Igor Grešovnik (2008 - present). License:
// https://github.com/ajgorhoe/IGLib.modules.IGLibCore/blob/main/LICENSE.md

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Parsing
{


    /// <summary>
    /// <para>Implementation that follows this:</para>
    /// https://stackoverflow.com/questions/298830/split-string-containing-command-line-parameters-into-string-in-c-sharp
    /// </summary>
    [Obsolete("Not used any more, may be removed in the future.")]
    public class CommandLineParserWindows
    {



        [DllImport("shell32.dll ", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] 
            string lpCmdLine, out int pNumArgs
            );


        /// <summary>Splits command-line string to arguments in the same way as Windows OS does when running
        /// a program (it uses a Window function via pinvoke).</summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        /// <exception cref="System.ComponentModel.Win32Exception"></exception>
        public static string[] CommandLineToArgs(string commandLine)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new NotSupportedException("This method is only supported on Windows OS.");
            }
            else
            {
                int argc;
                IntPtr args = CommandLineToArgvW(commandLine, out argc);
                if (args == IntPtr.Zero)
                    throw new System.ComponentModel.Win32Exception();
                try
                {
                    string[] arguments = new string[argc];
                    for (int i = 0; i < arguments.Length; i++)
                    {
                        var p = Marshal.ReadIntPtr(args, i * IntPtr.Size);
                        arguments[i] = Marshal.PtrToStringUni(p);
                    }

                    return arguments;
                }
                finally
                {
                    Marshal.FreeHGlobal(args);
                }
            }
                
        }



    }


}
