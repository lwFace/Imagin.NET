using System;
using System.Collections;
using System.Collections.Generic;

namespace Imagin.Common.Linq
{
    public static partial class IEnumerableExtensions
    {
        public static bool Empty(this IEnumerable input)
        {
            foreach (var i in input)
                return false;

            return true;
        }

        public static void ForEach(this IEnumerable Source, Action<object> Action)
        {
            foreach (var i in Source)
                Action(i);
        }

        public static object First(this IEnumerable input)
        {
            foreach (var i in input)
                return i;

            return null;
        }

        public static T First<T>(this IEnumerable input)
        {
            foreach (var i in input)
            {
                if (i is T)
                    return (T)i;
            }
            return default;
        }

        public static object Last(this IEnumerable input)
        {
            var result = default(object);
            input.ForEach(i => result = i);
            return result;
        }

        public static IEnumerable<T> Select<T>(this IEnumerable input, Func<object, T> action)
        {
            foreach (var i in input)
                yield return action(i);
        }
    }
}