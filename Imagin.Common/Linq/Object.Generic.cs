using System;

namespace Imagin.Common.Linq
{
    public static partial class ObjectExtensions
    {
        public static void If<T>(this T a, Predicate<T> b, Action<T> c)
        {
            if (b(a))
                c(a);
        }

        /* Remarks

        To implement equality in a class, define the following methods and tweak accordingly...

        A. public static bool operator ==(T a, T b) => a.EqualsOverload(b);
        B. public static bool operator !=(T a, T b) => !(a == b);
        C. public override bool Equals(object b) => Equals(b as T);
        D. public bool Equals(T b) => this.Equals<T>(b) && ...;
        E. public override int GetHashCode() => ...;

        */

        /// <summary>
        /// Implements <see cref="IEquatable{T}.Equals(T)"/> for any given <see langword="class"/> that implements <see cref="IEquatable{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals<T>(this T a, T b)
        {
            if (ReferenceEquals(b, null))
                return false;

            if (ReferenceEquals(a, b))
                return true;

            if (a.GetType() != b.GetType())
                return false;

            return true;
        }

        /// <summary>
        /// Implements the <see langword="=="/> operator for any given <see langword="class"/> or <see langword="struct"/>. Example: <see langword="public static bool operator ==(left, right) => left.EqualsEquals(right);"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool EqualsOverload<T>(this T a, T b)
        {
            if (ReferenceEquals(a, null))
            {
                //null == null = true
                if (ReferenceEquals(b, null))
                    return true;

                //Only the left side is null
                return false;
            }
            return a.Equals(b);
        }

        //-----------------------------------------------------------------------------------

        /// <summary>
        /// Gets whether or not <see cref="object"/> is equal to any given <see cref="object"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool EqualsAny<T>(this T input, params T[] values)
        {
            foreach (var i in values)
            {
                if (input.Equals(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets whether or not the <see cref="object"/> is <see cref="System.Nullable"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Nullable<T>(this T input) => input.GetType().IsNullable();
    }
}