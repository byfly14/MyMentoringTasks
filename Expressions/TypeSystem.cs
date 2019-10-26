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
                foreach (var arg in seqType.GetGenericArguments())
                {
                    var iEnum = typeof(IEnumerable<>).MakeGenericType(arg);

                    if (iEnum.IsAssignableFrom(seqType))
                    {
                        return iEnum;
                    }
                }
            }
        }
    }
}
