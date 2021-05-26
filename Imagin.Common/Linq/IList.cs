using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Linq
{
    public static partial class IListExtensions
    {
        public static void AddSorted<T>(this IList<T> input, T item, IComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;

            int i = 0;
            while (i < input.Count && comparer.Compare(input[i], item) < 0)
                i++;

            input.Insert(i, item);
        }

        public static bool Any(this IList input) 
        {
            if (input != null)
            {
                foreach (var i in input)
                    return true;
            }
            return false;
        }

        public static object First(this IList input)
        {
            object result = null;

            if (input == null)
                return result;

            foreach (var i in input)
                return i;

            return result;
        }

        public static T First<T>(this IList input, Predicate<T> predicate = null)
        {
            foreach (var i in input)
            {
                if (i is T && predicate?.Invoke((T)i) != false)
                    return (T)i;
            }
            return default(T);
        }

        public static void ForEach(this IList input, Action<object> Action)
        {
            foreach (var i in input)
                Action(i);
        }

        public static object Last(this IList input)
        {
            var Result = default(object);
            input.ForEach(i => Result = i);
            return Result;
        }

        public static IEnumerable<T> Select<T>(this IList input, Func<object, T> select)
        {
            foreach (var i in input)
                yield return select(i);
        }

        public static object[] ToArray(this IList input) => input.Cast<object>().ToArray();

        public static List<object> ToList(this IList input)
        {
            var result = new List<object>();
            input.ForEach(i => result.Add(i));
            return result;
        }
    }
}
