using System;
using System.Collections.Generic;

namespace Imagin.Common.Linq
{
    public static partial class ArrayExtensions
    {
        public static IEnumerable<object> Where(this Array value, Predicate<object> predicate)
        {
            foreach (var i in value)
            {
                if (predicate(i))
                    yield return i;
            }
            yield break;
        }
    }
}