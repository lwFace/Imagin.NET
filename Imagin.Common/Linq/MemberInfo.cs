using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Common.Linq
{
    public static class MemberInfoExtensions
    {
        public static T GetAttribute<T>(this MemberInfo value) where T : Attribute
        {
            foreach (var i in value.GetCustomAttributes(true))
            {
                if (i is T)
                    return (T)i;
            }
            return default;
        }

        public static IEnumerable<T> GetAttributes<T>(this MemberInfo value) where T : Attribute
        {
            foreach (var i in value.GetCustomAttributes(true))
                yield return (T)i;

            yield break;
        }

        public static IEnumerable<Attribute> GetAttributes(this MemberInfo input) => input.GetAttributes<Attribute>();

        public static bool HasAttribute<T>(this MemberInfo input)
        {
            foreach (var i in input.GetCustomAttributes(true))
            {
                if (i is T)
                    return true;
            }
            return false;
        }
    }
}