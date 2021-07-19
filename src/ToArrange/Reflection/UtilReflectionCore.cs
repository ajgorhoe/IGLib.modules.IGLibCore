using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IG.Lib
{

    /// <summary>Some convenient utilities that use reflection.</summary>
    public static class UtilReflectionCore
    {


        public static Type GetEnumerableType(Type type)
        {
            if (type.IsInterface && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return intType.GetGenericArguments()[0];
                }
            }
            return null;
        }


        public static bool ISEnumerableOf(Type elementType, Type queriedType, bool considerStringAsEnumerable = false)
        {
            Type actualElementType = GetEnumerableType(queriedType);
            if (actualElementType == null)
                return false; // not IEnumerable<someType>
            if (actualElementType == elementType)
            {
                if (!considerStringAsEnumerable && typeof(string).IsAssignableFrom(queriedType))
                {
                    // the String type implements IEnumerable<char> but we don't consider strings as IEnumerable for the purpose of this method.
                    return false;
                }
                return true;
            }
            return false;
        }



    }



}
