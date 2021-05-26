using Imagin.Common.Linq;
using System.Collections.Generic;

namespace Imagin.Common
{
    public static class List
    {
        public static List<T> New<T>(params T[] items)
        {
            var result = new List<T>();
            items.ForEach(i => result.Add(i));
            return result;
        }
    }
}