using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Linq
{
    public static class EnumExtensions
    {
        public static Enum Add<Enum>(this System.Enum input, Enum value) where Enum : struct, IFormattable, IComparable, IConvertible => (Enum)System.Enum.ToObject(typeof(Enum), input.To<int>() | value.To<int>());

        public static Enum Add(this Enum input, Enum value) => Enum.ToObject(input.GetType(), input.To<int>() | value.To<int>()) as Enum;

        public static Attribute GetAttribute<Attribute>(this Enum input) where Attribute : System.Attribute
        {
            var info = input.GetType().GetMember(input.ToString());

            foreach (var i in info[0].GetCustomAttributes(true))
            {
                if (i is Attribute)
                    return (Attribute)i;
            }

            return default(Attribute);
        }

        public static IEnumerable<Attribute> GetAttributes(this Enum input)
        {
            var info = input.GetType().GetMember(input.ToString());
            return info[0].GetCustomAttributes(true).Cast<Attribute>() ?? Enumerable.Empty<Attribute>();
        }

        public static bool HasAll(this Enum input, params Enum[] values) 
        {
            foreach (var i in values)
            {
                if (!input.HasFlag(i))
                    return false;
            }
            return true;
        }

        public static bool HasAny(this Enum input, params Enum[] values) 
        {
            foreach (var i in values)
            {
                if (input.HasFlag(i))
                    return true;
            }
            return false;
        }

        public static bool HasNone(this Enum input, params Enum[] values) 
        {
            return !input.HasAny(values);
        }

        public static bool HasAttribute<Attribute>(this Enum input) where Attribute : System.Attribute
        {
            var info = input.GetType().GetMember(input.ToString());

            foreach (var i in info[0].GetCustomAttributes(true))
            {
                if (i is Attribute)
                    return true;
            }

            return false;
        }

        public static Enum Remove<Enum>(this System.Enum input, Enum value) where Enum : struct, IFormattable, IComparable, IConvertible 
            => (Enum)System.Enum.ToObject(typeof(Enum), input.To<int>() & ~value.To<int>());

        public static Enum Remove(this Enum input, Enum value) 
            => Enum.ToObject(input.GetType(), input.To<int>() & ~value.To<int>()) as Enum;
    }
}