using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Linq
{
    public static partial class IListExtensions
    {
        public static T First<T>(this IList<T> input) => input.Any() ? input[0] : default;

        public static void For<T>(this IList<T> input, int from, Action<IList<T>, int> action)
        {
            for (var i = from; i < input.Count(); i++)
                action(input, i);
        }

        public static void For<T>(this IList<T> input, int from, int until, Action<IList<T>, int> action)
        {
            for (var i = from; i < until; i++)
                action(input, i);
        }

        public static void For<T>(this IList<T> input, int from, Predicate<int> until, Action<IList<T>, int> action)
        {
            for (var i = from; until(i); i++)
                action(input, i);
        }

        public static T Last<T>(this IList<T> input) => input.Any() ? input[input.Count - 1] : default;

        public static void Remove<T>(this IList<T> input, Predicate<T> where)
        {
            for (var i = input.Count - 1; i >= 0; i--)
            {
                if (where(input[i]))
                    input.Remove(input[i]);
            }
        }

        public static void Shuffle<T>(this IList<T> input)
        {
            int n = input.Count;
            while (n > 1)
            {
                n--;
                int k = Random.NextInt32(n + 1);
                T value = input[k];
                input[k] = input[n];
                input[n] = value;
            }
        }
    }
}