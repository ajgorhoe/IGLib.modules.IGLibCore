using IGLib.ConsoleUtils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace IGLib
{

    public static class BaseUtils
    {

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
            inputString = inputString!.Trim();
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
            inputString = inputString!.Trim();
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

        #region ParsingBasicTypesGeneric


        /// <summary>Parses the specified string by the appropriate TryParse method (such as <see cref="double.TryParse(string?, NumberStyles, IFormatProvider?, out double))
        /// selected accoding to the actual type of the <typeparamref name="BasicType"/> type parameter. If successful, 
        /// it returns true and assigns the parsed numerical value to <paramref name="valueVariable"/>. 
        /// <para>Warning: Even if parsing is not successful, <paramref name="valueVariable"/> may change.</para></summary>
        /// <typeparam name="BasicType">Type of the value that is parsed from string and assigned to <paramref name="valueVariable"/>.</typeparam>
        /// <param name="str">String that is parsed.</param>
        /// <param name="valueVariable">Referene (variable) that the parsed value is assigned to.</param>
        /// <param name="formatProvider">The <see cref="IFormatProvider"/> used for parsing.</param>
        /// <returns>True if parsing was successful (i.e., the provided <paramref name="str"/> actually corresponds to
        /// a numeric value of type <typeparamref name="BasicType"/>), false if not.</returns>
        /// <exception cref="ArgumentException">When parsing is not implemented for the specified type.</exception>
        public static bool TryParse<BasicType>(string str, out BasicType valueVariable, IFormatProvider? formatProvider = null)
            where BasicType : struct  // , IConvertible
        {
            //value = default;
            //return false;
            Type valueType = typeof(BasicType);
            switch (Type.GetTypeCode(valueType))
            {

                // Integer types:
                case TypeCode.Byte:  // byte
                    {
                        Byte temp;
                        bool result = Byte.TryParse(str, NumberStyles.Integer, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case TypeCode.SByte:  // sbyte
                    {
                        SByte temp;
                        bool result = SByte.TryParse(str, NumberStyles.Integer, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case TypeCode.Int16:  // short
                    {
                        Int16 temp;
                        bool result = Int16.TryParse(str, NumberStyles.Integer, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case TypeCode.UInt16:  // ushort
                    {
                        UInt16 temp;
                        bool result = UInt16.TryParse(str, NumberStyles.Integer, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case TypeCode.Int32:  // int
                    {
                        Int32 temp;
                        bool result = Int32.TryParse(str, NumberStyles.Integer, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case TypeCode.UInt32:  // uint
                    {
                        UInt32 temp;
                        bool result = UInt32.TryParse(str, NumberStyles.Integer, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case TypeCode.Int64:  // long
                    {
                        Int64 temp;
                        bool result = Int64.TryParse(str, NumberStyles.Integer, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case TypeCode.UInt64:  // uint64
                    {
                        UInt64 temp;
                        bool result = UInt64.TryParse(str, NumberStyles.Integer, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }

                // Floating point types:
                case TypeCode.Double:  // double
                    {
                        Double temp;
                        bool result = Double.TryParse(str, NumberStyles.Float | NumberStyles.AllowThousands, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case TypeCode.Single:  // float
                    {
                        Single temp;
                        bool result = Single.TryParse(str, NumberStyles.Float, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case TypeCode.Decimal:  // decimal
                    {
                        Decimal temp;
                        bool result = Decimal.TryParse(str, NumberStyles.Float, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }


                // Types char & bool:
                case TypeCode.Char:  // char
                    {
                        Char temp;
                        bool result = Char.TryParse(str, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case TypeCode.Boolean:  // bool
                    {
                        Boolean temp;
                        bool result = Boolean.TryParse(str, out temp);
                        if (!result)
                        {
                            if (IsTruePredefinedString(str))
                            {
                                temp = true;
                                result = true;
                            }
                            else if (IsFalsePredefinedString(str))
                            {
                                temp = false;
                                result = true;
                            }
                            else if (IsBooleanAnyIntegerAccepted)
                            {
                                long intValue;
                                bool isInteger = long.TryParse(str, NumberStyles.Integer, formatProvider, out intValue);
                                if (isInteger)
                                {
                                    temp = (intValue != 0);
                                    result = true;
                                }
                            }
                        }
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }

                // Date and time types:
                case TypeCode.DateTime:
                    {
                        DateTime temp;
                        bool result = DateTime.TryParse(str, formatProvider,
                            DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                default:
                    break;
            }
            switch (valueType)
            {
                // Date and time types:
                case Type t when t == typeof(DateTimeOffset):
                    {
                        DateTimeOffset temp;
                        bool result = DateTimeOffset.TryParse(str, formatProvider,
                            DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
#if NET8_0_OR_GREATER
                case Type t when t == typeof(TimeOnly):
                    {
                        TimeOnly temp;
                        bool result = TimeOnly.TryParse(str, formatProvider,
                            DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case Type t when t == typeof(DateOnly):
                    {
                        DateOnly temp;
                        bool result = DateOnly.TryParse(str, formatProvider,
                            DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }

                // Pointer types:
                case Type t when t == typeof(IntPtr):
                    {
                        IntPtr temp;
                        bool result = IntPtr.TryParse(str, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
                case Type t when t == typeof(UIntPtr):
                    {
                        UIntPtr temp;
                        bool result = UIntPtr.TryParse(str, formatProvider, out temp);
                        valueVariable = (BasicType)(object)temp;
                        return result;
                    }
#endif

                default:
                    break;
            }
            throw new NotImplementedException($"Generic {nameof(TryParse)} is not implemented for type of the {nameof(valueVariable)} parameter, {valueType.Name}.");
        }


        #endregion ParsingBasicTypesGeneric


    }


}
