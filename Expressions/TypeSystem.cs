using System;
using System.Collections.Generic;

namespace Expressions
{

    internal static class TypeSystem
    {
        internal static Type GetElementType(Type seqType)
        {
            var iEnum = FindIEnumerable(seqType);

            return iEnum == null ? seqType : iEnum.GetGenericArguments()[0];
        }

        private static Type FindIEnumerable(Type seqType)
        {
            while (true)
            {
                if (seqType == null)
                {
                    return null;
                }

                if (seqType.IsArray)
                {
                    return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
                }


                if (seqType.IsGenericType)
                {
                    foreach (var arg in seqType.GetGenericArguments())
                    {
                        var iEnum = typeof(IEnumerable<>).MakeGenericType(arg);

                        if (iEnum.IsAssignableFrom(seqType))
                        {
                            return iEnum;
                        }
                    }
                }

                var interfaces = seqType.GetInterfaces();

                if (interfaces.Length > 0)
                {
                    foreach (var intface in interfaces)
                    {
                        var iEnum = FindIEnumerable(intface);

                        if (iEnum != null) return iEnum;
                    }
                }

                if (seqType.BaseType == null || seqType.BaseType == typeof(object)) return null;
                seqType = seqType.BaseType;
            }
        }
    }
}
