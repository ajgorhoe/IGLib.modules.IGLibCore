
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;

#if NETFRAMEWORK
using System.Security.Principal;
using System.Security.AccessControl;
#endif

namespace IG.Lib
{

    ///// <summary>Lockable object, has a Lock property that returns object on which
    ///// lock must be performed in order to lock the object.</summary>
    //public interface ILockable
    //{
    //    object Lock { get; }
    //}

    /// <summary>General utilities.</summary>
    /// $A Igor Apr10 Jun15;
    public class Util
    {


        #region ThreadSynchronization

        private static object _lockGlobal = new object();

        /// <summary>Global, process-level locking object. 
        /// <para>This object can be used for synchronization of any static methods.</para>
        /// <para>Warning: Do not use this lock for locking long lasting operations, since this can result in deadlocks.</para></summary>
        public static object LockGlobal
        { get { return _lockGlobal; } }


        /// <summary>Check whether the specified mutex has been abandoned, and returns true
        /// if it has been (otherwise, false is returned).
        /// <para>After the call, mutex is no longer in abandoned state (WaitOne() will not throw an exception)
        /// if it has been before the call.</para>
        /// <para>Call does not block.</para></summary>
        /// <param name="m">Mutex that is checked, must not be null.</param>
        /// <returns>true if mutex has been abandoned, false otherwise.</returns>
        public static bool MutexCheckAbandoned(Mutex m)
        {
            bool ret = false;
            if (m == null)
                throw new ArgumentException("Mutex to be checked is not specified (null argument).");
            bool acquired = false;
            try
            {
                acquired = m.WaitOne(0);
                if (acquired)
                {
                    try
                    {
                        m.ReleaseMutex();
                    }
                    catch { }
                }
            }
            catch
            {
                ret = true;
                try
                {
                    m.ReleaseMutex();
                }
                catch { }
            }
            return ret;
        }

        /// <summary>Name of the global mutex.</summary>
        public const string MutexGlobalName = "Global\\IG.Lib.Utils.MutexGlobal.R2D2_by_Igor_Gresovnik";

        protected static volatile Mutex _mutexGlobal;

