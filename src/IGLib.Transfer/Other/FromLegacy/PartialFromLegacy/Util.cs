// Copyright (c) Igor Grešovnik (2008 - present), IGLib license; http://www2.arnes.si/~ljc3m2/igor/iglib/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;

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




    }
}
