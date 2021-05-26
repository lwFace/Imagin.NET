using Imagin.Common.Collections;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Linq
{
    public static partial class ICollectExtensions
    {
        public static T[] ToArray<T>(this ICollect input)
        {
            var result = new T[input.Count];
            for (var i = 0; i < input.Count; i++)
                result[i] = (T)input[i];

            return result;
        }

        public static IEnumerable<T> Where<T>(this ICollect input, Predicate<T> predicate)
        {
            foreach (var i in input)
            {
                if (i is T t)
                {
                    if (predicate(t))
                        yield return t;
                }
            }
        }
    }
}