        /// <summary>Mutex for system-wide exclusive locks.</summary>
        public static Mutex MutexGlobal
        {
            get
            {
                if (_mutexGlobal == null)
                {
                    lock (LockGlobal)
                    {
                        if (_mutexGlobal == null)
                        {
                            bool createdNew = false;
#if NETFRAMEWORK
                            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                            MutexSecurity mutexsecurity = new MutexSecurity();
                            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.FullControl, AccessControlType.Allow));
                            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.ChangePermissions, AccessControlType.Deny));
                            mutexsecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.Delete, AccessControlType.Deny));
                            //_mutexGlobal = new Mutex(false, MutexGlobalName);
                            _mutexGlobal = new Mutex(false, MutexGlobalName,
                                out createdNew, mutexsecurity);
#else
                            _mutexGlobal = new Mutex(false, MutexGlobalName, out createdNew);
#endif
                        }
                    }
                }
                return _mutexGlobal;
            }
        }

        /// <summary>Check whether the global mutex (property <see cref="MutexGlobal"/>) has been abandoned, 
        /// and returns true if it has been (otherwise, false is returned).
        /// <para>After the call, mutex is no longer in abandoned state (WaitOne() will not throw an exception)
        /// if it has been before the call.</para>
        /// <para>Call does not block.</para></summary>
        /// <returns>true if mutex has been abandoned, false otherwise.</returns>
        public static bool MutexGlobalCheckAbandoned()
        {
            return MutexCheckAbandoned(MutexGlobal);
        }

        /// <summary>Suspends execution of the current thread for the specified time (in seconds).</summary>
        /// <param name="sleepTimeInSeconds">Sleeping time in seconds. If less than 0 then thread sleeps indefinitely.</param>
        public static void SleepSeconds(double sleepTimeInSeconds)
        {
            const int largeNumberOfMilliseconds = 100000;
            if (sleepTimeInSeconds >= 0)
            {
                int timeMs = (int)Math.Ceiling(sleepTimeInSeconds * 1000.0);
                Thread.Sleep(timeMs);
            }
            else
            {
                if (OutputLevel >= 0)
                {
                    Console.WriteLine(
                        Environment.NewLine + Environment.NewLine
                        + "WARNING: Sleeping indefinitely in thread Np. " + Thread.CurrentThread.ManagedThreadId
                        + Environment.NewLine + Environment.NewLine);
                }

                while (true)
                    Thread.Sleep(largeNumberOfMilliseconds);
            }
        }

        #endregion ThreadSynchronization


        #region OutputLevelGlobal

        private static volatile int _outputLevel = 0;

        /// <summary>Serves as default output level for new objects of many classes that include the output
        /// level property (usually named "OutputLevel"). Such a property defines how much information about
        /// operation of the object is ouput to the console.</summary>
        /// <remarks>
        /// <para>General guidlines for use of the output level property in classes:</para>
        /// <para>The property usually defineds the quantity of output produced by an object of a class
        /// that implements this property. It is not strictly prescribed what certain values of the property 
        /// mean. By loose agreement, any negative value means unspecified output level (property not yet initialized),
        /// 0 means that no output is produced, 1 means only the most important information is ouptut and higher 
        /// values mean that more detailed information about operation is output to the console.</para>
        /// <para>For example application, see e.g. the IG.Gr.PlotterZedGraph in the 2D plotting library that uses IGLib.</para>
        /// </remarks>
        public static int OutputLevel
        {
            get { lock (LockGlobal) { return _outputLevel; } }
            set { lock (LockGlobal) { _outputLevel = value; } }
        }

        #endregion OutputLevelGlobal


        #region HashFunctions

        /// <summary>Standard string representation of null values of objects (often used when overriding 
        /// <see cref="object.ToString"/> method).</summary>
        public const string NullRepresentationString = "null";


        private static volatile int _maxLengthIntToString;

        /// <summary>Returns maximal length of string representation of integer value of type <see cref="int"/></summary>
        protected internal static int MaxLengthIntToString
        {
            get
            {
                if (_maxLengthIntToString == 0)
                {
                    lock (LockGlobal)
                    {
                        if (_maxLengthIntToString == 0)
                            _maxLengthIntToString = int.MinValue.ToString().Length;
                    }
                }
                return _maxLengthIntToString;
            }
        }

        /// <summary>Returns an integer hash function of the specified object.
        /// <para>Returned integer is always positive.</para>
        /// <para>This hash function is bound to the <see cref="object.ToString"/> method of the specified object,
        /// which means that it returns the same value for any two objects that have the same string
        /// representation.</para></summary>
        /// <param name="obj">Object whose hash function is returned.</param>
        /// <remarks><para>This hash function is calculated in such a way that <see cref="object.ToString"/>() is
        /// called first on <paramref name="obj"/> in order to obtain object's string representation (or, if the object is
        /// null, the <see cref="Util.NullRepresentationString"/> is taken), and then the <see cref="string.GetHashCode"/> 
        /// is called on the obtained string and its value returned.</para></remarks>
        public static int GetHashFunctionInt(object obj)
        {
            if (obj == null)
                return Util.NullRepresentationString.GetHashCode();
            else
            {
                string stringrep = obj.ToString();
                if (stringrep == null)
                    throw new InvalidOperationException("String representation of non-null object whose hash function is calculated is null.");
                int ret = stringrep.GetHashCode();
                if (ret < 0)
                {
                    ret = -ret;
                }
                return ret;
            }
        }


        /// <summary>Returns a string-valued hash function of the specified object.
        /// <para>This hash function is bound to the <see cref="object.ToString"/> method of the specified object,
        /// which means that it returns the same value for any two objects that have the same string
        /// representation.</para></summary>
        /// <param name="obj">Object whose string-valued hash function is returned.</param>
        /// <remarks><para>This hash function is calculated in such a way that <see cref="object.ToString"/>() is
        /// called first on <paramref name="obj"/> in order to obtain object's string representation (or, if the object is
        /// null, the <see cref="Util.NullRepresentationString"/> is taken), and then the <see cref="string.GetHashCode"/> 
        /// is called on the obtained string and its value returned.</para></remarks>
        /// <seealso cref="Util"/>
        public static string GetHashFunctionString(Object obj)
        {
            int maxLength = MaxLengthIntToString;
            char[] generatedCode = GetHashFunctionInt(obj).ToString().ToCharArray();
            int length = 0;
            if (generatedCode != null)
                length = generatedCode.Length;
            if (length > maxLength)
            {
                if (OutputLevel >= 1)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("WARNING: length of hash code " + length + " is greater than the maximum (" + maxLength + ".");
                    Console.WriteLine(Environment.NewLine);
                }
                maxLength = length;
            }
            char[] returnedCode = new char[maxLength];
            for (int i = 1; i <= maxLength; i++)
            {
                // Copy character representations of digits of generated hash code 
                // to the (equilength) returned code, starting form the last one:
                if (i <= length)
                {
                    // Copy character from teh generated code:
                    returnedCode[maxLength - i] = generatedCode[length - i];
                }
                else
                {
                    // No characters left in the generated code, fill remaining most significant
                    // characters with '0':
                    returnedCode[maxLength - i] = '0';
                }
            }
            return new string(returnedCode);
        }


        #endregion HashFunctions


        #region ListResize

        /// <summary>Allocates or re-allocates (resizes) the specified list in such a way that it
        /// contains the specified number of elements after operation.</summary>
        /// <typeparam name="T">Type of the list element.</typeparam>
        /// <param name="list">List to be allocated. </param>
        /// <param name="count">Number of elements list will contain after operation.</param>
        /// <param name="defaultElement">Elements to be added to the list if there are currently 
        /// too few elements.</param>
        /// <param name="reduceCapacity">If true then capacity is reduced if the current list's
        /// capacity exceeds the specified number  of elements.</param>
        /// $A Igor Apr10;
        public static void ResizeList<T>(ref List<T> list, int count, T defaultElement,
                bool reduceCapacity)
        {
            if (list == null)
                list = new List<T>(count);
            else
            {
                if (list.Capacity < count)
                    list.Capacity = count;
            }
            int currentCount = list.Count;
            if (currentCount < count)
            {
                for (int i = currentCount + 1; i <= count; ++i)
                    list.Add(defaultElement);
            }
            else if (currentCount > count)
            {
                list.RemoveRange(count, currentCount - count);
            }
            if (reduceCapacity && list.Capacity > count)
                list.Capacity = count;
        }


        /// <summary>Allocates or re-allocates (resizes) the specified list in such a way that it
        /// contains the specified number of elements after operation.
        /// If new size is smaller than the original size of the list then its capacity is not
        /// reduced.</summary>
        /// <typeparam name="T">Type of the list element.</typeparam>
        /// <param name="list">List to be allocated. </param>
        /// <param name="count">Number of elements list will contain after operation.</param>
        /// <param name="defaultElement">Elements to be added to the list if there are currently 
        /// too few elements.</param>
        /// $A Igor Apr10;
        public static void ResizeList<T>(ref List<T> list, int count, T defaultElement)
        {
            ResizeList<T>(ref list, count, defaultElement, false  /* reduceCapacity */ );
        }


        /// <summary>Allocates or re-allocates (resizes) the specified list in such a way that it
        /// contains the specified number of elements after operation. 
        /// If list must be enlarged then null elements are inserted to new places.
        /// List must contain elements of some reference type!</summary>
        /// <typeparam name="T">Type of the list element, must be a reference type.</typeparam>
        /// <param name="list">List to be allocated. </param>
        /// <param name="count">Number of elements list will contain after operation.</param>
        /// <param name="reduceCapacity">If true then capacity is reduced if the current list's
        /// capacity exceeds the specified number  of elements.</param>
        /// $A Igor Apr10;
        public static void ResizeListRefType<T>(ref List<T> list, int count,
                bool reduceCapacity) where T : class
        {
            ResizeList<T>(ref list, count, (T)null /* defaultElement */,
                reduceCapacity);
        }

        /// <summary>Allocates or re-allocates (resizes) the specified list in such a way that it
        /// contains the specified number of elements after operation. 
        /// If list must be enlarged then null elements are inserted to new places.
        /// List must contain elements of some reference type!</summary>
        /// <typeparam name="T">Type of the list element, must be a reference type.</typeparam>
        /// <param name="list">List to be allocated. </param>
        /// <param name="count">Number of elements list will contain after operation.</param>
        /// $A Igor Apr10;
        public static void ResizeListRefType<T>(ref List<T> list, int count) where T : class
        {
            ResizeList<T>(ref list, count, (T)null /* defaultElement */,
                false /* reduceCapacity */);
        }

        /// <summary>Copies all elements of the specified list to a target list.
        /// After operation, target list contains all elements of the source list (only references are copied for objects)
        /// in the same order. 
        /// If the original list is null then target list can either be allocated (if it was allocated before the call) or not.
        /// Target list is allocated or re-allocated as necessary.</summary>
        /// <typeparam name="T">Type of elements contained in the list.</typeparam>
        /// <param name="original">Original list.</param>
        /// <param name="target">List that elements of the original list are copied to.</param>
        public static void CopyList<T>(List<T> original, ref List<T> target)
        {
            if (original == null)
            {
                if (target != null)
                    target.Clear();
            }
            else
            {
                int numEl = original.Count;
                if (target == null)
                    target = new List<T>(original.Capacity);
                int numElTarget = target.Count;
                if (numElTarget > numEl)
                {
                    target.RemoveRange(numEl, numElTarget - numEl);
                    numElTarget = target.Count;
                }
                for (int i = 0; i < numEl; ++i)
                {
                    if (i < numElTarget)
                        target[i] = original[i];
                    else
                        target.Add(original[i]);
                }
            }
        }

        /// <summary>Copies all elements of the specified list to a target table.
        /// After operation, target table contains all elements of the source list (only references are copied for objects)
        /// in the same order. 
        /// If the original list is null then target table will also become null.
        /// Target table is allocated or re-allocated as necessary.</summary>
        /// <typeparam name="T">Type of elements contained in the list.</typeparam>
        /// <param name="original">Original list.</param>
        /// <param name="target">Table that elements of the original list are copied to.</param>
        public static void CopyList<T>(List<T> original, ref T[] target)
        {
            if (original == null)
            {
                if (target != null)
                    target = null;
            }
            else
            {
                int numEl = original.Count;
                if (target == null)
                    target = new T[numEl];
                else if (target.Count() != numEl)
                    target = new T[numEl];
                for (int i = 0; i < numEl; ++i)
                {
                    target[i] = original[i];
                }
            }
        }

        #endregion ListResize


        #region Arrays_Collections


        /// <summary>Creates and returns a stirng representation of a list of items.
        /// <para>Each item in the list is represented by calling its own ToString() method.</para></summary>
        /// <typeparam name="T">Type of elements of the listt.</typeparam>
        /// <param name="elementList">List whose string representation is returned.</param>
        public static string ToString<T>(IList<T> elementList)
        {
            return ToString(elementList, false /* newLines */, 0 /* numIndent */);
        }


        /// <summary>Creates and returns a stirng representation of a list of items.
        /// <para>Each item in the list is represented by calling its own ToString() method.</para></summary>
        /// <typeparam name="T">Type of elements of the listt.</typeparam>
        /// <param name="elementList">List whose string representation is returned.</param>
        /// <param name="newLines">If true then representation of each element is positioned in its ownl line.</param>
        /// <param name="numIndent">Indentation. If greater than 0 then this number of spaces is inserted before each 
        /// line of the returned string (including in the beginning of the returned string, which also applies
        /// if <paramref name="newLines"/> is false).</param>
        /// <returns></returns>
        public static string ToString<T>(IList<T> elementList, bool newLines, int numIndent = 0)
        {
            StringBuilder sb = new StringBuilder();
            if (numIndent < 0)
                throw new ArgumentException("Indentation can not be negative.");
            //else if (numIndent > 0)
            //{
            //    for (int i = 0; i < numIndent; ++i)
            //        sb.Append(" ");
            //    sb.Append(' ', numIndent);
            //    indent = sb.ToString();
            //    sb.Clear();
            //}
            if (elementList == null)
            {
                if (numIndent > 0)
                    sb.Append(' ', numIndent);
                sb.Append("null");
            }
            else
            {
                int length = elementList.Count;
                if (numIndent > 0)
                    sb.Append(' ', numIndent);
                sb.Append('{');
                if (newLines)
                {
                    sb.AppendLine();
                    if (numIndent > 0)
                        sb.Append(' ', numIndent);
                }
                for (int i = 0; i < length; ++i)
                {
                    if (newLines)
                        sb.Append("  ");
                    sb.Append(elementList[i].ToString());
                    if (i < length - 1)
                    {
                        if (newLines)
                            sb.Append(",");
                        else
                            sb.Append(", ");
                    }
                    if (newLines)
                    {
                        sb.AppendLine();
                        if (numIndent > 0)
                            sb.Append(' ', numIndent);
                    }
                }
            }
            sb.Append('}');
            if (newLines)
                sb.Append(Environment.NewLine);
            return sb.ToString();
        }

        /// <summary>Returns true if the specified enumerables (collections) are equal, false otherwise.
        /// <para>Enumerables are considered equal if they are both null, or they are of the same size and all
        /// elements are equal.</para></summary>
        /// <typeparam name="T">Type of elements of the enumerables.</typeparam>
        /// <param name="a">First enumerable to be compared.</param>
        /// <param name="b">Second enumerable to be compared.</param>
        /// <returns>True if the two enumerables are of equal lengths and have equal adjacent elements or are both null; false otherwise.</returns>
        public static bool AreEqual<T>(IEnumerable<T> a, IEnumerable<T> b)
            where T : IComparable<T>
        {
            if (a == null)
                return (b == null);
            else if (b == null)
                return false;
            else
            {
                IEnumerator<T> enuma = a.GetEnumerator();
                IEnumerator<T> enumb = b.GetEnumerator();
                while (true)
                {
                    bool isNexta = enuma.MoveNext();
                    bool isNextb = enumb.MoveNext();
                    if (!isNexta)
                    {
                        if (isNextb)
                            return false;  // incompatible element count
                        else
                            return true; // all elements were checked and there were no disagreements
                    }
                    else if (!isNextb)
                        return false;  // incompatible element count
                    else
                    {
                        if (!enuma.Current.Equals(enumb.Current))
                            return false;  // unequal elements detected
                    }
                }
            }
        }

        /// <summary>Returns true if the specified collections are equal, false otherwise.
        /// <para>Collection are considered equal if they are both null, or they are of the same size and all
        /// elements are equal.</para></summary>
        /// <remarks>There is also a method for comparing variables of <see cref="IEnumerable{T}"/> interface which can be used
        /// in all places where this method is used. A special method for collections was created for efficiency reasons,
        /// because the <see cref="IList{T}"/> interface implements the Count property, thus collections of unequal sizes
        /// can be immediately detected as unequal by comparing their size, and one does not need to iterate over elements.</remarks>
        /// <typeparam name="T">Type of elements of the collections.</typeparam>
        /// <param name="a">First collection to be compared.</param>
        /// <param name="b">Second collection to be compared.</param>
        /// <returns>True if the two collection are of equal lengths and have equal adjacent elements or are both null; false otherwise.</returns>
        public static bool AreEqual<T>(IList<T> a, IList<T> b)
            where T : IComparable<T>
        {
            if (a == null)
                return (b == null);
            else if (b == null)
                return false;
            else if (a.Count != b.Count)  // unequal size. This is the difference with comparing IEnumerable<T>.
                return false;
            else
            {
                if (a == null)
                    return (b == null);
                else if (b == null)
                    return false;
                else
                {
                    int la = a.Count, lb = b.Count;
                    if (la != lb)
                        return false;
                    else
                    {
                        for (int i = 0; i < la; ++i)
                            if (!a[i].Equals(b[i]))
                                return false;
                        return true;
                    }
                }
            }
        }




        /// <summary>Concatenates an arbitrary number of arrays or lists of the specified type, and returns the result.</summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="arrays">An arbitrary-length list of array or list parameters to be concatenated.</param>
        /// <returns>An array that contains, in order of appearance of the listed list/array parameters, all elements of
        /// those lists/arrays.</returns>
        public static T[] Concatenate<T>(params IList<T>[] arrays)
        {
            var result = new T[arrays.Sum(a => a.Count)];
            int offset = 0;
            for (int whichArray = 0; whichArray < arrays.Length; whichArray++)
            {
                arrays[whichArray].CopyTo(result, offset);
                offset += arrays[whichArray].Count;
            }
            return result;
        }


        #endregion Arrays_Collections


        #region HexadecimalStrings

        /// <summary>Returns a byte array that is represented by a hexadecimal string.</summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] FromHexString(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return null;
            int length = hex.Length;
            int numBytes = 0;
            bool withSeparator = false;  // whether a separator is used in the string
            char separator = '0';
            if (length >= 3)
            {
                separator = hex[2];
                if (!char.IsLetterOrDigit(separator))
                    withSeparator = true;
            }
            if (withSeparator)
            {
                numBytes = (length + 1) / 3;
                if ((length + 1) % 3 != 0)
                    throw new ArgumentException("Hexadecimal string with separators is of invalid length " + length + ".");
            }
            else
            {
                numBytes = length / 2;
                if ((length) % 2 != 0)
                    throw new ArgumentException("Hexadecimal string (without separators) is of invalid length " + length + ".");
            }
            byte[] bytes = new byte[numBytes];
            int whichHex = 0;
            for (int i = 0; i < numBytes; ++i)
            {
                char c1 = hex[whichHex];
                char c2 = hex[whichHex + 1];
                bytes[i] = (byte)(16 * HexCharToInt(c1) + HexCharToInt(c2));
                if (withSeparator)
                    whichHex += 3;
                else
                    whichHex += 2;
                // arr[i] = (byte)((HexCharToInt(hex[i << 1]) << 4) + (HexCharToInt(hex[(i << 1) + 1])));
            }

            return bytes;
        }

        /// <summary>Returns value of the specified hexadecimal character (e.g. 9 for '9', 10 for 'a' or 'A', 15 for 'f' or 'F').</summary>
        /// <param name="hex">Hexadecimal character whose integer value is returned.</param>
        /// <returns></returns>
        public static int HexCharToInt(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }


        /// <summary>Returns a hexadecimal string representation of the specified byte array using lower case letters for digits above 9.</summary>
        /// <param name="bytes">Array of bytes whose hexadecial representation is to be returned.</param>
        /// <param name="separator">If not null or empty string then this string is inserted between hexadecimal digits.
        /// <para>If specified then it must be a single character string, and may not be a digit or a letter.</para></param>
        public static string ToHexString(byte[] bytes, string separator = null)
        {
            return ToHexString(bytes, false /* upperCase */, separator);
        }

        /// <summary>Returns a hexadecimal string representation of the specified byte array.</summary>
        /// <param name="bytes">Array of bytes whose hexadecial representation is to be returned.</param>
        /// <param name="upperCase">Whether digits greater than 9 should be represented by upper case letters (default is false).</param>
        /// <param name="separator">If not null or empty string then this string is inserted between hexadecimal digits.
        /// <para>If specified then it must be a single character string, and may not be a digit or a letter.</para></param>
        public static string ToHexString(byte[] bytes, bool upperCase, string separator = null)
        {
            int numBytes = 0;
            if (bytes != null)
                numBytes = bytes.Length;
            bool withSeparator = (!string.IsNullOrEmpty(separator));
            if (withSeparator)
            {
                if (separator.Length > 1)
                    throw new ArgumentException("Separator length must be 1 for generating hexadecimal representations of byte arrays.");
                char first = separator[0];
                if (char.IsLetterOrDigit(first))
                    throw new ArgumentException("Invalid separator " + separator + ", may not be a digit or a letter.");
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < numBytes; ++i)
            {
                if (upperCase)
                    sb.Append(bytes[i].ToString("X2"));
                else
                    sb.Append(bytes[i].ToString("x2"));
                if (withSeparator && i < numBytes - 1)
                    sb.Append(separator);
            }
            return sb.ToString();
        }

        /// <summary>Returns true if the two specified hexadecimal strings represent the same sequence of bytes (or the same number),
        /// and false otherwise.
        /// <para>If any string is null or its length is 0 then false is returned.</para></summary>
        /// <param name="hexString1">The first hexadecimal sequence to be compared.</param>
        /// <param name="hexString2">The second hexadecimal sequence to be compared.</param>
        public static bool AreHexStringsEqual(string hexString1, string hexString2)
        {
            bool ret = false;
            byte[] b1 = FromHexString(hexString1);
            byte[] b2 = FromHexString(hexString2);
            if (b1 != null && b2 != null)
                if (b1.Length == b2.Length)
                {
                    int length = b1.Length;
                    for (int i = 0; i < length; ++i)
                        if (b1[i] != b2[i])
                            return false;
                    ret = true;
                }
            return ret;

            //if (hexString1 != null)
            //    hexString1 = hexString1.ToUpper();
            //if (hexString2 != null)
            //    hexString2 = hexString2.ToUpper();
            //return hexString1 == hexString2;
        }

        #endregion HexadecimalStrings



        #region CollectionToString

        /// <summary>Returns a string representing the specified collection of objects.
        /// Each object is printeed by its ToString() method.
        /// Works on all collections, including lists and arrays.</summary>
        /// <param name="list">Collection to be converted to srting.</param>
        /// <param name="addNewlines">If true then a newline is added before each element printed.</param>
        /// <param name="numIndent">Number of spaces aded before each element.</param>
        public static string CollectionToString(System.Collections.ICollection list, bool addNewlines,
            int numIndent)
        {
            if (list == null)
                return "<null list>";
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('{');
                int count = list.Count;
                int i = 0;
                foreach (object obj in list)
                {
                    if (addNewlines)
                        sb.AppendLine();
                    if (numIndent > 0)
                        sb.Append(' ', numIndent);
                    sb.Append(obj.ToString());
                    if (i + 1 < count)
                        sb.Append(", ");
                    ++i;
                }
                if (addNewlines)
                    sb.AppendLine();
                sb.Append('}');
                return sb.ToString();
            }
        }

        /// <summary>Returns a string representing the specified collection of objects.
        /// Each object is printeed by its ToString() method.
        /// A  newline and two spaces are added before each element printed.
        /// Works on all collections, including lists and arrays.</summary>
        /// <param name="list">Collection to be converted to srting.</param>
        public static string CollectionToString(System.Collections.ICollection list)
        {
            return CollectionToString(list, true, 2);
        }



        /// <summary>Returns a string representing the specified list in long form.
        /// Count property (i.e. number of elements in collection) is also printed.
        /// Works on all collections, including lists and arrays.</summary>
        /// <param name="collection">Collection to be converted to srting.</param>
        /// <param name="addNewlines">If true then a newline is added before each element printed.</param>
        /// <param name="numIndent">Number of spaces aded before each element.</param>
        public static string CollectionToStringLong(System.Collections.ICollection collection, bool addNewlines,
            int numIndent)
        {
            string prefix = null;
            if (collection != null)
            {
                prefix = "[Count: " + collection.Count + "] ";
            }
            return prefix + CollectionToString(collection, addNewlines, numIndent);
        }

        /// <summary>Returns a string representing the specified list in long form.
        /// Count property (i.e. number of elements in collection) is also printed.
        /// Works on all collections, including lists and arrays.
        /// A  newline and two spaces are added before each element printed.</summary>
        /// <param name="collection">Collection to be converted to srting.</param>
        public static string CollectionToStringLong(System.Collections.ICollection collection)
        {
            return CollectionToStringLong(collection, true, 2);
        }


        /// <summary>Returns a string representing the specified generic list in short form (without count and capacity).</summary>
        /// <typeparam name="T">Type of list elements.</typeparam>
        /// <param name="list">List to be converted to srting.</param>
        /// <param name="addNewlines">If true then a newline is added before each element printed.</param>
        /// <param name="numIndent">Number of spaces aded before each element.</param>
        public static string ListToString<T>(List<T> list, bool addNewlines,
            int numIndent)
        {
            // return ListToString(list, addNewlines, numIndent);
            return ToString(list, addNewlines, numIndent);
        }

        /// <summary>Returns a string representing the specified generic list in long form.
        /// Count and Capacity properties are also printed.
        /// A  newline and two spaces are added before each element printed.</summary>
        /// <typeparam name="T">Type of list elements.</typeparam>
        /// <param name="list">List to be converted to srting.</param>
        public static string ListToString<T>(List<T> list)
        {
            return ListToString(list, true, 2);
        }



        /// <summary>Returns a string representing the specified generic list in long form.
        /// Count and Capacity properties are also printed.</summary>
        /// <typeparam name="T">Type of list elements.</typeparam>
        /// <param name="list">List to be converted to srting.</param>
        /// <param name="addNewlines">If true then a newline is added before each element printed.</param>
        /// <param name="numIndent">Number of spaces aded before each element.</param>
        public static string ListToStringLong<T>(List<T> list, bool addNewlines,
            int numIndent)
        {
            string prefix = null;
            if (list != null)
            {
                prefix = "[Count: " + list.Count + ", Capacity: " + list.Count() + "] ";
            }
            return prefix + ListToString(list, addNewlines, numIndent);
        }

        /// <summary>Returns a string representing the specified generic list in long form.
        /// Count and Capacity properties are also printed.
        /// A  newline and two spaces are added before each element printed.</summary>
        /// <typeparam name="T">Type of list elements.</typeparam>
        /// <param name="list">List to be converted to srting.</param>
        public static string ListToStringLong<T>(List<T> list)
        {
            return ListToStringLong(list, true, 2);
        }


        #endregion CollectionToString

        #region ToString

        // Globalization:

        /// <summary>Converts obect of the specified type to its string representation, where
        /// numbers are converted in ivariant culture (ignoring any localization settings).
        /// <para>This method can be used to avoid problems with differen local settinggs when
        /// transfering numerical values through text files.</para></summary>
        /// <typeparam name="ObjectType">Type of the object to be converted to string.</typeparam>
        /// <param name="obj">Object to be converted.</param>
        public static string ObjectToString<ObjectType>(ObjectType obj)
        {
            return ObjectToString<ObjectType>(obj, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>Converts obect of the specified type to its string representation, where
        /// numbers are converted in ivariant culture (ignoring any localization settings).
        /// <para>This method can be used to avoid problems with differen local settinggs when
        /// transfering numerical values through text files.</para></summary>
        /// <typeparam name="ObjectType">Type of the object to be converted to string.</typeparam>
        /// <param name="obj">Object to be converted.</param>
        /// <param name="cultureInfo">Culture info used in conversion.</param>
        public static string ObjectToString<ObjectType>(ObjectType obj, System.Globalization.CultureInfo cultureInfo)
        {
            if (IsNumeric(obj))
            {
                object expression = obj;
                return Convert.ToString(expression, cultureInfo);
            }
            else
            {
                return obj.ToString();
            }
        }


        /// <summary>Returns a flag indicating whether the specified object is of numeric type (such as int, float, double, etc.).
        /// <para>When called on an arbitrary object, the correct type parameter will be inferred, and
        /// we can get the desired information if </para></summary>
        /// <typeparam name="ObjectType">Type of the object that is queried.</typeparam>
        /// <param name="obj">Object for which we query whether it represents a numerical value.</param>
        public static bool IsNumeric<ObjectType>(ObjectType obj)
        {
            if (Equals(obj, null))
            {
                return false;
            }
            Type objType = typeof(ObjectType);
            if (objType.IsPrimitive)
            {
                return (objType != typeof(bool) &&
                    objType != typeof(char) &&
                    objType != typeof(IntPtr) &&
                    objType != typeof(UIntPtr)) ||
                    objType == typeof(decimal);
            }
            return false;
        }

        /// <summary>Test conversion to strings with invariant culture info.</summary>
        public static void TestToString()
        {
            Console.WriteLine(Environment.NewLine + "Test of conversion of numbers to string with invariant culture:" + Environment.NewLine);
            Console.WriteLine("Default (straightfrward) conversion: ");
            double d = 1.234e-6;
            Console.WriteLine("Through Console.WriteLine: " + d);
            Console.WriteLine("Through ToString(): " + d.ToString());
            Console.WriteLine("Through generic Util.ToString(): " +
                Util.ObjectToString<double>(d));
            Console.WriteLine("Through ToString() with NonGeneric Util.ToString(): " +
                Util.ObjectToString(d));
            object o = d;
            Console.WriteLine("Through NonGeneric Util.ToString(), cast to object: " +
                Util.ObjectToString(o));
            Console.WriteLine("Through NonGeneric Util.ToString(), cast to object and back to double: " +
                Util.ObjectToString((double)o));
            Console.WriteLine("Test of number to string conversion finished.");
        }


        #endregion ToString


        #region StringParse


        /// <summary>Tries to parse a string representation of an object of the specified type and return 
        /// it through output argument. Invariant culture is used in parsing.</summary>
        /// <typeparam name="ReturnType">Type of the object whose value is tried to be parsed from the string.</typeparam>
        /// <param name="strValue">String that is converted to obect of the specified type.</param>
        /// <param name="parsedValue">Value (of the specified type)vthat is obtained from the parsed string.</param>
        /// <returns>true if string was successfully converted to the object of the specified type, false if not 
        /// (in this case <paramref name="parsedValue"/> retains its previous value).</returns>
        public static bool TryParse<ReturnType>(string strValue, ref ReturnType parsedValue)
        {
            return TryParse<ReturnType>(strValue, ref parsedValue, System.Globalization.CultureInfo.InvariantCulture);
        }


        /// <summary>Converts a string to the object of the specified type and returns the entity, by using the 
        /// invariant culture.
        /// <para>This works for simple types, for complex types deserialization must be used.</para></summary>
        /// <typeparam name="ReturnType">Type of the entity to be returned, can be int.</typeparam>
        /// <param name="strValue">String to be converted to other type.</param>
        /// <returns>Object of the specified type converted form a string.</returns>
        public static ReturnType Parse<ReturnType>(string strValue)
        {
            return Parse<ReturnType>(strValue, System.Globalization.CultureInfo.InvariantCulture);
        }


        /// <summary>Converts a string to the entity of the specified type and returns that entity, by using invariant culture.
        /// <para>This works for simple types, for complex types deserialization must be used.</para></summary>
        /// <param name="strValue">String to be converted to other type.</param>
        /// <param name="propertyType">Type of the entity to be parsed from a string.</param>
        /// <returns>Object of the specified type converted form a string.</returns>
        public static object Parse(string strValue, Type propertyType)
        {
            return Parse(strValue, propertyType, System.Globalization.CultureInfo.InvariantCulture);
        }


        /// <summary>Tries to parse a string representation of an object of the specified type and return it through output argument.</summary>
        /// <typeparam name="ReturnType">Type of the object whose value is tried to be parsed from the string.</typeparam>
        /// <param name="strValue">String that is converted to obect of the specified type.</param>
        /// <param name="parsedValue">Value (of the specified type)vthat is obtained from the parsed string.</param>
        /// <param name="cultureInfo">Culture info used in conversion.</param>
        /// <returns>true if string was successfully converted to the object of the specified type, false if not 
        /// (in this case <paramref name="parsedValue"/> retains its previous value).</returns>
        public static bool TryParse<ReturnType>(string strValue, ref ReturnType parsedValue, System.Globalization.CultureInfo cultureInfo)
        {
            bool parsed = false;
            try
            {
                parsedValue = (ReturnType)Parse<ReturnType>(strValue, cultureInfo);
                parsed = true;
            }
            catch (Exception)
            { }
            return parsed;
        }


        /// <summary>Converts a string to the object of the specified type and returns the entity, by using the 
        /// specified culture info.
        /// <para>This works for simple types, for complex types deserialization must be used.</para></summary>
        /// <typeparam name="ReturnType">Type of the entity to be returned, can be int.</typeparam>
        /// <param name="strValue">String to be converted to other type.</param>
        /// <param name="cultureInfo">Culture info used in conversion.</param>
        /// <returns>Object of the specified type converted form a string.</returns>
        public static ReturnType Parse<ReturnType>(string strValue, System.Globalization.CultureInfo cultureInfo)
        {
            return (ReturnType)Parse(strValue, typeof(ReturnType), cultureInfo);
        }


        /// <summary>Converts a string to the entity of the specified type and returns that entity.
        /// <para>This works for simple types, for complex types deserialization must be used.</para></summary>
        /// <param name="strValue">String to be converted to other type.</param>
        /// <param name="propertyType">Type of the entity to be parsed from a string.</param>
        /// <param name="cultureInfo">Culture info used in conversion.</param>
        /// <returns>Object of the specified type converted form a string.</returns>
        public static object Parse(string strValue, Type propertyType, System.Globalization.CultureInfo cultureInfo)
        {
            var underlyingType = Nullable.GetUnderlyingType(propertyType);
            if (underlyingType == null)
                return Convert.ChangeType(strValue, propertyType, cultureInfo);
            return String.IsNullOrEmpty(strValue)
              ? null
              : Convert.ChangeType(strValue, underlyingType, cultureInfo);
        }



        /// <summary>Tries to parse a string representation of a boolean.</summary>
        /// <param name="str">String that is converted to boolean.</param>
        /// <param name="parsedValue">Boolean value parsed from the specified string.</param>
        /// <returns>true if string was successfully converted to boolean, false if not 
        /// (in this case <paramref name="parsedValue"/> retains its previous value).</returns>
        public static bool TryParseBoolean(string str, ref bool parsedValue)
        {
            bool parsed = false;
            try
            {
                parsedValue = ParseBoolean(str);
                parsed = true;
            }
            catch (Exception)
            { }
            return parsed;
        }


        /// <summary>Converts the specified string to a boolean value, if possible, and returns it.
        /// If conversion is not possible then exception is thrown.
        /// Recognized representations of true: "true", "1", "yes", "y" (case insensitive).
        /// Recognized representations of false: "false", "0", "no", "n" (case insensitive).</summary>
        /// <param name="str">String representation of boolean to beparsed.</param>
        /// <returns>Boolean value represented by the specified string.</returns>
        /// <exception cref="System.ArgumentNullException">When the string is null.</exception>
        /// <exception cref="System.FormatException">When the string can not represent a boolean value.</exception>
        public static bool ParseBoolean(string str)
        {
            bool value;
            try
            {
                value = bool.Parse(str);
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(str))
                    throw;
                str = str.ToLower();
                if (str == "0")
                    value = false;
                else if (str == "1")
                    value = true;
                else if (str == "false")
                    value = false;
                else if (str == "true")
                    value = true;
                else if (str == "no")
                    value = false;
                else if (str == "yes")
                    value = true;
                else if (str == "n")
                    value = false;
                else if (str == "y")
                    value = true;
                else throw;

            }
            return value;
        }


        /// <summary>Tries to parse a string representation of a <see cref="ThreadPriority"/> enum.</summary>
        /// <param name="str">String that is converted to  a <see cref="ThreadPriority"/> value.</param>
        /// <param name="parsedValue">Boolean value parsed from the specified string.</param>
        /// <returns>true if string was successfully converted to <see cref="ThreadPriority"/>, false if not 
        /// (in this case <paramref name="parsedValue"/> retains its previous value).</returns>
        /// <seealso cref="Util.ParseThreadPriority"/>
        public static bool TryParseThreadPriority(string str, ref ThreadPriority parsedValue)
        {
            bool parsed = false;
            try
            {
                parsedValue = ParseThreadPriority(str);
                parsed = true;
            }
            catch (Exception)
            { }
            return parsed;
        }


        /// <summary>Converts the specified string to a <see cref="ThreadPriority"/> enum value, 
        /// if possible, and returns it. If conversion is not possible then exception is thrown.
        /// <para>Recognized representations (not case sensitive):</para>
        /// <para><see cref="ThreadPriority.Lowest"/>: "0", "lowest", "idle"</para>
        /// <para><see cref="ThreadPriority.BelowNormal"/>: "1", "belownormal", "low"</para>
        /// <para><see cref="ThreadPriority.Normal"/>: "2", "normal"</para>
        /// <para><see cref="ThreadPriority.AboveNormal"/>: "3", "abovenormal", "high"</para>
        /// <para><see cref="ThreadPriority.Highest"/>: "4", "Highest", "realtime"</para>
        /// </summary>
        /// <param name="str">String representation of a <see cref="ThreadPriority"/> value to be parsed.</param>
        /// <returns>The <see cref="ThreadPriority"/> value represented by the specified string.</returns>
        /// <exception cref="System.ArgumentNullException">When the string is null.</exception>
        /// <exception cref="System.FormatException">When the string can not represent a boolean value.</exception>
        public static ThreadPriority ParseThreadPriority(string str)
        {
            ThreadPriority value;
            try
            {
                value = (ThreadPriority)Enum.Parse(typeof(ThreadPriority), str, true /* ignoreCase */);
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(str))
                    throw;
                int intVal = 0;
                bool parsed = false;
                parsed = int.TryParse(str, out intVal);
                if (parsed)
                {
                    // Strig represents an integer, convert it to the returned type:
                    if (intVal <= (int)ThreadPriority.Lowest)
                        value = ThreadPriority.Lowest;
                    else if (intVal >= (int)ThreadPriority.Highest)
                        value = ThreadPriority.Highest;
                    else
                        value = (ThreadPriority)intVal;
                    return value;
                }
                str = str.ToLower();
                if (str == "lowest" || str == "idle")
                    value = ThreadPriority.Lowest;
                else if (str == "belownormal" || str == "low")
                    value = ThreadPriority.BelowNormal;
                else if (str == "normal")
                    value = ThreadPriority.Normal;
                else if (str == "abovenormal" || str == "high")
                    value = ThreadPriority.AboveNormal;
                else if (str == "highest" || str == "realtime")
                    value = ThreadPriority.Highest;
                else throw;
            }
            return value;
        }


        #endregion StringParse


        #region IGLib

        public const string IGLibUrl = "http://www2.arnes.si/~ljc3m2/igor/iglib/";

        public const string IGLibCodeDocumentationUrl = "http://www2.arnes.si/~fgreso/code_documentation/generated/iglib/html/index.html";

        public const string IGLibAuthor = "Igor Grešovnik";

        #endregion IGLib

    }
}
